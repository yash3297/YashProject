using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.QRY.Synch
{
    public class SynchEagleSoftQRY
    {
        public static string GetEagleSoftAppointmentData = " select appointment_id as Appt_EHR_ID, first_name as First_name,last_name as Last_name ,middle_initial as MI,salutation, "
                                                         + " status,sex,marital_status,birth_date,email_address as Email ,cell_phone as Mobile_Contact,home_phone as Home_Contact,work_phone, "
                                                         + " address_1 as Address,address_2,city,state as ST,zipcode as Zip,location_id as Operatory_EHR_ID,op.chair_name as Operatory_Name, "
                                                         + " a.start_time AS Appt_DateTime,a.end_time AS Appt_EndDateTime ,a.appointment_notes as [comment],appointment_type_id as ApptType_EHR_ID, "
                                                         + " apt.description as ApptType,case when arrival_status = 4 then Convert(int, '8') else Convert(int,confirmation_status) end as appointment_status_ehr_key, "
                                                         + " ISNULL(date_appointed,start_time) as EHR_Entry_Date,a.patient_id as Patient_EHR_id, classification  , sooner_if_possible As is_asap  "
                                                         + " from appointment as a inner join patient as p on a.patient_id = p.patient_id "
                                                         + " left join chairs as op on a.location_id = op.chair_num "
                                                         + " left join appt_types as apt on apt.type_id = a.appointment_type_id "
                                                         + " where (classification = 1 OR classification = 16 OR classification = 32 OR classification = 64 OR classification = 4) and a.start_time > ?  and (case when arrival_status = 4 then Convert(int, '8') else Convert(int,confirmation_status) end) is not null AND  appointment_type_id  is not null  ";

        public static string GetEagleSoftAppointmentEhrIds = " select appointment_id as Appt_EHR_ID"
                                                         + " from appointment as a inner join patient as p on a.patient_id = p.patient_id "
                                                         + " left join chairs as op on a.location_id = op.chair_num "
                                                         + " left join appt_types as apt on apt.type_id = a.appointment_type_id "
                                                         + " where (classification = 1) and a.start_time > ?  and (case when arrival_status = 4 then Convert(int, '8') else Convert(int,confirmation_status) end) is not null ";

        public static string GetEaglesoftAppointment_Procedures_Data = "      SELECT CONVERT(Varchar(50),Appt.appointment_id) AS appointment_id, "
                                                 + "    (CASE WHEN ISNULL(AD.tooth,'') != '' THEN AD.tooth + '-' ELSE '' END) AS tooth, "
                                                 + "    (CASE WHEN ISNULL(AD.surface,'') != '' THEN AD.surface + '-' ELSE '' END) AS surface, "
                                                 + "    ISNULL(SS.service_code,'') AS service_code, "
                                                 + "    (CASE WHEN ISNULL(SS.ada_code,'') != '' THEN SS.ada_code + ',' ELSE '' END) AS ada_code, "
                                                 + "    (CASE WHEN ISNULL(SS.schedabbr,'') != '' THEN SS.schedabbr + ',' ELSE '' END) AS schedabbr "
                                                 + "    FROM appointment Appt "
                                                 + "    LEFT JOIN appointment_detail AD ON Appt.appointment_id = AD.appt_id "
                                                 + "    LEFT JOIN Services  SS ON AD.service_code = SS.service_code "
                                                 + "    WHERE (Appt.classification = 1 OR Appt.classification = 16 OR Appt.classification = 32 OR Appt.classification = 64 OR classification = 4) "
                                                 + "    AND Appt.start_time > ?  and (case when Appt.arrival_status = 4 then Convert(int, '8') else Convert(int,Appt.confirmation_status) end) is not null ";

        //public static string GetEagleSoftDeletedAppointmentData = "select appointment_id, first_name,last_name,middle_initial,salutation,status,sex,marital_status,birth_date,email_address"
        //+ ",cell_phone,home_phone,work_phone,address_1,address_2,city,state,zipcode,location_id as op_id,op.description as OperatoryName,a.start_time,a.end_time"
        //+ ",a.appointment_notes as coment,appointment_type_id,apt.description as AppointmentTypeName,Arrival_Status,Confirmation_Status ,'' as Appointment_Status"
        //+ ",date_appointed as EHR_Entry_Date,a.patient_id as Patient_EHR_id from appointment as a inner join patient as p on a.patient_id = p.patient_id "
        //+ "inner join positions as op on a.location_id = op.position_id inner join appt_types as apt on apt.type_id = a.appointment_type_id where classification = 16 and ";


        public static string UpdateEagleSoftConfirmAppointmentData = "Update appointment set confirmation_status = @confirmation_status, date_confirmed = @date_Confirmed where appointment_id = @AppointmentId  INSERT INTO Appt_log (appt_id, old_slot, new_date, appt_log_action, user_id,modified_by) VALUES (@AppointmentId, 1,(SELECT convert(date, start_Time) from appointment where appointment_id = @AppointmentId), 'R',@EHR_User_ID,@EHR_User_ID)";

        public static string UpdateEagleSoftPatientReceive_sms = "Update patient set receives_sms = @receives_sms where patient_id = @patient_id";


        public static string UpdateEagleSoft_Status_MarkAsWalkedOut = "Update appointment set arrival_status = 4, date_confirmed = @date_Confirmed where appointment_id = @AppointmentId  INSERT INTO Appt_log (appt_id, old_slot, new_date, appt_log_action, user_id,modified_by) VALUES (@AppointmentId, 1,(SELECT convert(date, start_Time) from appointment where appointment_id = @AppointmentId), 'R',@EHR_User_ID,@EHR_User_ID)";

        public static string GetEagleSoftAppointmentProviderData = " select  distinct ap.appointment_id, ap.provider_id ,p.first_name + '  ' + p.last_name as ProviderName  "
                                                                 + " from appointment_provider as ap inner join provider as p on p.provider_id = ap.provider_id "
                                                                 + " where ap.appointment_id = ?";

        public static string GetEagleSoftAppointmentProviderDataNew = "select  distinct ap.appointment_id, ap.provider_id ,p.first_name + '  ' + p.last_name as ProviderName " +
                                        " from appointment_provider as ap " +
                                        " inner join provider as p on p.provider_id = ap.provider_id " +
                                        " inner join appointment as a on a.appointment_id = ap.appointment_id " +
                                        " Where a.start_time > '@ToDate'";

        public static string GetEagleSoftOperatoryEventData = "select appointment_id as OE_EHR_ID,location_id as Operatory_EHR_ID,start_time as StartTime,end_time as EndTime,appointment_notes as [comment] from appointment where classification = 2 and start_time > ? ";

        public static string GetEagleSoftOperatoryOfficeHours = "Select Location_id AS Operatory_Id,MIN (mon_start) AS mon_beg,max(mon_end) AS Mon_end,MIN(mon_l_start) AS Mon_l_beg,Max(mon_l_end) AS Mon_l_end, MIN (tue_start) AS tue_beg,max(tue_end) AS tue_end,MIN(tue_l_start) AS Tue_l_beg,Max(Tue_l_end) AS Tue_l_end, MIN (Wed_start) AS Wed_beg,max(Wed_end) AS Wed_end,MIN(Wed_l_start) AS Wed_l_beg,Max(Wed_l_end) AS Wed_l_end, MIN (thu_start) AS thu_beg,max(thu_end) AS thu_end,MIN(thu_l_start) AS thu_l_beg,Max(thu_l_end) AS thu_l_end, MIN (fri_start) AS fri_beg,max(fri_end) AS fri_end,MIN(fri_l_start) AS fri_l_beg,Max(fri_l_end) AS fri_l_end, MIN (sat_start) AS sat_beg,max(sat_end) AS sat_end,MIN(sat_l_start) AS sat_l_beg,Max(sat_l_end) AS sat_l_end, MIN (sun_start) AS sun_beg,max(sun_end) AS sun_end,MIN(sun_l_start) AS sun_l_beg,Max(sun_l_end) AS sun_l_end from Provider_schedule PS inner join provider P on PS.PROVIDER_ID = P.PROVIDER_ID where P.status = 'A'  Group by Location_id";

        public static string GetEagleSoftProviderOfficeHours = " select PH.* From provider_hours PH INNER JOIN Provider P ON PH.Provider_Id = P.Provider_Id ";

        public static string GetEagleSoftProviderHours = " Select 0 As Clinic_Number,@Service_Install_Id AS Service_Install_Id,'' AS PH_LocalDB_Id, convert(varchar, CH.Hours_id) AS PH_EHR_Id, '' AS PH_Web_id,CH.Provider_id AS Provider_EHR_Id,CH.Chair_num AS Operatory_EHR_ID , convert(varchar, CH.occur_on_date,111 ) + ' '+ convert(varchar,CH.start_time) AS StartTIme,convert(varchar, CH.occur_on_date,111 ) + ' '+ convert(varchar,case when lunch_start is not null then CH.Lunch_Start ELSE CH.ENd_time END ) AS EndTime ,R.description AS [comment],'' AS Entry_DateTime,'' AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated From Custom_Hours CH LEFT JOIN recurrance R ON CH.Recurrance_Id = R.Recurrance_Id WHERE CH.occur_on_date >= @ToDate AND CH.start_time IS NOT NULL UNION "
                                                       + " Select 0 As Clinic_Number,@Service_Install_Id AS Service_Install_Id,'' AS PH_LocalDB_Id, convert(varchar, convert(varchar, CH.Hours_id) + '_' + '2' ) AS PH_EHR_Id, '' AS PH_Web_id,CH.Provider_id AS Provider_EHR_Id,CH.Chair_num AS Operatory_EHR_ID , convert(varchar, CH.occur_on_date,111 ) + ' '+ convert(varchar,case when lunch_start is not null then CH.Lunch_ENd ELSE CH.start_time END ) AS StartTIme,convert(varchar, CH.occur_on_date,111 ) + ' '+ convert(varchar,CH.ENd_time) AS EndTime ,R.description AS [comment],'' AS Entry_DateTime,'' AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated From Custom_Hours CH LEFT JOIN recurrance R ON CH.Recurrance_Id = R.Recurrance_Id WHERE CH.occur_on_date >= @ToDate AND CH.start_time IS NOT NULL AND CH.Lunch_start is not null and CH.lunch_end is not null ";

        public static string GetEagleSoftOperatoryChairData = "select MIN( hours_id) as OE_EHR_ID,chair_num as Operatory_EHR_ID,CH.provider_id as Provider_EHR_ID,DATEADD(minute, 1, occur_on_date ) as StartTime,DATEADD(minute, 1439, occur_on_date ) as EndTime,MIN( R.Description) as [comment] ,1 as FullDay  from custom_hours CH Inner join recurrance R ON CH.recurrance_id = R.recurrance_id WHERE R.Hours_start_Time is null AND CH.occur_on_date >= @occur_on_date GROUP BY CH.chair_num,CH.occur_on_date,CH.provider_id  ";
        //public static string GetEagleSoftOperatoryChairData = "select MIN( hours_id) as OE_EHR_ID,chair_num as Operatory_EHR_ID,CH.provider_id as Provider_EHR_ID,DATEADD(minute, 1, occur_on_date ) as StartTime,DATEADD(minute, 1439, occur_on_date ) as EndTime,MIN( R.Description) as [comment] ,1 as FullDay  from custom_hours CH Inner join recurrance R ON CH.recurrance_id = R.recurrance_id INNER JOIN Provider PR ON PR.Provider_id = CH.provider_id WHERE PR.Status = 'A' AND R.Hours_start_Time is null AND CH.occur_on_date >= @occur_on_date GROUP BY CH.chair_num,CH.occur_on_date,CH.provider_id  ";

        //public static string GetEagleSoftOperatoryHours = "Select '' AS OH_LocalDB_ID, CH.Hours_id AS OH_EHR_ID, '' AS OH_Web_ID,CH.Chair_num AS Operatory_EHR_ID , convert(varchar, CH.occur_on_date,111 ) + ' '+ convert(varchar,CH.start_time) AS StartTIme,convert(varchar, CH.occur_on_date,111 ) + ' '+ convert(varchar,CH.ENd_time) AS EndTime ,R.description AS [comment],'' AS Entry_DateTime,'' AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated,Lunch_Start,Lunch_End From Custom_Hours CH LEFT JOIN recurrance R ON CH.Recurrance_Id = R.Recurrance_Id WHERE CH.occur_on_date >= @ToDate  AND CH.start_time IS NOT NULL ";

        public static string GetEagleSoftOperatoryHours = " Select 0 As Clinic_Number,@Service_Install_Id AS Service_Install_Id,'' AS OH_LocalDB_ID, convert(varchar, CH.Hours_id) AS OH_EHR_ID, '' AS OH_Web_ID,CH.Chair_num AS Operatory_EHR_ID , convert(varchar, CH.occur_on_date,111 ) + ' '+ convert(varchar,CH.start_time) AS StartTIme,   convert(varchar,  CH.occur_on_date,111 ) + ' '+ convert(varchar, case when lunch_start is not null then CH.Lunch_Start ELSE CH.ENd_time END ) AS EndTime ,R.description AS [comment],'' AS Entry_DateTime,'' AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated From Custom_Hours CH LEFT JOIN recurrance R ON CH.Recurrance_Id = R.Recurrance_Id WHERE CH.occur_on_date >= @ToDate  AND CH.start_time IS NOT NULL UNION "
                                                        + " Select 0 As Clinic_Number,@Service_Install_Id AS Service_Install_Id,'' AS OH_LocalDB_ID,convert(varchar, convert(varchar, CH.Hours_id) + '_' + '2' )  AS OH_EHR_ID, '' AS OH_Web_ID,CH.Chair_num AS Operatory_EHR_ID , convert(varchar, CH.occur_on_date,111 ) + ' '+ convert(varchar, case when lunch_start is not null then CH.Lunch_ENd ELSE CH.start_time END ) AS StartTIme,convert(varchar, CH.occur_on_date,111 ) + ' '+ convert(varchar,CH.ENd_time) AS EndTime ,R.description AS [comment],'' AS Entry_DateTime,'' AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated From Custom_Hours CH LEFT JOIN recurrance R ON CH.Recurrance_Id = R.Recurrance_Id WHERE CH.occur_on_date >= @ToDate AND CH.start_time IS NOT NULL AND CH.Lunch_start is not null and CH.lunch_end is not null ";
        //    UNION "
        //+ " select MIN( hours_id) as OE_EHR_ID,chair_num as Operatory_EHR_ID,DATEADD(minute, 1, occur_on_date ) as StartTime,CONVERT(DATETIME, CONVERT(CHAR(8),  occur_on_date,112)  + ' ' + CONVERT(CHAR(8), R.Hours_start_Time,108)) as EndTime,MIN( R.Description) as [comment] ,0 as FullDay from custom_hours CH Inner join recurrance R ON CH.recurrance_id = R.recurrance_id WHERE R.Hours_start_Time is NOT null AND CH.occur_on_date >= @occur_on_date  GROUP BY CH.chair_num,CH.occur_on_date ,R.Hours_start_Time UNION  "
        //+ " select MIN( hours_id) as OE_EHR_ID,chair_num as Operatory_EHR_ID,CONVERT(DATETIME, CONVERT(CHAR(8),  occur_on_date,112)  + ' ' + CONVERT(CHAR(8), R.Hours_End_Time,108)) as StartTime,DATEADD(minute, 1439, occur_on_date ) as EndTime,MIN( R.Description) as [comment] ,0 as FullDay from custom_hours CH Inner join recurrance R ON CH.recurrance_id = R.recurrance_id WHERE R.Hours_start_Time is NOT null AND CH.occur_on_date >= @occur_on_date  GROUP BY CH.chair_num,CH.occur_on_date ,R.Hours_End_Time";

        // public static string GetEagleSoftOperatoryChairData = "select MIN( hours_id) as OE_EHR_ID,chair_num as Operatory_EHR_ID,DATEADD(minute, 1, occur_on_date ) as StartTime,DATEADD(minute, 1439, occur_on_date ) as EndTime,MIN( R.Description) as [comment]  from custom_hours CH Inner join recurrance R ON CH.recurrance_id = R.recurrance_id WHERE CH.occur_on_date >= ? GROUP BY CH.chair_num,CH.occur_on_date ORDER BY MIN( hours_id) ";

        public static string GetEagleSoftHolidayData = "SELECT hol_date as SchedDate,hol_name as [comment] from holidays where hol_date >= ? and hol_date <= ?";

        public static string GetEagleSoftHolidayConsiderOnNextDay = "Select option_2 AS 'AllowNextDayHoliday' FROM Scheduler_preferences ";

        public static string GetEagleSoftProviderData = "select provider_id as Provider_EHR_ID,first_name as First_Name,last_name as Last_Name,sex as gender,p.description as provider_speciality, (case when status = 'A' then 1 else 0 end ) AS Is_Active from Provider as h inner join positions as p on p.position_id = h.position_id where trim(first_name) <>'' and trim(last_name) <>'' and p.position_id != 5";

        public static string GetEagleSoftIdelProvider = "select  TOP 1 Provider_id FROM Provider where status = 'A'";

        public static string GetEagleSoftFolderListData = "select group_id as FolderList_EHR_ID,description as Folder_Name from document_group where type = 'I'";


        public static string GetEagleSoftOperatoryData = "select CH.chair_num as Operatory_EHR_ID, Chair_name as Operatory_Name,ISNULL(svc.position,ch.chair_num) as OperatoryOrder from Chairs CH LEFT JOIN scheduler_view_chair SVC ON CH.chair_num = SVC.Chair_num AND SVC.view_id= (select top 1 view_id from scheduler_view order by view_name) and show_chair = 'Y' ";

        public static string GetEaglesoftDeletedOperatoryData = "Select chair_num as Operatory_EHR_ID from chairs where chair_num NOT IN (Select distinct(chair_num)  from scheduler_view_chair where show_chair = 'Y' )";
        //public static string GetEaglesoftDeletedOperatoryData = "select chair_num as Operatory_EHR_ID from Chairs as c  where ((cast((select count(view_id) from scheduler_view ) as int) - 1) = cast((select count(view_id) from scheduler_view_chair where show_chair = 'N' and chair_num = c.chair_num ) as int) ) "
        //                                             + " or chair_num not in (select chair_num from scheduler_view_chair) "; 

        public static string GetEagleSoftApptTypeData = "select type_id AS ApptType_EHR_ID,description as Type_Name from appt_types where active = 1";

        //public static string GetEagleSoftPatientData = " select p.patient_id as Patient_EHR_ID ,p.first_name as First_name,p.last_name as Last_name,p.middle_initial as Middle_Name, "
        //                                             + " p.salutation as Salutation ,p.status as Status,p.sex as Sex,p.marital_status as MaritalStatus,p.birth_date as Birth_Date , "
        //                                             + " p.email_address as Email,p.cell_phone as Mobile , p.home_phone as Home_Phone,p.work_phone as Work_Phone,p.address_1 as Address1, "
        //                                             + " p.address_2 as Address2,p.city as City,p.state as State,p.zipcode as Zipcode, p.responsible_party_status as ResponsibleParty_Status, "
        //                                             // + " p.current_bal as CurrentBal,p.thirty_day as ThirtyDay,p.sixty_day as SixtyDay,p.ninety_day as NinetyDay, "
        //                                             + "  p.current_bal as ThirtyDay,p.thirty_day as SixtyDay,p.sixty_day as NinetyDay,p.ninety_day as Over90, "
        //                                             + " p.first_visit_date as FirstVisit_Date,p.last_date_seen as LastVisit_Date,p.responsible_party as Guar_ID,p.receives_sms as ReceiveSms, "
        //                                             + " p.receive_email as ReceiveEmail, ISNULL( p.collections_mtd,0) AS collect_payment, "
        //                                             + " case when p.next_regular_appointment > p.next_preventive_appointment OR p.next_regular_appointment is null then p.next_preventive_appointment else p.next_regular_appointment end as nextvisit_date, " 
        //                                             + " p.prim_benefits_remaining as PrimBenefitsRemaining,p.Sec_Benefits_Remaining as SecBenefitsRemaining, "
        //                                             + " p.Prim_Outstanding_Balance as PrimOutstandingBalance  ,p.Sec_Outstanding_Balance as SecOutstandingBalance,p.preferred_name as preferred_name , "
        //                                             + " ep.insurance_company_id as PrimaryInsuranceCompanyId ,icp.name as PrimaryInsuranceCompanyName,esec.insurance_company_id as SecondaryInsuranceCompanyId, "
        //                                             + " icsec.name as SecondaryInsuranceCompanyName  from patient as p "
        //                                             + " left outer join employer as ep on ep.employer_id = p.prim_employer_id "
        //                                             + " left outer join insurance_company as icp on icp.insurance_company_id = ep.insurance_company_id " 
        //                                             + " left outer join employer as esec on esec.employer_id = p.sec_employer_id "
        //                                             + " left outer join insurance_company as icsec on icsec.insurance_company_id = esec.insurance_company_id ";

        //change GetEagleSoftPatientData (2020_07_03) if next visit date < getdate() then next visit will be null as per hardi suggestion

        public static string GetEagleSoftPatientIds = " SELECT Patient_id FROM Patient";

        public static string GetEagleSoftInsertPatientData = "Select p.patient_id as Patient_EHR_ID, 0 AS Clinic_Number from patient as p ";

        public static string GetEagleSoftPatientData = " select distinct 0 AS Clinic_Number, 1 as Service_Install_Id, p.patient_id as Patient_EHR_ID ,p.first_name as First_name,p.last_name as Last_name,p.middle_initial as Middle_Name, "
                                                     + " p.salutation as Salutation ,(case when p.status = 'A' then 'A' else 'I' END) as Status,(case WHEN p.Patient_status = 'Y' AND p.status = 'A' then 'Active' when p.Patient_status = 'Y' AND p.status != 'A' THEN 'InActive' else 'NonPatient' END) as EHR_Status,(case when p.sex = 'F' THEN 'Female' WHEN p.sex = 'M' Then 'Male' ELSE 'Unknown' END ) as Sex,( case when p.marital_status = 'M' then 'Married' when p.marital_status = 'C' then 'Child' when p.marital_status = 'W' then 'Widowed'  when p.marital_status = 'D' then 'Divorced'  when p.marital_status = 'S' then 'Single' when p.marital_status = 'X' then 'Separated' ELSE 'Unknown' END ) as MaritalStatus,p.birth_date as Birth_Date , "
                                                     + " p.email_address as Email,p.cell_phone as Mobile , p.home_phone as Home_Phone,p.work_phone as Work_Phone,p.address_1 as Address1, p.prim_member_id as Primary_Ins_Subscriber_ID , p.sec_member_id as Secondary_Ins_Subscriber_ID , "
                                                     + " p.address_2 as Address2,p.city as City,p.state as State,p.zipcode as Zipcode, p.responsible_party_status as ResponsibleParty_Status, "
                                                     // + "  p.current_bal as CurrentBal,p.thirty_day as ThirtyDay,p.sixty_day as SixtyDay,p.ninety_day as NinetyDay,0 as Over90, "
                                                     + " ISNULL( p.current_bal,0) + ISNULL(p.thirty_day,0) + ISNULL( p.sixty_day,0) + ISNULL( p.ninety_day,0)  as CurrentBal,ISNULL( p.current_bal,0) as ThirtyDay, ISNULL( p.thirty_day,0) as SixtyDay,ISNULL( p.sixty_day,0) as NinetyDay, ISNULL( p.ninety_day,0) as Over90, "
                                                     + " p.first_visit_date as FirstVisit_Date,p.last_date_seen as LastVisit_Date,p.responsible_party as Guar_ID,( CASE WHEN  p.receives_sms = 'N' THEN 'N' ELSE 'Y' END ) as ReceiveSms, "
                                                     //+ " (select top 1 start_time from appointment where patient_id=p.patient_id order by start_time) as FirstVisit_Date,(select top 1 start_time from appointment where patient_id=p.patient_id and start_time<getdate() order by start_time desc) as LastVisit_Date,(select top 1 start_time from appointment where patient_id=p.patient_id and start_time>getdate() order by start_time asc) as NextVistitDate,p.responsible_party as Guar_ID,( CASE WHEN  p.receives_sms = 'N' THEN 'N' ELSE 'Y' END ) as ReceiveSms, "
                                                     + " ( CASE WHEN   p.receive_email  = 'N' THEN 'N' ELSE 'Y' END )  as ReceiveEmail, ISNULL( p.collections_ytd,0)  AS collect_payment, " //+ ISNULL( p.charges_ytd,0)
                                                     + " case when p.next_regular_appointment > p.next_preventive_appointment OR p.next_regular_appointment is null "
                                                     + " then (case when p.next_preventive_appointment >= convert(date, getdate()) then p.next_preventive_appointment  else null end)  "
                                                     + " else (case when p.next_regular_appointment >= convert(date, getdate()) then p.next_regular_appointment else null end) end as nextvisit_date, "
                                                     + " p.prim_benefits_remaining as PrimBenefitsRemaining,p.Sec_Benefits_Remaining as SecBenefitsRemaining,( CASE WHEN ( ISNULL(icp.name,'') =  '' AND ISNULL(icsec.name,'') = '' ) THEN 0 ELSE (p.prim_benefits_remaining + p.Sec_Benefits_Remaining) END ) AS remaining_benefit,( CASE WHEN ( ISNULL(icp.name,'') =  '' AND ISNULL(icsec.name,'') = '' ) THEN 0 ELSE (p.Prim_Outstanding_Balance + p.Sec_Outstanding_Balance ) END ) AS used_benefit, "
                                                     + " p.Prim_Outstanding_Balance as PrimOutstandingBalance  ,p.Sec_Outstanding_Balance as SecOutstandingBalance,p.preferred_name as preferred_name , "
                                                     + " ep.insurance_company_id as Primary_Insurance ,icp.name as Primary_Insurance_CompanyName,esec.insurance_company_id as Secondary_Insurance, "
                                                     + " icsec.name as Secondary_Insurance_CompanyName,psp.preferred_dentist AS Pri_Provider_ID,psp.preferred_hygienist AS Sec_Provider_ID,(case when pr.next_recall_date is not null then convert(varchar,pr.next_recall_date)+'@'+convert(varchar,RCL.description)+'@'+convert(varchar, RCL.recall_id) else '' end ) AS due_date, 1 AS InsUptDlt,'Y' as ReceiveVoiceCall,case when P.Notes like '%Spanish%' THEN 'Spanish' WHEN P.Notes Like '%French%' THEN 'French' ELSE 'English' END AS PreferredLanguage,P.Notes AS Patient_Note,"
                                                     + " P.social_security AS SSN, p.encrypted_social_security, P.drivers_license AS driverlicense,ep.group_number AS GroupId ,0 AS emergencycontactId,"
                                                     + " (Select top 1 answer from patient_answers pn inner join patient_prompts pp on pp.patient_prompt_id = pn.patient_prompt_id where pp.prompt = 'Emergency Contact' and pn.patient_id = P.Patient_id) AS EmergencyContact_First_Name,'' AS EmergencyContact_Last_Name,"
                                                     + " (Select top 1 answer from patient_answers pn inner join patient_prompts pp on pp.patient_prompt_id = pn.patient_prompt_id where pp.prompt = 'Emergency Contact #' and pn.patient_id = P.Patient_id)  AS emergencycontactnumber,p.School,'' AS Employer, "
                                                     + " SP.Patient_Id AS SpouseId, SP.First_name AS Spouse_First_Name,SP.Last_Name AS Spouse_Last_Name,"
                                                     + " RP.Patient_id AS responsiblepartyId,RP.First_Name AS ResponsibleParty_First_Name,RP.Last_name AS ResponsibleParty_Last_Name,RP.social_security AS responsiblepartyssn, RP.encrypted_social_security as RespEncrypted_social_security, RP.birth_date AS responsiblepartybirthdate,icp.phone1 AS Prim_Ins_Company_Phonenumber,icsec.phone1 AS Sec_Ins_Company_Phonenumber "
                                                     + " from patient as p "
                                                     + " left outer join employer as ep on ep.employer_id = p.prim_employer_id "
                                                     + " left outer join insurance_company as icp on icp.insurance_company_id = ep.insurance_company_id "
                                                     + " left outer join employer as esec on esec.employer_id = p.sec_employer_id "
                                                     + " left outer join insurance_company as icsec on icsec.insurance_company_id = esec.insurance_company_id "
                                                     + " LEFT JOIN (Select MAX(patient_recalls.next_recall_date) AS next_recall_date  ,patient_recalls.Patient_Id, MIN(patient_recalls.recall_id ) AS Recall_Id From patient_recalls INNER join patient p on p.patient_id = patient_recalls.patient_id  where p.receive_recalls = 'Y' group by patient_recalls.patient_id) as pr ON PR.Patient_id = p.patient_id LEFT join recalls as r on pr.recall_id=r.recall_id LEFT JOIN recalls RCL ON RCL.recall_id = pr.recall_id "
                                                     + " LEFT JOIN ( Select Prim_responsible_id AS Responsible_id  ,MIN(Patient_id) AS SpouseId from Patient where prim_relationship = 'W' AND status = 'A'  AND Prim_responsible_id <> ''  Group by Prim_responsible_id ) AS SP1 ON SP1.Responsible_id   = P.Patient_Id "
                                                     + " LEFT JOIN Patient SP ON SP.Patient_id = SP1.SpouseId "
                                                     + " LEFT JOIN Patient RP ON RP.Patient_id = p.Prim_responsible_id left join patient_site_providers PSP on PSP.patient_id = P.Patient_Id AND P.Practice_id = PSP.Practice_Id "
                                                     ;
        //(select MIN (AA.next_recall_date ) AS next_recall_date  ,AA.Patient_Id,( select MIN ( recall_id) AS Recall_id from patient_recalls  where patient_id = AA.Patient_id and next_recall_date =  MIN (AA.next_recall_date ) ) AS Recall_Id FROM ( Select  next_recall_date  ,Patient_Id From patient_recalls where default_recall_yn = 'Y' ) AS AA Group by AA.Patient_Id) as pr ON PR.Patient_id = p.patient_id LEFT join recalls as r on pr.recall_id=r.recall_id LEFT JOIN recalls RCL ON RCL.recall_id = pr.recall_id "; //next_recall_date  > getdate() AND


        public static string GetEagleSoftPatientDataFromPayment = " select distinct 0 AS Clinic_Number, 1 as Service_Install_Id, p.patient_id as Patient_EHR_ID ,p.first_name as First_name,p.last_name as Last_name,p.middle_initial as Middle_Name, "
                                                     + " p.salutation as Salutation ,(case when p.status = 'A' then 'A' else 'I' END) as Status,(case WHEN p.Patient_status = 'Y' AND p.status = 'A' then 'Active' when p.Patient_status = 'Y' AND p.status != 'A' THEN 'InActive' else 'NonPatient' END) as EHR_Status,(case when p.sex = 'F' THEN 'Female' WHEN p.sex = 'M' Then 'Male' ELSE 'Unknown' END ) as Sex,( case when p.marital_status = 'M' then 'Married' when p.marital_status = 'C' then 'Child' when p.marital_status = 'W' then 'Widowed'  when p.marital_status = 'D' then 'Divorced' when p.marital_status = 'S' then 'Single' when p.marital_status = 'X' then 'Separated' ELSE 'Unknown' END ) as MaritalStatus,p.birth_date as Birth_Date , "
                                                     + " p.email_address as Email,p.cell_phone as Mobile , p.home_phone as Home_Phone,p.work_phone as Work_Phone,p.address_1 as Address1, p.prim_member_id as Primary_Ins_Subscriber_ID , p.sec_member_id as Secondary_Ins_Subscriber_ID , "
                                                     + " p.address_2 as Address2,p.city as City,p.state as State,p.zipcode as Zipcode, p.responsible_party_status as ResponsibleParty_Status, "
                                                     // + "  p.current_bal as CurrentBal,p.thirty_day as ThirtyDay,p.sixty_day as SixtyDay,p.ninety_day as NinetyDay,0 as Over90, "
                                                     + " ISNULL( p.current_bal,0) + ISNULL(p.thirty_day,0) + ISNULL( p.sixty_day,0) + ISNULL( p.ninety_day,0)  as CurrentBal,ISNULL( p.current_bal,0) as ThirtyDay, ISNULL( p.thirty_day,0) as SixtyDay,ISNULL( p.sixty_day,0) as NinetyDay, ISNULL( p.ninety_day,0) as Over90, "
                                                     + " p.first_visit_date as FirstVisit_Date,p.last_date_seen as LastVisit_Date,p.responsible_party as Guar_ID,( CASE WHEN  p.receives_sms = 'N' THEN 'N' ELSE 'Y' END ) as ReceiveSms, "
                                                     + " ( CASE WHEN   p.receive_email  = 'N' THEN 'N' ELSE 'Y' END )  as ReceiveEmail, ISNULL( p.collections_ytd,0)  AS collect_payment, " //+ ISNULL( p.charges_ytd,0)
                                                     + " case when p.next_regular_appointment > p.next_preventive_appointment OR p.next_regular_appointment is null "
                                                     + " then (case when p.next_preventive_appointment >= convert(date, getdate()) then p.next_preventive_appointment  else null end)  "
                                                     + " else (case when p.next_regular_appointment >= convert(date, getdate()) then p.next_regular_appointment else null end) end as nextvisit_date, "
                                                     + " p.prim_benefits_remaining as PrimBenefitsRemaining,p.Sec_Benefits_Remaining as SecBenefitsRemaining,( CASE WHEN ( ISNULL(icp.name,'') =  '' AND ISNULL(icsec.name,'') = '' ) THEN 0 ELSE (p.prim_benefits_remaining + p.Sec_Benefits_Remaining) END ) AS remaining_benefit,( CASE WHEN ( ISNULL(icp.name,'') =  '' AND ISNULL(icsec.name,'') = '' ) THEN 0 ELSE (p.Prim_Outstanding_Balance + p.Sec_Outstanding_Balance ) END ) AS used_benefit, "
                                                     + " p.Prim_Outstanding_Balance as PrimOutstandingBalance  ,p.Sec_Outstanding_Balance as SecOutstandingBalance,p.preferred_name as preferred_name , "
                                                     + " ep.insurance_company_id as Primary_Insurance ,icp.name as Primary_Insurance_CompanyName,esec.insurance_company_id as Secondary_Insurance, "
                                                     + " icsec.name as Secondary_Insurance_CompanyName,p.preferred_dentist AS Pri_Provider_ID,p.preferred_hygienist AS Sec_Provider_ID,(case when pr.next_recall_date is not null then convert(varchar,pr.next_recall_date)+'@'+convert(varchar,RCL.description)+'@'+convert(varchar, RCL.recall_id) else '' end ) AS due_date, 1 AS InsUptDlt,'Y' as ReceiveVoiceCall,case when P.Notes like '%Spanish%' THEN 'Spanish' WHEN P.Notes Like '%French%' THEN 'French' ELSE 'English' END AS PreferredLanguage,P.Notes AS Patient_Note"
                                                     + " from patient as p "
                                                     + " left outer join employer as ep on ep.employer_id = p.prim_employer_id "
                                                     + " left outer join insurance_company as icp on icp.insurance_company_id = ep.insurance_company_id "
                                                     + " left outer join employer as esec on esec.employer_id = p.sec_employer_id "
                                                     + " left outer join insurance_company as icsec on icsec.insurance_company_id = esec.insurance_company_id LEFT JOIN (select MIN (AA.next_recall_date ) AS next_recall_date  ,AA.Patient_Id,( select MIN ( recall_id) AS Recall_id from patient_recalls  where patient_id = AA.Patient_id and next_recall_date =  MIN (AA.next_recall_date ) ) AS Recall_Id FROM ( Select  patient_recalls.next_recall_date  ,patient_recalls.Patient_Id From patient_recalls INNER join patient p on p.patient_id = patient_recalls.patient_id where next_recall_date  > getdate() AND p.receive_recalls = 'Y' ) AS AA Group by AA.Patient_Id) as pr ON PR.Patient_id = p.patient_id LEFT join recalls as r on pr.recall_id=r.recall_id LEFT JOIN recalls RCL ON RCL.recall_id = pr.recall_id WHERE p.patient_id = @PatientEHRId ";



        public static string GetEagleSoftAppointmentsPatientData = " select distinct 0 AS Clinic_Number, 1 as Service_Install_Id, p.patient_id as Patient_EHR_ID ,p.first_name as First_name,p.last_name as Last_name,p.middle_initial as Middle_Name, "
                                                     + " p.salutation as Salutation ,(case when p.status = 'A' then 'A' else 'I' END) as Status,(case WHEN p.Patient_status = 'Y' AND p.status = 'A' then 'Active' when p.Patient_status = 'Y' AND p.status != 'A' THEN 'InActive' else 'NonPatient' END) as EHR_Status,(case when p.sex = 'F' THEN 'Female' WHEN p.sex = 'M' Then 'Male' ELSE 'Unknown' END ) as Sex,( case when p.marital_status = 'M' then 'Married' when p.marital_status = 'C' then 'Child' when p.marital_status = 'W' then 'Widowed'  when p.marital_status = 'D' then 'Divorced' when p.marital_status = 'S' then 'Single' when p.marital_status = 'X' then 'Separated' ELSE 'Unknown' END ) as MaritalStatus,p.birth_date as Birth_Date , "
                                                     + " p.email_address as Email,p.cell_phone as Mobile , p.home_phone as Home_Phone,p.work_phone as Work_Phone,p.address_1 as Address1, p.prim_member_id as Primary_Ins_Subscriber_ID , p.sec_member_id as Secondary_Ins_Subscriber_ID , "
                                                     + " p.address_2 as Address2,p.city as City,p.state as State,p.zipcode as Zipcode, p.responsible_party_status as ResponsibleParty_Status, "
                                                     + " ISNULL( p.current_bal,0) + ISNULL(p.thirty_day,0) + ISNULL( p.sixty_day,0) + ISNULL( p.ninety_day,0)  as CurrentBal,ISNULL( p.current_bal,0) as ThirtyDay, ISNULL( p.thirty_day,0) as SixtyDay,ISNULL( p.sixty_day,0) as NinetyDay, ISNULL( p.ninety_day,0) as Over90, "
                                                     + " p.first_visit_date as FirstVisit_Date,p.last_date_seen as LastVisit_Date,p.responsible_party as Guar_ID,( CASE WHEN  p.receives_sms = 'N' THEN 'N' ELSE 'Y' END ) as ReceiveSms, "
                                                     + " ( CASE WHEN   p.receive_email  = 'N' THEN 'N' ELSE 'Y' END )  as ReceiveEmail, ISNULL( p.collections_ytd,0)  AS collect_payment, " //+ ISNULL( p.charges_ytd,0)
                                                     + " case when p.next_regular_appointment > p.next_preventive_appointment OR p.next_regular_appointment is null "
                                                     + " then (case when p.next_preventive_appointment >= convert(date, getdate()) then p.next_preventive_appointment  else null end)  "
                                                     + " else (case when p.next_regular_appointment >= convert(date, getdate()) then p.next_regular_appointment else null end) end as nextvisit_date, "
                                                     + " p.prim_benefits_remaining as PrimBenefitsRemaining,p.Sec_Benefits_Remaining as SecBenefitsRemaining,( CASE WHEN ( ISNULL(icp.name,'') =  '' AND ISNULL(icsec.name,'') = '' ) THEN 0 ELSE (p.prim_benefits_remaining + p.Sec_Benefits_Remaining) END ) AS remaining_benefit,( CASE WHEN ( ISNULL(icp.name,'') =  '' AND ISNULL(icsec.name,'') = '' ) THEN 0 ELSE (p.Prim_Outstanding_Balance + p.Sec_Outstanding_Balance ) END ) AS used_benefit, "
                                                     + " p.Prim_Outstanding_Balance as PrimOutstandingBalance  ,p.Sec_Outstanding_Balance as SecOutstandingBalance,p.preferred_name as preferred_name , "
                                                     + " ep.insurance_company_id as Primary_Insurance ,icp.name as Primary_Insurance_CompanyName,esec.insurance_company_id as Secondary_Insurance, "
                                                     + " icsec.name as Secondary_Insurance_CompanyName,psp.preferred_dentist AS Pri_Provider_ID,psp.preferred_hygienist AS Sec_Provider_ID,(case when pr.next_recall_date is not null then convert(varchar,pr.next_recall_date)+'@'+convert(varchar,RCL.description)+'@'+convert(varchar, RCL.recall_id) else '' end ) AS due_date, 1 AS InsUptDlt,'Y' as ReceiveVoiceCall,case when P.Notes like '%Spanish%' THEN 'Spanish' WHEN P.Notes Like '%French%' THEN 'French' ELSE 'English' END AS PreferredLanguage,P.Notes AS Patient_Note,"
                                                     + " P.social_security AS SSN,P.encrypted_social_security, P.drivers_license AS driverlicense,ep.group_number AS GroupId,0 AS emergencycontactId,"
                                                     + " (Select top 1 answer from patient_answers pn inner join patient_prompts pp on pp.patient_prompt_id = pn.patient_prompt_id where pp.prompt = 'Emergency Contact' and pn.patient_id = P.Patient_id ) AS EmergencyContact_First_Name,'' AS EmergencyContact_Last_Name,"
                                                     + " (Select top 1 answer from patient_answers pn inner join patient_prompts pp on pp.patient_prompt_id = pn.patient_prompt_id where pp.prompt = 'Emergency Contact #' and pn.patient_id = P.Patient_id )  AS emergencycontactnumber,p.School,'' AS Employer, "
                                                     + " SP.Patient_Id AS SpouseId, SP.First_name AS Spouse_First_Name,SP.Last_Name AS Spouse_Last_Name,"
                                                     + " RP.Patient_id AS responsiblepartyId,RP.First_Name AS ResponsibleParty_First_Name,RP.Last_name AS ResponsibleParty_Last_Name,RP.social_security AS responsiblepartyssn,RP.encrypted_social_security as RespEncrypted_social_security, RP.birth_date AS responsiblepartybirthdate,icp.phone1 AS Prim_Ins_Company_Phonenumber,icsec.phone1 AS Sec_Ins_Company_Phonenumber "
                                                     + " from patient as p "
                                                     + " Inner Join appointment As Apt on p.patient_id = Apt.patient_id"
                                                     + " left outer join employer as ep on ep.employer_id = p.prim_employer_id "
                                                     + " left outer join insurance_company as icp on icp.insurance_company_id = ep.insurance_company_id "
                                                     + " left outer join employer as esec on esec.employer_id = p.sec_employer_id "
                                                     + " left outer join insurance_company as icsec on icsec.insurance_company_id = esec.insurance_company_id "
                                                     + " LEFT JOIN (Select MAX(patient_recalls.next_recall_date) AS next_recall_date  ,patient_recalls.Patient_Id, MIN(patient_recalls.recall_id ) AS Recall_Id From patient_recalls INNER join patient p on p.patient_id = patient_recalls.patient_id  where p.receive_recalls = 'Y' group by patient_recalls.patient_id) as pr ON PR.Patient_id = p.patient_id LEFT join recalls as r on pr.recall_id=r.recall_id LEFT JOIN recalls RCL ON RCL.recall_id = pr.recall_id "
                                                     + " LEFT JOIN ( Select Prim_responsible_id AS Responsible_id  ,MIN(Patient_id) AS SpouseId from Patient where prim_relationship = 'W' AND status = 'A'  AND Prim_responsible_id <> ''  Group by Prim_responsible_id ) AS SP1 ON SP1.Responsible_id   = P.Patient_Id "
                                                     + " LEFT JOIN Patient SP ON SP.Patient_id = SP1.SpouseId "
                                                     + " LEFT JOIN Patient RP ON RP.Patient_id = p.Prim_responsible_id left join patient_site_providers PSP on PSP.patient_id = P.Patient_Id AND P.Practice_id = PSP.Practice_Id "
                                                     //(select MIN (AA.next_recall_date ) AS next_recall_date  ,AA.Patient_Id,( select MIN ( recall_id) AS Recall_id from patient_recalls  where patient_id = AA.Patient_id and next_recall_date =  MIN (AA.next_recall_date ) ) AS Recall_Id FROM ( Select  next_recall_date  ,Patient_Id From patient_recalls where default_recall_yn = 'Y' ) AS AA Group by AA.Patient_Id) as pr ON PR.Patient_id = p.patient_id LEFT join recalls as r on pr.recall_id=r.recall_id LEFT JOIN recalls RCL ON RCL.recall_id = pr.recall_id " //next_recall_date  > getdate() AND
                                                     + " where Apt.start_time > ?  AND ( Apt.classification = 1 OR Apt.classification = 16 OR Apt.classification = 32 OR Apt.classification = 64) ";


        public static string InsertPatientDetails = " INSERT INTO patient (Patient_Id,Last_Name, First_Name, Middle_Initial, Cell_Phone, Email_Address, preferred_dentist, First_Visit_Date,Status,Practice_Id,responsible_party,current_bal,thirty_day,sixty_day,ninety_day) "
                                                   + " VALUES (@Patient_Id,@Last_Name, @First_Name, @Middle_Initial, @Cell_Phone, @Email_Address, @preferred_dentist, @First_Visit_Date,@Status,@OperatoryId,@Patient_Gur_id,0,0,0,0) Select @FindMaxPatient  ";

        public static string InsertPatientDetails_With_BirthDate = " INSERT INTO patient (Patient_Id,Last_Name, First_Name, Middle_Initial, Cell_Phone, Email_Address,birth_date, preferred_dentist, First_Visit_Date,Status,Practice_Id,responsible_party,current_bal,thirty_day,sixty_day,ninety_day) "
                                                 + " VALUES (@Patient_Id,@Last_Name, @First_Name, @Middle_Initial, @Cell_Phone, @Email_Address, @Birth_Date, @preferred_dentist, @First_Visit_Date,@Status,@OperatoryId,@Patient_Gur_id,0,0,0,0) Select @FindMaxPatient  ";


        public static string UpdatePatientDetails = " UPDATE Patient SET responsible_party = Patient_Id,prim_responsible_id = case when prim_relationship  is not null  then case when prim_relationship = '' then NULL else Patient_id end else NULL end ,Sec_responsible_id = case when sec_relationship  is not null  then case when sec_relationship = '' then NULL else Patient_id end else NULL end  WHERE Patient_Id = @Patient_Id";

        public static string CheckModeExistAsEmergency_Cont = "IF NOT EXISTS ( select 1 from patient_prompts where patient_prompt_id = '4' and prompt = 'Emergency Contact') BEGIN Insert Into patient_prompts (patient_prompt_id,prompt) VALUES ('4','Emergency Contact')  END";

        public static string CheckModeExistAsEmergency_Name = "IF NOT EXISTS ( select 1 from patient_prompts where patient_prompt_id = '5' and prompt = 'Emergency Contact #') BEGIN Insert Into patient_prompts (patient_prompt_id,prompt) VALUES ('5','Emergency Contact #')  END";

        public static string InsertEmergencyContact = " INSERT INTO patient_answers (Patient_Prompt_Id,Patient_id,answer) values (@EmergencyType,@Patient_EHR_Id,@EmergencyValue)";

        public static string InsertPatientUsed = "INSERT INTO used_patients ( Patient_Id,Time_used ) VALUES (@Patient_Id,@Time_used) "
                                                + " INSERT INTO patient_site_providers ( patient_id ,practice_id,preferred_dentist,preferred_hygienist) VALUES (@Patient_Id,(SELECT TOP 1 (Practice_Id) FROM Chairs ), @preferredDentist,@preferredHygenist) ";

        public static string InsertAppointment = " INSERT INTO appointment (description,start_time,end_time,patient_id,location_id,classification,appointment_type_id,date_appointed,scheduled_by,modified_by,confirmation_status,allday_event,sooner_if_possible,private,auto_confirm_sent,appointment_notes) "
                                               + "  VALUES (@description,@start_time,@end_time,@patient_id,@location_id,@classification,@appointment_type_id,@date_appointed,@scheduled_by,@modified_by,@confirmation_status,@allday_event,@sooner_if_possible,@private,@auto_confirm_sent,@appointment_notes) SELECT @@identity";

        public static string InsertAppointmentDetail = " INSERT INTO appointment_detail(appt_id,line_item,service_code,sequence,surface,fee,description,tooth_info,surface_detail,abutment_direction,root_data,tooth,patient_id,provider_id,extra_pat_id,extra_line_num,sort_order,status,standard_fee_id) "
                                               + " values(@appt_id,@line_number,@service_code,( Select Top 1 sequence from Planned_services where Patient_id =@patient_id  AND line_number = @line_number and Service_code = @service_code ),( Select Top 1 Surface from Planned_services where Patient_id = @patient_id AND line_number = @line_number and Service_code = @service_code ),"
                                               + "( Select Top 1 fee from Planned_services where Service_code = @service_code ),( Select Top 1 description from Planned_services where Service_code = @service_code),(Select ISNULL( (select top 1 tooth_data from Planned_services_extra where patient_id = @patient_id),'NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN')),(Select ISNULL( (select top 1 surface_detail from Planned_services_extra where patient_id = @patient_id),'NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN')),(Select ISNULL( (select top 1 abutment_direction from Planned_services_extra where patient_id = @patient_id),'NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN')),(Select ISNULL( (select top 1 root_data from Planned_services_extra where patient_id = @patient_id),'NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN')),( Select tooth from Planned_services where Service_code = @service_code ),@patient_id,@provider_id),1,( Select Top 1 sort_order from Planned_services where Patient_id =@patient_id  AND line_number = @line_number and Service_code = @service_code ),'A',( Select Top 1 standard_fee_id from Planned_services where Patient_id =@patient_id  AND line_number = @line_number and Service_code = @service_code )) ";

        public static string UpdatePlannedServices = " update planned_services Set appt_id = @appt_id,status = 'A' WHERE Patient_id = @Patient_id AND Line_number = @line_number AND service_code =  @service_code ";

        public static string InsertPlannedServices = @" 
            
            insert into planned_services(patient_id,line_number,service_code,sequence,provider_id,date_planned,fee,status,status_date,description,sort_order,appt_id,lab_fee,pre_fee,lab_fee2,standard_fee_id)
            select @patient_id,@line_number,@service_code,(select top 1 Sequence from  [services] where service_code = @service_code),@provider_id,@date_planned,(select top 1 fee from  [services] where service_code = @service_code),'A',@status_date,(select top 1 description from  [services] where service_code = @service_code),@line_number,@appt_id,0,(select top 1 fee from  [services] where service_code = @service_code),0,(select top 1 standard_fee_id from  [services] where service_code = @service_code)
            
            insert into planned_services_extra(patient_id,line_number,tooth_data,surface_detail,root_data,show_on_chart)
            select @patient_id,@LNumber,(Select ISNULL( (select top 1 tooth_data from Planned_services_extra where patient_id = @patient_id),'NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN')),(Select ISNULL( (select top 1 surface_detail from Planned_services_extra where patient_id = @patient_id),'NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN')),(Select ISNULL( (select top 1 root_data from Planned_services_extra where patient_id = @patient_id),'NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN')),(Select ISNULL( (select top 1 show_on_chart from Planned_services_extra where patient_id = @patient_id),'Z'))

            ";


        public static string Update_Patinet_Record_By_Patient_Form = " UPDATE Patient SET ColumnName = @ehrfield_value "
                                                            + " WHERE Patient_Id = @Patient_EHR_ID ";

        public static string InsertAppointmentLog = " INSERT INTO appt_log (appt_id, user_id,new_slot, new_date, appt_log_action, modified_by) VALUES (@appt_id, @EHR_User_ID,1, @new_date, 'R',@EHR_User_ID) ";

        public static string InsertAppointmentProvider = " INSERT INTO appointment_provider ( appointment_id,provider_id,start_time,end_time) values (@appointment_id,@provider_id,@start_time,@end_time)";

        public static string GetPatientListFromEagleSoft = " SELECT  Patient_Id AS Patient_EHR_ID, First_Name AS FirstName,Last_Name AS LastName, (First_Name + ' ' + Last_Name) AS Patient_Name,birth_date as birth_date,TRIM( cell_phone) AS Mobile,TRIM(Home_Phone) AS Home_Phone,TRIM(Work_Phone) AS Work_Phone,Status,responsible_party,email_address as Email From Patient ";

        public static string GetAllEagleSoftPatientRecallTpyeDueDateData = "select patient_id,pr.recall_id as RecallType_EHR_ID,next_recall_date ,default_recall_yn,r.description as RecallType_Name from patient_recalls as pr inner join recalls as r on pr.recall_id=r.recall_id";

        public static string GetEagleSoftPatientRecallTpyeDueDateData = "select patient_id,pr.recall_id as RecallType_EHR_ID,next_recall_date ,default_recall_yn,r.description as RecallType_Name from patient_recalls as pr inner join recalls as r on pr.recall_id=r.recall_id where patient_id = ? ";

        public static string GetBookOperatoryAppointmenetWiseDateTime = "Select AP.Appointment_id AS Appointment_EHR_Id, AP.location_id,AP.start_Time,AP.End_Time,P.First_name AS FirstName,P.Last_name AS LastName,p.cell_phone AS Mobile,p.email_address AS Email,PP.First_Name AS ProviderFirstName,PP.Last_Name AS ProviderLastName FROM Appointment AP INNER JOIN Patient P ON AP.Patient_id = P.Patient_id LEFT JOIN appointment_provider PV ON PV.Appointment_id = AP.Appointment_id LEFT JOIN Provider PP ON PP.Provider_Id = PV.Provider_id where AP.Start_Time > ? and  AP.End_Time < ? ";

        public static string GetEagleSoftPatient_RecallType = "select recall_id as RecallType_EHR_ID ,description as RecallType_Name from recalls";

        public static string GetEagleSoftUserData = "select provider_id as User_EHR_ID,'' AS User_LocalDB_ID,'' as User_web_Id,first_name as First_Name,last_name as Last_Name,'' as Password ,hire_date as EHR_Entry_DateTime,"
                                                    + " last_logon as Last_Updated_DateTime,'' as LocalDb_EntryDatetime,case when status= 'A' then 1 else 0 end as Is_active,0 as is_deleted,0 as Is_Adit_Updated,0 as Clinic_Number,1 as Service_Install_Id from  provider where position_id=5";

        public static string InsertEaglesoftUserLogin_Data = "INSERT INTO Provider(provider_id,first_name,last_name,sex,status,collections_go_to,provider_on_insurance,mtd_wo_statements,ytd_wo_statements,mtd_charges,ytd_charges,mtd_collections,ytd_collections,calculate_productivity,position_id,specialty,password,current_bal,contract_balance,estimated_insurance,access_basic,access_accounting,access_productivity,mtd_new_patients,mtd_other_debits,ytd_other_debits,mtd_other_credits,ytd_other_credits,use_practice_address,practice_id,bank_account,site_id,access_prescriptions,operatory_access,access_medical,access_timeclock_management,access_lab,view_docs,add_docs,edit_docs,delete_docs,pass_answer_1,pass_answer_2,pass_answer_3,access_intellicare,security_profile,provider_color,last_detail_id) "
                                                           + "VALUES('ADT','Adit','Adit','M','A','ADT','ADT','0','0','0','0','0','0','Y','5','301','b81631220264b6e19517cd8578fe843cf965d940863e7bf30114cd99339e4ee8','0','0','0','YYYYN','YYYYN','NN','0','0.00','0.00','0.00','0.00','Y','1','NO ACCOUNT','ADT','NNNNN','YYYYYYYYYYYYYYYYYYYYYYYYY','NN','NN','NNNNN','N','N','N','N','e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855','e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855','e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855','NNNN','0','16777215','973') ";

        public static string GetEagleSoftSpecialty = "select position_id as Speciality_EHR_ID,description as speciality_Name from positions ";

        public static string GetPatientInsuranceCompanyDetails = " select er.employer_id, maximum_coverage AS benefits_remaining , yearly_deductible AS remaining_deductible "
                                                               + " from employer er join insurance_company ic on ic.insurance_company_id = er.insurance_company_id where ic.name = @insurance_company_Name ";

        public static string GetEaglesoftActualVersion = " SELECT top 1 version from system_preferences ";

        public static string InsertPatientPryInsuranceDetail = " update patient set  prim_responsible_id  = @patient_id, prim_relationship = 'S', prim_employer_id = @employer_id, "
                                              + " prim_outstanding_balance = 0, prim_benefits_remaining  = @benefits_remaining, prim_remaining_deductible  = @remaining_deductible, "
                                              + " patient_status = 'Y',policy_holder_status  = 'Y'  where patient_id = @patient_id";


        public static string InsertPatientSecInsuranceDetail = " update patient set  sec_responsible_id  = @patient_id, sec_relationship = 'S', sec_employer_id = @employer_id, "
                                              + " sec_outstanding_balance = 0, sec_benefits_remaining  = @benefits_remaining, sec_remaining_deductible  = @remaining_deductible, "
                                              + " patient_status = 'Y',policy_holder_status  = 'Y'  where patient_id = @patient_id";

        public static string InsertPatientDocData = "Insert Into Document (document_name, document_type, document_creation_date, document_modified_date, "
                                                      + " open_with, ref_id, ref_table, user_id, original_user_id, private, display_in_docmgr, "
                                                      + " signed, needs_converted, notice_of_privacy, privacy_authorization, "
                                                      + " consent, practice_id, custom_document_id, headerfooter_added ) "
                                            + " VALUES  ( @document_name, @document_type, @document_creation_date, @document_modified_date, "
                                                      + " @open_with, @ref_id, @ref_table, @EHR_User_ID,  @EHR_User_ID, @private, @display_in_docmgr, "
                                                      + " @signed, @needs_converted, @notice_of_privacy, @privacy_authorization, "
                                                      + " @consent, @practice_id, @custom_document_id, @headerfooter_added ) "
                                                      + "";


        public static string InsertPatientDocData_version32 = "Insert Into Document (document_name, document_type, document_creation_date, document_modified_date, "
                                                      + " open_with, ref_id, ref_table, user_id, original_user_id, private, display_in_docmgr, "
                                                      + " signed, needs_converted, notice_of_privacy, privacy_authorization, "
                                                      + " consent, practice_id, custom_document_id, headerfooter_added,document_group_id ) "
                                            + " VALUES  ( @document_name, @document_type, @document_creation_date, @document_modified_date, "
                                                      + " @open_with, @ref_id, @ref_table, @EHR_User_ID, @EHR_User_ID, @private, @display_in_docmgr, "
                                                      + " @signed, @needs_converted, @notice_of_privacy, @privacy_authorization, "
                                                      + " @consent, @practice_id, @custom_document_id, @headerfooter_added, @document_group_id) "
                                                      + "";

        public static string InsertPatientTreatmentDocData = "Insert Into Document (document_name, document_type, document_creation_date, document_modified_date, "
                                                      + " open_with, ref_id, ref_table, user_id, original_user_id, private, display_in_docmgr, "
                                                      + " signed, needs_converted, notice_of_privacy, privacy_authorization, "
                                                      + " consent, practice_id, custom_document_id, headerfooter_added,document_group_id ) "
                                            + " VALUES  ( '@document_name', @document_type, @document_creation_date, @document_modified_date, "
                                                      + " @open_with, @ref_id, @ref_table, @EHR_User_ID, @EHR_User_ID, @private, @display_in_docmgr, "
                                                      + " @signed, @needs_converted, @notice_of_privacy, @privacy_authorization, "
                                                      + " @consent, @practice_id, @custom_document_id, @headerfooter_added,@document_group_id ) ";

        public static string InsertPatientInsuranceCarrierDocData = "Insert Into Document (document_name, document_type, document_creation_date, document_modified_date, "
                                                   + " open_with, ref_id, ref_table, user_id, original_user_id, private, display_in_docmgr, "
                                                   + " signed, needs_converted, notice_of_privacy, privacy_authorization, "
                                                   + " consent, practice_id, custom_document_id, headerfooter_added,document_group_id ) "
                                                   + " VALUES  ( '@document_name', @document_type, @document_creation_date, @document_modified_date, "
                                                   + " @open_with, @ref_id, @ref_table, @EHR_User_ID, @EHR_User_ID, @private, @display_in_docmgr, "
                                                   + " @signed, @needs_converted, @notice_of_privacy, @privacy_authorization, "
                                                   + " @consent, @practice_id, @custom_document_id, @headerfooter_added,@document_group_id ) ";

        public static string InsertPatientAllergies = "if not exists ( select 1 from patient_alerts  where Patient_id = @Patient_id AND Alert_id = @Alert_id) begin "
                                                      + " Insert Into patient_alerts ( Patient_id, Alert_id ) VALUES (@Patient_id,@Alert_id) end";

        public static string DeletePatientAllergies = "Delete from patient_alerts where Patient_id = @Patient_id and Alert_id = @Alert_id";

        public static string GetEagleSoftDocPath = " SELECT top(1) server_computer,server_share from practice_preferences ";

        public static string GetEagleSoftAllergies = " select '' AS Disease_LocalDB_ID,convert(varchar,Alert_id) AS Disease_EHR_ID,'' AS Disease_Web_ID,description AS Disease_Name,'A' AS Disease_Type,getdate() AS EHR_Entry_DateTime,'' AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated from Alert  where alert_type = 1 and show_in_list_yn = 'Y' ";

        //rooja 23-8-24
        public static string GetEagleSoftMedicalHistoryAllergies = " select * from list_item Where id = @SectionEHR_ID";
        
        public static string GetEagleSoftMedication = "Select Trim(str(drug_id)) as Medication_EHR_ID, " +
                                                      "description as Medication_Name, " +
                                                      "'' as Medication_Description, " +
                                                      "'' as Medication_Notes, " +
                                                      "'' as Medication_Sig, " +
                                                      "drug_id as Medication_Parent_EHR_ID, " +
                                                      "'Drug' as Medication_Type, " +
                                                      "'' as Allow_Generic_Sub, " +
                                                      "dispense as Drug_Quantity," +
                                                      "refills as Refills, " +
                                                      "'True' as Is_Active, " +
                                                      "getdate() as EHR_Entry_DateTime, " +
                                                      "'' as Medication_Provider_ID, " +
                                                      "0 as is_deleted, " +
                                                      "0 as Is_Adit_Updated, " +
                                                      "'0' as Clinic_Number FROM prescription_drugs Where(description is not null and description <> '')";

        public static string GetEagleSoftPatientMedication = "select mp.patient_id as Patient_EHR_ID, " +
                                                      "mp.drug_id as Medication_EHR_ID, " +
                                                      "Mp.rx_id as PatientMedication_EHR_ID, " +
                                                      "m.description as Medication_Name, " +
                                                      "case when mp.allow_generic = 'Y' then 'Drug' else 'Medication' end as Medication_Type, " +
                                                      "'' as Medication_Note, " +
                                                      "mp.dispense as Drug_Quantity, " +
                                                      "mp.provider_id as Provider_EHR_ID, " +
                                                      "mp.date_entered as Start_Date, " +
                                                      "mp.date_expires as End_Date, " +
                                                      "mp.date_entered AS EHR_Entry_DateTime, " +
                                                      "Patient_instructions as Patient_Notes, " +
                                                      "0 AS is_deleted, " +
                                                      "'0' as Clinic_Number, " +
                                                      "Case When mp.status = 'E' Then 'False' Else Case When (date_expires+1 > getdate() or date_expires is null) Then 'True' else 'False' End End As Is_Active, " +
                                                      "mp.status from prescriptions as mp left join " +
                                                      "prescription_drugs as m on m.drug_id = mp.drug_id Inner Join Patient P on Mp.Patient_id = P.Patient_id where mp.patient_id <> ''";

        public static string GetEagleSoftPatientAllergies = "select '0' AS PatientDisease_LocalDB_ID,convert(varchar,AL.Alert_id) AS Disease_EHR_ID,PA.patient_id  AS Patient_EHR_ID, '' AS PatientDisease_Web_ID,AL.description AS Disease_Name,'A' AS Disease_Type,getdate() AS EHR_Entry_DateTime,getdate() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated,0 as Clinic_Number from  patient_alerts PA inner join Alert AL on PA.alert_id = AL.alert_id   where AL.alert_type = 1 and AL.show_in_list_yn = 'Y'  ";
        public static string GetEaglesoftSectionItemMaster = "Select 0 As Clinic_Number,@Service_Install_Id as Service_Install_Id,'' AS SectionItemMaster_LocalDB_ID,'' AS SectionItem_Web_Id,convert(varchar,SI.Id) AS SectionItem_EHR_Id,SI.Type AS SectionItemType,(CASE WHEN SI.Type = 3 THEN S.title ELSE SI.Text END) AS SectionItemName,SI.[Index] AS SectionItemOrder,convert(varchar,SI.Section_Id) AS Section_EHR_Id,"
                                                        + " convert(varchar,S.Form_Id) AS Form_EHR_Id,SQ.Allow_Comment AS AllowComment,SQ.alert_on_yes AS AlertOnYes, SQ.alert_on_no AS AlertOnNo,convert(varchar, SQ.Alert_Id) AS Alert_EHR_Id,convert(varchar,Q.Question_Type) AS Question_Type,convert(varchar,Q.Answer_Type) AS Answer_Type,C.Allow_Edit AS AllowEditComment,convert(varchar,LQ.number_of_columns) AS NumberOfColumns,GetDate() AS Entry_DateTime,GetDate() AS Last_Sync_Date,0 as Is_Adit_updated,0 AS Is_deleted"
                                                        + " From Section_Item SI"
                                                        + " Inner Join Section S ON SI.Section_id = S.Id"
                                                        + " Left Join Single_Question SQ ON SQ.Section_Item_id = SI.Id"
                                                        + " Left Join Question Q ON Q.Section_Item_Id= SI.Id"
                                                        + " Left Join [Comment] C ON C.Section_Item_id = SI.Id"
                                                       + " Left Join List_Question LQ ON LQ.Section_Item_Id = SI.Id WHERE SI.Type NOT IN (4) ";


        public static string GetEagleSoftSectionItemOptions = "Select 0 As Clinic_Number,@Service_Install_Id as Service_Install_Id,'' AS SectionItemOptionMaster_LocalDB_ID,'' AS SectionItemOption_WEB_Id,convert(varchar,LI.Id) AS SectionItemOption_EHR_Id,LI.Text AS SectionItemOptionName,convert(varchar,Q.Question_Type) AS Question_Type,convert(varchar,Q.Answer_Type) AS Answer_Type,convert(varchar,SI.Id) AS SectionItem_EHR_Id,convert(varchar,S.Id) AS Section_EHR_Id,convert(varchar,S.Form_id) AS Form_EHR_Id,convert(varchar,LQ.number_of_columns) AS NumberOfColumns,GetDate() AS Entry_DateTime,GetDate() AS Last_Sync_Date,0 as Is_Adit_updated,0 AS Is_deleted"
                                                            + " From Section_Item SI"
                                                           + "  Inner Join Section S ON SI.Section_id = S.Id"
                                                           + "  Inner Join List_Item LI ON LI.Section_Item_id = SI.Id"
                                                           + "  Left Join Single_Question SQ ON SQ.Section_Item_id = SI.Id"
                                                           + "  Left Join Question Q ON Q.Section_Item_Id= SI.Id"
                                                           + "  Left Join [Comment] C ON C.Section_Item_id = SI.Id"
                                                           + "  Left Join List_Question LQ ON LQ.Section_Item_Id = SI.Id UNION "
                                                           + " Select  0 As Clinic_Number,@Service_Install_Id as Service_Install_Id,'' AS SectionItemOptionMaster_LocalDB_ID,'' AS SectionItemOption_WEB_Id, convert(varchar, SI.Id) +'_' + convert(varchar,SI.Type) AS SectionItemOption_EHR_Id,SI.Text AS SectionItemOptionName,convert(varchar,Q.Question_Type) AS Question_Type,convert(varchar,Q.Answer_Type) AS Answer_Type,convert(varchar,SI.Id) AS SectionItem_EHR_Id,convert(varchar,S.Id) AS Section_EHR_Id,convert(varchar,S.Form_id) AS Form_EHR_Id,'0' AS NumberOfColumns,GetDate() AS Entry_DateTime,GetDate() AS Last_Sync_Date,0 as Is_Adit_updated,0 AS Is_deleted From Section_Item SI Inner Join Section S ON SI.Section_id = S.Id Left Join Single_Question SQ ON SQ.Section_Item_id = SI.Id Left Join Question Q ON Q.Section_Item_Id= SI.Id Left Join [Comment] C ON C.Section_Item_id = SI.Id Left Join List_Question LQ ON LQ.Section_Item_Id = SI.Id WHERE SI.Type = 3 ";

        public static string GetEagleSoftFormMaster = "SELECT 0 As Clinic_Number,@Service_Install_Id as Service_Install_Id,'' AS FormMaster_LocalDB_ID,'' AS FormMaster_web_Id,convert(varchar,Id) AS Form_EHR_Id,Description AS FormName,active AS Is_Active,convert(varchar,Form_Type_id) AS Form_Type_Id,Is_Default,GetDate() AS Entry_DateTime,GetDate() AS Last_Sync_Date,"
                                                       + "  created_date AS EHR_Entry_DateTime,Last_Modified_date AS EHR_Modified_DateTime,0 AS Is_Adit_Updated,0 AS Is_deleted  FROM Form";

        public static string GetEagleSoftSectionMaster = "Select 0 As Clinic_Number,@Service_Install_Id as Service_Install_Id,'' AS SectionMaster_LocalDB_ID,'' AS SectionMaster_Web_Id,convert(varchar,Id) AS Section_EHR_Id,title AS SectionName,Show_Title,Show_Border,[index] AS Section_Order,convert(varchar,form_id) AS Form_EHR_Id,GetDate() AS Entry_DateTime,  "
                                                         + " GetDate() AS Last_Sync_Date,0 AS Is_Adit_Updated,0 AS is_deleted From Section";

        public static string GetPatientWiseFormEntry = "Select Form_Id AS FormMaster_EHR_Id , Patient_Id AS Patient_EHR_Id,MAX(Id) AS FormInstanceId From Form_instance Group by Form_Id ,Patient_Id";

        public static string GetPatientWiseFormEntryByPatID = "Select Form_Id AS FormMaster_EHR_Id , Patient_Id AS Patient_EHR_Id,MAX(Id) AS FormInstanceId From Form_instance  Where Patient_Id = '@Patient_EHR_ID' Group by Form_Id ,Patient_Id";

        public static string UpdateEaglesoftListItemAnswer = "UPDATE List_Item_Answer  SET answer_choice = @answer_choice WHERE instance_id = @instance_id and List_item_id = @list_item_id";

        public static string UpdateEaglesoftSingleQuestionAnswer = "UPDATE Single_Question_answer  SET answer_choice = @answer_choice,[comment] = @comment WHERE instance_id = @instance_id and section_item_id = @section_item_id";

        public static string UpdateEaglesoftSingleQuestionAnswerFeelFree = "UPDATE Single_Question_answer  SET answer_choice = @answer_choice,[comment] = @comment,Answer_text = @AnswerText WHERE instance_id = @instance_id and section_item_id = @section_item_id";

        public static string UpdateEaglesoftCommentAnswer = "UPDATE Comment_answer  SET Text = @commentAns WHERE instance_id = @instance_id and Section_item_id = @sectionitemId";

        public static string UpdateEaglesoftFormInstance = "UPDATE form_instance  SET [date] = getdate() WHERE id = @instance_id";

        public static string InsertEaglesoftListItemAnswer = "Insert into List_Item_Answer  (id,instance_id,answer_choice,list_item_id) values ((select max(id) + 1 from List_Item_Answer  ),@instance_id,@answer_choice,@List_item_id)";

        public static string InsertEaglesoftSingleQuestionAnswer = "Insert into Single_Question_answer  (id,instance_id,answer_choice,section_item_id,[comment]) values ((select max(id) + 1 from Single_Question_answer),@instance_id,@answer_choice,@section_item_id,@comment)";

        public static string InsertEaglesoftSingleQuestionAnswerFeelFree = "Insert into Single_Question_answer  (id,instance_id,answer_choice,Answer_text,section_item_id,[comment]) values ((select max(id) + 1 from Single_Question_answer),@instance_id,@answer_choice,@AnswerText,@section_item_id,@comment)";

        public static string InsertEaglesoftCommentAnswer = "Insert into Comment_answer (id,instance_id,section_item_id,text) values ((select max(id) + 1 from Comment_answer ),@instance_id,@Section_item_id,@commentAns)";

        public static string InsertFormInstance = "Insert into Form_Instance (Id,description,form_id,patient_id,provider_id,[date],entry_type) values ((select max(id) + 1 from form_instance),'',@formid,@patientId,(select preferred_dentist From Patient where Patient_id = @patientId),getdate(),0) SELECT CONVERT(NUMERIC(10,0),( MAX(ISNULL(id,0)) ) ) From Form_Instance ";

        public static string GetEagleSoftAlertMaster = "Select 0 As Clinic_Number,@Service_Install_Id as Service_Install_Id,''AS AlertMaster_LocalDB_ID,'' AS AlertMaster_web_Id, convert(varchar,Alert_Id) AS  Alert_EHR_Id, Description AS Alert_Name,0 AS Is_Adit_Updated,0 AS is_deleted FROM Alert";

        public static string GetPatientWiseRecallData = "select '' AS Patient_RecallType_LocalDB_ID, convert(varchar, PR.Patient_Id) + ''+convert(varchar, PR.Recall_Id)+''+ convert(varchar, year( next_recall_date))+''+ convert(varchar, month(next_recall_date))+''+convert(varchar, day(next_recall_date)) AS Patient_Recall_Id, PR.Next_recall_date AS Recall_Date,PR.last_recall_date AS Last_Recall_Date,PR.Patient_id AS Patient_EHR_id,'' AS Patient_Web_ID,PR.recall_id AS RecallType_EHR_ID,PR.Provider_Id AS Provider_EHR_ID,R.description AS RecallType_Name,'' AS RecallType_Descript,PR.default_recall_yn AS Default_Recall,getdate() AS Entry_DateTime,getdate() AS Last_Sync_Date,PR.date_added as  EHR_Entry_DateTime,0 AS Is_Adit_Updated,0 AS Clinic_Number,@Service_Install_Id AS Service_Install_Id,1 AS InsUptDlt,0 AS Is_Deleted From patient_recalls  PR INNER JOIN recalls R ON PR.Recall_id = R.Recall_Id ";

        // public static string SavePatientPayment_TransactionHeader = "IF NOT EXISTS ( select 1 from Transactions_header where User_Id= (Select Preferred_dentist from patient where Patient_id = @PatientEHRID ) and Tran_date=@PaymentDate and amount=-@Amount and paytype_id=@PayTypeId and description=@Note) BEGIN Insert Into Transactions_header (Tran_num,User_Id,Type,Tran_date,Resp_party_id,amount,service_code,paytype_id,sequence,statement_num,surface,fee,discount_surcharge,tax,description,defective,impacts,status,adjustment_type,claim_id,est_primary,est_secondary,paid_primary,paid_secondary,bulk_payment_num,aging_date,tooth,lab_fee,lab_fee2,lab_code,lab_code2,pre_fee,standard_fee_id,practice_id,procedure_type_codes,balance) values ((SELECT MAX(Tran_num)+1 FROM Transactions ),(Select Preferred_dentist from patient where Patient_id = @PatientEHRID ),'A',@PaymentDate,@PatientEHRID,-@Amount,NULL,@PayTypeId,'','','',0,0,0,@Note,'N','C','A',NULL,'',0,0,0,0,NULL,@PaymentDate,'',0,0,'','',0,'',1,'',0)  SElect @@Identity   END";

        public static string SavePatientPayment_TransactionHeader = "IF NOT EXISTS ( select 1 from Transactions_header where User_Id= (Select Preferred_dentist from patient where Patient_id = @PatientEHRID ) and Tran_date=@PaymentDate and amount=-@Amount and paytype_id=@PayTypeId and description=@Note) BEGIN Insert Into Transactions_header (Tran_num,User_Id,Type,Tran_date,Resp_party_id,amount,service_code,paytype_id,sequence,statement_num,surface,fee,discount_surcharge,tax,description,defective,impacts,status,adjustment_type,claim_id,est_primary,est_secondary,paid_primary,paid_secondary,bulk_payment_num,aging_date,tooth,lab_fee,lab_fee2,lab_code,lab_code2,pre_fee,standard_fee_id,practice_id,procedure_type_codes,balance) values ((SELECT MAX(Tran_num)+1 FROM Transactions ),(Select ISNULL((Select Preferred_dentist from patient where Patient_id = @PatientEHRID),(Select top 1 Preferred_dentist from patient)) ),'A',@PaymentDate,@PatientEHRID,-@Amount,NULL,@PayTypeId,'','','',0,0,0,@Note,'N','C','A',NULL,'',0,0,0,0,NULL,@PaymentDate,'',0,0,'','',0,'',1,'',0)  SElect @@Identity   END";
        //public static string SavePatientPayment_TransactionHeader = "IF NOT EXISTS ( select 1 from Transactions_header where User_Id= (Select Preferred_dentist from patient where Patient_id = ? ) and Tran_date=? and amount=? and paytype_id=? and description=?) BEGIN Insert Into Transactions_header (Tran_num,User_Id,Type,Tran_date,Resp_party_id,amount,service_code,paytype_id,sequence,statement_num,surface,fee,discount_surcharge,tax,description,defective,impacts,status,adjustment_type,claim_id,est_primary,est_secondary,paid_primary,paid_secondary,bulk_payment_num,aging_date,tooth,lab_fee,lab_fee2,lab_code,lab_code2,pre_fee,standard_fee_id,practice_id,procedure_type_codes,balance) values ((SELECT MAX(Tran_num)+1 FROM Transactions ),(Select ISNULL((Select Preferred_dentist from patient where Patient_id = ?),(Select top 1 Preferred_dentist from patient)) ),'A',?,?,?,NULL,?,'','','',0,0,0,?,'N','C','A',NULL,'',0,0,0,0,NULL,?,'',0,0,'','',0,'',1,'',0)  SElect @@Identity   END";

        public static string SavePatientPayment_TransactionHeaderForRefund = "IF NOT EXISTS ( select 1 from Transactions_header where User_Id= (Select Preferred_dentist from patient where Patient_id = @PatientEHRID ) and Tran_date=@PaymentDate and amount=@Amount and adjustment_type=@Adjustment_Type_Id and description=@Note and Type=@PaymentType) BEGIN Insert Into Transactions_header (Tran_num,User_Id,Type,Tran_date,Resp_party_id,amount,service_code,paytype_id,sequence,statement_num,surface,fee,discount_surcharge,tax,description,defective,impacts,status,adjustment_type,claim_id,est_primary,est_secondary,paid_primary,paid_secondary,bulk_payment_num,aging_date,tooth,lab_fee,lab_fee2,lab_code,lab_code2,pre_fee,standard_fee_id,practice_id,procedure_type_codes,balance) values ((SELECT MAX(Tran_num)+1 FROM Transactions ),@EHR_User_ID,@PaymentType,@PaymentDate,@PatientEHRID,@Amount,NULL,NULL,NULL,NULL,'',0,0,0,@Note,'N','C','A',@Adjustment_Type_Id,NULL,0,0,0,0,NULL,@PaymentDate,'',0,0,'','',0,NULL,1,'',0)  SElect @@Identity   END";

        public static string CheckPaymentModeExistAsAditPay = "IF NOT EXISTS ( select 1 from paytype where description= 'Adit Pay' ) BEGIN INSERT INTO paytype  (paytype_id,sequence,description,prompt,display_on_payment_screen,currency_type,include_on_deposit_yn,central_id,system_required) values ((select max(paytype_id) + 1 from paytype ),1,'Adit Pay','','Y','P','N',' ',0) select @@IDENTITY  END ELSE BEGIN  select paytype_id from paytype where description = 'Adit Pay' END";

        public static string CheckPaymentModeExistAsCareCredit = "IF NOT EXISTS ( select 1 from paytype where description= 'CareCredit' ) BEGIN INSERT INTO paytype  (paytype_id,sequence,description,prompt,display_on_payment_screen,currency_type,include_on_deposit_yn,central_id,system_required) values ((select max(paytype_id) + 1 from paytype ),1,'CareCredit','','Y','P','N',' ',0) select @@IDENTITY  END ELSE BEGIN  select paytype_id from paytype where description = 'CareCredit' END";

        public static string UpdatePaytypeFieldForDepositReport = "update paytype set include_on_deposit_yn='Y' where description= 'Adit Pay'";

        public static string CheckModeExistAsAditPayInAdjustmentType = "IF NOT EXISTS ( select 1 from adjustment_type where description= 'Adit Pay Refund' ) BEGIN INSERT INTO adjustment_type  (adjustment_type_id,description,impacts,central_id) values ((select MAX(adjustment_type_id)+1 from adjustment_type),'Adit Pay Refund','C',' ') select @@IDENTITY  END ELSE BEGIN  select adjustment_type_id from adjustment_type where description = 'Adit Pay Refund' END";

        public static string CheckModeExistAsCareCreditInAdjustmentType = "IF NOT EXISTS ( select 1 from adjustment_type where description= 'CareCredit Refund' ) BEGIN INSERT INTO adjustment_type  (adjustment_type_id,description,impacts,central_id) values ((select MAX(adjustment_type_id)+1 from adjustment_type),'CareCredit Refund','C',' ') select @@IDENTITY  END ELSE BEGIN  select adjustment_type_id from adjustment_type where description = 'CareCredit Refund' END";

        public static string CheckModeExistAsDiscountType = "IF NOT EXISTS ( select 1 from adjustment_type where description= 'Adit Pay Discount' ) BEGIN INSERT INTO adjustment_type  (adjustment_type_id,description,impacts,central_id) values ((select MAX(adjustment_type_id)+1 from adjustment_type),'Adit Pay Discount','C',' ') select @@IDENTITY  END ELSE BEGIN  select adjustment_type_id from adjustment_type where description = 'Adit Pay Discount' END";

        public static string CheckModeExistAsCareCreditDiscountType = "IF NOT EXISTS ( select 1 from adjustment_type where description= 'CareCredit Discount' ) BEGIN INSERT INTO adjustment_type  (adjustment_type_id,description,impacts,central_id) values ((select MAX(adjustment_type_id)+1 from adjustment_type),'CareCredit Discount','C',' ') select @@IDENTITY  END ELSE BEGIN  select adjustment_type_id from adjustment_type where description = 'CareCredit Discount' END";

        public static string SavePatientPayment_TransactionDetails = "Insert into transactions_detail (detail_id,tran_num,user_id,date_entered,provider_id,collections_go_to,patient_id,amount,provider_practice_id,patient_practice_id,applied_to,status,status_modifier,posneg) values ((select max(detail_id)+1 from transactions_detail ),@TransactionHeaderId,(Select ISNULL((Select Preferred_dentist from patient where Patient_id = @PatientEHRID),(Select top 1 Preferred_dentist from patient)) ),@PaymentDate,(Select Preferred_dentist from patient where Patient_id = @PatientEHRID ),(Select Preferred_dentist from patient where Patient_id = @PatientEHRID ),@PatientEHRID,@Amount,1,1,NULL,1,NULL,0 )";

        public static string SavePatientPayment_TransactionDetails_Refund = "Insert into transactions_detail (detail_id,tran_num,user_id,date_entered,provider_id,collections_go_to,patient_id,amount,provider_practice_id,patient_practice_id,applied_to,status,status_modifier,posneg) values ((select max(detail_id)+1 from transactions_detail ),@TransactionHeaderId,@EHR_User_ID,@PaymentDate,(Select Preferred_dentist from patient where Patient_id = @PatientEHRID ),(Select Preferred_dentist from patient where Patient_id = @PatientEHRID ),@PatientEHRID,@Amount,1,1,(SELECT top 1 tran_num FROM Transactions_detail  WHERE amount > 0  and patient_id = @PatientEHRID Order by date_entered DESC ),1,NULL,0 )";

        public static string DeleteDuplicateTransactionDetails = " Delete from transactions_detail   where detail_id IN ( select DetailsId FROM ( select MAX(detail_id) AS DetailsId, tran_num,provider_Id,patient_id,amount,applied_to,status,count(1) AS cnt from transactions_detail where tran_num = @TransactionHeaderId group by tran_num,provider_Id,patient_id,amount,applied_to,status having count(1) > 1 order by tran_num ) AS AA )";

        // public static string UpdatePatientBalance = "Update patient set current_bal = current_bal + ( ? )  where patient_id = ? "; //current_bal + ( @PaymentAmount + @DiscountAmount - @RefundAmount
        //
        public static string UpdatePatientBalance = "update patient set current_bal = isnull((select sum(amount) from transactions_detail where patient_id = ?),0)  where patient_id = ?";

        public static string SavePatientPayment_TransactionExtra = " Insert into Transactions_extra (tran_num,tooth_data,surface_detail,abutment_direction,root_data,show_on_chart,draw_order_data) values (@TransactionHeaderId,'NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN','NNNNNNNNNNNNNNNNNNNNNNN',-1,'','','')";

        public static string DeleteTransactionHeader = " Delete from Transactions_header where Tran_Num = @TransactionHeaderId ";

        public static string DeleteTransactionDetails = " Delete from transactions_detail where Tran_Num = @TransactionHeaderId ";

        public static string InsertPatientPaymentLog = " if not exists ( select 1 from operatory_notes  where patient_id = @PatientEHRId AND date_entered = @date_entered AND note_type = @note_type AND note_type_id = @notetypeid AND description = @NoteDescription  and note = @Note ) begin "
                                                + "  INSERT INTO operatory_notes (note_id,patient_id,date_entered,user_id,note_class,note_type,note_type_id,description,note,color,post_proc_status,date_modified,modified_by,locked_eod,status,"
                                                + " tooth_data,claim_id,statement_yn,resp_party_id,tooth,tran_num,archive_name,archive_path,service_code,practice_id,freshness) values (((Select MAX( Note_id) FROM operatory_notes) + 1),@PatientEHRId,@date_entered,@EHR_User_ID,"
                                                + " 'T',@note_type,@notetypeid,@NoteDescription,@Note,0,'V',NULL,@EHR_User_ID,0,'A',NULL,1,'N',@PatientEHRId,'',0,'','','',1,getdate()) end  Select @@Identity "; //'A',4,

        public static string DeletePatientLogBlankMobile = " Delete from operatory_notes  Where patient_id = @PatientEHRId AND date_entered > '20211105' AND Note_Type = @note_type and Note_Type_Id = @notetypeid AND Note = @Note AND description = @description AND date_entered = @date_entered AND ( note like  'SMS sent -%' OR Note like 'SMS received -%' OR Note like 'SMS received -%' OR Note like 'Inbound call answered - Duration:%' OR Note like 'Outbound call%Duration:%' ) ";

        public static string DeletePatientLogMobileExists = " Delete from operatory_notes  Where patient_id = @PatientEHRId AND date_entered > '20211105' AND Note_Type = 'G' and Note_Type_Id = 7 AND Note = @Note AND description = @description AND date_entered = @date_entered AND Note_id != @NoteId AND ( note like  'SMS sent -%' OR Note like 'SMS received -%' OR Note like 'SMS received -%' OR Note like 'Inbound call answered - Duration:%' OR Note like 'Outbound call%Duration:%' ) ";

        public static string GetPatientStatus = "select Patient_id as Patient_EHR_Id from Patient as PT where PT.Status = 'A' and PT.Patient_id not in (select PT.Patient_id from Patient as PT  Inner Join Appointment AP  On PT.Patient_id = AP.Patient_id where AP.Start_time < getdate() and AP.classification = '1' and AP.arrival_status = 4) ORDER BY Patient_id";

        public static string GetPatientStatusByPatID = "select Patient_id as Patient_EHR_Id from Patient as PT where PT.Status = 'A' and PT.Patient_id not in (select PT.Patient_id from Patient as PT  Inner Join Appointment AP  On PT.Patient_id = AP.Patient_id where AP.Start_time < getdate() and AP.classification = '1' and AP.arrival_status = 4) And Patient_id = '@Patient_EHR_ID' ORDER BY Patient_id";

        public static string GetDuplicatePatientLog = " select MAX(OP.Note_id) AS NoteId, OP.patient_id AS PatientEHRId, OP.date_entered AS PatientSMSCallLogDate,OP.note_type,OP.note_type_id,OP.description AS app_alias,OP.note AS text,COUNT(1) AS CNT,PT.Cell_phone AS Mobile from operatory_notes OP INNER JOIN Patient PT ON OP.Patient_id = PT.Patient_id where OP.date_entered > '20211105' AND OP.Note_Type = 'G' and OP.Note_Type_Id = 7 AND ( note like  'SMS sent -%' OR Note like 'SMS received -%' OR Note like 'SMS received -%' OR Note like 'Inbound call answered - Duration:%' OR Note like 'Outbound call%Duration:%' ) and op.patient_id IN (19418)  GROUP BY OP.patient_id,OP.date_entered,OP.note_type,OP.note_type_id,OP.description,OP.note,PT.Cell_phone HAVING COUNT(1) > 1 UNION "
                                                        + " select MAX(OP.Note_id) AS NoteId, OP.patient_id, OP.date_entered,OP.note_type,OP.note_type_id,OP.description,OP.note,COUNT(1) AS CNT,PT.Cell_phone AS Mobile from operatory_notes OP INNER JOIN Patient PT ON OP.Patient_id = PT.Patient_id where  ( PT.cell_phone = '' Or PT.Cell_phone is null) AND OP.date_entered > '20211105' AND OP.Note_Type = 'G' and OP.Note_Type_Id = 7 AND ( note like  'SMS sent -%' OR Note like 'SMS received -%' OR Note like 'SMS received -%' OR Note like 'Inbound call answered - Duration:%' OR Note like 'Outbound call%Duration:%' ) and op.patient_id IN (19418) GROUP BY OP.patient_id,OP.date_entered,OP.note_type,OP.note_type_id,OP.description,OP.note,PT.Cell_phone  ";

        public static string GetEaglesoftPatientImagesData = "select 0 as Patient_Images_LocalDB_ID,'' as Patient_Images_Web_ID,p.Patient_Image_id as Patient_Images_EHR_ID,p.patient_id as Patient_EHR_ID,'' as Patient_Web_ID,'' as Image_EHR_Name , convert(varchar,p.patient_id) + '~' + Convert(varchar,p.Patient_Image_id) + '.EOP' as Patient_Images_FilePath,0 as Is_Deleted,0 as Is_Adit_Updated,getdate() as Entry_DateTime,now() as AditApp_Entry_DateTime ,'0' as Clinic_Number,'1' as Service_Install_Id from Patient as p inner join images as i on i.image_id = p.Patient_image_id";

        //rooja insert holidays to ehr
        public static string InsertHolidays = " INSERT INTO holidays (hol_date,practice_id,hol_name) "
                                              + "  VALUES (@hol_date,@practice_id,@hol_name)";

        public static string InsertMedication = "Insert into prescription_drugs(description,refills,allow_generic,dispense) Values ('@MedicationName',0,'Y',0);Select @@Identity as MedicationNum;";

        public static string InsertPatientMedication = "Insert into prescriptions (patient_id, drug_id, refills, allow_generic, rx_instructions, patient_instructions, description, provider_id, eod, alert_yn, entered_by, daw, dispense, date_entered) Values(@Patient_EHR_ID, @MedicationNum, 999, 'Y', '@Medication_Note', '@Medication_Note', '', @Provider_EHR_ID, 1, 'Y', '@Entered_By', 'N', '0', now());Select @@Identity as PatientMedicationNum;";

        public static string UpdatePatientMedicationNotes = "Update prescriptions set patient_instructions = '@Medication_Note', drug_id = @MedicationNum where rx_id = @PatientMedication_EHR_ID ";

        public static string DeletePatientMedication = "Update prescriptions Set status = 'E' where rx_id = @PatientMedication_EHR_ID ";

        //rooja 
        public static string GetEaglesoftInsuranceData = "SELECT *,'' as Clinic_Number from insurance_company; ";
    }
}
