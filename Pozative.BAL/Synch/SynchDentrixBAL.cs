using System;
using Pozative.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pozative.BAL
{
    public class SynchDentrixBAL
    {

        public static bool GetEHRConnection(int Application_ID)
        {
            if (Application_ID == 1)
            {
                return SynchDentrixDAL.GetDentrixConnection();
            }
            else
            {
                return SynchDentrixDAL.GetDentrixConnection();
            }

        }
        #region EHR_VersionNumber

        public static string GetDenrtrixEHR_VersionNumber()
        {
            return SynchDentrixDAL.GetDenrtrixEHR_VersionNumber();
        }
        #endregion

        #region FolderList

        public static bool Save_FolderList_Dentrix_To_Local(DataTable dtDentrixOperatory, string Service_Install_Id, string clinicNumber)
        {
            return SynchDentrixDAL.Save_FolderList_Dentrix_To_Local(dtDentrixOperatory, Service_Install_Id, clinicNumber);
        }

        public static DataTable GetDentrixFolderListData()
        {
            return SynchDentrixDAL.GetDentrixFolderListData();
        }

        #endregion

        #region Appointment

        public static DataTable GetDentrixAppointmentData(string strApptID = "")
        {
            return SynchDentrixDAL.GetDentrixAppointmentData(strApptID);
        }

        public static DataTable GetDentrixAppointmentIds()
        {
            return SynchDentrixDAL.GetDentrixAppointmentIds();
        }

        public static DataTable GetDentrixAppointment_Procedures_Data(string strApptID = "")
        {
            return SynchDentrixDAL.GetDentrixAppointment_Procedures_Data(strApptID);
        }


        public static DataTable GetDentrixApplicationVersion()
        {
            return SynchDentrixDAL.GetDentrixApplicationVersion();
        }

        public static bool Save_Appointment_Dentrix_To_Local(DataTable dtDentrixAppointment)
        {
            return SynchDentrixDAL.Save_Appointment_Dentrix_To_Local(dtDentrixAppointment);
        }

        #region Deleted Appointment

        public static DataTable GetDentrixDeletedAppointmentData()
        {
            return SynchDentrixDAL.GetDentrixDeletedAppointmentData();
        }
        public static bool Update_DeletedAppointment_Dentrix_To_Local(DataTable dtDentrixDeletedAppointment)
        {
            return SynchDentrixDAL.Update_DeletedAppointment_Dentrix_To_Local(dtDentrixDeletedAppointment);
        }
        #endregion
        #endregion

        #region OperatoryEvent

        public static DataTable GetDentrixOperatoryEventData()
        {
            return SynchDentrixDAL.GetDentrixOperatoryEventData();
        }

        public static bool Save_OperatoryEvent_Dentrix_To_Local(DataTable dtDentrixOperatoryEvent)
        {
            return SynchDentrixDAL.Save_OperatoryEvent_Dentrix_To_Local(dtDentrixOperatoryEvent);
        }

        #endregion

        #region Provider

        #region Provider

        public static DataTable GetDentrixProviderData()
        {
            return SynchDentrixDAL.GetDentrixProviderData();
        }

        public static bool Save_Provider_Dentrix_To_Local(DataTable dtDentrixProvider)
        {
            return SynchDentrixDAL.Save_Provider_Dentrix_To_Local(dtDentrixProvider);
        }

        #endregion

        #region ProviderOfficeHours

        public static DataTable GetDentrixProviderOfficeHours()
        {
            return SynchDentrixDAL.GetDentrixProviderOfficeHours();
        }

        #endregion

        #region ProvideeCustomeHours

        public static DataTable GetDentrixProviderHoursData()
        {
            return SynchDentrixDAL.GetDentrixProviderHoursData();
        }

        public static bool Save_ProviderHours_Dentrix_To_Local(DataTable dtDentrixProviderHours)
        {
            return SynchDentrixDAL.Save_ProviderHours_Dentrix_To_Local(dtDentrixProviderHours);
        }

        #endregion

        #endregion

        #region Speciality

        public static bool Save_Speciality_Dentrix_To_Local(DataTable dtDentrixSpeciality)
        {
            return SynchDentrixDAL.Save_Speciality_Dentrix_To_Local(dtDentrixSpeciality);
        }

        #endregion

        #region Operatory

        #region Operatory

        public static DataTable GetDentrixOperatoryData()
        {
            return SynchDentrixDAL.GetDentrixOperatoryData();
        }
        public static bool Save_Operatory_Dentrix_To_Local(DataTable dtDentrixOperatory)
        {

            return SynchDentrixDAL.Save_Operatory_Dentrix_To_Local(dtDentrixOperatory);
        }

        #endregion

        #region OperatoryCustomeHours

        public static DataTable GetDentrixOperatoryHoursData()
        {
            return SynchDentrixDAL.GetDentrixOperatoryHoursData();
        }
        public static bool Save_OperatoryHours_Dentrix_To_Local(DataTable dtDentrixOperatoryHours)
        {
            return SynchDentrixDAL.Save_OperatoryHours_Dentrix_To_Local(dtDentrixOperatoryHours);
        }

        #endregion

        #region OperatoryOfficeHours

        public static DataTable GetDentrixOperatoryOfficeHours()
        {
            return SynchDentrixDAL.GetDentrixOperatoryOfficeHours();
        }

        #endregion

        #endregion

        #region ApptType

        public static DataTable GetDentrixApptTypeData()
        {
            return SynchDentrixDAL.GetDentrixApptTypeData();
        }

        public static bool Save_ApptType_Dentrix_To_Local(DataTable dtDentrixApptType)
        {
            return SynchDentrixDAL.Save_ApptType_Dentrix_To_Local(dtDentrixApptType);
        }

        #endregion

        #region Patient

        public static DataTable GetDentrixPatientData(string strPatID = "")
        {
            return SynchDentrixDAL.GetDentrixPatientData(strPatID);
        }
        public static DataTable GetDentrixNewPatientData()
        {
            return SynchDentrixDAL.GetDentrixNewPatientData();
        }
        public static DataTable GetDentrixPatientStatusData(string strPatID = "")
        {
            return SynchDentrixDAL.GetDentrixPatientStatusData(strPatID);
        }
        public static DataTable GetDentrixAppointmentsPatientData(string strPatID = "")
        {
            return SynchDentrixDAL.GetDentrixAppointmentsPatientData(strPatID);
        }
        
        public static bool Save_Patient_Dentrix_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                return SynchDentrixDAL.Save_Patient_Dentrix_To_Local_New(dtOpenDentalDataToSave, Clinic_Number, Service_Install_Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static DataTable GetDentrixPatientdue_date(string strPatID = "")
        {
            return SynchDentrixDAL.GetDentrixPatientdue_date(strPatID);
        }
        public static DataTable GetDentrixPatientcollect_payment()
        {
            return SynchDentrixDAL.GetDentrixPatientcollect_payment();
        }
        public static DataTable GetDentrixPatient_recall()
        {
            return SynchDentrixDAL.GetDentrixPatient_recall();
        }
        public static DataTable GetDentrixPatient_RecallType()
        {
            return SynchDentrixDAL.GetDentrixPatient_RecallType();
        }
        public static DataTable GetDentrixPatientNextApptDate()
        {
            return SynchDentrixDAL.GetDentrixPatientNextApptDate();
        }
        public static bool Save_Patient_Dentrix_To_Local(DataTable dtDentrixPatient, string InsertTableName, DataTable dtDentrixPatientdue_date, DataTable dtLocalPatient, bool bSetDeleted = false)
        {
            return SynchDentrixDAL.Save_Patient_Dentrix_To_Local(dtDentrixPatient, InsertTableName, dtDentrixPatientdue_date, dtLocalPatient, bSetDeleted);
        }
        public static DataTable GetDentrixPatientDiseaseData()
        {
            return SynchDentrixDAL.GetDentrixPatientDiseaseData();
        }
        public static DataTable GetDentrixPatientMedicationData(string Patient_EHR_IDS)
        {
            return SynchDentrixDAL.GetDentrixPatientMedicationData(Patient_EHR_IDS);
        }
        public static bool Save_PatientDisease_Dentrix_To_Local(DataTable dtDentrixPatient)
        {
            return SynchDentrixDAL.Save_PatientDisease_Dentrix_To_Local(dtDentrixPatient);
        }

        #endregion

        #region  Disease

        public static DataTable GetDentrixDiseaseData()
        {
            return SynchDentrixDAL.GetDentrixDiseaseData();
        }

        public static DataTable GetDentrixMedicationData()
        {
            return SynchDentrixDAL.GetDentrixMedicationData();
        }
        public static bool Save_Disease_Dentrix_To_Local(DataTable dtDentrixDisease)
        {
            return SynchDentrixDAL.Save_Disease_Dentrix_To_Local(dtDentrixDisease);
        }

        #endregion

        #region RecallType

        public static DataTable GetDentrixRecallTypeData()
        {
            return SynchDentrixDAL.GetDentrixRecallTypeData();
        }

        public static bool Save_RecallType_Dentrix_To_Local(DataTable dtDentrixRecallType)
        {
            return SynchDentrixDAL.Save_RecallType_Dentrix_To_Local(dtDentrixRecallType);
        }

        #endregion

        #region Speciality

        public static DataTable GetDentrixApptStatusData()
        {
            return SynchDentrixDAL.GetDentrixApptStatusData();
        }

        public static bool Save_ApptStatus_Dentrix_To_Local(DataTable dtDentrixApptStatus)
        {
            return SynchDentrixDAL.Save_ApptStatus_Dentrix_To_Local(dtDentrixApptStatus);
        }

        #endregion

        #region User
        public static DataTable GetDentrixUsersData()
        {
            return SynchDentrixDAL.GetDentrixUsersData();
        }
        public static bool Save_Users_Dentrix_To_Local(DataTable dtDentrixUser)
        {
            return SynchDentrixDAL.Save_Users_Dentrix_To_Local(dtDentrixUser);
        }
        public static string GetPatientName(Int64 patientid)
        {
            return SynchDentrixDAL.GetPatientName(patientid);
        }
        #endregion

        #region Holidays

        public static DataTable GetDentrixHolidaysData()
        {
            return SynchDentrixDAL.GetDentrixHolidaysData();
        }
        public static DataTable GetDentrixOperatoryHolidaysData(DataTable dtOperatory)
        {
            return SynchDentrixDAL.GetDentrixOperatoryHolidaysData(dtOperatory);
        }
        public static bool Save_Holidays_Dentrix_To_Local(DataTable dtDentrixOperatoryEvent)
        {
            return SynchDentrixDAL.Save_Holidays_Dentrix_To_Local(dtDentrixOperatoryEvent);
        }
        public static bool Save_Opeatory_Holidays_Dentrix_To_Local(DataTable dtDentrixOperatoryEvent)
        {
            return SynchDentrixDAL.Save_Opeatory_Holidays_Dentrix_To_Local(dtDentrixOperatoryEvent);
        }

        #endregion

        #region CreateAppointment

        public static DataTable GetDentrixPatientID_NameData()
        {
            return SynchDentrixDAL.GetDentrixPatientID_NameData();
        }

        public static DataTable GetDentrixIdelProvider()
        {
            return SynchDentrixDAL.GetDentrixIdelProvider();
        }

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate)
        {
            return SynchDentrixDAL.GetBookOperatoryAppointmenetWiseDateTime(ApptDate);
        }
        public static int Save_Patient_Local_To_Dentrix(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, int tmpPatient_Gur_id, string Birth_Date)
        {
            try
            {
                return SynchDentrixDAL.Save_Patient_Local_To_Dentrix(LastName, FirstName, MiddleName, Mobile, Email, PriProv, DateFirstVisit, tmpPatient_Gur_id, Birth_Date);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public static int Save_Appointment_Local_To_Dentrix(string PatNum, string AppointmentConfirmationStatus, int Pattern, string Op, string ProvNum, string AptDateTime, string DateTStamp, string AppointmentTypeNum, string PatientName, string ApptComment, string TreatmentCodes)
        {
            return SynchDentrixDAL.Save_Appointment_Local_To_Dentrix(PatNum, AppointmentConfirmationStatus, Pattern, Op, ProvNum, AptDateTime, DateTStamp, AppointmentTypeNum, PatientName, ApptComment, TreatmentCodes);
        }
        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id)
        {
            return SynchDentrixDAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id, tmpAppt_Web_id);
        }
        public static bool Update_Status_EHR_Appointment_Live_To_DentrixEHR(DataTable dtLiveAppointment,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchDentrixDAL.Update_Status_EHR_Appointment_Live_To_DentrixEHR(dtLiveAppointment,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
        }
        public static bool Update_Receive_SMS_Patient_EHR_Live_To_DentrixEHR(DataTable dtLiveAppointment, string Locationid, string Loc_ID,string _filename_EHR_patientoptout = "",string _EHRLogdirectory_EHR_patientoptout = "")
        {
            return SynchDentrixDAL.Update_Receive_SMS_Patient_EHR_Live_To_DentrixEHR(dtLiveAppointment, Locationid, Loc_ID,_filename_EHR_patientoptout,_EHRLogdirectory_EHR_patientoptout);
        }
        #endregion

        #region Patient_Form
        public static bool Save_Patient_Form_Local_To_Dentrix(DataTable dtWebPatient_Form)
        {
            return SynchDentrixDAL.Save_Patient_Form_Local_To_Dentrix(dtWebPatient_Form);
        }

        public static bool Save_Document_in_Dentrix(string strPatientFormID = "")
        {
            return SynchDentrixDAL.Save_Document_in_Dentrix(strPatientFormID);
        }
        
        public static DataTable GetDentrixMedicleFormData()
        {
            return SynchDentrixDAL.GetDentrixMedicleFormData();
        }

        public static DataTable GetDentrixMedicleFormQuestionData()
        {
            return SynchDentrixDAL.GetDentrixMedicleFormQuestionData();
        }
        public static bool SaveMedicalHistoryLocalToDentrix(string strPatientFormID = "")
        {
            return SynchDentrixDAL.SaveMedicalHistoryLocalToDentrix(strPatientFormID);
        }
        public static bool SaveDiseaseLocalToDentrix(string strPatientFormID = "")
        {
            return SynchDentrixDAL.SaveDiseaseLocalToDentrix(strPatientFormID);
        }

        public static bool DeleteDiseaseLocalToDentrix(string strPatientFormID = "")
        {
            return SynchDentrixDAL.DeleteDiseaseLocalToDentrix(strPatientFormID);
        }

        public static bool SaveMedicationLocalToDentrix(ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            return SynchDentrixDAL.SaveMedicationLocalToDentrix(ref isRecordSaved, ref SavePatientEHRID, strPatientFormID);
        }

        public static bool DeleteMedicationLocalToDentrix(ref bool isRecordDeleted, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            return SynchDentrixDAL.DeleteMedicationLocalToDentrix(ref isRecordDeleted, ref DeletePatientEHRID, strPatientFormID);
        }

        #endregion

        #region Treatment DOc
        public static bool Save_Treatment_Document_in_Dentrix(string strTreatmentPlanID = "")
        {
            return SynchDentrixDAL.Save_Treatment_Document_in_Dentrix(strTreatmentPlanID);
        }
        #endregion

        public static DataTable GetDentrixPatientdue_date_AllData()
        {
            return SynchDentrixDAL.GetDentrixPatientdue_date_AllData();
        }
        public static Int64 SavePatientPaymentTOEHR(DataTable dtTable, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            return SynchDentrixDAL.SavePatientPaymentTOEHR( dtTable, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
        }
        public static string GetPatGuaridAndProviders(string Patid)
        {
            return SynchDentrixDAL.GetPatGuaridAndProviders(Patid);
        }

        public static string Save_PatientPaymentLog_LocalToDentrix(DataTable dtWebPatientPayment)
        {
            return SynchDentrixDAL.Save_PatientPaymentLog_LocalToDentrix(dtWebPatientPayment);
        }
        public static string Save_PatientSMSCallLog_LocalToDentrix(DataTable dtWebPatientPayment)
        {
            return SynchDentrixDAL.Save_PatientSMSCallLog_LocalToDentrix(dtWebPatientPayment);
        }

        public static void DeleteDuplicatePatientLog()
        {
            SynchDentrixDAL.DeleteDuplicatePatientLog();
        }
        public static DataTable GetDentrixPatientImagesData(string connectionString, string imagePathName)
        {
            return SynchDentrixDAL.GetDentrixPatientImagesData(connectionString, imagePathName);
        }

        #region Insurance
        public static DataTable GetDentrixInsuranceData()
        {
            return SynchDentrixDAL.GetDentrixInsuranceData();
        }

        public static bool Save_Insurance_Dentrix_To_Local(DataTable dtDentrixInsurance)
        {
            return SynchDentrixDAL.Save_Insurance_Dentrix_To_Local(dtDentrixInsurance);
        }

        #region Treatment DOc
        public static bool Save_InsuranceCarrier_Document_in_Dentrix(string strInsuranceCarrierID = "")
        {
            return SynchDentrixDAL.Save_InsuranceCarrier_Document_in_Dentrix(strInsuranceCarrierID);
        }
        #endregion

        #endregion
    }
}
