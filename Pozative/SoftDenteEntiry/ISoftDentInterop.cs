using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public interface ISoftDentInterop
    {
        bool Open(string path, string faircomServer, out ErrorCode errorCode);

        void Close();

        void SetLogging(bool loggingOn);

        IntPtr GetAppointments(DateTime begin, DateTime end);

        IntPtr GetAppointment(string PMSRecordID);

        void ConfirmAppointment(
          string PMSRecordID,
          string Method,
          DateTime Date,
          string Status,
          string Reason);

        IntPtr GetOffice(DateTime dateTime);

        void SavePatient(string xml);

        void SaveAppointment(string xml);

        IntPtr GetPatient(string recordID);

        string CreateRestorativeChartImage(string PMSRecordID, int view, int mode);

        string CreatePerioChartImage(string PMSRecordID, int view, int mode);

        IntPtr GetPatientDentalInsurancePlan(string PMSRecordID, bool primary);

        IntPtr GetPatientDentalInsurance(string PMSRecordID, bool primary);

        IntPtr GetPatientGuarantors(string PMSRecordID);

        IntPtr GetImages(string PMSRecordID);

        void SaveInsurance(
          string patientID,
          int nGuarIndex,
          string personXml,
          string planXml,
          string companyXml,
          bool primary);

        IntPtr GetPatientPreferredPharmacy(string patientID);

        IntPtr GetPatientPotrait(string patientID);

        IntPtr GetResponsiblePartyForPatient(string patientID);

        IntPtr GetResponsibleParty(string acctID);

        bool PatientHasOpenLabCases(string PMSRecordID);

        IntPtr GetMedicalHistoryConditions();

        IntPtr GetMedicalHistoryAllergies();

        IntPtr GetMostRecentMedicalHistory(string patientID);

        void SaveMedicalHistory(string patientID, string xml);

        IntPtr GetInsuranceCompany(string PMSRecordID);

        IntPtr GetInsuranceCompanyList();

        void SaveInsuranceCompany(string xml);

        IntPtr SaveNewInsuranceCompany(string xml);

        IntPtr GetInsurancePlanList();

        IntPtr GetInsurancePlan(string PMSRecordID);

        IntPtr GetPharmacy(string PMSRecordID);

        IntPtr GetPharmacyList();

        void SavePharmacy(string xml);

        IntPtr SaveNewPharmacy(string xml);

        IntPtr GetDrugList();

        IntPtr GetEmployer(string PMSRecordID);

        IntPtr GetEmployerList();

        void SaveEmployer(string xml);

        IntPtr SaveNewEmployer(string xml);

        IntPtr GetProvider(string PMSRecordID);

        IntPtr GetProviderList();

        IntPtr GetSpecialty();

        void SaveProvider(string xml);

        string SaveNewProvider(string xml);

        IntPtr GetConsultingProvider(string PMSRecordID);

        IntPtr GetConsultingProviderList();

        void SaveConsultingProvider(string xml);

        string SaveNewConsultingProvider(string xml);

        IntPtr GetADACodeList();      

        int ArchiveEForm(
          string patientID,
          string eformFilePath,
          DateTime archiveDateTime,
          string description);

        IntPtr GetImage(string imageID);

        IntPtr PearlLoginUser(string userName, string password);

        void PearlClearDeviceKey();

        IntPtr PearlGetPatientList();

        IntPtr PearlGetInitialPatientList();

        IntPtr PearlGetFirstPatientCount(int count, string deviceKey);

        IntPtr PearlGetNextPatientCount(int count, string deviceKey);

        IntPtr PearlQueryPatientList(string query);

        IntPtr PearlGetPatient(string recordID);

        IntPtr PearlGetPatientDetail(string recordID);

        IntPtr PearlGetPatientClinicalProfile(string recordID);

        IntPtr PearlGetPatientAlerts(string recordID);

        IntPtr PearlGetPatientMedication(string recordID);

        IntPtr PearlGetMoreAppointments(string recordID, int count);

        IntPtr PearlGetFirstPatientClinicalImages(string recordID, int count, string deviceKey);

        IntPtr PearlGetNextPatientClinicalImages(string recordID, int count, string deviceKey);

        IntPtr PearlGetConsultingProviderList();

        IntPtr PearlGetFirstConsultingProviderCount(int count, string deviceKey);

        IntPtr PearlGetNextConsultingProviderCount(int count, string deviceKey);

        IntPtr PearlQueryConsultingProviderList(string query);

        IntPtr PearlGetConsultingProvider(string recordID);

        IntPtr PearlGetConsultingProviderProfile(string recordID);

        IntPtr PearlGetConsultingProviderDetails(string recordID);

        IntPtr PearlGetFinancialData(string providerId, int timePeriod);

        int PearlSaveNewPrescription(string patientID, string userID, string xml);

        int PearlSavePatientCallDetails(string userID, string xml);

        IntPtr PearlGetBlockedSlots(DateTime start, DateTime end);

        IntPtr PearlGetAppointments(string providerId, DateTime start, DateTime end);

        IntPtr PearlGetCallList(string providerId, bool refresh);

        int PearlSaveConsultingProviderCallDetails(string userID, string xml);

        IntPtr GetEServiceFlags();

        IntPtr GetMissedApptsRevenueAnalysis(DateTime startDate, DateTime endDate);

        IntPtr LinkCleardown(string PMSRecordID);

        IntPtr LinkConfigure(string xmlConfiguration);

        IntPtr LinkLookupPatient(string xmlPatient, string xmlSource);

        IntPtr LinkAddPatient(string xmlPatient, string xmlSource);

        IntPtr LinkUpdatePatient(
          string patientID,
          string xmlPatient,
          string xmlSource,
          int option);

        IntPtr LinkLookupGuarantor(string xmlGuarantor, string xmlSource);

        IntPtr LinkAddGuarantor(string xmlGuarantor, string xmlSource);

        IntPtr LinkUpdateGuarantor(
          string accountID,
          string xmlGuarantor,
          string xmlSource,
          int option);

        IntPtr GetPatientEmployee(string patientID);

        IntPtr GetResponsiblePartyEmployee(string patientID);

        void SaveEmployerForPatient(string patientID, string status, string employer);

        void SaveResponsiblePartyForPatient(string patientID, string responsibleParty);

        int PutImage(
          string patientID,
          int imageType,
          int acquisitionRegion,
          string imageFileName,
          string imageFilePath,
          string toothAssociations,
          string acquisitionDate,
          string acquisitionTime);

        IntPtr GetOperatories();

        IntPtr PearlGetAppointmentsByOperatory(string operatoryID, DateTime start, DateTime end);

        IntPtr PearlMakeAppointment(
          string patientID,
          string providerID,
          string operatoryID,
          DateTime dateTime,
          int duration,
          string note);

        int PearlMakeBlockedSlot(
          string userID,
          string operatoryID,
          string providerID,
          DateTime dateTime,
          int duration,
          string note);

        IntPtr PearlGetNextCallListCount(string providerID, int count, string deviceKey);

        IntPtr PearlGetFirstCallListCount(
          string providerID,
          int count,
          string deviceKey,
          bool refresh);

        IntPtr GetPatientPerson(string patientID, int index);

        void SavePatientPerson(
          string patientID,
          int index,
          string personXML,
          string planXML,
          string companyXML);

        IntPtr GetEFormPathAndName(string patientID);

        IntPtr GetFirstPharmacyListCount(int count, string deviceKey);

        IntPtr GetNextPharmacyListCount(int count, string deviceKey);

        IntPtr GetPatientSpouse(string patientID);

        bool SavePatientSpouse(string patientID, string spouse);

        IntPtr GetFirstSDClinicalNotes(string patientID, int count, string deviceKey);

        IntPtr GetNextSDClinicalNotes(string patientID, int count, string deviceKey);

        IntPtr GetPreviousSDClinicalNotes(string patientID, int count, string deviceKey);

        IntPtr MakeClinicalNote(string userID, string patientID, string providerID, string note);

        int GetClinicalNoteDateCount(string patientID);

        IntPtr GetPatientMedicalAlert(string patientID);

        bool KioskCheckIn(string appointmentID);

        void SetAdapterVersion(int major, int minor, int build, int revision);
    }
}
