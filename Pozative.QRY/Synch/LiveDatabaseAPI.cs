using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pozative.UTL;

namespace Pozative.QRY
{
    public class LiveDatabaseAPI
    {
        #region Adit Api URL

        public static string GetAditLocationAndOrganization()
        {
            string strApiLocOrg = string.Empty;
            //strApiLocOrg = HostName_Adit + "webhooks/findLocationDetails?email=" + AdminUserId + "&password=" + AdminPassword + "";
            strApiLocOrg = Utility.HostName_Adit + "webhooks/findLocationDetails?created_by=" + Utility.User_ID + "";
            return strApiLocOrg;
        }

        public static string GetApiERHListWithWebId()
        {
            string strApiListEHR = string.Empty;
            strApiListEHR = Utility.HostName_Adit + "webhooks/ehrmaster?created_by=" + Utility.User_ID + "";
            return strApiListEHR;
        }

        public static string GetLocUpdateVersion()
        {
            string strApiListEHR = string.Empty;
            strApiListEHR = Utility.MultiRecordHostName + "v1/webhooks/get/ehrclienthistory?appointmentlocation_id=" + Utility.Location_ID + "";
            return strApiListEHR;
        }


        public static string GetAdminUserLogin()
        {
            string strAdminUserLogin = string.Empty;
            // strAdminUserLogin = HostName_Adit + "auth/login?email=" + AdminUserId + "&password=" + AdminPassword + "";
            strAdminUserLogin = Utility.HostName_Adit + "auth/login";
            return strAdminUserLogin;
        }

        public static string LiveRecord_Push_API(string TableName)
        {
            string strRestClientURL = string.Empty;
            switch (TableName.ToLower())
            {
                case "provider":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/provider";
                    break;
                case "speciality":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/provider/speciality";
                    break;
                case "operatory":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/operatory";
                    break;
                case "patient":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/patient";
                    break;
                case "type":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/appointmenttype";
                    break;
                case "appointment":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/appointment";
                    break;
                case "ehr":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/appointmentlocation";
                    break;
            }
            return strRestClientURL;
        }

        public static string LiveRecord_WithList_Push_API(string TableName)
        {
            string strRestClientURL = string.Empty;


            switch (TableName.ToLower())
            {
                case ("provider"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/provider";
                    break;
                case ("folderlist"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/ehrfolder";
                    break;
                case ("ehrsynctime"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/updateehrsynctime";                   
                    break;
                case ("providerhours"):
                    if (Utility.Application_ID == 3)
                    {
                        strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/providercustomhourdentrix/";
                    }
                    else
                    {
                        strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/providercustomhour/";
                    }
                    break;
                case ("speciality"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/provider/speciality";
                    break;
                case ("operatory"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/operatory";
                    break;
                case ("operatoryevent"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/blockappointment";
                    break;
                case ("operatorydayoff"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/operatorydayoffappointment";
                    break;
                case ("operatoryhours"):
                    if (Utility.Application_ID == 3)
                    {
                        strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/operatorycustomhourdentrix";
                    }
                    else
                    {
                        strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/operatorycustomhour";
                    }
                    break;
                case ("patient"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/patient";
                    break;
                case ("patientmultilocation"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/patient/multilocation";
                    break;
                case ("patientstatus"):
                    //strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/patient/status";
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/patient/status/latest";
                    break;
                case ("patientbalance"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/patient/updatebalance";
                    break;
                case ("disease"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/disease";
                    break;
                case ("patientdisease"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/patientdisease";
                    break;
                case ("recalltype_apptstatus"):
                    //strRestClientURL = MultiRecordHostName + "v1/webhooks/recalltype";
                    strRestClientURL = Utility.HostName_Adit + "webhooks/engage/appreminder/syncrecallaptypes";
                    break;
                case ("apptstatus_with_type"):
                    strRestClientURL = Utility.HostName_Adit + "webhooks/appointment/syncstatus";
                    break;
                case ("apptstatus"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/apptstatus";
                    break;
                case ("type"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/appointmenttype";
                    break;
                case ("appointment"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/appointment";
                    break;
                case ("appointmentmultilocation"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/appointment/multilocation";
                    break;
                case ("statusappointmentlist"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/updateconfirmappointment";
                    break;
                case ("ehr"):
                    strRestClientURL = Utility.HostName_Adit + "webhooks/appointmentlocation";
                    break;
                case ("holiday"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/holidayappointment";
                    break;
                case ("providerofficehours"):
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/providerbusinesshour";
                    break;
                case "ehrupdateversion":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/ehrclienthistory";
                    break;
                case "updateehrupdateversion":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/updatehistory";
                    break;
                case "updateclientehrversion":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/updateclientehrversion?appointmentlocation=Location_Id&client_application_version=" + Utility.EHR_VersionNumber.ToString();
                    break;
                case "operatoryofficehours":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/operatorybusinesshour";
                    break;
                case "is_appt_doublebook":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/appointment/sendmaildoublebook";
                    break;
                case "eaglesoftformmaster":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/eaglesoft/formaster";
                    break;
                case "eaglesoftsectionmaster":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/eaglesoft/sectionmaster";
                    break;
                case "eaglesoftalertmaster":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/eaglesoft/alertmaster";
                    break;
                case "eaglesoftsectionitemmaster":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/eaglesoft/sectionitemmaster";
                    break;
                case "eaglesoftsectionitemoptionmaster":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/eaglesoft/sectionitemoptionmaster";
                    break;
                case "dentrix_form":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/dentrix/formaster";
                    break;
                case "dentrix_formquestion":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/dentrix/formquestion";
                    break;
                case "abeldent_form":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/abeldent/formaster";
                    break;
                case "abeldent_formquestion":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/abeldent/formquestion";
                    break;
                case "od_sheetdef":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/opendental/sheet";
                    break;
                case "od_sheetfielddef":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/opendental/sheetfield";
                    break;
                case "practiceweb_sheetdef":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/practiceweb/sheet";
                    break;
                case "practiceweb_sheetfielddef":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/practiceweb/sheetfield";
                    break;
                case "cd_formmaster":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/cleardent/formmaster";
                    break;
                case "cd_questionmaster":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/cleardent/questionmaster";
                    break;
                case "easydental_question":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/easydental/form";
                    break;
                case "easydental_form":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/easydental/formmaster";
                    break;
                case "patientprofileimage":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/image/patient/profile";
                    break;
                case "multisync":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/image/patient/profile/multisync";
                    break;
                case "multisyncupdate":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/image/patient/profile/multisync/update";
                    break;
                case "addimagename":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/image/patient/profile/addimagename";
                    break;
                case "updateimagename":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/image/patient/profile/updateimagename";
                    break;
                case "patientprofileimageremove":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/image/patient/profile/remove";
                    break;
                case "appointmentdoublebook":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/appointment/sendmaildoublebook";
                    break;
                case "users":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/ehruser";
                    break;
                case "createorglocdir":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/image/patient/profile/createorglocdir";
                    break;
                case "medication":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/medication";
                    break;
                case "patientmedication":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/patientmedication";
                    break;
                case "pozativeconfiguration":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/ehrconfiguration";
                    break;
                case "eventacknowledgement":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/nchan-update";
                    break;
                case "EHRAndSystemCredsResult":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/zoho-assist-details";
                    break;
                case "ehrinfovalidate":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/update-zoho-assist-validation";
                    break;
                case "systemusers":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/zoho-assist-create-serveruser";
                    break;
                case "zohoinstallvalidate":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/update-zoho-assist-installed";
                    break;
                case "insurance":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/insurance";
                    break;
            }
            return strRestClientURL;
        }

        public static string LiveRecord_Pull_API(string TableName, string LocationId)
        {
            string strRestClientURL = string.Empty;

            switch (TableName.ToLower())
            {
                case "insurancecarrier_document":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/rcm/get-patient-pdf?locationId=" + LocationId + "";
                    break;               
                case "change_insurancecarrier_doc_status":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/rcm/update-patient-pdf";
                    break;
                case "change_treatmentplan_doc_status":
                    strRestClientURL = Utility.HostName_Adit + "patient-form/treatment-plan/update-ehr-import-status";
                    break;
                case "treatmentplan_pdf":
                    strRestClientURL = Utility.HostName_Adit + "patient-form/treatment-plan/export/" + LocationId;
                    break;
                case "treatmentplan_document":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/patient-form/treatment-plan/get-ehr-sync";
                    break;
                case "provider":
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/provider?location=" + Utility.Location_ID + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    strRestClientURL = Utility.HostName_Adit + "webhooks/provider?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    break;
                case "operatory":
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/operatory?location=" + Utility.Location_ID + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    strRestClientURL = Utility.HostName_Adit + "webhooks/operatory?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    break;
                case "patient":
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/patient?location=" + Utility.Location_ID + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    strRestClientURL = Utility.HostName_Adit + "webhooks/patient?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    break;
                case "patientform":
                    //strRestClientURL = Utility.HostName_Adit + "patientform/getapprovelist?org=" + Utility.Organization_ID + "&loc=" + Utility.Loc_ID + "";
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/patientform/getapprovelist?org=" + Utility.Organization_ID + "&loc=" + LocationId + "";
                    break;
                case "patientformdocattachment_list":
                    strRestClientURL = Utility.HostName_Adit + "patient-form/get-list-of-uploaded-documents";
                    break;
                case "patientformdocattachment":
                    strRestClientURL = Utility.HostName_Adit + "patient-form/get-base64-data-for-file";
                    break;
                //case "patientformdocattachment_list":
                //    strRestClientURL = Utility.HostName_Adit + "patient-form/get-list-of-uploaded-documents";
                //    break;
                //case "patientformdocattachment":             
                //    strRestClientURL = Utility.HostName_Adit + "patient-form/get-base64-data-for-file";
                //    break;
                case "patientportal":
                    //strRestClientURL = Utility.HostName_Adit + "patientform/getapprovelist?org=" + Utility.Organization_ID + "&loc=" + Utility.Loc_ID + "";
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/patientpreference/syncpatientdata?org=" + Utility.Organization_ID + "&loc=" + LocationId + "";
                    break;
                case "patientportalupdatedinehr":
                    //strRestClientURL = Utility.HostName_Adit + "patientform/getapprovelist?org=" + Utility.Organization_ID + "&loc=" + Utility.Loc_ID + "";
                    strRestClientURL = Utility.HostName_Adit + "webhooks/patientpreference/updatepatientfromehr";
                    break;
                case "patient_document":
                    //strRestClientURL = Utility.HostName_Adit + "patientform/pdfExport";
                    strRestClientURL = Utility.HostName_Adit + "patient-form/pdf-export-ehr";
                    break;
                case "patientformupdaterecordid":
                    //strRestClientURL = Utility.HostName_Adit + "patientform/updateEHRImportStatus";
                    strRestClientURL = Utility.HostName_Adit + "patient-form/update-ehr-import-status";
                    break;
                case "type":
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/appointmenttype?location=" + Utility.Location_ID + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    strRestClientURL = Utility.HostName_Adit + "webhooks/appointmenttype?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    break;
                case "appointment":
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/appointment?location=" + Utility.Location_ID + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/appointment?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/appointment?location=" + Utility.Location_ID + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/pull/appointment?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    break;
                case "ehr_appointment":
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/appointment/ehr?location=" + Utility.Location_ID + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/appointment/ehr?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/pull/appointment/ehr?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    break;
                case "ehr_appointment_without_patientid":
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/appointment/patientnotexist?location=" + Utility.Location_ID + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/appointment/patientnotexist?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/pull/appointment/patientnotexist?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    break;
                case "installpozative":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/ehrstatus?location=" + Utility.Location_ID + "";
                    break;
                case "eaglesoftsectionmaster":
                    //strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/eaglesoft/get/MedicalHistoryAPI?appointmentlocation=" + Utility.Location_ID;
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/eaglesoft/get/MedicalHistoryAPI?appointmentlocation=" + LocationId;
                    break;
                case "geteaglesoftehrforms":
                    //strRestClientURL = Utility.HostName_Adit + "patientform/getEagleSoftEHRForms";
                    strRestClientURL = Utility.HostName_Adit + "patient-form/get-eaglesoft-ehr-forms";
                    break;
                case "patientpaymentlog":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/adit-pay/ehr-sync/get";
                    break;
                case "financialpatientpayment":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/financing/ehr-sync/get";
                    break;
                case "patientpaymentlogstatus":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/adit-pay/ehr-sync/status";
                    break;
                case "patientsmscalllog":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/getlistofsmsforehr";
                    break;
                case "patientsmscalllogstatus":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/updatestatusofsmsfromehr";
                    break;
                case "patientfollowuplogstatus":
                    strRestClientURL = Utility.HostName_Adit + "practiceanalytics/followup/patientNote/sync/confirm";
                    break;
                case "patientnotexistcount":
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/appointment/count/patientnotexist?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/pull/appointment/count/patientnotexist?location=" + LocationId + "&exclude=organization&created_by=" + Utility.User_ID + "";
                    break;
                case "patientfollowup":
                    strRestClientURL = Utility.HostName_Adit + "practiceanalytics/followup/patientNote/sync?locationId=" + LocationId + "&organizationId=" + Utility.Organization_ID + "&limit=10&skip=0";
                    break;
                case "deleteappointmentfromweb":
                    //strRestClientURL = Utility.HostName_Adit + "webhooks/appointment/ehrapptlist/deleteprocess?location=" + LocationId + "";
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/pull/appointment/ehrapptlist/deleteprocess?location=" + LocationId + "";
                    break;
                case "appointmentidsfordelete":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/appointment/deleteprocess";
                    break;
                case "patientoptout":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/engage/patientoptout/sync?locationId=" + LocationId + "&organizationId=" + Utility.Organization_ID;
                    break;
                case "patientoptout_confirm":
                    strRestClientURL = Utility.HostName_Adit + "webhooks/engage/patientoptout/sync/confirm";//?locationId=" + LocationId + "&organizationId=" + Utility.Organization_ID;
                    break;
                case "pocustomhour": //provider and opetaory custom hour  
                    strRestClientURL = Utility.HostName_Adit + "webhooks/sync/customhour?location=" + LocationId;
                    break;
                case "entryuserid": //userid for ehr
                    strRestClientURL = Utility.MultiRecordHostName + "v1/pull/webhooks/get/ehruser?location=" + LocationId;
                    break;
                case "getehrconfiguration": 
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/getehrconfiguration";
                    break;
                case "ehrinfo":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/get/ehrinfo?location=";
                    break;
                case "ehrinfovalidate":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/update-zoho-assist-validation";
                    break;
                case "zohoinstall":
                    strRestClientURL = Utility.MultiRecordHostName + "v1/webhooks/zoho-assist-details?locationId=" + LocationId;
                    break;
                
            }

            return strRestClientURL;
        }

        public static string UpdateApplicationVersionOnLiveDatabase()
        {
            string strApiUpdateVersionNo = string.Empty;
            strApiUpdateVersionNo = Utility.HostName_Adit + "webhooks/desktopversion";
            return strApiUpdateVersionNo;
        }

        public static string CheckLocationTimeZoneWithSystemTimeZone()
        {
            string strLocationTimeZone = string.Empty;
            strLocationTimeZone = Utility.HostName_Adit + "webhooks/ehrtimezone";
            return strLocationTimeZone;
        }

        public static string AditLocationSyncEnable(string Location_ID, string User_ID)
        {
            //string strApiAditLocationSyncEnable = string.Empty;
            //strApiAditLocationSyncEnable = Utility.HostName_Adit + "webhooks/ehrrecordsync?location=" + Location_ID + "&created_by=" + User_ID + ""; //Utility.Location_ID,Utility.User_ID
            //return strApiAditLocationSyncEnable;

            string strApiAditLocationSyncEnable = string.Empty;
            System.Guid guid = System.Guid.NewGuid();
            string Uinq_Id = Convert.ToString(guid); // add rand id suggestion by dhaval
            strApiAditLocationSyncEnable = Utility.MultiRecordHostName + "v1/webhooks/ehrrecordsync?location=" + Location_ID + "&created_by=" + User_ID + "&rand=" + Uinq_Id + ""; //Utility.Location_ID,Utility.User_ID
            return strApiAditLocationSyncEnable;

        }

        public static string AditPaymentSMSCallStatusUpdate(string Location_ID, string User_ID)
        {
            string strApiAditPaymentSMSCallStatusUpdate = string.Empty;
            strApiAditPaymentSMSCallStatusUpdate = Utility.HostName_Adit + "webhooks/updatelogdate"; //Utility.Location_ID,Utility.User_ID
            return strApiAditPaymentSMSCallStatusUpdate;
        }

        #endregion

        #region Pozative Api URL

        public static string GetPozativeLocation()
        {
            string strApiLoc = string.Empty;
            strApiLoc = Utility.HostName_Pozative + "api/ehruser";
            return strApiLoc;
        }

        public static string LiveRecord_Push_PozativeAPI()
        {
            string strRestClientURL = string.Empty;
            strRestClientURL = Utility.HostName_Pozative + "api/appointment";
            return strRestClientURL;
        }

        public static string UpdatePozativeLocationMachineId(string LocationId, string MachineId)
        {
            string strLoc = string.Empty;
            strLoc = Utility.HostName_Pozative + "api/ehrmachine?locationid=" + LocationId + "&machineid=" + MachineId + "";
            return strLoc;
        }

        public static string UpdateWebTimeZone()
        {
            string strLoc = string.Empty;
            strLoc = Utility.HostName_Adit + "webhooks/timezone";
            return strLoc;
        }
        public static string SaveEHRLogs()
        {
            string strSave = string.Empty;
            strSave = Utility.MultiRecordHostName + "v1/webhooks/saveehrlogs";
            return strSave;
        }
        public static string GetAutoPlayAudioText()
        {
            string strAudioText = string.Empty;
            strAudioText = Utility.MultiRecordHostName + "v1/webhooks/welcomeaudiomsg";
            return strAudioText;
        }
        public static string SendEmailEHR()
        {
            string strEmail = string.Empty;
            strEmail = Utility.HostName_Adit + "appointmentlocation/sendemailehr";
            return strEmail;
        }
        public static string IsValidOTP()
        {
            string strOTP = string.Empty;
            strOTP = Utility.HostName_Adit + "auth/verifyusersyncappuser";
            return strOTP;
        }
        public static string GetMasterSync()
        {
            string strGetMasterSync = string.Empty;
            strGetMasterSync = Utility.MultiRecordHostName + "v1/webhooks/getmastersync";
            return strGetMasterSync;
        }
        #endregion

    }
}
