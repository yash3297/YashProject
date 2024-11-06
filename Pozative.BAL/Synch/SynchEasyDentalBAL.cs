using System;
using Pozative.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pozative.BAL
{
    public class SynchEasyDentalBAL
    {

        public static bool GetEHRConnection(int Application_ID)
        {
            if (Application_ID == 1)
            {
                return SynchEasyDentalDAL.GetEasyDentalConnection();
            }
            else
            {
                return SynchEasyDentalDAL.GetEasyDentalConnection();
            }

        }


        #region Appointment

        public static DataTable GetEasyDentalAppointmentData()
        {
            return SynchEasyDentalDAL.GetEasyDentalAppointmentData();
        }

        public static DataTable GetEasyDentalAppointmentIds()
        {
            return SynchEasyDentalDAL.GetEasyDentalAppointmentIds();
        }
        public static string GetPatientName(int patientid)
        {
            return SynchEasyDentalDAL.GetPatientName(patientid);
        }

        #region Getting ProcedureData for Appointments
        public static DataTable GetEasyDentalAppointment_Procedures_Data()
        {
            return SynchEasyDentalDAL.GetEasyDentalAppointment_Procedures_Data();
        }

        public static DataTable GetEasyDentalAppointment_Procedures_SecondData()
        {
            return SynchEasyDentalDAL.GetEasyDentalAppointment_Procedures_SecondData();
        }

        //public static DataTable GetEasyDentalAppointment_Procedures_SubData()
        //{
        //    return SynchEasyDentalDAL.GetEasyDentalAppointment_Procedures_SubData();
        //}

        public static DataTable GetEasyDentalAppointment_ApptId_Procedures_Data()
        {
            return SynchEasyDentalDAL.GetEasyDentalAppointment_ApptId_Procedures_Data();
        }
        public static DataTable GetEasyDentalAppointment_Procedures_Type0_Data()
        {
            return SynchEasyDentalDAL.GetEasyDentalAppointment_Procedures_Type0_Data();
        }
        public static DataTable GetEasyDentalAppointment_Procedures_Type1_Data()
        {
            return SynchEasyDentalDAL.GetEasyDentalAppointment_Procedures_Type1_Data();
        }
        public static DataTable GetEasyDentalProcDescForCodeType0_Data()
        {
            return SynchEasyDentalDAL.GetEasyDentalProcDescForCodeType0_Data();
        }
        public static DataTable GetEasyDentalProcDescForCodeType1_Data()
        {
            return SynchEasyDentalDAL.GetEasyDentalProcDescForCodeType1_Data();
        }
        #endregion
        public static DataTable GetEasyDentalAppointmentNoteData(string AppointmentId)
        {
            return SynchEasyDentalDAL.GetEasyDentalAppointmentNoteData(AppointmentId);
        }

        public static DataTable GetEasyDentalApplicationVersion()
        {
            return SynchEasyDentalDAL.GetEasyDentalApplicationVersion();
        }

        public static bool Save_Appointment_EasyDental_To_Local(DataTable dtEasyDentalAppointment)
        {
            return SynchEasyDentalDAL.Save_Appointment_EasyDental_To_Local(dtEasyDentalAppointment);
        }

        #region Deleted Appointment

        public static DataTable GetEasyDentalDeletedAppointmentData()
        {
            return SynchEasyDentalDAL.GetEasyDentalDeletedAppointmentData();
        }
        public static bool Update_DeletedAppointment_EasyDental_To_Local(DataTable dtEasyDentalDeletedAppointment)
        {
            return SynchEasyDentalDAL.Update_DeletedAppointment_EasyDental_To_Local(dtEasyDentalDeletedAppointment);
        }
        #endregion
        #endregion

        #region OperatoryEvent

        public static DataTable GetEasyDentalOperatoryEventData()
        {
            return SynchEasyDentalDAL.GetEasyDentalOperatoryEventData();
        }

        public static bool Save_OperatoryEvent_EasyDental_To_Local(DataTable dtEasyDentalOperatoryEvent)
        {
            return SynchEasyDentalDAL.Save_OperatoryEvent_EasyDental_To_Local(dtEasyDentalOperatoryEvent);
        }

        #endregion

        #region Provider

        #region Provider

        public static DataTable GetEasyDentalProviderData()
        {
            return SynchEasyDentalDAL.GetEasyDentalProviderData();
        }

        public static bool Save_Provider_EasyDental_To_Local(DataTable dtEasyDentalProvider)
        {
            return SynchEasyDentalDAL.Save_Provider_EasyDental_To_Local(dtEasyDentalProvider);
        }

        #endregion    

        #region ProviderOfficeHours

        public static DataTable GetEasyDentalProviderOfficeHours()
        {
            return SynchEasyDentalDAL.GetEasyDentalProviderOfficeHours();
        }

        #endregion

        #region ProvideeCustomeHours

        public static DataTable GetEasyDentalProviderHoursData()
        {
            return SynchEasyDentalDAL.GetEasyDentalProviderHoursData();
        }

        public static bool Save_ProviderHours_EasyDental_To_Local(DataTable dtEasyDentalProviderHours)
        {
            return SynchEasyDentalDAL.Save_ProviderHours_EasyDental_To_Local(dtEasyDentalProviderHours);
        }

        #endregion

        #endregion

        #region Speciality

        public static DataTable GetEasyDentalSpecialityData()
        {
            return SynchEasyDentalDAL.GetEasyDentalSpecialityData();
        }

        public static bool Save_Speciality_EasyDental_To_Local(DataTable dtEasyDentalSpeciality)
        {
            return SynchEasyDentalDAL.Save_Speciality_EasyDental_To_Local(dtEasyDentalSpeciality);
        }

        #endregion

        #region Operatory

        #region Operatory

        public static DataTable GetEasyDentalOperatoryData()
        {
            return SynchEasyDentalDAL.GetEasyDentalOperatoryData();
        }
        public static bool Save_Operatory_EasyDental_To_Local(DataTable dtEasyDentalOperatory)
        {

            return SynchEasyDentalDAL.Save_Operatory_EasyDental_To_Local(dtEasyDentalOperatory);
        }

        #endregion

        #region OperatoryCustomeHours

        public static DataTable GetEasyDentalOperatoryHoursData()
        {
            return SynchEasyDentalDAL.GetEasyDentalOperatoryHoursData();
        }
        public static bool Save_OperatoryHours_EasyDental_To_Local(DataTable dtEasyDentalOperatoryHours)
        {
            return SynchEasyDentalDAL.Save_OperatoryHours_EasyDental_To_Local(dtEasyDentalOperatoryHours);
        }

        #endregion

        #region OperatoryOfficeHours

        public static DataTable GetEasyDentalOperatoryOfficeHours()
        {
            return SynchEasyDentalDAL.GetEasyDentalOperatoryOfficeHours();
        }

        #endregion

        #endregion

        #region ApptType

        public static DataTable GetEasyDentalApptTypeData()
        {
            return SynchEasyDentalDAL.GetEasyDentalApptTypeData();
        }

        public static bool Save_ApptType_EasyDental_To_Local(DataTable dtEasyDentalApptType)
        {
            return SynchEasyDentalDAL.Save_ApptType_EasyDental_To_Local(dtEasyDentalApptType);
        }

        #endregion

        #region Patient

        public static DataTable GetEasyDentalPatientData()
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientData();
        }

        public static DataTable GetEasyDentalPatientData(string PatientEHRIDs)
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientData(PatientEHRIDs);
        }
        public static DataTable GetEasyDentalPatientData(Int32 PatientId)
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientData(PatientId);
        }
        public static DataTable GetEasyDentalPatientdue_date()
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientdue_date();
        }
        public static DataTable GetEasyDentalPatientdue_date(string PatientEHRIDs)
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientdue_date(PatientEHRIDs);
        }
        public static DataTable GetEasyDentalPatientcollect_payment(string PatientId)
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientcollect_payment(PatientId);
        }
        public static DataTable GetEasyDentalPatient_RecallType()
        {
            return SynchEasyDentalDAL.GetEasyDentalPatient_RecallType();
        }
        public static DataTable GetEasyDentalPatientNextApptDate()
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientNextApptDate();
        }
        public static DataTable GetEasyDentalPatientNextApptDate(string PatientEHRIDs)
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientNextApptDate(PatientEHRIDs);
        }
        public static bool Save_Patient_EasyDental_To_Local(DataTable dtEasyDentalPatient, string InsertTableName, DataTable dtEasyDentalPatientNextApptDate, DataTable dtEasyDentalPatientdue_date, DataTable dtLocalPatient, bool bSetDeleted = false)
        {
            return SynchEasyDentalDAL.Save_Patient_EasyDental_To_Local(dtEasyDentalPatient, InsertTableName, dtEasyDentalPatientNextApptDate, dtEasyDentalPatientdue_date, dtLocalPatient, bSetDeleted);
        }
        public static bool Save_ApptPatient_EasyDental_To_Local(DataTable dtEasyDentalPatient, string InsertTableName, DataTable dtEasyDentalPatientNextApptDate, DataTable dtEasyDentalPatientdue_date)
        {
            return SynchEasyDentalDAL.Save_ApptPatient_EasyDental_To_Local(dtEasyDentalPatient, InsertTableName, dtEasyDentalPatientNextApptDate, dtEasyDentalPatientdue_date);
        }

        public static DataTable GetEasyDentalPatientStatusAppointmentData(string PatientEHRIDs)
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientStatusAppointmentData(PatientEHRIDs);
        }
        #endregion

        #region  Disease

        public static DataTable GetEasyDentalDiseaseData()
        {
            return SynchEasyDentalDAL.GetEasyDentalDiseaseData();
        }

        public static bool Save_Disease_EasyDental_To_Local(DataTable dtEasyDentalDisease)
        {
            return SynchEasyDentalDAL.Save_Disease_EasyDental_To_Local(dtEasyDentalDisease);
        }

        #endregion

        #region "Medication"
        public static DataTable GetEasyDentalMedicationData()
        {
            return SynchEasyDentalDAL.GetEasyDentalMedicationData();
        }
        public static bool SavePatientMedicationLocalToEasyDental()
        {
            return SynchEasyDentalDAL.SavePatientMedicationLocalToEasyDental();
        }
        #endregion

        #region RecallType

        public static DataTable GetEasyDentalRecallTypeData()
        {
            return SynchEasyDentalDAL.GetEasyDentalRecallTypeData();
        }

        public static bool Save_RecallType_EasyDental_To_Local(DataTable dtEasyDentalRecallType)
        {
            return SynchEasyDentalDAL.Save_RecallType_EasyDental_To_Local(dtEasyDentalRecallType);
        }

        #endregion

        #region User
        public static DataTable GetEasyDentalUserData()
        {
            return SynchEasyDentalDAL.GetEasyDentalUserData();
        }

        public static bool Save_User_EasyDental_To_Local(DataTable dtEasyDentalUser)
        {
            return SynchEasyDentalDAL.Save_User_EasyDental_To_Local(dtEasyDentalUser);
        }

        #endregion

        #region Speciality

        public static DataTable GetEasyDentalApptStatusData()
        {
            return SynchEasyDentalDAL.GetEasyDentalApptStatusData();
        }

        public static bool Save_ApptStatus_EasyDental_To_Local(DataTable dtEasyDentalApptStatus)
        {
            return SynchEasyDentalDAL.Save_ApptStatus_EasyDental_To_Local(dtEasyDentalApptStatus);
        }

        #endregion

        #region Holidays

        public static DataTable GetEasyDentalHolidaysData()
        {
            return SynchEasyDentalDAL.GetEasyDentalHolidaysData();
        }
        public static DataTable GetEasyDentalOperatoryHolidaysData(DataTable dtOperatory)
        {
            return SynchEasyDentalDAL.GetEasyDentalOperatoryHolidaysData(dtOperatory);
        }
        public static bool Save_Holidays_EasyDental_To_Local(DataTable dtEasyDentalOperatoryEvent)
        {
            return SynchEasyDentalDAL.Save_Holidays_EasyDental_To_Local(dtEasyDentalOperatoryEvent);
        }
        public static bool Save_Opeatory_Holidays_EasyDental_To_Local(DataTable dtEasyDentalOperatoryEvent)
        {
            return SynchEasyDentalDAL.Save_Opeatory_Holidays_EasyDental_To_Local(dtEasyDentalOperatoryEvent);
        }

        #endregion

        #region CreateAppointment

        public static DataTable GetEasyDentalPatientID_NameData()
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientID_NameData();
        }
        public static string SetFormatePhoneNumber(string Phonenumber)
        {
            return SynchEasyDentalDAL.SetFormatePhoneNumber(Phonenumber);
        }
        public static DataTable GetEasyDentalPatientStatusData(string PatientEHRIDs)
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientStatusData(PatientEHRIDs);
        }


        public static DataTable GetEasyDentalIdelProvider()
        {
            return SynchEasyDentalDAL.GetEasyDentalIdelProvider();
        }

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate)
        {
            return SynchEasyDentalDAL.GetBookOperatoryAppointmenetWiseDateTime(ApptDate);
        }
        public static int Save_Patient_Local_To_EasyDental(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, int tmpPatient_Gur_id, string Birth_Date)
        {
            try
            {
                return SynchEasyDentalDAL.Save_Patient_Local_To_EasyDental(LastName, FirstName, MiddleName, Mobile, Email, PriProv, DateFirstVisit, tmpPatient_Gur_id, Birth_Date);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public static int Save_Appointment_Local_To_EasyDental(string PatNum, int Pattern, string Op, string ProvNum, string AptDateTime, string DateTStamp, string AppointmentTypeNum, string PatientName, string ApptComment)
        {
            return SynchEasyDentalDAL.Save_Appointment_Local_To_EasyDental(PatNum, Pattern, Op, ProvNum, AptDateTime, DateTStamp, AppointmentTypeNum, PatientName, ApptComment);
        }
        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id)
        {
            return SynchEasyDentalDAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id, tmpAppt_Web_id);
        }
        public static bool Update_Status_EHR_Appointment_Live_To_EasyDentalEHR(DataTable dtLiveAppointment,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchEasyDentalDAL.Update_Status_EHR_Appointment_Live_To_EasyDentalEHR(dtLiveAppointment,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
        }
        public static bool Update_Receive_SMS_Patient_EHR_Live_To_EasyDentalEHR(DataTable dtLiveAppointment, string Locationid, string Loc_ID,string _filename_EHR_patientoptout= "",string _EHRLogdirectory_EHR_patientoptout = "")
        {
            return SynchEasyDentalDAL.Update_Receive_SMS_Patient_EHR_Live_To_EasyDentalEHR(dtLiveAppointment, Locationid, Loc_ID,_filename_EHR_patientoptout,_EHRLogdirectory_EHR_patientoptout);
        }
        #endregion

        #region Patient_Form
        public static bool Save_Patient_Form_Local_To_EasyDental(DataTable dtWebPatient_Form)
        {
            return SynchEasyDentalDAL.Save_Patient_Form_Local_To_EasyDental(dtWebPatient_Form);
        }

        public static bool Save_Document_in_EasyDental()
        {
            return SynchEasyDentalDAL.Save_Document_in_EasyDental();
        }

        public static DataTable GetEasyDentalMedicleQuestionData()
        {
            return SynchEasyDentalDAL.GetEasyDentalMedicleQuestionData();
        }


        public static bool SaveMedicalHistoryLocalToEasyDental()
        {
            return SynchEasyDentalDAL.SaveMedicalHistoryLocalToEasyDental();
        }
        public static bool SaveDiseaseLocalToEasyDental()
        {
            return SynchEasyDentalDAL.SaveDiseaseLocalToEasyDental();
        }

        //public static string Save_PatientPaymentLog_LocalToEasyDental(DataTable dtWebPatientPayment)
        //{
        //    return SynchEasyDentalDAL.Save_PatientPaymentLog_LocalToEasyDental(dtWebPatientPayment);
        //}

        public static string Save_PatientSMSCallLog_LocalToEasyDental(DataTable dtWebPatientPayment)
        {
            return SynchEasyDentalDAL.Save_PatientSMSCallLog_LocalToEasyDental(dtWebPatientPayment);
        }
        public static DataTable GetEasyDentalPatientImagesData(string connectionString, string imagePathName)
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientImagesData(connectionString, imagePathName);
        }


        public static void DeleteDuplicatePatientLog()
        {
            SynchEasyDentalDAL.DeleteDuplicatePatientLog();
        }

        public static Int64 SavePatientPaymentTOEHR(DataTable dtTable, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            return SynchEasyDentalDAL.SavePatientPaymentTOEHR(dtTable, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
        }

        public static string GetPatientGuarid(string Patid)
        {
            return SynchEasyDentalDAL.GetPatientGuarid(Patid);
        }

        //public static Int64 SavePatientPaymentTOEHR(DataTable dtTable, DataTable splittable)
        //{
        //    return SynchEasyDentalDAL.SavePatientPaymentTOEHR(dtTable, splittable);
        //}

        public static bool SaveMedicationLocalToEasyDental(ref bool isRecordSaved, ref string SavePatientEHRID)
        {
            return SynchEasyDentalDAL.SaveMedicationLocalToEasyDental(ref isRecordSaved, ref SavePatientEHRID);
        }

        public static DataTable GetEasyDentalPatientMedication(string Patient_EHR_IDS)
        {
            return SynchEasyDentalDAL.GetEasyDentalPatientMedication(Patient_EHR_IDS);
        }

        public static bool DeleteMedicationLocalToEasyDental(ref bool isRecordDeleted, ref string DeletePatientEHRID)
        {
            return SynchEasyDentalDAL.DeleteMedicationLocalToEasyDental(ref isRecordDeleted, ref DeletePatientEHRID);
        }

        #endregion









    }
}
