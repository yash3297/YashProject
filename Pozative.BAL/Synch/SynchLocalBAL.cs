using Pozative.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pozative.BAL
{
    public class SynchLocalBAL
    {

        public static bool Update_Sync_Table_Datetime(string TableName)
        {
            return SynchLocalDAL.Update_Sync_Table_Datetime(TableName);
        }

        public static bool Sync_check_for_treatmentDoct(string TreatmentPlanId,string _filename_EHR_treatmentplan_document="",string _EHRLogdirectory_EHR_treatmentplan_document="" )
        {
            return SynchLocalDAL.Sync_check_for_treatmentDoct(TreatmentPlanId,_filename_EHR_treatmentplan_document,_EHRLogdirectory_EHR_treatmentplan_document);
        }

        public static void CreateRecordForTreatmentDoc(string PatientName, string SubmittedDate, string Patient_EHR_ID, string Patient_Web_ID, string TreatmentPlanId, string TreatmentPlanName, string Clinic_Number, string Service_Install_Id,string _filename_EHR_treatmentplan_document="",string _EHRLogdirectory_EHR_treatmentplan_document="")
        {
          

            SynchLocalDAL.CreateRecordForTreatmentDoc(PatientName, SubmittedDate, Patient_EHR_ID, Patient_Web_ID, TreatmentPlanId, TreatmentPlanName, Clinic_Number, Service_Install_Id,_filename_EHR_treatmentplan_document,_EHRLogdirectory_EHR_treatmentplan_document);
        }
        public static void UpdateTreatmentDocInlocal(string TreatmentID,string _filename_EHR_treatmentplan_document = "",string _EHRLogdirectory_EHR_treatmentplan_document = "")
        {
            SynchLocalDAL.UpdateTreatmentDocInlocal(TreatmentID,_filename_EHR_treatmentplan_document,_EHRLogdirectory_EHR_treatmentplan_document);
        }


        public static void UPDATERecordForTreatmentDoc(string TreatmentPlan_Id, string FileName,string _filename_EHR_treatmentplan_document="",string _EHRLogdirectory_EHR_treatmentplan_document="")
        {
            SynchLocalDAL.UPDATERecordForTreatmentDoc(TreatmentPlan_Id, FileName,_filename_EHR_treatmentplan_document,_EHRLogdirectory_EHR_treatmentplan_document);
        }

        public static DataTable ChangeStatusForTreatmentDoc(String Status)
        {
            return SynchLocalDAL.ChangeStatusForTreatmentDoc(Status);
        }


        #region Appointment

        public static DataTable GetLocalAppointmentData(string Service_Install_Id, string Appt_EHR_ID = "")
        {
            return SynchLocalDAL.GetLocalAppointmentData(Service_Install_Id, Appt_EHR_ID);
        }

        public static DataTable GetLocalAppointmentData_AllRecords(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalAppointmentData_AllRecords(Service_Install_Id);
        }

        public static DataTable GetLocalCompareForDeletedAppointmentData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalCompareForDeletedAppointmentData(Service_Install_Id);
        }

        public static DataTable GetLocalAppointmentConformStatusData(string appointment_id, string Service_Install_Id,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchLocalDAL.GetLocalAppointmentConformStatusData(appointment_id, Service_Install_Id,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
        }

        public static bool UpdateLocalAppointmentConformStatusData(string appointment_id, string Service_Install_Id)
        {
            return SynchLocalDAL.UpdateLocalAppointmentConformStatusData(appointment_id, Service_Install_Id);
        }

        public static DataTable GetLocalNewWebAppointmentData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalNewWebAppointmentData(Service_Install_Id);
        }

        public static DataTable GetLastTwoDaysLocalAppointmentData(string strApptID = "")
        {
            return SynchLocalDAL.GetLastTwoDaysLocalAppointmentData(strApptID);
        }

        public static DataTable GetIs_Appt_DoubleBook_AppointmentData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetIs_Appt_DoubleBook_AppointmentData(Service_Install_Id);
        }

        public static DataTable GetLocalPozativeAppointmentData()
        {
            return SynchLocalDAL.GetLocalPozativeAppointmentData();
        }

        public static bool Save_Appointment_Is_Appt_DoubleBook_In_Local(string Appt_Web_ID, string Service_Install_Id, DataTable dtApponitmentConflict = null, string appointment_EHR_id = "", string locationid = "")
        {
            return SynchLocalDAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(Appt_Web_ID, Service_Install_Id, dtApponitmentConflict, appointment_EHR_id, locationid);
        }


        #endregion

        #region  OperatoryEvent

        public static DataTable GetLocalOperatoryEventData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalOperatoryEventData(Service_Install_Id);
        }

        public static DataTable GetLocalOperatoryChairData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalOperatoryChairData(Service_Install_Id);
        }

        public static DataTable GetPushLocalOperatoryEventData()
        {
            return SynchLocalDAL.GetPushLocalOperatoryEventData();
        }

        public static DataTable GetPushLocalOperatoryDayOffData()
        {
            return SynchLocalDAL.GetPushLocalOperatoryDayOffData();
        }

        public static DataTable DeleteLocalOperatoryEventData(string Service_Install_Id)
        {
            return SynchLocalDAL.DeleteLocalOperatoryEventData(Service_Install_Id);
        }

        public static void DeleteLocalOperatoryEventDataAll()
        {
            SynchLocalDAL.DeleteLocalOperatoryEventDataAll();
        }

        #endregion

        #region  Provider

        public static DataTable GetLocalProviderData(string Clinic_Number, string ServiceInstallId)
        {
            return SynchLocalDAL.GetLocalProviderData(Clinic_Number, ServiceInstallId);
        }

        public static DataTable GetPushLocalProviderData()
        {
            return SynchLocalDAL.GetPushLocalProviderData();
        }

        #endregion

        #region ProviderHours

        public static DataTable GetLocalProviderHoursData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalProviderHoursData(Service_Install_Id);
        }

        public static DataTable GetPushLocalProviderHoursData()
        {
            return SynchLocalDAL.GetPushLocalProviderHoursData();
        }

        public static DataTable DeleteAbeldentLocalProviderHoursData(string Service_Install_Id)
        {
            return SynchLocalDAL.DeleteAbeldentLocalProviderHoursData(Service_Install_Id);
        }

        public static void DeleteAbeldentLocalProviderHoursAll()
        {
            SynchLocalDAL.DeleteAbeldentLocalProviderHoursAll();
        }

        #endregion

        #region  Speciality

        public static DataTable GetLocalSpecialityData(string ServiceInstallId)
        {
            return SynchLocalDAL.GetLocalSpecialityData(ServiceInstallId);
        }

        public static DataTable GetPushLocalSpecialityData()
        {
            return SynchLocalDAL.GetPushLocalSpecialityData();
        }

        #endregion

        #region FolderList
        public static DataTable GetLocalFolderListData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalFolderListData(Service_Install_Id);
        }
        public static DataTable GetPushLocalFolderListData()
        {
            return SynchLocalDAL.GetPushLocalFolderListData();
        }
        #endregion

        #region  Operatory

        public static DataTable GetLocalOperatoryData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalOperatoryData(Service_Install_Id);
        }

        public static DataTable GetPushLocalOperatoryData()
        {
            return SynchLocalDAL.GetPushLocalOperatoryData();
        }

        #endregion

        #region OperatoryHours

        public static DataTable GetLocalOperatoryHoursData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalOperatoryHoursData(Service_Install_Id);
        }

        public static DataTable GetPushLocalOperatoryHoursData()
        {
            return SynchLocalDAL.GetPushLocalOperatoryHoursData();
        }

        public static DataTable DeleteLocalOperatoryHoursData(string Service_Install_Id)
        {
            return SynchLocalDAL.DeleteLocalOperatoryHoursData(Service_Install_Id);
        }

        public static void DeleteLocalOperatoryHoursAll()
        {
            SynchLocalDAL.DeleteLocalOperatoryHoursAll();
        }

        #endregion

        #region OperatoryOfficeHours

        public static DataTable GetLocalOperatoryOfficeHoursData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalOperatoryOfficeHoursData(Service_Install_Id);
        }

        public static DataTable GetPushLocalOperatoryOfficeHoursData()
        {
            return SynchLocalDAL.GetPushLocalOperatoryOfficeHoursData();
        }


        #endregion

        #region  Appointment Type

        public static DataTable GetLocalApptTypeData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalApptTypeData(Service_Install_Id);
        }

        public static DataTable GetPushLocalApptTypeData()
        {
            return SynchLocalDAL.GetPushLocalApptTypeData();
        }

        #endregion

        #region  Patient

        public static DataTable GetLocalPatientData(string Service_Install_Id, string Pat_EHR_ID = "")
        {
            return SynchLocalDAL.GetLocalPatientData(Service_Install_Id, Pat_EHR_ID);
        }
        public static DataTable GetLocalNewPatientData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalNewPatientData(Service_Install_Id);
        }
        public static DataTable GetLocalOpenDentalLanguageList()
        {
            return SynchLocalDAL.GetLocalOpenDentalLanguageList();
        }
        public static DataTable GetLocalPatientCompareDeletedData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientCompareDeletedData(Service_Install_Id);
        }

        public static DataTable GetLocalPatientDataByPatientEHRID(string PatientEHRIDs, string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, Service_Install_Id);
        }

        public static DataTable GetPushLocalPatientData(string strPatID = "")
        {
            return SynchLocalDAL.GetPushLocalPatientData(strPatID);
        }

        public static DataTable GetAllLocalPatientData()
        {
            return SynchLocalDAL.GetAllLocalPatientData();
        }

        public static DataTable GetAllLocalActivePatientData()
        {
            return SynchLocalDAL.GetAllLocalActivePatientData();
        }

        public static DataTable GetPushLocalPatientStatusData()
        {
            return SynchLocalDAL.GetPushLocalPatientStatusData();
        }

        public static DataTable GetPushLocalPatientStatusData(int Service_Install_Id, int clinicnumber)
        {
            return SynchLocalDAL.GetPushLocalPatientStatusData(Service_Install_Id, clinicnumber);
        }

        public static void UpdatePatient_Status(DataTable dtPatientStatus, string Service_Install_Id, string clinicNumber = "", string strPatID = "")
        {
            SynchLocalDAL.UpdatePatient_Status(dtPatientStatus, Service_Install_Id, clinicNumber, strPatID);
        }

        public static void UpdatePatient_Status(DataTable dtPatientStatus, string Service_Install_Id, int clinicNumber)
        {
            SynchLocalDAL.UpdatePatient_Status(dtPatientStatus, Service_Install_Id, clinicNumber);
        }

        public static bool Update_PatientBalance(DataTable dtPatientBalance, string Service_Install_Id)
        {
            return SynchLocalDAL.Update_PatientBalance(dtPatientBalance, Service_Install_Id);
        }

        public static DataTable GetLocalPatientImagesData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientImagesData(Service_Install_Id);
        }

        public static void Save_ApptPatientBalance(DataTable dtPatientBalance, string Service_Install_Id)
        {
            SynchLocalDAL.Save_ApptPatientBalance(dtPatientBalance, Service_Install_Id);
        }

        #endregion

        #region Patient Form

        public static DataTable GetLocalPatientFormData(string Clinic_Number, string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientFormData(Clinic_Number, Service_Install_Id);
        }

        public static DataTable GetLocalPatientFormDiseaseResponse(string Clinic_Number, string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientFormDiseaseResponse(Clinic_Number, Service_Install_Id);
        }

        public static DataTable GetLocalPatientFormDiseaseDeleteResponse(string Clinic_Number, string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientFormDiseaseDeleteResponse(Clinic_Number, Service_Install_Id);
        }

        public static DataTable GetLocalPatientFormDiseaseResponseToSaveINEHR(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientFormDiseaseResponseToSaveINEHR(Service_Install_Id);
        }

        public static DataTable GetLocalPatientFormDocData(string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchLocalDAL.GetLocalPatientFormDocData(Service_Install_Id, strPatientFormID);
        }

        public static DataTable GetLocalPatientFormDocAttachmentData(string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchLocalDAL.GetLocalPatientFormDocAttachmentData(Service_Install_Id, strPatientFormID);
        }

        public static bool UpdatePatientFormMedicalHistory_Fields(DataTable dtEHRUPdate_Data)
        {
            return SynchLocalDAL.UpdatePatientFormMedicalHistory_Fields(dtEHRUPdate_Data);
        }

        public static bool Update_PatientForm_MedicalHistory_Field_Pushed(string patientFormWebId, string Service_Install_Id)
        {
            return SynchLocalDAL.Update_PatientForm_MedicalHistory_Field_Pushed(patientFormWebId, Service_Install_Id);
        }

        public static DataTable GetLocalPendingPatientFormData(string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchLocalDAL.GetLocalPendingPatientFormData(Service_Install_Id, strPatientFormID);
        }
        public static DataTable GetLocalPendingPatientFormDocAttachmentData(string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchLocalDAL.GetLocalPendingPatientFormDocAttachmentData(Service_Install_Id, strPatientFormID);
        }
        public static DataTable GetLocalPendingTreatmentDocData(string Service_Install_Id, string strTreatmentPlanID = "")
        {
            return SynchLocalDAL.GetLocalPendingTreatmentDocData(Service_Install_Id, strTreatmentPlanID);
        }

        public static DataTable GetLocalPendingPatientFormMedicalHistory(string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchLocalDAL.GetLocalPendingPatientFormMedicalHistory(Service_Install_Id, strPatientFormID);
        }

        public static DataTable GetMedicalHistoryPatientWithForm(string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchLocalDAL.GetMedicalHistoryPatientWithForm(Service_Install_Id, strPatientFormID);
        }

        public static bool UpdateMedicalHistorySubmitRecords(string patientform_web_id, string formMaster_EHR_id, Int64 formInstanceId, string DbString, string Service_Install_Id)
        {
            return SynchLocalDAL.UpdateMedicalHistorySubmitRecords(patientform_web_id, formMaster_EHR_id, formInstanceId, DbString, Service_Install_Id);
        }

        public static bool InsertMedicalHistorySubmitRecords(string patientform_web_id, string formMaster_EHR_id, string patient_ehr_id, string DbString, string Service_Install_Id)
        {
            return SynchLocalDAL.InsertMedicalHistorySubmitRecords(patientform_web_id, formMaster_EHR_id, patient_ehr_id, DbString, Service_Install_Id);
        }

        public static DataTable GetLocalNewWebPatient_FormData(string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchLocalDAL.GetLocalNewWebPatient_FormData(Service_Install_Id, strPatientFormID);

        }
        public static DataTable GetLocalWebPatientPaymentData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalWebPatientPaymentData(Service_Install_Id);

        }
        public static DataTable GetPatientPaymentTableBlankStructure()
        {
            return SynchLocalDAL.GetPatientPaymentTableBlankStructure();

        }
        //public static DataTable GetLocalWebPatientPaymentSplitData(string Service_Install_Id)
        //{
        //    return SynchLocalDAL.GetLocalWebPatientPaymentSplitData(Service_Install_Id);

        //}

        public static DataTable GetLocalWebPatientPaymentDataErroAPI(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalWebPatientPaymentDataErroAPI(Service_Install_Id);

        }
        public static void UpdateWebPatientPaymentDataErroAPI()
        {
            SynchLocalDAL.UpdateWebPatientPaymentDataErroAPI();

        }
        public static void UpdateWebPatientSMSCallDataErroAPI()
        {
            SynchLocalDAL.UpdateWebPatientSMSCallDataErroAPI();

        }
        public static DataTable GetLocalWebPatientSMSCallLogData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalWebPatientSMSCallLogData(Service_Install_Id);

        }
        //public static DataTable GetLivePatientFormDocData()
        //{
        //    return SynchLocalDAL.GetLivePatientFormDocData();
        //}

        #endregion

        #region  Disease


        public static DataTable GetLocalPatientDiseaseData(string Servic_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientDiseaseData(Servic_Install_Id);
        }
        public static DataTable GetPushLocalPatientDiseaseData()
        {
            return SynchLocalDAL.GetPushLocalPatientDiseaseData();
        }
        public static DataTable GetLocalDiseaseData(string Servic_Install_Id)
        {
            return SynchLocalDAL.GetLocalDiseaseData(Servic_Install_Id);
        }

        public static DataTable GetPushLocalDiseaseData()
        {
            return SynchLocalDAL.GetPushLocalDiseaseData();
        }

        #region Medication
        public static DataTable GetLocalPatientMedicationData(string Servic_Install_Id, string Patient_EHR_IDS = "")
        {
            return SynchLocalDAL.GetLocalPatientMedicationData(Servic_Install_Id, Patient_EHR_IDS);
        }
        public static DataTable GetPushLocalPatientMedicationData()
        {
            return SynchLocalDAL.GetPushLocalPatientMedicationData();
        }
        public static DataTable GetLocalMedicationData(string Servic_Install_Id)
        {
            return SynchLocalDAL.GetLocalMedicationData(Servic_Install_Id);
        }

        public static DataTable GetPushLocalMedicationData()
        {
            return SynchLocalDAL.GetPushLocalMedicationData();
        }
        public static bool Save_Medication_EHR_To_Local(DataTable dtMedication, string Service_Install_Id)
        {
            return SynchLocalDAL.Save_Medication_EHR_To_Local(dtMedication, Service_Install_Id);
        }
        public static bool Save_PatientMedication_EHR_To_Local(DataTable dtPatientMedication, string Service_Install_Id)
        {
            return SynchLocalDAL.Save_PatientMedication_EHR_To_Local(dtPatientMedication, Service_Install_Id);
        }

        #endregion

        #endregion

        #region  RecallType

        public static DataTable GetLocalRecallTypeData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalRecallTypeData(Service_Install_Id);
        }

        public static DataTable GetLocalUser(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalUser(Service_Install_Id);
        }
        public static DataTable GetPushLocalRecallTypeData()
        {
            return SynchLocalDAL.GetPushLocalRecallTypeData();
        }

        public static DataTable GetPushLocalUser()
        {
            return SynchLocalDAL.GetPushLocalUser();
        }

        #endregion

        #region Appointment Status

        public static DataTable GetPushLocalAppointmentStatusData()
        {
            return SynchLocalDAL.GetPushLocalAppointmentStatusData();
        }

        public static DataTable GetLocalAppointmentStatusData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalAppointmentStatusData(Service_Install_Id);
        }

        #endregion


        #region Insurance

        public static DataTable GetPushLocalInsuranceData()
        {
            return SynchLocalDAL.GetPushLocalInsuranceData();
        }

        public static DataTable GetLocalInsuranceData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalInsuranceData(Service_Install_Id);
        }
        #endregion


        #region  Holidays

        public static DataTable GetLocalHolidayData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalHolidayData(Service_Install_Id);
        }

        public static DataTable GetLocalEaglesoftHolidayData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalEaglesoftHolidayData(Service_Install_Id);
        }

        public static DataTable GetLocalDentrixHolidayData()
        {
            return SynchLocalDAL.GetLocalDentrixHolidayData();
        }
        public static DataTable GetLocalDentrixOperatoryHolidayData()
        {
            return SynchLocalDAL.GetLocalDentrixOperatoryHolidayData();
        }
        public static DataTable GetPushLocalHolidayData()
        {
            return SynchLocalDAL.GetPushLocalHolidayData();
        }

        #endregion

        #region ProviderOfficeHours

        public static DataTable GetLocalProviderOfficeHours(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalProviderOfficeHours(Service_Install_Id);
        }

        public static DataTable GetPushLocalProviderOfficeHoursData()
        {
            return SynchLocalDAL.GetPushLocalProviderOfficeHoursData();
        }
        #endregion


        public static DataTable GetLocalMedicalHistoryRecords(string tableName, string Service_Install_Id, bool isAllRecords = false, string strPatientFormID = "")
        {
            return SynchLocalDAL.GetLocalMedicalHistoryRecords(tableName, Service_Install_Id, isAllRecords, strPatientFormID);
        }


        public static DataTable GetDentrixLocalMedicleFormData()
        {
            return SynchLocalDAL.GetDentrixLocalMedicleFormData();
        }

        public static DataTable GetAbelDentLocalMedicleFormData()
        {
            return SynchLocalDAL.GetAbelDentLocalMedicleFormData();
        }

        public static DataTable GetAbelDentLocalMedicleAnswerData()
        {
            return SynchLocalDAL.GetAbelDentLocalMedicleAnswerData();
        }


        public static DataTable GetDentrixLocalFormQuestionData()
        {
            return SynchLocalDAL.GetDentrixLocalFormQuestionData();
        }

        public static DataTable GetAbelDentLocalFormQuestionData()
        {
            return SynchLocalDAL.GetAbelDentLocalFormQuestionData();
        }


        public static DataTable GetEasyDentalLocalMedicleQuestionData()
        {
            return SynchLocalDAL.GetEasyDentalLocalMedicleQuestionData();
        }

        public static DataTable GetLocalPatientWiseRecallTypeData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientWiseRecallTypeData(Service_Install_Id);
        }

        public static DataTable GetLocalPatientWisePaymentLog(bool isblankStructure, string ServiceInstallId, string clinicNumber)
        {
            return SynchLocalDAL.GetLocalPatientWisePaymentLog(isblankStructure, ServiceInstallId, clinicNumber);
        }

        public static DataTable GetLocalPatientWiseSMSCallLog(bool isblankStructure, string ServiceInstallId, string clinicNumber, int logType,string _filename_EHR_patient_sms_call="",string _EHRLogdirectory_EHR_patient_sms_call="")
        {
            return SynchLocalDAL.GetLocalPatientWiseSMSCallLog(isblankStructure, ServiceInstallId, clinicNumber, logType,_filename_EHR_patient_sms_call,_EHRLogdirectory_EHR_patient_sms_call);
        }

        public static bool CallPatientSMSCallAPIForStatusCompleted(DataTable dtPatientSMSLog, string status, string servericeInstallId, string clinicNumber, string Loc_Id, string LocationId,string _filename_EHR_patient_sms_call = "",string _EHRLogdirectory_EHR_patient_sms_call = "")
        {
            return SynchLocalDAL.CallPatientSMSCallAPIForStatusCompleted(dtPatientSMSLog, status, servericeInstallId, clinicNumber, Loc_Id, LocationId,_filename_EHR_patient_sms_call,_EHRLogdirectory_EHR_patient_sms_call);
        }

        public static bool CallPatientFollowUpStatusCompleted(DataTable dtPatientSMSLog, string status, string servericeInstallId, string clinicNumber, string Loc_Id, string LocationId,string _filename_EHR_PatientFollowUp = "",string _EHRLogdirectory_EHR_PatientFollowUp = "")
        {
            return SynchLocalDAL.CallPatientFollowUpStatusCompleted(dtPatientSMSLog, status, servericeInstallId, clinicNumber, Loc_Id, LocationId ,_filename_EHR_PatientFollowUp,_EHRLogdirectory_EHR_PatientFollowUp);
        }

        public static bool Update_PatientProfileImageStatus(string PatientWebId, string Service_Install_Id)
        {
            return SynchLocalDAL.Update_PatientProfileImageStatus(PatientWebId, Service_Install_Id);
        }

        public static bool Save_PatientProfileImage_EHR_To_Local(DataTable dtEagleSoftPatientProfileImage, string Service_Install_Id)
        {
            return SynchLocalDAL.Save_PatientProfileImage_EHR_To_Local(dtEagleSoftPatientProfileImage, Service_Install_Id);
        }

        public static DataTable GetLocalPatientProfileImageRecords(string Service_Install_Id, bool isAllRecords = false)
        {
            return SynchLocalDAL.GetLocalPatientProfileImageRecords(Service_Install_Id, isAllRecords);
        }

        public static DataTable GetLocalPatientFormMedicationResponse(string Clinic_Number, string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientFormMedicationResponse(Clinic_Number, Service_Install_Id);
        }

        public static DataTable GetLocalPatientFormMedicationRemovedResponse(string Clinic_Number, string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalPatientFormMedicationRemovedResponse(Clinic_Number, Service_Install_Id);
        }

        public static DataTable GetLocalInsertPatientData(string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalInsertPatientData(Service_Install_Id);
        }

        public static DataTable GetAllLocalNoteId(string ServiceInstallId)
        {
            return SynchLocalDAL.GetAllLocalNoteId(ServiceInstallId);
        }
        public static bool CallUpdateNoteDataMoveToCorrespondInTracker(DataTable dtNote, string Installation_ID)
        {
            return SynchLocalDAL.CallUpdateNoteDataMoveToCorrespondInTracker(dtNote, Installation_ID);
        }

        public static DataTable GetLocalApptPatientBalance(string Service_Install_Id, int clinicNumber)
        {
            return SynchLocalDAL.GetLocalApptPatientBalance(Service_Install_Id, clinicNumber);
        }
        public static DataTable GetPushLocalPatientBalanceData()
        {
            return SynchLocalDAL.GetPushLocalPatientBalanceData();
        }

        public static string Get_Patient_EHR_ID_from_Patient_Form(string strPatientFormWebID)
        {
            return SynchLocalDAL.Get_Patient_EHR_ID_from_Patient_Form(strPatientFormWebID);
        }

        public static DataTable GetLocalZohoDetailsData()
        {
            return SynchLocalDAL.GetLocalZohoDetailsData();
        }

        public static bool Save_ZohoDetailsData(int Zoho_LocalDB_ID, string Organisation_ID, string Organisation_Name, string Location_ID, string Location_Name, string EHR_Pass, string EHR_User, string Server_User, string Server_Pass,
            bool is_Confirmed, bool is_Installed, bool is_Valid, string User_ID, string User_Name, string Clinic_Number, string Service_Install_Id)
        {
            return SynchLocalDAL.Save_ZohoDetailsData(Zoho_LocalDB_ID, Organisation_ID, Organisation_Name, Location_ID, Location_Name, EHR_Pass, EHR_User, Server_User, Server_Pass, is_Confirmed, is_Installed, is_Valid, User_ID, User_Name, Clinic_Number, Service_Install_Id);
        }

        public static DataTable GetLocalSystemUsersData(string Clinic_Number, string Service_Install_Id)
        {
            return SynchLocalDAL.GetLocalSystemUsersData(Clinic_Number, Service_Install_Id);
        }

        public static bool Save_SystemUsersData(DataTable dtUsers, string Clinic_Number, string Service_Install_Id)
        {
            return SynchLocalDAL.Save_SystemUsersData(dtUsers, Clinic_Number, Service_Install_Id);
        }

        public static DataTable GetPushLocalSystemusersData(string Clinic_Number, string Service_Install_Id)
        {
            return SynchLocalDAL.GetPushLocalSystemusersData(Clinic_Number, Service_Install_Id);
        }

        //rooja https://app.asana.com/0/1203599217474380/1207342219246181/f
        #region Insurance Carrier
        public static bool Sync_check_for_InsuranceCarrier(string InsuranceCarrierId, string _filename_EHR_InsuranceCarrier_document = "", string _EHRLogdirectory_EHR_InsuranceCarrier_document = "")
        {
            return SynchLocalDAL.Sync_check_for_InsuranceCarrier(InsuranceCarrierId, _filename_EHR_InsuranceCarrier_document, _EHRLogdirectory_EHR_InsuranceCarrier_document);
        }

        public static void CreateRecordForInsuranceCarrierDoc(string PatientName, string SubmittedDate, string Patient_EHR_ID, string Patient_Web_ID, string InsuranceCarrierId, string InsuranceCarrierDocName, string InsuranceCarrierFolderName, string Clinic_Number, string Service_Install_Id, bool FileCreated, string _filename_EHR_InsuranceCarrier_document = "", string _EHRLogdirectory_EHR_InsuranceCarrier_document = "")
        {
            SynchLocalDAL.CreateRecordForInsuranceCarrierDoc(PatientName, SubmittedDate, Patient_EHR_ID, Patient_Web_ID, InsuranceCarrierId, InsuranceCarrierDocName, InsuranceCarrierFolderName, Clinic_Number, Service_Install_Id, FileCreated, _filename_EHR_InsuranceCarrier_document, _EHRLogdirectory_EHR_InsuranceCarrier_document);
        }
        public static void UpdateInsuranceCarrierDocInlocal(string InsuranceCarrierID, string _filename_EHR_InsuranceCarrier_document = "", string _EHRLogdirectory_EHR_InsuranceCarrier_document = "")
        {
            SynchLocalDAL.UpdateInsuranceCarrierDocInlocal(InsuranceCarrierID, _filename_EHR_InsuranceCarrier_document, _EHRLogdirectory_EHR_InsuranceCarrier_document);
        }
        public static DataTable ChangeStatusForInsuranceCarrierDoc(String Status)
        {
            return SynchLocalDAL.ChangeStatusForInsuranceCarrierDoc(Status);
        }
        #endregion    
    }
}
