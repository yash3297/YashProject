using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.QRY
{
    public class SynchAbelDentQRY
    {

        #region AbelDent Version
        public static string GetEHRActualVersionAbelDent = "SELECT CONCAT(Major_Version , '.' , Minor_Version,'.',Service_Release,'.',Build_Version) as Varsion from USysDbStamp WITH(NOLOCK)";        
        #endregion
        #region Provider
        public static string GetAbelDentProviderData = "Select '' as Provider_LocalDB_ID,(LTRIM(RTRIM(isnull(did,'')))) as Provider_EHR_ID,dname as First_Name,'' AS Last_Name,'' AS MI,'' AS gender,dtitle as Provider_Speciality,(case when dinactive = 0 then 1 else 0 end ) as is_active, 0 as Is_Adit_Updated from dnt WITH(NOLOCK) order by Provider_EHR_ID asc"; //(case when dinactive = 'True' then 1 else 0 end ) as is_active

        public static string GetProviderCustomHours = " SELECT tm.LoggedinUserId AS PH_EHR_ID, tm.Providers as Provider_EHR_ID, tm.SearchDate,CONCAT(tm.SearchDate,' ',tm.SearchTime) AS StartTime ,CONCAT(tm.FromDate,' ',tm.ToDate)AS EndTime, "
    + " '' AS Operatory_EHR_ID, tm.WorkToDo AS comment, CONCAT(tm.FromDueDate,' ', tm.ToDueDate) AS Entry_DateTime, 0 as Clinic_Number"
            + " FROM TreatmentManagerSearchHistory tm WITH(NOLOCK) WHERE tm.SearchDate> @ToDate AND tm.SearchTime<> '00:00:00'";
        public static string GetAbelDentDefaultProviderData = " Select TOP 1 did FROM dnt WITH(NOLOCK) WHERE dinactive = 1";

        #endregion
        #region CustomHours
        public static string GetActiveColumns = "Select schpvdrs from sys where snum = 'SCHPG'";
        public static string GetSlotOPColumns = "SELECT LTRIM(RTRIM(SUBSTRING(CAST(l.snum as varchar(10)), 4, 10))) as schpvdrs from sys  as l WITH(NOLOCK) where l.snum like 'COL%' and l.scurrmess != '' order by l.snum asc ";
        public static string GetProviderName = "Select scurrmess from sys where s30daymess = '@ProviderID';";
        public static string AppMapedData = "Select adate as StartDate,atime as StartTime,achair as Operatory_Id,adid as Provider_Id,atimereq as TimeReq from apt where achair in ('@ColumnID') and adate >= getdate() and adate <= DATEADD(m, 6, GetDate()) order by adate";
        public static string SystemData = "Select sunitmins as unitParday,sdaybegins as startDate,sdayend as endDate,sworkdays as workingDay from sys where snum = ' ';";
        public static string ColumnProviderData = "Select substring(CAST(snum as varchar(10)),4,10) as Operatory_EHR_ID, scurrmess AS Provider_Name, s30daymess as Provider_EHR_ID from sys where snum like 'COL%' and scurrmess != '' ";
        #endregion
        #region PatienFormInsurance

        public static string InsertPatient_InsurancePlan = "INSERT INTO Ins "
                                                     + " (CarrierId, AnniversaryDate,AllowsAssignment, BasicPercentage, MajorPercentage, OrthoPercentage, PreventativePercentage, SingleBasicDeductible, SingleMajorDeductible, SingleOrthoDeductible,  "
                                                     + " SinglePreventativeDeductible, SingleBasicMaximum, SingleMajorMaximum, SingleOrthoMaximum, SinglePreventativeMaximum, FamilyBasicDeductible, FamilyMajorDeductible,  "
                                                     + " FamilyOrthoDeductible, FamilyPreventativeDeductible, FamilyBasicMaximum, FamilyMajorMaximum, FamilyOrthoMaximum, FamilyPreventativeMaximum, PlanSingleDeductible,  "
                                                     + " PlanFamilyDeductible, PlanComboMaximum, PlanMaximum,  Interval, RootPlaningInterval, ScalingInterval, AnniversaryYears, ModifiedTimeStamp, CreatedTimeStamp, ModifiedUserId,  "
                                                     + " CreatedUserId,ModifiedMachineName,CreatedMachineName) "
                                                     + " VALUES  "
                                                     + " (@CarrierId,'2020-01-01',1,100,100,100,100,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,12,12,1,getdate(),getdate(), "
                                                     + " (select top 1 userid from [User] WITH(NOLOCK)),(select top 1 userid from [User] WITH(NOLOCK)),@ModifiedMachineName,@CreatedMachineName)";

        #endregion   
        #region AppointmentData
        //Appt type sub type id Data store in // select * from NoteDisplayOptions
        public static string GetAbelDentAppointmentData = "     SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Appt_LocalDB_ID,AP.aidentifier AS Appt_EHR_ID,'' AS Appt_Web_ID, CT.plname AS Last_Name,CT.pfname AS First_Name,CT.pinitial AS MI,ISNULL(CT.pphone,'') AS Home_Contact,  ISNULL(II.infmobile,'') AS Mobile_Contact,II.infemail AS Email, SUBSTRING(( ISNULL(CT.pstreetadr,'' ) + ' ' + ISNULL(CT.pstreetadr2,'' ) ),0,200) AS [Address], CA.cdesc AS City,'' AS [ST],ISNULL(CT.pcitycode,'') AS Zip, "
                                                            + " AP.achair AS Operatory_EHR_ID,SS.scurrmess AS Operatory_Name ,AP.adid AS Provider_EHR_ID,PD.dname AS Provider_Name,AP.apwork + ' ' +  AP.apwrk2 AS Comment,(CASE When CT.pbirth IS NULL then CONVERT(datetime,'17530101',112) else CT.pbirth end) AS Birth_Date, "
                                                            + " '' AS ApptType_EHR_ID, "
                                                            + " '' AS ApptType , "
                                                            + " AP.apwork AS WorkToDo ,"
                                                            + " CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS Appt_DateTime, (CASE WHEN Ap.aconfstat is null then '' WHEN apsid = '' THEN 'bl' WHEN apsid = '\' then 'ss' WHEN apsid = '\\' then 'ds' ELSE apsid END) as appointment_status_ehr_key, (CASE when Ap.aconfstat is null then '' else APS.apsdesc end) as Appointment_status,"
                                                            + " DATEADD(mi,CONVERT(INT,ap.atimereq * @_minutesInUnit),CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108))) AS Appt_EndDateTime, "
                                                            + " AP.astatus AS[Status],'new' AS Patient_Status,'EHR' as Is_Appt, (CASE WHEN apsdesc = 'Confirmed' THEN 'Y' WHEN apsdesc = 'Preconfirmed' then 'P' ELSE '' END) as confirmed_status_ehr_key,(CASE WHEN apsid = 'Y' THEN 'Confirmed' WHEN apsid = 'P' THEN 'Preconfirmed' ELSE '' END) as confirmed_status, "
                                                            + " GETDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated, AP.apid AS patient_ehr_id,'False' AS is_deleted,0 AS is_Status_Updated_From_Web, getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE  WHEN ASAP.IsASAP = 0 THEN 'False' WHEN ASAP.IsASAP IS NULL THEN 'False' ELSE 'True' END as is_asap  "
                                                            + " FROM apt AP WITH(NOLOCK)"
                                                            + " LEFT JOIN pat CT WITH(NOLOCK) ON CT.pid = AP.apid "
                                                            + " LEFT JOIN cty CA WITH(NOLOCK) on CA.ccode = CT.pcitycode "
                                                            + " LEFT JOIN inf II WITH(NOLOCK) on II.infpid = CT.pid"
                                                            + " LEFT JOIN dnt PD WITH(NOLOCK) ON PD.did = AP.adid"
                                                            + " LEFT JOIN aps APS WITH(NOLOCK) on APS.apsid = AP.aconfstat"
                                                            + " LEFT JOIN sys SS WITH(NOLOCK) ON substring(CAST(SS.snum as varchar(10)),4,10) = AP.achair  "
                                                            + " LEFT JOIN(SELECT AP.aidentifier, AP.apid as PatientID, CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS APt_DateTime, "
                                                            + " (CASE WHEN SR.Inactive = 0 and(AP.adate BETWEEN SR.StartDate AND SR.EndDate) THEN 1 "
                                                            + "    WHEN SR.Inactive = 0 AND(AP.adate > SR.StartDate AND AP.adate > SR.EndDate) THEN 1 "
                                                            + "      ELSE 0 "
                                                            + "  END ) AS IsASAP "
                                                            + " FROM apt AP "
                                                            + " INNER JOIN SchedulingRequest SR on sR.PID = AP.apid AND SR.Inactive = 0 "
															+ " where "
                                                            + " (CASE WHEN SR.Inactive = 0 and (AP.adate BETWEEN SR.StartDate AND SR.EndDate) THEN 1  "
															+ " 	WHEN SR.Inactive = 0 AND (AP.adate > SR.StartDate AND AP.adate > SR.EndDate) THEN 1 "
															+ "     ELSE 0 "
															+ " END ) > 0 ) AS ASAP ON ASAP.aidentifier = AP.aidentifier "
                                                            + " WHERE CT.pid <> 0  AND CT.plname<> '' And Not ((CT.pfname = '' OR CT.plname = '')) "
                                                            + " and AP.adate > @ToDate AND  AP.aidentifier is Not null and AP.aidentifier !='00000000-0000-0000-0000-000000000000' and  SS.snum like 'COL%'"; //@ToDate
                                                            //+ " AND  AP.aidentifier is Not null and AP.aidentifier !='00000000-0000-0000-0000-000000000000' and  SS.snum like 'COL%'"; //@ToDate

        public static string GetAbelDentAppointmentData_12_10_6 = "     SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Appt_LocalDB_ID,AP.aidentifier AS Appt_EHR_ID,'' AS Appt_Web_ID, CT.plname AS Last_Name,CT.pfname AS First_Name,CT.pinitial AS MI,ISNULL(CT.pphone,'') AS Home_Contact,  ISNULL(II.infmobile,'') AS Mobile_Contact,II.infemail AS Email, SUBSTRING(( ISNULL(CT.pstreetadr,'' ) + ' ' + ISNULL(CT.pstreetadr2,'' ) ),0,200) AS [Address], CA.cdesc AS City,'' AS [ST],ISNULL(CT.pcitycode,'') AS Zip, "
                                                            + " AP.achair AS Operatory_EHR_ID,SS.scurrmess AS Operatory_Name ,AP.adid AS Provider_EHR_ID,PD.dname AS Provider_Name,AP.apwork + ' ' +  AP.apwrk2 AS Comment,(CASE When CT.pbirth IS NULL then CONVERT(datetime,'17530101',112) else CT.pbirth end) AS Birth_Date, "
                                                            + " '' AS ApptType_EHR_ID, "
                                                            + " '' AS ApptType , "
                                                            + " AP.apwork AS WorkToDo ,"
                                                            + " CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS Appt_DateTime, (CASE WHEN Ap.aconfstat is null then '' WHEN apsid = '' THEN 'bl' WHEN apsid = '\' then 'ss' WHEN apsid = '\\' then 'ds' ELSE apsid END) as appointment_status_ehr_key, (CASE when Ap.aconfstat is null then '' else APS.apsdesc end) as Appointment_status,"
                                                            + " DATEADD(mi,CONVERT(INT,ap.atimereq * @_minutesInUnit),CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108))) AS Appt_EndDateTime, "
                                                            + " AP.astatus AS[Status],'new' AS Patient_Status,'EHR' as Is_Appt, (CASE WHEN apsdesc = 'Confirmed' THEN 'Y' WHEN apsdesc = 'Preconfirmed' then 'P' ELSE '' END) as confirmed_status_ehr_key,(CASE WHEN apsid = 'Y' THEN 'Confirmed' WHEN apsid = 'P' THEN 'Preconfirmed' ELSE '' END) as confirmed_status, "
                                                            + " GETDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated, AP.apid AS patient_ehr_id,'False' AS is_deleted,0 AS is_Status_Updated_From_Web, getdate() as Entry_DateTime,getdate() as Last_Sync_Date, 'False' as is_asap "
                                                            + " FROM apt AP WITH(NOLOCK)"
                                                            + " LEFT JOIN pat CT WITH(NOLOCK) ON CT.pid = AP.apid "
                                                            + " LEFT JOIN cty CA WITH(NOLOCK) on CA.ccode = CT.pcitycode "
                                                            + " LEFT JOIN inf II WITH(NOLOCK) on II.infpid = CT.pid"
                                                            + " LEFT JOIN dnt PD WITH(NOLOCK) ON PD.did = AP.adid"
                                                            + " LEFT JOIN aps APS WITH(NOLOCK) on APS.apsid = AP.aconfstat"
                                                            + " LEFT JOIN sys SS WITH(NOLOCK) ON substring(CAST(SS.snum as varchar(10)),4,10) = AP.achair  "
                                                            + " WHERE CT.pid <> 0  AND CT.plname<> '' And Not ((CT.pfname = '' OR CT.plname = '')) "
                                                            + " and AP.adate > @ToDate AND  AP.aidentifier is Not null and AP.aidentifier !='00000000-0000-0000-0000-000000000000' and  SS.snum like 'COL%'"; //@ToDate

        public static string GetAbelDentAppointmentDataByApptID = "     SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Appt_LocalDB_ID,AP.aidentifier AS Appt_EHR_ID,'' AS Appt_Web_ID, CT.plname AS Last_Name,CT.pfname AS First_Name,CT.pinitial AS MI,ISNULL(CT.pphone,'') AS Home_Contact,  ISNULL(II.infmobile,'') AS Mobile_Contact,II.infemail AS Email, SUBSTRING(( ISNULL(CT.pstreetadr,'' ) + ' ' + ISNULL(CT.pstreetadr2,'' ) ),0,200) AS [Address], CA.cdesc AS City,'' AS [ST],ISNULL(CT.pcitycode,'') AS Zip, "
                                                            + " AP.achair AS Operatory_EHR_ID,SS.scurrmess AS Operatory_Name ,AP.adid AS Provider_EHR_ID,PD.dname AS Provider_Name,AP.apwork + ' ' +  AP.apwrk2 AS Comment,(CASE When CT.pbirth IS NULL then CONVERT(datetime,'17530101',112) else CT.pbirth end) AS Birth_Date, "
                                                            + " '' AS ApptType_EHR_ID, "
                                                            + " '' AS ApptType , "
                                                            + " AP.apwork AS WorkToDo ,"
                                                            + " CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS Appt_DateTime, (CASE WHEN Ap.aconfstat is null then '' WHEN apsid = '' THEN 'bl' WHEN apsid = '\' then 'ss' WHEN apsid = '\\' then 'ds' ELSE apsid END) as appointment_status_ehr_key, (CASE when Ap.aconfstat is null then '' else APS.apsdesc end) as Appointment_status,"
                                                            + " DATEADD(mi,CONVERT(INT,ap.atimereq * @_minutesInUnit),CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108))) AS Appt_EndDateTime, "
                                                            + " AP.astatus AS[Status],'new' AS Patient_Status,'EHR' as Is_Appt, (CASE WHEN apsdesc = 'Confirmed' THEN 'Y' WHEN apsdesc = 'Preconfirmed' then 'P' ELSE '' END) as confirmed_status_ehr_key,(CASE WHEN apsid = 'Y' THEN 'Confirmed' WHEN apsid = 'P' THEN 'Preconfirmed' ELSE '' END) as confirmed_status, "
                                                            + " GETDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated, AP.apid AS patient_ehr_id,'False' AS is_deleted,0 AS is_Status_Updated_From_Web, getdate() as Entry_DateTime,getdate() as Last_Sync_Date,CASE  WHEN ASAP.IsASAP = 0 THEN 'False' WHEN ASAP.IsASAP IS NULL THEN 'False' ELSE 'True' END as is_asap"
                                                            + " FROM apt AP WITH(NOLOCK)"
                                                            + " LEFT JOIN pat CT WITH(NOLOCK) ON CT.pid = AP.apid "
                                                            + " LEFT JOIN cty CA WITH(NOLOCK) on CA.ccode = CT.pcitycode "
                                                            + " LEFT JOIN inf II WITH(NOLOCK) on II.infpid = CT.pid"
                                                            + " LEFT JOIN dnt PD WITH(NOLOCK) ON PD.did = AP.adid"
                                                            + " LEFT JOIN aps APS WITH(NOLOCK) on APS.apsid = AP.aconfstat"
                                                            + " LEFT JOIN sys SS WITH(NOLOCK) ON substring(CAST(SS.snum as varchar(10)),4,10) = AP.achair  "
                                                            + " LEFT JOIN(SELECT AP.aidentifier, AP.apid as PatientID, CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS APt_DateTime, "
                                                            + " (CASE WHEN SR.Inactive = 0 and(AP.adate BETWEEN SR.StartDate AND SR.EndDate) THEN 1 "
                                                            + "    WHEN SR.Inactive = 0 AND(AP.adate > SR.StartDate AND AP.adate > SR.EndDate) THEN 1 "
                                                            + "      ELSE 0 "
                                                            + "  END ) AS IsASAP "
                                                            + " FROM apt AP "
                                                            + " INNER JOIN SchedulingRequest SR on sR.PID = AP.apid AND SR.Inactive = 0 "
                                                            + " where "
                                                            + " (CASE WHEN SR.Inactive = 0 and (AP.adate BETWEEN SR.StartDate AND SR.EndDate) THEN 1 "
                                                            + " 	WHEN SR.Inactive = 0 AND (AP.adate > SR.StartDate AND AP.adate > SR.EndDate) THEN 1 "
                                                            + " 	  ELSE 0 "
                                                            + " END ) > 0 ) AS ASAP ON ASAP.aidentifier = AP.aidentifier "
                                                            + " WHERE AP.aidentifier = @Appt_EHR_ID and CT.pid <> 0  AND CT.plname<> '' And Not ((CT.pfname = '' OR CT.plname = '')) "
                                                            + " and AP.adate > @ToDate AND  AP.aidentifier is Not null and AP.aidentifier !='00000000-0000-0000-0000-000000000000' and  SS.snum like 'COL%'"; //@ToDate

        public static string GetAbelDentAppointmentDataByApptID_12_10_6 = "     SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Appt_LocalDB_ID,AP.aidentifier AS Appt_EHR_ID,'' AS Appt_Web_ID, CT.plname AS Last_Name,CT.pfname AS First_Name,CT.pinitial AS MI,ISNULL(CT.pphone,'') AS Home_Contact,  ISNULL(II.infmobile,'') AS Mobile_Contact,II.infemail AS Email, SUBSTRING(( ISNULL(CT.pstreetadr,'' ) + ' ' + ISNULL(CT.pstreetadr2,'' ) ),0,200) AS [Address], CA.cdesc AS City,'' AS [ST],ISNULL(CT.pcitycode,'') AS Zip, "
                                                            + " AP.achair AS Operatory_EHR_ID,SS.scurrmess AS Operatory_Name ,AP.adid AS Provider_EHR_ID,PD.dname AS Provider_Name,AP.apwork + ' ' +  AP.apwrk2 AS Comment,(CASE When CT.pbirth IS NULL then CONVERT(datetime,'17530101',112) else CT.pbirth end) AS Birth_Date, "
                                                            + " '' AS ApptType_EHR_ID, "
                                                            + " '' AS ApptType , "
                                                            + " AP.apwork AS WorkToDo ,"
                                                            + " CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS Appt_DateTime, (CASE WHEN Ap.aconfstat is null then '' WHEN apsid = '' THEN 'bl' WHEN apsid = '\' then 'ss' WHEN apsid = '\\' then 'ds' ELSE apsid END) as appointment_status_ehr_key, (CASE when Ap.aconfstat is null then '' else APS.apsdesc end) as Appointment_status,"
                                                            + " DATEADD(mi,CONVERT(INT,ap.atimereq * @_minutesInUnit),CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108))) AS Appt_EndDateTime, "
                                                            + " AP.astatus AS[Status],'new' AS Patient_Status,'EHR' as Is_Appt, (CASE WHEN apsdesc = 'Confirmed' THEN 'Y' WHEN apsdesc = 'Preconfirmed' then 'P' ELSE '' END) as confirmed_status_ehr_key,(CASE WHEN apsid = 'Y' THEN 'Confirmed' WHEN apsid = 'P' THEN 'Preconfirmed' ELSE '' END) as confirmed_status, "
                                                            + " GETDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated, AP.apid AS patient_ehr_id,'False' AS is_deleted,0 AS is_Status_Updated_From_Web, getdate() as Entry_DateTime,getdate() as Last_Sync_Date, 'False' as is_asap "
                                                            + " FROM apt AP WITH(NOLOCK)"
                                                            + " LEFT JOIN pat CT WITH(NOLOCK) ON CT.pid = AP.apid "
                                                            + " LEFT JOIN cty CA WITH(NOLOCK) on CA.ccode = CT.pcitycode "
                                                            + " LEFT JOIN inf II WITH(NOLOCK) on II.infpid = CT.pid"
                                                            + " LEFT JOIN dnt PD WITH(NOLOCK) ON PD.did = AP.adid"
                                                            + " LEFT JOIN aps APS WITH(NOLOCK) on APS.apsid = AP.aconfstat"
                                                            + " LEFT JOIN sys SS WITH(NOLOCK) ON substring(CAST(SS.snum as varchar(10)),4,10) = AP.achair  "
                                                            + " WHERE AP.aidentifier = @Appt_EHR_ID and CT.pid <> 0  AND CT.plname<> '' And Not ((CT.pfname = '' OR CT.plname = '')) "
                                                            + " and AP.adate > @ToDate AND  AP.aidentifier is Not null and AP.aidentifier !='00000000-0000-0000-0000-000000000000' and  SS.snum like 'COL%'"; //@ToDate

        public static string GetAbelDentAppointmentEhrIds = "SELECT a.apid AS Appt_EHR_ID FROM apt a WITH(NOLOCK)"
                                                            + " INNER JOIN pat pa WITH(NOLOCK) ON pa.pid = a.apid "
                                                            + " INNER JOIN dnt o WITH(NOLOCK) ON a.adid = o.did "
                                                            + " WHERE a.adate > @ToDate;";

        public static string GetAbelDentAppointmentsPatientData = " Select DISTINCT PA.pid AS Patient_EHR_ID,'' AS Patient_Web_ID,PA.pfname AS First_name,PA.plname AS Last_name, "
                                                                + " PA.pinitial AS Middle_Name,PA.pmrmrs AS Salutation,case when PA.pinactive = 0 then 'A' else 'I' end AS [Status], PA.pgender,'' AS MaritalStatus, "
                                                                + " (CASE When PA.pbirth IS NULL then CONVERT(datetime,'17530101',112) else PA.pbirth end) AS Birth_Date,AA.infemail as Email, ISNULL(AA.infmobile,'') AS Mobile, ISNULL(PA.pphone, '') AS Home_Phone, ISNULL(PA.pworkphn, '') AS Work_Phone, "
                                                                + " ISNULL(PA.pstreetadr, '') AS Address1, ISNULL(PA.pstreetadr2, '') AS Address2, CT.cdesc AS[City],'' AS[State],ISNULL(PA.ppostal,'') AS Zipcode, "
                                                                + " (case WHEN P2.pinactive is NULL then '' when P2.pinactive = 1 then '' ELSE 'Active' end) AS ResponsibleParty_Status,ISNULL(AG.Pat_AR30,0) AS ThirtyDay,ISNULL(AG.Pat_AR60,0) AS SixtyDay,ISNULL(AG.Pat_AR90,0) AS NinetyDay,ISNULL(AG.Pat_AR90_Plus,0) AS Over90,ISNULL(AG.Pat_Total,0) AS CurrentBal, ISNULL(HH.RemainingAmount,0) - ISNULL(ISB.totalCoverageUsed,0) as remaining_benefit,ISNULL(ISB.totalCoverageUsed,0) as used_benefit,'' AS ssn, '' as school,"
                                                                + " FVisit.FirstVisitDate AS FirstVisit_Date,ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance, ISNULL(PrimIns.PrimaryInsuranceCompanyName, '') AS Primary_Insurance_CompanyName, PrimIns.PrimarySubcriberId as Primary_Ins_Subscriber_ID, ISNULL(PrimIns.Prim_Ins_Company_Phonenumber,'') as Prim_Ins_Company_Phonenumber,SecIns.SecondarySubcriberId as Secondary_Ins_Subscriber_ID,ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance,ISNULL(SecIns.SecondaryInsuranceCompanyName,'') AS Secondary_Insurance_CompanyName,ISNULL(SecIns.Sec_Ins_Company_Phonenumber,'') as Sec_Ins_Company_Phonenumber,"
                                                                + " (CASE WHEN PA.pchargeto = 0 then PA.pid ELSE PA.pchargeto END) AS Guar_ID,'' as employer, ISNULL(PA.pdentist,'') AS Pri_Provider_ID,ISNULL(PA.phygienist,'') AS Sec_Provider_ID, (CASE WHEN AA.infallowtextmsg = 1 then 'Y' else 'N' end) AS ReceiveSms, (CASE WHEN AA.infallownewsemail = 1 then 'Y' else 'N' end) AS ReceiveEmail,"
                                                                + "  LVisit.LastVisitDate  AS LastVisit_Date, (SELECT MIN (adate) From apt WITH(NOLOCK) Where adate > GETDATE() AND astatus NOT IN ('R') and apid = PA.pid) AS nextvisit_date,(case WHEN PA.pgender = 'M' THEN 'MALE' WHEN PA.pgender = 'F' THEN 'FEMALE' WHEN PA.pgender = '' THEN '' END ) as SEX,"
                                                                + " AB.DueDate AS due_date,ISNULL(AG.Collect_total,0) AS collect_payment, '' AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated,'' AS preferred_name,"
                                                                + " 1 As InsUptDlt,'Y' as ReceiveVoiceCall,CASE WHEN PA.pnativetongue = 'E' THEN 'English' WHEN PA.pnativetongue = 'F' THEN 'French' WHEN PA.pnativetongue = '' THEN 'ENGLISH' END AS PreferredLanguage,AA.infnote AS Patient_note, "
                                                                + " P2.pid AS responsiblepartyid,P2.pfname AS ResponsibleParty_First_Name,P2.plname AS ResponsibleParty_Last_Name,'' AS responsiblepartyssn,P2.pbirth AS responsiblepartybirthdate,'' AS EmergencyContactId,'' AS EmergencyContact_First_Name,'' as Sec_Ins_Company_Phonenumber,"
                                                                + " '' AS EmergencyContact_Last_Name,'' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' AS driverlicense,"
                                                                + " '' AS groupid,0 as Clinic_Number, 1 as Service_Install_Id, (case WHEN PA.pinactive = 1 then 'InActive' ELSE 'Active' end ) AS EHR_Status,0 as Patient_LocalDB_ID from pat PA WITH(NOLOCK)"
                                                                + " INNER JOIN apt AS AP WITH(NOLOCK) on Ap.apid = PA.pid"
                                                                + " LEFT join inf AA WITH(NOLOCK) on AA.infpid = PA.pid"
                                                                + " LEFT join pat P2 WITH(NOLOCK) on PA.presides = P2.pid"
                                                                + " LEFT join cty CT WITH(NOLOCK) on CT.ccode = PA.pcitycode"                                                                                                                                                                                                    
                                                                + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , CAST(NS.inscoid AS varchar) as PrimaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as PrimarySubcriberId, NS.insname as PrimaryInsuranceCompanyName, NS.insphone as Prim_Ins_Company_Phonenumber from pat as p WITH(NOLOCK)  "
                                                                + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 1) as PrimIns on PrimIns.PatientEHRId = PA.pid "
                                                                + " LEFT JOIN(select DISTINCT p.pid as PatientEHRId , NS.inscoid as SecondaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as SecondarySubcriberId, NS.insname as SecondaryInsuranceCompanyName , NS.insphone as Sec_Ins_Company_Phonenumber from pat as p WITH(NOLOCK) "
                                                                + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "                                                                
                                                                + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 2) as SecIns on SecIns.PatientEHRId = PA.pid "
                                                                + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MAX(Appt.adate) as LastVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null AND Appt.adate <= CONVERT(DATE, GETDATE()) Group by Appt.apid ) AS LVisit on LVisit.Patient_EHR_Id =  PA.pid  "
                                                                + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MIN(Appt.adate) as FirstVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null Group by Appt.apid) AS FVisit on FVisit.Patient_EHR_Id =  PA.pid  "
                                                                + " LEFT JOIN (select PP.pid,CONVERT( varchar, ( case when PP.paptinvl > 12 then (DATEADD(Day, CAST(PP.paptinvl AS INT),  AA.DueDate)) else ((DATEADD(Month, CAST(PP.paptinvl AS INT),  AA.DueDate)))END)) +'@'+ CONVERT( varchar,  rl.msgmessage) + '@' + CAST(rl.msgidentifier AS varchar) AS DueDate   from pat PP WITH(NOLOCK) "
                                                                + " INNER JOIN (SELECT apid, Max(ap1.adate) AS DueDate FROM apt ap1 WITH(NOLOCK) Where aconfstat = 'D' AND astatus  != 'R' GROUP BY apid) AS AA ON PP.pid = AA.apid "
                                                                    + " LEFT JOIN msg rl WITH(NOLOCK) on rl.msgid = PP.paptinvl where rl.msgid != '' and rl.msgid != 0 and rl.msgtype = 'PATAI' and ISNUMERIC(rl.msgid) = 1 and rl.msgid > 0) as AB on AB.pid = PA.pid "
                                                                + " LEFT JOIN (Select Trn.tpid, "
                                                                + " SUM(CASE WHEN Trn.tdate <= CONVERT(DATE, GETDATE()) AND Trn.tdate >= DATEADD(day, -29, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg)/100.00) ELSE 0 END) - MIN(TT.PatientPayment) As Pat_AR30, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -30, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -59, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR60, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -60, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -89, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -90, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90_Plus, "
                                                                + " SUM((Trn.tamount - Trn.tinschg) / 100.00) - MIN(TT.PatientPayment)  as Pat_Total,MIN(TT.PatientPayment) as Collect_total "
                                                                + " FROM trn Trn "
                                                                + " JOIN Pat Pat On Pat.Pid = Trn.tpid "
                                                                + " LEFT JOIN(Select Trn.tpid as PatientId, "
                                                                + " SUM(Case When Trn.tptype != 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as PatientPayment, "
		                                                        + " SUM(Case When Trn.tptype  = 'R' THEN Trn.tinschg/100.00 ELSE 0 END) as InsPayment "
                                                                + " from trn Trn Where Trn.tprepay != 1 "
                                                                + " GROUP BY Trn.tpid) as TT on TT.PatientId = Trn.tpid "
                                                                + " where Pat.pInactive = 0 and Trn.tprepay != 1"
                                                                + " Group by Trn.tpid "
                                                                + " HAVING SUM((Trn.tamount - Trn.tinschg)/100.00) - MIN(TT.PatientPayment) > 0 ) as AG on AG.tpid = PA.pid "
                                                                + " LEFT JOIN (Select i1.ixipid, i1.ixiplno, COALESCE(i2.ixiplanid,i1.ixiplanid) as ixiplanid, COALESCE(i2.ixicertno, i1.ixicertno) as ixicertno, "
                                                                        + " COALESCE(i2.ixiassigned, i1.ixiassigned) as ixiassigned, CASE WHEN i1.ixisubpid != 0 THEN i1.ixisubpid ELSE i1.ixipid END as ixisubpid, i1.ixireltosub, "
                                                                        + " COALESCE(i2.ixigroupno, i1.ixigroupno) as ixigroupno, COALESCE(i2.ixidivsect, i1.ixidivsect) as ixidivsect, "
                                                                        + " COALESCE(i2.ixisubuse, i1.ixisubuse) as ixisubuse, COALESCE(i2.ixiReleaseOfInfo, i1.ixiReleaseOfInfo) as ixiReleaseOfInfo, "
                                                                        + " NS.inscoid, Ns.InsName, NS.insphone, NS.inscarrierid, "
                                                                        + " nsp.ninscoid, nsp.nspdeductibles, nsp.nspmaximums, nsp.nassigned, nsp.nsppreventive, nsp.nspbasic, nsp.nspmajor, nsp.nsportho, nsp.nspplantype, "
                                                                        + " nsp.nanniv, nsp.nplanname, nsp.nsppercentagelimit, nsp.nfeeyear, nsp.nchargesched, nsp.ncopaysched, nsp.nspexclusive "
                                                                        + " from ixi as i1 "
                                                                        + " LEFT JOIN ixi as i2 on i1.ixisubpid = i2.ixipid AND i2.ixiplno = i1.ixisubuse "
                                                                        + " join nsp Nsp on nsp.nid = COALESCE(i2.ixiplanid, i1.ixiplanid) "
                                                                        + " JOIN ins as NS WITH(NOLOCK) on NS.inscoid = Nsp.ninscoid where i1.ixiplno = 1) as INF on INF.ixipid = PA.pid "
                                                                        + "           LEFT JOIN (Select Itn.itnsubscriber, Itn.itnplanid, "
                                                                        + "      SUM(Itn.itndedamount/100.00) as deductionPaid, "
                                                                        + "	   SUM(Itn.itnbenefit/100.00) as totalCoverageUsed "
                                                                        + " from Itn "
                                                                        + " group by Itn.itnsubscriber, Itn.itnplanid) as ISB on ISB.itnsubscriber = INF.ixisubpid and ISB.itnplanid = INF.ixiplanid "
                                                                + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , case when nspdeductibles = '' then LEFT(SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000) + 'X') -1) else LEFT(SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000) + 'X') -1) end as RemainingAmount from  pat as p WITH(NOLOCK) "
                                                                        + " Inner JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                        + " Inner Join nsp as NP with(NOLOCK) on NP.nid = XI.ixiplanid "
                                                                + " where NP.nspmaximums != '' or NP.nspdeductibles != '') as HH on HH.PatientEHRId = PA.pid "
                                                                + " WHERE PA.pid <> ''  AND PA.plname<> '' and ((PA.pfname <> '' OR PA.plname <> '') AND (AP.adate >  @ToDate))";

        public static string GetAbelDentAppointmentsPatientData_12_10_6 = " Select DISTINCT PA.pid AS Patient_EHR_ID,'' AS Patient_Web_ID,PA.pfname AS First_name,PA.plname AS Last_name, "
                                                                + " PA.pinitial AS Middle_Name,PA.pmrmrs AS Salutation,case when PA.pinactive = 0 then 'A' else 'I' end AS [Status], PA.pgender,'' AS MaritalStatus, "
                                                                + " (CASE When PA.pbirth IS NULL then CONVERT(datetime,'17530101',112) else PA.pbirth end) AS Birth_Date,AA.infemail as Email, ISNULL(AA.infmobile,'') AS Mobile, ISNULL(PA.pphone, '') AS Home_Phone, ISNULL(PA.pworkphn, '') AS Work_Phone, "
                                                                + " ISNULL(PA.pstreetadr, '') AS Address1, ISNULL(PA.pstreetadr2, '') AS Address2, CT.cdesc AS[City],'' AS[State],ISNULL(PA.ppostal,'') AS Zipcode, "
                                                                + " (case WHEN P2.pinactive is NULL then '' when P2.pinactive = 1 then '' ELSE 'Active' end) AS ResponsibleParty_Status,ISNULL(AG.Pat_AR30,0) AS ThirtyDay,ISNULL(AG.Pat_AR60,0) AS SixtyDay,ISNULL(AG.Pat_AR90,0) AS NinetyDay,ISNULL(AG.Pat_AR90_Plus,0) AS Over90,ISNULL(AG.Pat_Total,0) AS CurrentBal, ISNULL(HH.RemainingAmount,0) - ISNULL(ISB.totalCoverageUsed,0) as remaining_benefit,ISNULL(ISB.totalCoverageUsed,0) as used_benefit,'' AS ssn, '' as school,"
                                                                + " FVisit.FirstVisitDate AS FirstVisit_Date,ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance, ISNULL(PrimIns.PrimaryInsuranceCompanyName, '') AS Primary_Insurance_CompanyName, PrimIns.PrimarySubcriberId as Primary_Ins_Subscriber_ID, ISNULL(PrimIns.Prim_Ins_Company_Phonenumber,'') as Prim_Ins_Company_Phonenumber,SecIns.SecondarySubcriberId as Secondary_Ins_Subscriber_ID,ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance,ISNULL(SecIns.SecondaryInsuranceCompanyName,'') AS Secondary_Insurance_CompanyName,ISNULL(SecIns.Sec_Ins_Company_Phonenumber,'') as Sec_Ins_Company_Phonenumber,"
                                                                + " (CASE WHEN PA.pchargeto = 0 then PA.pid ELSE PA.pchargeto END) AS Guar_ID,'' as employer, ISNULL(PA.pdentist,'') AS Pri_Provider_ID,ISNULL(PA.phygienist,'') AS Sec_Provider_ID, (CASE WHEN AA.infallowtextmsg = 1 then 'Y' else 'N' end) AS ReceiveSms, (CASE WHEN AA.infallownewsemail = 1 then 'Y' else 'N' end) AS ReceiveEmail,"
                                                                + "  LVisit.LastVisitDate  AS LastVisit_Date, (SELECT MIN (adate) From apt WITH(NOLOCK) Where adate > GETDATE() AND astatus NOT IN ('R') and apid = PA.pid) AS nextvisit_date,(case WHEN PA.pgender = 'M' THEN 'MALE' WHEN PA.pgender = 'F' THEN 'FEMALE' WHEN PA.pgender = '' THEN '' END ) as SEX,"
                                                                + " AB.DueDate AS due_date,ISNULL(AG.Collect_total,0) AS collect_payment, '' AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated,'' AS preferred_name,"
                                                                + " 1 As InsUptDlt,'Y' as ReceiveVoiceCall,CASE WHEN PA.pnativetongue = 'E' THEN 'English' WHEN PA.pnativetongue = 'F' THEN 'French' WHEN PA.pnativetongue = '' THEN 'ENGLISH' END AS PreferredLanguage,AA.infnote AS Patient_note, "
                                                                + " P2.pid AS responsiblepartyid,P2.pfname AS ResponsibleParty_First_Name,P2.plname AS ResponsibleParty_Last_Name,'' AS responsiblepartyssn,P2.pbirth AS responsiblepartybirthdate,'' AS EmergencyContactId,'' AS EmergencyContact_First_Name,'' as Sec_Ins_Company_Phonenumber,"
                                                                + " '' AS EmergencyContact_Last_Name,'' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' AS driverlicense,"
                                                                + " '' AS groupid,0 as Clinic_Number, 1 as Service_Install_Id, (case WHEN PA.pinactive = 1 then 'InActive' ELSE 'Active' end ) AS EHR_Status,0 as Patient_LocalDB_ID from pat PA WITH(NOLOCK)"
                                                                + " INNER JOIN apt AS AP WITH(NOLOCK) on Ap.apid = PA.pid"
                                                                + " LEFT join inf AA WITH(NOLOCK) on AA.infpid = PA.pid"
                                                                + " LEFT join pat P2 WITH(NOLOCK) on PA.presides = P2.pid"
                                                                + " LEFT join cty CT WITH(NOLOCK) on CT.ccode = PA.pcitycode"
                                                                + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , CAST(NS.inscoid AS varchar) as PrimaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as PrimarySubcriberId, NS.insname as PrimaryInsuranceCompanyName, NS.insphone as Prim_Ins_Company_Phonenumber from pat as p WITH(NOLOCK)  "
                                                                + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 1) as PrimIns on PrimIns.PatientEHRId = PA.pid "
                                                                + " LEFT JOIN(select DISTINCT p.pid as PatientEHRId , NS.inscoid as SecondaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as SecondarySubcriberId, NS.insname as SecondaryInsuranceCompanyName , NS.insphone as Sec_Ins_Company_Phonenumber from pat as p WITH(NOLOCK) "
                                                                + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 2) as SecIns on SecIns.PatientEHRId = PA.pid "
                                                                + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MAX(Appt.adate) as LastVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null AND Appt.adate <= CONVERT(DATE, GETDATE()) Group by Appt.apid ) AS LVisit on LVisit.Patient_EHR_Id =  PA.pid  "
                                                                + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MIN(Appt.adate) as FirstVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null Group by Appt.apid) AS FVisit on FVisit.Patient_EHR_Id =  PA.pid  "
                                                                + " LEFT JOIN (select PP.pid,CONVERT( varchar, ( case when PP.paptinvl > 12 then (DATEADD(Day, CAST(PP.paptinvl AS INT),  AA.DueDate)) else ((DATEADD(Month, CAST(PP.paptinvl AS INT),  AA.DueDate)))END)) +'@'+ CONVERT( varchar,  rl.msgmessage) + '@' + CAST(rl.msgid AS varchar) AS DueDate   from pat PP WITH(NOLOCK) "
                                                                + " INNER JOIN (SELECT apid, Max(ap1.adate) AS DueDate FROM  apt ap1 WITH(NOLOCK) Where aconfstat = 'D' AND astatus  != 'R' GROUP BY apid) AS AA ON PP.pid = AA.apid "
                                                                    + " LEFT JOIN msg rl WITH(NOLOCK) on rl.msgid = PP.paptinvl where rl.msgid != '' and rl.msgid != 0 and rl.msgtype = 'PATAI' and ISNUMERIC(rl.msgid) = 1 and rl.msgid > 0) as AB on AB.pid = PA.pid "
                                                                + " LEFT JOIN (Select Trn.tpid, "
                                                                + " SUM(CASE WHEN Trn.tdate <= CONVERT(DATE, GETDATE()) AND Trn.tdate >= DATEADD(day, -29, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg)/100.00) ELSE 0 END) - MIN(TT.PatientPayment) As Pat_AR30, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -30, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -59, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR60, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -60, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -89, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -90, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90_Plus, "
                                                                + " SUM((Trn.tamount - Trn.tinschg) / 100.00) - MIN(TT.PatientPayment)  as Pat_Total,MIN(TT.PatientPayment) as Collect_total "
                                                                + " FROM trn Trn "
                                                                + " JOIN Pat Pat On Pat.Pid = Trn.tpid "
                                                                + " LEFT JOIN(Select Trn.tpid as PatientId, "
                                                                + " SUM(Case When Trn.tptype != 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as PatientPayment, "
                                                                + " SUM(Case When Trn.tptype  = 'R' THEN Trn.tinschg/100.00 ELSE 0 END) as InsPayment "
                                                                + " from trn Trn Where Trn.tprepay != 1 "
                                                                + " GROUP BY Trn.tpid) as TT on TT.PatientId = Trn.tpid "
                                                                + " where Pat.pInactive = 0 and Trn.tprepay != 1 "
                                                                + " Group by Trn.tpid "
                                                                + " HAVING SUM((Trn.tamount - Trn.tinschg)/100.00) - MIN(TT.PatientPayment) > 0 ) as AG on AG.tpid = PA.pid "
                                                                + " LEFT JOIN (Select i1.ixipid, i1.ixiplno, COALESCE(i2.ixiplanid,i1.ixiplanid) as ixiplanid, COALESCE(i2.ixicertno, i1.ixicertno) as ixicertno, "
                                                                        + " COALESCE(i2.ixiassigned, i1.ixiassigned) as ixiassigned, CASE WHEN i1.ixisubpid != 0 THEN i1.ixisubpid ELSE i1.ixipid END as ixisubpid, i1.ixireltosub, "
                                                                        + " COALESCE(i2.ixigroupno, i1.ixigroupno) as ixigroupno, COALESCE(i2.ixidivsect, i1.ixidivsect) as ixidivsect, "
                                                                        + " COALESCE(i2.ixisubuse, i1.ixisubuse) as ixisubuse, COALESCE(i2.ixiReleaseOfInfo, i1.ixiReleaseOfInfo) as ixiReleaseOfInfo, "
                                                                        + " NS.inscoid, Ns.InsName, NS.insphone, NS.inscarrierid, "
                                                                        + " nsp.ninscoid, nsp.nspdeductibles, nsp.nspmaximums, nsp.nassigned, nsp.nsppreventive, nsp.nspbasic, nsp.nspmajor, nsp.nsportho, nsp.nspplantype, "
                                                                        + " nsp.nanniv, nsp.nplanname, nsp.nsppercentagelimit, nsp.nfeeyear, nsp.nchargesched, nsp.ncopaysched, nsp.nspexclusive "
                                                                        + " from ixi as i1 "
                                                                        + " LEFT JOIN ixi as i2 on i1.ixisubpid = i2.ixipid AND i2.ixiplno = i1.ixisubuse "
                                                                        + " join nsp Nsp on nsp.nid = COALESCE(i2.ixiplanid, i1.ixiplanid) "
                                                                        + " JOIN ins as NS WITH(NOLOCK) on NS.inscoid = Nsp.ninscoid where i1.ixiplno = 1) as INF on INF.ixipid = PA.pid "
                                                                        + "           LEFT JOIN (Select Itn.itnsubscriber, Itn.itnplanid, "
                                                                        + "      SUM(Itn.itndedamount/100.00) as deductionPaid, "
                                                                        + "	   SUM(Itn.itnbenefit/100.00) as totalCoverageUsed "
                                                                        + " from Itn "
                                                                        + " group by Itn.itnsubscriber, Itn.itnplanid) as ISB on ISB.itnsubscriber = INF.ixisubpid and ISB.itnplanid = INF.ixiplanid "
                                                                + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , case when nspdeductibles = '' then LEFT(SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000) + 'X') -1) else LEFT(SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000) + 'X') -1) end as RemainingAmount from  pat as p WITH(NOLOCK) "
                                                                        + " Inner JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                        + " Inner Join nsp as NP with(NOLOCK) on NP.nid = XI.ixiplanid "
                                                                + " where NP.nspmaximums != '' or NP.nspdeductibles != '') as HH on HH.PatientEHRId = PA.pid "
                                                                + " WHERE PA.pid <> ''  AND PA.plname<> '' and ((PA.pfname <> '' OR PA.plname <> '') AND (AP.adate >  @ToDate))";

        public static string GetAbelDentAppointmentsPatientDataByPatID = " Select DISTINCT PA.pid AS Patient_EHR_ID,'' AS Patient_Web_ID,PA.pfname AS First_name,PA.plname AS Last_name, "
                                                                + " PA.pinitial AS Middle_Name,PA.pmrmrs AS Salutation,case when PA.pinactive = 0 then 'A' else 'I' end AS [Status], PA.pgender,'' AS MaritalStatus, "
                                                                + " (CASE When PA.pbirth IS NULL then CONVERT(datetime,'17530101',112) else PA.pbirth end) AS Birth_Date,AA.infemail as Email, ISNULL(AA.infmobile,'') AS Mobile, ISNULL(PA.pphone, '') AS Home_Phone, ISNULL(PA.pworkphn, '') AS Work_Phone, "
                                                                + " ISNULL(PA.pstreetadr, '') AS Address1, ISNULL(PA.pstreetadr2, '') AS Address2, CT.cdesc AS[City],'' AS[State],ISNULL(PA.ppostal,'') AS Zipcode, "
                                                                + " (case WHEN P2.pinactive is NULL then '' when P2.pinactive = 1 then '' ELSE 'Active' end) AS ResponsibleParty_Status,ISNULL(AG.Pat_AR30,0) AS ThirtyDay,ISNULL(AG.Pat_AR60,0) AS SixtyDay,ISNULL(AG.Pat_AR90,0) AS NinetyDay,ISNULL(AG.Pat_AR90_Plus,0) AS Over90,ISNULL(AG.Pat_Total,0) AS CurrentBal, 0 AS remaining_benefit, 0 AS used_benefit,'' AS ssn, '' as school,"
                                                                + " '' AS FirstVisit_Date,ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance, ISNULL(PrimIns.PrimaryInsuranceCompanyName, '') AS Primary_Insurance_CompanyName, PrimIns.PrimarySubcriberId as Primary_Ins_Subscriber_ID, ISNULL(PrimIns.Prim_Ins_Company_Phonenumber,'') as Prim_Ins_Company_Phonenumber,SecIns.SecondarySubcriberId as Secondary_Ins_Subscriber_ID,ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance,ISNULL(SecIns.SecondaryInsuranceCompanyName,'') AS Secondary_Insurance_CompanyName,ISNULL(SecIns.Sec_Ins_Company_Phonenumber,'') as Sec_Ins_Company_Phonenumber,"
                                                                + " (CASE WHEN PA.pchargeto = 0 then PA.pid ELSE PA.pchargeto END) AS Guar_ID,'' as employer, ISNULL(PA.pdentist,'') AS Pri_Provider_ID,ISNULL(PA.phygienist,'') AS Sec_Provider_ID, (CASE WHEN AA.infallowtextmsg = 1 then 'Y' else 'N' end) AS ReceiveSms, (CASE WHEN AA.infallownewsemail = 1 then 'Y' else 'N' end) AS ReceiveEmail,"
                                                                + "  LVisit.LastVisitDate  AS LastVisit_Date, (SELECT MIN (adate) From apt WITH(NOLOCK) Where adate > GETDATE() AND astatus NOT IN ('R') and apid = PA.pid) AS nextvisit_date,(case WHEN PA.pgender = 'M' THEN 'MALE' WHEN PA.pgender = 'F' THEN 'FEMALE' WHEN PA.pgender = '' THEN '' END ) as SEX,"
                                                                + " AB.DueDate AS due_date,ISNULL(TP.COLLEctPAY,0) AS collect_payment, '' AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated,'' AS preferred_name,"
                                                                + " 1 As InsUptDlt,'Y' as ReceiveVoiceCall,CASE WHEN PA.pnativetongue = 'E' THEN 'English' WHEN PA.pnativetongue = 'F' THEN 'French' WHEN PA.pnativetongue = '' THEN 'ENGLISH' END AS PreferredLanguage,AA.infnote AS Patient_note, "
                                                                + " P2.pid AS responsiblepartyid,P2.pfname AS ResponsibleParty_First_Name,P2.plname AS ResponsibleParty_Last_Name,'' AS responsiblepartyssn,P2.pbirth AS responsiblepartybirthdate,'' AS EmergencyContactId,'' AS EmergencyContact_First_Name,'' as Sec_Ins_Company_Phonenumber,"
                                                                + " '' AS EmergencyContact_Last_Name,'' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' AS driverlicense,"
                                                                + " '' AS groupid,0 as Clinic_Number, 1 as Service_Install_Id, (case WHEN PA.pinactive = 1 then 'InActive' ELSE 'Active' end ) AS EHR_Status,0 as Patient_LocalDB_ID from pat PA WITH(NOLOCK)"
                                                                + " INNER JOIN apt AS AP WITH(NOLOCK) on Ap.apid = PA.pid"
                                                                + " LEFT join inf AA WITH(NOLOCK) on AA.infpid = PA.pid"
                                                                + " LEFT join pat P2 WITH(NOLOCK) on PA.presides = P2.pid"
                                                                + " LEFT join cty CT WITH(NOLOCK) on CT.ccode = PA.pcitycode"
                                                                + " LEFT JOIN (SELECT Trn.tpid as PatientId, SUM(Trn.tpamt/100.00) as COLLEctPAY FROM  [trn] Trn where Trn.tprepay = 1 GROUP BY Trn.tpid) as TP ON TP.PatientId = PA.pid "
                                                                + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , CAST(NS.inscoid AS varchar) as PrimaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as PrimarySubcriberId, NS.insname as PrimaryInsuranceCompanyName, NS.insphone as Prim_Ins_Company_Phonenumber from pat as p WITH(NOLOCK)  "
                                                                + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 1) as PrimIns on PrimIns.PatientEHRId = PA.pid "
                                                                + " LEFT JOIN(select DISTINCT p.pid as PatientEHRId , NS.inscoid as SecondaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as SecondarySubcriberId, NS.insname as SecondaryInsuranceCompanyName , NS.insphone as Sec_Ins_Company_Phonenumber from pat as p WITH(NOLOCK) "
                                                                + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 2) as SecIns on SecIns.PatientEHRId = PA.pid "
                                                                + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MAX(Appt.adate) as LastVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null AND Appt.adate <= CONVERT(DATE, GETDATE()) Group by Appt.apid ) AS LVisit on LVisit.Patient_EHR_Id =  PA.pid  "
                                                                + " LEFT JOIN (select PP.pid,CONVERT( varchar, ( case when PP.paptinvl > 12 then (DATEADD(Day, CAST(PP.paptinvl AS INT),  AA.DueDate)) else ((DATEADD(Month, CAST(PP.paptinvl AS INT),  AA.DueDate)))END)) +'@'+ CONVERT( varchar,  rl.msgmessage) + '@' + CAST(rl.msgidentifier AS varchar) AS DueDate   from pat PP WITH(NOLOCK) "
                                                                + " INNER JOIN (SELECT apid, Max(ap1.adate) AS DueDate FROM  apt ap1 WITH(NOLOCK) Where aconfstat = 'D' AND astatus  != 'R' GROUP BY apid) AS AA ON PP.pid = AA.apid "
                                                                    + " LEFT JOIN msg rl WITH(NOLOCK) on rl.msgid = PP.paptinvl where rl.msgid != '' and rl.msgid != 0 and rl.msgtype = 'PATAI' and ISNUMERIC(rl.msgid) = 1 and rl.msgid > 0) as AB on AB.pid = PA.pid "
                                                                + " LEFT JOIN (Select Trn.tpid, "
                                                                + " SUM(CASE WHEN Trn.tdate <= CONVERT(DATE, GETDATE()) AND Trn.tdate >= DATEADD(day, -29, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg)/100.00) ELSE 0 END) - 0 As Pat_AR30, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -30, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -59, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR60, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -60, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -89, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -90, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - MIN(TT.PatientPayment) As Pat_AR90_Plus, "
                                                                + " SUM((Trn.tamount - Trn.tinschg) / 100.00) - MIN(TT.PatientPayment)  as Pat_Total "
                                                                + " FROM trn Trn "
                                                                + " JOIN Pat Pat On Pat.Pid = Trn.tpid "
                                                                + " LEFT JOIN(Select Trn.tpid as PatientId, "
                                                                + " SUM(Case When Trn.tptype != 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as PatientPayment, "
                                                                + " SUM(Case When Trn.tptype  = 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as InsPayment "
                                                                + " from trn Trn "
                                                                + " GROUP BY Trn.tpid) as TT on TT.PatientId = Trn.tpid "
                                                                + " where Pat.pInactive = 0 and Trn.tprepay != 1"
                                                                + " Group by Trn.tpid "
                                                                + " HAVING SUM((Trn.tamount - Trn.tinschg)/100.00) - MIN(TT.PatientPayment) > 0 ) as AG on AG.tpid = PA.pid "
                                                                + " WHERE PA.pid = @Patient_EHR_ID and PA.pid <> ''  AND PA.plname<> '' and ((PA.pfname <> '' OR PA.plname <> '') AND (AP.adate >  @ToDate))";

        public static string GetAbelDentAppointmentsPatientDataByPatID_12_10_6 = " Select DISTINCT PA.pid AS Patient_EHR_ID,'' AS Patient_Web_ID,PA.pfname AS First_name,PA.plname AS Last_name, "
                                                                + " PA.pinitial AS Middle_Name,PA.pmrmrs AS Salutation,case when PA.pinactive = 0 then 'A' else 'I' end AS [Status], PA.pgender,'' AS MaritalStatus, "
                                                                + " (CASE When PA.pbirth IS NULL then CONVERT(datetime,'17530101',112) else PA.pbirth end) AS Birth_Date,AA.infemail as Email, ISNULL(AA.infmobile,'') AS Mobile, ISNULL(PA.pphone, '') AS Home_Phone, ISNULL(PA.pworkphn, '') AS Work_Phone, "
                                                                + " ISNULL(PA.pstreetadr, '') AS Address1, ISNULL(PA.pstreetadr2, '') AS Address2, CT.cdesc AS[City],'' AS[State],ISNULL(PA.ppostal,'') AS Zipcode, "
                                                                + " (case WHEN P2.pinactive is NULL then '' when P2.pinactive = 1 then '' ELSE 'Active' end) AS ResponsibleParty_Status,ISNULL(AG.Pat_AR30,0) AS ThirtyDay,ISNULL(AG.Pat_AR60,0) AS SixtyDay,ISNULL(AG.Pat_AR90,0) AS NinetyDay,ISNULL(AG.Pat_AR90_Plus,0) AS Over90,ISNULL(AG.Pat_Total,0) AS CurrentBal, 0 AS remaining_benefit, 0 AS used_benefit,'' AS ssn, '' as school,"
                                                                + " '' AS FirstVisit_Date,ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance, ISNULL(PrimIns.PrimaryInsuranceCompanyName, '') AS Primary_Insurance_CompanyName, PrimIns.PrimarySubcriberId as Primary_Ins_Subscriber_ID, ISNULL(PrimIns.Prim_Ins_Company_Phonenumber,'') as Prim_Ins_Company_Phonenumber,SecIns.SecondarySubcriberId as Secondary_Ins_Subscriber_ID,ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance,ISNULL(SecIns.SecondaryInsuranceCompanyName,'') AS Secondary_Insurance_CompanyName,ISNULL(SecIns.Sec_Ins_Company_Phonenumber,'') as Sec_Ins_Company_Phonenumber,"
                                                                + " (CASE WHEN PA.pchargeto = 0 then PA.pid ELSE PA.pchargeto END) AS Guar_ID,'' as employer, ISNULL(PA.pdentist,'') AS Pri_Provider_ID,ISNULL(PA.phygienist,'') AS Sec_Provider_ID, (CASE WHEN AA.infallowtextmsg = 1 then 'Y' else 'N' end) AS ReceiveSms, (CASE WHEN AA.infallownewsemail = 1 then 'Y' else 'N' end) AS ReceiveEmail,"
                                                                + "  LVisit.LastVisitDate  AS LastVisit_Date, (SELECT MIN (adate) From apt WITH(NOLOCK) Where adate > GETDATE() AND astatus NOT IN ('R') and apid = PA.pid) AS nextvisit_date,(case WHEN PA.pgender = 'M' THEN 'MALE' WHEN PA.pgender = 'F' THEN 'FEMALE' WHEN PA.pgender = '' THEN '' END ) as SEX,"
                                                                + " AB.DueDate AS due_date,ISNULL(TP.COLLEctPAY,0) AS collect_payment, '' AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated,'' AS preferred_name,"
                                                                + " 1 As InsUptDlt,'Y' as ReceiveVoiceCall,CASE WHEN PA.pnativetongue = 'E' THEN 'English' WHEN PA.pnativetongue = 'F' THEN 'French' WHEN PA.pnativetongue = '' THEN 'ENGLISH' END AS PreferredLanguage,AA.infnote AS Patient_note, "
                                                                + " P2.pid AS responsiblepartyid,P2.pfname AS ResponsibleParty_First_Name,P2.plname AS ResponsibleParty_Last_Name,'' AS responsiblepartyssn,P2.pbirth AS responsiblepartybirthdate,'' AS EmergencyContactId,'' AS EmergencyContact_First_Name,'' as Sec_Ins_Company_Phonenumber,"
                                                                + " '' AS EmergencyContact_Last_Name,'' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' AS driverlicense,"
                                                                + " '' AS groupid,0 as Clinic_Number, 1 as Service_Install_Id, (case WHEN PA.pinactive = 1 then 'InActive' ELSE 'Active' end ) AS EHR_Status,0 as Patient_LocalDB_ID from pat PA WITH(NOLOCK)"
                                                                + " INNER JOIN apt AS AP WITH(NOLOCK) on Ap.apid = PA.pid"
                                                                + " LEFT join inf AA WITH(NOLOCK) on AA.infpid = PA.pid"
                                                                + " LEFT join pat P2 WITH(NOLOCK) on PA.presides = P2.pid"
                                                                + " LEFT join cty CT WITH(NOLOCK) on CT.ccode = PA.pcitycode"
                                                                + " LEFT JOIN (SELECT Trn.tpid as PatientId, SUM(Trn.tpamt/100.00) as COLLEctPAY FROM  [trn] Trn where Trn.tprepay = 1 GROUP BY Trn.tpid) as TP ON TP.PatientId = PA.pid "
                                                                + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , CAST(NS.inscoid AS varchar) as PrimaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as PrimarySubcriberId, NS.insname as PrimaryInsuranceCompanyName, NS.insphone as Prim_Ins_Company_Phonenumber from pat as p WITH(NOLOCK)  "
                                                                + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 1) as PrimIns on PrimIns.PatientEHRId = PA.pid "
                                                                + " LEFT JOIN(select DISTINCT p.pid as PatientEHRId , NS.inscoid as SecondaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as SecondarySubcriberId, NS.insname as SecondaryInsuranceCompanyName , NS.insphone as Sec_Ins_Company_Phonenumber from pat as p WITH(NOLOCK) "
                                                                + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 2) as SecIns on SecIns.PatientEHRId = PA.pid "
                                                                + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MAX(Appt.adate) as LastVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null AND Appt.adate <= CONVERT(DATE, GETDATE()) Group by Appt.apid ) AS LVisit on LVisit.Patient_EHR_Id =  PA.pid  "
                                                                + " LEFT JOIN (select PP.pid,CONVERT( varchar, ( case when PP.paptinvl > 12 then (DATEADD(Day, CAST(PP.paptinvl AS INT),  AA.DueDate)) else ((DATEADD(Month, CAST(PP.paptinvl AS INT),  AA.DueDate)))END)) +'@'+ CONVERT( varchar,  rl.msgmessage) + '@' + CAST(rl.msgid AS varchar) AS DueDate   from pat PP WITH(NOLOCK) "
                                                                + " INNER JOIN (SELECT apid, Max(ap1.adate) AS DueDate FROM  apt ap1 WITH(NOLOCK) Where aconfstat = 'D' AND astatus  != 'R' GROUP BY apid) AS AA ON PP.pid = AA.apid "
                                                                    + " LEFT JOIN msg rl WITH(NOLOCK) on rl.msgid = PP.paptinvl where rl.msgid != '' and rl.msgid != 0 and rl.msgtype = 'PATAI' and ISNUMERIC(rl.msgid) = 1 and rl.msgid > 0) as AB on AB.pid = PA.pid "
                                                                + " LEFT JOIN (Select Trn.tpid, "
                                                                + " SUM(CASE WHEN Trn.tdate <= CONVERT(DATE, GETDATE()) AND Trn.tdate >= DATEADD(day, -29, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg)/100.00) ELSE 0 END) - 0 As Pat_AR30, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -30, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -59, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR60, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -60, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -89, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -90, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - MIN(TT.PatientPayment) As Pat_AR90_Plus, "
                                                                + " SUM((Trn.tamount - Trn.tinschg) / 100.00) - MIN(TT.PatientPayment)  as Pat_Total "
                                                                + " FROM trn Trn "
                                                                + " JOIN Pat Pat On Pat.Pid = Trn.tpid "
                                                                + " LEFT JOIN(Select Trn.tpid as PatientId, "
                                                                + " SUM(Case When Trn.tptype != 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as PatientPayment, "
                                                                + " SUM(Case When Trn.tptype  = 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as InsPayment "
                                                                + " from trn Trn "
                                                                + " GROUP BY Trn.tpid) as TT on TT.PatientId = Trn.tpid "
                                                                + " where Pat.pInactive = 0 and Trn.tprepay != 1"
                                                                + " Group by Trn.tpid "
                                                                + " HAVING SUM((Trn.tamount - Trn.tinschg)/100.00) - MIN(TT.PatientPayment) > 0 ) as AG on AG.tpid = PA.pid "
                                                                + " WHERE PA.pid = @Patient_EHR_ID and PA.pid <> ''  AND PA.plname<> '' and ((PA.pfname <> '' OR PA.plname <> '') AND (AP.adate >  @ToDate))";

        public static string AbelDentAppointment_Procedures_Data = "  SELECT appt.apid as Patient_EHR_ID, LOWER(appt.aidentifier) as Appointment_ID, stuff(( SELECT  distinct ',' + AD.ijcode FROM apt Plog WITH(NOLOCK) LEFT JOIN  tdi AD WITH(NOLOCK) ON Plog.apid = AD.ipid WHERE Plog.adate > getdate() and Plog.apid = appt.apid and Plog.adate = appt.adate for xml path('')),1,1,'') as ProcedureCode , "
                                                                + "   SUBSTRING(stuff((SELECT distinct ',' + ss.jdesc1 FROM apt Plog WITH(NOLOCK) LEFT JOIN  tdi AD ON Plog.apid = AD.ipid LEFT JOIN jcf SS ON AD.ijcode = SS.jcode WHERE Plog.adate > getdate() and Plog.apid = appt.apid and Plog.adate = appt.adate for xml path('')),1,1,''),0,3999) as ProcedureDesc  "
                                                                + "   FROM apt appt WITH(NOLOCK)"
                                                                + "   WHERE appt.adate > getdate() and appt.apid <> 0 and appt.aidentifier is Not null"
                                                                + "   GROUP BY appt.adate, appt.apid, appt.aidentifier";

        public static string AbelDentAppointment_Procedures_DataByApptID = "  SELECT appt.apid as Patient_EHR_ID, LOWER(appt.aidentifier) as Appointment_ID, stuff(( SELECT  distinct ',' + AD.ijcode FROM apt Plog WITH(NOLOCK) LEFT JOIN  tdi AD WITH(NOLOCK) ON Plog.apid = AD.ipid WHERE Plog.adate > getdate() and Plog.apid = appt.apid and Plog.adate = appt.adate for xml path('')),1,1,'') as ProcedureCode , "
                                                        + "   stuff((SELECT distinct ',' + ss.jdesc1 FROM apt Plog WITH(NOLOCK) LEFT JOIN  tdi AD ON Plog.apid = AD.ipid LEFT JOIN jcf SS ON AD.ijcode = SS.jcode WHERE Plog.adate > getdate() and Plog.apid = appt.apid and Plog.adate = appt.adate for xml path('')),1,1,'') as ProcedureDesc "
                                                        + "   FROM apt appt WITH(NOLOCK)"
                                                        + "   WHERE appt.aidentifier = @Appt_EHR_ID and appt.adate > getdate() and appt.apid <> 0 and appt.aidentifier is Not null"
                                                        + "   GROUP BY appt.adate, appt.apid, appt.aidentifier";

        public static string InsertAppointment_APN_Details = " INSERT INTO apn ( apnpid,apndate,apncol,apntime,apntype,apnline,apnsubtype,apnentrydate,apnentrytime,apninit,apnnote,apnloggedinuser,apndisplayflag,apnreason)"
                                                         + "VALUES(@PatientID, @ApptDate, @Operatory, @ApptTime, @ApptType,0,'', @ApptDateNow, @ApptTimeNow, @Note,'', ISNULL(( SELECT TOP 1 apnloggedinuser FROM apn WITH(NOLOCK))''), NULL)";

        public static string InsertAppointmentDetails = "  INSERT INTO apt ( apid,adate,achair,atime,astatus,atimereq,apxfee,apwork,apwrk2,adid,apallocate,aconfstat,alabstat,ashortstat,afuture,Expire,aduedate,aidentifier,ArrivalTime,EndOfWaitTime,EndOfAppointmentTime,UpdateInfo) "
                                                         + " VALUES(@PatientID, @ApptDate, '@OperatoryId', @ApptTime,'A',@ReqTime,0,'@TreatmentCode','', '@ProviderId', @ReqTime, '', '', '', '', '', GETDATE(),'@UniqID', '', '', '',0)";



        public static string UpdateAppointmentStatusFromWeb = "UPDATE apt SET aconfstat = '@Status' WHERE aidentifier = '@AppointmentId' ";

        public static string getAppointmentDateTimeWise = " Select Ap.aidentifier as Appointment_EHR_Id,0 AS Clinic_Number,1 as Service_Install_Id,Ap.achair AS location_id, "
                                                            + " CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS start_Time, "
                                                            + " CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + DATEADD(MINUTE, AP.atimereq * @_minutesInUnit, CONVERT(CHAR(8), AP.atime, 108))) as End_Time, "
                                                            + " Pa.pfname as FirstName,Pa.plname as LastName,Inf.infmobile AS Mobile,Inf.infemail As Email, "
                                                            + " Pd.dname AS ProviderFirstName,'' AS ProviderLastName from apt as Ap "
                                                            + " left join pat Pa on Pa.pid = Ap.apid "
                                                            + " left join inf Inf on Inf.infpid = Pa.pid "
                                                            + " LEFT JOIN dnt PD  ON PD.did = AP.achair "
                                                            + " where Ap.astatus != 'R' and CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) >= @DateTime AND "
                                                            + " CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + DATEADD(MINUTE, AP.atimereq * @_minutesInUnit, CONVERT(CHAR(8), AP.atime, 108))) >= @DateTime ";


        #endregion
        #region ApptStatus
        public static string GetAbelDentApptStatusData = "Select 0 AS Clinic_Number,1 as Service_Install_Id,'' AS ApptStatus_LocalDB_ID,(CASE WHEN apsid = '' THEN 'bl' WHEN apsid = '\' then 'ss' WHEN apsid = '\\' then 'ds' ELSE apsid END) as ApptStatus_EHR_ID,'normal' as ApptStatus_Type,apsdesc as ApptStatus_Name,0 AS Is_Adit_Updated from aps WITH(NOLOCK) order by apsorder asc";
        #endregion
        #region Holiday        
        public static string GetAbelDentHolidayData = "SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS H_LocalDB_ID,IX.ixipid AS H_EHR_ID,'' AS H_Web_ID,HD.nsjclass AS H_Operatory_EHR_ID,'' AS SchedDate, HD.onote AS comment,GETDATE() AS Entry_DateTime,0 AS Is_Adit_Updated FROM ixi IX WITH(NOLOCK) left join nsj as HD WITH(NOLOCK) on HD.onid = IX.ixiplanid WHERE HD.onid = 'HOLIDAY'";
        #endregion
        #region Patient Medication
        public static string GetAbelDentPatientMedicationData = "select ClinicalPatient.LegacyPID as Patient_EHR_ID, "
                                                                                    + " ClinicalEntity.Code as Medication_EHR_ID, "
                                                                                    + " ClinicalAct.ID as PatientMedication_EHR_ID, "
                                                                                    + " ClinicalConcept.SummaryDescription as Medication_Name, "
                                                                                    + " ClinicalEntity.ImplementingClass as Medication_Type, "
                                                                                    + " ClinicalAct.Note as Medication_Note, "
                                                                                    + " ClinicalSubstanceAdministration.DoseQuantityValue as Drug_Quantity, "
                                                                                    + " '' as Provider_EHR_ID, "
                                                                                    + " ISNULL(ClinicalSubstanceAdministration.DrugStartDate,'') as Start_Date, "
                                                                                    + " '' as End_Date, "
                                                                                    + " ISNULL(ClinicalSubstanceAdministration.WrittenDate,'') AS EHR_Entry_DateTime, "
                                                                                    + " '' as Patient_Notes, "
                                                                                    + " 0 AS is_deleted, "
                                                                                    + " '0' as Clinic_Number, "
                                                                                    + "  0 As Is_Active, "
                                                                                    + " ISNULL(ClinicalAct.StatusCode, '') from ClinicalAct "
                                                                                    + " left join ClinicalParticipation on ClinicalParticipation.ActID = ClinicalAct.ID "
                                                                                    + " Left join ClinicalConcept on ClinicalConcept.ID = ClinicalAct.MainConceptID "
                                                                                    + " Left join ClinicalEntity on ClinicalEntity.Code = ClinicalAct.MainConceptID "
                                                                                    + " left join ClinicalSubstanceAdministration on ClinicalSubstanceAdministration.ID = ClinicalAct.ID "
                                                                                    + " Inner Join ClinicalPatient on  ClinicalParticipation.RoleID = ClinicalPatient.ID "
                                                                                    + " Where ClinicalAct.ClassCode = 'sbadm'";

        public static string GetAbelDentMedicationMaster = " Select ClinicalEntity.Code as Medication_EHR_ID, ClinicalConcept.SummaryDescription as Medication_Name,  '' as Medication_Description,   '' as Medication_Notes,  '' as Medication_Sig, '' as Medication_Parent_EHR_ID,  ClinicalEntity.ImplementingClass as Medication_Type,  '' as Allow_Generic_Sub, ISNULL(CONVERT(NVARCHAR, UserDefinedDrug.StrengthValue),'') + ' ' + ISNULL(CONVERT(NVARCHAR, UserDefinedDrug.StrengthUnits),'') as   Drug_Quantity, '' as Refills, 'True' as Is_Active,  "
        + " getdate() as EHR_Entry_DateTime,'' as Medication_Provider_ID,  0 as is_deleted,  0 as Is_Adit_Updated,  '0' as Clinic_Number "
        + " from  ClinicalConcept "
        + " Left join ClinicalEntity on ClinicalEntity.Code = ClinicalConcept.ID "
        + " Left join UserDefinedDrug on UserDefinedDrug.ID = ClinicalEntity.ID "
        + " Where ClinicalEntity.ImplementingClass = 'UserDefinedDrug' ";

        #region Insert Medication Data

        public static string GetMedicationConceptGuid_Medication_1 = "select ID from ClinicalConcept where ConceptIdentifier = '@MedicationNameINCAP' ";

        public static string InsertAbelDentClinicalAct_Medication_2 = "INSERT INTO dbo.[ClinicalAct] ([ID], [TimeBegun], [TimeEnded], [MainConceptID], [Note], [ImplementingClass], [EffectiveTimeBegun], [EffectiveTimeEnded], [MoodCode], [AvailabilityTime], [SourceRoleID], [ClassCode], [Duration], [DurationUnits], [Frequency], [FrequencyUnits] ) VALUES (@GuidId,GETDATE(),NULL,@ConceptGuID, @PatientNote,'Prescription','@DateTime','@EffTimeEnd','o',GETDATE(),@ProviderUniqID,'sbadm',3,'d',0,'d')";

        public static string GetPatientUniqIDfor_Medication_3 = "select ID from ClinicalPatient where LTRIM(RTRIM(LegacyPID)) = LTRIM(RTRIM('@PatientId'))";

        public static string InsertAbelDentClinicalParticipation_Medication_4 = "Insert into ClinicalParticipation (ID,ActID,RoleID,TypeCode) Values (newid(),@ActID,@PatientGuid,'sbj')";

        public static string InsertAbeldentClinicalParticipation_Medication_4_1 = "INSERT INTO dbo.[ClinicalParticipation] ([ID], [ActID], [RoleID], [TypeCode] ) VALUES (newid(), @ActID, @ConceptGuID, 'tpa')";

        public static string InsertAbeldentClinicalParticipation_Medication_4_2 = "INSERT INTO dbo.[ClinicalParticipation] ([ID], [ActID], [RoleID], [TypeCode] ) VALUES (newid(),@ActID, @ProviderUniqID, 'aut' )";

        public static string GetAbelDentClinicalProviderUniqID = "select ID from ClinicalProvider where LegacyProviderID = '?'";

        public static string SelectAbelDentUserDefinedDrugData_Medication_5 = "Select ClinicalEntity.Code as MedicationEHRID,UserDefinedDrug.StrengthValue as DoseQuantityValue, UserDefinedDrug.StrengthUnits as DoseQuantityUnits from UserDefinedDrug left join ClinicalEntity on UserDefinedDrug.ID = ClinicalEntity.ID where ClinicalEntity.ClassCode = 'cdrug' and ClinicalEntity.Description = @Medication_Name ";

        public static string InsertAbelDentClinicalSubstanceAdministration_Medication_6 = "Insert into ClinicalSubstanceAdministration (ID,RouteCode,DoseQuantityValue,DoseQuantityUnits,TransmissionMethod,WrittenDate,Repeats,TotalQuantity,LongTerm,SubstitutionCode,FormCode) "
        + " VALUES (@ActId,'PO', @DoseQunValue, @DoseQunUnits,NULL, '@DateTime', 0,0,0,'GE','CA') ";

        #endregion

        #endregion
        #region Operatory
        public static string GetAbelDentOperatoryData = "SELECT 0 AS Clinic_Number,1 as Service_Install_Id,'' as Operatory_LocalDB_ID,substring(CAST(l.snum as varchar(10)),4,10) as Operatory_EHR_ID,l.scurrmess as Operatory_Name,0 AS Is_Adit_Updated,'False' as is_deleted from sys  as l WITH(NOLOCK) where l.snum like 'COL%' order by l.snum asc";
        //public static string GetAbelDentOperatoryData = "SELECT DISTINCT 0 AS Clinic_Number,1 as Service_Install_Id,'' as Operatory_LocalDB_ID,did AS Operatory_EHR_ID,dname AS Operatory_Name,0 AS Is_Adit_Updated from dnt";

        public static string GetOperatoryDateTimeWise = "     Select AP.apid AS Appointment_EHR_Id, 0 AS Clinic_Number,1 as Service_Install_Id,apallocate AS location_id,"
                                                          + " CAST(adate as DATETIME) + CAST(atime as DATETIME) AS start_Time,DATEADD(HOUR,DATEPART(HOUR, CAST(AN.apnentrydate as DATETIME) + CAST(AN.apnentrytime as DATETIME) ),DATEADD(MINUTE,DATEPART(MINUTE, CAST(AP.adate as DATETIME) + CAST(AP.atime as DATETIME) ),DATEADD(SECOND, DATEPART(SECOND, CAST(AN.apnentrydate as DATETIME) + CAST(AN.apnentrytime as DATETIME) ),CAST(AP.adate as DATETIME) + CAST(AP.atime as DATETIME)))) AS End_Time,P.pfname,P.plname,P.pphone AS Mobile,AP.adid AS ProviderFirstName,'' AS ProviderLastName FROM apt AP WITH(NOLOCK)"
                                                          + " LEFT JOIN apn AN WITH(NOLOCK) ON AN.apnpid = AP.apid"
                                                          + " LEFT JOIN pat P WITH(NOLOCK) ON P.pid = AP.apid where CAST(adate as DATETIME) + CAST(atime as DATETIME) >= GETDATE() and DATEADD(HOUR, DATEPART(HOUR, CAST(AN.apnentrydate as DATETIME) + CAST(AN.apnentrytime as DATETIME) ),DATEADD(MINUTE, DATEPART(MINUTE, CAST(AP.adate as DATETIME) + CAST(AP.atime as DATETIME) ),DATEADD(SECOND, DATEPART(SECOND, CAST(AN.apnentrydate as DATETIME) + CAST(AN.apnentrytime as DATETIME) ),CAST(AP.adate as DATETIME) + CAST(AP.atime as DATETIME)))) >= GETDATE() and AP.astatus = 'A'";
        #endregion
        #region OperatoryOfficeHours
        public static string GetOperatoryOfficeHours = "select 0 AS Clinic_Number,1 as Service_Install_Id,0 as OOH_LocalDB_ID,1 as OOH_EHR_ID ,'' as OOH_Web_ID,'0' as Provider_EHR_ID, 'Sunday' as [WeekDay],             sdaybegins as StartTime1 ,  sdayend as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 1, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as OOH_LocalDB_ID,2 as OOH_EHR_ID ,'' as OOH_Web_ID,'0' as Provider_EHR_ID, 'Monday' as [WeekDay],    sdaybegins  as StartTime1 ,  sdayend as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 2, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as OOH_LocalDB_ID,3 as OOH_EHR_ID ,'' as OOH_Web_ID,'0' as Provider_EHR_ID, 'Tuesday' as [WeekDay],   sdaybegins  as StartTime1 ,  sdayend as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 3, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as OOH_LocalDB_ID,4 as OOH_EHR_ID ,'' as OOH_Web_ID,'0' as Provider_EHR_ID, 'Wednesday' as [WeekDay], sdaybegins  as StartTime1 ,  sdayend as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 4, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as OOH_LocalDB_ID,5 as OOH_EHR_ID ,'' as OOH_Web_ID,'0' as Provider_EHR_ID, 'Thursday' as [WeekDay],  sdaybegins  as StartTime1 ,  sdayend as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 5, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as OOH_LocalDB_ID,6 as OOH_EHR_ID ,'' as OOH_Web_ID,'0' as Provider_EHR_ID, 'Friday' as [WeekDay],    sdaybegins  as StartTime1 ,  sdayend as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 6, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as OOH_LocalDB_ID,7 as OOH_EHR_ID ,'' as OOH_Web_ID,'0' as Provider_EHR_ID, 'Saturday' as [WeekDay],  sdaybegins   as StartTime1 , sdayend  as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 7, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' '	";

        public static string GetOperatoryOfficeHoursOP = "Select substring(CAST(snum as varchar(10)),4,10) as Operatory_EHR_ID from sys where snum like 'COL%' and s30daymess != ''";

        #endregion

        #region ProviderOfficeHours
        public static string GetProviderOfficeHours = "select 0 AS Clinic_Number,1 as Service_Install_Id,0 as POH_LocalDB_ID,1 as POH_EHR_ID ,'' as POH_Web_ID,'0' as Provider_EHR_ID, 'Sunday' as [WeekDay],             sdaybegins  as StartTime1 , sdayend  as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 1, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as POH_LocalDB_ID,2 as POH_EHR_ID ,'' as POH_Web_ID,'0' as Provider_EHR_ID, 'Monday' as [WeekDay],   sdaybegins  as StartTime1 , sdayend  as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 2, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as POH_LocalDB_ID,3 as POH_EHR_ID ,'' as POH_Web_ID,'0' as Provider_EHR_ID, 'Tuesday' as [WeekDay],  sdaybegins  as StartTime1 , sdayend  as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 3, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as POH_LocalDB_ID,4 as POH_EHR_ID ,'' as POH_Web_ID,'0' as Provider_EHR_ID, 'Wednesday' as [WeekDay],sdaybegins  as StartTime1 , sdayend  as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 4, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as POH_LocalDB_ID,5 as POH_EHR_ID ,'' as POH_Web_ID,'0' as Provider_EHR_ID, 'Thursday' as [WeekDay], sdaybegins  as StartTime1 , sdayend  as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 5, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as POH_LocalDB_ID,6 as POH_EHR_ID ,'' as POH_Web_ID,'0' as Provider_EHR_ID, 'Friday' as [WeekDay],   sdaybegins  as StartTime1 , sdayend  as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 6, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' ' "
+ " UNION select 0 AS Clinic_Number,1 as Service_Install_Id,0 as POH_LocalDB_ID,7 as POH_EHR_ID ,'' as POH_Web_ID,'0' as Provider_EHR_ID, 'Saturday' as [WeekDay], sdaybegins  as StartTime1 , sdayend  as EndTime1 ,getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE WHEN SUBSTRING(sworkdays, 7, 1) = 'Y' THEN 0 ELSE 1 END as is_deleted, 0 as Is_Adit_Updated from sys where snum = ' '	";

        public static string GetProviderOfficeHoursOP = "Select s30daymess as Provider_EHR_ID from sys where snum like 'COL%' and s30daymess != ''";

        #endregion
        #region Speciality
        public static string GetAbelDentSpecialty = " SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Speciality_LocalDB_ID,'' AS Speciality_Web_ID,msgid AS Speciality_EHR_ID,msgmessage AS Speciality_Name,getdate() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated FROM msg as l WITH(NOLOCK) where l.msgtype like 'SPCTY%' order by l.msgid asc ";
        #endregion
        #region Patient
        public static string GetAbelDentPatientData = " Select DISTINCT PA.pid AS Patient_EHR_ID,'' AS Patient_Web_ID,PA.pfname AS First_name,PA.plname AS Last_name, "
                                                            + " PA.pinitial AS Middle_Name,PA.pmrmrs AS Salutation,case when PA.pinactive = 0 then 'A' else 'I' end AS [Status], (case WHEN PA.pgender = 'M' THEN 'MALE' WHEN PA.pgender = 'F' THEN 'FEMALE' WHEN PA.pgender = '' THEN '' END ) as SEX,'' AS MaritalStatus,'' AS school, "
                                                            + " (CASE When PA.pbirth IS NULL then CONVERT(datetime,'17530101',112) else PA.pbirth end) AS Birth_Date, AA.infemail as Email, ISNULL(AA.infmobile,'') AS Mobile, ISNULL(PA.pphone, '') AS Home_Phone, ISNULL(PA.pworkphn, '') AS Work_Phone, "
                                                            + " ISNULL(PA.pstreetadr, '') AS Address1, ISNULL(PA.pstreetadr2, '') AS Address2, CT.cdesc AS[City],'' AS[State],ISNULL(PA.ppostal,'') AS Zipcode, "
                                                            + " (case WHEN P2.pinactive is NULL then '' when P2.pinactive = 1 then '' ELSE 'Active' end) AS ResponsibleParty_Status,ISNULL(AG.Pat_AR30,0) AS ThirtyDay,ISNULL(AG.Pat_AR60,0) AS SixtyDay,ISNULL(AG.Pat_AR90,0) AS NinetyDay,ISNULL(AG.Pat_AR90_Plus,0) AS Over90,ISNULL(AG.Pat_Total,0) AS CurrentBal,ISNULL(HH.RemainingAmount,0) - ISNULL(ISB.totalCoverageUsed,0) as remaining_benefit,ISNULL(ISB.totalCoverageUsed,0) as used_benefit, PA.paltid AS ssn,AA.infemployer as employer, "
                                                            + " FVisit.FirstVisitDate AS FirstVisit_Date,ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance, ISNULL(PrimIns.PrimaryInsuranceCompanyName, '') AS Primary_Insurance_CompanyName, PrimIns.PrimarySubcriberId as Primary_Ins_Subscriber_ID, ISNULL(PrimIns.Prim_Ins_Company_Phonenumber,'') as Prim_Ins_Company_Phonenumber,ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance,ISNULL(SecIns.SecondaryInsuranceCompanyName,'') AS Secondary_Insurance_CompanyName,ISNULL(SecIns.Sec_Ins_Company_Phonenumber,'') as Sec_Ins_Company_Phonenumber, ( case WHEN PA.pinactive = 1 then 'InActive' ELSE 'Active' end ) AS EHR_Status, 0 AS Is_Deleted, "
                                                            + " (CASE WHEN PA.pchargeto = 0 then PA.pid ELSE PA.pchargeto END) AS Guar_ID,SecIns.SecondarySubcriberId as Secondary_Ins_Subscriber_ID, ISNULL(PA.pdentist,'') AS Pri_Provider_ID, ISNULL(PA.phygienist, '') AS Sec_Provider_ID, (CASE WHEN AA.infallowtextmsg = 1 then 'Y' else 'N' end) AS ReceiveSms, (CASE WHEN AA.infallownewsemail = 1 then 'Y' else 'N' end) AS ReceiveEmail, "
                                                            + " LVisit.LastVisitDate AS LastVisit_Date, (SELECT MIN (adate) From apt WITH(NOLOCK) Where adate > GETDATE() AND astatus NOT IN ('R') and apid = PA.pid) AS nextvisit_date, "
                                                            + " AB.DueDate AS due_date,ISNULL(AG.Collect_total,0) AS collect_payment, '' AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated, AA.infgivenname AS preferred_name, "
                                                            + " 1 As InsUptDlt,'' as ReceiveVoiceCall,CASE WHEN PA.pnativetongue = 'E' THEN 'English' WHEN PA.pnativetongue = 'F' THEN 'French' WHEN PA.pnativetongue = '' THEN 'ENGLISH' END AS PreferredLanguage, AA.infnote AS Patient_note, "
                                                            + " P2.pid AS responsiblepartyid, P2.pfname AS ResponsibleParty_First_Name, P2.plname AS ResponsibleParty_Last_Name,'' AS responsiblepartyssn, P2.pbirth AS responsiblepartybirthdate,'' AS EmergencyContactId,'' AS EmergencyContact_First_Name, "
                                                            + " '' AS EmergencyContact_Last_Name,'' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' AS driverlicense, "
                                                            + " '' AS groupid,0 as Clinic_Number, 1 as Service_Install_Id,0 as Patient_LocalDB_ID from pat PA WITH(NOLOCK)"
                                                            + " LEFT JOIN inf AA  WITH(NOLOCK) on AA.infpid = PA.pid "
                                                            + " LEFT JOIN  pat P2 WITH(NOLOCK) on PA.pchargeto = P2.pid "
                                                            + " LEFT JOIN  cty CT WITH(NOLOCK) on CT.ccode = PA.pcitycode "                                                                                                                                                                                                
                                                            + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , CAST(NS.inscoid AS varchar) as PrimaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as PrimarySubcriberId, NS.insname as PrimaryInsuranceCompanyName, NS.insphone as Prim_Ins_Company_Phonenumber from pat as p WITH(NOLOCK)  "
                                                            + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                            + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 1) as PrimIns on PrimIns.PatientEHRId = PA.pid "
                                                            + " LEFT JOIN(select DISTINCT p.pid as PatientEHRId , NS.inscoid as SecondaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as SecondarySubcriberId, NS.insname as SecondaryInsuranceCompanyName , NS.insphone as Sec_Ins_Company_Phonenumber from pat as p WITH(NOLOCK) "
                                                            + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                            + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 2) as SecIns on SecIns.PatientEHRId = PA.pid "
                                                            + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MAX(Appt.adate) as LastVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null AND Appt.adate <= CONVERT(DATE, GETDATE()) Group by Appt.apid ) AS LVisit on LVisit.Patient_EHR_Id =  PA.pid  "
                                                            + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MIN(Appt.adate) as FirstVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null Group by Appt.apid) AS FVisit on FVisit.Patient_EHR_Id =  PA.pid  "
                                                            + " LEFT JOIN (select PP.pid,CONVERT( varchar, ( case when PP.paptinvl > 12 then (DATEADD(Day, CAST(PP.paptinvl AS INT),  AA.DueDate)) else ((DATEADD(Month, CAST(PP.paptinvl AS INT),  AA.DueDate)))END)) +'@'+ CONVERT( varchar,  rl.msgmessage) + '@' + CAST(rl.msgidentifier AS varchar) AS DueDate   from pat PP WITH(NOLOCK) "
                                                                + " INNER JOIN (SELECT apid, Max(ap1.adate) AS DueDate FROM  apt ap1 WITH(NOLOCK) Where aconfstat = 'D' AND astatus  != 'R' GROUP BY apid) AS AA ON PP.pid = AA.apid " 
                                                                    + " LEFT JOIN msg rl WITH(NOLOCK) on rl.msgid = PP.paptinvl where rl.msgid != '' and rl.msgid != 0 and rl.msgtype = 'PATAI' and ISNUMERIC(rl.msgid) = 1 and rl.msgid > 0) as AB on AB.pid = PA.pid "
                                                            + " LEFT JOIN (Select Trn.tpid, "
                                                                + " SUM(CASE WHEN Trn.tdate <= CONVERT(DATE, GETDATE()) AND Trn.tdate >= DATEADD(day, -29, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg)/100.00) ELSE 0 END) - MIN(TT.PatientPayment) As Pat_AR30, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -30, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -59, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR60, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -60, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -89, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -90, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90_Plus, "
                                                                + " SUM((Trn.tamount - Trn.tinschg) / 100.00) - MIN(TT.PatientPayment)  as Pat_Total,MIN(TT.PatientPayment) as Collect_total "
                                                                + " FROM trn Trn "
                                                                + " JOIN Pat Pat On Pat.Pid = Trn.tpid "
                                                                + " LEFT JOIN(Select Trn.tpid as PatientId, "
                                                                + " SUM(Case When Trn.tptype != 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as PatientPayment, "
                                                                + " SUM(Case When Trn.tptype  = 'R' THEN Trn.tinschg/100.00 ELSE 0 END) as InsPayment "
                                                                + " from trn Trn Where Trn.tprepay != 1  "
                                                                + " GROUP BY Trn.tpid) as TT on TT.PatientId = Trn.tpid "
                                                                + " where Pat.pInactive = 0 and Trn.tprepay != 1"
                                                                + " Group by Trn.tpid "
                                                                + " HAVING SUM((Trn.tamount - Trn.tinschg)/100.00) - MIN(TT.PatientPayment) > 0 ) as AG on AG.tpid = PA.pid "
                                                                + " LEFT JOIN (Select i1.ixipid, i1.ixiplno, COALESCE(i2.ixiplanid,i1.ixiplanid) as ixiplanid, COALESCE(i2.ixicertno, i1.ixicertno) as ixicertno, "
                                                                        + " COALESCE(i2.ixiassigned, i1.ixiassigned) as ixiassigned, CASE WHEN i1.ixisubpid != 0 THEN i1.ixisubpid ELSE i1.ixipid END as ixisubpid, i1.ixireltosub, "
                                                                        + " COALESCE(i2.ixigroupno, i1.ixigroupno) as ixigroupno, COALESCE(i2.ixidivsect, i1.ixidivsect) as ixidivsect, "
                                                                        + " COALESCE(i2.ixisubuse, i1.ixisubuse) as ixisubuse, COALESCE(i2.ixiReleaseOfInfo, i1.ixiReleaseOfInfo) as ixiReleaseOfInfo, "
                                                                        + " NS.inscoid, Ns.InsName, NS.insphone, NS.inscarrierid, "
                                                                        + " nsp.ninscoid, nsp.nspdeductibles, nsp.nspmaximums, nsp.nassigned, nsp.nsppreventive, nsp.nspbasic, nsp.nspmajor, nsp.nsportho, nsp.nspplantype, "
                                                                        + " nsp.nanniv, nsp.nplanname, nsp.nsppercentagelimit, nsp.nfeeyear, nsp.nchargesched, nsp.ncopaysched, nsp.nspexclusive "
                                                                        + " from ixi as i1 "
                                                                        + " LEFT JOIN ixi as i2 on i1.ixisubpid = i2.ixipid AND i2.ixiplno = i1.ixisubuse "
                                                                        + " join nsp Nsp on nsp.nid = COALESCE(i2.ixiplanid, i1.ixiplanid) "
                                                                        + " JOIN ins as NS WITH(NOLOCK) on NS.inscoid = Nsp.ninscoid where i1.ixiplno = 1) as INF on INF.ixipid = PA.pid "
                                                                        + "           LEFT JOIN(Select Itn.itnsubscriber, Itn.itnplanid, "
                                                                        + "      SUM(Itn.itndedamount/100.00) as deductionPaid, "
                                                                        + "	   SUM(Itn.itnbenefit/100.00) as totalCoverageUsed "
                                                                        + " from Itn "
                                                                        + " group by Itn.itnsubscriber, Itn.itnplanid) as ISB on ISB.itnsubscriber = INF.ixisubpid and ISB.itnplanid = INF.ixiplanid "
                                                                + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , case when nspdeductibles = '' then LEFT(SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000) + 'X') -1) else LEFT(SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000) + 'X') -1) end as RemainingAmount from  pat as p WITH(NOLOCK) "
                                                                        + " Inner JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                        + " Inner Join nsp as NP with(NOLOCK) on NP.nid = XI.ixiplanid "
                                                                + " where NP.nspmaximums != '' or NP.nspdeductibles != '') as HH on HH.PatientEHRId = PA.pid ";

        public static string GetAbelDentPatientDataWithCondition = " Select DISTINCT PA.pid AS Patient_EHR_ID,'' AS Patient_Web_ID,PA.pfname AS First_name,PA.plname AS Last_name, "
                                                            + " PA.pinitial AS Middle_Name,PA.pmrmrs AS Salutation,case when PA.pinactive = 0 then 'A' else 'I' end AS [Status], (case WHEN PA.pgender = 'M' THEN 'MALE' WHEN PA.pgender = 'F' THEN 'FEMALE' WHEN PA.pgender = '' THEN '' END ) as SEX,'' AS MaritalStatus,'' AS school, "
                                                            + " (CASE When PA.pbirth IS NULL then CONVERT(datetime,'17530101',112) else PA.pbirth end) AS Birth_Date, AA.infemail as Email, ISNULL(AA.infmobile,'') AS Mobile, ISNULL(PA.pphone, '') AS Home_Phone, ISNULL(PA.pworkphn, '') AS Work_Phone, "
                                                            + " ISNULL(PA.pstreetadr, '') AS Address1, ISNULL(PA.pstreetadr2, '') AS Address2, CT.cdesc AS[City],'' AS[State],ISNULL(PA.ppostal,'') AS Zipcode, "
                                                            + " (case WHEN P2.pinactive is NULL then '' when P2.pinactive = 1 then '' ELSE 'Active' end) AS ResponsibleParty_Status,ISNULL(AG.Pat_AR30,0) AS ThirtyDay,ISNULL(AG.Pat_AR60,0) AS SixtyDay,ISNULL(AG.Pat_AR90,0) AS NinetyDay,ISNULL(AG.Pat_AR90_Plus,0) AS Over90,ISNULL(AG.Pat_Total,0) AS CurrentBal,ISNULL(HH.RemainingAmount,0) - ISNULL(ISB.totalCoverageUsed,0) as remaining_benefit,ISNULL(ISB.totalCoverageUsed,0) as used_benefit, PA.paltid AS ssn,AA.infemployer as employer, "
                                                            + " FVisit.FirstVisitDate AS FirstVisit_Date,ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance, ISNULL(PrimIns.PrimaryInsuranceCompanyName, '') AS Primary_Insurance_CompanyName, PrimIns.PrimarySubcriberId as Primary_Ins_Subscriber_ID, ISNULL(PrimIns.Prim_Ins_Company_Phonenumber,'') as Prim_Ins_Company_Phonenumber,ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance,ISNULL(SecIns.SecondaryInsuranceCompanyName,'') AS Secondary_Insurance_CompanyName,ISNULL(SecIns.Sec_Ins_Company_Phonenumber,'') as Sec_Ins_Company_Phonenumber, ( case WHEN PA.pinactive = 1 then 'InActive' ELSE 'Active' end ) AS EHR_Status, 0 AS Is_Deleted, "
                                                            + " (CASE WHEN PA.pchargeto = 0 then PA.pid ELSE PA.pchargeto END) AS Guar_ID,SecIns.SecondarySubcriberId as Secondary_Ins_Subscriber_ID, ISNULL(PA.pdentist,'') AS Pri_Provider_ID, ISNULL(PA.phygienist, '') AS Sec_Provider_ID, (CASE WHEN AA.infallowtextmsg = 1 then 'Y' else 'N' end) AS ReceiveSms, (CASE WHEN AA.infallownewsemail = 1 then 'Y' else 'N' end) AS ReceiveEmail, "
                                                            + " LVisit.LastVisitDate AS LastVisit_Date, (SELECT MIN (adate) From apt WITH(NOLOCK) Where adate > GETDATE() AND astatus NOT IN ('R') and apid = PA.pid) AS nextvisit_date, "
                                                            + " AB.DueDate AS due_date,ISNULL(AG.Collect_total,0) AS collect_payment, '' AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated, AA.infgivenname AS preferred_name, "
                                                            + " 1 As InsUptDlt,'' as ReceiveVoiceCall,CASE WHEN PA.pnativetongue = 'E' THEN 'English' WHEN PA.pnativetongue = 'F' THEN 'French' WHEN PA.pnativetongue = '' THEN 'ENGLISH' END AS PreferredLanguage, AA.infnote AS Patient_note, "
                                                            + " P2.pid AS responsiblepartyid, P2.pfname AS ResponsibleParty_First_Name, P2.plname AS ResponsibleParty_Last_Name,'' AS responsiblepartyssn, P2.pbirth AS responsiblepartybirthdate,'' AS EmergencyContactId,'' AS EmergencyContact_First_Name, "
                                                            + " '' AS EmergencyContact_Last_Name,'' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' AS driverlicense, "
                                                            + " '' AS groupid,0 as Clinic_Number, 1 as Service_Install_Id,0 as Patient_LocalDB_ID from pat PA WITH(NOLOCK)"
                                                            + " LEFT JOIN inf AA  WITH(NOLOCK) on AA.infpid = PA.pid "
                                                            + " LEFT JOIN  pat P2 WITH(NOLOCK) on PA.pchargeto = P2.pid "
                                                            + " LEFT JOIN  cty CT WITH(NOLOCK) on CT.ccode = PA.pcitycode "
                                                            + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId ,CAST(NS.inscoid AS varchar) as PrimaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as PrimarySubcriberId, NP.nplanname as PrimaryInsuranceCompanyName, NS.insphone as Prim_Ins_Company_Phonenumber from pat as p WITH(NOLOCK) "
                                                            + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                            + " LEFT OUTER JOIN nsp as NP WITH(NOLOCK) on NP.nid = XI.ixiplanid "
                                                            + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = NP.ninscoid where XI.ixiplno = 1) as PrimIns on PrimIns.PatientEHRId = PA.pid "
                                                            + " LEFT JOIN(select DISTINCT p.pid as PatientEHRId , XI.ixiplanid as SecondaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as SecondarySubcriberId, NP.nplanname  as SecondaryInsuranceCompanyName , NS.insphone as Sec_Ins_Company_Phonenumber from pat as p WITH(NOLOCK)"
                                                            + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                            + " LEFT OUTER JOIN nsp as NP WITH(NOLOCK) on NP.nid = XI.ixiplanid "
                                                            + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = NP.ninscoid where XI.ixiplno = 2) as SecIns on SecIns.PatientEHRId = PA.pid"
                                                            + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MAX(Appt.adate) as LastVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null AND Appt.adate <= CONVERT(DATE, GETDATE()) Group by Appt.apid ) AS LVisit on LVisit.Patient_EHR_Id =  PA.pid  "
                                                            + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MIN(Appt.adate) as FirstVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null Group by Appt.apid) AS FVisit on FVisit.Patient_EHR_Id =  PA.pid  "
                                                            + " LEFT JOIN (select PP.pid,CONVERT( varchar, ( case when PP.paptinvl > 12 then (DATEADD(Day, CAST(PP.paptinvl AS INT),  AA.DueDate)) else ((DATEADD(Month, CAST(PP.paptinvl AS INT),  AA.DueDate)))END)) +'@'+ CONVERT( varchar,  rl.msgmessage) + '@' + CAST(rl.msgidentifier AS varchar) AS DueDate   from pat PP WITH(NOLOCK) "
                                                                + " INNER JOIN (SELECT apid, Max(ap1.adate) AS DueDate FROM  apt ap1 WITH(NOLOCK) Where aconfstat = 'D' AND astatus  != 'R' GROUP BY apid) AS AA ON PP.pid = AA.apid "
                                                                    + " LEFT JOIN msg rl WITH(NOLOCK) on rl.msgid = PP.paptinvl where rl.msgid != '' and rl.msgid != 0 and rl.msgtype = 'PATAI' and ISNUMERIC(rl.msgid) = 1 and rl.msgid > 0) as AB on AB.pid = PA.pid "
                                                            + " LEFT JOIN (Select Trn.tpid, "
                                                                + " SUM(CASE WHEN Trn.tdate <= CONVERT(DATE, GETDATE()) AND Trn.tdate >= DATEADD(day, -29, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg)/100.00) ELSE 0 END) - MIN(TT.PatientPayment) As Pat_AR30, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -30, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -59, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR60, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -60, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -89, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -90, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0  As Pat_AR90_Plus, "
                                                                + " SUM((Trn.tamount - Trn.tinschg) / 100.00) - MIN(TT.PatientPayment)  as Pat_Total,MIN(TT.PatientPayment) as Collect_total "
                                                                + " FROM trn Trn "
                                                                + " JOIN Pat Pat On Pat.Pid = Trn.tpid "
                                                                + " LEFT JOIN(Select Trn.tpid as PatientId, "
                                                                + " SUM(Case When Trn.tptype != 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as PatientPayment, "
                                                                + " SUM(Case When Trn.tptype  = 'R' THEN Trn.tinschg/100.00 ELSE 0 END) as InsPayment "
                                                                + " from trn Trn Where Trn.tprepay != 1 "
                                                                + " GROUP BY Trn.tpid) as TT on TT.PatientId = Trn.tpid "
                                                                + " where Pat.pInactive = 0 and Trn.tprepay != 1"
                                                                + " Group by Trn.tpid "
                                                                + " HAVING SUM((Trn.tamount - Trn.tinschg)/100.00) - MIN(TT.PatientPayment) > 0 ) as AG on AG.tpid = PA.pid "
                                                                + " LEFT JOIN (Select i1.ixipid, i1.ixiplno, COALESCE(i2.ixiplanid,i1.ixiplanid) as ixiplanid, COALESCE(i2.ixicertno, i1.ixicertno) as ixicertno, "
                                                                        + " COALESCE(i2.ixiassigned, i1.ixiassigned) as ixiassigned, CASE WHEN i1.ixisubpid != 0 THEN i1.ixisubpid ELSE i1.ixipid END as ixisubpid, i1.ixireltosub, "
                                                                        + " COALESCE(i2.ixigroupno, i1.ixigroupno) as ixigroupno, COALESCE(i2.ixidivsect, i1.ixidivsect) as ixidivsect, "
                                                                        + " COALESCE(i2.ixisubuse, i1.ixisubuse) as ixisubuse, COALESCE(i2.ixiReleaseOfInfo, i1.ixiReleaseOfInfo) as ixiReleaseOfInfo, "
                                                                        + " NS.inscoid, Ns.InsName, NS.insphone, NS.inscarrierid, "
                                                                        + " nsp.ninscoid, nsp.nspdeductibles, nsp.nspmaximums, nsp.nassigned, nsp.nsppreventive, nsp.nspbasic, nsp.nspmajor, nsp.nsportho, nsp.nspplantype, "
                                                                        + " nsp.nanniv, nsp.nplanname, nsp.nsppercentagelimit, nsp.nfeeyear, nsp.nchargesched, nsp.ncopaysched, nsp.nspexclusive "
                                                                        + " from ixi as i1 "
                                                                        + " LEFT JOIN ixi as i2 on i1.ixisubpid = i2.ixipid AND i2.ixiplno = i1.ixisubuse "
                                                                        + " join nsp Nsp on nsp.nid = COALESCE(i2.ixiplanid, i1.ixiplanid) "
                                                                        + " JOIN ins as NS WITH(NOLOCK) on NS.inscoid = Nsp.ninscoid where i1.ixiplno = 1) as INF on INF.ixipid = PA.pid "
                                                                        + "           LEFT JOIN(Select Itn.itnsubscriber, Itn.itnplanid, "
                                                                        + "      SUM(Itn.itndedamount/100.00) as deductionPaid, "
                                                                        + "	   SUM(Itn.itnbenefit/100.00) as totalCoverageUsed "
                                                                        + " from Itn "
                                                                        + " group by Itn.itnsubscriber, Itn.itnplanid) as ISB on ISB.itnsubscriber = INF.ixisubpid and ISB.itnplanid = INF.ixiplanid "
                                                                + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , case when nspdeductibles = '' then LEFT(SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000) + 'X') -1) else LEFT(SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000) + 'X') -1) end as RemainingAmount from  pat as p WITH(NOLOCK) "
                                                                        + " Inner JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                        + " Inner Join nsp as NP with(NOLOCK) on NP.nid = XI.ixiplanid "
                                                                + " where NP.nspmaximums != '' or NP.nspdeductibles != '') as HH on HH.PatientEHRId = PA.pid ";

        public static string GetAbelDentPatientData_12_10_6 = "Select DISTINCT PA.pid AS Patient_EHR_ID,'' AS Patient_Web_ID, PA.pfname AS First_name, PA.plname AS Last_name, "
                                                            + " PA.pinitial AS Middle_Name,PA.pmrmrs AS Salutation,case when PA.pinactive = 0 then 'A' else 'I' end AS [Status], (case WHEN PA.pgender = 'M' THEN 'MALE' WHEN PA.pgender = 'F' THEN 'FEMALE' WHEN PA.pgender = '' THEN '' END ) as SEX,'' AS MaritalStatus,'' AS school, "
                                                            + " (CASE When PA.pbirth IS NULL then CONVERT(datetime,'17530101',112) else PA.pbirth end) AS Birth_Date, AA.infemail as Email, ISNULL(AA.infmobile,'') AS Mobile, ISNULL(PA.pphone, '') AS Home_Phone, ISNULL(PA.pworkphn, '') AS Work_Phone, "
                                                            + " ISNULL(PA.pstreetadr, '') AS Address1, ISNULL(PA.pstreetadr2, '') AS Address2, CT.cdesc AS[City],'' AS[State],ISNULL(PA.ppostal,'') AS Zipcode, "
                                                            + " (case WHEN P2.pinactive is NULL then '' when P2.pinactive = 1 then '' ELSE 'Active' end) AS ResponsibleParty_Status,ISNULL(AG.Pat_AR30,0) AS ThirtyDay,ISNULL(AG.Pat_AR60,0) AS SixtyDay,ISNULL(AG.Pat_AR90,0) AS NinetyDay,ISNULL(AG.Pat_AR90_Plus,0) AS Over90,ISNULL(AG.Pat_Total,0) AS CurrentBal,ISNULL(HH.RemainingAmount,0) - ISNULL(ISB.totalCoverageUsed,0) as remaining_benefit,ISNULL(ISB.totalCoverageUsed,0) as used_benefit, PA.paltid AS ssn,AA.infemployer as employer, "
                                                            + " FVisit.FirstVisitDate AS FirstVisit_Date,ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance, ISNULL(PrimIns.PrimaryInsuranceCompanyName, '') AS Primary_Insurance_CompanyName, PrimIns.PrimarySubcriberId as Primary_Ins_Subscriber_ID, ISNULL(PrimIns.Prim_Ins_Company_Phonenumber,'') as Prim_Ins_Company_Phonenumber,ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance,ISNULL(SecIns.SecondaryInsuranceCompanyName,'') AS Secondary_Insurance_CompanyName,ISNULL(SecIns.Sec_Ins_Company_Phonenumber,'') as Sec_Ins_Company_Phonenumber, ( case WHEN PA.pinactive = 1 then 'InActive' ELSE 'Active' end ) AS EHR_Status, 0 AS Is_Deleted, "
                                                            + " (CASE WHEN PA.pchargeto = 0 then PA.pid ELSE PA.pchargeto END) AS Guar_ID,SecIns.SecondarySubcriberId as Secondary_Ins_Subscriber_ID, ISNULL(PA.pdentist,'') AS Pri_Provider_ID, ISNULL(PA.phygienist, '') AS Sec_Provider_ID, (CASE WHEN AA.infallowtextmsg = 1 then 'Y' else 'N' end) AS ReceiveSms, (CASE WHEN AA.infallownewsemail = 1 then 'Y' else 'N' end) AS ReceiveEmail, "
                                                            + " LVisit.LastVisitDate AS LastVisit_Date, (SELECT MIN (adate) From apt WITH(NOLOCK) Where adate > GETDATE() AND astatus NOT IN ('R') and apid = PA.pid) AS nextvisit_date, "
                                                            + " AB.DueDate AS due_date,ISNULL(AG.Collect_total,0) AS collect_payment, '' AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated, '' AS preferred_name, "
                                                            + " 1 As InsUptDlt,'' as ReceiveVoiceCall,CASE WHEN PA.pnativetongue = 'E' THEN 'English' WHEN PA.pnativetongue = 'F' THEN 'French' WHEN PA.pnativetongue = '' THEN 'ENGLISH' END AS PreferredLanguage, AA.infnote AS Patient_note, "
                                                            + " P2.pid AS responsiblepartyid, P2.pfname AS ResponsibleParty_First_Name, P2.plname AS ResponsibleParty_Last_Name,'' AS responsiblepartyssn, P2.pbirth AS responsiblepartybirthdate,'' AS EmergencyContactId,'' AS EmergencyContact_First_Name, "
                                                            + " '' AS EmergencyContact_Last_Name,'' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' AS driverlicense, "
                                                            + " '' AS groupid,0 as Clinic_Number, 1 as Service_Install_Id,0 as Patient_LocalDB_ID from pat PA WITH(NOLOCK)"
                                                            + " LEFT JOIN inf AA  WITH(NOLOCK) on AA.infpid = PA.pid "
                                                            + " LEFT JOIN  pat P2 WITH(NOLOCK) on PA.pchargeto = P2.pid "
                                                            + " LEFT JOIN  cty CT WITH(NOLOCK) on CT.ccode = PA.pcitycode "                                                                                                                                                                                                
                                                            + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , CAST(NS.inscoid AS varchar) as PrimaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as PrimarySubcriberId, NS.insname as PrimaryInsuranceCompanyName, NS.insphone as Prim_Ins_Company_Phonenumber from pat as p WITH(NOLOCK)  "
                                                            + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                            + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 1) as PrimIns on PrimIns.PatientEHRId = PA.pid "
                                                            + " LEFT JOIN(select DISTINCT p.pid as PatientEHRId , NS.inscoid as SecondaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as SecondarySubcriberId, NS.insname as SecondaryInsuranceCompanyName , NS.insphone as Sec_Ins_Company_Phonenumber from pat as p WITH(NOLOCK) "
                                                            + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                            + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 2) as SecIns on SecIns.PatientEHRId = PA.pid "
                                                            + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MAX(Appt.adate) as LastVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null AND Appt.adate <= CONVERT(DATE, GETDATE()) Group by Appt.apid ) AS LVisit on LVisit.Patient_EHR_Id =  PA.pid  "
                                                            + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MIN(Appt.adate) as FirstVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null Group by Appt.apid) AS FVisit on FVisit.Patient_EHR_Id =  PA.pid  "
                                                            + " LEFT JOIN (select PP.pid,CONVERT( varchar, ( case when PP.paptinvl > 12 then (DATEADD(Day, CAST(PP.paptinvl AS INT),  AA.DueDate)) else ((DATEADD(Month, CAST(PP.paptinvl AS INT),  AA.DueDate)))END)) +'@'+ CONVERT( varchar,  rl.msgmessage) + '@' + CAST(rl.msgid AS varchar) AS DueDate   from pat PP WITH(NOLOCK) "
                                                                + " INNER JOIN (SELECT apid, Max(ap1.adate) AS DueDate FROM  apt ap1 WITH(NOLOCK) Where aconfstat = 'D' AND astatus  != 'R' GROUP BY apid) AS AA ON PP.pid = AA.apid " 
                                                                    + " LEFT JOIN msg rl WITH(NOLOCK) on rl.msgid = PP.paptinvl where rl.msgid != '' and rl.msgid != 0 and rl.msgtype = 'PATAI' and ISNUMERIC(rl.msgid) = 1 and rl.msgid > 0) as AB on AB.pid = PA.pid "
                                                            + " LEFT JOIN (Select Trn.tpid, "
                                                                + " SUM(CASE WHEN Trn.tdate <= CONVERT(DATE, GETDATE()) AND Trn.tdate >= DATEADD(day, -29, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg)/100.00) ELSE 0 END) - MIN(TT.PatientPayment) As Pat_AR30, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -30, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -59, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR60, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -60, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -89, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -90, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90_Plus, "
                                                                + " SUM((Trn.tamount - Trn.tinschg) / 100.00) - MIN(TT.PatientPayment)  as Pat_Total,MIN(TT.PatientPayment) as Collect_total "
                                                                + " FROM trn Trn "
                                                                + " JOIN Pat Pat On Pat.Pid = Trn.tpid "
                                                                + " LEFT JOIN(Select Trn.tpid as PatientId, "
                                                                + " SUM(Case When Trn.tptype != 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as PatientPayment, "
                                                                + " SUM(Case When Trn.tptype  = 'R' THEN Trn.tinschg/100.00 ELSE 0 END) as InsPayment "
                                                                + " from trn Trn Where Trn.tprepay != 1  "
                                                                + " GROUP BY Trn.tpid) as TT on TT.PatientId = Trn.tpid "
                                                                + " where Pat.pInactive = 0 and Trn.tprepay != 1"
                                                                + " Group by Trn.tpid "
                                                                + " HAVING SUM((Trn.tamount - Trn.tinschg)/100.00) - MIN(TT.PatientPayment) > 0 ) as AG on AG.tpid = PA.pid "
                                                                + " LEFT JOIN (Select i1.ixipid, i1.ixiplno, COALESCE(i2.ixiplanid,i1.ixiplanid) as ixiplanid, COALESCE(i2.ixicertno, i1.ixicertno) as ixicertno, "
                                                                        + " COALESCE(i2.ixiassigned, i1.ixiassigned) as ixiassigned, CASE WHEN i1.ixisubpid != 0 THEN i1.ixisubpid ELSE i1.ixipid END as ixisubpid, i1.ixireltosub, "
                                                                        + " COALESCE(i2.ixigroupno, i1.ixigroupno) as ixigroupno, COALESCE(i2.ixidivsect, i1.ixidivsect) as ixidivsect, "
                                                                        + " COALESCE(i2.ixisubuse, i1.ixisubuse) as ixisubuse, COALESCE(i2.ixiReleaseOfInfo, i1.ixiReleaseOfInfo) as ixiReleaseOfInfo, "
                                                                        + " NS.inscoid, Ns.InsName, NS.insphone, NS.inscarrierid, "
                                                                        + " nsp.ninscoid, nsp.nspdeductibles, nsp.nspmaximums, nsp.nassigned, nsp.nsppreventive, nsp.nspbasic, nsp.nspmajor, nsp.nsportho, nsp.nspplantype, "
                                                                        + " nsp.nanniv, nsp.nplanname, nsp.nsppercentagelimit, nsp.nfeeyear, nsp.nchargesched, nsp.ncopaysched, nsp.nspexclusive "
                                                                        + " from ixi as i1 "
                                                                        + " LEFT JOIN ixi as i2 on i1.ixisubpid = i2.ixipid AND i2.ixiplno = i1.ixisubuse "
                                                                        + " join nsp Nsp on nsp.nid = COALESCE(i2.ixiplanid, i1.ixiplanid) "
                                                                        + " JOIN ins as NS WITH(NOLOCK) on NS.inscoid = Nsp.ninscoid where i1.ixiplno = 1) as INF on INF.ixipid = PA.pid "
                                                                        + "           LEFT JOIN(Select Itn.itnsubscriber, Itn.itnplanid, "
                                                                        + "      SUM(Itn.itndedamount/100.00) as deductionPaid, "
                                                                        + "	   SUM(Itn.itnbenefit/100.00) as totalCoverageUsed "
                                                                        + " from Itn "
                                                                        + " group by Itn.itnsubscriber, Itn.itnplanid) as ISB on ISB.itnsubscriber = INF.ixisubpid and ISB.itnplanid = INF.ixiplanid "
                                                                + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , case when nspdeductibles = '' then LEFT(SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000) + 'X') -1) else LEFT(SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000) + 'X') -1) end as RemainingAmount from  pat as p WITH(NOLOCK) "
                                                                        + " Inner JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                        + " Inner Join nsp as NP with(NOLOCK) on NP.nid = XI.ixiplanid "
                                                                + " where NP.nspmaximums != '' or NP.nspdeductibles != '') as HH on HH.PatientEHRId = PA.pid ";

        public static string GetAbelDentPatientDataWithCondition_12_10_6 = " Select DISTINCT PA.pid AS Patient_EHR_ID,'' AS Patient_Web_ID,PA.pfname AS First_name,PA.plname AS Last_name, "
                                                            + " PA.pinitial AS Middle_Name,PA.pmrmrs AS Salutation,case when PA.pinactive = 0 then 'A' else 'I' end AS [Status], (case WHEN PA.pgender = 'M' THEN 'MALE' WHEN PA.pgender = 'F' THEN 'FEMALE' WHEN PA.pgender = '' THEN '' END ) as SEX,'' AS MaritalStatus,'' AS school, "
                                                            + " (CASE When PA.pbirth IS NULL then CONVERT(datetime,'17530101',112) else PA.pbirth end) AS Birth_Date, AA.infemail as Email, ISNULL(AA.infmobile,'') AS Mobile, ISNULL(PA.pphone, '') AS Home_Phone, ISNULL(PA.pworkphn, '') AS Work_Phone, "
                                                            + " ISNULL(PA.pstreetadr, '') AS Address1, ISNULL(PA.pstreetadr2, '') AS Address2, CT.cdesc AS[City],'' AS[State],ISNULL(PA.ppostal,'') AS Zipcode, "
                                                            + " (case WHEN P2.pinactive is NULL then '' when P2.pinactive = 1 then '' ELSE 'Active' end) AS ResponsibleParty_Status,ISNULL(AG.Pat_AR30,0) AS ThirtyDay,ISNULL(AG.Pat_AR60,0) AS SixtyDay,ISNULL(AG.Pat_AR90,0) AS NinetyDay,ISNULL(AG.Pat_AR90_Plus,0) AS Over90,ISNULL(AG.Pat_Total,0) AS CurrentBal,ISNULL(HH.RemainingAmount,0) - ISNULL(ISB.totalCoverageUsed,0) as remaining_benefit,ISNULL(ISB.totalCoverageUsed,0) as used_benefit, PA.paltid AS ssn,AA.infemployer as employer, "
                                                            + " FVisit.FirstVisitDate AS FirstVisit_Date,ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance, ISNULL(PrimIns.PrimaryInsuranceCompanyName, '') AS Primary_Insurance_CompanyName, PrimIns.PrimarySubcriberId as Primary_Ins_Subscriber_ID, ISNULL(PrimIns.Prim_Ins_Company_Phonenumber,'') as Prim_Ins_Company_Phonenumber,ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance,ISNULL(SecIns.SecondaryInsuranceCompanyName,'') AS Secondary_Insurance_CompanyName,ISNULL(SecIns.Sec_Ins_Company_Phonenumber,'') as Sec_Ins_Company_Phonenumber, ( case WHEN PA.pinactive = 1 then 'InActive' ELSE 'Active' end ) AS EHR_Status, 0 AS Is_Deleted, "
                                                            + " (CASE WHEN PA.pchargeto = 0 then PA.pid ELSE PA.pchargeto END) AS Guar_ID,SecIns.SecondarySubcriberId as Secondary_Ins_Subscriber_ID, ISNULL(PA.pdentist,'') AS Pri_Provider_ID, ISNULL(PA.phygienist, '') AS Sec_Provider_ID, (CASE WHEN AA.infallowtextmsg = 1 then 'Y' else 'N' end) AS ReceiveSms, (CASE WHEN AA.infallownewsemail = 1 then 'Y' else 'N' end) AS ReceiveEmail, "
                                                            + " LVisit.LastVisitDate AS LastVisit_Date, (SELECT MIN (adate) From apt WITH(NOLOCK) Where adate > GETDATE() AND astatus NOT IN ('R') and apid = PA.pid) AS nextvisit_date, "
                                                            + " AB.DueDate AS due_date,ISNULL(AG.Collect_total,0) AS collect_payment, '' AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated, '' AS preferred_name, "
                                                            + " 1 As InsUptDlt,'' as ReceiveVoiceCall,CASE WHEN PA.pnativetongue = 'E' THEN 'English' WHEN PA.pnativetongue = 'F' THEN 'French' WHEN PA.pnativetongue = '' THEN 'ENGLISH' END AS PreferredLanguage, AA.infnote AS Patient_note, "
                                                            + " P2.pid AS responsiblepartyid, P2.pfname AS ResponsibleParty_First_Name, P2.plname AS ResponsibleParty_Last_Name,'' AS responsiblepartyssn, P2.pbirth AS responsiblepartybirthdate,'' AS EmergencyContactId,'' AS EmergencyContact_First_Name, "
                                                            + " '' AS EmergencyContact_Last_Name,'' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' AS driverlicense, "
                                                            + " '' AS groupid,0 as Clinic_Number, 1 as Service_Install_Id,0 as Patient_LocalDB_ID from pat PA WITH(NOLOCK)"
                                                            + " LEFT JOIN inf AA  WITH(NOLOCK) on AA.infpid = PA.pid "
                                                            + " LEFT JOIN  pat P2 WITH(NOLOCK) on PA.pchargeto = P2.pid "
                                                            + " LEFT JOIN  cty CT WITH(NOLOCK) on CT.ccode = PA.pcitycode "
                                                            + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId ,CAST(NS.inscoid AS varchar) as PrimaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as PrimarySubcriberId, NP.nplanname as PrimaryInsuranceCompanyName, NS.insphone as Prim_Ins_Company_Phonenumber from pat as p WITH(NOLOCK) "
                                                            + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                            + " LEFT OUTER JOIN nsp as NP WITH(NOLOCK) on NP.nid = XI.ixiplanid "
                                                            + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = NP.ninscoid where XI.ixiplno = 1) as PrimIns on PrimIns.PatientEHRId = PA.pid "
                                                            + " LEFT JOIN(select DISTINCT p.pid as PatientEHRId , XI.ixiplanid as SecondaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as SecondarySubcriberId, NP.nplanname  as SecondaryInsuranceCompanyName , NS.insphone as Sec_Ins_Company_Phonenumber from pat as p WITH(NOLOCK)"
                                                            + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                            + " LEFT OUTER JOIN nsp as NP WITH(NOLOCK) on NP.nid = XI.ixiplanid "
                                                            + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = NP.ninscoid where XI.ixiplno = 2) as SecIns on SecIns.PatientEHRId = PA.pid"
                                                            + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MAX(Appt.adate) as LastVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null AND Appt.adate <= CONVERT(DATE, GETDATE()) Group by Appt.apid ) AS LVisit on LVisit.Patient_EHR_Id =  PA.pid  "
                                                            + " LEFT JOIN ( SELECT Appt.apid as Patient_EHR_Id, MIN(Appt.adate) as FirstVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null Group by Appt.apid) AS FVisit on FVisit.Patient_EHR_Id =  PA.pid  "
                                                            + " LEFT JOIN (select PP.pid,CONVERT( varchar, ( case when PP.paptinvl > 12 then (DATEADD(Day, CAST(PP.paptinvl AS INT),  AA.DueDate)) else ((DATEADD(Month, CAST(PP.paptinvl AS INT),  AA.DueDate)))END)) +'@'+ CONVERT( varchar,  rl.msgmessage) + '@' + CAST(rl.msgid AS varchar) AS DueDate   from pat PP WITH(NOLOCK) "
                                                                + " INNER JOIN (SELECT apid, Max(ap1.adate) AS DueDate FROM  apt ap1 WITH(NOLOCK) Where aconfstat = 'D' AND astatus  != 'R' GROUP BY apid) AS AA ON PP.pid = AA.apid "
                                                                    + " LEFT JOIN msg rl WITH(NOLOCK) on rl.msgid = PP.paptinvl where rl.msgid != '' and rl.msgid != 0 and rl.msgtype = 'PATAI' and ISNUMERIC(rl.msgid) = 1 and rl.msgid > 0) as AB on AB.pid = PA.pid "
                                                            + " LEFT JOIN (Select Trn.tpid, "
                                                                + " SUM(CASE WHEN Trn.tdate <= CONVERT(DATE, GETDATE()) AND Trn.tdate >= DATEADD(day, -29, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg)/100.00) ELSE 0 END) - MIN(TT.PatientPayment) As Pat_AR30, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -30, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -59, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR60, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -60, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -89, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90, "
                                                                + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -90, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0  As Pat_AR90_Plus, "
                                                                + " SUM((Trn.tamount - Trn.tinschg) / 100.00) - MIN(TT.PatientPayment)  as Pat_Total,MIN(TT.PatientPayment) as Collect_total "
                                                                + " FROM trn Trn "
                                                                + " JOIN Pat Pat On Pat.Pid = Trn.tpid "
                                                                + " LEFT JOIN(Select Trn.tpid as PatientId, "
                                                                + " SUM(Case When Trn.tptype != 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as PatientPayment, "
                                                                + " SUM(Case When Trn.tptype  = 'R' THEN Trn.tinschg/100.00 ELSE 0 END) as InsPayment "
                                                                + " from trn Trn Where Trn.tprepay != 1 "
                                                                + " GROUP BY Trn.tpid) as TT on TT.PatientId = Trn.tpid "
                                                                + " where Pat.pInactive = 0 and Trn.tprepay != 1"
                                                                + " Group by Trn.tpid "
                                                                + " HAVING SUM((Trn.tamount - Trn.tinschg)/100.00) - MIN(TT.PatientPayment) > 0 ) as AG on AG.tpid = PA.pid "
                                                                + " LEFT JOIN (Select i1.ixipid, i1.ixiplno, COALESCE(i2.ixiplanid,i1.ixiplanid) as ixiplanid, COALESCE(i2.ixicertno, i1.ixicertno) as ixicertno, "
                                                                        + " COALESCE(i2.ixiassigned, i1.ixiassigned) as ixiassigned, CASE WHEN i1.ixisubpid != 0 THEN i1.ixisubpid ELSE i1.ixipid END as ixisubpid, i1.ixireltosub, "
                                                                        + " COALESCE(i2.ixigroupno, i1.ixigroupno) as ixigroupno, COALESCE(i2.ixidivsect, i1.ixidivsect) as ixidivsect, "
                                                                        + " COALESCE(i2.ixisubuse, i1.ixisubuse) as ixisubuse, COALESCE(i2.ixiReleaseOfInfo, i1.ixiReleaseOfInfo) as ixiReleaseOfInfo, "
                                                                        + " NS.inscoid, Ns.InsName, NS.insphone, NS.inscarrierid, "
                                                                        + " nsp.ninscoid, nsp.nspdeductibles, nsp.nspmaximums, nsp.nassigned, nsp.nsppreventive, nsp.nspbasic, nsp.nspmajor, nsp.nsportho, nsp.nspplantype, "
                                                                        + " nsp.nanniv, nsp.nplanname, nsp.nsppercentagelimit, nsp.nfeeyear, nsp.nchargesched, nsp.ncopaysched, nsp.nspexclusive "
                                                                        + " from ixi as i1 "
                                                                        + " LEFT JOIN ixi as i2 on i1.ixisubpid = i2.ixipid AND i2.ixiplno = i1.ixisubuse "
                                                                        + " join nsp Nsp on nsp.nid = COALESCE(i2.ixiplanid, i1.ixiplanid) "
                                                                        + " JOIN ins as NS WITH(NOLOCK) on NS.inscoid = Nsp.ninscoid where i1.ixiplno = 1) as INF on INF.ixipid = PA.pid "
                                                                        + "           LEFT JOIN(Select Itn.itnsubscriber, Itn.itnplanid, "
                                                                        + "      SUM(Itn.itndedamount/100.00) as deductionPaid, "
                                                                        + "	   SUM(Itn.itnbenefit/100.00) as totalCoverageUsed "
                                                                        + " from Itn "
                                                                        + " group by Itn.itnsubscriber, Itn.itnplanid) as ISB on ISB.itnsubscriber = INF.ixisubpid and ISB.itnplanid = INF.ixiplanid "
                                                                + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , case when nspdeductibles = '' then LEFT(SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000) + 'X') -1) else LEFT(SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000) + 'X') -1) end as RemainingAmount from  pat as p WITH(NOLOCK) "
                                                                        + " Inner JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                        + " Inner Join nsp as NP with(NOLOCK) on NP.nid = XI.ixiplanid "
                                                                + " where NP.nspmaximums != '' or NP.nspdeductibles != '') as HH on HH.PatientEHRId = PA.pid ";


        public static string GetAbelDentPatientBalance = "   Select DISTINCT AP.apnpid AS Patient_EHR_ID, AP.apnpid as Appointment_EHR_ID, "
                                                         + " AG.Pat_AR30 AS ThirtyDay,AG.Pat_AR60 AS SixtyDay,AG.Pat_AR90 AS NinetyDay,AG.Pat_AR90_Plus AS Over90,AG.Pat_Total AS CurrentBal, ISNULL(HH.RemainingAmount,0) - ISNULL(ISB.totalCoverageUsed,0) as remaining_benefit,ISNULL(ISB.totalCoverageUsed,0) as used_benefit,0 AS Clinic_Number, ISNULL(AG.Collect_total,0) as collect_payment "
                                                         + " from apn AP WITH(NOLOCK) "
                                                         + " LEFT JOIN(Select Trn.tpid, "
                                                         + " SUM(CASE WHEN Trn.tdate <= CONVERT(DATE, GETDATE()) AND Trn.tdate >= DATEADD(day, -29, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg)/100.00) ELSE 0 END) - MIN(TT.PatientPayment) As Pat_AR30, "
                                                         + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -30, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -59, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR60, "
                                                         + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -60, CONVERT(date, GETDATE())) AND trn.tdate >= DATEADD(day, -89, CONVERT(date, GETDATE()))  THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90, "
                                                         + " SUM(CASE WHEN Trn.tdate <= DATEADD(day, -90, CONVERT(date, GETDATE())) THEN((Trn.tamount - Trn.tinschg) / 100.00) ELSE 0 END) - 0 As Pat_AR90_Plus, "
                                                         + " SUM((Trn.tamount - Trn.tinschg) / 100.00) - MIN(TT.PatientPayment)  as Pat_Total,MIN(TT.PatientPayment) as Collect_total "
                                                         + " FROM trn Trn "
                                                         + " JOIN Pat Pat On Pat.Pid = Trn.tpid "
                                                         + " LEFT JOIN(Select Trn.tpid as PatientId, "
                                                         + " SUM(Case When Trn.tptype != 'R' THEN Trn.tpamt/100.00 ELSE 0 END) as PatientPayment "
                                                         + " from trn Trn Where Trn.tprepay != 1 "
                                                         + " GROUP BY Trn.tpid) as TT on TT.PatientId = Trn.tpid "
                                                         + " where Pat.pInactive = 0 and Trn.tprepay != 1"
                                                         + " Group by Trn.tpid "
                                                         + " HAVING SUM((Trn.tamount - Trn.tinschg)/100.00) - MIN(TT.PatientPayment) > 0 ) as AG on AG.tpid = AP.apnpid	"
                                                            + " LEFT JOIN (Select i1.ixipid, i1.ixiplno, COALESCE(i2.ixiplanid,i1.ixiplanid) as ixiplanid, COALESCE(i2.ixicertno, i1.ixicertno) as ixicertno, "
                                                            + " COALESCE(i2.ixiassigned, i1.ixiassigned) as ixiassigned, CASE WHEN i1.ixisubpid != 0 THEN i1.ixisubpid ELSE i1.ixipid END as ixisubpid, i1.ixireltosub, "
                                                            + " COALESCE(i2.ixigroupno, i1.ixigroupno) as ixigroupno, COALESCE(i2.ixidivsect, i1.ixidivsect) as ixidivsect, "
                                                            + " COALESCE(i2.ixisubuse, i1.ixisubuse) as ixisubuse, COALESCE(i2.ixiReleaseOfInfo, i1.ixiReleaseOfInfo) as ixiReleaseOfInfo, "
                                                            + " NS.inscoid, Ns.InsName, NS.insphone, NS.inscarrierid, "
                                                            + " nsp.ninscoid, nsp.nspdeductibles, nsp.nspmaximums, nsp.nassigned, nsp.nsppreventive, nsp.nspbasic, nsp.nspmajor, nsp.nsportho, nsp.nspplantype, "
                                                            + " nsp.nanniv, nsp.nplanname, nsp.nsppercentagelimit, nsp.nfeeyear, nsp.nchargesched, nsp.ncopaysched, nsp.nspexclusive "
                                                            + " from ixi as i1 "
                                                            + " LEFT JOIN ixi as i2 on i1.ixisubpid = i2.ixipid AND i2.ixiplno = i1.ixisubuse "
                                                            + " join nsp Nsp on nsp.nid = COALESCE(i2.ixiplanid, i1.ixiplanid) "
                                                            + " JOIN ins as NS WITH(NOLOCK) on NS.inscoid = Nsp.ninscoid where i1.ixiplno = 1) as INF on INF.ixipid = AP.apnpid "
                                                            + "           LEFT JOIN (Select Itn.itnsubscriber, Itn.itnplanid, "
                                                            + "      SUM(Itn.itndedamount/100.00) as deductionPaid, "
                                                            + "	   SUM(Itn.itnbenefit/100.00) as totalCoverageUsed "
                                                            + " from Itn "
                                                            + " group by Itn.itnsubscriber, Itn.itnplanid) as ISB on ISB.itnsubscriber = INF.ixisubpid and ISB.itnplanid = INF.ixiplanid "
                                                         + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , case when nspdeductibles = '' then LEFT(SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspmaximums, PATINDEX('%[0-9.-]%', nspmaximums), 8000) + 'X') -1) else LEFT(SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000),PATINDEX('%[^0-9.-]%', SUBSTRING(nspdeductibles, PATINDEX('%[0-9.-]%', nspdeductibles), 8000) + 'X') -1) end as RemainingAmount from  pat as p WITH(NOLOCK) "
                                                                + " Inner JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                                + " Inner Join nsp as NP with(NOLOCK) on NP.nid = XI.ixiplanid "
                                                                + " where NP.nspmaximums != '' or NP.nspdeductibles != '') as HH on HH.PatientEHRId = AP.apnpid "
                                                         + " Where AP.apnnote Like '%Dismissed%' and  "
                                                         + " AP.apndate BETWEEN DATEADD(day, -7, CONVERT(binary, GETDATE())) AND CONVERT(date, GETDATE()) ";

        public static string GetAelDentLastVisit_Date = "   SELECT Appt.apid as Patient_EHR_Id,"
                                                        + " MAX(Appt.adate) as LastVisitDate"
                                                        + " FROM apt Appt  "
                                                        + " WHERE appt.astatus != 'R' "
                                                        + " AND appt.aidentifier is not null "
                                                        + " AND Appt.adate <= CONVERT(DATE, GETDATE()) group by Appt.apid"; 

        public static string GetAbelDentNextApptDate = "select MIN (adate) AS nextvisit_date,apid,astatus From apt WITH(NOLOCK)"
                                                            + " Where adate > GETDATE() AND astatus NOT IN('R') Order by adate desc;";


        public static string GetPatientName = "Select pfname+', '+plname from pat where pid=@PatientId";
        public static string GetPatientUniqeId = "Select Salt2 from pat where pid=@PatientId";


        public static string GetAbelDentPatientIdData = "Select DISTINCT PA.pid AS Patient_EHR_ID from pat PA Where PA.pid <> ''";

        public static string Update_Receive_SMS_Patient_EHR_Live_To_AbelDentEHR = " UPDATE inf SET infallowtextmsg = @receives_sms WHERE infpid = @patient_id ";

        public static string Update_Patient_Record_By_Patient_Form = " UPDATE pat SET ColumnName = @ehrfield_value WHERE pid = @Patient_EHR_ID ";
        public static string Update_Patient_Info_Record_By_Patient_Form = " UPDATE inf SET ColumnName = @ehrfield_value WHERE infpid = @Patient_EHR_ID ";
        public static string Update_Patient_Ins1_Record_By_Patient_Form = " UPDATE ixi SET ColumnName = @ehrfield_value WHERE ixipid = @Patient_EHR_ID AND ixiplno = 1";
        public static string Update_Patient_Ins2_Record_By_Patient_Form = " UPDATE ixi SET ColumnName = @ehrfield_value WHERE ixipid = @Patient_EHR_ID AND ixiplno = 2";
        public static string Update_Patient_nsp_Record_By_Patient_Form = " UPDATE nsp SET ColumnName = @ehrfield_value WHERE nid = @InsCode ";

        public static string GetAbelDentPatientListData = " SELECT  0 AS Clinic_Number,1 as Service_Install_Id,pid AS Patient_EHR_ID,plname,pfname as FirstName, (pfname + '' + plname) AS Patient_Name, "
        + " ISNULL(inf.infmobile,'') AS Mobile, ISNULL(pat.pphone, '') AS Home_Phone, ISNULL(pworkphn, '') AS Work_Phone, "
        + " CASE WHEN pinactive = 1 THEN 'I' ELSE 'A' END AS  Status, 0 AS responsible_party,'' as Email,pbirth AS Birth_Date From pat WITH(NOLOCK) "
        + " Inner join inf on inf.infpid = pat.pid ";

        public static string GetAbelDentPatientRecall = "SELECT 0 AS Clinic_Number,1 as Service_Install_Id,'' AS Patient_RecallType_LocalDB_ID,rpid AS Patient_Recall_Id,PA.pid AS Patient_EHR_Id,'' AS Patient_Web_ID,rdate as Recall_Date,rduedate as Last_Recall_Date,rdid AS Provider_EHR_ID,RC.rpid AS RecallType_EHR_ID,RC.rtype AS RecallType_Name,RC.rpwork + ' ' + RC.rpwrk2 AS RecallType_Descript,'B' as Default_Recall,RC.rdate AS Entry_DateTime,GETDATE() as Last_Sync_Date,'' as EHR_Entry_DateTime,RC.rinactive AS IS_Deleted,0 as IS_Adit_Updated From rcl RC WITH(NOLOCK) left join pat PA WITH(NOLOCK) on PA.pid = RC.rpid where PA.pid != 0";

        public static string GetAbelDentRecallType = "SELECT 0 AS Clinic_Number,1 as Service_Install_Id,'' AS RecallType_LocalDB_ID,msgidentifier AS RecallType_EHR_ID,'' AS RecallType_Web_ID,msgmessage AS RecallType_Name,msgtype AS RecallType_Descript,getdate() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated from msg WITH(NOLOCK) where msgtype = 'PATAI' and msgid != '' and msgmessage != ''";
        public static string GetAbelDentRecallType_12_10_6 = "SELECT 0 AS Clinic_Number,1 as Service_Install_Id,'' AS RecallType_LocalDB_ID,msgid AS RecallType_EHR_ID,'' AS RecallType_Web_ID,msgmessage AS RecallType_Name,msgtype AS RecallType_Descript,getdate() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated from msg WITH(NOLOCK) where msgtype = 'PATAI' and msgid != ''";

        public static string InsertPatientDetails = "  INSERT INTO pat (pid,plname,pfname,pinitial,pdentist,pstreetadr,pcitycode,ppostal,pphone,pworkphn,pworkext,pbirth,"
                                                    + "pchargeto,paptinvl,plastckp,paptflag,pgender,pmrmrs,pbestphone,pshortnotice,pmedical,phygienist,pnormunits,pstatus,plnamcase,pfnamcase,"
                                                    + "passistant,punlisted,pjrsr,pbesttime,presides,paltid,pnativetongue,pstudent,phandicapped,pataptfrom,pataptsto,pataptdays,preferdrid,"
                                                    + "plateflag,ppremedflag,pinactive,pnonpatient,psince,pmailingname,pstreetadr2,WebPassword,PasswordExpiration,pscalinginvl,pscalingunits,pphoneldprefix,"
                                                    + "pworkphnldprefix,ppersonalid,PasswordHash2,Salt2)VALUES(((select MAX(ISNULL(pid,0)) AS pid from pat WITH(NOLOCK)) + 1), '@LastName', '@FirstName','@MName', '@ProviderID','','', "
                                                    + "NULL, @MobileNo, NULL, NULL, @BirthDate, NULL, NULL, NULL, NULL,'','','', NULL, 0, '',0, NULL,1,1, '',0, NULL, NULL, NULL, NULL, NULL,0,0, "
                                                    + "GETDATE(), GETDATE(), NULL, NULL,0,0,0,1,0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, @UniqID)";

        public static string InsertPatientDetails_14_4_2 = "  INSERT INTO pat (pid,plname,pfname,pinitial,pdentist,pstreetadr,pcitycode,ppostal,pphone,pworkphn,pworkext,pbirth,"
                                                    + "pchargeto,paptinvl,plastckp,paptflag,pgender,pmrmrs,pbestphone,pshortnotice,pmedical,phygienist,pnormunits,pstatus,plnamcase,pfnamcase,"
                                                    + "passistant,punlisted,pjrsr,pbesttime,presides,paltid,pnativetongue,pstudent,phandicapped,pataptfrom,pataptsto,pataptdays,preferdrid,"
                                                    + "plateflag,ppremedflag,pinactive,pnonpatient,psince,pmailingname,pstreetadr2,WebPassword,PasswordExpiration,pscalinginvl,pscalingunits,pphoneldprefix,"
                                                    + "pworkphnldprefix,ppersonalid)VALUES(((select MAX(ISNULL(pid,0)) AS pid from pat WITH(NOLOCK)) + 1), '@LastName', '@FirstName','@MName', '@ProviderID','','', "
                                                    + "NULL, @MobileNo, NULL, NULL, @BirthDate, NULL, NULL, NULL, NULL,'','','', NULL, 0, '',0, NULL,1,1, '',0, NULL, NULL, NULL, NULL, NULL,0,0, "
                                                    + "GETDATE(), GETDATE(), NULL, NULL,0,0,0,1,0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)";

        public static string GetAbelDentIdelUser = "SELECT TOP 1 apnloggedinuser as User_name FROM apn";

        public static string InsertPatientContactDetails = " INSERT INTO inf (infpid,infnote,infemployer,infoccupation,infmedical,infemail,infmobile,infemailpref)"
                                                            + " VALUES ('@pid','','','','','@Email','@MobileNo',NULL) ";

                //public static string PatientSMScallLog = "INSERT INTO PatientNote(PatientID,Importance,DateTime,Text,CategoryID,SecurityOn,AuthorID,Deleted,DisplayOnOpen,IsFamilyNote,OriginalID) VALUES "
        //                                                + "(@PatientEHRId,@NoteTyp,@PaymentDate,@PatientNote','672A32AC-4083-4EEE-BC79-7A4C18E4DBB2',0,(select top 1 AuthorID from PatientNote),0,0,0,NULL)  Select @@identity END ELSE BEGIN ( SELECT top 1 ID FROM PatientNote WHERE PatientID = @PatientEHRId AND Importance = @NoteType AND Text = @PatientNote AND DateTime = @PaymentDate )  END ";

        //public static string PatientPaymentLog = "INSERT INTO PatientNote(PatientID,Importance,DateTime,Text,CategoryID,SecurityOn,AuthorID,Deleted,DisplayOnOpen,IsFamilyNote,OriginalID) VALUES "
        //                                                + "(@PatientEHRId,@NoteTyp,@PaymentDate,@PatientNote','51CF254F-43C0-4F53-977B-A5CEB583DCBB',0,(select top 1 AuthorID from PatientNote),0,0,0,NULL)  Select @@identity END ELSE BEGIN ( SELECT top 1 ID FROM PatientNote WHERE PatientID = @PatientEHRId AND Importance = @NoteType AND Text = @PatientNote AND DateTime = @PaymentDate )  END ";


        public static string GetPatientTableColumnsName = "select 0 AS Clinic_Number,1 as Service_Install_Id,Column_Name AS EHRColumnName,Table_Name AS TableName,Data_Type AS EHRDataType,IS_Nullable AS AllowNull,"
                                                            + " isnull( (Case when Character_maximum_length is not null then Character_maximum_length else numeric_precision end ),0) AS Size, "
                                                            + " '' AS PatientFormColumnsName,'' AS DefaultValue from information_schema.columns where table_name = 'Pat' AND Column_Name NOT IN ('pjrsr','pataptdays','preferdrid','plateflag','ppremedflag','pnonpatient','pmailingname','WebPassword','PasswordExpiration', "
                                                            + " 'pscalinginvl','psxalingunits','pphoneidprefix','pworkphnidprefix','ppersonalid','PasswordHash2','Salt2','plateflag','ppremedflag','phandicapped','pstudent','pnativetongue','pbesttime','punlisted','pfnamcase')";


        public static string UpdatePatientDefaultPhone = " UPDATE Contact SET DefaultPhoneId = @DefaultPhoneId WHERE ContactId = @ContactId ";

        public static string GetAbelDentPatientStatusNew_Existing = "Select pid as Patient_EHR_Id from pat WITH(NOLOCK) where pid <> '' and pid not in (select p.pid as Patient_EHR_Id from pat p WITH(NOLOCK) INNER join apt AP WITH(NOLOCK) on AP.apid = p.pid where  AP.adate < getdate() AND AP.aconfstat = 'D' AND AP.astatus != 'R' AND AP.aidentifier is not null)";
        
        public static string GetAbelDentPatientStatusNew_ExistingByPatID = " Select pid as Patient_EHR_Id from pat WITH(NOLOCK) " +
                                                                    " where pid = @Patient_EHR_ID and pid <> '' and " +
                                                                    " pid not in (select p.pid as Patient_EHR_Id from pat p WITH(NOLOCK) INNER join apt AP WITH(NOLOCK) on AP.apid = p.pid where  AP.adate < getdate() AND AP.aconfstat = 'D' AND AP.astatus != 'R' AND AP.aidentifier is not null)";

        #endregion
        #region Document
        public static string GetAbelDentDocumentTypeID = "IF NOT EXISTS(select 1 from DocumentType where FriendlyName = '@DocumentName') BEGIN Insert Into DocumentType (ID,FriendlyName,Status,ShouldEditAttributes,ShouldAddTask,IsClinicalResult) "
                                                  + " values(@UniqID,'@DocumentName',1,1,1,0); select ID from DocumentType where FriendlyName = '@DocumentName'; "
                                                  + " END ELSE BEGIN select ID from DocumentType where FriendlyName = '@DocumentName'; END ";

        public static string InsertAbelDentDocument_3 = "INSERT INTO dbo.[Document] ([ID], [Source], [Description], [State], [FileName], [DocTypeID] ) VALUES (@ActID, 'Document via Adit', '','Active', @FileName, @DocumentTypeID)";

        public static string GetAbelDentClinicalPatient = "select ID from ClinicalPatient where RTRIM(LTRIM(LegacyPID)) = '@PatientId'";

        public static string InsertAbelDentClinicalParticipation_2 = "INSERT INTO dbo.[ClinicalParticipation] ([ID], [ActID], [RoleID] ) VALUES (@UniqID, @ActID, @PatientUniqID) ";

        public static string InsertAbelDentClinicalAct_1 = "INSERT INTO dbo.[ClinicalAct] ([ID], [TimeBegun], [TimeEnded], [Note], [ImplementingClass], [AvailabilityTime], [SourceRoleID], [ClassCode] ) VALUES(@UniqID, @Time, @Time, '','Document', GETDATE(), @UserID, 'DOC')"; //select Id from ClinicalAct where Convert(Date,TimeBegun)='2024-4-25'

        #endregion
        #region Patient Disease

        public static string GetAbelDentPatientDiseaseData = "  SELECT '' as PatientDisease_LocalDB_ID,0 AS Clinic_Number,1 as Service_Install_Id,ClinicalConcept.ID AS Disease_EHR_ID, ClinicalPatient.LegacyPID AS Patient_EHR_ID,'' as PatientDisease_Web_ID, ClinicalConcept.ConceptIdentifier as Disease_Name,'A' as Disease_Type,0 as is_deleted,getdate() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated,getdate() as Last_Sync_Date "
                                                            + " FROM ClinicalAct WITH(NOLOCK)  INNER JOIN "
                                                            + " ClinicalAdverseReactionAllergy WITH(NOLOCK) ON ClinicalAct.ID = ClinicalAdverseReactionAllergy.ID INNER JOIN"
                                                            + " ClinicalAllergyAct WITH(NOLOCK) ON ClinicalAct.ID = ClinicalAllergyAct.ActID INNER JOIN "
                                                            + " ClinicalPatient WITH(NOLOCK) ON ClinicalAllergyAct.PatientID = ClinicalPatient.ID INNER JOIN "
                                                            + " ClinicalConcept WITH(NOLOCK) ON ClinicalAct.MainConceptID = ClinicalConcept.ID where ClinicalAct.ImplementingClass = 'ClinicalAdverseReactionAllergy'";

        public static string GetAbelDentAllergies = "Select 0 AS Clinic_Number,1 as Service_Install_Id,CA.ID as Disease_EHR_ID,'' AS Disease_LocalDB_ID,'' as Disease_Web_ID,CA.ConceptIdentifier as Disease_Name,'A' as Disease_Type, 0 as is_deleted,0 as Is_Adit_Updated  from ClinicalConcept CA WITH(NOLOCK) where TerminologyID = 'Text'  and PATINDEX('%[a-zA-Z]%', CA.ConceptIdentifier) = 1;";        

        public static string GetAbelDentProblems = "Select 0 AS Clinic_Number,1 as Service_Install_Id,ProblemID as Disease_EHR_ID,'' AS Disease_LocalDB_ID,'' as Disease_Web_ID,DisplayCode as Disease_Name,'P' as Disease_Type, 0 as is_deleted,0 as Is_Adit_Updated FROM ClinicalProblemAct ";

        #endregion        
        #region Insert Allergy 

        public static string InsertAbelDentClinicalConcept_Allergy_1 = "IF NOT EXISTS(select 1 from ClinicalConcept where ConceptIdentifier = '(@AllergyTextCapi)') BEGIN Insert into ClinicalConcept(ID, TerminologyID, ConceptIdentifier, SummaryDescription) "
                                                                            + " VALUES(@GuidId,'Text','(@AllergyTextCapi)','(@AllergyTextSmall)'); select ID from ClinicalConcept where ConceptIdentifier = '(@AllergyTextCapi)'; "
                                                                        + " END ELSE BEGIN select ID from ClinicalConcept where ConceptIdentifier = '(@AllergyTextCapi)'; END "; 

        public static string InsertAbelDentClinicalAct_Allergy_2 = "INSERT INTO dbo.[ClinicalAct] ([ID], [TimeBegun], [TimeEnded], [MainConceptID], [ImplementingClass], [EffectiveTimeBegun], [EffectiveTimeEnded], [AvailabilityTime], [SourceRoleID], [ClassCode], [Text] ) VALUES (@ActID, @CurDateTime, NULL, @ClinicalConceptID, 'ClinicalAdverseReactionAllergy', NULL, NULL, @CurDateTime, '94a8b483-630a-42df-81d6-9b52d2425e27', 'adv', '(@AllergyText) (@AllergyText)' )";        
        public static string GetAbelDentClinicalPatient_Allergy_3 = "select ID from ClinicalPatient where LTRIM(RTRIM(LegacyPID)) = LTRIM(RTRIM('@PatientId'))";
        public static string InsertAbelDentClinicalAllergyAct_4 = "INSERT INTO dbo.[ClinicalAllergyAct] ([PatientID], [ActID], [Significance] ) VALUES (@PatientUniqID, @ActID, 100 )";
        public static string InsertAbelDentClinicalAdverseReactionAllergy_Allergy_5 = "INSERT INTO dbo.[ClinicalAdverseReactionAllergy] ([ID], [AdverseReactionTypeConceptID] ) VALUES (@ActID, @ClinicalCoceptID )";
        public static string InsertAbelDentClinicalObservation_Allergy_6 = "INSERT INTO dbo.[ClinicalObservation] ([ID], [IsPositiveIndication] ) VALUES (@ActID, 1 )"; 
        public static string InsertAbelDentClinicalActRelationship_Problem_Allergy_7 = "INSERT INTO dbo.[ClinicalActRelationship] ([ID], [SourceActID], [DestinationActID], [RelationshipTypeCode], [DocumentationCategoryID] ) VALUES (NEWID(), '5dcb58f3-3778-4b84-8d06-14927fa1b608', @ActID, 'pt', 'S_AllergAdver' )"; //if allergic then add id into AdverseReactionTypeConceptID else NULL
        public static string InsertAbelDentClinicalParticipation_Allergy_8 = "INSERT INTO dbo.[ClinicalParticipation] ([ID], [ActID], [RoleID], [TypeCode] ) VALUES (NEWID(), @ActID, '2863f550-9bbe-4d14-9bbe-3f9b41323162', 'agn' )";

        #endregion       
        #region Insert AllergyProblems 

        public static string InsertAbelDentClinicalConcept_Problem_1 = "INSERT INTO dbo.[ClinicalConcept] ([ID], [TerminologyID], [ConceptIdentifier], [SummaryDescription] ) VALUES (@ClinicalCoceptID, 'Text', '@AllergyTextCapi', '@AllergyTextCapi' )";
        public static string InsertAbelDentMertial_Problem_2 = "INSERT INTO dbo.[Material] ([ID] ) VALUES (@MaterialGuID)";
        public static string InsertAbelDentClinicalEntity_Problem_3 = "INSERT INTO dbo.[ClinicalEntity] ([ID], [Description], [IsGroup], [ClassCode], [Code], [ImplementingClass], [Quantity], [Determiner] ) VALUES (@MaterialGuID, '@AllergyTextCapi', 0, 'matl', @ClinicalCoceptID, 'Material', 1, 'k' )";

        public static string InsertAbelDentClinicalAct_Problem_2 = "INSERT INTO dbo.[ClinicalAct] ([ID], [TimeBegun], [TimeEnded], [MainConceptID], [ImplementingClass], [EffectiveTimeBegun], [EffectiveTimeEnded], [AvailabilityTime], [SourceRoleID], [ClassCode], [Text] ) VALUES (@ActGuID, '2024-06-04 09:01:10.155', NULL, @ClinicalCoceptID, 'ClinicalAdverseReactionAllergy', NULL, NULL, '2024-06-04 09:01:25.097', '94a8b483-630a-42df-81d6-9b52d2425e27', 'adv', 'Marica' )";

        public static string InsertAbelDentClinicalActRelationship_Problem_4 = "INSERT INTO dbo.[ClinicalActRelationship] ([ID], [SourceActID], [DestinationActID], [RelationshipTypeCode], [DocumentationCategoryID] ) VALUES (NEWID(), NEWID(), @ActID, 'pt', 'S_AllergAdver' )";
        public static string InsertAbelDentClinical_ClinicalObservation_Problem_5 = "INSERT INTO dbo.[ClinicalObservation] ([ID], [IsPositiveIndication] ) VALUES (@ActID, 1 )";

        #endregion       
        #region Payment
        public static string InsertPatientPaymentLog = "IF NOT EXISTS ( SELECT 1 FROM PatientNote WITH(NOLOCK) WHERE PatientID = @PatNum AND CategoryID = @CategoryID AND cast([Text] as nvarchar(max)) <> '@PatientNote') BEGIN  INSERT INTO PatientNote(ID,PatientID,Importance,DateTime,Text,CategoryID,SecurityOn,AuthorID,Deleted,DisplayOnOpen,IsFamilyNote,OriginalID) VALUES "
                                                                        + " (@GuidId,@PatNum,@Importance,@PaymentDate,'@PatientNote',@CategoryID,0,@User,0,0,0,NULL) END";
                                                                        
        public static string GetInsertPatientPaymentLog_Id = "SELECT top 1 ID FROM PatientNote WITH(NOLOCK) WHERE PatientID = @PatNum AND CategoryID = @CategoryID AND cast([Text] as nvarchar(max)) <> '@PatientNote' ";

        //INSERT INTO dbo.[PatientNote] ([ID], [PatientID], [Importance], [DateTime], [Text], [CategoryID], [SecurityOn], [AuthorID], [DisplayOnOpen], [IsFamilyNote] ) 
        //VALUES(newid(), 11199, 'N', GETDATE(), 'Test 2 by adit', ''672a32ac-4083-4eee-bc79-7a4c18e4dbb2'', 1, 'Adit', 0, 0 )

        public static string InsertPatientSMSCallLog = "INSERT INTO dbo.[PatientNote] ([ID], [PatientID], [Importance], [DateTime], [Text], [CategoryID], [SecurityOn], [AuthorID], [DisplayOnOpen], [IsFamilyNote] ) "
            + "VALUES(@GuidId, @PatNum, 'N', GETDATE(), '@PatientNote', @CategoryID, 1, 'Adit', 0, 0 )";

        public static string InsertPatientSMSCallLog_old = "IF NOT EXISTS(SELECT 1 FROM PatientNote WITH(NOLOCK) WHERE PatientID = @PatNum AND CategoryID = @CategoryID AND cast([Text] as nvarchar(max)) <> '@PatientNote') BEGIN Insert into PatientNote(ID,PatientID,Importance,DateTime,Text,CategoryID,SecurityOn,AuthorID,Deleted,DisplayOnOpen,IsFamilyNote) "
                + " VALUES(@GuidId, @c, @Importance, @PaymentDate, '@PatientNote', @CategoryID, 0, @User, 0, 0, 0); SELECT ID FROM PatientNote WITH(NOLOCK) WHERE PatientID = @PatNum AND CategoryID = @CategoryID AND cast([Text] as nvarchar(max)) <> '@PatientNote'; "
                + " END ELSE BEGIN SELECT ID FROM PatientNote WITH(NOLOCK) WHERE PatientID = @PatNum AND CategoryID = @CategoryID AND cast([Text] as nvarchar(max)) <> '@PatientNote';  END"; //OriginalID

        public static string InsertPatientSMSCallLog_12_10_6 = "IF NOT EXISTS(SELECT 1 FROM PatientNote WITH(NOLOCK) WHERE PatientID = @PatNum AND CategoryID = @CategoryID AND cast([Text] as nvarchar(max)) <> '@PatientNote') BEGIN Insert into PatientNote(ID,PatientID,Importance,DateTime,Text,CategoryID,SecurityOn,AuthorID,Deleted,DisplayOnOpen,IsFamilyNote) "
                + " VALUES(@GuidId, @PatNum, @Importance, @PaymentDate, '@PatientNote', @CategoryID, 0, @User, 0, 0, 0); SELECT ID FROM PatientNote WITH(NOLOCK) WHERE PatientID = @PatNum AND CategoryID = @CategoryID AND cast([Text] as nvarchar(max)) <> '@PatientNote'; "
                + " END ELSE BEGIN SELECT ID FROM PatientNote WITH(NOLOCK) WHERE PatientID = @PatNum AND CategoryID = @CategoryID AND cast([Text] as nvarchar(max)) <> '@PatientNote';  END";

        public static string CheckPaymentModeExistAsAditPay = "IF NOT EXISTS ( select 1 from msg WITH(NOLOCK) where msgmessage = 'Adit Pay' ) BEGIN INSERT INTO msg (msgtype,msgid,msgmessage,msgalias) values "
                                                                + "('PY','B','Adit Pay',''); select msgid from msg WITH(NOLOCK) where msgmessage = 'Adit Pay'  END ELSE BEGIN  select msgidentifier from msg WITH(NOLOCK) where msgmessage = 'Adit Pay' END";

        public static string CheckPaymentModeExistAsAditPayDiscount = "IF NOT EXISTS ( select 1 from DiscountAndTaxEntries WITH(NOLOCK) where EntryDescription = 'Adit Pay Discount' ) BEGIN INSERT INTO DiscountAndTaxEntries (EntryIsDiscount,EntryDescription,EntryAdjustmentAmount) values (1,'Adit Pay Discount',0); select EntryID from DiscountAndTaxEntries WITH(NOLOCK) where EntryDescription = 'Adit Pay Discount'  END ELSE BEGIN  select EntryID from DiscountAndTaxEntries WITH(NOLOCK) where EntryDescription = 'Adit Pay Discount' END ";

        
        public static string InsertPaymentTo_rwpTable = "Insert into rwp (rwpnum,rwpinscoid,rwpacct,rwpremaining,rwpamount,rwpentrydate,rwpentrytime,rwprecep,rwptype,rwppostdate,rwppayor,rwppayee,rwpnote,rwpreference,rwpbankid,rwppractid,rwpbankacct,rwpdate,rwpDiscountAmount,rwppvdr,rwpdirectdeposit)" 
                                                           + "VALUES (((select MAX(ISNULL(rwpnum,0)) AS TransID from rwp WITH(NOLOCK)) + 1),'',@PatientEHRID,0,@Amount,@date_entered,@time_entered,'',@TransactionType,GETDATE(),@PatientName,'','','','','?','',GETDATE(),0,'',0)";        

        public static string InsertPaymentTo_trnTable = "INSERT INTO trn (tno,tpid,tdate,tchgfrm,tamount,tinschg,tpamt,tptype,trecep,tinstrans,tdid,tprtr,txxxx,ttref,tprepay)"
                                                     + " VALUES(((select MAX(ISNULL(tno,0)) AS TransID from trn WITH(NOLOCK)) + 1),@PatientEHRID,@date_entered,@PatientEHRID,0,0,@Amount,'B','',0, @ProviderEHRId,0,0,'P',@Prepay)";

        public static string InsertTreatmentTo_tdiTable = "INSERT INTO tdi (ipid,itrid,idate,iitem,ijcode,itooth,isurf,idid,iresppvdr,itime,iefee,ilabfee,iinschg,itype,iinsprt,itypecodes,imodifier,iidentifier)"
                                                     + " VALUES (@PatientEHRId,'99999998',@Datenow,0,@Code,'','',@ProviderEHRId,@ProviderEHRId,0,@AmountProc,0,0,'P','100000','','',@Guid)";

        public static string InsertTransactionsto_TransactionsTable = "Insert into Transactions (TransID,Date,patID,ChartNum,Grp,ToothNum,ProvID,Code,ChartCode,Extra,Descr,Billed,Units,Surfaces,Deleted,PlanNum,MatID,Phase,Type,Appt,Applied,RespProvID,ItemNum,DatePosted,Identifier,Modifier,GroupAssociation,IsRamq)" 
                                                                                          + " Values (((select MAX(ISNULL(TransID,0)) AS TransID from Transactions WITH(NOLOCK)) + 1),@Date,@Patient,0,0,0,@ProviderID,@Code,0,NULL,@Descr,@Billed,0,'',0,0,NULL,0,'P',1,0,@ProviderID,0,@Date,NULL,'',0,0) ";


        //public static string InsertPaymentExtra_PxnTable = "INSERT INTO pxn (pxnpaynum,pxnpaidtno)"
        // + "VALUES(@rwpID,@TranID)";


        public static string InsertPaymentTo_trnTable_discount = "INSERT INTO trn (tno,tpid,tdate,tchgfrm,tamount,tinschg,tpamt,tptype,trecep,tinstrans,tdid,tprtr,txxxx,ttref,tprepay)"
                                                     + " VALUES(((select MAX(ISNULL(tno,0)) AS TransID from trn WITH(NOLOCK)) + 1),@PatientEHRID,@date_entered,@PatientEHRID,0,0,@Amount,'W','',0, @ProviderEHRId,0,0,'P',@Prepay)";

        public static string InsertPaymentTransactionsForDiscount = "INSERT into txi (txitno,txittref,IsBold,txiMsgIdentifier) Values (@TranNo,@DiscountString,0,0)";

        public static string InsertPaymentTo_trnTable_refund = "INSERT INTO trn (tno,tpid,tdate,tchgfrm,tamount,tinschg,tpamt,tptype,trecep,tinstrans,tdid,tprtr,txxxx,ttref,tprepay)"
                                                     + " VALUES(((select MAX(ISNULL(tno,0)) AS TransID from trn WITH(NOLOCK)) + 1),@PatientEHRID,@date_entered,@PatientEHRID,0,0,@Amount,'','',0, @ProviderEHRId,0,0,'C',@Prepay)";

        public static string InsertPaymentTransactionsForRefund = "INSERT into txi (txitno,txittref,IsBold,txiMsgIdentifier) Values (@TranNo,'Refund',0,0)";

        public static string GetBankIdFromFinancialTransaction = "IF EXISTS ( select 1 From FinancialTransaction WITH(NOLOCK) where ContactId = @PatientEHRId )"
                                                                 + " BEGIN  Select top 1  BankId From FinancialTransaction WITH(NOLOCK) where ContactId = 1 order by FinancialTransactionId desc  END "
                                                                 + " ELSE BEGIN SELECT  top 1 BankId FROM FinancialTransaction WITH(NOLOCK) GROUP BY BankId ORDER BY COUNT(1) DESC END ";

        public static string GetPatientBalance = "Select CASE WHEN SUM(Billed/100) IS not NULL THEN SUM(Billed/100)  else 0 END as balance  from Transactions WITH(NOLOCK) where patID = (select pid from pat WITH(NOLOCK) where pid=@PatientEHRId)";
        #endregion
        #region MedicleForm



        public static string GetAbelDentMedicleFormData = " SELECT 0 as AbelDent_Form_LocalDB_ID,LOWER(convert(varchar(50),S.Id)) as AbelDent_Form_EHR_ID,0 as AbelDent_Form_Web_ID,LOWER(convert(varchar(50),S.Id)) as AbelDent_Form_EHRUnique_ID,  "
                                                        + " S.Name as AbelDent_Form_Name,'' as Version,GETDATE() as Version_Date,(CASE WHEN S.Locked = 0 then 'True' else 'False' end) as Is_Active,'' as FormRespondentType, "
                                                        + " LOWER(convert(varchar(50),S.Id)) as CategoryId,'' as FormFlags,'' as MonthtoExpiration,GETDATE() as EHR_Entry_DateTime,getdate() as Last_Sync_Date ,"
                                                        + " getdate() as Entry_DateTime,0 as Is_Adit_Updated,0 as is_deleted,0 As Clinic_Number,1 as Service_Install_Id from Survey S WITH(NOLOCK)";

        public static string GetAbelDentMediclePartialQuestionData = " SELECT LOWER(SQR.SurveyId) as AbelDent_QuestionsTypeId,s.DisplayName as AbelDent_QuestionTypeName,'0' as AbelDent_ResponsetypeId, "
                                                                        + " LOWER(SQ.Question) as AbelDent_QuestionName, "
                                                                        + " LOWER(convert(varchar(50),SQ.Id)) as AbelDent_Question_EHR_ID,(case when SQ.Type = 2 then 'MultipleChoice' when SQ.Type = 3 then 'Checkbox' when SQ.Type = 0 then 'TextBox' when SQ.Type = 1 then 'ParagraphText' when SQ.Type = 4 then 'DropdownList' when SQ.Type = 5 then 'Scale' when SQ.Type = 6 then 'Rating' end) as InputType,'False' as Is_OptionField, "
                                                                        + " STUFF((SELECT ', ' + b.Text  "
                                                                        + "                       FROM SurveyAnswer b "
                                                                        + "                       WHERE b.QuestionId = SQ.Id "
                                                                        + "                       FOR XML PATH('')), 1, 2, '')  Options, "
                                                                        + " 0 as Clinic_Number,1 as Service_Install_Id from SurveyQuestion SQ "
                                                                        + " left join SurveyQuestionRelationship SQR on SQR.QuestionId = SQ.Id "
                                                                        + " left join Survey s on s.Id = SQR.SurveyId where SQ.Id = @AbelDent_Question_EHR_ID ";

        public static string GetAbelDentMedicleFormQuestionData = "SELECT 0 as AbelDent_FormQuestion_LocalDB_ID,'' as AbelDent_FormQuestion_Web_ID , LOWER(SQR.SurveyId) as AbelDent_Form_EHRUnique_ID, LOWER(convert(varchar(50),SQ.Id)) as AbelDent_Question_EHR_ID, LOWER(convert(varchar(5000), SQ.Id)) as AbelDent_Question_EHRUnique_ID ,"
                                                                    + " LOWER(convert(varchar(50), SQR.SurveyId)) as AbelDent_QuestionsTypeId, '' as AbelDent_QyestionTypeName,'0' as AbelDent_ResponsetypeId,SQ.Question as AbelDent_QuestionName,'' as AbelDent_Question_DefaultValue,'' as QuestionVersion, "
                                                                    + " GETDATE() as QuestionVersion_Date,(case when SQ.Type = 2 then 'MultipleChoice' when SQ.Type = 3 then 'Checkbox' when SQ.Type = 0 then 'TextBox' when SQ.Type = 1 then 'ParagraphText' when SQ.Type = 4 then 'DropdownList' when SQ.Type = 5 then 'Scale' when SQ.Type = 6 then 'Rating' end) as InputType,Convert(Bit,0) as Is_OptionField,'' as Options,Convert(Bit,0) as Is_Required, convert(varchar(10),SQR.QuestionOrder) as QuestionOrder,GETDATE() as EHR_Entry_DateTime,getdate() as Last_Sync_Date , "
                                                                    + " getdate() as Entry_DateTime,Convert(Bit,0) as Is_Adit_Updated,Convert(Bit,0) as is_deleted,Convert(Bit,0) as Is_MultiField,0 As Clinic_Number,1 AS Service_Install_Id from  [SurveyQuestion] as SQ WITH(NOLOCK) inner join  [SurveyQuestionRelationship] as SQR WITH(NOLOCK) on SQ.Id = SQR.QuestionId"; 

        public static string GetAbelDentMediclePartialResponseData = "SELECT '' as AbelDent_Response_Web_ID,SAR.Id as AbelDent_Response_EHR_ID,'' as PatientForm_Web_ID,SR.PatientId as Patient_EHR_ID,SAR.Id as responsesetuniqueidMain,SA.Id as AbelDent_Question_EHR_ID,SA.QuestionId as AbelDent_Question_EHRUnique_ID,'' as AbelDent_QuestionsTypeId,'' as AbelDent_ResponsetypeId,GETDATE() as Entry_DateTime,SR.ResponseSubmittedDate as EHR_Entry_DateTime,SA.Text as Answer_Value,0 as Clinic_Number,1 as Service_Install_Id  from SurveyAnswerResponse as SAR inner join SurveyResponse as SR on SR.Id = SAR.SurveyResponseId inner join SurveyAnswer as SA on SA.Id = SAR.AnswerId where SR.PatientId = @PatientID and SR.SurveyId = '@SurveryID' ";

        public static string InsertAbelDentMedicleResponseSetData = "Insert into SurveyResponse(Id,SurveyId,PatientId,ResponseSubmittedDate) Values (((select MAX(ISNULL(Id,0)) from SurveyResponse WITH(NOLOCK)) + 1),@AbelDent_FormUniqueId,@Patient_EHR_id,GETDATE());";

        public static string InsertAbelDentMedicleResponseData = "Insert into SurveyAnswerResponse(Id,AnswerId,SurveyResponseId,Text,SelectedValue) Values (@GuidId,@AnswerId,@ResponseID,@Text,NULL);";        


        #endregion        
        #region User
        public static string GetAbelDentUser = " select ID as User_EHR_ID,'' AS User_Local_DB_ID,'' as User_web_Id,Name as First_Name,LastName as Last_Name,Password as Password ,'' as EHR_Entry_DateTime, " +
                                             " '' as Last_Updated_DateTime,GETDATE() as LocalDb_EntryDatetime,Active as Is_active,0 as is_deleted,0 as Is_Adit_Updated,0 as Clinic_Number,1 as Service_Install_Id from Member ";

        public static string GetAbelDentUserLoginId = "IF NOT EXISTS(select 1 from Member where Name = 'Adit') BEGIN insert into Member(ID,FirstName,LastName,Name,Description,Password,Active,AssociatedPersonID,OMDUserName,OMDPassword,PreferredLanguage,PasswordHash2,Salt2) " +
                                                            " values (@UniqID,'Adit','','Adit','',NULL,1,@AssociateID,NULL,NULL,'Eng',NULL,@SaltID); select ID from Member where Name = 'Adit'; " +
                                                            " END ELSE BEGIN select ID from Member where Name = 'Adit'; END";

        public static string GetAbelDentUserLoginId_14_4_2 = "IF NOT EXISTS(select 1 from Member where Name = 'Adit') BEGIN insert into Member(ID,FirstName,LastName,Name,Description,Password,Active,AssociatedPersonID,OMDUserName,OMDPassword,PreferredLanguage) " +
                                                            " values (@UniqID,'Adit','','Adit','',NULL,1,@AssociateID,NULL,NULL,'Eng'); select ID from Member where Name = 'Adit'; " +
                                                            " END ELSE BEGIN select ID from Member where Name = 'Adit'; END";

        public static string GetAbelDentUserLoginId_12_10_6 = "IF NOT EXISTS(select 1 from Member where Name = 'Adit') BEGIN insert into Member(ID,FirstName,LastName,Name,Description,Password,Active,AssociatedPersonID,OMDUserName,OMDPassword) " +
                                                            " values (@UniqID,'Adit','','Adit','',NULL,1,@AssociateID,NULL,NULL); select ID from Member where Name = 'Adit'; " +
                                                            " END ELSE BEGIN select ID from Member where Name = 'Adit'; END";

        #endregion
        #region Create Funcation in DB
        public static string CreateFungetNumericValue = "CREATE FUNCTION dbo.getNumericValue "
                                                            + " ( @inputString VARCHAR(256) )"
                                                            + " RETURNS VARCHAR(256) "
                                                            + " AS "
                                                            + " BEGIN "
                                                            + "  DECLARE @integerPart INT "
                                                            + "  SET @integerPart = PATINDEX('%[^0-9]%', @inputString) "
                                                            + "  BEGIN "
                                                            + "    WHILE @integerPart > 0 "
                                                            + "    BEGIN "
                                                            + "      SET @inputString = STUFF(@inputString, @integerPart, 1, '' ) "
                                                            + "      SET @integerPart = PATINDEX('%[^0-9]%', @inputString) "
                                                            + "    END "
                                                            + "  END "
                                                            + "  RETURN ISNULL(@inputString,0) "
                                                            + " END "
                                                            + " GO";
        #endregion


        #region Appt_15
        public static string GetAbelDentPatientData_15 = " Select DISTINCT PA.pid AS Patient_EHR_ID,'' AS Patient_Web_ID,PA.pfname AS First_name,PA.plname AS Last_name, "
                                                             + " PA.pinitial AS Middle_Name,PA.pmrmrs AS Salutation,case when PA.pinactive = 0 then 'A' else 'I' end AS [Status], (case WHEN PA.pgender = 'M' THEN 'MALE' WHEN "
                                                                 + " PA.pgender = 'F' THEN 'FEMALE' WHEN PA.pgender = '' THEN '' END ) as SEX,'' AS MaritalStatus,'' AS school, "
                                                            + " (CASE When PA.pbirth IS NULL then CONVERT(datetime, '17530101', 112) else PA.pbirth end) AS Birth_Date, AA.infemail as Email, ISNULL(AA.infmobile,'') AS Mobile, ISNULL(PA.pphone, '') AS Home_Phone, ISNULL(PA.pworkphn, '') AS Work_Phone, "
                                                              + " ISNULL(PA.pstreetadr, '') AS Address1, ISNULL(PA.pstreetadr2, '') AS Address2, CT.cdesc AS[City],'' AS[State],ISNULL(PA.ppostal,'') AS Zipcode, "
                                                              + " (case WHEN P2.pinactive is NULL then '' when P2.pinactive = 1 then '' ELSE 'Active' end) AS ResponsibleParty_Status, ISNULL(HH.Pat_AR30, 0) AS ThirtyDay, ISNULL(HH.Pat_AR60, 0) AS SixtyDay, ISNULL(HH.Pat_AR90, 0) AS NinetyDay, ISNULL(HH.Pat_AR90_Plus, 0) AS Over90, ISNULL(HH.Pat_Total, 0) AS CurrentBal,'' as remaining_benefit,'' as used_benefit, PA.paltid AS ssn,AA.infemployer as employer, "
                                                             + " FVisit.FirstVisitDate AS FirstVisit_Date,ISNULL(PrimIns.PrimaryInsuranceId,'') AS Primary_Insurance, ISNULL(PrimIns.PrimaryInsuranceCompanyName, '') AS Primary_Insurance_CompanyName, PrimIns.PrimarySubcriberId as Primary_Ins_Subscriber_ID, ISNULL(PrimIns.Prim_Ins_Company_Phonenumber,'') as Prim_Ins_Company_Phonenumber,ISNULL(SecIns.SecondaryInsuranceId,'') AS Secondary_Insurance, ISNULL(SecIns.SecondaryInsuranceCompanyName, '') AS Secondary_Insurance_CompanyName, ISNULL(SecIns.Sec_Ins_Company_Phonenumber, '') as Sec_Ins_Company_Phonenumber, ( case WHEN PA.pinactive = 1 then 'InActive' ELSE 'Active' end ) AS EHR_Status, 0 AS Is_Deleted, "
                                                                + " (CASE WHEN PA.pchargeto = 0 then PA.pid ELSE PA.pchargeto END) AS Guar_ID, SecIns.SecondarySubcriberId as Secondary_Ins_Subscriber_ID, ISNULL(PA.pdentist,'') AS Pri_Provider_ID, ISNULL(PA.phygienist, '') AS Sec_Provider_ID, (CASE WHEN AA.infallowtextmsg = 1 then 'Y' else 'N' end) AS ReceiveSms, (CASE WHEN AA.infallownewsemail = 1 then 'Y' else 'N' end) AS ReceiveEmail, "
                                                                + "  LVisit.LastVisitDate AS LastVisit_Date, (SELECT MIN (adate) From apt WITH(NOLOCK) Where adate > GETDATE() AND astatus NOT IN('R') and apid = PA.pid) AS nextvisit_date, "
                                                                + " AB.DueDate AS due_date,'' AS collect_payment, '' AS EHR_Entry_DateTime, 0 AS Is_Adit_Updated, AA.infgivenname AS preferred_name,  "
                                                            + " 1 As InsUptDlt,'' as ReceiveVoiceCall,CASE WHEN PA.pnativetongue = 'E' THEN 'English' WHEN PA.pnativetongue = 'F' THEN 'French' WHEN PA.pnativetongue = '' THEN 'ENGLISH' END AS PreferredLanguage, AA.infnote AS Patient_note, "
                                                            + " P2.pid AS responsiblepartyid, P2.pfname AS ResponsibleParty_First_Name, P2.plname AS ResponsibleParty_Last_Name,'' AS responsiblepartyssn, P2.pbirth AS responsiblepartybirthdate,'' AS EmergencyContactId,'' AS EmergencyContact_First_Name, "
                                                            + " '' AS EmergencyContact_Last_Name,'' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' AS driverlicense, "
                                                            + " '' AS groupid,0 as Clinic_Number, 1 as Service_Install_Id,0 as Patient_LocalDB_ID from pat PA WITH(NOLOCK) "
                                                            + " LEFT JOIN inf AA WITH(NOLOCK) on AA.infpid = PA.pid "
                                                            + " LEFT JOIN  pat P2 WITH(NOLOCK) on PA.pchargeto = P2.pid "
                                                            + " LEFT JOIN  cty CT WITH(NOLOCK) on CT.ccode = PA.pcitycode "
                                                            + " LEFT JOIN (select DISTINCT p.pid as PatientEHRId , CAST(NS.inscoid AS varchar) as PrimaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as PrimarySubcriberId, NS.insname as PrimaryInsuranceCompanyName, NS.insphone as Prim_Ins_Company_Phonenumber from pat as p WITH(NOLOCK) "
                                                            + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                            + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 1) as PrimIns on PrimIns.PatientEHRId = PA.pid "
                                                            + " LEFT JOIN(select DISTINCT p.pid as PatientEHRId , NS.inscoid as SecondaryInsuranceId, CASE when XI.ixisubpid = 0 then p.pid else XI.ixisubpid end as SecondarySubcriberId, NS.insname as SecondaryInsuranceCompanyName , NS.insphone as Sec_Ins_Company_Phonenumber from pat as p WITH(NOLOCK) "
                                                            + " LEFT OUTER JOIN ixi as XI WITH(NOLOCK) on XI.ixipid = p.pid "
                                                            + " LEFT OUTER JOIN ins as NS WITH(NOLOCK) on NS.inscoid = XI.ixiplanid where XI.ixiplno = 2) as SecIns on SecIns.PatientEHRId = PA.pid "
                                                            + " LEFT JOIN(SELECT Appt.apid as Patient_EHR_Id, MAX(Appt.adate) as LastVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null AND Appt.adate <= CONVERT(DATE, GETDATE()) Group by Appt.apid ) AS LVisit on LVisit.Patient_EHR_Id =  PA.pid "
                                                            + " LEFT JOIN (SELECT Appt.apid as Patient_EHR_Id, MIN(Appt.adate) as FirstVisitDate FROM apt Appt WHERE appt.astatus != 'R' AND appt.aidentifier is not null Group by Appt.apid) AS FVisit on FVisit.Patient_EHR_Id =  PA.pid "
                                                            + " LEFT JOIN (select PP.pid, CONVERT(varchar, ( case when PP.paptinvl > 12 then (DATEADD(Day, CAST(PP.paptinvl AS INT),  AA.DueDate)) else ((DATEADD(Month, CAST(PP.paptinvl AS INT), AA.DueDate)))END)) +'@'+ CONVERT(varchar, rl.msgmessage) + '@' + CAST(rl.msgidentifier AS varchar) AS DueDate   from pat PP WITH(NOLOCK) "
                                                            + "  INNER JOIN(SELECT apid, Max(ap1.adate) AS DueDate FROM  apt ap1 WITH(NOLOCK) Where aconfstat = 'D' AND astatus  != 'R' GROUP BY apid) AS AA ON PP.pid = AA.apid "
                                                                + "  LEFT JOIN msg rl WITH(NOLOCK) on rl.msgid = PP.paptinvl where rl.msgid != '' and rl.msgid != 0 and rl.msgtype = 'PATAI' and ISNUMERIC(rl.msgid) = 1 and rl.msgid > 0) as AB on AB.pid = PA.pid "
                                                                   + " LEFT JOIN(select tpid as PatientEHRId, "
                                                                   + "   SUM(CASE WHEN ageDiff < 30  THEN Pat ELSE 0 END) as 'Pat_AR30', "
                                                                    + "  SUM(CASE WHEN ageDiff >= 30 and ageDiff < 60  THEN Pat ELSE 0 END) as 'Pat_AR60', "
                                                                    + "  SUM(CASE WHEN ageDiff >= 60 and ageDiff < 90  THEN Pat ELSE 0 END) as 'Pat_AR90', "
                                                                    + "  SUM(CASE WHEN ageDiff >= 90 THEN Pat ELSE 0 END) as 'Pat_AR90_Plus', "
                                                                    + "   SUM(Pat) as Pat_Total "
                                                                     + "   from(SELECT A.tpid, DATeDIFF(day, COALESCE(trn.tdate, A.BilledDate), CONVERT(date, GETDATE())) AS ageDiff,(SUM(A.tamount) - Sum(A.tpamt))/100.00 as Pat "
                                                                        + "  FROM AccountFinancialView AS A WITH(NOLOCK) "
                                                                        + "    LEFT JOIN trn trn WITH(NOLOCK) ON A.billTno = trn.tno "
                                                                        + "  WHERE  ((A.tdate <= GETDATE() AND A.tprepay<> '1')) "
                                                                        + "	GROUP BY A.tpid,trn.tdate,A.BilledDate) as a group by tpid) as HH on HH.PatientEHRId = PA.pid ";


        public static string GetAbelDentAppointmentData_15 = "     SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Appt_LocalDB_ID,AP.aidentifier AS Appt_EHR_ID,'' AS Appt_Web_ID, CT.plname AS Last_Name,CT.pfname AS First_Name,CT.pinitial AS MI,ISNULL(CT.pphone,'') AS Home_Contact,  ISNULL(II.infmobile,'') AS Mobile_Contact,II.infemail AS Email, SUBSTRING(( ISNULL(CT.pstreetadr,'' ) + ' ' + ISNULL(CT.pstreetadr2,'' ) ),0,200) AS [Address], CA.cdesc AS City,'' AS [ST],ISNULL(CT.pcitycode,'') AS Zip, "
                                                            + " AP.achair AS Operatory_EHR_ID,SS.scurrmess AS Operatory_Name ,AP.adid AS Provider_EHR_ID,PD.dname AS Provider_Name,AP.apwork + ' ' +  AP.apwrk2 AS Comment,(CASE When CT.pbirth IS NULL then CONVERT(datetime,'17530101',112) else CT.pbirth end) AS Birth_Date, "
                                                            + " '' AS ApptType_EHR_ID, '' AS ApptType,AP.apwork AS WorkToDo , CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS Appt_DateTime, (CASE WHEN Ap.aconfstat is null then '' WHEN apsid = '' THEN 'bl' WHEN apsid = '\' then 'ss' WHEN apsid = '\\' then 'ds' ELSE apsid END) as appointment_status_ehr_key, (CASE when Ap.aconfstat is null then '' else APS.apsdesc end) as Appointment_status,"
                                                            + " DATEADD(mi,CONVERT(INT,ap.atimereq * @_minutesInUnit),CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108))) AS Appt_EndDateTime, "
                                                            + " AP.astatus AS[Status],'new' AS Patient_Status,'EHR' as Is_Appt, (CASE WHEN apsdesc = 'Confirmed' THEN 'Y' WHEN apsdesc = 'Preconfirmed' then 'P' ELSE '' END) as confirmed_status_ehr_key,(CASE WHEN apsid = 'Y' THEN 'Confirmed' WHEN apsid = 'P' THEN 'Preconfirmed' ELSE '' END) as confirmed_status, "
                                                            + " GETDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated, AP.apid AS patient_ehr_id,'False' AS is_deleted,0 AS is_Status_Updated_From_Web, getdate() as Entry_DateTime,getdate() as Last_Sync_Date, CASE  WHEN ASAP.IsASAP = 0 THEN 'False' WHEN ASAP.IsASAP IS NULL THEN 'False' ELSE 'True' END as is_asap "
                                                            + " FROM apt AP WITH(NOLOCK)"
                                                            + " LEFT JOIN pat CT WITH(NOLOCK) ON CT.pid = AP.apid "
                                                            + " LEFT JOIN cty CA WITH(NOLOCK) on CA.ccode = CT.pcitycode "
                                                            + " LEFT JOIN inf II WITH(NOLOCK) on II.infpid = CT.pid"
                                                            + " LEFT JOIN dnt PD WITH(NOLOCK) ON PD.did = AP.adid"
                                                            + " LEFT JOIN aps APS WITH(NOLOCK) on APS.apsid = AP.aconfstat"
                                                            //+ " LEFT JOIN WorkToDoGlobal WDG WITH(NOLOCK) ON WDG.Name = AP.apwork"
                                                            + " LEFT JOIN sys SS WITH(NOLOCK) ON substring(CAST(SS.snum as varchar(10)),4,10) = AP.achair  "
                                                            + " LEFT JOIN(SELECT AP.aidentifier, AP.apid as PatientID, CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS APt_DateTime, "
                                                            + " (CASE WHEN SR.Inactive = 0 and(AP.adate BETWEEN SR.StartDate AND SR.EndDate) THEN 1 "
                                                            + "    WHEN SR.Inactive = 0 AND(AP.adate > SR.StartDate AND AP.adate > SR.EndDate) THEN 1 "
                                                            + "      ELSE 0 "
                                                            + "  END ) AS IsASAP "
                                                            + " FROM apt AP "
                                                            + " INNER JOIN SchedulingRequest SR on sR.PID = AP.apid AND SR.Inactive = 0 "
                                                            + " where "
                                                            + " (CASE WHEN SR.Inactive = 0 and (AP.adate BETWEEN SR.StartDate AND SR.EndDate) THEN 1 "
                                                            + " 	WHEN SR.Inactive = 0 AND (AP.adate > SR.StartDate AND AP.adate > SR.EndDate) THEN 1 "
                                                            + " 	  ELSE 0 "
                                                            + " END ) > 0 ) AS ASAP ON ASAP.aidentifier = AP.aidentifier "
                                                            + " WHERE CT.pid <> 0  AND CT.plname<> '' And Not ((CT.pfname = '' OR CT.plname = '')) "
                                                            + " and AP.adate > @ToDate AND  AP.aidentifier is Not null and AP.aidentifier !='00000000-0000-0000-0000-000000000000' and  SS.snum like 'COL%'";

        public static string GetAbelDentAppointmentDataByApptID_15 = "     SELECT 0 AS Clinic_Number,1 as Service_Install_Id,0 AS Appt_LocalDB_ID,AP.aidentifier AS Appt_EHR_ID,'' AS Appt_Web_ID, CT.plname AS Last_Name,CT.pfname AS First_Name,CT.pinitial AS MI,ISNULL(CT.pphone,'') AS Home_Contact,  ISNULL(II.infmobile,'') AS Mobile_Contact,II.infemail AS Email, SUBSTRING(( ISNULL(CT.pstreetadr,'' ) + ' ' + ISNULL(CT.pstreetadr2,'' ) ),0,200) AS [Address], CA.cdesc AS City,'' AS [ST],ISNULL(CT.pcitycode,'') AS Zip, "
                                                            + " AP.achair AS Operatory_EHR_ID,SS.scurrmess AS Operatory_Name ,AP.adid AS Provider_EHR_ID,PD.dname AS Provider_Name,AP.apwork + ' ' +  AP.apwrk2 AS Comment,(CASE When CT.pbirth IS NULL then CONVERT(datetime,'17530101',112) else CT.pbirth end) AS Birth_Date, "
                                                            + " '' AS ApptType_EHR_ID, '' AS ApptType,AP.apwork AS WorkToDo , CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS Appt_DateTime, (CASE WHEN Ap.aconfstat is null then '' WHEN apsid = '' THEN 'bl' WHEN apsid = '\' then 'ss' WHEN apsid = '\\' then 'ds' ELSE apsid END) as appointment_status_ehr_key, (CASE when Ap.aconfstat is null then '' else APS.apsdesc end) as Appointment_status,"
                                                            + " DATEADD(mi,CONVERT(INT,ap.atimereq * @_minutesInUnit),CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108))) AS Appt_EndDateTime, "
                                                            + " AP.astatus AS[Status],'new' AS Patient_Status,'EHR' as Is_Appt, (CASE WHEN apsdesc = 'Confirmed' THEN 'Y' WHEN apsdesc = 'Preconfirmed' then 'P' ELSE '' END) as confirmed_status_ehr_key,(CASE WHEN apsid = 'Y' THEN 'Confirmed' WHEN apsid = 'P' THEN 'Preconfirmed' ELSE '' END) as confirmed_status, "
                                                            + " GETDATE() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated, AP.apid AS patient_ehr_id,'False' AS is_deleted,0 AS is_Status_Updated_From_Web, getdate() as Entry_DateTime,getdate() as Last_Sync_Date,CASE  WHEN ASAP.IsASAP = 0 THEN 'False' WHEN ASAP.IsASAP IS NULL THEN 'False' ELSE 'True' END as is_asap "
                                                            + " FROM apt AP WITH(NOLOCK)"
                                                            + " LEFT JOIN pat CT WITH(NOLOCK) ON CT.pid = AP.apid "
                                                            + " LEFT JOIN cty CA WITH(NOLOCK) on CA.ccode = CT.pcitycode "
                                                            + " LEFT JOIN inf II WITH(NOLOCK) on II.infpid = CT.pid"
                                                            + " LEFT JOIN dnt PD WITH(NOLOCK) ON PD.did = AP.adid"
                                                            + " LEFT JOIN aps APS WITH(NOLOCK) on APS.apsid = AP.aconfstat"
                                                            //+ " LEFT JOIN WorkToDoGlobal WDG WITH(NOLOCK) ON WDG.Name = AP.apwork"
                                                            + " LEFT JOIN sys SS WITH(NOLOCK) ON substring(CAST(SS.snum as varchar(10)),4,10) = AP.achair  "
                                                            + " LEFT JOIN(SELECT AP.aidentifier, AP.apid as PatientID, CONVERT(DATETIME, CONVERT(CHAR(8), AP.adate, 112) + ' ' + CONVERT(CHAR(8), AP.atime, 108)) AS APt_DateTime, "
                                                            + " (CASE WHEN SR.Inactive = 0 and(AP.adate BETWEEN SR.StartDate AND SR.EndDate) THEN 1 "
                                                            + "    WHEN SR.Inactive = 0 AND(AP.adate > SR.StartDate AND AP.adate > SR.EndDate) THEN 1 "
                                                            + "      ELSE 0 "
                                                            + "  END ) AS IsASAP "
                                                            + " FROM apt AP "
                                                            + " INNER JOIN SchedulingRequest SR on sR.PID = AP.apid AND SR.Inactive = 0 "
                                                            + " where "
                                                            + " (CASE WHEN SR.Inactive = 0 and (AP.adate BETWEEN SR.StartDate AND SR.EndDate) THEN 1 "
                                                            + " 	WHEN SR.Inactive = 0 AND (AP.adate > SR.StartDate AND AP.adate > SR.EndDate) THEN 1 "
                                                            + " 	  ELSE 0 "
                                                            + " END ) > 0 ) AS ASAP ON ASAP.aidentifier = AP.aidentifier "
                                                            + " WHERE AP.aidentifier = @Appt_EHR_ID and CT.pid <> 0  AND CT.plname<> '' And Not ((CT.pfname = '' OR CT.plname = '')) "
                                                            + " and AP.adate > @ToDate AND  AP.aidentifier is Not null and AP.aidentifier !='00000000-0000-0000-0000-000000000000' and  SS.snum like 'COL%'"; //@ToDate
        #endregion
        #region PatientAging_15
        public static string GetAbelDentPatientBalance_15 = " select tpid, "
        + " SUM(CASE WHEN ageDiff< 30  THEN Pat ELSE 0 END) as 'Pat_AR30', "
        + " SUM(CASE WHEN ageDiff >= 30 and ageDiff < 60  THEN Pat ELSE 0 END) as 'Pat_AR60', "
	    + " SUM(CASE WHEN ageDiff >= 60 and ageDiff < 90  THEN Pat ELSE 0 END) as 'Pat_AR90', "
	    + " SUM(CASE WHEN ageDiff >= 90 THEN Pat ELSE 0 END) as 'Pat_AR90_Plus', "
	    + " SUM(Pat) as Pat_Total "
            + " from "
            + " ( "
            + " SELECT A.tpid, "
        + " DATeDIFF(day, COALESCE(trn.tdate, A.BilledDate), CONVERT(date, GETDATE())) AS ageDiff, "
  	    + " (SUM(A.tamount) - Sum(A.tpamt))/100.00 as Pat "
        + " FROM AccountFinancialView AS A WITH(NOLOCK) "
        + " LEFT JOIN trn trn WITH(NOLOCK) ON A.billTno = trn.tno "
        + " WHERE  ((A.tdate <= GETDATE() AND A.tprepay<> '1'))	"
	    + " GROUP BY A.tpid,trn.tdate,A.BilledDate) as a group by tpid ";
        #endregion
        #region Patient Medication_15
        public static string GetAbelDentPatientMedicationData_15 = "select Rx.PatientID as Patient_EHR_ID, "
                                                                            + " RxUserDefinedDrug.ID as Medication_EHR_ID, "
                                                                            + " Rx.ID as PatientMedication_EHR_ID, "
                                                                            + " RxDetails.DrugName as Medication_Name, "
                                                                            + " 'UserDefinedDrug' as Medication_Type, "
                                                                            + " RxDetails.Notes as Medication_Note, "
                                                                            + " RxDetails.Quantity as Drug_Quantity, "
                                                                            + " Rx.PrescribedBy as Provider_EHR_ID, "
                                                                            + "  cast(Rx.WrittenDate as smalldatetime) as Start_Date, "
                                                                            + " '' as End_Date, "
                                                                            + " getdate() AS EHR_Entry_DateTime, "
                                                                            + " '' as Patient_Notes, "
                                                                            + " case when Rx.IsDeleted = 1 then 'True' else 'False' end AS is_deleted, "
                                                                            + " '0' as Clinic_Number, "
                                                                            + "  case when Rx.IsActive = 0 then 'True' else 'False' end As Is_Active "
                                                                            + "  from Rx "
                                                                            + " left join RxDetails on Rx.RxDetailsID = RxDetails.ID "
                                                                            + " left join RxUserDefinedDrug on RxUserDefinedDrug.Name = RxDetails.DrugName ";

        public static string GetAbelDentMedicationMaster_15 = " Select ID as Medication_EHR_ID, Name as Medication_Name,  '' as Medication_Description,   '' as Medication_Notes,  '' as Medication_Sig, '' as Medication_Parent_EHR_ID,  'UserDefinedDrug' as Medication_Type,  '' as Allow_Generic_Sub, Strength as   Drug_Quantity, '' as Refills, Case when IsActive = 1 then 'True' else 'False' end as Is_Active, "
        + " getdate() as EHR_Entry_DateTime,'' as Medication_Provider_ID,  0 as is_deleted,  0 as Is_Adit_Updated,  '0' as Clinic_Number "
        + " from  RxUserDefinedDrug ";

        #endregion
        #region Document_15
        
        public static string InsertAbelDentDocument_15 = "INSERT INTO dbo.[Document] ([ID], [Source], [Description], [State], [FileName], [DocTypeID],DateTimeStamp,PatientId,ProviderId,Note,ReceiptDate,CreationDate) "
+ " VALUES(@DocumentID, 'Document via Adit', '','Active', @FileName, @DocumentTypeID, GETDATE(), @PatientEHRID,'','', DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())),DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))) ";

        #endregion
        #region Insert Allergy_15
        public static string GetAbelDentAllergen_15 = "select ID,Name,IsClinicallySignificant from Allergen where Name = '@AllergyTextCapi' ";

        public static string InsertAbelDentAllergy_15 = " Insert into Allergy  (PatientId,Allergen,Severity,IsClinicallySignificant,IsActive,IsDeleted,LastUpdatedByProviderId,LastReviewedByProviderId,LastUpdatedByProviderOn,IsNegative) "
                                                    + " VALUES(@PatientEHRID,@AllergenName,'Moderate',1,1,0,'','',GETDATE() AT TIME ZONE N'@Timezone',0) ";
        #endregion
        #region Patient Disease_15
        public static string GetAbelDentProblems_15 = "Select 0 AS Clinic_Number,1 as Service_Install_Id,ProblemID as Disease_EHR_ID,'' AS Disease_LocalDB_ID,'' as Disease_Web_ID,DisplayCode as Disease_Name,'P' as Disease_Type, 0 as is_deleted,0 as Is_Adit_Updated FROM ClinicalProblemAct ";

        public static string GetAbelDentAllergies_15 = "Select 0 AS Clinic_Number,1 as Service_Install_Id,CA.ID as Disease_EHR_ID,'' AS Disease_LocalDB_ID,'' as Disease_Web_ID,CA.Name as Disease_Name,'A' as Disease_Type, 0 as is_deleted,0 as Is_Adit_Updated  from Allergen CA WITH(NOLOCK);";

        public static string GetAbelDentPatientDiseaseData_15 = " SELECT '' as PatientDisease_LocalDB_ID,0 AS Clinic_Number,1 as Service_Install_Id,AA.Id AS Disease_EHR_ID, AA.PatientId AS Patient_EHR_ID,'' as PatientDisease_Web_ID, AA.Allergen as  Disease_Name,'A' as Disease_Type,0 as is_deleted,getdate() AS EHR_Entry_DateTime,0 AS Is_Adit_Updated,getdate() as Last_Sync_Date "
                   + " FROM Allergy AA WITH(NOLOCK) "
                  + " left join Allergen AG on AG.Name = AA.Allergen";

        #endregion        
        #region MedicleForm_15

        public static string GetAbelDentMedicleFormData_15 = "SELECT 0 as AbelDent_Form_LocalDB_ID,LOWER(convert(varchar(50),S.Id)) as AbelDent_Form_EHR_ID,0 as AbelDent_Form_Web_ID,LOWER(convert(varchar(50),S.Id)) as AbelDent_Form_EHRUnique_ID,  "
                                                        + " S.Name as AbelDent_Form_Name,'' as Version,GETDATE() as Version_Date,(CASE WHEN S.IsLocked = 0 then 'True' else 'False' end) as Is_Active,'' as FormRespondentType, "
                                                        + " LOWER(convert(varchar(50),S.Id)) as CategoryId,'' as FormFlags,'' as MonthtoExpiration,GETDATE() as EHR_Entry_DateTime,getdate() as Last_Sync_Date , "
                                                        + " getdate() as Entry_DateTime,0 as Is_Adit_Updated,0 as is_deleted,0 As Clinic_Number,1 as Service_Install_Id from HealthHistory S WITH(NOLOCK)";

        public static string GetAbelDentMediclePartialQuestionData_15 = " SELECT SQR.HealthHistoryId as AbelDent_QuestionsTypeId, s.Name as AbelDent_QuestionTypeName,'0' as AbelDent_ResponsetypeId, "
                                 + "  LOWER(SQ.Question) as AbelDent_QuestionName, "
                                 + "  LOWER(convert(varchar(50),SQ.Id)) as AbelDent_Question_EHR_ID,(case when SQ.Type = 1 then 'Checkbox' when SQ.Type = 3 then 'MultipleChoice' when SQ.Type = 0 then 'TextBox'  end) as InputType,'False' as Is_OptionField, "
                                 + "  STUFF((SELECT ', ' + b.Text "
                                                + "  FROM HealthHistoryAnswerOption b "
                                                + "  WHERE b.QuestionId = SQ.Id and b.Type != 3 "
                                                + "  FOR XML PATH('')), 1, 2, '')  Options, "
                                 + "  0 as Clinic_Number,1 as Service_Install_Id from HealthHistoryQuestion SQ "
                                 + "  left join HealthHistoryQuestionRelationship SQR on SQR.QuestionId = SQ.Id "
                                 + "  left join HealthHistory s on s.Id = SQR.HealthHistoryId where SQ.Id = @AbelDent_Question_EHR_ID ";                                                                         

        public static string GetAbelDentMedicleFormQuestionData_15 = "SELECT 0 as AbelDent_FormQuestion_LocalDB_ID,'' as AbelDent_FormQuestion_Web_ID , LOWER(SQR.HealthHistoryId) as AbelDent_Form_EHRUnique_ID, LOWER(convert(varchar(50),SQ.Id)) as AbelDent_Question_EHR_ID, LOWER(convert(varchar(5000), SQ.Id)) as AbelDent_Question_EHRUnique_ID , "

        + " LOWER(convert(varchar(50), SQR.HealthHistoryId)) as AbelDent_QuestionsTypeId, '' as AbelDent_QyestionTypeName,'0' as AbelDent_ResponsetypeId,SQ.Question as AbelDent_QuestionName,'' as AbelDent_Question_DefaultValue,'' as QuestionVersion, "
        + " GETDATE() as QuestionVersion_Date,(case when SQ.Type = 1 then 'Checkbox' when SQ.Type = 3 then 'MultipleChoice' when SQ.Type = 0 then 'TextBox'  end) as InputType,Convert(Bit,0) as Is_OptionField,'' as Options,Convert(Bit,0) as Is_Required, convert(varchar(10),SQR.QuestionOrder) as QuestionOrder,GETDATE() as EHR_Entry_DateTime,getdate() as Last_Sync_Date , "
        + " getdate() as Entry_DateTime,Convert(Bit,0) as Is_Adit_Updated,Convert(Bit,0) as is_deleted,Convert(Bit,0) as Is_MultiField,0 As Clinic_Number,1 AS Service_Install_Id "
        + " from HealthHistoryQuestion as SQ WITH(NOLOCK) inner join  HealthHistoryQuestionRelationship as SQR WITH(NOLOCK) on SQ.Id = SQR.QuestionId";

        public static string GetAbelDentMediclePartialResponseData_15 = "SELECT '' as AbelDent_Response_Web_ID,SAR.ID as AbelDent_Response_EHR_ID,'' as PatientForm_Web_ID,SR.PatientId as Patient_EHR_ID,SAR.Id as responsesetuniqueidMain,SA.Id as AbelDent_Question_EHR_ID,SA.QuestionId as AbelDent_Question_EHRUnique_ID,'' as AbelDent_QuestionsTypeId,'' as AbelDent_ResponsetypeId,GETDATE() as Entry_DateTime, "
                            + " SR.Date as EHR_Entry_DateTime,SA.Text as Answer_Value,0 as Clinic_Number,1 as Service_Install_Id "
                            + " from HealthHistoryAnswerResponse as SAR "
                            + " inner join HealthHistoryResponse as SR on SR.Id = SAR.ResponseId "
                            + " inner join HealthHistoryAnswerOption as SA on SA.Id = SAR.AnswerId where SR.PatientId = @PatientID and SR.HealthHistoryId = '@SurveryID' ";        

        public static string InsertAbelDentHealthHistoryResponse_15 = "Insert Into HealthHistoryResponse (HealthHistoryId,PatientId,Date,IsLocked,IsEmailed,IsActive,IsWebLocked)"
                + "values(@AbelDent_FormUniqueId,@Patient_EHR_id, GETDATE() AT TIME ZONE N'@Timezone',0,1,1,1);Select @@Identity";

        public static string InsertAbelDentHealthHistoryAnswerResponse_15 = "Insert into HealthHistoryAnswerResponse (AnswerId,ResponseId,Text) VALUES (@AnswerId,@ResponseID,@Text);Select @@Identity";

        public static string InsertAbelDentHealthHistoryReview_15 = "Insert Into HealthHistoryReview(Id,ReviewDate,IssignedOff) values ('@ResponseEHRID',GETDATE() AT TIME ZONE N'@Timezone',0)";        

        public static string InsertAbelDentHealthHistoryReviewDetails_15 = " Insert Into HealthHistoryReviewDetail (HealthHistoryReviewId,HealthHistoryId,QuestionId,IsQuestionResponseClinicallySignificant,IsMoreInformationNeeded) "
                    + "VALUES(@HealthHistoryReviewId, @HealthHistoryId, @QuestionId,0,1) ";

        public static string GetAbelDentMedicationQuestionDate_15 = "select Id from HealthHistoryAnswerOption where Text LIKE '%@AnswerText%' and QuestionId = '@QuestionEHRID'";

        #endregion
        #region Insert Medication Data_15

        public static string GetMedicationRxUserDefinedDrug_15 = "Select name,Strength from RxUserDefinedDrug where Name = '@MedicationName' ";

        public static string InsertAbelDentRxDetails_Medication_15 = "Insert into RxDetails (ID,DrugName,Strength,MedicationType,Dose,Frequency,Quantity,Refills,Notes,IsPRN,Condition,IsControlled,CanSubstitute,Duration) "
                                + " VALUES (@UniqeID,'@DrugName','@Strength',0,NULL,'','',0,'@Notes',0,'',0,1,'')";

        public static string InsertAbelDentRx_Medication_15 = " Insert into Rx  (ID,PatientID,RxDetailsID,Type,IsActive,WrittenBy,WrittenDate,ReviewedBy,ReviewedDate,PrintedBy,PrintedDate,IsDeleted,PrescribedBy,ReasonForReprinting) "
            + " VALUES(newid(), @PatientEHRID, @RxDetailsUniqID,0,1, @UserID, GETDATE(), NULL, NULL, @UserID, GETDATE(),0, NULL, NULL) ";

        #endregion

    }
}
