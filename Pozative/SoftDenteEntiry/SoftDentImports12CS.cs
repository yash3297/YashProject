using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    internal class SoftDentImports12CS : ISoftDentImports
    {
        public string FileName
        {
            get
            {
                return "SDInterop_12CS.dll";
            }
        }

        public int SDInteropInitializeDB(string path, bool loggingOn, string faircomServer)
        {
            return SoftDentRawImports12CS.SDInteropInitializeDB(path, loggingOn, faircomServer);
        }

        public void SDInteropCloseFiles()
        {
            SoftDentRawImports12CS.SDInteropCloseFiles();
        }

        public void SDInteropSetLogging(bool loggingOn)
        {
            SoftDentRawImports12CS.SDInteropSetLogging(loggingOn);
        }

        public IntPtr SDInteropGetAppointmentsForDay(
          string startDate,
          string startTime,
          string endDate,
          string endTime,
          string bookIds)
        {
            return SoftDentRawImports12CS.SDInteropGetAppointmentsForDay(startDate, startTime, endDate, endTime, bookIds);
        }

        public IntPtr SDInteropGetAppointment(string PMSRecordID)
        {
            return SoftDentRawImports12CS.SDInteropGetAppointment(PMSRecordID);
        }

        public void SDInteropConfirmAppointment(
          string PMSRecordID,
          string Method,
          string Date,
          string Time,
          string Status,
          string Reason)
        {
            SoftDentRawImports12CS.SDInteropConfirmAppointment(PMSRecordID, Method, Date, Time, Status, Reason);
        }

        public IntPtr SDInteropGetOffice(string dateTime)
        {
            return SoftDentRawImports12CS.SDInteropGetOffice(dateTime);
        }

        public void SDInteropSavePatient(string xml)
        {
            SoftDentRawImports12CS.SDInteropSavePatient(xml);
        }

        public void SDInteropSaveAppointment(string xml)
        {
            SoftDentRawImports12CS.SDInteropSaveAppointment(xml);
        }

        public IntPtr SDInteropGetPatient(string recordID)
        {
            return SoftDentRawImports12CS.SDInteropGetPatient(recordID);
        }

        public IntPtr SDInteropCreateRestorativeChartImage(
          string PMSRecordID,
          int view,
          int mode)
        {
            return SoftDentRawImports12CS.SDInteropCreateRestorativeChartImage(PMSRecordID, view, mode);
        }

        public IntPtr SDInteropCreatePerioChartImage(string PMSRecordID, int view, int mode)
        {
            return SoftDentRawImports12CS.SDInteropCreatePerioChartImage(PMSRecordID, view, mode);
        }

        public IntPtr SDInteropGetPatientDentalInsurancePlan(string PMSRecordID, bool primary)
        {
            return SoftDentRawImports12CS.SDInteropGetPatientDentalInsurancePlan(PMSRecordID, primary);
        }

        public IntPtr SDInteropGetPatientDentalInsurance(string PMSRecordID, bool primary)
        {
            return SoftDentRawImports12CS.SDInteropGetPatientDentalInsurance(PMSRecordID, primary);
        }

        public IntPtr SDInteropGetPatientGuarantors(string PMSRecordID)
        {
            return SoftDentRawImports12CS.SDInteropGetPatientGuarantors(PMSRecordID);
        }

        public IntPtr SDInteropGetImages(string PMSRecordID)
        {
            return SoftDentRawImports12CS.SDInteropGetImages(PMSRecordID);
        }

        public int SDInteropSaveInsurance(
          string patientID,
          int nGuarIndex,
          string personXml,
          string planXml,
          string companyXml,
          bool primary)
        {
            return SoftDentRawImports12CS.SDInteropSaveInsurance(patientID, nGuarIndex, personXml, planXml, companyXml, primary);
        }

        public IntPtr SDInteropGetPatientPreferredPharmacy(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropGetPatientPreferredPharmacy(patientID);
        }

        public IntPtr SDInteropGetPatientPortrait(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropGetPatientPortrait(patientID);
        }

        public IntPtr SDInteropGetMedicalHistoryConditions()
        {
            return SoftDentRawImports12CS.SDInteropGetMedicalHistoryConditions();
        }

        public IntPtr SDInteropGetMedicalHistoryAllergies()
        {
            return SoftDentRawImports12CS.SDInteropGetMedicalHistoryAllergies();
        }

        public IntPtr SDInteropGetMostRecentMedicalHistory(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropGetMostRecentMedicalHistory(patientID);
        }

        public void SDInteropSaveMedicalHistory(string patientID, string xml)
        {
            SoftDentRawImports12CS.SDInteropSaveMedicalHistory(patientID, xml);
        }

        public IntPtr SDInteropGetInsuranceCompany(string PMSRecordID)
        {
            return SoftDentRawImports12CS.SDInteropGetInsuranceCompany(PMSRecordID);
        }

        public IntPtr SDInteropGetInsuranceCompanyList()
        {
            return SoftDentRawImports12CS.SDInteropGetInsuranceCompanyList();
        }

        public void SDInteropSaveInsuranceCompany(string xml)
        {
            SoftDentRawImports12CS.SDInteropSaveInsuranceCompany(xml);
        }

        public IntPtr SDInteropSaveNewInsuranceCompany(string xml)
        {
            return SoftDentRawImports12CS.SDInteropSaveNewInsuranceCompany(xml);
        }

        public IntPtr SDInteropGetInsurancePlan(string PMSRecordID)
        {
            return SoftDentRawImports12CS.SDInteropGetInsurancePlan(PMSRecordID);
        }

        public IntPtr SDInteropGetInsurancePlanList()
        {
            return SoftDentRawImports12CS.SDInteropGetInsurancePlanList();
        }

        public IntPtr SDInteropGetEmployer(string PMSRecordID)
        {
            return SoftDentRawImports12CS.SDInteropGetEmployer(PMSRecordID);
        }

        public IntPtr SDInteropGetEmployerList()
        {
            return SoftDentRawImports12CS.SDInteropGetEmployerList();
        }

        public void SDInteropSaveEmployer(string xml)
        {
            SoftDentRawImports12CS.SDInteropSaveEmployer(xml);
        }

        public IntPtr SDInteropSaveNewEmployer(string xml)
        {
            return SoftDentRawImports12CS.SDInteropSaveNewEmployer(xml);
        }

        public IntPtr SDInteropGetPharmacy(string PMSRecordID)
        {
            return SoftDentRawImports12CS.SDInteropGetPharmacy(PMSRecordID);
        }

        public IntPtr SDInteropGetPharmacyList()
        {
            return SoftDentRawImports12CS.SDInteropGetPharmacyList();
        }

        public void SDInteropSavePharmacy(string xml)
        {
            SoftDentRawImports12CS.SDInteropSavePharmacy(xml);
        }

        public IntPtr SDInteropSaveNewPharmacy(string xml)
        {
            return SoftDentRawImports12CS.SDInteropSaveNewPharmacy(xml);
        }

        public IntPtr SDInteropGetDrugList()
        {
            return SoftDentRawImports12CS.SDInteropGetDrugList();
        }

        public IntPtr SDInteropGetDenist(string PMSRecordID)
        {
            return SoftDentRawImports12CS.SDInteropGetDenist(PMSRecordID);
        }

        public IntPtr SDInteropGetDentistList()
        {
            return SoftDentRawImports12CS.SDInteropGetDentistList();
        }

        public IntPtr GetSpecialty()
        {
            try
            {
                return SoftDentRawImports12CS.GetSpecialty();
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public void SDInteropSaveDentist(string xml)
        {
            SoftDentRawImports12CS.SDInteropSaveDentist(xml);
        }

        public IntPtr SDInteropSaveNewDentist(string xml)
        {
            return SoftDentRawImports12CS.SDInteropSaveNewDentist(xml);
        }

        public IntPtr SDInteropGetReferringDoctor(string PMSRecordID)
        {
            return SoftDentRawImports12CS.SDInteropGetReferringDoctor(PMSRecordID);
        }

        public IntPtr SDInteropGetReferringDoctorList()
        {
            return SoftDentRawImports12CS.SDInteropGetReferringDoctorList();
        }

        public void SDInteropSaveReferringDoctor(string xml)
        {
            SoftDentRawImports12CS.SDInteropSaveReferringDoctor(xml);
        }

        public IntPtr SDInteropSaveNewReferringDoctor(string xml)
        {
            return SoftDentRawImports12CS.SDInteropSaveNewReferringDoctor(xml);
        }

        public IntPtr SDInteropGetADACodeList()
        {
            return SoftDentRawImports12CS.SDInteropGetADACodeList();
        }

        public int SDInteropArchiveEForm(
          string patientID,
          string eformFilePath,
          string archiveDate,
          string archiveTime,
          string description)
        {
            return SoftDentRawImports12CS.SDInteropArchiveEForm(patientID, eformFilePath, archiveDate, archiveTime, description);
        }

        public IntPtr SDInteropGetImage(string imageRecordID)
        {
            return SoftDentRawImports12CS.SDInteropGetImage(imageRecordID);
        }

        public IntPtr SDInteropPearlGetPatientList()
        {
            return SoftDentRawImports12CS.SDInteropPearlGetPatientList();
        }

        public IntPtr SDInteropPearlGetInitialPatientList()
        {
            return SoftDentRawImports12CS.SDInteropPearlGetInitialPatientList();
        }

        public IntPtr SDInteropPearlGetFirstPatientCount(int count, string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetFirstPatientCount(count, deviceKey);
        }

        public IntPtr SDInteropPearlGetNextPatientCount(int count, string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetNextPatientCount(count, deviceKey);
        }

        public IntPtr SDInteropPearlQueryPatientList(string query)
        {
            return SoftDentRawImports12CS.SDInteropPearlQueryPatientList(query);
        }

        public IntPtr SDInteropPearlGetPatient(string recordID)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetPatient(recordID);
        }

        public IntPtr SDInteropPearlGetPatientDetail(string recordID)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetPatientDetail(recordID);
        }

        public IntPtr SDInteropPearlGetPatientClinicalProfile(string recordID)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetPatientClinicalProfile(recordID);
        }

        public IntPtr SDInteropPearlGetPatientAlerts(string recordID)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetPatientAlerts(recordID);
        }

        public IntPtr SDInteropPearlGetPatientMedication(string recordID)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetPatientMedication(recordID);
        }

        public IntPtr SDInteropPearlGetMoreAppointments(string recordID, int count)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetMoreAppointments(recordID, count);
        }

        public IntPtr SDInteropPearlGetFirstPatientClinicalImages(
          string recordID,
          int count,
          string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetFirstPatientClinicalImages(recordID, count, deviceKey);
        }

        public IntPtr SDInteropPearlGetNextPatientClinicalImages(
          string recordID,
          int count,
          string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetNextPatientClinicalImages(recordID, count, deviceKey);
        }

        public IntPtr SDInteropPearlClearDeviceKey()
        {
            return SoftDentRawImports12CS.SDInteropPearlClearDeviceKey();
        }

        public IntPtr SDInteropPearlGetConsultingProviderList()
        {
            return SoftDentRawImports12CS.SDInteropPearlGetConsultingProviderList();
        }

        public IntPtr SDInteropPearlGetFirstConsultingProviderCount(int count, string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetFirstConsultingProviderCount(count, deviceKey);
        }

        public IntPtr SDInteropPearlGetNextConsultingProviderCount(int count, string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetNextConsultingProviderCount(count, deviceKey);
        }

        public IntPtr SDInteropPearlQueryConsultingProviderList(string query)
        {
            return SoftDentRawImports12CS.SDInteropPearlQueryConsultingProviderList(query);
        }

        public IntPtr SDInteropPearlGetConsultingProvider(string recordID)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetConsultingProvider(recordID);
        }

        public IntPtr SDInteropPearlGetConsultingProviderProfile(string recordID)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetConsultingProviderProfile(recordID);
        }

        public IntPtr SDInteropPearlGetConsultingProviderDetails(string recordID)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetConsultingProviderDetails(recordID);
        }

        public IntPtr SDInteropPearlGetFinancialData(string providerId, int timePeriod)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetFinancialData(providerId, timePeriod);
        }

        public int SDInteropPearlSaveNewPrescription(string patientID, string userID, string xml)
        {
            return SoftDentRawImports12CS.SDInteropPearlSaveNewPrescription(patientID, userID, xml);
        }

        public int SDInteropPearlSavePatientCallDetails(string userID, string xml)
        {
            return SoftDentRawImports12CS.SDInteropPearlSavePatientCallDetails(userID, xml);
        }

        public IntPtr SDInteropPearlGetBlockedSlots(
          string startDate,
          string startTime,
          string endDate,
          string endTime)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetBlockedSlots(startDate, startTime, endDate, endTime);
        }

        public IntPtr SDInteropPearlGetAppointments(
          string providerID,
          string startDate,
          string startTime,
          string endDate,
          string endTime)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetAppointments(providerID, startDate, startTime, endDate, endTime);
        }

        public IntPtr SDInteropPearlGetCallList(string providerID, bool refresh)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetCallList(providerID, refresh);
        }

        public int SDInteropSaveConsultingProviderCallDetails(string userID, string xml)
        {
            return SoftDentRawImports12CS.SDInteropSaveConsultingProviderCallDetails(userID, xml);
        }

        public IntPtr SDInteropPearlLoginUser(string userName, string password)
        {
            return SoftDentRawImports12CS.SDInteropPearlLoginUser(userName, password);
        }

        public IntPtr SDInteropGetResponsiblePartyForPatient(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropGetResponsiblePartyForPatient(patientID);
        }

        public IntPtr SDInteropGetResponsibleParty(string acctID)
        {
            return SoftDentRawImports12CS.SDInteropGetResponsibleParty(acctID);
        }

        public IntPtr SDInteropGetPatientEmployee(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropGetPatientEmployee(patientID);
        }

        public IntPtr SDInteropGetResponsiblePartyEmployee(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropGetResponsiblePartyEmployee(patientID);
        }

        public void SDInteropSaveEmployerForPatient(string patientID, string status, string employer)
        {
            SoftDentRawImports12CS.SDInteropSaveEmployerForPatient(patientID, status, employer);
        }

        public void SDInteropSaveResponsiblePartyForPatient(string patientID, string responsibleParty)
        {
            SoftDentRawImports12CS.SDInteropSaveResponsiblePartyForPatient(patientID, responsibleParty);
        }

        public bool SDInteropPatientHasOpenLabCases(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropPatientHasOpenLabCases(patientID);
        }

        public IntPtr SDInteropGetEServiceFlags()
        {
            return SoftDentRawImports12CS.SDInteropGetEServiceFlags();
        }

        public IntPtr SDInteropGetMissedApptsRevenueAnalysis(string startDate, string endDate)
        {
            return SoftDentRawImports12CS.SDInteropGetMissedApptsRevenueAnalysis(startDate, endDate);
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
            return SoftDentRawImports12CS.SDInteropPutImage(patientID, imageType, acquisitionRegion, imageFileName, imageFilePath, toothAssociations, acquisitionDate, acquisitionTime);
        }

        public IntPtr SDInteropGetOperatories()
        {
            return SoftDentRawImports12CS.SDInteropGetOperatories();
        }

        public IntPtr SDInteropPearlGetAppointmentsByOperatory(
          string operatoryID,
          string startDate,
          string startTime,
          string endDate,
          string endTime)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetAppointmentsByOperatory(operatoryID, startDate, startTime, endDate, endTime);
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
            return SoftDentRawImports12CS.SDInteropPearlMakeAppointment(patientID, providerID, operatoryID, date, time, duration, note);
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
            return SoftDentRawImports12CS.SDInteropPearlMakeBlockedSlot(userID, operatoryID, providerID, date, time, duration, note);
        }

        public IntPtr SDInteropPearlGetNextCallListCount(
          string providerID,
          int count,
          string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetNextCallListCount(providerID, count, deviceKey);
        }

        public IntPtr SDInteropPearlGetFirstCallListCount(
          string providerID,
          int count,
          string deviceKey,
          bool refresh)
        {
            return SoftDentRawImports12CS.SDInteropPearlGetFirstCallListCount(providerID, count, deviceKey, refresh);
        }

        public IntPtr SDInteropGetPatientPerson(string patientID, int index)
        {
            return SoftDentRawImports12CS.SDInteropGetPatientPerson(patientID, index);
        }

        public void SDInteropSavePatientPerson(
          string patientID,
          int index,
          string personXML,
          string planXML,
          string companyXML)
        {
            SoftDentRawImports12CS.SDInteropSavePatientPerson(patientID, index, personXML, planXML, companyXML);
        }

        public void SDInteropCheckForMidnight()
        {
            SoftDentRawImports12CS.SDInteropCheckForMidnight();
        }

        public IntPtr SDInteropGetEFormPathAndName(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropGetEFormPathAndName(patientID);
        }

        public IntPtr SDInteropGetFirstPharmacyListCount(int count, string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropGetFirstPharmacyListCount(count, deviceKey);
        }

        public IntPtr SDInteropGetNextPharmacyListCount(int count, string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropGetNextPharmacyListCount(count, deviceKey);
        }

        public IntPtr SDInteropGetPatientSpouse(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropGetPatientSpouse(patientID);
        }

        public bool SDInteropSavePatientSpouse(string patientID, string spouse)
        {
            return SoftDentRawImports12CS.SDInteropSavePatientSpouse(patientID, spouse);
        }

        public IntPtr SDInteropGetFirstSDClinicalNotes(
          string patientID,
          int count,
          string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropGetFirstSDClinicalNotes(patientID, count, deviceKey);
        }

        public IntPtr SDInteropGetNextSDClinicalNotes(
          string patientID,
          int count,
          string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropGetNextSDClinicalNotes(patientID, count, deviceKey);
        }

        public IntPtr SDInteropGetPreviousSDClinicalNotes(
          string patientID,
          int count,
          string deviceKey)
        {
            return SoftDentRawImports12CS.SDInteropGetPreviousSDClinicalNotes(patientID, count, deviceKey);
        }

        public IntPtr SDInteropMakeClinicalNote(
          string userID,
          string patientID,
          string providerID,
          string note)
        {
            return SoftDentRawImports12CS.SDInteropMakeClinicalNote(userID, patientID, providerID, note);
        }

        public int SDInteropGetClinicalNoteDateCount(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropGetClinicalNoteDateCount(patientID);
        }

        public IntPtr SDInteropLinkCleardown(string PMSRecordID)
        {
            return SoftDentRawImports12CS.SDInteropLinkCleardown(PMSRecordID);
        }

        public IntPtr SDInteropLinkConfigure(string xmlConfiguration)
        {
            return SoftDentRawImports12CS.SDInteropLinkConfigure(xmlConfiguration);
        }

        public IntPtr SDInteropLinkLookupPatient(string xmlPatient, string xmlSource)
        {
            return SoftDentRawImports12CS.SDInteropLinkLookupPatient(xmlPatient, xmlSource);
        }

        public IntPtr SDInteropLinkAddPatient(string xmlPatient, string xmlSource)
        {
            return SoftDentRawImports12CS.SDInteropLinkAddPatient(xmlPatient, xmlSource);
        }

        public IntPtr SDInteropLinkUpdatePatient(
          string patientID,
          string xmlPatient,
          string xmlSource,
          int option)
        {
            return SoftDentRawImports12CS.SDInteropLinkUpdatePatient(patientID, xmlPatient, xmlSource, option);
        }

        public IntPtr SDInteropLinkLookupGuarantor(string xmlGuarantor, string xmlSource)
        {
            return SoftDentRawImports12CS.SDInteropLinkLookupGuarantor(xmlGuarantor, xmlSource);
        }

        public IntPtr SDInteropLinkAddGuarantor(string xmlGuarantor, string xmlSource)
        {
            return SoftDentRawImports12CS.SDInteropLinkAddGuarantor(xmlGuarantor, xmlSource);
        }

        public IntPtr SDInteropLinkUpdateGuarantor(
          string accountID,
          string xmlGuarantor,
          string xmlSource,
          int option)
        {
            return SoftDentRawImports12CS.SDInteropLinkUpdateGuarantor(accountID, xmlGuarantor, xmlSource, option);
        }

        public IntPtr SDInteropGetPatientMedicalAlert(string patientID)
        {
            return SoftDentRawImports12CS.SDInteropGetPatientMedicalAlert(patientID);
        }

        public bool SDInteropKioskCheckIn(string appointmentID)
        {
            return SoftDentRawImports12CS.SDInteropKioskCheckIn(appointmentID);
        }

        public void SDInteropSetAdapterVersion(int major, int minor, int build, int revision)
        {
            SoftDentRawImports12CS.SDInteropSetAdapterVersion(major, minor, build, revision);
        }
    }
}
