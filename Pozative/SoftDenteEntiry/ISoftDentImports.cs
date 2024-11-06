using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    internal interface ISoftDentImports
    {
        string FileName { get; }

        int SDInteropInitializeDB(string path, bool loggingOn, string faircomServer);

        void SDInteropCloseFiles();

        void SDInteropSetLogging(bool loggingOn);

        IntPtr SDInteropGetAppointmentsForDay(
          string startDate,
          string startTime,
          string endDate,
          string endTime,
          string bookIds);

        IntPtr SDInteropGetAppointment(string PMSRecordID);

        void SDInteropConfirmAppointment(
          string PMSRecordID,
          string method,
          string date,
          string time,
          string status,
          string reason);

        IntPtr SDInteropGetOffice(string dateTime);

        void SDInteropSavePatient(string xml);

        void SDInteropSaveAppointment(string xml);

        IntPtr SDInteropGetPatient(string recordID);

        IntPtr SDInteropCreateRestorativeChartImage(string PMSRecordID, int view, int mode);

        IntPtr SDInteropCreatePerioChartImage(string PMSRecordID, int view, int mode);

        IntPtr SDInteropGetPatientDentalInsurancePlan(string PMSRecordID, bool primary);

        IntPtr SDInteropGetPatientDentalInsurance(string PMSRecordID, bool primary);

        IntPtr SDInteropGetPatientGuarantors(string PMSRecordID);

        IntPtr SDInteropGetImages(string PMSRecordID);

        int SDInteropSaveInsurance(
          string patientID,
          int nGuarIndex,
          string personXml,
          string planXml,
          string companyXml,
          bool primary);

        IntPtr SDInteropGetPatientPreferredPharmacy(string patientID);

        IntPtr SDInteropGetPatientPortrait(string patientID);

        IntPtr SDInteropGetResponsiblePartyForPatient(string patientID);

        IntPtr SDInteropGetResponsibleParty(string acctID);

        bool SDInteropPatientHasOpenLabCases(string patientID);

        IntPtr SDInteropGetMedicalHistoryConditions();

        IntPtr SDInteropGetMedicalHistoryAllergies();

        IntPtr SDInteropGetMostRecentMedicalHistory(string patientID);

        void SDInteropSaveMedicalHistory(string patientID, string xml);

        IntPtr SDInteropGetInsuranceCompany(string PMSRecordID);

        IntPtr SDInteropGetInsuranceCompanyList();

        void SDInteropSaveInsuranceCompany(string xml);

        IntPtr SDInteropSaveNewInsuranceCompany(string xml);

        IntPtr SDInteropGetInsurancePlan(string PMSRecordID);

        IntPtr SDInteropGetInsurancePlanList();

        IntPtr SDInteropGetEmployer(string PMSRecordID);

        IntPtr SDInteropGetEmployerList();

        void SDInteropSaveEmployer(string xml);

        IntPtr SDInteropSaveNewEmployer(string xml);

        IntPtr SDInteropGetPharmacy(string PMSRecordID);

        IntPtr SDInteropGetPharmacyList();

        void SDInteropSavePharmacy(string xml);

        IntPtr SDInteropSaveNewPharmacy(string xml);

        IntPtr SDInteropGetDrugList();

        IntPtr SDInteropGetDenist(string PMSRecordID);

        IntPtr SDInteropGetDentistList();

        IntPtr GetSpecialty();

        void SDInteropSaveDentist(string xml);

        IntPtr SDInteropSaveNewDentist(string xml);

        IntPtr SDInteropGetReferringDoctor(string PMSRecordID);

        IntPtr SDInteropGetReferringDoctorList();

        void SDInteropSaveReferringDoctor(string xml);

        IntPtr SDInteropSaveNewReferringDoctor(string xml);

        IntPtr SDInteropGetADACodeList();

        int SDInteropArchiveEForm(
          string patientID,
          string eformFilePath,
          string archiveDate,
          string archiveTime,
          string description);

        IntPtr SDInteropGetImage(string imageRecordID);

        IntPtr SDInteropPearlGetPatientList();

        

        IntPtr SDInteropPearlGetInitialPatientList();

        IntPtr SDInteropPearlGetFirstPatientCount(int count, string deviceKey);

        IntPtr SDInteropPearlGetNextPatientCount(int count, string deviceKey);

        IntPtr SDInteropPearlQueryPatientList(string query);

        IntPtr SDInteropPearlGetPatient(string recordID);

        IntPtr SDInteropPearlGetPatientDetail(string recordID);

        IntPtr SDInteropPearlGetPatientClinicalProfile(string recordID);

        IntPtr SDInteropPearlGetPatientAlerts(string recordID);

        IntPtr SDInteropPearlGetPatientMedication(string recordID);

        IntPtr SDInteropPearlGetMoreAppointments(string recordID, int count);

        IntPtr SDInteropPearlGetFirstPatientClinicalImages(
          string recordID,
          int count,
          string deviceKey);

        IntPtr SDInteropPearlGetNextPatientClinicalImages(
          string recordID,
          int count,
          string deviceKey);

        IntPtr SDInteropPearlClearDeviceKey();

        IntPtr SDInteropPearlGetConsultingProviderList();

        IntPtr SDInteropPearlGetFirstConsultingProviderCount(int count, string deviceKey);

        IntPtr SDInteropPearlGetNextConsultingProviderCount(int count, string deviceKey);

        IntPtr SDInteropPearlQueryConsultingProviderList(string query);

        IntPtr SDInteropPearlGetConsultingProvider(string recordID);

        IntPtr SDInteropPearlGetConsultingProviderProfile(string recordID);

        IntPtr SDInteropPearlGetConsultingProviderDetails(string recordID);

        IntPtr SDInteropPearlGetFinancialData(string providerId, int timePeriod);

        int SDInteropPearlSaveNewPrescription(string patientID, string userID, string xml);

        int SDInteropPearlSavePatientCallDetails(string userID, string xml);

        IntPtr SDInteropPearlGetBlockedSlots(
          string startDate,
          string startTime,
          string endDate,
          string endTime);

        IntPtr SDInteropPearlGetAppointments(
          string providerID,
          string startDate,
          string startTime,
          string endDate,
          string endTime);

        IntPtr SDInteropPearlGetCallList(string providerID, bool refresh);

        int SDInteropSaveConsultingProviderCallDetails(string userID, string xml);

        IntPtr SDInteropPearlLoginUser(string userName, string password);

        IntPtr SDInteropGetEServiceFlags();

        IntPtr SDInteropGetMissedApptsRevenueAnalysis(string startDate, string endDate);

        IntPtr SDInteropLinkCleardown(string PMSRecordID);

        IntPtr SDInteropLinkConfigure(string xmlConfiguration);

        IntPtr SDInteropLinkLookupPatient(string xmlPatient, string xmlSource);

        IntPtr SDInteropLinkAddPatient(string xmlPatient, string xmlSource);

        IntPtr SDInteropLinkUpdatePatient(
          string patientID,
          string xmlPatient,
          string xmlSource,
          int option);

        IntPtr SDInteropLinkLookupGuarantor(string xmlGuarantor, string xmlSource);

        IntPtr SDInteropLinkAddGuarantor(string xmlGuarantor, string xmlSource);

        IntPtr SDInteropLinkUpdateGuarantor(
          string accountID,
          string xmlGuarantor,
          string xmlSource,
          int option);

        IntPtr SDInteropGetPatientEmployee(string patientID);

        IntPtr SDInteropGetResponsiblePartyEmployee(string patientID);

        void SDInteropSaveEmployerForPatient(string patientID, string status, string employer);

        void SDInteropSaveResponsiblePartyForPatient(string patientID, string responsibleParty);

        int SDInteropPutImage(
          string patientID,
          int imageType,
          int acquisitionRegion,
          string imageFileName,
          string imageFilePath,
          string toothAssociations,
          string acquisitionDate,
          string acquisitionTime);

        IntPtr SDInteropGetOperatories();

        IntPtr SDInteropPearlGetAppointmentsByOperatory(
          string operatoryID,
          string startDate,
          string startTime,
          string endDate,
          string endTime);

        IntPtr SDInteropPearlMakeAppointment(
          string patientID,
          string providerID,
          string operatoryID,
          string date,
          string time,
          int duration,
          string note);

        int SDInteropPearlMakeBlockedSlot(
          string userID,
          string operatoryID,
          string providerID,
          string date,
          string time,
          int duration,
          string note);

        IntPtr SDInteropPearlGetNextCallListCount(string providerID, int count, string deviceKey);

        IntPtr SDInteropPearlGetFirstCallListCount(
          string providerID,
          int count,
          string deviceKey,
          bool refresh);

        IntPtr SDInteropGetPatientPerson(string patientID, int index);

        void SDInteropSavePatientPerson(
          string patientID,
          int index,
          string personXML,
          string planXML,
          string companyXML);

        void SDInteropCheckForMidnight();

        IntPtr SDInteropGetEFormPathAndName(string patientID);

        IntPtr SDInteropGetFirstPharmacyListCount(int count, string deviceKey);

        IntPtr SDInteropGetNextPharmacyListCount(int count, string deviceKey);

        IntPtr SDInteropGetPatientSpouse(string patientID);

        bool SDInteropSavePatientSpouse(string patientID, string spouse);

        IntPtr SDInteropGetFirstSDClinicalNotes(string patientID, int count, string deviceKey);

        IntPtr SDInteropGetNextSDClinicalNotes(string patientID, int count, string deviceKey);

        IntPtr SDInteropGetPreviousSDClinicalNotes(string patientID, int count, string deviceKey);

        IntPtr SDInteropMakeClinicalNote(
          string userID,
          string patientID,
          string providerID,
          string note);

        int SDInteropGetClinicalNoteDateCount(string patientID);

        IntPtr SDInteropGetPatientMedicalAlert(string patientID);

        bool SDInteropKioskCheckIn(string appointmentID);

        void SDInteropSetAdapterVersion(int major, int minor, int build, int revision);
    }
}
