using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Pozative
{
    public class AppointmentManager
    {
        private ISoftDentInterop _Interop = InteropFactory.GetSoftDentInterop();

        public Appointment[] GetAppointments(DateTime begin, DateTime end)
        {
            try
            {

                //MessageBox.Show("Start Fetch Appointment");
                IntPtr appointments = this._Interop.GetAppointments(begin, end);
                // MessageBox.Show("Appointment fetched");
                if (appointments == IntPtr.Zero)
                    return (Appointment[])null;

                AppointmentSeries appointmentSeries = DataContractManager.DeserializeXML<AppointmentSeries>(appointments);
                //MessageBox.Show("Appointment Deseriallize");
                if (appointmentSeries == null)
                    return (Appointment[])null;
                Appointment[] array = appointmentSeries.Appointments.ToArray();
                //  MessageBox.Show("Converted to array");
                foreach (Appointment appointment in array)
                    AppointmentManager.UpdateAppointmentRecordID(appointment);
                // MessageBox.Show("UPdate Ids");
                return array;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SaveAppointments(string appts)
        {
            this._Interop.SaveAppointment(appts);
            //if (appointments == IntPtr.Zero)
            //    return (Appointment[])null;
            //AppointmentSeries appointmentSeries = DataContractManager.DeserializeXML<AppointmentSeries>(appointments);
            //if (appointmentSeries == null)
            //    return (Appointment[])null;
            //Appointment[] array = appointmentSeries.Appointments.ToArray();
            //foreach (Appointment appointment in array)
            //    AppointmentManager.UpdateAppointmentRecordID(appointment);
            //return array;
        }


        public BlockedSlot[] GetBlockedSlot(DateTime begin, DateTime end)
        {
            try
            {
                IntPtr blocked = this._Interop.PearlGetBlockedSlots(begin, end);
                if (blocked == IntPtr.Zero)
                    return (BlockedSlot[])null;
                BlockedSlotSeries blockedSlotSeries = DataContractManager.DeserializeXML<BlockedSlotSeries>(blocked);
                if (blockedSlotSeries == null)
                    return (BlockedSlot[])null;
                BlockedSlot[] array = blockedSlotSeries.BlockedSlots.ToArray();
                return array;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Appointment GetAppointmentById(string RecordsId)
        {
            try
            {
                IntPtr appointment = this._Interop.GetAppointment(RecordsId);
                if (appointment == IntPtr.Zero)
                    return (Appointment)null;
                Appointment Appt = DataContractManager.DeserializeXML<Appointment>(appointment);
                //AppointmentSeries appointmentSeries = DataContractManager.DeserializeXML<AppointmentSeries>(appointment);
                //if (appointmentSeries == null)
                //    return (Appointment)null;
                //Appointment Appt =  //appointmentSeries.Appointments.ToArray();
                //foreach (Appointment appointment in array)
                //    AppointmentManager.UpdateAppointmentRecordID(appointment);
                return Appt;
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
                IntPtr provoders = this._Interop.GetProviderList();
                if (provoders == IntPtr.Zero)
                    return (Provider[])null;
                ProviderSeries providerSeries = DataContractManager.DeserializeXML<ProviderSeries>(provoders);
                if (providerSeries == null)
                    return (Provider[])null;
                Provider[] array = providerSeries.Providers.ToArray();
                //foreach (Provider appointment in array)
                //    AppointmentManager.UpdateAppointmentRecordID(appointment);
                return array;
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
                IntPtr operatories = this._Interop.GetOperatories();
                if (operatories == IntPtr.Zero)
                    return (Operatory[])null;
                OperatorySeries providerSeries = DataContractManager.DeserializeXML<OperatorySeries>(operatories);
                if (providerSeries == null)
                    return (Operatory[])null;
                Operatory[] array = providerSeries.Operatories.ToArray();
                //foreach (Provider appointment in array)
                //    AppointmentManager.UpdateAppointmentRecordID(appointment);
                return array;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Operatory[] GetSpecilty()
        {
            try
            {
                IntPtr operatories = this._Interop.GetSpecialty();
                if (operatories == IntPtr.Zero)
                    return (Operatory[])null;
                OperatorySeries providerSeries = DataContractManager.DeserializeXML<OperatorySeries>(operatories);
                if (providerSeries == null)
                    return (Operatory[])null;
                Operatory[] array = providerSeries.Operatories.ToArray();
                //foreach (Provider appointment in array)
                //    AppointmentManager.UpdateAppointmentRecordID(appointment);
                return array;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PatientMin[] GetPatient()
        {
            try
            {
                IntPtr patients = this._Interop.PearlGetPatientList();
                if (patients == IntPtr.Zero)
                    return (PatientMin[])null;
                PatientMinSeries patientSeries = DataContractManager.DeserializeXML<PatientMinSeries>(patients);
                if (patientSeries == null)
                    return (PatientMin[])null;
                PatientMin[] array = patientSeries.PatientMins.ToArray();
                return array;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Patient GetPatient(string RecordsId)
        {
            try
            {
                IntPtr patients = this._Interop.GetPatient(RecordsId);
                if (patients == IntPtr.Zero)
                    return (Patient)null;
                Patient patientSeries = DataContractManager.DeserializeXML<Patient>(patients);
                if (patientSeries == null)
                    return (Patient)null;
                return patientSeries;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetIds(string PMSRecordID, ref string patId)
        {
            try
            {
                string[] strArray = PMSRecordID.Split(':');
                string str = strArray[0];
                patId = (string)null;
                if (strArray.Length == 2)
                    patId = strArray[1];
                return str;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void UpdateAppointmentRecordID(Appointment appointment)
        {
            try
            {
                appointment.PMSRecordID = appointment.RecordID + ":" + appointment.Patient.RecordID;
            }
            catch (Exception)
            {                
                throw;
            }
            
        }
    }
}
