using Pozative.BAL;
using Pozative.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pozative.BAL.Synch;
using System.Threading;
using System.Timers;
using System.Diagnostics;
using Microsoft.Win32;

namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable

        bool IsSoftdentProviderSync = false;
        bool IsSoftdentOperatorySync = false;
        bool IsSoftdentApptStatusSync = false;

        private BackgroundWorker bwSynchSoftDent_Appointment = new BackgroundWorker();
        private System.Timers.Timer timerSynchSoftDent_Appointment = null;

        private BackgroundWorker bwSynchSoftDent_OperatoryEvent = new BackgroundWorker();
        private System.Timers.Timer timerSynchSoftDent_OperatoryEvent = null;

        private BackgroundWorker bwSynchSoftDent_Operatory = new BackgroundWorker();
        private System.Timers.Timer timerSynchSoftDent_Operatory = null;

        private BackgroundWorker bwSynchSoftdent_Provider = new BackgroundWorker();
        private System.Timers.Timer timerSynchSoftdent_Provider = null;

        private BackgroundWorker bwSynchSoftdent_Speciality = new BackgroundWorker();
        private System.Timers.Timer timerSynchSoftdent_Speciality = null;

        private BackgroundWorker bwSynchSoftdent_ApptType = new BackgroundWorker();
        private System.Timers.Timer timerSynchSoftdent_ApptType = null;

        private BackgroundWorker bwSynchSoftDent_Patient = new BackgroundWorker();
        private System.Timers.Timer timerSynchSoftDent_Patient = null;

        private BackgroundWorker bwSynchSoftdent_RecallType = new BackgroundWorker();
        private System.Timers.Timer timerSynchSoftdent_RecallType = null;

        private BackgroundWorker bwSynchSoftDent_ApptStatus = new BackgroundWorker();
        private System.Timers.Timer timerSynchSoftDent_ApptStatus = null;

        //private System.Threading.Timer m_objTimer;
        //private bool m_blnStarted;
        //private readonly int m_intTickMs = 1000;
        //private object m_objLockObject = new object();

        #endregion

        public void SoftDentSync_Elapsed(object sender, ElapsedEventArgs e)
        {
            // MessageBox.Show("Timer called for Other Records");
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync");
            bool IsSyncing = false;
            if (key != null)
            {
                IsSyncing = bool.Parse(key.GetValue("IsSyncing").ToString());
            }

            if (!IsSoftDentSyncing && !IsSyncing)
            {
                //MessageBox.Show("Timer called for Other Records and pass to inner loop");
                RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                key1.SetValue("IsSyncing", true);

                SoftDentSync.Enabled = false;
                IsSoftDentSyncing = true;


                try
                {
                    Process myProcess = new Process();

                    try
                    {
                        myProcess.StartInfo.UseShellExecute = false;
                        // You can start any process, HelloWorld is a do-nothing example.
                        myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\SoftDentSync.exe";
                        myProcess.StartInfo.Arguments = "False";
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();
                        // This code assumes the process you are starting will terminate itself.
                        // Given that is is started without a window so you cannot terminate it
                        // on the desktop, it must terminate itself or you can do it programmatically
                        // from this application using the Kill method.
                    }
                    catch (Exception e1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("SoftDentSync_Elapsed_Err " + e1.Message);
                        throw;
                    }
                    // IsSoftDentSyncing = false;                   
                }
                catch (Exception ex)
                {
                    key1.SetValue("IsSyncing", false);
                    ObjGoalBase.WriteToErrorLogFile("SoftDentSync_Elapsed_Err " + ex.Message);
                }
                finally
                {
                    SoftDentSync.Enabled = true;
                }
            }

        }


        public void SoftDentPatientSync_Elapsed(object sender, ElapsedEventArgs e)
        {
            //MessageBox.Show("Timer called for Apponitment Records and pass");
            if (Utility.IsApplicationIdleTimeOff)
            {
                try
                {
                    //MessageBox.Show("Call Exe with True");
                    RegistryKey key11 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\SoftdentAppointmentSync");
                    bool IsSyncing11 = false;
                    if (key11 != null)
                    {
                        IsSyncing11 = bool.Parse(key11.GetValue("IsSyncing").ToString());
                    }
                    //MessageBox.Show("Call Exe with True_1 value " + IsSyncing11.ToString());
                    if (!IsSyncing11)
                    {
                        // MessageBox.Show("Timer called for Apponitment Records and pass to inner loop");
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\SoftdentAppointmentSync");
                        key1.SetValue("IsSyncing", true);
                        //IsSoftDentSyncing = true;
                        Process myProcess = new Process();

                        try
                        {
                            myProcess.StartInfo.UseShellExecute = false;
                            // You can start any process, HelloWorld is a do-nothing example.
                            myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\SoftDentSync.exe";
                            myProcess.StartInfo.Arguments = "True";
                            myProcess.StartInfo.CreateNoWindow = true;
                            myProcess.Start();
                            IsSoftDentSyncing = false;
                        }
                        catch (Exception e1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("SoftDentSync_Elapsed_Err " + e1.Message);
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\SoftdentAppointmentSync");
                    key1.SetValue("IsSyncing", false);
                    ObjGoalBase.WriteToErrorLogFile("SoftDentSync_Elapsed_Err " + ex.Message);
                    throw;
                }
            }
        }


        public void SyncSoftDentRecordsInitialy()
        {

            if (!IsSoftDentSyncing && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    // MessageBox.Show("Call Exe with False");
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync");
                    bool IsSyncing = false;
                    if (key != null)
                    {
                        IsSyncing = bool.Parse(key.GetValue("IsSyncing").ToString());
                    }
                    if (!IsSyncing)
                    {
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                        key1.SetValue("IsSyncing", true);
                        IsSoftDentSyncing = true;
                        Process myProcess = new Process();

                        try
                        {
                            myProcess.StartInfo.UseShellExecute = false;
                            // You can start any process, HelloWorld is a do-nothing example.
                            myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\SoftDentSync.exe";
                            myProcess.StartInfo.Arguments = "False";
                            myProcess.StartInfo.CreateNoWindow = true;
                            myProcess.Start();
                            // IsSoftDentSyncing = false;
                        }
                        catch (Exception e1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("SoftDentSync_Elapsed_Err " + e1.Message);
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                    key1.SetValue("IsSyncing", false);
                    ObjGoalBase.WriteToErrorLogFile("SoftDentSync_Elapsed_Err " + ex.Message);
                    throw;
                }
            }
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    //MessageBox.Show("Call Exe with True");
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\SoftdentAppointmentSync");
                    bool IsSyncing = false;
                    if (key != null)
                    {
                        IsSyncing = bool.Parse(key.GetValue("IsSyncing").ToString());
                    }
                    // MessageBox.Show("Call Exe with True_1 value " + IsSyncing.ToString() );
                    if (!IsSyncing)
                    {
                        //  MessageBox.Show("Call Exe with True_1");
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\SoftdentAppointmentSync");
                        key1.SetValue("IsSyncing", true);
                        Process myProcess = new Process();

                        try
                        {
                            myProcess.StartInfo.UseShellExecute = false;
                            // You can start any process, HelloWorld is a do-nothing example.
                            myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\SoftDentSync.exe";
                            myProcess.StartInfo.Arguments = "True";
                            myProcess.StartInfo.CreateNoWindow = true;
                            myProcess.Start();
                            IsSoftDentSyncing = false;
                        }
                        catch (Exception e1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("SoftDentSync_Elapsed_Err " + e1.Message);
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\SoftdentAppointmentSync");
                    key1.SetValue("IsSyncing", false);
                    ObjGoalBase.WriteToErrorLogFile("SoftDentSync_Elapsed_Err " + ex.Message);
                    throw;
                }
            }

        }

        #region GeneralMethods

        public Appointment[] OpenAppointment(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                OpenConnection();
                if (!this.IsOpen)
                    throw new InvalidOperationException("SoftDent database is not open.");

                return new AppointmentManager().GetAppointments(FromDate, ToDate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Provider[] GetProvider()
        {
            try
            {
                OpenConnection();
                if (!this.IsOpen)
                    throw new InvalidOperationException("SoftDent database is not open.");
                return new AppointmentManager().GetProvider();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Operatory[] GetOperatories()
        {
            try
            {
                OpenConnection();
                if (!this.IsOpen)
                    throw new InvalidOperationException("SoftDent database is not open.");
                return new AppointmentManager().GetOperatories();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PatientMin[] GetPatientList()
        {
            try
            {

                OpenConnection();
                if (!this.IsOpen)
                    throw new InvalidOperationException("SoftDent database is not open.");
                return new AppointmentManager().GetPatient();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public BlockedSlot[] GetBlockedSlot(DateTime FromDate, DateTime ToDate)
        {
            try
            {

                OpenConnection();
                if (!this.IsOpen)
                    throw new InvalidOperationException("SoftDent database is not open.");
                return new AppointmentManager().GetBlockedSlot(FromDate, ToDate);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private DataTable GetAppointmentList()
        {
            try
            {
                DataTable dtResult = new DataTable();
                DateTime tempLastSyncDate = Utility.LastSyncDateAditServer;
                DateTime tempFromDate = tempLastSyncDate;
                DateTime tempToDate = tempLastSyncDate;
                for (int i = 1; i <= 6; i++)
                {
                    tempToDate = tempFromDate.AddMonths(1);
                    Appointments = OpenAppointment(tempFromDate, tempToDate);
                    dtResult.Load(CreateAppointmentDataTable(Appointments).CreateDataReader());
                    tempFromDate = tempToDate;
                }
                return dtResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetAppointmentStatusList()
        {
            try
            {
                DataTable dtResult = new DataTable();
                DateTime tempLastSyncDate = Utility.LastSyncDateAditServer;
                DateTime tempFromDate = tempLastSyncDate;
                DateTime tempToDate = tempLastSyncDate;
                for (int i = 1; i <= 6; i++)
                {
                    tempToDate = tempFromDate.AddMonths(1);
                    Appointments = OpenAppointment(tempFromDate, tempToDate);
                    dtResult.Load(CreateAppointmentStatusDataTable(Appointments, dtResult).CreateDataReader());
                    tempFromDate = tempToDate;
                }
                return dtResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetOperatoryEventList()
        {
            try
            {
                DataTable dtResult = new DataTable();
                DateTime tempLastSyncDate = Utility.LastSyncDateAditServer;
                DateTime tempFromDate = tempLastSyncDate;
                DateTime tempToDate = tempLastSyncDate;
                for (int i = 1; i <= 6; i++)
                {
                    tempToDate = tempFromDate.AddMonths(1);
                    BlockedSlots = GetBlockedSlot(tempFromDate, tempToDate);
                    dtResult.Load(CreateOperatoryEventDataTable(BlockedSlots).CreateDataReader());
                    tempFromDate = tempToDate;
                }
                return dtResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetOperatoriesList()
        {
            try
            {
                DataTable dtOperatory = new DataTable();
                Operatories = GetOperatories();
                dtOperatory = CreateOperatoryDataTable(Operatories);
                return dtOperatory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetSpecialityList()
        {
            try
            {
                Providers = GetProvider();
                return CreateSpecialityDataTable(Providers);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetProviderList()
        {
            try
            {
                Providers = GetProvider();
                return CreateProviderDataTable(Providers);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetOperatoryList()
        {
            try
            {
                Operatories = GetOperatories();
                return CreateOperatoryDataTable(Operatories);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable GetAllPatientList()
        {
            try
            {
                PatientMins = GetPatientList();
                return CreateDataTable_Patient(PatientMins);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateDataTable_Patient(IList<PatientMin> item)
        {
            try
            {
                Type type = typeof(PatientMin);
                var properties = type.GetProperties();

                DataTable dtdataTable = new DataTable();
                dtdataTable = CreateColumnsInDatatable("Patient");

                foreach (PatientMin entity in item)
                {
                    DataRow drnew = dtdataTable.NewRow();
                    Patient ptResult = new AppointmentManager().GetPatient(entity.RecordID);

                    drnew["Patient_EHR_ID"] = entity.RecordID;
                    drnew["First_name"] = entity.FirstName;
                    drnew["Last_name"] = entity.LastName;
                    drnew["Middle_Name"] = ptResult.MiddleName;
                    drnew["Salutation"] = ptResult.Title;
                    if (ptResult.RecordStatus.ToString() == "Active")
                    {
                        drnew["Status"] = "A";
                    }
                    else
                    {
                        drnew["Status"] = "I";
                    }
                    drnew["Sex"] = ptResult.Sex;
                    drnew["MaritalStatus"] = ptResult.MaritalStatus;
                    drnew["Birth_Date"] = ptResult.Birthdate;
                    drnew["Email"] = ptResult.ContactInformation.EMailAddress1;
                    drnew["Mobile"] = ptResult.ContactInformation.Mobile.Number;
                    drnew["Home_Phone"] = ptResult.ContactInformation.HomePhone.Number;
                    drnew["Work_Phone"] = ptResult.ContactInformation.WorkPhone.Number;
                    if (ptResult.ContactInformation.HomeAddress.StreetAddressLines.Count() > 0)
                    {
                        drnew["Address1"] = ptResult.ContactInformation.HomeAddress.StreetAddressLines[0].ToString();
                    }
                    if (ptResult.ContactInformation.HomeAddress.StreetAddressLines.Count() > 1)
                    {
                        drnew["Address2"] = ptResult.ContactInformation.HomeAddress.StreetAddressLines[1].ToString();
                    }
                    drnew["City"] = ptResult.ContactInformation.HomeAddress.Locale;
                    //drnew["State"] = ptResult.ContactInformation.HomeAddress;
                    drnew["Zipcode"] = ptResult.ContactInformation.HomeAddress.PostalCode;
                    drnew["ResponsibleParty_Status"] = "";//ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["CurrentBal"] = "0";// ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["ThirtyDay"] = "0";// ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["SixtyDay"] = "0";// ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["NinetyDay"] = "0";// ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["Over90"] = "0";// ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["FirstVisit_Date"] = ptResult.FirstVisit;
                    drnew["LastVisit_Date"] = ptResult.LastVisit;
                    drnew["Primary_Insurance"] = "";// ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["Primary_Insurance_CompanyName"] = "";// ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["Secondary_Insurance"] = "";//  ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["Secondary_Insurance_CompanyName"] = "";//  ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["Guar_ID"] = "";//  ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["Pri_Provider_ID"] = "0";// ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["Sec_Provider_ID"] = "0";// ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["ReceiveSms"] = "0";// ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["ReceiveEmail"] = "0";// ptResult.ContactInformation.HomeAddress.Locale;
                    //drnew["nextvisit_date"] = ptResult.ContactInformation.HomeAddress.Locale;
                    //drnew["due_date"] = ptResult.ContactInformation.HomeAddress.Locale;
                    //drnew["remaining_benefit"] = ptResult.ContactInformation.HomeAddress.Locale;
                    //drnew["collect_payment"] = ptResult.ContactInformation.HomeAddress.Locale;
                    drnew["Last_Sync_Date"] = DateTime.Now;
                    drnew["EHR_Entry_DateTime"] = ptResult.RecordCreated;
                    drnew["Is_Adit_Updated"] = false;
                    drnew["preferred_name"] = "";// ptResult.ContactInformation.HomeAddress.Locale;

                    dtdataTable.Rows.Add(drnew);
                }
                return dtdataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateOperatoryDataTable(IList<Operatory> item)
        {
            try
            {

                Type type = typeof(Operatory);
                var properties = type.GetProperties();

                DataTable dtdataTable = new DataTable();
                dtdataTable = CreateColumnsInDatatable("Operatory");

                foreach (Operatory entity in item)
                {
                    DataRow drnew = dtdataTable.NewRow();
                    drnew["Operatory_EHR_ID"] = entity.RecordID;
                    drnew["Operatory_Name"] = entity.Name;
                    drnew["Last_Sync_Date"] = DateTime.Now;
                    drnew["EHR_Entry_DateTime"] = entity.RecordCreated;
                    drnew["Is_Adit_Updated"] = false;
                    dtdataTable.Rows.Add(drnew);
                }
                return dtdataTable;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private DataTable CreateAppointmentDataTable(IList<Appointment> item)
        {
            try
            {

                Type type = typeof(Appointment);
                var properties = type.GetProperties();

                DataTable dtdataTable = new DataTable();
                dtdataTable = CreateColumnsInDatatable("Appointment");

                DataTable dtOperatory = GetOperatoriesList();
                string procedureCode = "";


                foreach (Appointment entity in item)
                {
                    procedureCode = "";
                    DataRow drnew = dtdataTable.NewRow();
                    drnew["Appt_EHR_ID"] = entity.RecordID.ToString();
                    drnew["Last_Name"] = entity.Patient.LastName;
                    drnew["First_Name"] = entity.Patient.FirstName;
                    drnew["MI"] = entity.Patient.MiddleName;
                    drnew["Home_Contact"] = entity.Patient.ContactInformation.HomePhone.Number.ToString();
                    drnew["Mobile_Contact"] = entity.Patient.ContactInformation.Mobile.Number.ToString();
                    drnew["Email"] = entity.Patient.ContactInformation.EMailAddress1.ToString();
                    if (entity.Patient.ContactInformation.HomeAddress.StreetAddressLines.Count > 0)
                    {
                        drnew["Address"] = entity.Patient.ContactInformation.HomeAddress.StreetAddressLines[0].ToString();
                    }
                    drnew["City"] = entity.Patient.ContactInformation.HomeAddress.Locale.ToString();
                    drnew["ST"] = entity.Patient.ContactInformation.HomeAddress.AdministrativeArea.ToString();
                    drnew["Zip"] = entity.Patient.ContactInformation.HomeAddress.PostalCode.ToString();
                    drnew["Operatory_EHR_ID"] = entity.ExamRoom.ToString();
                    var result = dtOperatory.AsEnumerable().Where(o => o.Field<object>("Operatory_EHR_ID").ToString().ToUpper() == entity.ExamRoom.ToString().ToUpper());
                    if (result.Count() > 0)
                    {
                        drnew["Operatory_Name"] = result.First().Field<object>("Operatory_Name").ToString();
                    }
                    drnew["Provider_EHR_ID"] = entity.Provider.RecordID.ToString();
                    drnew["Provider_Name"] = entity.Provider.FirstName + " " + entity.Provider.LastName.ToString();
                    drnew["comment"] = entity.Notes.ToString();
                    drnew["birth_date"] = entity.Patient.Birthdate.ToString();
                    drnew["ApptType_EHR_ID"] = string.Empty;
                    drnew["ApptType"] = string.Empty;
                    drnew["Appt_DateTime"] = entity.DateAndTime;
                    drnew["Appt_EndDateTime"] = entity.DateAndTime.AddMinutes(entity.Duration);
                    drnew["Status"] = 1;
                    drnew["Patient_Status"] = "new";
                    drnew["appointment_status_ehr_key"] = string.Empty;
                    if (entity.IsCheckedIn)
                    {
                        drnew["Appointment_Status"] = "CheckedIn";
                    }
                    else if (entity.IsSeated)
                    {
                        drnew["Appointment_Status"] = "Seated";
                    }
                    else if (entity.IsConfirmed)
                    {
                        drnew["Appointment_Status"] = "Confirmed";
                    }
                    else if (entity.IsCheckedOut)
                    {
                        drnew["Appointment_Status"] = "CheckedOut";
                    }
                    else if (entity.IsCompleted)
                    {
                        drnew["Appointment_Status"] = "Completed";
                    }
                    else
                    {
                        drnew["Appointment_Status"] = "Scheduled";
                    }
                    drnew["Is_Appt"] = "EHR";
                    drnew["is_ehr_updated"] = false;
                    //drnew["Remind_DateTime"] = false;
                    drnew["Entry_DateTime"] = entity.RecordCreated;
                    drnew["Last_Sync_Date"] = DateTime.Now;
                    drnew["EHR_Entry_DateTime"] = entity.RecordCreated;
                    drnew["Is_Adit_Updated"] = false;
                    drnew["patient_ehr_id"] = entity.Patient.RecordID;
                    for (int i = 0; i <= entity.Procedures.Count; i++)
                    {
                        if (entity.Procedures[i].ProcedureCode != string.Empty)
                        {
                            if (procedureCode != "")
                            {
                                procedureCode = procedureCode + "," + entity.Procedures[i].ProcedureCode;
                            }
                            else
                            {
                                procedureCode = entity.Procedures[i].ProcedureCode;
                            }
                        }

                    }
                    drnew["ProcedureCode"] = procedureCode;

                    //drnew["unschedule_status_ehr_key"] = entity.Patient.RecordID;
                    //drnew["unschedule_status"] = entity.Patient.RecordID;
                    //drnew["confirmed_status_ehr_key"] = entity.Patient.RecordID;
                    drnew["confirmed_status"] = "";
                    drnew["is_deleted"] = false;

                    dtdataTable.Rows.Add(drnew);
                }
                return dtdataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateAppointmentStatusDataTable(IList<Appointment> item, DataTable dtResult)
        {
            try
            {

                Type type = typeof(Appointment);
                var properties = type.GetProperties();

                DataTable dtdataTable = new DataTable();
                dtdataTable = CreateColumnsInDatatable("Appointment_Status");

                string AppointmentStatus = string.Empty;

                foreach (Appointment entity in item)
                {
                    if (entity.IsCheckedIn)
                    {
                        AppointmentStatus = "CheckedIn";
                    }
                    else if (entity.IsSeated)
                    {
                        AppointmentStatus = "Seated";
                    }
                    else if (entity.IsConfirmed)
                    {
                        AppointmentStatus = "Confirmed";
                    }
                    else if (entity.IsCheckedOut)
                    {
                        AppointmentStatus = "CheckedOut";
                    }
                    else if (entity.IsCompleted)
                    {
                        AppointmentStatus = "Completed";
                    }
                    else
                    {
                        AppointmentStatus = "Scheduled";
                    }

                    var result = dtdataTable.AsEnumerable().Where(o => o.Field<object>("ApptStatus_Name").ToString().ToUpper() == AppointmentStatus.ToUpper());
                    var result1 = dtResult.AsEnumerable().Where(o => o.Field<object>("ApptStatus_Name").ToString().ToUpper() == AppointmentStatus.ToUpper());
                    if (result != null && result.Count() == 0 && result1 != null && result1.Count() == 0)
                    {
                        DataRow drnew = dtdataTable.NewRow();
                        drnew["ApptStatus_Name"] = AppointmentStatus;
                        drnew["Last_Sync_Date"] = DateTime.Now;
                        drnew["Is_Adit_Updated"] = false;
                        dtdataTable.Rows.Add(drnew);
                    }
                }

                return dtdataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateOperatoryEventDataTable(IList<BlockedSlot> item)
        {
            try
            {
                Type type = typeof(BlockedSlot);
                var properties = type.GetProperties();

                DataTable dtdataTable = new DataTable();
                dtdataTable = CreateColumnsInDatatable("OperatoryEvent");

                DataTable dtOperatory = GetOperatoriesList();

                foreach (BlockedSlot entity in item)
                {
                    DataRow drnew = dtdataTable.NewRow();
                    drnew["OE_EHR_ID"] = entity.RecordID.ToString();
                    drnew["Operatory_EHR_ID"] = entity.ExamRoom.ToString();
                    drnew["StartTime"] = entity.DateAndTime.ToString();
                    drnew["EndTime"] = Convert.ToDateTime(entity.DateAndTime).AddMinutes(Convert.ToInt16(entity.Duration));
                    drnew["comment"] = entity.Description.ToString();
                    drnew["Entry_DateTime"] = entity.RecordCreated;
                    drnew["Last_Sync_Date"] = DateTime.Now;
                    drnew["Is_Adit_Updated"] = false;
                    drnew["is_deleted"] = false;
                    dtdataTable.Rows.Add(drnew);
                }
                return dtdataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateSpecialityDataTable(IList<Provider> item)
        {
            try
            {
                Type type = typeof(Provider);
                var properties = type.GetProperties();

                DataTable dtdataTable = new DataTable();
                dtdataTable = CreateColumnsInDatatable("Speciality");

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Speciality_EHR_ID");
                dataTable.Columns.Add("Speciality_Name");

                foreach (Provider entity in item)
                {
                    DataRow drNew = dataTable.NewRow();
                    drNew["Speciality_EHR_ID"] = entity.TypeID.ToString();
                    drNew["Speciality_Name"] = entity.Type.ToString();
                    dataTable.Rows.Add(drNew);
                }

                dataTable = Utility.CreateDistinctRecords(dataTable, "", "Speciality_Name");

                dtdataTable.Load(dataTable.CreateDataReader());

                return dtdataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateProviderDataTable(IList<Provider> item)
        {
            try
            {
                Type type = typeof(Provider);
                var properties = type.GetProperties();

                DataTable dtdataTable = new DataTable();
                dtdataTable = CreateColumnsInDatatable("Providers");

                foreach (Provider entity in item)
                {
                    DataRow drnew = dtdataTable.NewRow();
                    drnew["Provider_EHR_ID"] = entity.RecordID;
                    drnew["Last_Name"] = entity.LastName;
                    drnew["First_Name"] = entity.FirstName;
                    drnew["MI"] = entity.MiddleName;
                    drnew["gender"] = entity.Sex;
                    drnew["provider_speciality"] = entity.Type;
                    drnew["bio"] = entity.RecordID;
                    drnew["education"] = entity.Degree;
                    drnew["accreditation"] = entity.LicenseNumber;
                    drnew["membership"] = string.Empty;
                    drnew["language"] = string.Empty;
                    drnew["age_treated_min"] = entity.RecordID;
                    drnew["age_treated_max"] = entity.RecordID;
                    drnew["is_active"] = entity.RecordStatus.ToString() == "Active" ? true : false;
                    drnew["Last_Sync_Date"] = DateTime.Now;
                    drnew["EHR_Entry_DateTime"] = entity.RecordCreated;
                    drnew["Is_Adit_Updated"] = false;
                    dtdataTable.Rows.Add(drnew);
                }

                return dtdataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable CreateColumnsInDatatable(string tablename)
        {
            try
            {

                DataTable dataTable = new DataTable();
                DataTable dtColumns = SystemBAL.GetColumns(tablename);
                foreach (DataRow drRow in dtColumns.Rows)
                {
                    if (drRow["DATA_TYPE"].ToString() == "nvarchar")
                    {
                        dataTable.Columns.Add(new DataColumn(drRow["COLUMN_NAME"].ToString(), typeof(System.String)));
                    }
                    else if (drRow["DATA_TYPE"].ToString() == "bigint")
                    {
                        dataTable.Columns.Add(new DataColumn(drRow["COLUMN_NAME"].ToString(), typeof(System.Int64)));
                    }
                    else if (drRow["DATA_TYPE"].ToString() == "bit")
                    {
                        dataTable.Columns.Add(new DataColumn(drRow["COLUMN_NAME"].ToString(), typeof(System.Boolean)));
                    }
                    else if (drRow["DATA_TYPE"].ToString() == "datetime")
                    {
                        dataTable.Columns.Add(new DataColumn(drRow["COLUMN_NAME"].ToString(), typeof(System.DateTime)));
                    }
                    else if (drRow["DATA_TYPE"].ToString() == "image")
                    {
                        dataTable.Columns.Add(new DataColumn(drRow["COLUMN_NAME"].ToString(), typeof(System.Byte[])));
                    }
                    else
                    {
                        dataTable.Columns.Add(new DataColumn(drRow["COLUMN_NAME"].ToString(), typeof(System.String)));
                    }
                    dataTable.Columns[drRow["COLUMN_NAME"].ToString()].ExtendedProperties["Size"] = drRow["CHARACTER_MAXIMUM_LENGTH"].ToString();
                }
                return dataTable;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private DataTable CompareDataTableRecords(ref DataTable dtSource, DataTable dtDestination, string compareColumnName, string primarykeyColumns, string ignoreColumns)
        {
            try
            {
                if (dtSource.Columns.Contains(primarykeyColumns))
                {
                    dtSource.Columns.Remove(primarykeyColumns);
                    dtSource.Columns.Add(primarykeyColumns, typeof(Int64));
                    dtSource.Columns[primarykeyColumns].DefaultValue = 0;
                }

                DataTable dtSourceTemp = dtSource;

                #region Provider available in Local Database Or Not available in local Database
                string[] ignoreColumnsArr = new string[] { "" };
                ignoreColumnsArr = ignoreColumns.Split(',');
                try
                {
                    dtSource.AsEnumerable()
                               .All(o =>
                               {
                                   try
                                   {
                                       var result = dtDestination.AsEnumerable().Where(a => a.Field<object>(compareColumnName).ToString().Trim() == o.Field<object>(compareColumnName).ToString().Trim());
                                       if (result != null && result.Count() > 0)
                                       {
                                           foreach (DataColumn dcCol in dtSourceTemp.Columns)
                                           {
                                               try
                                               {
                                                   var result1 = ignoreColumnsArr.AsEnumerable().Where(b => b.ToString().ToUpper() == dcCol.ColumnName.ToUpper());

                                                   if ((result1 != null && result1.Count() == 0))
                                                   {
                                                       if (dcCol.DataType.Name.ToString().ToLower() == "boolean" || dcCol.DataType.Name.ToString().ToLower() == "sbyte")
                                                       {
                                                           if (result.First().Field<object>(dcCol.ColumnName) != null && o[dcCol.ColumnName] != null && o[dcCol.ColumnName].ToString() != string.Empty && result.First().Field<object>(dcCol.ColumnName).ToString() != string.Empty
                                                               && Convert.ToBoolean(o[dcCol.ColumnName]) != Convert.ToBoolean(result.First().Field<object>(dcCol.ColumnName)))
                                                           {
                                                               o["InsUptDlt"] = 2;
                                                               o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                               break;
                                                           }
                                                           else
                                                           {
                                                               o["InsUptDlt"] = 4;
                                                               o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                           }
                                                       }
                                                       else
                                                       {
                                                           if (dcCol.ColumnName.ToLower() == "birth_date")
                                                           {
                                                               try
                                                               {
                                                                   if (result.First().Field<object>(dcCol.ColumnName) != null && o[dcCol.ColumnName] != null
                                                                       && string.IsNullOrEmpty(o[dcCol.ColumnName].ToString()) == false
                                                                       && string.IsNullOrEmpty(result.First().Field<object>(dcCol.ColumnName).ToString()) == false)
                                                                   {
                                                                       if (Convert.ToDateTime(o[dcCol.ColumnName]).ToShortDateString() != Convert.ToDateTime(result.First().Field<object>(dcCol.ColumnName)).ToShortDateString())
                                                                       {
                                                                           o["InsUptDlt"] = 2;
                                                                           o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                                           break;
                                                                       }
                                                                   }
                                                                   else if (o[dcCol.ColumnName] == null && result.First().Field<object>(dcCol.ColumnName) != null)
                                                                   {
                                                                       o["InsUptDlt"] = 2;
                                                                       o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                                       break;
                                                                   }
                                                                   else if (o[dcCol.ColumnName] != null && result.First().Field<object>(dcCol.ColumnName) == null)
                                                                   {
                                                                       o["InsUptDlt"] = 2;
                                                                       o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                                       break;
                                                                   }
                                                                   else if (o[dcCol.ColumnName] != null && result.First().Field<object>(dcCol.ColumnName) != null && o[dcCol.ColumnName].ToString() == "" && result.First().Field<object>(dcCol.ColumnName) != "")
                                                                   {
                                                                       o["InsUptDlt"] = 2;
                                                                       o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                                       break;
                                                                   }
                                                                   else if (o[dcCol.ColumnName] != null && result.First().Field<object>(dcCol.ColumnName) != null && o[dcCol.ColumnName].ToString() != "" && result.First().Field<object>(dcCol.ColumnName) == "")
                                                                   {
                                                                       o["InsUptDlt"] = 2;
                                                                       o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                                       break;
                                                                   }
                                                               }
                                                               catch (Exception ex)
                                                               {
                                                                   Utility.WriteToErrorLogFromAll("SynchPartial_Softdent_CompareDataTableRecords_Birthdate" + ex.Message);
                                                               }
                                                           }
                                                           else if (result.First().Field<object>(dcCol.ColumnName) != null && string.IsNullOrEmpty(result.First().Field<object>(dcCol.ColumnName).ToString()) == false && o[dcCol.ColumnName].ToString().Trim().ToLower() != result.First().Field<object>(dcCol.ColumnName).ToString().Trim().ToLower())
                                                           {
                                                               o["InsUptDlt"] = 2;
                                                               o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                               break;
                                                           }
                                                           else if ((result.First().Field<object>(dcCol.ColumnName) == null || (result.First().Field<object>(dcCol.ColumnName) != null && result.First().Field<object>(dcCol.ColumnName).ToString() == string.Empty))
                                                               && o[dcCol.ColumnName] != null && o[dcCol.ColumnName].ToString().Trim().ToLower() != string.Empty)
                                                           {
                                                               o["InsUptDlt"] = 2;
                                                               o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                               break;
                                                           }
                                                           else
                                                           {
                                                               o["InsUptDlt"] = 4;
                                                               o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                           }
                                                       }
                                                   }
                                               }
                                               catch (Exception ex)
                                               {
                                                   Utility.WriteToErrorLogFromAll("SynchPartial_Softdent_CompareDataTableRecords_Update Data" + ex.Message);
                                               }
                                           }
                                       }
                                       else
                                       {
                                           o["InsUptDlt"] = 1;
                                       }
                                   }
                                   catch (Exception ex)
                                   {
                                       Utility.WriteToErrorLogFromAll("SynchPartial_Softdent_CompareDataTableRecords_destination data" + ex.Message);
                                   }
                                   return true;
                               });
                }
                catch (Exception ex)
                {
                    Utility.WriteToErrorLogFromAll("SynchPartial_Softdent_CompareDataTableRecords_CheckData exist in local " + ex.Message);
                }

                #endregion

                #region Records available in local but not available in SoftDent
                dtDestination.AsEnumerable().Where(o => o.Field<object>(compareColumnName).ToString() != "0" && o.Field<object>(compareColumnName).ToString() != "")
                    .All(o =>
                    {
                        try
                        {
                            var result = dtSourceTemp.AsEnumerable().Where(a => a.Field<object>(compareColumnName).ToString().Trim() == o.Field<object>(compareColumnName).ToString().Trim());
                            if (result != null && result.Count() == 0)
                            {
                                o["InsUptDlt"] = 3;
                            }
                            else if (result == null)
                            {
                                o["InsUptDlt"] = 3;
                            }
                        }
                        catch (Exception ex)
                        {
                            Utility.WriteToErrorLogFromAll("SynchPartial_Softdent_CompareDataTableRecords_data exist in local but not in EHR " + ex.Message);
                        }
                        return true;
                    });
                #endregion

                return dtDestination;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("SynchPartial_Softdent_CompareDataTableRecords " + ex.Message);
                throw;
            }
        }

        private DataTable CompareDataTableRecordsWithTwoColumns(ref DataTable dtSource, DataTable dtDestination, string compareColumnName1, string compareColumnName2, string primarykeyColumns, string ignoreColumns)
        {
            try
            {
                DataTable dtSourceTemp = dtSource;

                #region Provider available in Local Database Or Not available in local Database
                string[] ignoreColumnsArr = new string[] { "" };
                ignoreColumnsArr = ignoreColumns.Split(',');

                dtSource.AsEnumerable()
                    .All(o =>
                    {
                        var result = dtDestination.AsEnumerable().Where(a => a.Field<object>(compareColumnName1).ToString().Trim() == o.Field<object>(compareColumnName1).ToString().Trim() && a.Field<object>(compareColumnName2).ToString().Trim() == o.Field<object>(compareColumnName2).ToString().Trim());
                        if (result != null && result.Count() > 0)
                        {
                            foreach (DataColumn dcCol in dtSourceTemp.Columns)
                            {
                                var result1 = ignoreColumnsArr.AsEnumerable().Where(b => b.ToString().ToUpper() == dcCol.ColumnName.ToUpper());

                                if ((result1 != null && result1.Count() == 0))
                                {
                                    if (dcCol.DataType.Name.ToString().ToLower() == "boolean" || dcCol.DataType.Name.ToString().ToLower() == "sbyte")
                                    {
                                        if (result.First().Field<object>(dcCol.ColumnName) != null && o[dcCol.ColumnName] != null && o[dcCol.ColumnName].ToString() != string.Empty && result.First().Field<object>(dcCol.ColumnName).ToString() != string.Empty
                                            && Convert.ToBoolean(o[dcCol.ColumnName]) != Convert.ToBoolean(result.First().Field<object>(dcCol.ColumnName)))
                                        {
                                            o["InsUptDlt"] = 2;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                            break;
                                        }
                                        else
                                        {
                                            o["InsUptDlt"] = 4;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                        }
                                    }
                                    else
                                    {
                                        if (result.First().Field<object>(dcCol.ColumnName) != null && string.IsNullOrEmpty(result.First().Field<object>(dcCol.ColumnName).ToString()) == false && o[dcCol.ColumnName].ToString().Trim().ToLower() != result.First().Field<object>(dcCol.ColumnName).ToString().Trim().ToLower())
                                        {
                                            o["InsUptDlt"] = 2;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                            break;
                                        }
                                        else if ((result.First().Field<object>(dcCol.ColumnName) == null || (result.First().Field<object>(dcCol.ColumnName) != null && result.First().Field<object>(dcCol.ColumnName).ToString() == string.Empty))
                                            && o[dcCol.ColumnName] != null && o[dcCol.ColumnName].ToString().Trim().ToLower() != string.Empty)
                                        {
                                            o["InsUptDlt"] = 2;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                            break;
                                        }
                                        else
                                        {
                                            o["InsUptDlt"] = 4;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            o["InsUptDlt"] = 1;
                        }
                        return true;
                    });

                #endregion

                #region Records available in local but not available in SoftDent
                dtDestination.AsEnumerable().Where(o => o.Field<object>(compareColumnName1).ToString() != "0" && o.Field<object>(compareColumnName1).ToString() != "" && o.Field<object>(compareColumnName2).ToString() != "0" && o.Field<object>(compareColumnName2).ToString() != "")
                    .All(o =>
                    {
                        var result = dtSourceTemp.AsEnumerable().Where(a => a.Field<object>(compareColumnName1).ToString().Trim() == o.Field<object>(compareColumnName1).ToString().Trim() && a.Field<object>(compareColumnName2).ToString().Trim() == o.Field<object>(compareColumnName2).ToString().Trim());
                        if (result != null && result.Count() == 0)
                        {
                            o["InsUptDlt"] = 3;
                        }
                        else if (result == null)
                        {
                            o["InsUptDlt"] = 3;
                        }
                        return true;
                    });
                #endregion

                return dtDestination;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Sync Provider Records

        private void CallSynch_SoftDentProvider()
        {
            CommonFunction.GetMasterSync();

            if (!Is_synched_Provider)
            {
                Is_synched_Provider = true;

                try
                {
                    DataTable dtSoftDentProvider = GetProviderList();
                    dtSoftDentProvider.Columns.Add("InsUptDlt", typeof(int));
                    dtSoftDentProvider.Columns["InsUptDlt"].DefaultValue = 0;
                    DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", "1");

                    if (!dtLocalProvider.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalProvider.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalProvider.Columns["InsUptDlt"].DefaultValue = 0;
                    }
                    dtLocalProvider = CompareDataTableRecords(ref dtSoftDentProvider, dtLocalProvider, "Provider_EHR_ID", "Provider_LocalDB_ID", "Provider_LocalDB_ID,Provider_EHR_ID,Provider_Web_ID,image,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    //dtLocalProvider.AsEnumerable().Where(o => string.IsNullOrEmpty(o.Field<object>("InsUptDlt").ToString()) == false && o.Field<object>("InsUptDlt").ToString() == "3").Count() > 0
                    if (dtLocalProvider.Select("InsUptDlt=3").Count() > 0)
                    {
                        dtSoftDentProvider.Load(dtLocalProvider.Select("InsUptDlt=3").CopyToDataTable().CreateDataReader());
                    }

                    #region Provider available in Local Database Or Not available in local Database
                    //dtSoftDentProvider.AsEnumerable()
                    //    .All(o =>
                    //        {
                    //            var result = dtLocalProvider.AsEnumerable().Where(a => a.Field<object>("Provider_EHR_ID").ToString().Trim() == o.Field<object>("Provider_EHR_ID").ToString().Trim());
                    //            if (result != null && result.Count() > 0)
                    //            {
                    //                foreach (DataColumn dcCol in dtSoftDentProvider.Columns)
                    //                {
                    //                    if (o[dcCol.ColumnName].ToString().Trim().ToLower() != result.First().Field<object>(dcCol.ColumnName).ToString().Trim().ToLower())
                    //                     {
                    //                         o["InsUptDlt"] = 2;
                    //                         break;                                             
                    //                     }
                    //                }                                   
                    //            }
                    //            else
                    //            {
                    //                o["InsUptDlt"] = 1;
                    //            }                               
                    //            return true;
                    //        });

                    #endregion

                    #region Records available in local but not available in SoftDent
                    //dtLocalProvider.AsEnumerable()
                    //    .All(o =>
                    //        {
                    //            var result = dtSoftDentProvider.AsEnumerable().Where(a => a.Field<object>("Provider_EHR_ID").ToString().Trim() == o.Field<object>("Provider_EHR_ID").ToString().Trim());
                    //            if (result != null && result.Count() == 0)
                    //            {
                    //                o["InsUptDlt"] = 3;
                    //            }
                    //            else if (result == null)
                    //            {
                    //                o["InsUptDlt"] = 3;
                    //            }
                    //            return true;
                    //        });
                    #endregion

                    dtSoftDentProvider.AcceptChanges();

                    if (dtSoftDentProvider != null && dtSoftDentProvider.Rows.Count > 0 && dtSoftDentProvider.AsEnumerable()
                        .Where(o => Convert.ToInt16(o.Field<object>("InsUptDlt")) == 1 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 2 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 3 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 4).Count() > 0)
                    {
                        DataTable dtSaveRecords = dtSoftDentProvider.Clone();
                        bool status = false;
                        if (dtSoftDentProvider.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtSoftDentProvider.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchSoftDentBAL.Save_SoftDent_To_Local(dtSaveRecords, "Providers", "Provider_LocalDB_ID,Provider_Web_ID", "Provider_LocalDB_ID");
                        }
                        else
                        {
                            if (dtSoftDentProvider.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                            ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsSoftdentProviderSync = true;
                            SynchDataLiveDB_Push_Provider();

                        }
                        else
                        {
                            IsSoftdentProviderSync = false;
                        }
                    }
                    Is_synched_Provider = false;
                }
                catch (Exception ex)
                {
                    Is_synched_Provider = false;
                    ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database)]" + ex.Message);
                }

            }
        }

        #endregion

        #region Synch Operatory

        public void SynchDataSoftDent_Operatory()
        {
            try
            {
                DataTable dtSoftDentOperatory = GetOperatoriesList();

                dtSoftDentOperatory.Columns.Add("InsUptDlt", typeof(int));
                dtSoftDentOperatory.Columns["InsUptDlt"].DefaultValue = 0;

                DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData("1");
                if (!dtLocalOperatory.Columns.Contains("InsUptDlt"))
                {
                    dtLocalOperatory.Columns.Add("InsUptDlt", typeof(int));
                    dtLocalOperatory.Columns["InsUptDlt"].DefaultValue = 0;
                }

                dtLocalOperatory = CompareDataTableRecords(ref dtSoftDentOperatory, dtLocalOperatory, "Operatory_Name", "Operatory_LocalDB_ID", "Operatory_LocalDB_ID,Operatory_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                if (dtLocalOperatory.Select("InsUptDlt=3").Count() > 0)
                {
                    dtSoftDentOperatory.Load(dtLocalOperatory.Select("InsUptDlt=3").CopyToDataTable().CreateDataReader());
                }

                dtSoftDentOperatory.AcceptChanges();

                if (dtSoftDentOperatory != null && dtSoftDentOperatory.Rows.Count > 0 && dtSoftDentOperatory.AsEnumerable()
                        .Where(o => Convert.ToInt16(o.Field<object>("InsUptDlt")) == 1 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 2 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 3 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 4).Count() > 0)
                {
                    bool status = false;
                    DataTable dtSaveRecords = dtSoftDentOperatory.Clone();
                    if (dtSoftDentOperatory.Select("InsUptDlt IN (1,2)").Count() > 0)
                    {
                        dtSaveRecords.Load(dtSoftDentOperatory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                        status = SynchSoftDentBAL.Save_SoftDent_To_Local(dtSaveRecords, "Operatory", "Operatory_LocalDB_ID,Operatory_Web_ID,OperatoryOrder", "Operatory_LocalDB_ID");
                    }
                    else
                    {
                        if (dtSoftDentOperatory.Select("InsUptDlt IN (4)").Count() > 0)
                        {
                            status = true;
                        }
                    }
                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                        ObjGoalBase.WriteToSyncLogFile("Operatory Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        IsSoftdentOperatorySync = true;
                        SynchDataLiveDB_Push_Operatory();
                    }
                    else
                    {
                        IsSoftdentOperatorySync = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Operatory Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Speciality

        public void SynchDataSoftDent_Speciality()
        {
            try
            {
                DataTable dtSoftDentSpeciality = GetSpecialityList();

                dtSoftDentSpeciality.Columns.Add("InsUptDlt", typeof(int));
                dtSoftDentSpeciality.Columns["InsUptDlt"].DefaultValue = 0;

                DataTable dtLocalSpeciality = SynchLocalBAL.GetLocalSpecialityData("1");
                if (!dtLocalSpeciality.Columns.Contains("InsUptDlt"))
                {
                    dtLocalSpeciality.Columns.Add("InsUptDlt", typeof(int));
                    dtLocalSpeciality.Columns["InsUptDlt"].DefaultValue = 0;
                }


                dtLocalSpeciality = CompareDataTableRecords(ref dtSoftDentSpeciality, dtLocalSpeciality, "Speciality_Name", "Speciality_LocalDB_ID", "Speciality_LocalDB_ID,Speciality_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                dtSoftDentSpeciality.AcceptChanges();

                if (dtSoftDentSpeciality != null && dtSoftDentSpeciality.Rows.Count > 0)
                {
                    bool status = false;
                    DataTable dtSaveRecords = dtSoftDentSpeciality.Clone();
                    if (dtSoftDentSpeciality.Select("InsUptDlt IN (1,2)").Count() > 0)
                    {
                        dtSaveRecords.Load(dtSoftDentSpeciality.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                        status = SynchSoftDentBAL.Save_SoftDent_To_Local(dtSaveRecords, "Speciality", "Speciality_LocalDB_ID,Speciality_Web_ID", "Speciality_LocalDB_ID");
                    }
                    else
                    {
                        if (dtSoftDentSpeciality.Select("InsUptDlt IN (4)").Count() > 0)
                        {
                            status = true;
                        }
                    }
                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality");
                        ObjGoalBase.WriteToSyncLogFile("Speciality Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        SynchDataLiveDB_Push_Speciality();
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Speciality Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Patient

        public void SynchDataSoftDent_Patient()
        {
            try
            {
                IsParientFirstSync = false;
                DataTable dtSoftDentPatientList = GetAllPatientList();

                dtSoftDentPatientList.Columns.Add("InsUptDlt", typeof(int));
                dtSoftDentPatientList.Columns["InsUptDlt"].DefaultValue = 0;

                DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData("1");

                if (!dtLocalPatient.Columns.Contains("InsUptDlt"))
                {
                    dtLocalPatient.Columns.Add("InsUptDlt", typeof(int));
                    dtLocalPatient.Columns["InsUptDlt"].DefaultValue = 0;
                }

                //dtLocalProvider = CompareDataTableRecords(ref dtSoftDentProvider, dtLocalProvider, "Provider_EHR_ID", "Provider_LocalDB_ID", "Provider_LocalDB_ID,Provider_EHR_ID,Provider_Web_ID,image,Is_Adit_Updated,Last_Sync_Date,InsUptDlt");

                dtLocalPatient = CompareDataTableRecords(ref dtSoftDentPatientList, dtLocalPatient, "Patient_EHR_ID", "Patient_LocalDB_ID", "Patient_LocalDB_ID,Patient_EHR_ID,Patient_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                //DataTable dtSoftDentPatientdue_date = SynchSoftDentBAL.GetSoftDentPatientdue_date();

                //TotalPatientRecord = dtSoftDentPatient.Rows.Count;
                //GetPatientRecord = 0;

                dtSoftDentPatientList.AcceptChanges();


                bool status = false;
                DataTable dtSaveRecords = dtSoftDentPatientList.Clone();
                if (dtSoftDentPatientList.Select("InsUptDlt IN (1,2)").Count() > 0)
                {
                    dtSaveRecords.Load(dtSoftDentPatientList.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                    status = SynchSoftDentBAL.Save_SoftDent_To_Local(dtSaveRecords, "Patient", "Patient_LocalDB_ID,Patient_Web_ID", "Patient_LocalDB_ID");
                }
                else
                {
                    if (dtSoftDentPatientList.Select("InsUptDlt IN (4)").Count() > 0)
                    {
                        status = true;
                    }
                }
                if (status)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                    ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    SynchDataLiveDB_Push_Patient();
                }

                //if (dtSoftDentPatientList != null && dtSoftDentPatientList.Rows.Count > 0)
                //{
                //    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                //    IsGetParientRecordDone = true;
                //}
                //else
                //{
                //    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                //    IsGetParientRecordDone = true;
                //}
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Appointment

        public void SynchDataSoftDent_Appointment()
        {
            try
            {
                if (IsSoftdentProviderSync && IsSoftdentOperatorySync && IsSoftdentApptStatusSync)
                {
                    DataTable dtSoftDentAppointment = GetAppointmentList();

                    dtSoftDentAppointment.Columns.Add("InsUptDlt", typeof(int));
                    dtSoftDentAppointment.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData("1");

                    if (!dtLocalAppointment.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalAppointment.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalAppointment.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalAppointment = CompareDataTableRecords(ref dtSoftDentAppointment, dtLocalAppointment, "Appt_EHR_ID", "Appt_LocalDB_ID", "Appt_LocalDB_ID,Appt_EHR_ID,Appt_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Remind_DateTime,Clinic_Number,Service_Install_Id");

                    dtSoftDentAppointment.AcceptChanges();

                    bool status = false;
                    DataTable dtSaveRecords = dtSoftDentAppointment.Clone();
                    if (dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)").Count() > 0 || dtLocalAppointment.Select("InsUptDlt IN (3)").Count() > 0)
                    {
                        if (dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                        }
                        if (dtLocalAppointment.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtLocalAppointment.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                        }
                        status = SynchSoftDentBAL.Save_SoftDent_To_Local(dtSaveRecords, "Appointment", "Appt_LocalDB_ID,Appt_Web_ID", "Appt_LocalDB_ID");
                    }

                    else
                    {
                        if (dtSoftDentAppointment.Select("InsUptDlt IN (4)").Count() > 0)
                        {
                            status = true;
                        }
                    }
                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                        ObjGoalBase.WriteToSyncLogFile("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        IsApptTypeSyncPush = true;
                        SynchDataLiveDB_Push_Appointment();
                    }
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Sync Operatory Event

        public void SynchDataSoftDent_OperatoryEvent()
        {
            if (IsSoftdentOperatorySync)
            {
                try
                {
                    DataTable dtSoftDentOperatoryEvent = GetOperatoryEventList();

                    dtSoftDentOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                    dtSoftDentOperatoryEvent.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData("1");

                    if (!dtLocalOperatoryEvent.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalOperatoryEvent.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalOperatoryEvent = CompareDataTableRecords(ref dtSoftDentOperatoryEvent, dtLocalOperatoryEvent, "OE_EHR_ID", "OE_LocalDB_ID", "OE_LocalDB_ID,OE_EHR_ID,OE_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    dtSoftDentOperatoryEvent.AcceptChanges();

                    bool status = false;
                    DataTable dtSaveRecords = dtSoftDentOperatoryEvent.Clone();
                    if (dtSoftDentOperatoryEvent.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                    {
                        if (dtSoftDentOperatoryEvent.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtSoftDentOperatoryEvent.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                        }
                        if (dtLocalOperatoryEvent.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtLocalOperatoryEvent.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                        }
                        //dtSaveRecords.Load(dtSoftDentOperatoryEvent.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                        status = SynchSoftDentBAL.Save_SoftDent_To_Local(dtSaveRecords, "OperatoryEvent", "OE_LocalDB_ID,OE_Web_ID", "OE_LocalDB_ID");
                    }
                    else
                    {
                        if (dtSoftDentOperatoryEvent.Select("InsUptDlt IN (4)").Count() > 0)
                        {
                            status = true;
                        }
                    }
                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent");
                        ObjGoalBase.WriteToSyncLogFile("OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        SynchDataLiveDB_Push_OperatoryEvent();
                    }
                    IsEHRAllSync = true;
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Sync Appointment Status

        public void SynchDataSoftDent_ApptStatus()
        {
            try
            {
                DataTable dtSoftDentApptStatus = GetAppointmentStatusList();

                dtSoftDentApptStatus.Columns.Add("InsUptDlt", typeof(int));
                dtSoftDentApptStatus.Columns["InsUptDlt"].DefaultValue = 0;

                DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalAppointmentStatusData("1");

                if (!dtLocalApptStatus.Columns.Contains("InsUptDlt"))
                {
                    dtLocalApptStatus.Columns.Add("InsUptDlt", typeof(int));
                    dtLocalApptStatus.Columns["InsUptDlt"].DefaultValue = 0;
                }

                dtLocalApptStatus = CompareDataTableRecords(ref dtSoftDentApptStatus, dtLocalApptStatus, "ApptStatus_Name", "ApptStatus_LocalDB_ID", "ApptStatus_LocalDB_ID,ApptStatus_EHR_ID,ApptStatus_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                dtSoftDentApptStatus.AcceptChanges();

                bool status = false;
                DataTable dtSaveRecords = dtSoftDentApptStatus.Clone();
                if (dtSoftDentApptStatus.Select("InsUptDlt IN (1,2)").Count() > 0)
                {
                    dtSaveRecords.Load(dtSoftDentApptStatus.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                    status = SynchSoftDentBAL.Save_SoftDent_To_Local(dtSaveRecords, "Appointment_Status", "ApptStatus_LocalDB_ID,ApptStatus_Web_ID", "ApptStatus_LocalDB_ID");
                }
                else
                {
                    if (dtSoftDentApptStatus.Select("InsUptDlt IN (4)").Count() > 0)
                    {
                        status = true;
                    }
                }
                if (status)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus");
                    ObjGoalBase.WriteToSyncLogFile("ApptStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    IsSoftdentApptStatusSync = true;
                    SynchDataLiveDB_Push_ApptStatus();
                }
                else
                {
                    IsSoftdentApptStatusSync = false;
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ApptStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

    }
}
