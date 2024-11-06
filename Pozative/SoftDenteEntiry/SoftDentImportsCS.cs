// Decompiled with JetBrains decompiler
// Type: PW.Adapters.SoftDent.Database.Interop.SoftDentImportsCS
// Assembly: RVGMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C5DF1CD-A026-4FA6-83A7-5CF2AF8D7D47
// Assembly location: D:\SoftdentDll\exe\RVGMobile.exe

using System;

namespace Pozative
{
  internal class SoftDentImportsCS : ISoftDentImports
  {
    public string FileName
    {
      get
      {
        return "SDInteropCS.dll";
      }
    }

    public int SDInteropInitializeDB(string path, bool loggingOn, string faircomServer)
    {
      return SoftDentRawImportsCS.SDInteropInitializeDB(path, loggingOn, faircomServer);
    }

    public void SDInteropCloseFiles()
    {
      SoftDentRawImportsCS.SDInteropCloseFiles();
    }

    public void SDInteropSetLogging(bool loggingOn)
    {
      SoftDentRawImportsCS.SDInteropSetLogging(loggingOn);
    }

    public IntPtr SDInteropGetAppointmentsForDay(
      string startDate,
      string startTime,
      string endDate,
      string endTime,
      string bookIds)
    {
      return SoftDentRawImportsCS.SDInteropGetAppointmentsForDay(startDate, startTime, endDate, endTime, bookIds);
    }

    public IntPtr SDInteropGetAppointment(string PMSRecordID)
    {
      return SoftDentRawImportsCS.SDInteropGetAppointment(PMSRecordID);
    }

    public void SDInteropConfirmAppointment(
      string PMSRecordID,
      string Method,
      string Date,
      string Time,
      string Status,
      string Reason)
    {
      SoftDentRawImportsCS.SDInteropConfirmAppointment(PMSRecordID, Method, Date, Time, Status, Reason);
    }

    public IntPtr SDInteropGetOffice(string dateTime)
    {
      return SoftDentRawImportsCS.SDInteropGetOffice(dateTime);
    }

    public void SDInteropSavePatient(string xml)
    {
      SoftDentRawImportsCS.SDInteropSavePatient(xml);
    }

    public void SDInteropSaveAppointment(string xml)
    {
        SoftDentRawImportsCS.SDInteropSaveAppointment(xml);
    }

    public IntPtr SDInteropGetPatient(string recordID)
    {
      return SoftDentRawImportsCS.SDInteropGetPatient(recordID);
    }

    public IntPtr SDInteropCreateRestorativeChartImage(
      string PMSRecordID,
      int view,
      int mode)
    {
      return SoftDentRawImportsCS.SDInteropCreateRestorativeChartImage(PMSRecordID, view, mode);
    }

    public IntPtr SDInteropCreatePerioChartImage(string PMSRecordID, int view, int mode)
    {
      return SoftDentRawImportsCS.SDInteropCreatePerioChartImage(PMSRecordID, view, mode);
    }

    public IntPtr SDInteropGetPatientDentalInsurancePlan(string PMSRecordID, bool primary)
    {
      return SoftDentRawImportsCS.SDInteropGetPatientDentalInsurancePlan(PMSRecordID, primary);
    }

    public IntPtr SDInteropGetPatientDentalInsurance(string PMSRecordID, bool primary)
    {
      return SoftDentRawImportsCS.SDInteropGetPatientDentalInsurance(PMSRecordID, primary);
    }

    public IntPtr SDInteropGetPatientGuarantors(string PMSRecordID)
    {
      return SoftDentRawImportsCS.SDInteropGetPatientGuarantors(PMSRecordID);
    }

    public IntPtr SDInteropGetImages(string PMSRecordID)
    {
      return SoftDentRawImportsCS.SDInteropGetImages(PMSRecordID);
    }

    public int SDInteropSaveInsurance(
      string patientID,
      int nGuarIndex,
      string personXml,
      string planXml,
      string companyXml,
      bool primary)
    {
      return SoftDentRawImportsCS.SDInteropSaveInsurance(patientID, nGuarIndex, personXml, planXml, companyXml, primary);
    }

    public IntPtr SDInteropGetPatientPreferredPharmacy(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropGetPatientPreferredPharmacy(patientID);
    }

    public IntPtr SDInteropGetPatientPortrait(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropGetPatientPortrait(patientID);
    }

    public IntPtr SDInteropGetMedicalHistoryConditions()
    {
      return SoftDentRawImportsCS.SDInteropGetMedicalHistoryConditions();
    }

    public IntPtr SDInteropGetMedicalHistoryAllergies()
    {
      return SoftDentRawImportsCS.SDInteropGetMedicalHistoryAllergies();
    }

    public IntPtr SDInteropGetMostRecentMedicalHistory(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropGetMostRecentMedicalHistory(patientID);
    }

    public void SDInteropSaveMedicalHistory(string patientID, string xml)
    {
      SoftDentRawImportsCS.SDInteropSaveMedicalHistory(patientID, xml);
    }

    public IntPtr SDInteropGetInsuranceCompany(string PMSRecordID)
    {
      return SoftDentRawImportsCS.SDInteropGetInsuranceCompany(PMSRecordID);
    }

    public IntPtr SDInteropGetInsuranceCompanyList()
    {
      return SoftDentRawImportsCS.SDInteropGetInsuranceCompanyList();
    }

    public void SDInteropSaveInsuranceCompany(string xml)
    {
      SoftDentRawImportsCS.SDInteropSaveInsuranceCompany(xml);
    }

    public IntPtr SDInteropSaveNewInsuranceCompany(string xml)
    {
      return SoftDentRawImportsCS.SDInteropSaveNewInsuranceCompany(xml);
    }

    public IntPtr SDInteropGetInsurancePlan(string PMSRecordID)
    {
      return SoftDentRawImportsCS.SDInteropGetInsurancePlan(PMSRecordID);
    }

    public IntPtr SDInteropGetInsurancePlanList()
    {
      return SoftDentRawImportsCS.SDInteropGetInsurancePlanList();
    }

    public IntPtr SDInteropGetEmployer(string PMSRecordID)
    {
      return SoftDentRawImportsCS.SDInteropGetEmployer(PMSRecordID);
    }

    public IntPtr SDInteropGetEmployerList()
    {
      return SoftDentRawImportsCS.SDInteropGetEmployerList();
    }

    public void SDInteropSaveEmployer(string xml)
    {
      SoftDentRawImportsCS.SDInteropSaveEmployer(xml);
    }

    public IntPtr SDInteropSaveNewEmployer(string xml)
    {
      return SoftDentRawImportsCS.SDInteropSaveNewEmployer(xml);
    }

    public IntPtr SDInteropGetPharmacy(string PMSRecordID)
    {
      return SoftDentRawImportsCS.SDInteropGetPharmacy(PMSRecordID);
    }

    public IntPtr SDInteropGetPharmacyList()
    {
      return SoftDentRawImportsCS.SDInteropGetPharmacyList();
    }

    public void SDInteropSavePharmacy(string xml)
    {
      SoftDentRawImportsCS.SDInteropSavePharmacy(xml);
    }

    public IntPtr SDInteropSaveNewPharmacy(string xml)
    {
      return SoftDentRawImportsCS.SDInteropSaveNewPharmacy(xml);
    }

    public IntPtr SDInteropGetDrugList()
    {
      return SoftDentRawImportsCS.SDInteropGetDrugList();
    }

    public IntPtr SDInteropGetDenist(string PMSRecordID)
    {
      return SoftDentRawImportsCS.SDInteropGetDenist(PMSRecordID);
    }

    public IntPtr SDInteropGetDentistList()
    {
      return SoftDentRawImportsCS.SDInteropGetDentistList();
    }

    public IntPtr GetSpecialty()
    {
        try
        {
            return SoftDentRawImportsCS.GetSpecialty();
        }
        catch (Exception)
        {
            
            throw;
        }
        
    }

    public void SDInteropSaveDentist(string xml)
    {
      SoftDentRawImportsCS.SDInteropSaveDentist(xml);
    }

    public IntPtr SDInteropSaveNewDentist(string xml)
    {
      return SoftDentRawImportsCS.SDInteropSaveNewDentist(xml);
    }

    public IntPtr SDInteropGetReferringDoctor(string PMSRecordID)
    {
      return SoftDentRawImportsCS.SDInteropGetReferringDoctor(PMSRecordID);
    }

    public IntPtr SDInteropGetReferringDoctorList()
    {
      return SoftDentRawImportsCS.SDInteropGetReferringDoctorList();
    }

    public void SDInteropSaveReferringDoctor(string xml)
    {
      SoftDentRawImportsCS.SDInteropSaveReferringDoctor(xml);
    }

    public IntPtr SDInteropSaveNewReferringDoctor(string xml)
    {
      return SoftDentRawImportsCS.SDInteropSaveNewReferringDoctor(xml);
    }

    public IntPtr SDInteropGetADACodeList()
    {
      return SoftDentRawImportsCS.SDInteropGetADACodeList();
    }

    public int SDInteropArchiveEForm(
      string patientID,
      string eformFilePath,
      string archiveDate,
      string archiveTime,
      string description)
    {
      return SoftDentRawImportsCS.SDInteropArchiveEForm(patientID, eformFilePath, archiveDate, archiveTime, description);
    }

    public IntPtr SDInteropGetImage(string imageRecordID)
    {
      return SoftDentRawImportsCS.SDInteropGetImage(imageRecordID);
    }

    public IntPtr SDInteropPearlGetPatientList()
    {
      return SoftDentRawImportsCS.SDInteropPearlGetPatientList();
    }

    public IntPtr SDInteropPearlGetInitialPatientList()
    {
      return SoftDentRawImportsCS.SDInteropPearlGetInitialPatientList();
    }

    public IntPtr SDInteropPearlGetFirstPatientCount(int count, string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetFirstPatientCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlGetNextPatientCount(int count, string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetNextPatientCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlQueryPatientList(string query)
    {
      return SoftDentRawImportsCS.SDInteropPearlQueryPatientList(query);
    }

    public IntPtr SDInteropPearlGetPatient(string recordID)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetPatient(recordID);
    }

    public IntPtr SDInteropPearlGetPatientDetail(string recordID)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetPatientDetail(recordID);
    }

    public IntPtr SDInteropPearlGetPatientClinicalProfile(string recordID)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetPatientClinicalProfile(recordID);
    }

    public IntPtr SDInteropPearlGetPatientAlerts(string recordID)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetPatientAlerts(recordID);
    }

    public IntPtr SDInteropPearlGetPatientMedication(string recordID)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetPatientMedication(recordID);
    }

    public IntPtr SDInteropPearlGetMoreAppointments(string recordID, int count)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetMoreAppointments(recordID, count);
    }

    public IntPtr SDInteropPearlGetFirstPatientClinicalImages(
      string recordID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetFirstPatientClinicalImages(recordID, count, deviceKey);
    }

    public IntPtr SDInteropPearlGetNextPatientClinicalImages(
      string recordID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetNextPatientClinicalImages(recordID, count, deviceKey);
    }

    public IntPtr SDInteropPearlClearDeviceKey()
    {
      return SoftDentRawImportsCS.SDInteropPearlClearDeviceKey();
    }

    public IntPtr SDInteropPearlGetConsultingProviderList()
    {
      return SoftDentRawImportsCS.SDInteropPearlGetConsultingProviderList();
    }

    public IntPtr SDInteropPearlGetFirstConsultingProviderCount(int count, string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetFirstConsultingProviderCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlGetNextConsultingProviderCount(int count, string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetNextConsultingProviderCount(count, deviceKey);
    }

    public IntPtr SDInteropPearlQueryConsultingProviderList(string query)
    {
      return SoftDentRawImportsCS.SDInteropPearlQueryConsultingProviderList(query);
    }

    public IntPtr SDInteropPearlGetConsultingProvider(string recordID)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetConsultingProvider(recordID);
    }

    public IntPtr SDInteropPearlGetConsultingProviderProfile(string recordID)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetConsultingProviderProfile(recordID);
    }

    public IntPtr SDInteropPearlGetConsultingProviderDetails(string recordID)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetConsultingProviderDetails(recordID);
    }

    public IntPtr SDInteropPearlGetFinancialData(string providerId, int timePeriod)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetFinancialData(providerId, timePeriod);
    }

    public int SDInteropPearlSaveNewPrescription(string patientID, string userID, string xml)
    {
      return SoftDentRawImportsCS.SDInteropPearlSaveNewPrescription(patientID, userID, xml);
    }

    public int SDInteropPearlSavePatientCallDetails(string userID, string xml)
    {
      return SoftDentRawImportsCS.SDInteropPearlSavePatientCallDetails(userID, xml);
    }

    public IntPtr SDInteropPearlGetBlockedSlots(
      string startDate,
      string startTime,
      string endDate,
      string endTime)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetBlockedSlots(startDate, startTime, endDate, endTime);
    }

    public IntPtr SDInteropPearlGetAppointments(
      string providerID,
      string startDate,
      string startTime,
      string endDate,
      string endTime)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetAppointments(providerID, startDate, startTime, endDate, endTime);
    }

    public IntPtr SDInteropPearlGetCallList(string providerID, bool refresh)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetCallList(providerID, refresh);
    }

    public int SDInteropSaveConsultingProviderCallDetails(string userID, string xml)
    {
      return SoftDentRawImportsCS.SDInteropSaveConsultingProviderCallDetails(userID, xml);
    }

    public IntPtr SDInteropPearlLoginUser(string userName, string password)
    {
      return SoftDentRawImportsCS.SDInteropPearlLoginUser(userName, password);
    }

    public IntPtr SDInteropGetResponsiblePartyForPatient(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropGetResponsiblePartyForPatient(patientID);
    }

    public IntPtr SDInteropGetResponsibleParty(string acctID)
    {
      return SoftDentRawImportsCS.SDInteropGetResponsibleParty(acctID);
    }

    public IntPtr SDInteropGetPatientEmployee(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropGetPatientEmployee(patientID);
    }

    public IntPtr SDInteropGetResponsiblePartyEmployee(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropGetResponsiblePartyEmployee(patientID);
    }

    public void SDInteropSaveEmployerForPatient(string patientID, string status, string employer)
    {
      SoftDentRawImportsCS.SDInteropSaveEmployerForPatient(patientID, status, employer);
    }

    public void SDInteropSaveResponsiblePartyForPatient(string patientID, string responsibleParty)
    {
      SoftDentRawImportsCS.SDInteropSaveResponsiblePartyForPatient(patientID, responsibleParty);
    }

    public bool SDInteropPatientHasOpenLabCases(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropPatientHasOpenLabCases(patientID);
    }

    public IntPtr SDInteropGetEServiceFlags()
    {
      return SoftDentRawImportsCS.SDInteropGetEServiceFlags();
    }

    public IntPtr SDInteropGetMissedApptsRevenueAnalysis(string startDate, string endDate)
    {
      return SoftDentRawImportsCS.SDInteropGetMissedApptsRevenueAnalysis(startDate, endDate);
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
      return SoftDentRawImportsCS.SDInteropPutImage(patientID, imageType, acquisitionRegion, imageFileName, imageFilePath, toothAssociations, acquisitionDate, acquisitionTime);
    }

    public IntPtr SDInteropGetOperatories()
    {
      return SoftDentRawImportsCS.SDInteropGetOperatories();
    }

    public IntPtr SDInteropPearlGetAppointmentsByOperatory(
      string operatoryID,
      string startDate,
      string startTime,
      string endDate,
      string endTime)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetAppointmentsByOperatory(operatoryID, startDate, startTime, endDate, endTime);
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
      return SoftDentRawImportsCS.SDInteropPearlMakeAppointment(patientID, providerID, operatoryID, date, time, duration, note);
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
      return SoftDentRawImportsCS.SDInteropPearlMakeBlockedSlot(userID, operatoryID, providerID, date, time, duration, note);
    }

    public IntPtr SDInteropPearlGetNextCallListCount(
      string providerID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetNextCallListCount(providerID, count, deviceKey);
    }

    public IntPtr SDInteropPearlGetFirstCallListCount(
      string providerID,
      int count,
      string deviceKey,
      bool refresh)
    {
      return SoftDentRawImportsCS.SDInteropPearlGetFirstCallListCount(providerID, count, deviceKey, refresh);
    }

    public IntPtr SDInteropGetPatientPerson(string patientID, int index)
    {
      return SoftDentRawImportsCS.SDInteropGetPatientPerson(patientID, index);
    }

    public void SDInteropSavePatientPerson(
      string patientID,
      int index,
      string personXML,
      string planXML,
      string companyXML)
    {
      SoftDentRawImportsCS.SDInteropSavePatientPerson(patientID, index, personXML, planXML, companyXML);
    }

    public void SDInteropCheckForMidnight()
    {
      SoftDentRawImportsCS.SDInteropCheckForMidnight();
    }

    public IntPtr SDInteropGetEFormPathAndName(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropGetEFormPathAndName(patientID);
    }

    public IntPtr SDInteropGetFirstPharmacyListCount(int count, string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropGetFirstPharmacyListCount(count, deviceKey);
    }

    public IntPtr SDInteropGetNextPharmacyListCount(int count, string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropGetNextPharmacyListCount(count, deviceKey);
    }

    public IntPtr SDInteropGetPatientSpouse(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropGetPatientSpouse(patientID);
    }

    public bool SDInteropSavePatientSpouse(string patientID, string spouse)
    {
      return SoftDentRawImportsCS.SDInteropSavePatientSpouse(patientID, spouse);
    }

    public IntPtr SDInteropGetFirstSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropGetFirstSDClinicalNotes(patientID, count, deviceKey);
    }

    public IntPtr SDInteropGetNextSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropGetNextSDClinicalNotes(patientID, count, deviceKey);
    }

    public IntPtr SDInteropGetPreviousSDClinicalNotes(
      string patientID,
      int count,
      string deviceKey)
    {
      return SoftDentRawImportsCS.SDInteropGetPreviousSDClinicalNotes(patientID, count, deviceKey);
    }

    public IntPtr SDInteropMakeClinicalNote(
      string userID,
      string patientID,
      string providerID,
      string note)
    {
      return SoftDentRawImportsCS.SDInteropMakeClinicalNote(userID, patientID, providerID, note);
    }

    public int SDInteropGetClinicalNoteDateCount(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropGetClinicalNoteDateCount(patientID);
    }

    public IntPtr SDInteropLinkCleardown(string PMSRecordID)
    {
      return SoftDentRawImportsCS.SDInteropLinkCleardown(PMSRecordID);
    }

    public IntPtr SDInteropLinkConfigure(string xmlConfiguration)
    {
      return SoftDentRawImportsCS.SDInteropLinkConfigure(xmlConfiguration);
    }

    public IntPtr SDInteropLinkLookupPatient(string xmlPatient, string xmlSource)
    {
      return SoftDentRawImportsCS.SDInteropLinkLookupPatient(xmlPatient, xmlSource);
    }

    public IntPtr SDInteropLinkAddPatient(string xmlPatient, string xmlSource)
    {
      return SoftDentRawImportsCS.SDInteropLinkAddPatient(xmlPatient, xmlSource);
    }

    public IntPtr SDInteropLinkUpdatePatient(
      string patientID,
      string xmlPatient,
      string xmlSource,
      int option)
    {
      return SoftDentRawImportsCS.SDInteropLinkUpdatePatient(patientID, xmlPatient, xmlSource, option);
    }

    public IntPtr SDInteropLinkLookupGuarantor(string xmlGuarantor, string xmlSource)
    {
      return SoftDentRawImportsCS.SDInteropLinkLookupGuarantor(xmlGuarantor, xmlSource);
    }

    public IntPtr SDInteropLinkAddGuarantor(string xmlGuarantor, string xmlSource)
    {
      return SoftDentRawImportsCS.SDInteropLinkAddGuarantor(xmlGuarantor, xmlSource);
    }

    public IntPtr SDInteropLinkUpdateGuarantor(
      string accountID,
      string xmlGuarantor,
      string xmlSource,
      int option)
    {
      return SoftDentRawImportsCS.SDInteropLinkUpdateGuarantor(accountID, xmlGuarantor, xmlSource, option);
    }

    public IntPtr SDInteropGetPatientMedicalAlert(string patientID)
    {
      return SoftDentRawImportsCS.SDInteropGetPatientMedicalAlert(patientID);
    }

    public bool SDInteropKioskCheckIn(string appointmentID)
    {
      return SoftDentRawImportsCS.SDInteropKioskCheckIn(appointmentID);
    }

    public void SDInteropSetAdapterVersion(int major, int minor, int build, int revision)
    {
      SoftDentRawImportsCS.SDInteropSetAdapterVersion(major, minor, build, revision);
    }
  }
}
