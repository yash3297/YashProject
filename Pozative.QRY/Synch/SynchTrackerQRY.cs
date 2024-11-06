using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.QRY
{
    public class SynchTrackerQRY
    {
        public static string GetTrackerAppointmentData = " set transaction isolation level read uncommitted  SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Appt_LocalDB_ID,AP.AppointmentId AS Appt_EHR_ID,'' AS Appt_Web_ID, CT.LastName AS Last_Name,CT.FirstName AS First_Name,'' AS MI, "
                                                          + "  ISNULL(CT.PhoneNumber,'') AS Home_Contact,"
                                                          + "  ISNULL(CT.MobileNumber,'') AS Mobile_Contact,"
                                                          + "  ISNULL(CT.Email,'') AS Email, "
                                                          + "  SUBSTRING(( ISNULL(CT.Address1,'' ) + ' ' + ISNULL(CT.Address2,'' ) ),0,200) AS [Address],"
                                                          + "  CT.City AS City,'' AS [ST],ISNULL(CT.PostalCode,'') AS Zip, AP.ScheduleColumnId AS Operatory_EHR_ID,SC.ScheduleColumnName AS Operatory_Name"
                                                          + "  ,AP.ProviderId AS Provider_EHR_ID,PD.ProviderName AS Provider_Name,AP.comments AS Comment, CT.Birthdate AS birth_date,ISNULL(RL.Reasonid,0) AS ApptType_EHR_ID,"
                                                          + "  ISNULL(AP.Reason,ISNULL(RL.Reason,'None')) AS ApptType, CAST(AP.AppointmentDate as DATETIME) + CAST(AP.AppointmentTime as DATETIME)AS Appt_DateTime,"
                                                          + "  DATEADD(HOUR,DATEPART(HOUR, CAST(AP.AppointmentDate as DATETIME) + CAST(AP.Duration as DATETIME) ),DATEADD(MINUTE,DATEPART(MINUTE, CAST(AP.AppointmentDate as DATETIME) + CAST(AP.Duration as DATETIME) ),DATEADD(SECOND, DATEPART(SECOND, CAST(AP.AppointmentDate as DATETIME) + CAST(Duration as DATETIME) ),CAST(AP.AppointmentDate as DATETIME) + CAST(AP.AppointmentTime as DATETIME)))) AS Appt_EndDateTime,"
                                                          + "  APS.AppointmentStatusDescription AS [Status],'new' AS Patient_Status,CASE WHEN AP.FlowState = 'Completed' THEN '1005' ELSE Convert(varchar,AP.StatusId) END AS appointment_status_ehr_key , CASE WHEN AP.FlowState = 'Completed' THEN 'Completed' ELSE APS.AppointmentStatusDescription END AS  Appointment_Status,'EHR' as Is_Appt,CASE WHEN FlowState = 'Completed' THEN 1005 WHEN FlowState = 'Reception' THEN 1009 WHEN FlowState = 'Recovery' THEN 1010 WHEN FlowState = 'In Chair' THEN 1004 WHEN FlowState = 'X-Rays' THEN 1008 WHEN FlowState = 'On Deck' THEN 1003 WHEN FlowState = 'Checked In' THEN 1002 WHEN FlowState = 'Arrived' THEN 1001 WHEN ISConfirmed = 1 THEN 1007 WHEN ISPreconfirmed = 1 THEN 1006 ELSE '' END AS confirmed_status_ehr_key,"
                                                          + "  CASE WHEN FlowState IS NOT NULL THEN FlowState WHEN ISConfirmed = 1 THEN 'Confirmed' WHEN ISPreconfirmed = 1 THEN 'Preconfirmed' ELSE '' END AS confirmed_status,0 AS is_ehr_updated,"
                                                          + "  AP.CreatedTimeStamp AS EHR_Entry_DateTime,0 AS Is_Adit_Updated, AP.ContactId AS patient_ehr_id,CONVERT(bit,case when statusid in (3,5,6,7,8,9) then 1 else 0 end) AS is_deleted,0 AS is_Status_Updated_From_Web,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, convert(bit, CASE WHEN AP.StatusId = 2 THEN 1 ELSE 0 END ) AS is_asap"
                                                          + "  FROM Appointment AP "
                                                          + "  INNER JOIN Contact CT ON AP.ContactId = CT.ContactId "
                                                          + "  INNER JOIN ScheduleColumn SC ON SC.ScheduleColumnId = AP.ScheduleColumnId "
                                                          + "  INNER JOIN Provider PD ON PD.ProviderId = AP.ProviderId  "
                                                          + "  Left outer join ReasonLookup RL on RL.Reason = AP.Reason"
                                                          + "  INNER JOIN AppointmentStatus APS ON APS.AppointmentStatusId = AP.StatusId WHERE CAST(AP.AppointmentDate as DATETIME) + CAST(AP.AppointmentTime as DATETIME) >= @ToDate ";

        public static string TrackerAppointment_Procedures_Data = "  SELECT appt.AppointmentId,"
                                                //+"  stuff(( "
                                                //+"      SELECT   ',' " 
                                                //+"          + (CASE WHEN CONVERT(Varchar(5),Plog.ToothNumber) !='' THEN CONVERT(Varchar(5),Plog.ToothNumber) + '-'  ELSE '' END) "
                                                //+"         + (CASE WHEN Plog.SurfaceDescription  != '' THEN Plog.SurfaceDescription + '-'  ELSE '' END) "
                                                //+"          + Plog.Description "
                                                //+"          FROM EstimateEntry Plog "
                                                //+"          WHERE Plog.AppointmentId = appt.AppointmentId "
                                                //+"         ORDER BY Plog.ToothNumber "
                                                //+"          for xml path('') "
                                                //+"      ),1,1,'') as ProcedureDesc, "
                                                + "  stuff(( "
                                                + "      SELECT   ','  + ProcedureCode "
                                                + "          FROM EstimateEntry Plog "
                                                + "          WHERE Plog.AppointmentId = appt.AppointmentId "
                                                + "          ORDER BY Plog.ToothNumber "
                                                + "      for xml path('') "
                                                + "      ),1,1,'') as ProcedureCode "
                                                + "  FROM appointment appt "
                                                //        + "where AppointmentId=@Appt_EHR_ID"
                                                + "  WHERE appt.AppointmentDate > @ToDate"
                                                + "  GROUP BY appt.AppointmentId";

        public static string TrackerAppointment_Procedures_DataByApptID = "  SELECT appt.AppointmentId,"
                                              + "  stuff(( "
                                              + "      SELECT   ','  + ProcedureCode "
                                              + "          FROM EstimateEntry Plog "
                                              + "          WHERE Plog.AppointmentId = appt.AppointmentId "
                                              + "          ORDER BY Plog.ToothNumber "
                                              + "      for xml path('') "
                                              + "      ),1,1,'') as ProcedureCode "
                                              + "  FROM appointment appt "
                                              + "  WHERE appt.AppointmentDate > @ToDate and appt.AppointmentId = @Appt_EHR_ID"
                                              + "  GROUP BY appt.AppointmentId";

        public static string GetTrackerAppointmentEhrIds = " SELECT AP.AppointmentId AS Appt_EHR_ID"
                                                          + "  FROM Appointment AP "
                                                          + "  INNER JOIN Contact CT ON AP.ContactId = CT.ContactId "
                                                          + "  INNER JOIN ScheduleColumn SC ON SC.ScheduleColumnId = AP.ScheduleColumnId "
                                                          + "  INNER JOIN Provider PD ON PD.ProviderId = AP.ProviderId  "
                                                          + "  INNER JOIN AppointmentStatus APS ON APS.AppointmentStatusId = AP.StatusId WHERE AP.AppointmentDate > @ToDate ";


        public static string GetTrackerApptStatusData = "SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,AppointmentStatusId AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,AppointmentStatusDescription AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated FROM AppointmentStatus "
                                             + "   UNION SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,1001 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'Arrived' AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated "
                                             + "   UNION SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,1002 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'Check In' AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated "
                                             + "   UNION SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,1003 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'On Deck' AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated "
                                             + "   UNION SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,1004 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'In Chair' AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated "
                                             + "   UNION SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,1005 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'Completed' AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated "
                                             + "   UNION SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,1006 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'Preconfirmed' AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated "
                                             + "   UNION SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,1007 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'Confirmed' AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated "
                                             + "   UNION SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,1008 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'X-Rays' AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated "
                                             + "   UNION SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,1009 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'Reception' AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated "
                                             + "   UNION SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_Id,1010 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'Recovery' AS ApptStatus_Name,'confirm' AS ApptStatus_Type,0 AS Is_Adit_Updated ";


        public static string InsertPatientTreatmentDocument = "INSERT INTO Activity(Type,ContactId,Message,AttachmentFilePath,ModifiedUserId,ModifiedTimeStamp,CreatedUserId,CreatedTimeStamp,ModifiedMachineName,CreatedMachineName,SerializedTeeth,IsActive,FileType) VALUES(@Type,@ContactId,@Message,@AttachmentFilePath,@ModifiedUserId,@ModifiedTimeStamp,@CreatedUserId,@CreatedTimeStamp,@ModifiedMachineName,@CreatedMachineName,@SerializedTeeth,@IsActive,@FileType)";

        public static string InsertPatientInsuranceCarrierDocument = "INSERT INTO Activity(Type,ContactId,Message,AttachmentFilePath,ModifiedUserId,ModifiedTimeStamp,CreatedUserId,CreatedTimeStamp,ModifiedMachineName,CreatedMachineName,SerializedTeeth,IsActive,FileType) VALUES(@Type,@ContactId,@Message,@AttachmentFilePath,@ModifiedUserId,@ModifiedTimeStamp,@CreatedUserId,@CreatedTimeStamp,@ModifiedMachineName,@CreatedMachineName,@SerializedTeeth,@IsActive,@FileType)";

        //public static string GetTrackerApptTypeData = "SELECT 0 AS Clinic_Number,1 as Service_Install_Id,'' AS ApptType_LocalDB_ID,1 AS ApptType_EHR_ID,'' AS ApptType_Web_ID, 'None' AS [Type_Name],0 AS Is_Adit_Updated ";

        //public static string GetTrackerApptTypeData = "SELECT ReasonId as ApptType_EHR_ID,Reason as Type_Name from ReasonLookup";

        public static string GetTrackerApptTypeData = "SELECT ReasonId as ApptType_EHR_ID,Reason as Type_Name, 0 AS Clinic_Number,1 as Service_Install_Id,'' AS ApptType_LocalDB_ID,'' AS ApptType_Web_ID,0 AS Is_Adit_Updated from ReasonLookup";


        public static string GetTrackerOperatoryData = " SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Operatory_LocalDB_ID,ScheduleCOlumnId AS Operatory_EHR_ID,'' AS Operatory_Web_ID, ScheduleColumnName AS Operatory_Name,0 AS Is_Adit_Updated,SortOrder as OperatoryOrder  FROM ScheduleColumn ";

        //public static string GetTrackerOperatoryEventData = "SELECT DISTINCT 0 AS OE_LocalDB_ID,convert(varchar, HH.HourId) AS OE_EHR_ID,'' AS OE_Web_ID,convert(varchar, HH.ScheduleColumnId) AS Operatory_EHR_ID," 
        //                                                    + "  CAST(HH.StartDate as DATETIME) + "
        //                                                    + "  CAST(CASE WHEN (SELECT COUNT(1) FROM [HOUR] WHERE ScheduleColumnId = HH.ScheduleColumnId AND StartDate = HH.StartDate AND ENDTIME < HH.StartTime AND StartTIme < HH.StartTIme ) > 0 "
        //                                                    + "            THEN (SELECT TOP 1 EndTIme FROM [HOUR] WHERE ScheduleColumnId = HH.ScheduleColumnId AND StartDate = HH.StartDate AND ENDTIME < HH.StartTime Order by ENdtime desc ) "
        //                                                    + "            ELSE  '00:01:00' END as DATETIME) AS StartTime ,"
        //                                                    + "  case when   cast(HH.StartTIme as time) = '00:00:00.0000000' then  CAST(HH.StartDate as DATETIME) + CAST('23:59:00.0000000' as time) else  CAST(HH.StartDate as DATETIME) + CAST(HH.StartTIme as DATETIME) end AS EndTime,"
        //                                                    + "  convert(varchar, HH.Reason) AS comment,GetDate() AS Entry_DateTime,'' AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated"
        //                                                    + "  FROM [HOUR] HH WHERE HH.StartDate > @ToDate ";

        //public static string GetTrackerOperatoryEventData = "select * from (SELECT DISTINCT 0 AS OE_LocalDB_ID,convert(varchar, HH.HourId) AS OE_EHR_ID,'' AS OE_Web_ID,convert(varchar, HH.ScheduleColumnId) AS Operatory_EHR_ID, "
        //                                                    + " CAST(HH.StartDate as DATETIME) + "
        //                                                    + " CAST((SELECT TOP 1 DATEADD(mi,15,[EndTime]) as EndTIme FROM [HOUR] WHERE ScheduleColumnId = HH.ScheduleColumnId AND StartDate = HH.StartDate AND ENDTIME < HH.StartTime Order by ENdtime desc )  "
        //                                                    + "           as DATETIME) AS StartTime , "
        //                                                    + " case when   cast(HH.StartTIme as time) = '00:00:00.0000000' then  CAST(HH.StartDate as DATETIME) + CAST('23:59:00.0000000' as time) else  CAST(HH.StartDate as DATETIME) + CAST(DATEADD(mi,-15,HH.StartTIme) as DATETIME) end AS EndTime, "
        //                                                    + " convert(varchar, HH.Reason) AS comment,GetDate() AS Entry_DateTime,getdate() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated "
        //                                                    + " FROM [HOUR] HH where HH.StartDate >= @ToDate) as a where StartTime is not null and a.StartTime <> a.EndTime " 
        //                                                    + " Union all " 
        //                                                    + " SELECT '' AS OE_LocalDB_ID,convert(varchar, HH.HourId) AS OE_EHR_ID,'' AS OH_Web_ID,HH.ScheduleColumnId AS Operatory_EHR_ID, "
        //                                                    + " CAST(HH.StartDate as DATETIME) + case when cast(HH.StartTIme as time) = '00:00:00.0000000'  then  CAST('00:00:01.0000000' as time) else CAST(HH.StartTime as DATETIME) end AS StartTime, "
        //                                                    + " CAST(HH.StartDate as DATETIME) + case when cast(HH.EndTime as time) = '00:00:00.0000000' then  CAST('23:59:59.0000000' as time) else CAST(HH.EndTime as DATETIME) end AS EndTime,ISNULL( HH.Reason,'') AS [comment], "
        //                                                    + " getdate() AS Entry_DateTime,getdate() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated "
        //                                                    + " FROM HOUR HH INNER JOIN ScheduleColumn SC ON HH.ScheduleColumnId = HH.ScheduleColumnId INNER JOIN Provider P ON P.ProviderId = SC.ProviderId where cast(HH.StartTIme as time) = '00:00:00.0000000' and  HH.StartDate >= @ToDate";

        public static string GetEHRActualVersionTracker = "IF ( (Select count(1) from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ComputerInfo') > 0 AND (Select count(1) from INFORMATION_SCHEMA.COLUMNS where COLUMN_NAME = 'LastExecutable') > 0  ) BEGIN 	SELECT  substring(LastExecutable,10,15) as version FROM ComputerInfo END";
        /*select ApplicationVersion from Version where VersionId = (select Max(VersionId) from Version) and ApplicationVersion != '';
         select ApplicationVersion from Version where ApplicationVersion != ''; */

        public static string GetTrackerOperatoryEventData = " set transaction isolation level read uncommitted  SELECT 0 AS Clinic_Number,1 AS Service_Install_Id,'' AS OE_LocalDB_ID,convert(varchar, HH.HourId) AS OE_EHR_ID,'' AS OE_Web_ID,HH.ScheduleColumnId AS Operatory_EHR_ID, "
                                                            + " CAST(HH.StartDate as DATETIME) +  CAST('00:00:01.0000000' as time) AS StartTime, "
                                                            + " CAST(HH.StartDate as DATETIME) + CAST('23:59:59.0000000' as time) AS EndTime,convert(varchar,ISNULL( HH.Reason,'')) AS [comment],  "
                                                            + " getdate() AS Entry_DateTime,getdate() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated  "
                                                            + " FROM HOUR HH where cast(HH.StartTIme as time) = '00:00:00.0000000' and HH.StartDate >= @ToDate AND HH.StartDate <= DATEADD(MONTH,9,@ToDate) "
                                                            + "  union select 0 AS Clinic_Number,1 AS Service_Install_Id,0 AS OE_LocalDB_ID, "
                                                            + "  AppointmentId AS OE_EHR_ID,'' AS OH_Web_ID,ScheduleColumnId AS Operatory_EHR_ID, "
                                                            + "  CAST(AppointmentDate as DATETIME) +  CAST(AppointmentTime as time) AS StartTime, "
                                                            + "  (DATEADD(HOUR,DATEPART(HOUR, CAST(AppointmentDate as DATETIME) + CAST(Duration as DATETIME) ),DATEADD(MINUTE,DATEPART(MINUTE, CAST(AppointmentDate as DATETIME) + CAST(Duration as DATETIME) ),DATEADD(SECOND, DATEPART(SECOND, CAST(AppointmentDate as DATETIME) + CAST(Duration as DATETIME) ),CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME))))) AS EndTime, "
                                                            + "  convert(varchar, ISNULL( Detail,'')) AS [comment],   "
                                                            + "  getdate() AS Entry_DateTime,getdate() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated  "
                                                            + "  from Appointment where contactid is NULL and appointmentdate >= @ToDate AND Appointmentdate <= DATEADD(MONTH,9,@ToDate) ";
        //11.29
        public static string GetTrackerProviderData_11_29 = " SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Provider_LocalDB_ID,PD.ProviderId AS Provider_EHR_ID ,'' AS Provider_Web_ID,'' AS Last_Name, PD.ProviderName AS First_Name,'' AS MI,'' AS gender,"
                                                       + " ( Case when PD.IsSpecialist = 1 then 'Specialist' else 'None-Specialist' end ) AS provider_speciality,'' AS bio, '' AS education, '' AS accreditation,'' AS membership,'' AS [language],'' AS age_treated_min,'' AS age_treated_max, PD.IsActive AS is_active,"
                                                       + " 0 AS Is_Adit_Updated FROM Provider PD ";
        //11.27
        public static string GetTrackerProviderData_11_27 = " SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Provider_LocalDB_ID,PD.ProviderId AS Provider_EHR_ID ,'' AS Provider_Web_ID,'' AS Last_Name, PD.ProviderName AS First_Name,'' AS MI,'' AS gender,"
                                               + " 'Specialist' AS provider_speciality,'' AS bio, '' AS education, '' AS accreditation,'' AS membership,'' AS [language],'' AS age_treated_min,'' AS age_treated_max, PD.IsActive AS is_active,"
                                               + " 0 AS Is_Adit_Updated FROM Provider PD ";

        public static string GetTrackerDefaultProviderData = " Select TOP 1 ProviderId FROM Provider WHERE IsActive = 1";
        //11.29
        public static string GetTrackerHolidayData_11_29 = " SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS H_LocalDB_ID,convert(varchar,HD.HolidayId) AS H_EHR_ID,'' AS H_Web_ID,ISNULL( HD.ProviderId,0) AS H_Operatory_EHR_ID,HD.HolidayDate AS SchedDate, HD.HolidayName AS comment,HD.CreatedTImeStamp AS Entry_DateTime,0 AS Is_Adit_Updated FROM Holiday HD   WHERE HD.HolidayDate > @ToDate ";
        //public static string GetTrackerHolidayData_11_29 = " SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS H_LocalDB_ID,'' AS H_Web_ID,ISNULL( HD.ProviderId,0) AS H_Operatory_EHR_ID,HD.HolidayDate AS SchedDate, HD.HolidayName AS comment,HD.CreatedTImeStamp AS Entry_DateTime,0 AS Is_Adit_Updated FROM Holiday HD   WHERE HD.HolidayDate > @ToDate ";
        //11.27
        public static string GetTrackerHolidayData_11_27 = " SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS H_LocalDB_ID,HD.HolidayId AS H_EHR_ID,'' AS H_Web_ID,0 AS H_Operatory_EHR_ID,HD.HolidayDate AS SchedDate, HD.HolidayName AS comment,HD.CreatedTImeStamp AS Entry_DateTime,0 AS Is_Adit_Updated FROM Holiday HD   WHERE HD.HolidayDate > @ToDate ";

        public static string GetOperatoryDateTimeWise = " Select AP.AppointmentId AS Appointment_EHR_Id, 0 AS Clinic_Number,1 as Service_Install_Id,ScheduleColumnId AS location_id,CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME) AS start_Time,"
                                                         + "    DATEADD(HOUR,DATEPART(HOUR, CAST(AP.AppointmentDate as DATETIME) + CAST(AP.Duration as DATETIME) ),DATEADD(MINUTE,DATEPART(MINUTE, CAST(AP.AppointmentDate as DATETIME) + CAST(AP.Duration as DATETIME) ),DATEADD(SECOND, DATEPART(SECOND, CAST(AP.AppointmentDate as DATETIME) + CAST(Duration as DATETIME) ),CAST(AP.AppointmentDate as DATETIME) + CAST(AP.AppointmentTime as DATETIME)))) AS End_Time,P.FirstName,P.LastName,P.MobileNumber AS Mobile,P.Email,PP.ProviderName AS ProviderFirstName,'' AS ProviderLastName "
                                                         + "    FROM Appointment AP LEFT JOIN Contact P ON p.ContactId = AP.ContactId LEFT JOIN Provider PP ON PP.ProviderId = P.ProviderId where CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME) >= @DateTime "
                                                         + "    and DATEADD(HOUR,DATEPART(HOUR, CAST(AP.AppointmentDate as DATETIME) + CAST(AP.Duration as DATETIME) ),DATEADD(MINUTE,DATEPART(MINUTE, CAST(AP.AppointmentDate as DATETIME) + CAST(AP.Duration as DATETIME) ),DATEADD(SECOND, DATEPART(SECOND, CAST(AP.AppointmentDate as DATETIME) + CAST(Duration as DATETIME) ),CAST(AP.AppointmentDate as DATETIME) + CAST(AP.AppointmentTime as DATETIME)))) >= @DateTime and AP.StatusId not in (3,5,6,7,8,9) ";

        public static string GetTrackerSpecialty = " SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Speciality_LocalDB_ID,Speciality_EHR_ID,'' AS Speciality_Web_ID,Speciality_Name,getdate() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated "
                                                       + "  FROM ( Select 1 AS Speciality_EHR_ID, 'Specialist' AS Speciality_Name UNION "
                                                       + "  Select 2 AS Speciality_EHR_ID, 'None-Specialist' AS Speciality_Name ) AS AA ";


        // public static string GetTrackerRecallType = " SELECT 0 AS Clinic_Number,1 as Service_Install_Id,'' AS RecallType_LocalDB_ID,1 AS RecallType_EHR_ID,'' AS RecallType_Web_ID,'None' AS RecallType_Name,'None' AS RecallType_Descript,getdate() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated ";
        public static string GetTrackerRecallType = " SELECT 0 AS Clinic_Number,1 as Service_Install_Id,'' AS RecallType_LocalDB_ID,ReasonId AS RecallType_EHR_ID,'' AS RecallType_Web_ID,Reason AS RecallType_Name,'None' AS RecallType_Descript,getdate() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated,(case when IsRecall=1 then 0 else 1 end) AS is_deleted from ReasonLookup where IsRecall=1";

        public static string GetTrackerUser = " select UserId as User_EHR_ID,'' AS User_Local_DB_ID,'' as User_web_Id,Username as First_Name,'' as Last_Name,PasswordHash as Password ,CreatedTimeStamp as EHR_Entry_DateTime,"
                                               + "ModifiedTimeStamp as Last_Updated_DateTime,'' as LocalDb_EntryDatetime,IsActive as Is_active,0 as is_deleted,0 as Is_Adit_Updated,0 as Clinic_Number,1 as Service_Install_Id from [user]";

        //public static string GetTrackerOperatoryTimeOff = " select * from  ";

        public static string GetTrackerPatientData = " set transaction isolation level read uncommitted  SELECT DISTINCT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Patient_LocalDB_ID,CT.ContactId AS Patient_EHR_ID,'' AS Patient_Web_ID,CT.FirstName AS First_name,CT.LastName AS Last_name, '' AS Middle_Name,CT.Title AS Salutation, "
                                                   + " case when CT.IsActive = 0 then 'I' else 'A' end AS [Status],( case WHEN CT.IsPatient = 1 AND CT.IsActive = 1 then 'Active' when  CT.IsPatient = 1 AND CT.IsActive = 0 AND CT.InactiveReason = 'Deceased' THEN 'Deleted' when  CT.IsPatient = 1 AND CT.IsActive = 0 AND CT.InactiveReason != 'Deceased'  THEN 'InActive'  else 'NonPatient' end ) AS [EHR_Status], CONVERT(bit,CASE WHEN CT.IsActive = 0 AND CT.InactiveReason = 'Deceased' THEN 1 ELSE 0 END) AS Is_Deleted ,CT.Sex,'' AS MaritalStatus,CT.BirthDate AS Birth_Date,CT.Email,ISNULL(CT.MobileNumber,'') AS Mobile,ISNULL(CT.PhoneNumber,'') AS Home_Phone,ISNULL(CT.WorkPhoneNumber,'') AS Work_Phone, "
                                                   + " ISNULL(CT.Address1,'') AS Address1,ISNULL(CT.Address2,'') AS Address2, ISNULL(CT.City,'') AS City,R.RegionAbbreviation AS [State],ISNULL(CT.PostalCode,'') AS Zipcode,'' AS ResponsibleParty_Status, "
                                                   + " 0 AS CurrentBal,0 AS ThirtyDay,0 AS SixtyDay,0 AS NinetyDay,0 AS Over90, CT.IdentificationNumber AS ssn,Case when CT.IsStudent = 1 THEN CT.Company ELSE '' END AS School, Case when CT.IsStudent = 0 THEN CT.Company ELSE '' END AS Employer,"
                                                   + " FirstVisit_Date, "
                                                   + " LastVisit_Date, "
                                                   //+ " (SELECT TOP 1 CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME) FROM Appointment WHERE Statusid NOT IN (3,6,7,8,9) AND ContactId = CT.ContactId Order by AppointmentDate  ) AS FirstVisit_Date, "
                                                   //+ " (SELECT TOP 1 CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME) FROM Appointment WHERE Statusid NOT IN (3,6,7,8,9) AND ContactId = CT.ContactId AND AppointmentDate <= GetDate() Order by AppointmentDate DESC ) AS LastVisit_Date, "
                                                   + " ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance,ISNULL(PrimIns.PrimaryInsuranceCompanyName,'') AS Primary_Insurance_CompanyName,Prim_Ins_Company_Phonenumber,isnull(PrimIns.PrimarySubcriberId,'') as Primary_Ins_Subscriber_ID, "
                                                   + " ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance,ISNULL(SecIns.SecondaryInsuranceCompanyName,'') AS Secondary_Insurance_CompanyName,Sec_Ins_Company_Phonenumber,CT.ClientId AS Guar_ID,isnull(SecIns.SecondarySubcriberId,'') as Secondary_Ins_Subscriber_ID, "
                                                   + " ISNULL(CT.ProviderId,0) AS Pri_Provider_ID,ISNULL(CT.ProviderId,0) AS Sec_Provider_ID, ISNULL( CP.IsSMSEnabled,0) AS ReceiveSms,ISNULL( CT.HasEmailConsent,0) AS ReceiveEmail, "
                                                   + " nextvisit_date, "
                                                   //+ " (SELECT TOP 1 CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME) FROM Appointment WHERE Statusid NOT IN (3,6,7,8,9) AND ContactId = CT.ContactId AND AppointmentDate > GetDate() Order by AppointmentDate  ) AS nextvisit_date, "
                                                   //+ " Convert(varchar,RC.DueDate) + '@None@1|' AS due_date,ISNULL((( CV.SingleBasicAdjustmentTotal + CV.SingleMajorAdjustmentTotal +  CV.SingleOrthoAdjustmentTotal + CV.SinglePreventativeAdjustmentTotal ) - INvoice.InsuranceAmount ),0) AS remaining_benefit,ISNULL(Invoice.InsurancePaidAmount,0) AS collect_payment,CT.CreatedTimeStamp AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated,ISNULL(CT.NickName,'') AS preferred_name,ISNULL(INvoice.InsuranceAmount,0) AS used_benefit, 1 As InsUptDlt,'Y' as ReceiveVoiceCall,CASE WHEN CT.Language = 'ENG' THEN 'English' WHEN CT.Language = 'FRE' THEN 'French' ELSE 'Spanish' END AS PreferredLanguage,CT.ContactMethod AS Patient_note, "
                                                   + " ( case when AA.DueDate is not NULL then AA.DueDate when AB.DueDate is not NULL then AB.DueDate when AC.DueDate is Not NULL then AC.DueDate else NULL end )  AS due_date,ISNULL((( CV.SingleBasicAdjustmentTotal + CV.SingleMajorAdjustmentTotal +  CV.SingleOrthoAdjustmentTotal + CV.SinglePreventativeAdjustmentTotal ) - INvoice.InsuranceAmount ),0) AS remaining_benefit,ISNULL(Invoice.InsurancePaidAmount,0) AS collect_payment,CT.CreatedTimeStamp AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated,ISNULL(CT.NickName,'') AS preferred_name,ISNULL(INvoice.InsuranceAmount,0) AS used_benefit, 1 As InsUptDlt,'Y' as ReceiveVoiceCall,CASE WHEN CT.Language = 'ENG' THEN 'English' WHEN CT.Language = 'FRE' THEN 'French' ELSE 'Spanish' END AS PreferredLanguage,CT.ContactMethod AS Patient_note, "
                                                   + "  C2.ContactId AS responsiblepartyid,C2.FirstName AS ResponsibleParty_First_Name,C2.LastName AS ResponsibleParty_Last_Name,C2.IdentificationNumber AS responsiblepartyssn,C2.BirthDate AS responsiblepartybirthdate,"
                                                   + " EM.ContactId AS EmergencyContactId,EM.FirstName AS EmergencyContact_First_Name,EM.LastName AS EmergencyContact_Last_Name,EM.MobileNumber AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name ,'' AS Spouse_Last_Name,'' AS driverlicense,'' AS groupid"
                                                   + " FROM CONTACT CT  LEFT JOIN ContactPhone CP ON CP.ContactId = CT.ContactId AND CP.IsDefault = 1  "
                                                   + " LEFT join  Contact C2 on CT.ClientId = C2.ContactId"
                                                   + " LEFT join  Region R on R.RegionId = C2.RegionId"
                                                   + " LEFT join ContactRelationship CR on CT.ContactId = CR.ContactId AND CR.RelationshipType = 3 "
                                                   + " LEFT join contact EM on EM.contactid = CR.RelatedContactId"
                                                   + " LEFT JOIN (select contactId, SUM(SingleBasicAdjustmentTotal) AS SingleBasicAdjustmentTotal, SUM(SingleMajorAdjustmentTotal) AS SingleMajorAdjustmentTotal,SUM(SingleOrthoAdjustmentTotal) AS SingleOrthoAdjustmentTotal ,SUM(SinglePreventativeAdjustmentTotal) AS SinglePreventativeAdjustmentTotal  from Coverage Group by contactId) CV ON CV.ContactId = CT.ContactId  "
                                                   + " LEFT JOIN ( SELECT SUM(InsuranceAmount) AS InsuranceAmount, SUM(InsurancePaidAmount) AS InsurancePaidAmount,PatientId FROM Invoice Group by PatientId ) AS Invoice ON Invoice.PatientId = CT.ContactId  "
                                                   + " left join (select p.contactid as PatientEHRId ,pinsu.carrierid as PrimaryInsuranceId, ps.insuranceCode as PrimarySubcriberId, "
                                                   + " pc.company as PrimaryInsuranceCompanyName,pc.PhoneNumber as Prim_Ins_Company_Phonenumber  from dbo.Contact as p "
                                                   + " left outer join dbo.Subscriber as ps on ps.contactid = p.contactid  "
                                                   + " LEFT OUTER join InsurancePlan as pinsu on pinsu.PlanId = ps.InsuranceplanId "
                                                   + " LEFT OUTER join Carrier as pc on pc.carrierid = pinsu.carrierid "
                                                   + " LEFT OUTER JOIN Coverage AS Pcov on pcov.Subscriberid =  ps. Subscriberid  and pcov.ContactId=ps.ContactId where pcov.coveragetype = 1) as PrimIns on PrimIns.PatientEHRId = CT.ContactId "
                                                   + " left join (select p.contactid as PatientEHRId ,pinsu.carrierid as SecondaryInsuranceId, ps.insuranceCode as SecondarySubcriberId, "
                                                   + " pc.company as SecondaryInsuranceCompanyName , pc.PhoneNumber as Sec_Ins_Company_Phonenumber from dbo.Contact as p  "
                                                   + " left outer join dbo.Subscriber as ps on ps.contactid = p.contactid   "
                                                   + " LEFT OUTER join InsurancePlan as pinsu on pinsu.PlanId = ps.InsuranceplanId  "
                                                   + " LEFT OUTER join Carrier as pc on pc.carrierid = pinsu.carrierid  "
                                                   + " LEFT OUTER JOIN Coverage AS Pcov on pcov.Subscriberid = ps.Subscriberid  and pcov.ContactId=ps.ContactId where pcov.coveragetype = 2) as SecIns on SecIns.PatientEHRId = CT.ContactId  "
                                                   + " LEFT JOIN ( select  AA.ContactId, ap.Reason, rl.ReasonId, CONVERT(varchar, AA.DueDate)+ '@' + ap.Reason + '@' + CONVERT(varchar, rl.ReasonId)+ '|' AS DueDate from appointment ap inner join ( SELECT ap1.AppointmentId, ap1.Contactid, ap1.OriginalDate  AS DueDate FROM appointment ap1 INNER JOIN ( SELECT MAX(AppointmentId) AS AppointmentId, Contactid, MAX(OriginalDate) AS MaxDateTime from appointment ap1 inner join ReasonLookup rl1 on rl1.Reason = ap1.Reason where StatusId = 7 and rl1.IsRecall = 1 group by contactid ) groupedap1  ON ap1.Contactid = groupedap1.Contactid and ap1.AppointmentId = groupedap1.AppointmentId  ) AS AA ON ap.ContactId = aa.ContactId and ap.AppointmentId=AA.AppointmentId LEFT join ReasonLookup rl on rl.Reason = ap.Reason ) AS AA ON AA.ContactId = CT.ContactId "
                                                   + " LEFT JOIN ( select CT.ContactId, convert( varchar, ( case when userecall = 1 then ( case when recallinterval = 'D' then DATEADD( day, RecallLength,  AC.DueDate  ) when recallinterval  IN ('M', 'Months', 'Month') then DATEADD(month, RecallLength, AC.DueDate) when recallinterval = 'W' then DATEADD( week, RecallLength, AC.DueDate  ) else DATEADD(year, RecallLength, AC.DueDate) end ) else null end ))+'@'+AC.Reason+'@'+ CONVERT(varchar, AC.ReasonId)+ '|' AS DueDate FROM Contact CT INNER join ( select AA.*, ap.Reason, rl.ReasonId from appointment ap inner join(  Select ap1.Contactid, ap1.AppointmentId, ap1.AppointmentDate  AS DueDate FROM appointment ap1 INNER JOIN ( SELECT Contactid, MAX(Appointmentdate) AS MaxDateTime, MAX(AppointmentId) AS AppointmentId from appointment ap1 inner join ReasonLookup rl1 on rl1.Reason = ap1.Reason where AP1.FlowState = 'Completed' and rl1.IsRecall = 1 group by contactid ) groupedap1 ON ap1.Contactid = groupedap1.Contactid AND ap1.AppointmentId = groupedap1.AppointmentId ) AS AA ON ap.ContactId = aa.ContactId and ap.AppointmentId=AA.AppointmentId  LEFT join ReasonLookup rl on rl.Reason = ap.Reason where AP.FlowState = 'Completed' and rl.IsRecall = 1 ) AS AC ON CT.contactid = AC.contactid and ct.UseRecall = 1  ) AS AB ON AB.ContactId = CT.ContactId "
                                                   + " LEFT JOIN ( Select AA.ContactId, CASE WHEN AA.Reason IS not NULL THEN convert( varchar, Max( DATEADD(month, 6, AD.DueDate)))+'@'+AA.Reason + '@' + convert(varchar, RL.ReasonId) + '|' else CONVERT( varchar, Max( DATEADD(month, 6, AD.DueDate) ) )+ '@NoReason@0|' END as DueDate from appointment AA INNER join (  select AA.ContactID,MAX(AA.AppointmentId) AS AppointmentId,max(AA.AppointmentDate) as DueDate from Appointment AA left join EstimateEntry ES on AA.AppointmentId = ES.AppointmentId left join ReasonLookup RL on RL.Reason = AA.Reason where AA.FlowState = 'Completed' and ( ES.ProcedureCode like 'D1110%' or ES.ProcedureCode like 'D1120%' or ES.ProcedureCode like 'D4341%' or ES.ProcedureCode like 'D4342%' or ES.ProcedureCode like 'D4910%' or ES.ProcedureCode like '01103%' or ES.ProcedureCode like '01202%' or ES.ProcedureCode like '1103%' or ES.ProcedureCode like '11101%' or ES.ProcedureCode like '11107%' or ES.ProcedureCode like '11111%' or ES.ProcedureCode like '11112%' or ES.ProcedureCode like '11113%' or ES.ProcedureCode like '11114%' or ES.ProcedureCode like '11117%' or ES.ProcedureCode like '1202%' or ES.ProcedureCode like '12111%' or ES.ProcedureCode like '12113%' or ES.ProcedureCode like '13401%' ) group by AA.ContactId ) as AD ON AA.ContactId = AD.ContactId and AA.AppointmentId=AD.AppointmentId left join EstimateEntry ES on AA.AppointmentId = ES.AppointmentId left join ReasonLookup RL on RL.Reason = AA.Reason where AA.FlowState = 'Completed' and ( ES.ProcedureCode like 'D1110%' or ES.ProcedureCode like 'D1120%' or ES.ProcedureCode like 'D4341%' or ES.ProcedureCode like 'D4342%' or ES.ProcedureCode like 'D4910%' or ES.ProcedureCode like '01103%' or ES.ProcedureCode like '01202%' or ES.ProcedureCode like '1103%' or ES.ProcedureCode like '11101%' or ES.ProcedureCode like '11107%' or ES.ProcedureCode like '11111%' or ES.ProcedureCode like '11112%' or ES.ProcedureCode like '11113%' or ES.ProcedureCode like '11114%' or ES.ProcedureCode like '11117%' or ES.ProcedureCode like '1202%' or ES.ProcedureCode like '12111%' or ES.ProcedureCode like '12113%' or ES.ProcedureCode like '13401%' ) group by AA.ContactId, AA.Reason, RL.ReasonId  ) AS AC ON AC.ContactId = CT.ContactId "
                                                   + " LEFT JOIN ( SELECT MIN(CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME)) As FirstVisit_Date, A.ContactID FROM Appointment A INNER JOIN Contact CT on A.ContactId = CT.ContactId And Statusid NOT IN (3, 6, 7, 8, 9) Group By A.ContactID) AS FData on FData.ContactID = CT.CONTACTID "
                                                   + " LEFT JOIN ( SELECT Max(CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME)) As LastVisit_Date, A.ContactID FROM Appointment A Inner Join Contact CT on A.ContactID = CT.ContactID And Statusid NOT IN (3, 6, 7, 8, 9) And AppointmentDate <= GetDate() Group By A.ContactID) As LData on LData.ContactID = CT.ContactID "
                                                   + " LEFT JOIN ( SELECT Min(CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME)) As nextvisit_date, A.ContactID FROM  Appointment A Inner Join Contact CT on CT.ContactID = A.ContactID And Statusid NOT IN (3, 6, 7, 8, 9) And AppointmentDate > GetDate() Group By A.ContactID) NData on NData.ContactID = CT.ContactID "
                                                   + " where CT.FirstName is not null and CT.FirstName<>'' and CT.LastName<>'' and CT.LastName is not null";

        // + " LEFT JOIN (select ct.contactid, MAX ( case when userecall = 1 then  ( case when recallinterval = 'D' then DATEADD( day, RecallLength,  ap.AppointmentDate  ) when recallinterval  IN ('M','Months','Month')  then DATEADD( month, RecallLength,  ap.AppointmentDate  ) when recallinterval = 'W' then DATEADD( week, RecallLength,  ap.AppointmentDate  ) else DATEADD( year, RecallLength,  ap.AppointmentDate  )  end ) else DATEADD(day, RecallLength,ap.AppointmentDate  ) end ) AS DueDate		 FROM Contact CT inner join ( select distinct ct.contactid, ( case when ( select count(1) from appointment where contactid = ct.contactid and (CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME)) > getdate()) > 0 then ( Select MAX(CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME)) from appointment where contactid = ct.contactid )  else ( Select MAX(CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME)) from appointment where contactid = ct.contactid and (CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME)) < getdate()) end ) AS AppointmentDate from appointment a inner join contact ct on a.contactid = ct.contactid)  ap on ct.contactid = ap.contactid  group by ct.contactid) AS RC ON RC.ContactId = CT.ContactId  ";
        //+ " WHERE CT.IsPatient = 1 ";

        public static string GetTrackerAppointmentsPatientData = " set transaction isolation level read uncommitted  SELECT DISTINCT 0 AS Clinic_Number, 1 as Service_Install_Id, 0 AS Patient_LocalDB_ID,CT.ContactId AS Patient_EHR_ID,'' AS Patient_Web_ID,CT.FirstName AS First_name,CT.LastName AS Last_name, '' AS Middle_Name,CT.Title AS Salutation, "
                                                  + " case when CT.IsActive = 0 then 'I' else 'A' end AS [Status],( case WHEN CT.IsPatient = 1 AND CT.IsActive = 1 then 'Active' when  CT.IsPatient = 1 AND CT.IsActive = 0 AND CT.InactiveReason = 'Deceased' THEN 'Deleted' when  CT.IsPatient = 1 AND CT.IsActive = 0 AND CT.InactiveReason != 'Deceased'  THEN 'InActive'  else 'NonPatient' end ) AS [EHR_Status], CONVERT(bit,CASE WHEN CT.IsActive = 0 AND CT.InactiveReason = 'Deceased' THEN 1 ELSE 0 END) AS Is_Deleted,CT.Sex,'' AS MaritalStatus,CT.BirthDate AS Birth_Date,CT.Email,ISNULL(CT.MobileNumber,'') AS Mobile,ISNULL(CT.PhoneNumber,'') AS Home_Phone,ISNULL(CT.WorkPhoneNumber,'') AS Work_Phone, "
                                                  + " ISNULL(CT.Address1,'') AS Address1,ISNULL(CT.Address2,'') AS Address2, ISNULL(CT.City,'') AS City,'' AS [State],ISNULL(CT.PostalCode,'') AS Zipcode,'' AS ResponsibleParty_Status, "
                                                  + " 0 AS CurrentBal,0 AS ThirtyDay,0 AS SixtyDay,0 AS NinetyDay,0 AS Over90, CT.IdentificationNumber AS ssn,Case when CT.IsStudent = 1 THEN CT.Company ELSE '' END AS School, Case when CT.IsStudent = 0 THEN CT.Company ELSE '' END AS Employer,"
                                                  + " FirstVisit_Date, "
                                                  + " LastVisit_Date, "
                                                  //+ " (SELECT TOP 1 CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME) FROM Appointment WHERE Statusid NOT IN (3,6,7,8,9) AND ContactId = CT.ContactId Order by AppointmentDate  ) AS FirstVisit_Date, "
                                                  //+ " (SELECT TOP 1 CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME) FROM Appointment WHERE Statusid NOT IN (3,6,7,8,9) AND ContactId = CT.ContactId AND AppointmentDate <= GetDate() Order by AppointmentDate DESC ) AS LastVisit_Date, "
                                                  + " ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance,ISNULL(PrimIns.PrimaryInsuranceCompanyName,'') AS Primary_Insurance_CompanyName,Prim_Ins_Company_Phonenumber,isnull(PrimIns.PrimarySubcriberId,'') as Primary_Ins_Subscriber_ID, "
                                                  + " ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance,ISNULL(SecIns.SecondaryInsuranceCompanyName,'') AS Secondary_Insurance_CompanyName,Sec_Ins_Company_Phonenumber,CT.ClientId AS Guar_ID,isnull(SecIns.SecondarySubcriberId,'') as Secondary_Ins_Subscriber_ID, "
                                                  + " ISNULL(CT.ProviderId,0) AS Pri_Provider_ID,ISNULL(CT.ProviderId,0) AS Sec_Provider_ID, ISNULL( CP.IsSMSEnabled,0) AS ReceiveSms,ISNULL( CT.HasEmailConsent,0) AS ReceiveEmail, "
                                                  + " nextvisit_date, "
                                                  //+ " (SELECT TOP 1 CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME) FROM Appointment WHERE Statusid NOT IN (3,6,7,8,9) AND ContactId = CT.ContactId AND AppointmentDate > GetDate() Order by AppointmentDate  ) AS nextvisit_date, "
                                                  // + " Convert(varchar,RC.DueDate) + '@None@1|' AS due_date,ISNULL((( CV.SingleBasicAdjustmentTotal + CV.SingleMajorAdjustmentTotal +  CV.SingleOrthoAdjustmentTotal + CV.SinglePreventativeAdjustmentTotal ) - INvoice.InsuranceAmount ),0) AS remaining_benefit,ISNULL(Invoice.InsurancePaidAmount,0) AS collect_payment,CT.CreatedTimeStamp AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated,ISNULL(CT.NickName,'') AS preferred_name,ISNULL(INvoice.InsuranceAmount,0) AS used_benefit, 1 As InsUptDlt,'Y' as ReceiveVoiceCall,CASE WHEN CT.Language = 'ENG' THEN 'English' WHEN CT.Language = 'FRE' THEN 'French' ELSE 'Spanish' END AS PreferredLanguage,CT.ContactMethod AS Patient_note, "
                                                  + " (select case when AA.DueDate is not NULL then AA.DueDate when AB.DueDate is not NULL then AB.DueDate when AC.DueDate is Not NULL then AC.DueDate else NULL end )  AS due_date,ISNULL((( CV.SingleBasicAdjustmentTotal + CV.SingleMajorAdjustmentTotal +  CV.SingleOrthoAdjustmentTotal + CV.SinglePreventativeAdjustmentTotal ) - INvoice.InsuranceAmount ),0) AS remaining_benefit,ISNULL(Invoice.InsurancePaidAmount,0) AS collect_payment,CT.CreatedTimeStamp AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated,ISNULL(CT.NickName,'') AS preferred_name,ISNULL(INvoice.InsuranceAmount,0) AS used_benefit, 1 As InsUptDlt,'Y' as ReceiveVoiceCall,CASE WHEN CT.Language = 'ENG' THEN 'English' WHEN CT.Language = 'FRE' THEN 'French' ELSE 'Spanish' END AS PreferredLanguage,CT.ContactMethod AS Patient_note, "
                                                  + "  C2.ContactId AS responsiblepartyid,C2.FirstName AS ResponsibleParty_First_Name,C2.LastName AS ResponsibleParty_Last_Name,C2.IdentificationNumber AS responsiblepartyssn,C2.BirthDate AS responsiblepartybirthdate,"
                                                  + " EM.ContactId AS EmergencyContactId,EM.FirstName AS EmergencyContact_First_Name,EM.LastName AS EmergencyContact_Last_Name,EM.MobileNumber AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name ,'' AS Spouse_Last_Name,'' AS driverlicense,'' AS groupid "
                                                  + " FROM CONTACT CT  "
                                                  + " Inner Join Appointment AP  On CT.ContactID = AP.ContactID "
                                                  + " LEFT join  Contact C2 on CT.ClientId = C2.ContactId"
                                                  + " LEFT join ContactRelationship CR on CT.ContactId=CR.ContactId AND CR.RelationshipType = 3 "
                                                  + " LEFT join contact EM on EM.contactid = CR.RelatedContactId"
                                                  + " LEFT JOIN ContactPhone CP ON CP.ContactId = CT.ContactId AND CP.IsDefault = 1  "
                                                  + " LEFT JOIN (select contactId, SUM(SingleBasicAdjustmentTotal) AS SingleBasicAdjustmentTotal, SUM(SingleMajorAdjustmentTotal) AS SingleMajorAdjustmentTotal,SUM(SingleOrthoAdjustmentTotal) AS SingleOrthoAdjustmentTotal ,SUM(SinglePreventativeAdjustmentTotal) AS SinglePreventativeAdjustmentTotal  from Coverage Group by contactId) CV ON CV.ContactId = CT.ContactId  "
                                                  + " LEFT JOIN ( SELECT SUM(InsuranceAmount) AS InsuranceAmount, SUM(InsurancePaidAmount) AS InsurancePaidAmount,PatientId FROM Invoice Group by PatientId ) AS Invoice ON Invoice.PatientId = CT.ContactId  "
                                                  + " left join (select p.contactid as PatientEHRId ,pinsu.carrierid as PrimaryInsuranceId, ps.insuranceCode as PrimarySubcriberId, "
                                                   + " pc.company as PrimaryInsuranceCompanyName,pc.PhoneNumber as Prim_Ins_Company_Phonenumber  from dbo.Contact as p "
                                                   + " left outer join dbo.Subscriber as ps on ps.contactid = p.contactid  "
                                                   + " LEFT OUTER join InsurancePlan as pinsu on pinsu.PlanId = ps.InsuranceplanId "
                                                   + " LEFT OUTER join Carrier as pc on pc.carrierid = pinsu.carrierid "
                                                   + " LEFT OUTER JOIN Coverage AS Pcov on pcov.Subscriberid =  ps. Subscriberid  and pcov.ContactId=ps.ContactId where pcov.coveragetype = 1) as PrimIns on PrimIns.PatientEHRId = CT.ContactId "
                                                   + " left join (select p.contactid as PatientEHRId ,pinsu.carrierid as SecondaryInsuranceId, ps.insuranceCode as SecondarySubcriberId, "
                                                   + " pc.company as SecondaryInsuranceCompanyName , pc.PhoneNumber as Sec_Ins_Company_Phonenumber from dbo.Contact as p  "
                                                   + " left outer join dbo.Subscriber as ps on ps.contactid = p.contactid   "
                                                   + " LEFT OUTER join InsurancePlan as pinsu on pinsu.PlanId = ps.InsuranceplanId  "
                                                   + " LEFT OUTER join Carrier as pc on pc.carrierid = pinsu.carrierid  "
                                                   + " LEFT OUTER JOIN Coverage AS Pcov on pcov.Subscriberid = ps.Subscriberid  and pcov.ContactId=ps.ContactId where pcov.coveragetype = 2) as SecIns on SecIns.PatientEHRId = CT.ContactId  "
                                                   + " LEFT JOIN ( select  AA.ContactId, ap.Reason, rl.ReasonId, CONVERT(varchar, AA.DueDate)+ '@' + ap.Reason + '@' + CONVERT(varchar, rl.ReasonId)+ '|' AS DueDate from appointment ap inner join ( SELECT ap1.AppointmentId, ap1.Contactid, ap1.OriginalDate  AS DueDate FROM appointment ap1 INNER JOIN ( SELECT MAX(AppointmentId) AS AppointmentId, Contactid, MAX(OriginalDate) AS MaxDateTime from appointment ap1 inner join ReasonLookup rl1 on rl1.Reason = ap1.Reason where StatusId = 7 and rl1.IsRecall = 1 group by contactid ) groupedap1  ON ap1.Contactid = groupedap1.Contactid and ap1.AppointmentId = groupedap1.AppointmentId  ) AS AA ON ap.ContactId = aa.ContactId and ap.AppointmentId=AA.AppointmentId LEFT join ReasonLookup rl on rl.Reason = ap.Reason ) AS AA ON AA.ContactId = CT.ContactId "
                                                   + " LEFT JOIN ( select CT.ContactId, convert( varchar, ( case when userecall = 1 then ( case when recallinterval = 'D' then DATEADD( day, RecallLength,  AC.DueDate  ) when recallinterval  IN ('M', 'Months', 'Month') then DATEADD(month, RecallLength, AC.DueDate) when recallinterval = 'W' then DATEADD( week, RecallLength, AC.DueDate  ) else DATEADD(year, RecallLength, AC.DueDate) end ) else null end ))+'@'+AC.Reason+'@'+ CONVERT(varchar, AC.ReasonId)+ '|' AS DueDate FROM Contact CT INNER join ( select AA.*, ap.Reason, rl.ReasonId from appointment ap inner join(  Select ap1.Contactid, ap1.AppointmentId, ap1.AppointmentDate  AS DueDate FROM appointment ap1 INNER JOIN ( SELECT Contactid, MAX(Appointmentdate) AS MaxDateTime, MAX(AppointmentId) AS AppointmentId from appointment ap1 inner join ReasonLookup rl1 on rl1.Reason = ap1.Reason where AP1.FlowState = 'Completed' and rl1.IsRecall = 1 group by contactid ) groupedap1 ON ap1.Contactid = groupedap1.Contactid AND ap1.AppointmentId = groupedap1.AppointmentId ) AS AA ON ap.ContactId = aa.ContactId and ap.AppointmentId=AA.AppointmentId  LEFT join ReasonLookup rl on rl.Reason = ap.Reason where AP.FlowState = 'Completed' and rl.IsRecall = 1 ) AS AC ON CT.contactid = AC.contactid and ct.UseRecall = 1  ) AS AB ON AB.ContactId = CT.ContactId "
                                                   + " LEFT JOIN ( Select AA.ContactId, CASE WHEN AA.Reason IS not NULL THEN convert( varchar, Max( DATEADD(month, 6, AD.DueDate)))+'@'+AA.Reason + '@' + convert(varchar, RL.ReasonId) + '|' else CONVERT( varchar, Max( DATEADD(month, 6, AD.DueDate) ) )+ '@NoReason@0|' END as DueDate from appointment AA INNER join (  select AA.ContactID,MAX(AA.AppointmentId) AS AppointmentId,max(AA.AppointmentDate) as DueDate from Appointment AA left join EstimateEntry ES on AA.AppointmentId = ES.AppointmentId left join ReasonLookup RL on RL.Reason = AA.Reason where AA.FlowState = 'Completed' and ( ES.ProcedureCode like 'D1110%' or ES.ProcedureCode like 'D1120%' or ES.ProcedureCode like 'D4341%' or ES.ProcedureCode like 'D4342%' or ES.ProcedureCode like 'D4910%' or ES.ProcedureCode like '01103%' or ES.ProcedureCode like '01202%' or ES.ProcedureCode like '1103%' or ES.ProcedureCode like '11101%' or ES.ProcedureCode like '11107%' or ES.ProcedureCode like '11111%' or ES.ProcedureCode like '11112%' or ES.ProcedureCode like '11113%' or ES.ProcedureCode like '11114%' or ES.ProcedureCode like '11117%' or ES.ProcedureCode like '1202%' or ES.ProcedureCode like '12111%' or ES.ProcedureCode like '12113%' or ES.ProcedureCode like '13401%' ) group by AA.ContactId ) as AD ON AA.ContactId = AD.ContactId and AA.AppointmentId=AD.AppointmentId left join EstimateEntry ES on AA.AppointmentId = ES.AppointmentId left join ReasonLookup RL on RL.Reason = AA.Reason where AA.FlowState = 'Completed' and ( ES.ProcedureCode like 'D1110%' or ES.ProcedureCode like 'D1120%' or ES.ProcedureCode like 'D4341%' or ES.ProcedureCode like 'D4342%' or ES.ProcedureCode like 'D4910%' or ES.ProcedureCode like '01103%' or ES.ProcedureCode like '01202%' or ES.ProcedureCode like '1103%' or ES.ProcedureCode like '11101%' or ES.ProcedureCode like '11107%' or ES.ProcedureCode like '11111%' or ES.ProcedureCode like '11112%' or ES.ProcedureCode like '11113%' or ES.ProcedureCode like '11114%' or ES.ProcedureCode like '11117%' or ES.ProcedureCode like '1202%' or ES.ProcedureCode like '12111%' or ES.ProcedureCode like '12113%' or ES.ProcedureCode like '13401%' ) group by AA.ContactId, AA.Reason, RL.ReasonId  ) AS AC ON AC.ContactId = CT.ContactId "
                                                   + " LEFT JOIN ( SELECT MIN(CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME)) As FirstVisit_Date, A.ContactID FROM Appointment A INNER JOIN Contact CT on A.ContactId = CT.ContactId And Statusid NOT IN (3, 6, 7, 8, 9) Group By A.ContactID) AS FData on FData.ContactID = CT.CONTACTID "
                                                   + " LEFT JOIN ( SELECT Max(CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME)) As LastVisit_Date, A.ContactID FROM Appointment A Inner Join Contact CT on A.ContactID = CT.ContactID And Statusid NOT IN (3, 6, 7, 8, 9) And AppointmentDate <= GetDate() Group By A.ContactID) As LData on LData.ContactID = CT.ContactID "
                                                   + " LEFT JOIN ( SELECT Min(CAST(AppointmentDate as DATETIME) + CAST(AppointmentTime as DATETIME)) As nextvisit_date, A.ContactID FROM  Appointment A Inner Join Contact CT on CT.ContactID = A.ContactID And Statusid NOT IN (3, 6, 7, 8, 9) And AppointmentDate > GetDate() Group By A.ContactID) NData on NData.ContactID = CT.ContactID "
                                                   + " WHERE (AP.AppointmentDate > @ToDate) and CT.FirstName is not null and CT.FirstName<>'' and CT.LastName<>'' and CT.LastName is not null";

        public static string GetTrackerPatientStatusNew_Existing = "select ContactId as Patient_EHR_Id from Contact as CT where CT.IsPatient = 1 and CT.ContactId not in (select CT.ContactId from Contact as CT  Inner Join Appointment AP  On CT.ContactID = AP.ContactID where AP.AppointmentDate < getdate() and AP.FlowState = 'Completed')";
        public static string GetTrackerPatientListData = " SELECT  0 AS Clinic_Number,1 as Service_Install_Id,ContactId AS Patient_EHR_ID,FirstName,LastName, (FirstName + ' ' + LastName) AS Patient_Name,BirthDate as birth_date, REPLACE(REPLACE(REPLACE(REPLACE(MobileNumber,'-', ''),'(',''),')',''),' ','') AS Mobile,REPLACE(REPLACE(REPLACE(REPLACE(PhoneNumber,'-', ''),'(',''),')',''),' ','') AS Home_Phone,REPLACE(REPLACE(REPLACE(REPLACE(WorkPhoneNumber,'-', ''),'(',''),')',''),' ','') AS Work_Phone,CASE WHEN ISActive = 1 THEN 'A' ELSE 'I' END AS  Status, 0 AS responsible_party,Email From Contact ";

        public static string GetTrackerPatientIdsData = "select ContactId as Patient_EHR_ID from Contact CT where CT.FirstName is not null and CT.FirstName<>'' and CT.LastName<>'' and CT.LastName is not null";
        
        public static string InsertPatientDetails = "INSERT INTO Contact ( ClientId,Title,FirstName,LastName,NickName,JrSr,Pronunciation,Address1,Address2,City,RegionId,PostalCode,PhoneNumber,WorkPhoneNumber,FaxNumber,OtherPhoneNumber,OtherType,Extension1,Extension2,Email,Language,Company,Occupation, "
                                                      + " UseRecall,UseShortRecall,UseStatement,IsActive,EnteredDate,InactiveDate,InactiveReason,IsPatient,IsVendor,IsProfessional,IsOther,BirthDate,Sex,IdentificationNumber,PracticeId,ProviderId,Provider2Id,RecallLength,RecallInterval,SIN,Categories,"
                                                      + " ExcludeInterest,ContactMethod,Referred,ReferredId,IsSignature,IsHandicapped,IsStudent,Model,Procedures,ExternalContactId,Chart,WebUpdateLink,FeeGuideKey,SeriesKey,RelativeYear,DefaultImageId,ModifiedUserId,ModifiedTimeStamp,CreatedUserId,"
                                                      + " CreatedTimeStamp,ModifiedMachineName,CreatedMachineName,MobileNumber,ContactMethodAppt,ContactMethodFinancial,ContactMethodMarketing,DefaultPhoneId,IdentificationType,HasEmailConsent,DoseSpotId,BookSameDayOnly )VALUES ("
                                                      + "  @Patient_Gur_id,'',@FirstName,@LastName,'','','','','','',1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,@Email,'ENG',NULL,NULL,1,0,1,1,GETDATE(),NULL,NULL,1,0,0,0,@BirthDate,'',NULL,ISNULL((SELECT TOP 1 PracticeId FROM Contact),''),@ProviderId,NULL,6,'Month',"
                                                       + "  NULL,NULL,0,NULL,NULL,NULL,0,0,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,@ModifiedUserId,GETDATE(),@CreatedUserId,GETDATE(),ISNULL((SELECT TOP 1 MODIFIEDMACHINENAME FROM CONTACT),''),"
                                                      + "  ISNULL((SELECT TOP 1 CREATEDMACHINENAME FROM CONTACT),''),@MobileNo,1,2,2,(SELECT TOP 1 DefaultPHoneId From COntact),NULL,NULL,NULL,0) SELECT @@IDENTITY ";

        public static string InsertPatientContactDetails = " INSERT INTO ContactPhone ( ContactId,Phonetype,PhoneNumber,Extension,isDefault,isSmsEnabled,isLongDistance,PhoneDescription,PhoneNote )"
                                                            + " VALUES ( @ContactId,3,@MobileNo,NULL,1,0,0,NULL,NULL) SELECT @@IDENTITY "
                                                            + " ";

        public static string UpdatePatientDefaultPhone = " UPDATE Contact SET DefaultPhoneId = @DefaultPhoneId WHERE ContactId = @ContactId ";

        public static string InsertAppointmentDetails = " INSERT INTO Appointment ( ContactId,ProviderId,ScheduleColumnId,IsPreconfirmed,IsConfirmed,MasterId,AppointmentDate,AppointmentTime,Duration,OriginalDate,Reason,Detail,AppointmentAmount,"
                                                        + " IsRecall,IsPersonal,IsAllDayAppointment,HasAlarm,NotifyTime,StatusId,CheckIn,InChair,OutChair,CheckOut,FlowState,FlowChange,Comments,BookedUserId,BookedTimeStamp,BookedMachineName,"
                                                        + " CreatedUserId,CreatedTimeStamp,ModifiedUserId,ModifiedTimeStamp,ModifiedMachineName,CreatedMachineName,RebookInfo,ConfirmedTimeStamp,ConfirmedUserId,ConfirmedMachineName)"
                                                        + " VALUES ( @ContactId,@ProviderId,@ScheduleColumnId,0,0,NULL,CONVERT(VARCHAR(10), @StartDate, 111),CONVERT(VARCHAR(10), @StartDate, 108),"
                                                        + " CONVERT(TIME,@EndDate - @StartDate),"
                                                         + " CONVERT(VARCHAR(10), @StartDate, 111),(select Reason From ReasonLookup where ReasonId = @reasionId),'Web Appointment',0.00,0,0,0,0,NULL,@StatusId,NULL,NULL,NULL,NULL,NULL,NULL,NULL, @BookedUserId ,GetDate(),ISNULL(( SELECT TOP 1 BookedMachineName FROM Appointment ),''),@CreatedUserId,"
                                                        + "  GetDate(),@ModifiedUserId,GetDate(),ISNULL(( SELECT TOP 1 ModifiedMachineName FROM Appointment ),''),ISNULL(( SELECT TOP 1 ModifiedMachineName FROM Appointment ),''),NULL,NULL,NULL,NULL) SELECT @@IDENTITY ";




        public static string InsertEstimate = "INSERT INTO Estimate (PatientId,EstimateDate,EstimateAmount,PracticeId,ProviderId,IsPrinted,IsPredetermination,Comment,InsuranceNote,InternalNote,PrimaryInsuranceAmount,OtherInsuranceAmount,UseEDI,SortOrder,GroupLabel,IsOrthodontics,DiagnosticCode,InstitutionCode,AccidentDate,InitialDateUpper,InitialDateLower,MaterialUpper,MaterialLower,InsuranceClaimStatus,IsActive,ModifiedUserId,ModifiedTimeStamp,CreatedUserId,CreatedTimeStamp,ModifiedMachineName,CreatedMachineName) values "
                                            + " ( @Patient_EHR_Id,@Appointment_DateTime,isnull((select TOP 1 FGP1.Amount from FeeGuideProcedure FGP INNER JOIN FeeGuidePrice FGP1 ON FGP.FeeGuideProcedureId = FGP1.FeeGuideProcedureId WHERE FGP.Code IN (@ProcedureCodeId)),0),(Select Top 1 PracticeId FROm Estimate ),@Provider_EHR_Id,0,0,@Comment,NULL,NULL,0.00,0.00,0,0,(select top 1 grouplabel from Estimate),0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,1,@ModifiedUserId,getdate(),@CreatedUserId,getdate(),(select top 1 ModifiedMachineName FROM Estimate),(select top 1 CreatedMachineName FROM Estimate)) SELECT @@IDENTITY ";

        public static string InsertEstimateEntry = "INSERT INTO EstimateEntry (EstimateId,AppointmentId,ProviderId,PhaseLabel,SubGroup,SortOrder,"
                                                   + "[Description],ProcedureCode,TotalAmount,PatientAmount,PrimaryInsuranceAmount,OtherInsuranceAmount,"
                                                   + " DiscountAmount,AssociatedFeeCode,ProcedureType,Remarks,InsuranceApprovalStatusId,TreatmentPlanStatusId,"
                                                   + "SurfaceDescription,ToothDescription,AreaData,AreaCount,AreaType,ConditionType,MaterialType,RestorationState,ToothNumber,Units,ParentId,"
                                                   + "ConditionValue,IsPracticeManagementProcessed,IsChartable,CoverageExplanation,TransactionDate,EntryDate,CreatedUserId,CreatedTimeStamp,"
                                                   + "ModifiedUserId,ModifiedTimeStamp,ModifiedMachineName,CreatedMachineName,AlternateToothId,ReferralId,ProductId) VALUES (@EstimateId,@Appointment_EHR_Id,@Provider_EHR_Id,"
                                                   + "(Select Top 1 PhaseLabel FROM EstimateEntry),(select top 1 subgroup from EstimateEntry),(select MAX(sortorder) + 1 FROM EstimateEntry Where EstimateId = @EstimateId),"
                                                   + "isnull((select TOP 1 CodeDescription from FeeGuideProcedure WHERE Code = @ProcedureCodeId),'0'),"
                                                   + "isnull((select TOP 1 Code from FeeGuideProcedure WHERE Code = @ProcedureCodeId),'0'),"
                                                   + "isnull((select TOP 1 FGP1.Amount from FeeGuideProcedure FGP INNER JOIN FeeGuidePrice FGP1 ON FGP.FeeGuideProcedureId = FGP1.FeeGuideProcedureId WHERE FGP.Code = @ProcedureCodeId),'0'),"
                                                   + "isnull((select TOP 1 FGP1.Amount from FeeGuideProcedure FGP INNER JOIN FeeGuidePrice FGP1 ON FGP.FeeGuideProcedureId = FGP1.FeeGuideProcedureId WHERE FGP.Code = @ProcedureCodeId),'0'),"
                                                   + "0,0,0,NULL,NULL,NULL,0,1,NULL,NULL,0,0,0,0,0,0,0,NULL,NULL,0,0,1,NULL,@Appointment_DateTime,"
                                                    + "getdate(),@CreatedUserId,getdate(),@ModifiedUserId,getdate(),"
                                                   + "(select top 1 ModifiedMachineName FROM EstimateEntry),(select top 1 CreatedMachineName FROM EstimateEntry),NULL,NULL,NULL)";
        public static string UpdateAppointmentStatusFromWeb = " UPDATE Appointment SET @FieldToUpdate WHERE AppointmentId = @AppointmentId ";

        public static string Update_Receive_SMS_Patient_EHR_Live_To_TrackerEHR = " UPDATE CONTACT SET IsSMSEnabled = @receives_sms WHERE ContactId = @patient_id ";

        public static string UpdateEstimateEntry = "UPDATE EstimateEntry set AppointmentId = @Appointment_EHR_Id where EstimateEntryId IN (select EstimateEntryId from EstimateEntry EE, Estimate E where E.EstimateId=EE.EstimateId and PatientId = @Patient_EHR_Id) and ProcedureCode = @ProcedureCodeId";

        public static string GetEstimateEntryId = "select EstimateEntryId from EstimateEntry EE, Estimate E where E.EstimateId=EE.EstimateId and PatientId = @Patient_EHR_Id";

        public static string GetPatientTableColumnsName = " select 0 AS Clinic_Number,1 as Service_Install_Id,Column_Name AS EHRColumnName,Table_Name AS TableName,Data_Type AS EHRDataType,IS_Nullable AS AllowNull,"
                                                          + "   isnull( (Case when Character_maximum_length is not null then Character_maximum_length else numeric_precision end ),0) AS Size, '' AS PatientFormColumnsName,'' AS DefaultValue  "
                                                          + "    from information_schema.columns where table_name = 'Contact' AND Column_Name NOT IN ('ContactId','FirstNameSoundex','LastNameSoundex')  ";

        public static string Update_Patinet_Record_By_Patient_Form = " UPDATE Patient SET ColumnName = @ehrfield_value WHERE Patient_Id = @Patient_EHR_ID ";

        public static string GetProviderCustomHours = " set transaction isolation level read uncommitted "
                                                    + " declare @l_tempdate datetime = @ToDate declare @l_strSql varchar(MAX)  set @l_strSql = ' set transaction isolation level read uncommitted SET XACT_ABORT ON "
                                                    + " declare @lastsyncdate datetime = '''+ convert(varchar, @l_tempdate)+'''"
                                                    + " declare @l_sIndex int"
                                                    + " declare @l_eIndex int"
                                                    + " declare @l_WeekSIndex int = 1"
                                                    + " declare @l_WeekEIndex int = 7 "
                                                    + " declare @l_ProviderId int,@l_ParentScheduleColumnId int,@l_startdatehour date,@l_weekday int,@l_PH_EHR_ID varchar(50),@l_PH_Web_ID VARCHAR(250),@l_Provider_EHR_ID VARCHAR(10),@l_Operatory_EHR_ID VARCHAR(100),@l_comment VARCHAR(2000),@l_HourId varchar(50),@l_ScheduleColumnId varchar(50),@l_StartDate date,@l_StartTime time,@l_EndTime time,@l_Reason varchar(2000),@l_ScheduleColumnName varchar(100)"
                                                    + " create table #Adit_OperatoryList (	 Id numeric(18,0) IDENTITY(1,1), ScheduleColumnId int, ProviderId int, ParentScheduleColumnId int, ScheduleColumnName varchar(100))"
                                                    + " create table #HourList ( Id numeric(18,0) IDENTITY(1,1), ScheduleColumnId int, StartDate Date, WeekDay int)"
                                                    + " create table #OperatoryCustomHour ( [ID] BIGINT IDENTITY(1,1),[PH_EHR_ID] NVARCHAR(50), [PH_Web_ID] NVARCHAR(250), [Provider_EHR_ID] NVARCHAR(10), [Operatory_EHR_ID] NVARCHAR(100), [StartTime] DATETIME, [EndTime] DATETIME, [comment] NVARCHAR(2000),[Entry_DateTime] DATETIME, [Last_Sync_Date] DATETIME, [is_deleted] BIT DEFAULT 0, [Is_Adit_Updated] BIT DEFAULT 0, [Clinic_Number] NVARCHAR(10) DEFAULT (0), [Service_Install_Id] NVARCHAR(8) DEFAULT (1), [WeekDays] int, [OperatoryName] varchar(100), [HourId] numeric(18,0), [StartDate] date,[IsTimeBlank] bit )"
                                                    + " CREATE TABLE #Provider_Result (Provider_EHR_Id VARCHAR(50),StartTime DateTime,EndTime DateTime,StartDate DAte,HourId Numeric(18,0),Ignore bit)"
                                                    + " INSERT INTO #HourList Select SchedulecolumnId , MAX (StartDate) AS startdate ,weekday from Hour WHERE startdate < @lastsyncdate Group by ScheduleColumnId,weekday order by SchedulecolumnId,weekday"
                                                    + " INsert into #Adit_OperatoryList select ScheduleColumnId,ProviderId,HoursColumnId,ScheduleColumnName from schedulecolumn where isactive = 1  order by schedulecolumnId "
                                                    + " set @l_sIndex = 1"
                                                    + " set @l_eIndex = ( Select count(1) FROM #Adit_OperatoryList )"
                                                    + " while(@l_sIndex <= @l_eIndex)"
                                                    + " BEGIN  select @l_ScheduleColumnId = ScheduleColumnId,@l_ProviderId = ProviderId,@l_ParentScheduleColumnId = ParentScheduleColumnId,@l_ScheduleColumnName=ScheduleColumnName  FROM #Adit_OperatoryList WHERE Id = @l_sIndex"
                                                    + "   SET @lastsyncdate = ( select  DATEADD(day, -2, getdate()))"
                                                    + "   WHILE ( @l_WeekSIndex <= @l_WeekEIndex AND @lastsyncdate <= (DATEADD(MONTH,4,getdate())) )"
                                                    + "   BEGIN SET @l_startdatehour = (SELECT CONVERT(CHAR(12),DATEADD(DAY, (DATEDIFF(DAY, ((@l_WeekSIndex + 5) % 7), @lastsyncdate) / 7) * 7 + 7, ((@l_WeekSIndex + 5) % 7)),111))"
                                                    + "     IF ( ( SELECT COUNT(1) FROM HOur where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex AND StartDate = @l_startdatehour ) > 0 )"
                                                    + "     BEGIN"
                                                    + "       IF EXIStS (SELECT 1 FROM HOur where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex AND StartDate = @l_startdatehour AND StartTime <> ''00:00:00.0000000'')"
                                                    + " 		BEGIN SELECT @l_HourId = HourId,@l_ScheduleColumnId = ScheduleColumnId, @l_StartDate = StartDate,@l_Weekday = Weekday, @l_StartTime = StartTime, @l_EndTime = EndTime , @l_Reason = Reason "
                                                    + " 			 FROM HOur where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex AND StartDate = @l_startdatehour"
                                                    + " 			 INSERT INTO #OperatoryCustomHour"
                                                    + " 			 SELECT convert( varchar, HourId ) + ''_'' + convert( varchar, @l_ProviderId )  + ''_'' + convert( varchar, ScheduleColumnId ) + ''_'' + CONVERT(CHAR(8),StartDate,112),"
                                                    + " 			 '''',@l_ProviderId,ScheduleColumnId,"
                                                    + " 			 (CAST(StartDate as DATETIME) + CAST(StartTime as DATETIME)), "
                                                    + " 			 (CAST(StartDate as DATETIME) + CAST((DATEADD(mi, @TrackerScheduleInterval,   EndTime)) as DATETIME)), "
                                                    + " 		      @l_Reason,getdate(),getdate(),0,0,0,1,WeekDay,@l_ScheduleColumnName,HourId,StartDate,0"
                                                    + " 		       FROM HOur where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex AND StartDate = @l_startdatehour 			 "
                                                    + " 	     END    "
                                                    + " 	     ELSE"
                                                    + " 	     BEGIN"
                                                    + " 			  SELECT @l_HourId = HourId,@l_ScheduleColumnId = ScheduleColumnId, @l_StartDate = StartDate,@l_Weekday = Weekday, @l_StartTime = StartTime, @l_EndTime = EndTime , @l_Reason = Reason "
                                                    + " 			 FROM HOur where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex AND StartDate = @l_startdatehour 			 "
                                                    + " 			 INSERT INTO #OperatoryCustomHour"
                                                    + " 			 SELECT convert( varchar, @l_HourId ) + ''_'' + convert( varchar, @l_ProviderId )  + ''_'' + convert( varchar, @l_ScheduleColumnId ) + ''_'' + CONVERT(CHAR(8),@l_startdatehour,112),"
                                                    + " 			 '''',@l_ProviderId,@l_ScheduleColumnId,"
                                                    + " 			 (CAST(@l_StartDate as DATETIME) + CAST(@l_StartTime as DATETIME)), "
                                                    + " 			 (CAST(@l_StartDate as DATETIME) + CAST(''00:07:00'' as DATETIME)), "
                                                    + " 		      @l_Reason,getdate(),getdate(),0,0,0,1,@l_WeekSIndex,@l_ScheduleColumnName,@l_HourId,@l_StartDate,1"
                                                    + " 	     END"
                                                    + "     END"
                                                    + "     ELSE IF ( ( select count(1) from ( Select SchedulecolumnId ,  MAX(StartDate) AS startdate ,weekday from Hour WHERE startdate < @l_startdatehour and Weekday = @l_WeekSIndex AND schedulecolumnId = @l_ScheduleColumnId Group by ScheduleColumnId,weekday ) AA ) > 0 )"
                                                    + "     BEGIN SELECT @l_HourId = HourId,@l_ScheduleColumnId = ScheduleColumnId, @l_StartDate = StartDate,@l_Weekday = Weekday, @l_StartTime = StartTime, @l_EndTime = EndTime , @l_Reason = Reason 		"
                                                    + " 				 FROM  HOur where ScheduleColumnId = @l_ScheduleColumnId "
                                                    + " 				 AND WeekDay = @l_WeekSIndex "
                                                    + " 				 AND StartDate = ( Select MAX(StartDate) from Hour WHERE startdate < @l_startdatehour and Weekday = @l_WeekSIndex "
                                                    + " 				 AND schedulecolumnId = @l_ScheduleColumnId Group by ScheduleColumnId,weekday )"
                                                    + " 			IF  ( @l_StartTime <> ''00:00:00.0000000'' )"
                                                    + " 			BEGIN"
                                                    + " 				 INSERT INTO #OperatoryCustomHour"
                                                    + " 				 SELECT convert( varchar, HourId ) + ''_'' + convert( varchar, @l_ProviderId )  + ''_'' + convert( varchar, ScheduleColumnId ) + ''_'' + CONVERT(CHAR(8),@l_startdatehour,112),"
                                                    + " 				 '''',@l_ProviderId,ScheduleColumnId,"
                                                    + " 				 (CAST(@l_startdatehour as DATETIME) + CAST(StartTime as DATETIME)), "
                                                    + " 				 (CAST(@l_startdatehour as DATETIME) + CAST((DATEADD(mi,  @TrackerScheduleInterval,   EndTime)) as DATETIME)), "
                                                    + " 				 @l_Reason,getdate(),getdate(),0,0,0,1,WeekDay,@l_ScheduleColumnName,HourId,StartDate,0"
                                                    + " 				 FROM HOur where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex AND StartDate = ( Select MAX(StartDate) from Hour WHERE startdate < @l_startdatehour and Weekday = @l_WeekSIndex AND schedulecolumnId = @l_ScheduleColumnId Group by ScheduleColumnId,weekday ) 	"
                                                    + " 		     END"
                                                    + " 		     ELSE"
                                                    + " 		     BEGIN"
                                                    + " 				 INSERT INTO #OperatoryCustomHour"
                                                    + " 				 SELECT convert( varchar, HourId ) + ''_'' + convert( varchar, @l_ProviderId )  + ''_'' + convert( varchar, ScheduleColumnId ) + ''_'' + CONVERT(CHAR(8),@l_startdatehour,112),"
                                                    + " 				 '''',@l_ProviderId,ScheduleColumnId,"
                                                    + " 				 (CAST(@l_startdatehour as DATETIME) + CAST(StartTime as DATETIME)), "
                                                    + " 				 (CAST(@l_startdatehour as DATETIME) + CAST(''00:07:00'' as DATETIME)), "
                                                    + " 				 @l_Reason,getdate(),getdate(),0,0,0,1,WeekDay,@l_ScheduleColumnName,HourId,StartDate,1"
                                                    + " 				 FROM HOur where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex AND StartDate = ( Select MAX(StartDate) from Hour WHERE startdate < @l_startdatehour and Weekday = @l_WeekSIndex AND schedulecolumnId = @l_ScheduleColumnId Group by ScheduleColumnId,weekday ) 	"
                                                    + " 		     END"
                                                    + "     END"
                                                    + "     ELSE"
                                                    + "     BEGIN"
                                                    + " 		IF EXIStS (SELECT 1 FROM #HourList where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex )"
                                                    + " 		BEGIN"
                                                    + " 			SELECT @l_HourId = HourId,@l_ScheduleColumnId = ScheduleColumnId, @l_StartDate = StartDate,@l_Weekday = Weekday, @l_StartTime = StartTime, @l_EndTime = EndTime , @l_Reason = Reason 		"
                                                    + " 				 FROM  HOur where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex AND StartDate = ( SELECT startdate FROM #HourList where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex )				 "
                                                    + " 			declare @date1 date = ( SELECT startdate FROM #HourList where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex )"
                                                    + " 			IF  ( @l_StartTime <> ''00:00:00.0000000'' )"
                                                    + " 			BEGIN"
                                                    + " 				 INSERT INTO #OperatoryCustomHour"
                                                    + " 				 SELECT convert( varchar, HourId ) + ''_'' + convert( varchar, @l_ProviderId )  + ''_'' + convert( varchar, ScheduleColumnId ) + ''_'' + CONVERT(CHAR(8),@l_startdatehour,112),"
                                                    + " 				 '''',@l_ProviderId,@l_ScheduleColumnId,"
                                                    + " 				 (CAST(@l_startdatehour as DATETIME) + CAST(StartTime as DATETIME)), "
                                                    + " 				 (CAST(@l_startdatehour as DATETIME) + CAST((DATEADD(mi, @TrackerScheduleInterval,   EndTime)) as DATETIME)), "
                                                    + " 				  Reason,getdate(),getdate(),0,0,0,1,Weekday,@l_ScheduleColumnName,HourId,@l_startdatehour,0"
                                                    + " 				  FROM HOur where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex AND StartDate = ( SELECT startdate FROM #HourList where ScheduleColumnId = @l_ScheduleColumnId AND WeekDay = @l_WeekSIndex )"
                                                    + " 			END"
                                                    + " 			ELSE"
                                                    + " 			BEGIN"
                                                    + " 				 INSERT INTO #OperatoryCustomHour"
                                                    + " 				 SELECT convert( varchar, @l_HourId ) + ''_'' + convert( varchar, @l_ProviderId )  + ''_'' + convert( varchar, @l_ScheduleColumnId ) + ''_'' + CONVERT(CHAR(8),@l_startdatehour,112),"
                                                    + " 				 '''',@l_ProviderId,@l_ScheduleColumnId,"
                                                    + " 				 (CAST(@l_startdatehour as DATETIME) + CAST(@l_StartTime as DATETIME)), "
                                                    + " 				 (CAST(@l_startdatehour as DATETIME) + CAST(''00:07:00'' as DATETIME)), "
                                                    + " 				  @l_Reason,getdate(),getdate(),0,0,0,1,@l_WeekSIndex,@l_ScheduleColumnName,@l_HourId,@l_StartDate,1	"
                                                    + " 			END"
                                                    + " 		END    "
                                                    + "     END"
                                                    + " 		SET @l_WeekSIndex = @l_WeekSIndex + 1	    "
                                                    + "     IF( @l_WeekSIndex = 8 )	"
                                                    + " 	BEGIN"
                                                    + " 		SET @l_WeekSIndex = 1"
                                                    + " 		SET @lastsyncdate = ( SELECT MAX(StartTime) FROM #OperatoryCustomHour where Operatory_EHR_ID = @l_ScheduleColumnId )"
                                                    + " 	END"
                                                    + "   END"
                                                    + "   SET @l_sIndex = @l_sIndex+ 1"
                                                    + " END"

                                                    + " INSERT INTO #OperatoryCustomHour"
                                                    + " SELECT convert( varchar, A.HourId ) + ''_'' + convert( varchar, B.ProviderId )  + ''_'' + convert( varchar, B.ScheduleColumnId ) + ''_'' + CONVERT(CHAR(8),A.StartDate,112),"
                                                    + " '''',A.Provider_EHR_Id,B.ScheduleColumnId, A.StartTime,A.EndTime,A.Comment,getdate(),getdate(),0,0,0,1,A.WeekDays,C.ScheduleColumnName,A.HourId,A.StartDate,A.IsTimeBlank"
                                                    + " FROM #OperatoryCustomHour A INNER JOIN  #Adit_OperatoryList B ON A.Operatory_EHR_ID = ISNULL( B.ParentScheduleColumnId  ,0)"
                                                    + " INNER JOIN ScheduleColumn C ON C.ScheduleColumnId = B.ScheduleColumnId"
                                                    + " order by starttime"

                                                    + " Select '''' AS OH_LocalDB_ID, PH_EHR_ID AS OH_EHR_ID,'''' AS OH_Web_ID,Operatory_EHR_ID,StartTime,EndTime,ISNULL(comment,'''') AS Comment,getdate() AS Entry_DateTime,getdate() Last_Sync_date,0 AS Is_deleted,0 AS Is_Adit_Updated,0 AS Clinic_number, 1 AS Service_Install_Id  FROM #OperatoryCustomHour order by convert(int, operatory_EHR_Id),StartTime"

                                                    + " INSERT INTO #Provider_Result"
                                                    + " SELECT AB.*,CASE WHEN AC.CNT > 1 AND CONVERT(CHAR(8), AB.StartTime,108) = ''00:00:00'' THEN 1 ELSE 0 END AS Ignor "
                                                    + " FROM ( SELECT DISTINCT Provider_EHR_Id,(StartTime),(EndTime),CONVERT(CHAR(8),StartTime,112) AS StartDate,MIN(hourId) AS HourId From #OperatoryCustomHour "
                                                    + " GROUP BY Provider_EHR_Id,CONVERT(CHAR(8),StartTime,112),StartTime,EndTime ) AS AB"
                                                    + " INNER JOIN ( SELECT Provider_EHR_id,StartDate,count(1) AS CNT FROM ("
                                                    + " SELECT DISTINCT Provider_EHR_Id,(StartTime),(EndTime),CONVERT(CHAR(8),StartTime,112) AS StartDate,CONVERT(CHAR(8), StartTime,108) AS STime From #OperatoryCustomHour "
                                                    + " GROUP BY Provider_EHR_Id,CONVERT(CHAR(8),StartTime,112),StartTime,EndTime ) AS AA"
                                                    + " GROUP BY Provider_EHR_id,StartDate ) AS AC ON AB.Provider_EHR_Id = AC.Provider_EHR_Id AND AB.StartDate = AC.StartDate "

                                                    + " SELECT '''' AS PH_LocalDB_ID, (convert( varchar, HourId ) + ''_'' + Provider_EHR_Id + ''_'' + convert(varchar, StartDate,112)) AS PH_EHR_ID,''''PH_Web_ID,Provider_EHR_Id,0 AS Operatory_EHR_ID,StartTime,EndTime,'''' AS comment,getdate() AS Entry_DateTime,getdate() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated,0 AS Clinic_Number,1 AS Service_Install_Id  FROM #Provider_Result WHERE Ignore = 0"

                                                    + " IF OBJECT_ID(''tempdb..#Adit_OperatoryList'') IS NOT NULL"
                                                    + " DROP TABLE #Adit_OperatoryList"
                                                    + " IF OBJECT_ID(''tempdb..#HourList'') IS NOT NULL"
                                                    + " DROP TABLE #HourList"
                                                    + " IF OBJECT_ID(''tempdb..#OperatoryCustomHour'') IS NOT NULL"
                                                    + " DROP TABLE #OperatoryCustomHour 		"
                                                    + " IF OBJECT_ID(''tempdb..#Provider_Result'') IS NOT NULL"
                                                    + " DROP TABLE #Provider_Result"

                                                    + " SET NOCOUNT OFF"
                                                    + " SET XACT_ABORT OFF '  exec(@l_strsql) ";

        public static string GetOperatoryCustomHours = "SELECT 0 AS Clinic_Number,1 as Service_Install_Id,'' AS OH_LocalDB_ID,HH.HourId AS OH_EHR_ID,'' AS OH_Web_ID,HH.ScheduleColumnId AS Operatory_EHR_ID,"
                                                   + " CAST(HH.StartDate as DATETIME) + case when cast(HH.StartTIme as time) = '00:00:00.0000000'  then  CAST('00:00:01.0000000' as time) else CAST(HH.StartTime as DATETIME) end AS StartTime,CAST(HH.StartDate as DATETIME) + case when cast(HH.EndTime as time) = '00:00:00.0000000' then  CAST('23:59:59.0000000' as time) else CAST(DATEADD(mi,15,HH.[EndTime]) as DATETIME) end AS EndTime,ISNULL( HH.Reason,'') AS [comment],"
                                                   + " '' AS Entry_DateTime,'' AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated"
                                                   + " FROM HOUR HH INNER JOIN ScheduleColumn SC ON SC.ScheduleColumnId = HH.ScheduleColumnId INNER JOIN Provider P ON P.ProviderId = SC.ProviderId where  HH.StartDate >= @ToDate AND HH.StartDate <= DATEADD(MONTH,9,@ToDate) and HH.StartTime <> HH.EndTime AND HH.Reason IS NOT NULL";

        public static string GetOperatoryOfficeHours = "select 0 AS Clinic_Number,1 as Service_Install_Id,0 as OOH_LocalDB_ID,Convert(varchar,HourId) as OOH_EHR_ID ,'' as OOH_Web_ID,'0' as Operatory_EHR_ID, "
                                                    + "  case when [Weekday] = 1 then 'Sunday' else "
                                                    + "  case when [Weekday] = 2 then 'Monday' else "
                                                    + "  case when [Weekday] = 3 then 'Tuesday'  else "
                                                    + "  case when [Weekday] = 4 then 'Wednesday'  else "
                                                    + "  case when [Weekday] = 5 then 'Thursday'  else "
                                                    + "  case when [Weekday] = 6 then 'Friday' else "
                                                    + "  case when [Weekday] = 7 then 'Saturday' end end end end end end end as [WeekDay], "
                                                    + "  cast(StartDate as datetime) + cast(StartTime as time) as StartTime1, "
                                                    + "  cast(StartDate as datetime) + cast(DATEADD(mi,15,[EndTime]) as time) as EndTime1, "
                                                    + "  cast ('01/01/2020 00:00:00' as datetime) as StartTime2, "
                                                    + "  cast ('01/01/2020 00:00:00' as datetime) as EndTime2, "
                                                    + "  cast ('01/01/2020 00:00:00' as datetime) as StartTime3, "
                                                    + "  cast ('01/01/2020 00:00:00' as datetime) as EndTime3, "
                                                    + "  getdate() as Entry_DateTime, "
                                                    + "  getdate() as Last_Sync_Date, "
                                                    + "  0 as is_deleted, "
                                                    + "  0 as Is_Adit_Updated "
                                                    + "  from [Hour] where [StartDate] in (SELECT min([StartDate]) [StartDate] "
                                                    + "   FROM [Tracker].[dbo].[Hour]) AND 1 = 2 ";


        public static string GetProviderOfficeHours = "select 0 AS Clinic_Number,1 as Service_Install_Id,0 as POH_LocalDB_ID,Convert(varchar,HourId) as POH_EHR_ID ,'' as POH_Web_ID,'0' as Provider_EHR_ID, "
                                                   + "  case when [Weekday] = 1 then 'Sunday' else "
                                                   + "  case when [Weekday] = 2 then 'Monday' else "
                                                   + "  case when [Weekday] = 3 then 'Tuesday'  else "
                                                   + "  case when [Weekday] = 4 then 'Wednesday'  else "
                                                   + "  case when [Weekday] = 5 then 'Thursday'  else "
                                                   + "  case when [Weekday] = 6 then 'Friday' else "
                                                   + "  case when [Weekday] = 7 then 'Saturday' end end end end end end end as [WeekDay], "
                                                   + "  cast(StartDate as datetime) + cast(StartTime as time) as StartTime1, "
                                                   + "  cast(StartDate as datetime) + cast(DATEADD(mi,15,[EndTime]) as time) as EndTime1, "
                                                   + "  cast ('01/01/2020 00:00:00' as datetime) as StartTime2, "
                                                   + "  cast ('01/01/2020 00:00:00' as datetime) as EndTime2, "
                                                   + "  cast ('01/01/2020 00:00:00' as datetime) as StartTime3, "
                                                   + "  cast ('01/01/2020 00:00:00' as datetime) as EndTime3, "
                                                   + "  getdate() as Entry_DateTime, "
                                                   + "  getdate() as Last_Sync_Date, "
                                                   + "  0 as is_deleted, "
                                                   + "  0 as Is_Adit_Updated "
                                                   + "  from [Hour] where [StartDate] in (SELECT min([StartDate]) [StartDate] "
                                                   + "   FROM [Tracker].[dbo].[Hour]) AND 1 = 2";


        #region PatienFormInsurance

        public static string InsertPatient_InsurancePlan = "INSERT INTO InsurancePlan "
                                                     + " (CarrierId, AnniversaryDate,AllowsAssignment, BasicPercentage, MajorPercentage, OrthoPercentage, PreventativePercentage, SingleBasicDeductible, SingleMajorDeductible, SingleOrthoDeductible,  "
                                                     + " SinglePreventativeDeductible, SingleBasicMaximum, SingleMajorMaximum, SingleOrthoMaximum, SinglePreventativeMaximum, FamilyBasicDeductible, FamilyMajorDeductible,  "
                                                     + " FamilyOrthoDeductible, FamilyPreventativeDeductible, FamilyBasicMaximum, FamilyMajorMaximum, FamilyOrthoMaximum, FamilyPreventativeMaximum, PlanSingleDeductible,  "
                                                     + " PlanFamilyDeductible, PlanComboMaximum, PlanMaximum,  Interval, RootPlaningInterval, ScalingInterval, AnniversaryYears, ModifiedTimeStamp, CreatedTimeStamp, ModifiedUserId,  "
                                                     + " CreatedUserId,ModifiedMachineName,CreatedMachineName) "
                                                     + " VALUES  "
                                                     + " (@CarrierId,'2020-01-01',1,100,100,100,100,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,12,12,1,getdate(),getdate(), "
                                                     + " @ModifiedUserId, @CreatedUserId,@ModifiedMachineName,@CreatedMachineName)";


        public static string InsertPatient_Subscriber = "INSERT INTO Subscriber "
                                                     + " (ContactId, InsuranceCode, InsurancePlanId, PayeeType) "
                                                     + " VALUES (@ContactId,@InsuranceCode,@InsurancePlanId,1) ";

        public static string InsertPatient_Coverage = " INSERT INTO Coverage "
                                                     + " (SubscriberId, ContactId, CoverageType, Relationship, SingleBasicAdjustmentTotal, SingleMajorAdjustmentTotal, SingleOrthoAdjustmentTotal, SinglePreventativeAdjustmentTotal,  "
                                                     + " SingleBasicDeductibleAdjustmentTotal, SingleMajorDeductibleAdjustmentTotal, SingleOrthoDeductibleAdjustmentTotal, SinglePreventativeDeductibleAdjustmentTotal, SingleAdjustmentDate,  "
                                                     + " PlanSingleDeductibleAdjustment , EffectiveDate, ScalingAdjustmentTotal, RootPlaningAdjustmentTotal, LastVerifiedDate) "
                                                     + " VALUES (@SubscriberId,@ContactId,@CoverageType,1,0,0,0,0,0,0,0,0,getdate(),0,getdate(),0,0,getdate())";

        #endregion

        public static string GetTrackerUserLoginId = "IF NOT EXISTS(select 1 from [User] where UserId = 'Adit') BEGIN insert into [User] (UserId,Username,isActive,CreatedUserId,CreatedTimeStamp,ModifiedUserId,ModifiedTimeStamp,ModifiedMachineName,CreatedMachineName) values ('Adit','Adit',1,'Admin',getdate(),'Admin',getdate(),'',''); select UserId from [User] where UserId = 'Adit'; END ELSE BEGIN select UserId from [User]" +
           " where UserId = 'Adit'; END"; // have to insert created and modified user as admin while creating adit as user

        public static string GetPrimaryPovider = "select providerid from contact where contact=@ContactId";

        public static string GetPatientName = "select FirstName+', '+LastName from Contact where ContactId=@PatientId";

        //public static string InsertPatientNotes = "  IF NOT EXISTS ( SELECT 1 FROM NOTE WHERE ContactId = @PatientEHRId AND NoteType = @NoteType AND NoteText = @PatientNote AND CreatedTimeStamp = @PaymentDate ) BEGIN  INSERT INTO NOTE (ProviderId, ContactId, NoteType, NoteText, NoteRTF, IsActive, AuthenticationCode, IsImportant, SerializedTeeth, IsFamilyNote, OriginalId, CreatedUserId, CreatedTimeStamp, ModifiedUserId, ModifiedTimeStamp, ModifiedMachineName, CreatedMachineName, AuthorizedTimeStamp, AuthorizedUserId, AuthorizedMachineName, Tags) VALUES "
        //                                        + " ( ( SELECT ProviderId FROM Contact where contactid = @PatientEHRId ),@PatientEHRId,@NoteType,@PatientNote,NULL,1,NULL,0,0,0,NULL,@CreatedUserId,@PaymentDate,@ModifiedUserId,getdate(),(SELECT HOST_NAME()),(SELECT HOST_NAME()),NULL,NULL,NULL,NULL)  Select @@identity END ELSE BEGIN ( SELECT top 1 NoteId FROM NOTE WHERE ContactId = @PatientEHRId AND NoteType = @NoteType AND NoteText = @PatientNote AND CreatedTimeStamp = @PaymentDate )  END ";

        public static string InsertPatientNotes = " IF NOT EXISTS ( SELECT 1 FROM Activity WHERE ContactId = @PatientEHRId AND Message = @PatientNote AND CreatedTimeStamp = @PaymentDate ) " +
                                                " BEGIN INSERT INTO Activity (Type, ContactId, Message, ModifiedUserId, ModifiedTimeStamp, CreatedUserId, CreatedTimeStamp, ModifiedMachineName,	CreatedMachineName, SerializedTeeth, IsActive,Comments) " +
                                                " VALUES (@NoteType,@PatientEHRId,@PatientNote,@ModifiedUserId,getdate(),@CreatedUserId,@PaymentDate,(SELECT HOST_NAME()),(SELECT HOST_NAME()),0,1,@Comments) " +
                                                " Select @@identity   " +                                                
                                                " END " +
                                                " ELSE BEGIN " +
                                                " (SELECT top 1 ActivityId FROM Activity WHERE ContactId = @PatientEHRId AND Message = @PatientNote AND CreatedTimeStamp = @PaymentDate) " +
                                                " END ";

        public static string CheckPaymentModeExistAsAditPay = "IF NOT EXISTS ( select 1 from Method where MethodDescription = 'Adit Pay' ) BEGIN INSERT INTO Method (MethodId,MethodDescription,IsDirectDeposit,IsActive,AppliesToContactPayment,AppliesToCarrierPayment,AppliesToContactRefund,AppliesToCarrierRefund,AppliesToAdjustment,CanBePostDated) values ((select MAX(MethodId)+1 as MethodId from Method),'Adit Pay',0,1,1,0,1,0,0,0); select MethodId from Method where MethodDescription = 'Adit Pay'  END ELSE BEGIN  select MethodId from Method where MethodDescription = 'Adit Pay' END ";

        public static string CheckPaymentModeExistAsCareCredit = "IF NOT EXISTS ( select 1 from Method where MethodDescription = 'CareCredit' ) BEGIN INSERT INTO Method (MethodId,MethodDescription,IsDirectDeposit,IsActive,AppliesToContactPayment,AppliesToCarrierPayment,AppliesToContactRefund,AppliesToCarrierRefund,AppliesToAdjustment,CanBePostDated) values ((select MAX(MethodId)+1 as MethodId from Method),'CareCredit',0,1,1,0,1,0,0,0); select MethodId from Method where MethodDescription = 'CareCredit'  END ELSE BEGIN  select MethodId from Method where MethodDescription = 'CareCredit' END ";

        public static string CheckPaymentModeExistAsAditPayDiscount = "IF NOT EXISTS ( select 1 from Method where MethodDescription = 'Adit Pay Discount' ) BEGIN INSERT INTO Method (MethodId,MethodDescription,IsDirectDeposit,IsActive,AppliesToContactPayment,AppliesToCarrierPayment,AppliesToContactRefund,AppliesToCarrierRefund,AppliesToAdjustment,CanBePostDated) values ((select MAX(MethodId)+1 as MethodId from Method),'Adit Pay Discount',0,1,0,0,0,0,1,0); select MethodId from Method where MethodDescription = 'Adit Pay Discount'  END ELSE BEGIN  select MethodId from Method where MethodDescription = 'Adit Pay Discount' END ";

        public static string CheckPaymentModeExistAsCareCreditDiscount = "IF NOT EXISTS ( select 1 from Method where MethodDescription = 'CareCredit Discount' ) BEGIN INSERT INTO Method (MethodId,MethodDescription,IsDirectDeposit,IsActive,AppliesToContactPayment,AppliesToCarrierPayment,AppliesToContactRefund,AppliesToCarrierRefund,AppliesToAdjustment,CanBePostDated) values ((select MAX(MethodId)+1 as MethodId from Method),'CareCredit Discount',0,1,0,0,0,0,1,0); select MethodId from Method where MethodDescription = 'CareCredit Discount'  END ELSE BEGIN  select MethodId from Method where MethodDescription = 'CareCredit Discount' END ";

        //public static string InsertPaymentToFinancialTransaction = " INSERT INTO FinancialTransaction(ContactId, ContractId, CarrierId, IsDirectDeposit, BankId, TransactionDate, TransactionType, TransactionAmount, AllocatedAmount, AuthorizationCode, MethodId, IsDeleted, ReversalReason, IsPostdated, DepositedDate, ChequeNumber, Notes, AttachmentFileName, ModifiedUserId, ModifiedTimeStamp, CreatedUserId, CreatedTimeStamp, ModifiedMachineName, CreatedMachineName, NsfDate)"
        //                                                           + "VALUES (@PatientEHRId,null,null,0,1,@PaymentDate,2,@Amount,0.00,null,@MethodId,0,0,0,null,@ChequeNumber,@PaymentNote,null,'Admin',GETDATE(),'Admin',GETDATE(),(select top 1 ModifiedMachineName from TransactionDetail),(select top 1 ModifiedMachineName from TransactionDetail),null)SELECT @@IDENTITY";
        //public static string InsertPaymentToFinancialTransaction = "IF NOT EXISTS ( select 1 from FinancialTransaction where ContactId=@PatientEHRId and TransactionDate=@PaymentDate and TransactionAmount=@Amount and Notes=@PaymentNote and MethodId=@MethodId ) BEGIN INSERT INTO FinancialTransaction(ContactId, ContractId, CarrierId, IsDirectDeposit, BankId, TransactionDate, TransactionType, TransactionAmount, AllocatedAmount, AuthorizationCode, MethodId, IsDeleted, ReversalReason, IsPostdated, DepositedDate, ChequeNumber, Notes, AttachmentFileName, ModifiedUserId, ModifiedTimeStamp, CreatedUserId, CreatedTimeStamp, ModifiedMachineName, CreatedMachineName, NsfDate) VALUES ((select clientid from Contact where ContactId=@PatientEHRId),null,null,0,@BankId,@PaymentDate,@TransactionType,@Amount,@Amount,null,@MethodId,0,0,0,null,@ChequeNumber,@PaymentNote,null,'Admin',GETDATE(),'Admin',GETDATE(),(select top 1 ModifiedMachineName from FinancialTransaction),(select top 1 ModifiedMachineName from FinancialTransaction),null) SELECT @@IDENTITY END";

        public static string InsertPaymentToFinancialTransaction = "IF NOT EXISTS ( select 1 from FinancialTransaction where ContactId=@PatientEHRId and TransactionDate=@PaymentDate and TransactionAmount=@Amount and Notes=@PaymentNote and MethodId=@MethodId ) BEGIN INSERT INTO FinancialTransaction(ContactId, ContractId, CarrierId, IsDirectDeposit, BankId, TransactionDate, TransactionType, TransactionAmount, AllocatedAmount, AuthorizationCode, MethodId, IsDeleted, ReversalReason, IsPostdated, DepositedDate, ChequeNumber, Notes, AttachmentFileName, ModifiedUserId, ModifiedTimeStamp, CreatedUserId, CreatedTimeStamp, ModifiedMachineName, CreatedMachineName, NsfDate) VALUES ((select clientid from Contact where ContactId=@PatientEHRId),null,null,0,@BankId,@PaymentDate,@TransactionType,@Amount,@Amount,null,@MethodId,0,0,0,null,@ChequeNumber,@PaymentNote,null,@ModifiedUserId, GETDATE(), @CreatedUserId,GETDATE(),(select top 1 ModifiedMachineName from FinancialTransaction),(select top 1 ModifiedMachineName from FinancialTransaction),null) SELECT @@IDENTITY END";

        public static string InsertPaymentToTransactionDetail = "INSERT INTO TransactionDetail(ContactId, CarrierId, InvoiceEntryId, CoverageId, FinancialTransactionId, ProviderId, PracticeId, OriginalId, CoverageOrder, ReversedId, IsHistory, IsReversal, TransactionType, Description, TransactionAmount, PaidAmount, TransactionDate, AgeingDate, ModifiedUserId, ModifiedTimeStamp, CreatedUserId, CreatedTimeStamp, ModifiedMachineName, CreatedMachineName, ResponsibleId, IsNsf, ContractId)"
                                                                  + "VALUES ((select clientid from Contact where ContactId=@PatientEHRId),null,null,null,@FinancialTransactionId,@ProviderEHRId,(select top 1 PracticeId from TransactionDetail),null,null,null,0,0,@TransactionType,@PaymentNote,@Amount,0.00,GETDATE(),GETDATE(),@ModifiedUserId,GETDATE(),@CreatedUserId,GETDATE(),(select top 1 ModifiedMachineName from TransactionDetail),(select top 1 ModifiedMachineName from TransactionDetail),1,0,null)";

        public static string InsertPaymentToTransactionDetailForRefund = "IF NOT EXISTS ( Select 1 FROM TransactionDetail WHERE ContactId = @PatientEHRId AND ProviderId = @ProviderEHRId AND TransactionDate = @PaymentDate AND Description = @PaymentNote AND TransactionAmount = @Amount ) BEGIN " +
            "INSERT INTO TransactionDetail(ContactId, CarrierId, InvoiceEntryId, CoverageId, FinancialTransactionId, ProviderId, PracticeId, OriginalId, CoverageOrder, ReversedId, IsHistory, IsReversal, TransactionType, Description, TransactionAmount, PaidAmount, TransactionDate, AgeingDate, ModifiedUserId, ModifiedTimeStamp, CreatedUserId, CreatedTimeStamp, ModifiedMachineName, CreatedMachineName, ResponsibleId, IsNsf, ContractId)"
                                                               + "VALUES ((select clientid from Contact where ContactId=@PatientEHRId),null,null,null,null,@ProviderEHRId,(select top 1 PracticeId from TransactionDetail),null,99,null,0,0,6,@PaymentNote,@Amount,0.00,@PaymentDate,@PaymentDate,@ModifiedUserId,GETDATE(),@CreatedUserId,GETDATE(),(select top 1 ModifiedMachineName from TransactionDetail),(select top 1 ModifiedMachineName from TransactionDetail),1,0,null) END";

        //Done by Rishi, discussed with Shruti, wrong bank id for ContactID = 1
        public static string GetBankIdFromFinancialTransaction = "IF EXISTS ( select 1 From FinancialTransaction where ContactId = @PatientEHRId )"
                                                                 + " BEGIN  Select top 1  BankId From FinancialTransaction where ContactId = @PatientEHRId order by FinancialTransactionId desc  END "
                                                                 + " ELSE BEGIN SELECT  top 1 BankId FROM  FinancialTransaction GROUP BY BankId ORDER BY COUNT(1) DESC END ";

        public static string GetPatientBalance = "Select CASE WHEN SUM(TransactionAmount) IS not NULL THEN SUM(TransactionAmount)  else 0 END as balance  from TransactionDetail where ContactId =(select clientid from Contact where ContactId=@PatientEHRId)";

        public static string InsertDiscount = "Declare @SIndex int = 1 "
 + " Declare @EIndex int = 0 "
 + " declare @AdjustDiscountAmount Decimal(18,4) = 0 "
 + "CREATE Table #AdjustDiscount (Id int NOT NULL identity(1,1),TransactionDetailsId numeric(18,0),Amount decimal (18,4)) "
 + "IF(@DiscountAmount <=  (Select SUM(TransactionAmount) from TransactionDetail where ContactId = (select clientid from Contact where ContactId=@PatientEHRId))) "
 + " BEGIN "
     + "  TRUNCATE TABle #AdjustDiscount "

       + " INSERT INTO #AdjustDiscount SELECT TransactionDetailId,TransactionAmount FROM TransactionDetail WHERE TransactionAmount > 0 AND ContactId = (select clientid from Contact where ContactId=@PatientEHRId) Order by CreatedTimeStamp DESC "

       + " SET @EIndex = (Select COUNT(1) FROM #AdjustDiscount) "

       + " WHILE(@SIndex <= @EIndex AND @DiscountAmount > 0 ) "

       + " BEGIN "
           + "  IF((SELECT Amount FROM #AdjustDiscount WHERE Id = @SIndex ) < @DiscountAmount ) "

             + " BEGIN "
               + " SET @AdjustDiscountAmount = (SELECT Amount FROM #AdjustDiscount WHERE Id = @SIndex ) "

               + " SET @DiscountAmount = @DiscountAmount - @AdjustDiscountAmount "

             + " END "
             + " ELSE "

             + " BEGIN "
               + " SEt @AdjustDiscountAmount = @DiscountAmount "

               + " SET @DiscountAmount = 0 "

             + " END"

             + " INSERT INTO TransactionDetail(ContactId, CarrierId, InvoiceEntryId, CoverageId, FinancialTransactionId, ProviderId, PracticeId, OriginalId, CoverageOrder, ReversedId, IsHistory, IsReversal, TransactionType, Description, TransactionAmount, PaidAmount, TransactionDate, AgeingDate, ModifiedUserId, ModifiedTimeStamp, CreatedUserId, CreatedTimeStamp, ModifiedMachineName, CreatedMachineName, ResponsibleId, IsNsf, ContractId) "

                 + "  VALUES ((select clientid from Contact where ContactId=@PatientEHRId),null,null,null, @FinancialTransactionId, @ProviderEHRId, (select top 1 PracticeId from TransactionDetail),"
                      + "  (SELECT TransactionDetailsId FROM #AdjustDiscount WHERE Id = @SIndex ),null,null,0,0,@TransactionType,@PaymentNote,-@AdjustDiscountAmount,0.00,GETDATE(),GETDATE(),@ModifiedUserId,GETDATE(),@CreatedUserId,GETDATE(),(select top 1 ModifiedMachineName from TransactionDetail),(select top 1 ModifiedMachineName from TransactionDetail),1,0,null) "

             + " set @SIndex = @SIndex + 1 "

       + " END "
 + " END "
 + " ELSE "
 + " BEGIN "
            + " Set @AdjustDiscountAmount = @DiscountAmount "

             + " INSERT INTO TransactionDetail(ContactId, CarrierId, InvoiceEntryId, CoverageId, FinancialTransactionId, ProviderId, PracticeId, OriginalId, CoverageOrder, ReversedId, IsHistory, IsReversal, TransactionType, Description, TransactionAmount, PaidAmount, TransactionDate, AgeingDate, ModifiedUserId, ModifiedTimeStamp, CreatedUserId, CreatedTimeStamp, ModifiedMachineName, CreatedMachineName, ResponsibleId, IsNsf, ContractId) "

                 + "  VALUES ((select clientid from Contact where ContactId=@PatientEHRId),null,null,null, @FinancialTransactionId, @ProviderEHRId, (select top 1 PracticeId from TransactionDetail),"
                      + " null,null,null,0,0,@TransactionType,@PaymentNote,-@AdjustDiscountAmount,0.00,GETDATE(),GETDATE(),@ModifiedUserId,GETDATE(),@CreatedUserId,GETDATE(),(select top 1 ModifiedMachineName from TransactionDetail),(select top 1 ModifiedMachineName from TransactionDetail),1,0,null) "

 //    + "  RAISError('Discount is greater than the pending balance',16,1) "
 + " END "
 + " DROP TABLE #AdjustDiscount ";


        public static string InsertPatientMedication = "IF NOT EXISTS(SELECT 1 FROM NOTE WHERE ContactId = @PatientEHRId AND NoteType = @NoteType AND NoteText = @PatientNote) BEGIN INSERT INTO NOTE(ProviderId, ContactId, NoteType, NoteText, NoteRTF, IsActive, AuthenticationCode, IsImportant, SerializedTeeth, IsFamilyNote, OriginalId, CreatedUserId, CreatedTimeStamp, ModifiedUserId, ModifiedTimeStamp, ModifiedMachineName, CreatedMachineName, AuthorizedTimeStamp, AuthorizedUserId, AuthorizedMachineName, Tags) VALUES " +
                               " ((SELECT ProviderId FROM Contact where contactid = @PatientEHRId),@PatientEHRId,@NoteType,@PatientNote,@PatientNoteRTF,1,NULL,1,0,0,NULL,@CreatedUserId,getdate(),@ModifiedUserId,getdate(),(SELECT HOST_NAME()),(SELECT HOST_NAME()),NULL,NULL,NULL,NULL)  Select @@identity END ELSE BEGIN(SELECT top 1 NoteId FROM NOTE WHERE ContactId = @PatientEHRId AND NoteType = @NoteType AND NoteText = @PatientNote)  END;";

        public static string UpdatePatientMedicationNotes = "IF NOT EXISTS(SELECT 1 FROM NOTE WHERE NoteID = @PatientMedication_EHR_ID AND NoteType = @NoteType AND NoteText = @PatientNote) BEGIN Update NOTE Set NoteText = @PatientNote where NoteID = @PatientMedication_EHR_ID END;";

        public static string GetTrackerPatientMedicationData = "Select 0 As Clinic_Number, 1 as Service_Install_Id, ContactID AS Patient_EHR_ID, 0 as Medication_EHR_ID , "
                                                             + " NoteID as PatientMedication_EHR_ID, '' AS Medication_Web_ID, ProviderID AS Provider_EHR_ID, '' AS Medication_Name,"
                                                             + " 'M' AS Medication_Type,0 AS Drug_Quantity, '' AS Medication_Note, NoteText As Patient_Notes, "
                                                             + " CreatedTimeStamp AS EHR_Entry_DateTime, CreatedTimeStamp AS Start_Date, GETDATE() AS End_Date, GETDATE() AS Last_Sync_Date,"
                                                             + " 0 AS Is_deleted,0 AS Is_Adit_Updated,Case When IsActive = 1 Then 'True' Else 'False' End As Is_Active From Note where NoteType = 8 ";

        public static string DeletePatientMedication = "IF EXISTS(SELECT 1 FROM NOTE WHERE NoteID = @PatientMedication_EHR_ID AND NoteType = @NoteType)"
                                                     + " BEGIN DELETE FROM NOTE WHERE NoteID = @PatientMedication_EHR_ID and NoteType = @NoteType END";

        //rooja
        public static string InsertPatientFormDocData = "INSERT INTO Activity(Type,ContactId,Message,AttachmentFilePath,ModifiedUserId,ModifiedTimeStamp,CreatedUserId,CreatedTimeStamp,ModifiedMachineName,CreatedMachineName,SerializedTeeth,IsActive,FileType) VALUES(@Type,@ContactId,@Message,@AttachmentFilePath,@ModifiedUserId,@ModifiedTimeStamp,@CreatedUserId,@CreatedTimeStamp,@ModifiedMachineName,@CreatedMachineName,@SerializedTeeth,@IsActive,@FileType)";

        //Sandeep Sharma
        public static string NoteDataMoveToCorrespondInTracker = " insert into Activity (Type, ContactId, Message, ModifiedUserId, ModifiedTimeStamp, CreatedUserId, CreatedTimeStamp, ModifiedMachineName, CreatedMachineName, SerializedTeeth, IsActive) " +
                                                                " Select " +
                                                                " case when NoteType=1 then 'Medical' else " +
                                                                " 	case when NoteType=4 then 'Financial' else " +
                                                                " 		case when NoteType=5 then 'Clinical' else " +
                                                                " 			case when NoteType= 6 then 'Schedule' else " +
                                                                " 				case when NoteType=7 then 'Ortho' else  " +
                                                                " 					case when NoteType=8 then 'Pre Med' else  " +
                                                                " 						case when NoteType=9 then  'Social 'else " +
                                                                " 							case when NoteType=10 then  'Prescription 'else " +
                                                                " 								'Other' " +
                                                                " 							end " +
                                                                " 						end " +
                                                                " 					end " +
                                                                " 				end " +
                                                                " 			end " +
                                                                " 		end " +
                                                                " 	end " +
                                                                " end " +
                                                                " as NoteType, " +
                                                                " ContactId,NoteText,ModifiedUserId, ModifiedTimeStamp,CreatedUserId, CreatedTimeStamp, ModifiedMachineName, CreatedMachineName, SerializedTeeth,1 from NOTE where NoteId = @NoteId ; " +
                                                                " Select @@identity ;" +                                                              
                                                                " delete from NOTE where NoteId = @NoteId ;";
        //rooja 
        public static string GetTrackerInsuranceData = "SELECT *,'' as Clinic_Number from carrier; ";

        //rooja get patient doc location
        public static string GetTrackerPatientDocLocation = " select ApplicationOptionId,MachineName,UserId,GroupName,ApplicationOptionName,ApplicationOptionValue from applicationoption where ApplicationOptionName='PatientDocumentsFolder';";

    }
}
