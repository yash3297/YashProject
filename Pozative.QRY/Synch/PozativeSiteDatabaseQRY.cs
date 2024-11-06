using Pozative.UTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.QRY
{
    public class PozativeSiteDatabaseQRY
    {

        #region LastUpdateRecord

        public static string PozativeEHRGetLastUpdateDate = "SELECT Sync_Table_Name FROM Sync_Table WHERE Sync_Table_Name=@Sync_Table_Name";

        public static string PozativeEHRInsert_Sync_Table_DateTime = "INSERT INTO Sync_Table (Sync_Table_Name,Last_Sync_Date) VALUES (@Sync_Table_Name,@Last_Sync_Date)";

        public static string PozativeEHRUpdate_Sync_Table_DateTime = "UPDATE Sync_Table SET Last_Sync_Date=@Last_Sync_Date WHERE Sync_Table_Name=@Sync_Table_Name";

        #endregion

        #region PozativeEHR Appointment Local

        public static string GetPozativeEHRLocalAppointmentData = "SELECT * FROM Pozative_Patient ";

        public static string Insert_PozativeEHR_Appointment = " INSERT INTO Pozative_Patient (appointment_id,Appt_Web_ID, patient_guid, patient_name, patient_phone, email_status, phone_status, "
                                        + " appointment_date, modified_time_stamp, created_at, queue_status, email, op_id,ReceivesSms, Last_Sync_Date) "
                                        + " VALUES (@appointment_id, @Appt_Web_ID, @patient_guid, @patient_name, @patient_phone, @email_status, @phone_status, "
                                        + " @appointment_date, @modified_time_stamp,  @created_at, @queue_status, @email, @op_id, @ReceivesSms, @Last_Sync_Date) ";


        public static string Update_PozativeEHR_Appointment = " UPDATE Pozative_Patient SET patient_guid = @patient_guid, patient_name = @patient_name, patient_phone = @patient_phone, "
                                                 + " appointment_date = @appointment_date, modified_time_stamp = @modified_time_stamp, created_at = @created_at, email = @email, "
                                                 + " op_id = @op_id, ReceivesSms = @ReceivesSms, Last_Sync_Date = @Last_Sync_Date "
                                                 + " WHERE appointment_id = @appointment_id ";


        public static string Delete_PozativeEHRLocal_Appointment = " DELETE FROM Pozative_Patient WHERE appointment_id = @appointment_id ";

        public static string Update_PozativeEHRAppointment_Web_Id = "UPDATE Pozative_Patient SET Appt_Web_ID = @Web_ID WHERE appointment_id = @EHR_ID ";

        #endregion

        #region  Pozative Appointment Live

        public static string GetSyncRequirePozativeEHRLocalAppointmentData = "SELECT * FROM Pozative_Patient WHERE Appt_Web_ID = 0 ";

        public static string CheckRecordIsExitsInLiveDB = "SELECT * FROM pozative_live.patients p WHERE locationid = @Location_Id AND appointment_id = @Appointment_Id;";


        public static string Insert_PozativeEHRLive_Appointment = " INSERT INTO patients (appointment_id, patient_guid, patient_name, patient_phone, email_status, phone_status, "
                                          + " appointment_date, modified_time_stamp, created_at, queue_status, email, locationid, operatory_id, opendentalid, eaglesoft_op_id) "
                                          + " VALUES (@appointment_id, @patient_guid, @patient_name, @patient_phone, @email_status, @phone_status, "
                                          + " @appointment_date, @modified_time_stamp, @created_at, @queue_status, @email, @locationid,@operatory_id, @opendentalid, @eaglesoft_op_id); ";


        public static string Update_PozativeEHRLive_Appointment = " UPDATE patients SET patient_guid = @patient_guid, patient_name =@patient_name, patient_phone = @patient_phone, "
                                                + " appointment_date = @appointment_date, modified_time_stamp = @modified_time_stamp, "
                                                + " created_at = @created_at, email = @email, operatory_id = @operatory_id, opendentalid = @opendentalid, eaglesoft_op_id = @eaglesoft_op_id "
                                                + " WHERE appointment_id = @appointment_id And locationid = @locationid ";

        #endregion
        
        #region OpenDental

        // public static string GetPozativeOpenDentalAppointmentData = " SELECT * FROM `appointment` where AptStatus=2 and DATE_FORMAT(AptDateTime, '%m/%d/%Y') >= '" + DateTime.Now.ToString("MM/dd/yyyy") + "'; ";


        public static string GetPozativeOpenDentalAppointmentData = " SELECT a.AptNum AS Appt_EHR_ID, pat.PatNum AS PatNum, pat.LName AS Last_Name, pat.FName AS First_Name, pat.MiddleI AS MI, "
                                                          + " pat.HmPhone AS Home_Contact, pat.WirelessPhone AS Mobile_Contact, pat.Email AS Email, pat.Address AS Address, "
                                                          + " pat.City AS City, pat.State AS ST, pat.Zip AS Zip, o.OperatoryNum AS Operatory_EHR_ID, o.OpName AS Operatory_Name, "
                                                          + " pro.ProvNum AS Provider_EHR_ID, CONCAT(pro.LName, ' ' , pro.FName) AS Provider_Name, "
                                                          + " a.AppointmentTypeNum AS ApptType_EHR_ID, aptype.AppointmentTypeName AS ApptType, a.AptDateTime AS Appt_DateTime "
                                                          + " FROM appointment a "
                                                          + " JOIN patient pat ON pat.PatNum = a.PatNum "
                                                          + " JOIN operatory o ON o.OperatoryNum = a.OP "
                                                          + " Left JOIN appointmenttype aptype ON aptype.AppointmentTypeNum = a.AppointmentTypeNum "
                                                          + " JOIN provider pro ON pro.ProvNum = a.ProvNum "
                                                          + " WHERE AptStatus=2 and a.DateTStamp > @ToDate ;";
                                                           //  DATE_FORMAT(a.DateTStamp, '%m/%d/%Y') = '" + Utility.Datetimesetting().ToString("yyyy/MM/dd") + "'; ";


        #endregion

        #region Dentrix

        public static string GetPozativeDentrixAppointmentData = " SELECT V_appt.appointment_id, a_appt.patid, V_appt.patient_name, V_appt.appointment_date, V_appt.start_hour, V_appt.start_minute, "
                                                        + " V_appt.patient_phone, V_appt.provider_last_name, V_appt.provider_id, V_appt.provider_first_name, "
                                                        + " v_opt.op_id, v_opt.op_title, adr.street1, adr.city, adr.country, adr.state, adr.zipcode, v_not.notetext, "
                                                        + " AType.def_id AS ApptType_EHR_ID, AType.descript AS ApptType_Name "
                                                        + " FROM admin.v_appointment V_appt "
                                                        + " JOIN admin.v_operatory v_opt ON v_opt.op_id = V_appt.Operatory_id "
                                                        + " JOIN admin.appt a_appt ON a_appt.apptid = V_appt.appointment_id "
                                                        + " Left JOIN admin.address adr ON adr.addrid = a_appt.newpataddrid "
                                                        + " Left JOIN admin.v_Appointment_Types AType ON AType.def_id = a_appt.appttype "
                                                        + " Left  JOIN admin.v_notes v_not ON v_not.noteid = a_appt.newpataddrid  AND v_not.notetype = 116"
                                                        + " WHERE  a_appt.status=-106 AND appointment_date > ?";

        public static string GetPozativeDentrixAppointmentDataDTX5 = " SELECT V_appt.appointment_id, a_appt.patid, V_appt.patient_name, V_appt.appointment_date, V_appt.start_hour, V_appt.start_minute, "
                                                      + " V_appt.patient_phone, V_appt.provider_last_name, V_appt.provider_id, V_appt.provider_first_name, "
                                                      + " v_opt.op_id, v_opt.op_title, adr.street1, adr.city, adr.country, adr.state, adr.zipcode, a_appt.Phone , "
                                                      + " AType.def_id AS ApptType_EHR_ID, AType.descript AS ApptType_Name "
                                                      + " FROM admin.v_appointment V_appt "
                                                      + " JOIN admin.v_operatory v_opt ON v_opt.op_id = V_appt.Operatory_id "
                                                      + " JOIN admin.appt a_appt ON a_appt.apptid = V_appt.appointment_id "
                                                      + " Left JOIN admin.address adr ON adr.addrid = a_appt.newpataddrid "
                                                      + " Left JOIN admin.v_Appointment_Types AType ON AType.def_id = a_appt.appttype "
                                                      + " WHERE  a_appt.status=-106 AND appointment_date > ?";

        #endregion
        
    }
}
