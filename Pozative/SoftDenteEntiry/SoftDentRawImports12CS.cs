using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    internal static class SoftDentRawImports12CS
    {
        [DllImport("SDInterop_12CS.Dll", EntryPoint = "InitializeDB")]
        public static extern int SDInteropInitializeDB(
          string path,
          bool loggingOn,
          string faircomServer);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "CloseFiles")]
        public static extern void SDInteropCloseFiles();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SetLogging")]
        public static extern void SDInteropSetLogging(bool loggingOn);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetAppointments")]
        public static extern IntPtr SDInteropGetAppointmentsForDay(
          string startDate,
          string startTime,
          string endDate,
          string endTime,
          string bookIds);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetAppointment")]
        public static extern IntPtr SDInteropGetAppointment(string PMSRecordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "ConfirmAppointment")]
        public static extern void SDInteropConfirmAppointment(
          string PMSRecordID,
          string Method,
          string Date,
          string Time,
          string Status,
          string Reason);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetOffice")]
        public static extern IntPtr SDInteropGetOffice(string dateTime);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SavePatient")]
        public static extern void SDInteropSavePatient(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveAppointment")]
        public static extern void SDInteropSaveAppointment(string xml);


        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetPatient")]
        public static extern IntPtr SDInteropGetPatient(string recordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "CreateRestorativeChartImage")]
        public static extern IntPtr SDInteropCreateRestorativeChartImage(
          string PMSRecordID,
          int view,
          int mode);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "CreatePerioChartImage")]
        public static extern IntPtr SDInteropCreatePerioChartImage(
          string PMSRecordID,
          int view,
          int mode);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetPatientDentalInsurancePlan")]
        public static extern IntPtr SDInteropGetPatientDentalInsurancePlan(
          string PMSRecordID,
          bool primary);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetPatientDentalInsurance")]
        public static extern IntPtr SDInteropGetPatientDentalInsurance(
          string PMSRecordID,
          bool primary);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetPatientGuarantors")]
        public static extern IntPtr SDInteropGetPatientGuarantors(string PMSRecordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetImages")]
        public static extern IntPtr SDInteropGetImages(string PMSRecordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SavePatientInsurance")]
        public static extern int SDInteropSaveInsurance(
          string patientID,
          int nGuarIndex,
          string personXml,
          string planXml,
          string companyXml,
          bool primary);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetPatientPreferredPharmacy")]
        public static extern IntPtr SDInteropGetPatientPreferredPharmacy(string patientID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetPatientPortrait")]
        public static extern IntPtr SDInteropGetPatientPortrait(string patientID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "PatientHasOpenLabCases")]
        public static extern bool SDInteropPatientHasOpenLabCases(string patientID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetMedicalHistoryConditions")]
        public static extern IntPtr SDInteropGetMedicalHistoryConditions();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetMedicalHistoryAllergies")]
        public static extern IntPtr SDInteropGetMedicalHistoryAllergies();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetMostRecentMedicalHistory")]
        public static extern IntPtr SDInteropGetMostRecentMedicalHistory(string patientID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveMedicalHistory")]
        public static extern void SDInteropSaveMedicalHistory(string patientID, string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetInsuranceCompany")]
        public static extern IntPtr SDInteropGetInsuranceCompany(string PMSRecordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetInsuranceCompanyList")]
        public static extern IntPtr SDInteropGetInsuranceCompanyList();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveInsuranceCompany")]
        public static extern void SDInteropSaveInsuranceCompany(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveNewInsuranceCompany")]
        public static extern IntPtr SDInteropSaveNewInsuranceCompany(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetInsurancePlan")]
        public static extern IntPtr SDInteropGetInsurancePlan(string PMSRecordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetInsurancePlanList")]
        public static extern IntPtr SDInteropGetInsurancePlanList();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetEmployer")]
        public static extern IntPtr SDInteropGetEmployer(string PMSRecordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetEmployerList")]
        public static extern IntPtr SDInteropGetEmployerList();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveEmployer")]
        public static extern void SDInteropSaveEmployer(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveNewEmployer")]
        public static extern IntPtr SDInteropSaveNewEmployer(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetPharmacy")]
        public static extern IntPtr SDInteropGetPharmacy(string PMSRecordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetPharmacyList")]
        public static extern IntPtr SDInteropGetPharmacyList();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SavePharmacy")]
        public static extern void SDInteropSavePharmacy(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveNewPharmacy")]
        public static extern IntPtr SDInteropSaveNewPharmacy(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetDrugList")]
        public static extern IntPtr SDInteropGetDrugList();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetDentist")]
        public static extern IntPtr SDInteropGetDenist(string PMSRecordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetDentistList")]
        public static extern IntPtr SDInteropGetDentistList();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveDentist")]
        public static extern void SDInteropSaveDentist(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveNewDentist")]
        public static extern IntPtr SDInteropSaveNewDentist(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetReferringDoctor")]
        public static extern IntPtr SDInteropGetReferringDoctor(string PMSRecordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetReferringDoctorList")]
        public static extern IntPtr SDInteropGetReferringDoctorList();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveReferringDoctor")]
        public static extern void SDInteropSaveReferringDoctor(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "SaveNewReferringDoctor")]
        public static extern IntPtr SDInteropSaveNewReferringDoctor(string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetADACodeList")]
        public static extern IntPtr SDInteropGetADACodeList();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "ArchiveEForm")]
        public static extern int SDInteropArchiveEForm(
          string patientID,
          string eformFilePath,
          string archiveDate,
          string archiveTime,
          string description);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetImage")]
        public static extern IntPtr SDInteropGetImage(string imageRecordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetPatientList")]
        public static extern IntPtr SDInteropPearlGetPatientList();

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetSpecialty")]
        public static extern IntPtr GetSpecialty();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetInitialPatientList")]
        public static extern IntPtr SDInteropPearlGetInitialPatientList();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetFirstPatientCount")]
        public static extern IntPtr SDInteropPearlGetFirstPatientCount(
          int count,
          string deviceKey);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetNextPatientCount")]
        public static extern IntPtr SDInteropPearlGetNextPatientCount(int count, string deviceKey);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlQueryPatientList")]
        public static extern IntPtr SDInteropPearlQueryPatientList(string query);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetPatient")]
        public static extern IntPtr SDInteropPearlGetPatient(string recordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetPatientDetail")]
        public static extern IntPtr SDInteropPearlGetPatientDetail(string recordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetPatientClinicalProfile")]
        public static extern IntPtr SDInteropPearlGetPatientClinicalProfile(string recordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetPatientAlerts")]
        public static extern IntPtr SDInteropPearlGetPatientAlerts(string recordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetPatientMedication")]
        public static extern IntPtr SDInteropPearlGetPatientMedication(string recordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetMoreAppointments")]
        public static extern IntPtr SDInteropPearlGetMoreAppointments(string recordID, int count);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetFirstPatientClinicalImages")]
        public static extern IntPtr SDInteropPearlGetFirstPatientClinicalImages(
          string recordID,
          int count,
          string deviceKey);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetNextPatientClinicalImages")]
        public static extern IntPtr SDInteropPearlGetNextPatientClinicalImages(
          string recordID,
          int count,
          string deviceKey);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlClearDeviceKey")]
        public static extern IntPtr SDInteropPearlClearDeviceKey();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetConsultingProviderList")]
        public static extern IntPtr SDInteropPearlGetConsultingProviderList();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetFirstConsultingProviderCount")]
        public static extern IntPtr SDInteropPearlGetFirstConsultingProviderCount(
          int count,
          string deviceKey);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetNextConsultingProviderCount")]
        public static extern IntPtr SDInteropPearlGetNextConsultingProviderCount(
          int count,
          string deviceKey);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlQueryConsultingProviderList")]
        public static extern IntPtr SDInteropPearlQueryConsultingProviderList(string query);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetConsultingProvider")]
        public static extern IntPtr SDInteropPearlGetConsultingProvider(string recordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetConsultingProviderProfile")]
        public static extern IntPtr SDInteropPearlGetConsultingProviderProfile(string recordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetConsultingProviderDetails")]
        public static extern IntPtr SDInteropPearlGetConsultingProviderDetails(string recordID);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetFinancialData")]
        public static extern IntPtr SDInteropPearlGetFinancialData(
          string providerId,
          int timePeriod);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlSaveNewPrescription")]
        public static extern int SDInteropPearlSaveNewPrescription(
          string patientID,
          string userID,
          string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlSavePatientCallDetails")]
        public static extern int SDInteropPearlSavePatientCallDetails(string userID, string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetBlockedSlots")]
        public static extern IntPtr SDInteropPearlGetBlockedSlots(
          string startDate,
          string startTime,
          string endDate,
          string endTime);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetAppointments")]
        public static extern IntPtr SDInteropPearlGetAppointments(
          string providerID,
          string startDate,
          string startTime,
          string endDate,
          string endTime);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlGetCallList")]
        public static extern IntPtr SDInteropPearlGetCallList(string providerID, bool refresh);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlSaveConsultingProviderCallDetails")]
        public static extern int SDInteropSaveConsultingProviderCallDetails(string userID, string xml);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "PearlLoginUser")]
        public static extern IntPtr SDInteropPearlLoginUser(string userName, string password);

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetEServiceFlags")]
        public static extern IntPtr SDInteropGetEServiceFlags();

        [DllImport("SDInterop_12CS.Dll", EntryPoint = "GetMissedApptsRevenueAnalysis")]
        public static extern IntPtr SDInteropGetMissedApptsRevenueAnalysis(
          string startDate,
          string endDate);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetResponsiblePartyForPatient")]
        public static extern IntPtr SDInteropGetResponsiblePartyForPatient(string patientID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetResponsibleParty")]
        public static extern IntPtr SDInteropGetResponsibleParty(string acctID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetPatientEmployee")]
        public static extern IntPtr SDInteropGetPatientEmployee(string patientID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetResponsiblePartyEmployee")]
        public static extern IntPtr SDInteropGetResponsiblePartyEmployee(string patientID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "SaveEmployerForPatient")]
        public static extern void SDInteropSaveEmployerForPatient(
          string patientID,
          string status,
          string employer);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "SaveResponsiblePartyForPatient")]
        public static extern void SDInteropSaveResponsiblePartyForPatient(
          string patientID,
          string responsibleParty);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "PutImage")]
        public static extern int SDInteropPutImage(
          string patientID,
          int imageType,
          int acquisitionRegion,
          string imageFileName,
          string imageFilePath,
          string toothAssociations,
          string acquisitionDate,
          string acquisitionTime);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetOperatories")]
        public static extern IntPtr SDInteropGetOperatories();

        [DllImport("SDInterop_12CS.dll", EntryPoint = "PearlGetAppointmentsByOperatory")]
        public static extern IntPtr SDInteropPearlGetAppointmentsByOperatory(
          string operatoryID,
          string startDate,
          string startTime,
          string endDate,
          string endTime);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "PearlMakeAppointment")]
        public static extern IntPtr SDInteropPearlMakeAppointment(
          string patientID,
          string providerID,
          string operatoryID,
          string date,
          string time,
          int duration,
          string note);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "PearlMakeBlockedSlot")]
        public static extern int SDInteropPearlMakeBlockedSlot(
          string userID,
          string operatoryID,
          string providerID,
          string date,
          string time,
          int duration,
          string note);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "PearlGetNextCallListCount")]
        public static extern IntPtr SDInteropPearlGetNextCallListCount(
          string providerID,
          int count,
          string deviceKey);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "PearlGetFirstCallListCount")]
        public static extern IntPtr SDInteropPearlGetFirstCallListCount(
          string providerID,
          int count,
          string deviceKey,
          bool refresh);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetPatientPerson")]
        public static extern IntPtr SDInteropGetPatientPerson(string patientID, int index);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "SavePatientPerson")]
        public static extern void SDInteropSavePatientPerson(
          string patientID,
          int index,
          string personXML,
          string planXML,
          string companyXML);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "CheckForMidnight")]
        public static extern void SDInteropCheckForMidnight();

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetEFormPathAndName")]
        public static extern IntPtr SDInteropGetEFormPathAndName(string patientID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetFirstPharmacyListCount")]
        public static extern IntPtr SDInteropGetFirstPharmacyListCount(
          int count,
          string deviceKey);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetNextPharmacyListCount")]
        public static extern IntPtr SDInteropGetNextPharmacyListCount(int count, string deviceKey);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetPatientSpouse")]
        public static extern IntPtr SDInteropGetPatientSpouse(string patientID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "SavePatientSpouse")]
        public static extern bool SDInteropSavePatientSpouse(string patientID, string spouse);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetFirstSDClinicalNotes")]
        public static extern IntPtr SDInteropGetFirstSDClinicalNotes(
          string patientID,
          int count,
          string deviceKey);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetNextSDClinicalNotes")]
        public static extern IntPtr SDInteropGetNextSDClinicalNotes(
          string patientID,
          int count,
          string deviceKey);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetPreviousSDClinicalNotes")]
        public static extern IntPtr SDInteropGetPreviousSDClinicalNotes(
          string patientID,
          int count,
          string deviceKey);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "MakeClinicalNote")]
        public static extern IntPtr SDInteropMakeClinicalNote(
          string userID,
          string patientID,
          string providerID,
          string note);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetClinicalNoteDateCount")]
        public static extern int SDInteropGetClinicalNoteDateCount(string patientID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "LinkCleardown")]
        public static extern IntPtr SDInteropLinkCleardown(string PMSRecordID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "LinkConfigure")]
        public static extern IntPtr SDInteropLinkConfigure(string xmlConfiguration);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "LinkLookupPatient")]
        public static extern IntPtr SDInteropLinkLookupPatient(
          string xmlPatient,
          string xmlSource);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "LinkAddPatient")]
        public static extern IntPtr SDInteropLinkAddPatient(string xmlPatient, string xmlSource);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "LinkUpdatePatient")]
        public static extern IntPtr SDInteropLinkUpdatePatient(
          string patientID,
          string xmlPatient,
          string xmlSource,
          int option);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "LinkLookupGuarantor")]
        public static extern IntPtr SDInteropLinkLookupGuarantor(
          string xmlGuarantor,
          string xmlSource);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "LinkAddGuarantor")]
        public static extern IntPtr SDInteropLinkAddGuarantor(
          string xmGuarantor,
          string xmlSource);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "LinkUpdateGuarantor")]
        public static extern IntPtr SDInteropLinkUpdateGuarantor(
          string accountID,
          string xmlGuarantor,
          string xmlSource,
          int option);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "GetPatientMedicalAlert")]
        public static extern IntPtr SDInteropGetPatientMedicalAlert(string patientID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "KioskCheckIn")]
        public static extern bool SDInteropKioskCheckIn(string appointmentID);

        [DllImport("SDInterop_12CS.dll", EntryPoint = "SetAdapterVersion")]
        public static extern void SDInteropSetAdapterVersion(
          int major,
          int minor,
          int build,
          int revision);
    }
}
