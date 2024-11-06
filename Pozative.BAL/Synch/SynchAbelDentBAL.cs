using System;
using Pozative.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pozative.BAL
{
    public class SynchAbelDentBAL
    {

        public static bool Save_AbelDent_To_Local(DataTable dtAbelDentDataToSave, string tablename, string ignoreColumnsName, string primaryKeyColumnsName)
        {
            try
            {
                return SynchAbelDentDAL.Save_AbelDent_To_Local(dtAbelDentDataToSave, tablename, ignoreColumnsName, primaryKeyColumnsName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Save_AbelDent_To_Local(DataTable dtAbelDentDataToSave, string patientTableName, DataTable DtLocal, bool bSetDeleted = false)
        {
            try
            {
                return SynchAbelDentDAL.Save_AbelDent_To_Local(dtAbelDentDataToSave, patientTableName, DtLocal, bSetDeleted);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Save_Patient_Local_To_AbelDent(string LastName, string FirstName, string MiddleName, ref Int64 PatientID, string MobileNo, string Email, string ApptProv, string AppointmentDateTime, int Patient_Gur_id, string OperatoryId, string Birth_Date)
        {
            try
            {
                return SynchAbelDentDAL.Save_Patient_Local_To_AbelDent(LastName, FirstName, MiddleName,ref PatientID,MobileNo, Email, ApptProv, AppointmentDateTime, Patient_Gur_id, OperatoryId, Birth_Date);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static string Get_Patient_UniqID(int PatientID)
        {
            try
            {
                return SynchAbelDentDAL.Save_Patient_UniqID(PatientID);
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Save_AbelDentPatient_To_Local

        public static bool Save_Patient_AbelDent_To_Local(DataTable dtAbelDentPatient, string Service_Install_Id)
        {
            return SynchAbelDentDAL.Save_Patient_AbelDent_To_Local(dtAbelDentPatient, Service_Install_Id);
        }

        public static string Save_Appointment_Local_To_AbelDent(string FirstNameLastName, DateTime AppointmentStartTime, DateTime AppointmentEndTime, Int64 PatientID,string PatUniqID, string OperatoryId,int reqTime,
            string classification, string ApptType, DateTime AppointedDateTime, string ProvNum, string AppointmentConfirmationStatus, bool allday_event, bool sooner_if_possible, bool privateAppointment, bool auto_confirm_sent, string procedureCode, string apptStatusId)
        {
            return SynchAbelDentDAL.Save_Appointment_Local_To_AbelDent(FirstNameLastName, AppointmentStartTime, AppointmentEndTime, PatientID,PatUniqID, OperatoryId, reqTime,classification, ApptType, AppointedDateTime, ProvNum, AppointmentConfirmationStatus, allday_event, sooner_if_possible, privateAppointment, auto_confirm_sent, procedureCode, apptStatusId);
        }

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string AppointmentEHRId, string AppointmentWebId)
        {
            return SynchAbelDentDAL.Update_Appointment_EHR_Id_Web_Book_Appointment(AppointmentEHRId, AppointmentWebId);
        }

        public static bool SavePatientAllergies_To_AbelDent(string strPatientFormID = "")
        {
            return SynchAbelDentDAL.SavePatientAllergies_To_AbelDent(strPatientFormID);
        }

        public static bool SavePatientMedicationLocalToAbelDent(ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            return SynchAbelDentDAL.SavePatientMedicationLocalToAbelDent(ref isRecordSaved, ref SavePatientEHRID, strPatientFormID);
        }

        //public static bool Save_Provider_AbelDent_To_Local(DataTable dtAbelDentProvider)
        //{
        //    return SynchAbelDentDAL.Save_Provider_AbelDent_To_Local(dtAbelDentProvider);
        //}

        //public static bool Save_Speciality_AbelDent_To_Local(DataTable dtAbelDentSpeciality)
        //{
        //    return SynchAbelDentDAL.Save_Speciality_AbelDent_To_Local(dtAbelDentSpeciality);
        //}

        public static DataTable GetAbelDentOperatoryData()
        {
            return SynchAbelDentDAL.GetAbelDentOperatoryData();
        }

        public static DataTable GetAbelDentOperatoryOfficeHours()
        {
            return SynchAbelDentDAL.GetAbelDentOperatoryOfficeHours();
        }

        public static DataTable GetAbelDentOperatoryOfficeHoursOP()
        {
            return SynchAbelDentDAL.GetAbelDentOperatoryOfficeHoursOP();
        }

        public static DataTable GetAbelDentProviderOfficeHours()
        {
            return SynchAbelDentDAL.GetAbelDentProviderOfficeHours();
        }

        public static DataTable GetAbelDentProviderOfficeHoursOP()
        {
            return SynchAbelDentDAL.GetAbelDentProviderOfficeHoursOP();
        }

        public static DataTable GetAbelDentSpecialityData()
        {
            return SynchAbelDentDAL.GetAbelDentSpecialityData();
        }

        //public static bool Save_Operatory_AbelDent_To_Local(DataTable dtAbelDentOperatory)
        //{

        //    return SynchAbelDentDAL.Save_Operatory_AbelDent_To_Local(dtAbelDentOperatory);
        //}

        public static DataTable GetAbelDentApptTypeData()
        {
            return SynchAbelDentDAL.GetAbelDentApptTypeData();
        }

        //public static bool Save_ApptType_AbelDent_To_Local(DataTable dtAbelDentApptType)
        //{
        //    return SynchAbelDentDAL.Save_ApptType_AbelDent_To_Local(dtAbelDentApptType);
        //}

        ////public static DataTable GetAbelDentPatientInsuranceData(string PatientId)
        ////{
        ////    return SynchAbelDentDAL.GetAbelDentPatientInsuranceData(PatientId);
        ////}

        #region Patient

        public static DataTable GetAbelDentPatientData()
        {
            return SynchAbelDentDAL.GetAbelDentPatientData();
        }

        public static DataTable GetAbelDentPatientDataWithCondition(string condition)
        {
            return SynchAbelDentDAL.GetAbelDentPatientDataWithCondition(condition);
        }

        public static DataTable GetAbelDentPatientIDData()
        {
            return SynchAbelDentDAL.GetAbelDentPatientIdData();
        }
        public static DataTable GetAbelDentPatientStatusData(string strPatID = "")
        {
            return SynchAbelDentDAL.GetAbelDentPatientStatusData(strPatID);
        }   
        
        public static DataTable GetAbelDentPatient_Balance()
        {
            return SynchAbelDentDAL.GetAbelDentPatient_Balance();
        }

        public static DataTable GetAbelDentPatient_LastVisitDate()
        {
            return SynchAbelDentDAL.GetAbelDentPatient_LastVisitDate();
        }

        public static DataTable GetAbelDentAppointmentsPatientData(string strPatID = "")
        {
            return SynchAbelDentDAL.GetAbelDentAppointmentsPatientData(strPatID);
        }

        public static DataTable GetAbelDentMedicleFormData()
        {
            return SynchAbelDentDAL.GetAbelDentMedicleFormData();
        }

        public static DataTable GetAbelDentMedicleAnswerData()
        {
            return SynchAbelDentDAL.GetAbelDentMedicalAnswerData();
        }

        public static DataTable GetAbelDentPatientListData()
        {
            return SynchAbelDentDAL.GetAbelDentPatientListData();
        }

        public static DataTable GetAbelDentMedicleFormQuestionData()
        {
            return SynchAbelDentDAL.GetAbelDentMedicleFormQuestionData();
        }
        public static bool SaveMedicalHistoryLocalToAbelDent()
        {
            return SynchAbelDentDAL.SaveMedicalHistoryLocalToAbelDent();
        }

        public static DataTable GetAbelDentPatientDiseaseData()
        {
            return SynchAbelDentDAL.GetAbelDentPatientDiseaseData();
        }

        public static bool Save_PatientDisease_AbelDent_To_Local(DataTable dtOpenDentalDisease, string Service_Install_Id)
        {
            return SynchAbelDentDAL.Save_PatientDisease_AbelDent_To_Local(dtOpenDentalDisease, Service_Install_Id);
        }

        public static string GetPatientName(string patientid)
        {
            return SynchAbelDentDAL.GetPatientName(patientid);
        }    //GetPatientId

        public static string GetPatientId(string patientid)
        {
            return SynchAbelDentDAL.GetPatientId(patientid);
        }

        public static bool Save_Document_in_AbelDent(string strPatientFormID = "")
        {
            return SynchAbelDentDAL.Save_Document_in_AbelDent(strPatientFormID);
        }

        public static bool Save_Treatment_Document_in_AbelDent(string strTreatmentPlanID = "")
        {
            return SynchAbelDentDAL.Save_Treatment_Document_in_AbelDent(strTreatmentPlanID);
        }

        public static DataTable GetAbelDentMedicationData()
        {
            return SynchAbelDentDAL.GetAbelDentMedicationData();
        }

        public static DataTable GetAbelDentPatientMedicationData(string PatientEHRID)
        {
            return SynchAbelDentDAL.GetAbelDentPatientMedicationData(PatientEHRID);
        }

        #endregion

        ////public static DataTable GetAbelDentPatientcollect_payment()
        ////{
        ////    return SynchAbelDentDAL.GetAbelDentPatientcollect_payment();
        ////}

        public static DataTable GetAbelDentPatient_recall()
        {
            return SynchAbelDentDAL.GetAbelDentPatient_recall();
        }

        public static DataTable GetAbelDentRecallTypeData()
        {
            return SynchAbelDentDAL.GetAbelDentRecallTypeData();
        }
        public static DataTable GetAbelDentApptStatusData()
        {
            return SynchAbelDentDAL.GetAbelDentApptStatusData();
        }

        public static string GetEHR_VersionNumber()
        {
            return SynchAbelDentDAL.GetEHR_VersionNumber();
        }      
       

        public static DataTable GetAbelDentHolidayData()
        {
            return SynchAbelDentDAL.GetAbelDentHolidaysData();
        }

        public static bool Update_Status_EHR_Appointment_Live_To_AbelDentEHR(DataTable dtLiveAppointment,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchAbelDentDAL.Update_Status_EHR_Appointment_Live_To_AbelDentEHR(dtLiveAppointment,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
        }

        public static bool Insert_Status_EHR_Appointment_To_AbelDentEHR(DataTable dtLiveAppointment,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchAbelDentDAL.Insert_Status_EHR_Appointment_To_AbelDentEHR(dtLiveAppointment,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
        }

        public static bool Update_Receive_SMS_Patient_EHR_Live_To_AbelDentEHR(DataTable dtLiveAppointment,string locationid, string Loc_ID)
        {
            return SynchAbelDentDAL.Update_Receive_SMS_Patient_EHR_Live_To_AbelDentEHR(dtLiveAppointment,locationid,Loc_ID);
        }
        public static bool Save_Patient_Form_Local_To_AbelDent(DataTable dtWebPatient_Form)
        {
            return SynchAbelDentDAL.Save_Patient_Form_Local_To_AbelDent(dtWebPatient_Form);
        }

        public static DataTable GetAbelDentDefaultProviderData()
        {
            return SynchAbelDentDAL.GetAbelDentDefaultProviderData();
        }

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime datetime,int _minutesInUnit)
        {
            return SynchAbelDentDAL.GetBookOperatoryAppointmenetWiseDateTime(datetime, _minutesInUnit);
        }

        public static List<string> GetAbelDentActiveColumnData()
        {
            return SynchAbelDentDAL.GetAbelDentActiveColumnData();
        }

        public static List<AbelOpenings> GetAbelDentOpeingsList(int TimeUnits, DateTime StartDate, DateTime EndDate, List<string> ColumnIDs, string Days, bool ReservTime, string ReservWork, int NumberOfOpenings, int _unitNumbersPerDay, DateTime _dayStartTime, DateTime _dayEndTime, int _minutesInUnit,bool _isFreeBlock)
        {
            return SynchAbelDentDAL.OpeningsList(TimeUnits, StartDate, EndDate, ColumnIDs, Days, ReservTime, ReservWork, NumberOfOpenings, _unitNumbersPerDay, _dayStartTime, _dayEndTime, _minutesInUnit, _isFreeBlock);
        }

        public static void GetAbelDentSystemData(ref DateTime startDate, ref DateTime endDate, ref int unitDay,ref string workingDay)
        {
            SynchAbelDentDAL.GetAbelDentSystemData(ref startDate,ref endDate,ref unitDay,ref workingDay);
        }

        //public static string Save_PatientPaymentLog_LocalToAbelDent(DataTable dtWebPatientPayment, string DbString, string ServiceInstallationId)
        //{
        //   return SynchAbelDentDAL.Save_PatientPaymentLog_LocalToAbelDent(dtWebPatientPayment, DbString, ServiceInstallationId);
        //}
        public static string Save_PatientSMSCallLog_LocalToAbelDent(DataTable dtWebPatientPayment, string DbString, string ServiceInstallationId)
        {
            return SynchAbelDentDAL.Save_PatientSMSCallLog_LocalToAbelDent(dtWebPatientPayment, DbString, ServiceInstallationId);
        }

        public static Int64 SavePatientPaymentTOEHR(string DbString, DataTable dtTable, string ServiceInstallationId, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            return SynchAbelDentDAL.SavePatientPaymentTOEHR(DbString, dtTable, ServiceInstallationId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
        }

        #region Provider
        public static DataTable GetAbelDentProviderData()
        {
            return SynchAbelDentDAL.GetAbelDentProviderData();
        }
        public static bool Save_Provider_AbelDent_To_Local(DataTable dtAbelDentProvider, string Service_Install_Id)
        {
            return SynchAbelDentDAL.Save_Provider_AbelDent_To_Local(dtAbelDentProvider, Service_Install_Id);
        }
        #endregion

        #region Operatory
        public static bool Save_Operatory_AbelDent_To_Local(DataTable dtAbelDentOperatory, string Service_Install_Id)
        {
            return SynchAbelDentDAL.Save_Operatory_AbelDent_To_Local(dtAbelDentOperatory, Service_Install_Id);
        }
        #endregion

        #region  Appointment 
        public static bool Save_ApptStatus_AbelDent_To_Local(DataTable dtAbelDentApptStatus, string Service_Install_Id)
        {
            return SynchAbelDentDAL.Save_ApptStatus_AbelDent_To_Local(dtAbelDentApptStatus, Service_Install_Id);
        }
        public static DataTable GetAbelDentAppointmentData(string strApptID = "",string _minutesInUnit = "15")
        {
            return SynchAbelDentDAL.GetAbelDentAppointmentData(strApptID, _minutesInUnit);
        }
        public static DataTable GetAbelDentAppointment_Procedures_Data(string strApptID = "")
        {
            return SynchAbelDentDAL.GetAbelDentAppointment_Procedures_Data(strApptID);
        }
        public static DataTable GetAbelDentAppointmentIds()
        {
            return SynchAbelDentDAL.GetAbelDentAppointmentIds();
        }
        #endregion

        #region  Disease       

        public static DataTable GetAbelDentDiseaseData()
        {
            return SynchAbelDentDAL.GetAbelDentDiseaseData();
        }

        public static DataTable GetAbelDentProblemData()
        {
            return SynchAbelDentDAL.GetAbelDentProblemData();
        }

        public static bool Save_Disease_AbelDent_To_Local(DataTable dtOpenDentalDisease, string Service_Install_Id)
        {
            return SynchAbelDentDAL.Save_Disease_AbelDent_To_Local(dtOpenDentalDisease, Service_Install_Id);
        }

        #endregion

        #region User
        public static DataTable GetAbelDentUser()
        {
            return SynchAbelDentDAL.GetAbelDentUser();
        }
        public static bool Save_Users_AbelDent_To_Local(DataTable dtTrackerUser)
        {
            return SynchAbelDentDAL.Save_Users_AbelDent_To_Local(dtTrackerUser);
        }
        #endregion

        #region Event Listener
        public static bool Save_Patient_AbelDent_To_Local_New(DataTable dtSaveRecords, string Clinic_Number, string Service_Install_Id)
        {
            return SynchAbelDentDAL.Save_Patient_AbelDent_To_Local_New(dtSaveRecords, Clinic_Number, Service_Install_Id);
        }
        #endregion
    }

}
