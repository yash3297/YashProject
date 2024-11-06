using System;
using Pozative.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pozative.BAL
{
    public class SynchEaglesoftBAL
    {

        #region ActualVersionNumber


        public static string GetEaglesoftEHR_VersionNumber(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftEHR_VersionNumber(DbString);
        }
            

        #endregion 

        #region Appointment

        public static DataTable GetEaglesoftAppointmentData(string DbString, string strApptID = "")
        {
            return SynchEaglesoftDAL.GetEaglesoftAppointmentData(DbString, strApptID);
        }
       
        public static DataTable GetEaglesoftAppointmentIds(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftAppointmentIds(DbString);
        }

        public static DataTable GetEaglesoftAppointment_Procedures_Data(string DbString, string strApptID = "")
        {
            return SynchEaglesoftDAL.GetEaglesoftAppointment_Procedures_Data(DbString, strApptID);
        }

        public static DataTable GetEaglesoftAppointmentProviderData(string Appointment_EHR_ID, string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftAppointmentProviderData(Appointment_EHR_ID, DbString);
        }

        public static DataTable GetEaglesoftAppointmentProviderData_New(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftAppointmentProviderData_New(DbString);
        }

        public static bool Save_Appointment_Eaglesoft_To_Local(DataTable dtEaglesoftAppointment, string Service_Install_Id)
        {

            return SynchEaglesoftDAL.Save_Appointment_Eaglesoft_To_Local(dtEaglesoftAppointment, Service_Install_Id);
        }

        public static bool Update_Status_EHR_Appointment_Live_To_Eaglesoft(DataTable dtLiveAppointment, string DbString, string LocationId,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchEaglesoftDAL.Update_Status_EHR_Appointment_Live_To_Eaglesoft(dtLiveAppointment, DbString, LocationId,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
        }
        public static bool Update_Receive_SMS_Patient_EHR_Live_To_Eaglesoft(DataTable dtLiveAppointment, string DbString, string LocationId,string LocId,string _filename_EHR_patientoptout = "",string _EHRLogdirectory_EHR_patientoptout = "")
        {
            return SynchEaglesoftDAL.Update_Receive_SMS_Patient_EHR_Live_To_Eaglesoft(dtLiveAppointment, DbString, LocationId,LocId,_filename_EHR_patientoptout,_EHRLogdirectory_EHR_patientoptout);
        }

        public static DataTable GetEagleSoftCustPrompts(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftCustPrompts(DbString);
        }

        #endregion

        #region  OperatoryEvent

        public static DataTable GetEaglesoftOperatoryEventData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftOperatoryEventData(DbString);
        }

        public static DataTable GetEaglesoftOperatoryChairData(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftOperatoryChairData(DbString);
        }

        public static DataTable GetEaglesoftOperatoryHours(string DbString, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.GetEaglesoftOperatoryHours(DbString, Service_Install_Id);
        }

        public static bool Save_OperatoryEvent_Eaglesoft_To_Local(DataTable dtEaglesoftOperatoryEvent, string Service_Install_Id)
        {

            return SynchEaglesoftDAL.Save_OperatoryEvent_Eaglesoft_To_Local(dtEaglesoftOperatoryEvent, Service_Install_Id);
        }

        public static bool Save_OperatoryDayOff_Eaglesoft_To_Local(DataTable dtEaglesoftOperatoryEvent, string Service_Install_Id)
        {

            return SynchEaglesoftDAL.Save_OperatoryDayOff_Eaglesoft_To_Local(dtEaglesoftOperatoryEvent, Service_Install_Id);
        }

        public static DataTable GetPatientListFromEagleSoft(string DbString)
        {
            return SynchEaglesoftDAL.GetPatientListFromEagleSoft(DbString);
        }
        public static DataTable GetEagleSoftIdelProvider(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftIdelProvider(DbString);
        }
        #endregion

        #region Synch Provider

        public static DataTable GetEaglesoftProviderData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftProviderData(DbString);
        }

        public static bool Save_Provider_Eaglesoft_To_Local(DataTable dtEaglesoftProvider, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_Provider_Eaglesoft_To_Local(dtEaglesoftProvider, Service_Install_Id);
        }

        #endregion

        #region Speciality

        public static DataTable GetEaglesoftSpecilityData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftSpecialityData(DbString);
        }

        public static bool Save_Speciality_Eaglesoft_To_Local(DataTable dtEaglesoftSpeciality, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_Speciality_Eaglesoft_To_Local(dtEaglesoftSpeciality, Service_Install_Id);
        }

        #endregion

        #region Operatory

        public static DataTable GetEaglesoftOperatoryData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftOperatoryData(DbString);
        }
        public static DataTable GetEaglesoftDeletedOperatoryData(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftDeletedOperatoryData(DbString);
        }

        public static bool Save_Operatory_Eaglesoft_To_Local(DataTable dtEaglesoftOperatory, string Service_Install_Id)
        {

            return SynchEaglesoftDAL.Save_Operatory_Eaglesoft_To_Local(dtEaglesoftOperatory, Service_Install_Id);
        }

        #endregion

        #region Folder List

        public static DataTable GetEaglesoftFolderListData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftFolderListData(DbString);
        }

        public static bool Save_FolderList_Eaglesoft_To_Local(DataTable dtEaglesoftOperatory, string Service_Install_Id, string clinicNumber)
        {

            return SynchEaglesoftDAL.Save_FolderList_Eaglesoft_To_Local(dtEaglesoftOperatory, Service_Install_Id, clinicNumber);
        }

        public static DataTable GetEaglesoftDeletedFolderListData(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftDeletedFolderListData(DbString);
        }

        #endregion

        #region  Appointment Type

        public static DataTable GetEaglesoftApptTypeData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftApptTypeData(DbString);
        }

        public static bool Save_ApptType_Eaglesoft_To_Local(DataTable dtEaglesoftApptType, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_ApptType_Eaglesoft_To_Local(dtEaglesoftApptType, Service_Install_Id);
        }

        #endregion

        #region  Patient
        public static DataTable GetEaglesoftInsertPatientData(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftInsertPatientData(DbString);
        }
        public static DataTable GetEaglesoftPatientData(string DbString, string strPatIDs = "")
        {
            return SynchEaglesoftDAL.GetEaglesoftPatientData(DbString, strPatIDs);
        }

        public static DataTable GetEaglesoftAppointmentsPatientData(string DbString, string strPatID = "")
        {
            return SynchEaglesoftDAL.GetEaglesoftAppointmentsPatientData(DbString, strPatID);
        }

        public static DataTable GetAllEaglesoftPatientRecallTpyeDueDate(string DbString)
        {
            return SynchEaglesoftDAL.GetAllEaglesoftPatientRecallTpyeDueDate(DbString);
        }

        public static DataTable GetEaglesoftPatientRecallTpyeDueDate(string Patient_EHR_ID, string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftPatientRecallTpyeDueDate(Patient_EHR_ID, DbString);
        }

        public static bool Save_Patient_Eaglesoft_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id, bool isApptPatient)
        {
            return SynchEaglesoftDAL.Save_Patient_Eaglesoft_To_Local_New(dtOpenDentalDataToSave, Clinic_Number, Service_Install_Id, isApptPatient);
        }
        public static bool Save_Patient_Eaglesoft_To_Local(DataTable dtEaglesoftPatient, string InsertTableName, DataTable dtLocalPatient, string DbString, string Service_Install_Id, bool bSetDeleted = false)
        {
            //DataTable dtEaglesoftPatientRecallTpyeDueDate = SynchEaglesoftBAL.GetAllEaglesoftPatientRecallTpyeDueDate(DbString);
            return SynchEaglesoftDAL.Save_Patient_Eaglesoft_To_Local(dtEaglesoftPatient, InsertTableName, dtLocalPatient, Service_Install_Id, bSetDeleted);
        }

        #endregion

        #region  RecallType

        public static DataTable GetEaglesoftRecallTypeData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftRecallTypeData(DbString);
        }

        public static bool Save_RecallType_Eaglesoft_To_Local(DataTable dtEaglesoftRecallType, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_RecallType_Eaglesoft_To_Local(dtEaglesoftRecallType, Service_Install_Id);
        }

        #endregion

        #region User
       
        public static DataTable GetEaglesoftUserData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftUserData(DbString);
        }

        public static bool Save_User_Eaglesoft_To_Local(DataTable dtEaglesoftUser, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_User_Eaglesoft_To_Local(dtEaglesoftUser, Service_Install_Id);
        }
        #endregion

        #region  Appointment Status

        public static bool Save_AppointmentStatus_Eaglesoft_To_Local(DataTable dtEaglesoftAppointmentStatus, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_AppointmentStatus_Eaglesoft_To_Local(dtEaglesoftAppointmentStatus, Service_Install_Id);
        }

        #endregion

        #region Holidays

        public static DataTable GetEaglesoftHolidaysData(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftHolidaysData(DbString);
        }

        public static bool Save_Holidays_Eaglesoft_To_Local(DataTable dtEaglesoftOperatoryEvent, string Service_Install_Id)
        {

            return SynchEaglesoftDAL.Save_Holidays_Eaglesoft_To_Local(dtEaglesoftOperatoryEvent, Service_Install_Id);
        }

        #endregion

        #region PatientForm

        public static DataTable GetInsuratnce_CompanyName(string DbString)
        {
            return SynchEaglesoftDAL.GetInsuratnce_CompanyName(DbString);
        }

        public static string Save_Patient_Local_To_EagleSoft(string LastName, string FirstName, string MiddleName, string MobileNo, string Email, string ApptProv, string AppointmentDateTime, string Patient_Gur_id, int OperatoryId, string Birth_Date, string DbString)
        {
            return SynchEaglesoftDAL.Save_Patient_Local_To_EagleSoft(LastName, FirstName, MiddleName, MobileNo, Email, ApptProv, AppointmentDateTime, Patient_Gur_id, OperatoryId, Birth_Date, DbString);
        }

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime dateTime, string DbString)
        {
            return SynchEaglesoftDAL.GetBookOperatoryAppointmenetWiseDateTime(dateTime, DbString);
        }

        public static Int64 Save_Appointment_Local_To_EagleSoft(string FirstNameLastName, DateTime AppointmentStartTime, DateTime AppointmentEndTime, string PatNum, string OperatoryId,
            string classification, string ApptTypeId, DateTime AppointedDateTime, string ProvNum, string AppointmentConfirmationStatus, string apptcomment,
            bool allday_event, bool sooner_if_possible, bool privateAppointment, bool auto_confirm_sent, string TreatmentCodes, string DbString)
        {
            return SynchEaglesoftDAL.Save_Appointment_Local_To_EagleSoft(FirstNameLastName, AppointmentStartTime, AppointmentEndTime, PatNum, OperatoryId, classification, ApptTypeId, AppointedDateTime, ProvNum, AppointmentConfirmationStatus, apptcomment, allday_event, sooner_if_possible, privateAppointment, auto_confirm_sent, TreatmentCodes, DbString);
        }

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string AppointmentEHRId, string AppointmentWebId, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Update_Appointment_EHR_Id_Web_Book_Appointment(AppointmentEHRId, AppointmentWebId, Service_Install_Id);
        }

        public static bool Save_Patient_Form_Local_To_EagleSoft(DataTable dtWebPatient_Form, string DbString, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_Patient_Form_Local_To_EagleSoft(dtWebPatient_Form, DbString, Service_Install_Id);
        }

        public static bool Save_Document_in_EagleSoft(string DbString, string Service_Install_Id, string DocumentPath, string strPatientFormID = "")
        {
            return SynchEaglesoftDAL.Save_Document_in_EagleSoft(DbString, Service_Install_Id, DocumentPath, strPatientFormID);
        }

       
        public static string GetEagleSoftDocPath(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftDocPath(DbString);
        }

        #endregion

        #region Treatment Document

        public static bool Save_Treatment_Document_in_EagleSoft(string DbString, string Service_Install_Id, string DocumentPath, string strTreatmentPlanID = "")
        {
            return SynchEaglesoftDAL.Save_Treatment_Document_in_EagleSoft(DbString, Service_Install_Id, DocumentPath, strTreatmentPlanID);
        }

        #endregion

        #region Insurance Carrier

        public static bool Save_InsuranceCarrier_Document_in_EagleSoft(string DbString, string Service_Install_Id, string DocumentPath, string strInsuranceCarrierID = "")
        {
            return SynchEaglesoftDAL.Save_InsuranceCarrier_Document_in_EagleSoft(DbString, Service_Install_Id, DocumentPath, strInsuranceCarrierID);
        }

        #endregion

        #region ProviderOFficeHours

        public static DataTable GetEagleSoftProviderOfficeHours(string DbString, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.GetEagleSoftProviderOfficeHours(DbString, Service_Install_Id);
        }

        public static DataTable GetEagleSoftOperatoryOfficeHours(string DbString, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.GetEagleSoftOperatoryOfficeHours(DbString, Service_Install_Id);
        }

        public static bool Save_ProviderOfficeHours_Eaglesoft_To_Local(DataTable dtEagleSoftProviderOfficeHours, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_ProviderOfficeHours_Eaglesoft_To_Local(dtEagleSoftProviderOfficeHours, Service_Install_Id);
        }

        public static DataTable GetEagleSoftProviderHours(string DbString, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.GetEagleSoftProviderHours(DbString, Service_Install_Id);
        }

        public static DataTable GetEagleSoftPatientDiseaseData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftPatientDiseaseData(DbString);
        }

        public static DataTable GetEagleSoftPatientMedicationData(string DbString, string Patient_EHR_IDS)
        {
            return SynchEaglesoftDAL.GetEagleSoftPatientMedicationData(DbString, Patient_EHR_IDS);
        }
        public static DataTable GetEagleSoftDiseaseData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftDiseaseData(DbString);
        }
        public static DataTable GetEagleSoftMedicationData(string DbString)
        {
            return SynchEaglesoftDAL.GetEagleSoftMedicationData(DbString);
        }

        public static bool Save_Disease_EagleSoft_To_Local(DataTable dtEagleSoftDisease, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_Disease_EagleSoft_To_Local(dtEagleSoftDisease, Service_Install_Id);
        }

        public static bool Save_PatientDisease_EagleSoft_To_Local(DataTable dtEagleSoftpatientDisease, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_PatientDisease_EagleSoft_To_Local(dtEagleSoftpatientDisease, Service_Install_Id);
        }

        #endregion

        #region Get Medical History
        public static DataSet GetEaglesoftMedicalHistoryData(string DbString, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.GetEaglesoftMedicalHistoryData(DbString, Service_Install_Id);
        }
        public static DataTable GetPatientWiseMedicalForm(string DbString, string strPatientID = "")
        {
            return SynchEaglesoftDAL.GetPatientWiseMedicalForm(DbString, strPatientID);
        }
        #endregion


        public static bool SaveAllergiesToEaglesoft(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchEaglesoftDAL.SaveAllergiesToEaglesoft(DbString, Service_Install_Id, strPatientFormID);
        }
        public static bool DeleteAllergiesToEaglesoft(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchEaglesoftDAL.DeleteAllergiesToEaglesoft(DbString, Service_Install_Id, strPatientFormID);
        }
        public static DataTable GetPatientWiseRecall(string DbString, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.GetPatientWiseRecall(DbString, Service_Install_Id);
        }

        public static bool SavePatientWiseRecallType_Eaglesoft_To_Local(DataTable dtEaglesoftRecallType, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.SavePatientWiseRecallType_Eaglesoft_To_Local(dtEaglesoftRecallType, Service_Install_Id);
        }

        public static Int64 SavePatientPaymentTOEHR(string DbString, DataTable dtTable,string ServiceInstallationId,string _filename_EHR_Payment="", string _EHRLogdirectory_EHR_Payment="")
        {
            return SynchEaglesoftDAL.SavePatientPaymentTOEHR(DbString, dtTable, ServiceInstallationId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
        }

        //public static string Save_PatientPaymentLog_LocalToEagelsoft(DataTable dtWebPatientPayment, string dbString, string ServiceInstallationId)
        //{
        //    return SynchEaglesoftDAL.Save_PatientPaymentLog_LocalToEagelsoft(dtWebPatientPayment, dbString, ServiceInstallationId);
        //}
        public static string Save_PatientSMSCallLog_LocalToEagelsoft(DataTable dtWebPatientPayment, string dbString, string ServiceInstallationId)
        {
            return SynchEaglesoftDAL.Save_PatientSMSCallLog_LocalToEagelsoft(dtWebPatientPayment, dbString, ServiceInstallationId);
        }

        public static DataTable GetEaglesoftPatientStatusData(string clinicNumber, string dbString, string strPatID = "")
        {
            return SynchEaglesoftDAL.GetEaglesoftPatientStatusData(clinicNumber, dbString, strPatID);
        }

        public static DataTable GetEaglesoftPatientImagesData(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftPatientImagesData("0", DbString);
        }

        public static void DeleteDuplicatePatientLog(string DbConnectionString, string ServiceInstalltionId)
        {
            SynchEaglesoftDAL.DeleteDuplicatePatientLog(DbConnectionString, ServiceInstalltionId);
        }

        public static bool SaveMedicationToEaglesoft(string DbString, string Service_Install_Id, ref bool isRecordSaved, ref string Save_Patient_EHR_ids, string strPatientFormID = "")
        {
            return SynchEaglesoftDAL.SaveMedicationToEaglesoft(DbString, Service_Install_Id, ref isRecordSaved, ref Save_Patient_EHR_ids, strPatientFormID);
        }

        public static bool DeleteMedicationToEaglesoft(string DbString, string Service_Install_Id, ref bool isRecordsDeleted, ref string Delete_Patient_EHR_ids, string strPatientFormID = "")
        {
            return SynchEaglesoftDAL.DeleteMedicationToEaglesoft(DbString, Service_Install_Id, ref isRecordsDeleted, ref Delete_Patient_EHR_ids, strPatientFormID);
        }
        public static bool DecryptSSN(ref System.Data.DataTable dtData)
        {
            return SynchEaglesoftDAL.DecryptSSN(ref dtData);
        }

        #region Insurance
        public static DataTable GetEaglesoftInsuranceData(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftInsuranceData(DbString);
        }

        public static bool Save_Insurance_Eaglesoft_To_Local(DataTable dtEaglesoftInsurance, string Service_Install_Id)
        {
            return SynchEaglesoftDAL.Save_Insurance_Eaglesoft_To_Local(dtEaglesoftInsurance, Service_Install_Id);
        }

        #endregion
    }
}
