using System;
using Pozative.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pozative.UTL;


namespace Pozative.BAL
{
    public class SynchOpenDentalBAL
    {

        public static bool GetEHRConnection(int Application_ID, string DbString)
        {
            if (Application_ID == 1)
            {
                return SynchOpenDentalDAL.GetOpenDentalConnection(DbString);
            }
            else
            {
                return SynchOpenDentalDAL.GetOpenDentalConnection(DbString);
            }

        }

        #region ActualVersion

        public static string GetOpenDentalActualVersion(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalActualVersion(DbString);
            // return SynchOpenDentalDAL.GetOpenDentalDocPath(EHRActualVersion);
        }

        #endregion

        #region  Appointment

        public static DataTable GetOpenDentalAppointmentData(string DbString, string Appt_EHR_ID = "")
        {
            return SynchOpenDentalDAL.GetOpenDentalAppointmentData(DbString, Appt_EHR_ID);
        }

        public static DataTable GetOpenDentalAppointmentIds(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalAppointmentIds(DbString);
        }

        public static DataTable GetOpendentalAppointment_Procedures_Data(string DbString)
        {
            return SynchOpenDentalDAL.GetOpendentalAppointment_Procedures_Data(DbString);
        }

        public static DataTable GetOpenDentalDeletedAppointmentData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalDeletedAppointmentData(DbString);
        }


        public static bool Save_Appointment_OpenDental_To_Local(DataTable dtOpenDentalAppointment, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_Appointment_OpenDental_To_Local_SqlServer(dtOpenDentalAppointment, Service_Install_Id);
            }
            else
            {
                return SynchOpenDentalDAL.Save_Appointment_OpenDental_To_Local(dtOpenDentalAppointment, Service_Install_Id);
            }
        }

        public static long Save_Appointment_Local_To_OpenDental(string PatNum, string AptStatus, string Pattern, string Confirmed, string Op, string ProvNum,
                                                              string AptDateTime, string IsNewPatient, string DateTStamp, string AppointmentTypeNum, string apptcomment, string Clinic_Number, string TreatmentCodes, string DbString)
        {
            return SynchOpenDentalDAL.Save_Appointment_Local_To_OpenDental(PatNum, AptStatus, Pattern, Confirmed, Op, ProvNum, AptDateTime, IsNewPatient, DateTStamp, AppointmentTypeNum, apptcomment, Clinic_Number, TreatmentCodes, DbString);
        }

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id, tmpAppt_Web_id, Service_Install_Id);
        }

        public static bool Update_Status_EHR_Appointment_Live_To_Opendental(DataTable dtLiveAppointment, string DbString,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchOpenDentalDAL.Update_Status_EHR_Appointment_Live_To_Opendental(dtLiveAppointment, DbString,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
        }
        public static bool Update_Receive_SMS_Patient_EHR_Live_To_Opendental(DataTable dtLiveAppointment, string DbString,string LocationId, string Loc_ID,string _filename_EHR_patientoptout = "",string _EHRLogdirectory_EHR_patientoptout = "")
        {
            return SynchOpenDentalDAL.Update_Receive_SMS_Patient_EHR_Live_To_Opendental(dtLiveAppointment, DbString, LocationId,Loc_ID,_filename_EHR_patientoptout,_EHRLogdirectory_EHR_patientoptout);
        }

        #endregion

        #region  OperatoryEvent

        public static DataTable GetOpenDentalOperatoryEventData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalOperatoryEventData(DbString);
        }

        public static DataTable GetOpenDentalOperatoryAllEventData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalOperatoryAllEventData(DbString);
        }

        public static bool Save_OperatoryEvent_OpenDental_To_Local(DataTable dtOpenDentalOperatoryEvent, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_OperatoryEvent_OpenDental_To_Local_SqlServer(dtOpenDentalOperatoryEvent, Service_Install_Id);
            }
            else
            {
                return SynchOpenDentalDAL.Save_OperatoryEvent_OpenDental_To_Local(dtOpenDentalOperatoryEvent, Service_Install_Id);
            }
        }

        #endregion

        #region  Provider

        public static DataTable GetOpenDentalIdelProvider(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalIdelProvider(DbString);
        }

        public static DataTable GetOpenDentalProviderData(string DbString, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.GetOpenDentalProviderData(DbString, Service_Install_Id);
        }

        public static bool Save_Provider_OpenDental_To_Local(DataTable dtOpenDentalProvider, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_Provider_OpenDental_To_Local_SqlServer(dtOpenDentalProvider, Service_Install_Id);
            }
            else
            {
                return SynchOpenDentalDAL.Save_Provider_OpenDental_To_Local(dtOpenDentalProvider, Service_Install_Id);
            }
        }

        #endregion

        #region Provider Hours

        public static DataTable GetOpenDentalProviderHoursData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalProviderHoursData(DbString);
        }

        public static bool Save_ProviderHours_OpenDental_To_Local(DataTable dtOpenDentalProviderHours, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.Save_ProviderHours_OpenDental_To_Local(dtOpenDentalProviderHours, Service_Install_Id);
        }

        #endregion

        #region  Speciality

        public static bool Save_Speciality_OpenDental_To_Local(DataTable dtOpenDentalSpeciality, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_Speciality_OpenDental_To_Local_SqlServer(dtOpenDentalSpeciality, Service_Install_Id);
            }
            else
            {
                return SynchOpenDentalDAL.Save_Speciality_OpenDental_To_Local(dtOpenDentalSpeciality, Service_Install_Id);
            }
        }

        #endregion

        #region FolderList

        public static DataTable GetOpenDentalFolderListData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalFolderListData(DbString);
        }

        public static bool Save_FolderList_OpenDental_To_Local(DataTable dtOpenDentalFolderList, string Service_Install_Id, string clinicNumber)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_FolderList_OpenDental_To_Local_SqlServer(dtOpenDentalFolderList, Service_Install_Id, clinicNumber);
            }
            else
            {
                return SynchOpenDentalDAL.Save_FolderList_OpenDental_To_Local(dtOpenDentalFolderList, Service_Install_Id, clinicNumber);
            }
        }

        public static DataTable GetOpenDentalDeletedFolderListData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalDeletedFolderListData(DbString);
        }

        #endregion

        #region  Operatory

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate, string ClinicNumber, string DbString)
        {
            return SynchOpenDentalDAL.GetBookOperatoryAppointmenetWiseDateTime(ApptDate, ClinicNumber, DbString);
        }

        public static DataTable GetOpenDentalOperatoryData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalOperatoryData(DbString);
        }

        public static DataTable GetOpenDentalDeletedOperatoryData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalDeletedOperatoryData(DbString);
        }
        public static bool Save_Operatory_OpenDental_To_Local(DataTable dtOpenDentalOperatory, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_Operatory_OpenDental_To_Local_SqlServer(dtOpenDentalOperatory, Service_Install_Id);
            }
            else
            {
                return SynchOpenDentalDAL.Save_Operatory_OpenDental_To_Local(dtOpenDentalOperatory, Service_Install_Id);
            }
        }

        #endregion

        #region  Appointment Type

        public static DataTable GetOpenDentalApptTypeData(string DbString, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.GetOpenDentalApptTypeData(DbString, Service_Install_Id);
        }

        public static bool Save_ApptType_OpenDental_To_Local(DataTable dtOpenDentalApptType, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_ApptType_OpenDental_To_Local_SqlServer(dtOpenDentalApptType, Service_Install_Id);
            }
            else
            {
                return SynchOpenDentalDAL.Save_ApptType_OpenDental_To_Local(dtOpenDentalApptType, Service_Install_Id);
            }
        }

        #endregion

        #region  Patient

        public static DataTable GetPatientWisePendingAmount(string Clinic_number, string DbString)
        {
            return SynchOpenDentalDAL.GetPatientWisePendingAmount(Clinic_number, DbString);
        }

        public static DataTable GetOpenDentalPatientData(string Clinic_Number, string DbString, bool isNewQuery, string PatientEHRID = "")
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientData(Clinic_Number, DbString, isNewQuery, PatientEHRID);
        }

        public static DataTable GetOpenDentalInsertPatientData(string Clinic_Number, string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalInsertPatientData(Clinic_Number, DbString);
        }


        public static DataTable GetOpenDentalPatientStatusData(string Clinic_Number, string DbString, string Patient_EHR_ID = "")
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientStatusData(Clinic_Number, DbString, Patient_EHR_ID);
        }

        public static DataTable GetOpenDentalAppointmentsPatientData(string Clinic_Number, string DbString, bool isNew, string Pat_EHR_ID = "")
        {
            return SynchOpenDentalDAL.GetOpenDentalAppointmentsPatientData(Clinic_Number, DbString, isNew, Pat_EHR_ID);
        }
        public static DataTable GetOpenDentalApptPatientdue_date(string Clinic_number, string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalApptPatientdue_date(Clinic_number, DbString);
        }
        //public static DataTable GetOpenDentalPatientDocData()
        //{
        //    return SynchOpenDentalDAL.GetOpenDentalPatientDocData();
        //}
        public static DataTable GetOpenDentalPatientdue_date(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientdue_date(DbString);
        }

        public static DataTable GetOpenDentalPatientWiseRecallDate(string DbString, string serviceInstallId)
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientWiseRecallDate(DbString, serviceInstallId);
        }

        public static DataTable GetOpenDentalPatientNextApptDate(string Clinic_number, string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientNextApptDate(Clinic_number, DbString);
        }

        public static DataTable GetOpenDentalPatientInsBenafit(string Clinic_number, string PatientId, string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientInsBenafit(Clinic_number, PatientId, DbString);
        }

        public static DataTable GetOpenDentalPatientLastVisit_Date(string Clinic_number, string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientLastVisit_Date(Clinic_number, DbString);
        }

        public static DataTable GetOpenDentalPatientID_NameData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientID_NameData(DbString);
        }
        public static DataTable GetOpenDentalPatientInsuranceData(string Clinic_number, string PatientId, string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientInsuranceData(Clinic_number, PatientId, DbString);
        }

        public static DataTable GetOpenDentalPatientImagesData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientImagesData(DbString);
        }
        public static DataTable GetOpenDentalInsuranceData(string DbString, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.GetOpenDentalInsuranceData(DbString, Service_Install_Id);
        }

        public static bool Save_Patient_OpenDental_To_Local(DataTable dtOpenDentalPatient, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_Patient_OpenDental_To_Local_SqlServer(dtOpenDentalPatient, Service_Install_Id);
            }
            else
            {
                return SynchOpenDentalDAL.Save_Patient_OpenDental_To_Local(dtOpenDentalPatient, Service_Install_Id);
            }
        }

        public static bool Save_PatientAppointment_OpenDental_To_Local(DataTable dtOpenDentalPatient, string Service_Install_Id)
        {

            return SynchOpenDentalDAL.Save_PatientAppointment_OpenDental_To_Local(dtOpenDentalPatient, Service_Install_Id);
            //}
        }




        public static Int64 Save_Patient_Local_To_OpenDental(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, Int64 tmpPatient_Gur_id, string Birth_Date, string Clinic_Number, string DbString)
        {
            try
            {
                return SynchOpenDentalDAL.Save_Patient_Local_To_OpenDental(LastName, FirstName, MiddleName, Mobile, Email, PriProv, DateFirstVisit, tmpPatient_Gur_id, Birth_Date, Clinic_Number, DbString);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public static DataTable GetOpenDentalPatientDiseaseData(string DbString, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientDiseaseData(DbString, Service_Install_Id);
        }

        public static bool Save_PatientDisease_OpenDental_To_Local(DataTable dtOpenDentalDisease, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.Save_PatientDisease_OpenDental_To_Local(dtOpenDentalDisease, Service_Install_Id);
        }

        #endregion

        #region Patient Form


        public static bool Save_Patient_Form_Local_To_OpenDental(DataTable dtWebPatient_Form, string DbString, string Installation_ID)
        {
            return SynchOpenDentalDAL.Save_Patient_Form_Local_To_OpenDental(dtWebPatient_Form, DbString, Installation_ID);
        }
        public static bool Save_PatientPayment_Local_To_OpenDental(DataTable dtWebPatientPayment, string DbString, string Installation_ID, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            return SynchOpenDentalDAL.Save_PatientPayment_Local_To_OpenDental(dtWebPatientPayment, DbString, Installation_ID,  _filename_EHR_Payment,  _EHRLogdirectory_EHR_Payment );
        }
        public static string GetOpenDentalDocPath(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalDocPath(DbString);
        }

        public static bool SaveMedicalHistoryLocalToOpenDental(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchOpenDentalDAL.SaveMedicalHistoryLocalToOpenDental(DbString, Service_Install_Id, strPatientFormID);
        }

        public static bool SavePatientDisease(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchOpenDentalDAL.SavePatientDisease(DbString, Service_Install_Id, strPatientFormID);
        }
        public static bool DeletePatientDisease(string DbString, string Service_Install_Id, string strPatientFormId = "")
        {
            return SynchOpenDentalDAL.DeletePatientDisease(DbString, Service_Install_Id, strPatientFormId);
        }
        #endregion

        #region  Disease

        public static DataTable GetOpenDentalDiseaseData(string DbString, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.GetOpenDentalDiseaseData(DbString, Service_Install_Id);
        }

        public static bool Save_Disease_OpenDental_To_Local(DataTable dtOpenDentalDisease, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.Save_Disease_OpenDental_To_Local(dtOpenDentalDisease, Service_Install_Id);
        }

        public static DataTable GetOpenDentalMedicationData(string DbString, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.GetOpenDentalMedicationData(DbString, Service_Install_Id);
        }

        public static DataTable GetOpenDentalPatientMedicationData(string DbString, string Service_Install_Id, string Patient_EHR_IDS)
        {
            return SynchOpenDentalDAL.GetOpenDentalPatientMedicationData(DbString, Service_Install_Id, Patient_EHR_IDS);
        }
        #endregion

        #region  RecallType

        public static DataTable GetOpenDentalRecallTypeData(string DbString, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.GetOpenDentalRecallTypeData(DbString, Service_Install_Id);
        }

        public static bool Save_RecallType_OpenDental_To_Local(DataTable dtOpenDentalRecallType, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_RecallType_OpenDental_To_Local_SqlServer(dtOpenDentalRecallType, Service_Install_Id);
            }
            else
            {
                return SynchOpenDentalDAL.Save_RecallType_OpenDental_To_Local(dtOpenDentalRecallType, Service_Install_Id);
            }
        }

        #endregion

        #region User

        public static DataTable GetOpenDentalUserData(string DbString, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.GetOpenDentalUserData(DbString, Service_Install_Id);
        }

        public static bool Save_User_OpenDental_To_Local(DataTable dtOpendentalUser, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.Save_User_OpenDental_To_Local(dtOpendentalUser, Service_Install_Id);
        }
        #endregion

        #region  Appointment Status

        public static bool Save_ApptStatus_OpenDental_To_Local(DataTable dtOpenDentalApptStatus, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_ApptStatus_OpenDental_To_Local_SqlServer(dtOpenDentalApptStatus, Service_Install_Id);
            }
            else
            {
                return SynchOpenDentalDAL.Save_ApptStatus_OpenDental_To_Local(dtOpenDentalApptStatus, Service_Install_Id);
            }
        }

        public static DataTable GetOpenDentalAppointmentStatus(string DbString, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.GetOpenDentalAppointmentStatus(DbString, Service_Install_Id);
        }

        #endregion

        #region  Holidays

        public static DataTable GetOpenDentalHolidayData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalHolidayData(DbString);
        }

        public static bool Save_Holiday_OpenDental_To_Local(DataTable dtOpenDentalHoliday, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.Save_Holiday_OpenDental_To_Local(dtOpenDentalHoliday, Service_Install_Id);
        }

        #endregion

        #region Insurance

        public static bool Save_Insurance_OpenDental_To_Local(DataTable dtOpenDentalInsurance, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SynchOpenDentalDAL.Save_Insurance_OpenDental_To_Local_SqlServer(dtOpenDentalInsurance, Service_Install_Id);
            }
            else
            {
                return SynchOpenDentalDAL.Save_Insurance_OpenDental_To_Local(dtOpenDentalInsurance, Service_Install_Id);
            }
        }

        #endregion

        public static bool Save_Document_in_OpenDental(string DbString, string Service_Install_Id, string DocPath, string strPatientFormID = "", string strPatientID = "")
        {
            return SynchOpenDentalDAL.Save_Document_in_OpenDental(DbString, Service_Install_Id, DocPath, strPatientFormID, strPatientID);
        }
        public static bool Save_Treatment_Document_in_OpenDental(string DbString, string Service_Install_Id, string DocPath, string strTreatmentPlanID = "", string strPatientEHRId = "")
        {
            return SynchOpenDentalDAL.Save_Treatment_Document_in_OpenDental(DbString, Service_Install_Id, DocPath, strTreatmentPlanID, strPatientEHRId);
        }
        public static bool Save_InsuranceCarrier_Document_in_OpenDental(string DbString, string Service_Install_Id, string DocPath, string strInsuranceCarrierID = "", string strPatientEHRId = "")
        {
            return SynchOpenDentalDAL.Save_InsuranceCarrier_Document_in_OpenDental(DbString, Service_Install_Id, DocPath, strInsuranceCarrierID, strPatientEHRId);
        }

        public static DataSet GetOpenDentalMedicalHistoryData(string DbString, string Service_Install_Id)
        {
            return SynchOpenDentalDAL.GetOpenDentalMedicalHistoryData(DbString, Service_Install_Id);
        }

        #region  Clinic

        public static DataTable GetOpenDentalClinicData(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalClinicData(DbString);
        }
        #endregion

        //public static string Save_PatientPaymentLog_LocalToOpenDental(DataTable dtWebPatientPayment, string DbConnectionString, string ServiceInstalltionId)
        //{
        //    return SynchOpenDentalDAL.Save_PatientPaymentLog_LocalToOpenDental(dtWebPatientPayment, DbConnectionString, ServiceInstalltionId);
        //}

        public static string Save_PatientSMSCallLog_LocalToOpenDental(DataTable dtWebPatientPayment, string DbConnectionString, string ServiceInstalltionId)
        {
            return SynchOpenDentalDAL.Save_PatientSMSCallLog_LocalToOpenDental(dtWebPatientPayment, DbConnectionString, ServiceInstalltionId);
        }

        public static void DeleteDuplicatePatientLog(string DbConnectionString, string ServiceInstalltionId)
        {
            SynchOpenDentalDAL.DeleteDuplicatePatientLog(DbConnectionString, ServiceInstalltionId);
        }

        public static bool SavePatientMedication(string DbString, string Service_Install_Id, ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            return SynchOpenDentalDAL.SavePatientMedication(DbString, Service_Install_Id, ref isRecordSaved, ref SavePatientEHRID, strPatientFormID);
        }

        public static bool DeletePatientMedication(string DbString, string Service_Install_Id, ref bool isRecordDeleted, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            return SynchOpenDentalDAL.DeletePatientMedication(DbString, Service_Install_Id, ref isRecordDeleted, ref DeletePatientEHRID, strPatientFormID);
        }

        public static bool Save_Patient_OpenDental_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                return SynchOpenDentalDAL.Save_Patient_OpenDental_To_Local_New(dtOpenDentalDataToSave, Clinic_Number, Service_Install_Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
