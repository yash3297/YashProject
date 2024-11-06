// Decompiled with JetBrains decompiler
// Type: PW.Adapters.SoftDent.Database.Interop.SoftDentImports14CS
// Assembly: RVGMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C5DF1CD-A026-4FA6-83A7-5CF2AF8D7D47
// Assembly location: D:\SoftdentDll\exe\RVGMobile.exe

using System;

namespace Pozative
{
  internal class SoftDentImports14CS : ISoftDentImports
  {
    public string FileName
    {
      get
      {
        return "SDInterop_14CS.dll";
      }
    }

    public int SDInteropInitializeDB(string path, bool loggingOn, string faircomServer)
    {
      return SoftDentRawImports14CS.SDInteropInitializeDB(path, loggingOn, faircomServer);
    }

    public void SDInteropCloseFiles()
    {
      SoftDentRawImports14CS.SDInteropCloseFiles();
    }

    public void SDInteropSetLogging(bool loggingOn)
    {
      SoftDentRawImports14CS.SDInteropSetLogging(loggingOn);
    }

    public IntPtr SDInteropGetAppointmentsForDay(
      string startDate,
      string startTime,
      string endDate,
      string endTime,
      string bookIds)
    {
      return SoftDentRawImports14CS.SDInteropGetAppointmentsForDay(startDate, startTime, endDate, endTime, bookIds);
    }

    public IntPtr SDInteropGetAppointment(string PMSRecordID)
    {
      return SoftDentRawImports14CS.SDInteropGetAppointment(PMSRecordID);
    }

    public void SDInteropConfirmAppointment(
      string PMSRecordID,
      string Method,
      string Date,
      string Time,
      string Status,
      string Reason)
    {
      SoftDentRawImports14CS.SDInteropConfirmAppointment(PMSRecordID, Method, Date, Time, Status, Reason);
    }

    public IntPtr SDInteropGetOffice(string dateTime)
    {
      return SoftDentRawImports14CS.SDInteropGetOffice(dateTime);
    }

    public void SDInteropSavePatient(string xml)
    {
      SoftDentRawImports14CS.SDInteropSavePatient(xml);
    }

    public void SDInteropSaveAppointment(string xml)
    {
        SoftDentRawImports14CS.SDInteropSaveAppointment(xml);
    }

    public IntPtr SDInteropGetPatient(string recordID)
    {
      return SoftDentRawImports14CS.SDInteropGetPatient(recordID);
    }

    public IntPtr SDInteropCreateRestorativeChartImage(
      string PMSRecordID,
      int view,
      int mode)
    {
      return SoftDentRawImports14CS.SDInteropCreateRestorativeChartImage(PMSRecordID, view, mode);
    }

    public IntPtr SDInteropCreatePerioChartImage(string PMSRecordID, int view, int mode)
    {
      return SoftDentRawImports14CS.SDInteropCreatePerioChartImage(PMSRecordID, view, mode);
    }

    public IntPtr SDInteropGetPatientDentalInsurancePlan(string PMSRecordID, bool primary)
    {
      return SoftDentRawImports14CS.SDInteropGetPatientDentalInsurancePlan(PMSRecordID, primary);
    }

    public IntPtr SDInteropGetPatientDentalInsurance(string PMSRecordID, bool primary)
    {
      return SoftDentRawImports14CS.SDInteropGetPatientDentalInsurance(PMSRecordID, primary);
    }

    public IntPtr SDInteropGetPatientGuarantors(string PMSRecordID)
    {
      return SoftDentRawImports14CS.SDInteropGetPatientGuarantors(PMSRecordID);
    }

    public IntPtr SDInteropGetImages(string PMSRecordID)
    {
      return SoftDentRawImports14CS.SDInteropGetImages(PMSRecordID);
    }

    public int SDInteropSaveInsurance(
      string patientID,
      int nGuarIndex,
      string personXml,
      string planXml,
      string companyXml,
      bool primary)
    {
      return SoftDentRawImports14CS.SDInteropSaveInsurance(patientID, nGuarIndex, personXml, planXml, companyXml, primary);
    }

    public IntPtr SDInteropGetPatientPreferredPharmacy(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropGetPatientPreferredPharmacy(patientID);
    }

    public IntPtr SDInteropGetPatientPortrait(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropGetPatientPortrait(patientID);
    }

    public IntPtr SDInteropGetMedicalHistoryConditions()
    {
      return SoftDentRawImports14CS.SDInteropGetMedicalHistoryConditions();
    }

    public IntPtr SDInteropGetMedicalHistoryAllergies()
    {
      return SoftDentRawImports14CS.SDInteropGetMedicalHistoryAllergies();
    }

    public IntPtr SDInteropGetMostRecentMedicalHistory(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropGetMostRecentMedicalHistory(patientID);
    }

    public void SDInteropSaveMedicalHistory(string patientID, string xml)
    {
      SoftDentRawImports14CS.SDInteropSaveMedicalHistory(patientID, xml);
    }

    public IntPtr SDInteropGetInsuranceCompany(string PMSRecordID)
    {
      return SoftDentRawImports14CS.SDInteropGetInsuranceCompany(PMSRecordID);
    }

    public IntPtr SDInteropGetInsuranceCompanyList()
    {
      return SoftDentRawImports14CS.SDInteropGetInsuranceCompanyList();
    }

    public void SDInteropSaveInsuranceCompany(string xml)
    {
      SoftDentRawImports14CS.SDInteropSaveInsuranceCompany(xml);
    }

    public IntPtr SDInteropSaveNewInsuranceCompany(string xml)
    {
      return SoftDentRawImports14CS.SDInteropSaveNewInsuranceCompany(xml);
    }

    public IntPtr SDInteropGetInsurancePlan(string PMSRecordID)
    {
      return SoftDentRawImports14CS.SDInteropGetInsurancePlan(PMSRecordID);
    }

    public IntPtr SDInteropGetInsurancePlanList()
    {
      return SoftDentRawImports14CS.SDInteropGetInsurancePlanList();
    }

    public IntPtr SDInteropGetEmployer(string PMSRecordID)
    {
      return SoftDentRawImports14CS.SDInteropGetEmployer(PMSRecordID);
    }

    public IntPtr SDInteropGetEmployerList()
    {
      return SoftDentRawImports14CS.SDInteropGetEmployerList();
    }

    public void SDInteropSaveEmployer(string xml)
    {
      SoftDentRawImports14CS.SDInteropSaveEmployer(xml);
    }

    public IntPtr SDInteropSaveNewEmployer(string xml)
    {
      return SoftDentRawImports14CS.SDInteropSaveNewEmployer(xml);
    }

    public IntPtr SDInteropGetPharmacy(string PMSRecordID)
    {
      return SoftDentRawImports14CS.SDInteropGetPharmacy(PMSRecordID);
    }

    public IntPtr SDInteropGetPharmacyList()
    {
      return SoftDentRawImports14CS.SDInteropGetPharmacyList();
    }

    public void SDInteropSavePharmacy(string xml)
    {
      SoftDentRawImports14CS.SDInteropSavePharmacy(xml);
    }

    public IntPtr SDInteropSaveNewPharmacy(string xml)
    {
      return SoftDentRawImports14CS.SDInteropSaveNewPharmacy(xml);
    }

    public IntPtr SDInteropGetDrugList()
    {
      return SoftDentRawImports14CS.SDInteropGetDrugList();
    }

    public IntPtr SDInteropGetDenist(string PMSRecordID)
    {
      return SoftDentRawImports14CS.SDInteropGetDenist(PMSRecordID);
    }

    public IntPtr SDInteropGetDentistList()
    {
      return SoftDentRawImports14CS.SDInteropGetDentistList();
    }

    public IntPtr GetSpecialty()
    {
        try
        {
            return SoftDentRawImports14CS.GetSpecialty();
        }
        catch (Exception)
        {
            
            throw;
        }
       
    }

    public void SDInteropSaveDentist(string xml)
    {
      SoftDentRawImports14CS.SDInteropSaveDentist(xml);
    }

    public IntPtr SDInteropSaveNewDentist(string xml)
    {
      return SoftDentRawImports14CS.SDInteropSaveNewDentist(xml);
    }

    public IntPtr SDInteropGetReferringDoctor(string PMSRecordID)
    {
      return SoftDentRawImports14CS.SDInteropGetReferringDoctor(PMSRecordID);
    }

    public IntPtr SDInteropGetReferringDoctorList()
    {
      return SoftDentRawImports14CS.SDInteropGetReferringDoctorList();
    }

    public void SDInteropSaveReferringDoctor(string xml)
    {
      SoftDentRawImports14CS.SDInteropSaveReferringDoctor(xml);
    }

    public IntPtr SDInteropSaveNewReferringDoctor(string xml)
    {
      return SoftDentRawImports14CS.SDInteropSaveNewReferringDoctor(xml);
    }

    public IntPtr SDInteropGetADACodeList()
    {
      return SoftDentRawImports14CS.SDInteropGetADACodeList();
    }

    public int SDInteropArchiveEForm(
      string patientID,
      string eformFilePath,
      string archiveDate,
      string archiveTime,
      string description)
    {
      return SoftDentRawImports14CS.SDInteropArchiveEForm(patientID, eformFilePath, archiveDate, archiveTime, description);
    }

    public IntPtr SDInteropGetImage(string imageRecordID)
    {
      return SoftDentRawImports14CS.SDInteropGetImage(imageRecordID);
    }

    public IntPtr SDInteropPearlGetPatientList()
    {
      return SoftDentRawImports14CS.SDInteropPearlGetPatientList();
    }

    public IntPtr SDInteropPearlGetInitialPatientList()
    {
      return SoftDentRawImports14CS.SDInteropPearlGetInitialPatientList();
    }

    public IntPtr SDInteropPearlGetFirstPatientCount(int count, string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetFirstPatientCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlGetNextPatientCount(int count, string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetNextPatientCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlQueryPatientList(string query)
    {
      return SoftDentRawImports14CS.SDInteropPearlQueryPatientList(query);
    }

    public IntPtr SDInteropPearlGetPatient(string recordID)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetPatient(recordID);
    }

    public IntPtr SDInteropPearlGetPatientDetail(string recordID)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetPatientDetail(recordID);
    }

    public IntPtr SDInteropPearlGetPatientClinicalProfile(string recordID)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetPatientClinicalProfile(recordID);
    }

    public IntPtr SDInteropPearlGetPatientAlerts(string recordID)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetPatientAlerts(recordID);
    }

    public IntPtr SDInteropPearlGetPatientMedication(string recordID)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetPatientMedication(recordID);
    }

    public IntPtr SDInteropPearlGetMoreAppointments(string recordID, int count)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetMoreAppointments(recordID, count);
    }

    public IntPtr SDInteropPearlGetFirstPatientClinicalImages(
      string recordID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetFirstPatientClinicalImages(recordID, count, deviceKey);
    }

    public IntPtr SDInteropPearlGetNextPatientClinicalImages(
      string recordID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetNextPatientClinicalImages(recordID, count, deviceKey);
    }

    public IntPtr SDInteropPearlClearDeviceKey()
    {
      return SoftDentRawImports14CS.SDInteropPearlClearDeviceKey();
    }

    public IntPtr SDInteropPearlGetConsultingProviderList()
    {
      return SoftDentRawImports14CS.SDInteropPearlGetConsultingProviderList();
    }

    public IntPtr SDInteropPearlGetFirstConsultingProviderCount(int count, string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetFirstConsultingProviderCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlGetNextConsultingProviderCount(int count, string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetNextConsultingProviderCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlQueryConsultingProviderList(string query)
    {
      return SoftDentRawImports14CS.SDInteropPearlQueryConsultingProviderList(query);
    }

    public IntPtr SDInteropPearlGetConsultingProvider(string recordID)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetConsultingProvider(recordID);
    }

    public IntPtr SDInteropPearlGetConsultingProviderProfile(string recordID)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetConsultingProviderProfile(recordID);
    }

    public IntPtr SDInteropPearlGetConsultingProviderDetails(string recordID)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetConsultingProviderDetails(recordID);
    }

    public IntPtr SDInteropPearlGetFinancialData(string providerId, int timePeriod)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetFinancialData(providerId, timePeriod);
    }

    public int SDInteropPearlSaveNewPrescription(string patientID, string userID, string xml)
    {
      return SoftDentRawImports14CS.SDInteropPearlSaveNewPrescription(patientID, userID, xml);
    }

    public int SDInteropPearlSavePatientCallDetails(string userID, string xml)
    {
      return SoftDentRawImports14CS.SDInteropPearlSavePatientCallDetails(userID, xml);
    }

    public IntPtr SDInteropPearlGetBlockedSlots(
      string startDate,
      string startTime,
      string endDate,
      string endTime)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetBlockedSlots(startDate, startTime, endDate, endTime);
    }

    public IntPtr SDInteropPearlGetAppointments(
      string providerID,
      string startDate,
      string startTime,
      string endDate,
      string endTime)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetAppointments(providerID, startDate, startTime, endDate, endTime);
    }

    public IntPtr SDInteropPearlGetCallList(string providerID, bool refresh)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetCallList(providerID, refresh);
    }

    public int SDInteropSaveConsultingProviderCallDetails(string userID, string xml)
    {
      return SoftDentRawImports14CS.SDInteropSaveConsultingProviderCallDetails(userID, xml);
    }

    public IntPtr SDInteropPearlLoginUser(string userName, string password)
    {
      return SoftDentRawImports14CS.SDInteropPearlLoginUser(userName, password);
    }

    public IntPtr SDInteropGetResponsiblePartyForPatient(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropGetResponsiblePartyForPatient(patientID);
    }

    public IntPtr SDInteropGetResponsibleParty(string acctID)
    {
      return SoftDentRawImports14CS.SDInteropGetResponsibleParty(acctID);
    }

    public IntPtr SDInteropGetPatientEmployee(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropGetPatientEmployee(patientID);
    }

    public IntPtr SDInteropGetResponsiblePartyEmployee(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropGetResponsiblePartyEmployee(patientID);
    }

    public void SDInteropSaveEmployerForPatient(string patientID, string status, string employer)
    {
      SoftDentRawImports14CS.SDInteropSaveEmployerForPatient(patientID, status, employer);
    }

    public void SDInteropSaveResponsiblePartyForPatient(string patientID, string responsibleParty)
    {
      SoftDentRawImports14CS.SDInteropSaveResponsiblePartyForPatient(patientID, responsibleParty);
    }

    public bool SDInteropPatientHasOpenLabCases(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropPatientHasOpenLabCases(patientID);
    }

    public IntPtr SDInteropGetEServiceFlags()
    {
      return SoftDentRawImports14CS.SDInteropGetEServiceFlags();
    }

    public IntPtr SDInteropGetMissedApptsRevenueAnalysis(string startDate, string endDate)
    {
      return SoftDentRawImports14CS.SDInteropGetMissedApptsRevenueAnalysis(startDate, endDate);
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
      return SoftDentRawImports14CS.SDInteropPutImage(patientID, imageType, acquisitionRegion, imageFileName, imageFilePath, toothAssociations, acquisitionDate, acquisitionTime);
    }

    public IntPtr SDInteropGetOperatories()
    {
      return SoftDentRawImports14CS.SDInteropGetOperatories();
    }

    public IntPtr SDInteropPearlGetAppointmentsByOperatory(
      string operatoryID,
      string startDate,
      string startTime,
      string endDate,
      string endTime)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetAppointmentsByOperatory(operatoryID, startDate, startTime, endDate, endTime);
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
      return SoftDentRawImports14CS.SDInteropPearlMakeAppointment(patientID, providerID, operatoryID, date, time, duration, note);
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
      return SoftDentRawImports14CS.SDInteropPearlMakeBlockedSlot(userID, operatoryID, providerID, date, time, duration, note);
    }

    public IntPtr SDInteropPearlGetNextCallListCount(
      string providerID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetNextCallListCount(providerID, count, deviceKey);
    }

    public IntPtr SDInteropPearlGetFirstCallListCount(
      string providerID,
      int count,
      string deviceKey,
      bool refresh)
    {
      return SoftDentRawImports14CS.SDInteropPearlGetFirstCallListCount(providerID, count, deviceKey, refresh);
    }

    public IntPtr SDInteropGetPatientPerson(string patientID, int index)
    {
      return SoftDentRawImports14CS.SDInteropGetPatientPerson(patientID, index);
    }

    public void SDInteropSavePatientPerson(
      string patientID,
      int index,
      string personXML,
      string planXML,
      string companyXML)
    {
      SoftDentRawImports14CS.SDInteropSavePatientPerson(patientID, index, personXML, planXML, companyXML);
    }

    public void SDInteropCheckForMidnight()
    {
      SoftDentRawImports14CS.SDInteropCheckForMidnight();
    }

    public IntPtr SDInteropGetEFormPathAndName(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropGetEFormPathAndName(patientID);
    }

    public IntPtr SDInteropGetFirstPharmacyListCount(int count, string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropGetFirstPharmacyListCount(count, deviceKey);
    }

    public IntPtr SDInteropGetNextPharmacyListCount(int count, string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropGetNextPharmacyListCount(count, deviceKey);
    }

    public IntPtr SDInteropGetPatientSpouse(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropGetPatientSpouse(patientID);
    }

    public bool SDInteropSavePatientSpouse(string patientID, string spouse)
    {
      return SoftDentRawImports14CS.SDInteropSavePatientSpouse(patientID, spouse);
    }

    public IntPtr SDInteropGetFirstSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropGetFirstSDClinicalNotes(patientID, count, deviceKey);
    }

    public IntPtr SDInteropGetNextSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropGetNextSDClinicalNotes(patientID, count, deviceKey);
    }

    public IntPtr SDInteropGetPreviousSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImports14CS.SDInteropGetPreviousSDClinicalNotes(patientID, count, deviceKey);
    }

    public IntPtr SDInteropMakeClinicalNote(
      string userID,
      string patientID,
      string providerID,
      string note)
    {
      return SoftDentRawImports14CS.SDInteropMakeClinicalNote(userID, patientID, providerID, note);
    }

    public int SDInteropGetClinicalNoteDateCount(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropGetClinicalNoteDateCount(patientID);
    }

    public IntPtr SDInteropLinkCleardown(string PMSRecordID)
    {
      return SoftDentRawImports14CS.SDInteropLinkCleardown(PMSRecordID);
    }

    public IntPtr SDInteropLinkConfigure(string xmlConfiguration)
    {
      return SoftDentRawImports14CS.SDInteropLinkConfigure(xmlConfiguration);
    }

    public IntPtr SDInteropLinkLookupPatient(string xmlPatient, string xmlSource)
    {
      return SoftDentRawImports14CS.SDInteropLinkLookupPatient(xmlPatient, xmlSource);
    }

    public IntPtr SDInteropLinkAddPatient(string xmlPatient, string xmlSource)
    {
      return SoftDentRawImports14CS.SDInteropLinkAddPatient(xmlPatient, xmlSource);
    }

    public IntPtr SDInteropLinkUpdatePatient(
      string patientID,
      string xmlPatient,
      string xmlSource,
      int option)
    {
      return SoftDentRawImports14CS.SDInteropLinkUpdatePatient(patientID, xmlPatient, xmlSource, option);
    }

    public IntPtr SDInteropLinkLookupGuarantor(string xmlGuarantor, string xmlSource)
    {
      return SoftDentRawImports14CS.SDInteropLinkLookupGuarantor(xmlGuarantor, xmlSource);
    }

    public IntPtr SDInteropLinkAddGuarantor(string xmlGuarantor, string xmlSource)
    {
      return SoftDentRawImports14CS.SDInteropLinkAddGuarantor(xmlGuarantor, xmlSource);
    }

    public IntPtr SDInteropLinkUpdateGuarantor(
      string accountID,
      string xmlGuarantor,
      string xmlSource,
      int option)
    {
      return SoftDentRawImports14CS.SDInteropLinkUpdateGuarantor(accountID, xmlGuarantor, xmlSource, option);
    }

    public IntPtr SDInteropGetPatientMedicalAlert(string patientID)
    {
      return SoftDentRawImports14CS.SDInteropGetPatientMedicalAlert(patientID);
    }

    public bool SDInteropKioskCheckIn(string appointmentID)
    {
      return SoftDentRawImports14CS.SDInteropKioskCheckIn(appointmentID);
    }

    public void SDInteropSetAdapterVersion(int major, int minor, int build, int revision)
    {
      SoftDentRawImports14CS.SDInteropSetAdapterVersion(major, minor, build, revision);
    }
  }
}
