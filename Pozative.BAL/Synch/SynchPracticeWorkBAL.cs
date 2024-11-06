using System;
using Pozative.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pozative.BAL
{
    public class SynchPracticeWorkBAL
    {

        public static bool Save_PracticeWork_To_Local(DataTable dtSoftDentDataToSave, string tablename, string ignoreColumnsName, string primaryKeyColumnsName)
        {
            try
            {
                return SynchPracticeWorkDAL.Save_PracticeWork_To_Local(dtSoftDentDataToSave, tablename, ignoreColumnsName, primaryKeyColumnsName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Save_Appointment_PracticeWork_To_Local(DataTable dtPracticeWorkAppointment)
        {
            return SynchPracticeWorkDAL.Save_Appointment_PracticeWork_To_Local(dtPracticeWorkAppointment);
        }
        
        public static Int64 Save_Patient_Local_To_PracticeWork(string LastName, string FirstName, string MiddleName, string MobileNo, string Email, string ApptProv, string AppointmentDateTime, Int64 Patient_Gur_id, int OperatoryId, string Birth_Date)
        {
            try
            {
                return SynchPracticeWorkDAL.Save_Patient_Local_To_PracticeWork(LastName, FirstName, MiddleName, MobileNo, Email, ApptProv, AppointmentDateTime, Patient_Gur_id, OperatoryId, Birth_Date);
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public static Int64 Save_Appointment_Local_To_PracticeWork(string FirstNameLastName, DateTime AppointmentStartTime, DateTime AppointmentEndTime, string PatNum, string OperatoryId,
            string classification, string ApptTypeId, DateTime AppointedDateTime, string ProvNum, string AppointmentConfirmationStatus, bool allday_event, bool sooner_if_possible, bool privateAppointment, bool auto_confirm_sent)
        {
            return SynchPracticeWorkDAL.Save_Appointment_Local_To_PracticeWork(FirstNameLastName, AppointmentStartTime, AppointmentEndTime, PatNum, OperatoryId, classification, ApptTypeId, AppointedDateTime, ProvNum, AppointmentConfirmationStatus, allday_event, sooner_if_possible, privateAppointment, auto_confirm_sent);
        }

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string AppointmentEHRId, string AppointmentWebId, string Service_Install_Id)
        {
            return SynchEaglesoftBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(AppointmentEHRId, AppointmentWebId, Service_Install_Id);
        }


        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime datetime)
        {
            return SynchPracticeWorkDAL.GetBookOperatoryAppointmenetWiseDateTime(datetime);
        }

        public static DataTable GetPracticeWorkAppointmentData(string strApptID = "")
        {
            return SynchPracticeWorkDAL.GetPracticeWorkAppointmentData(strApptID);
        }

        public static DataTable GetPracticeWorkAppointment_Procedures_Data(string strApptID = "")
        {
            return SynchPracticeWorkDAL.GetPracticeWorkAppointment_Procedures_Data(strApptID);
        }

  

        public static DataTable GetPracticeWorkAppointmentIds()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkAppointmentIds();
        }

        public static DataTable GetPracticeWorkOperatoryEventData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkOperatoryEventData();
        }

        public static DataTable GetPracticeWorkProviderData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkProviderData();
        }

        public static DataTable GetPracticeWorkDefaultProviderData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkDefaultProviderData();
        }
       
        public static DataTable GetPracticeWorkOperatoryData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkOperatoryData();
        }

        public static DataTable GetPracticeWorkSpecialityData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkSpecialityData();
        }

        public static DataTable GetPracticeWorkApptTypeData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkApptTypeData();
        }

        public static DataTable GetPracticeWorkPatientData( string strPatID = "")
        {
            return SynchPracticeWorkDAL.GetPracticeWorkPatientData(strPatID);
        }
        public static DataTable GetPracticeWorkNewPatientData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkNewPatientData();
        }
        public static bool Save_Patient_PracticeWork_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                return SynchPracticeWorkDAL.Save_Patient_PracticeWork_To_Local_New(dtOpenDentalDataToSave, Clinic_Number, Service_Install_Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool GetPracticeWorkPatientCellPhoneStatusData(bool IsCellPhoneAvailable)
        {
            return SynchPracticeWorkDAL.GetPracticeWorkPatientCellPhoneStatusData(IsCellPhoneAvailable);
        }
        public static DataTable GetPracticeWorkAppointmentsPatientData(string strPatID = "")
        {
            return SynchPracticeWorkDAL.GetPracticeWorkAppointmentsPatientData(strPatID);
        }


        public static DataTable GetPracticeWorkPatientListData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkPatientListData();
        }

        public static DataTable GetPracticeWorkRecallTypeData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkRecallTypeData();
        }

        public static DataTable GetPracticeWorkApptStatusData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkApptStatusData();
        }

      
        public static DataTable GetPracticeWorkHolidayData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkHolidaysData();
        }

        public static bool Update_Status_EHR_Appointment_Live_To_PracticeWorkEHR(DataTable dtLiveAppointment,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            return SynchPracticeWorkDAL.Update_Status_EHR_Appointment_Live_To_PracticeWorkEHR(dtLiveAppointment,_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
        }

        public static bool Save_Patient_Form_Local_To_PracticeWork(DataTable dtWebPatient_Form, string Service_Install_Id)
        {
            return SynchPracticeWorkDAL.Save_Patient_Form_Local_To_PracticeWork(dtWebPatient_Form, Service_Install_Id);
        }

        public static DataTable GetProviderCustomHours()
        {
            return SynchPracticeWorkDAL.GetProviderCustomHours();
        }

        public static DataTable GetOperatoryCustomHours()
        {
            return SynchPracticeWorkDAL.GetOperatoryCustomHours();
        }

        public static DataTable GetPracticeWorkProviderOfficeHours()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkProviderOfficeHours();
        }
        public static DataTable GetPracticeWorkOperatoryOfficeHours()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkOperatoryOfficeHours();
        }


        public static DataTable GetPracticeWorkOperatoryChairData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkOperatoryChairData();
        }

        public static bool Save_OperatoryDayOff_PracticeWork_To_Local(DataTable dtPracticeWorkOperatoryChair)
        {
            return SynchPracticeWorkDAL.Save_OperatoryDayOff_PracticeWork_To_Local(dtPracticeWorkOperatoryChair);
        }

        public static DataTable GetPracticeWorkDiseaseData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkDiseaseData();
        }

        public static DataTable GetPracticeWorkMedicationData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkMedicationData();
        }

        public static DataTable GetPracticeWorkPatientMedicationData(string Patient_EHR_IDS)
        {
            return SynchPracticeWorkDAL.GetPracticeWorkPatientMedicationData(Patient_EHR_IDS);
        }

        public static DataTable GetPracticeWorkPatientDiseaseData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkPatientDiseaseData();
        }
        public static bool SaveAllergiesToPracticeWork(string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchPracticeWorkDAL.SaveAllergiesToPracticeWork(Service_Install_Id, strPatientFormID);
        }
        public static bool DeleteAllergiesToPracticeWork(string Service_Install_Id, string strPatientFormID = "")
        {
            return SynchPracticeWorkDAL.DeleteAllergiesToPracticeWork(Service_Install_Id, strPatientFormID);
        }
        public static DataTable GetPracticeWorkPatientStatusData(string clinicNumber, string dbString, string strPatID = "")
        {
            return SynchPracticeWorkDAL.GetPracticeWorkPatientStatusData(clinicNumber, dbString, strPatID);
        }

        public static bool SaveMedicationToPracticeWork(string Service_Install_Id, ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            return SynchPracticeWorkDAL.SaveMedicationToPracticeWork(Service_Install_Id, ref isRecordSaved, ref SavePatientEHRID, strPatientFormID);
        }

        public static bool DeleteMedicationToPracticeWork(string Service_Install_Id, ref bool isRecordDeleted, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            return SynchPracticeWorkDAL.DeleteMedicationToPracticeWork(Service_Install_Id, ref isRecordDeleted, ref DeletePatientEHRID, strPatientFormID);
        }

        #region Insurance
        public static DataTable GetPracticeWorkInsuranceData()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkInsuranceData();
        }

        public static bool Save_Insurance_PracticeWork_To_Local(DataTable dtPracticeWorkInsurance, string Service_Install_Id)
        {
            return SynchPracticeWorkDAL.Save_Insurance_PrackticeWork_To_Local(dtPracticeWorkInsurance, Service_Install_Id);
        }

        #endregion
    }
}
