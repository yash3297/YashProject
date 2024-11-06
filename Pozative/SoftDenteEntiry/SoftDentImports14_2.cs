// Decompiled with JetBrains decompiler
// Type: PW.Adapters.SoftDent.Database.Interop.SoftDentImports14_2
// Assembly: RVGMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C5DF1CD-A026-4FA6-83A7-5CF2AF8D7D47
// Assembly location: D:\SoftdentDll\exe\RVGMobile.exe

using System;

namespace Pozative
{
  internal class SoftDentImports14_2 : ISoftDentImports
  {
    public string FileName
    {
      get
      {
        return "SDInterop_14_2.dll";
      }
    }

    public int SDInteropInitializeDB(string path, bool loggingOn, string faircomServer)
    {
      return SoftDentRawImports14_2.SDInteropInitializeDB(path, loggingOn, faircomServer);
    }

    public void SDInteropCloseFiles()
    {
      SoftDentRawImports14_2.SDInteropCloseFiles();
    }

    public void SDInteropSetLogging(bool loggingOn)
    {
      SoftDentRawImports14_2.SDInteropSetLogging(loggingOn);
    }

    public IntPtr SDInteropGetAppointmentsForDay(
      string startDate,
      string startTime,
      string endDate,
      string endTime,
      string bookIds)
    {
      return SoftDentRawImports14_2.SDInteropGetAppointmentsForDay(startDate, startTime, endDate, endTime, bookIds);
    }

    public IntPtr SDInteropGetAppointment(string PMSRecordID)
    {
      return SoftDentRawImports14_2.SDInteropGetAppointment(PMSRecordID);
    }

    public void SDInteropConfirmAppointment(
      string PMSRecordID,
      string Method,
      string Date,
      string Time,
      string Status,
      string Reason)
    {
      SoftDentRawImports14_2.SDInteropConfirmAppointment(PMSRecordID, Method, Date, Time, Status, Reason);
    }

    public IntPtr SDInteropGetOffice(string dateTime)
    {
      return SoftDentRawImports14_2.SDInteropGetOffice(dateTime);
    }

    public void SDInteropSavePatient(string xml)
    {
      SoftDentRawImports14_2.SDInteropSavePatient(xml);
    }

    public void SDInteropSaveAppointment(string xml)
    {
        SoftDentRawImports14_2.SDInteropSaveAppointment(xml);
    }

    public IntPtr SDInteropGetPatient(string recordID)
    {
      return SoftDentRawImports14_2.SDInteropGetPatient(recordID);
    }

    public IntPtr SDInteropCreateRestorativeChartImage(
      string PMSRecordID,
      int view,
      int mode)
    {
      return SoftDentRawImports14_2.SDInteropCreateRestorativeChartImage(PMSRecordID, view, mode);
    }

    public IntPtr SDInteropCreatePerioChartImage(string PMSRecordID, int view, int mode)
    {
      return SoftDentRawImports14_2.SDInteropCreatePerioChartImage(PMSRecordID, view, mode);
    }

    public IntPtr SDInteropGetPatientDentalInsurancePlan(string PMSRecordID, bool primary)
    {
      return SoftDentRawImports14_2.SDInteropGetPatientDentalInsurancePlan(PMSRecordID, primary);
    }

    public IntPtr SDInteropGetPatientDentalInsurance(string PMSRecordID, bool primary)
    {
      return SoftDentRawImports14_2.SDInteropGetPatientDentalInsurance(PMSRecordID, primary);
    }

    public IntPtr SDInteropGetPatientGuarantors(string PMSRecordID)
    {
      return SoftDentRawImports14_2.SDInteropGetPatientGuarantors(PMSRecordID);
    }

    public IntPtr SDInteropGetImages(string PMSRecordID)
    {
      return SoftDentRawImports14_2.SDInteropGetImages(PMSRecordID);
    }

    public int SDInteropSaveInsurance(
      string patientID,
      int nGuarIndex,
      string personXml,
      string planXml,
      string companyXml,
      bool primary)
    {
      return SoftDentRawImports14_2.SDInteropSaveInsurance(patientID, nGuarIndex, personXml, planXml, companyXml, primary);
    }

    public IntPtr SDInteropGetPatientPreferredPharmacy(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropGetPatientPreferredPharmacy(patientID);
    }

    public IntPtr SDInteropGetPatientPortrait(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropGetPatientPortrait(patientID);
    }

    public IntPtr SDInteropGetMedicalHistoryConditions()
    {
      return SoftDentRawImports14_2.SDInteropGetMedicalHistoryConditions();
    }

    public IntPtr SDInteropGetMedicalHistoryAllergies()
    {
      return SoftDentRawImports14_2.SDInteropGetMedicalHistoryAllergies();
    }

    public IntPtr SDInteropGetMostRecentMedicalHistory(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropGetMostRecentMedicalHistory(patientID);
    }

    public void SDInteropSaveMedicalHistory(string patientID, string xml)
    {
      SoftDentRawImports14_2.SDInteropSaveMedicalHistory(patientID, xml);
    }

    public IntPtr SDInteropGetInsuranceCompany(string PMSRecordID)
    {
      return SoftDentRawImports14_2.SDInteropGetInsuranceCompany(PMSRecordID);
    }

    public IntPtr SDInteropGetInsuranceCompanyList()
    {
      return SoftDentRawImports14_2.SDInteropGetInsuranceCompanyList();
    }

    public void SDInteropSaveInsuranceCompany(string xml)
    {
      SoftDentRawImports14_2.SDInteropSaveInsuranceCompany(xml);
    }

    public IntPtr SDInteropSaveNewInsuranceCompany(string xml)
    {
      return SoftDentRawImports14_2.SDInteropSaveNewInsuranceCompany(xml);
    }

    public IntPtr SDInteropGetInsurancePlan(string PMSRecordID)
    {
      return SoftDentRawImports14_2.SDInteropGetInsurancePlan(PMSRecordID);
    }

    public IntPtr SDInteropGetInsurancePlanList()
    {
      return SoftDentRawImports14_2.SDInteropGetInsurancePlanList();
    }

    public IntPtr SDInteropGetEmployer(string PMSRecordID)
    {
      return SoftDentRawImports14_2.SDInteropGetEmployer(PMSRecordID);
    }

    public IntPtr SDInteropGetEmployerList()
    {
      return SoftDentRawImports14_2.SDInteropGetEmployerList();
    }

    public void SDInteropSaveEmployer(string xml)
    {
      SoftDentRawImports14_2.SDInteropSaveEmployer(xml);
    }

    public IntPtr SDInteropSaveNewEmployer(string xml)
    {
      return SoftDentRawImports14_2.SDInteropSaveNewEmployer(xml);
    }

    public IntPtr SDInteropGetPharmacy(string PMSRecordID)
    {
      return SoftDentRawImports14_2.SDInteropGetPharmacy(PMSRecordID);
    }

    public IntPtr SDInteropGetPharmacyList()
    {
      return SoftDentRawImports14_2.SDInteropGetPharmacyList();
    }

    public void SDInteropSavePharmacy(string xml)
    {
      SoftDentRawImports14_2.SDInteropSavePharmacy(xml);
    }

    public IntPtr SDInteropSaveNewPharmacy(string xml)
    {
      return SoftDentRawImports14_2.SDInteropSaveNewPharmacy(xml);
    }

    public IntPtr SDInteropGetDrugList()
    {
      return SoftDentRawImports14_2.SDInteropGetDrugList();
    }

    public IntPtr SDInteropGetDenist(string PMSRecordID)
    {
      return SoftDentRawImports14_2.SDInteropGetDenist(PMSRecordID);
    }

    public IntPtr SDInteropGetDentistList()
    {
      return SoftDentRawImports14_2.SDInteropGetDentistList();
    }

    public IntPtr GetSpecialty()
    {
        try
        {
            return SoftDentRawImports14_2.GetSpecialty();
        }
        catch (Exception)
        {
            
            throw;
        }
        
    }

    public void SDInteropSaveDentist(string xml)
    {
      SoftDentRawImports14_2.SDInteropSaveDentist(xml);
    }

    public IntPtr SDInteropSaveNewDentist(string xml)
    {
      return SoftDentRawImports14_2.SDInteropSaveNewDentist(xml);
    }

    public IntPtr SDInteropGetReferringDoctor(string PMSRecordID)
    {
      return SoftDentRawImports14_2.SDInteropGetReferringDoctor(PMSRecordID);
    }

    public IntPtr SDInteropGetReferringDoctorList()
    {
      return SoftDentRawImports14_2.SDInteropGetReferringDoctorList();
    }

    public void SDInteropSaveReferringDoctor(string xml)
    {
      SoftDentRawImports14_2.SDInteropSaveReferringDoctor(xml);
    }

    public IntPtr SDInteropSaveNewReferringDoctor(string xml)
    {
      return SoftDentRawImports14_2.SDInteropSaveNewReferringDoctor(xml);
    }

    public IntPtr SDInteropGetADACodeList()
    {
      return SoftDentRawImports14_2.SDInteropGetADACodeList();
    }

    public int SDInteropArchiveEForm(
      string patientID,
      string eformFilePath,
      string archiveDate,
      string archiveTime,
      string description)
    {
      return SoftDentRawImports14_2.SDInteropArchiveEForm(patientID, eformFilePath, archiveDate, archiveTime, description);
    }

    public IntPtr SDInteropGetImage(string imageRecordID)
    {
      return SoftDentRawImports14_2.SDInteropGetImage(imageRecordID);
    }

    public IntPtr SDInteropPearlGetPatientList()
    {
      return SoftDentRawImports14_2.SDInteropPearlGetPatientList();
    }

    public IntPtr SDInteropPearlGetInitialPatientList()
    {
      return SoftDentRawImports14_2.SDInteropPearlGetInitialPatientList();
    }

    public IntPtr SDInteropPearlGetFirstPatientCount(int count, string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetFirstPatientCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlGetNextPatientCount(int count, string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetNextPatientCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlQueryPatientList(string query)
    {
      return SoftDentRawImports14_2.SDInteropPearlQueryPatientList(query);
    }

    public IntPtr SDInteropPearlGetPatient(string recordID)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetPatient(recordID);
    }

    public IntPtr SDInteropPearlGetPatientDetail(string recordID)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetPatientDetail(recordID);
    }

    public IntPtr SDInteropPearlGetPatientClinicalProfile(string recordID)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetPatientClinicalProfile(recordID);
    }

    public IntPtr SDInteropPearlGetPatientAlerts(string recordID)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetPatientAlerts(recordID);
    }

    public IntPtr SDInteropPearlGetPatientMedication(string recordID)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetPatientMedication(recordID);
    }

    public IntPtr SDInteropPearlGetMoreAppointments(string recordID, int count)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetMoreAppointments(recordID, count);
    }

    public IntPtr SDInteropPearlGetFirstPatientClinicalImages(
      string recordID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetFirstPatientClinicalImages(recordID, count, deviceKey);
    }

    public IntPtr SDInteropPearlGetNextPatientClinicalImages(
      string recordID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetNextPatientClinicalImages(recordID, count, deviceKey);
    }

    public IntPtr SDInteropPearlClearDeviceKey()
    {
      return SoftDentRawImports14_2.SDInteropPearlClearDeviceKey();
    }

    public IntPtr SDInteropPearlGetConsultingProviderList()
    {
      return SoftDentRawImports14_2.SDInteropPearlGetConsultingProviderList();
    }

    public IntPtr SDInteropPearlGetFirstConsultingProviderCount(int count, string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetFirstConsultingProviderCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlGetNextConsultingProviderCount(int count, string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetNextConsultingProviderCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlQueryConsultingProviderList(string query)
    {
      return SoftDentRawImports14_2.SDInteropPearlQueryConsultingProviderList(query);
    }

    public IntPtr SDInteropPearlGetConsultingProvider(string recordID)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetConsultingProvider(recordID);
    }

    public IntPtr SDInteropPearlGetConsultingProviderProfile(string recordID)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetConsultingProviderProfile(recordID);
    }

    public IntPtr SDInteropPearlGetConsultingProviderDetails(string recordID)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetConsultingProviderDetails(recordID);
    }

    public IntPtr SDInteropPearlGetFinancialData(string providerId, int timePeriod)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetFinancialData(providerId, timePeriod);
    }

    public int SDInteropPearlSaveNewPrescription(string patientID, string userID, string xml)
    {
      return SoftDentRawImports14_2.SDInteropPearlSaveNewPrescription(patientID, userID, xml);
    }

    public int SDInteropPearlSavePatientCallDetails(string userID, string xml)
    {
      return SoftDentRawImports14_2.SDInteropPearlSavePatientCallDetails(userID, xml);
    }

    public IntPtr SDInteropPearlGetBlockedSlots(
      string startDate,
      string startTime,
      string endDate,
      string endTime)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetBlockedSlots(startDate, startTime, endDate, endTime);
    }

    public IntPtr SDInteropPearlGetAppointments(
      string providerID,
      string startDate,
      string startTime,
      string endDate,
      string endTime)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetAppointments(providerID, startDate, startTime, endDate, endTime);
    }

    public IntPtr SDInteropPearlGetCallList(string providerID, bool refresh)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetCallList(providerID, refresh);
    }

    public int SDInteropSaveConsultingProviderCallDetails(string userID, string xml)
    {
      return SoftDentRawImports14_2.SDInteropSaveConsultingProviderCallDetails(userID, xml);
    }

    public IntPtr SDInteropPearlLoginUser(string userName, string password)
    {
      return SoftDentRawImports14_2.SDInteropPearlLoginUser(userName, password);
    }

    public IntPtr SDInteropGetResponsiblePartyForPatient(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropGetResponsiblePartyForPatient(patientID);
    }

    public IntPtr SDInteropGetResponsibleParty(string acctID)
    {
      return SoftDentRawImports14_2.SDInteropGetResponsibleParty(acctID);
    }

    public IntPtr SDInteropGetPatientEmployee(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropGetPatientEmployee(patientID);
    }

    public IntPtr SDInteropGetResponsiblePartyEmployee(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropGetResponsiblePartyEmployee(patientID);
    }

    public void SDInteropSaveEmployerForPatient(string patientID, string status, string employer)
    {
      SoftDentRawImports14_2.SDInteropSaveEmployerForPatient(patientID, status, employer);
    }

    public void SDInteropSaveResponsiblePartyForPatient(string patientID, string responsibleParty)
    {
      SoftDentRawImports14_2.SDInteropSaveResponsiblePartyForPatient(patientID, responsibleParty);
    }

    public bool SDInteropPatientHasOpenLabCases(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropPatientHasOpenLabCases(patientID);
    }

    public IntPtr SDInteropGetEServiceFlags()
    {
      return SoftDentRawImports14_2.SDInteropGetEServiceFlags();
    }

    public IntPtr SDInteropGetMissedApptsRevenueAnalysis(string startDate, string endDate)
    {
      return SoftDentRawImports14_2.SDInteropGetMissedApptsRevenueAnalysis(startDate, endDate);
    }

    public int SDInteropPutImage(
      string patientID,
      int imageType,
      int acquisitionRegion,
      string imageFileName,
      string imageFilePath,
      string toothAssociations,
      string acquisitionDate,
      string acquisitionTime)
    {
      return SoftDentRawImports14_2.SDInteropPutImage(patientID, imageType, acquisitionRegion, imageFileName, imageFilePath, toothAssociations, acquisitionDate, acquisitionTime);
    }

    public IntPtr SDInteropGetOperatories()
    {
      return SoftDentRawImports14_2.SDInteropGetOperatories();
    }

    public IntPtr SDInteropPearlGetAppointmentsByOperatory(
      string operatoryID,
      string startDate,
      string startTime,
      string endDate,
      string endTime)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetAppointmentsByOperatory(operatoryID, startDate, startTime, endDate, endTime);
    }

    public IntPtr SDInteropPearlMakeAppointment(
      string patientID,
      string providerID,
      string operatoryID,
      string date,
      string time,
      int duration,
      string note)
    {
      return SoftDentRawImports14_2.SDInteropPearlMakeAppointment(patientID, providerID, operatoryID, date, time, duration, note);
    }

    public int SDInteropPearlMakeBlockedSlot(
      string userID,
      string operatoryID,
      string providerID,
      string date,
      string time,
      int duration,
      string note)
    {
      return SoftDentRawImports14_2.SDInteropPearlMakeBlockedSlot(userID, operatoryID, providerID, date, time, duration, note);
    }

    public IntPtr SDInteropPearlGetNextCallListCount(
      string providerID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetNextCallListCount(providerID, count, deviceKey);
    }

    public IntPtr SDInteropPearlGetFirstCallListCount(
      string providerID,
      int count,
      string deviceKey,
      bool refresh)
    {
      return SoftDentRawImports14_2.SDInteropPearlGetFirstCallListCount(providerID, count, deviceKey, refresh);
    }

    public IntPtr SDInteropGetPatientPerson(string patientID, int index)
    {
      return SoftDentRawImports14_2.SDInteropGetPatientPerson(patientID, index);
    }

    public void SDInteropSavePatientPerson(
      string patientID,
      int index,
      string personXML,
      string planXML,
      string companyXML)
    {
      SoftDentRawImports14_2.SDInteropSavePatientPerson(patientID, index, personXML, planXML, companyXML);
    }

    public void SDInteropCheckForMidnight()
    {
      SoftDentRawImports14_2.SDInteropCheckForMidnight();
    }

    public IntPtr SDInteropGetEFormPathAndName(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropGetEFormPathAndName(patientID);
    }

    public IntPtr SDInteropGetFirstPharmacyListCount(int count, string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropGetFirstPharmacyListCount(count, deviceKey);
    }

    public IntPtr SDInteropGetNextPharmacyListCount(int count, string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropGetNextPharmacyListCount(count, deviceKey);
    }

    public IntPtr SDInteropGetPatientSpouse(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropGetPatientSpouse(patientID);
    }

    public bool SDInteropSavePatientSpouse(string patientID, string spouse)
    {
      return SoftDentRawImports14_2.SDInteropSavePatientSpouse(patientID, spouse);
    }

    public IntPtr SDInteropGetFirstSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropGetFirstSDClinicalNotes(patientID, count, deviceKey);
    }

    public IntPtr SDInteropGetNextSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropGetNextSDClinicalNotes(patientID, count, deviceKey);
    }

    public IntPtr SDInteropGetPreviousSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14_2.SDInteropGetPreviousSDClinicalNotes(patientID, count, deviceKey);
    }

    public IntPtr SDInteropMakeClinicalNote(
      string userID,
      string patientID,
      string providerID,
      string note)
    {
      return SoftDentRawImports14_2.SDInteropMakeClinicalNote(userID, patientID, providerID, note);
    }

    public int SDInteropGetClinicalNoteDateCount(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropGetClinicalNoteDateCount(patientID);
    }

    public IntPtr SDInteropLinkCleardown(string PMSRecordID)
    {
      return SoftDentRawImports14_2.SDInteropLinkCleardown(PMSRecordID);
    }

    public IntPtr SDInteropLinkConfigure(string xmlConfiguration)
    {
      return SoftDentRawImports14_2.SDInteropLinkConfigure(xmlConfiguration);
    }

    public IntPtr SDInteropLinkLookupPatient(string xmlPatient, string xmlSource)
    {
      return SoftDentRawImports14_2.SDInteropLinkLookupPatient(xmlPatient, xmlSource);
    }

    public IntPtr SDInteropLinkAddPatient(string xmlPatient, string xmlSource)
    {
      return SoftDentRawImports14_2.SDInteropLinkAddPatient(xmlPatient, xmlSource);
    }

    public IntPtr SDInteropLinkUpdatePatient(
      string patientID,
      string xmlPatient,
      string xmlSource,
      int option)
    {
      return SoftDentRawImports14_2.SDInteropLinkUpdatePatient(patientID, xmlPatient, xmlSource, option);
    }

    public IntPtr SDInteropLinkLookupGuarantor(string xmlGuarantor, string xmlSource)
    {
      return SoftDentRawImports14_2.SDInteropLinkLookupGuarantor(xmlGuarantor, xmlSource);
    }

    public IntPtr SDInteropLinkAddGuarantor(string xmlGuarantor, string xmlSource)
    {
      return SoftDentRawImports14_2.SDInteropLinkAddGuarantor(xmlGuarantor, xmlSource);
    }

    public IntPtr SDInteropLinkUpdateGuarantor(
      string accountID,
      string xmlGuarantor,
      string xmlSource,
      int option)
    {
      return SoftDentRawImports14_2.SDInteropLinkUpdateGuarantor(accountID, xmlGuarantor, xmlSource, option);
    }

    public IntPtr SDInteropGetPatientMedicalAlert(string patientID)
    {
      return SoftDentRawImports14_2.SDInteropGetPatientMedicalAlert(patientID);
    }

    public bool SDInteropKioskCheckIn(string appointmentID)
    {
      return SoftDentRawImports14_2.SDInteropKioskCheckIn(appointmentID);
    }

    public void SDInteropSetAdapterVersion(int major, int minor, int build, int revision)
    {
      SoftDentRawImports14_2.SDInteropSetAdapterVersion(major, minor, build, revision);
    }
  }
}
