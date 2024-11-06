using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Pozative.QRY
{
    public class SynchOpenDentalQRY
    {

        public static string GetOpenDentalAppointmentData = " SELECT a.AptNum AS Appt_EHR_ID,pat.PatNum AS PatNum,pat.LName AS Last_Name,pat.FName AS First_Name,pat.MiddleI AS MI, "
                                                          + " pat.HmPhone AS Home_Contact, pat.WirelessPhone AS Mobile_Contact,pat.Email AS Email,pat.Address AS Address, "
                                                          + " pat.City AS City,pat.State AS ST,pat.Zip AS Zip,case when a.OP is null then 0 else a.OP end AS Operatory_EHR_ID,o.OpName AS Operatory_Name, "
                                                          + " pro.ProvNum AS Provider_EHR_ID, CONCAT(pro.LName, ' ' , pro.FName) AS Provider_Name , "
                                                          + " a.AppointmentTypeNum AS ApptType_EHR_ID, aptype.AppointmentTypeName AS ApptType , "
                                                          + " a.AptDateTime AS Appt_DateTime, CHAR_LENGTH(a.Pattern)*5 AS ApptMin , "
                                                          + " a.Note AS comment, pat.Birthdate AS birth_date, "
                                                          + " a.AptStatus AS appointment_status_ehr_key, a.AptStatus AS Appointment_Status, "
                                                          + " a.DateTStamp AS EHR_Entry_DateTime, "
                                                          + " a.Confirmed AS confirmed_status_ehr_key, "
                                                          + " (SELECT ItemName FROM definition WHERE DefNum = a.Confirmed) AS confirmed_status, "
                                                          + " a.UnschedStatus AS unschedule_status_ehr_key, "
                                                          + " (SELECT ItemName FROM definition WHERE DefNum = a.UnschedStatus) AS Unsched_Status, a.priority AS is_asap, "
                                                          + " IFNull(a.clinicnum,o.clinicnum) as Clinic_Number,pcod.ProcedureCode,pcod.ProcedureDesc "
                                                          + " FROM appointment a "
                                                          + " INNER JOIN patient pat ON pat.PatNum = a.PatNum "
                                                          + " INNER JOIN operatory o ON a.OP = o.OperatoryNum  "
                                                          + " INNER JOIN provider pro ON pro.ProvNum = a.ProvNum "
                                                          + " LEFT JOIN appointmenttype aptype ON aptype.AppointmentTypeNum = a.AppointmentTypeNum "
                                                          + " LEFT JOIN(select b.aptnum, group_concat(procedurecodes separator ',') as ProcedureCode, group_concat(procdesc separator ',') as ProcedureDesc"
                                                          + " from(select a.AptNum, pc.ProcCode  as procedurecodes,"
                                                          + " (CASE WHEN toothnum = '' and surf = ''  THEN AbbrDesc WHEN (toothnum!='' and surf = '') then concat(toothnum,'-', AbbrDesc) "
                                                          + " WHEN(toothnum= '' and surf!='') then concat(surf,'-', AbbrDesc) "
                                                          + " ELSE concat(toothnum,'-', surf,'-', AbbrDesc) end) as procdesc "
                                                          + " from appointment a "
                                                          + " left join procedurelog pl  on a.aptnum= pl.aptnum "
                                                          + " inner join procedurecode pc on pl.CodeNum= pc.CodeNum "
                                                          + " WHERE a.AptDateTime >  @ToDate)as b "
                                                          + " group by b.aptnum) AS pcod ON pcod.aptnum = a.AptNum "
                                                          + " WHERE  a.AptDateTime > @ToDate "; // AND a.OP is not null
                                                                                                  //+ " WHERE (a.DateTStamp > @ToDate OR a.AptDateTime > @ToDate) ; "; // AND a.OP is not null
        public static string GetOpenDentalAppointmentEhrIDs = " SELECT a.AptNum AS Appt_EHR_ID"
                                                          + " FROM appointment a "
                                                          + " JOIN patient pat ON pat.PatNum = a.PatNum "
                                                          + " LEFT JOIN operatory o ON a.OP = o.OperatoryNum  "
                                                          + " Left JOIN appointmenttype aptype ON aptype.AppointmentTypeNum = a.AppointmentTypeNum "
                                                          + " JOIN provider pro ON pro.ProvNum = a.ProvNum "
                                                          + " WHERE  a.AptDateTime > @ToDate ; ";

        public static string OpendentalAppointment_Procedures_Data = " select b.aptnum,group_concat(procedurecodes separator ',') as ProcedureCode, group_concat(procdesc separator ',') as ProcedureDesc "
                                                            + " from(select a.AptNum,pc.ProcCode  as procedurecodes, "
                                                            + " (CASE WHEN toothnum='' and surf=''  THEN AbbrDesc WHEN (toothnum!='' and surf='') then concat(toothnum,'-',AbbrDesc) "
                                                            + " WHEN (toothnum='' and surf!='') then concat(surf,'-',AbbrDesc) "
                                                            + " ELSE concat(toothnum,'-',surf,'-',AbbrDesc) end) as procdesc "
                                                            + " from appointment a "
                                                            + "  left join procedurelog pl  on a.aptnum= pl.aptnum "
                                                            + " inner join procedurecode pc  on pl.CodeNum= pc.CodeNum WHERE  a.AptDateTime > @ToDate)as b "
                                                            + " group by b.aptnum";

        public static string GetOpenDentalAppointmentData_15_4 = " SELECT a.AptNum AS Appt_EHR_ID,pat.PatNum AS PatNum,pat.LName AS Last_Name,pat.FName AS First_Name,pat.MiddleI AS MI, "
                                                          + " pat.HmPhone AS Home_Contact, pat.WirelessPhone AS Mobile_Contact,pat.Email AS Email,pat.Address AS Address, "
                                                          + " pat.City AS City,pat.State AS ST,pat.Zip AS Zip,case when a.OP is null then 0 else a.OP end AS Operatory_EHR_ID,o.OpName AS Operatory_Name, "
                                                          + " pro.ProvNum AS Provider_EHR_ID, CONCAT(pro.LName, ' ' , pro.FName) AS Provider_Name , "
                                                          + " a.AppointmentTypeNum AS ApptType_EHR_ID, aptype.AppointmentTypeName AS ApptType , "
                                                          + " a.AptDateTime AS Appt_DateTime, CHAR_LENGTH(a.Pattern)*5 AS ApptMin , "
                                                          + " a.Note AS comment, pat.Birthdate AS birth_date, "
                                                          + " a.AptStatus AS appointment_status_ehr_key, a.AptStatus AS Appointment_Status, "
                                                          + " a.DateTStamp AS EHR_Entry_DateTime, "
                                                          + " a.Confirmed AS confirmed_status_ehr_key, "
                                                          + " (SELECT ItemName FROM definition WHERE DefNum = a.Confirmed) AS confirmed_status, "
                                                          + " a.UnschedStatus AS unschedule_status_ehr_key, "
                                                          + " (SELECT ItemName FROM definition WHERE DefNum = a.UnschedStatus) AS Unsched_Status, 0 AS is_asap, "
                                                          + " IFNull(a.clinicnum,o.clinicnum) as Clinic_Number "
                                                          + " FROM appointment a "
                                                          + " JOIN patient pat ON pat.PatNum = a.PatNum "
                                                          + " LEFT JOIN operatory o ON a.OP = o.OperatoryNum  "
                                                          + " Left JOIN appointmenttype aptype ON aptype.AppointmentTypeNum = a.AppointmentTypeNum "
                                                          + " JOIN provider pro ON pro.ProvNum = a.ProvNum "
                                                          + " WHERE  a.AptDateTime > @ToDate "; // AND a.OP is not null
        //+ " WHERE (a.DateTStamp > @ToDate OR a.AptDateTime > @ToDate) ; "; // AND a.OP is not null
        public static string GetOpenDentalAppointmentEhrIDs_15_4 = " SELECT a.AptNum AS Appt_EHR_ID"
                                                          + " FROM appointment a "
                                                          + " JOIN patient pat ON pat.PatNum = a.PatNum "
                                                          + " LEFT JOIN operatory o ON a.OP = o.OperatoryNum  "
                                                          + " Left JOIN appointmenttype aptype ON aptype.AppointmentTypeNum = a.AppointmentTypeNum "
                                                          + " JOIN provider pro ON pro.ProvNum = a.ProvNum "
                                                          + " WHERE  a.AptDateTime > @ToDate ; ";

        public static string GetOpenDentalApptPatientdue_date = "SELECT r.recallNum, rt.RecallTypeNum, r.DateDue AS due_date, r.patnum AS patient_id, rt.description AS recall_type,p.clinicnum As Clinic_Number  FROM recall r  Left Join patient p on p.PatNum = r.PatNum  JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum inner join appointment ap on ap.patnum = p.patnum where ap.aptdatetime > @ToDate and p.clinicnum = @clinicnum;";

        public static string GetOpenDentalPatientStatusNew_Existing = "select patnum as Patient_EHR_Id from patient where PatNum <> ''  AND LName <> '' and (Not ((FName = '' OR LName = '') and patstatus = 4))  and patnum not in (select p.patnum as Patient_EHR_Id from patient p INNER join appointment a on a.patnum = p.patnum where  a.aptdatetime < curdate() and a.AptStatus = 2) "; //and (DateFirstVisit = '' OR DateFirstVisit = '0001-01-01' OR DateFirstVisit < curdate())

        public static string GetOpenDentalPatientStatusNew_Existing_Clinic = "select patnum as Patient_EHR_Id from patient where PatNum <> ''  AND LName <> '' and (Not ((FName = '' OR LName = '') and patstatus = 4)) and patnum not in (select distinct patnum as Patient_EHR_Id from appointment a where  a.aptdatetime < curdate() and a.AptStatus = 2) and (clinicnum = @Clinic_Number) "; // and (DateFirstVisit = '' OR DateFirstVisit = '0001-01-01' OR DateFirstVisit < curdate())

        public static string GetOpenDentalDeletedAppointmentData = " SELECT ObjectNum As Appt_EHR_ID, DateTStamp AS EHR_Entry_DateTime "
                                                          + " FROM deletedobject WHERE ObjectType = 0 AND DateTStamp > @ToDate ; ";

        public static string GetOpenDentalDeletedAppointmentData_Clinic_Number = " SELECT d.ObjectNum As Appt_EHR_ID, d.DateTStamp AS EHR_Entry_DateTime ,IFNull(a.clinicnum,o.clinicnum) as Clinic_Number "
                                                          + " FROM deletedobject d "
                                                          + "  LEFT JOIN appointment a on a.AptNum = d.ObjectNum  LEFT JOIN operatory o ON a.OP = o.OperatoryNum  "
                                                          + " WHERE d.ObjectType = 0 AND d.DateTStamp > @ToDate union "
                                                          + "   select aptnum as Appt_EHR_ID,datetstamp as EHR_Entry_DateTime, clinicnum as Clinic_Number "
                                                          + "   from appointment where aptstatus = 3  and op = 0 and AptDateTime >= @ToDate ; ";

        public static string GetOpenDentalDeletedAppointmentData17_2Plus = " SELECT h.AptNum AS Appt_EHR_ID,IFNull(h.clinicnum,o.clinicnum) as Clinic_Number FROM histappointment h LEFT JOIN operatory o ON h.OP = o.OperatoryNum  where HistApptAction = 4 AND HistDateTStamp > @ToDate "
                                                          + " union "
                                                          + "   select DISTINCT aptnum as Appt_EHR_ID, clinicnum as Clinic_Number "
                                                          + "   from appointment where aptstatus = 3 and op = 0 and AptDateTime >= @ToDate ; ";

        //public static string GetOpenDentalPatientInsuranceData = "SELECT distinct p.PatNum as Patient_EHR_ID,ip.carrierNum as Primary_Insurance,c.CarrierName as Primary_Insurance_CompanyName ,c.Phone as Prim_Ins_Company_Phonenumber,i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid "
        //                                                  + " FROM patplan p join inssub i on "
        //                                                  + " p.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum "
        //                                                  + " join patient pat on pat.PatNum = p.PatNum "
        //                                                  + " join carrier c on c.carrierNum = ip.carrierNum where p.PatNum = @PatientId ; ";

        public static string GetOpenDentalPatientInsuranceData = "SELECT distinct p.PatNum as Patient_EHR_ID,ip.carrierNum as Primary_Insurance,c.CarrierName as Primary_Insurance_CompanyName ,c.Phone as Prim_Ins_Company_Phonenumber,i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid "
                                                         + " FROM patplan p join inssub i on "
                                                         + " p.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum "
                                                         + " join patient pat on pat.PatNum = p.PatNum "
                                                         + " join carrier c on c.carrierNum = ip.carrierNum where pat.clinicnum = @Clinic_number; ";

        //rooja 
        public static string GetOpenDentalInsuranceData = "SELECT CarrierNum,CarrierName,Address,Address2,City,State,Zip,Phone,ElectID,NoSendElect,SecUserNumEntry,SecDateEntry,IsHidden,TIN,CarrierGroupName,'' as Clinic_Number from carrier; ";

        public static string GetOpenDentalOperatoryEventData = " SELECT IFNULL(OP.ScheduleOpNum,S.ScheduleNum) AS ScheduleNum ,S.SchedDate,S.StartTime,S.StopTime,S.Note,S.DateTStamp, IFNULL(OP.OperatoryNum,0) AS OperatoryNum, IFNULL(o.clinicnum,0) as Clinic_Number,case when itemvalue = 'NS' then 'False' else 'True' end as Allow_Book_Appt "
                                                          + " FROM schedule S JOIN scheduleop OP ON S.ScheduleNum = OP.ScheduleNum left join operatory o on o.OperatoryNum = OP.OperatoryNum left join Definition d on d.defnum=s.blockouttype "
                                                          + " WHERE  S.SchedDate > @ToDate and S.SchedType = 2 ; ";

        public static string GetOpenDentalOperatoryAllEventData = " SELECT IFNULL(OP.ScheduleOpNum,S.ScheduleNum) AS ScheduleNum ,S.SchedDate,S.StartTime,S.StopTime,S.Note,S.DateTStamp, IFNULL(OP.OperatoryNum,0) AS OperatoryNum,IFNULL(o.clinicnum,0) as Clinic_Number "
                                                          + " FROM schedule S JOIN scheduleop OP ON S.ScheduleNum = OP.ScheduleNum left join operatory o on o.OperatoryNum = OP.OperatoryNum "
                                                          + " WHERE  S.SchedDate > @ToDate and S.SchedType = 2;";

        //public static string GetOpenDentalHoliday = " Select ScheduleNum, SchedDate, StartTime, StopTime,SchedType,ProvNum,BlockoutType,Note,Status,DateTStamp,clinicnum  as Clinic_Number from schedule "
        //                                          + " WHERE SchedType = 0 AND DateTStamp > @ToDate ; ";

        public static string GetOpenDentalHoliday = " Select  CAST(ScheduleNum AS CHAR) as ScheduleNum, SchedDate, StartTime, StopTime,SchedType,ProvNum,BlockoutType,Note,Status,DateTStamp,clinicnum as Clinic_Number from schedule "
                                                  + " WHERE SchedType = 0 AND DateTStamp > @ToDate ; ";

        public static string GetOpenDentalHoliday_15_4 = " Select CAST(SD.ScheduleNum AS CHAR) as ScheduleNum, SD.SchedDate, SD.StartTime, SD.StopTime,SD.SchedType,SD.ProvNum,SD.BlockoutType,SD.Note,SD.Status,SD.DateTStamp,IfNull(o.ClinicNum,0) as Clinic_Number "
                                                       + " FROM schedule sd left JOIN scheduleop sdo ON sdo.ScheduleNum = sd.ScheduleNum LEFT JOIN operatory o ON o.OperatoryNum = sdo.OperatoryNum "
                                                       + " WHERE sd.SchedType = 0 AND sd.scheddate > @ToDate ; ";

        //public static string GetOpenDentalProviderData = " SELECT p.provnum AS Provider_EHR_ID,p.lname AS Last_Name,p.fname AS First_Name, p.mi, "
        //                                               + " d.itemname AS provider_speciality, (case when p.IsHidden = 0 then 1 else 0 end) as is_active, IfNULL(PCL.ClinicNum,0) as Clinic_Number "
        //                                               + " FROM provider p JOIN definition d ON d.DefNum = p.Specialty LEFT JOIN providercliniclink PCL ON PCL.ProvNum = p.provnum ; ";

        public static string GetOpenDentalProviderData = " SELECT p.provnum AS Provider_EHR_ID,p.lname AS Last_Name,p.fname AS First_Name, p.mi, "
                                                       + " d.itemname AS provider_speciality, (case when p.IsHidden = 0 then 1 else 0 end) as is_active, IfNULL(PCL.ClinicNum,0) as Clinic_Number "
                                                       + " FROM provider p Left JOIN definition d ON d.DefNum = p.Specialty LEFT JOIN providerclinic PCL ON PCL.ProvNum = p.provnum ; ";

        public static string GetOpenDentalProviderData_link = " SELECT p.provnum AS Provider_EHR_ID, p.lname AS Last_Name, p.fname AS First_Name, p.mi, "
                                                  + " d.itemname AS provider_speciality, (case when p.IsHidden = 0 then 1 else 0 end) as is_active, IfNULL(PCL.ClinicNum,0) as Clinic_Number "
                                                  + " FROM provider p Left JOIN definition d ON d.DefNum = p.Specialty LEFT JOIN providercliniclink PCL ON PCL.ProvNum = p.provnum ; ";

        public static string GetOpenDentalProviderData_15_4 = " SELECT p.provnum AS Provider_EHR_ID, p.lname AS Last_Name, p.fname AS First_Name, p.mi, "
                                                            + " d.itemname AS provider_speciality, (case when p.IsHidden = 0 then 1 else 0 end) as is_active, 0 as Clinic_Number "
                                                            + " FROM provider p Left JOIN definition d ON d.DefNum = p.Specialty ; ";

        //public static string GetOpenDentalProviderHoursData = " SELECT sd.scheduleNum,IFNULL(sdo.ScheduleOpNum,0), "
        //                                        + " (CASE WHEN IFNULL(sdo.ScheduleOpNum,0) = 0 THEN sd.scheduleNum ELSE CONCAT(CONCAT(sd.scheduleNum,'_',sdo.ScheduleOpNum),'_',IfNULL(PCL.ClinicNum,IfNull(o.ClinicNum,sd.ClinicNum))) END) AS PH_EHR_ID, "
        //                                        + " sd.provNum AS Provider_EHR_ID,sd.scheddate,CONCAT(sd.scheddate,' ',sd.StartTime) AS StartTime ,CONCAT(sd.scheddate,' ',sd.StopTime)AS EndTime, "
        //                                        + " sdo.OperatoryNum AS Operatory_EHR_ID,sd.note AS comment,sd.status, sd.DateTStamp AS Entry_DateTime, "
        //                                        + " IfNULL(PCL.ClinicNum,IfNull(o.ClinicNum,sd.ClinicNum))  as Clinic_Number "
        //                                        + " FROM schedule sd left JOIN scheduleop sdo ON sdo.ScheduleNum = sd.ScheduleNum LEFT JOIN Providercliniclink PCL ON PCL.ProvNum = sd.provNum  LEFT JOIN operatory o ON o.OperatoryNum = sdo.OperatoryNum "
        //                                        + " WHERE sd.SchedType = 1 AND sd.scheddate > @ToDate ; ";

        //public static string GetOpenDentalProviderHoursData = " SELECT sd.scheduleNum,IFNULL(sdo.ScheduleOpNum,0), "
        //                                        + " (CASE WHEN IFNULL(sdo.ScheduleOpNum,0) = 0 THEN sd.scheduleNum ELSE CONCAT(CONCAT(sd.scheduleNum,'_',sdo.ScheduleOpNum),'_',IfNull(o.ClinicNum,sd.ClinicNum)) END) AS PH_EHR_ID, "
        //                                        + " sd.provNum AS Provider_EHR_ID,sd.scheddate,CONCAT(sd.scheddate,' ',sd.StartTime) AS StartTime ,CONCAT(sd.scheddate,' ',sd.StopTime)AS EndTime, "
        //                                        + " sdo.OperatoryNum AS Operatory_EHR_ID,sd.note AS comment,sd.status, sd.DateTStamp AS Entry_DateTime, "
        //                                        + " IfNull(o.ClinicNum,sd.ClinicNum)  as Clinic_Number "
        //                                        + " FROM schedule sd left JOIN scheduleop sdo ON sdo.ScheduleNum = sd.ScheduleNum LEFT JOIN operatory o ON o.OperatoryNum = sdo.OperatoryNum "
        //                                        + " WHERE sd.SchedType = 1 AND sd.scheddate > @ToDate ; ";


        public static string GetOpenDentalProviderHoursData = " SELECT sd.scheduleNum,IFNULL(sdo.ScheduleOpNum,0), "
                                                            + " (CASE WHEN IFNULL(sdo.ScheduleOpNum,0) = 0 THEN sd.scheduleNum ELSE CONCAT(CONCAT(sd.scheduleNum,'_',sdo.ScheduleOpNum),'_',IfNull(o.ClinicNum,sd.ClinicNum)) END) AS PH_EHR_ID, "
                                                            + " sd.provNum AS Provider_EHR_ID,sd.scheddate,CONCAT(sd.scheddate,' ',sd.StartTime) AS StartTime ,CONCAT(sd.scheddate,' ',sd.StopTime)AS EndTime, "
                                                            + " sdo.OperatoryNum AS Operatory_EHR_ID,sd.note AS comment,sd.status, sd.DateTStamp AS Entry_DateTime, "
                                                            + " IfNull(o.ClinicNum,sd.ClinicNum) as Clinic_Number "
                                                            + " FROM schedule sd left JOIN scheduleop sdo ON sdo.ScheduleNum = sd.ScheduleNum LEFT JOIN operatory o ON o.OperatoryNum = sdo.OperatoryNum "
                                                            + " WHERE sd.SchedType = 1 AND sd.scheddate > @ToDate AND sd.starttime <> '00:00:00'; ";

        public static string GetOpenDentalProviderHoursData_15_4 = " SELECT sd.scheduleNum,IFNULL(sdo.ScheduleOpNum,0), "
                                                                 + " (CASE WHEN IFNULL(sdo.ScheduleOpNum,0) = 0 THEN sd.scheduleNum ELSE CONCAT(CONCAT(sd.scheduleNum,'_',sdo.ScheduleOpNum),'_',IfNull(o.ClinicNum,0)) END) AS PH_EHR_ID, "
                                                                 + " sd.provNum AS Provider_EHR_ID,sd.scheddate,CONCAT(sd.scheddate,' ',sd.StartTime) AS StartTime ,CONCAT(sd.scheddate,' ',sd.StopTime)AS EndTime, "
                                                                 + " sdo.OperatoryNum AS Operatory_EHR_ID,sd.note AS comment,sd.status, sd.DateTStamp AS Entry_DateTime, "
                                                                 + " IfNull(o.ClinicNum,0) as Clinic_Number "
                                                                 + " FROM schedule sd left JOIN scheduleop sdo ON sdo.ScheduleNum = sd.ScheduleNum LEFT JOIN operatory o ON o.OperatoryNum = sdo.OperatoryNum "
                                                                 + " WHERE sd.SchedType = 1 AND sd.scheddate > @ToDate AND sd.starttime <> '00:00:00'; ";

        public static string GetOpenDentalOperatoryData = "SELECT OperatoryNum AS Operatory_EHR_ID,OpName AS Operatory_Name,clinicnum as Clinic_Number,ItemOrder AS OperatoryOrder,IsHidden as Is_Deleted FROM operatory o;";

        public static string GetOpenDentalFolderListData = "select DefNum as FolderList_EHR_ID,ItemOrder as FolderOrder, ItemName as Folder_Name, IsHidden as Is_Deleted from definition where Category=18";

        public static string GetOpenDentalDeletedOperatoryData = "SELECT OperatoryNum AS Operatory_EHR_ID,OpName AS Operatory_Name,clinicnum as Clinic_Number FROM operatory o where IsHidden = 1 ;";

        public static string GetOpenDentalDeletedFolderListData = "select DefNum as FolderList_EHR_ID,ItemOrder as FolderOrder, ItemName as Folder_Name, IsHidden as Is_Deleted from definition where Category=18 and IsHidden = 1";


        public static string GetOpenDentalApptTypeData = "SELECT AppointmentTypeNum AS ApptType_EHR_ID, AppointmentTypeName AS Type_Name,'' as Clinic_Number FROM appointmenttype a;";

        public static string GetProcDescription = "select group_concat(AbbrDesc separator ', #') as ProcDesc from procedurecode  where CodeNum in(@codenum);";

        public static string Is_Update_Status_EHR_Appointment_Live_To_Opendental = "SELECT AptStatus, Confirmed,IFNull(a.clinicnum,o.clinicnum) as Clinic_Number  FROM appointment LEFT JOIN operatory o ON a.OP = o.OperatoryNum  Where AptNum  = @Appt_EHR_ID;";

        //public static string GetOpenDentalPatientData = " SELECT p.PatNum AS Patient_EHR_ID,FName AS First_name,LName AS Last_name,MiddleI AS Middle_Name,Title AS Salutation, "
        //                                  + " PatStatus AS Status,Gender AS Sex, Position AS MaritalStatus, Birthdate AS Birth_Date,Email AS Email,WirelessPhone AS Mobile,HmPhone AS Home_Phone, "
        //                                  + " WkPhone AS Work_Phone,Address AS Address1,Address2 AS Address2,City AS City,State AS State,Zip AS Zipcode,BalTotal AS CurrentBal, "
        //                                  + " Bal_0_30 AS ThirtyDay,Bal_31_60 AS SixtyDay,Bal_61_90 AS NinetyDay,BalOver90 AS Over90,DateFirstVisit AS FirstVisit_Date, "
        //                                  + " (Select AptDateTime from Appointment a WHERE a.PatNum = p.Patnum And a.AptStatus=2 ORDER BY AptDateTime DESC LIMIT 1) As LastVisit_Date, "
        //                                  + " Guarantor AS Guar_ID,PriProv AS Pri_Provider_ID,SecProv AS Sec_Provider_ID,TxtMsgOk AS ReceiveSMS, "
        //                                  + "  ''  AS nextvisit_date,'' AS due_date , '' AS remaining_benefit, '' AS collect_payment "
        //                                  + " FROM patient p;";

        #region PatientQuery //Must change select statement form all query




        public static string GetOpenDentalPatientData_New = "SELECT DISTINCT '0' as 'Patient_LocalDB_ID', p.PatNum AS Patient_EHR_ID, '0' as 'Patient_Web_ID', p.fname AS First_name, p.lname AS Last_name, p.MiddleI AS Middle_Name, p.Salutation, case when p.patstatus = 0 then 'A' else 'I' end as Status, case when p.Gender = 0 then 'Male' when p.Gender = 1 then 'Female' when p.Gender > 1 then 'Unknown' end AS Sex, case when p.Position = 0 then 'Single' when p.Position = 1 then 'Married' when p.Position = 2 then 'Child' when p.Position = 3 then 'Widowed' when p.Position = 4 then 'Divorced' end AS MaritalStatus, IF( p.Birthdate = '0001-01-01' or p.Birthdate = '0000-00-00', NULL, p.Birthdate ) AS Birth_Date, p.Email AS Email, REPLACE( REPLACE( REPLACE( REPLACE(p.WirelessPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Mobile, REPLACE( REPLACE( REPLACE( REPLACE(p.HmPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Home_Phone, REPLACE( REPLACE( REPLACE( REPLACE(p.WkPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Work_Phone, p.Address AS Address1, p.Address2 AS Address2, p.City AS City, p.State AS State, p.Zip AS Zipcode, '' as ResponsibleParty_Status, p.EstBalance AS CurrentBal, p.Bal_0_30 AS ThirtyDay, p.Bal_31_60 AS SixtyDay, p.Bal_61_90 AS NinetyDay, p.BalOver90 AS Over90, IF( p.DateFirstVisit = '0001-01-01' or p.DateFirstVisit = '0000-00-00', NULL, p.DateFirstVisit ) AS FirstVisit_Date, LastVisit_Date, PI.Primary_Insurance, PI.Primary_Insurance_CompanyName, PI.SubscriberID as Primary_Ins_Subscriber_ID, SI.Secondary_Insurance, SI.Secondary_Insurance_CompanyName, SI.SubscriberID as Secondary_Ins_Subscriber_ID, p.Guarantor AS Guar_ID, p.PriProv AS Pri_Provider_ID, p.SecProv AS Sec_Provider_ID, ( case when Cast(p.TxtMsgOk As Char) = '0' or Cast(p.TxtMsgOk As Char) = '' or Cast(p.TxtMsgOk As Char) = '1' then 'Y' else 'N' end ) as ReceiveSMS, p.email as ReceiveEmail, NextVisit_Date, DD.DueDate as due_date, IFNULL(AC.TotalBenafit, 0) AS remaining_benefit, CP.CollectPayment AS collect_payment, Now() as Last_Sync_Date, p.dateTStamp AS EHR_Entry_DateTime, 'false' as 'Is_Adit_Updated', p.Preferred As preferred_name, IFNULL(AA.UsedBenafit, 0) As used_benefit, p.clinicnum as Clinic_Number, '' as 'Service_Install_Id', case when p.patstatus IN(4, 5) then 1 else 0 end as is_deleted, case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then 'NonPatient' when p.patstatus = 2 then 'InActive' when p.patstatus = 3 then 'InActive' when p.patstatus IN(4, 5) then 'Deleted' when p.patstatus = 6 then 'NonPatient' end as EHR_Status, case when p.prefercontactmethod = 1 then 'N' else 'Y' end as ReceiveVoiceCall, '' as 'Patient_status', '' as 'Patient_status_Compare', p.AddrNote AS Patient_Note, CASE WHEN ISNULL(p.language) THEN 'eng' When p.Language = '' Then 'eng' ELSE p.language END AS PreferredLanguage, p.SSN as SSN, '' as driverlicense, '' AS groupid, '' AS emergencycontactId, pn.ICEPhone AS emergencycontactnumber, p.SchoolName as school, emp.EmpName AS employer, PG.spouseId, resp.responsparty AS responsiblepartyId, resp.SSN AS responsiblepartyssn, resp.Birthdate AS responsiblepartybirthdate, PG.Spouse_Last_Name, resp.FName AS ResponsibleParty_First_Name, resp.Lname AS ResponsibleParty_Last_Name, '' AS Prim_Ins_Company_Phonenumber, '' as Sec_Ins_Company_Phonenumber, pn.ICEName AS EmergencyContact_First_Name, '' AS EmergencyContact_Last_Name, PG.Spouse_First_Name, 'false' as Is_Status_Adit_Updated, 1 AS InsUptDlt FROM patient p left join employer as emp on p.EmployerNum = emp.EmployerNum left join patientnote as pn on p.Patnum = pn.Patnum left join patient as resp on p.Patnum = resp.responsparty and resp.responsparty > 0 left JOIN ( select patnum, IFNULL( sum(InsPayAmt), 0 ) AS UsedBenafit from claimproc group by patnum ) as AA on AA.patnum = P.PatNum LEFT JOIN ( SELECT PatNum, IFNULL( Sum(PP.Payamt), 0 ) AS CollectPayment FROM Payment PP Group by PatNum ) AS CP ON CP.PatNum = P.PatNum left JOIN ( SELECT distinct i.subscriber AS PatNum, b.benefitNum, IFNULL( (b.MonetaryAmt), 0 ) AS TotalBenafit FROM benefit b LEFT JOIN inssub i on i.plannum = b.plannum and benefitType = 5 and CoverageLevel = 1 group by i.subscriber ) as AC on AC.PatNum = P.Patnum left join ( SELECT PT.patnum, max(PT.AptDateTime) as LastVisit_Date from appointment PT WHERE AptDateTime < Now() GROUP by PT.patnum ) Ldata on p.PatNum = Ldata.patnum left join ( SELECT PT.patnum, min(PT.AptDateTime) as nextvisit_date from appointment PT WHERE AptDateTime > Now() AND AptStatus NOT IN (3,5) GROUP by PT.patnum ) Ndata on p.PatNum = Ndata.patnum LEFT JOIN ( Select patient_id, group_concat( DueDate Order By DateDue Asc, Description Asc separator '|' ) as DueDate FROM ( SELECT r.patnum AS patient_id, p.clinicnum As Clinic_Number, CONCAT( r.DateDue, '@', rt.description, '@', rt.RecallTypeNum ) AS DueDate, r.DateDue, rt.description FROM recall r INNER Join patient p on p.PatNum = r.PatNum INNER JOIN appointment ap on ap.patnum = p.patnum INNER JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum where r.DateDue > '00010101' Group By r.patnum, r.RecallTypeNum Order By r.DateDue ) AS B Group By B.patient_id ) AS DD ON DD.patient_id = p.patnum Left Join ( SELECT pat.PatNum, ip.carrierNum as Primary_Insurance, c.CarrierName as Primary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum join carrier c on c.carrierNum = ip.carrierNum where PP.Ordinal = 1 ) PI on PI.PatNum = p.PatNum Left join( SELECT pat.PatNum, ip.carrierNum as Secondary_Insurance, c.CarrierName as Secondary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum join carrier c on c.carrierNum = ip.carrierNum where PP.Ordinal = 2 ) SI on SI.PatNum = p.PatNum left join( select patnumchild, max(patnumguardian) as spouseId, FName as Spouse_First_Name, LName as Spouse_Last_Name from patient pa INNER JOIN guardian g ON pa.patnum = g.patnumguardian and relationship = 18 group by patnumchild ) PG on p.patnum = PG.patnumchild where p.PatNum <> '' AND p.lname <> '' and ( Not( ( p.fname = '' OR p.lname = '' ) and p.patstatus = 4 ) )";

        public static string GetOpenDentalPatientData = " SELECT DISTINCT p.PatNum AS Patient_EHR_ID,p.FName AS First_name,p.lname AS Last_name,p.MiddleI AS Middle_Name,p.Salutation, "
                                                     + "   p.Preferred As preferred_name,  Cast(p.Gender As char(7)) AS Sex, Cast(p.Position As char(8)) AS MaritalStatus,"
                                                     + "   IF(p.Birthdate = '0001-01-01' or p.Birthdate = '0000-00-00', NULL, p.Birthdate) AS Birth_Date,p.Email AS Email,p.WirelessPhone AS Mobile,p.HmPhone AS Home_Phone, "
                                                     + "   p.WkPhone AS Work_Phone,p.Address AS Address1,p.Address2 AS Address2,p.City AS City,p.State AS State,p.Zip AS Zipcode,p.EstBalance AS CurrentBal, "
                                                     + "   p.Bal_0_30 AS ThirtyDay,p.Bal_31_60 AS SixtyDay,p.Bal_61_90 AS NinetyDay,p.BalOver90 AS Over90, IF(p.DateFirstVisit = '0001-01-01' or p.DateFirstVisit = '0000-00-00', NULL, p.DateFirstVisit) AS FirstVisit_Date, "
                                                     + "   '' As LastVisit_Date, p.dateTStamp AS EHR_Entry_DateTime,p.language as PreferredLanguage, "
                                                     + "   p.Guarantor AS Guar_ID,p.PriProv AS Pri_Provider_ID,p.SecProv AS Sec_Provider_ID, Cast(p.TxtMsgOk As Char) AS ReceiveSMS, "
                                                     + "    ''  AS nextvisit_date,'' AS due_date , '0' As used_benefit, '0' AS remaining_benefit, '' AS collect_payment, 1 AS InsUptDlt, "
                                                     + "   p.clinicnum as Clinic_Number,case when p.patstatus IN (4,5) then 1 else 0 end as is_deleted, "
                                                     + "   case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then 'NonPatient' when p.patstatus = 2 then 'InActive' when p.patstatus = 3 then 'InActive' when p.patstatus IN (4,5) then 'Deleted' when  p.patstatus = 6 then 'NonPatient'  end as EHR_Status "
                                                     + "  ,case when p.patstatus = 0 then 'A' else 'I' end as Status, case when p.prefercontactmethod = 1 then 'N' else 'Y' end as ReceiveVoiceCall,p.AddrNote AS Patient_Note, "
                                                     + "  p.SSN as ssn, p.SchoolName as school, '' as driverlicense,emp.EmpName AS employer, "
                                                     + "  '' AS emergencycontactId,pn.ICEName AS EmergencyContact_First_Name,'' AS EmergencyContact_Last_Name,pn.ICEPhone AS emergencycontactnumber, "
                                                     + "   resp.responsparty AS responsiblepartyId,resp.FName AS ResponsibleParty_First_Name,resp.Lname AS ResponsibleParty_Last_Name,resp.SSN AS responsiblepartyssn, "
                                                     + "  resp.Birthdate AS responsiblepartybirthdate,"
                                                     + "   PG.spouseId,PG.Spouse_First_Name,PG.Spouse_Last_Name,'' as Sec_Ins_Company_Phonenumber, '' AS Prim_Ins_Company_Phonenumber , '' AS groupid "

                                                      + " FROM patient p left join employer as emp on p.EmployerNum = emp.EmployerNum "
                                                       + "  left join patientnote as pn on p.Patnum=pn.Patnum "
                                                       + "  left join patient as resp on p.Patnum=resp.responsparty and resp.responsparty > 0 "
                                                       + "  left join (select patnumchild, max(patnumguardian) as spouseId,FName as Spouse_First_Name,LName as Spouse_Last_Name "
                                                       + "  from patient pa,guardian g where pa.patnum=g.patnumguardian and relationship =18 group by patnumchild ) PG on p.patnum = PG.patnumchild "
                                                       //+ "  left join inssub AS ins ON ins.subscriber = p.patnum "
                                                       //+ "  left join insplan AS inp ON inp.PlanNum=ins.PlanNum "
                                                       //+ "  left join carrier AS C ON c.CarrierNum=inp.CarrierNum  "
                                                       + " WHERE p.PatNum <> ''  AND p.lname <> '' And Not ((p.FName = '' OR p.lname = '') and p.patstatus = 4)"
                                                       + " and (p.clinicnum in (@Clinic_Number))";

        public static string GetOpenDentalInsertPatientData = "Select p.PatNum as Patient_EHR_ID,p.clinicnum as Clinic_Number from patient p where p.PatNum <> '' AND p.lname <> '' and ( Not( ( p.fname = '' OR p.lname = '' ) and p.patstatus = 4 ) ) and (p.clinicnum = @Clinic_Number);";

        public static string GetOpenDentalPatientData_Clinic_New = "SELECT DISTINCT '0' as 'Patient_LocalDB_ID', p.PatNum AS Patient_EHR_ID, '0' as 'Patient_Web_ID', p.fname AS First_name, p.lname AS Last_name, p.MiddleI AS Middle_Name, p.Salutation," +
            " case when p.patstatus = 0 then 'A' else 'I' end as Status, case when p.Gender = 0 then 'Male' " +
            "when p.Gender = 1 then 'Female' when p.Gender > 1 then 'Unknown' end AS Sex, case when p.Position = 0 then 'Single' when p.Position = 1 " +
            "then 'Married' when p.Position = 2 then 'Child' when p.Position = 3 then 'Widowed' when p.Position = 4 then 'Divorced' end " +
            "AS MaritalStatus, IF( p.Birthdate = '0001-01-01' or p.Birthdate = '0000-00-00', NULL, p.Birthdate ) AS Birth_Date, p.Email AS Email, " +
            "REPLACE( REPLACE( REPLACE( REPLACE(p.WirelessPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Mobile, " +
            "REPLACE( REPLACE( REPLACE( REPLACE(p.HmPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Home_Phone, " +
            "REPLACE( REPLACE( REPLACE( REPLACE(p.WkPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Work_Phone, p.Address AS Address1," +
            " p.Address2 AS Address2, p.City AS City, p.State AS State, p.Zip AS Zipcode, '' as ResponsibleParty_Status, p.EstBalance AS CurrentBal, " +
            "p.Bal_0_30 AS ThirtyDay, p.Bal_31_60 AS SixtyDay, p.Bal_61_90 AS NinetyDay, p.BalOver90 AS Over90, " +
            "IF( p.DateFirstVisit = '0001-01-01' or p.DateFirstVisit = '0000-00-00', NULL, p.DateFirstVisit ) AS FirstVisit_Date, LastVisit_Date, " +
            "PI.Primary_Insurance, PI.Primary_Insurance_CompanyName, PI.SubscriberID as Primary_Ins_Subscriber_ID, SI.Secondary_Insurance, " +
            "SI.Secondary_Insurance_CompanyName, SI.SubscriberID as Secondary_Ins_Subscriber_ID, p.Guarantor AS Guar_ID, p.PriProv AS Pri_Provider_ID," +
            " p.SecProv AS Sec_Provider_ID, ( case when Cast(p.TxtMsgOk As Char) = '0' or Cast(p.TxtMsgOk As Char) = '' " +
            "or Cast(p.TxtMsgOk As Char) = '1' then 'Y' else 'N' end ) as ReceiveSMS, 'Y' as ReceiveEmail, NextVisit_Date, " +
            "DD.DueDate as due_date, IFNULL(AC.TotalBenafit, 0) AS remaining_benefit, IFNULL(CP.CollectPayment,0) AS collect_payment, Now() as Last_Sync_Date," +
            " p.dateTStamp AS EHR_Entry_DateTime, 'false' as 'Is_Adit_Updated', p.Preferred As preferred_name, IFNULL(AA.UsedBenafit, 0) As used_benefit, " +
            "p.clinicnum as Clinic_Number, '' as 'Service_Install_Id', case when p.patstatus IN(4, 5) then 1 else 0 end as is_deleted, " +
            "case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then 'NonPatient' when p.patstatus = 2 then 'InActive'" +
            " when p.patstatus = 3 then 'InActive' when p.patstatus IN(4, 5) then 'Deleted' when p.patstatus = 6 then 'NonPatient' end as EHR_Status, " +
            "case when p.prefercontactmethod = 1 then 'N' else 'Y' end as ReceiveVoiceCall, '' as 'Patient_status', '' as 'Patient_status_Compare'," +
            " p.AddrNote AS Patient_Note, CASE WHEN ISNULL(p.language) THEN 'eng' When p.Language = '' Then 'eng' ELSE p.language END " +
            "AS PreferredLanguage, p.SSN as SSN, '' as driverlicense, '' AS groupid, '' AS emergencycontactId, pn.ICEPhone AS emergencycontactnumber," +
            " p.SchoolName as school, emp.EmpName AS employer, PG.spouseId, resp.responsparty AS responsiblepartyId, resp.SSN AS responsiblepartyssn, " +
            "resp.Birthdate AS responsiblepartybirthdate, PG.Spouse_Last_Name, resp.FName AS ResponsibleParty_First_Name, " +
            "resp.Lname AS ResponsibleParty_Last_Name, '' AS Prim_Ins_Company_Phonenumber, '' as Sec_Ins_Company_Phonenumber, " +
            "pn.ICEName AS EmergencyContact_First_Name, '' AS EmergencyContact_Last_Name, PG.Spouse_First_Name, 'false' as Is_Status_Adit_Updated," +
            " 1 AS InsUptDlt FROM patient p left join employer as emp on p.EmployerNum = emp.EmployerNum left join patientnote as pn on p.Patnum = pn.Patnum left join patient as resp on p.Patnum = resp.responsparty and resp.responsparty > 0 left JOIN ( select patnum, IFNULL( sum(InsPayAmt), 0 ) AS UsedBenafit from claimproc Where ClinicNum = @Clinic_Number group by patnum ) as AA on AA.patnum = P.PatNum LEFT JOIN ( SELECT PatNum, IFNULL( Sum(PP.Payamt), 0 ) AS CollectPayment FROM Payment PP WHERE PP.ClinicNum = @Clinic_Number Group by PatNum ) AS CP ON CP.PatNum = P.PatNum left JOIN ( SELECT distinct i.subscriber AS PatNum, b.benefitNum, IFNULL( (b.MonetaryAmt), 0 ) AS TotalBenafit FROM benefit b LEFT JOIN inssub i on i.plannum = b.plannum and benefitType = 5 and CoverageLevel = 1 group by i.subscriber ) as AC on AC.PatNum = P.Patnum left join ( SELECT PT.patnum, max(PT.AptDateTime) as LastVisit_Date from appointment PT WHERE AptDateTime < Now() AND PT.ClinicNum = @Clinic_Number GROUP by PT.patnum ) Ldata on p.PatNum = Ldata.patnum left join ( SELECT PT.patnum, min(PT.AptDateTime) as nextvisit_date from appointment PT WHERE AptDateTime > Now() AND AptStatus NOT IN (3,5) AND PT.ClinicNum = @Clinic_Number GROUP by PT.patnum ) Ndata on p.PatNum = Ndata.patnum LEFT JOIN ( Select patient_id, group_concat(DueDate Order By DateDue Asc, Description Asc separator '|') as DueDate FROM ( SELECT r.patnum AS patient_id, p.clinicnum As Clinic_Number, CONCAT( r.DateDue, '@', rt.description, '@', rt.RecallTypeNum ) AS DueDate, r.DateDue, rt.description FROM recall r INNER Join patient p on p.PatNum = r.PatNum AND P.ClinicNum = @Clinic_Number INNER JOIN appointment ap on ap.patnum = p.patnum AND ap.ClinicNum = @Clinic_Number INNER JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum where p.clinicnum = @Clinic_Number AND r.DateDue > '00010101'  Group By r.patnum, r.RecallTypeNum Order By r.DateDue ) AS B Group By B.patient_id ) AS DD ON DD.patient_id = p.patnum Left Join ( SELECT pat.PatNum, ip.carrierNum as Primary_Insurance, c.CarrierName as Primary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum AND pat.Clinicnum = @Clinic_Number join carrier c on c.carrierNum = ip.carrierNum where pat.clinicnum = @Clinic_Number and PP.Ordinal = 1 ) PI on PI.PatNum = p.PatNum Left join( SELECT pat.PatNum, ip.carrierNum as Secondary_Insurance, c.CarrierName as Secondary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum AND PAT.ClinicNum = @Clinic_Number join carrier c on c.carrierNum = ip.carrierNum where pat.clinicnum = @Clinic_Number and PP.Ordinal = 2 ) SI on SI.PatNum = p.PatNum left join( select patnumchild, max(patnumguardian) as spouseId, FName as Spouse_First_Name, LName as Spouse_Last_Name from patient pa INNER JOIN guardian g ON pa.patnum = g.patnumguardian and relationship = 18 group by patnumchild ) PG on p.patnum = PG.patnumchild where p.PatNum <> '' AND p.lname <> '' and ( Not( ( p.fname = '' OR p.lname = '' ) and p.patstatus = 4 ) ) and (p.clinicnum = @Clinic_Number)";

        public static string GetOpenDentalPatientData_Clinic = "  SELECT DISTINCT p.PatNum AS Patient_EHR_ID,p.fname AS First_name,p.lname AS Last_name,p.MiddleI AS Middle_Name,p.Salutation, "
                                                     + "   p.Preferred As preferred_name,  Cast(p.Gender As char(7)) AS Sex, Cast(p.Position As char(8)) AS MaritalStatus,"
                                                     + "   IF(p.Birthdate = '0001-01-01' or p.Birthdate = '0000-00-00', NULL, p.Birthdate) AS Birth_Date,p.Email AS Email,p.WirelessPhone AS Mobile,p.HmPhone AS Home_Phone, "
                                                     + "   p.WkPhone AS Work_Phone,p.Address AS Address1,p.Address2 AS Address2,p.City AS City,p.State AS State,p.Zip AS Zipcode,p.EstBalance AS CurrentBal, "
                                                     + "   p.Bal_0_30 AS ThirtyDay,p.Bal_31_60 AS SixtyDay,p.Bal_61_90 AS NinetyDay,p.BalOver90 AS Over90, IF(p.DateFirstVisit = '0001-01-01' or p.DateFirstVisit = '0000-00-00', NULL, p.DateFirstVisit) AS FirstVisit_Date, "
                                                     + "   '' As LastVisit_Date, p.dateTStamp AS EHR_Entry_DateTime,p.language as PreferredLanguage, "
                                                     + "   p.Guarantor AS Guar_ID,p.PriProv AS Pri_Provider_ID,p.SecProv AS Sec_Provider_ID, Cast(p.TxtMsgOk As Char) AS ReceiveSMS, "
                                                     + "    ''  AS nextvisit_date,'' AS due_date , '0' As used_benefit, '0' AS remaining_benefit, '' AS collect_payment, 1 AS InsUptDlt, "
                                                     + "   p.clinicnum as Clinic_Number,case when p.patstatus IN (4,5) then 1 else 0 end as is_deleted, "
                                                     + "   case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then 'NonPatient' when p.patstatus = 2 then 'InActive' when p.patstatus = 3 then 'InActive' when p.patstatus IN (4,5) then 'Deleted' when  p.patstatus = 6 then 'NonPatient'  end as EHR_Status "
                                                     + "  ,case when p.patstatus = 0 then 'A' else 'I' end as Status, case when p.prefercontactmethod = 1 then 'N' else 'Y' end as ReceiveVoiceCall,p.AddrNote AS Patient_Note, "
                                                       + " p.SSN as ssn, p.SchoolName as school, '' as driverlicense,emp.EmpName AS employer, "
                                                     + "  '' AS emergencycontactId,pn.ICEName AS EmergencyContact_First_Name,'' AS EmergencyContact_Last_Name,pn.ICEPhone AS emergencycontactnumber, "
                                                     + "   resp.responsparty AS responsiblepartyId,resp.FName AS ResponsibleParty_First_Name,resp.Lname AS ResponsibleParty_Last_Name,resp.SSN AS responsiblepartyssn,resp.Birthdate AS responsiblepartybirthdate, "
                                                     + "   PG.spouseId,PG.Spouse_First_Name,PG.Spouse_Last_Name,'' as Sec_Ins_Company_Phonenumber, '' AS Prim_Ins_Company_Phonenumber , '' AS groupid "
                                                       + " FROM patient p left join employer as emp on p.EmployerNum = emp.EmployerNum "
                                                       + "  left join patientnote as pn on p.Patnum=pn.Patnum "
                                                       + "  left join patient as resp on p.Patnum=resp.responsparty and resp.responsparty > 0"
                                                       + "  left join (select patnumchild, max(patnumguardian) as spouseId,FName as Spouse_First_Name,LName as Spouse_Last_Name "
                                                       + "  from patient pa,guardian g where pa.patnum=g.patnumguardian and relationship =18 group by patnumchild ) PG on p.patnum = PG.patnumchild "
                                                       //+ "  left join inssub AS ins ON ins.subscriber = p.patnum "
                                                       //+ "  left join insplan AS inp ON inp.PlanNum=ins.PlanNum "
                                                       //+ "  left join carrier AS C ON c.CarrierNum=inp.CarrierNum"
                                                       + " where  p.PatNum <> ''  AND p.lname <> '' and (Not ((p.fname = '' OR p.lname = '') and p.patstatus = 4)) and (p.clinicnum = @Clinic_Number) ;"; //  and p.patstatus <> 4





        public static string GetOpenDentalAppointmentsPatientData = " SELECT DISTINCT p.PatNum AS Patient_EHR_ID,p.FName AS First_name,p.lname AS Last_name,p.MiddleI AS Middle_Name,p.Salutation, "
                                                     + "   p.Preferred As preferred_name,  Cast(p.Gender As char(7)) AS Sex, Cast(p.Position As char(8)) AS MaritalStatus,"
                                                     + "   p.Birthdate AS Birth_Date,p.Email AS Email,p.WirelessPhone AS Mobile,p.HmPhone AS Home_Phone, "
                                                     + "   p.WkPhone AS Work_Phone,p.Address AS Address1,p.Address2 AS Address2,p.City AS City,p.State AS State,p.Zip AS Zipcode,p.EstBalance AS CurrentBal, "
                                                     + "   p.Bal_0_30 AS ThirtyDay,p.Bal_31_60 AS SixtyDay,p.Bal_61_90 AS NinetyDay,p.BalOver90 AS Over90,p.DateFirstVisit AS FirstVisit_Date, "
                                                     + "   '' As LastVisit_Date, p.dateTStamp AS EHR_Entry_DateTime,p.language as PreferredLanguage, "
                                                     + "   p.Guarantor AS Guar_ID,p.PriProv AS Pri_Provider_ID,p.SecProv AS Sec_Provider_ID, Cast(p.TxtMsgOk As Char) AS ReceiveSMS, "
                                                     + "    ''  AS nextvisit_date,'' AS due_date , '0' As used_benefit, '0' AS remaining_benefit, '' AS collect_payment, 1 AS InsUptDlt, "
                                                     + "   p.clinicnum as Clinic_Number,case when p.patstatus IN (4,5) then 1 else 0 end as is_deleted, "
                                                     + "   case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then 'NonPatient' when p.patstatus = 2 then 'InActive' when p.patstatus = 3 then 'InActive' when p.patstatus IN (4,5) then 'Deleted' when  p.patstatus = 6 then 'NonPatient'  end as EHR_Status "
                                                     + "  ,case when p.patstatus = 0 then 'A' else 'I' end as Status, case when p.prefercontactmethod = 1 then 'N' else 'Y' end as ReceiveVoiceCall,p.AddrNote AS Patient_Note, "
                                                    + " p.SSN as ssn, p.SchoolName as school, '' as driverlicense,emp.EmpName AS employer, "
                                                     + "  '' AS emergencycontactId,pn.ICEName AS EmergencyContact_First_Name,'' AS EmergencyContact_Last_Name,pn.ICEPhone AS emergencycontactnumber, "
                                                     + "   resp.responsparty AS responsiblepartyId,resp.FName AS ResponsibleParty_First_Name,resp.Lname AS ResponsibleParty_Last_Name,resp.SSN AS responsiblepartyssn,resp.Birthdate AS responsiblepartybirthdate, "
                                                     + "   PG.spouseId,PG.Spouse_First_Name,PG.Spouse_Last_Name,'' as Sec_Ins_Company_Phonenumber, '' AS Prim_Ins_Company_Phonenumber , '' AS groupid "
                                                    + " FROM patient p Inner Join appointment a ON p.PatNum = a.PatNum left join employer as emp on p.EmployerNum = emp.EmployerNum  "
                                                        + " left join patientnote as pn on p.Patnum=pn.Patnum "
                                                       + "  left join patient as resp on p.Patnum=resp.responsparty and resp.responsparty > 0"
                                                       + "  left join (select patnumchild, max(patnumguardian) as spouseId,FName as Spouse_First_Name,LName as Spouse_Last_Name "
                                                       + "  from patient pa,guardian g where pa.patnum=g.patnumguardian and relationship =18 group by patnumchild ) PG on p.patnum = PG.patnumchild "
                                                    //+ "  left join inssub AS ins ON ins.subscriber = p.patnum "
                                                    //+ "  left join insplan AS inp ON inp.PlanNum=ins.PlanNum "
                                                    //+ "  left join carrier AS C ON c.CarrierNum=inp.CarrierNum"
                                                    + " WHERE p.PatNum <> ''  AND p.lname <> '' and ((p.FName <> '' OR p.lname <> '') and p.patstatus <> 4)"
                                                    + " And a.AptDateTime > @ToDate ; ";

        //public static string GetOpenDentalAppointmentsPatientData_Clinic_New = "SELECT DISTINCT '0' as 'Patient_LocalDB_ID', p.PatNum AS Patient_EHR_ID, '0' as 'Patient_Web_ID', p.fname AS First_name, p.lname AS Last_name, p.MiddleI AS Middle_Name, p.Salutation, case when p.patstatus = 0 then 'A' else 'I' end as Status, case when p.Gender = 0 then 'Male' when p.Gender = 1 then 'Female' when p.Gender > 1 then 'Unknown' end AS Sex, case when p.Position = 0 then 'Single' when p.Position = 1 then 'Married' when p.Position = 2 then 'Child' when p.Position = 3 then 'Widowed' when p.Position = 4 then 'Divorced' end AS MaritalStatus, IF(p.Birthdate = '0001-01-01' or p.Birthdate = '0000-00-00', NULL, p.Birthdate) AS Birth_Date, p.Email AS Email, REPLACE(REPLACE(REPLACE(REPLACE(p.WirelessPhone, '(', ''), ')', ''), '-', ''), ' ', '') AS Mobile, REPLACE(REPLACE(REPLACE(REPLACE(p.HmPhone, '(', ''), ')', ''), '-', ''), ' ', '') AS Home_Phone, REPLACE(REPLACE(REPLACE(REPLACE(p.WkPhone, '(', ''), ')', ''), '-', ''), ' ', '') AS Work_Phone, p.Address AS Address1, p.Address2 AS Address2, p.City AS City, p.State AS State, p.Zip AS Zipcode, '' as ResponsibleParty_Status, p.EstBalance AS CurrentBal, p.Bal_0_30 AS ThirtyDay, p.Bal_31_60 AS SixtyDay, p.Bal_61_90 AS NinetyDay, p.BalOver90 AS Over90, IF(p.DateFirstVisit = '0001-01-01' or p.DateFirstVisit = '0000-00-00', NULL, p.DateFirstVisit) AS FirstVisit_Date, LastVisit_Date, PI.Primary_Insurance, PI.Primary_Insurance_CompanyName, PI.SubscriberID as Primary_Ins_Subscriber_ID, SI.Secondary_Insurance, SI.Secondary_Insurance_CompanyName, SI.SubscriberID as Secondary_Ins_Subscriber_ID, p.Guarantor AS Guar_ID, p.PriProv AS Pri_Provider_ID, p.SecProv AS Sec_Provider_ID, (case when Cast(p.TxtMsgOk As Char) = '0' or Cast(p.TxtMsgOk As Char) = '' or Cast(p.TxtMsgOk As Char) = '1' then 'Y' else 'N' end) as ReceiveSMS, p.email as ReceiveEmail, NextVisit_Date, DD.DueDate as due_date, IFNULL(AC.TotalBenafit, 0) AS remaining_benefit, CP.CollectPayment AS collect_payment, Now() as Last_Sync_Date, p.dateTStamp AS EHR_Entry_DateTime, 'false' as 'Is_Adit_Updated', p.Preferred As preferred_name, IFNULL(AA.UsedBenafit, 0) As used_benefit, p.clinicnum as Clinic_Number, '' as 'Service_Install_Id', case when p.patstatus IN(4, 5) then 1 else 0 end as is_deleted, case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then 'NonPatient' when p.patstatus = 2 then 'InActive' when p.patstatus = 3 then 'InActive' when p.patstatus IN(4, 5) then 'Deleted' when p.patstatus = 6 then 'NonPatient' end as EHR_Status, case when p.prefercontactmethod = 1 then 'N' else 'Y' end as ReceiveVoiceCall, '' as 'Patient_status', '' as 'Patient_status_Compare', p.AddrNote AS Patient_Note, CASE WHEN ISNULL(p.language) THEN 'eng' When p.Language = '' Then 'eng' ELSE p.language END AS PreferredLanguage, p.SSN as SSN, '' as driverlicense, '' AS groupid, '' AS emergencycontactId, pn.ICEPhone AS emergencycontactnumber, p.SchoolName as school, emp.EmpName AS employer, PG.spouseId, resp.responsparty AS responsiblepartyId, resp.SSN AS responsiblepartyssn, resp.Birthdate AS responsiblepartybirthdate, PG.Spouse_Last_Name, resp.FName AS ResponsibleParty_First_Name, resp.Lname AS ResponsibleParty_Last_Name, '' AS Prim_Ins_Company_Phonenumber, '' as Sec_Ins_Company_Phonenumber, pn.ICEName AS EmergencyContact_First_Name, '' AS EmergencyContact_Last_Name, PG.Spouse_First_Name, 'false' as Is_Status_Adit_Updated, 1 AS InsUptDlt FROM patient p left join employer as emp on p.EmployerNum = emp.EmployerNum left join patientnote as pn on p.Patnum = pn.Patnum left join patient as resp on p.Patnum = resp.responsparty and resp.responsparty > 0 LEFT JOIN (select patnum,IFNULL(sum(InsPayAmt),0) AS UsedBenafit from claimproc Where ClinicNum = @Clinic_Number group by patnum) as AA on AA.patnum = p.patnum LEFT JOIN (SELECT PatNum, IFNULL(Sum(PP.Payamt),0) AS CollectPayment FROM payment PP WHERE PP.ClinicNum = @Clinic_Number Group by PatNum) AS CP ON CP.patnum = p.patnum left JOIN (SELECT distinct i.subscriber AS PatNum, b.benefitNum, IFNULL((b.MonetaryAmt), 0) AS TotalBenafit FROM benefit b LEFT JOIN inssub i on i.plannum = b.plannum and benefitType = 5 and CoverageLevel = 1 group by i.subscriber) as AC on AC.patnum = p.patnum LEFT JOIN (SELECT PT.patnum, max(PT.AptDateTime) as LastVisit_Date from appointment PT WHERE AptDateTime < Now() AND PT.ClinicNum = @Clinic_Number GROUP by PT.patnum) Ldata on p.PatNum = Ldata.patnum LEFT JOIN (SELECT PT.patnum, max(PT.AptDateTime) as nextvisit_date from appointment PT WHERE AptDateTime > Now() AND PT.ClinicNum = @Clinic_Number GROUP by PT.patnum) Ndata on p.PatNum = Ndata.patnum LEFT JOIN ( Select patient_id, group_concat(DueDate separator '|') as DueDate FROM (SELECT r.patnum AS patient_id, p.clinicnum As Clinic_Number, CONCAT(r.DateDue, '@', rt.description, '@', rt.RecallTypeNum) AS DueDate FROM recall r INNER Join patient p on p.PatNum = r.PatNum AND p.clinicnum = @Clinic_Number INNER JOIN appointment ap on ap.patnum = p.patnum AND ap.ClinicNum = @Clinic_Number INNER JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum where ap.aptdatetime > Now() AND p.clinicnum = @Clinic_Number) AS B Group By B.patient_id ) AS DD ON DD.patient_id = p.patnum LEFT JOIN ( SELECT pat.PatNum, ip.carrierNum as Primary_Insurance, c.CarrierName as Primary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum AND pat.Clinicnum = @Clinic_Number join carrier c on c.carrierNum = ip.carrierNum where pat.clinicnum = @Clinic_Number and pp.ordinal = 1 ) PI on PI.PatNum = p.PatNum LEFT JOIN ( SELECT pat.PatNum, ip.carrierNum as Secondary_Insurance, c.CarrierName as Secondary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum AND pat.clinicnum = @Clinic_Number join carrier c on c.carrierNum = ip.carrierNum where pat.clinicnum = @Clinic_Number and pp.ordinal = 2 ) SI on SI.PatNum = p.PatNum LEFT JOIN ( select patnumchild, max(patnumguardian) as spouseId, FName as Spouse_First_Name, LName as Spouse_Last_Name from patient pa INNER JOIN guardian g ON pa.patnum = g.patnumguardian and relationship = 18 group by patnumchild ) PG on p.patnum = PG.patnumchild INNER join (select patnum, min(aptdatetime) as aptdatetime from appointment where aptdatetime > @ToDate group by patnum) ap on p.patnum = ap.patnum WHERE p.PatNum <> '' AND p.lname <> '' AND (Not((p.fname = '' OR p.lname = '') and p.patstatus = 4)) AND (p.clinicnum = @Clinic_Number);";
        //Updated this query as below for DueDate
        public static string GetOpenDentalAppointmentsPatientData_Clinic_New = "SELECT DISTINCT '0' as 'Patient_LocalDB_ID', p.PatNum AS Patient_EHR_ID, '0' as 'Patient_Web_ID', p.fname AS First_name, p.lname AS Last_name, p.MiddleI AS Middle_Name, p.Salutation, case when p.patstatus = 0 then 'A' else 'I' end as Status, case when p.Gender = 0 then 'Male' when p.Gender = 1 then 'Female' when p.Gender > 1 then 'Unknown' end AS Sex, case when p.Position = 0 then 'Single' when p.Position = 1 then 'Married' when p.Position = 2 then 'Child' when p.Position = 3 then 'Widowed' when p.Position = 4 then 'Divorced' end AS MaritalStatus, IF(p.Birthdate = '0001-01-01' or p.Birthdate = '0000-00-00', NULL, p.Birthdate ) AS Birth_Date, p.Email AS Email, REPLACE(REPLACE(REPLACE(REPLACE(p.WirelessPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Mobile, REPLACE( REPLACE(REPLACE(REPLACE(p.HmPhone, '(', ''), ')', ''), '-', ''), ' ', '' ) AS Home_Phone, REPLACE( REPLACE(REPLACE(REPLACE(p.WkPhone, '(', ''), ')', ''), '-', ''), ' ', '' ) AS Work_Phone, p.Address AS Address1, p.Address2 AS Address2, p.City AS City, p.State AS State, p.Zip AS Zipcode, '' as ResponsibleParty_Status, p.EstBalance AS CurrentBal, p.Bal_0_30 AS ThirtyDay, p.Bal_31_60 AS SixtyDay, p.Bal_61_90 AS NinetyDay, p.BalOver90 AS Over90, IF(p.DateFirstVisit = '0001-01-01' or p.DateFirstVisit = '0000-00-00', NULL, p.DateFirstVisit ) AS FirstVisit_Date, LastVisit_Date, PI.Primary_Insurance, PI.Primary_Insurance_CompanyName, PI.SubscriberID as Primary_Ins_Subscriber_ID, SI.Secondary_Insurance, SI.Secondary_Insurance_CompanyName, SI.SubscriberID as Secondary_Ins_Subscriber_ID, p.Guarantor AS Guar_ID, p.PriProv AS Pri_Provider_ID, p.SecProv AS Sec_Provider_ID, ( case when Cast(p.TxtMsgOk As Char) = '0' or Cast(p.TxtMsgOk As Char) = '' or Cast(p.TxtMsgOk As Char) = '1' then 'Y' else 'N' end ) as ReceiveSMS, p.email as ReceiveEmail, NextVisit_Date, DD.DueDate as due_date, IFNULL(AC.TotalBenafit, 0) AS remaining_benefit, CP.CollectPayment AS collect_payment, Now() as Last_Sync_Date, p.dateTStamp AS EHR_Entry_DateTime, 'false' as 'Is_Adit_Updated', p.Preferred As preferred_name, IFNULL(AA.UsedBenafit, 0) As used_benefit, p.clinicnum as Clinic_Number, '' as 'Service_Install_Id', case when p.patstatus IN(4, 5) then 1 else 0 end as is_deleted, case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then 'NonPatient' when p.patstatus = 2 then 'InActive' when p.patstatus = 3 then 'InActive' when p.patstatus IN(4, 5) then 'Deleted' when p.patstatus = 6 then 'NonPatient' end as EHR_Status, case when p.prefercontactmethod = 1 then 'N' else 'Y' end as ReceiveVoiceCall, '' as 'Patient_status', '' as 'Patient_status_Compare', p.AddrNote AS Patient_Note, CASE WHEN ISNULL(p.language) THEN 'eng' When p.Language = '' Then 'eng' ELSE p.language END AS PreferredLanguage, p.SSN as SSN, '' as driverlicense, '' AS groupid, '' AS emergencycontactId, pn.ICEPhone AS emergencycontactnumber, p.SchoolName as school, emp.EmpName AS employer, PG.spouseId, resp.responsparty AS responsiblepartyId, resp.SSN AS responsiblepartyssn, resp.Birthdate AS responsiblepartybirthdate, PG.Spouse_Last_Name, resp.FName AS ResponsibleParty_First_Name, resp.Lname AS ResponsibleParty_Last_Name, '' AS Prim_Ins_Company_Phonenumber, '' as Sec_Ins_Company_Phonenumber, pn.ICEName AS EmergencyContact_First_Name, '' AS EmergencyContact_Last_Name, PG.Spouse_First_Name, 'false' as Is_Status_Adit_Updated, 1 AS InsUptDlt FROM patient p left join employer as emp on p.EmployerNum = emp.EmployerNum left join patientnote as pn on p.Patnum = pn.Patnum left join patient as resp on p.Patnum = resp.responsparty and resp.responsparty > 0 LEFT JOIN (select patnum, IFNULL(sum(InsPayAmt), 0 ) AS UsedBenafit from claimproc Where ClinicNum = @Clinic_Number group by patnum ) as AA on AA.patnum = p.patnum LEFT JOIN(SELECT PatNum, IFNULL(Sum(PP.Payamt), 0 ) AS CollectPayment FROM payment PP WHERE PP.ClinicNum = @Clinic_Number Group by PatNum ) AS CP ON CP.patnum = p.patnum left JOIN (SELECT distinct i.subscriber AS PatNum, b.benefitNum, IFNULL((b.MonetaryAmt), 0 ) AS TotalBenafit FROM benefit b LEFT JOIN inssub i on i.plannum = b.plannum and benefitType = 5 and CoverageLevel = 1 group by i.subscriber ) as AC on AC.patnum = p.patnum LEFT JOIN(SELECT PT.patnum, max(PT.AptDateTime) as LastVisit_Date from appointment PT WHERE AptDateTime < Now() AND PT.ClinicNum = @Clinic_Number GROUP by PT.patnum ) Ldata on p.PatNum = Ldata.patnum LEFT JOIN(SELECT PT.patnum, min(PT.AptDateTime) as nextvisit_date from appointment PT WHERE AptDateTime > Now() AND AptStatus NOT IN (3,5) AND PT.ClinicNum = @Clinic_Number GROUP by PT.patnum ) Ndata on p.PatNum = Ndata.patnum LEFT JOIN(Select patient_id, group_concat(DueDate Order By DateDue Asc, Description Asc separator '|') as DueDate FROM (SELECT r.patnum AS patient_id, p.clinicnum As Clinic_Number, CONCAT( r.DateDue, '@', rt.description, '@', rt.RecallTypeNum ) AS DueDate, r.DateDue, rt.description FROM recall r INNER Join patient p on p.patnum = r.patnum AND p.clinicnum = @Clinic_Number INNER JOIN appointment ap on ap.patnum = p.patnum AND ap.ClinicNum = @Clinic_Number INNER JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum where ap.aptdatetime > @ToDate AND p.clinicnum = @Clinic_Number AND r.DateDue > '00010101' Group By r.patnum, r.RecallTypeNum Order By r.DateDue ) AS B Group By B.patient_id ) AS DD ON DD.patient_id = p.patnum LEFT JOIN ( SELECT pat.PatNum, ip.carrierNum as Primary_Insurance, c.CarrierName as Primary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum AND pat.Clinicnum = @Clinic_Number join carrier c on c.carrierNum = ip.carrierNum where pat.clinicnum = @Clinic_Number and pp.ordinal = 1 ) PI on PI.PatNum = p.PatNum LEFT JOIN( SELECT pat.PatNum, ip.carrierNum as Secondary_Insurance, c.CarrierName as Secondary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum AND pat.clinicnum = @Clinic_Number join carrier c on c.carrierNum = ip.carrierNum where pat.clinicnum = @Clinic_Number and pp.ordinal = 2 ) SI on SI.PatNum = p.PatNum LEFT JOIN( select patnumchild, max(patnumguardian) as spouseId, FName as Spouse_First_Name, LName as Spouse_Last_Name from patient pa INNER JOIN guardian g ON pa.patnum = g.patnumguardian and relationship = 18 group by patnumchild ) PG on p.patnum = PG.patnumchild INNER join( select patnum, min(aptdatetime) as aptdatetime from appointment where aptdatetime > @ToDate group by patnum ) ap on p.patnum = ap.patnum WHERE p.PatNum <> '' AND p.lname <> '' AND ( Not( ( p.fname = '' OR p.lname = '' ) and p.patstatus = 4 ) ) AND(p.clinicnum = @Clinic_Number);";

        public static string GetOpenDentalAppointmentsPatientData_Clinic = " " +
            "select p.DateTStamp AS EHR_Entry_DateTime, p.PatNum AS Patient_EHR_ID, p.FName AS First_name, p.lname AS Last_name, " +
            "p.HmPhone AS Home_Phone, p.MiddleI AS Middle_Name,case when p.patstatus = 0 then 'A' else 'I' end as Status," +
            " p.Email AS Email, p.WirelessPhone AS Mobile,'' as LastVisit_Date,'Y'  as ReceiveEmail," +
            "CASE WHEN Cast(p.TxtMsgOk As Char) IN ('0','1','') THEN 'Y' ELSE 'N' END AS ReceiveSMS, " +
            "aptdatetime AS nextvisit_date,1 AS InsUptDlt, p.language as PreferredLanguage, p.clinicnum as Clinic_Number," +
            "case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then  'NonPatient' when p.patstatus = 2 then 'InActive' " +
            "when p.patstatus = 3 then 'InActive' when p.patstatus IN (4,5) then 'Deleted' when p.patstatus = 6 then 'NonPatient'  " +
            "end as EHR_Status ,DD.DueDate as due_date " +
            "from patient p " +
            " INNER join (select patnum, min(aptdatetime) as aptdatetime from appointment where aptdatetime > @ToDate group by patnum) ap on p.patnum = ap.patnum " +
            " LEFT JOIN (Select patient_id,group_concat(DueDate separator '|') as DueDate FROM ( SELECT r.patnum AS patient_id, p.clinicnum As Clinic_Number, CONCAT(r.DateDue, '@', rt.description, '@', rt.RecallTypeNum) AS DueDate FROM recall r INNER Join patient p on p.PatNum = r.PatNum INNER JOIN appointment ap on ap.patnum = p.patnum INNER JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum where ap.aptdatetime > @ToDate AND p.clinicnum = @Clinic_Number ) AS B Group By B.patient_id ) AS DD ON DD.patient_id = p.patnum "
            + "  WHERE p.PatNum<> ''  AND p.lname<> '' and ((p.FName<> '' OR p.lname<> '') and p.patstatus<> 4) and(p.clinicnum = @Clinic_Number);";


        #endregion

        public static string GetOpenDentalPatientDocData = " SELECT p.PatNum AS Patient_EHR_ID,FName AS First_name,LName AS Last_name,ImageFolder,clinicnum as Clinic_Number "
                                                     + " FROM patient p";

        public static string GetOpenDentalPatientImagesData = @" 
select
0 as Patient_Images_LocalDB_ID,
'' as Patient_Images_Web_ID,
d.docnum as Patient_Images_EHR_ID,
d.patnum as Patient_EHR_ID,
'' as Patient_Web_ID,
'' as Image_EHR_Name,
CONCAT_WS('\\\\',(case when d.cropX>0 or d.cropY>0 then CONCAT(p.imagefolder,'\\\\','Thumbnails') else p.imagefolder end),(case when d.cropX>0 or d.cropY>0 then REPLACE(d.FileName,CONCAT('.',SUBSTRING_INDEX(d.FileName, '.', -1)),CONCAT('_100',CONCAT('.',SUBSTRING_INDEX(d.FileName, '.', -1)))) else d.FileName end )) as Patient_Images_FilePath,
0 as Is_Deleted,
0 as Is_Adit_Updated,
NOW() as Entry_DateTime,
NOW() as AditApp_Entry_DateTime,
p.clinicnum as Clinic_Number,
0 as Service_Install_Id
from document as d
inner join (select max(datetstamp) as datetstamp,patnum from document d Where DocCategory = 190 and imgtype = 2 group by patnum) as a on a.datetstamp = d.datetstamp and a.patnum = d.patnum 
inner join patient p on p.patnum = d.patnum
";

        public static string GetPWPatientImagesData = @" 
select
0 as Patient_Images_LocalDB_ID,
'' as Patient_Images_Web_ID,
d.docnum as Patient_Images_EHR_ID,
d.patnum as Patient_EHR_ID,
'' as Patient_Web_ID,
'' as Image_EHR_Name,
CONCAT_WS('\\\\',(case when d.cropX>0 or d.cropY>0 then CONCAT(p.imagefolder,'\\\\','Thumbnails') else p.imagefolder end),(case when d.cropX>0 or d.cropY>0 then REPLACE(d.FileName,CONCAT('.',SUBSTRING_INDEX(d.FileName, '.', -1)),CONCAT('_100',CONCAT('.',SUBSTRING_INDEX(d.FileName, '.', -1)))) else d.FileName end )) as Patient_Images_FilePath,
0 as Is_Deleted,
0 as Is_Adit_Updated,
NOW() as Entry_DateTime,
NOW() as AditApp_Entry_DateTime,
p.clinicnum as Clinic_Number,
0 as Service_Install_Id
from document as d
inner join (select max(datetstamp) as datetstamp,patnum from document d Where DocCategory = (select defnum from definition where category = 18  and itemvalue = 'P' and ishidden = 0 order by defnum limit 1) and imgtype = 2 group by patnum) as a on a.datetstamp = d.datetstamp and a.patnum = d.patnum 
inner join patient p on p.patnum = d.patnum
";

        //public static string GetPatientWisePendingAmount = " SELECT AA.PatNum, IFNULL(AB.PaymentAmount,0) AS Current_Payment, ( AA.TotalBill - IFNULL(AB.PaymentAmount,0) ) AS Remaining_Benefit FROM "
        //                                                 + " ( SELECT SUM(ProcFee) AS TotalBill,PL.PatNum FROM procedurelog PL WHERE PL.APTNum > 0 and procStatus = 2 GROUP BY PL.PatNum ) AS AA "
        //                                                 + " LEFT JOIN ( SELECT SUM(PayAmt) AS PaymentAmount, PatNum FROM payment  GROUP BY PatNum ) AS AB ON AA.PatNum = AB.PatNum ";
        public static string GetPatientWisePendingAmount = " SELECT AA.PatNum,AA.clinicnum as Clinic_Number, IFNULL(AB.PaymentAmount,0) AS Current_Payment, ( AA.TotalBill - IFNULL(AB.PaymentAmount,0) ) AS Remaining_Benefit FROM "
                                                         + " ( SELECT SUM(ProcFee) AS TotalBill,PL.PatNum,PL.clinicnum FROM procedurelog PL WHERE PL.APTNum > 0 and procStatus = 2 GROUP BY PL.PatNum, PL.clinicnum ) AS AA "
                                                         + " LEFT JOIN ( SELECT SUM(PayAmt) AS PaymentAmount, PatNum,clinicnum FROM payment  GROUP BY PatNum,clinicnum ) AS AB ON AA.PatNum = AB.PatNum And AA.clinicnum = AB.clinicnum  where AA.clinicnum = @Clinic_number ;";

        public static string GetOpenDentalPatientID_NameData = "  SELECT p.PatNum AS Patient_EHR_ID,FName AS FirstName,LName AS LastName, CONCAT(FName, ' ', LName) AS Patient_Name, REPLACE(REPLACE(REPLACE(REPLACE(WirelessPhone,'(',''),')',''),'-',''),' ','') AS Mobile, REPLACE(REPLACE(REPLACE(REPLACE(HmPhone,'(',''),')',''),'-',''),' ','') AS Home_Phone, REPLACE(REPLACE(REPLACE(REPLACE(WkPhone,'(',''),')',''),'-',''),' ','') AS Work_Phone,Birthdate AS birth_date, Guarantor,clinicnum  as Clinic_Number,p.Email FROM patient p;";

        public static string GetOpenDentalIdelProvider = " SELECT ProvNum FROM provider where ishidden = 0 Limit 1;";

        public static string GetOpenDentalPatientdue_date = "SELECT r.recallNum, rt.RecallTypeNum, r.DateDue AS due_date, r.patnum AS patient_id, rt.description AS recall_type,p.clinicnum As Clinic_Number "
                                                           + " FROM recall r "
                                                           + " Left Join patient p on p.PatNum = r.PatNum "
                                                           + " JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum;";

        public static string GetOpenDentalPatientWiseRecallDates = "SELECT '' AS Patient_Recall_Id,p.patnum AS Patient_EHR_id,'' AS Patient_Web_ID,r.datedue AS Recall_Date,r.DatePrevious AS Last_Recall_Date, '' AS Provider_EHR_ID,r.RecallTypeNum AS RecallType_EHR_ID,rt.description AS RecallType_Name,'' AS RecallType_Descript,"
                                                             + " 'N' AS Default_Recall,NOW() AS Entry_DateTime,NOW() AS Last_Sync_Date, r.DateTStamp AS EHR_Entry_DateTime,0 AS Is_Deleted,0 AS Is_Adit_Updated,p.clinicnum AS Clinic_Number,@Service_Install_Id As Service_Install_Id,1 AS InsUptDlt"
                                                              + "  FROM recall r  Left Join patient p on p.PatNum = r.PatNum  JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum;";

        public static string GetOpenDentalNextApptDate = "select AptDateTime AS nextvisit_date,PatNum,AptStatus,clinicnum as Clinic_Number From appointment "
                                                       + " Where AptDateTime > @ToDate and clinicnum = @Clinic_number AND AptStatus NOT IN (3,5) Order by AptDateTime desc ;";

        //public static string GetOpenDentalPatientInsBenafit = " SELECT IFNULL(Sum(MonetaryAmt),0) AS TotalBenafit,IFNULL((SELECT Sum(InsPayAmt) AS UsedBenafit FROM claimproc c where patnum = @PatientId),0) AS UsedBenafit FROM benefit b "
        //                                               + " where benefitType = 5 and planNum in (SELECT PlanNum FROM inssub i where subscriber = @PatientId)and CoverageLevel = 1 ;";

        public static string GetOpenDentalPatientInsBenafit = " select patient_ehr_id,SUM(TotalBenafit) AS TotalBenafit,UsedBenafit FROM ( SELECT distinct i.subscriber AS patient_ehr_id,b.benefitNum, IFNULL((b.MonetaryAmt),0) AS TotalBenafit,c.UsedBenafit FROM benefit b inner join inssub i on i.plannum = b.plannum and benefitType = 5 and CoverageLevel = 1 "
                                                            + "  inner join ( select patnum,IFNULL(sum(InsPayAmt),0) AS UsedBenafit from claimproc group by  patnum )  c on c.patnum  = i.subscriber inner join patient p on p.patnum = c.patnum where p.clinicnum = @Clinic_number) AS AA group by AA.patient_ehr_id,AA.UsedBenafit;";

        public static string GetOpenDentalPatientLastVisit_Date = "select AptDateTime AS lastvisit_date,PatNum,AptStatus,clinicnum as Clinic_Number From appointment "
                                                     + " Where AptDateTime < @ToDate  AND AptStatus NOT IN (3,5) Order by AptDateTime desc ;";

        public static string GetOpenDentalRecallTypeData = "SELECT RecallTypeNum AS RecallType_EHR_ID, Description AS RecallType_Name,'' AS Clinic_Number FROM recalltype;";

        public static string GetOpenDentalUserData = "select UserNum as User_EHR_ID,'' AS User_LocalDB_ID,'' as User_web_Id,UserName as First_Name,'' as Last_Name,Password as Password ,DateTLastLogin as EHR_Entry_DateTime,DateTLastLogin as Last_Updated_DateTime,curdate() as LocalDb_EntryDatetime,(case when IsHidden = 0 then 1 else 0 end) as Is_active,0 as is_deleted,0 as Is_Adit_Updated,ClinicNum as Clinic_Number,0 as Service_Install_Id from userod;";
        public static string GetOpenDentalUserData_15_4 = "select UserNum as User_EHR_ID,'' AS User_LocalDB_ID,'' as User_web_Id,UserName as First_Name,'' as Last_Name,Password as Password ,curdate() as EHR_Entry_DateTime,curdate() as Last_Updated_DateTime,curdate() as LocalDb_EntryDatetime,(case when IsHidden = 0 then 1 else 0 end) as Is_active,0 as is_deleted,0 as Is_Adit_Updated,ClinicNum as Clinic_Number,0 as Service_Install_Id from userod";
        public static string GetOpenDentalAppointmentStatus = "SELECT DefNum AS ApptStatus_EHR_ID, ItemName AS ApptStatus_Name,  (CASE WHEN IFNULL(Category,0) = 2 THEN 'confirm' ELSE 'unshedule' END) AS ApptStatus_Type,'' As Clinic_Number  FROM definition Where Category = 13 OR Category = 2 ;";

        public static string InsertPatientDetails = " INSERT INTO patient (LName, FName, Middlei, WirelessPhone, Email, PriProv, DateFirstVisit, SchedBeforeTime,SchedAfterTime, Guarantor,clinicnum, SecUserNumEntry) "
                                                  + " VALUES (@LName, @FName, @Middlei, @WirelessPhone, @Email, @PriProv, @DateFirstVisit, @SchedBeforeTime, @SchedAfterTime, @Guarantor,@clinicnum,@EHR_User_ID) ;";

        public static string InsertPatientDetails_With_BirthDate = " INSERT INTO patient (LName, FName, Middlei, WirelessPhone, Email, PriProv, DateFirstVisit, SchedBeforeTime,SchedAfterTime, Guarantor, birthdate,clinicnum, SecUserNumEntry) "
                                              + " VALUES (@LName, @FName, @Middlei, @WirelessPhone, @Email, @PriProv, @DateFirstVisit, @SchedBeforeTime, @SchedAfterTime, @Guarantor, @Birth_Date,@clinicnum,@EHR_User_ID);";

        public static string InsertPatientDetails_15_4 = " INSERT INTO patient (LName, FName, Middlei, WirelessPhone, Email, PriProv, DateFirstVisit, SchedBeforeTime,SchedAfterTime, Guarantor,clinicnum) "
                                                  + " VALUES (@LName, @FName, @Middlei, @WirelessPhone, @Email, @PriProv, @DateFirstVisit, @SchedBeforeTime, @SchedAfterTime, @Guarantor,@clinicnum) ;";

        public static string InsertPatientDetails_With_BirthDate_15_4 = " INSERT INTO patient (LName, FName, Middlei, WirelessPhone, Email, PriProv, DateFirstVisit, SchedBeforeTime,SchedAfterTime, Guarantor, birthdate,clinicnum) "
                                              + " VALUES (@LName, @FName, @Middlei, @WirelessPhone, @Email, @PriProv, @DateFirstVisit, @SchedBeforeTime, @SchedAfterTime, @Guarantor, @Birth_Date,@clinicnum);";



        public static string UpdatePatientGuarantorID = " Update patient SET Guarantor = @Guarantor WHERE PatNum = @PatNum ;";

        public static string InsertAppointmentDetails = " INSERT INTO Appointment (PatNum, AptStatus, Pattern, Confirmed, Op, ProvNum, AptDateTime, IsNewPatient, "
                                                  + " DateTStamp, AppointmentTypeNum, DateTimeArrived, DateTimeSeated, DateTimeDismissed,Note,ProcDescript,ProcsColored,clinicnum,SecUserNumEntry) "
                                                  + " VALUES (@PatNum, @AptStatus, @Pattern, @Confirmed, @Op, @ProvNum, @AptDateTime, @IsNewPatient, "
                                                  + " @DateTStamp, @AppointmentTypeNum, @DateTimeArrived, @DateTimeSeated, @DateTimeDismissed, @apptcomment,@ProcDescription,@ProcsColored,@clinicnum,@EHR_User_ID) ;";

        public static string InsertAppointmentDetails_15_4 = " INSERT INTO Appointment (PatNum, AptStatus, Pattern, Confirmed, Op, ProvNum, AptDateTime, IsNewPatient, "
                                                + " DateTStamp, AppointmentTypeNum, DateTimeArrived, DateTimeSeated, DateTimeDismissed,Note,ProcDescript,clinicnum) "
                                                + " VALUES (@PatNum, @AptStatus, @Pattern, @Confirmed, @Op, @ProvNum, @AptDateTime, @IsNewPatient, "
                                                + " @DateTStamp, @AppointmentTypeNum, @DateTimeArrived, @DateTimeSeated, @DateTimeDismissed, @apptcomment,@ProcDescription,@clinicnum) ;";

        public static string InsertHistAppointmentDetails = "insert into histappointment(HistUserNum,HistDateTStamp,HistApptAction,ApptSource,AptNum,PatNum,AptStatus,Pattern,"
                                                 + "Confirmed,TimeLocked,Op,Note,ProvNum,ProvHyg,AptDateTime,NextAptNum,UnschedStatus,IsNewPatient,ProcDescript,Assistant,"
                                                 + "ClinicNum, IsHygiene, DateTStamp,DateTimeArrived,DateTimeSeated,DateTimeDismissed,InsPlan1,InsPlan2,DateTimeAskedToArrive,ProcsColored,ColorOverride,AppointmentTypeNum,SecUserNumEntry,SecDateTEntry,Priority,ProvBarText,PatternSecondary) values(@EHR_User_ID,curdate(),1,0,@AptNum,@PatNum,@AptStatus,@Pattern,@Confirmed,0,@Op,'',@ProvNum,0,@AptDateTime,"
                                                 + "0,0,@IsNewPatient,@ProcDescription,0,@clinicnum,0,@DateTStamp,curdate(),curdate(),curdate(),0,0,curdate(),'',0,@AppointmentTypeNum,@EHR_User_ID,curdate(),0,'','');";

        public static string InsertHistAppointmentDetails_15_4 = "insert into histappointment(HistDateTStamp,HistApptAction,ApptSource,AptNum,PatNum,AptStatus,Pattern,"
                                                + "Confirmed,TimeLocked,Op,Note,ProvNum,ProvHyg,AptDateTime,NextAptNum,UnschedStatus,IsNewPatient,ProcDescript,Assistant,"
                                                + "ClinicNum, IsHygiene, DateTStamp,DateTimeArrived,DateTimeSeated,DateTimeDismissed,InsPlan1,InsPlan2,DateTimeAskedToArrive,ProcsColored,ColorOverride,AppointmentTypeNum,SecUserNumEntry,SecDateTEntry,Priority,ProvBarText,PatternSecondary) values(curdate(),1,0,@AptNum,@PatNum,@AptStatus,@Pattern,@Confirmed,0,@Op,'',@ProvNum,0,@AptDateTime,"
                                                + "0,0,@IsNewPatient,@ProcDescription,0,@clinicnum,0,@DateTStamp,curdate(),curdate(),curdate(),0,0,curdate(),'',0,@AppointmentTypeNum,curdate(),0,'','');";

        public static string InsertRecallEntryForAppt = "insert into recall(Patnum,DateDueCalc,DateDue,DatePrevious,Note,RecallInterval,DateTstamp,RecallTypeNum,RecallStatus,IsDisabled,DisableUntilBalance,DisableUntilDate,DateScheduled,priority,TimePatternOverride) values(@PatNum,curdate(),curdate(),curdate(),'',(select defaultinterval from recalltype limit 1),curdate(),(select recalltypenum from recalltype limit 1),0,0,0,curdate(),curdate(),0,'')";

        public static string InsertSecuritylogEntryforAppt = "insert into securitylog(PermType,usernum,logdatetime,logtext,patnum,CompName,Fkey,LogSource,DefNum,DefNumError,DateTPrevious) values(0,@EHR_User_ID,curdate(),'',@PatNum,'',0,0,0,0,curdate())";

        public static string InsertSecuritylogEntryforAppt_15_4 = "insert into securitylog(logdatetime,logtext,patnum,CompName,Fkey,LogSource,DefNum,DefNumError,DateTPrevious) values(curdate(),'',@PatNum,,'',0,0,0,0,curdate())";

        public static string InsertSecuritylogHashEntryForAppt = "insert into securityloghash(Securitylognum,LogHash) values((select securitylognum from securitylog where patnum = @PatNum order by 1 desc limit 1),'')";

        public static string InsertSignalodEntryForAppt = "insert into signalod(DateViewing,SigDateTime,Fkey,FkeyType,IType,RemoteRole,MsgValue) values(curdate(),curdate(),0,'Undefined',0,0,'')";

        public static string InsertProcedureLog = "INSERT INTO Procedurelog (PatNum, AptNum, ProcDate, ProcFee, ProcStatus, ProvNum, DateEntryC, ClinicNum, CodeNum, DateTP, DateTStamp, SecDateEntry,CodeMod1,CodeMod2,CodeMod3,CodeMod4,RevCode,UnitQty,icdVersion,SecUserNumEntry)"
                                + "VALUES(@PatNum, @AptNum, @ProcDate, IFNULL((select Amount from Fee where CodeNum = @CodeNum), 0), 1, @ProvNum, @DateEntryC, @ClinicNum, @CodeNum, @DateTP, @DateTStamp, @SecDateEntry,'','','','','',1,9,@EHR_User_ID);";

        public static string InsertProcedureLog_15_4 = "INSERT INTO Procedurelog (PatNum, AptNum, ProcDate, ProcFee, ProcStatus, ProvNum, DateEntryC, ClinicNum, CodeNum, DateTP, DateTStamp,CodeMod1,CodeMod2,CodeMod3,CodeMod4,RevCode,UnitQty,icdVersion)"
                               + "VALUES(@PatNum, @AptNum, @ProcDate, IFNULL((select Amount from Fee where CodeNum = @CodeNum), 0), 1, @ProvNum, @DateEntryC, @ClinicNum, @CodeNum, @DateTP, @DateTStamp,'','','','','',1,9);";


        public static string GetBookOperatoryAppointmenetWiseDateTime = " Select AptNum, OP, AptDateTime, CHAR_LENGTH(Pattern)*5 AS ApptMin,IFNull(a.clinicnum,o.clinicnum) as Clinic_Number,p.fname AS FirstName, p.lname AS LastName,p.WirelessPhone AS Mobile,p.Email,PP.FName AS ProviderFirstName,PP.LName AS ProviderLastName  FROM appointment a LEFT JOIN operatory o ON a.OP = o.OperatoryNum LEFT JOIn patient p on p.PatNum = a.patnum LEFT JOIN provider PP ON PP.ProvNum = a.ProvNum  "
                                                                   + " WHERE cast( a.AptDateTime  as date) = @ToDate and IFNull(a.clinicnum,o.clinicnum) = @Clinic_Number ;";

        public static string GetOpenDentalClinic = "SELECT 0 AS Clinic_Number,'head Quarters' As Description Union All SELECT ClinicNum AS Clinic_Number, Description FROM clinic ;";

        public static string Update_Status_EHR_Appointment_Live_To_Opendental = " UPDATE appointment SET Confirmed = @AptStatus, DateTStamp = NOW()  "
                                       + " WHERE AptNum = @Appt_EHR_ID ";

        public static string Update_Receive_SMS_Patient_EHR_Live_To_Opendental = " UPDATE patient SET TxtMsgOk = @TxtMsgOk WHERE patNum = @patient_ehr_id; ";

        public static string Insert_paitent_insurance = " insert into inssub (plannum,subscriber,SubscriberID, SecUserNumEntry) values (@plannum,@subscriber,@SubscriberID,@EHR_User_ID); ";

        public static string Insert_paitent_insurance_15_4 = " insert into inssub (plannum,subscriber,SubscriberID) values (@plannum,@subscriber,@SubscriberID); ";


        public static string Insert_paitent_insurance_patplan = " insert into patplan (patnum,ordinal,inssubnum)values(@patnum,@ordinal,@inssubnum); ";

        public static string Update_paitent_insurance = " Update inssub Set plannum = @plannum, SubscriberID =@SubscriberID Where insSubNum = @insSubNum";

        public static string Update_paitent_insurance_patplan = " Update patplan Set inssubnum = @inssubnum Where patplannum = @patplannum; ";

        public static string Update_Patinet_Record_By_Patient_Form = " UPDATE patient SET ColumnName = @ehrfield_value " + " WHERE PatNum = @Patient_EHR_ID ";

        public static string Update_PatientEmergencyC_By_Patient_Form = "update patientnote set ColumnName = @ehrfield_value " + " where PatNum =@Patient_EHR_ID ";

        public static string Get_Employer_Record_For_Patient_Form = "Select EmployerNum from patient where  PatNum =@Patient_EHR_ID)";

        public static string InsertPatientDocData = " Insert Into document (Description,DateCreated,DocCategory,PatNum,FileName,ImgType,ToothNumbers,DateTStamp) "
                                      + " Values(@Description, NOW(), @DocCategory, @PatNum,@FileName, @ImgType, @ToothNumbers, NOW());";

        public static string UpdatePatientDocFileName = " Update document Set FileName = @FileName Where DocNum = @DocNum ;";

        public static string UpdatePatientFileName = " Update patient Set ImageFolder = @ImageFolder Where PatNum = @PatNum ;";

        public static string GetOpenDentalDocPath = " SELECT * FROM preference Where prefName = 'DocPath' ;";

        public static string InsertUserLogin_ID = " INSERT INTO userod (usernum, username, password, usergroupnum, employeenum, clinicnum, provnum, ishidden, tasklistinbox, "
                                                   + "anesthprovtype, defaulthidepopups, passwordisstrong, clinicisrestricted, inboxhidepopups, usernumcemt, datetfail, failedattempts, "
                                                   + "domainuser, ispasswordresetrequired, mobilewebpin, mobilewebpinfailedattempts, datetlastlogin) "
                                                   + "SELECT * FROM (SELECT (SELECT MAX( usernum ) FROM userod) +1 as usernum, @username as username, 'SHA3_512$pOL0tPvgXAxnwLDmuN1lOMBx1Y6urCtfqSeNW+lG7lCDCTyC+Ev2OZhe+eScgpZZGXBNkYY25JWm0R3otuqHTg==$Gx4w8eRHTJ6q1ximO3LPguL66Qc3+Wu0jDWsw/sf3MgdtXA1O/BvHX8FuwNA2lhoydGy34SazeHV7EQxThh0sQ==' as password, '1' as usergroupnum, '0' as employeenum, "
                                                   + "@clinicnum as clinicnum, '0' as provnum,'0' as ishidden, '0' as tasklistinbox, '3' as anesthprovtype, "
                                                   + "'0' as defaulthidepopups, '0' as passwordisstrong, '0' as clinicisrestricted, '0' as inboxhidepopups, "
                                                   + "'0' as usernumcemt, '0001-01-01 00:00:00' as datetfail, '0' as failedattempts, '' as domainuser, '0' as ispasswordresetrequired, "
                                                   + " '' as mobilewebpin, '0' as mobilewebpinfailedattempts,@lastlogindatetime as datetlastlogin) AS tmp"
                                                   + " WHERE NOT EXISTS (SELECT usernum from userod where username = 'Adit') LIMIT 1;";

        public static string InsertUserLogin_ID_15_4 = "INSERT INTO userod (usernum, username, password, usergroupnum, employeenum, clinicnum,"
                                                    + "provnum, ishidden, tasklistinbox, anesthprovtype, defaulthidepopups, passwordisstrong, clinicisrestricted, inboxhidepopups,usernumcemt)"
                                                    + "SELECT * FROM(SELECT (SELECT MAX(usernum ) FROM userod) +1 as usernum, 'Adit' as username,"
                                                    + "'NfBcvlJIj3ubzQ8+yPT9Ww==' as password,'1' as usergroupnum, '0' as employeenum,0 as clinicnum, '0' as provnum,'0' as ishidden, '0' as tasklistinbox, '3' as anesthprovtype, '0' as defaulthidepopups,'0' as passwordisstrong, '0' as clinicisrestricted, '0' as inboxhidepopups, '0' as usernumcemt) AS tmp "
                                                    + "WHERE NOT EXISTS(SELECT usernum from userod where username = 'Adit') LIMIT 1;";


        public static string InsertAdit_Usergroup = " INSERT INTO usergroupattach (userNum, userGroupNum) "
                                                  + " SELECT * FROM (SELECT @usernum as userNum, '1' as userGroupNum) AS tmp "
                                                  + " WHERE NOT EXISTS (SELECT userNum FROM usergroupattach WHERE userNum = @usernum) LIMIT 1;";

        public static string InsertAdit_Userodpref = " INSERT INTO userodpref (usernum,fkey,fkeyType,ValueString,clinicNum) "
                                                   + " SELECT * FROM (SELECT @usernum as userNum, '1' as fkey, '3' as fkeyType, '' as ValueString, @clinicnum as clinicNum) AS tmp "
                                                   + " WHERE NOT EXISTS (SELECT userNum from userodpref where userNum = @usernum) LIMIT 1;";

        public static string GetOpenDentalPatientDiseaseData = "select d.patnum as Patient_EHR_ID,d.DiseaseDefNum as Disease_EHR_ID,DiseaseName as Disease_Name,'P' as Disease_Type,d.DateTStamp AS EHR_Entry_DateTime , "
                                                             + " case when d.probstatus = 2 or ifnull(IsHidden,0) = 1 then 1 else 0 end AS is_deleted,p.clinicnum Clinic_Number from disease as d left join diseasedef df on df.DiseaseDefNum = d.DiseaseDefNum left join patient p on p.patnum = d.patnum "
                                                             + " Union All "
                                                             + " select d.patnum as Patient_EHR_ID,d.allergydefNum as Disease_EHR_ID,Description as Disease_Name,'A' as Disease_Type,d.DateTStamp AS EHR_Entry_DateTime , case when d.StatusIsActive = 0 or ifnull(IsHidden,0) = 1 then 1 else 0 end AS is_deleted,p.clinicnum  from allergy as d left join allergydef df on df.allergydefNum = d.allergydefNum left join patient p on p.patnum = d.patnum ;";
        public static string GetOpenDentalDiseaseData = " SELECT DiseaseDefNum AS Disease_EHR_ID ,DiseaseName AS Disease_Name, "
                                                      + " ifnull(IsHidden,0) AS is_deleted,DateTStamp AS EHR_Entry_DateTime ,'P' AS Disease_Type ,'' Clinic_Number "
                                                      + " FROM diseasedef d "
                                                      + " Union All "
                                                      + " SELECT AllergyDefNum AS Disease_EHR_ID ,Description AS Disease_Name, "
                                                      + " ifnull(IsHidden,0) AS is_deleted,DateTStamp AS EHR_Entry_DateTime ,'A' AS Disease_Type ,'' Clinic_Number "
                                                      + " FROM allergydef; ";
        //public static string GetOpenDentalMedicationData = " SELECT medicationnum as Medication_EHR_ID,case when medicationnum = genericnum then medname else CONCAT(medname ,'(',(select medname from medication where medicationnum = "
        //                                              + " m.genericnum) ,')') end as Medication_Name,case when medicationnum = genericnum then 'Drug' else 'Brand' end as Medication_Type , '' as Drug_Quantity, "
        //                                              + " datetstamp as EHR_Entry_DateTime,0 as is_deleted, 0 as Is_Adit_Updated ,'' Clinic_Number FROM medication m;";

        public static string GetOpenDentalMedicationDataNew = "SELECT medicationnum as Medication_EHR_ID, " +
                                                        "case when medicationnum = genericnum then medname else CONCAT(medname ,'(', (select medname from medication where medicationnum = m.genericnum) ,')') end as Medication_Name, " +
                                                        "'' Medication_Description, Notes as Medication_Notes,'' as Medication_Sig, " +
                                                        "genericnum as Medication_Parent_EHR_ID, " +
                                                        "case when medicationnum = genericnum then 'Drug' else 'Brand' end as Medication_Type , '' as Allow_Generic_Sub," +
                                                        "'' as Drug_Quantity,'' as Refills, 'TRUE' as Is_Active, " +
                                                        "datetstamp as EHR_Entry_DateTime, '' as Medication_Provider_ID," +
                                                        "0 as is_deleted, " +
                                                        "0 as Is_Adit_Updated , " +
                                                        "0 as Is_Adit_Updated , " +
                                                        "'' Clinic_Number FROM medication m where (medname is not null and medname <> ''); ";

        public static string GetOpenDentalPatientMedicationData = "select mp.patnum as Patient_EHR_ID,mp.medicationnum as Medication_EHR_ID,medicationpatnum as PatientMedication_EHR_ID,'' as Drug_Quantity,"
                                                      + " case when m.medicationnum =  m.genericnum then  m.medname else CONCAT(m.medname,'(',(select medname from medication where medicationnum = m.genericnum),')') End as Medication_Name,"
                                                      + " case when mp.medicationnum = genericnum then 'Drug' else 'Brand' end as Medication_Type,"
                                                      + " meddescript as Medication_Note,provnum as Provider_EHR_ID ,mp.datestart as Start_Date,mp.datestop as End_Date ,mp.DateTStamp AS EHR_Entry_DateTime , PatNote as Patient_Notes, 0 AS is_deleted,p.clinicnum Clinic_Number, "
                                                      + " case when (datestop >= curdate() or datestop = '0001-01-01') then 'True' else 'False' end as is_active "
                                                      + " from medicationpat as mp left join "
                                                      + " medication as m on m.medicationnum = mp.medicationnum left join patient p on p.patnum = mp.patnum";

        public static string GetOpenDentalMedicalHistoryForm = " select @Service_Install_Id AS Service_Install_Id ,'' Clinic_Number,'' AS SheetDefNum_LocalDB_ID, SheetDefNum AS SheetDefNum_EHR_ID,'' AS SheetDefNum_Web_ID,Description AS Sheet_Name,FontSize,FontName,Width,Height,IsLandscape,PageCount,IsMultiPage,now() AS EHR_Entry_DateTime,now() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated FROM sheetdef s;";

        public static string GetOpenDentalMedicalHistoryFields_15_4 = " select @Service_Install_Id AS Service_Install_Id ,'' Clinic_Number,'' AS SheetFieldDefNum_LocalDB_ID, sfd.sheetfielddefnum AS SheetFieldDefNum_EHR_ID,'' AS SheetFieldDefNum_Web_ID,sfd.Sheetdefnum AS SheetDefNum_EHR_ID,sfd.FieldType,sfd.FieldName,sfd.fieldvalue,sfd.fontsize,sfd.FontName,sfd.Fontisbold,sfd.xpos,sfd.ypos,sfd.width,sfd.height,sfd.growthbehavior,sfd.radiobuttonvalue,sfd.RadioButtonGroup, sfd.isrequired,sfd.taborder,sfd.ReportableName,sfd.TextAlign,sfd.ItemColor,s.SheetType,now() AS EHR_Entry_DateTime,now() AS Last_Sync_Date,0 AS is_deleted, 0 AS Is_Adit_Updated FROM sheetfielddef sfd inner join sheetdef s on sfd.sheetdefnum = s.sheetdefnum WHERE sfd.fieldtype IN (1,2,6,7,8,13);";

        public static string GetOpenDentalMedicalHistoryFields_17_Plus = " select @Service_Install_Id AS Service_Install_Id ,'' Clinic_Number,'' AS SheetFieldDefNum_LocalDB_ID, sfd.sheetfielddefnum AS SheetFieldDefNum_EHR_ID,'' AS SheetFieldDefNum_Web_ID,sfd.Sheetdefnum AS SheetDefNum_EHR_ID,sfd.FieldType,sfd.FieldName,sfd.fieldvalue,sfd.fontsize,sfd.FontName,sfd.Fontisbold,sfd.xpos,sfd.ypos,sfd.width,sfd.height,sfd.growthbehavior,sfd.radiobuttonvalue,sfd.RadioButtonGroup,sfd.isrequired,sfd.taborder,sfd.ReportableName,sfd.TextAlign,sfd.ItemColor,s.SheetType,IsLocked ,TabOrderMobile,UiLabelMobile,UiLabelMobileRadioButton,now() AS EHR_Entry_DateTime ,now() AS Last_Sync_Date,0 AS is_deleted, 0 AS Is_Adit_Updated FROM sheetfielddef sfd inner join sheetdef s on sfd.sheetdefnum = s.sheetdefnum WHERE sfd.fieldtype IN (1,2,6,7,8,13);";

        public static string InsertODPatMedicleResponseData = "Insert into sheet (SheetType,PatNum,DateTimeSheet,FontSize,FontName,Width,Height,IsLandScape,Description,ShowInTerminal,IsWebForm,IsMultiPage,IsDeleted,SheetDefnum,DateTSheetEdited,ClinicNum)"
                                                      + " values (@SheetType,@PatNum,NOW(),@FontSize,@FontName,@Width,@Height,@IsLandScape,@Description,0,0,@IsMultiPage,0,@SheetDefnum,NOW(),@ClinicNum)";

        //public static string InsertODPatMedicleResponseData_15_4 = "Insert into sheet (SheetType,PatNum,DateTimeSheet,FontSize,FontName,Width,Height,IsLandScape,Description,ShowInTerminal,IsWebForm,IsMultiPage,clinicnum)"
        //                                              + " values (@SheetType,@PatNum,NOW(),@FontSize,@FontName,@Width,@Height,@IsLandScape,@Description,0,0,@IsMultiPage,@clinicnum)";
        public static string InsertODPatMedicleResponseData_15_4 = "Insert into sheet (SheetType,PatNum,DateTimeSheet,FontSize,FontName,Width,Height,IsLandScape,Description,ShowInTerminal,IsWebForm,IsMultiPage)"
                                                      + " values (@SheetType,@PatNum,NOW(),@FontSize,@FontName,@Width,@Height,@IsLandScape,@Description,0,0,@IsMultiPage)";

        public static string InsertODPatMedicleQuestionResponseData_15_4 = "Insert into sheetfield (SheetNum,FieldType,FieldName,FieldValue,FontSize,FontName,FontIsBold,XPos,YPos,Width,Height,GrowthBehavior,RadioButtonValue,RadioButtonGroup,IsRequired,TabOrder,ReportableName,TextAlign,ItemColor) "
                                                      + " values (@SheetNum,@FieldType,@FieldName,@FieldValue,@FontSize,@FontName,@FontIsBold,@XPos,@YPos,@Width,@Height,@GrowthBehavior,@RadioButtonValue,@RadioButtonGroup,@IsRequired,@TabOrder,@ReportableName,@TextAlign,@ItemColor) ";

        public static string InsertODPatMedicleQuestionResponseData_17_Plus = "Insert into sheetfield (SheetNum,FieldType,FieldName,FieldValue,FontSize,FontName,FontIsBold,XPos,YPos,Width,Height,GrowthBehavior,RadioButtonValue,RadioButtonGroup,IsRequired,TabOrder,ReportableName,TextAlign,ItemColor,IsLocked ,TabOrderMobile,UiLabelMobile,UiLabelMobileRadioButton) "
                                                      + " values (@SheetNum,@FieldType,@FieldName,@FieldValue,@FontSize,@FontName,@FontIsBold,@XPos,@YPos,@Width,@Height,@GrowthBehavior,@RadioButtonValue,@RadioButtonGroup,@IsRequired,@TabOrder,@ReportableName,@TextAlign,@ItemColor,@IsLocked ,@TabOrderMobile,@UiLabelMobile,@UiLabelMobileRadioButton) ";

        public static string InsertPatientProblems = "INSERT INTO disease (PatNum, DiseaseDefNum, patnote, dateTStamp, probstatus, datestart, datestop, snomedproblemtype,functionstatus) "
                                                        + " SELECT * FROM (SELECT @PatNum as PatNum,@DiseaseDefNum as DiseaseDefNum,'' as patnote,@dateTStamp as dateTStamp, @probstatus as probstatus,@datestart as datestart, @datestop as datestop,'Web' as snomedproblemtype,0 as functionstatus) AS tmp "
                                                        + " WHERE NOT EXISTS ( SELECT PatNum FROM disease WHERE PatNum = @PatNum and DiseaseDefNum = @DiseaseDefNum) LIMIT 1; ";

        //public static string InsertPatientProblems = " INSERT INTO disease (PatNum, DiseaseDefNum, patnote, dateTStamp, probstatus, datestart, datestop, snomedproblemtype,functionstatus) "
        //                                          + " VALUES (@PatNum, @DiseaseDefNum, @patnote, @dateTStamp, @probstatus, @datestart, @datestop, @snomedproblemtype, @functionstatus) ;";

        //public static string InsertPatientAllergies = " INSERT INTO allergy (AllergyDefNum, PatNum, Reaction, StatusIsActive, dateTStamp, dateAdverseReaction, snomedReaction) "
        //                                          + " VALUES (@AllergyDefNum, @PatNum, @Reaction, @StatusIsActive, @dateTStamp, @dateAdverseReaction, @snomedReaction) ;";

        public static string InsertPatientAllergies = " INSERT INTO allergy (AllergyDefNum, PatNum, Reaction, StatusIsActive, dateTStamp, dateAdverseReaction, snomedReaction) "
                                                  + " SELECT * FROM (SELECT @AllergyDefNum as AllergyDefNum,@PatNum as PatNum,'' as Reaction,@StatusIsActive as StatusIsActive,@dateTStamp as dateTStamp, @dateAdverseReaction as dateAdverseReaction,'Web' as snomedReaction) AS tmp "
                                                  + " WHERE NOT EXISTS ( SELECT PatNum FROM allergy WHERE PatNum = @PatNum and AllergyDefNum = @AllergyDefNum) LIMIT 1; ";


        public static string DeletePatientProblems = " delete from disease where DiseaseDefNum = @DiseaseDefNum and  PatNum = @PatNum ;";
        public static string DeletePatientAllergies = " delete from allergy where AllergyDefNum = @AllergyDefNum and  PatNum = @PatNum ;";

        public static string checkAlreadyExistsPayment = "SELECT IF( EXISTS( select 1  From payment  where paytype = @paytype and paydate = @paydate and payamt = @payamt and patnum =  @patnum and  clinicnum = @clinicnum ), 1, 0)";

        public static string checkAlreadyExistsSplitPayment = "SELECT IF( EXISTS( select 1  From paysplit  where datePay = @paydate and splitamt = @splitamt and patnum =  @patnum and  clinicnum = @clinicnum AND PayNum = @paynum ), 1, 0)";

        public static string InsertPatientPayment = " Insert into payment ( paytype,paydate,payamt,checknum,bankbranch,paynote,issplit,patnum,clinicnum,dateentry,depositnum,isrecurringcc,SecUserNumEntry) "
                                                  + " values (@paytype,@paydate,@payamt,@checknum,@bankbranch,@paynote,0,@patnum,@clinicnum,curdate(),0,0,@EHR_User_ID) ;";

        public static string checkdiscountPayment = "SELECT IF( EXISTS( select 1  From Adjustment  where AdjDate = @PaymentDate and AdjAmt = @Amount and PatNum = @PatientId and  ClinicNum = @clinicNumber AND AdjNote = @Note ), 1, 0)";

        public static string InsertPatientDiscount = "Insert into Adjustment(AdjDate, AdjAmt, PatNum, AdjType, ProvNum, AdjNote, ProcDate, ProcNum, DateEntry, ClinicNum, StatementNum, SecUserNumEntry) values" +
                                                     "(@PaymentDate, @Amount, @PatientId, @discountMode, @ProviderId, @Note, @PaymentDate,0, @PaymentDate, @clinicNumber, 0, @EHR_User_ID)";

        public static string InsertPatientPayment_15_4 = " Insert into payment ( paytype,paydate,payamt,checknum,bankbranch,paynote,issplit,patnum,clinicnum,dateentry,depositnum,isrecurringcc) "
                                                 + " values (@paytype,@paydate,@payamt,@checknum,@bankbranch,@paynote,0,@patnum,@clinicnum,curdate(),0,0) ;";

        public static string InsertPatientDiscount_15_4 = "Insert into Adjustment(AdjDate, AdjAmt, PatNum, AdjType, ProvNum, AdjNote, ProcDate, ProcNum, DateEntry, ClinicNum, StatementNum) values" +
                                                     "(@PaymentDate, @Amount, @PatientId, @discountMode, @ProviderId, @Note, @PaymentDate,0, @PaymentDate, @clinicNumber, 0)";


        public static string InsertPatientSplitPayment = " insert into Paysplit (splitamt,patnum,procdate,paynum,isdiscount,discounttype,provnum,payplannum,datepay,procnum,dateentry,unearnedtype,clinicnum,SecUserNumEntry) "
                                                  + " values (@splitamt,@patnum,@procdate,@paynum,0,0,@provnum,0,@datepay,0,curdate(),0,@clinicnum,@EHR_User_ID) ;";
        public static string InsertPatientSplitPayment_15_4 = " insert into Paysplit (splitamt,patnum,procdate,paynum,isdiscount,discounttype,provnum,payplannum,datepay,procnum,dateentry,unearnedtype,clinicnum) "
                                                 + " values (@splitamt,@patnum,@procdate,@paynum,0,0,@provnum,0,@datepay,0,curdate(),0,@clinicnum) ;";


        public static string InsertPatientPaymentLog = "Insert INTO commlog (PatNum,commdatetime,commtype,Note,Mode_,SentorReceived,UserNum,SigIsTopaz,DateTstamp,DateTimeEnd,Commsource,ProgramNum) VALUES "
                                                        + " (@PatientEHRId,@CommentDate,@CommTyep,@Note,@Mode,@SentOrReceive,@EHR_User_ID,0,curdate(),'0001-01-01 00:00:00',0,0)";


        public static string DeleteDuplicateLogs = " Delete from commlog where PatNum = @PatientEHRId and convert( concat( date(commdatetime),' ',hour(commdatetime),':00:00'),datetime) = @CommentDate and commtype = @CommTyep and Note=@Note and Mode_=@Mode and SentorReceived=@SentOrReceive AND CommlogNum != @NoteId AND commdatetime > '20211115' AND  commtype IN (239) AND ( note like  'SMS sent -%' OR Note like 'SMS received -%' OR Note like 'SMS received -%' OR Note like 'Inbound call answered - Duration:%' ) ";

        public static string DeleteDuplicateLogsWithDate = " Delete from commlog where PatNum = @PatientEHRId AND convert( concat( date(commdatetime),' ',hour(commdatetime),':00:00'),datetime) = @commdatetime and commtype = @CommTyep and Note=@Note and Mode_=@Mode and SentorReceived=@SentOrReceive and CommlogNum != @NoteId AND commdatetime > '20211115' AND  commtype IN (239) AND ( note like  'SMS sent -%' OR Note like 'SMS received -%' OR Note like 'SMS received -%' OR Note like 'Inbound call answered - Duration:%' ) ";

        public static string DeleteDuplicateLogsBlankMobile = " Delete from commlog where PatNum = @PatientEHRId and Note=@Note and Mode_=@Mode and SentorReceived=@SentOrReceive AND commdatetime > '20211115' AND  commtype IN (239) and ( note like  'SMS sent -%' OR Note like 'SMS received -%' OR Note like 'SMS received -%' OR Note like 'Inbound call answered - Duration:%' ) ";

        public static string DeleteDuplicateLogsBlankMobileWithDate = " Delete from commlog where PatNum = @PatientEHRId and Note=@Note and Mode_=@Mode and SentorReceived=@SentOrReceive and convert( concat( date(commdatetime),' ',hour(commdatetime),':00:00'),datetime) = @commdatetime AND commdatetime > '20211115' AND  commtype IN (239) and ( note like  'SMS sent -%' OR Note like 'SMS received -%' OR Note like 'SMS received -%' OR Note like 'Inbound call answered - Duration:%' ) ";

        public static string CheckSMSCallRecords = " select ifnull(MAX(CommLogNum),0) AS NoteId,COUNT(1) AS TotalRecords from commlog where PatNum =  @PatientEHRId and convert( concat( date(commdatetime),' ',hour(commdatetime),':00:00'),datetime) =  @CommentDate and commtype = @CommTyep and Note=@Note and Mode_= @Mode and SentorReceived= @SentOrReceive AND commdatetime > '20211115' Group by PatNum,commdatetime,commtype,Note,Mode_,SentorReceived having COUNT(1) > 1 ";

        public static string CheckSMSCallRecordsBlankMobile = " select ifnull(MAX(CommLogNum),0) AS NoteId,COUNT(1) AS TotalRecords from commlog where PatNum =  @PatientEHRId and commtype = @CommTyep and Note=@Note and Mode_= @Mode and SentorReceived= @SentOrReceive AND commdatetime > '20211115' AND  commtype IN (239) Group by PatNum,commtype,Note,Mode_,SentorReceived having COUNT(1) > 1 ";

        public static string GetDuplicateRecords = " Select AA.*,PT.WirelessPhone AS Mobile FROM ( select  PatNum,convert( concat( date(commdatetime),' ',hour(commdatetime),':00:00'),datetime) AS commdatetime,commtype,Note,Mode_,SentorReceived,SigIsTopaz,DateTimeEnd,Commsource,ProgramNum,MAX(Commlognum) AS LogId ,  count(1) cnt From commlog where commtype IN (239) and commdatetime > '20211115' AND ( note like  'SMS sent -%' OR Note like 'SMS received -%' OR Note like 'SMS received -%' OR Note like 'Inbound call answered - Duration:%' ) group by PatNum,convert( concat( date(commdatetime),' ',hour(commdatetime),':00:00'),datetime),commtype,Note,Mode_,SentorReceived,SigIsTopaz,DateTimeEnd,Commsource,ProgramNum having count(1) > 1 ) AS AA inner join patient PT on PT.Patnum = AA.Patnum where AA.commtype IN (239) Order by AA.cnt desc  ";

        public static string InsertTreatmentPlan = "INSERT INTO treatplan (PatNum,DateTP,Heading,Note,Signature,SigIsTopaz,ResponsParty,DocNum,TPStatus,SecUserNumEntry,SecDateEntry,SecDateTEdit,"
                                                 + "UserNumPresenter,TPType,SignaturePractice, SignatureText,SignaturePracticeText)"
                                                 + "VALUES (@PatID,CURDATE(),'','','',0,@PatID,0,1,@EHR_User_ID,CURDATE(),NOW(),@EHR_User_ID,0,'','','');";

        public static string InsertTreatmentPlan_15_4 = "INSERT INTO treatplan (PatNum,DateTP,Heading,Note,Signature,SigIsTopaz,ResponsParty,DocNum,TPStatus)"
                                                + "VALUES (@PatID,CURDATE(),'','','',0,@PatID,0,1);";

        public static string InsertNewTreatPlanAttach = "INSERT INTO treatplanattach (TreatPlanNum,ProcNum,Priority)"
                                                      + "VALUES (@TreatPlanNum, @ProcNum, 0);";

        public static string GetTreatmentPlanKey = "SELECT MAX(TreatPlanNum) FROM treatplan WHERE PatNum  = @PatID;";

        public static string GetProcNumForProcedureLog = "SELECT ProcNum FROM procedurelog WHERE AptNum = @AptID AND CodeNum = @CodeNum;";

        public static string GetTreatPlanNumByProcNum = "SELECT TreatPlanNum FROM treatplanattach WHERE ProcNum = @ProcNum";

        public static string GetEHRActualVersionOpendental = "select ValueString from preference where PrefName = 'ProgramVersion'";

        public static string updateProcedureLog = " Update procedurelog set AptNum = @AptNum where ProcNum = @ProcNum ";

        //public static string GetPatientMedication = "Select ifNull((Select MedicationPatNum from Medicationpat where PatNum = @Patient_EHR_Id and MedicationNum = @Medication_EHR_ID And ((DateStart = '0001-01-01' And DateStop = '0001-01-01') OR DATEDIFF(DateStop, NOw()) > 0)),0) as MedicationNum;";

        public static string InsertPatientMedication = "Insert into medicationpat(PatNum,MedicationNum,PatNote,ProvNum,MedDescript,RxCui,ErxGuid,IsCpoe) Values (@PatNum,@MedicationNum,@PatNote,(Select PriProv from patient where PatNum = @PatNum),'',0,'',0);";

        public static string UpdatePatientMedicationNotes = "Update medicationpat set PatNote = @Medication_Note, MedicationNum = @MedicationNum where MedicationPatNum = @PatientMedication_EHR_ID ";

        //public static string GetMedication = "Select ifNull((Select MedicationNum from Medication where MedName = @MedicationName),0) as MedicationNum;";

        public static string InsertMedication = "Insert into Medication(MedName,GenericNum,Notes,RxCui) Values (@MedicationName,@GenericNum,@MedicationNote,0);";

        public static string DeletePatientMedication = "Delete From MedicationPat where MedicationPatNum = @PatientMedication_EHR_ID";// and PatNum = @Patient_EHR_ID;";

    }
}
