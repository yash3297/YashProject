using System;
using Pozative.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pozative.UTL;


namespace Pozative.BAL
{
    public class SynchPracticeWebBAL
    {

        public static bool GetEHRConnection(int Application_ID, string DbString)
        {
            if (Application_ID == 1)
            {
                return SynchPracticeWebDAL.GetPracticeWebConnection(DbString);
            }
            else
            {
                return SynchPracticeWebDAL.GetPracticeWebConnection(DbString);
            }

        }

        #region ActualVersion

        public static string GetPracticeWebActualVersion(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebActualVersion(DbString);
           // return SynchPracticeWebDAL.GetPracticeWebDocPath(EHRActualVersion);
        }

        #endregion

        #region  Appointment

        public static DataTable GetPracticeWebAppointmentData(string DbString, string strApptID = "")
        {
            return SynchPracticeWebDAL.GetPracticeWebAppointmentData(DbString, strApptID);
        }

        public static DataTable GetPracticeWebAppointmentIds(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebAppointmentIds(DbString);
        }

        public static DataTable GetPracticeWebAppointment_Procedures_Data(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebAppointment_Procedures_Data(DbString);
        }

        public static DataTable GetPracticeWebDeletedAppointmentData(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebDeletedAppointmentData(DbString);
        }

       
        public static bool Save_Appointment_PracticeWeb_To_Local(DataTable dtPracticeWebAppointment, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchPracticeWebDAL.Save_Appointment_PracticeWeb_To_Local_SqlServer(dtPracticeWebAppointment, Service_Install_Id);
            }
            else
            {
                return SynchPracticeWebDAL.Save_Appointment_PracticeWeb_To_Local(dtPracticeWebAppointment, Service_Install_Id);
            }
        }

        public static long Save_Appointment_Local_To_PracticeWeb(string PatNum, string AptStatus, string Pattern, string Confirmed, string Op, string ProvNum,
                                                              string AptDateTime, string IsNewPatient, string DateTStamp, string AppointmentTypeNum, string apptcomment, string Clinic_Number, string TreatmentCodes, string DbString)
        {
            return SynchPracticeWebDAL.Save_Appointment_Local_To_PracticeWeb(PatNum, AptStatus, Pattern, Confirmed, Op, ProvNum, AptDateTime, IsNewPatient, DateTStamp, AppointmentTypeNum, apptcomment, Clinic_Number, TreatmentCodes, DbString);
        }

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id, tmpAppt_Web_id, Service_Install_Id);
        }

        public static bool Update_Status_EHR_Appointment_Live_To_PracticeWeb(DataTable dtLiveAppointment, string DbString,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchPracticeWebDAL.Update_Status_EHR_Appointment_Live_To_PracticeWeb(dtLiveAppointment, DbString,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
        }
        public static bool Update_Receive_SMS_Patient_EHR_Live_To_PracticeWeb(DataTable dtLiveAppointment, string DbString,string LocationId, string Loc_ID,string _filename_EHR_patientoptout = "",string _EHRLogdirectory_EHR_patientoptout = "")
        {
            return SynchPracticeWebDAL.Update_Receive_SMS_Patient_EHR_Live_To_PracticeWeb(dtLiveAppointment, DbString, LocationId,Loc_ID,_filename_EHR_patientoptout,_EHRLogdirectory_EHR_patientoptout);
        }

        #endregion

        #region  OperatoryEvent

        public static DataTable GetPracticeWebOperatoryEventData(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebOperatoryEventData(DbString);
        }

        public static DataTable GetPracticeWebOperatoryAllEventData(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebOperatoryAllEventData(DbString);
        }

        public static bool Save_OperatoryEvent_PracticeWeb_To_Local(DataTable dtPracticeWebOperatoryEvent, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchPracticeWebDAL.Save_OperatoryEvent_PracticeWeb_To_Local_SqlServer(dtPracticeWebOperatoryEvent, Service_Install_Id);
            }
            else
            {
                return SynchPracticeWebDAL.Save_OperatoryEvent_PracticeWeb_To_Local(dtPracticeWebOperatoryEvent, Service_Install_Id);
            }
        }

        #endregion

        #region  Provider

        public static DataTable GetPracticeWebIdelProvider(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebIdelProvider(DbString);
        }

        public static DataTable GetPracticeWebProviderData(string DbString, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.GetPracticeWebProviderData(DbString, Service_Install_Id);
        }

        public static bool Save_Provider_PracticeWeb_To_Local(DataTable dtPracticeWebProvider, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchPracticeWebDAL.Save_Provider_PracticeWeb_To_Local_SqlServer(dtPracticeWebProvider, Service_Install_Id);
            }
            else
            {
                return SynchPracticeWebDAL.Save_Provider_PracticeWeb_To_Local(dtPracticeWebProvider, Service_Install_Id);
            }
        }

        #endregion

        #region Provider Hours

        public static DataTable GetPracticeWebProviderHoursData(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebProviderHoursData(DbString);
        }

        public static bool Save_ProviderHours_PracticeWeb_To_Local(DataTable dtPracticeWebProviderHours, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.Save_ProviderHours_PracticeWeb_To_Local(dtPracticeWebProviderHours, Service_Install_Id);
        }

        #endregion

        #region  Speciality

        public static bool Save_Speciality_PracticeWeb_To_Local(DataTable dtPracticeWebSpeciality, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchPracticeWebDAL.Save_Speciality_PracticeWeb_To_Local_SqlServer(dtPracticeWebSpeciality, Service_Install_Id);
            }
            else
            {
                return SynchPracticeWebDAL.Save_Speciality_PracticeWeb_To_Local(dtPracticeWebSpeciality, Service_Install_Id);
            }
        }

        #endregion

        #region  Operatory

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate, string ClinicNumber, string DbString)
        {
            return SynchPracticeWebDAL.GetBookOperatoryAppointmenetWiseDateTime(ApptDate, ClinicNumber, DbString);
        }

        public static DataTable GetPracticeWebOperatoryData(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebOperatoryData(DbString);
        }
        public static DataTable GetPracticeWebDeletedOperatoryData(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebDeletedOperatoryData(DbString);
        }
        public static bool Save_Operatory_PracticeWeb_To_Local(DataTable dtPracticeWebOperatory, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchPracticeWebDAL.Save_Operatory_PracticeWeb_To_Local_SqlServer(dtPracticeWebOperatory, Service_Install_Id);
            }
            else
            {
                return SynchPracticeWebDAL.Save_Operatory_PracticeWeb_To_Local(dtPracticeWebOperatory, Service_Install_Id);
            }
        }

        #endregion

        #region  Appointment Type

        public static DataTable GetPracticeWebApptTypeData(string DbString, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.GetPracticeWebApptTypeData(DbString, Service_Install_Id);
        }

        public static bool Save_ApptType_PracticeWeb_To_Local(DataTable dtPracticeWebApptType, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchPracticeWebDAL.Save_ApptType_PracticeWeb_To_Local_SqlServer(dtPracticeWebApptType, Service_Install_Id);
            }
            else
            {
                return SynchPracticeWebDAL.Save_ApptType_PracticeWeb_To_Local(dtPracticeWebApptType, Service_Install_Id);
            }
        }

        #endregion

        #region  Patient

        public static DataTable GetPatientWisePendingAmount(string DbString)
        {
            return SynchPracticeWebDAL.GetPatientWisePendingAmount(DbString);
        }

        public static DataTable GetPracticeWebInsertPatientData(string Clinic_Number, string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebInsertPatientData(Clinic_Number, DbString);
        }

        public static DataTable GetPracticeWebPatientData(string Clinic_Number, string DbString, string PatientEHRID = "")
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientData(Clinic_Number, DbString, PatientEHRID);
        }

        public static DataTable GetPracticeWebPatientStatusData(string Clinic_Number, string DbString, string strPatID = "")
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientStatusData(Clinic_Number, DbString, strPatID);
        }

        public static DataTable GetPracticeWebAppointmentsPatientData(string Clinic_Number, string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebAppointmentsPatientData(Clinic_Number, DbString);
        }
        //public static DataTable GetPracticeWebPatientDocData()
        //{
        //    return SynchPracticeWebDAL.GetPracticeWebPatientDocData();
        //}
        public static DataTable GetPracticeWebPatientdue_date(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientdue_date(DbString);
        }

        public static DataTable GetPracticeWebPatientWiseRecallDate(string DbString, string serviceInstallId)
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientWiseRecallDate(DbString, serviceInstallId);
        }

        public static DataTable GetPracticeWebPatientNextApptDate(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientNextApptDate(DbString);
        }

        public static DataTable GetPracticeWebPatientInsBenafit(string PatientId, string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientInsBenafit(PatientId, DbString);
        }

        public static DataTable GetPracticeWebPatientLastVisit_Date(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientLastVisit_Date(DbString);
        }

        public static DataTable GetPracticeWebPatientID_NameData(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientID_NameData(DbString);
        }
        public static DataTable GetPracticeWebPatientInsuranceData(string PatientId, string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientInsuranceData(PatientId, DbString);
        }

        public static DataTable GetPracticeWebPatientImagesData(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientImagesData(DbString);
        }

        public static bool Save_Patient_PracticeWeb_To_Local(DataTable dtPracticeWebPatient, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchPracticeWebDAL.Save_Patient_PracticeWeb_To_Local_SqlServer(dtPracticeWebPatient, Service_Install_Id);
            }
            else
            {
                return SynchPracticeWebDAL.Save_Patient_PracticeWeb_To_Local(dtPracticeWebPatient, Service_Install_Id);
            }
        }

        public static int Save_Patient_Local_To_PracticeWeb(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, Int64 tmpPatient_Gur_id, string Birth_Date, string Clinic_Number, string DbString)
        {
            try
            {
                return SynchPracticeWebDAL.Save_Patient_Local_To_PracticeWeb(LastName, FirstName, MiddleName, Mobile, Email, PriProv, DateFirstVisit, tmpPatient_Gur_id, Birth_Date, Clinic_Number, DbString);
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        public static DataTable GetPracticeWebPatientDiseaseData(string DbString, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientDiseaseData(DbString, Service_Install_Id);
        }

        public static bool Save_PatientDisease_PracticeWeb_To_Local(DataTable dtPracticeWebDisease, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.Save_PatientDisease_PracticeWeb_To_Local(dtPracticeWebDisease, Service_Install_Id);
        }

        #endregion

        #region Patient Form


        public static bool Save_Patient_Form_Local_To_PracticeWeb(DataTable dtWebPatient_Form, string DbString, string Installation_ID)
        {
            return SynchPracticeWebDAL.Save_Patient_Form_Local_To_PracticeWeb(dtWebPatient_Form, DbString, Installation_ID);
        }
        public static bool Save_PatientPayment_Local_To_PracticeWeb(DataTable dtWebPatientPayment, string DbString, string Installation_ID, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            return SynchPracticeWebDAL.Save_PatientPayment_Local_To_PracticeWeb(dtWebPatientPayment, DbString, Installation_ID, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
        }
        public static string GetPracticeWebDocPath(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebDocPath(DbString);
        }

        public static bool SaveMedicalHistoryLocalToPracticeWeb(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchPracticeWebDAL.SaveMedicalHistoryLocalToPracticeWeb(DbString, Service_Install_Id, strPatientFormID);
        }

        public static bool SavePatientDisease(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchPracticeWebDAL.SavePatientDisease(DbString, Service_Install_Id, strPatientFormID);
        }
        public static bool DeletePatientDisease(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchPracticeWebDAL.DeletePatientDisease(DbString, Service_Install_Id, strPatientFormID);
        }
        #endregion

        #region  Disease

        public static DataTable GetPracticeWebDiseaseData(string DbString, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.GetPracticeWebDiseaseData(DbString, Service_Install_Id);
        }

        public static bool Save_Disease_PracticeWeb_To_Local(DataTable dtPracticeWebDisease, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.Save_Disease_PracticeWeb_To_Local(dtPracticeWebDisease, Service_Install_Id);
        }

        public static DataTable GetPracticeWebMedicationData(string DbString, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.GetPracticeWebMedicationData(DbString, Service_Install_Id);
        }

        public static DataTable GetPracticeWebPatientMedicationData(string DbString, string Service_Install_Id, string Patient_EHR_IDS)
        {
            return SynchPracticeWebDAL.GetPracticeWebPatientMedicationData(DbString, Service_Install_Id, Patient_EHR_IDS);
        }
        #endregion

        #region  RecallType

        public static DataTable GetPracticeWebRecallTypeData(string DbString, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.GetPracticeWebRecallTypeData(DbString, Service_Install_Id);
        }

        public static bool Save_RecallType_PracticeWeb_To_Local(DataTable dtPracticeWebRecallType, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchPracticeWebDAL.Save_RecallType_PracticeWeb_To_Local_SqlServer(dtPracticeWebRecallType, Service_Install_Id);
            }
            else
            {
                return SynchPracticeWebDAL.Save_RecallType_PracticeWeb_To_Local(dtPracticeWebRecallType, Service_Install_Id);
            }
        }

        #endregion

        #region User

        public static DataTable GetPracticeWebUserData(string DbString, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.GetPracticeWebUserData(DbString, Service_Install_Id);
        }

        public static bool Save_User_PracticeWeb_To_Local(DataTable dtPracticeWebUser, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.Save_User_PracticeWeb_To_Local(dtPracticeWebUser, Service_Install_Id);
        }
        #endregion

        #region  Appointment Status

        public static bool Save_ApptStatus_PracticeWeb_To_Local(DataTable dtPracticeWebApptStatus, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchPracticeWebDAL.Save_ApptStatus_PracticeWeb_To_Local_SqlServer(dtPracticeWebApptStatus, Service_Install_Id);
            }
            else
            {
                return SynchPracticeWebDAL.Save_ApptStatus_PracticeWeb_To_Local(dtPracticeWebApptStatus, Service_Install_Id);
            }
        }

        public static DataTable GetPracticeWebAppointmentStatus(string DbString, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.GetPracticeWebAppointmentStatus(DbString, Service_Install_Id);
        }

        #endregion

        #region  Holidays

        public static DataTable GetPracticeWebHolidayData(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebHolidayData(DbString);
        }

        public static bool Save_Holiday_PracticeWeb_To_Local(DataTable dtPracticeWebHoliday, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.Save_Holiday_PracticeWeb_To_Local(dtPracticeWebHoliday, Service_Install_Id);
        }

        #endregion

        #region Treatment Document
        public static bool Save_Treatment_Document_in_PracticeWeb(string DbString, string Service_Install_Id, string DocPath, string strTreatmentPlanID = "", string strPatientEHRId = "")
        {
            return SynchPracticeWebDAL.Save_Treatment_Document_in_PracticeWeb(DbString, Service_Install_Id, DocPath, strTreatmentPlanID, strPatientEHRId);
        }
        #endregion

        #region Insurance Carrier
        public static bool Save_InsuranceCarrier_Document_in_PracticeWeb(string DbString, string Service_Install_Id, string DocPath, string strInsuranceCarrierID = "", string strPatientEHRId = "")
        {
            return SynchPracticeWebDAL.Save_InsuranceCarrier_Document_in_PracticeWeb(DbString, Service_Install_Id, DocPath, strInsuranceCarrierID, strPatientEHRId);
        }
        #endregion

        #region Insurance
        public static bool Save_Insurance_PracticeWeb_To_Local(DataTable dtPracticeWebInsurance, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchPracticeWebDAL.Save_Insurance_PracticeWeb_To_Local_SqlServer(dtPracticeWebInsurance, Service_Install_Id);
            }
            else
            {
                return SynchPracticeWebDAL.Save_Insurance_PracticeWeb_To_Local(dtPracticeWebInsurance, Service_Install_Id);
            }
        }
        #endregion
        public static bool Save_Document_in_PracticeWeb(string DbString, string Service_Install_Id, string DocPath, string strPatientFormID = "", string strPatientID = "")
        {
            return SynchPracticeWebDAL.Save_Document_in_PracticeWeb(DbString, Service_Install_Id, DocPath, strPatientFormID, strPatientID);
        }
        

        public static DataSet GetPracticeWebMedicalHistoryData(string DbString, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.GetPracticeWebMedicalHistoryData(DbString, Service_Install_Id);
        }

        #region  Clinic

        public static DataTable GetPracticeWebClinicData(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebClinicData(DbString);
        }
        #endregion

        //public static string Save_PatientPaymentLog_LocalToPracticeWeb(DataTable dtWebPatientPayment, string DbConnectionString, string ServiceInstalltionId)
        //{
        //    return SynchPracticeWebDAL.Save_PatientPaymentLog_LocalToPracticeWeb(dtWebPatientPayment, DbConnectionString, ServiceInstalltionId);
        //}

        public static string Save_PatientSMSCallLog_LocalToPracticeWeb(DataTable dtWebPatientPayment, string DbConnectionString, string ServiceInstalltionId)
        {
            return SynchPracticeWebDAL.Save_PatientSMSCallLog_LocalToPracticeWeb(dtWebPatientPayment, DbConnectionString, ServiceInstalltionId);
        }

        public static void DeleteDuplicatePatientLog(string DbConnectionString, string ServiceInstalltionId)
        {
            SynchPracticeWebDAL.DeleteDuplicatePatientLog(DbConnectionString, ServiceInstalltionId);
        }

        public static bool SavePatientMedication(string DbString, string Service_Install_Id, ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            return SynchPracticeWebDAL.SavePatientMedication(DbString, Service_Install_Id, ref isRecordSaved, ref SavePatientEHRID, strPatientFormID);
        }

        public static bool DeletePatientMedication(string DbString, string Service_Install_Id ,ref bool isRecordDeleted, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            return SynchPracticeWebDAL.DeletePatientMedication(DbString, Service_Install_Id, ref isRecordDeleted, ref DeletePatientEHRID, strPatientFormID);
        }

        public static bool Save_Patient_PracticeWeb_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                return SynchPracticeWebDAL.Save_Patient_PracticeWeb_To_Local_New(dtOpenDentalDataToSave, Clinic_Number, Service_Install_Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetPracticeWebInsuranceData(string DbString, string Service_Install_Id)
        {
            return SynchPracticeWebDAL.GetPracticeWebInsuranceData(DbString, Service_Install_Id);
        }
    }
}
