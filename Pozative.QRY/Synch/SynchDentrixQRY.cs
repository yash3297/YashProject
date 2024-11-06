using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pozative.QRY
{
    public class SynchDentrixQRY
    {

        #region Appointment
        //public static string GetDentrixAppointmentData = " SELECT convert(varchar(20),V_appt.appointment_id) as appointment_id,V_appt.patient_name,V_appt.appointment_date,V_appt.start_hour, V_appt.start_minute , V_appt.length, "
        //                                                + " V_appt.patient_phone,V_appt.provider_last_name,V_appt.provider_id, V_appt.provider_first_name, "
        //                                                + " V_appt.note AS comment,pat.birth_date , "
        //                                                + " v_opt.op_id , v_opt.op_title,adr.street1,adr.city,adr.country,adr.state,adr.zipcode,v_not.notetext, "
        //                                                + " a_appt.appttype AS ApptType_EHR_ID,case when  a_appt.appttype = 0 then '<none>' else  AType.descript end AS ApptType_Name, "
        //                                                + " pat.patient_id AS patId, (LTRIM(RTRIM(isnull(pat.address_line1,'')))) AS patAddress, (LTRIM(RTRIM(isnull(pat.city,'')))) AS patCity, "
        //                                                + " (LTRIM(RTRIM(isnull(pat.state,'')))) AS patState, V_appt.status_id AS appointment_status_ehr_key ,  (LTRIM(RTRIM(vasc.descript))) AS Appointment_Status, "
        //                                                + " (LTRIM(RTRIM(isnull(pat.zipcode,'')))) AS patZipcode, (LTRIM(RTRIM(isnull(pat.home_phone,'')))) AS patHomephone, "
        //                                                + " (LTRIM(RTRIM(isnull(pat.mobile_phone,'')))) AS patMobile, (LTRIM(RTRIM(isnull(pat.email,'')))) AS patEmail, "
        //                                                + " a_appt.automodifiedtimestamp AS EHR_Entry_DateTime,isnull(V_appt.appt_flag,0) as appt_flag   "
        //                                                + " FROM admin.v_appointment V_appt "
        //                                                + " JOIN admin.v_operatory v_opt ON v_opt.op_id = V_appt.Operatory_id "
        //                                                + " JOIN admin.appt a_appt ON a_appt.apptid = V_appt.appointment_id "
        //                                                + " Left JOIN admin.address adr ON adr.addrid = a_appt.newpataddrid "
        //                                                + " Left JOIN admin.v_Appointment_Types AType ON AType.def_id = a_appt.appttype "
        //                                                + " Left JOIN admin.v_notes v_not ON v_not.noteid = a_appt.newpataddrid  AND v_not.notetype = 116"
        //                                                + " Left JOIN admin.v_patient pat ON pat.patient_id = V_appt.patient_id "
        //                                                + " Left JOIN admin.V_appt_Status_Codes vasc ON vasc.defid = V_appt.status_id "
        //                                                + " WHERE V_appt.patient_name <> '' AND appointment_date >= ?";
        public static string GetDentrixAppointmentData = "SELECT convert(varchar(20),V_appt.appointment_id) as appointment_id,V_appt.patient_name,V_appt.appointment_date,V_appt.start_hour, V_appt.start_minute , V_appt.length, "
                                                       + " V_appt.patient_phone,V_appt.provider_last_name,V_appt.provider_id, V_appt.provider_first_name, "
                                                       + " V_appt.note AS comment,pat.birthdate as birth_date ,  "
                                                       + " v_opt.op_id , v_opt.op_title,adr.street1,adr.city,adr.country,adr.state,adr.zipcode,v_not.notetext, "
                                                       + " a_appt.appttype AS ApptType_EHR_ID,case when a_appt.appttype = 0 then '<none>' else  AType.descript end AS ApptType_Name, "
                                                       + " pat.patId AS patId, (LTRIM(RTRIM(isnull(adr.street1,'')))) AS patAddress, (LTRIM(RTRIM(isnull(adr.city, '')))) AS patCity, "
                                                       + " (LTRIM(RTRIM(isnull(adr.state, '')))) AS patState, V_appt.status_id AS appointment_status_ehr_key ,  (LTRIM(RTRIM(vasc.descript))) AS Appointment_Status, "
                                                       + " (LTRIM(RTRIM(isnull(adr.zipcode, '')))) AS patZipcode, (LTRIM(RTRIM(isnull(adr.phone, '')))) AS patHomephone, "
                                                       + " (LTRIM(RTRIM(isnull(pat.pager, '')))) AS patMobile, (LTRIM(RTRIM(isnull(pat.emailaddr, '')))) AS patEmail, "
                                                       + " a_appt.automodifiedtimestamp AS EHR_Entry_DateTime,isnull(V_appt.appt_flag,0) as appt_flag "
                                                       + " FROM admin.v_appointment V_appt "
                                                       + " JOIN admin.v_operatory v_opt ON v_opt.op_id = V_appt.Operatory_id "
                                                       + " JOIN admin.appt a_appt ON a_appt.apptid = V_appt.appointment_id "
                                                       + " Left JOIN admin.address adr ON adr.addrid = a_appt.newpataddrid "
                                                       + " Left JOIN admin.v_Appointment_Types AType ON AType.def_id = a_appt.appttype "
                                                       + " Left JOIN admin.v_notes v_not ON v_not.noteid = a_appt.newpataddrid  AND v_not.notetype = 116 "
                                                       + " Left JOIN admin.patient pat ON pat.patid = V_appt.patient_id "
                                                       + " Left JOIN admin.V_appt_Status_Codes vasc ON vasc.defid = V_appt.status_id "
                                                       + " WHERE V_appt.patient_name<> '' AND appointment_date >= ? ";

        //public static string GetDentrixAppointmentEhrIds = " SELECT convert(varchar(20),V_appt.appointment_id) As Appt_EHR_ID"
        //                                                + " FROM admin.v_appointment V_appt "
        //                                                + " JOIN admin.v_operatory v_opt ON v_opt.op_id = V_appt.Operatory_id "
        //                                                + " JOIN admin.appt a_appt ON a_appt.apptid = V_appt.appointment_id "
        //                                                + " Left JOIN admin.address adr ON adr.addrid = a_appt.newpataddrid "
        //                                                + " Left JOIN admin.v_Appointment_Types AType ON AType.def_id = a_appt.appttype "
        //                                                + " Left JOIN admin.v_notes v_not ON v_not.noteid = a_appt.newpataddrid  AND v_not.notetype = 116"
        //                                                + " Left JOIN admin.v_patient pat ON pat.patient_id = V_appt.patient_id "
        //                                                + " Left JOIN admin.V_appt_Status_Codes vasc ON vasc.defid = V_appt.status_id "
        //                                                + " WHERE V_appt.patient_name <> '' AND appointment_date >= ?";
        public static string GetDentrixAppointmentEhrIds = "SELECT convert(varchar(20),V_appt.appointment_id) As Appt_EHR_ID "
                                                         + " FROM admin.v_appointment V_appt "
                                                         + " JOIN admin.v_operatory v_opt ON v_opt.op_id = V_appt.Operatory_id "
                                                         + " JOIN admin.appt a_appt ON a_appt.apptid = V_appt.appointment_id "
                                                         + " Left JOIN admin.address adr ON adr.addrid = a_appt.newpataddrid "
                                                         + " Left JOIN admin.v_Appointment_Types AType ON AType.def_id = a_appt.appttype "
                                                         + " Left JOIN admin.v_notes v_not ON v_not.noteid = a_appt.newpataddrid  AND v_not.notetype = 116 "
                                                         + " Left JOIN admin.patient pat ON pat.patid = V_appt.patient_id "
                                                         + " Left JOIN admin.V_appt_Status_Codes vasc ON vasc.defid = V_appt.status_id "
                                                         + " WHERE V_appt.patient_name<> '' AND appointment_date >= ?  ";

        public static string DentrixAppointment_Procedures_Data = " SELECT apptid,codeid1 AS CodeId,codetype1 AS CodeType, (CASE WHEN codetype1 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype1 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype1 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype1 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype1 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype1 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype1 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype1 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype1 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid1 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid1 = proc1.proccodeid WHERE apt.apptdate >= ? and codeid1 > 0 Order by Apt.Apptid "
                                                        + " UNION ALL SELECT apptid,codeid2 AS CodeId,codetype2 AS CodeType, (CASE WHEN codetype2 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype2 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype2 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype2 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype2 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype2 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype2 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype2 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype2 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid2 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid2 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid2 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid3 AS CodeId,codetype3 AS CodeType, (CASE WHEN codetype3 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype3 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype3 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype3 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype3 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype3 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype3 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype3 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype3 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid3 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid3 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid3 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid4 AS CodeId,codetype4 AS CodeType, (CASE WHEN codetype4 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype4 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype4 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype4 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype4 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype4 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype4 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype4 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype4 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid4 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid4 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid4 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid5 AS CodeId,codetype5 AS CodeType, (CASE WHEN codetype5 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype5 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype5 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype5 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype5 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype5 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype5 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype5 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype5 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid5 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid5 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid5 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid6 AS CodeId,codetype6 AS CodeType, (CASE WHEN codetype6 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype6 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype6 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype6 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype6 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype6 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype6 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype6 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype6 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid6 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid6 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid6 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid7 AS CodeId,codetype7 AS CodeType, (CASE WHEN codetype7 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype7 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype7 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype7 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype7 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype7 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype7 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype7 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype7 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid7 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid7 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid7 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid8 AS CodeId,codetype8 AS CodeType, (CASE WHEN codetype1 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype8 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype8 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype8 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype8 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype8 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype8 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype8 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype8 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid8 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid8 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid8 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid9 AS CodeId,codetype9 AS CodeType, (CASE WHEN codetype9 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype9 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype9 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype9 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype9 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype9 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype9 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype9 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype9 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid9 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid9 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid9 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid10 AS CodeId,codetype10 AS CodeType, (CASE WHEN codetype10 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype10 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype10 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype10 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype10 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype10 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype10 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype10 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype10 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid10 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid10 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid10 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid11 AS CodeId,codetype11 AS CodeType, (CASE WHEN codetype11 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype11 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype11 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype11 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype11 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype11 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype11 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype11 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype11 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid11 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid11 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid11 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid12 AS CodeId,codetype12 AS CodeType, (CASE WHEN codetype12 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype12 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype12 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype12 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype12 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype12 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype12 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype12 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype12 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid12 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid12 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid12 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid13 AS CodeId,codetype13 AS CodeType, (CASE WHEN codetype13 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype13 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype13 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype13 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype13 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype13 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype13 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype13 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype13 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid13 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid13 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid13 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid14 AS CodeId,codetype14 AS CodeType, (CASE WHEN codetype14 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype14 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype14 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype14 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype14 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype14 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype14 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype14 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype14 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid14 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid14 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid14 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid15 AS CodeId,codetype15 AS CodeType, (CASE WHEN codetype15 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype15 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype15 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype15 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype15 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype15 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype15 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype15 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype15 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid15 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid15 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid15 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid16 AS CodeId,codetype16 AS CodeType, (CASE WHEN codetype16 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype16 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype16 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype16 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype16 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype16 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype16 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype16 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype16 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid16 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid16 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid16 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid17 AS CodeId,codetype17 AS CodeType, (CASE WHEN codetype17 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype17 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype17 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype17 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype17 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype17 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype17 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype17 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype17 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid17 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid17 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid17 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid18 AS CodeId,codetype18 AS CodeType, (CASE WHEN codetype18 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype18 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype18 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype18 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype18 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype18 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype18 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype18 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype18 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid18 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid18 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid18 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid19 AS CodeId,codetype19 AS CodeType, (CASE WHEN codetype19 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype19 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype19 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype19 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype19 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype19 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype19 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype19 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype19 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid19 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid19 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid19 > 0 "
                                                        + " UNION ALL SELECT apptid,codeid20 AS CodeId,codetype20 AS CodeType, (CASE WHEN codetype20 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype20 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype20 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype20 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype20 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype20 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype20 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype20 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype20 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid20 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid20 = proc1.proccodeid WHERE apt.apptdate >= ? and Codeid20 > 0 ";

        public static string DentrixAppointment_Procedures_DataByApptID = " SELECT apptid,codeid1 AS CodeId,codetype1 AS CodeType, (CASE WHEN codetype1 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype1 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype1 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype1 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype1 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype1 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype1 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype1 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype1 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid1 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid1 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and codeid1 > 0 Order by Apt.Apptid "
                                                       + " UNION ALL SELECT apptid,codeid2 AS CodeId,codetype2 AS CodeType, (CASE WHEN codetype2 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype2 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype2 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype2 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype2 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype2 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype2 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype2 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype2 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid2 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid2 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid2 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid3 AS CodeId,codetype3 AS CodeType, (CASE WHEN codetype3 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype3 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype3 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype3 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype3 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype3 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype3 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype3 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype3 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid3 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid3 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid3 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid4 AS CodeId,codetype4 AS CodeType, (CASE WHEN codetype4 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype4 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype4 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype4 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype4 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype4 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype4 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype4 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype4 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid4 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid4 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid4 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid5 AS CodeId,codetype5 AS CodeType, (CASE WHEN codetype5 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype5 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype5 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype5 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype5 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype5 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype5 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype5 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype5 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid5 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid5 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid5 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid6 AS CodeId,codetype6 AS CodeType, (CASE WHEN codetype6 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype6 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype6 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype6 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype6 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype6 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype6 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype6 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype6 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid6 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid6 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid6 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid7 AS CodeId,codetype7 AS CodeType, (CASE WHEN codetype7 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype7 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype7 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype7 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype7 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype7 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype7 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype7 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype7 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid7 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid7 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid7 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid8 AS CodeId,codetype8 AS CodeType, (CASE WHEN codetype1 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype8 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype8 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype8 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype8 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype8 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype8 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype8 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype8 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid8 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid8 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid8 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid9 AS CodeId,codetype9 AS CodeType, (CASE WHEN codetype9 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype9 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype9 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype9 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype9 = 1  THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype9 = 1  THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype9 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype9 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype9 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid9 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid9 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid9 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid10 AS CodeId,codetype10 AS CodeType, (CASE WHEN codetype10 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype10 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype10 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype10 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype10 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype10 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype10 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype10 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype10 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid10 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid10 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid10 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid11 AS CodeId,codetype11 AS CodeType, (CASE WHEN codetype11 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype11 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype11 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype11 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype11 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype11 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype11 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype11 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype11 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid11 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid11 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid11 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid12 AS CodeId,codetype12 AS CodeType, (CASE WHEN codetype12 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype12 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype12 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype12 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype12 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype12 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype12 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype12 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype12 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid12 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid12 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid12 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid13 AS CodeId,codetype13 AS CodeType, (CASE WHEN codetype13 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype13 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype13 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype13 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype13 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype13 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype13 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype13 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype13 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid13 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid13 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid13 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid14 AS CodeId,codetype14 AS CodeType, (CASE WHEN codetype14 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype14 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype14 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype14 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype14 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype14 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype14 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype14 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype14 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid14 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid14 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid14 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid15 AS CodeId,codetype15 AS CodeType, (CASE WHEN codetype15 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype15 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype15 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype15 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype15 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype15 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype15 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype15 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype15 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid15 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid15 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid15 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid16 AS CodeId,codetype16 AS CodeType, (CASE WHEN codetype16 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype16 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype16 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype16 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype16 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype16 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype16 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype16 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype16 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid16 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid16 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid16 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid17 AS CodeId,codetype17 AS CodeType, (CASE WHEN codetype17 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype17 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype17 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype17 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype17 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype17 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype17 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype17 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype17 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid17 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid17 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid17 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid18 AS CodeId,codetype18 AS CodeType, (CASE WHEN codetype18 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype18 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype18 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype18 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype18 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype18 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype18 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype18 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype18 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid18 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid18 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid18 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid19 AS CodeId,codetype19 AS CodeType, (CASE WHEN codetype19 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype19 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype19 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype19 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype19 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype19 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype19 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype19 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype19 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid19 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid19 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid19 > 0 "
                                                       + " UNION ALL SELECT apptid,codeid20 AS CodeId,codetype20 AS CodeType, (CASE WHEN codetype20 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+ CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END))ELSE '' END) AS Tooth,TRIM((TRIM(CASE WHEN surfm > 0 AND  codetype20 = 1 THEN 'M' ELSE '' END)+TRIM(CASE WHEN surfo > 0 AND codetype20 = 1  THEN 'O' ELSE '' END)+TRIM(CASE WHEN surfd > 0 AND codetype20 = 1  THEN 'D' ELSE '' END)+TRIM(CASE WHEN surfl > 0 AND codetype20 = 1 THEN 'L' ELSE '' END)+TRIM(CASE WHEN surff > 0 AND codetype20 = 1 THEN 'F' ELSE '' END)+TRIM(CASE WHEN surf5 > 0 AND codetype20 = 1  THEN '5' ELSE '' END))) AS Surface,(CASE WHEN codetype20 = 1 THEN proc.abbrevdescript ELSE  proc1.abbrevdescript END) AS abbrevdescript,(CASE WHEN codetype20 = 1 THEN proc.adacode ELSE proc1.adacode END)  AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid20 LEFT JOIN admin.proccode proc ON flog.proccodeid = proc.proccodeid LEFT JOIN admin.proccode proc1 ON Apt.Codeid20 = proc1.proccodeid WHERE apt.apptid = '@Appt_EHR_ID' and Codeid20 > 0 ";

        //public static string DentrixAppointment_Procedures_Data = " SELECT apptid,codeid1 AS CodeId,codetype1 AS CodeType, (CASE WHEN codetype1 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-' +TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+ (CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc, proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid1 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid1 WHERE apt.apptdate > ? and codeid1 > 0 ORDER BY apptid "
        //                                                + " UNION SELECT apptid,codeid2,codetype2, (CASE WHEN codetype2 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid2 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid2 WHERE apt.apptdate > ? and codeid2 > 0 "
        //                                                + " UNION SELECT apptid,codeid3,codetype3, (CASE WHEN codetype3 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid3 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid3 WHERE apt.apptdate > ? and codeid3 > 0  "
        //                                                + " UNION SELECT apptid,codeid4,codetype4, (CASE WHEN codetype4 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid4 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid4 WHERE apt.apptdate > ? and codeid4 > 0  "
        //                                                + " UNION SELECT apptid,codeid5,codetype5, (CASE WHEN codetype5 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid5 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid5 WHERE apt.apptdate > ? and codeid5 > 0  "
        //                                                + " UNION SELECT apptid,codeid6,codetype6, (CASE WHEN codetype6 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid6 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid6 WHERE apt.apptdate > ? and codeid6 > 0  "
        //                                                + " UNION SELECT apptid,codeid7,codetype7, (CASE WHEN codetype7 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid7 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid7 WHERE apt.apptdate > ? and codeid7 > 0  "
        //                                                + " UNION SELECT apptid,codeid8,codetype8, (CASE WHEN codetype8 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid8 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid8 WHERE apt.apptdate > ? and codeid8 > 0  "
        //                                                + " UNION SELECT apptid,codeid9,codetype9, (CASE WHEN codetype9 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid9 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid9 WHERE apt.apptdate > ? and codeid9 > 0  "
        //                                                + " UNION SELECT apptid,codeid10,codetype10, (CASE WHEN codetype10 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid10 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid10 WHERE apt.apptdate> ? and codeid10 > 0  "
        //                                                + " UNION SELECT apptid,codeid11,codetype11, (CASE WHEN codetype11 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid11 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid11 WHERE apt.apptdate> ? and codeid11 > 0  "
        //                                                + " UNION SELECT apptid,codeid12,codetype12, (CASE WHEN codetype12 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid12 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid12 WHERE apt.apptdate> ? and codeid12 > 0  "
        //                                                + " UNION SELECT apptid,codeid13,codetype13, (CASE WHEN codetype13 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid13 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid13 WHERE apt.apptdate> ? and codeid13 > 0  "
        //                                                + " UNION SELECT apptid,codeid14,codetype14, (CASE WHEN codetype14 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid14 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid14 WHERE apt.apptdate> ? and codeid14 > 0  "
        //                                                + " UNION SELECT apptid,codeid15,codetype15, (CASE WHEN codetype15 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid15 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid15 WHERE apt.apptdate> ? and codeid15 > 0  "
        //                                                + " UNION SELECT apptid,codeid16,codetype16, (CASE WHEN codetype16 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid16 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid16 WHERE apt.apptdate> ? and codeid16 > 0  "
        //                                                + " UNION SELECT apptid,codeid17,codetype17, (CASE WHEN codetype17 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid17 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid17 WHERE apt.apptdate> ? and codeid17 > 0  "
        //                                                + " UNION SELECT apptid,codeid18,codetype18, (CASE WHEN codetype18 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid18 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid18 WHERE apt.apptdate> ? and codeid18 > 0  "
        //                                                + " UNION SELECT apptid,codeid19,codetype19, (CASE WHEN codetype19 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid19 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid19 WHERE apt.apptdate> ? and codeid19 > 0  "
        //                                                + " UNION SELECT apptid,codeid20,codetype20, (CASE WHEN codetype20 = 1 THEN ((CASE WHEN toothrangestart > 0 AND toothrangestart < toothrangeend THEN CONVERT(VARCHAR(2),toothrangestart)+'-'+CONVERT(VARCHAR(2),toothrangeend) ELSE toothrangeend END)+'-'+TRIM(((CASE WHEN surfm > 0 THEN 'M' ELSE '' END)+(CASE WHEN surfo > 0 THEN 'O' ELSE '' END)+(CASE WHEN surfd > 0 THEN 'D' ELSE '' END)+(CASE WHEN surfl > 0 THEN 'L' ELSE '' END)+(CASE WHEN surff > 0 THEN 'F' ELSE '' END)+(CASE WHEN surf5 > 0 THEN '5' ELSE '' END)))+'-'+proc.abbrevdescript) ELSE proc.abbrevdescript END) AS ProcedureDesc,proc.adacode AS ProcedureCode FROM admin.appt apt LEFT JOIN admin.fullproclog flog ON flog.procid = apt.codeid20 LEFT JOIN admin.proccode proc ON proc.proccodeid = apt.codeid20 WHERE apt.apptdate> ? and codeid20 > 0  ";

        //public static string GetDentrixAppointmentDataDTXG5 = " SELECT V_appt.appointment_id,V_appt.patient_name,V_appt.appointment_date,V_appt.start_hour, V_appt.start_minute , V_appt.length, "
        //                                                + " V_appt.patient_phone,V_appt.provider_last_name,V_appt.provider_id, V_appt.provider_first_name, "
        //                                                + " V_appt.note AS comment,pat.birth_date , "
        //                                                + " v_opt.op_id , v_opt.op_title,adr.street1,adr.city,adr.country,adr.state,adr.zipcode,  a_appt.Phone , "
        //                                                + " AType.def_id AS ApptType_EHR_ID, AType.descript AS ApptType_Name, "
        //                                                + " pat.patient_id AS patId, (LTRIM(RTRIM(pat.address_line1))) AS patAddress, (LTRIM(RTRIM(pat.city))) AS patCity, "
        //                                                + " (LTRIM(RTRIM(pat.state))) AS patState, V_appt.status_id AS appointment_status_ehr_key ,  (LTRIM(RTRIM(vasc.descript))) AS Appointment_Status, "
        //                                                + " (LTRIM(RTRIM(pat.zipcode))) AS patZipcode, (LTRIM(RTRIM(pat.home_phone))) AS patHomephone, "
        //                                                + " (LTRIM(RTRIM(pat.mobile_phone))) AS patMobile, (LTRIM(RTRIM(pat.email))) AS patEmail, "
        //                                                + " a_appt.automodifiedtimestamp AS EHR_Entry_DateTime"
        //                                                + " FROM admin.v_appointment V_appt "
        //                                                + " JOIN admin.v_operatory v_opt ON v_opt.op_id = V_appt.Operatory_id "
        //                                                + " JOIN admin.appt a_appt ON a_appt.apptid = V_appt.appointment_id "
        //                                                + " Left JOIN admin.address adr ON adr.addrid = a_appt.newpataddrid "
        //                                                + " Left JOIN admin.v_Appointment_Types AType ON AType.def_id = a_appt.appttype "
        //                                                + " Left JOIN admin.v_patient pat ON pat.patient_id = V_appt.patient_id "
        //                                                + " Left JOIN admin.V_appt_Status_Codes vasc ON vasc.defid = V_appt.status_id "
        //                                                + " WHERE V_appt.patient_name <> '' AND  appointment_date > ?";


        public static string GetDentrixAppointmentDataDTXG5 = "select convert(varchar(10),appt.appointment_id) as appointment_id,appt.patid ,appt.appointment_date ,appt.start_hour ,appt.start_minute ,appt.length,appt.provider_id,  0 as appt_flag , "
                                                 + "  appt.apptreason as comment,appt.patient_phone ,appt.op_id,appt.patname as patient_name ,pr.lastName as provider_last_name,pr.firstname as provider_first_name,p.lastname, "
                                                 + " p.firstname,p.mi,p.birthdate as birth_date,ad.street1,ad.city,ad.country,ad.state,ad.zipcode,op.title as op_title, AType.def_id AS ApptType_EHR_ID, AType.descript AS ApptType_Name,  "
                                                 + "  appt.status_id AS appointment_status_ehr_key ,  (LTRIM(RTRIM(vasc.descript))) AS Appointment_Status,p.otherphone as Phone, "
                                                 + " (LTRIM(RTRIM(ad.zipcode))) AS patZipcode, (LTRIM(RTRIM(ad.phone))) AS patHomephone, (LTRIM(RTRIM(ad.street2))) AS patAddress, (LTRIM(RTRIM(ad.city))) AS patCity, (LTRIM(RTRIM(ad.state))) AS patState, "
                                                 + " (LTRIM(RTRIM(p.pager))) AS patMobile, (LTRIM(RTRIM(p.emailaddr))) AS patEmail,appt.automodifiedtimestamp AS EHR_Entry_DateTime   "
                                                 + " from (select V_appt.apptid as appointment_id,V_appt.patid ,V_appt.apptdate as appointment_date ,V_appt.timehr as start_hour ,V_appt.timemin as start_minute ,V_appt.apptlen as length,V_appt.provid as provider_id, "
                                                 + " V_appt.apptreason,V_appt.phone as patient_phone ,V_appt.opid as op_id,V_appt.appttype ,V_appt.status as status_id,V_appt.automodifiedtimestamp ,V_appt.patname "
                                                 + " from admin.appt as V_appt where V_appt.patname <> '' and apptdate >= ?) as appt  "
                                                 + " Left join admin.patient as p on appt.patid = p.patid  "
                                                 + " Left join admin.address as ad on p.addrid = ad.addrid "
                                                 + " Left join admin.provider_view as pr on appt.provider_id = pr.provid  "
                                                 + " Left join admin.operatory_view as op on appt.op_id = op.opid "
                                                 + " Left JOIN admin.v_Appointment_Types AType ON AType.def_id = appt.appttype  "
                                                 + " Left JOIN admin.V_appt_Status_Codes vasc ON vasc.defid = appt.status_id ";

        public static string GetDentrixAppointmentEhrIdDTXG5 = "select appt.appointment_id As Appt_EHR_ID"
                                                 + " from (select V_appt.apptid as appointment_id,V_appt.patid ,V_appt.apptdate as appointment_date ,V_appt.timehr as start_hour ,V_appt.timemin as start_minute ,V_appt.apptlen as length,V_appt.provid as provider_id, "
                                                 + " V_appt.apptreason,V_appt.phone as patient_phone ,V_appt.opid as op_id,V_appt.appttype ,V_appt.status as status_id,V_appt.automodifiedtimestamp ,V_appt.patname "
                                                 + " from admin.appt as V_appt where V_appt.patname <> '' and apptdate >= ?) as appt  "
                                                 + " Left join admin.patient as p on appt.patid = p.patid  "
                                                 + " Left join admin.address as ad on p.addrid = ad.addrid "
                                                 + " Left join admin.provider_view as pr on appt.provider_id = pr.provid  "
                                                 + " Left join admin.operatory_view as op on appt.op_id = op.opid "
                                                 + " Left JOIN admin.v_Appointment_Types AType ON AType.def_id = appt.appttype  "
                                                 + " Left JOIN admin.V_appt_Status_Codes vasc ON vasc.defid = appt.status_id ";


        public static string GetDentrixDeletedAppointmentData = "Select parentapptid AS appointment_id, automodifiedtimestamp AS EHR_Entry_DateTime  "
                                                    + " from Admin.appthist Where deletedflag = 1 AND automodifiedtimestamp > ? ";

        #endregion

        #region OperatoryEvent
        //public static string GetDentrixOperatoryEventData = " select * from admin.v_appt_book_events WHERE modified_time_stamp > ? or event_date >= ?";
        public static string GetDentrixOperatoryEventData = " select * from admin.v_appt_book_events WHERE event_date >= ?";
        #endregion

        #region Holiday

        public static string GetDentrixHolidayData = "call admin.sp_getpracticeschedexceptions(?,?)";

        public static string GetDentrixOperatoryHolidaysData = "call admin.sp_getoperatoryschedexceptions(?,?,?)";

        #endregion

        #region Provider
        public static string GetDentrixProviderData = " SELECT provider_id AS Provider_EHR_ID, (LTRIM(RTRIM(last_name))) AS Last_Name, (LTRIM(RTRIM(first_name))) AS First_Name, (LTRIM(RTRIM(mi))) AS MI, "
                                                    + " (LTRIM(RTRIM(S.Description))) AS provider_speciality ,(case when inactive = 0 then 1 else 0 end) AS Is_Active "
                                                    + " FROM admin.v_Provider P Left JOIN admin.v_referral_specialty S ON S.defid = P.specialty_id ";

        public static string GetDentrixProviderDataG5 = " SELECT provider_id AS Provider_EHR_ID, (LTRIM(RTRIM(last_name))) AS Last_Name, (LTRIM(RTRIM(first_name))) AS First_Name, (LTRIM(RTRIM(mi))) AS MI, "
                                                    + " (LTRIM(RTRIM(S.Description))) AS provider_speciality ,1 AS is_active "
                                                    + " FROM admin.v_Provider P Left JOIN admin.v_referral_specialty S ON S.defid = P.specialty_id ";

        public static string GetDentrixProviderDatafulldef = " SELECT provid AS Provider_EHR_ID, (LTRIM(RTRIM(lastname))) AS Last_Name, (LTRIM(RTRIM(firstname))) AS First_Name, (LTRIM(RTRIM(mi))) AS MI, "
                                                  + " (LTRIM(RTRIM(S.Descript))) AS provider_speciality,1 AS is_active "
                                                  + " FROM admin.Provider_view P Left JOIN admin.fulldef S ON S.defid = P.specialty AND S.deftype= 25 ";
        #endregion

        #region ProviderOfficeHours
        public static string GetDentrixPRoviderIds = " SELECT provider_id AS Provider_EHR_Id FROM admin.v_Provider ";

        public static string GetDentrixProviderOfficeHours = "CALL admin.sp_getproviderschedule('@provider_Id')";

        public static string GetDentrixProviderHoursData = "call admin.sp_getprovschedexceptions(?,?,?)";

        #endregion

        #region FolderList

        public static string GetDentrixFolderListData = "SELECT  name as Folder_Name,dcdoctypeid as FolderList_EHR_ID FROM admin.doccat";

        #endregion

        #region Operatory

        public static string GetDentrixOperatoryData = "SELECT opid AS Operatory_EHR_ID , (LTRIM(RTRIM(title))) AS Operatory_Name, 0 as OperatoryOrder FROM admin.Operatory_view order by opid ";

        #endregion

        #region OperatoryOfficeHours
        public static string GetDentrixOperatoryIds = " SELECT op_id AS Operatory_EHR_Id FROM admin.v_Operatory ";

        public static string GetDentrixOperatoryOfficeHours = "CALL admin.sp_getOperatoryschedule('@op_id')";

        public static string GetDentrixOperatoryHoursData = "call admin.sp_getoperatoryschedexceptions(?,?,?)";
        #endregion

        #region ApptType
        public static string GetDentrixApptTypeData = " SELECT def_id AS ApptType_EHR_ID, (LTRIM(RTRIM(descript))) AS Type_Name FROM admin.v_Appointment_Types";
        #endregion

        #region Patient
        //public static string GetDentrixPatientData = " SELECT v_p.patient_id AS Patient_EHR_ID, (LTRIM(RTRIM(v_p.first_name))) AS First_name, (LTRIM(RTRIM(v_p.last_name))) AS Last_name, "
        //                                           + " (LTRIM(RTRIM(v_p.mi))) AS Middle_Name, (LTRIM(RTRIM(v_p.Salutation))) AS Salutation,(LTRIM(RTRIM(v_p.preferred_name))) AS preferred_name,"
        //                                           + "  Cast(v_p.status As Char) AS Status, v_p.birth_date AS Birth_Date, (LTRIM(RTRIM(v_p.email))) AS Email, (LTRIM(RTRIM(v_p.mobile_phone))) AS Mobile, "
        //                                           + " (LTRIM(RTRIM(v_p.home_phone))) AS Home_Phone, (LTRIM(RTRIM(v_p.work_phone))) AS Work_Phone, (LTRIM(RTRIM(v_p.address_line1))) AS Address1, "
        //                                           + " (LTRIM(RTRIM(v_p.address_line2))) AS Address2, (LTRIM(RTRIM(v_p.city))) AS City, (LTRIM(RTRIM(v_p.state))) AS State, "
        //                                           + " (LTRIM(RTRIM(v_p.zipcode))) AS Zipcode,v_p.guar_id AS Guar_ID, Convert(varchar(10),v_p.gender) as sex,Convert(varchar(10),p.fampos) as MaritalStatus, "
        //                                           + " v_p.pri_provider_id AS Pri_Provider_ID, v_p.sec_provider_id AS Sec_Provider_ID, "
        //                                           + " v_p.first_visit_date AS FirstVisit_Date, v_p.last_visit_date AS LastVisit_Date, "
        //                                           + " v_pi.primary_insurance_carrier_id AS Primary_Insurance, "
        //                                           + " (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))) AS Primary_Insurance_CompanyName, "
        //                                           + " v_pi.secondary_insurance_carrier_id AS Secondary_Insurance, "
        //                                           + " (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))) AS Secondary_Insurance_CompanyName, "
        //                                           + " (ISNULL(v_gb.balance_0_30_days,0)) + (ISNULL(v_gb.balance_31_60_days,0)) + (ISNULL(v_gb.balance_61_90_days,0)) + (ISNULL(v_gb.balance_91_plus_days,0)) as CurrentBal, " 
        //                                           + " (ISNULL(v_gb.balance_0_30_days,0)) AS ThirtyDay, (ISNULL(v_gb.balance_31_60_days,0)) AS SixtyDay, "
        //                                           + " (ISNULL(v_gb.balance_61_90_days,0)) AS NinetyDay,(ISNULL(v_gb.balance_91_plus_days,0)) AS Over90,  ''  AS nextvisit_date,'' AS due_date , "
        //                                           + " ((ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid = v_pi.primary_insurance_carrier_id ),0) "
        //                                           + " + (ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid =v_pi.secondary_insurance_carrier_id ),0))) "
        //                                           + " - (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))) as remaining_benefit , "
        //                                           + " (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0)) as used_benefit, "
        //                                           + " '' AS collect_payment, v_p.privacy_flags, 1 AS InsUptDlt, 'Y' As ReceiveSMS, 'Y' As ReceiveEmail"
        //                                           + " FROM admin.v_patient v_p JOIN admin.v_patient_insurance v_pi ON v_p.patient_id = v_pi.patient_id "
        //                                           + " Left join admin.patient as p on p.patid = v_p.patient_id "
        //                                           + " Left Join admin.v_guarantor_balance v_gb ON v_p.guar_id = v_gb.guar_id  ";

        public static string GetDentrixPatientData = "SELECT 0 as Clinic_Number, 1 as Service_Install_ID, v_p.patient_id AS Patient_EHR_ID, (LTRIM(RTRIM(v_p.first_name))) AS First_name, (LTRIM(RTRIM(v_p.last_name))) AS Last_name, "
                                                    + " (LTRIM(RTRIM(v_p.mi))) AS Middle_Name, (LTRIM(RTRIM(v_p.Salutation))) AS Salutation,(LTRIM(RTRIM(v_p.preferred_name))) AS preferred_name, "
                                                    + " case when Cast(v_p.status As Char) = '1' then 'A' else 'I' End AS Status,case when v_p.status  = 1 then 'Active'  when v_p.status = 2 then 'NonPatient' when v_p.status = 3 then 'InActive' when v_p.status = 4 then 'InActive' END AS EHR_Status, v_p.birth_date AS Birth_Date, (LTRIM(RTRIM(v_p.email))) AS Email, (LTRIM(RTRIM(v_p.mobile_phone))) AS Mobile, "
                                                    + " (LTRIM(RTRIM(v_p.home_phone))) AS Home_Phone, (LTRIM(RTRIM(v_p.work_phone))) AS Work_Phone, (LTRIM(RTRIM(v_p.address_line1))) AS Address1, "
                                                    + " (LTRIM(RTRIM(v_p.address_line2))) AS Address2, (LTRIM(RTRIM(v_p.city))) AS City, (LTRIM(RTRIM(v_p.state))) AS State,case when privacyflags in (1,3,5,7) then 'N' else 'Y' end as ReceiveVoiceCall, "
                                                    + " (LTRIM(RTRIM(v_p.zipcode))) AS Zipcode,v_p.guar_id AS Guar_ID, "
                                                    + " case when Convert(varchar(10),v_p.gender) != '1' and  Convert(varchar(10),v_p.gender) != '2' then 'Unknown' "
                                                    + " when Convert(varchar(10),v_p.gender) = '2' then 'Female' else 'Male' end as sex, "
                                                    + " case when Convert(varchar(10),p.fampos) = '2' then 'Married'  "
                                                    + " when Convert(varchar(10),p.fampos) = '1' then 'Single'  "
                                                    + " when Convert(varchar(10),p.fampos) = '3' then 'Child'  "
                                                    + " when Convert(varchar(10),p.fampos) = '4' then 'Other' else 'Single' end as MaritalStatus,  "
                                                    + " v_p.pri_provider_id AS Pri_Provider_ID, v_p.sec_provider_id AS Sec_Provider_ID, "
                                                    + " v_p.first_visit_date AS FirstVisit_Date, v_p.last_visit_date AS LastVisit_Date, "
                                                    + " v_pi.primary_insurance_carrier_id AS Primary_Insurance, "
                                                    + " (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))) AS Primary_Insurance_CompanyName, "
                                                    + " v_pi.secondary_insurance_carrier_id AS Secondary_Insurance, "
                                                    + " (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))) AS Secondary_Insurance_CompanyName, "
                                                    + " (ISNULL(v_gb.balance_0_30_days,0)) + (ISNULL(v_gb.balance_31_60_days,0)) + (ISNULL(v_gb.balance_61_90_days,0)) +  (ISNULL(v_gb.balance_91_plus_days,0)) as CurrentBal,  "
                                                    + " (ISNULL(v_gb.balance_0_30_days,0)) AS ThirtyDay, (ISNULL(v_gb.balance_31_60_days,0)) AS SixtyDay, "
                                                    + " (ISNULL(v_gb.balance_61_90_days,0)) AS NinetyDay,(ISNULL(v_gb.balance_91_plus_days,0)) AS Over90,  nappt.Nextvisit_date  AS nextvisit_date,'' AS due_date , "
                                                    + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN ((ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid = v_pi.primary_insurance_carrier_id ),0) "
                                                    + " + (ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid =v_pi.secondary_insurance_carrier_id ),0))) "
                                                    + "- (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))) ELSE 0  END as remaining_benefit , "
                                                    + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))  ELSE 0  END as used_benefit, "
                                                    + "collect_payment AS collect_payment, v_p.privacy_flags, 1 AS InsUptDlt, "
                                                   + "case when (v_p.privacy_flags = 2 or v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveSMS,case when (v_p.privacy_flags = 2 or  v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveEmail,( case when nt.patient_note like '%Spanish%' then 'Spanish' when nt.patient_note like '%French%' then 'French' else 'English' end ) AS PreferredLanguage, NT.patient_Note AS Patient_Note,  "

                                                   + " v_p.social_sec_num AS ssn, v_p.driverslicense AS driverlicense, " //pat.empid,pat.claiminfid,v_p.patient_id,groupnum, patguar.guar_id" //v_pi.primary_insured_id,groupnum, patguar.guar_id
                                                   + " empl.name AS employer,claim.school AS school,pinsr.phone AS Prim_Ins_Company_Phonenumber,sinsr.phone AS Sec_Ins_Company_Phonenumber," //,patprins.groupnum AS groupid,                                                    
                                                   + " resp_party.guar_id AS responsiblepartyId, resp_party.first_name AS ResponsibleParty_First_Name, resp_party.last_name AS ResponsibleParty_Last_Name, "
                                                   + " resp_party.social_sec_num AS responsiblepartyssn, resp_party.birth_date AS responsiblepartybirthdate,"
                                                   + " '' AS groupid, '' AS emergencycontactId, '' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name,v_p.status as Ar_status "
                                                   + "  ,inprim.idnum as Primary_Ins_Subscriber_ID, insec.idnum as Secondary_Ins_Subscriber_ID "
                                                    + "FROM admin.v_patient v_p JOIN admin.v_patient_insurance v_pi ON v_p.patient_id = v_pi.patient_id "
                                                    + "Left join admin.patient as p on p.patid = v_p.patient_id "
                                                    + " left join admin.insured inprim on p.priminsuredid= inprim.insuredid "
                                                    + " left join admin.insured insec on p.secinsuredid= insec.insuredid "
                                                    + "Left Join admin.v_guarantor_balance v_gb ON v_p.guar_id = v_gb.guar_id "
                                                    + "LEFT JOIN ( Select FN.noteid as patid,fn.notetext as Patient_Note from admin.fullnotes FN where fn.notetype = 3 ) AS NT ON  v_p.patient_id = NT.PatId "
                                                   + "left join (select ap.patid as patient_id ,Min (convert(varchar(10), ap.apptdate) +' '+convert(varchar(10), ap.timehr)+':'+convert(varchar(10),ap.timemin)+':00') AS Nextvisit_date  from admin.appt as ap inner join (Select patid,min(apptdate) as apptdate from admin.appt  where apptdate >= ?  and status != -106 GROUP BY patid ) as v_S on v_S.patid = ap.patid and v_S.apptdate = ap.apptdate group by ap.patid) as nappt on nappt.patient_id = v_p.patient_id "
                                                   + "Left join (Select patid AS Patient_EHR_ID,isnull(Sum(amt),0) AS collect_payment from  admin.payment_view group by patid) as cp on cp.Patient_EHR_ID = v_p.patient_id "
                                                   + " LEFT JOIN admin.employer empl ON empl.empid = v_p.employer_id "
                                                   + " LEFT JOIN admin.claiminfo claim ON claim.claiminfid = p.claiminfid"
                                                   + " LEFT JOIN admin.inscarrier pinsr ON pinsr.insid = v_p.primdentalinsuredid"
                                                   + " LEFT JOIN admin.inscarrier sinsr ON sinsr.insid = v_p.secdentalinsuredid"
                                                   + " LEFT JOIN admin.v_guarantor_balance resp_party ON resp_party.guar_id = v_p.guar_id";

        public static string GetDentrixNewAllPatientData = "SELECT 0 as Clinic_Number, 1 as Service_Install_ID, v_p.patient_id AS Patient_EHR_ID, (LTRIM(RTRIM(v_p.first_name))) AS First_name, (LTRIM(RTRIM(v_p.last_name))) AS Last_name, "
                                            + " (LTRIM(RTRIM(v_p.mi))) AS Middle_Name, (LTRIM(RTRIM(v_p.Salutation))) AS Salutation,(LTRIM(RTRIM(v_p.preferred_name))) AS preferred_name, "
                                            + " case when Cast(v_p.status As Char) = '1' then 'A' else 'I' End AS Status,case when v_p.status  = 1 then 'Active'  when v_p.status = 2 then 'NonPatient' when v_p.status = 3 then 'InActive' when v_p.status = 4 then 'InActive' END AS EHR_Status, v_p.birth_date AS Birth_Date, (LTRIM(RTRIM(v_p.email))) AS Email, (LTRIM(RTRIM(v_p.mobile_phone))) AS Mobile, "
                                            + " (LTRIM(RTRIM(v_p.home_phone))) AS Home_Phone, (LTRIM(RTRIM(v_p.work_phone))) AS Work_Phone, (LTRIM(RTRIM(v_p.address_line1))) AS Address1, "
                                            + " (LTRIM(RTRIM(v_p.address_line2))) AS Address2, (LTRIM(RTRIM(v_p.city))) AS City, (LTRIM(RTRIM(v_p.state))) AS State,case when privacyflags in (1,3,5,7) then 'N' else 'Y' end as ReceiveVoiceCall, "
                                            + " (LTRIM(RTRIM(v_p.zipcode))) AS Zipcode,v_p.guar_id AS Guar_ID, "
                                            + " case when Convert(varchar(10),v_p.gender) != '1' and  Convert(varchar(10),v_p.gender) != '2' then 'Unknown' "
                                            + " when Convert(varchar(10),v_p.gender) = '2' then 'Female' else 'Male' end as sex, "
                                            + " case when Convert(varchar(10),p.fampos) = '2' then 'Married'  "
                                            + " when Convert(varchar(10),p.fampos) = '1' then 'Single'  "
                                            + " when Convert(varchar(10),p.fampos) = '3' then 'Child'  "
                                            + " when Convert(varchar(10),p.fampos) = '4' then 'Other' else 'Single' end as MaritalStatus,  "
                                            + " v_p.pri_provider_id AS Pri_Provider_ID, v_p.sec_provider_id AS Sec_Provider_ID, "
                                            + " v_p.first_visit_date AS FirstVisit_Date, v_p.last_visit_date AS LastVisit_Date, "
                                            + " v_pi.primary_insurance_carrier_id AS Primary_Insurance, "
                                            + " (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))) AS Primary_Insurance_CompanyName, "
                                            + " v_pi.secondary_insurance_carrier_id AS Secondary_Insurance, "
                                            + " (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))) AS Secondary_Insurance_CompanyName, "
                                            + " (ISNULL(v_gb.balance_0_30_days,0)) + (ISNULL(v_gb.balance_31_60_days,0)) + (ISNULL(v_gb.balance_61_90_days,0)) +  (ISNULL(v_gb.balance_91_plus_days,0)) as CurrentBal,  "
                                            + " (ISNULL(v_gb.balance_0_30_days,0)) AS ThirtyDay, (ISNULL(v_gb.balance_31_60_days,0)) AS SixtyDay, "
                                            + " (ISNULL(v_gb.balance_61_90_days,0)) AS NinetyDay,(ISNULL(v_gb.balance_91_plus_days,0)) AS Over90,  nappt.Nextvisit_date  AS nextvisit_date,'' AS due_date , "
                                            + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN ((ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid = v_pi.primary_insurance_carrier_id ),0) "
                                            + " + (ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid =v_pi.secondary_insurance_carrier_id ),0))) "
                                            + "- (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))) ELSE 0  END as remaining_benefit , "
                                            + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))  ELSE 0  END as used_benefit, "
                                            + "collect_payment AS collect_payment, v_p.privacy_flags, 1 AS InsUptDlt, "
                                           + "case when (v_p.privacy_flags = 2 or v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveSMS,case when (v_p.privacy_flags = 2 or  v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveEmail,( case when nt.patient_note like '%Spanish%' then 'Spanish' when nt.patient_note like '%French%' then 'French' else 'English' end ) AS PreferredLanguage, NT.patient_Note AS Patient_Note,  "

                                           + " v_p.social_sec_num AS ssn, v_p.driverslicense AS driverlicense, " //pat.empid,pat.claiminfid,v_p.patient_id,groupnum, patguar.guar_id" //v_pi.primary_insured_id,groupnum, patguar.guar_id
                                           + " empl.name AS employer,claim.school AS school,pinsr.phone AS Prim_Ins_Company_Phonenumber,sinsr.phone AS Sec_Ins_Company_Phonenumber," //,patprins.groupnum AS groupid,                                                    
                                           + " resp_party.guar_id AS responsiblepartyId, resp_party.first_name AS ResponsibleParty_First_Name, resp_party.last_name AS ResponsibleParty_Last_Name, "
                                           + " resp_party.social_sec_num AS responsiblepartyssn, resp_party.birth_date AS responsiblepartybirthdate,"
                                           + " '' AS groupid, '' AS emergencycontactId, '' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name,v_p.status as Ar_status "
                                            + "  ,inprim.idnum as Primary_Ins_Subscriber_ID, insec.idnum as Secondary_Ins_Subscriber_ID "                                                   
                                            + "FROM admin.v_patient v_p JOIN admin.v_patient_insurance v_pi ON v_p.patient_id = v_pi.patient_id "
                                            + "Left join admin.patient as p on p.patid = v_p.patient_id "
                                            + " left join admin.insured inprim on p.priminsuredid= inprim.insuredid "
                                                    + " left join admin.insured insec on p.secinsuredid= insec.insuredid "
                                            + "Left Join admin.v_guarantor_balance v_gb ON v_p.guar_id = v_gb.guar_id "
                                            + "LEFT JOIN ( Select FN.noteid as patid,fn.notetext as Patient_Note from admin.fullnotes FN where fn.notetype = 3 ) AS NT ON  v_p.patient_id = NT.PatId "
                                           + "left join (select ap.patid as patient_id ,Min (convert(varchar(10), ap.apptdate) +' '+convert(varchar(10), ap.timehr)+':'+convert(varchar(10),ap.timemin)+':00') AS Nextvisit_date  from admin.appt as ap inner join (Select patid,min(apptdate) as apptdate from admin.appt  where apptdate >= ?  and status != -106 GROUP BY patid ) as v_S on v_S.patid = ap.patid and v_S.apptdate = ap.apptdate group by ap.patid) as nappt on nappt.patient_id = v_p.patient_id "
                                           + "Left join (Select patid AS Patient_EHR_ID,isnull(Sum(amt),0) AS collect_payment from  admin.payment_view group by patid) as cp on cp.Patient_EHR_ID = v_p.patient_id "
                                           + " LEFT JOIN admin.employer empl ON empl.empid = v_p.employer_id "
                                           + " LEFT JOIN admin.claiminfo claim ON claim.claiminfid = p.claiminfid"
                                           + " LEFT JOIN admin.inscarrier pinsr ON pinsr.insid = v_p.primdentalinsuredid"
                                           + " LEFT JOIN admin.inscarrier sinsr ON sinsr.insid = v_p.secdentalinsuredid"
                                           + " LEFT JOIN admin.v_guarantor_balance resp_party ON resp_party.guar_id = v_p.guar_id Where v_p.patient_id in (@PaientEHRIDs)";


        public static string GetDentrixAppointmentsPatientData = "SELECT 0 as Clinic_Number, 1 as Service_Install_ID, v_p.patient_id AS Patient_EHR_ID, (LTRIM(RTRIM(v_p.first_name))) AS First_name, (LTRIM(RTRIM(v_p.last_name))) AS Last_name, "
                                                    + " (LTRIM(RTRIM(v_p.mi))) AS Middle_Name, (LTRIM(RTRIM(v_p.Salutation))) AS Salutation,(LTRIM(RTRIM(v_p.preferred_name))) AS preferred_name, "
                                                    + " case when Cast(v_p.status As Char) = '1' then 'A' else 'I' End AS Status,case when v_p.status  = 1 then 'Active'  when v_p.status = 2 then 'NonPatient' when v_p.status = 3 then 'InActive' when v_p.status = 4 then 'InActive' END AS EHR_Status, v_p.birth_date AS Birth_Date, (LTRIM(RTRIM(v_p.email))) AS Email, (LTRIM(RTRIM(v_p.mobile_phone))) AS Mobile, "
                                                    + " (LTRIM(RTRIM(v_p.home_phone))) AS Home_Phone, (LTRIM(RTRIM(v_p.work_phone))) AS Work_Phone, (LTRIM(RTRIM(v_p.address_line1))) AS Address1, "
                                                    + " (LTRIM(RTRIM(v_p.address_line2))) AS Address2, (LTRIM(RTRIM(v_p.city))) AS City, (LTRIM(RTRIM(v_p.state))) AS State,case when privacyflags in (1,3,5,7) then 'N' else 'Y' end as ReceiveVoiceCall, "
                                                    + " (LTRIM(RTRIM(v_p.zipcode))) AS Zipcode,v_p.guar_id AS Guar_ID, "
                                                    + " case when Convert(varchar(10),v_p.gender) != '1' and  Convert(varchar(10),v_p.gender) != '2' then 'Unknown' "
                                                    + " when Convert(varchar(10),v_p.gender) = '2' then 'Female' else 'Male' end as sex, "
                                                    + " case when Convert(varchar(10),p.fampos) = '2' then 'Married' "
                                                    + " when Convert(varchar(10),p.fampos) = '1' then 'Single' "
                                                    + " when Convert(varchar(10),p.fampos) = '3' then 'Child' "
                                                    + " when Convert(varchar(10),p.fampos) = '4' then 'Other' else 'Single' end as MaritalStatus,  "
                                                    + " v_p.pri_provider_id AS Pri_Provider_ID, v_p.sec_provider_id AS Sec_Provider_ID, "
                                                    + " v_p.first_visit_date AS FirstVisit_Date, v_p.last_visit_date AS LastVisit_Date, "
                                                    + " v_pi.primary_insurance_carrier_id AS Primary_Insurance, "
                                                    + " (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))) AS Primary_Insurance_CompanyName, "
                                                    + " v_pi.secondary_insurance_carrier_id AS Secondary_Insurance, "
                                                    + " (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))) AS Secondary_Insurance_CompanyName, "
                                                    + " (ISNULL(v_gb.balance_0_30_days,0)) + (ISNULL(v_gb.balance_31_60_days,0)) + (ISNULL(v_gb.balance_61_90_days,0)) +  (ISNULL(v_gb.balance_91_plus_days,0)) as CurrentBal,  "
                                                    + " (ISNULL(v_gb.balance_0_30_days,0)) AS ThirtyDay, (ISNULL(v_gb.balance_31_60_days,0)) AS SixtyDay, "
                                                    + " (ISNULL(v_gb.balance_61_90_days,0)) AS NinetyDay,(ISNULL(v_gb.balance_91_plus_days,0)) AS Over90,  nappt.Nextvisit_date  AS nextvisit_date,'' AS due_date , "
                                                    + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN ((ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid = v_pi.primary_insurance_carrier_id ),0) "
                                                    + " + (ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid =v_pi.secondary_insurance_carrier_id ),0))) "
                                                    + "- (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))) ELSE 0  END as remaining_benefit , "
                                                    + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))  ELSE 0  END as used_benefit, "
                                                    + "collect_payment AS collect_payment, v_p.privacy_flags, 1 AS InsUptDlt, "
                                                     + "case when (v_p.privacy_flags = 2 or v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveSMS,case when (v_p.privacy_flags = 2 or  v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveEmail,( case when nt.patient_note like '%Spanish%' then 'Spanish' when nt.patient_note like '%french%' then 'French' else 'English' end ) AS PreferredLanguage, NT.patient_Note AS Patient_Note,  "

                                                     + " v_p.social_sec_num AS ssn, v_p.driverslicense AS driverlicense, " //pat.empid,pat.claiminfid,v_p.patient_id,groupnum, patguar.guar_id" //v_pi.primary_insured_id,groupnum, patguar.guar_id
                                                     + " empl.name AS employer,claim.school AS school,pinsr.phone AS Prim_Ins_Company_Phonenumber,sinsr.phone AS Sec_Ins_Company_Phonenumber," //,patprins.groupnum AS groupid,                                                    
                                                     + " resp_party.guar_id AS responsiblepartyId, resp_party.first_name AS ResponsibleParty_First_Name, resp_party.last_name AS ResponsibleParty_Last_Name, "
                                                     + " resp_party.social_sec_num AS responsiblepartyssn, resp_party.birth_date AS responsiblepartybirthdate,"
                                                     + " '' AS groupid, '' AS emergencycontactId, '' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name,v_p.status as Ar_status "
                                                     + "  ,inprim.idnum as Primary_Ins_Subscriber_ID, insec.idnum as Secondary_Ins_Subscriber_ID "
                                                    + " FROM admin.v_patient v_p JOIN admin.v_patient_insurance v_pi ON v_p.patient_id = v_pi.patient_id "

                                                    + Environment.NewLine + " Inner Join admin.v_appointment V_appt On v_p.patient_id = V_appt.patient_id"
                                                    //+ " (SELECT Distinct pat.patient_id AS patId FROM admin.v_appointment V_appt "
                                                    //+ " JOIN admin.v_operatory v_opt ON v_opt.op_id = V_appt.Operatory_id "
                                                    //+ " JOIN admin.appt a_appt ON a_appt.apptid = V_appt.appointment_id "
                                                    //+ " Left JOIN admin.address adr ON adr.addrid = a_appt.newpataddrid "
                                                    //+ " Left JOIN admin.v_Appointment_Types AType ON AType.def_id = a_appt.appttype "
                                                    //+ " Left JOIN admin.v_notes v_not ON v_not.noteid = a_appt.newpataddrid  AND v_not.notetype = 116"
                                                    //+ " Left JOIN admin.v_patient pat ON pat.patient_id = V_appt.patient_id "
                                                    //+ " Left JOIN admin.V_appt_Status_Codes vasc ON vasc.defid = V_appt.status_id "
                                                    //+ " WHERE V_appt.patient_name <> '' AND V_appt.appointment_date > ?) "


                                                    + Environment.NewLine + " Left join admin.patient as p on p.patid = v_p.patient_id "
                                                    + " left join admin.insured inprim on p.priminsuredid= inprim.insuredid "
                                                    + " left join admin.insured insec on p.secinsuredid= insec.insuredid "
                                                    + " Left Join admin.v_guarantor_balance v_gb ON v_p.guar_id = v_gb.guar_id "
                                                     + "LEFT JOIN ( Select FN.noteid as patid,fn.notetext as Patient_Note from admin.fullnotes FN where fn.notetype = 3 ) AS NT ON  v_p.patient_id = NT.PatId "
                                                     + " left join (select ap.patid as patient_id ,Min (convert(varchar(10), ap.apptdate) +' '+convert(varchar(10), ap.timehr)+':'+convert(varchar(10),ap.timemin)+':00') AS Nextvisit_date  from admin.appt as ap inner join (Select patid,min(apptdate) as apptdate from admin.appt  where apptdate >= ?  and status != -106 GROUP BY patid ) as v_S on v_S.patid = ap.patid and v_S.apptdate = ap.apptdate group by ap.patid) as nappt on nappt.patient_id = v_p.patient_id "
                                                    + " Left join (Select patid AS Patient_EHR_ID,isnull(Sum(amt),0) AS collect_payment from  admin.payment_view group by patid) as cp on cp.Patient_EHR_ID = v_p.patient_id "
                                                     + " LEFT JOIN admin.employer empl ON empl.empid = v_p.employer_id "
                                                     + " LEFT JOIN admin.claiminfo claim ON claim.claiminfid = p.claiminfid"
                                                     + " LEFT JOIN admin.inscarrier pinsr ON pinsr.insid = v_p.primdentalinsuredid"
                                                     + " LEFT JOIN admin.inscarrier sinsr ON sinsr.insid = v_p.secdentalinsuredid"
                                                     + " LEFT JOIN admin.v_guarantor_balance resp_party ON resp_party.guar_id = v_p.guar_id"
                                                     + " Where (V_appt.patient_name <> '' AND V_appt.appointment_date >= ?)";

        #region New Appointment Patient
        public static string GetAppointmentPatientData1 = "SELECT 0                                                       AS Clinic_Number, " +
       "1                                                       AS " +
       "Service_Install_ID, " +
       "v_p.patid AS Patient_EHR_ID, " +
       "(Ltrim(Rtrim(v_p.firstname)) )                        AS " +
      "First_name, " +
       "( Ltrim(Rtrim(v_p.lastname)) )                         AS Last_name, " +
       "( Ltrim(Rtrim(v_p.mi)) )                                AS Middle_Name, " +
       "( Ltrim(Rtrim(v_p.salutation)) )                        AS Salutation, " +
       "( Ltrim(Rtrim(v_p.prefname)) )                    AS preferred_name, " +
      "CASE WHEN Cast(v_p.status AS CHAR) = '1' THEN 'A' ELSE 'I' END AS Status, " +
       "CASE WHEN v_p.status = 1 THEN 'Active' WHEN v_p.status = 2 THEN 'NonPatient' WHEN v_p.status = 3 THEN 'InActive' WHEN v_p.status = 4 THEN 'InActive' END AS EHR_Status, " +
       "v_p.birthdate AS Birth_Date, " +
       "(Ltrim(Rtrim(v_p.emailaddr)) )                          AS Email, " +
       "( Ltrim(Rtrim(v_p.pager)) )                      AS Mobile, " +
       "( Ltrim(Rtrim(v_p.workphone)) )                        AS Work_Phone, " +
       " a.phone as Home_Phone, a.street1 as Address1, a.street2 as Address2, a.city, a.state, a.zipcode, " +
       " CASE WHEN V_P.PrivacyFlags IN ( 1, 3, 5, 7 ) THEN 'N' ELSE 'Y' END AS ReceiveVoiceCall, " +
       "v_p.guarid AS Guar_ID, " +
       "CASE WHEN CONVERT(VARCHAR(10), v_p.gender) != '1' AND CONVERT(VARCHAR(10), v_p.gender) != '2' THEN 'Unknown' WHEN CONVERT(VARCHAR(10), v_p.gender) = '2' THEN 'Female' ELSE 'Male' END AS sex, " +
       "CASE WHEN CONVERT(VARCHAR(10), v_p.fampos) = '2' THEN 'Married' WHEN CONVERT(VARCHAR(10), v_p.fampos) = '1' THEN 'Single' WHEN CONVERT(VARCHAR(10), v_p.fampos) = '3' THEN 'Child' WHEN CONVERT(VARCHAR(10), v_p.fampos) = '4' THEN 'Other' ELSE 'Single' END AS MaritalStatus, " +
       "v_p.ProvID1 AS Pri_Provider_ID, " +
       "v_p.ProvID2 AS Sec_Provider_ID, " +
       "v_p.firstvisitdate AS FirstVisit_Date, " +
       "v_p.lastvisitdate AS LastVisit_Date, " +
       "v_p.privacyflags, " +
       "1                                                       AS InsUptDlt, " +
       "CASE WHEN(v_p.privacyflags = 2 OR v_p.privacyflags = 3 OR v_p.privacyflags = 6 OR v_p.privacyflags = 7) THEN 'N' ELSE 'Y' END AS ReceiveSMS, " +
       "CASE WHEN(v_p.privacyflags = 2 OR v_p.privacyflags = 3 OR v_p.privacyflags = 6 OR v_p.privacyflags = 7) THEN 'N' ELSE 'Y' END AS ReceiveEmail, " +
       "v_p.SSN AS ssn, " +
       "v_p.driverslicense AS driverlicense, " +
       "v_p.status AS Ar_status, " +
       "v_p.empid, " +
       "v_p.claiminfid " +
       "FROM admin.patient v_p " +
       "Inner Join admin.appt V_appt On v_p.patid = V_appt.patid " +
        "Left Join admin.address A on A.addrid = v_p.addrid " +
       "Where (V_appt.patname<> '' AND V_appt.apptdate >= ?)";

        public static string GetDentrixPatientData1 = "SELECT 0 AS Clinic_Number, " +
                                                      "1 AS " +
                                                      "Service_Install_ID, " +
                                                      "v_p.patid AS Patient_EHR_ID, " +
                                                      "(Ltrim(Rtrim(v_p.firstname))) AS First_name, " +
                                                      "( Ltrim(Rtrim(v_p.lastname))) AS Last_name, " +
                                                      "( Ltrim(Rtrim(v_p.mi))) AS Middle_Name, " +
                                                      "( Ltrim(Rtrim(v_p.salutation))) AS Salutation, " +
                                                      "( Ltrim(Rtrim(v_p.prefname))) AS preferred_name, " +
                                                      "CASE WHEN Cast(v_p.status AS CHAR) = '1' THEN 'A' ELSE 'I' END AS Status, " +
                                                      "CASE WHEN v_p.status = 1 THEN 'Active' WHEN v_p.status = 2 THEN 'NonPatient' WHEN v_p.status = 3 THEN 'InActive' WHEN v_p.status = 4 THEN 'InActive' END AS EHR_Status, " +
                                                      "v_p.birthdate AS Birth_Date, " +
                                                      "(Ltrim(Rtrim(v_p.emailaddr))) AS Email, " +
                                                      "(Ltrim(Rtrim(v_p.pager))) AS Mobile, " +
                                                      "(Ltrim(Rtrim(v_p.workphone))) AS Work_Phone, " +
                                                      "a.phone as Home_Phone, a.street1 as Address1, a.street2 as Address2, a.city, a.state, a.zipcode, " +
                                                      "CASE WHEN V_P.PrivacyFlags IN ( 1, 3, 5, 7 ) THEN 'N' ELSE 'Y' END AS ReceiveVoiceCall, " +
                                                      "v_p.guarid AS Guar_ID, " +
                                                      "CASE WHEN CONVERT(VARCHAR(10), v_p.gender) != '1' AND CONVERT(VARCHAR(10), v_p.gender) != '2' THEN 'Unknown' WHEN CONVERT(VARCHAR(10), v_p.gender) = '2' THEN 'Female' ELSE 'Male' END AS sex, " +
                                                      "CASE WHEN CONVERT(VARCHAR(10), v_p.fampos) = '2' THEN 'Married' WHEN CONVERT(VARCHAR(10), v_p.fampos) = '1' THEN 'Single' WHEN CONVERT(VARCHAR(10), v_p.fampos) = '3' THEN 'Child' WHEN CONVERT(VARCHAR(10), v_p.fampos) = '4' THEN 'Other' ELSE 'Single' END AS MaritalStatus, " +
                                                      "v_p.ProvID1 AS Pri_Provider_ID, " +
                                                      "v_p.ProvID2 AS Sec_Provider_ID, " +
                                                      "v_p.firstvisitdate AS FirstVisit_Date, " +
                                                      "v_p.lastvisitdate AS LastVisit_Date, " +
                                                      "v_p.privacyflags, " +
                                                      "1 AS InsUptDlt, " +
                                                      "CASE WHEN(v_p.privacyflags = 2 OR v_p.privacyflags = 3 OR v_p.privacyflags = 6 OR v_p.privacyflags = 7) THEN 'N' ELSE 'Y' END AS ReceiveSMS, " +
                                                      "CASE WHEN(v_p.privacyflags = 2 OR v_p.privacyflags = 3 OR v_p.privacyflags = 6 OR v_p.privacyflags = 7) THEN 'N' ELSE 'Y' END AS ReceiveEmail, " +
                                                      "v_p.SSN AS ssn, " +
                                                      "v_p.driverslicense AS driverlicense, " +
                                                      "v_p.status AS Ar_status, " +
                                                      "v_p.empid, " +
                                                      "v_p.claiminfid " +
                                                      "FROM admin.patient v_p " +
                                                      "Left Join admin.address A on A.addrid = v_p.addrid ";

        public static string GetAppointmentPatientDataEmpl = "Select * from admin.employer";

        public static string GetAppointmentPatientDataClaimInfo = "Select * from admin.claiminfo where claiminfid in (SELECT v_p.claiminfid FROM admin.patient v_p) ";

        public static string GetAppointmentPatietnDataInsurance = "Select p.patid,i.insplanid as Primary_Insurance, ic.insconame As Primary_Insurance_CompanyName, ic.phone AS Prim_Ins_Company_Phonenumber," +
                                                                  "i2.insplanid as Secondary_Insurance, ic2.insconame As Secondary_Insurance_CompanyName, ic2.phone AS Sec_Ins_Company_Phonenumber, " +
                                                                  " case when(isnull((LTRIM(RTRIM(ic.insconame))),'') != '' OR ISNULL((LTRIM(RTRIM(ic2.insconame))),'') != '')  " +
                                                                  " THEN((ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid = i.insplanid),0)  " +
                                                                  " + (ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid = i2.insplanid ),0)))  " +
                                                                  " - (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = p.patid ),0))) ELSE 0  END as remaining_benefit, " +
                                                                  " case when(isnull((LTRIM(RTRIM(ic.insconame))),'') != '' OR ISNULL((LTRIM(RTRIM(ic2.insconame))),'') != '')  " +
                                                                  " THEN(ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = p.patid),0))  ELSE 0  END as used_benefit " +
             "  ,i.idnum as Primary_Ins_Subscriber_ID, i2.idnum as Secondary_Ins_Subscriber_ID " +
            "from admin.patient p " +
                                                                  "Left join admin.insured i on p.priminsuredid = i.insuredid " +
                                                                  "Left Join admin.inscarrier ic on i.insplanid = ic.insid " +
                                                                  "Left join admin.insured i2 on p.secinsuredid = i2.insuredid " +
                                                                  "Left Join admin.inscarrier ic2 on i2.insplanid = ic2.insid ";

        public static string GetAppointmentPatientDataGurBalance = "SELECT guar_id AS responsiblepartyId, first_name AS ResponsibleParty_First_Name, last_name AS ResponsibleParty_Last_Name, " +
                                                                   "social_sec_num AS responsiblepartyssn, birth_date AS responsiblepartybirthdate, (ISNULL(balance_0_30_days,0)) + (ISNULL(balance_31_60_days,0)) + (ISNULL(balance_61_90_days,0)) +  (ISNULL(balance_91_plus_days,0)) as CurrentBal, " +
                                                                   "(ISNULL(balance_0_30_days,0)) AS ThirtyDay, (ISNULL(balance_31_60_days, 0)) AS SixtyDay, (ISNULL(balance_61_90_days, 0)) AS NinetyDay,(ISNULL(balance_91_plus_days, 0)) AS Over90 " +
                                                                   "From admin.v_guarantor_balance";

        //public static string GetAppointmentPatientDataNextVisitDate = "select ap.patid as patient_id ,Min (convert(varchar(10), ap.apptdate) +' '+convert(varchar(10), ap.timehr)+':'+convert(varchar(10),ap.timemin)+':00') AS Nextvisit_date " + 
        //    " from admin.appt as ap inner join (Select patid,min(apptdate) as apptdate from admin.appt  where apptdate >= '@ToDate' and status != -106 GROUP BY patid ) as v_S on v_S.patid = ap.patid and v_S.apptdate = ap.apptdate " + 
        //    " where ap.apptdate >= '@ToDate' group by ap.patid";

        public static string GetAppointmentPatientDataNextVisitDate = "SELECT ap.patid AS patient_id, ap.apptdate, ap.timehr, ap.timemin FROM admin.appt AS ap WHERE  ap.apptdate >= '@ToDate'";

        public static string GetAppointmentPatientDataCollectPayment = "Select patid,IsNull(Sum(amt),0) As Collect_Payment from admin.payment_view group by patid"; 

        public static string GetAppointmentPatientDataPatientNotes = "Select noteid as Patid, notetext as patient_Note from admin.v_notes where notetype = 3";

        #endregion

        #region G5
        public static string GetDentrixPatientDataG5 = "SELECT 0 as Clinic_Number, 1 as Service_Install_ID, v_p.patient_id AS Patient_EHR_ID, (LTRIM(RTRIM(v_p.first_name))) AS First_name, (LTRIM(RTRIM(v_p.last_name))) AS Last_name, "
                                                    + " (LTRIM(RTRIM(v_p.mi))) AS Middle_Name, (LTRIM(RTRIM(v_p.Salutation))) AS Salutation,(LTRIM(RTRIM(v_p.preferred_name))) AS preferred_name, "
                                                    + " case when Cast(v_p.status As Char) = '1' then 'A' else 'I' End AS Status,case when v_p.status  = 1 then 'Active'  when v_p.status = 2 then 'NonPatient' when v_p.status = 3 then 'InActive' when v_p.status = 4 then 'InActive' END AS EHR_Status, v_p.birth_date AS Birth_Date, (LTRIM(RTRIM(v_p.email))) AS Email, (LTRIM(RTRIM(v_p.mobile_phone))) AS Mobile, "
                                                    + " (LTRIM(RTRIM(v_p.home_phone))) AS Home_Phone, (LTRIM(RTRIM(v_p.work_phone))) AS Work_Phone, (LTRIM(RTRIM(v_p.address_line1))) AS Address1, "
                                                    + " (LTRIM(RTRIM(v_p.address_line2))) AS Address2, (LTRIM(RTRIM(v_p.city))) AS City, (LTRIM(RTRIM(v_p.state))) AS State,case when privacyflags in (1,3,5,7) then 'N' else 'Y' end as ReceiveVoiceCall, "
                                                    + " (LTRIM(RTRIM(v_p.zipcode))) AS Zipcode,v_p.guar_id AS Guar_ID, "
                                                    + " case when Convert(varchar(10),v_p.gender) != '1' and  Convert(varchar(10),v_p.gender) != '2' then 'Unknown' "
                                                    + " when Convert(varchar(10),v_p.gender) = '2' then 'Female' else 'Male' end as sex, "
                                                    + " case when Convert(varchar(10),p.fampos) = '2' then 'Married'  "
                                                    + " when Convert(varchar(10),p.fampos) = '1' then 'Single'  "
                                                    + " when Convert(varchar(10),p.fampos) = '3' then 'Child'  "
                                                    + " when Convert(varchar(10),p.fampos) = '4' then 'Other' else 'Single' end as MaritalStatus,  "
                                                    + " v_p.pri_provider_id AS Pri_Provider_ID, v_p.sec_provider_id AS Sec_Provider_ID, "
                                                    + " v_p.first_visit_date AS FirstVisit_Date, v_p.last_visit_date AS LastVisit_Date, "
                                                    + " v_pi.primary_insurance_carrier_id AS Primary_Insurance, "
                                                    + " (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))) AS Primary_Insurance_CompanyName, "
                                                    + " v_pi.secondary_insurance_carrier_id AS Secondary_Insurance, "
                                                    + " (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))) AS Secondary_Insurance_CompanyName, "
                                                    + " (ISNULL(v_gb.balance_0_30_days,0)) + (ISNULL(v_gb.balance_31_60_days,0)) + (ISNULL(v_gb.balance_61_90_days,0)) +  (ISNULL(v_gb.balance_91_plus_days,0)) as CurrentBal,  "
                                                    + " (ISNULL(v_gb.balance_0_30_days,0)) AS ThirtyDay, (ISNULL(v_gb.balance_31_60_days,0)) AS SixtyDay, "
                                                    + " (ISNULL(v_gb.balance_61_90_days,0)) AS NinetyDay,(ISNULL(v_gb.balance_91_plus_days,0)) AS Over90,  nappt.Nextvisit_date  AS nextvisit_date,'' AS due_date , "
                                                    + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN ((ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid = v_pi.primary_insurance_carrier_id ),0) "
                                                    + " + (ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid =v_pi.secondary_insurance_carrier_id ),0))) "
                                                    + "- (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))) ELSE 0  END as remaining_benefit , "
                                                    + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))  ELSE 0  END as used_benefit, "
                                                    + "collect_payment AS collect_payment, v_p.privacy_flags, 1 AS InsUptDlt, "
                                                   + "case when (v_p.privacy_flags = 2 or v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveSMS,case when (v_p.privacy_flags = 2 or  v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveEmail,( case when nt.patient_note like '%Spanish%' then 'Spanish' when nt.patient_note like '%French%' then 'French' else 'English' end ) AS PreferredLanguage, NT.patient_Note AS Patient_Note,  "
                                                   + " v_p.social_sec_num AS ssn, '' AS driverlicense, " //pat.empid,pat.claiminfid,v_p.patient_id,groupnum, patguar.guar_id" //v_pi.primary_insured_id,groupnum, patguar.guar_id
                                                   + " '' AS employer,claim.school AS school,pinsr.phone AS Prim_Ins_Company_Phonenumber,sinsr.phone AS Sec_Ins_Company_Phonenumber," //,patprins.groupnum AS groupid,                                                    
                                                   + " resp_party.guar_id AS responsiblepartyId, resp_party.first_name AS ResponsibleParty_First_Name, resp_party.last_name AS ResponsibleParty_Last_Name, "
                                                   + " resp_party.social_sec_num AS responsiblepartyssn, resp_party.birth_date AS responsiblepartybirthdate,"
                                                   + " '' AS groupid, '' AS emergencycontactId, '' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name "
                                                    + "  ,inprim.idnum as Primary_Ins_Subscriber_ID, insec.idnum as Secondary_Ins_Subscriber_ID "
                                                + "FROM admin.v_patient v_p JOIN admin.v_patient_insurance v_pi ON v_p.patient_id = v_pi.patient_id "
                                                    + "Left join admin.patient as p on p.patid = v_p.patient_id "
                                                    + " left join admin.insured inprim on p.priminsuredid= inprim.insuredid "
                                                    + " left join admin.insured insec on p.secinsuredid= insec.insuredid "  
                                                    + "Left Join admin.v_guarantor_balance v_gb ON v_p.guar_id = v_gb.guar_id "
                                                    + "LEFT JOIN ( Select pt.patid,fn.notetext as Patient_Note  from admin.patient pt INNER join admin.fullnotes FN ON FN.noteid = pt.patId where fn.notetype = 3 ) AS NT ON  v_p.patient_id = NT.PatId "
                                                   + "left join (select ap.patid as patient_id ,Min (convert(varchar(10), ap.apptdate) +' '+convert(varchar(10), ap.timehr)+':'+convert(varchar(10),ap.timemin)+':00') AS Nextvisit_date  from admin.appt as ap inner join (Select patid,min(apptdate) as apptdate from admin.appt  where apptdate >= ?  and status != -106 GROUP BY patid ) as v_S on v_S.patid = ap.patid and v_S.apptdate = ap.apptdate group by ap.patid) as nappt on nappt.patient_id = v_p.patient_id "
                                                   + "Left join (Select patid AS Patient_EHR_ID,isnull(Sum(amt),0) AS collect_payment from  admin.payment_view group by patid) as cp on cp.Patient_EHR_ID = v_p.patient_id "
                                                   //+ " LEFT JOIN admin.employer empl ON empl.empid = v_p.employer_id "
                                                   + " LEFT JOIN admin.claiminfo claim ON claim.claiminfid = p.claiminfid"
                                                   + " LEFT JOIN admin.inscarrier pinsr ON pinsr.insid = v_pi.primary_insured_id"
                                                   + " LEFT JOIN admin.inscarrier sinsr ON sinsr.insid = v_pi.secondary_insured_id"
                                                   + " LEFT JOIN admin.v_guarantor_balance resp_party ON resp_party.guar_id = v_p.guar_id";

        public static string GetDentrixNewAllPatientDataG5 = "SELECT 0 as Clinic_Number, 1 as Service_Install_ID, v_p.patient_id AS Patient_EHR_ID, (LTRIM(RTRIM(v_p.first_name))) AS First_name, (LTRIM(RTRIM(v_p.last_name))) AS Last_name, "
                                            + " (LTRIM(RTRIM(v_p.mi))) AS Middle_Name, (LTRIM(RTRIM(v_p.Salutation))) AS Salutation,(LTRIM(RTRIM(v_p.preferred_name))) AS preferred_name, "
                                            + " case when Cast(v_p.status As Char) = '1' then 'A' else 'I' End AS Status,case when v_p.status  = 1 then 'Active'  when v_p.status = 2 then 'NonPatient' when v_p.status = 3 then 'InActive' when v_p.status = 4 then 'InActive' END AS EHR_Status, v_p.birth_date AS Birth_Date, (LTRIM(RTRIM(v_p.email))) AS Email, (LTRIM(RTRIM(v_p.mobile_phone))) AS Mobile, "
                                            + " (LTRIM(RTRIM(v_p.home_phone))) AS Home_Phone, (LTRIM(RTRIM(v_p.work_phone))) AS Work_Phone, (LTRIM(RTRIM(v_p.address_line1))) AS Address1, "
                                            + " (LTRIM(RTRIM(v_p.address_line2))) AS Address2, (LTRIM(RTRIM(v_p.city))) AS City, (LTRIM(RTRIM(v_p.state))) AS State,case when privacyflags in (1,3,5,7) then 'N' else 'Y' end as ReceiveVoiceCall, "
                                            + " (LTRIM(RTRIM(v_p.zipcode))) AS Zipcode,v_p.guar_id AS Guar_ID, "
                                            + " case when Convert(varchar(10),v_p.gender) != '1' and  Convert(varchar(10),v_p.gender) != '2' then 'Unknown' "
                                            + " when Convert(varchar(10),v_p.gender) = '2' then 'Female' else 'Male' end as sex, "
                                            + " case when Convert(varchar(10),p.fampos) = '2' then 'Married'  "
                                            + " when Convert(varchar(10),p.fampos) = '1' then 'Single'  "
                                            + " when Convert(varchar(10),p.fampos) = '3' then 'Child'  "
                                            + " when Convert(varchar(10),p.fampos) = '4' then 'Other' else 'Single' end as MaritalStatus,  "
                                            + " v_p.pri_provider_id AS Pri_Provider_ID, v_p.sec_provider_id AS Sec_Provider_ID, "
                                            + " v_p.first_visit_date AS FirstVisit_Date, v_p.last_visit_date AS LastVisit_Date, "
                                            + " v_pi.primary_insurance_carrier_id AS Primary_Insurance, "
                                            + " (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))) AS Primary_Insurance_CompanyName, "
                                            + " v_pi.secondary_insurance_carrier_id AS Secondary_Insurance, "
                                            + " (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))) AS Secondary_Insurance_CompanyName, "
                                            + " (ISNULL(v_gb.balance_0_30_days,0)) + (ISNULL(v_gb.balance_31_60_days,0)) + (ISNULL(v_gb.balance_61_90_days,0)) +  (ISNULL(v_gb.balance_91_plus_days,0)) as CurrentBal,  "
                                            + " (ISNULL(v_gb.balance_0_30_days,0)) AS ThirtyDay, (ISNULL(v_gb.balance_31_60_days,0)) AS SixtyDay, "
                                            + " (ISNULL(v_gb.balance_61_90_days,0)) AS NinetyDay,(ISNULL(v_gb.balance_91_plus_days,0)) AS Over90,  nappt.Nextvisit_date  AS nextvisit_date,'' AS due_date , "
                                            + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN ((ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid = v_pi.primary_insurance_carrier_id ),0) "
                                            + " + (ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid =v_pi.secondary_insurance_carrier_id ),0))) "
                                            + "- (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))) ELSE 0  END as remaining_benefit , "
                                            + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))  ELSE 0  END as used_benefit, "
                                            + "collect_payment AS collect_payment, v_p.privacy_flags, 1 AS InsUptDlt, "
                                           + "case when (v_p.privacy_flags = 2 or v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveSMS,case when (v_p.privacy_flags = 2 or  v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveEmail,( case when nt.patient_note like '%Spanish%' then 'Spanish' when nt.patient_note like '%French%' then 'French' else 'English' end ) AS PreferredLanguage, NT.patient_Note AS Patient_Note,  "
                                           + " v_p.social_sec_num AS ssn, '' AS driverlicense, " //pat.empid,pat.claiminfid,v_p.patient_id,groupnum, patguar.guar_id" //v_pi.primary_insured_id,groupnum, patguar.guar_id
                                           + " '' AS employer,claim.school AS school,pinsr.phone AS Prim_Ins_Company_Phonenumber,sinsr.phone AS Sec_Ins_Company_Phonenumber," //,patprins.groupnum AS groupid,                                                    
                                           + " resp_party.guar_id AS responsiblepartyId, resp_party.first_name AS ResponsibleParty_First_Name, resp_party.last_name AS ResponsibleParty_Last_Name, "
                                           + " resp_party.social_sec_num AS responsiblepartyssn, resp_party.birth_date AS responsiblepartybirthdate,"
                                           + " '' AS groupid, '' AS emergencycontactId, '' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name "
                                            + "  ,inprim.idnum as Primary_Ins_Subscriber_ID, insec.idnum as Secondary_Ins_Subscriber_ID "
                                            + "FROM admin.v_patient v_p JOIN admin.v_patient_insurance v_pi ON v_p.patient_id = v_pi.patient_id "
                                            + "Left join admin.patient as p on p.patid = v_p.patient_id "
                                            + " left join admin.insured inprim on p.priminsuredid= inprim.insuredid "
                                            + " left join admin.insured insec on p.secinsuredid= insec.insuredid "
                                            + "Left Join admin.v_guarantor_balance v_gb ON v_p.guar_id = v_gb.guar_id "
                                            + "LEFT JOIN ( Select pt.patid,fn.notetext as Patient_Note  from admin.patient pt INNER join admin.fullnotes FN ON FN.noteid = pt.patId where fn.notetype = 3 ) AS NT ON  v_p.patient_id = NT.PatId "
                                           + "left join (select ap.patid as patient_id ,Min (convert(varchar(10), ap.apptdate) +' '+convert(varchar(10), ap.timehr)+':'+convert(varchar(10),ap.timemin)+':00') AS Nextvisit_date  from admin.appt as ap inner join (Select patid,min(apptdate) as apptdate from admin.appt  where apptdate >= ?  and status != -106 GROUP BY patid ) as v_S on v_S.patid = ap.patid and v_S.apptdate = ap.apptdate group by ap.patid) as nappt on nappt.patient_id = v_p.patient_id "
                                           + "Left join (Select patid AS Patient_EHR_ID,isnull(Sum(amt),0) AS collect_payment from  admin.payment_view group by patid) as cp on cp.Patient_EHR_ID = v_p.patient_id "
                                           //+ " LEFT JOIN admin.employer empl ON empl.empid = v_p.employer_id "
                                           + " LEFT JOIN admin.claiminfo claim ON claim.claiminfid = p.claiminfid"
                                           + " LEFT JOIN admin.inscarrier pinsr ON pinsr.insid = v_pi.primary_insured_id"
                                           + " LEFT JOIN admin.inscarrier sinsr ON sinsr.insid = v_pi.secondary_insured_id"
                                           + " LEFT JOIN admin.v_guarantor_balance resp_party ON resp_party.guar_id = v_p.guar_id Where v_p.patient_id in (@PaientEHRIDs)";


        public static string GetDentrixAppointmentsPatientDataG5 = "SELECT 0 as Clinic_Number, 1 as Service_Install_ID, v_p.patient_id AS Patient_EHR_ID, (LTRIM(RTRIM(v_p.first_name))) AS First_name, (LTRIM(RTRIM(v_p.last_name))) AS Last_name, "
                                                    + " (LTRIM(RTRIM(v_p.mi))) AS Middle_Name, (LTRIM(RTRIM(v_p.Salutation))) AS Salutation,(LTRIM(RTRIM(v_p.preferred_name))) AS preferred_name, "
                                                    + " case when Cast(v_p.status As Char) = '1' then 'A' else 'I' End AS Status,case when v_p.status  = 1 then 'Active'  when v_p.status = 2 then 'NonPatient' when v_p.status = 3 then 'InActive' when v_p.status = 4 then 'InActive' END AS EHR_Status, v_p.birth_date AS Birth_Date, (LTRIM(RTRIM(v_p.email))) AS Email, (LTRIM(RTRIM(v_p.mobile_phone))) AS Mobile, "
                                                    + " (LTRIM(RTRIM(v_p.home_phone))) AS Home_Phone, (LTRIM(RTRIM(v_p.work_phone))) AS Work_Phone, (LTRIM(RTRIM(v_p.address_line1))) AS Address1, "
                                                    + " (LTRIM(RTRIM(v_p.address_line2))) AS Address2, (LTRIM(RTRIM(v_p.city))) AS City, (LTRIM(RTRIM(v_p.state))) AS State,case when privacyflags in (1,3,5,7) then 'N' else 'Y' end as ReceiveVoiceCall, "
                                                    + " (LTRIM(RTRIM(v_p.zipcode))) AS Zipcode,v_p.guar_id AS Guar_ID, "
                                                    + " case when Convert(varchar(10),v_p.gender) != '1' and  Convert(varchar(10),v_p.gender) != '2' then 'Unknown' "
                                                    + " when Convert(varchar(10),v_p.gender) = '2' then 'Female' else 'Male' end as sex, "
                                                    + " case when Convert(varchar(10),p.fampos) = '2' then 'Married' "
                                                    + " when Convert(varchar(10),p.fampos) = '1' then 'Single' "
                                                    + " when Convert(varchar(10),p.fampos) = '3' then 'Child' "
                                                    + " when Convert(varchar(10),p.fampos) = '4' then 'Other' else 'Single' end as MaritalStatus,  "
                                                    + " v_p.pri_provider_id AS Pri_Provider_ID, v_p.sec_provider_id AS Sec_Provider_ID, "
                                                    + " v_p.first_visit_date AS FirstVisit_Date, v_p.last_visit_date AS LastVisit_Date, "
                                                    + " v_pi.primary_insurance_carrier_id AS Primary_Insurance, "
                                                    + " (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))) AS Primary_Insurance_CompanyName, "
                                                    + " v_pi.secondary_insurance_carrier_id AS Secondary_Insurance, "
                                                    + " (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))) AS Secondary_Insurance_CompanyName, "
                                                    + " (ISNULL(v_gb.balance_0_30_days,0)) + (ISNULL(v_gb.balance_31_60_days,0)) + (ISNULL(v_gb.balance_61_90_days,0)) +  (ISNULL(v_gb.balance_91_plus_days,0)) as CurrentBal,  "
                                                    + " (ISNULL(v_gb.balance_0_30_days,0)) AS ThirtyDay, (ISNULL(v_gb.balance_31_60_days,0)) AS SixtyDay, "
                                                    + " (ISNULL(v_gb.balance_61_90_days,0)) AS NinetyDay,(ISNULL(v_gb.balance_91_plus_days,0)) AS Over90,  nappt.Nextvisit_date  AS nextvisit_date,'' AS due_date , "
                                                    + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN ((ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid = v_pi.primary_insurance_carrier_id ),0) "
                                                    + " + (ISNULL((SELECT SUM(ic.maxcovperson) FROM admin.inscarrier ic WHERE ic.insid =v_pi.secondary_insurance_carrier_id ),0))) "
                                                    + "- (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))) ELSE 0  END as remaining_benefit , "
                                                    + " case when (isnull( (LTRIM(RTRIM(v_pi.primary_insurance_carrier_name))),'') != '' OR ISNULL( (LTRIM(RTRIM(v_pi.secondary_insurance_carrier_name))),'') != '') THEN (ISNULL((SELECT SUM(clm.totalreceived) FROM admin.claim clm WHERE clm.patid = v_pi.patient_id ),0))  ELSE 0  END as used_benefit, "
                                                    + "collect_payment AS collect_payment, v_p.privacy_flags, 1 AS InsUptDlt, "
                                                     + "case when (v_p.privacy_flags = 2 or v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveSMS,case when (v_p.privacy_flags = 2 or  v_p.privacy_flags = 3 or v_p.privacy_flags = 6 or v_p.privacy_flags = 7) then 'N' else 'Y' end As ReceiveEmail,( case when nt.patient_note like '%Spanish%' then 'Spanish' when nt.patient_note like '%french%' then 'French' else 'English' end ) AS PreferredLanguage, NT.patient_Note AS Patient_Note,  "
                                                     + " v_p.social_sec_num AS ssn, '' AS driverlicense, " //pat.empid,pat.claiminfid,v_p.patient_id,groupnum, patguar.guar_id" //v_pi.primary_insured_id,groupnum, patguar.guar_id
                                                     + " '' AS employer,claim.school AS school,pinsr.phone AS Prim_Ins_Company_Phonenumber,sinsr.phone AS Sec_Ins_Company_Phonenumber," //,patprins.groupnum AS groupid,                                                    
                                                     + " resp_party.guar_id AS responsiblepartyId, resp_party.first_name AS ResponsibleParty_First_Name, resp_party.last_name AS ResponsibleParty_Last_Name, "
                                                     + " resp_party.social_sec_num AS responsiblepartyssn, resp_party.birth_date AS responsiblepartybirthdate,"
                                                     + " '' AS groupid, '' AS emergencycontactId, '' AS emergencycontactnumber,'' AS spouseId,'' AS Spouse_First_Name,'' AS Spouse_Last_Name,'' as EmergencyContact_First_Name,'' as EmergencyContact_Last_Name "
                                                     + "  ,inprim.idnum as Primary_Ins_Subscriber_ID, insec.idnum as Secondary_Ins_Subscriber_ID "
                                                    + " FROM admin.v_patient v_p JOIN admin.v_patient_insurance v_pi ON v_p.patient_id = v_pi.patient_id "
                                                    + Environment.NewLine + " Inner Join admin.v_appointment V_appt On v_p.patient_id = V_appt.patient_id"
                                                    + Environment.NewLine + " Left join admin.patient as p on p.patid = v_p.patient_id "
                                                    + " left join admin.insured inprim on p.priminsuredid= inprim.insuredid "
                                                    + " left join admin.insured insec on p.secinsuredid= insec.insuredid "
                                                    + " Left Join admin.v_guarantor_balance v_gb ON v_p.guar_id = v_gb.guar_id "
                                                     + "LEFT JOIN ( Select pt.patid,fn.notetext as Patient_Note  from admin.patient pt INNER join admin.fullnotes FN ON FN.noteid = pt.patId where fn.notetype = 3 ) AS NT ON  v_p.patient_id = NT.PatId "
                                                     + " left join (select ap.patid as patient_id ,Min (convert(varchar(10), ap.apptdate) +' '+convert(varchar(10), ap.timehr)+':'+convert(varchar(10),ap.timemin)+':00') AS Nextvisit_date  from admin.appt as ap inner join (Select patid,min(apptdate) as apptdate from admin.appt  where apptdate >= ?  and status != -106 GROUP BY patid ) as v_S on v_S.patid = ap.patid and v_S.apptdate = ap.apptdate group by ap.patid) as nappt on nappt.patient_id = v_p.patient_id "
                                                    + " Left join (Select patid AS Patient_EHR_ID,isnull(Sum(amt),0) AS collect_payment from  admin.payment_view group by patid) as cp on cp.Patient_EHR_ID = v_p.patient_id "
                                                     //+ " LEFT JOIN admin.employer empl ON empl.empid = v_p.employer_id "
                                                     + " LEFT JOIN admin.claiminfo claim ON claim.claiminfid = p.claiminfid"
                                                     + " LEFT JOIN admin.inscarrier pinsr ON pinsr.insid = v_pi.primary_insured_id"
                                                     + " LEFT JOIN admin.inscarrier sinsr ON sinsr.insid = v_pi.secondary_insured_id"
                                                     + " LEFT JOIN admin.v_guarantor_balance resp_party ON resp_party.guar_id = v_p.guar_id"
                                                     + " Where (V_appt.patient_name <> '' AND V_appt.appointment_date >= ?)";
        #endregion


        //public static string GetDentrixNewPatientData = "SELECT 0 as Clinic_Number, 1 as Service_Install_ID, v_p.patient_id AS Patient_EHR_ID from admin.v_patient v_p";
        public static string GetDentrixNewPatientData = "SELECT 0 as Clinic_Number, 1 as Service_Install_ID, v_p.patid AS Patient_EHR_ID from admin.patient v_p ";

        public static string GetDentrixPatientNextApptDate = "select a_appt.apptdate, V_appt.appointment_date AS nextvisit_date,patid ,V_appt.start_hour,V_appt.Start_minute,V_appt.length "
                                                + " From admin.appt a_appt JOIN admin.v_appointment V_appt ON a_appt.apptid = V_appt.appointment_id Where a_appt.apptdate > ? Order by a_appt.apptdate desc ";

        // public static string GetDentrixPatientStatusNew_Existing = "SELECT patid as Patient_EHR_Id from admin.patient where patid not in (select p.patid as Patient_EHR_Id from admin.patient p INNER join admin.appt a on a.patid = p.patid where  (convert(varchar(10),A.apptdate) +' '+ convert(varchar(10),A.timehr) +':'+ convert(varchar(10),A.timemin)+':00')  < getdate() and a.status = -106 )";

        public static string GetDentrixPatientStatusNew_Existing = "SELECT patid as Patient_EHR_Id from admin.patient where patid not in (select distinct a.patid as Patient_EHR_Id from admin.appt a where a.patid != 0 and  ((a.apptdate < ? and a.timehr < 24 and a.timemin < 60) or (a.apptdate = ? and a.timehr < ? and a.timemin < ?)) and a.status = -106 )";

        // Below code is specific to Silvertone https://app.asana.com/0/1206996934454037/1206490186425567
        //public static string GetDentrixPatientStatusNew_Existing = "SELECT patid as Patient_EHR_Id from admin.patient where patid not in (select distinct a.patid as Patient_EHR_Id from admin.appt a where a.patid != 0 and  ((a.apptdate< ? and a.timehr< 24 and a.timemin< 60) or (a.apptdate = ? and a.timehr < ? and a.timemin < ?)) and(a.status= -106 or a.status= 9))";

        public static string GetDentrixPatientdue_date = "Select distinct pt.patient_id,rt.recallid AS recall_type_id, (LTRIM(RTRIM(pt.recall_type))) AS recall_type, pt.due_date "
                                                    + "  from ( select distinct patient_id,recall_type,due_date from admin.v_patient_recall )  pt  inner JOIN Admin.RecallType rt ON pt.recall_type = rt.name order by pt.due_date desc "; // where due_date > @todate

        public static string GetDentrixPatientdue_dateByPatID = "Select distinct pt.patient_id,rt.recallid AS recall_type_id, (LTRIM(RTRIM(pt.recall_type))) AS recall_type, pt.due_date "
                                                   + "  from ( select distinct patient_id,recall_type,due_date from admin.v_patient_recall )  pt  inner JOIN Admin.RecallType rt ON pt.recall_type = rt.name where pt.patient_id = '@Patient_EHR_ID' order by pt.due_date desc ";

        public static string GetDentrixPatient_recall = "call admin.sp_getallpatientrecalls()";
        public static string GetDentrixPatient_recallCustom = "Select Distinct rp.patid as Patient_id, p.patguid as patient_guid, p.lastname as last_name, p.firstname as first_name, rt.name as recall_type, rt.descript as recall_description," +
            " rp.duedate as due_date, rp.recallid as recall_type_id, rp.priordate as prior_date, rp.provid as prov_id, '' as provider_last_name, '' as provider_first_name " +
            " From admin.recallpending rp inner join admin.patient p on rp.patid = p.patid inner join admin.recalltype rt on rp.recallid = rt.recallid";


        public static string GetDentrixPatientcollect_payment = "Select patid AS Patient_EHR_ID,Sum(amt) AS collect_payment from  admin.payment_view group by patid ";

        public static string GetDentrixPatient_RecallType = "Select rt.recallid, (LTRIM(RTRIM(rt.name))) AS recall_type"
                                                + " from Admin.RecallType rt ";

        public static string GetDentrixPatientDiseaseData = "SELECT 0 as PatientDisease_LocalDB_ID, patid as Patient_EHR_ID,hhitemid as Disease_EHR_ID,'' as Disease_Web_ID,h.description as Disease_Name,case when h.hhtype = 1 then 'P' else 'A' end as Disease_Type,hl.automodifiedtimestamp as EHR_Entry_DateTime,case when Convert(bit,hl.status) = 0 then 1 else 0 end as is_deleted,0 as Is_Adit_Updated,0 as Clinic_Number from admin.hhlinkpat_item as hl left join admin.hhitem as h on h.itemid = hl.hhitemid where h.hhtype <> 3 ";

        //"select patid as Patient_EHR_ID,rxid as Medication_EHR_ID,rxid as PatientMedication_EHR_ID,drugname as Medication_Name,'Drug' as Medication_Type,dispense as Drug_Quantity, descript as Medication_Note,provid as Provider_EHR_ID ,rxdate as Start_Date,rxdate as End_Date ,createdate AS EHR_Entry_DateTime ,0 AS is_deleted,0 Clinic_Number from admin.rxrec  where patid <> 0";

        public static string GetDentrixPatientMedicationDataG62andG7 = "select PM.patid as Patient_EHR_ID, " +
                                                                       "IsNull(M.rxid,0) as Medication_EHR_ID, " +
                                                                       "PM.rxid as PatientMedication_EHR_ID, " +
                                                                       "PM.drugname as Medication_Name, " +
                                                                       "'Drug' as Medication_Type, " +
                                                                       "PM.dispense as Drug_Quantity,  " +
                                                                       "PM.descript as Medication_Note, " +
                                                                       "PM.provid as Provider_EHR_ID, " +
                                                                       "PM.rxdate as Start_Date, " +
                                                                       "PM.rxdate as End_Date, " +
                                                                       "PM.createdate AS EHR_Entry_DateTime, " +
                                                                       "PM.notetext as Patient_Notes,  " +
                                                                       "0 AS is_deleted, " +
                                                                       "Convert(bit,1) as is_active1, " +
                                                                       "0 Clinic_Number from admin.rxrec PM Left Join admin.rxrec M on PM.drugname = M.drugname and M.PatID = 0 " +
                                                                       "where PM.patid<> 0";

        public static string GetDentrixPatientMedicationDataG5andG6 = "select D.patid as Patient_EHR_ID, " +
                                                                      "D.rxdefid as Medication_EHR_ID,  " +
                                                                      "D.rxid as PatientMedication_EHR_ID,  " +
                                                                      "H.drugname as Medication_Name,  " +
                                                                      "'Drug' as Medication_Type,  " +
                                                                      "H.dispense as Drug_Quantity,  " +
                                                                      "sg.NoteText as Medication_Note,  " +
                                                                      "D.rscid as Provider_EHR_ID ,  " +
                                                                      "'' as Start_Date,  " +
                                                                      "'' as End_Date,  " +
                                                                      "D.rxdate AS EHR_Entry_DateTime ,  " +
                                                                      "nt.notetext as Patient_Notes,  " +
                                                                      "Convert(bit,1) as is_active1,  " +
                                                                      "0 AS is_deleted, " +
                                                                      "0 Clinic_Number " +
                                                                      "from admin.RxPatient D " +
                                                                      "Inner Join admin.RxDef H  on D.RxDefID = H.RxDefID " +
                                                                      "Inner Join admin.fullnotes nt on H.noteid = nt.noteid and nt.notetype = 34 " +
                                                                      "Inner Join admin.fullnotes sg on H.sigid = sg.noteid and sg.notetype = 35";

        //public static string GetDentrixPatientMedicationDataG7New = "select D.patid as Patient_EHR_ID, " +
        //                                                            "D.hhitemid as Medication_EHR_ID, " +
        //                                                            "D.linkid as PatientMedication_EHR_ID, " +
        //                                                            "H.description as Medication_Name, " +
        //                                                            "'Drug' as Medication_Type, " +
        //                                                            "'' as Drug_Quantity, " +
        //                                                            "'' as Medication_Note, " +
        //                                                            "VP.pri_provider_id as Provider_EHR_ID , " +
        //                                                            "D.startdate as Start_Date, " +
        //                                                            "D.enddate as End_Date , " +
        //                                                            "D.reporteddate AS EHR_Entry_DateTime , " +
        //                                                            "D.Note as Patient_Notes, " +
        //                                                            "Convert(bit,D.status) as is_active1, " +
        //                                                            "0 AS is_deleted," +
        //                                                            "0 Clinic_Number from admin.hhlinkpat_item D inner join admin.hhitem H on D.hhitemid = H.itemid Inner Join admin.v_patient VP on D.patid = VP.patient_id where H.hhtype = 3";

        public static string GetDentrixPatientMedicationDataG7New = "select D.patid as Patient_EHR_ID,  "
                                                                  + " D.hhitemid as Medication_EHR_ID, "
                                                                  + " D.linkid as PatientMedication_EHR_ID,  "
                                                                  + " H.description as Medication_Name,  "
                                                                  + " 'Drug' as Medication_Type, "
                                                                  + " '' as Drug_Quantity,  "
                                                                  + " '' as Medication_Note,  "
                                                                  + " VP.provid1 as Provider_EHR_ID ,  "
                                                                  + " D.startdate as Start_Date, "
                                                                  + " D.enddate as End_Date ,  "
                                                                  + " D.reporteddate AS EHR_Entry_DateTime ,  "
                                                                  + " D.Note as Patient_Notes,  "
                                                                  + " Convert(bit, D.status) as is_active1,  "
                                                                  + " 0 AS is_deleted, "
                                                                  + " 0 Clinic_Number from admin.hhlinkpat_item D inner join admin.hhitem H on D.hhitemid = H.itemid Inner Join admin.patient VP on D.patid = VP.patid where H.hhtype = 3 ";
        #endregion

        #region  Disease

        public static string GetDentrixDiseaseData = "select itemid as Disease_EHR_ID,description as Disease_Name,case when hhtype = 1 then 'P' else 'A' end as Disease_Type,automodifiedtimestamp as EHR_Entry_DateTime,case when Convert(bit,status) = 0 then 'true' else 'false' end as is_deleted from admin.hhitem where hhtype <> 3 ";

        //public static string GetDentrixMedicationData = "select rxid as Medication_EHR_ID,drugname as Medication_Name,'Drug' as Medication_Type,dispense as Drug_Quantity,createdate AS EHR_Entry_DateTime ,0 AS is_deleted,0 Clinic_Number from admin.rxrec  where patid = 0";
        public static string GetDentrixMedicationDataG7New = "SELECT itemid as Medication_EHR_ID, " +
                                                              "description as Medication_Name, " +
                                                              "description as Medication_Description, " +
                                                              "'' as Medication_Notes, " +
                                                              "'' as Medication_Sig, " +
                                                              "'' as Medication_Parent_EHR_ID, " +
                                                              "'Drug' as Medication_Type , " +
                                                              "'' as Allow_Generic_Sub, " +
                                                              "'' as Drug_Quantity," +
                                                              "'' as Refills, " +
                                                              "status as Is_Active, " +
                                                              "automodifiedtimestamp as EHR_Entry_DateTime, " +
                                                              "'' as Medication_Provider_ID, " +
                                                              "0 as is_deleted, " +
                                                              "0 as Is_Adit_Updated, " +
                                                              "'0' as Clinic_Number,popup FROM admin.hhitem where hhtype = 3 and (description is not null and description <> '') ";

        public static string GetDentrixMedicationDataG6andG5 = "SELECT M.RxDefID as Medication_EHR_ID, " +
                                                               "M.drugname as Medication_Name,  " +
                                                               "M.descript as Medication_Description,  " +
                                                               "nt.NoteText as Medication_Notes,  " +
                                                               "sg.NoteText as Medication_Sig,  " +
                                                               "'' as Medication_Parent_EHR_ID,  " +
                                                               "'Drug' as Medication_Type ,  " +
                                                               "M.aswritten as Allow_Generic_Sub,  " +
                                                               "M.dispense as Drug_Quantity, " +
                                                               "M.refills as Refills,  " +
                                                               "Case When M.active = 1 Then 'True' Else 'False' End as Is_Active,  " +
                                                               "M.automodifiedtimestamp as EHR_Entry_DateTime,  " +
                                                               "'' as Medication_Provider_ID,  " +
                                                               "0 as is_deleted,  " +
                                                               "0 as Is_Adit_Updated, " +
                                                               "'0' as Clinic_Number " +
                                                               "FROM admin.RxDef M " +
                                                               "Inner Join admin.fullnotes nt on M.noteid = nt.noteid and nt.notetype = 34 " +
                                                               "Inner Join admin.fullnotes sg on M.sigid = sg.noteid and sg.notetype = 35";

        public static string GetDentrixMedicationDataG62andG7 = "SELECT rxid as Medication_EHR_ID, " +
                                                              "drugname as Medication_Name, " +
                                                              "descript as Medication_Description, " +
                                                              "notetext as Medication_Notes, " +
                                                              "sigtext as Medication_Sig, " +
                                                              "'' as Medication_Parent_EHR_ID, " +
                                                              "'Drug' as Medication_Type, " +
                                                              "dispenseaswritten as Allow_Generic_Sub,  " +
                                                              "dispense as Drug_Quantity, " +
                                                              "refills as Refills, " +
                                                              "'True' as Is_Active, " +
                                                              "createdate as EHR_Entry_DateTime, " +
                                                              "provid as Medication_Provider_ID,  " +
                                                              "0 as is_deleted, " +
                                                              "0 as Is_Adit_Updated, " +
                                                              "'0' as Clinic_Number From admin.rxrec where patid = 0";

        public static string GetDentrixDiseaseAlertData = "select itemid as Disease_EHR_ID,description as Disease_Name,case when hhtype = 1 then 'P' else 'A' end as Disease_Type,automodifiedtimestamp as EHR_Entry_DateTime,popup,critical,Convert(bit,critical) as critical_TF from admin.hhitem where hhtype <> 3";

        public static string GetDentrixDiseaseDataG6andG5 = "SELECT defid as Disease_EHR_ID,descript as Disease_Name, 'A'  as Disease_Type,automodifiedtimestamp as EHR_Entry_DateTime,0 as is_deleted from admin.fulldef where deftype = 2";

        public static string InsertMedicationG7New = "Insert into admin.hhitem(hhtype,status,popup,showinquestionaire,olddefid,description,critical) Values (3,1,1,0,0,@Medication_Name,null)";

        public static string InsertMedicationG7 = "Insert into admin.rxrec(istemplate, isstandard, createdate, rxdate, drugname, descript, dispense, dispenseaswritten, refills, patid, provid, sigtext, notetext,rxguid) " +
                                                  " values(@IsTemplate, @IsStandard, getdate(), getdate(), @DrugName, @Description, @Dispense, @AsWritten, @Refills, @PatID, @ProvID, @SIGTEXT, @NOTETEXT, @RXGUID)";

        public static string InsertMedicationG5andG6 = "Insert into admin.RxDef(Active,std,rxdate,drugname,descript,dispense,refills,aswritten) values(@Active,@std,@rxdate,@drugname,@descript,@dispense,@refills,@aswritten)";

        public static string InsertPatientMedicationG7New = "Insert into admin.hhlinkpat_item(patid,hhitemid,status,reporteddate,startdate,enddate,note,popup,critical) Values (@Patient_EHR_ID,@Medication_EHR_ID,1,getdate(),getdate(),null,@Medication_Note,@popup,null)";

        public static string InsertPatientMedicationG5andG6 = "Insert into admin.RxPatient(rxdefid,patid,rxdate,rscid) Values(@Medication_EHR_ID,@Patient_EHR_ID,getdate(),@Provider_EHR_ID)";

        public static string UpdatePatientMedicationNotesG7New = "Update admin.hhlinkpat_item set hhitemid = @Medication_EHR_ID, note = @Medication_Note where LinkID = @PatientMedication_EHR_ID ";

        public static string UpdatePatientMedicationNotesG7 = "Update admin.rxrec set drugname = @Medication_Name, descript = @Medication_Description, sigtext = @Medication_SIG, notetext = @Medication_Note where rxid = @PatientMedication_EHR_ID";

        public static string DeletePatientMedicationG7New = "Delete From admin.hhlinkpat_item where LinkID = @PatientMedication_EHR_ID ";
        #endregion

        #region RecallType
        public static string GetDentrixRecallTypeData = "SELECT recallid AS RecallType_EHR_ID , (LTRIM(RTRIM(name))) AS RecallType_Name, "
                                               + " (LTRIM(RTRIM(descript))) AS RecallType_Descript, automodifiedtimestamp AS EHR_Entry_DateTime "
                                               + " FROM admin.RecallType where patid= 0 ";
        #endregion

        #region ApptStatus

        public static string GetDentrixApptStatusData = " SELECT defid AS ApptStatus_EHR_ID , (LTRIM(RTRIM(descript))) AS ApptStatus_Name, 'normal' AS ApptStatus_Type "
                                             + " FROM admin.V_appt_Status_Codes";
        #endregion

        #region Users
        public static string GetDentrixApptUsersData = "select rscid as User_EHR_ID,'' AS User_LocalDB_ID,'' as User_web_Id,firstname as First_Name,"
                                                       + "lastname as Last_Name,password as Password ,automodifiedtimestamp as EHR_Entry_DateTime,automodifiedtimestamp as Last_Updated_DateTime,"
                                                       + "'' as LocalDb_EntryDatetime,(case when inactive = 0 then 1 else 0 end)  as Is_active,0 as is_deleted,0 as Is_Adit_Updated,0 as Clinic_Number,1 as Service_Install_Id from admin.rsc where rsctype=2";
        #endregion

        #region CreateAppointment

        //public static string GetDentrixPatientID_NameData = "SELECT v_p.patient_id AS Patient_EHR_ID,(LTRIM(RTRIM(v_p.first_name))) AS FirstName,(LTRIM(RTRIM(v_p.last_name))) AS LastName,  (LTRIM(RTRIM(v_p.first_name))) + ' ' + (LTRIM(RTRIM(v_p.last_name))) AS Patient_Name,"
        //                                       + "(LTRIM(RTRIM(v_p.mobile_phone))) AS Mobile,birth_date as birth_date, (LTRIM(RTRIM(v_p.home_phone))) AS Home_Phone,(LTRIM(RTRIM(v_p.work_phone))) AS Work_Phone,v_p.guar_id as Guarantor,(LTRIM(RTRIM(v_p.email))) AS Email FROM admin.v_patient v_p ";
        public static string GetDentrixPatientID_NameData = "SELECT v_p.patid AS Patient_EHR_ID, "
                                                          + " (LTRIM(RTRIM(v_p.firstname))) AS FirstName, "
                                                          + " (LTRIM(RTRIM(v_p.lastname))) AS LastName, "
                                                          + " (LTRIM(RTRIM(v_p.firstname))) + ' ' + (LTRIM(RTRIM(v_p.lastname))) AS Patient_Name, "
                                                          + " (LTRIM(RTRIM(v_p.pager))) AS Mobile, "
                                                          + " birthdate as birth_date, "
                                                          + " (LTRIM(RTRIM(a.phone))) AS Home_Phone, "
                                                          + " (LTRIM(RTRIM(v_p.workphone))) AS Work_Phone, "
                                                          + " v_p.guarid as Guarantor, "
                                                          + " (LTRIM(RTRIM(v_p.emailaddr))) AS Email FROM admin.patient v_p "
                                                          + " Left Join admin.address A on A.addrid = v_p.addrid ";


        public static string GetDentrixIdelProvider = "SELECT top 1 rscid as Provider_id FROM admin.rsc where rsctype = 1 and inactive=0";
        //"SELECT top 1 provider_id FROM admin.v_Provider where inactive = 0";

        public static string GetDentrixIdelProviderG5 = "SELECT top 1 provider_id FROM admin.v_Provider";

        public static string UpdatePatientGuarantorID = " Update admin.patient SET isguar=?,isheadofhouse = ? ,guarid = ? ,famid = ? WHERE patid = ? ";

        public static string InsertPatientDetails = " INSERT INTO admin.patient (lastname,firstname,mi,provid1,isguar,gender,firstvisitdate,emailaddr,pager,patguid,addrid,chartnum,status) "
                                          + " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?) ";

        public static string InsertPatientDetails_With_Birthdate = " INSERT INTO admin.patient (lastname,firstname,mi,provid1,isguar,gender,firstvisitdate,emailaddr,pager,patguid,addrid,chartnum,status,birthdate) "
                                         + " VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?) ";

        //public static string GetBookOperatoryAppointmenetWiseDateTime = " Select appointment_id, Operatory_id, appointment_date,start_hour,start_minute, length AS ApptMin,isnull(P.first_name,appt.patient_name) AS FirstName,p.last_name AS LastName,isnull(p.mobile_phone,appt.patient_phone) AS Mobile,p.email AS Email,PP.first_name AS ProviderFirstName,PP.last_name AS ProviderLastName FROM admin.v_appointment as appt left join admin.v_patient p on appt.patient_id = p.patient_id Left join admin.v_Provider PP on PP.provider_id = appt.provider_id "
        //                                                   + " WHERE appointment_date >= ? ";
        public static string GetBookOperatoryAppointmenetWiseDateTime = "Select appointment_id, "
                                                                      + " Operatory_id,  "
                                                                      + " appointment_date, "
                                                                      + " start_hour, "
                                                                      + " start_minute, "
                                                                      + " length AS ApptMin, "
                                                                      + " isnull(P.firstname, appt.patient_name) AS FirstName, "
                                                                      + " p.lastname AS LastName, "
                                                                      + " isnull(p.pager, appt.patient_phone) AS Mobile, "
                                                                      + " p.emailaddr AS Email, "
                                                                      + " PP.first_name AS ProviderFirstName, "
                                                                      + " PP.last_name AS ProviderLastName "
                                                                      + " FROM admin.v_appointment as appt "
                                                                      + " left join admin.patient p on appt.patient_id = p.patid "
                                                                      + " Left join admin.v_Provider PP on PP.provider_id = appt.provider_id "
                                                                      + " WHERE appointment_date >= ? ";



        public static string InsertAppointmentDetails = " INSERT INTO admin.Appt(patid, status, apptlen, opid, provid, apptdate, createdate, appttype,timehr,timemin,timeblock,rsctype2,rsctype,rsctype3,patname,staffid,apptreason,codeid1,codetype1,codeid2,codetype2,codeid3,codetype3,codeid4,codetype4,codeid5,codetype5,codeid6,codetype6,codeid7,codetype7,codeid8,codetype8,codeid9,codetype9,codeid10,codetype10,codeid11,codetype11,codeid12,codetype12,codeid13,codetype13,codeid14,codetype14,codeid15,codetype15,codeid16,codetype16,codeid17,codetype17,codeid18,codetype18,codeid19,codetype19,codeid20,codetype20,createdbyuserid) "
                                               + " VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?) ";

        public static string InsertAppointmentComment = "INSERT INTO Admin.fullnotes (notetype,_unused1,noteid,notetext)VALUES(?,?,?,?)";

        public static string Is_Update_Status_EHR_Appointment_Live_To_EHR = "SELECT status FROM admin.Appt Where apptid  = ? ";

        public static string Update_Status_EHR_Appointment_Live_To_Local = " UPDATE admin.appt SET status = ? WHERE apptid = ? ";

        public static string Update_Receive_SMS_Patient_EHR_Live_To_Dentrix = " UPDATE admin.patient SET privacyflags = ? WHERE patid = ? ";

        public static string InsertPatientAddress = "insert into admin.address (ptrcount,_unused1) values (1,0)";

        // public static string GetPrimaryProviderId = "SELECT provid1 From admin.patient where patid=? and status = 1 order by firstname";

        public static string GetPrimaryProviderId = "SELECT provid1 From admin.patient where patid=? order by firstname";

        public static string Update_Patinet_Record_By_Patient_Form = "UPDATE admin.patient SET ColumnName = @ehrfield_value WHERE patid = @Patient_EHR_ID ";



        public static string Update_Patinet_Address_Record_By_Patient_Form = "UPDATE admin.address SET ColumnName = @ehrfield_value WHERE addrid = @Patient_EHR_ID ";

        public static string Insert_paitent_insurance = "insert into admin.insured (insplanid,inspartyid,idnum,ptrcount) values (?,?,?,1)";

        public static string Insert_paitent_PaymentLog = "insert into admin.Contact (Contactdate,Contacttime,patid,Provid,title,Contacttype) values (?,?,?,?,?,?)";

        public static string Insert_paitent_PaymentLogNote = "insert into admin.FullNotes (notetype,noteid,notetext) values (?,?,?)";

        public static string GetpatGurIdAndProviders = " select guarid from admin.Patient  where patid=?";

        public static string Insert_paitent_primaryinsurance_patplan = "update admin.patient set priminsrel = 1 , priminsuredid = ? where patid = ?";

        public static string Insert_paitent_secondaryinsurance_patplan = "update admin.patient set secinsrel = 1 , secinsuredid = ? where patid = ?";
        #endregion

        public static string GetDentrixApplicationVersion = "select optionvalue AS EHR_Sub_Version  from admin.options where optionkey = 'Data_Version'";
        //public static string GetDenrtrixEHR_VersionNumber = "SELECT substring(optionvalue, 193, 9) as version from admin.options where optionsection = 'SysReqWkstnData'";
        public static string GetDenrtrixEHR_VersionNumber = "SELECT substring(optionvalue, Instr(optionvalue, '<InstalledVersion>')+18, 9)  as version " +
            " From admin.options " +
            " Where optionsection = 'SysReqWkstnData'  and Instr(optionvalue, '<InstalledVersion>')>0";



        public static string GetGuarIdFromPatId = "SELECT guarid FROM admin.patient WHERE patid = ?";

        public static string GetAbbrevDescFromProcedure = "SELECT abbrevdescript FROM admin.proccode WHERE proccodeid = ?";

        //public static string GetProcdureIDFromProcedureCode = "SELECT MAX(procid) FROM fullproclog WHERE patid = @patid AND proccodeid = @proccodeid";
        //public static string GetDentrixProviderHoursData = " SELECT sd.scheduleNum,IFNULL(sdo.ScheduleOpNum,0), "
        //                                      + " (CASE WHEN IFNULL(sdo.ScheduleOpNum,0) = 0 THEN sd.scheduleNum ELSE CONCAT(sd.scheduleNum,'_',sdo.ScheduleOpNum)END) AS PH_EHR_ID, "
        //                                      + " sd.provNum AS Provider_EHR_ID,sd.scheddate,CONCAT(sd.scheddate,' ',sd.StartTime) AS StartTime ,CONCAT(sd.scheddate,' ',sd.StopTime)AS EndTime, "
        //                                      + " sdo.OperatoryNum AS Operatory_EHR_ID,sd.note AS comment,sd.status, sd.DateTStamp AS Entry_DateTime "
        //                                      + " FROM schedule sd left JOIN scheduleop sdo ON sdo.ScheduleNum = sd.ScheduleNum "
        //                                      + " WHERE sd.SchedType = 1 AND sd.scheddate > @ToDate ; ";

        //public static string GetDentrixProviderHoursData = " SELECT sd.scheduleNum,IFNULL(sdo.ScheduleOpNum,0), "
        //                                    + " (CASE WHEN IFNULL(sdo.ScheduleOpNum,0) = 0 THEN sd.scheduleNum ELSE CONCAT(sd.scheduleNum,'_',sdo.ScheduleOpNum)END) AS PH_EHR_ID, "
        //                                    + " sd.provNum AS Provider_EHR_ID,sd.scheddate,CONCAT(sd.scheddate,' ',sd.StartTime) AS StartTime ,CONCAT(sd.scheddate,' ',sd.StopTime)AS EndTime, "
        //                                    + " sdo.OperatoryNum AS Operatory_EHR_ID,sd.note AS comment,sd.status, sd.DateTStamp AS Entry_DateTime "
        //                                    + " FROM schedule sd left JOIN scheduleop sdo ON sdo.ScheduleNum = sd.ScheduleNum "
        //                                    + " WHERE sd.SchedType = 1 AND sd.scheddate > @ToDate ; ";


        #region MedicleForm

        public static string GetDentrixMedicleFormData = "SELECT 0 As Clinic_Number,1 as Service_Install_Id,0 as Dentrix_Form_LocalDB_ID,convert(varchar(10),formid) as Dentrix_Form_EHR_ID,convert(varchar(10),formuniqueid) as Dentrix_Form_EHRUnique_ID,'' as Dentrix_Form_Web_ID, "
                                                      + " formname as Dentrix_Form_Name,convert(varchar(10),formversion) as Version,versiondate as Version_Date,IsActive as Is_Active,convert(varchar(10),FormRespondentType) as FormRespondentType, "
                                                      + "convert(varchar(10),CategoryId) as CategoryId,convert(varchar(10),FormFlags) as FormFlags,convert(varchar(10),MonthstoExpiration) as MonthtoExpiration,automodifiedtimestamp as EHR_Entry_DateTime,getdate() as Last_Sync_Date , "
                                                      + "getdate() as Entry_DateTime,0 as Is_Adit_Updated,0 as is_deleted   from admin.pq_form";

        public static string GetDentrixMedicleFormQuestionData = "select 0 As Clinic_Number,1 AS Service_Install_Id,0 as Dentrix_FormQuestion_LocalDB_ID, '' as Dentrix_FormQuestion_Web_ID , convert(varchar(10), formuniqueid) as Dentrix_Form_EHRUnique_ID,convert(varchar(5000), fq.questionUniqueid) as Dentrix_Question_EHRUnique_ID , "
                                                       + "convert(varchar(10), questiontype) as Dentrix_QuestionsTypeId, '' as Dentrix_QyestionTypeName,'0' as Dentrix_ResponsetypeId,'' as Dentrix_QuestionName,'' as Dentrix_Question_DefaultValue,convert(varchar(10), QuestionVersion) as QuestionVersion, "
                                                       + "VersionDate as QuestionVersion_Date,'' as InputType,Convert(Bit,0) as Is_OptionField,'' as Options,Convert(Bit,0) as Is_Required, convert(varchar(10),fq.questionorder) as QuestionOrder,fq.automodifiedtimestamp as EHR_Entry_DateTime,getdate() as Last_Sync_Date , "
                                                       + "getdate() as Entry_DateTime,Convert(Bit,0) as Is_Adit_Updated,Convert(Bit,0) as is_deleted,q.questioninfo,Convert(Bit,0) as Is_MultiField,convert(varchar(10),q.questionid) as Dentrix_Question_EHR_ID from admin.pq_formquest as fq inner join admin.pq_question as q on q.questionuniqueid = fq.questionuniqueid";

        public static string GetDentrixMediclePartialQuestionData = "select * from Dentrix_ParitalControlForm where Dentrix_QuestionsTypeId = @Dentrix_QuestionsTypeId";

        public static string GetDentrixMediclePartialResponseData = "SELECT r.* ,rs.respondentpatid,rs.formuniqueid,rs.responsesetuniqueid as responsesetuniqueidMain  from admin.pq_responseset as rs left outer join admin.pq_response as r on r.responsesetuniqueid = rs. responsesetuniqueid  where respondentpatid = ? and formuniqueid = ?";

        public static string InsertDentrixMedicleResponseSetData = "insert into admin.pq_responseset(respondenttype,respondentpatid,formuniqueid,responsesetflags,patsignatureid,provsignatureid,witnesssignatureid,entrydate,responsedate,modifieddate) "
                                                       + " values (1,?,?,5,-1,-1,-1,getdate(),getdate(),getdate())";

        public static string InsertDentrixMedicleResponseData = "Insert into admin.pq_response (responsesetuniqueid,questionuniqueid,responsetype,responseinfo) "
                                                       + " values(?,?,?,?)";

        public static string InsertDentrixDiseaseAlertData = "insert into admin.hhlinkpat_item(patid,hhitemid,status,reporteddate,startdate,enddate,note,popup,critical) "
                                                      + " values (?,?,1,getdate(),getdate(),'9999/12/31',?,?,?)";

        public static string DeleteDentrixDiseaseAlertData = "delete from admin.hhlinkpat_item where patid = ? and hhitemid = ?";

        public static string InsertDentrixDiseaseAlertDataWithoutCritical = "insert into admin.hhlinkpat_item(patid,hhitemid,status,reporteddate,startdate,enddate,note,popup,critical) "
                                                      + " values (?,?,1,getdate(),getdate(),'9999/12/31',?,?,null)";

        public static string InsertDentrixDiseaseData = "insert into admin.hhlinkpat_item(patid,hhitemid,status,reporteddate,startdate,enddate,note) "
                                                      + " values (?,?,1,getdate(),getdate(),'9999/12/31',?)";

        public static string GetDuplicateRecords = " select pi.pager,pi.Lastname,pi.Firstname,cp.Contactdate,cp.patid,cp.Provid,cp.title,cp.Contacttype,cp.notetext,cp.logid   from (select Contactdate,ct.patid,Provid,ct.title,Contacttype,fn.notetext,MAX(Contactid) AS LogId,count(1) as cnt from admin.Contact as ct inner join admin.FullNotes as fn on ct.Contactid = fn.noteid inner join admin.patient as p on p.patid = ct.patid where notetype=49 and contacttype in (3,5) and ( notetext like  'SMS sent -%' OR notetext like 'SMS received -%' OR notetext like 'SMS received -%' OR notetext like 'Inbound call answered - Duration:%' or notetext like 'Outbound %' ) "
                                                      + "group by Contactdate,ct.patid,Provid,ct.title,Contacttype,fn.notetext having count(1) > 1) as cp inner join admin.patient as pi  on cp.patid = pi.patid order by cp.cnt desc";

        public static string DeleteDuplicateContactLogs = "Delete from admin.Contact where Contactid = ?";

        public static string InsertUSerIdToStaff = "insert into admin.rsc (rsctype,rscid,feesched,idnum,lastname,firstname) values (2,'ADIT',0,?,'Adit','Adit') ";

        public static string GetUSerId = "select staff_id from admin.v_staff where first_name='Adit'";

        public static string GetPatientName = "select firstname+','+lastname from admin.patient Where Patid=?";

        public static string DeleteDuplicateFullNoteLogs = "Delete from admin.FullNotes where noteid = ? and notetype = 49";

        public static string CheckSMSCallRecordsBlankMobile = "select isnull(MAX(Contactid),0) AS NoteId,COUNT(1) AS TotalRecords from admin.contact as ct inner join admin.FullNotes as fn on ct.Contactid = fn.noteid and notetype = 49  where Contactdate = ? and Contacttime = ? and patid =  ? and Provid =  ? and Contacttype = ? and notetext=? AND Contactdate > '2021/11/15' AND  Contacttype IN (3,5) Group by Patid,provid,Contactdate,contacttime,Contacttype,Notetext having COUNT(1) > 1";

        public static string CheckSMSCallDuplicateDateTime = "select isnull(MAX(Contactid),0) AS NoteId,COUNT(1) AS TotalRecords from admin.contact as ct inner join admin.FullNotes as fn on ct.Contactid = fn.noteid and notetype = 49  where Contactdate = ? and Contacttime = ? and patid =  ? and Provid =  ? and Contacttype = ? AND Contactdate > '2021/11/15' Group by Patid,provid,Contactdate,contacttime,Contacttype having COUNT(1) > 0";



        #endregion


        public static string UpdateProcedureCodeIdInAppointment = "UPDATE admin.appt SET apptreason = ?,codeid_Number = ?,codetype_Number = ?, amt = ? WHERE apptId= ?";

        //public static string UpdateProcedureCodeIdInAppointment = "UPDATE admin.appt SET codeid_Number = ?,codetype_Number = ? WHERE apptId= ?";

        //public static string InsertFullProcedureLog = "INSERT INTO fullproclog"
        //                                            + "(automodifiedtimestamp,"
        //                                            + "patid,procid,guarid,chartstatus,authstatus,"
        //                                            + "procdate,"
        //                                            + "proccodeid,claimid,proclogclass,proclogorder,provid,history,authstatus2,"
        //                                            + "amt,"
        //                                            + "amtpriminspaid,amtsecinspaid,amtpreauth,createdate,amtsecpreauthdollar,paintflag,amtsecpreauthcents,materialcost,labexpense,cdareason,"
        //                                            + "cdalabcode,toothrangestart,toothrangeend,surfacestring,surfm,surfo,surfd,surfl,surff,surf5,amtsecpreauth,invalidasofflagstpdate,"
        //                                            + "medproctype,srflag,donotbillinsflag,diag,refid,reftype,labfee2,cdalabcode2,srflagex,txcaseid,txcaseindex,"
        //                                            + "checknum,appliedtopa,startcompdatereq,completiondate,startdate,primestoverride4orphaned,secestoverride4orphaned,indentrixpay,"
        //                                            + "donotsetlastvisit)"
        //                                            + "VALUES"
        //                                            + "((SELECT CURRENT_TIMESTAMP()),"
        //                                            + "@patid,@procid,@guarid,@chartstatus,@authstatus,"
        //                                            + "(SELECT SYSDATE()),"
        //                                            + "@proccodeid,@claimid,@proclogclass,@proclogorder,@provid,@history,@authstatus2,"
        //                                            + "(SELECT amt FROM feeschedule WHERE proccodeid = @proccodeid),"
        //                                            + "@amtpriminspaid,@amtsecinspaid,@amtpreauth,@createdate,@amtsecpreauthdollar,@paintflag,@amtsecpreauthcents,@materialcost,@labexpense,@cdareason,"
        //                                            + "@cdalabcode,@toothrangestart,@toothrangeend,@surfacestring,@surfm,@surfo,@surfd,@surfl,@surff,@surf5,@amtsecpreauth,@invalidasofflagstpdate,"
        //                                            + "@medproctype,@srflag,@donotbillinsflag,@diag,@refid,@reftype,@labfee2,@cdalabcode2,@srflagex,@txcaseid,@txcaseindex,"
        //                                            + "@checknum,@appliedtopa,@startcompdatereq,@completiondate,@startdate,@primestoverride4orphaned,@secestoverride4orphaned,@indentrixpay,"
        //                                            + "@donotsetlastvisit)";

        public static string InsertPaymentAmount = "INSERT INTO admin.fullproclog(patid,guarid,chartstatus,procdate,proclogclass,proclogorder,provid,amt,createdate,checknum,proccodeid)"
                                                    + "VALUES(?,?,90,?,?,?,?,?,getdate(),?,?)";

        public static string InsertFullProcedureLog = "INSERT INTO admin.fullproclog(patid,guarid,chartstatus,procdate,proccodeid,provid,amt,createdate)"
                                                    + "VALUES(?,?,105,getdate(),?,?,?,getdate())";

        public static string GetProcIdForProcedure = "SELECT procid FROM admin.fullproclog WHERE patid = ? AND provid = ? AND proccodeid = ?";

        public static string GetAmountFromProcedure = "SELECT amt FROM admin.feeschedule WHERE proccodeid = ?";

        public static string InsertPatientAggingGuarantorID = " Insert into admin.aging (Guarid,agingdate,billingtype) values (?,?,1)";

        #region CheckDuplicatePatient and SSN
        //Dipika
        //public static string CheckDentrixDuplicatePatientData = "SELECT v_p.patient_id AS Patient_EHR_ID, (LTRIM(RTRIM(v_p.first_name))) AS First_name, (LTRIM(RTRIM(v_p.last_name))) AS Last_name, "
        //                                            + " case when v_p.status  = 1 then 'Active'  when v_p.status = 2 then 'NonPatient' when v_p.status = 3 then 'InActive' when v_p.status = 4 then 'InActive' END AS EHR_Status, (LTRIM(RTRIM(v_p.mobile_phone))) AS Mobile,(LTRIM(RTRIM(v_p.home_phone))) AS Home_Phone, (LTRIM(RTRIM(v_p.work_phone))) AS Work_Phone " +
        //                                              " from admin.v_patient v_p where (LTRIM(RTRIM(v_p.first_name))) = ? and (LTRIM(RTRIM(v_p.last_name))) = ? ";
        public static string CheckDentrixDuplicatePatientData = "SELECT v_p.patid AS Patient_EHR_ID, "
                                                              + " (LTRIM(RTRIM(v_p.firstname))) AS First_name, "
                                                              + " (LTRIM(RTRIM(v_p.lastname))) AS Last_name, "
                                                              + " case when v_p.status  = 1 then 'Active'  when v_p.status = 2 then 'NonPatient' when v_p.status = 3 then 'InActive' when v_p.status = 4 then 'InActive' END AS EHR_Status, "
                                                              + " (LTRIM(RTRIM(v_p.pager))) AS Mobile, "
                                                              + " (LTRIM(RTRIM(a.phone))) AS Home_Phone, "
                                                              + " (LTRIM(RTRIM(v_p.workphone))) AS Work_Phone "
                                                              + " from admin.patient v_p "
                                                              + " Left Join admin.address A on A.addrid = v_p.addrid "
                                                              + " where (LTRIM(RTRIM(v_p.firstname))) = ? and(LTRIM(RTRIM(v_p.lastname))) = ? ";



        public static string CheckDentrixPatientExistorNot = "select patid from admin.patient where patid = ?";
        public static string CheckDentrixDuplicatePatientSSNData = "SELECT ssn from admin.patient where ssn = ? ";
        #endregion

        public static string UpdateDentrixMedicleResponseData = "Update admin.pq_responseset set responsedate = getdate() where responsesetuniqueid = ?";

        //public static string CheckDentrixPatientExistorNot = "select patid from admin.patient where patid = ?";

        public static string GetDentrixChartNumberSettings = "select notetext as newId  from admin.FullNotes where notetype=13 and noteid=1";

        public static string InsertApptHistory = "Insert into admin.appthist (automodifiedtimestamp,apptdate,parentapptid,modifieddate,opid,modifiedtime,patid,provid,userid,staffid,apptreason,apptlen,status,schedflag,appttype,timehr,timemin,labcaseappt,notechange,createflag,lastflag,deletedflag,createddate,createduserid,note)" +
                                                      "values(getdate(),'@apptdate','@apptid','@curdate','@operatoryid','@curtime','@patientid','@providerid','','@user','@apptreason','@apptlen','FIRM',0,'General','@timehr','@timemin',0,0,1,1,0,'@curdate','','') ";

        public static string GetapptData = "select apptid as appointment_ehr_id,apptdate,opid,provid,patid,apptlen,apptreason,timehr,timemin from admin.appt where apptid = '@appointmentid'";

        //rooja 
        public static string GetDentrixInsuranceData = "SELECT insid,empid,insconame,groupname,street1,city,country,phone,state,zipcode,payerid,'' as Clinic_Number from admin.inscarrier";
    }
}




