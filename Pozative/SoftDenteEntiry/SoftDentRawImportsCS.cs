// Decompiled with JetBrains decompiler
// Type: PW.Adapters.SoftDent.Database.Interop.SoftDentRawImportsCS
// Assembly: RVGMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C5DF1CD-A026-4FA6-83A7-5CF2AF8D7D47
// Assembly location: D:\SoftdentDll\exe\RVGMobile.exe

using System;
using System.Runtime.InteropServices;

namespace Pozative
{
  internal static class SoftDentRawImportsCS
  {
    [DllImport("SDInteropCS.Dll", EntryPoint = "InitializeDB")]
    public static extern int SDInteropInitializeDB(
      string path,
      bool loggingOn,
      string faircomServer);

    [DllImport("SDInteropCS.Dll", EntryPoint = "CloseFiles")]
    public static extern void SDInteropCloseFiles();

    [DllImport("SDInteropCS.Dll", EntryPoint = "SetLogging")]
    public static extern void SDInteropSetLogging(bool loggingOn);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetAppointments")]
    public static extern IntPtr SDInteropGetAppointmentsForDay(
      string startDate,
      string startTime,
      string endDate,
      string endTime,
      string bookIds);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetAppointment")]
    public static extern IntPtr SDInteropGetAppointment(string PMSRecordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "ConfirmAppointment")]
    public static extern void SDInteropConfirmAppointment(
      string PMSRecordID,
      string Method,
      string Date,
      string Time,
      string Status,
      string Reason);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetOffice")]
    public static extern IntPtr SDInteropGetOffice(string dateTime);

    [DllImport("SDInteropCS.Dll", EntryPoint = "SavePatient")]
    public static extern void SDInteropSavePatient(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveAppointment")]
    public static extern void SDInteropSaveAppointment(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetPatient")]
    public static extern IntPtr SDInteropGetPatient(string recordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "CreateRestorativeChartImage")]
    public static extern IntPtr SDInteropCreateRestorativeChartImage(
      string PMSRecordID,
      int view,
      int mode);

    [DllImport("SDInteropCS.Dll", EntryPoint = "CreatePerioChartImage")]
    public static extern IntPtr SDInteropCreatePerioChartImage(
      string PMSRecordID,
      int view,
      int mode);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetPatientDentalInsurancePlan")]
    public static extern IntPtr SDInteropGetPatientDentalInsurancePlan(
      string PMSRecordID,
      bool primary);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetPatientDentalInsurance")]
    public static extern IntPtr SDInteropGetPatientDentalInsurance(
      string PMSRecordID,
      bool primary);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetPatientGuarantors")]
    public static extern IntPtr SDInteropGetPatientGuarantors(string PMSRecordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetImages")]
    public static extern IntPtr SDInteropGetImages(string PMSRecordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "SavePatientInsurance")]
    public static extern int SDInteropSaveInsurance(
      string patientID,
      int nGuarIndex,
      string personXml,
      string planXml,
      string companyXml,
      bool primary);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetPatientPreferredPharmacy")]
    public static extern IntPtr SDInteropGetPatientPreferredPharmacy(string patientID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetPatientPortrait")]
    public static extern IntPtr SDInteropGetPatientPortrait(string patientID);

    [DllImport("SDInteropCS.dll", EntryPoint = "PatientHasOpenLabCases")]
    public static extern bool SDInteropPatientHasOpenLabCases(string patientID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetMedicalHistoryConditions")]
    public static extern IntPtr SDInteropGetMedicalHistoryConditions();

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetMedicalHistoryAllergies")]
    public static extern IntPtr SDInteropGetMedicalHistoryAllergies();

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetMostRecentMedicalHistory")]
    public static extern IntPtr SDInteropGetMostRecentMedicalHistory(string patientID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveMedicalHistory")]
    public static extern void SDInteropSaveMedicalHistory(string patientID, string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetInsuranceCompany")]
    public static extern IntPtr SDInteropGetInsuranceCompany(string PMSRecordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetInsuranceCompanyList")]
    public static extern IntPtr SDInteropGetInsuranceCompanyList();

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveInsuranceCompany")]
    public static extern void SDInteropSaveInsuranceCompany(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveNewInsuranceCompany")]
    public static extern IntPtr SDInteropSaveNewInsuranceCompany(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetInsurancePlan")]
    public static extern IntPtr SDInteropGetInsurancePlan(string PMSRecordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetInsurancePlanList")]
    public static extern IntPtr SDInteropGetInsurancePlanList();

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetEmployer")]
    public static extern IntPtr SDInteropGetEmployer(string PMSRecordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetEmployerList")]
    public static extern IntPtr SDInteropGetEmployerList();

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveEmployer")]
    public static extern void SDInteropSaveEmployer(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveNewEmployer")]
    public static extern IntPtr SDInteropSaveNewEmployer(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetPharmacy")]
    public static extern IntPtr SDInteropGetPharmacy(string PMSRecordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetPharmacyList")]
    public static extern IntPtr SDInteropGetPharmacyList();

    [DllImport("SDInteropCS.Dll", EntryPoint = "SavePharmacy")]
    public static extern void SDInteropSavePharmacy(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveNewPharmacy")]
    public static extern IntPtr SDInteropSaveNewPharmacy(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetDrugList")]
    public static extern IntPtr SDInteropGetDrugList();

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetDentist")]
    public static extern IntPtr SDInteropGetDenist(string PMSRecordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetDentistList")]
    public static extern IntPtr SDInteropGetDentistList();

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveDentist")]
    public static extern void SDInteropSaveDentist(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveNewDentist")]
    public static extern IntPtr SDInteropSaveNewDentist(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetReferringDoctor")]
    public static extern IntPtr SDInteropGetReferringDoctor(string PMSRecordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetReferringDoctorList")]
    public static extern IntPtr SDInteropGetReferringDoctorList();

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveReferringDoctor")]
    public static extern void SDInteropSaveReferringDoctor(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "SaveNewReferringDoctor")]
    public static extern IntPtr SDInteropSaveNewReferringDoctor(string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetADACodeList")]
    public static extern IntPtr SDInteropGetADACodeList();

    [DllImport("SDInteropCS.Dll", EntryPoint = "ArchiveEForm")]
    public static extern int SDInteropArchiveEForm(
      string patientID,
      string eformFilePath,
      string archiveDate,
      string archiveTime,
      string description);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetImage")]
    public static extern IntPtr SDInteropGetImage(string imageRecordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetPatientList")]
    public static extern IntPtr SDInteropPearlGetPatientList();

    [DllImport("SDInteropCS.dll", EntryPoint = "GetSpecialty")]
    public static extern IntPtr GetSpecialty();

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetInitialPatientList")]
    public static extern IntPtr SDInteropPearlGetInitialPatientList();

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetFirstPatientCount")]
    public static extern IntPtr SDInteropPearlGetFirstPatientCount(
      int count,
      string deviceKey);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetNextPatientCount")]
    public static extern IntPtr SDInteropPearlGetNextPatientCount(int count, string deviceKey);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlQueryPatientList")]
    public static extern IntPtr SDInteropPearlQueryPatientList(string query);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetPatient")]
    public static extern IntPtr SDInteropPearlGetPatient(string recordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetPatientDetail")]
    public static extern IntPtr SDInteropPearlGetPatientDetail(string recordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetPatientClinicalProfile")]
    public static extern IntPtr SDInteropPearlGetPatientClinicalProfile(string recordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetPatientAlerts")]
    public static extern IntPtr SDInteropPearlGetPatientAlerts(string recordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetPatientMedication")]
    public static extern IntPtr SDInteropPearlGetPatientMedication(string recordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetMoreAppointments")]
    public static extern IntPtr SDInteropPearlGetMoreAppointments(string recordID, int count);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetFirstPatientClinicalImages")]
    public static extern IntPtr SDInteropPearlGetFirstPatientClinicalImages(
      string recordID,
      int count,
      string deviceKey);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetNextPatientClinicalImages")]
    public static extern IntPtr SDInteropPearlGetNextPatientClinicalImages(
      string recordID,
      int count,
      string deviceKey);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlClearDeviceKey")]
    public static extern IntPtr SDInteropPearlClearDeviceKey();

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetConsultingProviderList")]
    public static extern IntPtr SDInteropPearlGetConsultingProviderList();

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetFirstConsultingProviderCount")]
    public static extern IntPtr SDInteropPearlGetFirstConsultingProviderCount(
      int count,
      string deviceKey);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetNextConsultingProviderCount")]
    public static extern IntPtr SDInteropPearlGetNextConsultingProviderCount(
      int count,
      string deviceKey);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlQueryConsultingProviderList")]
    public static extern IntPtr SDInteropPearlQueryConsultingProviderList(string query);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetConsultingProvider")]
    public static extern IntPtr SDInteropPearlGetConsultingProvider(string recordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetConsultingProviderProfile")]
    public static extern IntPtr SDInteropPearlGetConsultingProviderProfile(string recordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetConsultingProviderDetails")]
    public static extern IntPtr SDInteropPearlGetConsultingProviderDetails(string recordID);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetFinancialData")]
    public static extern IntPtr SDInteropPearlGetFinancialData(
      string providerId,
      int timePeriod);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlSaveNewPrescription")]
    public static extern int SDInteropPearlSaveNewPrescription(
      string patientID,
      string userID,
      string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlSavePatientCallDetails")]
    public static extern int SDInteropPearlSavePatientCallDetails(string userID, string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetBlockedSlots")]
    public static extern IntPtr SDInteropPearlGetBlockedSlots(
      string startDate,
      string startTime,
      string endDate,
      string endTime);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetAppointments")]
    public static extern IntPtr SDInteropPearlGetAppointments(
      string providerID,
      string startDate,
      string startTime,
      string endDate,
      string endTime);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlGetCallList")]
    public static extern IntPtr SDInteropPearlGetCallList(string providerID, bool refresh);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlSaveConsultingProviderCallDetails")]
    public static extern int SDInteropSaveConsultingProviderCallDetails(string userID, string xml);

    [DllImport("SDInteropCS.Dll", EntryPoint = "PearlLoginUser")]
    public static extern IntPtr SDInteropPearlLoginUser(string userName, string password);

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetEServiceFlags")]
    public static extern IntPtr SDInteropGetEServiceFlags();

    [DllImport("SDInteropCS.Dll", EntryPoint = "GetMissedApptsRevenueAnalysis")]
    public static extern IntPtr SDInteropGetMissedApptsRevenueAnalysis(
      string startDate,
      string endDate);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetResponsiblePartyForPatient")]
    public static extern IntPtr SDInteropGetResponsiblePartyForPatient(string patientID);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetResponsibleParty")]
    public static extern IntPtr SDInteropGetResponsibleParty(string acctID);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetPatientEmployee")]
    public static extern IntPtr SDInteropGetPatientEmployee(string patientID);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetResponsiblePartyEmployee")]
    public static extern IntPtr SDInteropGetResponsiblePartyEmployee(string patientID);

    [DllImport("SDInteropCS.dll", EntryPoint = "SaveEmployerForPatient")]
    public static extern void SDInteropSaveEmployerForPatient(
      string patientID,
      string status,
      string employer);

    [DllImport("SDInteropCS.dll", EntryPoint = "SaveResponsiblePartyForPatient")]
    public static extern void SDInteropSaveResponsiblePartyForPatient(
      string patientID,
      string responsibleParty);

    [DllImport("SDInteropCS.dll", EntryPoint = "PutImage")]
    public static extern int SDInteropPutImage(
      string patientID,
      int imageType,
      int acquisitionRegion,
      string imageFileName,
      string imageFilePath,
      string toothAssociations,
      string acquisitionDate,
      string acquisitionTime);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetOperatories")]
    public static extern IntPtr SDInteropGetOperatories();

    [DllImport("SDInteropCS.dll", EntryPoint = "PearlGetAppointmentsByOperatory")]
    public static extern IntPtr SDInteropPearlGetAppointmentsByOperatory(
      string operatoryID,
      string startDate,
      string startTime,
      string endDate,
      string endTime);

    [DllImport("SDInteropCS.dll", EntryPoint = "PearlMakeAppointment")]
    public static extern IntPtr SDInteropPearlMakeAppointment(
      string patientID,
      string providerID,
      string operatoryID,
      string date,
      string time,
      int duration,
      string note);

    [DllImport("SDInteropCS.dll", EntryPoint = "PearlMakeBlockedSlot")]
    public static extern int SDInteropPearlMakeBlockedSlot(
      string userID,
      string operatoryID,
      string providerID,
      string date,
      string time,
      int duration,
      string note);

    [DllImport("SDInteropCS.dll", EntryPoint = "PearlGetNextCallListCount")]
    public static extern IntPtr SDInteropPearlGetNextCallListCount(
      string providerID,
      int count,
      string deviceKey);

    [DllImport("SDInteropCS.dll", EntryPoint = "PearlGetFirstCallListCount")]
    public static extern IntPtr SDInteropPearlGetFirstCallListCount(
      string providerID,
      int count,
      string deviceKey,
      bool refresh);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetPatientPerson")]
    public static extern IntPtr SDInteropGetPatientPerson(string patientID, int index);

    [DllImport("SDInteropCS.dll", EntryPoint = "SavePatientPerson")]
    public static extern void SDInteropSavePatientPerson(
      string patientID,
      int index,
      string personXML,
      string planXML,
      string companyXML);

    [DllImport("SDInteropCS.dll", EntryPoint = "CheckForMidnight")]
    public static extern void SDInteropCheckForMidnight();

    [DllImport("SDInteropCS.dll", EntryPoint = "GetEFormPathAndName")]
    public static extern IntPtr SDInteropGetEFormPathAndName(string patientID);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetFirstPharmacyListCount")]
    public static extern IntPtr SDInteropGetFirstPharmacyListCount(
      int count,
      string deviceKey);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetNextPharmacyListCount")]
    public static extern IntPtr SDInteropGetNextPharmacyListCount(int count, string deviceKey);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetPatientSpouse")]
    public static extern IntPtr SDInteropGetPatientSpouse(string patientID);

    [DllImport("SDInteropCS.dll", EntryPoint = "SavePatientSpouse")]
    public static extern bool SDInteropSavePatientSpouse(string patientID, string spouse);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetFirstSDClinicalNotes")]
    public static extern IntPtr SDInteropGetFirstSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetNextSDClinicalNotes")]
    public static extern IntPtr SDInteropGetNextSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetPreviousSDClinicalNotes")]
    public static extern IntPtr SDInteropGetPreviousSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey);

    [DllImport("SDInteropCS.dll", EntryPoint = "MakeClinicalNote")]
    public static extern IntPtr SDInteropMakeClinicalNote(
      string userID,
      string patientID,
      string providerID,
      string note);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetClinicalNoteDateCount")]
    public static extern int SDInteropGetClinicalNoteDateCount(string patientID);

    [DllImport("SDInteropCS.dll", EntryPoint = "LinkCleardown")]
    public static extern IntPtr SDInteropLinkCleardown(string PMSRecordID);

    [DllImport("SDInteropCS.dll", EntryPoint = "LinkConfigure")]
    public static extern IntPtr SDInteropLinkConfigure(string xmlConfiguration);

    [DllImport("SDInteropCS.dll", EntryPoint = "LinkLookupPatient")]
    public static extern IntPtr SDInteropLinkLookupPatient(
      string xmlPatient,
      string xmlSource);

    [DllImport("SDInteropCS.dll", EntryPoint = "LinkAddPatient")]
    public static extern IntPtr SDInteropLinkAddPatient(string xmlPatient, string xmlSource);

    [DllImport("SDInteropCS.dll", EntryPoint = "LinkUpdatePatient")]
    public static extern IntPtr SDInteropLinkUpdatePatient(
      string patientID,
      string xmlPatient,
      string xmlSource,
      int option);

    [DllImport("SDInteropCS.dll", EntryPoint = "LinkLookupGuarantor")]
    public static extern IntPtr SDInteropLinkLookupGuarantor(
      string xmlGuarantor,
      string xmlSource);

    [DllImport("SDInteropCS.dll", EntryPoint = "LinkAddGuarantor")]
    public static extern IntPtr SDInteropLinkAddGuarantor(
      string xmGuarantor,
      string xmlSource);

    [DllImport("SDInteropCS.dll", EntryPoint = "LinkUpdateGuarantor")]
    public static extern IntPtr SDInteropLinkUpdateGuarantor(
      string accountID,
      string xmlGuarantor,
      string xmlSource,
      int option);

    [DllImport("SDInteropCS.dll", EntryPoint = "GetPatientMedicalAlert")]
    public static extern IntPtr SDInteropGetPatientMedicalAlert(string patientID);

    [DllImport("SDInteropCS.dll", EntryPoint = "KioskCheckIn")]
    public static extern bool SDInteropKioskCheckIn(string appointmentID);

    [DllImport("SDInteropCS.dll", EntryPoint = "SetAdapterVersion")]
    public static extern void SDInteropSetAdapterVersion(
      int major,
      int minor,
      int build,
      int revision);
  }
}
