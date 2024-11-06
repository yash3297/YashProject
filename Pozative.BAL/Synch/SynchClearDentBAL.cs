using System;
using Pozative.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BAL
{
    public class SynchClearDentBAL
    {

        public static bool GetEHRConnection(int Application_ID)
        {
            if (Application_ID == 1)
            {
                return SynchClearDentDAL.GetClearDentConnection();
            }
            else
            {
                return SynchClearDentDAL.GetClearDentConnection();
            }

        }

        #region EHR_VersionNumber

        public static string GetClearDentEHR_VersionNumber()
        {
            return SynchClearDentDAL.GetClearDentEHR_VersionNumber();
        }

        #endregion

        #region Appointment

        public static DataTable GetClearDentAppointmentData(string strApptID = "")
        {
            return SynchClearDentDAL.GetClearDentAppointmentData(strApptID);
        }

        public static DataTable GetClearDentAppointmentIds()
        {
            return SynchClearDentDAL.GetClearDentAppointmentIds();
        }

        public static DataTable GetClearDentAppointment_Procedures_Data(string strApptID = "")
        {
            return SynchClearDentDAL.GetClearDentAppointment_Procedures_Data(strApptID);
        }

        public static bool Save_Appointment_ClearDent_To_Local(DataTable dtClearDentAppointment)
        {
            return SynchClearDentDAL.Save_Appointment_ClearDent_To_Local(dtClearDentAppointment);
        }


        #region DeletedAppointment

        public static DataTable GetClearDentDeletedAppointmentData()
        {
            return SynchClearDentDAL.GetClearDentDeletedAppointmentData();
        }

        public static bool Update_DeletedAppointment_ClearDent_To_Local(DataTable dtClearDentDeletedAppointment)
        {
            return SynchClearDentDAL.Update_DeletedAppointment_ClearDent_To_Local(dtClearDentDeletedAppointment);
        }

        #endregion

        #endregion

        #region OperatoryEvent

        public static DataTable GetClearDentOperatoryEventData()
        {
            return SynchClearDentDAL.GetClearDentOperatoryEventData();
        }
        public static DataTable GetClearDentOperatoryHours()
        {
            return SynchClearDentDAL.GetClearDentOperatoryHours();
        }
        public static bool Save_OperatoryEvent_ClearDent_To_Local(DataTable dtClearDentOperatoryEvent)
        {
            return SynchClearDentDAL.Save_OperatoryEvent_ClearDent_To_Local(dtClearDentOperatoryEvent);
        }

        public static DataTable GetClearDentOperatoryTimeOffHours()
        {
            return SynchClearDentDAL.GetClearDentOperatoryTimeOffHours();
        }
        #region DeletedOperatoryEvent

        public static DataTable GetClearDentDeletedOperatoryEventData()
        {
            return SynchClearDentDAL.GetClearDentDeletedOperatoryEventData();
        }
        public static DataTable GetLocalOperatoryEventData()
        {
            return SynchClearDentDAL.GetLocalOperatoryEventData();
        }
        public static bool Update_DeletedOperatoryEvent_ClearDent_To_Local(DataTable dtClearDentDeletedOperatoryEvent)
        {
            return SynchClearDentDAL.Update_DeletedOperatoryEvent_ClearDent_To_Local(dtClearDentDeletedOperatoryEvent);
        }
        #endregion

        #endregion

        #region Provider

        public static DataTable GetClearDentProviderData()
        {
            return SynchClearDentDAL.GetClearDentProviderData();
        }
        public static DataTable GetClearDentProviderHours()
        {
            return SynchClearDentDAL.GetClearDentProviderHours();
        }
        public static bool Save_Provider_ClearDent_To_Local(DataTable dtClearDentProvider)
        {
            return SynchClearDentDAL.Save_Provider_ClearDent_To_Local(dtClearDentProvider);
        }


        #region ProviderOfficeHours

        public static DataTable GetClearDentProviderOfficeHours()
        {
            return SynchClearDentDAL.GetClearDentProviderOfficeHours();
        }

        #endregion

        #endregion

        #region Speciality

        public static bool Save_Speciality_ClearDent_To_Local(DataTable dtClearDentSpeciality)
        {
            return SynchClearDentDAL.Save_Speciality_ClearDent_To_Local(dtClearDentSpeciality);
        }

        #endregion

        #region Folder List

        public static DataTable GetClearDentFolderListData()
        {
            return SynchClearDentDAL.GetClearDentFolderListData();
        }

        public static bool Save_FolderList_ClearDent_To_Local(DataTable dtClearDentOperatory, string Service_Install_Id, string clinicNumber)
        {

            return SynchClearDentDAL.Save_FolderList_ClearDent_To_Local(dtClearDentOperatory, Service_Install_Id, clinicNumber);
        }
        public static DataTable GetClearDentDeletedFolderListData()
        {
            return SynchClearDentDAL.GetClearDentDeletedFolderListData();
        }
        #endregion

        #region Operatory

        public static DataTable GetClearDentOperatoryData()
        {
            return SynchClearDentDAL.GetClearDentOperatoryData();
        }

        public static DataTable GetClearDentDeletedOperatoryData()
        {
            return SynchClearDentDAL.GetClearDentDeletedOperatoryData();
        }

        public static bool Save_Operatory_ClearDent_To_Local(DataTable dtClearDentOperatory)
        {

            return SynchClearDentDAL.Save_Operatory_ClearDent_To_Local(dtClearDentOperatory);
        }

        #endregion

        #region Operatory Office Hours

        public static DataTable GetClearDentOperatoryOfficeHours()
        {
            return SynchClearDentDAL.GetClearDentOperatoryOfficeHours();
        }

        #endregion

        #region ApptType

        public static DataTable GetClearDentApptTypeData()
        {
            return SynchClearDentDAL.GetClearDentApptTypeData();
        }

        public static bool Save_ApptType_ClearDent_To_Local(DataTable dtClearDentApptType)
        {
            return SynchClearDentDAL.Save_ApptType_ClearDent_To_Local(dtClearDentApptType);
        }

        #endregion

        #region Patient

        public static DataTable GetClearDentPatientInsuranceData(string PatientId)
        {
            return SynchClearDentDAL.GetClearDentPatientInsuranceData(PatientId);
        }
        public static DataTable GetClearDentPatientData()
        {
            return SynchClearDentDAL.GetClearDentPatientData();
        }

        public static DataTable GetClearDentPatientStatusData(string strPatID = "")
        {
            return SynchClearDentDAL.GetClearDentPatientStatusData(strPatID);
        }

        public static DataTable GetClearDentAppointmentsPatientData(string strPatID = "")
        {
            return SynchClearDentDAL.GetClearDentAppointmentsPatientData(strPatID);
        }

        public static DataTable GetCleardentPatientDataOfPatientId(string PatientIds)
        {
            try
            {
                return SynchClearDentDAL.GetCleardentPatientDataOfPatientId(PatientIds);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public static DataTable GetClearDentPatientIdsData()
        {
            try
            {
                return SynchClearDentDAL.GetClearDentPatientIdsData();
            }
            catch (Exception)
            {

                throw;
            }           
        }

        public static bool Save_Patient_Cleardent_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                return SynchClearDentDAL.Save_Patient_Cleardent_To_Local_New(dtOpenDentalDataToSave, Clinic_Number, Service_Install_Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static DataTable GetClearDentPatientcollect_payment()
        {
            return SynchClearDentDAL.GetClearDentPatientcollect_payment();
        }
        public static DataTable GetClearDentPatient_recall()
        {
            return SynchClearDentDAL.GetClearDentPatient_recall();
        }
        public static DataTable GetClearDentPatient_RecallType()
        {
            return SynchClearDentDAL.GetClearDentPatient_RecallType();
        }
        public static DataTable GetClearDentPatientdue_date()
        {
            return SynchClearDentDAL.GetClearDentPatientdue_date();
        }
        public static bool Save_Patient_ClearDent_To_Local(DataTable dtClearDentPatient, string InsertTableName, bool bSetDeleted = false)
        {
            return SynchClearDentDAL.Save_Patient_ClearDent_To_Local(dtClearDentPatient, InsertTableName, bSetDeleted);
        }


        public static string Push_Local_To_LiveDatabase_Patient_Async(string JsonString, string TableName)
        {
            return SynchClearDentDAL.Push_Local_To_LiveDatabase_Patient_Async(JsonString, TableName);
        }

        #endregion

        #region RecallType

        public static DataTable GetClearDentRecallTypeData()
        {
            return SynchClearDentDAL.GetClearDentRecallTypeData();
        }

        public static bool Save_RecallType_ClearDent_To_Local(DataTable dtClearDentRecallType)
        {
            return SynchClearDentDAL.Save_RecallType_ClearDent_To_Local(dtClearDentRecallType);
        }

        #endregion

        #region User
        public static DataTable GetClearDentUserData()
        {
            return SynchClearDentDAL.GetClearDentUserData();
        }

        public static bool Save_User_ClearDent_To_Local(DataTable dtCleardentUser)
        {
            return SynchClearDentDAL.Save_User_ClearDent_To_Local(dtCleardentUser);
        }
        #endregion

        #region ApptStatus

        public static DataTable GetClearDentApptStatusData()
        {
            return SynchClearDentDAL.GetClearDentApptStatusData();
        }

        public static bool Save_ApptStatus_ClearDent_To_Local(DataTable dtClearDentApptStatus)
        {
            return SynchClearDentDAL.Save_ApptStatus_ClearDent_To_Local(dtClearDentApptStatus);
        }
        #endregion

        //#region Holidays

        //public static DataTable GetClearDentHolidaysData()
        //{
        //    return SynchClearDentDAL.GetClearDentHolidaysData();
        //}
        //public static DataTable GetClearDentOperatoryHolidaysData(DataTable dtOperatory)
        //{
        //    return SynchClearDentDAL.GetClearDentOperatoryHolidaysData(dtOperatory);
        //}
        //public static bool Save_Holidays_ClearDent_To_Local(DataTable dtClearDentOperatoryEvent)
        //{
        //    return SynchClearDentDAL.Save_Holidays_ClearDent_To_Local(dtClearDentOperatoryEvent);
        //}
        //public static bool Save_Opeatory_Holidays_ClearDent_To_Local(DataTable dtClearDentOperatoryEvent)
        //{
        //    return SynchClearDentDAL.Save_Opeatory_Holidays_ClearDent_To_Local(dtClearDentOperatoryEvent);
        //}

        //#endregion

        #region CreateAppointment

        public static DataTable GetClearDentPatientID_NameData()
        {
            return SynchClearDentDAL.GetClearDentPatientID_NameData();
        }

        public static DataTable GetClearDentIdelProvider()
        {
            return SynchClearDentDAL.GetClearDentIdelProvider();
        }

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate)
        {
            return SynchClearDentDAL.GetBookOperatoryAppointmenetWiseDateTime(ApptDate);
        }
        public static int Save_Patient_Local_To_ClearDent(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, long tmpPatient_Gur_id, string Birth_Date)
        {
            try
            {
                return SynchClearDentDAL.Save_Patient_Local_To_ClearDent(LastName, FirstName, MiddleName, Mobile, Email, PriProv, DateFirstVisit, tmpPatient_Gur_id, Birth_Date);
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        public static int Save_Appointment_Local_To_ClearDent(string PatNum, int Pattern, string Op, string ProvNum, DateTime StartTime, DateTime EndTime, string DateTStamp, string AppointmentTypeNum, string PatientName, string comment, string TreatmentCodes, int appointmentstatuskey) //
        {
            return SynchClearDentDAL.Save_Appointment_Local_To_ClearDent(PatNum, Pattern, Op, ProvNum, StartTime, EndTime, DateTStamp, AppointmentTypeNum, PatientName, comment, TreatmentCodes, appointmentstatuskey); //
        }
        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id,string _filename_Appointment = "",string _EHRLogdirectory_Appointment = "")
        {
            return SynchClearDentDAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id, tmpAppt_Web_id, _filename_Appointment = "", _EHRLogdirectory_Appointment = "");
        }
        public static bool Update_Status_EHR_Appointment_Live_To_ClearDentEHR(DataTable dtLiveAppointment,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchClearDentDAL.Update_Status_EHR_Appointment_Live_To_ClearDentEHR(dtLiveAppointment,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
        }
        public static bool Update_Receive_SMS_Patient_EHR_Live_To_ClearDentEHR(DataTable dtLiveAppointment,string Locatioid, string Loc_ID,string _filename_EHR_patient_sms_call = "",string _EHRLogdirectory_EHR_patient_sms_call = "")
        {
            return SynchClearDentDAL.Update_Receive_SMS_Patient_EHR_Live_To_ClearDentEHR(dtLiveAppointment,Locatioid,Loc_ID,_filename_EHR_patient_sms_call,_EHRLogdirectory_EHR_patient_sms_call);
        }
        #endregion

        #region Patient_Form
        public static bool Save_Patient_Form_Local_To_ClearDent(DataTable dtWebPatient_Form)
        {
            return SynchClearDentDAL.Save_Patient_Form_Local_To_ClearDent(dtWebPatient_Form);
        }

        public static bool Save_Document_in_ClearDent(string strPatientFormID = "")
        {
            return SynchClearDentDAL.Save_Document_in_ClearDent(strPatientFormID);
        }

        public static bool Save_Treatment_Document_in_ClearDent(string strTreatmentPlanID = "")
        {
            return SynchClearDentDAL.Save_Treatment_Document_in_ClearDent(strTreatmentPlanID);
        }

        public static bool Save_InsuranceCarrier_Document_in_ClearDent(string strInsuranceCarrierID = "")
        {
            return SynchClearDentDAL.Save_InsuranceCarrier_Document_in_ClearDent(strInsuranceCarrierID);
        }

        public static string GetClearDentDocPath(string Connectionstring)
        {
            return SynchClearDentDAL.GetClearDentDocPath(Connectionstring);
        }
        #endregion

        public static DataSet GetClearDentMedicalHistoryData()
        {
            return SynchClearDentDAL.GetClearDentMedicalHistoryData();
        }

        public static DataTable GetCleardentMedicationData()
        {
            return SynchClearDentDAL.GetCleardentMedicationData();
        }

        public static DataTable GetCleardentPatientMedicationData(string PatientEHRID)
        {
            return SynchClearDentDAL.GetCleardentPatientMedicationData(PatientEHRID);
        }

        public static bool SaveMedicalHistoryLocalToClearDent(string strPatientFormID = "")
        {
            return SynchClearDentDAL.SaveMedicalHistoryLocalToClearDent(strPatientFormID);
        }

        public static string Save_PatientPayment_Local_To_ClearDent(DataTable dtWebPatientPayment, string DbConnectionString, string ServiceInstalltionId, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            return SynchClearDentDAL.Save_PatientPayment_Local_To_ClearDent(dtWebPatientPayment, DbConnectionString, ServiceInstalltionId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
        }

        public static string Save_PatientSMSCall_Local_To_ClearDent(DataTable dtWebPatientPayment, string DbConnectionString, string ServiceInstalltionId)
        {
            return SynchClearDentDAL.Save_PatientSMSCall_Local_To_ClearDent(dtWebPatientPayment, DbConnectionString, ServiceInstalltionId);
        }

        public static DataTable GetClearDentPatientImagesData(string DbString)
        {
            return SynchClearDentDAL.GetClearDentPatientImagesData(DbString);
        }

        public static void DeleteDuplicatePatientLog(string DbConnectionString, string ServiceInstalltionId)
        {
            SynchClearDentDAL.DeleteDuplicatePatientLog(DbConnectionString, ServiceInstalltionId);
        }

        public static bool SavePatientMedicationLocalToClearDent(string ServiceInstalltionId,ref bool IsSaveMedication, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            return SynchClearDentDAL.SavePatientMedicationLocalToClearDent(ServiceInstalltionId,ref IsSaveMedication, ref SavePatientEHRID, strPatientFormID);
        }
        public static bool DeletePatientMedicationLocalToClearDent(string ServiceInstalltionId,ref bool IsDeletedMedication, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            return SynchClearDentDAL.DeletePatientMedicationLocalToClearDent(ServiceInstalltionId, ref IsDeletedMedication, ref DeletePatientEHRID, strPatientFormID);
        }

        #region Insurance
        public static DataTable GetClearDentInsuranceData()
        {
            return SynchClearDentDAL.GetClearDentInsuranceData();
        }
        public static bool Save_Insurance_ClearDent_To_Local(DataTable dtClearDentInsurance)
        {
            return SynchClearDentDAL.Save_Insurance_ClearDent_To_Local(dtClearDentInsurance);
        }       
        #endregion


    }
}
