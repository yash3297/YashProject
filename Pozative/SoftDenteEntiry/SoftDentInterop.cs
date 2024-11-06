using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pozative;
using System.Threading;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Pozative
{
    internal class SoftDentInterop : ISoftDentInterop
    {
        private static int _OriginalThreadID = 0;
        private static object _LockLoadDatabase = new object();
        private static object _LockInstance = new object();
        private object _LockOpenDatabase = new object();
       // private Logger _Logger = LogManager.GetLogger("Pozative.SoftDentInterop");
        private static bool _LoadedDatabase;
        private static SoftDentInterop _Instance;
        private ISoftDentImports _Imports;
        private Timer _SystemDateTimer;

        public static int OriginalThreadID
        {
            get { return _OriginalThreadID; }
            set { _OriginalThreadID = value; } 
        }

        private static bool LoadedDatabase
        {
            get
            {
                lock (SoftDentInterop._LockLoadDatabase)
                    return SoftDentInterop._LoadedDatabase;
            }
            set
            {
                lock (SoftDentInterop._LockLoadDatabase)
                    SoftDentInterop._LoadedDatabase = value;
            }
        }

        private static bool Initialized { get; set; }

        private SoftDentInterop()
        {
        }

        public static SoftDentInterop Instance()
        {
            if (SoftDentInterop._Instance == null)
            {
                lock (SoftDentInterop._LockInstance)
                {
                    if (SoftDentInterop._Instance == null)
                        SoftDentInterop._Instance = new SoftDentInterop();
                }
            }
            return SoftDentInterop._Instance;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);

        public bool Open(string path, string faircomServer, out ErrorCode errorCode)
        {
            lock (this._LockOpenDatabase)
            {
                if (!SoftDentInterop.LoadedDatabase)
                {
                    try
                    {
                        if (SoftDentInterop.OriginalThreadID == 0)
                            SoftDentInterop.OriginalThreadID = Thread.CurrentThread.ManagedThreadId;
                        this.CheckThreadID();
                        this.Init(path);
                        //if (SoftDentVersion.IsClientServer && string.IsNullOrEmpty(faircomServer))
                        //{
                        // //   this._Logger.LogError((object)"Faircom server name is blank.");
                        //    errorCode = ErrorCode.DBServiceError;
                        //    return false;
                        //}
                        if (faircomServer == string.Empty)
                            faircomServer = (string)null;
                        errorCode = (ErrorCode)this._Imports.SDInteropInitializeDB(path, SoftDentRegistryKey.InteropLogging, faircomServer);
                        if (this._SystemDateTimer == null)
                            this._SystemDateTimer = new Timer(new TimerCallback(this.SystemDateTimerCallback), (object)null, 60000, 60000);
                        SoftDentInterop.LoadedDatabase = errorCode == ErrorCode.Success;
                        if (!SoftDentInterop.LoadedDatabase)                        
                            // this._Logger.LogError((object)("SoftDent database cannot be accessed:  " + path));                        
                        this.SetAdapterVersion();
                    }
                    catch (DllNotFoundException)
                    {
                        errorCode = ErrorCode.DBInvalidPath;
                       // this._Logger.LogError((object)new Exception(path + "\\SDInterop.dll could not be loaded.", (Exception)ex));
                    }
                    catch (Exception)
                    {
                        errorCode = ErrorCode.Failure;
                       // this._Logger.LogError((object)new Exception(path + "\\SDInterop.dll could not be loaded.", ex));
                    }
                }
                else
                    errorCode = ErrorCode.Success;
            }
            return SoftDentInterop.LoadedDatabase;
        }

        public void Close()
        {
            this._Imports.SDInteropCloseFiles();
            SoftDentInterop.LoadedDatabase = false;
        }

        public void SetLogging(bool loggingOn)
        {
            this._Imports.SDInteropSetLogging(loggingOn);
        }

        public void SetAdapterVersion()
        {
            Version version = this.GetType().Assembly.GetName().Version;
            this.SetAdapterVersion(version.Major, version.Minor, version.Build, version.Revision);
        }

        public void SetAdapterVersion(int major, int minor, int build, int revision)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSetAdapterVersion(1, 0, 3, 0);
        }

        public IntPtr GetAppointments(DateTime begin, DateTime end)
        {
            try
            {
                this.VerifyDatabase();
                string shortDateString1 = begin.ToShortDateString();
                string shortTimeString1 = begin.ToShortTimeString();
                string shortDateString2 = end.ToShortDateString();
                string shortTimeString2 = end.ToShortTimeString();
                if (Thread.CurrentThread.CurrentCulture.IetfLanguageTag != "en-US")
                {
                    CultureInfo cultureInfo = new CultureInfo("en-US");
                    shortDateString1 = begin.ToString(cultureInfo.DateTimeFormat.ShortDatePattern);
                    shortDateString2 = end.ToString(cultureInfo.DateTimeFormat.ShortDatePattern);
                }
                return this._Imports.SDInteropGetAppointmentsForDay(shortDateString1, shortTimeString1, shortDateString2, shortTimeString2, "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IntPtr GetAppointment(string PMSRecordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetAppointment(PMSRecordID);
        }

        public void ConfirmAppointment(
          string PMSRecordID,
          string method,
          DateTime date,
          string status,
          string reason)
        {
            this.VerifyDatabase();
            string shortDateString = date.ToShortDateString();
            string shortTimeString = date.ToShortTimeString();
            this._Imports.SDInteropConfirmAppointment(PMSRecordID, method, shortDateString, shortTimeString, status, reason);
        }

        public IntPtr GetOffice(DateTime dateTime)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetOffice(dateTime.ToShortDateString());
        }

        public IntPtr GetOperatories()
        {
            try
            {
                this.VerifyDatabase();
                return this._Imports.SDInteropGetOperatories();
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public void SavePatient(string xml)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSavePatient(xml);
        }

        public void SaveAppointment(string xml)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSaveAppointment(xml);
           // this._Imports.SDInteropSaveAppointment();
        }

        public IntPtr GetPatient(string recordID)
        {
            try
            {
                this.VerifyDatabase();
                return this._Imports.SDInteropGetPatient(recordID);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public string CreateRestorativeChartImage(string PMSRecordID, int view, int mode)
        {
            this.VerifyDatabase();
            return InteropHelper.PtrToString(this._Imports.SDInteropCreateRestorativeChartImage(PMSRecordID, view, mode));
        }

        public string CreatePerioChartImage(string PMSRecordID, int view, int mode)
        {
            this.VerifyDatabase();
            return InteropHelper.PtrToString(this._Imports.SDInteropCreatePerioChartImage(PMSRecordID, view, mode));
        }

        public IntPtr GetPatientDentalInsurancePlan(string PMSRecordID, bool primary)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPatientDentalInsurancePlan(PMSRecordID, primary);
        }

        public IntPtr GetPatientDentalInsurance(string PMSRecordID, bool primary)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPatientDentalInsurance(PMSRecordID, primary);
        }

        public IntPtr GetPatientGuarantors(string PMSRecordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPatientGuarantors(PMSRecordID);
        }

        public IntPtr GetImages(string PMSRecordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetImages(PMSRecordID);
        }

        public void SaveInsurance(
          string patientID,
          int nGuarIndex,
          string personXml,
          string planXml,
          string companyXml,
          bool primary)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSaveInsurance(patientID, nGuarIndex, personXml, planXml, companyXml, primary);
        }

        public IntPtr GetPatientPreferredPharmacy(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPatientPreferredPharmacy(patientID);
        }

        public IntPtr GetPatientPotrait(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPatientPortrait(patientID);
        }

        public IntPtr GetResponsiblePartyForPatient(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetResponsiblePartyForPatient(patientID);
        }

        public IntPtr GetResponsibleParty(string acctID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetResponsibleParty(acctID);
        }

        public IntPtr GetPatientEmployee(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPatientEmployee(patientID);
        }

        public IntPtr GetResponsiblePartyEmployee(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetResponsiblePartyEmployee(patientID);
        }

        public void SaveEmployerForPatient(string patientID, string status, string employer)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSaveEmployerForPatient(patientID, status, employer);
        }

        public void SaveResponsiblePartyForPatient(string patientID, string responsibleParty)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSaveResponsiblePartyForPatient(patientID, responsibleParty);
        }

        public bool PatientHasOpenLabCases(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPatientHasOpenLabCases(patientID);
        }

        public IntPtr GetPatientPerson(string patientID, int index)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPatientPerson(patientID, index);
        }

        public void SavePatientPerson(
          string patientID,
          int index,
          string personXML,
          string planXML,
          string companyXML)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSavePatientPerson(patientID, index, personXML, planXML, companyXML);
        }

        public IntPtr GetPatientSpouse(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPatientSpouse(patientID);
        }

        public bool SavePatientSpouse(string patientID, string spouse)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropSavePatientSpouse(patientID, spouse);
        }

        public IntPtr GetMedicalHistoryConditions()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetMedicalHistoryConditions();
        }

        public IntPtr GetMedicalHistoryAllergies()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetMedicalHistoryAllergies();
        }

        public IntPtr GetMostRecentMedicalHistory(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetMostRecentMedicalHistory(patientID);
        }

        public void SaveMedicalHistory(string patientID, string xml)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSaveMedicalHistory(patientID, xml);
        }

        public IntPtr GetPatientMedicalAlert(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPatientMedicalAlert(patientID);
        }

        public IntPtr GetInsuranceCompany(string PMSRecordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetInsuranceCompany(PMSRecordID);
        }

        public IntPtr GetInsuranceCompanyList()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetInsuranceCompanyList();
        }

        public void SaveInsuranceCompany(string xml)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSaveInsuranceCompany(xml);
        }

        public IntPtr SaveNewInsuranceCompany(string xml)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropSaveNewInsuranceCompany(xml);
        }

        public IntPtr GetInsurancePlanList()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetInsurancePlanList();
        }

        public IntPtr GetInsurancePlan(string PMSRecordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetInsurancePlan(PMSRecordID);
        }

        public IntPtr GetPharmacy(string PMSRecordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPharmacy(PMSRecordID);
        }

        public IntPtr GetPharmacyList()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPharmacyList();
        }

        public IntPtr GetFirstPharmacyListCount(int count, string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetFirstPharmacyListCount(count, deviceKey);
        }

        public IntPtr GetNextPharmacyListCount(int count, string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetNextPharmacyListCount(count, deviceKey);
        }

        public void SavePharmacy(string xml)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSavePharmacy(xml);
        }

        public IntPtr SaveNewPharmacy(string xml)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropSaveNewPharmacy(xml);
        }

        public IntPtr GetDrugList()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetDrugList();
        }

        public IntPtr GetEmployer(string PMSRecordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetEmployer(PMSRecordID);
        }

        public IntPtr GetEmployerList()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetEmployerList();
        }

        public void SaveEmployer(string xml)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSaveEmployer(xml);
        }

        public IntPtr SaveNewEmployer(string xml)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropSaveNewEmployer(xml);
        }

        public IntPtr GetProvider(string PMSRecordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetDenist(PMSRecordID);
        }

        public IntPtr GetProviderList()
        {
            try
            {
                this.VerifyDatabase();
                return this._Imports.SDInteropGetDentistList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IntPtr GetSpecialty()
        {
            try
            {
                this.VerifyDatabase();
                return this._Imports.GetSpecialty();
            }
            catch (Exception)
            {
                
                throw;
            }
          
        }

        public void SaveProvider(string xml)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSaveDentist(xml);
        }

        public string SaveNewProvider(string xml)
        {
            this.VerifyDatabase();
            return InteropHelper.PtrToString(this._Imports.SDInteropSaveNewDentist(xml));
        }

        public IntPtr GetConsultingProvider(string PMSRecordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetReferringDoctor(PMSRecordID);
        }

        public IntPtr GetConsultingProviderList()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetReferringDoctorList();
        }

        public void SaveConsultingProvider(string xml)
        {
            this.VerifyDatabase();
            this._Imports.SDInteropSaveReferringDoctor(xml);
        }

        public string SaveNewConsultingProvider(string xml)
        {
            this.VerifyDatabase();
            return InteropHelper.PtrToString(this._Imports.SDInteropSaveNewReferringDoctor(xml));
        }

        public IntPtr GetADACodeList()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetADACodeList();
        }

        public int ArchiveEForm(
          string patientID,
          string eformFilePath,
          DateTime archiveDateTime,
          string description)
        {
            this.VerifyDatabase();
            string date;
            string time;
            InteropHelper.DateTimeToString(archiveDateTime, out date, out time);
            return this._Imports.SDInteropArchiveEForm(patientID, eformFilePath, date, time, description);
        }

        public IntPtr GetImage(string imageID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetImage(imageID);
        }

        public int PutImage(
          string patientID,
          int imageType,
          int acquisitionRegion,
          string imageFileName,
          string imageFilePath,
          string toothAssociations,
          string acquisitionDate,
          string acquisitionTime)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPutImage(patientID, imageType, acquisitionRegion, imageFileName, imageFilePath, toothAssociations, acquisitionDate, acquisitionTime);
        }

        public IntPtr GetEFormPathAndName(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetEFormPathAndName(patientID);
        }

        public IntPtr PearlLoginUser(string userName, string password)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlLoginUser(userName, password);
        }

        public void PearlClearDeviceKey()
        {
            this.VerifyDatabase();
            this._Imports.SDInteropPearlClearDeviceKey();
        }

        public IntPtr PearlGetPatientList()
        {
            try
            {
                this.VerifyDatabase();
                return this._Imports.SDInteropPearlGetPatientList();
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        public IntPtr PearlGetInitialPatientList()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetInitialPatientList();
        }

        public IntPtr PearlGetFirstPatientCount(int count, string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetFirstPatientCount(count, deviceKey);
        }

        public IntPtr PearlGetNextPatientCount(int count, string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetNextPatientCount(count, deviceKey);
        }

        public IntPtr PearlQueryPatientList(string query)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlQueryPatientList(query);
        }

        public IntPtr PearlGetPatient(string recordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetPatient(recordID);
        }

        public IntPtr PearlGetPatientDetail(string recordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetPatientDetail(recordID);
        }

        public IntPtr PearlGetPatientClinicalProfile(string recordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetPatientClinicalProfile(recordID);
        }

        public IntPtr PearlGetPatientAlerts(string recordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetPatientAlerts(recordID);
        }

        public IntPtr PearlGetPatientMedication(string recordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetPatientMedication(recordID);
        }

        public IntPtr PearlGetMoreAppointments(string recordID, int count)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetMoreAppointments(recordID, count);
        }

        public IntPtr PearlGetFirstPatientClinicalImages(
          string recordID,
          int count,
          string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetFirstPatientClinicalImages(recordID, count, deviceKey);
        }

        public IntPtr PearlGetNextPatientClinicalImages(
          string recordID,
          int count,
          string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetNextPatientClinicalImages(recordID, count, deviceKey);
        }

        public IntPtr PearlGetConsultingProviderList()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetConsultingProviderList();
        }

        public IntPtr PearlGetFirstConsultingProviderCount(int count, string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetFirstConsultingProviderCount(count, deviceKey);
        }

        public IntPtr PearlGetNextConsultingProviderCount(int count, string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetNextConsultingProviderCount(count, deviceKey);
        }

        public IntPtr PearlQueryConsultingProviderList(string query)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlQueryConsultingProviderList(query);
        }

        public IntPtr PearlGetConsultingProvider(string recordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetConsultingProvider(recordID);
        }

        public IntPtr PearlGetConsultingProviderProfile(string recordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetConsultingProviderProfile(recordID);
        }

        public IntPtr PearlGetConsultingProviderDetails(string recordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetConsultingProviderDetails(recordID);
        }

        public IntPtr PearlGetFinancialData(string providerId, int timePeriod)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetFinancialData(providerId, timePeriod);
        }

        public int PearlSaveNewPrescription(string patientID, string userID, string xml)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlSaveNewPrescription(patientID, userID, xml);
        }

        public int PearlSavePatientCallDetails(string userID, string xml)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlSavePatientCallDetails(userID, xml);
        }

        public IntPtr PearlGetBlockedSlots(DateTime start, DateTime end)
        {
            try
            {
                this.VerifyDatabase();
                string date1;
                string time1;
                InteropHelper.DateTimeToString(start, out date1, out time1);
                string date2;
                string time2;
                InteropHelper.DateTimeToString(end, out date2, out time2);
                return this._Imports.SDInteropPearlGetBlockedSlots(date1, time1, date2, time2);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IntPtr PearlGetAppointments(string providerId, DateTime start, DateTime end)
        {
            this.VerifyDatabase();
            string date1;
            string time1;
            InteropHelper.DateTimeToString(start, out date1, out time1);
            string date2;
            string time2;
            InteropHelper.DateTimeToString(end, out date2, out time2);
            return this._Imports.SDInteropPearlGetAppointments(providerId, date1, time1, date2, time2);
        }

        public IntPtr PearlGetCallList(string providerId, bool refresh)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetCallList(providerId, refresh);
        }

        public int PearlSaveConsultingProviderCallDetails(string userID, string xml)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropSaveConsultingProviderCallDetails(userID, xml);
        }

        public IntPtr PearlGetAppointmentsByOperatory(
          string operatoryID,
          DateTime start,
          DateTime end)
        {
            this.VerifyDatabase();
            string date1;
            string time1;
            InteropHelper.DateTimeToString(start, out date1, out time1);
            string date2;
            string time2;
            InteropHelper.DateTimeToString(end, out date2, out time2);
            return this._Imports.SDInteropPearlGetAppointmentsByOperatory(operatoryID, date1, time1, date2, time2);
        }

        public IntPtr PearlMakeAppointment(
          string patientID,
          string providerID,
          string operatoryID,
          DateTime dateTime,
          int duration,
          string note)
        {
            this.VerifyDatabase();
            string date;
            string time;
            InteropHelper.DateTimeToString(dateTime, out date, out time);
            return this._Imports.SDInteropPearlMakeAppointment(patientID, providerID, operatoryID, date, time, duration, note);
        }

        public int PearlMakeBlockedSlot(
          string userID,
          string operatoryID,
          string providerID,
          DateTime dateTime,
          int duration,
          string note)
        {
            this.VerifyDatabase();
            string date;
            string time;
            InteropHelper.DateTimeToString(dateTime, out date, out time);
            return this._Imports.SDInteropPearlMakeBlockedSlot(userID, operatoryID, providerID, date, time, duration, note);
        }

        public IntPtr PearlGetFirstCallListCount(
          string providerID,
          int count,
          string deviceKey,
          bool refresh)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetFirstCallListCount(providerID, count, deviceKey, refresh);
        }

        public IntPtr PearlGetNextCallListCount(string providerID, int count, string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropPearlGetNextCallListCount(providerID, count, deviceKey);
        }

        public IntPtr GetEServiceFlags()
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetEServiceFlags();
        }

        public IntPtr GetMissedApptsRevenueAnalysis(DateTime startDate, DateTime endDate)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetMissedApptsRevenueAnalysis(startDate.ToShortDateString(), endDate.ToShortDateString());
        }

        public IntPtr LinkCleardown(string PMSRecordID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropLinkCleardown(PMSRecordID);
        }

        public IntPtr LinkConfigure(string xmlConfiguration)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropLinkConfigure(xmlConfiguration);
        }

        public IntPtr LinkLookupPatient(string xmlPatient, string xmlSource)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropLinkLookupPatient(xmlPatient, xmlSource);
        }

        public IntPtr LinkAddPatient(string xmlPatient, string xmlSource)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropLinkAddPatient(xmlPatient, xmlSource);
        }

        public IntPtr LinkUpdatePatient(
          string patientID,
          string xmlPatient,
          string xmlSource,
          int option)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropLinkUpdatePatient(patientID, xmlPatient, xmlSource, option);
        }

        public IntPtr LinkLookupGuarantor(string xmlGuarantor, string xmlSource)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropLinkLookupGuarantor(xmlGuarantor, xmlSource);
        }

        public IntPtr LinkAddGuarantor(string xmlGuarantor, string xmlSource)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropLinkAddGuarantor(xmlGuarantor, xmlSource);
        }

        public IntPtr LinkUpdateGuarantor(
          string accountID,
          string xmlGuarantor,
          string xmlSource,
          int option)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropLinkUpdateGuarantor(accountID, xmlGuarantor, xmlSource, option);
        }

        public IntPtr GetFirstSDClinicalNotes(string patientID, int count, string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetFirstSDClinicalNotes(patientID, count, deviceKey);
        }

        public IntPtr GetNextSDClinicalNotes(string patientID, int count, string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetNextSDClinicalNotes(patientID, count, deviceKey);
        }

        public IntPtr GetPreviousSDClinicalNotes(string patientID, int count, string deviceKey)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetPreviousSDClinicalNotes(patientID, count, deviceKey);
        }

        public IntPtr MakeClinicalNote(
          string userID,
          string patientID,
          string providerId,
          string note)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropMakeClinicalNote(userID, patientID, providerId, note);
        }

        public int GetClinicalNoteDateCount(string patientID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropGetClinicalNoteDateCount(patientID);
        }

        public bool KioskCheckIn(string appointmentID)
        {
            this.VerifyDatabase();
            return this._Imports.SDInteropKioskCheckIn(appointmentID);
        }

        private void Init(string path)
        {
            if (SoftDentInterop.Initialized)
                return;
            SoftDentVersion.Init(path);
            this._Imports = SoftDentImportsFactory.GetSoftDentImporter();
            this.LoadInteropDll(path, this._Imports.FileName);
            SoftDentInterop.Initialized = true;
        }

        private void VerifyDatabase()
        {
            if (!SoftDentInterop.LoadedDatabase)
                throw new Exception("SoftDent database must be opened explicitly.");
            this.CheckThreadID();
        }

        private void CheckThreadID()
        {
            if (SoftDentInterop.OriginalThreadID != Thread.CurrentThread.ManagedThreadId)
            {
                string message = "Data connection established with diffrent thread ID ----- " + SoftDentInterop.OriginalThreadID.ToString() + ":" + Thread.CurrentThread.ManagedThreadId.ToString();
                Console.WriteLine(message);
                throw new Exception(message);
            }
        }

        private bool LoadInteropDll(string path, string file)
        {
            string currentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = path;
            SoftDentInterop.SetDllDirectory(path);
            IntPtr num = SoftDentInterop.LoadLibrary(path + "\\" + file);
            SoftDentInterop.SetDllDirectory((string)null);
            Environment.CurrentDirectory = currentDirectory;
            return num != IntPtr.Zero;
        }

        private void SystemDateTimerCallback(object state)
        {
            this._Imports.SDInteropCheckForMidnight();
        }
    }
}
