using System;
using Pozative.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pozative.BAL
{
    public class SynchTrackerBAL
    {

        public static bool Save_Tracker_To_Local(DataTable dtSoftDentDataToSave, string tablename, string ignoreColumnsName, string primaryKeyColumnsName,string  _filename_EHR_PatientFormt="",string _EHRLogdirectory_EHR_PatientForm="")
        {
            try
            {
                return SynchTrackerDAL.Save_Tracker_To_Local(dtSoftDentDataToSave, tablename, ignoreColumnsName, primaryKeyColumnsName,_filename_EHR_PatientFormt,_EHRLogdirectory_EHR_PatientForm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Save_Tracker_Patient_To_Local_New(DataTable dtSoftDentDataToSave)
        {
            return SynchTrackerDAL.Save_Tracker_Patient_To_Local_New(dtSoftDentDataToSave);
        }

        public static bool Save_Tracker_To_Local(DataTable dtSoftDentDataToSave, string patientTableName, DataTable DtLocal, bool bSetDeleted =false)
        {
            try
            {
                return SynchTrackerDAL.Save_Tracker_To_Local(dtSoftDentDataToSave, patientTableName, DtLocal, bSetDeleted);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public static Int64 SavePatientPaymentTOEHR(string DbString, DataTable dtTable, DataTable splitdt, string ServiceInstallationId)
        //{
        //    return SynchTrackerDAL.SavePatientPaymentTOEHR(DbString, dtTable,splitdt, ServiceInstallationId);
        //}
        public static bool Save_Users_Tracker_To_Local(DataTable dtTrackerUser)
        {
            return SynchTrackerDAL.Save_Users_Tracker_To_Local(dtTrackerUser);
        }
        public static Int64 SavePatientPaymentTOEHR(string DbString, DataTable dtTable,  string ServiceInstallationId, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            return SynchTrackerDAL.SavePatientPaymentTOEHR(DbString, dtTable, ServiceInstallationId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
        }
        public static string GetPatientName(int patientid)
        {
            return SynchTrackerDAL.GetPatientName(patientid);
        }
        //public static bool GetEHRConnection(int Application_ID)
        //{
        //    if (Application_ID == 1)
        //    {
        //        return SynchTrackerDAL.GetTrackerConnection();
        //    }
        //    else
        //    {
        //        return SynchTrackerDAL.GetTrackerConnection();
        //    }

        //}

        public static Int64 Save_Patient_Local_To_Tracker(string LastName, string FirstName, string MiddleName, string MobileNo, string Email, string ApptProv, string AppointmentDateTime, int Patient_Gur_id, int OperatoryId, string Birth_Date)
        {
            try
            {
                return SynchTrackerDAL.Save_Patient_Local_To_Tracker(LastName, FirstName, MiddleName, MobileNo, Email, ApptProv, AppointmentDateTime, Patient_Gur_id, OperatoryId, Birth_Date);
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public static Int64 Save_Appointment_Local_To_Tracker(string FirstNameLastName, DateTime AppointmentStartTime, DateTime AppointmentEndTime, string PatNum, string OperatoryId,
            string classification, string ApptTypeId, DateTime AppointedDateTime,DateTime dob, string ProvNum, string AppointmentConfirmationStatus, bool allday_event, bool sooner_if_possible, bool privateAppointment, bool auto_confirm_sent,string procedureCode,string apptStatusId)
        {
            return SynchTrackerDAL.Save_Appointment_Local_To_Tracker(FirstNameLastName, AppointmentStartTime, AppointmentEndTime, PatNum, OperatoryId, classification, ApptTypeId, AppointedDateTime,dob, ProvNum, AppointmentConfirmationStatus, allday_event, sooner_if_possible, privateAppointment, auto_confirm_sent,procedureCode,apptStatusId);
        }

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string AppointmentEHRId, string AppointmentWebId, string Service_Install_Id)
        {
            return SynchEaglesoftBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(AppointmentEHRId, AppointmentWebId, Service_Install_Id);
        }


        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime datetime)
        {
            return SynchTrackerDAL.GetBookOperatoryAppointmenetWiseDateTime(datetime);
        }

        public static DataTable GetTrackerAppointmentData(string strApptID = "")
        {
            return SynchTrackerDAL.GetTrackerAppointmentData(strApptID);
        }

        public static DataTable GetTrackerAppointment_Procedures_Data(string strApptID = "")
        {
            return SynchTrackerDAL.GetTrackerAppointment_Procedures_Data(strApptID);
        }
        

        public static DataTable GetTrackerAppointmentIds()
        {
            return SynchTrackerDAL.GetTrackerAppointmentIds();
        }

        ////public static DataTable GetTrackerDeletedAppointmentData()
        ////{
        ////    return SynchTrackerDAL.GetTrackerDeletedAppointmentData();
        ////}
        ////public static DataTable GetTrackerDeletedOperatoryEventData()
        ////{
        ////    return SynchTrackerDAL.GetTrackerDeletedOperatoryEventData();
        ////}
        ////public static DataTable GetTrackerPatientdue_date()
        ////{
        ////    return SynchTrackerDAL.GetTrackerPatientdue_date();
        ////}

        //public static bool Save_Appointment_Tracker_To_Local(DataTable dtTrackerAppointment)
        //{
        //    return SynchTrackerDAL.Save_Appointment_Tracker_To_Local(dtTrackerAppointment);
        //}

        //public static bool Update_DeletedAppointment_Tracker_To_Local(DataTable dtTrackerDeletedAppointment)
        //{
        //    return SynchTrackerDAL.Update_DeletedAppointment_Tracker_To_Local(dtTrackerDeletedAppointment);
        //}

        //public static bool Update_DeletedOperatoryEvent_Tracker_To_Local(DataTable dtTrackerDeletedOperatoryEvent)
        //{
        //    return SynchTrackerDAL.Update_DeletedOperatoryEvent_Tracker_To_Local(dtTrackerDeletedOperatoryEvent);
        //}
        public static DataTable GetTrackerOperatoryEventData()
        {
            return SynchTrackerDAL.GetTrackerOperatoryEventData();
        }

        //public static bool Save_OperatoryEvent_Tracker_To_Local(DataTable dtTrackerOperatoryEvent)
        //{
        //    return SynchTrackerDAL.Save_OperatoryEvent_Tracker_To_Local(dtTrackerOperatoryEvent);
        //}

        public static DataTable GetTrackerProviderData()
        {
            return SynchTrackerDAL.GetTrackerProviderData();
        }

        public static DataTable GetTrackerDefaultProviderData()
        {
            return SynchTrackerDAL.GetTrackerDefaultProviderData();
        }

        //public static bool Save_Provider_Tracker_To_Local(DataTable dtTrackerProvider)
        //{
        //    return SynchTrackerDAL.Save_Provider_Tracker_To_Local(dtTrackerProvider);
        //}

        //public static bool Save_Speciality_Tracker_To_Local(DataTable dtTrackerSpeciality)
        //{
        //    return SynchTrackerDAL.Save_Speciality_Tracker_To_Local(dtTrackerSpeciality);
        //}

        public static DataTable GetTrackerOperatoryData()
        {
            return SynchTrackerDAL.GetTrackerOperatoryData();
        }

        public static DataTable GetTrackerSpecialityData()
        {
            return SynchTrackerDAL.GetTrackerSpecialityData();
        }

        //public static bool Save_Operatory_Tracker_To_Local(DataTable dtTrackerOperatory)
        //{

        //    return SynchTrackerDAL.Save_Operatory_Tracker_To_Local(dtTrackerOperatory);
        //}

        public static DataTable GetTrackerApptTypeData()
        {
            return SynchTrackerDAL.GetTrackerApptTypeData();
        }

        //public static bool Save_ApptType_Tracker_To_Local(DataTable dtTrackerApptType)
        //{
        //    return SynchTrackerDAL.Save_ApptType_Tracker_To_Local(dtTrackerApptType);
        //}

        ////public static DataTable GetTrackerPatientInsuranceData(string PatientId)
        ////{
        ////    return SynchTrackerDAL.GetTrackerPatientInsuranceData(PatientId);
        ////}

        public static DataTable GetTrackerPatientData()
        {
            return SynchTrackerDAL.GetTrackerPatientData();
        }
        public static DataTable GetTrackerPatientStatusData(string strPatID = "")
        {
            return SynchTrackerDAL.GetTrackerPatientStatusData(strPatID);
        }
        public static DataTable GetTrackerAppointmentsPatientData(string strPatID = "")
        {
            return SynchTrackerDAL.GetTrackerAppointmentsPatientData(strPatID);
        }

        public static DataTable GetTrackerPatientDatawithPatientId(string PatientIds)
        {
            try
            {
                return SynchTrackerDAL.GetTrackerPatientDataOfPatientId(PatientIds);
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        public static DataTable GetTrackerPatientIDsData()
        {
            try
            {
                return SynchTrackerDAL.GetTrackerPatientIDsData();
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public static bool Save_Patient_Tracker_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                return SynchTrackerDAL.Save_Patient_Tracker_To_Local_New(dtOpenDentalDataToSave, Clinic_Number, Service_Install_Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static DataTable GetTrackerPatientListData()
        {
            return SynchTrackerDAL.GetTrackerPatientListData();
        }


        ////public static DataTable GetTrackerPatientcollect_payment()
        ////{
        ////    return SynchTrackerDAL.GetTrackerPatientcollect_payment();
        ////}

        ////public static DataTable GetTrackerPatient_recall()
        ////{
        ////    return SynchTrackerDAL.GetTrackerPatient_recall();
        ////}

        ////public static DataTable GetTrackerPatient_RecallType()
        ////{
        ////    return SynchTrackerDAL.GetTrackerPatient_RecallType();
        ////}

        //public static bool Save_Patient_Tracker_To_Local(DataTable dtTrackerPatient)
        //{
        //    return SynchTrackerDAL.Save_Patient_Tracker_To_Local(dtTrackerPatient);
        //}

        public static DataTable GetTrackerRecallTypeData()
        {
            return SynchTrackerDAL.GetTrackerRecallTypeData();
        }

        public static DataTable GetTrackerUser()
        {
            return SynchTrackerDAL.GetTrackerUser();
        }

        //rooja tracker
        public static DataTable GetTrackerPatDocLocation()
        {
            return SynchTrackerDAL.GetTrackerPatDocLocation();
        }
        //public static bool Save_RecallType_Tracker_To_Local(DataTable dtTrackerRecallType)
        //{
        //    return SynchTrackerDAL.Save_RecallType_Tracker_To_Local(dtTrackerRecallType);
        //}

        public static DataTable GetTrackerApptStatusData()
        {
            return SynchTrackerDAL.GetTrackerApptStatusData();
        }

        public static string GetEHR_VersionNumber()
        {
            return SynchTrackerDAL.GetEHR_VersionNumber();
        }
        //public static string GetTrackerUserLoginId()
        //{
        //    return SynchTrackerDAL.GetTrackerUserLoginId();
        //}

        //public static bool Save_ApptStatus_Tracker_To_Local(DataTable dtTrackerApptStatus)
        //{
        //    return SynchTrackerDAL.Save_ApptStatus_Tracker_To_Local(dtTrackerApptStatus);
        //}

        ////#region Holidays


        ////public static DataTable GetTrackerOperatoryHolidaysData(DataTable dtOperatory)
        ////{
        ////    return SynchTrackerDAL.GetTrackerOperatoryHolidaysData(dtOperatory);
        ////}
        ////public static bool Save_Holidays_Tracker_To_Local(DataTable dtTrackerOperatoryEvent)
        ////{
        ////    return SynchTrackerDAL.Save_Holidays_Tracker_To_Local(dtTrackerOperatoryEvent);
        ////}
        ////public static bool Save_Opeatory_Holidays_Tracker_To_Local(DataTable dtTrackerOperatoryEvent)
        ////{
        ////    return SynchTrackerDAL.Save_Opeatory_Holidays_Tracker_To_Local(dtTrackerOperatoryEvent);
        ////}

        ////#endregion

        //#region CreateAppointment

        ////public static DataTable GetTrackerPatientID_NameData()
        ////{
        ////    return SynchTrackerDAL.GetTrackerPatientID_NameData();
        ////}

        ////public static DataTable GetTrackerIdelProvider()
        ////{
        ////    return SynchTrackerDAL.GetTrackerIdelProvider();
        ////}

        ////public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate)
        ////{
        ////    return SynchTrackerDAL.GetBookOperatoryAppointmenetWiseDateTime(ApptDate);
        ////}
        //public static int Save_Patient_Local_To_Tracker(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, int tmpPatient_Gur_id)
        //{
        //    return SynchTrackerDAL.Save_Patient_Local_To_Tracker(LastName, FirstName, MiddleName, Mobile, Email, PriProv, DateFirstVisit, tmpPatient_Gur_id);
        //}
        //public static int Save_Appointment_Local_To_Tracker(string PatNum, int Pattern, string Op, string ProvNum, DateTime StartTime, DateTime EndTime, string DateTStamp, string AppointmentTypeNum, string PatientName)
        //{
        //    return SynchTrackerDAL.Save_Appointment_Local_To_Tracker(PatNum, Pattern, Op, ProvNum, StartTime, EndTime, DateTStamp, AppointmentTypeNum, PatientName);
        //}
        //public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id)
        //{
        //    return SynchTrackerDAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id, tmpAppt_Web_id);
        //}
        //public static bool Update_Status_EHR_Appointment_Live_To_TrackerEHR(DataTable dtLiveAppointment)
        //{
        //    return SynchTrackerDAL.Update_Status_EHR_Appointment_Live_To_TrackerEHR(dtLiveAppointment);
        //}

        //#endregion

        public static DataTable GetTrackerHolidayData()
        {
            return SynchTrackerDAL.GetTrackerHolidaysData();
        }

        public static bool Update_Status_EHR_Appointment_Live_To_TrackerEHR(DataTable dtLiveAppointment, string _filename_EHR_appointment = "", string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchTrackerDAL.Update_Status_EHR_Appointment_Live_To_TrackerEHR(dtLiveAppointment,_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment);
        }
        public static bool Update_Receive_SMS_Patient_EHR_Live_To_TrackerEHR(DataTable dtLiveAppointment,string locationid, string Loc_ID, string _filename_EHR_patientoptout = "", string _EHRLogdirectory_EHR_patientoptout = "")
        {
            return SynchTrackerDAL.Update_Receive_SMS_Patient_EHR_Live_To_TrackerEHR(dtLiveAppointment,locationid,Loc_ID, _filename_EHR_patientoptout, _EHRLogdirectory_EHR_patientoptout);
        }
        public static bool Save_Document_in_Tracker(string strPatientFormID = "")
        {
            return SynchTrackerDAL.Save_Document_in_Tracker(strPatientFormID);
        }      
        public static bool Save_Patient_Form_Local_To_Tracker(DataTable dtWebPatient_Form)
        {
            return SynchTrackerDAL.Save_Patient_Form_Local_To_Tracker(dtWebPatient_Form);
        }

        public static bool Save_TreatmentDocument_Form_Local_To_Tracker(string strTreatmentPlanID = "")
        {
            return SynchTrackerDAL.Save_TreatmentDocument_Form_Local_To_Tracker(strTreatmentPlanID);
        }

        public static bool Save_InsuranceCarrierDocument_Form_Local_To_Tracker(string strTreatmentPlanID = "")
        {
            return SynchTrackerDAL.Save_InsuranceCarrierDocument_Form_Local_To_Tracker(strTreatmentPlanID);
        }

        public static bool Save_InsuranceCarrier_Form_Local_To_Tracker(string strInsuranceCarrierID = "")
        {
            return SynchTrackerDAL.Save_InsuranceCarrierDocument_Form_Local_To_Tracker(strInsuranceCarrierID);
        }

        public static DataSet GetProviderCustomHours()
        {
            return SynchTrackerDAL.GetProviderCustomHours();
        }

        public static DataTable GetOperatoryCustomHours()
        {
            return SynchTrackerDAL.GetOperatoryCustomHours();
        }

        public static DataTable GetTrackerProviderOfficeHours()
        {
            return SynchTrackerDAL.GetTrackerProviderOfficeHours();
        }
        public static DataTable GetTrackerOperatoryOfficeHours()
        {
            return SynchTrackerDAL.GetTrackerOperatoryOfficeHours();
        }


        //public static string Save_PatientPaymentLog_LocalToTracker(DataTable dtWebPatientPayment, string DbString, string ServiceInstallationId)
        //{
        //   return SynchTrackerDAL.Save_PatientPaymentLog_LocalToTracker(dtWebPatientPayment, DbString, ServiceInstallationId);
        //}
        public static string Save_PatientSMSCallLog_LocalToTracker(DataTable dtWebPatientPayment, string DbString, string ServiceInstallationId)
        {
            return SynchTrackerDAL.Save_PatientSMSCallLog_LocalToTracker(dtWebPatientPayment, DbString, ServiceInstallationId);
        }

        public static bool SavePatientMedicationLocalToTracker(ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            return SynchTrackerDAL.SavePatientMedicationLocalToTracker(ref isRecordSaved, ref SavePatientEHRID, strPatientFormID);
        }

        public static bool DeletePatientMedicationLocalToTracker(ref bool isRecordDeleted, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            return SynchTrackerDAL.DeletePatientMedicationLocalToTracker(ref isRecordDeleted, ref DeletePatientEHRID, strPatientFormID);
        }

        public static DataTable GetTrackerPatientMedicationData(string Patinet_EHR_IDS)
        {
            return SynchTrackerDAL.GetTrackerPatientMedicationData(Patinet_EHR_IDS);
        }
        public static DataTable Save_NoteDataMoveToCorrespondInTracker(DataTable dtNote, string DbString )
        {
            return SynchTrackerDAL.Save_NoteDataMoveToCorrespondInTracker(dtNote, DbString );
        }

        #region Insurance
        public static DataTable GetTrackerInsuranceData()
        {
            return SynchTrackerDAL.GetTrackerInsuranceData();
        }

        public static bool Save_Insurance_Tracker_To_Local(DataTable dtTrackerInsurance, string Service_Install_Id)
        {
            return SynchTrackerDAL.Save_Insurance_Tracker_To_Local(dtTrackerInsurance, Service_Install_Id);
        }

        #endregion
    }
}
