using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.QRY
{
    public class SynchPracticeWorkQRY
    {
        public static string GetPracticeWorkProviderData = "select 0 as Clinic_Number,1 as Service_Install_Id,0 AS Provider_LocalDB_ID," + "\"Employee Id\"" + " AS Provider_EHR_ID,'' AS Provider_Web_ID," + "\"First Name\"" + " AS First_Name," + "\"Last Name\"" + " AS Last_Name,'Mid init' AS MI,'' AS gender, '' AS provider_speciality,'' AS bio,Degree AS education,'' AS accreditation,'' AS membership,'' AS language,'' AS age_treated_min,'' AS age_treated_max,(case when InActive =1  then 0 else 1 end) AS is_active ,0 AS Is_Adit_Updated from " + "\"Employee list\"" + " where inactive = 0 "; //AND " + "\"Employee type\"" + " IN (2,5) 

        public static string GetPracticeWorkOperatoryEventData = "Select 0 as Clinic_Number,1 as Service_Install_Id,0 AS OE_LocalDB_ID,'' AS OE_EHR_ID,'' AS OE_Web_ID,BD.ResourceId AS Operatory_EHR_ID,BD.BlockDate AS StartTime,BD.BlockDate AS EndTime, BD.StartTime AS StartTimeS,BD.EndTime AS EndTimeE,BBD.Description AS Comment,CurDate() AS Last_Sync_Date, CUrDate() AS Entry_DateTime, 0 AS Is_Adit_Updated, 0 AS Is_Deleted,BD.BlockDate,1 AS InsUptDlt FROM " + "\"Block Book Detail\"" + "  BD INNER JOIN " + "\"Block Definitions\"" + " BBD ON BBD." + "\"Block def Id\"" + " = BD.DefinitionId WHERE BD.BlockDate > ?";

        public static string GetMaxPatientIdFromPatient = " Select " + "\"Next Person ID\"" + "  from " + "\"System Info\"" + "";

        public static string GetMaxAppointmentId = " Select " + "\"Next Visit ID\"" + "  from " + "\"System Info\"" + "";

        public static string GetDefaultProvider = " SELECT " + "\"Employee Id\"" + " AS Provider_EHR_ID FROM " + "\"Employee list\"" + " where inactive = 0";

        public static string GetBookOperatoryAppointmenetWiseDateTime = "Select 0 as Clinic_Number,1 as Service_Install_Id,AP." + "\"Resource ID\"" + " AS Operatory_EHR_ID,AP.Date AS Appt_DateTime,AP.Date AS Appt_EndDateTime,  AP." + "\"Start Time\"" + ",AP." + "\"End Time\"" + " FROM " + "\"Appointments\"" + " AP where AP.Date > ? and AP.Date < ? ";

        public static string GetPracticeWorkPatientDataList = "SELECT 0 as Clinic_Number,1 as Service_Install_Id, PF." + "\"Person ID\"" + " AS Patient_EHR_ID, PF." + "\"Last name\"" + " + ' ' +PF." + "\"First name\"" + " AS Patient_Name, PF." + "\"First Name\"" + " AS FirstName,PF." + "\"Last Name\"" + " AS LastName, "
                                                   + " replace(@FillerCondition,'-','')  AS Mobile,replace(pff.\"Home Phone\",'-','')" + " AS Home_Phone,PF." + "\"Lives w/ Patient ID\"" + " as responsible_party,replace(PFF." + "\"Work Phone 1\",'-','')" + " AS Work_Phone,PFF." + "\"Email address\"" + " AS Email "   //( CASE WHEN  pff." + "\"Filler\"" + " = '' THEN pff.cellphone ELSE pff." + "\"Filler\"" + " END )
                                                   + " FROM " + "\"Patient File\"" + " PF LEFT JOIN " + "\"Person File\"" + " PFF ON pf." + "\"person ID\"" + " = PFF." + "\"Person ID\"" + ""
                                                   + " order by pf." + "\"Last Name\"" + ",pf." + "\"first name\"" + "";

        public static string GetPracticeWorkPatientData = "SELECT  0 as Clinic_Number,1 as Service_Install_Id,0 AS Patient_LocalDB_ID,PFF." + "\"Person ID\"" + " AS Patient_EHR_ID,'' AS Patient_Web_ID,PFF." + "\"First Name\"" + " AS First_Name,PFF." + "\"Last Name\"" + " AS Last_name,"
                                                   + " PFF." + "\"Mid init\"" + " AS Middle_Name,PFF.Title AS Salutation, CASE WHEN PFF." + "\"Person Type\"" + " IN  (@persenType) THEN 'I' ELSE 'A' END AS Status,case when  pf." + "\"Last Name\"" + " is not null AND  PFF." + "\"Person Type\"" + " NOT IN  (@persenType)  THEN 'Active' when  pf." + "\"Last Name\"" + " is not null AND  PFF." + "\"Person Type\"" + " IN  (@persenType)  THEN 'InActive' else 'NonPatient' END AS EHR_Status,'Y' as ReceiveSms,'Y' as ReceiveEmail,case when PFF.Sex = 0 then 'Male' else 'Female' END AS Sex,CASE WHEN PFF." + "\"Marital status\"" + " = 0 THEN 'Single' WHEN  PFF." + "\"Marital status\"" + " = 1 THEN 'Married' WHEN PFF." + "\"Marital status\"" + " = 2 then 'Divorced' ELSE 'Widow' END as MaritalStatus,PFF.birthdate AS Birth_Date,PFF." + "\"Email address\"" + " AS Email,"
                                                   //+ " PFF." + "\"Mid init\"" + " AS Middle_Name,PFF.Title AS Salutation, CASE WHEN PFF." + "\"Person Type\"" + " IN  (37,65,66,69,70,74,81,78,82,102,113,84,85,86,90,92,94,116,117,118,126) THEN 'I' ELSE 'A' END AS Status, CASE WHEN PFF." + "\"Person Type\"" + " IN  (37,65,66,69,70,74,81,78,82,102,113,84,85,86,90,92,94,116,117,118,126) THEN 'INACTIVE' ELSE 'ACTIVE' END AS EHR_Status,'Y' as ReceiveSms,'Y' as ReceiveEmail,case when PFF.Sex = 0 then 'Male' else 'Female' END AS Sex,CASE WHEN PFF." + "\"Marital status\"" + " = 0 THEN 'Single' WHEN  PFF." + "\"Marital status\"" + " = 1 THEN 'Married' WHEN PFF." + "\"Marital status\"" + " = 2 then 'Divorced' ELSE 'Widow' END as MaritalStatus,PFF.birthdate AS Birth_Date,PFF." + "\"Email address\"" + " AS Email,"
                                                   + " @FillerCondition AS Mobile,pff." + "\"Home Phone\"" + " AS Home_Phone,PFF." + "\"Work Phone 1\"" + " AS Work_Phone,PFF.Address AS Address1,PFF." + "\"Address 2\"" + " AS Address2," //( CASE WHEN  pff." + "\"Filler\"" + " = '' THEN pff.cellphone ELSE pff." + "\"Filler\"" + " END )  
                                                   + " PFF." + "\"City\"" + " AS City,PFF.State,PFF.ZIP AS Zipcode,'' AS ResponsibleParty_Status,AR." + "\"AR Current\"" + " AS CurrentBal,AR." + "\"AR 30\"" + " AS ThirtyDay,AR." + "\"AR 60\"" + " AS SixtyDay,AR." + "\"AR 90\"" + " AS NinetyDay,AR." + "\"AR 120\"" + " AS Over90,"
                                                   + " PF." + "\"First Treated\"" + " AS FirstVisit_Date,PF." + "\"Last in date\"" + " AS LastVisit_Date,"
                                                   + " PFF." + "\"Prim Ins Plan ID\"" + " AS Primary_Insurance,"
                                                   + " IPI." + "\"Employer Name\"" + " AS Primary_Insurance_CompanyName,"
                                                   + " PFF." + "\"PriminsId\"" + " AS Primary_Ins_Subscriber_ID,"
                                                   + " PFF." + "\"Sec Ins Plan ID\"" + " AS Secondary_Insurance,"
                                                   + " IPI2." + "\"Employer Name\"" + " AS Secondary_Insurance_CompanyName,"
                                                   + " PFF." + "\"SecinsId\"" + " AS Secondary_Ins_Subscriber_ID,1 AS InsUptDlt,PFF.LegalName as preferred_name,PF." + "\"Lives w/ Patient ID\"" + " as Guar_ID,PF." + "\"Provider Emp ID\"" + "  as Pri_Provider_ID, '' as Sec_Provider_ID , AA.NextAppointmentDate as nextvisit_date ,CASE WHEN PF." + "\"Next recall date\"" + "  is not null THEN PF." + "\"Next recall date\"" + " WHEN PF." + "\"recall cycle days\"" + "   is not null and PF." + "\"Last recall date\"" + "  is not null THEN (dateadd(month, PF." + "\"recall cycle days\"" + " ,PF." + "\"last recall date\"" + " ) ) ELSE ' ' END as Due_Date , 0 as remaining_benefit , 0 as used_benefit , 0 as collect_payment,'Y' as ReceiveVoiceCall,'English' as PreferredLanguage,'' as Patient_Note, " //case when \"Daytime phone\" = 0 then 'N' else 'Y' end ReceiveVoiceCall "
                                                   + " PFF.SSN as ssn, '' as driverlicense,'' as groupid,'' as emergencycontactId ,'' as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name,'' as emergencycontactnumber,PF.\"School name\" as school,'' as employer, PFF.SpouseID as spouseId,PF.\"Respons party Pat ID\" as responsiblepartyId ,RESPP.birthdate as responsiblepartybirthdate ,RESPP.SSN as responsiblepartyssn ,RESPP." + "\"First Name\"" + " AS ResponsibleParty_First_Name,RESPP." + "\"Last Name\"" + " AS ResponsibleParty_Last_Name, "
                                                   + " IPI." + "\"Employer phone\"" + " AS Prim_Ins_Company_Phonenumber,IPI2." + "\"Employer phone\"" + " AS Sec_Ins_Company_Phonenumber , RESPP." + "\"First Name\"" + " AS ResponsibleParty_First_Name,RESPP.\"Last Name\" AS ResponsibleParty_Last_Name ,SPS." + "\"First Name\"" + " as Spouse_First_Name, SPS.\"Last Name\"" + "as Spouse_Last_Name "                                           
                                                   + " FROM " + "\"Person File\"" + " PFF LEFT JOIN " + "\"Patient File\"" + " PF ON pf." + "\"person ID\"" + " = PFF." + "\"Person ID\"" + ""
                                                   + " LEFT JOIN " + "\"Ins Plan Info\"" + " IPI ON PFF." + "\"Prim Ins Plan ID\"" + "  = IPI." + "\"Ins Plan Id\"" + ""
                                                   + " LEFT JOIN " + "\"Ins Plan Info\"" + " IPI2 ON PFF." + "\"Sec Ins Plan ID\"" + "  = IPI2." + "\"Ins Plan Id\"" + ""
                                                   + " LEFT JOIN " + "\"AR Info\"" + " AR ON AR." + "\"Person Id\"" + "  = PFF." + "\"Person Id\"" + ""
                                                   + " LEFT JOIN   ( select MIN(" + "\"date\"" + ") AS NextAppointmentDate," + "\"patient id\"" + " AS PatientEHRID  from appointments where Date >= curdate() group by " + "\"patient id\"" + ") AS AA ON AA.PatientEHRID = PF." + "\"person ID\"" + ""
                                                   + " LEFT JOIN   ( select MIN(" + "\"date\"" + ") AS NextAppointmentDate," + "\"patient id\"" + " AS PatientEHRID  from appointments where Date >= curdate() and " + "\"Recall Appointment\"" + " = 1 group by " + "\"patient id\"" + ") AS AB ON AB.PatientEHRID = PF." + "\"person ID\"" + ""
                                                   + " LEFT JOIN (Select SSN, birthdate,\"Person ID\" as PatientID,\"First Name\",\"Last Name\" from \"Person File\" ) as RESPP ON RESPP.PatientID = PF." + "\"Respons party Pat ID\"" + ""      
            + " LEFT JOIN (Select \"Person ID\" as PatientID,\"First Name\",\"Last Name\" from \"Person File\" ) as SPS ON SPS.PatientID = PFF." + "SpouseID" + ""
                                                   + " order by pf." + "\"Last Name\"" + ",pf." + "\"first name\"" + "";

        public static string GetPracticeWorkNewAllPatientData = "SELECT  0 as Clinic_Number,1 as Service_Install_Id,0 AS Patient_LocalDB_ID,PFF." + "\"Person ID\"" + " AS Patient_EHR_ID,'' AS Patient_Web_ID,PFF." + "\"First Name\"" + " AS First_Name,PFF." + "\"Last Name\"" + " AS Last_name,"
                                                   + " PFF." + "\"Mid init\"" + " AS Middle_Name,PFF.Title AS Salutation, CASE WHEN PFF." + "\"Person Type\"" + " IN  (@persenType) THEN 'I' ELSE 'A' END AS Status,case when  pf." + "\"Last Name\"" + " is not null AND  PFF." + "\"Person Type\"" + " NOT IN  (@persenType)  THEN 'Active' when  pf." + "\"Last Name\"" + " is not null AND  PFF." + "\"Person Type\"" + " IN  (@persenType)  THEN 'InActive' else 'NonPatient' END AS EHR_Status,'Y' as ReceiveSms,'Y' as ReceiveEmail,case when PFF.Sex = 0 then 'Male' else 'Female' END AS Sex,CASE WHEN PFF." + "\"Marital status\"" + " = 0 THEN 'Single' WHEN  PFF." + "\"Marital status\"" + " = 1 THEN 'Married' WHEN PFF." + "\"Marital status\"" + " = 2 then 'Divorced' ELSE 'Widow' END as MaritalStatus,PFF.birthdate AS Birth_Date,PFF." + "\"Email address\"" + " AS Email,"
                                                   //+ " PFF." + "\"Mid init\"" + " AS Middle_Name,PFF.Title AS Salutation, CASE WHEN PFF." + "\"Person Type\"" + " IN  (37,65,66,69,70,74,81,78,82,102,113,84,85,86,90,92,94,116,117,118,126) THEN 'I' ELSE 'A' END AS Status, CASE WHEN PFF." + "\"Person Type\"" + " IN  (37,65,66,69,70,74,81,78,82,102,113,84,85,86,90,92,94,116,117,118,126) THEN 'INACTIVE' ELSE 'ACTIVE' END AS EHR_Status,'Y' as ReceiveSms,'Y' as ReceiveEmail,case when PFF.Sex = 0 then 'Male' else 'Female' END AS Sex,CASE WHEN PFF." + "\"Marital status\"" + " = 0 THEN 'Single' WHEN  PFF." + "\"Marital status\"" + " = 1 THEN 'Married' WHEN PFF." + "\"Marital status\"" + " = 2 then 'Divorced' ELSE 'Widow' END as MaritalStatus,PFF.birthdate AS Birth_Date,PFF." + "\"Email address\"" + " AS Email,"
                                                   + " @FillerCondition AS Mobile,pff." + "\"Home Phone\"" + " AS Home_Phone,PFF." + "\"Work Phone 1\"" + " AS Work_Phone,PFF.Address AS Address1,PFF." + "\"Address 2\"" + " AS Address2," //( CASE WHEN  pff." + "\"Filler\"" + " = '' THEN pff.cellphone ELSE pff." + "\"Filler\"" + " END )  
                                                   + " PFF." + "\"City\"" + " AS City,PFF.State,PFF.ZIP AS Zipcode,'' AS ResponsibleParty_Status,AR." + "\"AR Current\"" + " AS CurrentBal,AR." + "\"AR 30\"" + " AS ThirtyDay,AR." + "\"AR 60\"" + " AS SixtyDay,AR." + "\"AR 90\"" + " AS NinetyDay,AR." + "\"AR 120\"" + " AS Over90,"
                                                   + " PF." + "\"First Treated\"" + " AS FirstVisit_Date,PF." + "\"Last in date\"" + " AS LastVisit_Date,"
                                                   + " PFF." + "\"Prim Ins Plan ID\"" + " AS Primary_Insurance,"
                                                   + " IPI." + "\"Employer Name\"" + " AS Primary_Insurance_CompanyName,"
                                                   + " PFF." + "\"PriminsId\"" + " AS Primary_Ins_Subscriber_ID,"
                                                   + " PFF." + "\"Sec Ins Plan ID\"" + " AS Secondary_Insurance,"
                                                   + " IPI2." + "\"Employer Name\"" + " AS Secondary_Insurance_CompanyName,"
                                                   + " PFF." + "\"SecinsId\"" + " AS Secondary_Ins_Subscriber_ID,1 AS InsUptDlt,PFF.LegalName as preferred_name,PF." + "\"Lives w/ Patient ID\"" + " as Guar_ID,PF." + "\"Provider Emp ID\"" + "  as Pri_Provider_ID, '' as Sec_Provider_ID , AA.NextAppointmentDate as nextvisit_date , CASE WHEN PF." + "\"Next recall date\"" + "  is not null THEN PF." + "\"Next recall date\"" + " WHEN PF." + "\"recall cycle days\"" + "   is not null and PF." + "\"Last recall date\"" + "  is not null THEN (dateadd(month, PF." + "\"recall cycle days\"" + " ,PF." + "\"last recall date\"" + " ) ) ELSE ' ' END as Due_Date , 0 as remaining_benefit , 0 as used_benefit , 0 as collect_payment,'Y' as ReceiveVoiceCall,'English' as PreferredLanguage,'' as Patient_Note, " //case when \"Daytime phone\" = 0 then 'N' else 'Y' end ReceiveVoiceCall "
                                                   + " PFF.SSN as ssn, '' as driverlicense,'' as groupid,'' as emergencycontactId ,'' as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name,'' as emergencycontactnumber,PF.\"School name\" as school,'' as employer, PFF.SpouseID as spouseId,PF.\"Respons party Pat ID\" as responsiblepartyId ,RESPP.birthdate as responsiblepartybirthdate ,RESPP.SSN as responsiblepartyssn ,RESPP." + "\"First Name\"" + " AS ResponsibleParty_First_Name,RESPP." + "\"Last Name\"" + " AS ResponsibleParty_Last_Name, "
                                                   + " IPI." + "\"Employer phone\"" + " AS Prim_Ins_Company_Phonenumber,IPI2." + "\"Employer phone\"" + " AS Sec_Ins_Company_Phonenumber , RESPP." + "\"First Name\"" + " AS ResponsibleParty_First_Name,RESPP.\"Last Name\" AS ResponsibleParty_Last_Name ,SPS." + "\"First Name\"" + " as Spouse_First_Name, SPS.\"Last Name\"" + "as Spouse_Last_Name "
                                                   + " FROM " + "\"Person File\"" + " PFF LEFT JOIN " + "\"Patient File\"" + " PF ON pf." + "\"person ID\"" + " = PFF." + "\"Person ID\"" + ""
                                                   + " LEFT JOIN " + "\"Ins Plan Info\"" + " IPI ON PFF." + "\"Prim Ins Plan ID\"" + "  = IPI." + "\"Ins Plan Id\"" + ""
                                                   + " LEFT JOIN " + "\"Ins Plan Info\"" + " IPI2 ON PFF." + "\"Sec Ins Plan ID\"" + "  = IPI2." + "\"Ins Plan Id\"" + ""
                                                   + " LEFT JOIN " + "\"AR Info\"" + " AR ON AR." + "\"Person Id\"" + "  = PFF." + "\"Person Id\"" + ""
                                                   + " LEFT JOIN   ( select MIN(" + "\"date\"" + ") AS NextAppointmentDate," + "\"patient id\"" + " AS PatientEHRID  from appointments where Date >= curdate() group by " + "\"patient id\"" + ") AS AA ON AA.PatientEHRID = PF." + "\"person ID\"" + ""
                                                   + " LEFT JOIN   ( select MIN(" + "\"date\"" + ") AS NextAppointmentDate," + "\"patient id\"" + " AS PatientEHRID  from appointments where Date >= curdate() and " + "\"Recall Appointment\"" + " = 1 group by " + "\"patient id\"" + ") AS AB ON AB.PatientEHRID = PF." + "\"person ID\"" + ""
                                                   + " LEFT JOIN (Select SSN, birthdate,\"Person ID\" as PatientID,\"First Name\",\"Last Name\" from \"Person File\" ) as RESPP ON RESPP.PatientID = PF." + "\"Respons party Pat ID\"" + ""
                                                   + " LEFT JOIN (Select \"Person ID\" as PatientID,\"First Name\",\"Last Name\" from \"Person File\" ) as SPS ON SPS.PatientID = PFF." + "SpouseID" + " where PFF.\"Person ID\" in (@PaientEHRIDs)"
                                                   + " order by pf." + "\"Last Name\"" + ",pf." + "\"first name\"" + "";


        public static string GetPracticeWorkNewPatientData = "SELECT  0 as Clinic_Number,1 as Service_Install_Id," + "\"Person ID\"" + " AS Patient_EHR_ID FROM " + "\"Person File\"";

        public static string GetPracticeWorkAppointmentsPatientData = "SELECT DISTINCT  0 AS Patient_LocalDB_ID,PFF." + "\"Person ID\"" + " AS Patient_EHR_ID,'' AS Patient_Web_ID,PFF." + "\"First Name\"" + " AS First_Name,PFF." + "\"Last Name\"" + " AS Last_name,"
                                           + " PFF." + "\"Mid init\"" + " AS Middle_Name,PFF.Title AS Salutation, CASE WHEN PFF." + "\"Person Type\"" + " IN   (@persenType) THEN 'I' ELSE 'A' END AS Status, case when  pf." + "\"Last Name\"" + " is not null AND  PFF." + "\"Person Type\"" + " NOT IN  (@persenType)  THEN 'Active' when  pf." + "\"Last Name\"" + " is not null AND  PFF." + "\"Person Type\"" + " IN  (@persenType)  THEN 'InActive' else 'NonPatient' END AS EHR_Status,'Y' as ReceiveSms,'Y' as ReceiveEmail,case when PFF.Sex = 0 then 'Male' else 'Female' END AS Sex,CASE WHEN PFF." + "\"Marital status\"" + " = 0 THEN 'Single' WHEN  PFF." + "\"Marital status\"" + " = 1 THEN 'Married' WHEN PFF." + "\"Marital status\"" + " = 2 then 'Divorced' ELSE 'Widow' END as MaritalStatus,PFF.birthdate AS Birth_Date,PFF." + "\"Email address\"" + " AS Email,"
                                           //+ " PFF." + "\"Mid init\"" + " AS Middle_Name,PFF.Title AS Salutation, CASE WHEN PFF." + "\"Person Type\"" + " IN   (37,65,66,69,70,74,78,82,102,113,81,84,85,86,90,92,94,116,117,118,126) THEN 'I' ELSE 'A' END AS Status, CASE WHEN PFF." + "\"Person Type\"" + " IN   (37,65,66,69,70,74,81,78,82,102,113,84,85,86,90,92,94,116,117,118,126) THEN 'INACTIVE' ELSE 'ACTIVE' END AS EHR_Status,'Y' as ReceiveSms,'Y' as ReceiveEmail,case when PFF.Sex = 0 then 'Male' else 'Female' END AS Sex,CASE WHEN PFF." + "\"Marital status\"" + " = 0 THEN 'Single' WHEN  PFF." + "\"Marital status\"" + " = 1 THEN 'Married' WHEN PFF." + "\"Marital status\"" + " = 2 then 'Divorced' ELSE 'Widow' END as MaritalStatus,PFF.birthdate AS Birth_Date,PFF." + "\"Email address\"" + " AS Email,"
                                           + " @FillerCondition AS Mobile,pff." + "\"Home Phone\"" + " AS Home_Phone,PFF." + "\"Work Phone 1\"" + " AS Work_Phone,PFF.Address AS Address1,PFF." + "\"Address 2\"" + " AS Address2,"//( CASE WHEN  pff." + "\"Filler\"" + " = '' THEN pff.cellphone ELSE pff." + "\"Filler\"" + " END ) 
                                           + " PFF." + "\"City\"" + " AS City,PFF.State,PFF.ZIP AS Zipcode,'' AS ResponsibleParty_Status,AR." + "\"AR Current\"" + " AS CurrentBal,AR." + "\"AR 30\"" + " AS ThirtyDay,AR." + "\"AR 60\"" + " AS SixtyDay,AR." + "\"AR 90\"" + " AS NinetyDay,AR." + "\"AR 120\"" + " AS Over90,"
                                           + " PF." + "\"First Treated\"" + " AS FirstVisit_Date,PF." + "\"Last in date\"" + " AS LastVisit_Date,"
                                           + " PFF." + "\"Prim Ins Plan ID\"" + " AS Primary_Insurance,"
                                           + " IPI." + "\"Employer Name\"" + " AS Primary_Insurance_CompanyName,"
                                           + " PFF." + "\"PriminsId\"" + " AS Primary_Ins_Subscriber_ID,"
                                           + " PFF." + "\"Sec Ins Plan ID\"" + " AS Secondary_Insurance,"
                                           + " IPI2." + "\"Employer Name\"" + " AS Secondary_Insurance_CompanyName,"
                                           + " PFF." + "\"SecinsId\"" + " AS Secondary_Ins_Subscriber_ID,1 AS InsUptDlt,PFF.LegalName as preferred_name,PF." + "\"Lives w/ Patient ID\"" + " as Guar_ID,PF." + "\"Provider Emp ID\"" + "  as Pri_Provider_ID, '' as Sec_Provider_ID , AA.NextAppointmentDate as nextvisit_date , CASE WHEN PF." + "\"Next recall date\"" + "  is not null THEN PF." + "\"Next recall date\"" + " WHEN PF." + "\"recall cycle days\"" + "   is not null and PF." + "\"Last recall date\"" + "  is not null THEN (dateadd(month, PF." + "\"recall cycle days\"" + " ,PF." + "\"last recall date\"" + " ) ) ELSE ' ' END as Due_Date , 0 as remaining_benefit , 0 as used_benefit , 0 as collect_payment,'Y' as ReceiveVoiceCall,'English' as PreferredLanguage,'' as Patient_Note, " //case when \"Daytime phone\" = 0 then 'N' else 'Y' end ReceiveVoiceCall ""
                                           + " PFF.SSN as ssn, '' as driverlicense,'' as groupid,'' as emergencycontactId ,'' as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name,'' as emergencycontactnumber,PF.\"School name\" as school,'' as employer, PFF.SpouseID as spouseId,PF.\"Respons party Pat ID\" as responsiblepartyId ,RESPP.birthdate as responsiblepartybirthdate ,RESPP.SSN as responsiblepartyssn, "
                                           + " IPI." + "\"Employer phone\"" + " AS Prim_Ins_Company_Phonenumber,IPI2." + "\"Employer phone\"" + " AS Sec_Ins_Company_Phonenumber , RESPP." + "\"First Name\"" + " AS ResponsibleParty_First_Name,RESPP." + "\"Last Name\"" + " AS ResponsibleParty_Last_Name,SPS." + "\"First Name\"" + " as Spouse_First_Name, SPS.\"Last Name\"" + "as Spouse_Last_Name "
                                           + " FROM (Select DISTINCT \"Patient Id\" FROM " + "\"Appointments\"" + "WHERE \"date\"" + " > ?) ap INNER JOIN " + "\"Person File\"" + " PFF  on PFF." + "\"person ID\"" + " = ap." + "\"Patient Id\"" + " LEFT JOIN " + "\"Patient File\"" + " PF ON pf." + "\"person ID\"" + " = PFF." + "\"Person ID\"" + ""
                                           + Environment.NewLine                                         
                                           + Environment.NewLine
                                           + " LEFT JOIN " + "\"Ins Plan Info\"" + " IPI ON PFF." + "\"Prim Ins Plan ID\"" + "  = IPI." + "\"Ins Plan Id\"" + ""
                                           + " LEFT JOIN " + "\"Ins Plan Info\"" + " IPI2 ON PFF." + "\"Sec Ins Plan ID\"" + "  = IPI2." + "\"Ins Plan Id\"" + ""
                                           + " LEFT JOIN " + "\"AR Info\"" + " AR ON AR." + "\"Person Id\"" + "  = PFF." + "\"Person Id\"" + ""
                                           + " LEFT JOIN   ( select MIN(" + "\"date\"" + ") AS NextAppointmentDate," + "\"patient id\"" + " AS PatientEHRID  from appointments where Date >= curdate() group by " + "\"patient id\"" + ") AS AA ON AA.PatientEHRID = PF." + "\"person ID\"" + ""
                                           + " LEFT JOIN   ( select MIN(" + "\"date\"" + ") AS NextAppointmentDate," + "\"patient id\"" + " AS PatientEHRID  from appointments where Date >= curdate() and " + "\"Recall Appointment\"" + " = 1 group by " + "\"patient id\"" + ") AS AB ON AB.PatientEHRID = PF." + "\"person ID\"" + ""
                                           + " LEFT JOIN (Select SSN, birthdate,\"Person ID\" as PatientID,\"First Name\",\"Last Name\" from \"Person File\" ) as RESPP ON RESPP.PatientID = PF." + "\"Respons party Pat ID\"" + ""
                                           + " LEFT JOIN (Select \"Person ID\" as PatientID,\"First Name\",\"Last Name\" from \"Person File\" ) as SPS ON SPS.PatientID = PFF." + "SpouseID" + ""
                                           + " order by pf." + "\"Last Name\"" + ",pf." + "\"first name\"" + "";

        public static string GetPracticeWorkAppointmentsPatientDataByPatID = "SELECT  0 AS Patient_LocalDB_ID,PFF." + "\"Person ID\"" + " AS Patient_EHR_ID,'' AS Patient_Web_ID,PFF." + "\"First Name\"" + " AS First_Name,PFF." + "\"Last Name\"" + " AS Last_name,"
                                   + " PFF." + "\"Mid init\"" + " AS Middle_Name,PFF.Title AS Salutation, CASE WHEN PFF." + "\"Person Type\"" + " IN   (65,66,70,81,84,85,86,90,92,94,116,117) THEN 'I' ELSE 'A' END AS Status, case when  pf." + "\"Last Name\"" + " is not null AND  PFF." + "\"Person Type\"" + " NOT IN  (65,66,70,81,84,85,86,90,92,94,116,117)  THEN 'Active' when  pf." + "\"Last Name\"" + " is not null AND  PFF." + "\"Person Type\"" + " IN  (65,66,70,81,84,85,86,90,92,94,116,117)  THEN 'InActive' else 'NonPatient' END AS EHR_Status,'Y' as ReceiveSms,'Y' as ReceiveEmail,case when PFF.Sex = 0 then 'Male' else 'Female' END AS Sex,CASE WHEN PFF." + "\"Marital status\"" + " = 0 THEN 'Single' WHEN  PFF." + "\"Marital status\"" + " = 1 THEN 'Married' WHEN PFF." + "\"Marital status\"" + " = 2 then 'Divorced' ELSE 'Widow' END as MaritalStatus,PFF.birthdate AS Birth_Date,PFF." + "\"Email address\"" + " AS Email,"
                                   //+ " PFF." + "\"Mid init\"" + " AS Middle_Name,PFF.Title AS Salutation, CASE WHEN PFF." + "\"Person Type\"" + " IN   (37,65,66,69,70,74,78,82,102,113,81,84,85,86,90,92,94,116,117,118,126) THEN 'I' ELSE 'A' END AS Status, CASE WHEN PFF." + "\"Person Type\"" + " IN   (37,65,66,69,70,74,81,78,82,102,113,84,85,86,90,92,94,116,117,118,126) THEN 'INACTIVE' ELSE 'ACTIVE' END AS EHR_Status,'Y' as ReceiveSms,'Y' as ReceiveEmail,case when PFF.Sex = 0 then 'Male' else 'Female' END AS Sex,CASE WHEN PFF." + "\"Marital status\"" + " = 0 THEN 'Single' WHEN  PFF." + "\"Marital status\"" + " = 1 THEN 'Married' WHEN PFF." + "\"Marital status\"" + " = 2 then 'Divorced' ELSE 'Widow' END as MaritalStatus,PFF.birthdate AS Birth_Date,PFF." + "\"Email address\"" + " AS Email,"
                                   + " @FillerCondition AS Mobile,pff." + "\"Home Phone\"" + " AS Home_Phone,PFF." + "\"Work Phone 1\"" + " AS Work_Phone,PFF.Address AS Address1,PFF." + "\"Address 2\"" + " AS Address2,"//( CASE WHEN  pff." + "\"Filler\"" + " = '' THEN pff.cellphone ELSE pff." + "\"Filler\"" + " END ) 
                                   + " PFF." + "\"City\"" + " AS City,PFF.State,PFF.ZIP AS Zipcode,'' AS ResponsibleParty_Status,AR." + "\"AR Current\"" + " AS CurrentBal,AR." + "\"AR 30\"" + " AS ThirtyDay,AR." + "\"AR 60\"" + " AS SixtyDay,AR." + "\"AR 90\"" + " AS NinetyDay,AR." + "\"AR 120\"" + " AS Over90,"
                                   + " PF." + "\"First Treated\"" + " AS FirstVisit_Date,PF." + "\"Last in date\"" + " AS LastVisit_Date,"
                                   + " PFF." + "\"Prim Ins Plan ID\"" + " AS Primary_Insurance,"
                                   + " IPI." + "\"Employer Name\"" + " AS Primary_Insurance_CompanyName,"
                                   + " PFF." + "\"PriminsId\"" + " AS Primary_Ins_Subscriber_ID,"
                                   + " PFF." + "\"Sec Ins Plan ID\"" + " AS Secondary_Insurance,"
                                   + " IPI2." + "\"Employer Name\"" + " AS Secondary_Insurance_CompanyName,"
                                   + " PFF." + "\"SecinsId\"" + " AS Secondary_Ins_Subscriber_ID,1 AS InsUptDlt,PFF.LegalName as preferred_name,PF." + "\"Lives w/ Patient ID\"" + " as Guar_ID,PF." + "\"Provider Emp ID\"" + "  as Pri_Provider_ID, '' as Sec_Provider_ID , AA.NextAppointmentDate as nextvisit_date , CASE WHEN PF." + "\"Next recall date\"" + "  is not null THEN PF." + "\"Next recall date\"" + " WHEN PF." + "\"recall cycle days\"" + "   is not null and PF." + "\"Last recall date\"" + "  is not null THEN (dateadd(month, PF." + "\"recall cycle days\"" + " ,PF." + "\"last recall date\"" + " ) ) ELSE ' ' END as Due_Date , 0 as remaining_benefit , 0 as used_benefit , 0 as collect_payment,'Y' as ReceiveVoiceCall,'English' as PreferredLanguage,'' as Patient_Note, " //case when \"Daytime phone\" = 0 then 'N' else 'Y' end ReceiveVoiceCall ""
                                   + " PFF.SSN as ssn, '' as driverlicense,'' as groupid,'' as emergencycontactId ,'' as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name,'' as emergencycontactnumber,PF.\"School name\" as school,'' as employer, PFF.SpouseID as spouseId,PF.\"Respons party Pat ID\" as responsiblepartyId ,RESPP.birthdate as responsiblepartybirthdate ,RESPP.SSN as responsiblepartyssn, "
                                   + " IPI." + "\"Employer phone\"" + " AS Prim_Ins_Company_Phonenumber,IPI2." + "\"Employer phone\"" + " AS Sec_Ins_Company_Phonenumber , RESPP." + "\"First Name\"" + " AS ResponsibleParty_First_Name,RESPP." + "\"Last Name\"" + " AS ResponsibleParty_Last_Name,SPS." + "\"First Name\"" + " as Spouse_First_Name, SPS.\"Last Name\"" + "as Spouse_Last_Name "
                                   + " FROM " + "\"Person File\"" + " PFF LEFT JOIN " + "\"Patient File\"" + " PF ON pf." + "\"person ID\"" + " = PFF." + "\"Person ID\"" + ""
                                   + Environment.NewLine
                                   + " Inner JOIN (Select \"Patient Id\" FROM " + "\"Appointments\"" + "WHERE \"date\"" + " > ?) ap on pf." + "\"person ID\"" + " = ap.\"Patient Id\""
                                   + Environment.NewLine
                                   + " LEFT JOIN " + "\"Ins Plan Info\"" + " IPI ON PFF." + "\"Prim Ins Plan ID\"" + "  = IPI." + "\"Ins Plan Id\"" + ""
                                   + " LEFT JOIN " + "\"Ins Plan Info\"" + " IPI2 ON PFF." + "\"Sec Ins Plan ID\"" + "  = IPI2." + "\"Ins Plan Id\"" + ""
                                   + " LEFT JOIN " + "\"AR Info\"" + " AR ON AR." + "\"Person Id\"" + "  = PFF." + "\"Person Id\"" + ""
                                   + " LEFT JOIN   ( select MIN(" + "\"date\"" + ") AS NextAppointmentDate," + "\"patient id\"" + " AS PatientEHRID  from appointments where Date >= curdate() group by " + "\"patient id\"" + ") AS AA ON AA.PatientEHRID = PF." + "\"person ID\"" + ""
                                   + " LEFT JOIN   ( select MIN(" + "\"date\"" + ") AS NextAppointmentDate," + "\"patient id\"" + " AS PatientEHRID  from appointments where Date >= curdate() and " + "\"Recall Appointment\"" + " = 1 group by " + "\"patient id\"" + ") AS AB ON AB.PatientEHRID = PF." + "\"person ID\"" + ""
                                   + " LEFT JOIN (Select SSN, birthdate,\"Person ID\" as PatientID,\"First Name\",\"Last Name\" from \"Person File\" ) as RESPP ON RESPP.PatientID = PF." + "\"Respons party Pat ID\"" + ""
                                   + " LEFT JOIN (Select \"Person ID\" as PatientID,\"First Name\",\"Last Name\" from \"Person File\" ) as SPS ON SPS.PatientID = PFF." + "SpouseID" + ""
                                   + " Where  PFF." + "\"Person ID\" = '@Patient_EHR_ID' "
                                   + " order by pf." + "\"Last Name\"" + ",pf." + "\"first name\"" + "";

        public static string GetPracticeWorkPatientCellPhoneStatusData = "select @FillerCondition AS Mobile FROM " + "\"Person File\"" + " PFF where 1 <> 1";

        public static string GetPatientStatus = "SELECT pf." + "\"person ID\"" + " AS Patient_EHR_Id From " + "\"Person File\"" + " PFF INNER JOIN " + "\"Patient File\"" + " PF ON pf." + "\"person ID\"" + " = PFF." + "\"Person ID\"" + " WHERE PFF." + "\"Person Type\"" + " not IN (@persenType) AND PF." + "\"person ID\"" + " NOT IN ( SELECT pfa." + "\"person ID\"" + " FROM " + "\"Person File\"" + " pfa inner join Appointments ap ON pfa." + "\"person ID\"" + " = ap." + "\"Patient ID\"" + " WHERE ap.date < curdate() and AP.Status = '3'  )";
        //public static string GetPatientStatus = "SELECT pf." + "\"person ID\"" + " AS Patient_EHR_Id From " + "\"Person File\"" + " PFF INNER JOIN " + "\"Patient File\"" + " PF ON pf." + "\"person ID\"" + " = PFF." + "\"Person ID\"" + " WHERE PFF." + "\"Person Type\"" + " not IN (37,65,66,69,70,74,81,78,82,102,113,84,85,86,90,92,94,116,117,118,126) AND PF." + "\"person ID\"" + " NOT IN ( SELECT pfa." + "\"person ID\"" + " FROM " + "\"Person File\"" + " pfa inner join Appointments ap ON pfa." + "\"person ID\"" + " = ap." + "\"Patient ID\"" + " WHERE ap.date < curdate()  )";

        public static string GetPatientStatusByPatID = "SELECT pf." + "\"person ID\"" + " AS Patient_EHR_Id From " + "\"Person File\"" + " PFF INNER JOIN " + "\"Patient File\"" + " PF ON pf." + "\"person ID\"" + " = PFF." + "\"Person ID\"" + " WHERE pf." + "\"person ID\" = '@Patient_EHR_ID' and PFF." + "\"Person Type\"" + " not IN (65,66,70,81,84,85,86,90,92,94,116,117) AND PF." + "\"person ID\"" + " NOT IN ( SELECT pfa." + "\"person ID\"" + " FROM " + "\"Person File\"" + " pfa inner join Appointments ap ON pfa." + "\"person ID\"" + " = ap." + "\"Patient ID\"" + " WHERE ap.date < curdate() and AP.Status = '3'  )";

        public static string GetPracticeWorkOperatoryData = "select distinct 0 as Clinic_Number,1 as Service_Install_Id,0 AS Operatory_LocalDB_ID, r." + "\"Resource Id\"" + " AS Operatory_EHR_ID,'' AS Operatory_Web_ID," + "\"Resource name\"" + " AS Operatory_Name,CURDATE() AS Last_Sync_Date,CURDATE() AS EHR_Entry_DateTime,0 AS is_deleted,0 AS Is_Adit_Updated  From " + "\"Resource Definitions\"" + " r inner join " + "\"Appointment books\"" +
                                                            " a on (r. " + "\"Resource Id\"" + "= a." + "\"Resource ID 1\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 2\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 3\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 4\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 5\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 6\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 7\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 8\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 9\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 10\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 11\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 12\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 13\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 14\"" + " or " +
                                                            " r. " + "\"Resource Id\"" + " = a." + "\"Resource ID 15\"" + " )";

        public static string GetPracticeWorkOperatorySequence = "select *  From " + "\"Appointment books\"" + "";

        public static string GetPracticeWorkSpecialtyData = "Select DISTINCT 0 as Clinic_Number,1 as Service_Install_Id,'' AS Speciality_LocalDB_ID,'' AS Speciality_EHR_ID,'' AS Speciality_Web_ID, Specialty AS Speciality_Name, curdate() AS Last_Sync_Date,CURDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated  From " + "\"Referral Sources\"" + " where inactive = 0 and Specialty <> ''";

        public static string GetOPeratpryDateRangeWise = "Select " + "\"Visit ID\"" + " AS Appt_EHR_ID, " + "\"Resource Id\"" + " AS Operatory_EHR_ID,date as Appt_datetime,date as Appt_EndDateTime," + "\"Start time\"" + "," + "\"End time\"" + ", P." + "\"First Name\"" + " AS FirstName,p." + "\"Last Name\"" + " AS LastName,@FillerCondition AS Mobile,P." + "\"Email Address\"" + " AS Email,PP." + "\"First Name\"" + " AS ProviderFirstName,PP." + "\"Last Name\"" + " AS ProviderLastName FROM Appointments  A LEFT JOIN " + "\"Person File\"" + " P ON A." + "\"Patient ID\"" + " = P." + "\"Person Id\"" + "  LEFT JOIN " + "\"Employee list\"" + " PP ON PP." + "\"Employee Id\"" + " = A." + "\"Dr Id\"" + " where A.date > ? and A.date < ?";

        public static string GetPraticeWorkAppointentData = "Select  0 as Clinic_Number,1 as Service_Install_Id,0 AS Appt_LocalDB_ID,AP." + "\"Visit ID\"" + " AS Appt_EHR_ID,'' AS Appt_Web_ID,PF." + "\"Last name\"" + " AS Last_Name,PF." + "\"First Name\"" + " AS First_Name,PF." + "\"Mid init\"" + " AS MI,PF." + "\"Home Phone\"" + " AS Home_Contact,@FillerCondition AS Mobile_Contact,PF." + "\"Email address\"" + " AS Email,PF.Address,PF.City,PF.State AS ST,PF.Zip,AP." + "\"Resource ID\"" + " AS Operatory_EHR_ID,RD." + "\"Resource name\"" + " AS Operatory_Name,AP." + "\"Dr Id\"" + " AS Provider_EHR_ID,( EL." + "\"First Name\"" + " + ' ' + EL." + "\"Last Name\"" + " ) AS Provider_Name,AP.Description AS comment,PF.Birthdate AS Birth_Date, 0 AS ApptType_EHR_ID,'none'  AS ApptType, AP.Date  AS Appt_DateTime,AP.Date AS Appt_EndDateTime,  AP." + "\"Start Time\"" + ",AP." + "\"End Time\"" + ",1 AS Status,'New' AS Patient_Status,0 AS appointment_status_ehr_key,'' AS Appointment_Status,'EHR' AS Is_Appt,0 AS IS_EHR_UPDATED,'' AS Remind_DateTime,CURDATE() AS Entry_DateTime,CURDATE() AS Last_Sync_Date,AP." + "\"Appt made on date\"" + " AS EHR_Entry_DateTime,0 AS Is_Adit_Updated,AP." + "\"Patient Id\"" + " AS patient_ehr_id,0 AS unschedule_status_ehr_key,'' AS unschedule_status,0 AS confirmed_status_ehr_key,'' AS confirmed_status,0 AS is_deleted,0 AS is_Status_Updated_From_Web,'' AS InsuranceCompanyName,0 AS Is_Appt_DoubleBook,AP." + "\"ASAP Appointment\"" + " AS is_asap, AP." + "\"Confirmed date\"" + ",AP." + "\"Check in time\"" + " AS CheckinTime,AP." + "\"Seated time\"" + " AS SeatedTime,AP." + "\"Check out time \"" + " AS CheckOutTime,AP.Status AS AppointmentActStatus"
                                                            + " FROM " + "\"Appointments\"" + " AP "
                                                            + " INNER JOIN " + "\"Person File\"" + " PF ON AP." + "\"Patient ID\"" + " = PF." + "\"Person ID\"" + ""
                                                            + " INNER JOIN " + "\"Resource Definitions\"" + " RD ON RD." + "\"Resource ID\"" + " = AP." + "\"Resource Id\"" + ""
                                                            + " INNER JOIN  " + "\"Employee list\"" + " EL ON EL." + "\"Employee Id\"" + " = AP." + "\"Dr ID\"" + ""
                                                            + " WHERE AP." + "\"date\"" + " > ? ";

        public static string GetPraticeWorkAppointentDataByApptID = "Select  0 as Clinic_Number,1 as Service_Install_Id,0 AS Appt_LocalDB_ID,AP." + "\"Visit ID\"" + " AS Appt_EHR_ID,'' AS Appt_Web_ID,PF." + "\"Last name\"" + " AS Last_Name,PF." + "\"First Name\"" + " AS First_Name,PF." + "\"Mid init\"" + " AS MI,PF." + "\"Home Phone\"" + " AS Home_Contact,@FillerCondition AS Mobile_Contact,PF." + "\"Email address\"" + " AS Email,PF.Address,PF.City,PF.State AS ST,PF.Zip,AP." + "\"Resource ID\"" + " AS Operatory_EHR_ID,RD." + "\"Resource name\"" + " AS Operatory_Name,AP." + "\"Dr Id\"" + " AS Provider_EHR_ID,( EL." + "\"First Name\"" + " + ' ' + EL." + "\"Last Name\"" + " ) AS Provider_Name,AP.Description AS comment,PF.Birthdate AS Birth_Date, 0 AS ApptType_EHR_ID,'none'  AS ApptType, AP.Date  AS Appt_DateTime,AP.Date AS Appt_EndDateTime,  AP." + "\"Start Time\"" + ",AP." + "\"End Time\"" + ",1 AS Status,'New' AS Patient_Status,0 AS appointment_status_ehr_key,'' AS Appointment_Status,'EHR' AS Is_Appt,0 AS IS_EHR_UPDATED,'' AS Remind_DateTime,CURDATE() AS Entry_DateTime,CURDATE() AS Last_Sync_Date,AP." + "\"Appt made on date\"" + " AS EHR_Entry_DateTime,0 AS Is_Adit_Updated,AP." + "\"Patient Id\"" + " AS patient_ehr_id,0 AS unschedule_status_ehr_key,'' AS unschedule_status,0 AS confirmed_status_ehr_key,'' AS confirmed_status,0 AS is_deleted,0 AS is_Status_Updated_From_Web,'' AS InsuranceCompanyName,0 AS Is_Appt_DoubleBook,AP." + "\"ASAP Appointment\"" + " AS is_asap, AP." + "\"Confirmed date\"" + ",AP." + "\"Check in time\"" + " AS CheckinTime,AP." + "\"Seated time\"" + " AS SeatedTime,AP." + "\"Check out time \"" + " AS CheckOutTime,AP.Status AS AppointmentActStatus"
                                                            + " FROM " + "\"Appointments\"" + " AP "
                                                            + " INNER JOIN " + "\"Person File\"" + " PF ON AP." + "\"Patient ID\"" + " = PF." + "\"Person ID\"" + ""
                                                            + " INNER JOIN " + "\"Resource Definitions\"" + " RD ON RD." + "\"Resource ID\"" + " = AP." + "\"Resource Id\"" + ""
                                                            + " INNER JOIN  " + "\"Employee list\"" + " EL ON EL." + "\"Employee Id\"" + " = AP." + "\"Dr ID\"" + ""
                                                            + " WHERE AP." + "\"date\"" + " > ? and AP." + "\"Visit ID\"" + " = '@Appt_ERH_ID'";

        public static string GetPraticeWorkAppointentEhrIds = "Select AP." + "\"Visit ID\"" + " AS Appt_EHR_ID"
                                                            + " FROM " + "\"Appointments\"" + " AP "
                                                            + " INNER JOIN " + "\"Person File\"" + " PF ON AP." + "\"Patient ID\"" + " = PF." + "\"Person ID\"" + ""
                                                            + " INNER JOIN " + "\"Resource Definitions\"" + " RD ON RD." + "\"Resource ID\"" + " = AP." + "\"Resource Id\"" + ""
                                                            + " INNER JOIN  " + "\"Employee list\"" + " EL ON EL." + "\"Employee Id\"" + " = AP." + "\"Dr ID\"" + ""
                                                            + " WHERE AP." + "\"date\"" + " > ? ";

        public static string PraticeWorkAppointment_Procedures_Data = " Select APPT. " + "\"visit id\"" + " AS Appt_EHR_ID, CD." + "\"Medical Code\"" + " AS ProcedureCode "
                                                            + " from Appointments APPT "
                                                            + " Left JOIN  " + "\"tx plan details\"" + " TPD ON APPT." + "\"visit id\"" + "= TPD." + "\"visit id\"" + ""
                                                            + " Left Join " + "\"Tx Code Defs\"" + " CD ON TPD." + "\"tx code id\"" + " = CD." + "\"tx code id\"" + ""
                                                             + " WHERE APPT." + "\"date\"" + " > ? ";

        public static string PraticeWorkAppointment_Procedures_DataByApptID = " Select APPT. " + "\"visit id\"" + " AS Appt_EHR_ID, CD." + "\"Medical Code\"" + " AS ProcedureCode "
                                                    + " from Appointments APPT "
                                                    + " Left JOIN  " + "\"tx plan details\"" + " TPD ON APPT." + "\"visit id\"" + "= TPD." + "\"visit id\"" + ""
                                                    + " Left Join " + "\"Tx Code Defs\"" + " CD ON TPD." + "\"tx code id\"" + " = CD." + "\"tx code id\"" + ""
                                                     + " WHERE APPT." + "\"date\"" + " > ? and APPT. " + "\"visit id\"" + " = '@Appt_EHR_ID'";

        public static string GetPracticeWorkOperatoryTimeOff = " SELECT  0 as Clinic_Number,1 as Service_Install_Id,'' AS OE_LocalDB_ID,'' AS OE_EHR_ID,'' AS OE_Web_ID,BBD.ResourceId AS Operatory_EHR_ID,BBD.BlockDate, BBD.StartTime,BBD.EndTime ,BD.Description AS comment,CURDATE() AS Last_Sync_Date,CURDATE() AS EHR_Entry_DateTime,0 AS is_deleted,0 AS Is_Adit_Updated FROM  " + "\"Block Book Series\"" + " BBS INNER JOIN    " + "\"Block Book detail\"" + " BBD ON BBS.SeriesId = BBD.SeriesId INNER JOIN " + "\"Block Definitions\"" + " BD ON BD." + "\"Block def Id\"" + " = BBD.DefinitionId WHERE BBD.blockdate >  " + " ? ";

        public static string GetPracticeWorkOperatoryCustomeHOurs = " select  0 as Clinic_Number,1 as Service_Install_Id,AD.* FROM " + "\"Open Close Times\"" + " AD  LEFT JOIN  " + "\"Appt Book Exceptions\"" + " B ON AD.Date = B.Date where AD.date >=  ?  and ( B." + "\"open for business\"" + " is null OR B." + "\"open for business\"" + " = 1 ) order by AD.date ";

        public static string GetPracticeWorkProviderCustomeHOurs = "select  0 as Clinic_Number,1 as Service_Install_Id,AD.* FROM " + "\"Open Close Times\"" + " AD  LEFT JOIN  " + "\"Appt Book Exceptions\"" + " B ON AD.Date = B.Date where AD.date >=  ?  and ( B." + "\"open for business\"" + " is null OR B." + "\"open for business\"" + " = 1 ) order by AD.date ";
        //" select 0 as Clinic_Number,1 as Service_Install_Id,AD.* FROM " + "\"Open Close Times\"" + " AD INNER JOIN ( Select * FROM ( Select MAX(Date) AS OfficeTime from " + "\"Open Close Times\"" + " where Date  >  ? UNION Select MAX(Date) AS OfficeTime FROM " + "\"Open Close Times\"" + " ) AS AB WHERE AB.OfficeTime is not null ) AS AC ON AC.OfficeTime = AD.Date LEFT JOIN  " + "\"Appt Book Exceptions\"" + " B ON AD.Date = B.Date   ";

        //select 0 as Clinic_Number,1 as Service_Install_Id,AD.* FROM "Open Close Times" AD 
        //INNER JOIN ( Select * FROM ( Select MAX(Date) AS OfficeTime from "Open Close Times" where Date  >  '2021-09-01' UNION Select MAX(Date) AS OfficeTime FROM "Open Close Times" ) AS AB WHERE AB.OfficeTime is not null ) AS AC ON AC.OfficeTime = AD.Date 
        //LEFT JOIN  "Appt Book Exceptions" B ON AD.Date = B.Date AND B."Open for business" = 1

        public static string GetPracticeWorkDiseaseMaster = "SELECT 0 as Clinic_Number,1 as Service_Install_Id,0 AS Disease_LocalDB_ID," + "\"Med Alert ID\"" + " AS Disease_EHR_ID,'' AS Disease_Web_ID,Description AS Disease_Name,'A' AS Disease_Type,CURDATE() AS EHR_Entry_DateTime,CURDATE() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated FROM " + "\"Medical Alerts\"" + "";

        //public static string GetMedicationMaster = "SELECT 0 as Clinic_Number,1 as Service_Install_Id,0 AS Medication_LocalDB_ID," + "\"DrugID\"" + " AS Medication_EHR_ID,'' AS Medication_Web_ID," + "\"Rx\"" + " AS Medication_Name,'M' AS Medication_Type," + "\"Dosage\"" + " AS Drug_Quantity,CURDATE() AS EHR_Entry_DateTime,CURDATE() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated FROM " + "\"Drugs\"" + "";
        public static string GetMedicationMaster = "Select \"DrugID\" as Medication_EHR_ID, " +
                                                   "\"Rx\" as Medication_Name, " +
                                                   "\"Label\" as Medication_Description,  " +
                                                   "CT.\"Text\" as Medication_Notes, " +
                                                   "\"Instructions\" as Medication_Sig, " +
                                                   "'' as Medication_Parent_EHR_ID, " +
                                                   "'Drug' as Medication_Type , '' as Allow_Generic_Sub, " +
                                                   "\"Dosage\" as Drug_Quantity, " +
                                                   "\"Quantity\" as Medication_Quantity, " +
                                                   "\"Refill\" as Refills,  " +
                                                   "(case when InActive = 0  then 'True' else 'False' end) as Is_Active, " +
                                                   "CURDATE() as EHR_Entry_DateTime, '' as Medication_Provider_ID," +
                                                   "0 as is_deleted, " +
                                                   "0 as Is_Adit_Updated , " +
                                                   "0 as Clinic_Number " +
                                                   "from \"Drugs\" DR Left Join \"Chart Text\" CT on DR.\"TextID\" = CT.\"Text ID\" Where (\"Rx\" is not null and \"Rx\" <> '')";

        public static string GetPatientMedication = "SELECT 0 as Clinic_Number,1 as Service_Install_Id,DRH.\"DrugHistoryID\" as PatientMedication_EHR_ID,0 AS Medication_LocalDB_ID,DRH." + "\"PatientPersonID\"" + " AS Patient_EHR_ID,DR." + "\"DrugID\"" + " AS Medication_EHR_ID,'' AS PatientMedication_Web_ID,DRH." + "\"ProviderEmployeeID\"" + " AS Provider_EHR_ID,DR." + "\"Rx\"" + " AS Medication_Name,'M' AS Medication_Type,DRH." + "\"Instructions\"" + " AS Patient_Notes," + "CT.\"Text\" AS Medication_Note," + "DR." + "\"Dosage\"" + " AS Drug_Quantity,CURDATE() AS Start_Date,CURDATE() AS End_Date,CURDATE() AS EHR_Entry_DateTime,CURDATE() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated,'true' As is_active FROM " + "\"DrugHistory\"" + " DRH "
                                                    + " INNER JOIN " + "\"Drugs\"" + " DR ON DR." + "\"DrugID\"" + " = DRH." + "\"DrugID\"" + " Left Join \"Chart Text\" CT on DR.\"TextID\" = CT.\"Text ID\"";

        //public static string GetPracticeWorkRecallData = "Select 0 AS RecallType_LocalDB_ID," + "\"Tx Set Id\"" + " AS RecallType_EHR_ID,'' AS RecallType_Web_ID," + "\"Schedule abbrev\"" + " AS RecallType_Name,Description AS RecallType_Descript,CurDate() AS Last_Sync_Date, CUrDate() AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated from " + "\"Tx Set Definitions\"" + " where " + "\"Recall Appointment\"" + " = 1";

        public static string InsertMedication = "Insert into \"Drugs\" (\"Rx\",\"Label\", " +
                        "\"Instructions\",\"Dosage\",\"Refill\",\"Inactive\") " +
                        " Values(@Medication_Name,'','','','','0')"; //Label,Rx,Dosage,Quantity,Refill,Instructions,Inactive,TextID,Filler1

        //Chart Text = SeqNum,Rep Flag,Compression Flag, Text, Hide, Spare
        public static string InsertPatientMedication = "Insert into \"DrugHistory\" (\"DrugID\", " +
                        "\"PatientPersonID\", " +
                        "\"Label\", " +
                        "\"Rx\", " +
                        "\"Dosage\", " +
                        "\"Quantity\", " +
                        "\"Refill\", " +
                        "\"Instructions\", " +
                        "\"TextID\", " +
                        "\"ActionTaken\", " +
                        "\"PharmacyID\", " +
                        "\"ProviderEmployeeID\", " +
                        "\"NumOfPrints\", " +
                        "\"Filler1\") " +
                        " Values(@Medication_EHR_ID, " +
                        " @Patient_EHR_ID, " +
                        " @Medication_Description, " +
                        " @Medication_Name, " +
                        " @Drug_Quantity, " +
                        " @Medication_Quantity, " +
                        " @Refills, " +
                        " @Medication_SIG, " +
                        " @Medication_Note_ID, " +
                        " 0, " +
                        " 0, " +
                        " @Provider_EHR_ID, " +
                        " '', " +
                        " '');"; //DrugID, PatientPersonID,Label,Rx,Dosage,Quantity,Refill,Instruction,TextID,ActinTake,PharmacyID,ProviderEmployeeID,NumOfPrints,ClinicianRxID,Filler1

        public static string UpdatePatientMedicationNotes = "Update \"Chart Text\" set \"Text\" = @Medication_Note Where \"Text ID\" = @PatientMedication_EHR_ID ";

        public static string DeletePatientMedication = "Delete From \"DrugHistory\" Where \"DrugHistoryID\" = @PatientMedication_EHR_ID ";

        public static string GetPracticeWorkRecallData = "Select 0 as Clinic_Number,1 as Service_Install_Id,0 AS RecallType_LocalDB_ID," + "\"Tx Code Id\"" + " AS RecallType_EHR_ID,'' AS RecallType_Web_ID," + "\"Description\"" + " AS RecallType_Name," + "\"CDT Description\"" + " AS RecallType_Descript,CurDate() AS Last_Sync_Date, CUrDate() AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated From " + "\"Tx Code Defs\"" + " where " + "\"Recall tx code\"" + " = 1";

        public static string GetPracticeWorkAppointmentStatus = "select 0 as Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_ID,1 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'NotConfirm' AS ApptStatus_Name,'normal' AS ApptStatus_Type,CURDATE() AS Last_Sync_Date,CURDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated UNION "
                                                             + " select 0 as Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_ID,2 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'Confirmed' AS ApptStatus_Name,'normal' AS ApptStatus_Type,CURDATE() AS Last_Sync_Date,CURDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated UNION "
                                                             + " select 0 as Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_ID,3 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'CheckIn' AS ApptStatus_Name,'normal' AS ApptStatus_Type,CURDATE() AS Last_Sync_Date,CURDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated UNION "
                                                             + " select 0 as Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_ID,4 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'Seated' AS ApptStatus_Name,'normal' AS ApptStatus_Type,CURDATE() AS Last_Sync_Date,CURDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated UNION "
                                                             + " select 0 as Clinic_Number,1 as Service_Install_Id,0 AS ApptStatus_LocalDB_ID,5 AS ApptStatus_EHR_ID,'' AS ApptStatus_Web_ID,'CheckedOut' AS ApptStatus_Name,'normal' AS ApptStatus_Type,CURDATE() AS Last_Sync_Date,CURDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated ";

        public static string GetPracticeWorkPatientDisease = "SELECT 0 as Clinic_Number,1 as Service_Install_Id,0 AS Disease_LocalDB_ID,PMA." + "\"PatientID\"" + " AS Patient_EHR_ID,PMA." + "\"MedAlertID\"" + " AS Disease_EHR_ID,'' AS PatientDisease_Web_ID,MA.Description AS Disease_Name,'A' AS Disease_Type,CURDATE() AS EHR_Entry_DateTime,CURDATE() AS Last_Sync_Date,0 AS is_deleted,0 AS Is_Adit_Updated FROM " + "\"PatMedAlerts\"" + "" + " PMA   "
                                                         + " LEFT JOIN " + "\"Medical Alerts\"" + " MA ON MA." + "\"Med Alert ID\"" + " = PMA." + "\"MedAlertID\"" + " ";


        public static string GetPracticeWorkAppointmentType = "select 0 as Clinic_Number,1 as Service_Install_Id,0 AS ApptType_LocalDB_ID,0 AS ApptType_EHR_ID,'' AS ApptType_Web_ID,'none' AS Type_Name,CURDATE() AS Last_Sync_Date,CURDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated";

        //public static string InsertPracticeWorkPatientFile = "insert into " + "\"patient file\"" + "" 
        //                                                        + "(" + "\"Person ID\"" + ""
        //                                                        + "," + "\"Date first entered\"" + ""
        //                                                        + "," + "\"Last name\"" + ""
        //                                                        + "," + "\"First name\"" + ""
        //                                                        + "," + "\"Lives w/ Patient Id\"" + ""
        //                                                        + "," + "\"Full time Student\"" + ""
        //                                                        + "," + "\"School name\"" + ""
        //                                                        + "," + "\"School City/State\"" + ""
        //                                                        + "," + "\"Primary Ins Pat ID\"" + ""
        //                                                        + "," + "\"Primary coverage num\"" + ""
        //                                                        + "," + "\"Prim relship to ins\"" + ""
        //                                                        + "," + "\"Prim Desc of other\"" + ""
        //                                                        + "," + "\"Prim ins ID Str\"" + ""
        //                                                        + "," + "\"Prim indv deduct bal\"" + ""
        //                                                        + "," + "\"prim indv annual max\"" + ""
        //                                                        + "," + "\"Second Ins Pat ID\"" + ""
        //                                                        + "," + "\"Secnd coverage num\"" + ""
        //                                                        + "," + "\"secnd relship to ins\"" + ""
        //                                                        + "," + "\"Secnd Desc of other\"" + ""
        //                                                        + "," + "\"Secnd Ins Id Str\"" + ""
        //                                                        + "," + "\"Secnd indv dedct bal\"" + ""
        //                                                        + "," + "\"Secnd indv annl max\"" + ""
        //                                                        + "," + "\"Prim Med Ins Pat ID\"" + ""
        //                                                        + "," + "\"Prim Med Cover Num\"" + ""
        //                                                        + "," + "\"Prim Med Rel to Ins\"" + ""
        //                                                        + "," + "\"Prim Med Other Desc\"" + ""
        //                                                        + "," + "\"Prim Med Ins ID Str\"" + ""
        //                                                        + "," + "\"Prim Med Ind Ded Bal\"" + ""
        //                                                        + "," + "\"Prim Med Ind Anl Max\"" + ""
        //                                                        + "," + "\"Secd Med Ins Pat ID\"" + ""
        //                                                        + "," + "\"Secd Med Cover Num\"" + ""
        //                                                        + "," + "\"Secd Med Rel to Ins\"" + ""
        //                                                        + "," + "\"Secd Med Other Desc\"" + ""
        //                                                        + "," + "\"Secd Med Ins ID Str\"" + ""
        //                                                        + "," + "\"Secd Med Ins Ded Bal\"" + ""
        //                                                        + "," + "\"Secd Med Ind Anl Max\"" + ""
        //                                                        + "," + "\"Respons party Pat ID\"" + ""
        //                                                        + "," + "\"Provider Emp ID\"" + ""
        //                                                        + "," + "\"Dentistry YTD\"" + ""
        //                                                        + "," + "\"On recall\"" + ""
        //                                                        + "," + "\"Recall emp ID\"" + ""
        //                                                        + "," + "\"Last recall date\"" + ""
        //                                                        + "," + "\"Last recall Tx code\"" + ""
        //                                                        + "," + "\"Recall cycle days\"" + ""
        //                                                        + "," + "\"Recall sched units\"" + ""
        //                                                        + "," + "\"Recall Dr schd units\"" + ""
        //                                                        + "," + "\"Recall non prod 2\"" + ""
        //                                                        + "," + "\"Recall prod 2\"" + ""
        //                                                        + "," + "\"Recall non prod 3\"" + ""
        //                                                        + "," + "\"Next recall date\"" + ""
        //                                                        + "," + "\"Last recall contact\"" + ""
        //                                                        + "," + "\"Next recall contact\"" + ""
        //                                                        + "," + "\"Contact comment\"" + ""
        //                                                        + "," + "\"Non recall emp ID\"" + ""
        //                                                        + "," + "\"Referred by persn ID\"" + ""
        //                                                        + "," + "\"Referred to persn ID\"" + ""
        //                                                        + "," + "\"Patient flags set\"" + ""
        //                                                        + "," + "\"Last in date\"" + ""
        //                                                        + "," + "\"Cancel fail count\"" + ""
        //                                                        + "," + "\"Cancel 1 date\"" + ""
        //                                                        + "," + "\"Cancel 1 status\"" + ""
        //                                                        + "," + "\"Cancel 2 date\"" + ""
        //                                                        + "," + "\"Cancel 2 status\"" + ""
        //                                                        + "," + "\"Cancel 3 date\"" + ""
        //                                                        + "," + "\"Cancel 3 status\"" + ""
        //                                                        + "," + "\"First treated\"" + ""
        //                                                        + "," + "\"Pre medication req\"" + ""
        //                                                        + "," + "\"Pre medication desc\"" + ""
        //                                                        + "," + "\"Medical alerts set\"" + ""
        //                                                        + "," + "\"LastMedupdate\"" + ""
        //                                                        + "," + "\"NoLongerPatient\"" + ""
        //                                                        + "," + "\"IntegrationID1\"" + ""
        //                                                        + "," + "\"IntegrationID2\"" + ""
        //                                                        + "," + "\"IntegrationID3\"" + ""
        //                                                        + "," + "\"LastBitewing\"" + ""
        //                                                        + "," + "\"LastFMS\"" + ""
        //                                                        + "," + "\"LastPanoramic\"" + ""
        //                                                        + "," + "\"PreferredPharmacyId\"" + ""
        //                                                        + "," + "\"tempMidInit\"" + ""
        //                                                        + "," + "\"tempJrSr\"" + ""
        //                                                        + "," + "\"ConvertedToCSI\"" + ""
        //                                                        + "," + "\"ClinicianPatientId\"" + ""
        //                                                        + "," + "\"Filler\"" + ""
        //                                                        + "," + "\"Spare\"" + ")"
        //                                                        + "Values"
        //                                                        + "(6886556
        //                                                        + ",CURDATE()
        //                                                        + ",'Adit'
        //                                                        + ",'Test'
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",64
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",6
        //                                                        + ",0
        //                                                        + ",2
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",64
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",NULL
        //                                                        + ",0
        //                                                        + ",NULL
        //                                                        + ",'---'
        //                                                        + ",NULL)";

        public static string UpdateAppointmentStatus = " UPDATE " + "\"Appointments\"" + " SET @FieldToUpdate WHERE " + "\"Visit ID\"" + " = @AppointmentId ";

        public static string InsertMedicalAlerts = " insert into PatMedAlerts (PatientID,MedAlertID,Spare) VALUES (@Patient_id,@Alert_id,'')";

        public static string DeleteMedicalAlerts = " Delete from PatMedAlerts where PatientID = @Patient_id and MedAlertID = @Alert_id";

        public static string UpdateNextPatientId = " Update  " + "\"System Info\"" + " SET " + "\"Next Person ID\"" + " =  @PatientIdNew WHERE " + "\"Next Person ID\"" + " = @PatientId";

        public static string UpdateNextVisitId = " Update  " + "\"System Info\"" + " SET " + "\"Next Visit ID\"" + " =  @visitIdNew Where " + "\"Next Visit ID\"" + " = @visitId";

        public static string InsertPracticeWorkPatientFile = "insert into " + "\"patient file\"" + ""
                                                                + "(" + "\"Person ID\"" + ""
                                                                + "," + "\"Date first entered\"" + ""
                                                                + "," + "\"Last name\"" + ""
                                                                + "," + "\"First name\"" + ""
                                                                + "," + "\"Lives w/ Patient Id\"" + ""
                                                                + "," + "\"Full time Student\"" + ""
                                                                + "," + "\"School name\"" + ""
                                                                + "," + "\"School City/State\"" + ""
                                                                + "," + "\"Primary Ins Pat ID\"" + ""
                                                                + "," + "\"Primary coverage num\"" + ""
                                                                + "," + "\"Prim relship to ins\"" + ""
                                                                + "," + "\"Prim Desc of other\"" + ""
                                                                + "," + "\"Prim ins ID Str\"" + ""
                                                                + "," + "\"Prim indv deduct bal\"" + ""
                                                                + "," + "\"prim indv annual max\"" + ""
                                                                + "," + "\"Second Ins Pat ID\"" + ""
                                                                + "," + "\"Secnd coverage num\"" + ""
                                                                + "," + "\"secnd relship to ins\"" + ""
                                                                + "," + "\"Secnd Desc of other\"" + ""
                                                                + "," + "\"Secnd Ins Id Str\"" + ""
                                                                + "," + "\"Secnd indv dedct bal\"" + ""
                                                                + "," + "\"Secnd indv annl max\"" + ""
                                                                + "," + "\"Prim Med Ins Pat ID\"" + ""
                                                                + "," + "\"Prim Med Cover Num\"" + ""
                                                                + "," + "\"Prim Med Rel to Ins\"" + ""
                                                                + "," + "\"Prim Med Other Desc\"" + ""
                                                                + "," + "\"Prim Med Ins ID Str\"" + ""
                                                                + "," + "\"Prim Med Ind Ded Bal\"" + ""
                                                                + "," + "\"Prim Med Ind Anl Max\"" + ""
                                                                + "," + "\"Secd Med Ins Pat ID\"" + ""
                                                                + "," + "\"Secd Med Cover Num\"" + ""
                                                                + "," + "\"Secd Med Rel to Ins\"" + ""
                                                                + "," + "\"Secd Med Other Desc\"" + ""
                                                                + "," + "\"Secd Med Ins ID Str\"" + ""
                                                                + "," + "\"Secd Med Ins Ded Bal\"" + ""
                                                                + "," + "\"Secd Med Ind Anl Max\"" + ""
                                                                + "," + "\"Respons party Pat ID\"" + ""
                                                                + "," + "\"Provider Emp ID\"" + ""
                                                                + "," + "\"Dentistry YTD\"" + ""
                                                                + "," + "\"On recall\"" + ""
                                                                + "," + "\"Recall emp ID\"" + ""
                                                                + "," + "\"Last recall date\"" + ""
                                                                + "," + "\"Last recall Tx code\"" + ""
                                                                + "," + "\"Recall cycle days\"" + ""
                                                                + "," + "\"Recall sched units\"" + ""
                                                                + "," + "\"Recall Dr schd units\"" + ""
                                                                + "," + "\"Recall non prod 2\"" + ""
                                                                + "," + "\"Recall prod 2\"" + ""
                                                                + "," + "\"Recall non prod 3\"" + ""
                                                                + "," + "\"Next recall date\"" + ""
                                                                + "," + "\"Last recall contact\"" + ""
                                                                + "," + "\"Next recall contact\"" + ""
                                                                + "," + "\"Contact comment\"" + ""
                                                                + "," + "\"Non recall emp ID\"" + ""
                                                                + "," + "\"Referred by persn ID\"" + ""
                                                                + "," + "\"Referred to persn ID\"" + ""
                                                                + "," + "\"Patient flags set\"" + ""
                                                                + "," + "\"Last in date\"" + ""
                                                                + "," + "\"Cancel fail count\"" + ""
                                                                + "," + "\"Cancel 1 date\"" + ""
                                                                + "," + "\"Cancel 1 status\"" + ""
                                                                + "," + "\"Cancel 2 date\"" + ""
                                                                + "," + "\"Cancel 2 status\"" + ""
                                                                + "," + "\"Cancel 3 date\"" + ""
                                                                + "," + "\"Cancel 3 status\"" + ""
                                                                + "," + "\"First treated\"" + ""
                                                                + "," + "\"Pre medication req\"" + ""
                                                                + "," + "\"Pre medication desc\"" + ""
                                                                + "," + "\"Medical alerts set\"" + ""
                                                                + "," + "\"LastMedupdate\"" + ""
                                                                + "," + "\"NoLongerPatient\"" + ""
                                                                + "," + "\"IntegrationID1\"" + ""
                                                                + "," + "\"IntegrationID2\"" + ""
                                                                + "," + "\"IntegrationID3\"" + ""
                                                                + "," + "\"LastBitewing\"" + ""
                                                                + "," + "\"LastFMS\"" + ""
                                                                + "," + "\"LastPanoramic\"" + ""
                                                                + "," + "\"PreferredPharmacyId\"" + ""
                                                                //+ "," + "\"tempMidInit\"" + ""
                                                                //+ "," + "\"tempJrSr\"" + ""
                                                                //+ "," + "\"ConvertedToCSI\"" + ""
                                                                //+ "," + "\"ClinicianPatientId\"" + ""
                                                                + "," + "\"Filler\"" + ""
                                                                + "," + "\"Spare\"" + ")"
                                                                + "Values"
                                                                + "(@PatientId "
                                                                + ",CURDATE()"
                                                                + ",@LastName"
                                                                + ",@FirstName,@GuarId,@Full_time_Student,@School_name,NULL,0,0,0,NULL,NULL,0,0,0,0,0,NULL,NULL,0,0,0,0,0,NULL,NULL,0,0,0,0,0,NULL,NULL,0,0,@GuarId,0,0,0,0,NULL,0,6,0,2,0,0,0,NULL,NULL,NULL,NULL,64,0,0,0,NULL,0,NULL,0,NULL,0,NULL,0,NULL,0,NULL,0,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,0"
                                                                  //+ ",NULL,NULL,0,NULL"
                                                                  + ",'---',NULL)";

        public static string InsertPracticeWorkPersonFile = "INSERT INTO " + "\"Person File\"" + ""
                                                               + "(" + "\"Person ID\"" + ""
                                                                + "," + "\"User ID\"" + ""
                                                                + "," + "\"Last Name\"" + ""
                                                                + "," + "\"First Name\"" + ""
                                                                + "," + "\"Person Type\"" + ""
                                                                + "," + "\"Mid init\"" + ""
                                                                + "," + "\"jrsr\"" + ""
                                                                + "," + "\"Title\"" + ""
                                                                + "," + "\"LegalName\"" + ""
                                                                + "," + "\"Address\"" + ""
                                                                + "," + "\"Address 2\"" + ""
                                                                + "," + "\"City\"" + ""
                                                                + "," + "\"State\"" + ""
                                                                + "," + "\"Zip\"" + ""
                                                                + "," + "\"Home Phone\"" + ""
                                                                + "," + "\"Work Phone 1\"" + ""
                                                                + "," + "\"Work Phone 2\"" + ""
                                                                + "," + "\"Daytime Phone\"" + ""
                                                                + "," + "\"Email Address\"" + ""
                                                                + "," + "\"Birthdate\"" + ""
                                                                + "," + "\"SSN\"" + ""
                                                                + "," + "\"Sex\"" + ""
                                                                + "," + "\"Marital Status\"" + ""
                                                                + "," + "\"info Complete\"" + ""
                                                                + "," + "\"Prim INs Plan ID\"" + ""
                                                                + "," + "\"Prim Family ded bal\"" + ""
                                                                + "," + "\"Prim Pending claims\"" + ""
                                                                + "," + "\"Prim ins paymnts YTD\"" + ""
                                                                + "," + "\"Sec Ins Plan ID\"" + ""
                                                                + "," + "\"Sec Family ded bal\"" + ""
                                                                + "," + "\"Sec Pending Claims\"" + ""
                                                                + "," + "\"Sec Ins paymnts YTD\"" + ""
                                                                + "," + "\"Prim Med INS plan ID\"" + ""
                                                                + "," + "\"Prim Med Fam Ded bal\"" + ""
                                                                + "," + "\"Prim MEd Pend Claims\"" + ""
                                                                + "," + "\"Prim Med Pmts YTD\"" + ""
                                                                + "," + "\"Sec Med Ins Plan ID\"" + ""
                                                                + "," + "\"Sec Med fam ded bal\"" + ""
                                                                + "," + "\"sec Med pend claims\"" + ""
                                                                + "," + "\"Sec Med pmts YTD\"" + ""
                                                                + "," + "\"WebEnabled\"" + ""
                                                                + "," + "\"WebUpdateneeded\"" + ""
                                                                + "," + "\"PrimaryLanguage\"" + ""
                                                                + "," + "\"CountryCodeId\"" + ""
                                                                + "," + "\"LastUpdated\"" + ""
                                                                + "," + "\"AUSMedicareNum\"" + ""
                                                                + "," + "\"FeeSched\"" + ""
                                                                + "," + "\"HIPAAConsentRcvd\"" + ""
                                                                + "," + "\"Organization\"" + ""
                                                                + "," + "\"Filler\"" + ""
                                                                + "," + "\"SpouseID\"" + ""
                                                                + "," + "\"PrimInsId\"" + ""
                                                                + "," + "\"SecINsId\"" + ""
                                                                + "," + "\"PrimMedinsId\"" + ""
                                                                + "," + "\"SecMedInsId\"" + ""
                                                                + "," + "\"Spare\"" + ") values "
                                                                + "(@PatientId,NULL,@LastName,@FirstName,1,@MiddleName,NULL,NULL,@LegalName,@Address1,@Address2,@City,@State,@Zip,@HomePhone,@WorkPhone1,@WorkPhone2,@OfficePhone,@Email"
                                                                + ",@BirthDate,@SSN,@Sex,@MaritalStatus,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,0,0,0,@Filler,0,NULL,NULL,NULL,NULL,NULL)";

        public static string InsertPracticeWorkAppointment = "INSERT INTO " + "\"Appointments\"" + "(" + "\"Date\"" + ""
                                                                + "," + "\"Resource ID\"" + ""
                                                                + "," + "\"Start time\"" + ""
                                                                + "," + "\"End time\"" + ""
                                                                + "," + "\"Visit ID\"" + ""
                                                                + "," + "\"Dr ID\"" + ""
                                                                + "," + "\"Patient ID\"" + ""
                                                                + "," + "\"Tx class ID\"" + ""
                                                                + "," + "\"Appt made on date\"" + ""
                                                                + "," + "\"Confirmed date\"" + ""
                                                                + "," + "\"Reminder sent date\"" + ""
                                                                + "," + "\"Check in time\"" + ""
                                                                + "," + "\"Seated time\"" + ""
                                                                + "," + "\"Check out time\"" + ""
                                                                + "," + "\"Description\"" + ""
                                                                + "," + "\"Status\"" + ""
                                                                + "," + "\"Cancel status\"" + ""
                                                                + "," + "\"ASAP appointment\"" + ""
                                                                + "," + "\"Recall Appointment\"" + ""
                                                                + "," + "\"Pending\"" + ""
                                                                + "," + "\"Asst time 1\"" + ""
                                                                + "," + "\"Dr time 1\"" + ""
                                                                + "," + "\"Asst time 2\"" + ""
                                                                + "," + "\"Dr time 2\"" + ""
                                                                + "," + "\"Asst time 3\"" + ""
                                                                + "," + "\"Est Production\"" + ""
                                                                + "," + "\"Note count\"" + ""
                                                                + "," + "\"Lab Case\"" + ""
                                                                + "," + "\"Lab ID\"" + ""
                                                                + "," + "\"Lab Case Pending\"" + ""
                                                                + "," + "\"WaitStarted\"" + ""
                                                                + "," + "\"TotalWaitTime\"" + ""
                                                                + "," + "\"LastUpdated\"" + ""
                                                                + "," + "\"IncompleteReason\"" + ""
                                                                + "," + "\"ColorCodeID\"" + ""
                                                                + "," + "\"NewPatAppt\"" + ""
                                                                + "," + "\"ConfirmationMethod\"" + ""
                                                                + "," + "\"CallPatientAfter\"" + ""
                                                                + "," + "\"NoDoubleBooking\"" + ""
                                                                + "," + "\"Filler\"" + ""
                                                                + "," + "\"AusApptType\"" + ""
                                                                + "," + "\"Spare\"" + ""
                                                                + ") values (@AppointmentDate,@OperatoryId,@StartDate,@EndDate,@AppointmentId,@ProviderId,@PatientId,@TaxClassId,CURDATE(),NULL,NULL,NULL,NULL,NULL,'WEB',0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,NULL,NULL,NULL,0,0,0,0,NULL,0,NULL,NULL,NULL) ";

        public static string Update_Person_Record_By_Patient_Form = " UPDATE " + "\"Person File\"" + " SET " + "\"FieldToUpdate\"" + " = ehrfield_value WHERE " + "\"Person ID\"" + " = @Patient_EHR_ID ";

        public static string Update_Patinet_Record_By_Patient_Form = " UPDATE " + "\"Patient File\"" + " SET " + "\"FieldToUpdate\"" + " = ehrfield_value WHERE " + "\"Person ID\"" + " = @Patient_EHR_ID ";


        //rooja Insurance master
        public static string GetPracticeworkInsuranceData = "SELECT '' as Clinic_Number," + "\"Insurance Co ID\"" + " ," + "\"Company Name\"" + "," + "\"Company Address1\"" + " ," + " " + "\"Company Address2\"" + "," + "\"Company City\"" + " ," + " " + "\"Company State\"" + "," + " " + "\"Company ZIP\"" + "," + " " + "\"Company phone\"" + "," + " " + "\"Elect Claims ID\"" + "," + " " + "\"Inactive\"" + " from " + "\"Ins Co Info\"" + "";

    }
}
