using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pozative.QRY
{
    public class SynchEasyDentalQRY
    {

        #region Appointment
        public static string GetEasyDentalAppointmentData = " Select AppointmentId as appointment_id,PatientName as patient_name,Date as appointment_date,Time as Start_hour,AppointLen as length,ProviderId as provider_id,'' as comment,OperatoryId as op_id,ApptType as ApptType_EHR_ID,PatientId as patId,Status as appointment_status_ehr_key,ModifiedDate as EHR_Entry_DateTime,Flag as appt_flag , NewPatAddressId as addrid,ap.Phone as patient_phone,ad.Street as Address1,ad.Street2 as Address2,ad.City as City,ad.State as State,ad.Zip as Zipcode,'' as provider_first_name,'' as provider_last_name,'' as birth_date, '' as op_title,'' as notetext,'' as ApptType_Name,'' as Appointment_Status,'' as patMobile,'' as patEmail from {pj Appt_dat ap LEFT OUTER JOIN Address_dat ad on ap.NewPatAddressId = ad.Addressid} WHERE PatientName <> '' and RscType = 3 and operatoryId <> '' and date > ?  Order by date desc";

        public static string GetEasyDentalAppointmentEhrIds = " Select AppointmentId as Appt_EHR_ID from {pj Appt_dat ap LEFT OUTER JOIN Address_dat ad on ap.NewPatAddressId = ad.Addressid} WHERE PatientName <> '' and RscType = 3 and operatoryId <> '' and date > ? Order by date desc";

        public static string GetPatientName = " select firstname+','+lastname from  pat_dat  Where Patid=?";
        #region Test Code

        public static string GetEasyDentalAppointment_Procedures_Data = " SELECT AppointmentId,ProcedureId,RangeStart,RangeEnd,Surface "
            + "FROM {pj Proc_log_dat plog LEFT OUTER JOIN Appt_dat apt ON "
            + "(plog.procedureid = apt.codeid1  AND  codetype1 = 1) OR (plog.procedureid = apt.codeid2  AND  codetype2 = 1) OR (plog.procedureid = apt.codeid3  AND codetype3 = 1) OR "
            + "(plog.procedureid = apt.codeid4  AND codetype4 = 1) OR (plog.procedureid = apt.codeid5  AND codetype5 = 1) OR (plog.procedureid = apt.codeid6  AND codetype6 = 1) OR "
            + "(plog.procedureid = apt.codeid7  AND  codetype7 = 1) OR (plog.procedureid = apt.codeid8  AND codetype8 = 1) OR (plog.procedureid = apt.codeid9  AND codetype9 = 1) OR "
            + "(plog.procedureid = apt.codeid10 AND  codetype10 = 1) OR (plog.procedureid = apt.codeid11 AND codetype11 = 1) OR (plog.procedureid = apt.codeid12 AND codetype12 = 1) OR "
            + "(plog.procedureid = apt.codeid13 AND  codetype13 = 1) OR (plog.procedureid = apt.codeid14 AND codetype14 = 1) OR  (plog.procedureid = apt.codeid15 AND codetype15 = 1) OR "
            + "(plog.procedureid = apt.codeid16 AND  codetype16 = 1) OR (plog.procedureid = apt.codeid17 AND codetype17 = 1) OR  (plog.procedureid = apt.codeid18 AND codetype18 = 1) OR "
            + "(plog.procedureid = apt.codeid19 AND  codetype19 = 1) OR (plog.procedureid = apt.codeid20 AND codetype20= 1) } "
            + "WHERE AppointmentId > 0 AND apt.Date > ?"; //ORDER BY AppointmentId ASC

        public static string GetEasyDentalAppointment_Procedures_Data_Second_Table = " SELECT AppointmentId,CodeId,AbbrevDesc,ADACode "
                                                            + " FROM {pj Proc_Co2_dat pcode LEFT OUTER JOIN Appt_dat apt ON pcode.codeid = apt.codeid1 OR pcode.codeid = apt.codeid2 "
                                                            + " OR pcode.codeid = apt.codeid3 OR pcode.codeid = apt.codeid4 OR pcode.codeid = apt.codeid5 OR pcode.codeid = apt.codeid6 "
                                                            + " OR pcode.codeid = apt.codeid7 OR pcode.codeid = apt.codeid8 OR pcode.codeid = apt.codeid9 OR pcode.codeid = apt.codeid10 "
                                                            + " OR pcode.codeid = apt.codeid11 OR pcode.codeid = apt.codeid12 OR pcode.codeid = apt.codeid13 OR pcode.codeid = apt.codeid14 "
                                                            + " OR pcode.codeid = apt.codeid15 OR pcode.codeid = apt.codeid16 OR pcode.codeid = apt.codeid17 OR pcode.codeid = apt.codeid18 "
                                                            + " OR pcode.codeid = apt.codeid19 OR pcode.codeid = apt.codeid20 } "
                                                            + " WHERE AppointmentId > 0 AND apt.Date > ?"; //apt.date > ? Order by apt.date desc // ORDER BY AppointmentId ASC

        public static string GetEasyDentalAppointment_Procedures_Data_FirstSub = "  SELECT ProcedureId, AbbrevDesc, ADACode "
                                                            + "  FROM {pj Proc_Co2_dat pcode LEFT OUTER JOIN Proc_log_dat plog ON plog.proccodeid = pcode.codeid } "
                                                            + " WHERE ProcedureId > 0 AND plog.Date > ?"; //plog.date > ?  Order by plog.date desc // ORDER BY ProcedureId ASC


        public static string GetEasyDentalAppointmentIDsForProcDesc = " SELECT appointmentid FROM Appt_dat WHERE AppointmentId > 0 AND date > ?";

        public static string GetEasyDentalProcsForCodeType0 = " SELECT AppointmentId, codeid_Number AS CodeId FROM Appt_dat "
                                                             +" WHERE AppointmentId > 0 AND codeid_Number > 0 AND codetype_Number = 0 AND date > ? ORDER BY AppointmentId";

        public static string GetEasyDentalProcsForCodeType1 = " SELECT AppointmentId, codeid_Number AS CodeId FROM Appt_dat "
                                                             + " WHERE AppointmentId > 0 AND codeid_Number > 0 AND codetype_Number = 1 AND date > ?";

        public static string GetEasyDentalProcDescForCodeType0 = " SELECT CodeId,ADACode,AbbrevDesc FROM Proc_Co2_Dat"; // WHERE CodeId = 397

        public static string GetEasyDentalProcDescForCodeType1 = " SELECT ProcedureId,ProcCodeId,RangeStart,RangeEnd,Surface FROM Proc_Log_Dat"; // WHERE ProcedureId = 448 // WHERE date > ?

        //public static string GetEasyDentalProcDescForCodeType1_Sub = " SELECT CodeId,ADACode,AbbrevDesc FROM Proc_Co2_Dat"; // WHERE CodeId = ( SELECT ProcCodeId FROM Proc_Log_Dat WHERE ProcedureId = 448 )

        #endregion

        public static string GetEasyDentalDeletedAppointmentData = "Select AppointmentId as appointment_id, ModifiedDate AS EHR_Entry_DateTime from Appt_dat Where operatoryId = '' and date > ? Order by date desc "; //RscType = 0

        public static string GetEasyDentalAppointmentNoteData = " Select * from Notes_dat where Type = 2";

        #endregion

        #region OperatoryEvent
        public static string GetEasyDentalOperatoryEventData = " select * from admin.v_appt_book_events WHERE modified_time_stamp > ? or event_date > ?";
        #endregion

        #region Holiday

        public static string GetEasyDentalHolidayData = "call admin.sp_getpracticeschedexceptions(?,?)";

        public static string GetEasyDentalOperatoryHolidaysData = "call admin.sp_getoperatoryschedexceptions(?,?,?)";

        #endregion

        #region Provider
        public static string GetEasyDentalProviderData = " SELECT RscId AS Provider_EHR_ID, Last AS Last_Name, First AS First_Name, Initial AS MI, u.Description AS provider_speciality ,InActive AS Is_Active FROM {oj RscProv_dat p LEFT OUTER JOIN DefProviderSpec_dat u ON p.Specialty = u.defid} where p.type = 1 ";

        public static string GetPatientPrimaryProvider = " SELECT provid1 From Pat_dat where patid=?";

        #endregion


        #region Speciality

        public static string GetEasyDentalSpecialityData = " SELECT 0 Clinic_Number,1 as Service_Install_Id,0 AS Speciality_LocalDB_ID,defid as Speciality_EHR_ID,'' AS Speciality_Web_ID,Description as Speciality_Name,0 AS Is_Adit_Updated FROM DefProviderSpec_dat";

        #endregion

        #region ProviderOfficeHours
        public static string GetEasyDentalPRoviderIds = " SELECT provider_id AS Provider_EHR_Id FROM admin.v_Provider ";

        public static string GetEasyDentalProviderOfficeHours = "CALL admin.sp_getproviderschedule('@provider_Id')";

        public static string GetEasyDentalProviderHoursData = "call admin.sp_getprovschedexceptions(?,?,?)";

        #endregion

        #region Operatory

        public static string GetEasyDentalOperatoryData = " SELECT RscId AS Operatory_EHR_ID , title AS Operatory_Name,0 as OperatoryOrder FROM RscOp_dat ";

        #endregion

        #region OperatoryOfficeHours
        public static string GetEasyDentalOperatoryIds = " SELECT op_id AS Operatory_EHR_Id FROM admin.v_Operatory ";

        public static string GetEasyDentalOperatoryOfficeHours = "CALL admin.sp_getOperatoryschedule('@op_id')";

        public static string GetEasyDentalOperatoryHoursData = "call admin.sp_getoperatoryschedexceptions(?,?,?)";
        #endregion

        #region ApptType
        public static string GetEasyDentalApptTypeData = " SELECT defid AS ApptType_EHR_ID,description AS Type_Name FROM DefApptCode_dat";
        #endregion

        #region Patient

        public static string GetEasyDentalPatientStatusAppointmentData = " Select distinct Patientid from Appt_dat where patientid <> 0 and ((date < ? and time < ?) or (date = ? and time < ?)) and status = 150";
        public static string GetEasyDentalPatientData = " Select 0 as Clinic_Number, 1 as Service_Install_ID, patid as Patient_EHR_ID,FirstName AS First_name,LastName as Last_Name,MI as Middle_Name, "
                                                        + " Salutation as Salutation,PrefName as preferred_name, \"status\" as Status ,BirthDate as Birth_Date,Email as Email, "
                                                        + " Pager as Mobile,ad.Phone as Home_Phone,WorkPhone as Work_Phone,ad.Street as Address1,ad.Street2 as Address2, "
                                                        + " ad.City as City,ad.State as State,ad.Zip as Zipcode,GuarId as Guar_ID,Gender as sex,famPos as MaritalStatus, "
                                                        + " Provid1 as Pri_Provider_ID,Provid2 as Sec_Provider_ID,FirstVisit as firstvisit_date,LastVisit as LastVisit_Date, "
                                                        + " PrInsuredid as Primary_Insurance,ScInsuredid as Secondary_Insurance ,'' as Primary_Insurance_CompanyName, "
                                                        + " '' as Secondary_Insurance_CompanyName,0 as CurrentBal,0 as ThirtyDay , 0 as SixtyDay,0 as NinetyDay, 0 as Over90, "
                                                        + " ''  AS nextvisit_date,'' AS due_date , 0 as remaining_benefit,prbenefits + scbenefits as used_benefit,'' AS collect_payment, "
                                                        + " privacyflags as privacy_flags ,1 AS InsUptDlt, 'Y' As ReceiveSMS, 'Y' As ReceiveEmail,'Y' as ReceiveVoiceCall,'' as EHR_Status,'' as Primary_Ins_Subscriber_ID,'' as Secondary_Ins_Subscriber_ID,'' as PreferredLanguage,'' as Patient_Note, "
                                                        + " ssn as ssn,driverslicense as driverlicense,'' as groupid ,'' as emergencycontactId,'' as emergencycontactnumber,'' as school ,'' as employer ,'' as spouseId ,GuarId as responsiblepartyId , "
                                                        + " '' as responsiblepartyssn ,'' as responsiblepartybirthdate ,'' as Spouse_Last_Name ,'' as ResponsibleParty_First_Name ,'' as ResponsibleParty_Last_Name ,'' as Prim_Ins_Company_Phonenumber,'' as Sec_Ins_Company_Phonenumber , "
                                                        + " '' as EmergencyContact_First_Name ,'' as EmergencyContact_Last_Name ,'' as Spouse_First_Name "
                                                        + " from {pj Pat_dat p LEFT OUTER JOIN Address_dat ad on p.Addressid = ad.Addressid}";


        public static string GetEasyDentalPatientNextApptDate = " select date AS nextvisit_date,patientid as Patient_EHR_ID From appt_dat Where date > ? Order by date desc ";

        public static string GetEasyDentalApptPatientNextApptDate = " select date AS nextvisit_date,patientid as Patient_EHR_ID From appt_dat Where date > ? and patientid in (@patientid) Order by date desc ";

        public static string GetEasyDentalPatientdue_date = " select prt.patid as patient_id,prt.recallid as recall_type_id,duedate as due_date,name as recall_type from {ij recpend_dat prt LEFT OUTER JOIN Rectype_dat rsc on prt.recallid = rsc.recallid} "; // where duedate > ?

        public static string GetEasyDentalApptPatientdue_date = " select prt.patid as patient_id,prt.recallid as recall_type_id,duedate as due_date,name as recall_type from {ij recpend_dat prt LEFT OUTER JOIN Rectype_dat rsc on prt.recallid = rsc.recallid} Where prt.duedate > ? AND prt.duedate < ? and prt.patid in (@patientid) "; // where duedate > ?

        public static string GetEasyDentalInsuranceData = " select insd.InsPartyId as patient_id,InsPlanId as InsId,InsCoName,IdNum,InsuredId,MaxCovPerson as PerCoverage,MaxCovFamily as FamCoverage,phone  from {pj Insured_dat insd LEFT  OUTER JOIN Ins_dat ins on Ins.InsId = insd.InsPlanId}";

        public static string GetEasyDentalPatientNoteData = " Select * from Notes_dat  where Type = 3";

        public static string GetEasyDentalEmployerData = " SELECT pat.patid as Patient_EHR_ID, name as employer FROM { pj Emp_Dat empl LEFT outer JOIN Pat_dat pat ON empl.empid = pat.empid } where patid <> 0";

        public static string GetEasyDentalResponsiblePartyData = "SELECT distinct Guar.Guarid as responsiblepartyId , pat.firstname as ResponsibleParty_First_Name, pat.lastname as ResponsibleParty_Last_Name, pat.ssn as responsiblepartyssn, pat.birthdate as responsiblepartybirthdate FROM { pj Pat_Dat Guar LEFT outer JOIN pat_Dat Pat ON pat.Patid = Guar.guarid } where guar.guarid <> 0";

        public static string GetEasyDentalAgingData = " Select * from Aging_dat";


        public static string GetEasyDentalPatientcollect_payment = " Select Sum(LastPayAmount) AS collect_payment from Aging_dat where Guaratorid = ?";

        public static string GetEasyDentalPatient_RecallType = "Select rt.recallid, (LTRIM(RTRIM(rt.name))) AS recall_type"
                                                + " from Admin.RecallType rt ";

        #endregion

        #region  Disease

        public static string GetEasyDentalDiseaseData = " select defid as Disease_EHR_ID,description as Disease_Name,'P' as Disease_Type from defMedAlert_dat ";

        #endregion

        #region RecallType
        public static string GetEasyDentalRecallTypeData = " SELECT distinct recallid as RecallType_EHR_ID,name as RecallType_Name,Description as RecallType_Descript from rectype_dat";
        #endregion
        #region User
        public static string GetEasyDentalUserData = " select RscId as User_EHR_ID,'' AS User_LocalDB_ID,'' as User_web_Id,First as First_Name,Last as Last_Name,'' as Password ,'' as EHR_Entry_DateTime,'' as Last_Updated_DateTime,''as LocalDb_EntryDatetime,inactive as Is_active,0 as is_deleted,0 as Is_Adit_Updated,1 as Clinic_Number,0 as Service_Install_Id from RscProv_dat where Type=2";

        public static string CheckAditUserExist = " Select RscId from RscProv_dat where  RscId='ADIT' ";

        public static string InsertAditUser = " insert into RscProv_dat(Type,RscId,FeeSchedule,Last,First) values(2,'ADIT',0,'Adit','Adit')";
        #endregion

        #region ApptStatus

        public static string GetEasyDentalApptStatusData = " SELECT defid AS ApptStatus_EHR_ID , description AS ApptStatus_Name, 'normal' AS ApptStatus_Type FROM DefRsStatus_dat";
        #endregion

        #region CreateAppointment
        public static string GetEasyDentalPatientID_NameData = " Select patid as Patient_EHR_ID,FirstName AS FirstName,LastName as LastName,FirstName+ ' ' +LastName AS Patient_Name,BirthDate as birth_date, Pager as Mobile, ad.Phone as Home_Phone, "
                                                 + " WorkPhone as Work_Phone,GuarId as Guar_ID,Email  from {pj Pat_dat p LEFT  OUTER JOIN Address_dat ad on p.Addressid = ad.Addressid}";

        public static string GetEasyDentalPatientStatusData = " Select patid as Patient_EHR_ID from Pat_dat";

        public static string GetEasyDentalIdelProvider = " SELECT RscId FROM RscProv_dat where InActive = 0 and type = 1";

        public static string GetEasyDentalIdelProviderG5 = "SELECT top 1 provider_id FROM admin.v_Provider";

        public static string UpdatePatientGuarantorID = " Update Pat_dat SET IsGuarantor=?,isheadofhouse = ? ,GuarId = ? ,familyid= ? WHERE patid = ? ";

        public static string InsertPatientAggingGuarantorID = " Insert into aging_dat (Guaratorid,agingdate,billingtype) values (?,?,1)";

        public static string InsertPatientDetails = " Insert into pat_dat (LastName,FirstName,MI,PrefName,ChartNum,ProvId1,IsGuarantor,Gender,Status,FamPos,AddressId,Email,Pager,GuidId,FirstVisit)values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

        public static string InsertPatientDetails_With_Birthdate = " Insert into pat_dat (LastName,FirstName,MI,PrefName,ChartNum,ProvId1,IsGuarantor,Gender,Status,FamPos,AddressId,Email,Pager,GuidId,FirstVisit,BirthDate)values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";// Insert into pat_dat (LastName,FirstName,MI,PrefName,ChartNum,ProvId1,IsGuarantor,firstvisit,Gender,Status,FamPos,AddressId,Email,Pager,GuidId,BirthDate)values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

      //  public static string GetBookOperatoryAppointmenetWiseDateTime = " Select AppointmentId as appointment_id,OperatoryId as operatory_id,Time as Start_hour,Date as appointment_date,AppointLen as ApptMin from Appt_dat WHERE PatientName <> '' and RscType = 3 and date >= ? Order by date desc ";
        public static string GetBookOperatoryAppointmenetWiseDateTime = "Select AppointmentId as appointment_id,OperatoryId as operatory_id,Time as Start_hour,Date as appointment_date,AppointLen as ApptMin,ProviderId as provider_id,PatientId as patId, PatientName as patient_name,'' AS FirstName,'' AS LastName,ap.phone AS Mobile,'' AS Email,'' AS ProviderFirstName,'' AS ProviderLastName from {pj Appt_dat ap LEFT OUTER JOIN Address_dat ad on ap.NewPatAddressId = ad.Addressid} WHERE PatientName <> '' and RscType = 3 and date >= ? Order by date desc ";

        public static string InsertAppointmentDetails = " INSERT INTO Appt_dat(patientid, status, appointlen,time, operatoryid, providerid, date, ModifiedDate, appttype,timeblock,rsctype2,rsctype,patientname,rsctype3,staffid) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

        public static string InsertAppointmentComment = "INSERT INTO Notes_dat(type,noteid,text)VALUES(?,?,?)";

        public static string Is_Update_Status_EHR_Appointment_Live_To_EHR = "SELECT status FROM admin.Appt Where apptid  = ? ";

        public static string Update_Status_EHR_Appointment_Live_To_Local = " UPDATE appt_dat SET status = ? WHERE appointmentid = ? ";

        public static string Update_Receive_SMS_Patient_EHR_Live_To_EasyDental = " UPDATE pat_dat SET privacyflags = ? WHERE patid = ? ";

        public static string InsertPatientAddress = "insert into Address_dat (ptrcount) values (1)";

        public static string Update_Patinet_Record_By_Patient_Form = " UPDATE pat_dat SET ColumnName = @ehrfield_value WHERE patid = @Patient_EHR_ID ";

        public static string Update_Patinet_Address_Record_By_Patient_Form = " UPDATE address_dat SET ColumnName = @ehrfield_value WHERE addressid = @Patient_EHR_ID ";

        public static string Insert_paitent_insurance = " insert into insured_dat (insplanid,inspartyid,idnum,ptrcount) values (?,?,?,1)";

        public static string Insert_paitent_primaryinsurance_patplan = " update pat_dat set prinsrel = 1 , prinsuredid = ? where patid = ?";

        public static string Insert_paitent_secondaryinsurance_patplan = " update pat_dat set scinsrel = 1 , scinsuredid = ? where patid = ?";

        public static string GetpatientGurId = " Select GuarId from pat_dat where patid = ?";

        public static string Insert_paitent_PaymentLog = " insert into Contact_dat(date,time,patid,Providerid,Data,Contacttype) values (?,?,?,?,?,?)";

        public static string Insert_paitent_PaymentLogNote = " insert into Notes_dat(type,noteid,text) values (?,?,?)";

        public static string InsertPaymentAmount = "INSERT INTO payment_dat(paymentid,patid,guarid,date,familylink,defpayid,Providerid,Amount,createdate,checknum,banknumber)"
                                                   + "VALUES(?,?,?,?,?,?,?,?,?,?,?)";
        public static string InsertProcLogAmount = "INSERT INTO Proc_Log_dat(patid,guarid,chartstatus,date,proccodeid,class,Providerid,Amount,createdate)"
                                                  + "VALUES(?,?,90,?,?,?,?,?,?)";
        #endregion

        public static string GetEasyDentalApplicationVersion = "select optionvalue AS EHR_Sub_Version  from admin.options where optionkey = 'Data_Version'";




        //public static string GetEasyDentalProviderHoursData = " SELECT sd.scheduleNum,IFNULL(sdo.ScheduleOpNum,0), "
        //                                      + " (CASE WHEN IFNULL(sdo.ScheduleOpNum,0) = 0 THEN sd.scheduleNum ELSE CONCAT(sd.scheduleNum,'_',sdo.ScheduleOpNum)END) AS PH_EHR_ID, "
        //                                      + " sd.provNum AS Provider_EHR_ID,sd.scheddate,CONCAT(sd.scheddate,' ',sd.StartTime) AS StartTime ,CONCAT(sd.scheddate,' ',sd.StopTime)AS EndTime, "
        //                                      + " sdo.OperatoryNum AS Operatory_EHR_ID,sd.note AS comment,sd.status, sd.DateTStamp AS Entry_DateTime "
        //                                      + " FROM schedule sd left JOIN scheduleop sdo ON sdo.ScheduleNum = sd.ScheduleNum "
        //                                      + " WHERE sd.SchedType = 1 AND sd.scheddate > @ToDate ; ";

        //public static string GetEasyDentalProviderHoursData = " SELECT sd.scheduleNum,IFNULL(sdo.ScheduleOpNum,0), "
        //                                    + " (CASE WHEN IFNULL(sdo.ScheduleOpNum,0) = 0 THEN sd.scheduleNum ELSE CONCAT(sd.scheduleNum,'_',sdo.ScheduleOpNum)END) AS PH_EHR_ID, "
        //                                    + " sd.provNum AS Provider_EHR_ID,sd.scheddate,CONCAT(sd.scheddate,' ',sd.StartTime) AS StartTime ,CONCAT(sd.scheddate,' ',sd.StopTime)AS EndTime, "
        //                                    + " sdo.OperatoryNum AS Operatory_EHR_ID,sd.note AS comment,sd.status, sd.DateTStamp AS Entry_DateTime "
        //                                    + " FROM schedule sd left JOIN scheduleop sdo ON sdo.ScheduleNum = sd.ScheduleNum "
        //                                    + " WHERE sd.SchedType = 1 AND sd.scheddate > @ToDate ; ";


        #region MedicleForm

        public static string GetEasyDentalMedicleQuestionData = " SELECT 0 as Clinic_Number,1 as Service_Install_ID,0 as EasyDental_Question_LocalDB_ID,'' as EasyDental_Question_Web_ID,QuestionId as EasyDental_QuestionId,Row as EasyDental_Row, "
                                            + " '' As EasyDental_FormMaster_Web_ID, "//####### it is new line
                                            + " Question as EasyDental_Question,QuestionType as EasyDental_QuestionType , DefaultAnsText ,"
                                            + " DefaultAnsNumber ,DefaultAnsBool , DefaultAnsDate, 0 as Is_Adit_Updated,0 as is_deleted,0 as EasyDental_FormId from Medques_dat";




        public static string GetEasyDentalMedicleResponseData = " SELECT patid as Patient_EHR_id,QuestionId as EasyDental_QuestionId from QuesResp_dat where patid = ? and QuestionId = ?";

        public static string InsertEasyDentalMedicleResponseSetData = " insert into QuesResp_dat(patid,QuestionId,QuestionType,AnsText,AnsNum,AnsBool,AnsDate) "
                                                                      + " values (?,?,?,?,?,?,?)";

        public static string InsertEasyDentalDiseaseData = "insert into admin.hhlinkpat_item(patid,hhitemid,status,reporteddate,startdate,enddate,note) "
                                                      + " values (?,?,1,getdate(),getdate(),'9999/12/31',?)";

        public static string GetDuplicateRecords = " select ct.date,ct.patid,Providerid,ct.title,Contacttype,fn.text,max(ContactId) as LogId,count(*) as cnt from {pj Contact_dat ct LEFT OUTER JOIN Notes_dat fn on ct.Contactid = fn.NoteID and type = 49} where fn.type = 49 and contacttype in (3,5) and (text like  'SMS sent -%' OR text like 'SMS received -%' OR text like 'SMS received -%' OR text like 'Inbound call answered - Duration:%' OR text like 'Outbound call answered - Duration:%') group by ct.date,ct.patid,Providerid,ct.title,Contacttype,fn.text having count(*) > 1";

        public static string DeleteDuplicateContactLogs = " Delete from Contact_dat where Contactid = ?";

        public static string DeleteDuplicateFullNoteLogs = " Delete from Notes_dat where noteid = ? and type = 49";

        public static string CheckSMSCallRecordsBlankMobile = " select MAX(Contactid) AS NoteId,COUNT(*) AS TotalRecords from {pj Contact_dat ct LEFT OUTER JOIN Notes_dat fn on ct.Contactid = fn.NoteID and type = 49} where ct.date = ? and ct.time = ? and patid = ? and Providerid = ? and Contacttype = ? and text= ? AND Contacttype IN (3,5) Group by Patid,providerid,ct.date,ct.time,Contacttype,text having COUNT(*) > 1"; //ct.date > ? AND  

        #endregion


        #region "Medication"
        public static string GetEasyDentalMedicationData = " Select RxDefId as Medication_EHR_ID, Name as Medication_Name, Description as Medication_Description, RxNoteId as Medication_Notes, SigNoteId as Medication_Sig, '' as Medication_Parent_EHR_ID, 'Drug' as Medication_Type , AsWritten as Allow_Generic_Sub, Dispense as Drug_Quantity, Refills as Refills, Active as Is_Active, CreateDate as EHR_Entry_DateTime, '' as Medication_Provider_ID, 0 as is_deleted, 0 as Is_Adit_Updated , '0' as Clinic_Number From RxDef_dat";
        public static string GetEasyDentalMedicationNotesData = " Select * from Notes_dat where Type in (34,35)";

        public static string GetMedication = " Select RxDefId from RxDef_dat where Name = @Medication_Name";
        public static string GetPatientMedication = " Select RxId  from RxPat_dat where RxDefId = @Medicatoin_EHR_ID and PatID = @Patient_EHR_ID";

        public static string InsertMedication = " Insert into RxDef_dat(Active, Standard, Name, Description, Dispense, REfills, AsWritten, CreateDate) Values(1,1,@Medication_Name,@Medication_Name,'0',0, 1,?)";
        
        public static string InsertMedicationInactive = " Insert into RxDef_dat(Active, Standard, Name, Description, Dispense, REfills, AsWritten, CreateDate) Values(0,0,@Medication_Name,@Medication_Description,'0',0, 1,?)";

        public static string InsertPatientMedication = " Insert into RxPat_dat(RxDefId,PatId,ProviderId,Date) Values(@Medication_EHR_ID,@Patient_EHR_ID,@Provider_EHR_ID,?)";

        //public static string GetEasyDentalPatientMedicationData = "  select Rxid as PatientMedication_EHR_ID,RxDefid as Medication_EHR_ID,Patid as Patient_EHR_ID,Date as Date_Entered,ProviderId as Provider_EHR_ID,'' AS Start_Date,'' AS End_Date,'' AS EHR_Entry_DateTime,'' AS Last_Sync_Date,'' AS Medication_Note,'' AS Patient_Notes,'' AS Expiry_Date,'M' AS Medication_Type,0 AS is_deleted,0 Clinic_Number,'True' is_active from RxPat_dat";

        public static string GetEasyDentalPatientMedicationData = " select PatID as Patient_EHR_ID, " +
                                                                  "RxDefID Medication_EHR_ID, " +
                                                                  "'' as Medication_Note, " +
                                                                  "ProviderId as Provider_EHR_ID, " +
                                                                  "'' as Medication_Name, " +
                                                                  "'M' as Medication_Type, " +
                                                                  "RxID PatientMedication_EHR_ID, " +
                                                                  "'' Patient_Notes, " +
                                                                  "'' Refills, " +
                                                                  "'true' as is_active, " +
                                                                  "'' as Drug_Quantity, " +
                                                                  "'' as Start_Date, " +
                                                                  "'' as End_Date, " +
                                                                  "Date as EHR_Entry_DateTime, " +
                                                                  "'' as Last_Sync_Date, " +
                                                                  "0 is_deleted, " +
                                                                  "'' Is_Adit_Updated, " +
                                                                  "0 as Clinic_Number, " +
                                                                  "1 as Service_Install_Id from RxPat_dat";

        public static string GetEasydentalmasterdataForPatMed = " select RxDefId as Medication_EHR_ID, Name as Medication_Name,Description as MedicalDescription,'' AS Medication_Type,'' AS Start_Date,'' AS End_Date,Dispense as Drug_Quantity,Refills as Refills,RxNoteId as Medication_Notes from RxDef_dat";
        public static string GetPatientMedicationNotesData = " Select * from Notes_dat where Type in (34,35)";

        public static string DeletePatientMedication = " Delete From RxPat_Dat where RxID = @PatientMedication_EHR_ID";
        #endregion


    }
}
