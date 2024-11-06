using Pozative.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BAL
{
    public class PushLiveDatabaseBAL
    {
        public static string Push_Local_To_LiveDatabase(string JsonString, string TableName, string EHR_ID, string Web_ID, string Service_Install_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase(JsonString, TableName, EHR_ID, Web_ID, Service_Install_Id);
        }

        public static string Push_Local_To_LiveDatabase_UpdateclientEHRVersion( string Location_id, string updateclientehrversion)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_UpdateclientEHRVersion( Location_id,  updateclientehrversion);
        
        }

        public static string Push_Local_To_LiveDatabase_WithList(string JsonString, string TableName)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_WithList(JsonString, TableName, "", "1");
        }

        public static string Push_Location_EHR(string JsonString, string LocationID)
        {
            return PushLiveDatabaseDAL.Push_Location_EHR(JsonString, LocationID);
        }

        public static string Push_Location_EHRUPdateForVersion()
        {
            return PushLiveDatabaseDAL.Push_Location_EHRUPdateForVersion();
        }

        public static string UpdateLocNewVersionOnAdit_Server_App(string JsonString)
        {
            return PushLiveDatabaseDAL.UpdateLocNewVersionOnAdit_Server_App(JsonString);
        }

        public static string Push_Local_To_PozativeLiveDatabase(string JsonString, string Appointment_Id, string Clinic_Number, string Service_Install_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_PozativeLiveDatabase(JsonString, Appointment_Id, Clinic_Number, Service_Install_Id);
        }

        public static bool UpdateLocalTableWebID(string TableName, string EHR_ID, string Web_ID, string Clinic_Number, string Service_Install_Id)
        {
            return PushLiveDatabaseDAL.UpdateLocalTableWebID(TableName, EHR_ID, Web_ID, Clinic_Number, Service_Install_Id);
        }

        public static string Push_Local_To_LiveDatabase_ApptType(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id,string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_ApptType(JsonString, TableName, Clinic_Number, Service_Install_Id,Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_Speciality(string JsonString, string TableName, string Location_id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Speciality(JsonString, TableName, Location_id);
        }

        public static string Push_Local_To_LiveDatabase_OperatoryEvent(string JsonString, string TableName, string Service_Install_Id, string Location_id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_OperatoryEvent(JsonString, TableName, Service_Install_Id,Location_id);
        }

        public static string Push_Local_To_LiveDatabase_OperatoryDayOff(string JsonString, string TableName, string Service_Install_Id, string Location_id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_OperatoryDayOff(JsonString, TableName, Service_Install_Id,Location_id);
        }

        public static string Push_Local_To_LiveDatabase_Holiday(string JsonString, string TableName, string Service_Install_Id, string Location_id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Holiday(JsonString, TableName, Service_Install_Id,Location_id);
        }

        public static string Push_Local_To_LiveDatabase_ProviderOfficeHours(string JsonString, string TableName, string Service_Install_Id,string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_ProviderOfficeHours(JsonString, TableName, Service_Install_Id,Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_Operatory(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Operatory(JsonString, TableName, Service_Install_Id,Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_FolderList(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_FolderList(JsonString, TableName, Service_Install_Id, Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_Provider(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id,string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Provider(JsonString, TableName, Clinic_Number, Service_Install_Id, Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_User(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id, string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_User(JsonString, TableName, Clinic_Number, Service_Install_Id, Location_Id);
        }
        public static string Push_Local_To_LiveDatabase_ProviderHours(string JsonString, string TableName, string Service_Install_Id,string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_ProviderHours(JsonString, TableName, Service_Install_Id, Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_OperatoryHours(string JsonString, string TableName, string Service_Install_Id,string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_OperatoryHours(JsonString, TableName, Service_Install_Id,Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_OperatoryOfficeHours(string JsonString, string TableName, string Service_Install_Id,string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_OperatoryOfficeHours(JsonString, TableName, Service_Install_Id,Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_Patient(string JsonString, string TableName, string Service_Install_Id,string Location_id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Patient(JsonString, TableName, Service_Install_Id,Location_id);
        }

        public static string Push_Local_To_LiveDatabase_PatientStatus(string JsonString, string TableName, string Service_Install_Id, string Location_id, DataTable dtPatientStatus)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_PatientStatus(JsonString, TableName, Service_Install_Id, Location_id, dtPatientStatus);
        }

        public static string Push_Local_To_LiveDatabase_Disease(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Disease(JsonString, TableName, Clinic_Number, Service_Install_Id);
        }

        public static string Push_Local_To_LiveDatabase_Medication(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Medication(JsonString, TableName, Clinic_Number, Service_Install_Id);
        }

        public static string Push_Local_To_LiveDatabase_PatientDisease(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_PatientDisease(JsonString, TableName, Clinic_Number, Service_Install_Id);
        }
        public static string Push_Local_To_LiveDatabase_RecallType(string JsonString, string TableName,string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_RecallType(JsonString, TableName,Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_ApptStatus(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_ApptStatus(JsonString, TableName, Service_Install_Id, Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_ApptStatus_With_Type(string JsonString, string TableName, string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_ApptStatus_With_Type(JsonString, TableName, Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_StatusAppointmentlist(string JsonString, string TableName, string Location_Id, string _filename_EHR_appointment = "", string _EHRLogdirectory_EHR_appointment = "")
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_StatusAppointmentlist(JsonString, TableName, Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_Appointment(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id,string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Appointment(JsonString, TableName, Clinic_Number, Service_Install_Id, Location_Id);
        }

        public static string Push_Local_To_LiveDatabase_Is_Appt_DoubleBook(string JsonString, string TableName, string Service_Install_Id,string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Is_Appt_DoubleBook(JsonString, TableName, Service_Install_Id,Location_Id);
        }


        public static string Push_Local_To_LiveDatabase_MedicalHistory(string jsonString, string TableName, string Service_Install_Id,string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, TableName, Service_Install_Id,Location_Id);
        }
        public static string Push_Local_To_LiveDatabase_PatientProfileImage(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Disease(JsonString, TableName, Clinic_Number, Service_Install_Id);
        }
        public static string GetLive_Push_Record(string TableName)
        {
            return PushLiveDatabaseDAL.GetLive_Push_Record(TableName);
        }

        public static string Push_Local_To_LiveDatabase_PatientMedication(string JsonString, string TableName, string Location_Id, string Clinic_Number, string Service_Install_ID)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_PatientMedication(JsonString, TableName, Location_Id, Clinic_Number, Service_Install_ID);
        }
        public static string Push_Local_To_LiveDatabase_PatientBalance(string JsonString, string TableName, string Service_Install_Id, string Location_id, DataTable dtPatientBalance)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_PatientBalance(JsonString, TableName, Service_Install_Id, Location_id, dtPatientBalance);
        }
        public static string Push_Local_To_LiveDatabase_PozativeConfiguration(string JsonString, string TableName, string Location_id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_PozativeConfiguration(JsonString, TableName, Location_id);
        }

        public static string Push_Local_To_LiveDatabase_EventAcknowledgement(string JsonString, string TableName, string Location_id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_EventAcknowledgement(JsonString, TableName, Location_id);
        }

        public static string Push_Local_To_LiveDatabase_SaveUser_Results(string JsonString, string TableName, string Location_id, string Clinic_Number, string Service_Install_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_SaveUser_Results(JsonString, TableName, Location_id, Clinic_Number, Service_Install_Id);
        }

        public static string Push_Local_To_LiveDatabase_EHRAndSystemCreds_Results(string JsonString, string TableName, string Location_id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_EHRAndSystemCreds_Results(JsonString, TableName, Location_id);
        }
        public static string Push_Local_To_LiveDatabase_ZohoInstall_Results(string JsonString, string TableName, string Location_id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_ZohoInstall_Results(JsonString, TableName, Location_id);
        }

        public static string Push_Local_To_LiveDatabase_Insurance(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            return PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_Insurance(JsonString, TableName, Service_Install_Id, Location_Id);
        }
    }
}
