using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pozative.QRY
{
    public class SynchClearDentQRY
    {

        #region Appointment

        public static string GetClearDentAppointmentData = "SELECT [fld_auto_intAppId] as appointment_id , [fld_dtmStartTime] as EHR_Entry_DateTime,(LTRIM(RTRIM(p.[fld_strFName]))) AS First_name, "
                                                           + " (LTRIM(RTRIM(p.[fld_strLName]))) AS Last_name ,(LTRIM(RTRIM(p.[fld_strMIni]))) AS Middle_Name,(LTRIM(RTRIM(p.[fld_strEmail]))) AS Email , "
                                                           + " (LTRIM(RTRIM(p.[fld_strMTel]))) AS Mobile_Contact ,(LTRIM(RTRIM(p.[fld_strHTel]))) AS Home_Contact, (LTRIM(RTRIM(p.[fld_strOTel]))) AS Work_Phone, "
                                                           + " (LTRIM(RTRIM(p.[fld_strAddr1]))) AS Address1 ,(LTRIM(RTRIM(p.[fld_strAddr2]))) AS Address2, (LTRIM(RTRIM(p.fld_strCity))) AS City, "
                                                           + " (LTRIM(RTRIM(p.fld_strCountry))) AS [State] ,(LTRIM(RTRIM([fld_strPCode]))) AS Zipcode,[fld_shtChId] as   Operatory_EHR_ID,op.fld_strChName as Operatory_Name "
                                                           + ", [fld_dtmStartTime] as StartTime,[fld_dtmEndTime] as EndTime,a.fld_shtPrId as provider_id1,pr.fld_strName as ProviderName1,a.[fld_strNotes] as comment "
                                                           + ", p.fld_dtmBth as birth_date,a.[fld_shtAppTypeId] as ApptType_EHR_ID,apt.fld_strAppTypeName as ApptType,'1' as [Status], '' as Patient_Status, "
                                                           + " case when fld_blnConfirmed = 1 AND a.fld_bytAppStatId != 7  then 99 else a.fld_bytAppStatId end as appointment_status_ehr_key, case when fld_blnConfirmed = 1  AND a.fld_bytAppStatId != 7 then 'Confirmed' else apps.fld_strDesc end as Appointment_Status,'EHR' as Is_Appt,a.[fld_intPatId] as patient_ehr_id "
                                                           + ", a.[fld_dtmRemDate] as Remind_DateTime,fld_shtPr2Id as provider_id2,pr2.fld_strName as ProviderName2,fld_blnConfirmed as Confirmed, ( CASE WHEN a.fld_bytAppStatId = 9 THEN 1 ELSE 0 END ) AS is_asap  FROM [tbl_SchApp] as a inner join [tbl_PatInfo] as p on p.[fld_auto_intPatId] = a.[fld_intPatId] "
                                                           + " inner join [tbl_Pr] as pr on  pr.fld_auto_shtPrId = a.fld_shtPrId left Outer join [tbl_Pr] as pr2 on  pr2.fld_auto_shtPrId = a.fld_shtPr2Id inner join [tbl_SchCh] as Op on op.fld_auto_shtChId = a.fld_shtChId inner join tbl_SchAppType as apt "
                                                           + " on apt.fld_auto_shtAppTypeId = a.fld_shtAppTypeId inner join tbl_CfgAppStat as apps on apps.fld_auto_bytAppStatId = a.fld_bytAppStatId where apps.fld_blnfree = 0 and a.[fld_dtmStartTime] > @ToDate";

        public static string GetClearDentAppointmentEhrIds = "SELECT [fld_auto_intAppId] as Appt_EHR_ID"
                                                           + " FROM [tbl_SchApp] as a inner join [tbl_PatInfo] as p on p.[fld_auto_intPatId] = a.[fld_intPatId] "
                                                           + " inner join [tbl_Pr] as pr on  pr.fld_auto_shtPrId = a.fld_shtPrId left Outer join [tbl_Pr] as pr2 on  pr2.fld_auto_shtPrId = a.fld_shtPr2Id inner join [tbl_SchCh] as Op on op.fld_auto_shtChId = a.fld_shtChId inner join tbl_SchAppType as apt "
                                                           + " on apt.fld_auto_shtAppTypeId = a.fld_shtAppTypeId inner join tbl_CfgAppStat as apps on apps.fld_auto_bytAppStatId = a.fld_bytAppStatId where apps.fld_blnfree = 0 and a.[fld_dtmStartTime] > @ToDate";

        public static string GetClearDentAppointment_Procedures_Data = "SELECT appt.fld_auto_intAppId AS appointment_id, "
                                                            //+ "  stuff(( SELECT   ','  + " 
                                                            //+ " 				(CASE WHEN CONVERT(Varchar(5),Prc.fld_strTh) !='' THEN CONVERT(Varchar(5),Prc.fld_strTh) + '-'  ELSE '' END)  " 
                                                            //+ " 				+ (CASE WHEN CONVERT(Varchar(5),Prc.fld_strSf) !='' THEN CONVERT(Varchar(5),Prc.fld_strSf) + '-'  ELSE '' END)  " 
                                                            //+ " 				+ PCode.fld_strCDesc  "
                                                            //+ " 			FROM tbl_SchApp_TrPlanProc Pro WITH(NOLOCK)  "
                                                            //+ "             LEFT JOIN tbl_TrPlanProc Prc WITH(NOLOCK) On Prc.fld_auto_intTransProcId = Pro.fld_intTrPlanProcId "
                                                            //+ " 			LEFT JOIN tbl_ProcInfo PCode WITH(NOLOCK) ON Prc.fld_strProcCode = PCode.fld_strProcCode " 
                                                            //+ " 			WHERE Pro.fld_intSchAppId = appt.fld_auto_intAppId " 
                                                            //+ " 			ORDER BY Prc.fld_strProcCode " 
                                                            //+ " 			for xml path('')),1,1,'') as ProcedureDesc , " 
                                                            + "  stuff(( SELECT   ','  + Prc.fld_strProcCode "
                                                            + " 			FROM tbl_SchApp_TrPlanProc Pro WITH(NOLOCK)  "
                                                            + " 				LEFT JOIN tbl_TrPlanProc Prc WITH(NOLOCK) On Prc.fld_auto_intTransProcId = Pro.fld_intTrPlanProcId "
                                                            + " 			WHERE Pro.fld_intSchAppId = appt.fld_auto_intAppId "
                                                            + " 			ORDER BY Prc.fld_strProcCode "
                                                            + " 			for xml path('')),1,1,'') as ProcedureCode  "
                                                            + " FROM tbl_SchApp appt inner join tbl_CfgAppStat as ass on ass.fld_auto_bytAppStatId = appt.fld_bytAppStatId "
                                                            + "  where ass.fld_blnfree = 0 and appt.[fld_dtmStartTime] > @ToDate "
                                                            + " GROUP BY appt.fld_auto_intAppId ;";

        public static string GetClearDentAppointment_Procedures_DataByAptID = "SELECT appt.fld_auto_intAppId AS appointment_id, "
                                                            //+ "  stuff(( SELECT   ','  + " 
                                                            //+ " 				(CASE WHEN CONVERT(Varchar(5),Prc.fld_strTh) !='' THEN CONVERT(Varchar(5),Prc.fld_strTh) + '-'  ELSE '' END)  " 
                                                            //+ " 				+ (CASE WHEN CONVERT(Varchar(5),Prc.fld_strSf) !='' THEN CONVERT(Varchar(5),Prc.fld_strSf) + '-'  ELSE '' END)  " 
                                                            //+ " 				+ PCode.fld_strCDesc  "
                                                            //+ " 			FROM tbl_SchApp_TrPlanProc Pro WITH(NOLOCK)  "
                                                            //+ "             LEFT JOIN tbl_TrPlanProc Prc WITH(NOLOCK) On Prc.fld_auto_intTransProcId = Pro.fld_intTrPlanProcId "
                                                            //+ " 			LEFT JOIN tbl_ProcInfo PCode WITH(NOLOCK) ON Prc.fld_strProcCode = PCode.fld_strProcCode " 
                                                            //+ " 			WHERE Pro.fld_intSchAppId = appt.fld_auto_intAppId " 
                                                            //+ " 			ORDER BY Prc.fld_strProcCode " 
                                                            //+ " 			for xml path('')),1,1,'') as ProcedureDesc , " 
                                                            + "  stuff(( SELECT   ','  + Prc.fld_strProcCode "
                                                            + " 			FROM tbl_SchApp_TrPlanProc Pro WITH(NOLOCK)  "
                                                            + " 				LEFT JOIN tbl_TrPlanProc Prc WITH(NOLOCK) On Prc.fld_auto_intTransProcId = Pro.fld_intTrPlanProcId "
                                                            + " 			WHERE Pro.fld_intSchAppId = appt.fld_auto_intAppId "
                                                            + " 			ORDER BY Prc.fld_strProcCode "
                                                            + " 			for xml path('')),1,1,'') as ProcedureCode  "
                                                            + " FROM tbl_SchApp appt inner join tbl_CfgAppStat as ass on ass.fld_auto_bytAppStatId = appt.fld_bytAppStatId "
                                                            + "  where ass.fld_blnfree = 0 and appt.[fld_dtmStartTime] > @ToDate And appt.fld_auto_intAppId = @Appt_EHR_ID "
                                                            + " GROUP BY appt.fld_auto_intAppId ;";

        #region DeletedAppointment
        //Below  logic change is for client specific to The Art Of Smile Dental Clinic(Vancouver) - https://app.asana.com/0/1204010716278938/1206862291905537/f
        public static string GetClearDentDeletedAppointmentData = "Select [fld_intAppId] AS Appt_EHR_ID, [fld_dtmCreateDate] as EHR_Entry_DateTime from [tbl_SchAppDeleted] Where [fld_intPatId] is not null and [fld_dtmCreateDate] > @ToDate "
                                                               + " Union all select ap.[fld_auto_intAppId] as appointment_id ,ap.[fld_dtmStartTime] as EHR_Entry_DateTime from [tbl_SchApp] ap inner join tbl_CfgAppStat ass on ap.fld_bytAppStatId = ass.fld_auto_bytAppStatId Where (ass.fld_blnfree = 1 or ass.fld_auto_bytAppStatId=5 or ass.fld_auto_bytAppStatId = 8 or ass.fld_auto_bytAppStatId = 4 or ass.fld_auto_bytAppStatId = 6 or ass.fld_auto_bytAppStatId = 15) and [fld_intPatId] is not null and [fld_dtmStartTime] > @ToDate ";//and ([fld_bytAppStatId] = 8 or [fld_bytAppStatId] = 5 or [fld_bytAppStatId] = 4 or [fld_bytAppStatId] = 6 or [fld_bytAppStatId] = 15 )  ";
        #endregion

        //public static string GetClearDentPatientData = " SELECT v_p.patient_id AS Patient_EHR_ID, (LTRIM(RTRIM(v_p.first_name))) AS First_name, (LTRIM(RTRIM(v_p.last_name))) AS Last_name, "
        //                                    + " (LTRIM(RTRIM(v_p.mi))) AS Middle_Name, (LTRIM(RTRIM(v_p.Salutation))) AS Salutation,(LTRIM(RTRIM(v_p.preferred_name))) AS preferred_name,"
        //                                    + "  v_p.status AS Status, v_p.birth_date AS Birth_Date, (LTRIM(RTRIM(v_p.email))) AS Email, (LTRIM(RTRIM(v_p.mobile_phone))) AS Mobile, "
        //                                    + " (LTRIM(RTRIM(v_p.home_phone))) AS Home_Phone, (LTRIM(RTRIM(v_p.work_phone))) AS Work_Phone, (LTRIM(RTRIM(v_p.address_line1))) AS Address1, "
        //                                    + " (LTRIM(RTRIM(v_p.address_line2))) AS Address2, (LTRIM(RTRIM(v_p.city))) AS City, (LTRIM(RTRIM(v_p.state))) AS State, "
        //                                    + " (LTRIM(RTRIM(v_p.zipcode))) AS Zipcode,v_p.guar_id AS Guar_ID, "
        //                                    + " v_p.pri_provider_id AS Pri_Provider_ID, v_p.sec_provider_id AS Sec_Provider_ID, "
        //                                    + " v_p.first_visit_date AS FirstVisit_Date, v_p.last_visit_date AS LastVisit_Date, "
        //                                    + " v_pi.primary_insured_id AS Primary_Insurance, "
        //                                    + " (LTRIM(RTRIM(v_pi.primary_insured_last_Name)) + ' '  + LTRIM(RTRIM(v_pi.primary_insured_First_Name))) AS Primary_Insurance_CompanyName, "
        //                                    + " v_pi.secondary_insured_id AS Secondary_Insurance, "
        //                                    + " (LTRIM(RTRIM(v_pi.secondary_insured_last_name)) + ' '  + LTRIM(RTRIM(v_pi.secondary_insured_first_name))) AS Secondary_Insurance_CompanyName, "
        //                                     ''  AS nextvisit_date,'' AS due_date , "
        //                                    + " ((ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid = v_pi.primary_insured_id),0) "
        //                                    + " + (ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid =v_pi.secondary_insured_id ),0))) "
        //                                    + " - (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))) as remaining_benefit , "
        //                                    + " (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0)) as used_benefit, "
        //                                    + " '' AS collect_payment, v_p.privacy_flags "
        //                                    + " FROM admin.v_patient v_p JOIN admin.v_patient_insurance v_pi ON v_p.patient_id = v_pi.patient_id "
        //                                    + " Left Join admin.v_guarantor_balance v_gb ON v_p.guar_id = v_gb.guar_id  ";

        #endregion

        #region Patient
        public static string GetClearDentPatientData = "SELECT * INTO #TempPay FROM (SELECT Sum(PatAmt) as PatAmt , sum(InsAmt) as UsedBeneFit , sum(Total) as CollectPayment, flt_intPatId FROM "
                                                       + " ( SELECT   PatAmt, InsAmt, Total, flt_intPatId FROM Vw_Ledger_Billing WHERE ([Desciption] NOT LIKE 'Pat. Payment%') UNION "
                                                       + "   SELECT PatPmt, InsPmt, Total, flt_intPatId FROM Vw_Ledger_Adjustment WHERE ([Description] NOT LIKE 'Pat. Payment%') UNION "
                                                       + "   SELECT PatPmt, InsPmt, Total, flt_intPatId FROM Vw_Ledger_Payment WHERE ([Description] NOT LIKE 'Pat. Payment%') UNION "
                                                       + "   SELECT fld_fltPatPmt, fld_fltInsPmt, Total, fld_intPatId FROM Vw_Ledger_HeldPayment WHERE ([Description] NOT LIKE 'Pat. Payment%')) AS A "
                                                       + "   group by flt_intPatId) as ppay" + Environment.NewLine 
                                                       + " select DISTINCT 0 AS Clinic_Number, 1 as Service_Install_Id, p.*,dts.FirstVisit_Date as FirstVisit_Date,dts.LastVisit_Date as LastVisit_Date, "
                                                       + " ndts.nextvisit_date  AS nextvisit_date, "
                                                       + " CONVERT(varchar, DUED.due_date) + '@'+DUED.recall_type + '@'+CONVERT(varchar,DUED.recall_type_id) + '|' AS due_date , "
                                                       + " 0 as remaining_benefit , case when pc.fld_bytMPriority = 0 then 'N' else 'Y' end as ReceiveVoiceCall,case when pc.fld_bytEmail = 0 then 'N' else 'Y' end as ReceiveEmail,case when pc.fld_blnMSMS = 0 then 'N' else 'Y' end as ReceiveSms , "
                                                       + "  isnull(ppay.UsedBeneFit,0) as used_benefit,isnull(ppay.CollectPayment,0) AS collect_payment, "
                                                       + " '' as recall_type ,'' as recall_type_id, "
                                                       + " ptpol.fld_strSubIdNo as Primary_Ins_Subscriber_ID,ptpol2.fld_strSubIdNo as Secondary_Ins_Subscriber_ID, "
                                                       + " (CASE WHEN INSP.Primary_Insurance IS NOT NULL THEN  INSP.Primary_Insurance ELSE 0 END ) as Primary_Insurance , "
                                                       + " (CASE WHEN INSP.Primary_Insurance_CompanyName IS NOT NULL THEN  INSP.Primary_Insurance_CompanyName ELSE '' END ) as Primary_Insurance_CompanyName,Prim_Ins_Company_Phonenumber, "
                                                       + " (CASE WHEN INSS.Secondary_Insurance IS NOT NULL THEN INSS.Secondary_Insurance ELSE 0 END ) as Secondary_Insurance, "
                                                       + " (CASE WHEN INSS.Secondary_Insurance_CompanyName IS NOT NULL THEN INSS.Secondary_Insurance_CompanyName ELSE '' END ) as Secondary_Insurance_CompanyName, Sec_Ins_Company_Phonenumber from "
                                                       + " (SELECT [fld_auto_intPatId] as Patient_EHR_ID ,(LTRIM(RTRIM([fld_strFName]))) AS First_name ,(LTRIM(RTRIM([fld_strLName]))) AS Last_name , "
                                                       // + " (LTRIM(RTRIM([fld_strMIni]))) AS Middle_Name ,(LTRIM(RTRIM(PT.fld_strTitle))) as Salutation , (LTRIM(RTRIM([fld_strPName]))) as preferred_name ,Case when isnull([fld_strSex],'0') = '0' then '' else [fld_strSex] end as Sex,'' as MaritalStatus,case when [fld_bytStatus] = 0 then 'I' else 'A' end as [Status],case when [fld_bytStatus] = 0 then 'INACTIVE' else 'ACTIVE' end as [EHR_Status] , "
                                                       + " (LTRIM(RTRIM([fld_strMIni]))) AS Middle_Name ,(LTRIM(RTRIM(PT.fld_strTitle))) as Salutation , (LTRIM(RTRIM([fld_strPName]))) as preferred_name ,Case when isnull([fld_strSex],'0') = '0' then '' else [fld_strSex] end as Sex,'' as MaritalStatus, case when [fld_bytStatus] = 0 then 'I' else 'A' end as [Status], CASE WHEN fld_bytStatus = 1 and pp.fld_strType != 'Deceased' THEN 'Active' WHEN fld_bytStatus = 1 and pp.fld_strType = 'Deceased' THEN 'InActive' WHEN fld_bytStatus = 0 THEN 'InActive' END AS EHR_Status, "
                                                       + " [fld_dtmBth] as Birth_Date ,(LTRIM(RTRIM([fld_strEmail])))  AS Email ,case when (LTRIM(RTRIM([fld_strMTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strMTel]))) end AS Mobile "
                                                       + " , case when (LTRIM(RTRIM([fld_strHTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strHTel]))) end AS Home_Phone ,case when (LTRIM(RTRIM([fld_strOTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strOTel]))) end  AS Work_Phone"
                                                       + " , (LTRIM(RTRIM([fld_strAddr1]))) AS Address1, (LTRIM(RTRIM([fld_strAddr2]))) AS Address2, (LTRIM(RTRIM(fld_strCity))) AS City, (LTRIM(RTRIM(fld_strProv))) AS [State]"
                                                       + " , (LTRIM(RTRIM([fld_strPCode]))) AS Zipcode, fam.fld_intHeadPatId AS Guar_ID , [fld_shtPrId] AS Pri_Provider_ID, [fld_shtHyId] AS Sec_Provider_ID"
                                                       + ",(fld_intSIN)as ssn,'' as emergencycontactId,fld_strEmergencyContact as emergencycontactnumber ,fld_strEmergencyName as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name ,fld_strSchool as school,responsiblepartyId,ResponsibleParty_First_Name,ResponsibleParty_Last_Name,responsiblepartybirthdate,responsiblepartyssn,'' as groupid,'' as spouseId, '' as Spouse_First_Name, '' as Spouse_Last_Name, '' as driverlicense, '' as employer"
                                                       + ", isnull(pfpi.fld_fltPCurr,0) as ThirtyDay , isnull(pfpi.fld_fltP30,0) as SixtyDay, isnull(pfpi.fld_fltP60,0) as NinetyDay, isnull(pfpi.fld_fltP90,0) as Over90"
                                                       + ", isnull(pfpi.fld_fltPCurr,0) + isnull(pfpi.fld_fltP30,0) + isnull(pfpi.fld_fltP60,0) + isnull(pfpi.fld_fltP90,0) As CurrentBal, 1 AS InsUptDlt, fld_intPri, fld_intSec,case when isnull(pin.fld_strPrefLang,'') ='' THEN 'English' else  isnull(pin.fld_strPrefLang,'') END  AS PreferredLanguage,pin.fld_strcontactnotes AS Patient_Note, 0 as IS_deleted"
                                                       + " FROM [tbl_PatInfo] as pin left join Tbl_PatFam Fam WIth(Nolock) ON pin.fld_intFamId = Fam.fld_auto_intfamid "
                                                       + " left join [tbl_PatType] as pp on pp.fld_auto_bytPatTypeId = pin.fld_byttypeid  "
                                                       + "left join (select pf.fld_intHeadPatId as responsiblepartyId,fld_strFName as ResponsibleParty_First_Name,fld_strLName as ResponsibleParty_Last_Name,fld_dtmBth as responsiblepartybirthdate, fld_intSIN as responsiblepartyssn  from tbl_PatInfo p,tbl_PatFam pf where p.fld_auto_intPatId=pf.fld_intHeadPatId )as resp on resp.responsiblepartyId=pin.fld_auto_intPatId"
                                                       + " left outer join [tbl_PatAR] as pfpi on pin.fld_auto_intPatId = pfpi.fld_intPatId "
                                                       + " left outer JOIN tbl_CfgPatTitle AS PT ON pin.fld_bytTitle = pt.fld_bytId) as p "                                                       
                                                       + " left JOIN tbl_PatContactPriority as pc ON p.Patient_EHR_ID = pc.fld_intPatId "
                                                       + " left join tbl_PatPol ptpol on ptpol.fld_intPatId = p.Patient_EHR_ID and p.fld_intPri = ptpol.fld_auto_intPatPolId "
                                                       + " left join tbl_PatPol ptpol2 on ptpol2.fld_intPatId = p.Patient_EHR_ID and p.fld_intSec = ptpol2.fld_auto_intPatPolId "
                                                       + " left outer join (SELECT ap.[fld_intPatId] ,min(ap.[fld_dtmStartTime]) as FirstVisit_Date,max(ap.[fld_dtmStartTime]) as LastVisit_Date FROM [tbl_SchApp] ap inner join tbl_CfgAppStat ass on ap.fld_bytAppStatId = ass.fld_auto_bytAppStatId where ap.[fld_dtmStartTime] < getdate() and ap.fld_bytAppStatId = 7 and ass.fld_blnfree = 0 and ap.[fld_intPatId] is not null group by ap.[fld_intPatId]) as dts on p.Patient_EHR_ID = dts.fld_intPatId "
                                                       + " left outer join (select ap.[fld_intPatId] , min(ap.[fld_dtmStartTime])  as nextvisit_date FROM [tbl_SchApp] ap inner join tbl_CfgAppStat ass on ap.fld_bytAppStatId = ass.fld_auto_bytAppStatId where ap.[fld_dtmStartTime] > getdate()  and [fld_intPatId] is not null and ass.fld_blnfree = 0 group by ap.[fld_intPatId]) as ndts on p.Patient_EHR_ID = ndts.fld_intPatId"

                                                       //  +" left outer join (SELECT [fld_intPatId] ,min ([fld_dtmStartTime]) as FirstVisit_Date,max([fld_dtmStartTime]) as LastVisit_Date FROM [tbl_SchApp] where [fld_dtmStartTime] < getdate() group by [fld_intPatId] ) as dts on p.Patient_EHR_ID = dts.fld_intPatId "
                                                       // +" left outer join (select [fld_intPatId] ,min([fld_dtmStartTime])  as nextvisit_date FROM [tbl_SchApp] where [fld_dtmStartTime] > getdate() group by [fld_intPatId] ) as ndts on p.Patient_EHR_ID = ndts.fld_intPatId "
                                                       + " LEFT JOIN ( Select  [fld_intPatId] as Patient_EHR_ID, ([fld_dtmDueDate]) as due_date, (rt.[fld_strRecTypeName]) as recall_type,(r.[fld_bytRecType]) as recall_type_id From "
                                                       + "      ( SELECT  MAX(fld_auto_intrecid) AS fld_auto_intrecid, fld_intpatid AS Patient_EHR_Id "
                                                       + "      FROM [tbl_Rec] as r inner join [tbl_RecType] as rt on  rt.fld_auto_bytRecTypeId = r.fld_bytRecType GROUP BY [fld_intPatId] ) AS RTC "
                                                       + "      INNER JOIN [tbl_Rec] as r ON r.fld_auto_intrecid = RTC.fld_auto_intrecid "
                                                       + "      inner join [tbl_RecType] as rt on  rt.fld_auto_bytRecTypeId = r.fld_bytRecType ) AS DUED ON DUED.Patient_EHR_ID =  p.Patient_EHR_ID "
                                                       + " LEFT JOIN (SELECT * FROM ( SELECT row_number() OVER ( PARTITION BY[fld_intSubPatId] ORDER BY [fld_intCarrId]) AS ID, "
                                                       + "  [fld_intCarrId] as Primary_Insurance ,[fld_strName] as Primary_Insurance_CompanyName ,[fld_strTel] as Prim_Ins_Company_Phonenumber,[fld_intFamId] ,[fld_intSubPatId] FROM [tbl_PatFamPol]as pfp "
                                                       + " inner join [tbl_InsPol] as ip on ip.fld_auto_intInsPolId = pfp.[fld_intInsPolId] "
                                                       + " inner join [tbl_InsCarr] as ic on ic.fld_auto_intCarrId = ip.fld_intCarrId ) AS AA WHERE ID = 1 ) AS INSP ON INSP.fld_intSubPatId =  p.Patient_EHR_ID "
                                                       + " LEFT JOIN ( SELECT * FROM ( SELECT row_number() OVER ( PARTITION BY[fld_intSubPatId] ORDER BY [fld_intCarrId]) AS ID, "
                                                       + "  [fld_intCarrId] as Secondary_Insurance ,[fld_strName] as Secondary_Insurance_CompanyName ,[fld_strTel] as Sec_Ins_Company_Phonenumber,[fld_intFamId] ,[fld_intSubPatId] FROM [tbl_PatFamPol]as pfp "
                                                       + " inner join [tbl_InsPol] as ip on ip.fld_auto_intInsPolId = pfp.[fld_intInsPolId] "
                                                       + " inner join [tbl_InsCarr] as ic on ic.fld_auto_intCarrId = ip.fld_intCarrId ) AS AB WHERE ID = 2 ) AS INSS ON INSS.fld_intSubPatId =  p.Patient_EHR_ID "
                                                       + " left outer join #TempPay as ppay on ppay.flt_intPatId = p.Patient_EHR_ID " 
                                                       + Environment.NewLine
                                                       + " Drop table #TempPay";

        public static string GetClearDentPatientDataOfPatientIds = "select DISTINCT 0 AS Clinic_Number, 1 as Service_Install_Id, p.*,dts.FirstVisit_Date as FirstVisit_Date,dts.LastVisit_Date as LastVisit_Date, "
                                                      + " ndts.nextvisit_date  AS nextvisit_date, "
                                                      + " CONVERT(varchar, DUED.due_date) + '@'+DUED.recall_type + '@'+CONVERT(varchar,DUED.recall_type_id) + '|' AS due_date , "
                                                      + " 0 as remaining_benefit , case when pc.fld_bytMPriority = 0 then 'N' else 'Y' end as ReceiveVoiceCall,case when pc.fld_bytEmail = 0 then 'N' else 'Y' end as ReceiveEmail,case when pc.fld_blnMSMS = 0 then 'N' else 'Y' end as ReceiveSms , "
                                                      + "  isnull(ppay.UsedBeneFit,0) as used_benefit,isnull(ppay.CollectPayment,0) AS collect_payment, "
                                                      + " '' as recall_type ,'' as recall_type_id, "
                                                      + " ptpol.fld_strSubIdNo as Primary_Ins_Subscriber_ID,ptpol2.fld_strSubIdNo as Secondary_Ins_Subscriber_ID, "
                                                      + " (CASE WHEN INSP.Primary_Insurance IS NOT NULL THEN  INSP.Primary_Insurance ELSE 0 END ) as Primary_Insurance , "
                                                      + " (CASE WHEN INSP.Primary_Insurance_CompanyName IS NOT NULL THEN  INSP.Primary_Insurance_CompanyName ELSE '' END ) as Primary_Insurance_CompanyName,Prim_Ins_Company_Phonenumber, "
                                                      + " (CASE WHEN INSS.Secondary_Insurance IS NOT NULL THEN INSS.Secondary_Insurance ELSE 0 END ) as Secondary_Insurance, "
                                                      + " (CASE WHEN INSS.Secondary_Insurance_CompanyName IS NOT NULL THEN INSS.Secondary_Insurance_CompanyName ELSE '' END ) as Secondary_Insurance_CompanyName, Sec_Ins_Company_Phonenumber from "
                                                      + " (SELECT [fld_auto_intPatId] as Patient_EHR_ID ,(LTRIM(RTRIM([fld_strFName]))) AS First_name ,(LTRIM(RTRIM([fld_strLName]))) AS Last_name , "
                                                      // + " (LTRIM(RTRIM([fld_strMIni]))) AS Middle_Name ,(LTRIM(RTRIM(PT.fld_strTitle))) as Salutation , (LTRIM(RTRIM([fld_strPName]))) as preferred_name ,Case when isnull([fld_strSex],'0') = '0' then '' else [fld_strSex] end as Sex,'' as MaritalStatus,case when [fld_bytStatus] = 0 then 'I' else 'A' end as [Status],case when [fld_bytStatus] = 0 then 'INACTIVE' else 'ACTIVE' end as [EHR_Status] , "
                                                      + " (LTRIM(RTRIM([fld_strMIni]))) AS Middle_Name ,(LTRIM(RTRIM(PT.fld_strTitle))) as Salutation , (LTRIM(RTRIM([fld_strPName]))) as preferred_name ,Case when isnull([fld_strSex],'0') = '0' then '' else [fld_strSex] end as Sex,'' as MaritalStatus, case when [fld_bytStatus] = 0 then 'I' else 'A' end as [Status], CASE WHEN fld_bytStatus = 1 and pp.fld_strType != 'Deceased' THEN 'Active' WHEN fld_bytStatus = 1 and pp.fld_strType = 'Deceased' THEN 'InActive' WHEN fld_bytStatus = 0 THEN 'InActive' END AS EHR_Status, "
                                                      + " [fld_dtmBth] as Birth_Date ,(LTRIM(RTRIM([fld_strEmail])))  AS Email ,case when (LTRIM(RTRIM([fld_strMTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strMTel]))) end AS Mobile "
                                                      + " , case when (LTRIM(RTRIM([fld_strHTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strHTel]))) end AS Home_Phone ,case when (LTRIM(RTRIM([fld_strOTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strOTel]))) end  AS Work_Phone"
                                                      + " , (LTRIM(RTRIM([fld_strAddr1]))) AS Address1, (LTRIM(RTRIM([fld_strAddr2]))) AS Address2, (LTRIM(RTRIM(fld_strCity))) AS City, (LTRIM(RTRIM(fld_strCountry))) AS [State]"
                                                      + " , (LTRIM(RTRIM([fld_strPCode]))) AS Zipcode, fam.fld_intHeadPatId AS Guar_ID , [fld_shtPrId] AS Pri_Provider_ID, [fld_shtHyId] AS Sec_Provider_ID"
                                                      + ",(fld_intSIN)as ssn,'' as emergencycontactId,fld_strEmergencyContact as emergencycontactnumber ,fld_strEmergencyName as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name ,fld_strSchool as school,responsiblepartyId,ResponsibleParty_First_Name,ResponsibleParty_Last_Name,responsiblepartybirthdate,responsiblepartyssn,'' as groupid,'' as spouseId, '' as Spouse_First_Name, '' as Spouse_Last_Name, '' as driverlicense, '' as employer"
                                                      + ", isnull(pfpi.fld_fltPCurr,0) as ThirtyDay , isnull(pfpi.fld_fltP30,0) as SixtyDay, isnull(pfpi.fld_fltP60,0) as NinetyDay, isnull(pfpi.fld_fltP90,0) as Over90"
                                                      + ", isnull(pfpi.fld_fltPCurr,0) + isnull(pfpi.fld_fltP30,0) + isnull(pfpi.fld_fltP60,0) + isnull(pfpi.fld_fltP90,0) As CurrentBal, 1 AS InsUptDlt, fld_intPri, fld_intSec,case when isnull(pin.fld_strPrefLang,'') ='' THEN 'English' else  isnull(pin.fld_strPrefLang,'') END  AS PreferredLanguage,pin.fld_strcontactnotes AS Patient_Note, 0 as IS_deleted"
                                                      + " FROM [tbl_PatInfo] as pin left join Tbl_PatFam Fam WIth(Nolock) ON pin.fld_intFamId = Fam.fld_auto_intfamid "
                                                      + " left join [tbl_PatType] as pp on pp.fld_auto_bytPatTypeId = pin.fld_byttypeid  "
                                                      + "left join (select pf.fld_intHeadPatId as responsiblepartyId,fld_strFName as ResponsibleParty_First_Name,fld_strLName as ResponsibleParty_Last_Name,fld_dtmBth as responsiblepartybirthdate, fld_intSIN as responsiblepartyssn  from tbl_PatInfo p,tbl_PatFam pf where p.fld_auto_intPatId=pf.fld_intHeadPatId )as resp on resp.responsiblepartyId=pin.fld_auto_intPatId"
                                                      + " left outer join [tbl_PatAR] as pfpi on pin.fld_auto_intPatId = pfpi.fld_intPatId "
                                                      + " left outer JOIN tbl_CfgPatTitle AS PT ON pin.fld_bytTitle = pt.fld_bytId) as p "                                                      
                                                      + " left JOIN tbl_PatContactPriority as pc ON p.Patient_EHR_ID = pc.fld_intPatId "
                                                      + " left join tbl_PatPol ptpol on ptpol.fld_intPatId = p.Patient_EHR_ID and p.fld_intPri = ptpol.fld_auto_intPatPolId "
                                                      + " left join tbl_PatPol ptpol2 on ptpol2.fld_intPatId = p.Patient_EHR_ID and p.fld_intSec = ptpol2.fld_auto_intPatPolId "
                                                      + " left outer join (SELECT ap.[fld_intPatId] ,min(ap.[fld_dtmStartTime]) as FirstVisit_Date,max(ap.[fld_dtmStartTime]) as LastVisit_Date FROM [tbl_SchApp] ap inner join tbl_CfgAppStat ass on ap.fld_bytAppStatId = ass.fld_auto_bytAppStatId where ap.[fld_dtmStartTime] < getdate() and ap.fld_bytAppStatId = 7 and ass.fld_blnfree = 0 and ap.[fld_intPatId] is not null group by ap.[fld_intPatId]) as dts on p.Patient_EHR_ID = dts.fld_intPatId "
                                                      + " left outer join (select ap.[fld_intPatId] , min(ap.[fld_dtmStartTime])  as nextvisit_date FROM [tbl_SchApp] ap inner join tbl_CfgAppStat ass on ap.fld_bytAppStatId = ass.fld_auto_bytAppStatId where ap.[fld_dtmStartTime] > getdate()  and [fld_intPatId] is not null and ass.fld_blnfree = 0 group by ap.[fld_intPatId]) as ndts on p.Patient_EHR_ID = ndts.fld_intPatId"

                                                      //  +" left outer join (SELECT [fld_intPatId] ,min ([fld_dtmStartTime]) as FirstVisit_Date,max([fld_dtmStartTime]) as LastVisit_Date FROM [tbl_SchApp] where [fld_dtmStartTime] < getdate() group by [fld_intPatId] ) as dts on p.Patient_EHR_ID = dts.fld_intPatId "
                                                      // +" left outer join (select [fld_intPatId] ,min([fld_dtmStartTime])  as nextvisit_date FROM [tbl_SchApp] where [fld_dtmStartTime] > getdate() group by [fld_intPatId] ) as ndts on p.Patient_EHR_ID = ndts.fld_intPatId "
                                                      + " LEFT JOIN ( Select  [fld_intPatId] as Patient_EHR_ID, ([fld_dtmDueDate]) as due_date, (rt.[fld_strRecTypeName]) as recall_type,(r.[fld_bytRecType]) as recall_type_id From "
                                                      + "      ( SELECT  MAX(fld_auto_intrecid) AS fld_auto_intrecid, fld_intpatid AS Patient_EHR_Id "
                                                      + "      FROM [tbl_Rec] as r inner join [tbl_RecType] as rt on  rt.fld_auto_bytRecTypeId = r.fld_bytRecType GROUP BY [fld_intPatId] ) AS RTC "
                                                      + "      INNER JOIN [tbl_Rec] as r ON r.fld_auto_intrecid = RTC.fld_auto_intrecid "
                                                      + "      inner join [tbl_RecType] as rt on  rt.fld_auto_bytRecTypeId = r.fld_bytRecType ) AS DUED ON DUED.Patient_EHR_ID =  p.Patient_EHR_ID "
                                                      + " LEFT JOIN (SELECT * FROM ( SELECT row_number() OVER ( PARTITION BY[fld_intSubPatId] ORDER BY [fld_intCarrId]) AS ID, "
                                                      + "  [fld_intCarrId] as Primary_Insurance ,[fld_strName] as Primary_Insurance_CompanyName ,[fld_strTel] as Prim_Ins_Company_Phonenumber,[fld_intFamId] ,[fld_intSubPatId] FROM [tbl_PatFamPol]as pfp "
                                                      + " inner join [tbl_InsPol] as ip on ip.fld_auto_intInsPolId = pfp.[fld_intInsPolId] "
                                                      + " inner join [tbl_InsCarr] as ic on ic.fld_auto_intCarrId = ip.fld_intCarrId ) AS AA WHERE ID = 1 ) AS INSP ON INSP.fld_intSubPatId =  p.Patient_EHR_ID "
                                                      + " LEFT JOIN ( SELECT * FROM ( SELECT row_number() OVER ( PARTITION BY[fld_intSubPatId] ORDER BY [fld_intCarrId]) AS ID, "
                                                      + "  [fld_intCarrId] as Secondary_Insurance ,[fld_strName] as Secondary_Insurance_CompanyName ,[fld_strTel] as Sec_Ins_Company_Phonenumber,[fld_intFamId] ,[fld_intSubPatId] FROM [tbl_PatFamPol]as pfp "
                                                      + " inner join [tbl_InsPol] as ip on ip.fld_auto_intInsPolId = pfp.[fld_intInsPolId] "
                                                      + " inner join [tbl_InsCarr] as ic on ic.fld_auto_intCarrId = ip.fld_intCarrId ) AS AB WHERE ID = 2 ) AS INSS ON INSS.fld_intSubPatId =  p.Patient_EHR_ID "
                                                      + " left outer join (SELECT Sum(PatAmt) as PatAmt , sum(InsAmt) as UsedBeneFit , sum(Total) as CollectPayment, flt_intPatId FROM "
                                                      + " ( SELECT   PatAmt, InsAmt, Total, flt_intPatId FROM Vw_Ledger_Billing WHERE  flt_intPatId in(@PatientIds) And ([Desciption] NOT LIKE 'Pat. Payment%') UNION "
                                                      + "   SELECT PatPmt, InsPmt, Total, flt_intPatId FROM Vw_Ledger_Adjustment WHERE flt_intPatId in(@PatientIds) And ([Description] NOT LIKE 'Pat. Payment%') UNION "
                                                      + "   SELECT PatPmt, InsPmt, Total, flt_intPatId FROM Vw_Ledger_Payment WHERE flt_intPatId in(@PatientIds) And ([Description] NOT LIKE 'Pat. Payment%') UNION "
                                                      + "   SELECT fld_fltPatPmt, fld_fltInsPmt, Total, fld_intPatId FROM Vw_Ledger_HeldPayment WHERE fld_intPatId in(@PatientIds) And ([Description] NOT LIKE 'Pat. Payment%')) AS A "
                                                      + "   group by flt_intPatId) as ppay on ppay.flt_intPatId = p.Patient_EHR_ID where p.Patient_EHR_ID in(@PatientIds)";


        public static string GetClearDentAppointmentsPatientData = "SELECT * INTO #TempPay FROM (SELECT Sum(PatAmt) as PatAmt , sum(InsAmt) as UsedBeneFit , sum(Total) as CollectPayment, flt_intPatId FROM "
                                               + " ( SELECT   PatAmt, InsAmt, Total, flt_intPatId FROM Vw_Ledger_Billing WHERE ([Desciption] NOT LIKE 'Pat. Payment%') UNION "
                                               + "   SELECT PatPmt, InsPmt, Total, flt_intPatId FROM Vw_Ledger_Adjustment WHERE ([Description] NOT LIKE 'Pat. Payment%') UNION "
                                               + "   SELECT PatPmt, InsPmt, Total, flt_intPatId FROM Vw_Ledger_Payment WHERE ([Description] NOT LIKE 'Pat. Payment%') UNION "
                                               + "   SELECT fld_fltPatPmt, fld_fltInsPmt, Total, fld_intPatId FROM Vw_Ledger_HeldPayment WHERE ([Description] NOT LIKE 'Pat. Payment%')) AS A "
                                               + "   group by flt_intPatId) as ppay" + Environment.NewLine 
                                               + "select DISTINCT 0 AS Clinic_Number, 1 as Service_Install_Id, p.*,dts.FirstVisit_Date as FirstVisit_Date,dts.LastVisit_Date as LastVisit_Date, "
                                               + " ndts.nextvisit_date  AS nextvisit_date, "
                                               + " CONVERT(varchar, DUED.due_date) + '@'+DUED.recall_type + '@'+CONVERT(varchar,DUED.recall_type_id) + '|' AS due_date , "
                                               + " 0 as remaining_benefit , case when pc.fld_bytMPriority = 0 then 'N' else 'Y' end as ReceiveVoiceCall,case when pc.fld_bytEmail = 0 then 'N' else 'Y' end as ReceiveEmail,case when pc.fld_blnMSMS = 0 then 'N' else 'Y' end as ReceiveSms , "
                                               + "  isnull(ppay.UsedBeneFit,0) as used_benefit,isnull(ppay.CollectPayment,0) AS collect_payment, "
                                               + " '' as recall_type ,'' as recall_type_id, "
                                               + " ptpol.fld_strSubIdNo as Primary_Ins_Subscriber_ID,ptpol2.fld_strSubIdNo as Secondary_Ins_Subscriber_ID, "
                                               + " (CASE WHEN INSP.Primary_Insurance IS NOT NULL THEN  INSP.Primary_Insurance ELSE 0 END ) as Primary_Insurance , "
                                               + " (CASE WHEN INSP.Primary_Insurance_CompanyName IS NOT NULL THEN  INSP.Primary_Insurance_CompanyName ELSE '' END ) as Primary_Insurance_CompanyName,Prim_Ins_Company_PhoneNumber, "
                                               + " (CASE WHEN INSS.Secondary_Insurance IS NOT NULL THEN INSS.Secondary_Insurance ELSE 0 END ) as Secondary_Insurance, "
                                               + " (CASE WHEN INSS.Secondary_Insurance_CompanyName IS NOT NULL THEN INSS.Secondary_Insurance_CompanyName ELSE '' END ) as Secondary_Insurance_CompanyName,Sec_Ins_Company_PhoneNumber  from "
                                               + " (SELECT [fld_auto_intPatId] as Patient_EHR_ID ,(LTRIM(RTRIM([fld_strFName]))) AS First_name ,(LTRIM(RTRIM([fld_strLName]))) AS Last_name , "
                                              // + " (LTRIM(RTRIM([fld_strMIni]))) AS Middle_Name ,(LTRIM(RTRIM(PT.fld_strTitle))) as Salutation , (LTRIM(RTRIM([fld_strPName]))) as preferred_name ,case when [fld_bytStatus] = 0 then 'I' else 'A' end as [Status] ,case when [fld_bytStatus] = 0 then 'INACTIVE' else 'ACTIVE' end as [EHR_Status],  "
                                              + " (LTRIM(RTRIM([fld_strMIni]))) AS Middle_Name ,(LTRIM(RTRIM(PT.fld_strTitle))) as Salutation , (LTRIM(RTRIM([fld_strPName]))) as preferred_name ,Case when isnull([fld_strSex],'0') = '0' then '' else [fld_strSex] end as Sex,'' as MaritalStatus,case when [fld_bytStatus] = 0 then 'I' else 'A' end as [Status] , CASE WHEN fld_bytStatus = 1 and pp.fld_strType != 'Deceased' THEN 'Active' WHEN fld_bytStatus = 1 and pp.fld_strType = 'Deceased' THEN 'InActive' WHEN fld_bytStatus = 0 THEN 'InActive' END AS EHR_Status,  "
                                               + " [fld_dtmBth] as Birth_Date ,(LTRIM(RTRIM([fld_strEmail])))  AS Email ,case when (LTRIM(RTRIM([fld_strMTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strMTel]))) end AS Mobile "
                                               + ",(fld_intSIN)as ssn,'' as emergencycontactId,fld_strEmergencyContact as emergencycontactnumber , fld_strEmergencyName as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name ,fld_strSchool as school,resp.responsiblepartyId,resp.ResponsibleParty_First_Name,resp.ResponsibleParty_Last_Name,resp.responsiblepartybirthdate,resp.responsiblepartyssn,'' as groupid,'' as spouseId, '' as Spouse_First_Name, '' as Spouse_Last_Name,'' as driverlicense, '' as employer"
                                               + " , case when (LTRIM(RTRIM([fld_strHTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strHTel]))) end AS Home_Phone ,case when (LTRIM(RTRIM([fld_strOTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strOTel]))) end  AS Work_Phone"
                                               + " , (LTRIM(RTRIM([fld_strAddr1]))) AS Address1, (LTRIM(RTRIM([fld_strAddr2]))) AS Address2, (LTRIM(RTRIM(fld_strCity))) AS City, (LTRIM(RTRIM(fld_strCountry))) AS [State]"
                                               + " , (LTRIM(RTRIM([fld_strPCode]))) AS Zipcode, fam.fld_intHeadPatId AS Guar_ID , [fld_shtPrId] AS Pri_Provider_ID, [fld_shtHyId] AS Sec_Provider_ID"
                                               + ", isnull(pfpi.fld_fltPCurr,0) as ThirtyDay , isnull(pfpi.fld_fltP30,0) as SixtyDay, isnull(pfpi.fld_fltP60,0) as NinetyDay, isnull(pfpi.fld_fltP90,0) as Over90"
                                               + ", isnull(pfpi.fld_fltPCurr,0) + isnull(pfpi.fld_fltP30,0) + isnull(pfpi.fld_fltP60,0) + isnull(pfpi.fld_fltP90,0) As CurrentBal, 1 AS InsUptDlt, fld_intPri, fld_intSec,case when isnull(pin.fld_strPrefLang,'') ='' THEN 'English' else  isnull(pin.fld_strPrefLang,'') END AS PreferredLanguage,pin.fld_strcontactnotes AS Patient_Note, 0 as IS_deleted "
                                               + " FROM [tbl_PatInfo] as pin left join Tbl_PatFam Fam WIth(Nolock) ON pin.fld_intFamId = Fam.fld_auto_intfamid "
                                               + " left join [tbl_PatType] as pp on pp.fld_auto_bytPatTypeId = pin.fld_byttypeid  "
                                               + "left join (select pf.fld_intHeadPatId as responsiblepartyId,fld_strFName as ResponsibleParty_First_Name,fld_strLName as ResponsibleParty_Last_Name,fld_dtmBth as responsiblepartybirthdate, fld_intSIN as responsiblepartyssn  from tbl_PatInfo p,tbl_PatFam pf where p.fld_auto_intPatId=pf.fld_intHeadPatId )as resp on resp.responsiblepartyId=pin.fld_auto_intPatId"

                                                //Inner join by yogesh for retrieve data only which have appointments
                                                + Environment.NewLine + " Inner Join (Select ap.[fld_intPatId] From [tbl_SchApp] ap inner join tbl_CfgAppStat as ass on ass.fld_auto_bytAppStatId = ap.fld_bytAppStatId "
                                                + " Where ( ass.fld_blnfree = 0 and [fld_dtmStartTime] > @ToDate)"
                                                + " ) as a On pin.fld_auto_intPatId = a.[fld_intPatId]"
                                                + Environment.NewLine


                                               + " left outer join [tbl_PatAR] as pfpi on pin.fld_auto_intPatId = pfpi.fld_intPatId "
                                               + " left outer JOIN tbl_CfgPatTitle AS PT ON pin.fld_bytTitle = pt.fld_bytId) as p "                                               
                                               + " left JOIN tbl_PatContactPriority as pc ON p.Patient_EHR_ID = pc.fld_intPatId "
                                               + " left join tbl_PatPol ptpol on ptpol.fld_intPatId = p.Patient_EHR_ID and p.fld_intPri = ptpol.fld_auto_intPatPolId "
                                               + " left join tbl_PatPol ptpol2 on ptpol2.fld_intPatId = p.Patient_EHR_ID and p.fld_intSec = ptpol2.fld_auto_intPatPolId "
                                               + " left outer join (SELECT ap.[fld_intPatId] ,min(ap.[fld_dtmStartTime]) as FirstVisit_Date,max(ap.[fld_dtmStartTime]) as LastVisit_Date FROM [tbl_SchApp] ap inner join tbl_CfgAppStat ass on ap.fld_bytAppStatId = ass.fld_auto_bytAppStatId where ap.[fld_dtmStartTime] < getdate() and ap.fld_bytAppStatId = 7 and ass.fld_blnfree = 0 and ap.[fld_intPatId] is not null group by ap.[fld_intPatId]) as dts on p.Patient_EHR_ID = dts.fld_intPatId "
                                               + " left outer join (select ap.[fld_intPatId] , min(ap.[fld_dtmStartTime])  as nextvisit_date FROM [tbl_SchApp] ap inner join tbl_CfgAppStat ass on ap.fld_bytAppStatId = ass.fld_auto_bytAppStatId where ap.[fld_dtmStartTime] > getdate()  and [fld_intPatId] is not null and ass.fld_blnfree = 0 group by ap.[fld_intPatId]) as ndts on p.Patient_EHR_ID = ndts.fld_intPatId"

                                               //+ " left outer join (SELECT [fld_intPatId] ,min ([fld_dtmStartTime]) as FirstVisit_Date,max([fld_dtmStartTime]) as LastVisit_Date FROM [tbl_SchApp] where [fld_dtmStartTime] < getdate() group by [fld_intPatId] ) as dts on p.Patient_EHR_ID = dts.fld_intPatId "
                                               //+ " left outer join (select [fld_intPatId] ,min([fld_dtmStartTime])  as nextvisit_date FROM [tbl_SchApp] where [fld_dtmStartTime] > getdate() group by [fld_intPatId] ) as ndts on p.Patient_EHR_ID = ndts.fld_intPatId "
                                               + " LEFT JOIN ( Select  [fld_intPatId] as Patient_EHR_ID, ([fld_dtmDueDate]) as due_date, (rt.[fld_strRecTypeName]) as recall_type,(r.[fld_bytRecType]) as recall_type_id From "
                                               + "      ( SELECT  MAX(fld_auto_intrecid) AS fld_auto_intrecid, fld_intpatid AS Patient_EHR_Id "
                                               + "      FROM [tbl_Rec] as r inner join [tbl_RecType] as rt on  rt.fld_auto_bytRecTypeId = r.fld_bytRecType GROUP BY [fld_intPatId] ) AS RTC "
                                               + "      INNER JOIN [tbl_Rec] as r ON r.fld_auto_intrecid = RTC.fld_auto_intrecid "
                                               + "      inner join [tbl_RecType] as rt on  rt.fld_auto_bytRecTypeId = r.fld_bytRecType ) AS DUED ON DUED.Patient_EHR_ID =  p.Patient_EHR_ID "
                                               + " LEFT JOIN (SELECT * FROM ( SELECT row_number() OVER ( PARTITION BY[fld_intSubPatId] ORDER BY [fld_intCarrId]) AS ID, "
                                               + "  [fld_intCarrId] as Primary_Insurance ,[fld_strName] as Primary_Insurance_CompanyName,[fld_strTel] as Prim_Ins_Company_PhoneNumber ,[fld_intFamId] ,[fld_intSubPatId] FROM [tbl_PatFamPol]as pfp "
                                               + " inner join [tbl_InsPol] as ip on ip.fld_auto_intInsPolId = pfp.[fld_intInsPolId] "
                                               + " inner join [tbl_InsCarr] as ic on ic.fld_auto_intCarrId = ip.fld_intCarrId ) AS AA WHERE ID = 1 ) AS INSP ON INSP.fld_intSubPatId =  p.Patient_EHR_ID "
                                               + " LEFT JOIN ( SELECT * FROM ( SELECT row_number() OVER ( PARTITION BY[fld_intSubPatId] ORDER BY [fld_intCarrId]) AS ID, "
                                               + "  [fld_intCarrId] as Secondary_Insurance ,[fld_strName] as Secondary_Insurance_CompanyName ,[fld_strTel] as Sec_Ins_Company_PhoneNumber,[fld_intFamId] ,[fld_intSubPatId] FROM [tbl_PatFamPol]as pfp "
                                               + " inner join [tbl_InsPol] as ip on ip.fld_auto_intInsPolId = pfp.[fld_intInsPolId] "
                                               + " inner join [tbl_InsCarr] as ic on ic.fld_auto_intCarrId = ip.fld_intCarrId ) AS AB WHERE ID = 2 ) AS INSS ON INSS.fld_intSubPatId =  p.Patient_EHR_ID "
                                               + " left outer join #TempPay as ppay on ppay.flt_intPatId = p.Patient_EHR_ID"
                                               + Environment.NewLine
                                               + " Drop table #TempPay";

        public static string GetClearDentAppointmentsPatientDataByPatID = "select DISTINCT 0 AS Clinic_Number, 1 as Service_Install_Id, p.*,dts.FirstVisit_Date as FirstVisit_Date,dts.LastVisit_Date as LastVisit_Date, "
                                               + " ndts.nextvisit_date  AS nextvisit_date, "
                                               + " CONVERT(varchar, DUED.due_date) + '@'+DUED.recall_type + '@'+CONVERT(varchar,DUED.recall_type_id) + '|' AS due_date , "
                                               + " 0 as remaining_benefit , case when pc.fld_bytMPriority = 0 then 'N' else 'Y' end as ReceiveVoiceCall,case when pc.fld_bytEmail = 0 then 'N' else 'Y' end as ReceiveEmail,case when pc.fld_blnMSMS = 0 then 'N' else 'Y' end as ReceiveSms , "
                                               + "  isnull(ppay.UsedBeneFit,0) as used_benefit,isnull(ppay.CollectPayment,0) AS collect_payment, "
                                               + " '' as recall_type ,'' as recall_type_id, "
                                               + " ptpol.fld_strSubIdNo as Primary_Ins_Subscriber_ID,ptpol2.fld_strSubIdNo as Secondary_Ins_Subscriber_ID, "
                                               + " (CASE WHEN INSP.Primary_Insurance IS NOT NULL THEN  INSP.Primary_Insurance ELSE 0 END ) as Primary_Insurance , "
                                               + " (CASE WHEN INSP.Primary_Insurance_CompanyName IS NOT NULL THEN  INSP.Primary_Insurance_CompanyName ELSE '' END ) as Primary_Insurance_CompanyName,Prim_Ins_Company_PhoneNumber, "
                                               + " (CASE WHEN INSS.Secondary_Insurance IS NOT NULL THEN INSS.Secondary_Insurance ELSE 0 END ) as Secondary_Insurance, "
                                               + " (CASE WHEN INSS.Secondary_Insurance_CompanyName IS NOT NULL THEN INSS.Secondary_Insurance_CompanyName ELSE '' END ) as Secondary_Insurance_CompanyName,Sec_Ins_Company_PhoneNumber  from "
                                               + " (SELECT [fld_auto_intPatId] as Patient_EHR_ID ,(LTRIM(RTRIM([fld_strFName]))) AS First_name ,(LTRIM(RTRIM([fld_strLName]))) AS Last_name , "
                                              // + " (LTRIM(RTRIM([fld_strMIni]))) AS Middle_Name ,(LTRIM(RTRIM(PT.fld_strTitle))) as Salutation , (LTRIM(RTRIM([fld_strPName]))) as preferred_name ,case when [fld_bytStatus] = 0 then 'I' else 'A' end as [Status] ,case when [fld_bytStatus] = 0 then 'INACTIVE' else 'ACTIVE' end as [EHR_Status],  "
                                              + " (LTRIM(RTRIM([fld_strMIni]))) AS Middle_Name ,(LTRIM(RTRIM(PT.fld_strTitle))) as Salutation , (LTRIM(RTRIM([fld_strPName]))) as preferred_name ,Case when isnull([fld_strSex],'0') = '0' then '' else [fld_strSex] end as Sex,'' as MaritalStatus,case when [fld_bytStatus] = 0 then 'I' else 'A' end as [Status] , CASE WHEN fld_bytStatus = 1 and pp.fld_strType != 'Deceased' THEN 'Active' WHEN fld_bytStatus = 1 and pp.fld_strType = 'Deceased' THEN 'InActive' WHEN fld_bytStatus = 0 THEN 'InActive' END AS EHR_Status,  "
                                               + " [fld_dtmBth] as Birth_Date ,(LTRIM(RTRIM([fld_strEmail])))  AS Email ,case when (LTRIM(RTRIM([fld_strMTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strMTel]))) end AS Mobile "
                                               + ",(fld_intSIN)as ssn,'' as emergencycontactId,fld_strEmergencyContact as emergencycontactnumber , fld_strEmergencyName as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name ,fld_strSchool as school,resp.responsiblepartyId,resp.ResponsibleParty_First_Name,resp.ResponsibleParty_Last_Name,resp.responsiblepartybirthdate,resp.responsiblepartyssn,'' as groupid,'' as spouseId, '' as Spouse_First_Name, '' as Spouse_Last_Name,'' as driverlicense, '' as employer"
                                               + " , case when (LTRIM(RTRIM([fld_strHTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strHTel]))) end AS Home_Phone ,case when (LTRIM(RTRIM([fld_strOTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strOTel]))) end  AS Work_Phone"
                                               + " , (LTRIM(RTRIM([fld_strAddr1]))) AS Address1, (LTRIM(RTRIM([fld_strAddr2]))) AS Address2, (LTRIM(RTRIM(fld_strCity))) AS City, (LTRIM(RTRIM(fld_strCountry))) AS [State]"
                                               + " , (LTRIM(RTRIM([fld_strPCode]))) AS Zipcode, fam.fld_intHeadPatId AS Guar_ID , [fld_shtPrId] AS Pri_Provider_ID, [fld_shtHyId] AS Sec_Provider_ID"
                                               + ", isnull(pfpi.fld_fltPCurr,0) as ThirtyDay , isnull(pfpi.fld_fltP30,0) as SixtyDay, isnull(pfpi.fld_fltP60,0) as NinetyDay, isnull(pfpi.fld_fltP90,0) as Over90"
                                               + ", isnull(pfpi.fld_fltPCurr,0) + isnull(pfpi.fld_fltP30,0) + isnull(pfpi.fld_fltP60,0) + isnull(pfpi.fld_fltP90,0) As CurrentBal, 1 AS InsUptDlt, fld_intPri, fld_intSec,case when isnull(pin.fld_strPrefLang,'') ='' THEN 'English' else  isnull(pin.fld_strPrefLang,'') END AS PreferredLanguage,pin.fld_strcontactnotes AS Patient_Note, 0 as IS_deleted "
                                               + " FROM [tbl_PatInfo] as pin left join Tbl_PatFam Fam WIth(Nolock) ON pin.fld_intFamId = Fam.fld_auto_intfamid "
                                               + " left join [tbl_PatType] as pp on pp.fld_auto_bytPatTypeId = pin.fld_byttypeid  "
                                               + "left join (select pf.fld_intHeadPatId as responsiblepartyId,fld_strFName as ResponsibleParty_First_Name,fld_strLName as ResponsibleParty_Last_Name,fld_dtmBth as responsiblepartybirthdate, fld_intSIN as responsiblepartyssn  from tbl_PatInfo p,tbl_PatFam pf where p.fld_auto_intPatId=pf.fld_intHeadPatId )as resp on resp.responsiblepartyId=pin.fld_auto_intPatId"

                                                //Inner join by yogesh for retrieve data only which have appointments
                                                + Environment.NewLine + " Inner Join (Select ap.[fld_intPatId] From [tbl_SchApp] ap inner join tbl_CfgAppStat as ass on ass.fld_auto_bytAppStatId = ap.fld_bytAppStatId "
                                                + " Where ( ass.fld_blnfree = 0 and [fld_dtmStartTime] > @ToDate and [fld_intPatId] = @Patient_EHR_ID)"
                                                + " ) as a On pin.fld_auto_intPatId = a.[fld_intPatId]"
                                                + Environment.NewLine


                                               + " left outer join [tbl_PatAR] as pfpi on pin.fld_auto_intPatId = pfpi.fld_intPatId "
                                               + " left outer JOIN tbl_CfgPatTitle AS PT ON pin.fld_bytTitle = pt.fld_bytId) as p "                                               
                                               + " left JOIN tbl_PatContactPriority as pc ON p.Patient_EHR_ID = pc.fld_intPatId "
                                               + " left join tbl_PatPol ptpol on ptpol.fld_intPatId = p.Patient_EHR_ID and p.fld_intPri = ptpol.fld_auto_intPatPolId "
                                               + " left join tbl_PatPol ptpol2 on ptpol2.fld_intPatId = p.Patient_EHR_ID and p.fld_intSec = ptpol2.fld_auto_intPatPolId "
                                               + " left outer join (SELECT ap.[fld_intPatId] ,min(ap.[fld_dtmStartTime]) as FirstVisit_Date,max(ap.[fld_dtmStartTime]) as LastVisit_Date FROM [tbl_SchApp] ap inner join tbl_CfgAppStat ass on ap.fld_bytAppStatId = ass.fld_auto_bytAppStatId where ap.[fld_dtmStartTime] < getdate() and ap.fld_bytAppStatId = 7 and ass.fld_blnfree = 0 and ap.[fld_intPatId] is not null group by ap.[fld_intPatId]) as dts on p.Patient_EHR_ID = dts.fld_intPatId "
                                               + " left outer join (select ap.[fld_intPatId] , min(ap.[fld_dtmStartTime])  as nextvisit_date FROM [tbl_SchApp] ap inner join tbl_CfgAppStat ass on ap.fld_bytAppStatId = ass.fld_auto_bytAppStatId where ap.[fld_dtmStartTime] > getdate()  and [fld_intPatId] is not null and ass.fld_blnfree = 0 group by ap.[fld_intPatId]) as ndts on p.Patient_EHR_ID = ndts.fld_intPatId"

                                               //+ " left outer join (SELECT [fld_intPatId] ,min ([fld_dtmStartTime]) as FirstVisit_Date,max([fld_dtmStartTime]) as LastVisit_Date FROM [tbl_SchApp] where [fld_dtmStartTime] < getdate() group by [fld_intPatId] ) as dts on p.Patient_EHR_ID = dts.fld_intPatId "
                                               //+ " left outer join (select [fld_intPatId] ,min([fld_dtmStartTime])  as nextvisit_date FROM [tbl_SchApp] where [fld_dtmStartTime] > getdate() group by [fld_intPatId] ) as ndts on p.Patient_EHR_ID = ndts.fld_intPatId "
                                               + " LEFT JOIN ( Select  [fld_intPatId] as Patient_EHR_ID, ([fld_dtmDueDate]) as due_date, (rt.[fld_strRecTypeName]) as recall_type,(r.[fld_bytRecType]) as recall_type_id From "
                                               + "      ( SELECT  MAX(fld_auto_intrecid) AS fld_auto_intrecid, fld_intpatid AS Patient_EHR_Id "
                                               + "      FROM [tbl_Rec] as r inner join [tbl_RecType] as rt on  rt.fld_auto_bytRecTypeId = r.fld_bytRecType GROUP BY [fld_intPatId] ) AS RTC "
                                               + "      INNER JOIN [tbl_Rec] as r ON r.fld_auto_intrecid = RTC.fld_auto_intrecid "
                                               + "      inner join [tbl_RecType] as rt on  rt.fld_auto_bytRecTypeId = r.fld_bytRecType ) AS DUED ON DUED.Patient_EHR_ID =  p.Patient_EHR_ID "
                                               + " LEFT JOIN (SELECT * FROM ( SELECT row_number() OVER ( PARTITION BY[fld_intSubPatId] ORDER BY [fld_intCarrId]) AS ID, "
                                               + "  [fld_intCarrId] as Primary_Insurance ,[fld_strName] as Primary_Insurance_CompanyName,[fld_strTel] as Prim_Ins_Company_PhoneNumber ,[fld_intFamId] ,[fld_intSubPatId] FROM [tbl_PatFamPol]as pfp "
                                               + " inner join [tbl_InsPol] as ip on ip.fld_auto_intInsPolId = pfp.[fld_intInsPolId] "
                                               + " inner join [tbl_InsCarr] as ic on ic.fld_auto_intCarrId = ip.fld_intCarrId ) AS AA WHERE ID = 1 ) AS INSP ON INSP.fld_intSubPatId =  p.Patient_EHR_ID "
                                               + " LEFT JOIN ( SELECT * FROM ( SELECT row_number() OVER ( PARTITION BY[fld_intSubPatId] ORDER BY [fld_intCarrId]) AS ID, "
                                               + "  [fld_intCarrId] as Secondary_Insurance ,[fld_strName] as Secondary_Insurance_CompanyName ,[fld_strTel] as Sec_Ins_Company_PhoneNumber,[fld_intFamId] ,[fld_intSubPatId] FROM [tbl_PatFamPol]as pfp "
                                               + " inner join [tbl_InsPol] as ip on ip.fld_auto_intInsPolId = pfp.[fld_intInsPolId] "
                                               + " inner join [tbl_InsCarr] as ic on ic.fld_auto_intCarrId = ip.fld_intCarrId ) AS AB WHERE ID = 2 ) AS INSS ON INSS.fld_intSubPatId =  p.Patient_EHR_ID "
                                               + " left outer join (SELECT Sum(PatAmt) as PatAmt , sum(InsAmt) as UsedBeneFit , sum(Total) as CollectPayment, flt_intPatId FROM "
                                               + " ( SELECT   PatAmt, InsAmt, Total, flt_intPatId FROM Vw_Ledger_Billing WHERE flt_intPatId = @Patient_EHR_ID And ([Desciption] NOT LIKE 'Pat. Payment%') UNION "
                                               + "   SELECT PatPmt, InsPmt, Total, flt_intPatId FROM Vw_Ledger_Adjustment WHERE flt_intPatId = @Patient_EHR_ID And ([Description] NOT LIKE 'Pat. Payment%') UNION "
                                               + "   SELECT PatPmt, InsPmt, Total, flt_intPatId FROM Vw_Ledger_Payment WHERE flt_intPatId = @Patient_EHR_ID And ([Description] NOT LIKE 'Pat. Payment%') UNION "
                                               + "   SELECT fld_fltPatPmt, fld_fltInsPmt, Total, fld_intPatId FROM Vw_Ledger_HeldPayment WHERE fld_intPatId = @Patient_EHR_ID And ([Description] NOT LIKE 'Pat. Payment%')) AS A "
                                               + "   group by flt_intPatId) as ppay on ppay.flt_intPatId = p.Patient_EHR_ID";

        public static string GetClerdentPatientIds = "select fld_auto_intPatId as Patient_EHR_Id from tbl_PatInfo";
        
        public static string GetClearDentPatientStatusNew_Existing = "select fld_auto_intPatId as Patient_EHR_Id from tbl_PatInfo where fld_auto_intPatId not in (Select fld_auto_intPatId FROM [tbl_PatInfo] as pin Inner Join (Select [fld_intPatId] From [tbl_SchApp] Where (fld_bytAppStatId = 7 and [fld_dtmStartTime] < getdate())) as a On pin.fld_auto_intPatId = a.[fld_intPatId])";


        public static string GetClearDentPatientdue_date = "SELECT [fld_intPatId] as Patient_EHR_ID,[fld_dtmDueDate] as due_date,rt.[fld_strRecTypeName] as recall_type,r.[fld_bytRecType] as recall_type_id "
                                                       + " FROM [tbl_Rec] as r inner join [tbl_RecType] as rt on  rt.fld_auto_bytRecTypeId = r.fld_bytRecType Order by [fld_dtmDueDate] desc";

        public static string GetClearDentPatientInsuranceData = "SELECT [fld_intCarrId] as Primary_Insurance ,[fld_strName] as Primary_Insurance_CompanyName ,[fld_intFamId] ,[fld_intSubPatId] AS Patient_EHR_ID FROM [tbl_PatFamPol]as pfp "
                                              + "inner join [tbl_InsPol] as ip on ip.fld_auto_intInsPolId = pfp.[fld_intInsPolId] inner join [tbl_InsCarr] as ic on ic.fld_auto_intCarrId = ip.fld_intCarrId ";
        // + where [fld_intSubPatId] = @PatientId ";



        //public static string GetClearDentPatientdue_date = "Select pt.patient_id,rt.recallid AS recall_type_id, (LTRIM(RTRIM(pt.recall_type))) AS recall_type, pt.due_date "
        //                                        + " from admin.v_patient_recall pt JOIN Admin.RecallType rt ON pt.recall_type = rt.name";


        public static string GetClearDentPatient_recall = "call admin.sp_getallpatientrecalls()";

        public static string GetClearDentPatientcollect_payment = "Select patid AS Patient_EHR_ID,Sum(amt) AS collect_payment from  admin.payment_view group by patid ";

        public static string GetClearDentPatient_RecallType = "SELECT [fld_auto_bytRecTypeId] AS RecallType_EHR_ID , (LTRIM(RTRIM([fld_strRecTypeName]))) AS RecallType_Name FROM tbl_RecType";

        public static string GetClearDentPatientNextApptDate = "select fld_dtmStartTime AS nextvisit_date,fld_intPatId  "
                                       + " From tbl_SchApp Where fld_dtmStartTime > @ToDate Order by fld_dtmStartTime desc ";


        #endregion

        #region CreateAppointment

        public static string GetClearDentPatientID_NameData = "SELECT [fld_auto_intPatId] as Patient_EHR_ID , (LTRIM(RTRIM([fld_strFName]))) AS FirstName,(LTRIM(RTRIM([fld_strLName]))) AS LastName, (LTRIM(RTRIM([fld_strFName]))) + ' ' +(LTRIM(RTRIM([fld_strLName]))) AS Patient_Name , "
                                                            + "case when (LTRIM(RTRIM([fld_strMTel])))  = '604' then '' else (LTRIM(RTRIM([fld_strMTel]))) end AS Mobile ,fld_dtmBth as birth_date"
                                                            + ",case when (LTRIM(RTRIM([fld_strHTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strHTel]))) end AS Home_Phone ,case when (LTRIM(RTRIM([fld_strOTel]))) = '604' then '' else (LTRIM(RTRIM([fld_strOTel]))) end  AS Work_Phone "
                                                            + ",[fld_intFamId] AS Guarantor,(LTRIM(RTRIM([fld_strEmail])))  AS Email FROM [tbl_PatInfo] as pin";

        public static string GetClearDentIdelProvider = "SELECT TOP 1 [fld_auto_shtPrId] as provider_id FROM [tbl_Pr]  where fld_auto_shtPrId > 0 ";

        public static string UpdatePatientGuarantorID = " UPDATE tbl_PatInfo SET  fld_intFamId = @famid  WHERE (fld_auto_intPatId = @patid) ";

        public static string InsertPatientGuarantorID = "INSERT INTO  tbl_PatFam(fld_intHeadPatId)VALUES(@fld_intHeadPatId)";

        public static string InsertAppointmentDetails = "INSERT INTO tbl_SchApp(fld_intPatId,  fld_shtPrId, fld_shtChId, fld_shtAppTypeId, fld_bytAppStatId, fld_dtmStartTime,fld_dtmEndTime, fld_strNotes, fld_blnRemCall, fld_dtmRemDate, fld_blnConfirmed, fld_intRecId, fld_intRecAttId, "
                                                   + "fld_shtPr2Id, fld_bytPriority, fld_bitMarkTimeAvailable, fld_bitPreMed, fld_blnCopy)VALUES "
                                                   + "(@patid,@provid,@opid,@appttype,@apptstatus,@StartTime,@EndTime,@fld_strNotes,1,@createdate,0,NULL,NULL,NULL,0,NULL,0,NULL) ";

        public static string InsertAppointmentTreatmentPlan = @"
                                            insert into tbl_TrPlan(fld_dtmDate,fld_intPatId,fld_shtPrId,fld_blnIsNote,fld_blnIsPreAuth)
                                            select @Date,@PatientId,@ProviderId,0,0
                                            select SCOPE_IDENTITY()";

        public static string InsertAppointmentTreatmentPlanProc = @"
                                            
                                            insert into tbl_TrPlanProc(fld_intTransId,fld_dtmServiceDate,fld_bytStatus,fld_shtPrId,fld_strProcCode,fld_shtDisplayLineNo)
                                            select @TreatmentPlanId,@ServiceDate,'P',@ProviderId,@ProcCode,@LineNumber
                                            declare @TreatmentPlanProcId as int = SCOPE_IDENTITY() 

                                            insert into tbl_SchApp_TrPlanProc(fld_intTrPlanProcId,fld_intSchAppId)
                                            select @TreatmentPlanProcId,@AppointmentId";

        public static string GetBookOperatoryAppointmenetWiseDateTime = " Select A.[fld_auto_intAppId] as appointment_id,A.[fld_shtChId] as Operatory_id, A.[fld_dtmStartTime] as StartTime, A.[fld_dtmEndTime] AS EndTime,P.fld_strFName AS FirstName,P.fld_strLName AS LastName,"
                                                                         + " case when (LTRIM(RTRIM(P.[fld_strMTel]))) = '604' then '' else (LTRIM(RTRIM(P.[fld_strMTel]))) end AS Mobile,(LTRIM(RTRIM(P.[fld_strEmail]))) AS Email,PP.[fld_strName] AS ProviderFirstName,'' AS ProviderLastName"
                                                                         + " FROM tbl_SchApp A inner join tbl_CfgAppStat ass on A.fld_bytAppStatId = ass.fld_auto_bytAppStatId LEFT JOIN tbl_PatInfo P ON A.fld_intPatId = P.fld_auto_intPatId LEFT JOIN [tbl_Pr] PP ON PP.fld_auto_shtPrId = A.fld_shtPrId WHERE  ass.fld_blnfree = 0 and A.[fld_dtmStartTime] >= @ToDate ";

        public static string InsertPatientDetails = " INSERT INTO tbl_PatInfo(fld_strLName, fld_strFName, fld_strMIni,fld_shtPrId, fld_strEmail, fld_strMTel)VALUES "
                                                  + "(@lastname,@firstname,@mi,@provid1,@emailaddr,@pager)";

        public static string InsertPatientDetails_With_Birthdate = " INSERT INTO tbl_PatInfo(fld_strLName, fld_strFName, fld_strMIni,fld_shtPrId, fld_strEmail, fld_strMTel,fld_dtmBth)VALUES "
                                                 + "(@lastname,@firstname,@mi,@provid1,@emailaddr,@pager,@Birth_Date)";

        public static string Is_Update_Status_EHR_Appointment_Live_To_EHR = "SELECT fld_bytAppStatId as [status] FROM tbl_SchApp Where fld_auto_intAppId  = @Apptid ";

        public static string Update_Status_EHR_Appointment_Live_To_Local = " UPDATE tbl_SchApp SET [fld_blnConfirmed] = 1 WHERE fld_auto_intAppId = @Apptid ";

        public static string Update_Receive_SMS_Patient_EHR_Live_To_ClearDentEHR = " UPDATE tbl_PatContactPriority SET [fld_blnMSMS] = @receives_sms WHERE fld_intPatId = @patient_id ";

        public static string InsertAppointmentLogToRecAtt = "  INSERT INTO [tbl_RecAtt] ( fld_intRecId,fld_intPatId,fld_dtmContactDateTime,fld_shtMethod,fld_shtCaller,fld_shtResult,fld_strNote,fld_intAppId,fld_intTrPlanId,fld_strTrPlanStatus,fld_fltTrPlanFee,fld_shtTrPlanMainPrId,fld_blnIsDuplicate,fld_intToDoId,fld_intSMSGatewayId,fld_intPatDataExchangeId)"
                                                    + " VALUES (0,@PatientEHRId,@ContactDate,@shtmtd,(select fld_auto_shtUsrId from tbl_UsrInfo where fld_auto_shtUsrId=@User_EHR_Id),2,@Description,@ApptId,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL) ";

        #endregion

        #region ApptType

        public static string GetClearDentApptTypeData = "   SELECT [fld_auto_shtAppTypeId] as ApptType_EHR_ID,[fld_strAppTypeName] as Type_Name,[fld_strAppTypeDesc] as Type_Desc FROM [tbl_SchAppType]";

        #endregion

        #region EHR_VersionNumber

        public static string GetEHRActualVersionCleardent = " IF ( (Select count(1) from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'tbl_CleardentUpdateLog') > 0 "
                                                            + " AND (Select count(1) from INFORMATION_SCHEMA.COLUMNS where COLUMN_NAME = 'fld_strVersion') > 0  ) "
                                                            + " BEGIN "
                                                            + " select fld_strVersion as version from tbl_CleardentUpdateLog where fld_dtmDate  = ( SELECT MAX(fld_dtmDate) FROM tbl_CleardentUpdateLog) "
                                                            + " END ";

        #endregion

        #region ApptStatus

        public static string GetClearDentApptStatusData = " SELECT [fld_auto_bytAppStatId] as ApptStatus_EHR_ID ,(LTRIM(RTRIM([fld_strDesc]))) as ApptStatus_Name, 'normal' AS ApptStatus_Type FROM [tbl_CfgAppStat]";

        #endregion

        #region RecallType

        public static string GetClearDentRecallTypeData = "SELECT [fld_auto_bytRecTypeId] AS RecallType_EHR_ID , (LTRIM(RTRIM([fld_strRecTypeName]))) AS RecallType_Name, "
                                            + " (LTRIM(RTRIM(fld_strRecTypeDesc))) AS RecallType_Descript"
                                            + " FROM tbl_RecType";
        #endregion

        #region Operatory

        //public static string GetClearDentOperatoryData = " SELECT [fld_auto_shtChId] AS Operatory_EHR_ID , (LTRIM(RTRIM([fld_strChName]))) AS Operatory_Name FROM [tbl_SchCh] ";
        //       // Same output same above
        //public static string GetClearDentOperatoryData = "create table #temp ( Operatory_EHR_Id Numeric(18,0), Operatory_name varchar(50), OperatoryOrder int ) "
        //                                                + "    create table #temp1 ( 	 Id numeric(18,0) IDENTITY(1,1), 	 Operatory_EHR_Id Numeric(18,0), Operatory_name varchar(50), OperatoryOrder int )"
        //                                                + "    insert into #temp select fld_auto_shtChId,fld_strChName, so.fld_bytDisplayOrder as OPOrder from tbl_SchCh as sc"
        //                                                + "    left outer join ( select sgl.fld_shtSchChId,sgl.fld_bytDisplayOrder from tbl_SchChGrouplink as sgl "
        //                                                + "    inner join tbl_SchChGroup as sg on sgl.fld_shtSchChGroupId = sg.fld_auto_shtGroupId"
        //                                                + "    where sg.fld_auto_shtGroupId = (select fld_strOptions from ClearDent_Cfg.dbo.tbl_CfgOption WHERE fld_strOptName = 'Scheduler_SchCh_Group')"
        //                                                + "    ) as so on so.fld_shtSchChId = sc.fld_auto_shtChId WHERE SO.fld_bytDisplayOrder is not null"
        //                                                + "    insert into #temp1 select fld_auto_shtChId,fld_strChName, so.fld_bytDisplayOrder as OPOrder from tbl_SchCh as sc"
        //                                                + "    left outer join ( select sgl.fld_shtSchChId,sgl.fld_bytDisplayOrder from tbl_SchChGrouplink as sgl "
        //                                                + "    inner join tbl_SchChGroup as sg on sgl.fld_shtSchChGroupId = sg.fld_auto_shtGroupId"
        //                                                + "    where sg.fld_auto_shtGroupId = (select fld_strOptions from ClearDent_Cfg.dbo.tbl_CfgOption WHERE fld_strOptName = 'Scheduler_SchCh_Group')"
        //                                                + "    ) as so on so.fld_shtSchChId = sc.fld_auto_shtChId WHERE SO.fld_bytDisplayOrder is null  order by fld_strChName"
        //                                                + "    declare @l_sindex int = 1"
        //                                                + "    declare @l_eindex int = (select count(1) from #temp1 ) "
        //                                                + "    WHILE (@l_sindex <= @l_eindex)"
        //                                                + "    BEGIN "
        //                                                + "        INSERT INTO #temp (Operatory_EHR_Id,Operatory_name,OperatoryOrder) values ((select Operatory_EHR_Id FROM #temp1 where Id =@l_sindex ),(select Operatory_name FROM #temp1 where Id =@l_sindex ), (Select MAX(OperatoryOrder) from #temp) + 1 )"
        //                                                + "        set @l_sindex = @l_sindex + 1"
        //                                                + "    END"
        //                                                + "    select * From #temp "
        //                                                + "    IF OBJECT_ID('tempdb..#temp') IS NOT NULL"
        //                                                + "    DROP TABLE #temp"
        //                                                + "    IF OBJECT_ID('tempdb..#temp1') IS NOT NULL DROP TABLE #temp1";

        public static string GetClearDentOperatoryData = " select fld_auto_shtChId as Operatory_EHR_Id,fld_strChName as Operatory_name, "
                                                        + " ROW_NUMBER() OVER(ORDER BY CASE WHEN ISNULL(sgl.fld_bytDisplayOrder,0) = 0 THEN fld_strChName ELSE Convert(varchar(10),Sgl.fld_bytDisplayOrder) end) as OperatoryOrder"
                                                         + " from tbl_SchCh as s "
                                                         + " LEFT JOIN ClearDent_Cfg.dbo.tbl_CfgOption Copt On Copt.fld_strOptName = 'Scheduler_SchCh_Group' "
                                                         + " LEFT join tbl_SchChGroup as sg on Copt.fld_strOptions = sg.fld_auto_shtGroupId "
                                                         + " LEFT JOIn tbl_SchChGrouplink as sgl ON Sgl.fld_shtSchChId = s.fld_auto_shtChId AND sgl.fld_shtSchChGroupId = Copt.fld_strOptions ";


        public static string GetClearDentFolderListData = "SELECT fld_auto_shtFileCatId as FolderList_EHR_ID,fld_strCategory as Folder_Name,fld_blnLock as Is_Deleted FROM tbl_FileCat";

        public static string GetOpenDentalDeletedOperatoryData = " SELECT [fld_auto_shtChId] AS Operatory_EHR_ID , (LTRIM(RTRIM([fld_strChName]))) AS Operatory_Name FROM [tbl_SchCh] where fld_blnActive = 0 ";

        public static string GetClearDentOperatoryHours = " SELECT [fld_auto_intSchPrId],[fld_shtPrId],[fld_strRecurrence],[fld_blnWorking],[fld_strWorkingChairs],[fld_strDescription],[fld_intColorRGB] FROM [ClearDent].[dbo].[tbl_SchPr] where [fld_shtPrId]<>0  and fld_strWorkingChairs<>'' and fld_blnWorking = 1 "; //and fld_strRecurrence  like '%NoEndDate%' ";

        public static string GetOpenDentalDeletedFolderListData = "SELECT fld_auto_shtFileCatId as FolderList_EHR_ID,fld_strCategory as Folder_Name,fld_blnLock as Is_Deleted FROM tbl_FileCat where fld_blnLock = 1";


        public static string GetClearDentOperatoryTimeOffHours = "SELECT [fld_auto_intSchPrId],[fld_shtPrId],[fld_strRecurrence],[fld_blnWorking],[fld_strWorkingChairs],[fld_strDescription],[fld_intColorRGB] FROM [ClearDent].[dbo].[tbl_SchPr] where [fld_shtPrId]<>0 and fld_strWorkingChairs<>'' and fld_blnWorking = 0"; //and fld_strRecurrence  like '%NoEndDate%' ";

        #endregion

        #region Provider

        //public static string GetClearDentProviderData = "SELECT [fld_auto_shtPrId] as Provider_EHR_ID,(LTRIM(RTRIM(fld_strName))) as Provider_Name , [fld_strCode] as Provider_code,sp.[fld_strDescription] as provider_speciality,p.fld_bytstatus AS is_active"
        //+ " FROM [tbl_Pr] as p left outer join [tbl_CfgSpecialtyClassification] as sp on sp.fld_bytSpClsCode = p.fld_bytSpecialtyClass";

        public static string GetClearDentProviderData = "SELECT [fld_auto_shtPrId] as Provider_EHR_ID,(LTRIM(RTRIM(fld_strName))) as Provider_Name , [fld_strCode] as Provider_code,sp.[fld_strDescription] as provider_speciality,p.fld_bytstatus AS is_active"
        + " FROM [tbl_Pr] as p left outer join [tbl_CfgSpecialtyClassification] as sp on sp.fld_bytSpClsCode = p.fld_bytSpecialtyClass"
        + " where p.fld_auto_shtPrId != 0";

        #region ProviderOfficeHours

        public static string GetClearDentProviderOfficeHours = "SELECT        fld_auto_shtPrId, fld_strCode, fld_strName, fld_bytSpecialtyClass, " +
                  "    fld_strAddr, fld_strProv, fld_strGender, fld_dtmBth, fld_strHTel, fld_strMTel,fld_strEmail, fld_shtFSId, " +
                  "    case when fld_dtmMonStart is null then (select fld_dtmMonStart from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmMonStart end as fld_dtmMonStart, " +
                  "    case when fld_dtmTueStart is null then (select fld_dtmTueStart from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmTueStart end as fld_dtmTueStart, " +
                  "    case when fld_dtmWedStart is null then (select fld_dtmWedStart from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmWedStart end as fld_dtmWedStart, " +
                  "    case when fld_dtmThuStart is null then (select fld_dtmThuStart from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmThuStart end as fld_dtmThuStart, " +
                  "    case when fld_dtmFriStart is null then (select fld_dtmFriStart from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmFriStart end as fld_dtmFriStart, " +
                  "    case when fld_dtmSatStart is null then (select fld_dtmSatStart from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmSatStart end as fld_dtmSatStart, " +
                  "    case when fld_dtmSunStart is null then (select fld_dtmSunStart from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmSunStart end as fld_dtmSunStart, " +
                  "    case when fld_dtmMonEnd is null then (select fld_dtmMonEnd from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmMonEnd end as fld_dtmMonEnd, " +
                  "    case when fld_dtmTueEnd is null then (select fld_dtmTueEnd from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmTueEnd end as fld_dtmTueEnd, " +
                  "    case when fld_dtmWedEnd is null then (select fld_dtmWedEnd from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmWedEnd end as fld_dtmWedEnd, " +
                  "    case when fld_dtmThuEnd is null then (select fld_dtmThuEnd from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmThuEnd end as fld_dtmThuEnd, " +
                  "    case when fld_dtmFriEnd is null then (select fld_dtmFriEnd from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmFriEnd end as fld_dtmFriEnd, " +
                  "    case when fld_dtmSatEnd is null then (select fld_dtmSatEnd from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmSatEnd end as fld_dtmSatEnd, " +
                  "    case when fld_dtmSunEnd is null then (select fld_dtmSunEnd from  tbl_Pr WHERE fld_auto_shtPrId = 0) else fld_dtmSunEnd end as fld_dtmSunEnd, " +
                  "    fld_strCDAPrNo, fld_bytStatus, fld_blnIncorporated, fld_bytMemberClass, fld_blnRegCDAnet, fld_intColorRGB, fld_shtBillingPrId, fld_objLogo,  " +
                  "    fld_bytPrTypeId, fld_tsTimeStamp, fld_strCDHAUniqueNo " +
                  "     FROM tbl_Pr where fld_auto_shtPrId != 0  and 1 = 2";

        public static string GetDentrixProviderHoursData = "call admin.sp_getprovschedexceptions(?,?,?)";

        #endregion

        #region ProviderHours

        public static string GetClearDentProviderHours = " SELECT [fld_auto_intSchPrId],[fld_shtPrId],[fld_strRecurrence],[fld_blnWorking],[fld_strWorkingChairs],[fld_strDescription],[fld_intColorRGB] FROM [ClearDent].[dbo].[tbl_SchPr] where [fld_shtPrId]<>0 and fld_blnWorking = 1"; // and fld_strRecurrence  like '%NoEndDate%' ";

        //public static string GetClearDentProviderHours = " SELECT [fld_auto_intSchPrId],[fld_shtPrId],[fld_strRecurrence],[fld_blnWorking],[fld_strWorkingChairs],[fld_strDescription],[fld_intColorRGB] "
        //                                                +" FROM [tbl_SchPr] where fld_shtprid != 0 "
        //                                                +" UNION"
        //                                                +" SELECT pr.[fld_auto_intSchPrId],p.[fld_auto_shtPrId] As fld_shtPrId,pr.[fld_strRecurrence],pr.[fld_blnWorking],pr.[fld_strWorkingChairs],pr.[fld_strDescription],pr.[fld_intColorRGB] "
        //                                                +" FROM [tbl_SchPr] pr "
        //                                                +" join [tbl_Pr] p on p.[fld_auto_shtPrId] != 0 "
        //                                                +" where pr.fld_shtprid = 0 ";

        public static string GetClearDentOfficeNonWorkingHours = " SELECT [fld_auto_intSchPrId],[fld_shtPrId],[fld_strRecurrence],[fld_blnWorking],[fld_strWorkingChairs],[fld_strDescription],[fld_intColorRGB]  FROM [tbl_SchPr] where fld_blnWorking = 0";

        public static string GetClearDentOfficeWorkingHours = " SELECT [fld_auto_intSchPrId],[fld_shtPrId],[fld_strRecurrence],[fld_blnWorking],[fld_strWorkingChairs],[fld_strDescription],[fld_intColorRGB]  FROM [ClearDent].[dbo].[tbl_SchPr] where fld_blnWorking = 1 and fld_shtPrId = 0"; //and fld_strRecurrence  like '%NoEndDate%' ";

        #endregion

        #endregion

        #region OperatoryEvent

        public static string GetClearDentOperatoryEventData = "SELECT [fld_auto_intAppId] as [OE_EHR_ID],[fld_shtChId] as [Operatory_EHR_ID],[fld_dtmStartTime] as [StartTime],[fld_dtmEndTime] as [EndTime] "
                                           + ",[fld_strNotes] as [comment] FROM [tbl_SchApp] where fld_shtAppTypeId is null and [fld_dtmStartTime] > @ToDate ";

        #region DeletedOperatoryEvent

        public static string GetLocalOperatoryEventData = "SELECT * FROM OperatoryEvent WHERE StartTime > ?";

        public static string GetClearDentDeletedOperatoryEventData = "SELECT [fld_intAppId] as [OE_EHR_ID],[fld_shtChId] as [Operatory_EHR_ID],[fld_dtmStartTime] as [StartTime],[fld_dtmEndTime] as [EndTime] "
                                                                   + ",[fld_strNotes] as [comment], [fld_dtmCreateDate] as EHR_Entry_DateTime  FROM [tbl_SchAppDeleted] Where [fld_intPatId] is null and [fld_dtmCreateDate] > @ToDate ";

        #endregion

        #endregion

        #region OperatoryOfficeHours

        public static string GetClearDentOperatoryOfficeHours = "SELECT  fld_auto_shtChId, fld_strChName,fld_shtPrId, fld_blnActive,"
                                                               + " fld_dtmMonStart, fld_dtmTueStart,fld_dtmWedStart,fld_dtmThuStart,fld_dtmFriStart,fld_dtmSatStart,fld_dtmSunStart,fld_dtmMonEnd, fld_dtmTueEnd,fld_dtmWedEnd,fld_dtmThuEnd,fld_dtmFriEnd,fld_dtmSatEnd,fld_dtmSunEnd"
                                                               + " FROM tbl_SchCh ch join tbl_Pr Pr on pr.fld_auto_shtPrId = 0";

        #endregion OperatoryOfficeHours

        #region Patient_Form

        public static string Update_Patinet_Record_By_Patient_Form = "UPDATE tbl_PatInfo SET ColumnName = @ehrfield_value WHERE fld_auto_intPatId = @Patient_EHR_ID ";

        public static string InsertPatientDocData = " Insert Into  [tbl_File] (fld_intPatId,fld_strOFileName,fld_strDBFileName,fld_dtmCreate,fld_shtCreateUser ,fld_dtmLastMod,fld_shtLastModUser,fld_shtFileCatId) "
                                      + " Values(@fld_intPatId, @fld_strOFileName, @fld_strDBFileName, @fld_dtmCreate, @fld_shtCreateUser, @fld_dtmLastMod, @fld_shtLastModUser,@fld_shtFileCatId) ;";

        public static string InsertPatient_InsCov = "INSERT INTO tbl_InsCov (fld_strDescription) VALUES ('P: 0  B:0  M:0  O:0  R:0')";


        public static string InsertPatient_InsPol = "INSERT INTO tbl_InsPol (fld_intCarrId, fld_shtFSInsId, fld_intCovId, fld_bytPayeeCode, fld_blnSignature, fld_blnEDISubmission, "
                                      + " fld_blnRPSameAsSC) VALUES (@fld_intCarrId, (select top 1 [fld_auto_shtFSId] from tbl_FS),@fld_intCovId,4,0,1,1) ";

        public static string InsertPatient_PatFamPol = "INSERT INTO tbl_PatFamPol (fld_intFamId, fld_intInsPolId, fld_intSubPatId) VALUES (@fld_intFamId, @fld_intInsPolId, @fld_intSubPatId)";

        public static string InsertPatient_PatPol = " INSERT INTO tbl_PatPol (fld_intPatId, fld_intFamPolId, fld_blnCovered, fld_bytPayeeCode,  fld_bytSCRPMonth, fld_bytRPMonth,fld_strSubIdNo) "
                                      + " VALUES (@fld_intPatId,@fld_intFamPolId,1,4,12,12,@fld_strSubIdNo ) ";

        public static string Insert_paitent_primaryinsurance_patplan = "UPDATE tbl_PatInfo SET  fld_intPri = @insuredid WHERE (fld_auto_intPatId = @patid) ";


        public static string Insert_paitent_secondaryinsurance_patplan = "UPDATE tbl_PatInfo SET  fld_intSec = @insuredid WHERE (fld_auto_intPatId = @patid) ";

        #endregion

        #region Treatment Document
        public static string InsertPatientTreatmentDocData = " Insert Into  [tbl_File] (fld_intPatId,fld_strOFileName,fld_strDBFileName,fld_dtmCreate,fld_shtCreateUser ,fld_dtmLastMod,fld_shtLastModUser,fld_shtFileCatId) "
                                      + " Values(@fld_intPatId, @fld_strOFileName, @fld_strDBFileName, @fld_dtmCreate, @fld_shtCreateUser, @fld_dtmLastMod, @fld_shtLastModUser,@fld_shtFileCatId) ;";
        #endregion


        #region InsuranceCarrier Document
        public static string InsertPatientInsuranceCarrierDocData = " Insert Into  [tbl_File] (fld_intPatId,fld_strOFileName,fld_strDBFileName,fld_dtmCreate,fld_shtCreateUser ,fld_dtmLastMod,fld_shtLastModUser,fld_shtFileCatId) "
                                      + " Values(@fld_intPatId, @fld_strOFileName, @fld_strDBFileName, @fld_dtmCreate, @fld_shtCreateUser, @fld_dtmLastMod, @fld_shtLastModUser,@fld_shtFileCatId) ;";
        #endregion

        public static string GetCleardentMedicationMaster = "Select fld_auto_intTempId as Medication_EHR_ID, " +
             "fld_StrDrug as Medication_Name, " +
             "'' as Medication_Description, " +
             "fld_strNote as Medication_Notes, " +
             "fld_strSig as Medication_Sig, " +
             "'' as Medication_Parent_EHR_ID, " +
             "'' as Medication_Type, " +
             "fld_blnGenSubPer as Allow_Generic_Sub, " +
             "fld_shtDisp as Drug_Quantity, " +
             "fld_shtRefills as Refills, " +
             "'True' as Is_Active, " +
             "getdate() as EHR_Entry_DateTime, " +
             "'' as Medication_Provider_ID, " +
             "0 as is_deleted, " +
             "0 as Is_Adit_Updated, " +
             "'0' as Clinic_Number from tbl_PatPresTemp Where (fld_StrDrug is not null and fld_StrDrug <> '') ";

        // public static string GetCleardentPatientMedicationMaster = "SELECT 0 As Clinic_Number,1 as Service_Install_Id,pp.fld_intPatId AS Patient_EHR_ID,isnull(ppt.fld_auto_intTempid,0) as Medication_EHR_ID ,fld_auto_intprepid as PatientMedication_EHR_ID,'' AS Medication_Web_ID,pp.fld_auto_shtPrId AS Provider_EHR_ID,pp.fld_strDrug AS Medication_Name,'M' AS Medication_Type,pp.fld_shtDisp AS Drug_Quantity,pp.fld_strNote AS Medication_Note,GETDATE() AS EHR_Entry_DateTime,GETDATE() AS Start_Date,GETDATE() AS End_Date,GETDATE() AS Last_Sync_Date,pp.fld_strSig AS Patient_Notes,pp.fld_strDrug AS MedicalDescription,pp.fld_shtRefills AS Refills,pp.fld_dtmDate AS Date_Entered,getdate() AS Expiry_Date,0 AS Is_deleted,0 AS Is_Adit_Updated from tbl_PatPres PP LEFT join tbl_PatPresTemp ppt ON pp.fld_strDrug = ppt.fld_strDrug";
        public static string GetCleardentPatientMedicationMaster = "SELECT 0 As Clinic_Number,1 as Service_Install_Id,pp.fld_intPatId AS Patient_EHR_ID, "
                                                                 + " isnull(ppt.fld_auto_intTempid,0) as Medication_EHR_ID ,fld_auto_intprepid as PatientMedication_EHR_ID, "
                                                                 + " '' AS Medication_Web_ID, pp.fld_auto_shtPrId AS Provider_EHR_ID,pp.fld_strDrug AS Medication_Name,'M' AS Medication_Type, "
                                                                 + " pp.fld_shtDisp AS Drug_Quantity,pp.fld_strNote AS Medication_Note, "
                                                                 + " GETDATE() AS EHR_Entry_DateTime, GETDATE() AS Start_Date, GETDATE() AS End_Date, pp.fld_strNote as Patient_Notes, GETDATE() AS Last_Sync_Date,PP.fld_strSig as Medication_SIG, "
                                                                 + " 0 AS Is_deleted,0 AS Is_Adit_Updated, 'True' as is_active from tbl_PatPres PP LEFT join tbl_PatPresTemp ppt ON REPLACE(pp.fld_strDrug,',','') = REPLACE(ppt.fld_strDrug,',','') ";

        public static string GetClearDentMedicalFormMaster = "SELECT 0 As Clinic_Number,1 as Service_Install_Id,'' AS  CD_FormMaster_Local_Id,fld_auto_shtMedQTempId AS CD_FormMaster_EHR_ID,'' AS CD_FormMaster_Web_ID,fld_strDescription AS FormName_Name,GETDATE() AS EHR_Entry_DateTime,GETDATE() AS Last_Sync_Date,0 AS Is_deleted,0 AS Is_Adit_Updated FROM dbo.tbl_MedQTemp";

        public static string GetClearDentMedicalQuestionMaster = "SELECT 0 As Clinic_Number,1 as Service_Install_Id,'' AS CD_QuestionMaster_Local_Id,fld_auto_shtMedQId AS CD_QuestionMaster_EHR_ID,'' AS CD_QuestionMaster_Web_ID,fld_shtMedQTempId AS CD_FormMaster_EHR_ID,mqt.fld_strDescription as FormName_Name,fld_strMedQuestion AS Question_Description,fld_shtMedQSeq AS Question_Sequence,isnull(fld_blnWarn,0) AS Question_Warnings,1 AS Question_Type,'' AS Comment,GETDATE() AS EHR_Entry_DateTime,GETDATE() AS Last_Sync_Date,0 AS Is_deleted,0 AS Is_Adit_Updated FROM dbo.tbl_MedQ inner join tbl_MedQTemp as mqt on tbl_MedQ.fld_shtMedQTempId = mqt.fld_auto_shtMedQTempId ";

        public static string GetClearDentMedicleResponseData = "select * from tbl_PatMedQGroup as pm left outer join "
                                                                    + "[dbo].[tbl_PatMedQ] as pmq on pm.fld_auto_intPatMedQGroupId = pmq.fld_intPatMedQGroupId where pm.fld_strTempDesc = @fld_strFormName and pm.fld_intPatId = @fld_intPatId";

        public static string InsertCDPatMedicleResponseData = "INSERT INTO tbl_PatMedQGroup (fld_intPatId, fld_dtmChanged, fld_dtmCreate, fld_strTempDesc, fld_intCreateUsrId, fld_intChangedUsrId) "
                                                            + "VALUES (@fld_intPatId, getdate(), getdate(), @fld_strTempDesc, (SELECT TOP (1) [fld_auto_shtUsrId] FROM [tbl_UsrInfo]), (SELECT TOP (1) [fld_auto_shtUsrId] FROM [tbl_UsrInfo]))";

        public static string InsertCDPatMedicleQuestionResponseData = "INSERT INTO tbl_PatMedQ (fld_intPatMedQGroupId, fld_strMedQ, fld_shtMedQSeq, fld_blnAnswer, fld_blnWarn, fld_strAnswer) "
                                                            + "VALUES (@fld_intPatMedQGroupId, @fld_strMedQ, @fld_shtMedQSeq, @fld_blnAnswer, @fld_blnWarn, @fld_strAnswer)";

        /*-------------------------------------payment query for patient starts--------------------------------*/

       public static string InsertPatientPaymentLog = " IF NOT EXISTS ( SELECT 1 FROM [tbl_RecAtt]  where fld_intPatId = @PatientEHRId AND fld_dtmContactDateTime = @PaymentDate AND fld_shtMethod = @Method AND fld_strNote = @PatientNote ) BEGIN  INSERT INTO [tbl_RecAtt] ( fld_intRecId,fld_intPatId,fld_dtmContactDateTime,fld_shtMethod,fld_shtCaller,fld_shtResult,fld_strNote,fld_intAppId,fld_intTrPlanId,fld_strTrPlanStatus,fld_fltTrPlanFee,fld_shtTrPlanMainPrId,fld_blnIsDuplicate,fld_intToDoId,fld_intSMSGatewayId,fld_intPatDataExchangeId)"
                                                    + " VALUES (0,@PatientEHRId,@PaymentDate,@Method,@User_EHR_Id,6,@PatientNote,NULL,NULL,NULL,NULL,NULL,NULL,0,NULL,NULL); Select @@Identity END ELSE BEGIN SELECT 0 END ";

        //public static string InsertPatientPayment_Tranc = " IF NOT EXISTS ( Select 1 FROM tbl_trans WHERE flt_intPatId = @PatientEHRId AND fld_dtmTransDate = @PaymentDate" +
        //     " AND fld_strDescription =  @Description AND fld_shtServicePrId = @ProviderId AND fld_dtmPostDate=  @PostDate ) BEGIN " +
        //     "Insert into tbl_trans (flt_intPatId, fld_dtmTransDate, fld_strDescription,fld_shtServicePrId, fld_dtmPostDate) values " +
        //     "(@PatientEHRId , @PaymentDate, @Description,@ProviderId, @PostDate); Select @@Identity END ELSE BEGIN SELECT 0 END ";//will need payment date and now date as post date

        public static string checktransentryexist = "Select fld_auto_intTransId FROM tbl_trans WHERE flt_intPatId = @PatientEHRId AND fld_dtmTransDate = @PaymentDate" +
             " AND fld_strDescription =  @Description AND fld_shtServicePrId = @ProviderId AND fld_dtmPostDate=  @PostDate";

        public static string InsertPatientPayment_Tranc = "  Insert into tbl_trans (flt_intPatId, fld_dtmTransDate, fld_strDescription,fld_shtServicePrId, fld_dtmPostDate) values " +
            "(@PatientEHRId , @PaymentDate, @Description,@ProviderId, @PostDate);Select @@Identity";//will need payment date and now date as post date

        public static string InsertPatientPayment_TransProc = "Insert into tbl_TransProc (fld_intTransId,fld_strProcCode,fld_dtmServiceDate,fld_bytStatus,fld_shtPrId,fld_fltDrFee,fld_dtmPostDate,fld_dtmProcessDate)  "
                                                            + " values ( @TransId ,'CREDT' , @CurrentDate,'C', @ProviderId, 0.00, @PostDate ,  @ProcessDate);Select @@Identity";
        //will need  transid , currentdate,paymentdate ,payment date and providerid

       //public static string InsertPatientPayment_Payment = " IF NOT EXISTS ( SELECT fld_auto_intPayID FROM tbl_Payment WHERE CONVERT(date, fld_dtmDate) = CONVERT(date, @PaymentDate) AND fld_strDesc = @Description " +
       //     " AND fld_intPatId = @PatientEHRId AND " +
       //     " CONVERT(date, fld_dtmPostDate) = CONVERT(date, @PostDate) AND (fld_bytPmtType = @PaymentMode or @PaymentMode is null) AND fld_blnIsAdj = @fldblnisadj  AND (fld_bytAdjType = @fldbytAdjType or @fldbytAdjType is null) ) BEGIN " +
       //     "Insert into tbl_Payment (fld_dtmDate,fld_strDesc,fld_intPatId,fld_dtmPostDate,fld_bytPmtType,fld_blnIsAdj,fld_bytAdjType) "
       //                                                    + " Values( @PaymentDate,@Description, @PatientEHRId,  @PostDate, "
                                                           //+ " @PaymentMode,@fldblnisadj,@fldbytAdjType) ;Select @@Identity END ELSE BEGIN SELECT 0 END ";
       // public static string checkpaymententryexist = "SELECT fld_auto_intPayID FROM tbl_Payment WHERE CONVERT(date, fld_dtmDate) = CONVERT(date, @PaymentDate) AND fld_strDesc = @Description AND fld_intPatId =@PatientEHRId AND CONVERT(date, fld_dtmPostDate) = CONVERT(date, @PostDate) AND (fld_bytPmtType = @PaymentMode or @PaymentMode is null) AND fld_blnIsAdj = @fldblnisadj  AND (fld_bytAdjType = @fldbytAdjType or @fldbytAdjType is null)";
        public static string checkpaymententryexist = "SELECT fld_auto_intPayID FROM tbl_Payment WHERE fld_dtmDate = @PaymentDate AND fld_strDesc = @Description AND fld_intPatId =@PatientEHRId AND CONVERT(date, fld_dtmPostDate) = CONVERT(date, @PostDate) AND (fld_bytPmtType = @PaymentMode or @PaymentMode is null) AND fld_blnIsAdj = @fldblnisadj  AND (fld_bytAdjType = @fldbytAdjType or @fldbytAdjType is null)";
        public static string InsertPatientPayment_Payment = "Insert into tbl_Payment (fld_dtmDate,fld_strDesc,fld_intPatId,fld_dtmPostDate,fld_bytPmtType,fld_blnIsAdj,fld_bytAdjType) Values( @PaymentDate,@Description, @PatientEHRId,  @PostDate, @PaymentMode,@fldblnisadj,@fldbytAdjType) SELECT @@IDENTITY";
        //will need paymentdate,patientehrid,postdate,patientpaymentwebid,paymentmode 

        public static string InsertPatientPayment_PaySmt = " Insert into tbl_PmtSpt (fld_intPayId,fld_intTransProcId,fld_strDesc,fld_fltPatPmt,fld_dtmPostDate,fld_dtmProcessDate)  "
                                                           + " values  (@PaymentId, @TransProcId, @Description, @Amount , @PostDate, @ProcessDate);";
        //paymentid,transprocid,amount,postdate,processdate

        public static string GetOutStandingPatientBalance = "Select tp.fld_auto_intTransProcId as Transaction_PK_Id, tp.fld_dtmServiceDate as Procedure_Date,tr.flt_intPatId as Patient_Id,tp.fld_shtPrId as Provider_Id,"
                                                        + " tp.fld_strProcCode as ProcedureCode, ISNULL(tp.fld_fltDrFee-(-1*isnull(ps.fld_fltPatPmt,0)),0) as Outstanding_Amount,Pr.fld_strName AS Provider_Name"
                                                        + " from tbl_Trans tr"
                                                        + " INNER JOIN tbl_TransProc tp on tp.fld_intTransId = tr.fld_auto_intTransId"
                                                        + " LEFT JOIN (select fld_intTransProcId, sum(fld_fltPatPmt) fld_fltPatPmt from tbl_PmtSpt PSplit group by fld_intTransProcId"
                                                        + " ) ps on ps.fld_intTransProcId = tp.fld_auto_intTransProcId"
                                                        + " LEFT JOIN tbl_Pr Pr ON tp.fld_shtPrId = Pr.fld_auto_shtPrId "
                                                        + " WHERE tp.fld_fltDrFee > (-1*isnull(ps.fld_fltPatPmt,0)) AND tr.flt_intPatId  = @Patient_Id"
                                                        + " AND (@Provider_Id = '' OR tp.fld_shtPrId = @Provider_Id)"
                                                        + " AND (@procCode = '' OR tp.fld_strProcCode = @procCode)"
                                                        + " Order by tp.fld_dtmServiceDate Asc";

        public static string InsertLogTo_TransAudTr = "IF NOT EXISTS ( select 1 from tbl_TransAudTr where CONVERT(date, fld_dtmAudDate) = CONVERT(date, @AudDate) AND fld_strDescription = @Description " +
             "AND  fld_intPatId = @PatientEHRId AND fld_fltAmount = @Amount AND CONVERT(date, fld_dtmTransDate) = CONVERT(date, @TransDate) AND fld_intPayId = @PaymentId ) BEGIN " +
             "Insert into tbl_TransAudTr (fld_dtmAudDate,fld_strUserName,fld_strDescription,fld_intPatId,fld_fltAmount,fld_dtmTransDate,fld_intPayId,fld_strPatName)"
            + "values ( @AudDate,(select fld_strLogin from tbl_UsrInfo where fld_auto_shtUsrId=@UserName),@Description ,@PatientEHRId, @Amount,@TransDate,@PaymentId ,(select concat(fld_strLName , ',',' ', fld_strFName, " +
             "' ', '(', fld_auto_intPatId,')')as fld_strPatName from tbl_PatInfo WHERE fld_auto_intPatId=@PatientEHRId)) END ELSE BEGIN SELECT 0 END ";
        //input parameter patientehrid,amount,paymentdate,paymentid

        //public static string InsertLogTo_PatInfoAccessRecord = "IF ( (Select count(1) from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'tbl_PatInfoAccessRecord') > 0 ) "
        //     + "BEGIN IF NOT EXISTS ( SELECT 1 FROM tbl_PatInfoAccessRecord WHERE CONVERT(date, fld_dtmAccessDate) = CONVERT(date, @PaymentDate) AND fld_strDescription = @Description AND fld_intPatId = @PatientEHRId  ) BEGIN " +
        //     "Insert into tbl_PatInfoAccessRecord (fld_dtmAccessDate ,fld_strUserName,fld_strDescription,fld_intPatId,fld_strPatName) "
        //     + "  values( @PaymentDate,(select fld_strLogin from tbl_UsrInfo where fld_auto_shtUsrId=@UserName),@Description, @PatientEHRId, (select concat(fld_strLName , ',',' ', fld_strFName)as fld_strPatName from tbl_PatInfo WHERE fld_auto_intPatId = @PatientEHRId)) END ELSE BEGIN SELECT 0 END END ";//will need patientid and payment date as parameter
        /*-------------------------------------payment query for patient ends --------------------------------*/
        /*-------------------------------------payment query for patient ends --------------------------------*/

        public static string InsertLogTo_PatInfoAccessRecord = "IF ( (Select count(1) from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'tbl_PatInfoAccessRecord') > 0 ) "
            + "BEGIN IF NOT EXISTS ( SELECT 1 FROM tbl_PatInfoAccessRecord WHERE CONVERT(date, fld_dtmAccessDate) = CONVERT(date, @PaymentDate) AND fld_intPatId = @PatientEHRId  ) BEGIN " +
            "Insert into tbl_PatInfoAccessRecord (fld_dtmAccessDate ,fld_strUserName,fld_intPatId,fld_strPatName) "
            + "  values( @PaymentDate,(select fld_strLogin from tbl_UsrInfo where fld_auto_shtUsrId=@UserName), @PatientEHRId, (select concat(fld_strLName , ',',' ', fld_strFName)as fld_strPatName from tbl_PatInfo WHERE fld_auto_intPatId = @PatientEHRId)) END ELSE BEGIN SELECT 0 END END ";//will need patientid and payment date as parameter

        #region User  
        public static string GetEHRCleardentUserId = "IF NOT EXISTS ( select fld_auto_shtUsrId from tbl_UsrInfo where fld_strLogin = 'Adit' ) BEGIN insert into tbl_UsrInfo (fld_strLogin,fld_strPwd,fld_dtmCreateDate,fld_strNote,fld_stFName,fld_strLName,fld_blnStatus,fld_blnDeleted) values('Adit',convert(binary(16),'0x27121593910CB55BC2D9CD0D7CD5B602'),GETDATE(),'User for Adit Portal','Adit','Application',1,0); select fld_strLogin from tbl_UsrInfo where fld_strLogin='Adit';  END ELSE BEGIN  select fld_auto_shtUsrId from tbl_UsrInfo where fld_strLogin='Adit'; END ";

        public static string GetClearDentUserData = "select fld_auto_shtUsrId as User_EHR_ID,'' AS User_LocalDB_ID,'' as User_web_Id,fld_stFName as First_Name,fld_strLName as Last_Name,fld_strPwd as Password ,fld_dtmCreateDate as EHR_Entry_DateTime,"
                                                   + "fld_dtmCreateDate as Last_Updated_DateTime,'' as LocalDb_EntryDatetime,fld_blnStatus as Is_active,fld_blnDeleted as is_deleted,0 as Is_Adit_Updated,0 as Clinic_Number,1 as Service_Install_Id from tbl_UsrInfo";
        #endregion
        public static string InsertSMSCallLog = " If Not EXISTS (Select fld_intPatId From [tbl_RecAtt] Where fld_intPatId = @PatientEHRId "
                                                 + "    and fld_dtmContactDateTime = @PaymentDate and fld_shtMethod = @Method And fld_shtCaller = @EHR_User_Id"
                                                 + "     And fld_shtResult = 6 And fld_strNote = @PatientNote) "
                                                 + "     BEGIN"
                                                 + "      INSERT INTO[tbl_RecAtt] (fld_intRecId, fld_intPatId, fld_dtmContactDateTime, fld_shtMethod, fld_shtCaller,"
                                                 + "     fld_shtResult, fld_strNote, fld_intAppId, fld_intTrPlanId, fld_strTrPlanStatus, fld_fltTrPlanFee, fld_shtTrPlanMainPrId,"
                                                 + "     fld_blnIsDuplicate, fld_intToDoId, fld_intSMSGatewayId, fld_intPatDataExchangeId)"
                                                 + "     VALUES(0, @PatientEHRId, @PaymentDate, @Method, @EHR_User_Id, 6, "
                                                 + "      @PatientNote, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL) Select @@Identity"
                                                 + "     END"
                                                 + "     ELSE"
                                                 + "     BEGIN"
                                                 + "      (Select TOp 1 fld_auto_intRecAttId From [tbl_RecAtt] Where fld_intPatId = @PatientEHRId"
                                                 + "      and fld_dtmContactDateTime = @PaymentDate and fld_shtMethod = @Method And fld_shtCaller = @EHR_User_Id And fld_shtResult = 6"
                                                 + "      And fld_strNote = @PatientNote) END";

        public static string GetClearDentPatientImagesData = "SELECT 0 as Patient_Images_LocalDB_ID,'' as Patient_Images_Web_ID,fld_auto_intimgid as Patient_Images_EHR_ID,fld_intpatid as Patient_EHR_ID,'' as Patient_Web_ID, '' as Image_EHR_Name ,fld_strimgfilename as Patient_Images_FilePath,0 as Is_Deleted,0 as Is_Adit_Updated,getdate() as Entry_DateTime,getdate() as AditApp_Entry_DateTime, 0 as Clinic_Number,0 as Service_Install_Id  FROM [tbl_DigImgImages] where fld_blnIsPatPic = 1";

        public static string GetDuplicateRecords = " Select AA.*, case when (LTRIM(RTRIM(PT.[fld_strMTel]))) = '604' then '' else (LTRIM(RTRIM(PT.[fld_strMTel]))) end AS Mobile FROM ("
            + " select fld_intPatId, fld_dtmContactDateTime, fld_shtMethod, fld_shtCaller, fld_shtResult, fld_strNote, MAX(fld_auto_intRecAttId) AS LogId, count(1) As Cnt"
            + " From tbl_RecAtt  Where fld_dtmContactDateTime > '20211115'"
            + " group by fld_intPatId, fld_dtmContactDateTime, fld_shtMethod, fld_shtCaller, fld_shtResult, fld_strNote having count(1) > 1"
            + " UNION"
            + " select fld_intPatId, fld_dtmContactDateTime, fld_shtMethod, fld_shtCaller, fld_shtResult, fld_strNote, MAX(fld_auto_intRecAttId) AS LogId, count(1) As Cnt"
            + " From tbl_RecAtt As R Inner Join tbl_PatInfo As P On R.fld_intPatId = p.fld_auto_intPatId Where  fld_dtmContactDateTime > '20211115' and case when (LTRIM(RTRIM(P.[fld_strMTel]))) = '604' then '' else (LTRIM(RTRIM(IsNull(P.[fld_strMTel],'')))) end = ''"
            + " group by fld_intPatId, fld_dtmContactDateTime, fld_shtMethod, fld_shtCaller, fld_shtResult, fld_strNote having count(1) > 1"
            + " ) As AA inner join tbl_PatInfo PT on PT.fld_auto_intPatId = AA.fld_intPatId Order by AA.cnt desc";

        public static string DeleteDuplicateLogsWithDate = " Delete From [tbl_RecAtt] Where fld_intPatId = @PatientEHRId and fld_dtmContactDateTime = @LogDate and fld_shtMethod = @Method And fld_shtCaller = @EHR_User_Id And fld_shtResult = 6 And fld_strNote = @Note AND fld_dtmContactDateTime > '20211115' AND fld_auto_intRecAttId != @LogId";

        public static string DeleteDuplicateLogsBlankMobileWithDate = " Delete From [tbl_RecAtt] Where fld_intPatId = @PatientEHRId and fld_dtmContactDateTime = @LogDate and fld_shtMethod = @Method And fld_shtCaller = @EHR_User_Id And fld_shtResult = 6 And fld_strNote = @Note AND fld_dtmContactDateTime > '20211115' ";

        public static string InsertProcedureLog = "INSERT INTO tbl_SchApp_TrPlanProc (fld_intTrPlanProcId, fld_intSchAppId) "
                                                 + "VALUES(@fld_intTrPlanProcId, @AptID);";

        public static string GetTreatmentPlanProcedureLog = " SELECT fld_auto_intTransProcId FROM tbl_TrPlanProc WHERE fld_inttransId = @TreatmentPlanId AND fld_strProcCode = @ProcCode";

        public static string InsertProcedureForNewPlan = "INSERT INTO tbl_TrPlanProc(fld_intTransId,fld_dtmServiceDate,fld_bytStatus,fld_shtPrId,fld_strProcCode,fld_fltDrFee,fld_fltTotalChg,"
                                                         + "fld_fltInsChg,fld_fltPatChg,fld_strProcTypeCode,fld_shtDisplayLineNo,fld_fltPriInsChg,fld_fltSecInsChg,fld_fltPriInsCov,fld_fltSecInsCov)"
                                                         + "VALUES"
                                                         + "(@fld_intTransId"
                                                         + ",SYSDATETIME(),'P',@ProvID,@ProcCode,"
                                          + "(SELECT fld_fltFee FROM tbl_FSFee WHERE fld_shtFSId = (SELECT fld_shtFSId FROM tbl_Pr WHERE fld_auto_shtPrId = @ProvID) AND fld_strProcCode = @ProcCode),"
                                          + "(SELECT fld_fltFee FROM tbl_FSFee WHERE fld_shtFSId = (SELECT fld_shtFSId FROM tbl_Pr WHERE fld_auto_shtPrId = @ProvID) AND fld_strProcCode = @ProcCode),"
                                                         + "0.00,"
                                          + "(SELECT fld_fltFee FROM tbl_FSFee WHERE fld_shtFSId = (SELECT fld_shtFSId FROM tbl_Pr WHERE fld_auto_shtPrId = @ProvID) AND fld_strProcCode = @ProcCode),"
                                                         + "'X',0,0.00,0.00,0.00,0.00)";


        public static string InsertTreatmentPlan = "INSERT INTO tbl_TrPlan(fld_dtmDate,fld_intPatId,fld_strDesc,fld_shtPrId,fld_blnIsNote,fld_blnIsPreAuth)"
                                                  + "VALUES((SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))),@PatID,'',@ProvID,0,0)";

        public static string GetTreatmentPlanKey = "SELECT MAX(fld_auto_intTrPlanId) FROM tbl_TrPlan WHERE fld_intPatId  = @PatID AND fld_shtPrId = @ProvID";

        public static string GetMedication = "Select IsNull((Select fld_auto_intTempId from tbl_PatPresTemp where fld_strDrug = @MedicationName),0) as MedicationID";

        public static string InsertMedication = "Insert into tbl_PatPresTemp(fld_strDrug, fld_strNote, fld_shtDisp, fld_shtRefills, fld_strSig) Values(@MedicationName,@MedicationNote,'','0','');Select @@Identity;";

        public static string GetPatientMedication = "Select IsNull((Select fld_auto_intPrepId from tbl_PatPres where fld_intPatID = @Patient_EHR_ID and fld_strDrug = @MedicationName),0) as PatientMedicationID;";

        public static string InsertPatientMedication = "Insert into tbl_PatPres(fld_intPatID, fld_auto_shtPrId, fld_strDrug, fld_strNote, fld_shtDisp, fld_shtRefills, fld_blnGenSubPer, fld_strSig, fld_strPatIdNum, fld_dtmDate) Values(@Patient_EHR_ID, (Select isNull((Select [fld_shtPrId] from tbl_PatInfo where fld_auto_intPatId = @Patient_EHR_ID),(Select Top 1  fld_auto_shtPrId from [tbl_Pr] where fld_auto_shtPrId > 0 Order By fld_auto_shtPrId))), @MedicationName, @MedicationNote, '', '0', '0', @Medication_SIG, '', getdate());Select @@Identity;";

        public static string UpdatePatientMedicationNotes = "Update tbl_PatPres set fld_strNote = @MedicationNote, " +
                                                            "fld_strDrug = @MedicationName, " +
                                                            "fld_strSig = @Medication_SIG " +
                                                            //" , fld_auto_shtPrId =(Select isNull((Select[fld_shtPrId] from tbl_PatInfo where fld_auto_intPatId = @Patient_EHR_ID),(Select Top 1  fld_auto_shtPrId from[tbl_Pr] where fld_auto_shtPrId > 0 Order By fld_auto_shtPrId))) " +
                                                            "where fld_auto_intPrepId = @PatientMedication_EHR_ID";

        public static string DeletePatientMedication = "Delete From tbl_PatPres where fld_auto_intPrepId = @PatientMedication_EHR_ID and fld_intPatId = @Patient_EHR_ID;";


        //rooja 
        public static string GetCleardentInsuranceData = "SELECT fld_auto_intCarrId,fld_intEDICarrIdNo,fld_intNetId,fld_strName,fld_strCode,fld_strAddr,fld_strTel,fld_strFax,fld_strNotes,fld_intSecCarrTransCnt,fld_bytEncryMeth,fld_dtmReconcilDate,fld_strCDAVer,fld_bytMethod ,fld_bytClmForm ,fld_blnAllowEDI ,fld_strTransPrefix ,fld_shtFSId,fld_blnDefaultFS,fld_blnLabSameLine,fld_blnCombineElg,fld_bytDedMethod,fld_blnPrintUint,'' as Clinic_Number from tbl_InsCarr; ";
    }
}




