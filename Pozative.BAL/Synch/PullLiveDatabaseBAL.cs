using Pozative.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pozative.BAL
{
    public class PullLiveDatabaseBAL
    {

        public static DataTable GetLiveDB_AppointmentDetail()
        {
            return PullLiveDatabaseDAL.GetLiveDB_AppointmentDetail();
        }

        public static string GetLiveRecord(string TableName, string LocationId)
        {
            return PullLiveDatabaseDAL.GetLiveRecord(TableName, LocationId);
        }

        public static string GetTreatmentDocFromWeb(string TableName, string TreatmentPlanId)
        {
            return PullLiveDatabaseDAL.GetTreatmentDocFromWeb(TableName,TreatmentPlanId);
        }

        //rooja         
        public static string UpdateInsuranceCarrierDocStatus(string TableName)
        {
            return PullLiveDatabaseDAL.UpdateStatusInsuranceCarrierDoc(TableName);
        }   
        public static string UpdateTreatmentDocStatus(string TableName)
        {
            return PullLiveDatabaseDAL.UpdateStatusTreatmentDoc(TableName);
        }

        public static bool Save_Appointment_Live_To_Local(DataTable dtLiveAppointment, string _filename_Appointment, string _EHRLogdirectory_Appointment)
        {

            return PullLiveDatabaseDAL.Save_Appointment_Live_To_Local(dtLiveAppointment, _filename_Appointment, _EHRLogdirectory_Appointment);
        }

        public static bool Save_Pull_EHRAppointment_WithOut_PatientID_Live_To_Local(DataTable dtLiveAppointment,string _filename_ehr_appointment_without_patientid, string _EHRLogdirectory_ehr_appointment_without_patientid)
        {

            return PullLiveDatabaseDAL.Save_Pull_EHRAppointment_WithOut_PatientID_Live_To_Local(dtLiveAppointment, _filename_ehr_appointment_without_patientid, _EHRLogdirectory_ehr_appointment_without_patientid);
        }

        public static bool Save_Provider_Live_To_Local(DataTable dtLiveProvider, string _filename_Provider = "", string _EHRLogdirectory_Provider = "")
        {
            return PullLiveDatabaseDAL.Save_Provider_Live_To_Local(dtLiveProvider,  _filename_Provider,  _EHRLogdirectory_Provider);
        }

        public static bool Save_Operatory_Live_To_Local(DataTable dtLiveOperatory, string Service_Install_Id, string _filename_Operatory = "", string EHRLogdirectory_EHR_Operatory = "")
        {
            return PullLiveDatabaseDAL.Save_Operatory_Live_To_Local(dtLiveOperatory, Service_Install_Id,  _filename_Operatory , EHRLogdirectory_EHR_Operatory);
        }

        public static bool Save_ApptType_Live_To_Local(DataTable dtLiveApptType ,string _filename_ApptType = "", string _EHRLogdirectory_ApptType = "")
        {
            return PullLiveDatabaseDAL.Save_ApptType_Live_To_Local(dtLiveApptType,  _filename_ApptType ,  _EHRLogdirectory_ApptType );
        }

        public static bool Save_Patient_Live_To_Local(DataTable dtLivePatient,string _filename_Patient="", string _EHRLogdirectory_Patient="")
        {
            return PullLiveDatabaseDAL.Save_Patient_Live_To_Local(dtLivePatient,  _filename_Patient ,  _EHRLogdirectory_Patient );
        }
               
        public static bool Save_PatientForm_Live_To_Local(DataTable dtLivePatientForm,bool is_PatientPortal = false,string _filename_EHR_PatientFormt="", string _EHRLogdirectory_EHR_PatientForm="")
        {
            return PullLiveDatabaseDAL.Save_PatientForm_Live_To_Local(dtLivePatientForm,is_PatientPortal, _filename_EHR_PatientFormt, _EHRLogdirectory_EHR_PatientForm);
        }

        public static bool Save_PatientFormDoc_Live_To_Local(DataTable dtLivePatientFormDoc, string _filename_EHR_Patient_Document="",string  _EHRLogdirectory_EHR_Patient_Document="")
        {
            return PullLiveDatabaseDAL.Save_PatientFormDoc_Live_To_Local(dtLivePatientFormDoc, _filename_EHR_Patient_Document, _EHRLogdirectory_EHR_Patient_Document);
        }
        public static bool Save_PatientFormDocAttachment_Live_To_Local(DataTable dtLivePatientFormDoc)
        {
            return PullLiveDatabaseDAL.Save_PatientFormDocAttachment_Live_To_Local(dtLivePatientFormDoc);
        }

        public static bool Update_PatientFormDoc_Live_To_Local(string Patient_form_webId, string Service_Install_Id, string _filename_EHR_Patient_Document="", string _EHRLogdirectory_EHR_Patient_Document="")
        {
            return PullLiveDatabaseDAL.Update_PatientFormDoc_Live_To_Local(Patient_form_webId, Service_Install_Id, _filename_EHR_Patient_Document, _EHRLogdirectory_EHR_Patient_Document);
        }
        public static bool Update_PatientFormDocAttachment_Live_To_Local(string Patient_form_webId, string Service_Install_Id)
        {
            return PullLiveDatabaseDAL.Update_PatientFormDocAttachment_Live_To_Local(Patient_form_webId, Service_Install_Id);
        }
        //public static bool Update_PatientDocNotFound_Live_To_Local(string Patient_form_webId)
        //{
        //    return PullLiveDatabaseDAL.Update_PatientDocNotFound_Live_To_Local(Patient_form_webId);
        //}
        public static bool Update_PatientFormDoc_Local_To_EHR(string Patient_form_webId, string PatientDoc_EHR_ID, string Service_Instsll_Id)
        {
            return PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(Patient_form_webId, PatientDoc_EHR_ID, Service_Instsll_Id);
        }

        public static bool Save_PatientFormMedicalHistory_Live_To_Local(DataTable dtLocalPatientFormMedicalHistory)
        {
            return PullLiveDatabaseDAL.Save_PatientFormMedicalHistory_Live_To_Local(dtLocalPatientFormMedicalHistory);
        }

        public static bool Save_PatientPaymentLog_To_Local(DataTable dtLivePatientPaymentLog)
        {
            return PullLiveDatabaseDAL.Save_PatientPaymentLog_To_Local(dtLivePatientPaymentLog);
        }

        public static bool Save_PatientSMSCallLog_To_Local(DataTable dtLivePatientSMSCallLog,string _filename_EHR_patient_sms_call= "", string _EHRLogdirectory_EHR_patient_sms_call = "")
        {
            return PullLiveDatabaseDAL.Save_PatientSMSCallLog_To_Local(dtLivePatientSMSCallLog, _filename_EHR_patient_sms_call, _EHRLogdirectory_EHR_patient_sms_call);
        }
        //public static bool Save_PatientFormDocAttachment_Live_To_Local(DataTable dtLivePatientFormDoc)
        //{
        //    return PullLiveDatabaseDAL.Save_PatientFormDocAttachment_Live_To_Local(dtLivePatientFormDoc);
        //}

        //public static bool Update_PatientFormDocAttachment_Live_To_Local(string Patient_form_webId, string Service_Install_Id)
        //{
        //    return PullLiveDatabaseDAL.Update_PatientFormDocAttachment_Live_To_Local(Patient_form_webId, Service_Install_Id);
        //}
    }
}
