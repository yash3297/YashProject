using Pozative.UTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Pozative.QRY
{
    public class SynchLocalQRY
    {
        #region TreatmentDoc
        public static string SelectDocId = "select TreatmentPlanId from Treatment_Document where TreatmentPlanId =@TreatmentPlanId ";

        public static string Update_TreatmentDocNotFound_Live_To_Local = "UPDATE Treatment_Document set Is_Pdf_Created = 0 WHERE TreatmentPlanId =@TreatmentPlanId ";

        public static string Update_TreatmentFormDoc_Local_To_EHR = "UPDATE Treatment_Document set Is_EHR_Updated = 1 , TreatmentDoc_EHR_ID = @TreatmentDoc_EHR_ID WHERE TreatmentDoc_Web_ID =@TreatmentDoc_Web_ID";

        public static string GetLiveTreatmentFormDocData = " SELECT * From Treatment_Document where Is_Pdf_Created=1 And Is_EHR_Updated = 0 ";

        public static string UpdateDocStatus = "UPDATE Treatment_Document SET TreatmentDoc_Name = @TreatmentDoc_Name, Last_Sync_Date = @Last_Sync_Date,Is_Pdf_Created=@Is_Pdf_Created where TreatmentPlanId =@TreatmentPlanId";

        public static string UpdateDocAditUpdated = "UPDATE Treatment_Document SET Is_Adit_Updated = 1 where TreatmentPlanId =@TreatmentPlanId";

        public static string Insert_TreatmentDoc = "INSERT INTO Treatment_Document(Patient_EHR_ID,TreatmentDoc_Web_ID,Patient_Web_ID,TreatmentPlanId,TreatmentPlanName,Entry_DateTime,Clinic_Number,Service_Install_Id,PatientName,SubmittedDate)VALUES(@Patient_EHR_ID,@TreatmentDoc_Web_ID,@Patient_Web_ID,@TreatmentPlanId,@TreatmentPlanName,@Entry_DateTime,@Clinic_Number,@Service_Install_Id,@PatientName,@SubmittedDate)";

        public static string GetLocalPendingTreatmentDocData = " SELECT * From Treatment_Document where Is_Pdf_Created = 0  And Service_Install_Id = @Service_Install_Id ";

        public static string GetEHRPendingTreatmentDocData = "SELECT * From Treatment_Document where Is_EHR_Updated = 0 And Service_Install_Id = @Service_Install_Id And Is_Pdf_Created = 1";

        public static string ImportingTreatmentDocStatus = "select * from Treatment_Document where Is_Adit_Updated =0 and Is_EHR_Updated = 0";

        public static string CompletedTreatmentDocStatus = "select * from Treatment_Document where Is_Adit_Updated =0 and Is_EHR_Updated = 1";
        #endregion

        #region LastUpdateRecord

        public static string GetLastUpdateDate = "SELECT Sync_Table_Name FROM Sync_Table WHERE Sync_Table_Name=@Sync_Table_Name";

        public static string Insert_Sync_Table_DateTime = "INSERT INTO Sync_Table (Sync_Table_Name,Last_Sync_Date) VALUES (@Sync_Table_Name,@Last_Sync_Date)";

        public static string Update_Sync_Table_DateTime = "UPDATE Sync_Table SET Last_Sync_Date=@Last_Sync_Date WHERE Sync_Table_Name=@Sync_Table_Name";

        #endregion

        #region Appointment

        public static string GetLocalAppointmentData = "SELECT * FROM Appointment WHERE ( Appt_EHR_ID <> 0 OR Appt_EHR_ID <> '' OR  Appt_EHR_ID <> '0') and appt_datetime > @ToDate And Service_Install_Id = @Service_Install_Id";

        public static string GetLocalAppointmentDataAppointmentWise = "SELECT * FROM Appointment WHERE Appt_EHR_ID = @appointment_id And Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalAppointmentData_AllRecords = "SELECT * FROM Appointment where Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalAppointmentConformStatusData = "SELECT is_Status_Updated_From_Web,Clinic_Number,Service_Install_Id FROM Appointment Where Appt_EHR_ID = @appointment_id And Service_Install_Id = @Service_Install_Id";

        public static string UpdateLocalAppointmentConformStatusData = "UPDATE APPOINTMENT Set is_Status_Updated_From_Web = 1 Where Appt_EHR_ID = @appointment_id And Service_Install_Id = @Service_Install_Id";

        public static string GetLocalCompareForDeletedAppointmentData = "SELECT Appt_EHR_ID AS appointment_id,Clinic_Number,Service_Install_Id FROM Appointment WHERE is_deleted = 0 AND Appt_DateTime > @ToDate And Service_Install_Id = @Service_Install_Id";

        public static string GetLocalNewWebAppointmentData = "SELECT * FROM Appointment Where (Appt_EHR_ID = '0' OR Appt_EHR_ID = '') AND Is_Appt_DoubleBook = 0 And Service_Install_Id = @Service_Install_Id";

        public static string GetLastTowDaysLocalAppointmentData = " SELECT AP.Appt_EHR_ID,AP.Appt_Web_ID,AP.Last_Name,AP.First_Name,AP.Home_Contact,AP.Mobile_Contact,AP.Email,AP.Address,AP.City,AP.Operatory_EHR_ID,AP.Operatory_Name,AP.Provider_EHR_ID,AP.Provider_Name,AP."
                                                                + " birth_date,AP.ApptType_EHR_ID,AP.ApptType,AP.Appt_DateTime,AP.Status,AP.Appt_EndDateTime,AP.Patient_Status,AP.appointment_status_ehr_key,AP.Is_Appt,AP.is_ehr_updated,AP.Remind_DateTime,AP.Is_Adit_Updated,AP."
                                                                + " patient_ehr_id,AP.unschedule_status_ehr_key,AP.unschedule_status,AP.unschedule_status,AP.confirmed_status_ehr_key,AP.confirmed_status,AP.is_deleted,AP.MI,AP.ST,AP.Zip,AP.comment,AP.Appointment_Status, AP.is_asap, "
                                                                + " AP1.Appt_LocalDB_Id,AP1.Entry_DateTime,AP1.Last_Sync_Date,AP1.EHR_Entry_DateTime,"
                                                                + " AP.Clinic_Number,AP.Service_Install_Id, AP.proceduredesc,AP.procedurecode  "
                                                                + " FROM ( SELECT Service_Install_Id,Appt_EHR_Id ,MAX(Appt_LocalDB_Id) AS Appt_LocalDB_Id, MAX(Entry_DateTime) AS Entry_DateTime, MAX(Last_Sync_Date) AS Last_Sync_Date, MAX(EHR_Entry_DateTime) AS EHR_Entry_DateTime "
                                                                + " FROM Appointment WHERE Appt_EHR_Id <> '0' GROUP BY Service_Install_Id,Appt_EHR_Id ) AS AP1"
                                                                + " INNER JOIN Appointment AP ON Ap.Service_Install_Id = Ap1.Service_Install_Id And AP.Appt_EHR_Id = AP1.Appt_EHR_Id AND AP.Appt_LocalDB_Id = AP1.Appt_LocalDB_Id"
                                                                + " WHERE AP.Appt_EHR_ID <> '' AND AP.Appt_EHR_ID is not null and Cast(AP.Appt_EHR_ID As Numeric(20)) <> 0 AND  AP.Is_Adit_Updated = 0  Order by EHR_Entry_DateTime asc ";

        public static string GetLastTowDaysLocalAppointmentDataSoftdent = " SELECT AP.Appt_EHR_ID,AP.Appt_Web_ID,AP.Last_Name,AP.First_Name,AP.Home_Contact,AP.Mobile_Contact,AP.Email,AP.Address,AP.City,AP.Operatory_EHR_ID,AP.Operatory_Name,AP.Provider_EHR_ID,AP.Provider_Name,AP."
                                                                + " birth_date,AP.ApptType_EHR_ID,AP.ApptType,AP.Appt_DateTime,AP.Status,AP.Appt_EndDateTime,AP.Patient_Status,AP.appointment_status_ehr_key,AP.Is_Appt,AP.is_ehr_updated,AP.Remind_DateTime,AP.Is_Adit_Updated,AP."
                                                                + " patient_ehr_id,AP.unschedule_status_ehr_key,AP.unschedule_status,AP.unschedule_status,AP.confirmed_status_ehr_key,AP.confirmed_status,AP.is_deleted,AP.MI,AP.ST,AP.Zip,AP.comment,AP.Appointment_Status, AP.is_asap, "
                                                                + " AP1.Appt_LocalDB_Id,AP1.Entry_DateTime,AP1.Last_Sync_Date,AP1.EHR_Entry_DateTime,"
                                                                + " AP.Clinic_Number,AP.Service_Install_Id, AP.proceduredesc,AP.procedurecode  "
                                                                + " FROM ( SELECT Service_Install_Id,Appt_EHR_Id ,MAX(Appt_LocalDB_Id) AS Appt_LocalDB_Id, MAX(Entry_DateTime) AS Entry_DateTime, MAX(Last_Sync_Date) AS Last_Sync_Date, MAX(EHR_Entry_DateTime) AS EHR_Entry_DateTime "
                                                                + " FROM Appointment WHERE Appt_EHR_Id <> '0' GROUP BY Service_Install_Id,Appt_EHR_Id ) AS AP1"
                                                                + " INNER JOIN Appointment AP ON Ap.Service_Install_Id = Ap1.Service_Install_Id And AP.Appt_EHR_Id = AP1.Appt_EHR_Id AND AP.Appt_LocalDB_Id = AP1.Appt_LocalDB_Id"
                                                                + " WHERE AP.Appt_EHR_ID <> '' AND AP.Appt_EHR_ID is not null AND  AP.Is_Adit_Updated = 0  Order by EHR_Entry_DateTime asc ";


        public static string GetLastTowDaysLocalAppointmentDataByAptID = " SELECT AP.Appt_EHR_ID,AP.Appt_Web_ID,AP.Last_Name,AP.First_Name,AP.Home_Contact,AP.Mobile_Contact,AP.Email,AP.Address,AP.City,AP.Operatory_EHR_ID,AP.Operatory_Name,AP.Provider_EHR_ID,AP.Provider_Name,AP."
                                                                + " birth_date,AP.ApptType_EHR_ID,AP.ApptType,AP.Appt_DateTime,AP.Status,AP.Appt_EndDateTime,AP.Patient_Status,AP.appointment_status_ehr_key,AP.Is_Appt,AP.is_ehr_updated,AP.Remind_DateTime,AP.Is_Adit_Updated,AP."
                                                                + " patient_ehr_id,AP.unschedule_status_ehr_key,AP.unschedule_status,AP.unschedule_status,AP.confirmed_status_ehr_key,AP.confirmed_status,AP.is_deleted,AP.MI,AP.ST,AP.Zip,AP.comment,AP.Appointment_Status, AP.is_asap, "
                                                                + " AP1.Appt_LocalDB_Id,AP1.Entry_DateTime,AP1.Last_Sync_Date,AP1.EHR_Entry_DateTime,"
                                                                + " AP.Clinic_Number,AP.Service_Install_Id, AP.proceduredesc,AP.procedurecode  "
                                                                + " FROM ( SELECT Service_Install_Id,Appt_EHR_Id ,MAX(Appt_LocalDB_Id) AS Appt_LocalDB_Id, MAX(Entry_DateTime) AS Entry_DateTime, MAX(Last_Sync_Date) AS Last_Sync_Date, MAX(EHR_Entry_DateTime) AS EHR_Entry_DateTime "
                                                                + " FROM Appointment WHERE Appt_EHR_Id <> '0' GROUP BY Service_Install_Id,Appt_EHR_Id ) AS AP1"
                                                                + " INNER JOIN Appointment AP ON Ap.Service_Install_Id = Ap1.Service_Install_Id And AP.Appt_EHR_Id = AP1.Appt_EHR_Id AND AP.Appt_LocalDB_Id = AP1.Appt_LocalDB_Id"
                                                                + " WHERE Cast(AP.Appt_EHR_ID As Numeric(20)) > 0 AND  AP.Is_Adit_Updated = 0 And Ap.Appt_EHR_ID = @Appt_EHR_ID  Order by EHR_Entry_DateTime asc ";

        public static string GetLastTowDaysLocalAppointmentDataForAbeldent = " SELECT AP.Appt_EHR_ID,AP.Appt_Web_ID,AP.Last_Name,AP.First_Name,AP.Home_Contact,AP.Mobile_Contact,AP.Email,AP.Address,AP.City,AP.Operatory_EHR_ID,AP.Operatory_Name,AP.Provider_EHR_ID,AP.Provider_Name,AP."
                                                                + " birth_date,AP.ApptType_EHR_ID,AP.ApptType,AP.Appt_DateTime,AP.Status,AP.Appt_EndDateTime,AP.Patient_Status,AP.appointment_status_ehr_key,AP.Is_Appt,AP.is_ehr_updated,AP.Remind_DateTime,AP.Is_Adit_Updated,AP."
                                                                + " patient_ehr_id,AP.unschedule_status_ehr_key,AP.unschedule_status,AP.unschedule_status,AP.confirmed_status_ehr_key,AP.confirmed_status,AP.is_deleted,AP.MI,AP.ST,AP.Zip,AP.comment,AP.Appointment_Status, AP.is_asap, "
                                                                + " AP1.Appt_LocalDB_Id,AP1.Entry_DateTime,AP1.Last_Sync_Date,AP1.EHR_Entry_DateTime,"
                                                                + " AP.Clinic_Number,AP.Service_Install_Id, AP.proceduredesc,AP.procedurecode  "
                                                                + " FROM ( SELECT Service_Install_Id,Appt_EHR_Id ,MAX(Appt_LocalDB_Id) AS Appt_LocalDB_Id, MAX(Entry_DateTime) AS Entry_DateTime, MAX(Last_Sync_Date) AS Last_Sync_Date, MAX(EHR_Entry_DateTime) AS EHR_Entry_DateTime "
                                                                + " FROM Appointment WHERE Appt_EHR_Id <> '0' GROUP BY Service_Install_Id,Appt_EHR_Id ) AS AP1"
                                                                + " INNER JOIN Appointment AP ON Ap.Service_Install_Id = Ap1.Service_Install_Id And AP.Appt_EHR_Id = AP1.Appt_EHR_Id AND AP.Appt_LocalDB_Id = AP1.Appt_LocalDB_Id"
                                                                + " WHERE AP.Appt_EHR_ID <> '' and AP.Appt_EHR_ID is not null and CAST(AP.Appt_EHR_ID as NVARCHAR(50)) != '0' AND AP.Is_Adit_Updated = 0  Order by EHR_Entry_DateTime asc ";

        public static string GetLastTowDaysLocalAppointmentDataForAbeldentByAptID = " SELECT AP.Appt_EHR_ID,AP.Appt_Web_ID,AP.Last_Name,AP.First_Name,AP.Home_Contact,AP.Mobile_Contact,AP.Email,AP.Address,AP.City,AP.Operatory_EHR_ID,AP.Operatory_Name,AP.Provider_EHR_ID,AP.Provider_Name,AP."
                                                                + " birth_date,AP.ApptType_EHR_ID,AP.ApptType,AP.Appt_DateTime,AP.Status,AP.Appt_EndDateTime,AP.Patient_Status,AP.appointment_status_ehr_key,AP.Is_Appt,AP.is_ehr_updated,AP.Remind_DateTime,AP.Is_Adit_Updated,AP."
                                                                + " patient_ehr_id,AP.unschedule_status_ehr_key,AP.unschedule_status,AP.unschedule_status,AP.confirmed_status_ehr_key,AP.confirmed_status,AP.is_deleted,AP.MI,AP.ST,AP.Zip,AP.comment,AP.Appointment_Status, AP.is_asap, "
                                                                + " AP1.Appt_LocalDB_Id,AP1.Entry_DateTime,AP1.Last_Sync_Date,AP1.EHR_Entry_DateTime,"
                                                                + " AP.Clinic_Number,AP.Service_Install_Id, AP.proceduredesc,AP.procedurecode  "
                                                                + " FROM ( SELECT Service_Install_Id,Appt_EHR_Id ,MAX(Appt_LocalDB_Id) AS Appt_LocalDB_Id, MAX(Entry_DateTime) AS Entry_DateTime, MAX(Last_Sync_Date) AS Last_Sync_Date, MAX(EHR_Entry_DateTime) AS EHR_Entry_DateTime "
                                                                + " FROM Appointment WHERE Appt_EHR_Id <> '0' GROUP BY Service_Install_Id,Appt_EHR_Id ) AS AP1"
                                                                + " INNER JOIN Appointment AP ON Ap.Service_Install_Id = Ap1.Service_Install_Id And AP.Appt_EHR_Id = AP1.Appt_EHR_Id AND AP.Appt_LocalDB_Id = AP1.Appt_LocalDB_Id"
                                                                + " WHERE CAST(AP.Appt_EHR_ID as NVARCHAR(50)) != '0' AND AP.Is_Adit_Updated = 0 And AP.Appt_EHR_ID = @Appt_EHR_ID Order by EHR_Entry_DateTime asc ";

        public static string GetIs_Appt_DoubleBook_AppointmentData = " Select Appt_Web_ID,Clinic_Number,Service_Install_Id From Appointment "
                                        + " where Is_Appt_DoubleBook = 1 and Is_Adit_Updated = 0 AND Appt_Web_ID <> '' And Service_Install_Id = @Service_Install_Id ";

        public static string Insert_Appointment = " INSERT INTO Appointment (Appt_EHR_ID,patient_ehr_id, Appt_Web_ID, Last_Name, First_Name, MI, Home_Contact, Mobile_Contact, "
                                                + " Email, Address, City, ST, Zip,Operatory_EHR_ID, Operatory_Name, Provider_EHR_ID, Provider_Name, "
                                                + " comment, birth_date, ApptType_EHR_ID , ApptType, "
                                                + " Appt_DateTime, Appt_EndDateTime, Status, "
                                                + " appointment_status_ehr_key, Appointment_Status, "
                                                + " confirmed_status_ehr_key, confirmed_status, "
                                                + " unschedule_status_ehr_key, unschedule_status, "
                                                + " Patient_Status, Is_Appt,is_ehr_updated, Entry_DateTime, Last_Sync_Date, "
                                                + " EHR_Entry_DateTime, is_deleted, Is_Adit_Updated, is_asap,Clinic_Number,Service_Install_Id,appt_treatmentcode, "
                                                + " ProcedureDesc, ProcedureCode ) "
                                                + " VALUES (@Appt_EHR_ID, @patient_ehr_id, @Appt_Web_ID, @Last_Name, @First_Name, @MI, @Home_Contact, @Mobile_Contact, "
                                                + " @Email, @Address, @City, @ST, @Zip, @Operatory_EHR_ID, @Operatory_Name, @Provider_EHR_ID, @Provider_Name, "
                                                + " @comment, @birth_date, @ApptType_EHR_ID, @ApptType, "
                                                + " @Appt_DateTime, @Appt_EndDateTime, @Status, "
                                                + " @appointment_status_ehr_key, @Appointment_Status, "
                                                + " @confirmed_status_ehr_key, @confirmed_status, "
                                                + " @unschedule_status_ehr_key, @unschedule_status, "
                                                + " @Patient_Status, @Is_Appt, @is_ehr_updated, @Entry_DateTime, @Last_Sync_Date,"
                                                + " @EHR_Entry_DateTime, @is_deleted, @Is_Adit_Updated,@is_asap,@Clinic_Number,@Service_Install_Id,@appt_treatmentcode, "
                                                + " @ProcedureDesc, @ProcedureCode) ";


        public static string Update_Appointment_Where_Contact = " UPDATE Appointment SET Appt_EHR_ID = @Appt_EHR_ID, patient_ehr_id = @patient_ehr_id, Last_Name = @Last_Name, First_Name = @First_Name, MI = @MI, "
                                               + " Home_Contact = @Home_Contact , Email = @Email, Address = @Address, City = @City, ST = @ST, Zip = @Zip, "
                                               + " Operatory_EHR_ID = @Operatory_EHR_ID, Operatory_Name = @Operatory_Name, Provider_EHR_ID = @Provider_EHR_ID, Provider_Name = @Provider_Name, "
                                               + " comment = @comment, birth_date = @birth_date, "
                                               + " ApptType_EHR_ID = @ApptType_EHR_ID, ApptType = @ApptType, is_ehr_updated = @is_ehr_updated, "
                                               + " Appt_DateTime = @Appt_DateTime, Appt_EndDateTime = @Appt_EndDateTime, Status = @Status, "
                                               + " appointment_status_ehr_key = @appointment_status_ehr_key, Appointment_Status = @Appointment_Status, "
                                               + " confirmed_status_ehr_key = @confirmed_status_ehr_key, confirmed_status = @confirmed_status, "
                                               + " unschedule_status_ehr_key = @unschedule_status_ehr_key, unschedule_status = @unschedule_status, "
                                               + " Entry_DateTime = @Entry_DateTime, Last_Sync_Date = @Last_Sync_Date, "
                                               + " EHR_Entry_DateTime = @EHR_Entry_DateTime, is_deleted =@is_deleted, Is_Adit_Updated = @Is_Adit_Updated , is_asap = @is_asap "
                                               + " WHERE Appt_LocalDB_ID = @Appt_LocalDB_ID And Service_Install_Id = @Service_Install_Id";

        public static string Update_Appointment_Where_Appt_EHR_ID = " UPDATE Appointment SET  patient_ehr_id = @patient_ehr_id,Last_Name = @Last_Name, First_Name = @First_Name, MI = @MI, Home_Contact = @Home_Contact, "
                                             + " Mobile_Contact = @Mobile_Contact, Email =@Email, Address =@Address, City =@City, ST = @ST , Zip = @Zip, "
                                             + " Operatory_EHR_ID = @Operatory_EHR_ID, Operatory_Name = @Operatory_Name, Provider_EHR_ID = @Provider_EHR_ID, Provider_Name = @Provider_Name, "
                                             + " comment = @comment , birth_date = @birth_date, ApptType_EHR_ID = @ApptType_EHR_ID, ApptType = @ApptType, "
                                             + " is_ehr_updated = @is_ehr_updated, Appt_DateTime = @Appt_DateTime, Appt_EndDateTime = @Appt_EndDateTime,  Status = @Status,"
                                             + " appointment_status_ehr_key = @appointment_status_ehr_key,  Appointment_Status = @Appointment_Status, "
                                             + " confirmed_status_ehr_key = @confirmed_status_ehr_key, confirmed_status = @confirmed_status, "
                                             + " unschedule_status_ehr_key = @unschedule_status_ehr_key, unschedule_status = @unschedule_status, "
                                             + " Last_Sync_Date = @Last_Sync_Date, "
                                             + " EHR_Entry_DateTime = @EHR_Entry_DateTime, is_deleted =@is_deleted, Is_Adit_Updated = @Is_Adit_Updated , is_asap = @is_asap, "
                                             + " ProcedureDesc = @ProcedureDesc , ProcedureCode = @ProcedureCode "
                                             + " WHERE Appt_EHR_ID = @Appt_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Update_AppointmentMultiLocation_Where_Appt_EHR_ID = " UPDATE Appointment SET  patient_ehr_id = @patient_ehr_id,Last_Name = @Last_Name, First_Name = @First_Name, MI = @MI, Home_Contact = @Home_Contact, "
                                                   + " Mobile_Contact = @Mobile_Contact, Email =@Email, Address =@Address, City =@City, ST = @ST , Zip = @Zip, "
                                                   + " Operatory_EHR_ID = @Operatory_EHR_ID, Operatory_Name = @Operatory_Name, Provider_EHR_ID = @Provider_EHR_ID, Provider_Name = @Provider_Name, "
                                                   + " comment = @comment , birth_date = @birth_date, ApptType_EHR_ID = @ApptType_EHR_ID, ApptType = @ApptType, "
                                                   + " is_ehr_updated = @is_ehr_updated, Appt_DateTime = @Appt_DateTime, Appt_EndDateTime = @Appt_EndDateTime,  Status = @Status,"
                                                   + " appointment_status_ehr_key = @appointment_status_ehr_key,  Appointment_Status = @Appointment_Status, "
                                                   + " confirmed_status_ehr_key = @confirmed_status_ehr_key, confirmed_status = @confirmed_status, "
                                                   + " unschedule_status_ehr_key = @unschedule_status_ehr_key, unschedule_status = @unschedule_status, "
                                                   + " Last_Sync_Date = @Last_Sync_Date, "
                                                   + " EHR_Entry_DateTime = @EHR_Entry_DateTime, is_deleted =@is_deleted, Is_Adit_Updated = @Is_Adit_Updated , is_asap = @is_asap, "
                                                   + " ProcedureDesc = @ProcedureDesc , ProcedureCode = @ProcedureCode,Clinic_Number = @Clinic_Number "
                                                   + " WHERE Appt_EHR_ID = @Appt_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Appointment_Where_Appt_EHR_ID_Clinic_Number = " UPDATE Appointment SET  patient_ehr_id = @patient_ehr_id,Last_Name = @Last_Name, First_Name = @First_Name, MI = @MI, Home_Contact = @Home_Contact, "
                                     + " Mobile_Contact = @Mobile_Contact, Email =@Email, Address =@Address, City =@City, ST = @ST , Zip = @Zip, "
                                     + " Operatory_EHR_ID = @Operatory_EHR_ID, Operatory_Name = @Operatory_Name, Provider_EHR_ID = @Provider_EHR_ID, Provider_Name = @Provider_Name, "
                                     + " comment = @comment , birth_date = @birth_date, ApptType_EHR_ID = @ApptType_EHR_ID, ApptType = @ApptType, "
                                     + " is_ehr_updated = @is_ehr_updated, Appt_DateTime = @Appt_DateTime, Appt_EndDateTime = @Appt_EndDateTime,  Status = @Status,"
                                     + " appointment_status_ehr_key = @appointment_status_ehr_key,  Appointment_Status = @Appointment_Status, "
                                     + " confirmed_status_ehr_key = @confirmed_status_ehr_key, confirmed_status = @confirmed_status, "
                                     + " unschedule_status_ehr_key = @unschedule_status_ehr_key, unschedule_status = @unschedule_status, "
                                     + " Last_Sync_Date = @Last_Sync_Date, "
                                     + " EHR_Entry_DateTime = @EHR_Entry_DateTime, is_deleted =@is_deleted, Is_Adit_Updated = @Is_Adit_Updated , is_asap = @is_asap "
                                     + " WHERE Appt_EHR_ID = @Appt_EHR_ID  And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";


        public static string Update_Appointment_Where_Appt_Web_ID = " UPDATE Appointment SET Last_Name = @Last_Name, First_Name = @First_Name, MI = @MI, Home_Contact = @Home_Contact, "
                                             + " Mobile_Contact = @Mobile_Contact, Email = @Email, Address = @Address, City = @City, ST = @ST, Zip = @Zip, "
                                             + " Operatory_EHR_ID = @Operatory_EHR_ID, Operatory_Name = @Operatory_Name, Provider_EHR_ID = @Provider_EHR_ID, Provider_Name = @Provider_Name, "
                                             + " comment = @comment, birth_date = @birth_date, ApptType_EHR_ID = @ApptType_EHR_ID, ApptType = @ApptType, "
                                             + " is_ehr_updated= @is_ehr_updated, Appt_DateTime = @Appt_DateTime, Appt_EndDateTime = @Appt_EndDateTime, Status = @Status,  "
                                             + " appointment_status_ehr_key = @appointment_status_ehr_key,  Appointment_Status = @Appointment_Status, "
                                             + " confirmed_status_ehr_key = @confirmed_status_ehr_key, confirmed_status = @confirmed_status, "
                                             + " unschedule_status_ehr_key = @unschedule_status_ehr_key, unschedule_status = @unschedule_status, "
                                             + " Last_Sync_Date = @Last_Sync_Date, "
                                             + " EHR_Entry_DateTime = @EHR_Entry_DateTime,is_deleted =@is_deleted, Is_Adit_Updated = @Is_Adit_Updated, appt_treatmentcode=@appt_treatmentcode"
                                             + " WHERE Appt_Web_ID = @Appt_Web_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_Appointment = " UPDATE Appointment SET is_deleted = 1,Is_Adit_Updated = 0 WHERE is_deleted = 0 AND Appt_EHR_ID = @Appt_EHR_ID And Service_Install_Id = @Service_Install_Id AND Appt_EHR_ID <> '0' AND Is_Appt_DoubleBook = 0 ";

        public static string Delete_Appointment_With_Clinic_Number = " UPDATE Appointment SET is_deleted = 1,Is_Adit_Updated = 0 WHERE Appt_EHR_ID =@Appt_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id and is_deleted = 0";

        public static string InsertAppointment_With_DeleteFlag = "INSERT INTO Appointment (Appt_EHR_ID,Clinic_Number,is_deleted,Is_Adit_Updated,Appt_DateTime, Appt_EndDateTime,Entry_DateTime, Last_Sync_Date,EHR_Entry_DateTime,Service_Install_Id) "
                                                                + "  VALUES (@Appt_EHR_ID,@Clinic_Number,1,0,@Appt_DateTime, @Appt_EndDateTime,@Entry_DateTime, @Last_Sync_Date,@EHR_Entry_DateTime,@Service_Install_Id)";

        public static string Update_Appointment_Web_Id = "UPDATE Appointment SET Appt_Web_ID = @Web_ID ,Is_Adit_Updated = 1 WHERE Appt_EHR_ID = @EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Appointment_Web_Id_Where_Clinic_Number = "UPDATE Appointment SET Appt_Web_ID = @Web_ID ,Is_Adit_Updated = 1 WHERE Appt_EHR_ID =@EHR_ID  And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string UpdateAppointmentConfirmationSetConfirmed = "UPDATE Appointment SET appointment_status_ehr_key = @appointment_status_ehr_key ,Appointment_Status = @Appointment_Status,is_Status_Updated_From_Web = 1 WHERE Appt_EHR_ID =@Appt_EHR_ID And Service_Install_Id = @Service_Install_Id";

        public static string Update_ApptType_EHR_ID = "UPDATE Appointment SET Appt_EHR_ID = @Appt_EHR_ID ,Is_Adit_Updated = 0 WHERE Appt_Web_ID =@Appt_Web_ID And Service_Install_Id = @Service_Install_Id";

        public static string Update_Appointment_Is_Appt_DoubleBook_flg = " UPDATE Appointment SET Is_Appt_DoubleBook = 1, "
                                                    + " Is_Adit_Updated = 0 , Appointment_Status = 'Slot not available' "
                                                    + " WHERE Appt_Web_ID = @Appt_Web_ID And Service_Install_Id = @Service_Install_Id ";


        public static string SelectAppointmentEHRIdFromAppointment = "SELECT Appt_Web_ID FROM Appointment WHERE Appt_EHR_ID = @Appt_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Is_Appt_DoubleBook = "UPDATE Appointment SET Is_Adit_Updated = 1 WHERE Appt_Web_ID = @Web_ID And Service_Install_Id = @Service_Install_Id ";

        #endregion

        #region OperatoryEvent

        public static string GetLocalOperatoryEventData = "SELECT * FROM OperatoryEvent WHERE is_deleted = 0 AND StartTime > @ToDate And Service_Install_Id = @Service_Install_Id ";
        
        public static string DeleteAbeldentLocalOperatoryEventData = "UPDATE OperatoryEvent SET is_deleted = 1,Is_Adit_Updated = 0 Where is_deleted = 0 ";

        public static string DeleteAbeldentLocalOperatoryEventDataAll = "DELETE From OperatoryEvent";

        public static string GetLocalOperatoryChairData = "SELECT * FROM OperatoryTimeOff WHERE is_deleted = 0 AND StartTime > @ToDate And Service_Install_Id = @Service_Install_Id ";

        public static string GetPushLocalOperatoryEventData = "SELECT * FROM OperatoryEvent WHERE Is_Adit_Updated = 0";

        public static string GetPushLocalOperatoryDayOffData = "SELECT * FROM OperatoryTimeOff WHERE Is_Adit_Updated = 0";

        public static string Insert_OperatoryEventData = " INSERT INTO OperatoryEvent (OE_EHR_ID,OE_Web_ID, Operatory_EHR_ID, StartTime, EndTime , comment, Entry_DateTime, Last_Sync_Date, Is_Adit_Updated ,Clinic_Number,Service_Install_Id,Allow_Book_Appt) "
                                                         + " VALUES (@OE_EHR_ID, @OE_Web_ID, @Operatory_EHR_ID, @StartTime, @EndTime , @comment, @Entry_DateTime, @Last_Sync_Date, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id,@Allow_Book_Appt ) ";

        public static string Insert_OperatoryDayOffData = " INSERT INTO OperatoryTimeOff (OE_EHR_ID,OE_Web_ID, Operatory_EHR_ID,Provider_EHR_ID, StartTime, EndTime , comment, Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id) "
                                                         + " VALUES (@OE_EHR_ID, @OE_Web_ID, @Operatory_EHR_ID,@Provider_EHR_ID, @StartTime, @EndTime , @comment, @Entry_DateTime, @Last_Sync_Date, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id ) ";

        public static string Insert_OperatoryEventData_With_DeleteFlag = " INSERT INTO OperatoryEvent (OE_EHR_ID,OE_Web_ID, Operatory_EHR_ID, StartTime, EndTime , comment, Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,is_deleted,Clinic_Number,Service_Install_Id ) "
                                                      + " VALUES (@OE_EHR_ID, @OE_Web_ID, @Operatory_EHR_ID, @StartTime, @EndTime , @comment, @Entry_DateTime, @Last_Sync_Date, 0,1,@Clinic_Number,@Service_Install_Id ) ";

        public static string Update_OperatoryEventData = " UPDATE OperatoryEvent SET  Operatory_EHR_ID = @Operatory_EHR_ID, "
                                             + " StartTime = @StartTime, EndTime = @EndTime, comment = @comment , Is_Adit_Updated = 0 ,is_deleted = 0 ,Entry_DateTime = @Entry_DateTime,Allow_Book_Appt= @Allow_Book_Appt "
                                             + " WHERE OE_EHR_ID = @OE_EHR_ID And Service_Install_Id = @Service_Install_Id";

        public static string Update_OperatoryDayOffData = " UPDATE OperatoryTimeOff SET  Operatory_EHR_ID = @Operatory_EHR_ID, "
                                             + " Provider_EHR_ID=@Provider_EHR_ID,StartTime = @StartTime, EndTime = @EndTime, comment = @comment , Is_Adit_Updated = 0  "
                                             + " WHERE OE_EHR_ID = @OE_EHR_ID And Service_Install_Id = @Service_Install_Id";

        public static string Delete_OperatoryEventData = " UPDATE OperatoryEvent SET is_deleted = 1,Is_Adit_Updated = 0 WHERE  is_deleted = 0 AND OE_EHR_ID = @OE_EHR_ID And Service_Install_Id = @Service_Install_Id";

        public static string Delete_OperatoryDayOffData = " UPDATE OperatoryTimeOff SET is_deleted = 1,Is_Adit_Updated = 0 WHERE  is_deleted = 0 and OE_EHR_ID = @OE_EHR_ID And Service_Install_Id = @Service_Install_Id";

        public static string Update_OperatoryEvent_Web_Id = "UPDATE OperatoryEvent SET OE_Web_ID = @Web_ID ,Is_Adit_Updated = 1 WHERE OE_EHR_ID =@EHR_ID And Service_Install_Id = @Service_Install_Id";

        public static string Update_OperatoryDayoff_Web_Id = "UPDATE OperatoryTimeOff SET OE_Web_ID = @Web_ID ,Is_Adit_Updated = 1 WHERE OE_EHR_ID =@EHR_ID And Service_Install_Id = @Service_Install_Id";


        #endregion

        #region Holiday

        public static string GetLocalHolidayData = "SELECT * FROM Holiday WHERE is_deleted = 0 AND Service_Install_Id = @Service_Install_Id ";

        public static string GetPushLocalHolidayData = "SELECT * FROM Holiday WHERE Is_Adit_Updated = 0";

        public static string Insert_HolidayData = " INSERT INTO Holiday (H_EHR_ID,H_Web_ID, H_Operatory_EHR_ID, SchedDate, StartTime_1, EndTime_1 , StartTime_2, EndTime_2, StartTime_3, EndTime_3, comment, Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id ) "
                                                         + " VALUES (@H_EHR_ID, @H_Web_ID, @H_Operatory_EHR_ID, @SchedDate, @StartTime_1, @EndTime_1, @StartTime_2, @EndTime_2, @StartTime_3, @EndTime_3, @comment, @Entry_DateTime, @Last_Sync_Date, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id ) ";

        public static string Update_HolidayData = " UPDATE Holiday SET  H_Operatory_EHR_ID = @H_Operatory_EHR_ID, SchedDate = @SchedDate, "
                                             + " StartTime_1 = @StartTime_1, EndTime_1 = @EndTime_1, StartTime_2 = @StartTime_2, EndTime_2 = @EndTime_2, "
                                             + " StartTime_3 = @StartTime_3, EndTime_3 = @EndTime_3, comment = @comment , Is_Adit_Updated = 0 "
                                             + " WHERE H_EHR_ID = @H_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_HolidayData = " UPDATE Holiday SET is_deleted = 1,Is_Adit_Updated = 0 WHERE H_EHR_ID = @H_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        #region Dentrix

        public static string GetLocalDentrixHolidayData = "SELECT * FROM Holiday WHERE is_deleted = 0 AND H_Operatory_EHR_ID = '' ";

        public static string Update_Dentrix_HolidayData = " UPDATE Holiday SET comment = @comment , Is_Adit_Updated = 0,Last_Sync_Date = @Last_Sync_Date WHERE is_deleted = 0 and SchedDate = @SchedDate ";

        public static string Delete_Dentrix_HolidayData = "UPDATE Holiday SET is_deleted = 1,Is_Adit_Updated = 0 WHERE SchedDate = @SchedDate ";

        public static string GetLocalDentrixOperatoryHolidayData = "SELECT * FROM Holiday WHERE is_deleted = 0 AND H_Operatory_EHR_ID <> '' and SchedDate >= ? and SchedDate <= ?";

        public static string Update_Dentrix_Operatory_HolidayData = " UPDATE Holiday SET StartTime_1 = @StartTime_1, EndTime_1 = @EndTime_1, StartTime_2 = @StartTime_2, EndTime_2 = @EndTime_2, "
                                           + " StartTime_3 = @StartTime_3, EndTime_3 = @EndTime_3, comment = @comment , Is_Adit_Updated = 0 "
                                           + " WHERE H_Operatory_EHR_ID = @H_Operatory_EHR_ID and SchedDate = @SchedDate";

        public static string Delete_Dentrix_Operatory_HolidayData = "UPDATE Holiday SET is_deleted = 1,Is_Adit_Updated = 0 WHERE  H_Operatory_EHR_ID = @H_Operatory_EHR_ID and SchedDate = @SchedDate ";

        #endregion

        #region EagleSoft

        public static string GetLocalEaglesoftHolidayData = "SELECT * FROM Holiday WHERE is_deleted = 0 and Service_Install_Id = @Service_Install_Id";

        public static string Update_Eaglesoft_HolidayData = " UPDATE Holiday SET comment = ? , Is_Adit_Updated = 0,Last_Sync_Date = ? WHERE is_deleted = 0 and SchedDate = ? ";

        public static string Delete_Eaglesoft_HolidayData = "UPDATE Holiday SET is_deleted = 1,Is_Adit_Updated = 0 WHERE SchedDate = @SchedDate And Service_Install_Id = @Service_Install_Id";

        #endregion

        public static string Update_Holiday_Web_Id = "UPDATE Holiday SET H_Web_ID = @Web_ID ,Is_Adit_Updated = 1 WHERE H_EHR_ID = @EHR_ID And Service_Install_Id = @Service_Install_Id";


        #endregion

        #region User
        //public static string GetLocalUserData = "SELECT * FROM User_Login WHERE Service_Install_Id = @Service_Install_Id ";

        //public static string Insert_User = " INSERT INTO User_Login (User_EHR_ID,User_Web_ID,User_Name, "
        //                                     + " Password, First_name, Last_name,Is_Active,Clinic_Number,Created_Date,Last_Sync_Date,EHR_Entry_Date,Is_EHR_Updated,Is_Adit_Updated,Is_Deleted,Service_Install_Id) "
        //                                     + " VALUES (@User_EHR_ID, @User_Web_ID, @User_Name, @Password, @First_name, @Last_name,"
        //                                     + "@is_active, @Clinic_Number, @Created_Date, @Last_Sync_Date, @EHR_Entry_Date, @Is_EHR_Updated, @Is_Adit_Updated, @Is_Deleted, @Service_Install_Id)";

        //public static string Update_User = " UPDATE User_Login SET User_Name = @User_Name,Password = @Password, First_name = @First_name, Last_name = @Last_name"
        //                                     + " Is_Active = @is_active, Created_Date = @Created_Date, Is_EHR_Updated = @Is_EHR_Updated, is_active = @is_active, "
        //                                     + " Last_Sync_Date = @Last_Sync_Date, Is_Ad" +
        //    "                               WHERE User_EHR_ID = @User_EHR_IDWHERE User_LocalDB_ID = @User_LocalDB_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id";

        //public static string Delete_User = "UPDATE User_Login SET Is_Deleted = 1, Is_Adit_Updated = 0, Is_Active = 0 WHERE User_EHR_ID = @User_EHR_ID And Service_Install_Id = @Service_Install_Id";

        #endregion

        #region Provider

        public static string GetLocalProviderData = "SELECT * FROM Providers where Service_Install_Id = @Service_Install_Id";

        public static string GetLocalProviderData_Clinic = "SELECT * FROM Providers where Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id";

        public static string GetPushLocalProviderData = "SELECT * FROM Providers WHERE Is_Adit_Updated = 0 ";

        public static string Insert_Provider = " INSERT INTO Providers (Provider_EHR_ID, Provider_Web_ID,  Last_Name, "
                                             + " First_Name, MI,provider_speciality, gender, is_active, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id) "
                                             + " VALUES (@Provider_EHR_ID, @Provider_Web_ID, @Last_Name, @First_Name, "
                                             + " @MI, @provider_speciality, @gender , @is_active, @Last_Sync_Date, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id) ";

        public static string Update_Provider = " UPDATE Providers SET Last_Name = @Last_Name,First_Name = @First_Name, MI = @MI, "
                                             + " provider_speciality = @provider_speciality, gender = @gender, is_active = @is_active, "
                                             + " Last_Sync_Date = @Last_Sync_Date, Is_Adit_Updated = @Is_Adit_Updated "
                                             + " WHERE Provider_EHR_ID = @Provider_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Provider_Pull = " UPDATE Providers SET "
                                             + " Provider_Web_ID = @Provider_Web_ID, Last_Name = @Last_Name, First_Name = @First_Name, "
                                             + " gender = @gender, provider_speciality = @provider_speciality, bio = @bio, education = @education, "
                                             + " accreditation = @accreditation, membership = @membership, language = @language, age_treated_min = @age_treated_min, "
                                             + " age_treated_max = @age_treated_max "
                                             + " WHERE Provider_EHR_ID = @Provider_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id  ";

        public static string Delete_Provider = " Delete From Providers WHERE Provider_EHR_ID = @Provider_EHR_ID And  Clinic_Number = @Clinic_Number And Service_Install_Id =@Service_Install_Id  ";

        public static string Update_Provider_Web_Id = "UPDATE Providers SET Provider_Web_ID = @Web_ID, Is_Adit_Updated = 1 WHERE Provider_EHR_ID = @EHR_ID And Clinic_Number =  @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Update_User_Web_Id = "UPDATE Users SET User_Web_ID = @Web_ID, Is_Adit_Updated = 1 WHERE User_EHR_ID = @EHR_ID And Clinic_Number =  @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        #endregion

        #region ProviderHours

        public static string GetLocalProviderHoursData = "SELECT * FROM ProviderHours WHERE Is_Deleted = 0 and Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalProviderHoursData_BlankData = "SELECT * FROM ProviderHours WHERE 1 = 2";

        public static string GetPushLocalProviderHoursData = "SELECT * FROM ProviderHours WHERE Is_Adit_Updated = 0 order by starttime asc ";

        public static string Insert_ProviderHours = " INSERT INTO ProviderHours (PH_EHR_ID, PH_Web_ID,  Provider_EHR_ID, "
                                             + " Operatory_EHR_ID, StartTime,EndTime, comment, Entry_DateTime, Last_Sync_Date,is_deleted, Is_Adit_Updated,Clinic_Number,Service_Install_Id) "
                                             + " VALUES (@PH_EHR_ID, @PH_Web_ID, @Provider_EHR_ID,  "
                                             + " @Operatory_EHR_ID, @StartTime, @EndTime , @comment,@Entry_DateTime, @Last_Sync_Date,0, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id) ";

        public static string Update_ProviderHours = " UPDATE ProviderHours SET Provider_EHR_ID = @Provider_EHR_ID,Operatory_EHR_ID = @Operatory_EHR_ID, "
                                             + " StartTime = @StartTime, EndTime = @EndTime, comment =@comment , "
                                             + " Last_Sync_Date = @Last_Sync_Date, Is_Adit_Updated = @Is_Adit_Updated "
                                             + " WHERE PH_EHR_ID = @PH_EHR_ID  and Service_Install_Id = @Service_Install_Id ";


        public static string Delete_ProviderHours = " UPDATE ProviderHours SET is_deleted = 1,Is_Adit_Updated = 0 WHERE is_deleted = 0 and PH_EHR_ID = @PH_EHR_ID AND Service_Install_Id = @Service_Install_Id ";

        public static string DeleteAbelDent_ProviderHours = "UPDATE ProviderHours SET is_deleted = 1,Is_Adit_Updated = 0 WHERE is_deleted = 0 and Service_Install_Id = @Service_Install_Id  ";

        public static string DeleteAbelDent_ProviderHoursAll = "DELETE FROM ProviderHours";

        public static string Update_PH_SameDate_Is_Adit_Updated_flg = " UPDATE ProviderHours SET Is_Adit_Updated = 0 "
                                           + " WHERE Starttime > @Starttime and Starttime < @Endtime And Service_Install_Id = @Service_Install_Id  ";

        public static string Update_ProviderHours_Web_Id = "UPDATE ProviderHours SET PH_Web_ID = @Web_ID, Is_Adit_Updated = 1 WHERE PH_EHR_ID = @EHR_ID AND Service_Install_Id = @Service_Install_Id  ";


        #endregion

        #region Speciality

        public static string GetLocalSpecialityData = "SELECT * FROM Speciality where Service_Install_Id = @Service_Install_Id ";

        public static string GetPushLocalSpecialityData = "SELECT * FROM Speciality WHERE Is_Adit_Updated = 0";

        public static string Insert_Speciality = " INSERT INTO Speciality (Speciality_Name, Last_Sync_Date,Is_Adit_Updated,Clinic_Number,Service_Install_Id) VALUES (@Speciality_Name, @Last_Sync_Date,@Is_Adit_Updated,@Clinic_Number,@Service_Install_Id) ";

        public static string Update_Speciality_Web_Id = "UPDATE Speciality SET Is_Adit_Updated = 1 ";

        #endregion

        #region Folder List

        public static string GetLocalFolderListData = "SELECT * FROM FolderList where Service_Install_Id = @Service_Install_Id";

        public static string Insert_FolderList = "INSERT INTO FolderList(FolderList_EHR_ID,Folder_Name,Last_Sync_Date,Is_Adit_Updated,Clinic_Number,Service_Install_Id,FolderOrder)VALUES(@FolderList_EHR_ID,@Folder_Name,@Last_Sync_Date,@Is_Adit_Updated,@Clinic_Number,@Service_Install_Id,@FolderOrder)";

        public static string Update_FolderList = "UPDATE FolderList SET Folder_Name = @Folder_Name,Last_Sync_Date = @Last_Sync_Date,Is_Adit_Updated = @Is_Adit_Updated,Clinic_Number = @Clinic_Number,FolderOrder =  @FolderOrder WHERE FolderList_EHR_ID = @FolderList_EHR_ID AND Service_Install_Id = @Service_Install_Id";

        public static string Delete_FolderList = "UPDATE FolderList SET is_deleted = 1,Is_Adit_Updated = 0 WHERE is_deleted = 0 and FolderList_EHR_ID = @FolderList_EHR_ID  And Service_Install_Id = @Service_Install_Id  ";

        public static string Delete_False_FolderList = "UPDATE FolderList SET is_deleted = 0,Is_Adit_Updated = 0 WHERE is_deleted = 1 and FolderList_EHR_ID = @FolderList_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string GetPushLocalFolderListData = "SELECT * FROM FolderList WHERE Is_Adit_Updated = 0";

        public static string Update_FolderList_Web_Id = "UPDATE FolderList SET FolderList_Web_ID = @FolderList_Web_ID, Is_Adit_Updated = 1 WHERE FolderList_EHR_ID = @FolderList_EHR_ID  And Service_Install_Id = @Service_Install_Id ";

        #endregion

        #region Operatory

        public static string GetLocalOperatoryData = "SELECT * FROM Operatory where Service_Install_Id = @Service_Install_Id";

        public static string GetPushLocalOperatoryData = "SELECT * FROM Operatory WHERE Is_Adit_Updated = 0";

        public static string Insert_Operatory = " INSERT INTO Operatory (Operatory_EHR_ID,Operatory_Web_ID,Operatory_Name,Last_Sync_Date,Is_Adit_Updated,Clinic_Number,Service_Install_Id,OperatoryOrder) "
                                              + " VALUES (@Operatory_EHR_ID, @Operatory_Web_ID , @Operatory_Name,@Last_Sync_Date,@Is_Adit_Updated,@Clinic_Number,@Service_Install_Id,@OperatoryOrder) ";

        public static string Update_Operatory = " UPDATE Operatory SET Operatory_Name = @Operatory_Name,OperatoryOrder=@OperatoryOrder, "
                                              + " Last_Sync_Date = @Last_Sync_Date, Is_Adit_Updated = @Is_Adit_Updated, Clinic_Number = @Clinic_Number WHERE Operatory_EHR_ID = @Operatory_EHR_ID And Service_Install_Id = @Service_Install_Id  ";

        public static string Update_PatientForm_PatientEHRId = " UPDATE Patient_Form SET Patient_EHR_Id = @Patient_EHR_Id "
                                            + " WHERE PatientForm_Web_ID = @PatientForm_Web_ID  AND Service_Install_Id = @Service_Install_Id ";


        public static string UpdatePatientPaymentEHRId_In_Local = " UPDATE PatientPayment SET PaymentEHRId = @PaymentEHRId,PaymentUpdatedEHR = 1,PaymentUpdatedEHRDateTime = getdate()  "
                                            + " WHERE PatientPaymentWebId = @PatientPaymentWebId  AND Service_Install_Id = @Service_Install_Id ";

        public static string Update_Operatory_Pull = " UPDATE Operatory SET Operatory_Name = @Operatory_Name, Operatory_Web_ID = @Operatory_Web_ID, "
                                              + " Last_Sync_Date = @Last_Sync_Date WHERE Operatory_EHR_ID = @Operatory_EHR_ID AND Service_Install_Id = @Service_Install_Id  ";

        public static string Delete_Operatory = " UPDATE Operatory SET is_deleted = 1,Is_Adit_Updated = 0 WHERE is_deleted = 0 and Operatory_EHR_ID = @Operatory_EHR_ID  And Service_Install_Id = @Service_Install_Id  ";

        public static string Delete_False_Operatory = " UPDATE Operatory SET is_deleted = 0,Is_Adit_Updated = 0 WHERE is_deleted = 1 and Operatory_EHR_ID = @Operatory_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Operatory_Web_Id = "UPDATE Operatory SET Operatory_Web_ID = @Web_ID, Is_Adit_Updated = 1 WHERE Operatory_EHR_ID = @EHR_ID  And Service_Install_Id = @Service_Install_Id ";

        #endregion

        #region OperatoryHours

        public static string GetLocalOperatoryHoursData = "SELECT * FROM OperatoryHours where Service_Install_Id = @Service_Install_Id";

        public static string GetLocalOperatoryHoursData_BlankStructure = "SELECT * FROM OperatoryHours Where 1= 2";

        public static string GetLocalOperatoryOfficeHoursData = "SELECT * FROM OperatoryOfficeHours where Service_Install_Id = @Service_Install_Id ";

        public static string GetPushLocalOperatoryHoursData = "SELECT * FROM OperatoryHours WHERE Is_Adit_Updated = 0  order by is_deleted desc";

        public static string OperatoryHoursBlankStructure = "SELECT * FROM OperatoryHours WHERE 1 = 2";

        public static string Insert_OperatoryHours = " INSERT INTO OperatoryHours (OH_EHR_ID, OH_Web_ID,  Operatory_EHR_ID, "
                                             + "  StartTime,EndTime, comment, Entry_DateTime, Last_Sync_Date,is_deleted, Is_Adit_Updated,Clinic_Number,Service_Install_Id) "
                                             + " VALUES (@OH_EHR_ID, @OH_Web_ID, @Operatory_EHR_ID,  "
                                             + " @StartTime, @EndTime , @comment,@Entry_DateTime, @Last_Sync_Date,0, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id) ";

        public static string Update_OperatoryHours = " UPDATE OperatoryHours SET Operatory_EHR_ID = @Operatory_EHR_ID, "
                                             + " StartTime = @StartTime, EndTime = @EndTime, comment =@comment , "
                                             + " Last_Sync_Date = @Last_Sync_Date, Is_Adit_Updated = @Is_Adit_Updated "
                                             + " WHERE OH_EHR_ID = @OH_EHR_ID And Service_Install_Id = @Service_Install_Id  ";


        public static string Delete_OperatoryHours = " UPDATE OperatoryHours SET is_deleted = 1,Is_Adit_Updated = 0 WHERE is_deleted = 0 and OH_EHR_ID =@OH_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_AbelDentOperatoryHours = " UPDATE OperatoryHours SET is_deleted = 1,Is_Adit_Updated = 0 WHERE is_deleted = 0 and Service_Install_Id = @Service_Install_Id ";

        public static string Delete_AbelDentOperatoryHoursAll = " DELETE FROM OperatoryHours";

        public static string Update_OH_SameDate_Is_Adit_Updated_flg = " UPDATE OperatoryHours SET Is_Adit_Updated = 0 "
                                                   + " WHERE Starttime > @Starttime and Starttime < @Endtime And Service_Install_Id = @Service_Install_Id ";

        public static string Update_OperatoryHours_Web_Id = "UPDATE OperatoryHours SET OH_Web_ID = @Web_ID, Is_Adit_Updated = 1 WHERE OH_EHR_ID = @EHR_ID And Service_Install_Id = @Service_Install_Id ";


        #endregion


        #region ApptType

        public static string GetLocalApptTypeData = "SELECT * FROM Appointment_Type WHERE ApptType_EHR_ID is Not null and ApptType_EHR_ID <> '' And Service_Install_Id = @Service_Install_Id ";

        public static string GetPushLocalApptTypeData = "SELECT * FROM Appointment_Type WHERE ApptType_EHR_ID is Not null and ApptType_EHR_ID <> '' AND Is_Adit_Updated = 0";

        public static string Insert_ApptType = " INSERT INTO Appointment_Type (ApptType_EHR_ID, ApptType_Web_ID, Type_Name, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id) "
                                                                     + " VALUES (@ApptType_EHR_ID, @ApptType_Web_ID, @Type_Name, @Last_Sync_Date, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id) ";

        public static string Update_ApptType = " UPDATE Appointment_Type SET  Type_Name = @Type_Name, Last_Sync_Date = @Last_Sync_Date, Is_Adit_Updated = @Is_Adit_Updated WHERE ApptType_EHR_ID = @ApptType_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Update_ApptType_Pull = " UPDATE Appointment_Type SET ApptType_Web_ID = @ApptType_Web_ID, Type_Name = @Type_Name "
                                                                     + " WHERE ApptType_EHR_ID = @ApptType_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";


        // public static string Delete_ApptType = " Delete From Appointment_Type WHERE ApptType_EHR_ID = @ApptType_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";
        public static string Delete_ApptType = " Update Appointment_Type SET IS_Deleted = 1,IS_Adit_Updated = 0 WHERE ApptType_EHR_ID = @ApptType_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Update_ApptType_Web_Id = "UPDATE Appointment_Type SET ApptType_Web_ID = @Web_ID, Is_Adit_Updated = 1 WHERE ApptType_EHR_ID =@EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        #endregion
        
        //#region Patient

        //public static string GetLocalPatientData = "SELECT * FROM Patient";

        //public static string Insert_Patient = " INSERT INTO Patient (patient_ehr_id, patient_Web_ID, fullname,email,mobile,phone,birth_date,last_visit,next_visit,revenue, Last_Sync_Date) "
        //                                               + " VALUES (@patient_ehr_id, @patient_Web_ID, @fullname, @email,@mobile,@phone,@birth_date,@last_visit,@next_visit,@revenue,@Last_Sync_Date) ";

        //public static string Update_Patient = " UPDATE Patient SET  fullname = @fullname, email = @email, mobile = @mobile, phone = @phone, "
        //                                               + " birth_date = @birth_date, last_visit = @last_visit, next_visit = @next_visit, revenue = @revenue, Last_Sync_Date = @Last_Sync_Date "
        //                                               + " WHERE patient_ehr_id = @patient_ehr_id  ";

        //public static string Update_Patient_Pull = " UPDATE Patient SET patient_Web_ID = @patient_Web_ID, fullname = @fullname, email = @email, mobile = @mobile, phone = @phone, "
        //                                               + " birth_date = @birth_date, last_visit = @last_visit, next_visit = @next_visit, revenue = @revenue "
        //                                               + " WHERE patient_ehr_id = @patient_ehr_id  ";

        //public static string Delete_Patient = " Delete From Patient WHERE Patient_EHR_ID = @Patient_EHR_ID ";

        //public static string Update_Patient_Web_Id = "UPDATE Patient SET Patient_Web_ID = @Web_ID WHERE Patient_EHR_ID =@EHR_ID ";

        //#endregion

        #region Patient
        public static string GetLocalInsertPatientData = "Select patient_ehr_id, clinic_number, Is_Adit_Updated FROM Patient where Service_Install_Id = @Service_Install_Id;";

        public static string GetLocalPatientData = " SELECT Patient_LocalDB_ID, patient_ehr_id, patient_Web_ID, First_name, Last_name, "
                                     + " Middle_Name, Salutation, preferred_name, Status, Sex, MaritalStatus, Birth_Date, Email, "
                                     + " Mobile, Home_Phone, Work_Phone, Address1, Address2,City, State, Zipcode, ResponsibleParty_Status,  "
                                     + " (CASE WHEN CurrentBal IS NULL OR CurrentBal = '' THEN '0' ELSE CurrentBal END) AS CurrentBal, "
                                     + " (CASE WHEN ThirtyDay IS NULL OR ThirtyDay = '' THEN '0' ELSE ThirtyDay END) AS ThirtyDay, "
                                     + " (CASE WHEN SixtyDay IS NULL OR SixtyDay  = '' THEN '0' ELSE SixtyDay END) AS SixtyDay, "
                                     + " (CASE WHEN NinetyDay IS NULL OR NinetyDay = '' THEN '0' ELSE NinetyDay END) AS NinetyDay, "
                                     + " (CASE WHEN Over90 IS NULL  OR Over90 = '' THEN '0' ELSE Over90 END) AS Over90, FirstVisit_Date, LastVisit_Date,  "
                                     + " Primary_Insurance, Primary_Insurance_CompanyName, Primary_Ins_Subscriber_ID, "
                                     + " Secondary_Insurance, Secondary_Insurance_CompanyName, Secondary_Ins_Subscriber_ID, "
                                     + " Guar_ID, Pri_Provider_ID, Sec_Provider_ID, ReceiveSms, ReceiveEmail, nextvisit_date, due_date,  "
                                     + " (CASE WHEN remaining_benefit IS NULL OR remaining_benefit= ''  THEN '0' ELSE remaining_benefit END) AS remaining_benefit, "
                                     + " (CASE WHEN used_benefit IS NULL OR used_benefit = ''  THEN '0' ELSE used_benefit END) AS used_benefit, "
                                     + " (CASE WHEN collect_payment IS NULL OR collect_payment = ''  THEN '0' ELSE collect_payment END) AS collect_payment,  "
                                     + " EHR_Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id,is_deleted,EHR_Status,ReceiveVoiceCall,Patient_status,Patient_status_Compare,[ssn],[driverlicense],[groupid],[emergencycontactId],[EmergencyContact_First_Name],[EmergencyContact_Last_Name] ,[emergencycontactnumber],[school] ,[employer] ,[spouseId] ,[responsiblepartyId] ,[responsiblepartyssn],[responsiblepartybirthdate],[Spouse_First_Name] ,[Spouse_Last_Name] ,[ResponsibleParty_First_Name] ,[ResponsibleParty_Last_Name], [Prim_Ins_Company_Phonenumber] ,[Sec_Ins_Company_Phonenumber],Patient_Note,Is_Status_Adit_Updated,PreferredLanguage  FROM Patient where Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientDataEaglesoft = " SELECT Patient_LocalDB_ID, patient_ehr_id, patient_Web_ID, First_name, Last_name, "
                                             + " Middle_Name, Salutation, preferred_name, Status, Sex, MaritalStatus, Birth_Date, Email, "
                                             + " Mobile, Home_Phone, Work_Phone, Address1, Address2,City, State, Zipcode, ResponsibleParty_Status,  "
                                             + " (CASE WHEN CurrentBal IS NULL OR CurrentBal = '' THEN '0' ELSE CurrentBal END) AS CurrentBal, "
                                             + " (CASE WHEN ThirtyDay IS NULL OR ThirtyDay = '' THEN '0' ELSE ThirtyDay END) AS ThirtyDay, "
                                             + " (CASE WHEN SixtyDay IS NULL OR SixtyDay  = '' THEN '0' ELSE SixtyDay END) AS SixtyDay, "
                                             + " (CASE WHEN NinetyDay IS NULL OR NinetyDay = '' THEN '0' ELSE NinetyDay END) AS NinetyDay, "
                                             + " (CASE WHEN Over90 IS NULL  OR Over90 = '' THEN '0' ELSE Over90 END) AS Over90, FirstVisit_Date, LastVisit_Date,  "
                                             + " Primary_Insurance, Primary_Insurance_CompanyName, Primary_Ins_Subscriber_ID, "
                                             + " Secondary_Insurance, Secondary_Insurance_CompanyName, Secondary_Ins_Subscriber_ID, "
                                             + " Guar_ID, Pri_Provider_ID, Sec_Provider_ID, ReceiveSms, ReceiveEmail, nextvisit_date, due_date,  "
                                             + " (CASE WHEN remaining_benefit IS NULL OR remaining_benefit= ''  THEN '0' ELSE remaining_benefit END) AS remaining_benefit, "
                                             + " (CASE WHEN used_benefit IS NULL OR used_benefit = ''  THEN '0' ELSE used_benefit END) AS used_benefit, "
                                             + " (CASE WHEN collect_payment IS NULL OR collect_payment = ''  THEN '0' ELSE collect_payment END) AS collect_payment,  "
                                             + " EHR_Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id,is_deleted,EHR_Status,ReceiveVoiceCall,Patient_status,Patient_status_Compare,[ssn],[encrypted_social_security],[driverlicense],[groupid],[emergencycontactId],[EmergencyContact_First_Name],[EmergencyContact_Last_Name] ,[emergencycontactnumber],[school] ,[employer] ,[spouseId] ,[responsiblepartyId] ,[responsiblepartyssn],[Respencrypted_social_security],[responsiblepartybirthdate],[Spouse_First_Name] ,[Spouse_Last_Name] ,[ResponsibleParty_First_Name] ,[ResponsibleParty_Last_Name], [Prim_Ins_Company_Phonenumber] ,[Sec_Ins_Company_Phonenumber],Patient_Note,Is_Status_Adit_Updated,PreferredLanguage  FROM Patient where Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalNewPatientData = " SELECT patient_ehr_id,Is_Adit_Updated,Clinic_Number,Service_Install_Id FROM Patient where Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalOpenDentalLanguageList = " SELECT * from OpenDentalLanguageList ";

        public static string GetLocalPatientCompareDeletedData = "Select Patient_EHR_id from PatientCompare where Service_Install_Id = @Service_Install_Id and Is_Deleted = 1";

        public static string GetLocalPatientDataByPatientEHRIds = " SELECT Patient_LocalDB_ID, patient_ehr_id, patient_Web_ID, First_name, Last_name, "
                                            + " Middle_Name, Salutation, preferred_name, Status, Sex, MaritalStatus, Birth_Date, Email, "
                                            + " Mobile, Home_Phone, Work_Phone, Address1, Address2,City, State, Zipcode, ResponsibleParty_Status,  "
                                            + " (CASE WHEN CurrentBal IS NULL OR CurrentBal = '' THEN '0' ELSE CurrentBal END) AS CurrentBal, "
                                            + " (CASE WHEN ThirtyDay IS NULL OR ThirtyDay = '' THEN '0' ELSE ThirtyDay END) AS ThirtyDay, "
                                            + " (CASE WHEN SixtyDay IS NULL OR SixtyDay  = '' THEN '0' ELSE SixtyDay END) AS SixtyDay, "
                                            + " (CASE WHEN NinetyDay IS NULL OR NinetyDay = '' THEN '0' ELSE NinetyDay END) AS NinetyDay, "
                                            + " (CASE WHEN Over90 IS NULL  OR Over90 = '' THEN '0' ELSE Over90 END) AS Over90, FirstVisit_Date, LastVisit_Date,  "
                                            + " Primary_Insurance, Primary_Insurance_CompanyName, Primary_Ins_Subscriber_ID, "
                                            + " Secondary_Insurance, Secondary_Insurance_CompanyName, Secondary_Ins_Subscriber_ID, "
                                            + " Guar_ID, Pri_Provider_ID, Sec_Provider_ID, ReceiveSms, ReceiveEmail, nextvisit_date, due_date,  "
                                            + " (CASE WHEN remaining_benefit IS NULL OR remaining_benefit= ''  THEN '0' ELSE remaining_benefit END) AS remaining_benefit, "
                                            + " (CASE WHEN used_benefit IS NULL OR used_benefit = ''  THEN '0' ELSE used_benefit END) AS used_benefit, "
                                            + " (CASE WHEN collect_payment IS NULL OR collect_payment = ''  THEN '0' ELSE collect_payment END) AS collect_payment,  "
                                            + " EHR_Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id,is_deleted,EHR_Status,ReceiveVoiceCall,Patient_status,Patient_status_Compare,[ssn] ,[driverlicense],[groupid] ,[emergencycontactId] ,[EmergencyContact_First_Name],[EmergencyContact_Last_Name] ,[emergencycontactnumber],[school] ,[employer] ,[spouseId] ,[responsiblepartyId] ,[responsiblepartyssn] ,[responsiblepartybirthdate],[Spouse_First_Name] ,[Spouse_Last_Name] ,[ResponsibleParty_First_Name] ,[ResponsibleParty_Last_Name], [Prim_Ins_Company_Phonenumber] ,[Sec_Ins_Company_Phonenumber]  FROM Patient WHERE Patient_EHR_ID IN (@PatientEHRIDS)  And Service_Install_Id = @Service_Install_Id ";

        //https://app.asana.com/0/1145079499609557/1145305351438548 // Changed reterive query for this task
        //public static string GetPushLocalPatientData = "Select ( CASE WHEN PT.Mobile = '' THEN 'N' ELSE ( CASE WHEN PT1.CNT > 0 THEN 'N' WHEN PT2.CNT > 0 THEN 'Y' ELSE 'Y' END ) END ) AS ReceiveSMSModified ,PT.* FROM Patient PT "
        //                                                + " LEFT JOIN ( SELECT Mobile,COUNT(1) AS CNT FROM Patient WHERE ReceiveSMS= 'N' GROUP BY Mobile ) AS PT1 "
        //                                                + " ON PT.Mobile = PT1.Mobile "
        //                                                + " LEFT JOIN ( SELECT Mobile,COUNT(1) AS CNT FROM Patient WHERE ReceiveSMS= 'Y' GROUP BY Mobile ) AS PT2 "
        //                                                + " ON PT.Mobile = PT2.Mobile WHERE PT.Is_Adit_Updated = 0 AND PT.Patient_EHR_Id <> '' AND PT.First_Name <> '' AND PT.Last_Name <> ''";

        public static string GetPushLocalPatientData = "SELECT * FROM Patient WHERE Is_Adit_Updated = 0";

        public static string GetPushLocalPatientStatusData = "SELECT Patient_EHR_ID,Patient_Web_ID,Patient_Status,Last_Sync_Date,EHR_Entry_DateTime,Patient_LocalDB_ID,Clinic_Number,Service_Install_Id FROM Patient WHERE Is_Status_Adit_Updated = 0 ";

        public static string GetPushLocalPatientStatusData_ClinicWise = "SELECT Patient_EHR_ID,Patient_Web_ID,Patient_Status,Last_Sync_Date,EHR_Entry_DateTime,Patient_LocalDB_ID,Clinic_Number,Service_Install_Id FROM Patient WHERE Is_Status_Adit_Updated = 0 AND Clinic_Number = @Clinic_Number AND Service_Install_Id = @Service_Install_Id";

        public static string GetLocalPatientImagesData = "SELECT * FROM Patient_Images where Service_Install_Id = @Service_Install_Id and Is_Deleted = 0";

        public static string GetAllLocalPatientData = "SELECT Patient_EHR_ID,Patient_Web_ID,Mobile,First_name,Last_name FROM Patient";

        public static string GetAllLocalActivePatientData = "SELECT Patient_EHR_ID,Patient_Web_ID,Mobile,First_name,Last_name FROM Patient Where Status = 'A'";

        public static string PatientCompareQuery = " SELECT ACC.* FROM ( SELECT ( CASE WHEN PT.Patient_EHR_id IS NULL THEN 1 ELSE 2 END ) AS InsUptDlt,  PC.* FROM "
                                                   + "  ( SELECT Distinct Service_Install_Id,[Patient_EHR_ID] "
                                                   + "  FROM (SELECT DISTINCT Service_Install_Id,[Patient_EHR_ID],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                                   + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                                   + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],ReceiveVoiceCall,[PreferredLanguage],Patient_Note ,"
                                                   + "  [ssn] ,[driverlicense],[groupid] ,[emergencycontactId] ,[EmergencyContact_First_Name],[EmergencyContact_Last_Name] ,[emergencycontactnumber],[school] ,[employer] ,[spouseId] ,[responsiblepartyId] ,[responsiblepartyssn] ,[responsiblepartybirthdate],[Spouse_First_Name] ,[Spouse_Last_Name] ,[ResponsibleParty_First_Name] ,[ResponsibleParty_Last_Name], [Prim_Ins_Company_Phonenumber] ,[Sec_Ins_Company_Phonenumber] "
                                                   + "  FROM Patient where Service_Install_Id = @Service_Install_Id UNION All  "
                                                   + "  SELECT DISTINCT Service_Install_Id,[Patient_EHR_ID],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                                   + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                                   + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],ReceiveVoiceCall,[PreferredLanguage],Patient_Note, "
                                                   + "  [ssn] ,[driverlicense],[groupid] ,[emergencycontactId] ,[EmergencyContact_First_Name],[EmergencyContact_Last_Name] ,[emergencycontactnumber],[school] ,[employer] ,[spouseId] ,[responsiblepartyId] ,[responsiblepartyssn] ,[responsiblepartybirthdate],[Spouse_First_Name] ,[Spouse_Last_Name] ,[ResponsibleParty_First_Name] ,[ResponsibleParty_Last_Name], [Prim_Ins_Company_Phonenumber] ,[Sec_Ins_Company_Phonenumber] "
                                                   + "   FROM PatientCompare where Service_Install_Id = @Service_Install_Id ) data "
                                                   + "  GROUP BY  Service_Install_Id,[Patient_EHR_ID],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                                   + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                                   + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],ReceiveVoiceCall,[PreferredLanguage],Patient_Note, "
                                                   + "  [ssn] ,[driverlicense],[groupid] ,[emergencycontactId] ,[EmergencyContact_First_Name],[EmergencyContact_Last_Name] ,[emergencycontactnumber],[school] ,[employer] ,[spouseId] ,[responsiblepartyId] ,[responsiblepartyssn] ,[responsiblepartybirthdate],[Spouse_First_Name] ,[Spouse_Last_Name] ,[ResponsibleParty_First_Name] ,[ResponsibleParty_Last_Name], [Prim_Ins_Company_Phonenumber] ,[Sec_Ins_Company_Phonenumber] "
                                                   + "  HAVING count(1) = 1 ) AS RS "
                                                   + "  LEFT JOIN Patient PT ON Rs.Service_Install_Id = PT.Service_Install_Id And RS.Patient_EHR_Id = PT.Patient_EHR_Id "
                                                   + "  LEFT JOIN PatientCompare PC ON Pc.Service_Install_Id = Rs.Service_Install_Id And PC.Patient_EHR_Id = RS.Patient_EHR_Id  ) AS ACC WHERE Patient_EHR_id != '' And  Service_Install_Id = @Service_Install_Id ";

        public static string Insert_ApptPatient = " INSERT INTO Patient "
                                                    + "(patient_ehr_id, "
                                                    + " First_name, "
                                                    + " Last_name, "
                                                    + " Middle_Name, "
                                                    + " Status, "
                                                    + " Email, "
                                                    + " Mobile, "
                                                    + " Home_Phone, "
                                                    + " LastVisit_Date, "
                                                    + " ReceiveSms, "
                                                    + " ReceiveEmail, "
                                                    + " nextvisit_date, "
                                                    + " due_date, "
                                                    + " Clinic_Number,Service_Install_Id,[EHR_Status],[ReceiveVoiceCall],Is_Adit_Updated,EHR_Entry_DateTime) "
                                                    + " VALUES "
                                                    + "(@patient_ehr_id, "
                                                    + " @First_name, "
                                                    + " @Last_name, "
                                                    + " @Middle_Name, "
                                                    + " @Status, "
                                                    + " @Email, "
                                                    + " @Mobile, "
                                                    + " @Home_Phone, "
                                                    + " @LastVisit_Date, "
                                                    + " @ReceiveSms, "
                                                    + " @ReceiveEmail, "
                                                    + " @nextvisit_date, "
                                                    + " @due_date, "
                                                    + " @Clinic_Number,@Service_Install_Id,@EHR_Status,@ReceiveVoiceCall,0,@EHR_Entry_DateTime) ";

        public static string Update_ApptPatient = " UPDATE Patient SET "
                                                       + " First_name = @First_name , "
                                                       + " Last_name = @Last_name, "
                                                       + " Middle_Name = @Middle_Name, "
                                                       + " Status = @Status, "
                                                       + " Email = @Email, "
                                                       + " Mobile = @Mobile, "
                                                       + " Home_Phone = @Home_Phone, "
                                                       + " LastVisit_Date = @LastVisit_Date, "
                                                       + " ReceiveSms= @ReceiveSms, "
                                                       + " ReceiveEmail = @ReceiveEmail, "
                                                       + " nextvisit_date = @nextvisit_date, "
                                                       + " due_date = @due_date, "
                                                       + " Clinic_Number = @Clinic_Number, "
                                                       + " [EHR_Status] = @EHR_Status, "
                                                       + " [ReceiveVoiceCall] = @ReceiveVoiceCall, "
                                                       + " Is_Adit_Updated = 0,EHR_Entry_DateTime=@EHR_Entry_DateTime "
                                                       + " WHERE patient_ehr_id = @patient_ehr_id and Service_Install_Id = @Service_Install_Id  ";

        public static string Insert_Patient = " INSERT INTO Patient "
                                                       + "(patient_ehr_id, "
                                                       + " patient_Web_ID, "
                                                       + " First_name, "
                                                       + " Last_name, "
                                                       + " Middle_Name, "
                                                       + " Salutation, "
                                                       + " preferred_name, "
                                                       + " Status, "
                                                       + " Sex, "
                                                       + " MaritalStatus, "
                                                       + " Birth_Date, "
                                                       + " Email, "
                                                       + " Mobile, "
                                                       + " Home_Phone, "
                                                       + " Work_Phone, "
                                                       + " Address1, "
                                                       + " Address2, "
                                                       + " City, "
                                                       + " State, "
                                                       + " Zipcode, "
                                                       + " ResponsibleParty_Status, "
                                                       + " CurrentBal, "
                                                       + " ThirtyDay, "
                                                       + " SixtyDay, "
                                                       + " NinetyDay, "
                                                       + " Over90, "
                                                       + " FirstVisit_Date, "
                                                       + " LastVisit_Date, "
                                                       + " Primary_Insurance, "
                                                       + " Primary_Insurance_CompanyName, "
                                                       + " Secondary_Insurance, "
                                                       + " Secondary_Insurance_CompanyName, "
                                                       + " Guar_ID, "
                                                       + " Pri_Provider_ID, "
                                                       + " Sec_Provider_ID, "
                                                       + " ReceiveSms, "
                                                       + " ReceiveEmail, "
                                                       + " nextvisit_date, "
                                                       + " due_date, "
                                                       + " remaining_benefit, "
                                                       + " used_benefit, "
                                                       + " collect_payment, "
                                                       + " EHR_Entry_DateTime, "
                                                       + " Last_Sync_Date, "
                                                       + " Is_Adit_Updated ,"
                                                       + " Secondary_Ins_Subscriber_ID, "
                                                       + " Primary_Ins_Subscriber_ID,Clinic_Number,Service_Install_Id,EHR_Status,ReceiveVoiceCall,PreferredLanguage,Patient_Note,Is_Deleted, "
                                                       + " ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber ) "
                                                       + " VALUES "
                                                       + "(@patient_ehr_id, "
                                                       + " @patient_Web_ID, "
                                                       + " @First_name, "
                                                       + " @Last_name, "
                                                       + " @Middle_Name, "
                                                       + " @Salutation, "
                                                       + " @preferred_name, "
                                                       + " @Status, "
                                                       + " @Sex, "
                                                       + " @MaritalStatus,  "
                                                       + " @Birth_Date, "
                                                       + " @Email, "
                                                       + " @Mobile, "
                                                       + " @Home_Phone, "
                                                       + " @Work_Phone, "
                                                       + " @Address1, "
                                                       + " @Address2, "
                                                       + " @City, "
                                                       + " @State, "
                                                       + " @Zipcode, "
                                                       + " @ResponsibleParty_Status, "
                                                       + " @CurrentBal, "
                                                       + " @ThirtyDay, "
                                                       + " @SixtyDay, "
                                                       + " @NinetyDay, "
                                                       + " @Over90, "
                                                       + " @FirstVisit_Date, "
                                                       + " @LastVisit_Date, "
                                                       + " @Primary_Insurance, "
                                                       + " @Primary_Insurance_CompanyName, "
                                                       + " @Secondary_Insurance, "
                                                       + " @Secondary_Insurance_CompanyName, "
                                                       + " @Guar_ID, "
                                                       + " @Pri_Provider_ID, "
                                                       + " @Sec_Provider_ID, "
                                                       + " @ReceiveSms, "
                                                       + " @ReceiveEmail, "
                                                       + " @nextvisit_date, "
                                                       + " @due_date, "
                                                       + " @remaining_benefit, "
                                                       + " @used_benefit, "
                                                       + " @collect_payment, "
                                                       + " @EHR_Entry_DateTime, "
                                                       + " @Last_Sync_Date,"
                                                       + " @Is_Adit_Updated ,"
                                                       + " @Secondary_Ins_Subscriber_ID, "
                                                       + " @Primary_Ins_Subscriber_ID,@Clinic_Number,@Service_Install_Id,@EHR_Status,@ReceiveVoiceCall,@PreferredLanguage,@Patient_Note,@Is_Deleted, "
                                                       + " @ssn,@driverlicense,@groupid ,@emergencycontactId ,@EmergencyContact_First_Name,@EmergencyContact_Last_Name  ,@emergencycontactnumber,@school ,@employer ,@spouseId ,@responsiblepartyId ,@responsiblepartyssn ,@responsiblepartybirthdate,@Spouse_First_Name ,@Spouse_Last_Name ,@ResponsibleParty_First_Name ,@ResponsibleParty_Last_Name,@Prim_Ins_Company_Phonenumber ,@Sec_Ins_Company_Phonenumber ) ";

        public static string Update_Patient = " UPDATE Patient SET "
                                                       + " First_name = @First_name , "
                                                       + " Last_name = @Last_name, "
                                                       + " Middle_Name = @Middle_Name, "
                                                       + " Salutation = @Salutation, "
                                                       + " preferred_name = @preferred_name, "
                                                       + " Status = @Status, "
                                                       + " Sex = @Sex, "
                                                       + " MaritalStatus = @MaritalStatus, "
                                                       + " Birth_Date = @Birth_Date, "
                                                       + " Email = @Email, "
                                                       + " Mobile = @Mobile, "
                                                       + " Home_Phone = @Home_Phone, "
                                                       + " Work_Phone= @Work_Phone, "
                                                       + " Address1 = @Address1, "
                                                       + " Address2 = @Address2, "
                                                       + " City = @City, "
                                                       + " State = @State, "
                                                       + " Zipcode= @Zipcode, "
                                                       + " ResponsibleParty_Status = @ResponsibleParty_Status, "
                                                       + " CurrentBal = @CurrentBal, "
                                                       + " ThirtyDay= @ThirtyDay, "
                                                       + " SixtyDay = @SixtyDay, "
                                                       + " NinetyDay = @NinetyDay, "
                                                       + " Over90 = @Over90, "
                                                       + " FirstVisit_Date = @FirstVisit_Date, "
                                                       + " LastVisit_Date = @LastVisit_Date, "
                                                       + " Primary_Insurance= @Primary_Insurance,"
                                                       + " Primary_Insurance_CompanyName= @Primary_Insurance_CompanyName, "
                                                       + " Secondary_Insurance = @Secondary_Insurance, "
                                                       + " Secondary_Insurance_CompanyName= @Secondary_Insurance_CompanyName, "
                                                       + " Guar_ID= @Guar_ID, "
                                                       + " Pri_Provider_ID = @Pri_Provider_ID, "
                                                       + " Sec_Provider_ID= @Sec_Provider_ID, "
                                                       + " ReceiveSms= @ReceiveSms, "
                                                       + " ReceiveEmail = @ReceiveEmail, "
                                                       + " nextvisit_date = @nextvisit_date, "
                                                       + " due_date = @due_date, "
                                                       + " remaining_benefit = @remaining_benefit, "
                                                       + " used_benefit = @used_benefit, "
                                                       + " collect_payment = @collect_payment, "
                                                       + " EHR_Entry_DateTime = @EHR_Entry_DateTime,"
                                                       + " Is_Adit_Updated = @Is_Adit_Updated, "
                                                       + " EHR_Status = @EHR_Status, "
                                                       + " Secondary_Ins_Subscriber_ID  = @Secondary_Ins_Subscriber_ID, "
                                                       + " Primary_Ins_Subscriber_ID = @Primary_Ins_Subscriber_ID, "
                                                       + " ReceiveVoiceCall = @ReceiveVoiceCall,PreferredLanguage=@PreferredLanguage,Patient_Note = @Patient_Note,Is_Deleted = @Is_Deleted, "
                                                       + " ssn = @ssn,driverlicense = @driverlicense,groupid = @groupid ,emergencycontactId = @emergencycontactId ,EmergencyContact_First_Name = @EmergencyContact_First_Name,EmergencyContact_Last_Name = @EmergencyContact_Last_Name , "
                                                       + " emergencycontactnumber = @emergencycontactnumber,school = @school ,employer = @employer ,spouseId = @spouseId ,responsiblepartyId = @responsiblepartyId , "
                                                       + " responsiblepartyssn = @responsiblepartyssn ,responsiblepartybirthdate = @responsiblepartybirthdate,Spouse_First_Name = @Spouse_First_Name , "
                                                       + " Spouse_Last_Name = @Spouse_Last_Name ,ResponsibleParty_First_Name = @ResponsibleParty_First_Name , ResponsibleParty_Last_Name = @ResponsibleParty_Last_Name, "
                                                       + " Prim_Ins_Company_Phonenumber = @Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber = @Sec_Ins_Company_Phonenumber "
                                                       + " WHERE patient_ehr_id = @patient_ehr_id  And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Patient_Pull = " UPDATE Patient SET patient_Web_ID = @patient_Web_ID, fullname = @fullname, email = @email, mobile = @mobile, phone = @phone, "
                                                       + " birth_date = @birth_date, last_visit = @last_visit, next_visit = @next_visit, revenue = @revenue "
                                                       + " WHERE patient_ehr_id = @patient_ehr_id And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_Patient = " Delete From Patient WHERE Patient_EHR_ID = @Patient_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_Patient_By_PatientEHRIDs = "Update Patient Set is_deleted = 1, Is_Adit_Updated = 0, Status = 'I'  Where Patient_EHR_ID In (@PatientEHRIDs) and is_deleted = 0 ";

        public static string GetLocalUser_EHR_Id = "SELECT User_EHR_ID from location where Clinic_Number=@Clinic_Number and Service_Install_Id=@Service_Install_Id";

        public static string Update_Patient_Web_Id = "UPDATE Patient SET Patient_Web_ID = @Web_ID , Is_Adit_Updated = 1  WHERE Patient_EHR_ID = @EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Patient_Status_Web_Id = "UPDATE Patient SET Patient_Web_ID = @Web_ID , Is_Status_Adit_Updated = 1  WHERE Patient_EHR_ID = @EHR_ID And Service_Install_Id = @Service_Install_Id ";

        #endregion



        #region Patient Form

        public static string GetLocalNewWebPatient_FormData = " Select * From Patient_Form Where Is_EHR_Updated = 0 AND Is_Adit_Importing = 0 AND Is_Adit_Updated = 0  And Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalWebPatientPaymentData = "Select pp.*,p.Guar_ID,p.Pri_Provider_ID,p.Sec_Provider_ID From PatientPayment pp inner join patient p on p.Patient_EHR_ID = pp.PatientEHRId Where PaymentUpdatedEHR = 0 AND PaymentReceivedLocal = 1 AND PaymentStatusCompletedAdit = 0  AND TryInsert !>  3 AND pp.Service_Install_Id = @Service_Install_Id and PatientEHRId <> ''";

        public static string GetPatientPaymentTableBlankStructure = " SElect * FROM PatientPayment where 1=2 ";

        public static string GetLocalWebPatientPaymentSplitData = "Select pp.PatientEHRId,ppp.ProviderEHRId,ppp.Amount From PatientPaymentProvider ppp LEFT JOIN (PatientPayment pp INNER JOIN patient p on p.Patient_EHR_ID = pp.PatientEHRId) ON pp.PatientEHRId = ppp.PatientEHRID Where PaymentUpdatedEHR = 0 AND PaymentReceivedLocal = 1 AND PaymentStatusCompletedAdit = 0  And ppp.Service_Install_Id =@Service_Install_Id  and ppp.PatientEHRId <> ''";

        public static string GetLocalWebPatientPaymentDataErroAPI = " Select * From PatientPayment Where PaymentUpdatedEHR = 0 AND PaymentReceivedLocal = 1 AND PaymentStatusCompletedAdit = 0  And Service_Install_Id = @Service_Install_Id AND ErrorMessage <> ''";

        public static string GetLocalWebPatientSMSCallDataErroAPI = " Select * From PatientSMSCallLog Where LogUpdatedEHR = 0 AND LogReceivedLocal = 1 AND LogStatusCompletedAdit = 0  And Service_Install_Id = @Service_Install_Id AND ErrorMessage <> ''";

        public static string GetLocalWebPatientSMSCallLogData = "  Select PSC.*,PT.Mobile From PatientSMSCallLog PSC inner join patient PT on PSC.PatientEHRId = PT.Patient_EHR_id Where PSC.LogUpdatedEHR = 0 AND PSC.LogReceivedLocal = 1 AND PSC.LogStatusCompletedAdit = 0  And PSC.Service_Install_Id =  @Service_Install_Id ";

        public static string GetLocalPatientForm_Importing_PendingData = " Select PatientForm_Web_ID From Patient_Form Where Is_EHR_Updated = 1 AND Is_Adit_Updated = 0 AND Is_Adit_Importing = 0 And Service_Install_Id = @Service_Install_Id AND PatientType = 0 ";

        public static string GetLocalPatientForm_completed_PendingData = " Select PatientForm_Web_ID From Patient_Form Where Is_Adit_Updated = 0 AND Is_Adit_Importing = 1 And Service_Install_Id = @Service_Install_Id AND PatientType = 0 ";

        public static string GetLocalPatientPortal_completed_PendingData = " Select DISTINCT PatientForm_Web_ID,Patient_Web_Id,Service_Install_Id From Patient_Form Where IS_EHR_Updated = 1 AND Is_Adit_Updated = 0 And Service_Install_Id = @Service_Install_Id AND PatientType = 1 ";

        public static string GetLocalPatientFormData = " SELECT * From Patient_Form where Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormDiseaseResponse = " SELECT * From DiseaseResponse where Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormDiseaseDeleteResponse = " SELECT * From DiseaseDeleteResponse where Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormDiseaseDeleteResponse_Clinic = " SELECT * From DiseaseDeleteResponse where (Clinic_Number = @Clinic_Number) And Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormDiseaseResponse_Clinic = " SELECT * From DiseaseResponse where (Clinic_Number = @Clinic_Number) And Service_Install_Id = @Service_Install_Id  ";

        public static string GetLocalPatientFormDiseaseDeleteResponseToSaveINEHR = " SELECT * From DiseaseDeleteResponse DR WHERE DR.Is_EHR_Updated = 0 And DR.Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormDiseaseResponseToSaveINEHR = " SELECT DISTINCT case when PF.Patient_EHR_ID != '' then PF.Patient_EHR_ID else DR.Patient_ehr_id end AS PatientEHRId , DR.* From DiseaseResponse DR INNER JOIN Patient_Form PF ON PF.Service_Install_Id = DR.Service_Install_Id AND DR.PatientForm_Web_ID = PF.PatientForm_Web_ID WHERE DR.Is_EHR_Updated = 0 And DR.Service_Install_Id = @Service_Install_Id ";
               
        public static string GetLocalPendingPatientFormData = " SELECT * From Patient_Form where Is_EHR_Updated = 1 and Is_Pdf_Created = 0  And Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPendingPatientFormDocAttachmentData = " SELECT * From Patient_Form where Is_EHR_Updated = 1 and Is_AttachDocFound = 0  And Service_Install_Id = @Service_Install_Id ";
        
        public static string GetLocalPendingPatientFormMedicalHistory = " SELECT * From Patient_Form where Is_EHR_Updated = 1 and Is_MedicalHistoryFetched = 0 And Service_Install_Id = @Service_Install_Id ";

        public static string GetMedicalHistoryPatientWithForm = "Select DISTINCT PF.PatientForm_Web_id, PF.Patient_EHR_id,EMQ.FormMaster_EHR_id,0 AS FormInstanceId from Patient_Form PF Inner JOIN Eaglesoft_MHF_Question_submit EMQ ON PF.patientform_web_id = EMQ.PatientForm_web_id WHERE EMQ.IS_EHR_Updated = 0 AND PF.MedicalHistoryField = 0 AND PF.Is_MedicalHistoryFetched = 1 And PF.Service_Install_Id = @Service_Install_Id ";

        //public static string GetLocalPatientFormDocData = " SELECT * From Patient_Document where Service_Install_Id = @Service_Install_Id ";
        public static string GetLocalPatientFormDocData = " SELECT PD.PatientDoc_Name, PD.PatientDoc_Web_ID,PD.PatientDoc_EHR_ID, PD.Patient_EHR_ID,PD.folder_ehr_id,PD.Patient_Web_ID, PD.Service_Install_Id, PD.Clinic_Number ,PD.PatientDoc_Name,PF.folder_ehr_id,PF.folder_name,PF.DocNameFormat,PF.Form_Name,PF.Patient_Name,PF.submit_time,PF.Entry_DateTime " +
            "From Patient_Document as PD join Patient_form PF on PF.PatientForm_Web_ID = PD.PatientDoc_Web_ID " +
            "where PF.Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormDocAttachmentData = " SELECT distinct PD.PatientDoc_Name, PD.PatientDoc_Web_ID,PD.PatientDoc_EHR_ID, PD.Patient_EHR_ID,PD.folder_ehr_id,PD.Patient_Web_ID, PD.Service_Install_Id, PD.Clinic_Number ,PD.PatientDoc_Name,PF.folder_ehr_id,PF.folder_name,PD.DocNameFormat,PD.Form_Name,PF.Patient_Name,convert(nvarchar(10),PF.submit_time,101) as submit_time,PF.Entry_DateTime,PD.PatientForm_web_Id,PD.DocType,PD.Web_DocName,PD.Is_EHR_Updated " +
            "From PatientFormDocAttachment as PD join Patient_form PF on PF.PatientForm_Web_ID = PD.PatientForm_Web_ID " +
            "where PF.Service_Install_Id = @Service_Install_Id ";
        

        public static string GetLocalPatientFormMedicalHistory = " SELECT * From Medical_History where Service_Install_Id = @Service_Install_Id ";

        //public static string GetLivePatientFormDocData = " SELECT * From Patient_Document where Is_EHR_Updated = 0 And Service_Install_Id = @Service_Install_Id ";

        public static string GetLivePatientFormDocDataSynced = " SELECT * From Patient_Document where Is_EHR_Updated = 0 And Service_Install_Id = @Service_Install_Id And PatientDoc_Web_ID = @PatientDoc_Web_ID";

        public static string GetLivePatientFormDocAttachmentDataSynced = " SELECT * From PatientFormDocAttachment where Is_EHR_Updated = 0 And Service_Install_Id = @Service_Install_Id And PatientDoc_Web_ID = @PatientDoc_Web_ID";

         public static string GetLivePatientFormDocData = " SELECT PD.PatientDoc_Name, PD.PatientDoc_Web_ID, PD.Patient_EHR_ID,PD.folder_ehr_id,PD.PatientDoc_Name,PF.folder_ehr_id,PF.folder_name,PF.DocNameFormat,PF.Form_Name,PF.Patient_Name,PF.submit_time " +
            "From Patient_Document as PD join Patient_form PF on PF.PatientForm_Web_ID = PD.PatientDoc_Web_ID " +
            "where PD.Is_EHR_Updated = 0 And PD.Service_Install_Id = @Service_Install_Id  ";

        public static string GetLivePatientFormDocAttachmentData = " SELECT distinct [PatientDoc_LocalDB_ID], PD.PatientDoc_Name, PD.PatientDoc_Web_ID, PD.Patient_EHR_ID,PD.folder_ehr_id,PD.PatientDoc_Name,PF.folder_ehr_id,PF.folder_name,PD.DocNameFormat,PD.Form_Name,PF.Patient_Name,convert(nvarchar(10),PF.submit_time,101) as submit_time,PD.PatientForm_web_Id,PD.DocType,PD.Web_DocName " +
            "From PatientFormDocAttachment as PD join Patient_form PF on PF.PatientForm_Web_ID = PD.PatientForm_Web_ID " +
            "where PD.Is_EHR_Updated = 0 And PD.Service_Install_Id = @Service_Install_Id and PF.PatientForm_Web_ID = @PatientForm_Web_ID order by PatientDoc_LocalDB_ID ";

        //public static string GetLiveDentrixPatientFormDocData = " SELECT * From Patient_Document where Entry_DateTime >= @ToDate and Is_EHR_Updated = 0 And Service_Install_Id = @Service_Install_Id ";

        public static string GetLiveDentrixPatientFormDocData = " SELECT distinct PD.PatientDoc_Name, PD.PatientDoc_Web_ID, PD.Patient_EHR_ID,PD.folder_ehr_id,PD.PatientDoc_Name,PF.folder_ehr_id,PF.folder_name,PF.DocNameFormat,PF.Form_Name,PF.Patient_Name,PF.submit_time " +
            "From Patient_Document as PD join Patient_form PF on PF.PatientForm_Web_ID = PD.PatientDoc_Web_ID " +
            " where PF.Entry_DateTime >= @ToDate and PD.Is_EHR_Updated = 0 And PD.Service_Install_Id = @Service_Install_Id ";

        public static string GetLiveDentrixPatientFormDocAttachmentData = " SELECT distinct [PatientDoc_LocalDB_ID],PD.PatientDoc_Name, PD.PatientDoc_Web_ID, PD.Patient_EHR_ID,PD.folder_ehr_id,PD.PatientDoc_Name,PF.folder_ehr_id,PF.folder_name,PD.DocNameFormat,PD.Form_Name,PF.Patient_Name,convert(nvarchar(10),PF.submit_time,101) as submit_time,PD.PatientForm_web_Id,PD.DocType,PD.Web_DocName  " +
            "From PatientFormDocAttachment as PD join Patient_form PF on PF.PatientForm_Web_ID = PD.PatientForm_Web_ID " +
            " where PF.Entry_DateTime >= @ToDate and PD.Is_EHR_Updated = 0 And PD.Service_Install_Id = @Service_Install_Id and PF.PatientForm_Web_ID = @PatientForm_Web_ID order by PatientDoc_LocalDB_ID";

        public static string GetLiveDentrixPatientFormMedicalHistoryData = "SELECT * From Dentrix_Response where Is_EHR_Updated = 0 ";

        public static string GetLiveAbelDentPatientFormMedicalHistoryData = "SELECT * From AbelDent_Response where Is_EHR_Updated = 0 ";

        public static string GetLiveEasyDentalPatientFormMedicalHistoryData = "SELECT * From EasyDental_Response where Is_EHR_Updated = 0";

        public static string GetLiveCleardentPatientFormMedicalHistoryData = "SELECT * From CD_Response where Is_EHR_Updated = 0";

        public static string GetLiveOpenDentalPatientFormMedicalHistoryData = "SELECT * From OD_Response where Is_EHR_Updated = 0 and Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Insert_Patient_Form = " INSERT INTO Patient_Form "
                                                      + "(PatientForm_Web_ID, "
                                                      + " Patient_EHR_ID, "
                                                      + " Patient_Web_ID, "
                                                      + " ehrfield, "
                                                      + " ehrfield_value, "
                                                      + " Entry_DateTime, "
                                                      + " folder_ehr_id, "
                                                      + " folder_name, "
                                                      + " DocNameFormat, "
                                                      //+ " @Form_Name, "
                                                      //+ " @Patient_Name, "
                                                      + " submit_time, "
                                                      + " Is_Adit_Updated,Clinic_Number,Service_Install_Id,PatientType,Patient_Name,Form_Name) "//,Submit_Time
                                                      + " VALUES "
                                                      + "(@PatientForm_Web_ID, "
                                                      + " @Patient_EHR_ID, "
                                                      + " @Patient_Web_ID, "
                                                      + " @ehrfield,"
                                                      + " @ehrfield_value, "
                                                      + " @Entry_DateTime, "
                                                      + " @folder_ehr_id, "
                                                      + " @folder_name, "
                                                      + " @DocNameFormat, "
                                                      //+ " @Form_Name, "
                                                      //+ " @Patient_Name, "
                                                      + " @submit_time, "
                                                      + " @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id,@PatientType,@Patient_Name,@Form_Name) ";//,@Submit_Time
        public static string Insert_Patient_FormDoc = " INSERT INTO Patient_Document "
                                                    + "(PatientDoc_Web_ID, "
                                                    + " PatientDoc_EHR_ID, "
                                                    + " Patient_EHR_ID, "
                                                    + " Patient_Web_ID, "
                                                    + " PatientDoc_Name, "
                                                    + " Entry_DateTime, "
                                                    + " Is_EHR_Updated, "
                                                    + " folder_name, "
                                                    + " DocNameFormat, "
                                                    + " folder_ehr_id, "
                                                    + " Is_Adit_Updated,Clinic_Number,Service_Install_Id,Patient_Name,Form_Name) "
                                                    + " VALUES "
                                                    + "(@PatientDoc_Web_ID, "
                                                    + " @PatientDoc_EHR_ID, "
                                                    + " @Patient_EHR_ID, "
                                                    + " @Patient_Web_ID, "
                                                    + " @PatientDoc_Name,"
                                                    + " @Entry_DateTime, "
                                                    + " @Is_EHR_Updated, "
                                                    + " @folder_name, "
                                                    + " @DocNameFormat, "
                                                    + " @folder_ehr_id, "
                                                    + " @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id,@Patient_Name,@Form_Name) ";
        public static string Insert_Patient_FormDocAttachment = " INSERT INTO PatientFormDocAttachment "
                                                + "(PatientDoc_Web_ID, "
                                                + " PatientDoc_EHR_ID, "
                                                + " Patient_EHR_ID, "
                                                + " Patient_Web_ID, "
                                                + " PatientDoc_Name, "
                                                + " Entry_DateTime, "
                                                + " Is_EHR_Updated, "
                                                + " folder_name, "
                                                + " DocNameFormat, "
                                                + " folder_ehr_id, "
                                                + " Is_Adit_Updated,Clinic_Number,Service_Install_Id,Patient_Name,Form_Name,PatientForm_web_Id,DocType,Web_DocName) "
                                                + " VALUES "
                                                + "(@PatientDoc_Web_ID, "
                                                + " @PatientDoc_EHR_ID, "
                                                + " @Patient_EHR_ID, "
                                                + " @Patient_Web_ID, "
                                                + " @PatientDoc_Name,"
                                                + " @Entry_DateTime, "
                                                + " @Is_EHR_Updated, "
                                                + " @folder_name, "
                                                + " @DocNameFormat, "
                                                + " @folder_ehr_id, "
                                                + " @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id,@Patient_Name,@Form_Name,@PatientForm_web_Id,@DocType,@Web_DocName) ";
        
        public static string Update_Patient_Form_Pull = " UPDATE Patient_Form set ehrfield_value =@ehrfield_value "
                                                      + " WHERE PatientForm_Web_id =@PatientForm_Web_id AND ehrfield = @ehrfield And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Patient_Portal_Pull = " UPDATE Patient_Form set ehrfield_value =@ehrfield_value,is_EHR_updated = 0,is_adit_updated = 0 "
                                                      + " WHERE PatientForm_Web_id =@PatientForm_Web_id AND ehrfield = @ehrfield And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_Patient_Form = "";

        public static string Update_PatientForm_EHR_Update_Flg = "UPDATE Patient_Form SET Is_EHR_Updated = 1 WHERE PatientForm_Web_id = @PatientForm_Web_id And Service_Install_Id = @Service_Install_Id ";

        public static string Update_PatientPortal_EHR_Update_Flg = "UPDATE Patient_Form SET Is_Adit_Updated = 1 WHERE IS_EHR_updated = 1 AND PatientForm_Web_id = @PatientForm_Web_id And Service_Install_Id = @Service_Install_Id ";

        public static string Update_PatientForm_MedicalHistory_Field = "UPDATE Patient_Form SET Is_MedicalHistoryFetched = 1,MedicalHistorySubmission_Id = @MedicalHistorySubmission_Id WHERE PatientForm_Web_id = @PatientForm_Web_id And Service_Install_Id = @Service_Install_Id and PatientType = 0 ";

        public static string Update_PatientForm_MedicalHistory_Field_Pushed = "UPDATE Patient_Form SET MedicalHistoryField = 1 WHERE PatientForm_Web_id = @PatientForm_Web_id And Service_Install_Id = @Service_Install_Id and PatientType = 0 ";

        public static string Update_PatientForm_Sync_Flg = "UPDATE Patient_Form SET Is_Adit_Updated = 1 WHERE Is_Adit_Importing = 1 AND Is_Adit_Updated = 0 And Service_Install_Id = @Service_Install_Id and PatientType = 0 ";

        public static string Update_PatientForm_Importing_Flg = "UPDATE Patient_Form SET Is_Adit_Importing = 1 WHERE Is_EHR_Updated = 1 AND Is_Adit_Importing = 0 And Service_Install_Id = @Service_Install_Id  and PatientType = 0 ";

        public static string Update_PatientFormDoc_Local_To_EHR = "UPDATE Patient_Document set Is_EHR_Updated = 1 , PatientDoc_EHR_ID = @PatientDoc_EHR_ID WHERE PatientDoc_Web_ID =@PatientDoc_Web_ID And Service_Install_Id = @Service_Install_Id  ";

        public static string Update_PatientFormDocAttachment_Local_To_EHR = "UPDATE PatientFormDocAttachment set Is_EHR_Updated = 1 , PatientDoc_EHR_ID = @PatientDoc_EHR_ID WHERE PatientDoc_Web_ID =@PatientDoc_Web_ID And Service_Install_Id = @Service_Install_Id  ";

        public static string Update_PatientFormDoc_Live_To_Local = "UPDATE Patient_Form set Is_Pdf_Created = 1 WHERE PatientForm_Web_id =@PatientForm_Web_id And Service_Install_Id = @Service_Install_Id and PatientType = 0 ";

        public static string Update_PatientFormDocAttachment_Live_To_Local = "UPDATE Patient_Form set Is_AttachDocFound = 1 WHERE PatientForm_Web_id =@PatientForm_Web_id And Service_Install_Id = @Service_Install_Id and PatientType = 0 ";
                
        public static string Update_PatientDocNotFound_Live_To_Local = "UPDATE Patient_Form set Is_Pdf_Created = 0 WHERE PatientForm_Web_id =@PatientForm_Web_id And Service_Install_Id = @Service_Install_Id and PatientType = 0 ";

        public static string Update_PatientDocAttachmentNotFound_Live_To_Local = "UPDATE Patient_Form set Is_AttachDocFound = 0 WHERE PatientForm_Web_id =@PatientForm_Web_id And Service_Install_Id = @Service_Install_Id and PatientType = 0 ";

        public static string Update_Disease_EHR_Update_Flg = "UPDATE DiseaseResponse SET Is_EHR_Updated = 1,Allergy_Patient_EHR_Id = @Allergy_Patient_EHR_Id,Patient_EHR_ID = @Patient_EHR_ID WHERE PatientForm_Web_ID = @PatientForm_Web_ID And Disease_EHR_Id = @Disease_EHR_Id and Service_Install_Id = @Service_Install_Id ";

        public static string Update_DeleteDisease_EHR_Update_Flg = "UPDATE DiseaseDeleteResponse SET Is_EHR_Updated = 1 where Patient_EHR_ID = @Patient_EHR_ID and Disease_EHR_Id = @Disease_EHR_Id And Service_Install_Id = @Service_Install_Id ";


        #endregion

        #region Disease
        public static string GetLocalPatientDiseaseData = "SELECT * FROM PatientDiseaseMaster where Service_Install_Id = @Service_Install_Id ";

        public static string GetProcedureDesc = "select ProcedureDesc from Appointment where appt_treatmentcode=@treatcode And Appt_DateTime=@ApptDate AND Service_Install_Id=@service_install_id";

        public static string Insert_PatientDisease = " INSERT INTO PatientDiseaseMaster (Patient_EHR_ID,Disease_EHR_ID, PatientDisease_Web_ID,  Disease_Name, "
                                            + " Disease_Type, is_deleted, EHR_Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id) "
                                            + " VALUES (@Patient_EHR_ID,@Disease_EHR_ID, @Disease_Web_ID, @Disease_Name, "
                                            + " @Disease_Type, @is_deleted, @EHR_Entry_DateTime , @Last_Sync_Date, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id) ";

        public static string Update_PatientDisease = " UPDATE PatientDiseaseMaster SET Patient_EHR_ID = @Patient_EHR_ID,Disease_Name = @Disease_Name,is_deleted = @is_deleted, "
                                             + " EHR_Entry_DateTime = @EHR_Entry_DateTime, Last_Sync_Date = @Last_Sync_Date, Is_Adit_Updated = @Is_Adit_Updated "
                                             + " WHERE Patient_EHR_ID = @Patient_EHR_ID and Disease_EHR_ID = @Disease_EHR_ID AND Disease_Type = @Disease_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_PatientDisease = " UPDATE PatientDiseaseMaster SET is_deleted = 1 , Is_Adit_Updated = 0 WHERE Patient_EHR_ID = @Patient_EHR_ID and is_deleted = 0 and Disease_EHR_ID = @Disease_EHR_ID  AND Disease_Type = @Disease_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";
        public static string GetPushLocalPatientDiseaseData = "SELECT * FROM PatientDiseaseMaster WHERE Is_Adit_Updated = 0";

        public static string Update_PatientDisease_Web_Id = " UPDATE PatientDiseaseMaster SET PatientDisease_Web_ID = @Web_ID, Is_Adit_Updated = 1 "
                                             + " WHERE Patient_EHR_ID = @Patient_EHR_ID and Disease_EHR_ID = @EHR_ID AND Disease_Type = @Disease_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";


        public static string GetLocalDiseaseData = "SELECT * FROM DiseaseMaster where Service_Install_Id = @Service_Install_Id ";

        public static string GetPushLocalDiseaseData = "SELECT * FROM DiseaseMaster WHERE Is_Adit_Updated = 0";

        public static string Insert_Disease = " INSERT INTO DiseaseMaster (Disease_EHR_ID, Disease_Web_ID,  Disease_Name, "
                                             + " Disease_Type, is_deleted, EHR_Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id) "
                                             + " VALUES (@Disease_EHR_ID, @Disease_Web_ID, @Disease_Name, "
                                             + " @Disease_Type, @is_deleted, @EHR_Entry_DateTime , @Last_Sync_Date, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id) ";

        public static string Update_Disease = " UPDATE DiseaseMaster SET Disease_Name = @Disease_Name,is_deleted = @is_deleted, "
                                             + " EHR_Entry_DateTime = @EHR_Entry_DateTime, Last_Sync_Date = @Last_Sync_Date, Is_Adit_Updated = @Is_Adit_Updated "
                                             + " WHERE Disease_EHR_ID = @Disease_EHR_ID AND Disease_Type = @Disease_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_Disease = " UPDATE DiseaseMaster SET is_deleted = 1 , Is_Adit_Updated = 0 WHERE is_deleted = 0 and Disease_EHR_ID = @Disease_EHR_ID  AND Disease_Type = @Disease_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Disease_Web_Id = " UPDATE DiseaseMaster SET Disease_Web_ID = @Web_ID, Is_Adit_Updated = 1 "
                                                   + " WHERE Disease_EHR_ID = @EHR_ID AND Disease_Type = @Disease_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        #region Medication

        public static string GetLocalPatientMedicationData = "SELECT * FROM PatientMedication where Service_Install_Id = @Service_Install_Id ";

        public static string Insert_PatientMedication = " INSERT INTO PatientMedication (Patient_EHR_ID,Medication_EHR_ID, PatientMedication_EHR_ID,PatientMedication_Web_ID,   "
                                            + " Medication_Note,Medication_Name,Medication_Type,Provider_EHR_ID,Drug_Quantity,Start_Date,End_Date, is_deleted, EHR_Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id, Patient_Notes, is_active) "
                                            + " VALUES (@Patient_EHR_ID,@Medication_EHR_ID,@PatientMedication_EHR_ID, @Medication_Web_ID, @Medication_Note,@Medication_Name,@Medication_Type,@Provider_EHR_ID,@Drug_Quantity,@Start_Date,@End_Date, @is_deleted, @EHR_Entry_DateTime , @Last_Sync_Date, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id,@Patient_Notes, @Is_Active) ";

        public static string Update_PatientMedication = " UPDATE PatientMedication SET Medication_EHR_ID=@Medication_EHR_ID,Patient_EHR_ID = @Patient_EHR_ID,Medication_Note = @Medication_Note ,Medication_Name = @Medication_Name, Medication_Type = @Medication_Type, "
                                             + " is_deleted = @is_deleted,Provider_EHR_ID = @Provider_EHR_ID,Drug_Quantity = @Drug_Quantity,Start_Date = @Start_Date ,End_Date = @End_Date,  "
                                             + " EHR_Entry_DateTime = @EHR_Entry_DateTime, Last_Sync_Date = @Last_Sync_Date, Is_Adit_Updated = @Is_Adit_Updated, Patient_Notes = @Patient_Notes, Is_Active = @Is_Active "
                                             + " WHERE PatientMedication_EHR_ID = @PatientMedication_EHR_ID and Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_PatientMedication = " UPDATE PatientMedication SET is_deleted = 1 , Is_active = 0, Is_Adit_Updated = 0 WHERE PatientMedication_EHR_ID = @PatientMedication_EHR_ID and is_deleted = 0 and Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string GetPushLocalPatientMedicationData = "SELECT * FROM PatientMedication WHERE Is_Adit_Updated = 0";

        public static string Update_PatientMedication_Web_Id = " UPDATE PatientMedication SET PatientMedication_Web_ID = @Web_ID, Is_Adit_Updated = 1 "
                                             + " WHERE Patient_EHR_ID = @Patient_EHR_ID and Medication_EHR_ID = @EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";


        public static string GetLocalMedicationData = "SELECT * FROM MedicationMaster where Service_Install_Id = @Service_Install_Id ";

        public static string GetPushLocalMedicationData = "SELECT * FROM MedicationMaster WHERE Is_Adit_Updated = 0";

        //public static string Insert_Medication = " INSERT INTO MedicationMaster (Medication_EHR_ID, Medication_Web_ID,  Medication_Name, "
        //                                    + " Medication_Type,Drug_Quantity, is_deleted, EHR_Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,Clinic_Number,Service_Install_Id) "
        //                                    + " VALUES (@Medication_EHR_ID, @Medication_Web_ID, @Medication_Name, "
        //                                    + " @Medication_Type,@Drug_Quantity, @is_deleted, @EHR_Entry_DateTime , @Last_Sync_Date, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id) ";

        public static string Insert_Medication = "INSERT INTO MedicationMaster(Medication_EHR_ID, Medication_Web_ID, Medication_Name, Medication_Description, Medication_Notes, Medication_Sig, Medication_Parent_EHR_ID, Medication_Type, Allow_Generic_Sub, Drug_Quantity, Refills, Is_Active, is_deleted, EHR_Entry_DateTime, Medication_Provider_ID, Last_Sync_Date, Is_Adit_Updated, Clinic_Number, Service_Install_Id) " +
                                                 " VALUES(@Medication_EHR_ID, @Medication_Web_ID, @Medication_Name, @Medication_Description, @Medication_Notes, @Medication_Sig, @Medication_Parent_EHR_ID, @Medication_Type, @Allow_Generic_Sub, @Drug_Quantity, @Refills, @Is_Active, @is_deleted, @EHR_Entry_DateTime, @Medication_Provider_ID, @Last_Sync_Date, @Is_Adit_Updated, @Clinic_Number, @Service_Install_Id);";

        //public static string Update_Medication = " UPDATE MedicationMaster SET Medication_Name = @Medication_Name,is_deleted = @is_deleted,Drug_Quantity = @Drug_Quantity "
        //                                     + " EHR_Entry_DateTime = @EHR_Entry_DateTime, Last_Sync_Date = @Last_Sync_Date, Is_Adit_Updated = @Is_Adit_Updated "
        //                                     + " WHERE Medication_EHR_ID = @Medication_EHR_ID AND Medication_Type = @Medication_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Medication = "UPDATE MedicationMaster SET Medication_Name = @Medication_Name, Medication_Description = @Medication_Description, Medication_Notes = @Medication_Notes, Medication_Sig = @Medication_Sig, Medication_Parent_EHR_ID = @Medication_Parent_EHR_ID, " +
                                                 "is_deleted = @is_deleted, Drug_Quantity = @Drug_Quantity, Refills = @Refills, Is_Active = @Is_Active, " +
                                                 "EHR_Entry_DateTime = @EHR_Entry_DateTime, Last_Sync_Date = @Last_Sync_Date, Is_Adit_Updated = @Is_Adit_Updated, Allow_Generic_Sub = @Allow_Generic_Sub, Medication_Provider_ID = @Medication_Provider_ID " +
                                                 "WHERE Medication_EHR_ID = @Medication_EHR_ID AND Medication_Type = @Medication_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_Medication = " UPDATE MedicationMaster SET is_deleted = 1 , Is_Adit_Updated = 0 WHERE is_deleted = 0 and Medication_EHR_ID = @Medication_EHR_ID  AND Medication_Type = @Medication_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Medication_Web_Id = " UPDATE MedicationMaster SET Medication_Web_ID = @Web_ID, Is_Adit_Updated = 1 "
                                                   + " WHERE Medication_EHR_ID = @EHR_ID AND Medication_Type = @Medication_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Update_PatientMedications_Web_Id = "UPDATE PatientMedication SET Is_Adit_Updated = 1, PatientMedication_Web_ID = @PatientMedication_Web_ID "
                                                              + " WHERE PatientMedication_EHR_ID = @PatientMedication_EHR_ID  AND Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        //UPDATE MedicationMaster SET Medication_Web_ID = @Web_ID, Is_Adit_Updated = 1 "
        //                                           + " WHERE Medication_EHR_ID = @EHR_ID AND Medication_Type = @Medication_Type And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id 
        #endregion

        #endregion

        #region RecallType

        public static string GetLocalRecallTypeData = "SELECT * FROM RecallType  where Service_Install_Id = @Service_Install_Id";

        public static string GetLocalUserData = "SELECT * FROM Users where Service_Install_Id = @Service_Install_Id";

        public static string GetPushLocalRecallTypeData = "SELECT * FROM RecallType WHERE Cast(RecallType_EHR_ID As BIGINT) >= 0 AND Is_Adit_Updated = 0";

        public static string GetPushLocalUserData = "SELECT * FROM Users WHERE Is_Adit_Updated = 0";

        public static string Insert_RecallType = "INSERT INTO RecallType "
                                           + " (RecallType_EHR_ID, RecallType_Web_ID, RecallType_Name, RecallType_Descript,EHR_Entry_DateTime, Is_Adit_Updated,Clinic_Number,Service_Install_Id ) "
                                           + " VALUES (@RecallType_EHR_ID, @RecallType_Web_ID, @RecallType_Name, @RecallType_Descript, @EHR_Entry_DateTime, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id ) ";


        public static string Update_RecallType = "UPDATE RecallType SET  RecallType_Name = @RecallType_Name, RecallType_Descript = @RecallType_Descript, "
                                           + " EHR_Entry_DateTime = @EHR_Entry_DateTime, Is_Adit_Updated = @Is_Adit_Updated WHERE RecallType_EHR_ID = @RecallType_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Update_RecallType_Pull = " UPDATE RecallType SET RecallType_Web_ID = @RecallType_Web_ID, RecallType_Name = @RecallType_Name , RecallType_Descript = @RecallType_Descript, "
                                                                     + " WHERE RecallType_EHR_ID = @RecallType_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        // public static string Delete_RecallType = " Delete From RecallType WHERE RecallType_EHR_ID = @RecallType_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";
        public static string Delete_RecallType = "Update RecallType SET IS_Deleted = 1,IS_Adit_Updated = 0 WHERE RecallType_EHR_ID = @RecallType_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Update_RecallType_Web_Id = "UPDATE RecallType SET Is_Adit_Updated = 1 ";


        #endregion

        #region User
        public static string Insert_User = "INSERT INTO Users "
                                         + " (User_EHR_ID, User_Web_ID, First_Name,Last_Name,EHR_Entry_DateTime,Last_Updated_Datetime, Is_Adit_Updated,Is_Active,Is_Deleted,Clinic_Number,Service_Install_Id ) "
                                         + " VALUES (@User_EHR_ID, @User_Web_ID, @First_Name, @Last_Name, @EHR_Entry_DateTime,@Last_Updated_Datetime, @Is_Adit_Updated,@Is_Active,@Is_deleted,@Clinic_Number,@Service_Install_Id ) ";

        public static string Update_User = "UPDATE Users SET  First_Name = @First_Name,Last_Name = @Last_Name, "
                                          + " EHR_Entry_DateTime = @EHR_Entry_DateTime,Last_Updated_Datetime=@Last_Updated_Datetime,Is_Active=@Is_Active,Is_deleted=@Is_deleted ,Is_Adit_Updated = @Is_Adit_Updated WHERE User_EHR_ID = @User_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";

        public static string Delete_User = " Update Users set Is_deleted=1,Is_Adit_Updated = 0 where User_EHR_ID = @User_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id ";
        #endregion

        #region ApptStatus

        public static string GetLocalAppointmentStatusData = "SELECT * FROM Appointment_Status where Service_Install_Id = @Service_Install_Id";

        public static string GetPushLocalAppointmentStatusData = "SELECT * FROM Appointment_Status WHERE Is_Adit_Updated = 0";

        public static string Insert_AppointmentStatus = "INSERT INTO Appointment_Status "
                                           + " (ApptStatus_EHR_ID, ApptStatus_Web_ID, ApptStatus_Name, ApptStatus_Type, EHR_Entry_DateTime, Is_Adit_Updated ,Clinic_Number,Service_Install_Id) "
                                           + " VALUES (@ApptStatus_EHR_ID, @ApptStatus_Web_ID, @ApptStatus_Name, @ApptStatus_Type, @EHR_Entry_DateTime, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id ) ";


        public static string Update_AppointmentStatus = "UPDATE Appointment_Status SET  ApptStatus_Name = @ApptStatus_Name,ApptStatus_Type = @ApptStatus_Type, EHR_Entry_DateTime = @EHR_Entry_DateTime, "
                                           + "  Is_Adit_Updated = @Is_Adit_Updated WHERE ApptStatus_EHR_ID = @ApptStatus_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id= @Service_Install_Id ";

        public static string Update_AppointmentStatus_Pull = " UPDATE Appointment_Status SET ApptStatus_Web_ID = @ApptStatus_Web_ID, ApptStatus_Name = @ApptStatus_Name ,ApptStatus_Type = @ApptStatus_Type, "
                                           + " WHERE ApptStatus_EHR_ID = @ApptStatus_EHR_ID  And Service_Install_Id =@Service_Install_Id ";

        public static string Delete_AppointmentStatus = " Delete From Appointment_Status WHERE ApptStatus_EHR_ID = @ApptStatus_EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id= @Service_Install_Id ";

        public static string Update_AppointmentStatus_Web_Id = "UPDATE Appointment_Status SET ApptStatus_Web_ID = @Web_ID, Is_Adit_Updated = 1 WHERE ApptStatus_Web_ID = @EHR_ID And Clinic_Number = @Clinic_Number And Service_Install_Id= @Service_Install_Id ";

        #endregion

        //rooja https://app.asana.com/0/1203599217474380/1207035132178655/f
        #region Insurance

        public static string GetLocalInsuranceData = "SELECT * FROM Insurance where Service_Install_Id = @Service_Install_Id";

        public static string GetPushLocalInsuranceData = "SELECT * FROM Insurance WHERE Is_Adit_Updated = 0";

        public static string Insert_Insurance = "INSERT INTO Insurance "
                                           + " (Insurance_EHR_ID, Insurance_Web_ID, Insurance_Name, Address,Address2,City,State,Zipcode,Phone,ElectId,EmployerName,Last_Sync_Date,Is_Deleted, EHR_Entry_DateTime, Is_Adit_Updated ,Service_Install_Id,Clinic_Number) "
                                           + " VALUES (@Insurance_EHR_ID, @Insurance_Web_ID, @Insurance_Name, @Address,@Address2,@City,@State,@Zipcode,@Phone,@ElectId,@EmployerName,@Last_Sync_Date,@Is_Deleted, @EHR_Entry_DateTime, @Is_Adit_Updated,@Service_Install_Id,@Clinic_Number ) ";


        public static string Update_Insurance = "UPDATE Insurance SET  Insurance_Name = @Insurance_Name,Address = @Address, Address2 = @Address2,City = @City,State = @State,Zipcode = @Zipcode, Phone = @Phone, ElectId = @ElectId , EmployerName = @EmployerName , Last_Sync_Date = @Last_Sync_Date , Is_Deleted = @Is_Deleted, EHR_Entry_DateTime = @EHR_Entry_DateTime, "
                                           + "  Is_Adit_Updated = @Is_Adit_Updated WHERE Insurance_EHR_ID = @Insurance_EHR_ID  And Service_Install_Id= @Service_Install_Id ";

        public static string Update_Insurance_Pull = " UPDATE Insurance SET Insurance_Web_ID = @Insurance_Web_ID, Insurance_Name = @Insurance_Name ,Address = @Address, Address2 = @Address2,City = @City,State = @State,Zipcode = @Zipcode, Phone = @Phone, ElectId = @ElectId , EmployerName = @EmployerName , Last_Sync_Date = @Last_Sync_Date , Is_Deleted = @Is_Deleted,"
                                           + " WHERE Insurance_EHR_ID = @Insurance_EHR_ID  And Service_Install_Id =@Service_Install_Id ";

       public static string Delete_Insurance = "Update Insurance SET IS_Deleted = 1, IS_Adit_Updated = 0 WHERE Insurance_EHR_ID = @Insurance_EHR_ID And Service_Install_Id = @Service_Install_Id";

        public static string Update_Insurance_Web_Id = "UPDATE Insurance SET Insurance_Web_ID = @Web_ID, Is_Adit_Updated = 1 WHERE Insurance_EHR_ID = @EHR_ID And Service_Install_Id= @Service_Install_Id ";

        #endregion

        #region PozativeAppointment

        public static string GetLocalPozativeAppointmentData = "SELECT * FROM Pozative_Patient ";

        public static string Update_PozativeAppointment_Web_Id = "UPDATE Pozative_Patient SET Appt_Web_ID = @Web_ID WHERE appointment_id = @EHR_ID ";

        #endregion

        #region ProviderOfficeHours

        public static string ProviderOfficeHoursBlankStructure = " SElect * FROM ProviderOfficeHOurs where 1=2 ";

        public static string ProviderHoursBlankStructure = " SElect * FROM ProviderHours where 1=2 ";

        public static string ProviderOfficeHours = " SElect * FROM ProviderOfficeHOurs where Service_Install_Id = @Service_Install_Id ";

        public static string GetPushProviderOfficeHours = " SElect * FROM ProviderOfficeHOurs where Is_Adit_Updated = 0 ";
        //AND ( starttime1 <> '01/01/1900' OR ENDTIME1 <> '01/01/1900' OR starttime2 <> '01/01/1900' OR ENDTIME2 <> '01/01/1900' OR starttime3 <> '01/01/1900' OR ENDTIME3 <> '01/01/1900' )
        public static string Update_ProviderOfficeHours_Web_Id = "UPDATE ProviderOfficeHours SET POH_Web_ID = @Web_ID ,Is_Adit_Updated = 1 WHERE Provider_EHR_ID = @Provider_EHR_ID And Service_Install_Id = @Service_Install_Id";

        public static string ProviderOfficeHours_Insert = " INsert into ProviderOfficeHours (POH_EHR_ID,POH_Web_ID,Provider_EHR_ID,WeekDay,StartTime1,EndTime1,StartTime2,EndTime2,StartTime3,EndTime3,Entry_DateTime,Last_Sync_Date,is_deleted,Is_Adit_Updated,Clinic_Number,Service_Install_Id)values (@POH_EHR_ID,@POH_Web_ID,@Provider_EHR_ID,@WeekDay,@StartTime1,@EndTime1,@StartTime2,@EndTime2,@StartTime3,@EndTime3,GetDate(),@Last_Sync_Date,0,0,@Clinic_Number,@Service_Install_Id) ";

        public static string ProviderOfficeHours_Update = " UPDATE ProviderOfficeHours SET Provider_EHR_ID = @Provider_EHR_ID, WeekDay = @WeekDay,StartTime1=@StartTime1,EndTime1=@EndTime1,StartTime2=@StartTime2,EndTime2=@EndTime2,StartTime3=@StartTime3,EndTime3=@EndTime3,Last_Sync_Date=@Last_Sync_Date,is_deleted=@is_deleted,Is_Adit_Updated=0 WHERE POH_EHR_ID = @POH_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string ProviderOfficeHours_Delete = " UPDATE Holiday SET is_deleted = 1,Is_Adit_Updated = 0 WHERE is_deleted = 0 and POH_EHR_ID = @POH_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        #endregion

        #region OperatoryOfficeHours

        public static string GetPushLocalOperatoryOfficeHoursData = "SELECT * FROM OperatoryOfficeHours WHERE Is_Adit_Updated = 0";

        public static string OperatoryOfficeHoursBlankStructure = " SElect * FROM OperatoryOfficeHOurs where 1=2 ";

        public static string Update_OperatoryOfficeHours_Web_Id = "UPDATE OperatoryOfficeHours SET OOH_Web_ID = @Web_ID ,Is_Adit_Updated = 1 WHERE Operatory_EHR_ID = @Operatory_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        #endregion

        public static string GetLocalMedicalHistory = " SELECT * FROM MedicalHistoryTableName where Is_Adit_Updated = 0  And Service_Install_Id= @Service_Install_Id  ";

        public static string GetMedicalHistorySubmit = "SELECT A.*,B.SectionItemType FROM MedicalHistorySubmitTable A INNER JOIN EagleSoftSectionItemMaster B ON A.SectionItem_EHR_Id = B.SectionItem_EHR_Id WHERE A.PatientForm_Web_ID = @PatientForm_Web_ID AND A.FormMaster_EHR_id = @FormMaster_EHR_id AND A.Is_EHR_Updated = 0 And A.Service_Install_Id= @Service_Install_Id ";

        public static string GetLocalMedicalHistoryAllRecords = " SELECT * FROM MedicalHistoryTableName  where Service_Install_Id= @Service_Install_Id  ";

        public static string Update_Is_Appt_MedicalHistoryTable = "UPDATE MedicalHistoryTableName SET Is_Adit_Updated = 1,WebIdColumnsName = @Web_ID WHERE EHRColumnsName = EHRColumnId  And Service_Install_Id= @Service_Install_Id   ";

        public static string GetDentrixLocalMedicleFormData = "SELECT * FROM Dentrix_Form";

        public static string GetAbelDentLocalMedicleFormData = "SELECT * FROM AbelDent_Form";

        public static string GetAbelDentLocalMedicleAnswerData = "SELECT * FROM AbelDent_ParitalControlForm";

        public static string GetEasyDentalLocalMedicleQuestionData = "SELECT EQ.*,Ef.CD_FormMaster_Web_ID as EasyDental_FormMaster_Web_ID FROM EasyDental_Question as EQ left outer join CD_FormMaster as Ef on ef.CD_FormMaster_EHR_ID = eq.EasyDental_FormId";

        public static string GetDentrixLocalFormQuestionData = "SELECT * FROM Dentrix_FormQuestion";

        public static string GetAbelDentLocalFormQuestionData = "SELECT * FROM AbelDent_FormQuestion";

        public static string Update_Dentrix_Response_Response_EHR_ID = " UPDATE Dentrix_Response SET Dentrix_Response_EHR_ID = @Dentrix_Response_EHR_ID , Is_EHR_Updated = 1 ,EHR_Entry_DateTime = getdate() "
                                          + " WHERE Dentrix_Question_EHRUnique_ID = @Dentrix_Question_EHRUnique_ID  ";

        public static string Update_AbelDent_Response_Response_EHR_ID = " UPDATE AbelDent_Response SET AbelDent_Response_EHR_ID = @AbelDent_Response_EHR_ID , Is_EHR_Updated = 1 ,EHR_Entry_DateTime = getdate() "
                                          + " WHERE AbelDent_Question_EHRUnique_ID = @AbelDent_Question_EHRUnique_ID  ";

        public static string Update_Cleardent_Response_EHR_ID = " UPDATE CD_Response SET CD_intPatMedId = @CD_intPatMedId , Is_EHR_Updated = 1 ,EHR_Entry_DateTime = getdate() "
                                         + " WHERE CD_FormMaster_EHR_ID = @CD_FormMaster_EHR_ID and Patient_EHR_ID = @Patient_EHR_ID and CD_QuestionMaster_EHR_ID = @CD_QuestionMaster_EHR_ID  ";

        public static string Update_OpenDental_Response_EHR_ID = " UPDATE OD_Response SET Is_EHR_Updated = 1 ,EHR_Entry_DateTime = getdate() "
                                        + " WHERE OD_Response_LocalDB_ID = @OD_Response_LocalDB_ID And Service_Install_Id = @Service_Install_Id ";

        public static string Update_EasyDental_Response = " UPDATE EasyDental_Response SET Is_EHR_Updated = 1 WHERE Patient_EHR_ID = @Patient_EHR_ID and EasyDental_QuestionId = @EasyDental_QuestionId  ";


        public static string GetLocalPatientWiseRecallTypeData = "SELECT * FROM Patient_RecallType where Service_Install_Id = @Service_Install_Id";

        public static string GetLocalPatientWiseRecallTypeBlankData = "SELECT * FROM Patient_RecallType where 1=2";

        public static string Insert_PatientWiseRecallType = "INSERT INTO Patient_RecallType "
                                           + " (Patient_Recall_Id, Patient_EHR_id, Recall_Date,Last_Recall_Date, Provider_EHR_ID,RecallType_EHR_ID,RecallType_Name,RecallType_Descript,Default_Recall,Entry_DateTime,Last_Sync_Date,EHR_Entry_DateTime,Is_Deleted,Is_Adit_Updated,Clinic_Number,Service_Install_Id ) "
                                   + " VALUES (@Patient_Recall_Id, @Patient_EHR_id, @Recall_Date, @Last_Recall_Date, @Provider_EHR_ID, @RecallType_EHR_ID,@RecallType_Name,@RecallType_Descript,@Default_Recall, @Entry_DateTime, @Last_Sync_Date,@EHR_Entry_DateTime, 0, 0, @Clinic_Number,@Service_Install_Id ) ";

        public static string Update_PatientWiseRecallType = "UPDATE Patient_RecallType SET  Patient_EHR_id = @Patient_EHR_id, Recall_Date=@Recall_Date,Provider_EHR_ID=@Provider_EHR_ID,RecallType_EHR_ID=@RecallType_EHR_ID,RecallType_Name=@RecallType_Name,RecallType_Descript=@RecallType_Descript,Default_Recall=@Default_Recall, Last_Sync_Date=@Last_Sync_Date,EHR_Entry_DateTime=@EHR_Entry_DateTime,Is_Deleted=@Is_Deleted,Is_Adit_Updated=0,Clinic_Number=@Clinic_Number,Service_Install_Id=@Service_Install_Id where Patient_Recall_Id = @Patient_Recall_Id ";

        public static string Delete_PatientWiseRecallType = " UPdate Patient_RecallType Set Is_deleted = 1 where Is_deleted = 0 and Patient_RecallTypeId = Patient_Recall_Id";

        public static string GetLocalPatientPaymebtLogStructure = " Select * FROM PatientPayment where Clinic_Number = @Clinic_Number AND Service_Install_Id = @Service_Install_Id AND 1=2";


        public static string GetLocalPatientWiseSMSCallLogStructure = " Select * FROM PatientSMSCallLog where Clinic_Number = @Clinic_Number AND Service_Install_Id = @Service_Install_Id AND 1=2";

        public static string GetLocalPatientWiseSMSCallLog = " Select * FROM PatientSMSCallLog where Clinic_Number = @Clinic_Number AND Service_Install_Id = @Service_Install_Id AND LogType = @LogType ";

        public static string GetLocalPatientPaymebtLog = " Select * FROM PatientPayment where Clinic_Number = @Clinic_Number AND Service_Install_Id = @Service_Install_Id";

        public static string InsertIntoPatientPaymentLog = " Insert into PatientPayment (PatientEHRId,Patient_Web_ID,PatientPaymentWebId,ProviderEHRId,PaymentDate,Amount,PaymentNote,PaymentMode,PaymentType,PaymentInOut,BankOrBranchName,ChequeNumber,PaymentReceivedLocal,PaymentEntryDatetimeLocal,PaymentUpdatedEHR,PaymentUpdatedEHRDateTime,PaymentStatusCompletedAdit,PaymentStatusCompletedDateTime,PaymentEHRId,template,Clinic_Number,Service_Install_Id,EHRSyncPaymentLog,FirstName,LastName,Mobile,Email,Fees,Discount,EHRErroLog,TryInsert,PaymentMethod,EHRSyncFinancialLogSetting) "
                                                             + " VALUES (@PatientEHRId,@Patient_Web_ID,@PatientPaymentWebId,@ProviderEHRId,@PaymentDate,@Amount,@PaymentNote,@PaymentMode,@PaymentType,@PaymentInOut,@BankOrBranchName,@ChequeNumber,@PaymentReceivedLocal,@PaymentEntryDatetimeLocal,@PaymentUpdatedEHR,@PaymentUpdatedEHRDateTime,@PaymentStatusCompletedAdit,@PaymentStatusCompletedDateTime,@PaymentEHRId,@template,@Clinic_Number,@Service_Install_Id,@EHRSyncPaymentLog,@FirstName,@LastName,@Mobile,@Email,@Fees,@Discount,@EHRErroLog,@TryInsert,@PaymentMethod,@EHRSyncFinancialLogSetting)";

        public static string CheckPaymententryExist = "SELECT CASE WHEN EXISTS (SELECT * FROM PatientPayment WHERE PatientEHRId = @PatientEHRId AND PaymentDate = @PaymentDate AND Amount = @Amount AND PaymentNote = @PaymentNote AND PaymentMode = @PaymentMode) THEN 1  ELSE 0 END AS RecordExists";

        public static string UpdatePatientPaymentLog = " UPDATE PatientPayment SET PatientEHRId= @PatientEHRId,Patient_Web_ID=@Patient_Web_ID,PatientPaymentWebId=@PatientPaymentWebId,ProviderEHRId=@ProviderEHRId,PaymentDate=@PaymentDate,Amount=@Amount,PaymentNote=@PaymentNote,PaymentType=@PaymentType,PaymentInOut=@PaymentInOut,BankOrBranchName=@BankOrBranchName,ChequeNumber=@ChequeNumber,PaymentReceivedLocal=@PaymentReceivedLocal,PaymentEntryDatetimeLocal=@PaymentEntryDatetimeLocal,"
                                                      + " PaymentUpdatedEHR =  @PaymentUpdatedEHR,PaymentUpdatedEHRDateTime = @PaymentUpdatedEHRDateTime,PaymentStatusCompletedAdit =  @PaymentStatusCompletedAdit,PaymentStatusCompletedDateTime=@PaymentStatusCompletedDateTime,PaymentEHRId=@PaymentEHRId,template=@template,EHRSyncPaymentLog=@EHRSyncPaymentLog,FirstName =@FirstName, LastName =@LastName,Mobile =@Mobile,Email =@Email,Fees =@Fees,Discount =@Discount,EHRErroLog = @EHRErroLog where PatientPaymentWebId=@PatientPaymentWebId";

        public static string UpdatePatientPaymentLogTable = " UPDATE PatientPayment SET EHRErroLog = @EHRErroLog,EHRLogId=@EHRLogId,EHRDiscountId=@EHRDiscountId, PaymentUpdatedEHR = @PaymentUpdatedEHR,PaymentUpdatedEHRDateTime=@PaymentUpdatedEHRDateTime,PaymentStatusCompletedAdit=@PaymentStatusCompletedAdit,PaymentStatusCompletedDateTime=@PaymentStatusCompletedDateTime,PaymentEHRId=@PaymentEHRId,TryInsert=@TryInsert WHERE PatientPaymentWebId = @PatientPaymentWebId AND  Service_Install_Id= @Service_Install_Id";

        public static string UpdatePatientPaymentLogError = " UPDATE PatientPayment SET EHRErroLog = @EHRErroLog,TryInsert=@TryInsert WHERE PatientPaymentWebId = @PatientPaymentWebId AND  Service_Install_Id= @Service_Install_Id";

        public static string UpdatePatientSMSCallLogTable = " UPDATE PatientSMSCallLog SET LogUpdatedEHR = @LogUpdatedEHR,LogUpdatedEHRDateTime=@LogUpdatedEHRDateTime,LogStatusCompletedAdit=@LogStatusCompletedAdit,LogStatusCompletedDateTime=@LogStatusCompletedDateTime,LogEHRId=@LogEHRId WHERE PatientSMSCallLogWebId = @PatientSMSCallLogWebId AND  Service_Install_Id= @Service_Install_Id and PatientEHRId = @PatientEHRId";
        public static string UpdatePatientStatus = "UPDATE Patient SET  Patient_Status = @Patient_Status,  Is_Status_Adit_Updated = (case when Patient_status_Compare = 'New' then  1  else 0 end) WHERE Patient_EHR_ID = @Patient_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string UpdatePatientStatusAllBlank = " UPDATE Patient SET Patient_status_Compare = '' WHERE Service_Install_Id = @Service_Install_Id AND Patient_status_Compare <> ''";

        public static string UpdatePatientStatusAll = " UPDATE Patient SET Patient_status_Compare = Patient_Status, Patient_Status = 'Existing',Is_Status_Adit_Updated = 0 WHERE Service_Install_Id = @Service_Install_Id AND  Patient_Status = 'New'";

        public static string UpdatePatientStatus_ClinicWise = "UPDATE Patient SET  Patient_Status = @Patient_Status,  Is_Status_Adit_Updated = (case when Patient_status_Compare = 'New' then  1  else 0 end) WHERE Patient_EHR_ID = @Patient_EHR_ID And Service_Install_Id = @Service_Install_Id AND Clinic_Number = @Clinic_Number ";

        public static string UpdatePatientStatusAllBlank_ClinicWise = " UPDATE Patient SET Patient_status_Compare = '' WHERE Service_Install_Id = @Service_Install_Id AND Patient_status_Compare <> '' AND Clinic_Number = @Clinic_Number ";

        public static string UpdatePatientStatusAll_ClinicWise = " UPDATE Patient SET Patient_status_Compare = Patient_Status, Patient_Status = 'Existing',Is_Status_Adit_Updated = 0 WHERE Service_Install_Id = @Service_Install_Id AND  Patient_Status = 'New' AND Clinic_Number = @Clinic_Number ";

        public static string CheckSMSLogBeforeInsert = " Select count(1) from PatientSMSCallLog where smsId = @smsId and patientehrid = @PatientEHRId ";

        public static string UpdatePatientBalance = "UPDATE Patient SET CurrentBal = @CurrentBal,ThirtyDay = @ThirtyDay,SixtyDay = @SixtyDay,NinetyDay = @NinetyDay,Over90 = @Over90 WHERE Service_Install_Id = @Service_Install_Id AND Patient_EHR_ID = @Patient_EHR_ID";

        public static string InsertIntoPatientSMSCallLog = " Insert Into [PatientSMSCallLog] ([esId],[smsId],[PatientEHRId],[Patient_Web_ID],[PatientMobile],[PatientSMSCallLogWebId],[ProviderEHRId],[app_alias],[text],[message_direction],[message_type],[PatientSMSCallLogDate],[LogReceivedLocal],[LogEntryDatetimeLocal],[LogUpdatedEHR],[LogUpdatedEHRDateTime],[LogStatusCompletedAdit],[LogStatusCompletedDateTime],[LogEHRId],[Clinic_Number],[Service_Install_Id],[ErrorMessage],[LogType]) "
                                                          + " values (@esId,@smsId,@PatientEHRId,@Patient_Web_ID,@PatientMobile,@PatientSMSCallLogWebId,@ProviderEHRId,@app_alias,@text,@message_direction,@message_type,@PatientSMSCallLogDate,@LogReceivedLocal,@LogEntryDatetimeLocal,@LogUpdatedEHR,@LogUpdatedEHRDateTime,@LogStatusCompletedAdit,@LogStatusCompletedDateTime,@LogEHRId,@Clinic_Number,@Service_Install_Id,@ErrorMessage,@LogType)";


        public static string UpdatePatientSMSCallLog = " Update [PatientSMSCallLog] set esId = @esId,smsId = @smsId,PatientEHRId = @PatientEHRId,Patient_Web_ID = @Patient_Web_ID,PatientMobile = @PatientMobile,ProviderEHRId = @ProviderEHRId,app_alias = @app_alias, "
                                                     + " text = @text,message_direction = @message_direction,message_type = @message_type,PatientSMSCallLogDate = @PatientSMSCallLogDate,LogReceivedLocal = @LogReceivedLocal,LogEntryDatetimeLocal = @LogEntryDatetimeLocal,LogUpdatedEHR = @LogUpdatedEHR, "
                                                     + " LogUpdatedEHRDateTime = @LogUpdatedEHRDateTime, LogStatusCompletedAdit = @LogStatusCompletedAdit,LogStatusCompletedDateTime = @LogStatusCompletedDateTime,LogEHRId = @LogEHRId,Clinic_Number = @Clinic_Number,Service_Install_Id = @Service_Install_Id, ErrorMessage = @ErrorMessage,LogType = @LogType where PatientSMSCallLogWebId = @PatientSMSCallLogWebId ";

        public static string Insert_PatientProfileImageData = " INSERT INTO Patient_Images (Patient_Images_Web_ID,Patient_Images_EHR_ID, Patient_EHR_ID,Patient_Web_ID, Image_EHR_Name, Patient_Images_FilePath, Entry_DateTime , AditApp_Entry_DateTime, Is_Deleted, Is_Adit_Updated,Clinic_Number,Service_Install_Id ) "
                                                        + " VALUES (@Patient_Images_Web_ID, @Patient_Images_EHR_ID, @Patient_EHR_ID,@Patient_Web_ID, @Image_EHR_Name, @Patient_Images_FilePath, @Entry_DateTime, @AditApp_Entry_DateTime, @Is_Deleted, @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id ) ";

        public static string Update_PatientProfileImageData = " Update Patient_Images set Patient_Images_Web_ID = @Patient_Images_Web_ID,Patient_Images_EHR_ID = @Patient_Images_EHR_ID,Patient_Web_ID =@Patient_Web_ID, Image_EHR_Name = @Image_EHR_Name, Patient_Images_FilePath = @Patient_Images_FilePath, Entry_DateTime = @Entry_DateTime , AditApp_Entry_DateTime = @AditApp_Entry_DateTime, Is_Deleted = @Is_Deleted, Is_Adit_Updated = @Is_Adit_Updated,Is_ProfileImage_Update= 1 "
                                                      + " where Patient_EHR_ID = @Patient_EHR_ID and Clinic_Number = @Clinic_Number and Service_Install_Id = @Service_Install_Id and Is_Deleted = 0";

        public static string Delete_PatientProfileImageData = " Update Patient_Images set Is_Deleted = 1,Is_Adit_Updated = 0 where Patient_Images_EHR_ID =@Patient_Images_EHR_ID and Patient_EHR_ID = @Patient_EHR_ID and Clinic_Number = @Clinic_Number and Service_Install_Id = @Service_Install_Id and Is_Deleted = 0 ";

        public static string Update_PatientProfileImageStatus = "Update Patient_Images set Is_Adit_Updated = 1,AditApp_Entry_DateTime = getdate(),Is_ProfileImage_Update=0 WHERE Patient_Web_ID = @Patient_Web_ID and Service_Install_Id = @Service_Install_Id";

        public static string GetLocalPatientProfileImageRecords = " select pi.*,l.[Location_ID] from [Patient_Images] as pi inner join [Location] as l on l.[Clinic_Number] = pi.[Clinic_Number] and l.[Service_Install_Id] = pi.[Service_Install_Id] where pi.Service_Install_Id= @Service_Install_Id and pi.Is_Adit_Updated = 0  AND Image_EHR_name <> ''";

        public static string GetLocalPatientProfileImageAllRecords = " select pi.*,l.[Location_ID] from [Patient_Images] as pi inner join [Location] as l on l.[Clinic_Number] = pi.[Clinic_Number] and l.[Service_Install_Id] = pi.[Service_Install_Id]  where pi.Service_Install_Id= @Service_Install_Id ";

        public static string GetLocalPatientFormMedicationResponse = " SELECT * From MedicationResponse where Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormMedicationResponse_Clinic = " SELECT * From MedicationResponse where (Clinic_Number = @Clinic_Number) And Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormMedicationRemovedResponse = " SELECT * From MedicationRemovedResponse where Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormMedicationRemovedResponse_Clinic = " SELECT * From MedicationRemovedResponse where (Clinic_Number = @Clinic_Number) And Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormMedicationResponseToSaveINEHR = "SELECT DISTINCT case when PF.Patient_EHR_ID != '' then PF.Patient_EHR_ID else MR.Patient_ehr_id end AS PatientEHRId,M.Medication_Name as Medication_Name_Org,MR.* From MedicationResponse MR INNER JOIN Patient_Form PF ON PF.Service_Install_Id = MR.Service_Install_Id AND MR.PatientForm_Web_ID = PF.PatientForm_Web_ID LEFT JOIN MedicationMaster M on MR.Medication_EHR_ID = M.Medication_EHR_ID WHERE MR.Is_EHR_Updated = 0 And MR.Service_Install_Id = @Service_Install_Id ";

        public static string GetLocalPatientFormMedicationRemovedResponseToSaveINEHR = "SELECT DISTINCT case when PF.Patient_EHR_ID != '' then PF.Patient_EHR_ID else MR.Patient_ehr_id end AS PatientEHRId,MR.* From MedicationRemovedResponse MR INNER JOIN Patient_Form PF ON PF.Service_Install_Id = MR.Service_Install_Id AND MR.PatientForm_Web_ID = PF.PatientForm_Web_ID WHERE MR.Is_EHR_Updated = 0 And MR.Service_Install_Id = @Service_Install_Id ";

        public static string Update_Medication_EHR_Update_Flg = "UPDATE MedicationResponse SET Is_EHR_Updated = 1, PatientMedication_EHR_Id = @PatientMedication_EHR_Id, Patient_EHR_ID = @Patient_EHR_ID, Medication_EHR_ID = @Medication_EHR_ID WHERE MedicationResponse_Local_ID = @MedicationResponse_Local_Id And Service_Install_Id = @Service_Install_Id ";

        public static string Update_Removed_Medication_EHR_Update_Flg = "UPDATE MedicationRemovedResponse SET Is_EHR_Updated = 1 WHERE MedicationRemovedResponse_Local_ID = @MedicationRemovedResponse_Local_Id And PatientMedication_EHR_Id = @PatientMedication_EHR_Id And Patient_EHR_ID = @Patient_EHR_ID And Service_Install_Id = @Service_Install_Id ";

        public static string GetAllLocalNoteId = " Select LogEHRId,'PatientSMSCallLog' as tblName FROM PatientSMSCallLog where LogUpdatedEHR=1 and Service_Install_Id = @Service_Install_Id  and LogEHRId !='' UNION ALL select case when EHRLogId = '' then PaymentEHRId else EHRLogId end LogEHRId, 'PatientPayment' as tblName  FROM PatientPayment where PaymentUpdatedEHR=1 AND EHRSyncPaymentLog IN (1,3) and Service_Install_Id = @Service_Install_Id ";

        public static string GetUpdateNewNoteIdPatientSMSCallLog = " Update PatientSMSCallLog set LogEHRId=@NewLogEHRId where LogEHRId = @LogEHRId and Service_Install_Id=@Service_Install_Id ";

        //public static string GetUpdateNewNoteIdPatientPayment = " Update PatientPayment set PaymentEHRId=@NewLogEHRId where PaymentEHRId = @LogEHRId and Service_Install_Id=@Service_Install_Id ";
        public static string GetUpdateNewNoteIdPatientPayment = " Update PatientPayment " +
                                                                " set EHRLogId = (Case when EHRLogId = '' THEN '' ELSE CONVERT(NVARCHAR(20),@NewLogEHRId) END ),  " +
                                                                " PaymentEHRId = (Case when EHRLogId = '' THEN CONVERT(NVARCHAR(20),@NewLogEHRId)  ELSE CONVERT(NVARCHAR(20),PaymentEHRId)  END  )   " +
                                                                " where (case when EHRLogId = '' then CONVERT(NVARCHAR(20),PaymentEHRId) else CONVERT(NVARCHAR(20),EHRLogId) end) = @LogEHRId and Service_Install_Id=@Service_Install_Id ";
        
        //rooja  https://app.asana.com/0/1203599217474380/1207342219246181/f
        #region InsuranceCarrier
        public static string SelectInsuranceCarrierDocId = "select InsuranceCarrier_Doc_Web_ID from InsuranceCarrier_Document where InsuranceCarrier_Doc_Web_ID = @InsuranceCarrier_Doc_Web_ID ";

        public static string Update_InsuranceCarrierNotFound_Live_To_Local = "UPDATE InsuranceCarrier_Document set Is_Pdf_Created = 0 WHERE InsuranceCarrier_Doc_Web_ID =@InsuranceCarrier_Doc_Web_ID ";

        public static string Update_InsuranceCarrier_Local_To_EHR = "UPDATE InsuranceCarrier_Document set Is_EHR_Updated = 1 , InsuranceCarrier_Doc_EHR_ID = @InsuranceCarrier_Doc_EHR_ID WHERE InsuranceCarrier_Doc_Web_ID =@InsuranceCarrier_Doc_Web_ID";

        public static string GetLiveInsuranceCarrierDocData = " SELECT * From InsuranceCarrier_Document where Is_Pdf_Created=1 And Is_EHR_Updated = 0 ";

        public static string GetLiveDentrixInsruanceCarrierDocData = "SELECT ICD.*, ISNULL(FL.[Folder_Name],'')  From InsuranceCarrier_Document ICD LEFT JOIN FolderList FL ON ICD.[InsuranceCarrier_FolderName] = FL.[FolderList_EHR_ID] where ICD.Is_Pdf_Created= 1 And ICD.Is_EHR_Updated = 0 ";

        public static string UpdateInsuranceCarrierDocStatus = "UPDATE InsuranceCarrier_Document SET InsuranceCarrier_Doc_Name = @InsuranceCarrier_Doc_Name,InsuranceCarrier_FolderName = @InsuranceCarrier_FolderName, Last_Sync_Date = @Last_Sync_Date,Is_Pdf_Created=@Is_Pdf_Created where InsuranceCarrier_Doc_Web_ID =@InsuranceCarrier_Doc_Web_ID";

        public static string UpdateInsuranceCarrierDocAditUpdated = "UPDATE InsuranceCarrier_Document SET Is_Adit_Updated = 1 where InsuranceCarrier_Doc_Web_ID = @InsuranceCarrier_Doc_Web_ID";

        public static string Insert_InsuranceCarrierDoc = "INSERT INTO InsuranceCarrier_Document(Patient_EHR_ID,InsuranceCarrier_Doc_Web_ID,Patient_Web_ID,InsuranceCarrier_FolderName,InsuranceCarrier_Doc_Name,Entry_DateTime,Clinic_Number,Service_Install_Id,PatientName,SubmittedDate,is_PDF_Created)VALUES(@Patient_EHR_ID,@InsuranceCarrier_Doc_Web_ID,@Patient_Web_ID,@InsuranceCarrier_FolderName,@InsuranceCarrier_Doc_Name,@Entry_DateTime,@Clinic_Number,@Service_Install_Id,@PatientName,@SubmittedDate,@Is_PDF_Created)";

        public static string GetLocalPendingInsuranceCarrierDocData = " SELECT * From InsuranceCarrier_Document where Is_Pdf_Created = 0  And Service_Install_Id = @Service_Install_Id ";

        public static string GetEHRPendingInsuranceCarrierDocData = "SELECT * From InsuranceCarrier_Document where Is_EHR_Updated = 0 And Service_Install_Id = @Service_Install_Id And Is_Pdf_Created = 1";

        public static string ImportingInsuranceCarrierDocStatus = "select * from InsuranceCarrier_Document where Is_Adit_Updated =0 and Is_EHR_Updated = 0";

        public static string CompletedInsuranceCarrierDocStatus = "select * from InsuranceCarrier_Document where Is_Adit_Updated =0 and Is_EHR_Updated = 1";
        #endregion

        #region ApptPatientBalance

        public static string GetLocalApptPatientBalanceData = "SELECT * FROM Appointment_Patient WHERE Appointment_EHR_ID >= 0 And Service_Install_Id = @Service_Install_Id AND Clinic_Number = @Clinic_Number";

        public static string GetPushLocalApptPatientBalanceData = "SELECT Patient_EHR_ID,ApptPatient_Web_ID,CurrentBal,ThirtyDay,SixtyDay,NinetyDay,Over90,used_benefit,collect_payment,remaining_benefit,Clinic_Number,Service_Install_Id FROM Appointment_Patient WHERE Is_Adit_Updated = 0 ";

        public static string Insert_ApptPatientBalance = " INSERT INTO Appointment_Patient (Appointment_EHR_ID, ApptPatient_Web_ID, Patient_EHR_ID, CurrentBal,ThirtyDay,SixtyDay,NinetyDay,Over90,remaining_benefit,collect_payment,used_benefit,Is_Adit_Updated,Clinic_Number,Service_Install_Id) "
                                                                     + " VALUES (@Appointment_EHR_ID, @ApptPatient_Web_ID, @Patient_EHR_ID, @CurrentBal, @ThirtyDay, @SixtyDay, @NinetyDay, @Over90, @remaining_benefit, @collect_payment, @used_benefit, @Is_Adit_Updated, @Clinic_Number, @Service_Install_Id)";

        public static string Update_ApptPatientBalance = "UPDATE Appointment_Patient SET CurrentBal = @CurrentBal, ThirtyDay = @ThirtyDay, SixtyDay = @SixtyDay, NinetyDay = @NinetyDay, Over90 = @Over90 , remaining_benefit = @remaining_benefit, used_benefit = @used_benefit, collect_payment = @collect_payment, Is_Adit_Updated = 0 WHERE Service_Install_Id = @Service_Install_Id AND Patient_EHR_ID = @Patient_EHR_ID ";

        public static string Update_ApptPatientBalance_Web_Id = "UPDATE Appointment_Patient SET ApptPatient_Web_ID = @Web_ID , Is_Adit_Updated = 1  WHERE Patient_EHR_ID = @EHR_ID And Service_Install_Id = @Service_Install_Id ";

        #endregion

        #region BackupDB

        public static string GetAditHostServerData = "Select * from Adit_HostServer";
        public static string GetLocationData = "Select * from Location";
        public static string GetOrganizationData = "Select * from Organization";
        public static string GetServiceInstallationData = "Select * from Service_Installation";
        public static string GetSynchModuleData = "Select * from SyncModule";

        public static string Insert_AditHostServerData = "INSERT INTO Adit_HostServer (HostName,MultiRecordHostName,Is_Active,ServerType) VALUES (@HostName,@MultiRecordHostName,@Is_Active,@ServerType)";
        public static string Insert_LocationData = "INSERT INTO Location (Location_ID,name,google_address,phone,email,address,website_url,language,owner,location_numbers,Organization_ID,User_ID,Loc_ID,Clinic_Number,Service_Install_Id,AditSync,ApptAutoBook,AditLocationSyncEnable) "
                                            + " VALUES(@Location_ID, @name, @google_address, @phone, @email, @address, @website_url, @language, @owner, @location_numbers, @Organization_ID, @User_ID, @Loc_ID, @Clinic_Number, @Service_Install_Id, @AditSync, @ApptAutoBook, @AditLocationSyncEnable)";
        public static string Insert_OrganizationData = "INSERT INTO Organization   (Organization_ID  ,Name  ,phone  ,email  ,address  ,currency  ,info  ,is_active  ,owner  ,Adit_User_Email_ID  ,Adit_User_Email_Password)  "
                                                            + " VALUES(@Organization_ID  , @Name  , @phone  , @email  , @address  , @currency  , @info  , @is_active  , @owner  , @Adit_User_Email_ID  , @Adit_User_Email_Password  )";
        public static string Insert_ServiceInstallationData = "INSERT INTO Service_Installation (Installation_ID, Organization_ID, Location_ID, Application_Name, Application_Version, System_Name, System_processorID, Hostname, [Database], IntegrationKey, UserId, Password, Port, WebAdminUserToken, timezone, IS_Install, Installation_Date, Installation_Modify_Date, AditSync, PozativeSync, ApptAutoBook, PozativeEmail, PozativeLocationID, PozativeLocationName, DBConnString, Document_Path, Windows_Service_Version, ApplicationIdleTimeOff, AppIdleStartTime, AppIdleStopTime, ApplicationInstalledTime, EHR_Sub_Version, EHR_VersionNumber, NotAllowToChangeSystemDateFormat, DontAskPasswordOnSaveSetting, DentrixPDFConstring, DentrixPDFPassword,Adit_User_Email_ID,Adit_User_Email_Password) "
                                                 + " VALUES (@Installation_ID, @Organization_ID, @Location_ID, @Application_Name, @Application_Version, @System_Name, @System_processorID, @Hostname, @Database, @IntegrationKey, @UserId, @Password, @Port, @WebAdminUserToken, @timezone, @IS_Install, @Installation_Date, @Installation_Modify_Date, @AditSync, @PozativeSync, @ApptAutoBook, @PozativeEmail, @PozativeLocationID, @PozativeLocationName, @DBConnString, @Document_Path, @Windows_Service_Version, @ApplicationIdleTimeOff, @AppIdleStartTime, @AppIdleStopTime, @ApplicationInstalledTime, @EHR_Sub_Version, @EHR_VersionNumber, @NotAllowToChangeSystemDateFormat, @DontAskPasswordOnSaveSetting, @DentrixPDFConstring, @DentrixPDFPassword,@Adit_User_Email_ID,@Adit_User_Email_Password)";
        public static string Insert_SynchModuleData = "Insert Into SyncModule  (SyncModule_ID,SyncModule_Name,SyncModule_Pull,SyncModule_Push,SyncModule_EHR,Last_Update_Date,SyncDateTime) " 
                                                           + " VALUES (@SyncModule_ID,@SyncModule_Name,@SyncModule_Pull,@SyncModule_Push,@SyncModule_EHR,@Last_Update_Date,@SyncDateTime)";

        public static string CheckLocationTable = "Select 1 from Location Where Location_ID = @Location_ID";
        public static string CheckOrganizationTable = "Select 1 from Organization Where Organization_ID = Organization_ID";
        public static string CheckServiceInstallationTable = "Select 1 from Service_Installation Where Location_ID = @Installation_ID";

        public static string Delete_LocationTable = "Delete From Location";
        public static string Delete_OrganizationTable = "Delete From Organization";
        public static string Delete_Service_InstallationTable = "Delete From Service_Installation";
        public static string Delete_AditHostServerTable = "Delete From Adit_HostServer";
        public static string Delete_SynchModuleTable = "Delete From SyncModule";

        #endregion


        #region ZohoDetails
        public static string GetLocalZohoDetailsData = "SELECT * FROM ZohoAccessDetails;";

        public static string InsertLocalZohoDetailsData = "Insert into ZohoAccessDetails(Organisation_ID,Organisation_Name,Location_ID,Location_Name,EHR_Pass,EHR_User,Server_User,Server_Pass,is_Confirmed,is_Installed,is_Valid,Last_Updated_DateTime,Entry_DateTime,Clinic_Number,Service_Install_Id) " +
                                                          " Values(@Organisation_ID, @Organisation_Name, @Location_ID, @Location_Name, @EHR_Pass, @EHR_User, @Server_User, @Server_Pass, @is_Confirmed, @is_Installed, @is_Valid, @Last_Updated_DateTime, @Entry_DateTime, @Clinic_Number, @Service_Install_Id)";

        public static string UpdateLocalZohoDetailsData = "Update ZohoAccessDetails set " +
                                                          " Organisation_ID = @Organisation_ID, " +
                                                          " Organisation_Name = @Organisation_Name, " +
                                                          " Location_ID= @Location_ID, " +
                                                          " Location_Name= @Location_Name, " +
                                                          " EHR_Pass= @EHR_Pass, " +
                                                          " EHR_User= @EHR_User, " +
                                                          " Server_User= @Server_User, " +
                                                          " Server_Pass= @Server_Pass, " +
                                                          " is_Confirmed= @is_Confirmed, " +
                                                          " is_Installed= @is_Installed, " +
                                                          " is_Valid= @is_Valid, " +
                                                          " Last_Updated_DateTime= @Last_Updated_DateTime, " +
                                                          " Clinic_Number= @Clinic_Number, " +
                                                          " Service_Install_Id= @Service_Install_Id " +
                                                          " where Zoho_LocalDB_ID = @Zoho_LocalDB_ID";

        public static string GetLocalSystemUsersData = "SELECT * FROM SystemUsers;";

        public static string GetPushLocalSystemUsersData = "SELECT * FROM SystemUsers Where Is_Adit_Updated = 0 And Clinic_Number = @Clinic_Number And Service_Install_Id = @Service_Install_Id;";

        public static string InsertLocalSystemUsersData = "Insert into SystemUsers(Server_User_Name,Server_User_Password,Is_Active,is_deleted,is_Adit_Updated,Clinic_Number,Service_Install_Id) " +
                                                          " Values(@Server_User_Name, @Server_User_Password, @Is_Active, @is_deleted, 0, @Clinic_Number, @Service_Install_Id)";

        public static string UpdateLocalSystemUsersData = "Update SystemUsers set " +
                                                          "Server_User_Name = @Server_User_Name, " +
                                                          "Server_User_Password = @Server_User_Password, " +
                                                          "Is_Active = @Is_Active, " +
                                                          "is_deleted = @is_deleted, " +
                                                          "Clinic_Number = @Clinic_Number, " +
                                                          "is_Adit_Updated = 0, " +
                                                          "Service_Install_Id = @Service_Install_Id " +
                                                          "Where Server_User_LocalDB_ID = @Server_User_LocalDB_ID;";

        public static string DeleteLocalSystemUsersData = "Update SystemUsers set " +
                                                          "is_deleted = 1, is_Adit_Updated = 0 " +
                                                          "Where Server_User_LocalDB_ID = @Server_User_LocalDB_ID;";

        public static string UpdateLocalSystemUsersDataWebId = "Update SystemUsers set " +
                                                               "Server_User_Web_ID = @Server_User_Web_ID, Is_Adit_Updated = 1 " +
                                                               "Where " +
                                                               "Server_User_Name = @Server_User_Name " +
                                                               "And Clinic_Number = @Clinic_Number " +
                                                               "And Service_Install_Id = @Service_Install_Id";

        #endregion
    }
}
