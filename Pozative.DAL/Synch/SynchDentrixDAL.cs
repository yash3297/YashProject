using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using Pozative.QRY;
using System.Data.SqlServerCe;
using Pozative.UTL;
using System.Globalization;
using System.Data.SqlClient;
using Pozative.BO;
using RestSharp;
using System.Net;
using System.Collections;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Pozative.DAL
{
    public class SynchDentrixDAL
    {

        public static bool GetDentrixConnection()
        {

            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            bool IsConnected = false;
            try
            {
                conn.Open();
                IsConnected = true;
            }
            catch (Exception)
            {
                IsConnected = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return IsConnected;
        }

        #region EHR_VersionNumber

        public static string GetDenrtrixEHR_VersionNumber()
        {

            string version;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string OdbcSelect = SynchDentrixQRY.GetDenrtrixEHR_VersionNumber;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                if (OdbcDt.Rows[0]["version"].ToString() != "" || OdbcDt.Rows[0]["version"].ToString() != string.Empty)
                {

                    version = OdbcDt.Rows[0]["version"].ToString();

                }
                else
                {
                    version = "";
                }
                return version;
            }
            catch (Exception ex)
            {
                throw ex;
                version = "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return version;
        }


        #endregion

        #region Appointment

        public static DataTable GetDentrixAppointmentData(string strApptID = "")
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string OdbcSelect = SynchDentrixQRY.GetDentrixAppointmentData;
                if (!string.IsNullOrEmpty(strApptID))
                {
                    OdbcSelect = OdbcSelect + " And convert(varchar(20),V_appt.appointment_id) = '" + strApptID + "'";
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixAppointmentDataDTXG5;
                    if (!string.IsNullOrEmpty(strApptID))
                    {
                        OdbcSelect = OdbcSelect + " And convert(varchar(10),appt.appointment_id) = '" + strApptID + "'";
                    }
                }

                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixAppointmentIds()
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string OdbcSelect = SynchDentrixQRY.GetDentrixAppointmentEhrIds;
                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixAppointmentEhrIdDTXG5;
                }

                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixAppointment_Procedures_Data(string strApptID = "")
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string OdbcSelect = SynchDentrixQRY.DentrixAppointment_Procedures_Data;
                if (!string.IsNullOrEmpty(strApptID))
                {
                    OdbcSelect = SynchDentrixQRY.DentrixAppointment_Procedures_DataByApptID;
                    OdbcSelect = OdbcSelect.Replace("@Appt_EHR_ID", strApptID);
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate1", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate2", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate3", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate4", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate5", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate6", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate7", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate8", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate9", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate10", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate11", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate12", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate13", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate14", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate15", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate16", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate17", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate18", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate19", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate20", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static DataTable GetDentrixApplicationVersion()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixApplicationVersion;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_Appointment_Dentrix_To_Local(DataTable dtDentrixAppointment)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        bool is_ehr_updated = false;
                        string Apptdate = string.Empty;
                        string ApptTime = string.Empty;
                        string Mobile_Contact = string.Empty;
                        string Email = string.Empty;
                        string Home_Contact = string.Empty;
                        string Address = string.Empty;
                        string City = string.Empty;
                        string State = string.Empty;
                        string Zipcode = string.Empty;
                        string patient_first_name = string.Empty;
                        string patient_last_name = string.Empty;
                        string patient_mi_name = string.Empty;
                        string AppointmentStatus = string.Empty;

                        foreach (DataRow dr in dtDentrixAppointment.Rows)
                        {
                            try
                            {
                                is_ehr_updated = false;
                                if (dr["InsUptDlt"].ToString() == "")
                                {
                                    dr["InsUptDlt"] = "0";
                                }
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                                {
                                    switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                    {
                                        case 1:
                                            SqlCeCommand.CommandText = SynchLocalQRY.Insert_Appointment;
                                            is_ehr_updated = true;
                                            break;
                                        case 5:
                                            SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Contact;
                                            is_ehr_updated = true;
                                            break;
                                        case 4:
                                            SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID;
                                            is_ehr_updated = true;
                                            break;
                                        case 3:
                                            SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                            break;
                                    }
                                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        dr["patient_name"] = dr["patient_name"].ToString().Replace(",,", ",");
                                        string[] patientinfo = dr["patient_name"].ToString().Split(',');
                                        patient_last_name = patientinfo[0].ToString().Trim();
                                        if (patientinfo.Length == 2)
                                        {
                                            string[] patienfirstMI = patientinfo[1].ToString().Trim().Split(' ');
                                            if (patienfirstMI.Length == 2)
                                            {
                                                patient_first_name = patienfirstMI[0].ToString().Trim();
                                                patient_mi_name = patienfirstMI[1].ToString().Trim();
                                            }
                                            else
                                            {
                                                patient_first_name = patienfirstMI[0].ToString().Trim();
                                                patient_mi_name = string.Empty;
                                            }
                                        }
                                        else if (patientinfo.Length > 2)
                                        {
                                            string[] patienfirstMI = patientinfo[1].ToString().Trim().Split(' ');
                                            if (patientinfo.Length == 3)
                                            {
                                                patient_first_name = patientinfo[2].ToString().Trim();
                                                patient_mi_name = patientinfo[1].ToString().Trim();
                                            }
                                            else
                                            {
                                                patient_first_name = patientinfo[patientinfo.Length - 1].ToString().Trim();
                                            }
                                        }


                                        Apptdate = !Utility.NotAllowToChangeSystemDateFormat ? Convert.ToDateTime(dr["appointment_date"].ToString()).ToString("dd/MM/yyyy") : Convert.ToDateTime(dr["appointment_date"].ToString()).Date.ToShortDateString();
                                        ApptTime = Convert.ToInt32(dr["start_hour"].ToString()).ToString("00") + ":" + Convert.ToInt32(dr["start_minute"].ToString()).ToString("00");

                                        Mobile_Contact = string.Empty;
                                        Email = string.Empty;
                                        Home_Contact = string.Empty;
                                        Address = string.Empty;
                                        City = string.Empty;
                                        State = string.Empty;
                                        Zipcode = string.Empty;


                                        Mobile_Contact = dr["patMobile"].ToString();
                                        Email = dr["patEmail"].ToString();
                                        Home_Contact = dr["patHomephone"].ToString().Trim();
                                        Address = dr["patAddress"].ToString().Trim();
                                        City = dr["patCity"].ToString().Trim();
                                        State = dr["patState"].ToString().Trim();
                                        Zipcode = dr["patZipcode"].ToString().Trim();

                                        //DateTime ApptDateTime = DateTime.ParseExact(Apptdate + " " + ApptTime, "dd/MMM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                        DateTime ApptDateTime = !Utility.NotAllowToChangeSystemDateFormat ? DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) : Convert.ToDateTime(Apptdate + " " + ApptTime);
                                        DateTime ApptEndDateTime = ApptDateTime.AddMinutes(Convert.ToInt32(dr["length"].ToString().Trim()));

                                        string birthdate = string.Empty;
                                        if (!string.IsNullOrEmpty(dr["birth_date"].ToString()))
                                        {
                                            birthdate = dr["birth_date"].ToString();
                                        }

                                        if (dr["appointment_status_ehr_key"].ToString().Trim() == "-106")
                                        {
                                            AppointmentStatus = "<COMPLETE>";
                                        }
                                        else if (dr["appointment_status_ehr_key"].ToString().Trim() == "0")
                                        {
                                            AppointmentStatus = "<none>";
                                        }
                                        else
                                        {
                                            AppointmentStatus = dr["Appointment_Status"].ToString().Trim();
                                        }
                                        int commentlen = 1999;
                                        if (dr["comment"].ToString().Trim().Length < commentlen)
                                        {
                                            commentlen = dr["comment"].ToString().Trim().Length;
                                        }
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", string.Empty);
                                        SqlCeCommand.Parameters.AddWithValue("Last_Name", patient_last_name.Trim());
                                        SqlCeCommand.Parameters.AddWithValue("First_Name", patient_first_name.Trim());
                                        SqlCeCommand.Parameters.AddWithValue("MI", patient_mi_name.Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Home_Contact", Utility.ConvertContactNumber(Home_Contact.ToString().Trim()));
                                        SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", Utility.ConvertContactNumber(Mobile_Contact.Trim()));
                                        SqlCeCommand.Parameters.AddWithValue("Email", Email.ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Address", Address.ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("City", City.ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("ST", State.ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Zip", Zipcode.ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["op_id"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["op_title"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["provider_id"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Provider_Name", dr["provider_last_name"].ToString().Trim() + " " + dr["provider_first_name"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("ApptType", dr["ApptType_Name"].ToString().Trim().Replace(",", " "));
                                        SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                                        SqlCeCommand.Parameters.AddWithValue("birth_date", birthdate);
                                        SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", ApptDateTime.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", ApptEndDateTime.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Status", "1");
                                        SqlCeCommand.Parameters.AddWithValue("Patient_Status", "new");
                                        SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Appointment_Status", AppointmentStatus);
                                        SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", "");
                                        SqlCeCommand.Parameters.AddWithValue("confirmed_status", "");
                                        SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", "");
                                        SqlCeCommand.Parameters.AddWithValue("unschedule_status", "");
                                        SqlCeCommand.Parameters.AddWithValue("Is_Appt", "EHR");
                                        SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", is_ehr_updated);
                                        SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
                                        SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
                                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                        SqlCeCommand.Parameters.AddWithValue("Appt_LocalDB_ID", dr["Appt_LocalDB_ID"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patId"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("is_asap", Convert.ToInt16(dr["appt_flag"].ToString().Trim()) == 2 ? true : false);
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                        SqlCeCommand.Parameters.AddWithValue("appt_treatmentcode", "");
                                        SqlCeCommand.Parameters.AddWithValue("ProcedureDesc", dr["ProcedureDesc"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("ProcedureCode", dr["ProcedureCode"].ToString().Trim());
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }

                            }
                            catch (Exception E1)
                            {
                                Utility.WriteToErrorLogFromAll(dr["appointment_id"].ToString().Trim() + " " + E1.Message.ToString());
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #region Deleted Appointment

        public static DataTable GetDentrixDeletedAppointmentData()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixDeletedAppointmentData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Update_DeletedAppointment_Dentrix_To_Local(DataTable dtDentrixDeletedAppointment)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        string AppointmentStatus = string.Empty;
                        foreach (DataRow dr in dtDentrixDeletedAppointment.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }

                            if (dr["InsUptDlt"].ToString() == "1")
                            {
                                SqlCeCommand.CommandText = SynchLocalQRY.InsertAppointment_With_DeleteFlag;
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appointment_id"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                            else if (dr["InsUptDlt"].ToString() == "2")
                            {
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appointment_id"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }

                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #endregion

        #region OperatoryEvent

        public static DataTable GetDentrixOperatoryEventData()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string OdbcSelect = SynchDentrixQRY.GetDentrixOperatoryEventData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@EventDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static bool Save_OperatoryEvent_Dentrix_To_Local(DataTable dtDentrixOperatoryEvent)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        foreach (DataRow dr in dtDentrixOperatoryEvent.Rows)
                        {

                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }

                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {

                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryEventData;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryEventData;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryEventData;
                                        break;
                                }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["event_id"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    try
                                    {
                                        DateTime OE_Date = Convert.ToDateTime(dr["event_date"].ToString());

                                        int OE_StartHour = Convert.ToInt32(dr["start_time"].ToString()) / 12;
                                        int OE_StartMin = (Convert.ToInt32(dr["start_time"].ToString()) % 12) * 5;

                                        int OE_EndHour = Convert.ToInt32(dr["end_time"].ToString()) / 12;
                                        int OE_EndMin = (Convert.ToInt32(dr["end_time"].ToString()) % 12) * 5;

                                        DateTime StartTime;
                                        DateTime EndTime;
                                        if (!Utility.NotAllowToChangeSystemDateFormat)
                                        {
                                            StartTime = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(OE_Date).ToString("MM/dd/yyyy") + " " + OE_StartHour.ToString() + ":" + OE_StartMin.ToString()).ToString("MM/dd/yyyy HH:mm"));
                                            EndTime = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(OE_Date).ToString("MM/dd/yyyy") + " " + OE_EndHour.ToString() + ":" + OE_EndMin.ToString()).ToString("MM/dd/yyyy HH:mm"));
                                        }
                                        else
                                        {
                                            StartTime = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(OE_Date).ToShortDateString() + " " + OE_StartHour.ToString() + ":" + OE_StartMin.ToString()).ToString());
                                            EndTime = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(OE_Date).ToShortDateString() + " " + OE_EndHour.ToString() + ":" + OE_EndMin.ToString()).ToString(""));
                                        }
                                        int commentlen = 1999;
                                        if (dr["note"].ToString().Trim().Length < commentlen)
                                        {
                                            commentlen = dr["note"].ToString().Trim().Length;
                                        }

                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["event_id"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", string.Empty);
                                        SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["operatory_id"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("comment", dr["note"].ToString().Trim().Substring(0, commentlen));
                                        SqlCeCommand.Parameters.AddWithValue("StartTime", StartTime.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("EndTime", EndTime.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Convert.ToDateTime(dr["modified_time_stamp"].ToString().Trim()));
                                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                        SqlCeCommand.Parameters.AddWithValue("Allow_Book_Appt", Convert.ToBoolean(false));
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                    catch (Exception)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }
        #endregion

        #region Provider

        #region Provider

        public static DataTable GetDentrixProviderData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixProviderDataG5;
                }
                else
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixProviderData;
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {

                try
                {
                    #region if Error Generated to fetch the Speciality
                    OdbcCommand.CommandTimeout = 200;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = SynchDentrixQRY.GetDentrixProviderDatafulldef;
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DataTable OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                    return OdbcDt;
                    #endregion
                }
                catch (Exception ex1)
                {
                    throw ex1;
                }

                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_Provider_Dentrix_To_Local(DataTable dtDentrixProvider)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;
                    string Provider_Speciality = "";
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixProvider.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Provider;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Provider;
                                        break;
                                }

                                try
                                {
                                    Provider_Speciality = dr["provider_speciality"].ToString();
                                    if (Provider_Speciality.Length > 10)
                                    {
                                        Provider_Speciality = dr["provider_speciality"].ToString().Trim().Substring(12, dr["provider_speciality"].ToString().Trim().Length - 12);
                                    }

                                }
                                catch (Exception)
                                {
                                    Provider_Speciality = "";
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Provider_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("MI", dr["MI"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("gender", "");
                                SqlCeCommand.Parameters.AddWithValue("provider_speciality", Provider_Speciality);
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("is_active", Convert.ToInt16(dr["is_active"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region ProviderHours

        public static DataTable GetDentrixProviderHoursData()
        {
            DateTime FromDate = Utility.LastSyncDateAditServer;
            DateTime ToDate = DateTime.Today.AddMonths(4);
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            DataTable dtResultProviderHours = new DataTable();
            CommonDB.OdbcConnectionServer(ref conn);
            DataTable dtProvider = new DataTable();
            DataTable OdbcDt = new DataTable();
            try
            {

                dtProvider = GetProviderIDs();
                dtResultProviderHours = SynchLocalDAL.GetLocalProviderHoursBlankStructure();
                foreach (DataRow drRow in dtProvider.Rows)
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = SynchDentrixQRY.GetDentrixProviderHoursData;
                    //  OdbcSelect = OdbcSelect.Replace("@provider_Id", drRow["Provider_EHR_Id"].ToString());
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.Parameters.AddWithValue("provider_Id", drRow["Provider_EHR_Id"].ToString());
                    OdbcCommand.Parameters.AddWithValue("FromDate", FromDate);
                    OdbcCommand.Parameters.AddWithValue("ToDate", ToDate);
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                    {
                        OdbcDt.Rows.Clear();
                    }
                    OdbcDa.Fill(OdbcDt);
                    OdbcDt.AsEnumerable()
                     .All(o =>
                     {

                         for (int i = 1; i <= 3; i++)
                         {
                             if (o["start_time" + i.ToString()] != null && o["start_time" + i.ToString()].ToString() != string.Empty)
                             {
                                 DataRow drNewRow = dtResultProviderHours.NewRow();
                                 drNewRow["PH_EHR_ID"] = Convert.ToDateTime(o["sched_exception_date"].ToString()).ToString("yyyyMMdd") + "_" + o["prov_id"].ToString() + "_" + i.ToString();
                                 drNewRow["Provider_EHR_ID"] = o["prov_id"].ToString();
                                 drNewRow["Operatory_EHR_ID"] = "";
                                 drNewRow["StartTime"] = Convert.ToDateTime(Convert.ToDateTime(o["sched_exception_date"].ToString()).ToString("yyyy/MM/dd") + " " + o["start_time" + i.ToString()].ToString());
                                 drNewRow["EndTime"] = Convert.ToDateTime(Convert.ToDateTime(o["sched_exception_date"].ToString()).ToString("yyyy/MM/dd") + " " + o["end_time" + i.ToString()].ToString());
                                 drNewRow["Entry_DateTime"] = Convert.ToDateTime(o["modified_time_stamp"].ToString());
                                 dtResultProviderHours.Rows.Add(drNewRow);
                             }
                         }
                         return true;
                     });

                }

                return dtResultProviderHours;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_ProviderHours_Dentrix_To_Local(DataTable dtDentrixProviderHours)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixProviderHours.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_ProviderHours;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ProviderHours;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_ProviderHours;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("PH_EHR_ID", dr["PH_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("PH_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("StartTime", dr["StartTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("EndTime", dr["EndTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dr["Entry_DateTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //   SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region ProviderOfficeHours

        public static DataTable GetDentrixProviderOfficeHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            DataTable dtProviderOfficeHours = new DataTable();
            CommonDB.OdbcConnectionServer(ref conn);
            DataTable dtProvider = new DataTable();
            DataTable OdbcDt = new DataTable();
            try
            {

                dtProvider = GetProviderIDs();

                foreach (DataRow drRow in dtProvider.Rows)
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    OdbcCommand.CommandTimeout = 2000000;
                    string OdbcSelect = SynchDentrixQRY.GetDentrixProviderOfficeHours;
                    OdbcSelect = OdbcSelect.Replace("@provider_Id", drRow["Provider_EHR_Id"].ToString());
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    //OdbcCommand.Parameters.AddWithValue("provider_Id", drRow["Provider_EHR_Id"].ToString());
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                    {
                        OdbcDt.Rows.Clear();
                    }
                    OdbcDa.Fill(OdbcDt);
                    dtProviderOfficeHours.Load(OdbcDt.CreateDataReader());
                }

                return CreateTableOfProviderOfficeHours(dtProviderOfficeHours);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        private static string GetWeekDays(int weekDayIndex)
        {
            string returnweekdaysName = "";
            try
            {
                switch (weekDayIndex)
                {
                    case 1:
                        returnweekdaysName = "Monday";
                        break;
                    case 2:
                        returnweekdaysName = "Tuesday";
                        break;
                    case 3:
                        returnweekdaysName = "Wednesday";
                        break;
                    case 4:
                        returnweekdaysName = "Thursday";
                        break;
                    case 5:
                        returnweekdaysName = "Friday";
                        break;
                    case 6:
                        returnweekdaysName = "Saturday";
                        break;
                    case 7:
                        returnweekdaysName = "Sunday";
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnweekdaysName;
        }

        private static DataTable CreateTableOfProviderOfficeHours(DataTable dtProviderOfficeHours)
        {
            DataTable dtResultProviderOfficeHours = new DataTable();
            try
            {
                #region GetDistinct Provider & Get ProviderHour Structure
                DataTable dtProvider = GetProviderIDs();
                dtResultProviderOfficeHours = SynchLocalDAL.GetLocalProviderOfficeHoursBlankStructure();
                #endregion

                #region Create PRovider & Daywise Datatable
                dtProvider.AsEnumerable()
                    .All(o =>
                    {
                        for (int i = 1; i <= 7; i++)
                        {
                            DataRow drNewRow = dtResultProviderOfficeHours.NewRow();
                            drNewRow["POH_EHR_ID"] = o["Provider_EHR_ID"].ToString() + "_" + i.ToString();
                            drNewRow["Provider_EHR_ID"] = o["Provider_EHR_ID"].ToString();
                            drNewRow["WeekDay"] = GetWeekDays(i).ToString();
                            drNewRow["Clinic_Number"] = "0";
                            drNewRow["Service_Install_Id"] = "1";
                            dtResultProviderOfficeHours.Rows.Add(drNewRow);
                        }
                        return true;
                    });
                #endregion

                #region Update Start & EndDatetime in Provider Daywise DataTable
                string daystart1 = "", daystart2 = "", daystart3 = "", dayEnd1 = "", dayEnd2 = "", dayEnd3 = "";

                dtResultProviderOfficeHours.AsEnumerable()
                    .All(o =>
                    {

                        daystart1 = o["WeekDay"].ToString() + "_start_time1";
                        daystart2 = o["WeekDay"].ToString() + "_start_time2";
                        daystart3 = o["WeekDay"].ToString() + "_start_time3";
                        dayEnd1 = o["WeekDay"].ToString() + "_end_time1";
                        dayEnd2 = o["WeekDay"].ToString() + "_end_time2";
                        dayEnd3 = o["WeekDay"].ToString() + "_end_time3";

                        var resultProvider = dtProviderOfficeHours.AsEnumerable().Where(a => a.Field<object>("Prov_Id").ToString().ToUpper() == o["Provider_EHR_ID"].ToString().ToUpper());

                        if (resultProvider.Count() > 0)
                        {
                            if (resultProvider.Select(b => b.Field<object>(daystart1)).First() != null && resultProvider.Select(b => b.Field<object>(daystart1)).First().ToString() != "")
                            {
                                o["StartTime1"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(daystart1)).First().ToString());
                            }
                            else
                            {
                                o["StartTime1"] = "01/01/2020 00:00:00";
                            }

                            if (resultProvider.Select(b => b.Field<object>(daystart2)).First() != null && resultProvider.Select(b => b.Field<object>(daystart2)).First().ToString() != "")
                            {
                                o["StartTime2"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(daystart2)).First().ToString());
                            }
                            else
                            {
                                o["StartTime2"] = "01/01/2020 00:00:00";
                            }
                            if (resultProvider.Select(b => b.Field<object>(daystart3)).First() != null && resultProvider.Select(b => b.Field<object>(daystart3)).First().ToString() != "")
                            {
                                o["StartTime3"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(daystart3)).First().ToString());
                            }
                            else
                            {
                                o["StartTime3"] = "01/01/2020 00:00:00";
                            }

                            if (resultProvider.Select(b => b.Field<object>(dayEnd1)).First() != null && resultProvider.Select(b => b.Field<object>(dayEnd1)).First().ToString() != "")
                            {
                                o["EndTime1"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(dayEnd1)).First().ToString());
                            }
                            else
                            {
                                o["EndTime1"] = "01/01/2020 00:00:00";
                            }
                            if (resultProvider.Select(b => b.Field<object>(dayEnd2)).First() != null && resultProvider.Select(b => b.Field<object>(dayEnd2)).First().ToString() != "")
                            {
                                o["EndTime2"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(dayEnd2)).First().ToString());
                            }
                            else
                            {
                                o["EndTime2"] = "01/01/2020 00:00:00";
                            }
                            if (resultProvider.Select(b => b.Field<object>(dayEnd3)).First() != null && resultProvider.Select(b => b.Field<object>(dayEnd3)).First().ToString() != "")
                            {
                                o["EndTime3"] = Convert.ToDateTime("01/01/2020" + " " + resultProvider.Select(b => b.Field<object>(dayEnd3)).First().ToString());
                            }
                            else
                            {
                                o["EndTime3"] = "01/01/2020 00:00:00";
                            }
                        }
                        else
                        {
                            o["StartTime1"] = "01/01/2020 00:00:00";
                            o["EndTime1"] = "01/01/2020 00:00:00";
                            o["StartTime2"] = "01/01/2020 00:00:00";
                            o["EndTime2"] = "01/01/2020 00:00:00";
                            o["StartTime3"] = "01/01/2020 00:00:00";
                            o["EndTime3"] = "01/01/2020 00:00:00";
                        }
                        o["Entry_DateTime"] = DateTime.Now.ToString();
                        o["Last_Sync_Date"] = DateTime.Now.ToString();
                        return true;
                    });
                #endregion

            }
            catch (Exception)
            {
                throw;
            }
            return dtResultProviderOfficeHours;
        }

        private static DataTable GetProviderIDs()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                OdbcCommand.CommandTimeout = 2000000;
                string OdbcSelect = SynchDentrixQRY.GetDentrixPRoviderIds;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        #endregion

        #endregion

        #region FolderList

        public static DataTable GetDentrixFolderListData()
        {

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixFolderListData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_FolderList_Dentrix_To_Local(DataTable dtDentrixOperatory, string Service_Install_Id, string clinicNumber)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();    
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixOperatory.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_FolderList;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_FolderList;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_FolderList;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("FolderList_EHR_ID", dr["FolderList_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Folder_Name", dr["Folder_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", clinicNumber);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.Parameters.AddWithValue("FolderOrder", 0);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region Operatory

        #region Operatory

        public static DataTable GetDentrixOperatoryData()
        {

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixOperatoryData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_Operatory_Dentrix_To_Local(DataTable dtDentrixOperatory)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();    
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixOperatory.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Operatory;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Operatory;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Operatory;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("OperatoryOrder", dr["OperatoryOrder"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region OperatoryHours

        public static DataTable GetDentrixOperatoryHoursData()
        {
            DateTime FromDate = Utility.LastSyncDateAditServer;
            DateTime ToDate = DateTime.Today.AddMonths(4);
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            DataTable dtResultOperatoryHours = new DataTable();
            CommonDB.OdbcConnectionServer(ref conn);
            DataTable dtOperatory = new DataTable();
            DataTable OdbcDt = new DataTable();
            try
            {

                dtOperatory = GetOperatoryIDs();
                dtResultOperatoryHours = SynchLocalDAL.GetLocalOperatoryHoursBlankStructure();
                foreach (DataRow drRow in dtOperatory.Rows)
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = SynchDentrixQRY.GetDentrixOperatoryHoursData;
                    //  OdbcSelect = OdbcSelect.Replace("@Operatory_Id", drRow["Operatory_EHR_Id"].ToString());
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.Parameters.AddWithValue("op_id", drRow["Operatory_EHR_Id"].ToString());
                    OdbcCommand.Parameters.AddWithValue("FromDate", FromDate);
                    OdbcCommand.Parameters.AddWithValue("ToDate", ToDate);
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                    {
                        OdbcDt.Rows.Clear();
                    }
                    OdbcDa.Fill(OdbcDt);
                    OdbcDt.AsEnumerable()
                     .All(o =>
                     {

                         for (int i = 1; i <= 3; i++)
                         {
                             if (o["start_time" + i.ToString()] != null && o["start_time" + i.ToString()].ToString() != string.Empty)
                             {
                                 DataRow drNewRow = dtResultOperatoryHours.NewRow();
                                 drNewRow["OH_EHR_ID"] = Convert.ToDateTime(o["sched_exception_date"].ToString()).ToString("yyyyMMdd") + "_" + o["op_id"].ToString() + "_" + i.ToString();
                                 drNewRow["Operatory_EHR_ID"] = o["op_id"].ToString();
                                 drNewRow["OH_Web_ID"] = "";
                                 drNewRow["StartTime"] = Convert.ToDateTime(Convert.ToDateTime(o["sched_exception_date"].ToString()).ToString("yyyy/MM/dd") + " " + o["start_time" + i.ToString()].ToString());
                                 drNewRow["EndTime"] = Convert.ToDateTime(Convert.ToDateTime(o["sched_exception_date"].ToString()).ToString("yyyy/MM/dd") + " " + o["end_time" + i.ToString()].ToString());
                                 drNewRow["Entry_DateTime"] = Convert.ToDateTime(o["modified_time_stamp"].ToString());
                                 dtResultOperatoryHours.Rows.Add(drNewRow);
                             }
                         }
                         return true;
                     });

                }

                return dtResultOperatoryHours;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_OperatoryHours_Dentrix_To_Local(DataTable dtDentrixOperatoryHours)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixOperatoryHours.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryHours;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryHours;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryHours;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("OH_EHR_ID", dr["OH_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("OH_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());

                                SqlCeCommand.Parameters.AddWithValue("StartTime", dr["StartTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("EndTime", dr["EndTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dr["Entry_DateTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region OperatoryOfficeHours

        public static DataTable GetDentrixOperatoryOfficeHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            DataTable dtOperatoryOfficeHours = new DataTable();
            CommonDB.OdbcConnectionServer(ref conn);
            DataTable dtOperatory = new DataTable();
            DataTable OdbcDt = new DataTable();
            try
            {

                dtOperatory = GetOperatoryIDs();

                foreach (DataRow drRow in dtOperatory.Rows)
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = SynchDentrixQRY.GetDentrixOperatoryOfficeHours;
                    OdbcSelect = OdbcSelect.Replace("@op_id", drRow["Operatory_EHR_Id"].ToString());
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    //OdbcCommand.Parameters.AddWithValue("op_id", drRow["Operatory_EHR_Id"].ToString());
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                    {
                        OdbcDt.Rows.Clear();
                    }
                    OdbcDa.Fill(OdbcDt);
                    dtOperatoryOfficeHours.Load(OdbcDt.CreateDataReader());
                }

                return CreateTableOfOperatoryOfficeHours(dtOperatoryOfficeHours);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private static DataTable GetOperatoryIDs()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string OdbcSelect = SynchDentrixQRY.GetDentrixOperatoryIds;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private static DataTable CreateTableOfOperatoryOfficeHours(DataTable dtOperatoryOfficeHours)
        {
            DataTable dtResultOperatoryOfficeHours = new DataTable();
            try
            {
                #region GetDistinct Operatory & Get OperatoryHour Structure
                DataTable dtOperatory = GetOperatoryIDs();
                dtResultOperatoryOfficeHours = SynchLocalDAL.GetLocalOperatoryOfficeHoursBlankStructure();
                #endregion

                #region Create Operatory & Daywise Datatable
                dtOperatory.AsEnumerable()
                    .All(o =>
                    {
                        for (int i = 1; i <= 7; i++)
                        {
                            DataRow drNewRow = dtResultOperatoryOfficeHours.NewRow();
                            drNewRow["OOH_EHR_ID"] = o["Operatory_EHR_ID"].ToString() + "_" + i.ToString();
                            drNewRow["Operatory_EHR_ID"] = o["Operatory_EHR_ID"].ToString();
                            drNewRow["WeekDay"] = GetWeekDays(i).ToString();
                            drNewRow["Clinic_Number"] = "0";
                            drNewRow["Service_Install_Id"] = "1";
                            dtResultOperatoryOfficeHours.Rows.Add(drNewRow);
                        }
                        return true;
                    });
                #endregion

                #region Update Start & EndDatetime in Operatory Daywise DataTable
                string daystart1 = "", daystart2 = "", daystart3 = "", dayEnd1 = "", dayEnd2 = "", dayEnd3 = "";

                dtResultOperatoryOfficeHours.AsEnumerable()
                    .All(o =>
                    {
                        daystart1 = o["WeekDay"].ToString() + "_start_time1";
                        daystart2 = o["WeekDay"].ToString() + "_start_time2";
                        daystart3 = o["WeekDay"].ToString() + "_start_time3";
                        dayEnd1 = o["WeekDay"].ToString() + "_end_time1";
                        dayEnd2 = o["WeekDay"].ToString() + "_end_time2";
                        dayEnd3 = o["WeekDay"].ToString() + "_end_time3";

                        var resultOperatory = dtOperatoryOfficeHours.AsEnumerable().Where(a => a.Field<object>("op_id").ToString().ToUpper() == o["Operatory_EHR_ID"].ToString().ToUpper());

                        if (resultOperatory.Count() > 0)
                        {
                            if (resultOperatory.Select(b => b.Field<object>(daystart1)).First() != null && resultOperatory.Select(b => b.Field<object>(daystart1)).First().ToString() != "")
                            {
                                o["StartTime1"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(daystart1)).First().ToString());
                            }
                            else
                            {
                                o["StartTime1"] = "01/01/2020 00:00:00";
                            }

                            if (resultOperatory.Select(b => b.Field<object>(daystart2)).First() != null && resultOperatory.Select(b => b.Field<object>(daystart2)).First().ToString() != "")
                            {
                                o["StartTime2"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(daystart2)).First().ToString());
                            }
                            else
                            {
                                o["StartTime2"] = "01/01/2020 00:00:00";
                            }
                            if (resultOperatory.Select(b => b.Field<object>(daystart3)).First() != null && resultOperatory.Select(b => b.Field<object>(daystart3)).First().ToString() != "")
                            {
                                o["StartTime3"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(daystart3)).First().ToString());
                            }
                            else
                            {
                                o["StartTime3"] = "01/01/2020 00:00:00";
                            }

                            if (resultOperatory.Select(b => b.Field<object>(dayEnd1)).First() != null && resultOperatory.Select(b => b.Field<object>(dayEnd1)).First().ToString() != "")
                            {
                                o["EndTime1"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(dayEnd1)).First().ToString());
                            }
                            else
                            {
                                o["EndTime1"] = "01/01/2020 00:00:00";
                            }
                            if (resultOperatory.Select(b => b.Field<object>(dayEnd2)).First() != null && resultOperatory.Select(b => b.Field<object>(dayEnd2)).First().ToString() != "")
                            {
                                o["EndTime2"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(dayEnd2)).First().ToString());
                            }
                            else
                            {
                                o["EndTime2"] = "01/01/2020 00:00:00";
                            }
                            if (resultOperatory.Select(b => b.Field<object>(dayEnd3)).First() != null && resultOperatory.Select(b => b.Field<object>(dayEnd3)).First().ToString() != "")
                            {
                                o["EndTime3"] = Convert.ToDateTime("01/01/2020" + " " + resultOperatory.Select(b => b.Field<object>(dayEnd3)).First().ToString());
                            }
                            else
                            {
                                o["EndTime3"] = "01/01/2020 00:00:00";
                            }
                        }
                        else
                        {
                            o["StartTime1"] = "01/01/2020 00:00:00";
                            o["EndTime1"] = "01/01/2020 00:00:00";
                            o["StartTime2"] = "01/01/2020 00:00:00";
                            o["EndTime2"] = "01/01/2020 00:00:00";
                            o["StartTime3"] = "01/01/2020 00:00:00";
                            o["EndTime3"] = "01/01/2020 00:00:00";
                        }
                        return true;
                    });
                #endregion

            }
            catch (Exception)
            {
                throw;
            }
            return dtResultOperatoryOfficeHours;
        }

        #endregion

        #endregion

        #region ApptType

        public static DataTable GetDentrixApptTypeData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixApptTypeData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_ApptType_Dentrix_To_Local(DataTable dtDentrixApptType)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open();  
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixApptType.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptType;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_ApptType;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ApptType_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString().Trim().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    // SqlCetx.Commit();

                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region Patient

        public static DataTable GetDentrixPatientData(string strPatID = "")
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                if ((Utility.Application_Version.ToLower() != "DTX G5".ToLower()))
                    return GetDentrixPatientDataNew(false, strPatID);

                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                {
                    if (strPatID == "")
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatientDataG5;
                    }
                    else
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixNewAllPatientDataG5.Replace("@PaientEHRIDs", strPatID);
                    }
                }
                else
                {
                    if (strPatID == "")
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatientData; //rooja query change for task : https://app.asana.com/0/1203599217474380/1207588778429011/f
                    }
                    else
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixNewAllPatientData.Replace("@PaientEHRIDs", strPatID);
                    }
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixNewPatientData()
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixNewPatientData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixPatientStatusData(string strPatID = "")
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixPatientStatusNew_Existing;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect + " And patid = '" + strPatID + "'";
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Clear();
                //  OdbcSelect = OdbcSelect.Replace("getdate()", "'" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'");
                // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.AddWithValue("@ToLessDate", ToDate.ToString("yyyy/MM/dd"));
                OdbcCommand.Parameters.AddWithValue("@ToEqualDate", ToDate.ToString("yyyy/MM/dd"));
                OdbcCommand.Parameters.AddWithValue("@ToTime", ToDate.Hour);
                OdbcCommand.Parameters.AddWithValue("@ToMin", ToDate.Minute);
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixAppointmentsPatientData(string strPatID = "")
        {
            if ((Utility.Application_Version.ToLower() != "DTX G5".ToLower()))
                return GetDentrixAppointmentsPatientDataNew(true, strPatID);

            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixAppointmentsPatientDataG5;
                }
                else
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixAppointmentsPatientData;
                }

                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect + " And v_p.patient_id = '" + strPatID + "'";
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDateAppt", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixPatientDataNew(bool isApptPatient, string strPatID = "")
        {
            DataTable OdbcDt = new DataTable();
            DataTable dtSub = new DataTable();
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                DateTime ToDate = dtCurrentDtTime;

                OdbcCommand.CommandTimeout = 200;
                string OdbcSelect = "";
                if (isApptPatient)
                {
                    OdbcSelect = SynchDentrixQRY.GetAppointmentPatientData1;
                }
                else
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixPatientData1;
                }
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect + " and v_p.patid = '" + strPatID + "'";
                }

                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(OdbcDt);
                #region Employer
                OdbcDt.Columns.Add("employer", typeof(string));
                dtSub = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataEmpl;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtSub);

                var empl = from r1 in OdbcDt.AsEnumerable()
                           join r2 in dtSub.AsEnumerable()
                           on r1["empid"].ToString().Trim() equals r2["empid"].ToString().Trim()
                           into outer
                           from r2 in outer.DefaultIfEmpty()
                           select new { r1, employer = (r2 == null ? "" : r2["name"]) };
                foreach (var x in empl)
                {
                    x.r1.SetField("employer", x.employer == DBNull.Value ? "" : Convert.ToString(x.employer));
                }
                #endregion

                #region ClaimInfo
                OdbcDt.Columns.Add("school", typeof(string));
                dtSub = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataClaimInfo;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtSub);

                var ClaimInfo = from r1 in OdbcDt.AsEnumerable()
                                join r2 in dtSub.AsEnumerable()
                                on r1["claiminfid"].ToString().Trim() equals r2["claiminfid"].ToString().Trim()
                                into outer
                                from r2 in outer.DefaultIfEmpty()
                                select new { r1, school = (r2 == null ? "" : r2["school"]) };
                foreach (var x in ClaimInfo)
                {
                    x.r1.SetField("school", x.school == DBNull.Value ? "" : Convert.ToString(x.school));
                }
                #endregion

                #region Insurance

                OdbcDt.Columns.Add("Primary_Insurance", typeof(string));
                OdbcDt.Columns.Add("Primary_Insurance_CompanyName", typeof(string));
                OdbcDt.Columns.Add("Secondary_Insurance", typeof(string));
                OdbcDt.Columns.Add("Secondary_Insurance_CompanyName", typeof(string));
                OdbcDt.Columns.Add("remaining_benefit", typeof(string));
                OdbcDt.Columns.Add("used_benefit", typeof(string));
                OdbcDt.Columns.Add("Prim_Ins_Company_Phonenumber", typeof(string));
                OdbcDt.Columns.Add("Sec_Ins_Company_Phonenumber", typeof(string));
                OdbcDt.Columns.Add("Primary_Ins_Subscriber_ID", typeof(string));
                OdbcDt.Columns.Add("Secondary_Ins_Subscriber_ID", typeof(string));

                DataTable dtInsurance = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatietnDataInsurance;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect + " and p.patid in ('" + strPatID + "')";
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtInsurance);

                var Insurance = from r1 in OdbcDt.AsEnumerable()
                                join r2 in dtInsurance.AsEnumerable()
                                on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patid"].ToString().Trim()
                                into outer
                                from r2 in outer.DefaultIfEmpty()
                                select new
                                {
                                    r1,
                                    Primary_Insurance = (r2 == null ? "" : r2["Primary_Insurance"]),
                                    Primary_Insurance_CompanyName = (r2 == null ? "" : r2["Primary_Insurance_CompanyName"]),
                                    Secondary_Insurance = (r2 == null ? "" : r2["Secondary_Insurance"]),
                                    Secondary_Insurance_CompanyName = (r2 == null ? "" : r2["Secondary_Insurance_CompanyName"]),
                                    remaining_benefit = (r2 == null ? "" : r2["remaining_benefit"]),
                                    used_benefit = (r2 == null ? "" : r2["used_benefit"]),
                                    Prim_Ins_Company_Phonenumber = (r2 == null ? "" : r2["Prim_Ins_Company_Phonenumber"]),
                                    Sec_Ins_Company_Phonenumber = (r2 == null ? "" : r2["Sec_Ins_Company_Phonenumber"]),
                                    Primary_Ins_Subscriber_ID = (r2 == null ? "" : r2["Primary_Ins_Subscriber_ID"]),
                                    Secondary_Ins_Subscriber_ID = (r2 == null ? "" : r2["Secondary_Ins_Subscriber_ID"])
                                };
                foreach (var x in Insurance)
                {
                    x.r1.SetField("Primary_Insurance", x.Primary_Insurance == DBNull.Value ? "" : Convert.ToString(x.Primary_Insurance));
                    x.r1.SetField("Primary_Insurance_CompanyName", x.Primary_Insurance_CompanyName == DBNull.Value ? "" : Convert.ToString(x.Primary_Insurance_CompanyName));
                    x.r1.SetField("Secondary_Insurance", x.Secondary_Insurance == DBNull.Value ? "" : Convert.ToString(x.Secondary_Insurance));
                    x.r1.SetField("Secondary_Insurance_CompanyName", x.Secondary_Insurance_CompanyName == DBNull.Value ? "" : Convert.ToString(x.Secondary_Insurance_CompanyName));
                    x.r1.SetField("remaining_benefit", x.remaining_benefit == DBNull.Value ? "" : Convert.ToString(x.remaining_benefit));
                    x.r1.SetField("used_benefit", x.used_benefit == DBNull.Value ? "" : Convert.ToString(x.used_benefit));
                    x.r1.SetField("Prim_Ins_Company_Phonenumber", x.Prim_Ins_Company_Phonenumber == DBNull.Value ? "" : Convert.ToString(x.Prim_Ins_Company_Phonenumber));
                    x.r1.SetField("Sec_Ins_Company_Phonenumber", x.Sec_Ins_Company_Phonenumber == DBNull.Value ? "" : Convert.ToString(x.Sec_Ins_Company_Phonenumber));
                    x.r1.SetField("Primary_Ins_Subscriber_ID", x.Primary_Ins_Subscriber_ID == DBNull.Value ? "" : Convert.ToString(x.Primary_Ins_Subscriber_ID));
                    x.r1.SetField("Secondary_Ins_Subscriber_ID", x.Secondary_Ins_Subscriber_ID == DBNull.Value ? "" : Convert.ToString(x.Secondary_Ins_Subscriber_ID));
                }

                #endregion

                #region GuraratorAndBalance
                OdbcDt.Columns.Add("responsiblepartyId", typeof(string));
                OdbcDt.Columns.Add("ResponsibleParty_First_Name", typeof(string));
                OdbcDt.Columns.Add("ResponsibleParty_Last_Name", typeof(string));
                OdbcDt.Columns.Add("responsiblepartyssn", typeof(string));
                OdbcDt.Columns.Add("responsiblepartybirthdate", typeof(string));
                OdbcDt.Columns.Add("CurrentBal", typeof(string));
                OdbcDt.Columns.Add("ThirtyDay", typeof(string));
                OdbcDt.Columns.Add("SixtyDay", typeof(string));
                OdbcDt.Columns.Add("NinetyDay", typeof(string));
                OdbcDt.Columns.Add("Over90", typeof(string));


                dtSub = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataGurBalance;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtSub);

                var GurBal = from r1 in OdbcDt.AsEnumerable()
                             join r2 in dtSub.AsEnumerable()
                             on r1["guar_id"].ToString().Trim() equals r2["responsiblepartyId"].ToString().Trim()
                             into outer
                             from r2 in outer.DefaultIfEmpty()
                             select new
                             {
                                 r1,
                                 responsiblepartyId = (r2 == null ? "" : r2["responsiblepartyId"]),
                                 ResponsibleParty_First_Name = (r2 == null ? "" : r2["ResponsibleParty_First_Name"]),
                                 ResponsibleParty_Last_Name = (r2 == null ? "" : r2["ResponsibleParty_Last_Name"]),
                                 responsiblepartyssn = (r2 == null ? "" : r2["responsiblepartyssn"]),
                                 responsiblepartybirthdate = (r2 == null ? "" : r2["responsiblepartybirthdate"]),
                                 CurrentBal = (r2 == null ? "" : r2["CurrentBal"]),
                                 ThirtyDay = (r2 == null ? "" : r2["ThirtyDay"]),
                                 SixtyDay = (r2 == null ? "" : r2["SixtyDay"]),
                                 NinetyDay = (r2 == null ? "" : r2["NinetyDay"]),
                                 Over90 = (r2 == null ? "" : r2["Over90"])
                             };
                foreach (var x in GurBal)
                {
                    x.r1.SetField("responsiblepartyId", x.responsiblepartyId == DBNull.Value ? "" : Convert.ToString(x.responsiblepartyId));
                    x.r1.SetField("ResponsibleParty_First_Name", x.ResponsibleParty_First_Name == DBNull.Value ? "" : Convert.ToString(x.ResponsibleParty_First_Name));
                    x.r1.SetField("ResponsibleParty_Last_Name", x.ResponsibleParty_Last_Name == DBNull.Value ? "" : Convert.ToString(x.ResponsibleParty_Last_Name));
                    x.r1.SetField("responsiblepartyssn", x.responsiblepartyssn == DBNull.Value ? "" : Convert.ToString(x.responsiblepartyssn));
                    x.r1.SetField("responsiblepartybirthdate", x.responsiblepartybirthdate == DBNull.Value ? "" : Convert.ToString(x.responsiblepartybirthdate));
                    x.r1.SetField("CurrentBal", x.CurrentBal == DBNull.Value ? "" : Convert.ToString(x.CurrentBal));
                    x.r1.SetField("ThirtyDay", x.ThirtyDay == DBNull.Value ? "" : Convert.ToString(x.ThirtyDay));
                    x.r1.SetField("SixtyDay", x.SixtyDay == DBNull.Value ? "" : Convert.ToString(x.SixtyDay));
                    x.r1.SetField("NinetyDay", x.NinetyDay == DBNull.Value ? "" : Convert.ToString(x.NinetyDay));
                    x.r1.SetField("Over90", x.Over90 == DBNull.Value ? "" : Convert.ToString(x.Over90));
                }
                #endregion


                #region NextVisitDate
                OdbcDt.Columns.Add("Nextvisit_date", typeof(string));
                DataTable dtNextVisit = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataNextVisitDate;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect.Replace("group by ap.patid", " and ap.patid in ('" + strPatID + "') group by ap.patid");
                }
                OdbcSelect = OdbcSelect.Replace("@ToDate", ToDate.ToString("yyyy/MM/dd"));

                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Clear();
                //OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtNextVisit);
                
                DataTable newDtNextVisit = new DataTable();
                newDtNextVisit.Columns.Add("patient_id");
                newDtNextVisit.Columns.Add("Nextvisit_date");

                var minDates = dtNextVisit.AsEnumerable()
                            .GroupBy(row => row["patient_id"])
                            .Select(g => new
                            {
                                PatID = g.Key,
                                NextVisitDate = g.Min(row => row["apptdate"]),
                                NextVisitHour = g.Min(row => row["timehr"]),
                                NextVisitMin = g.Min(row => row["timemin"])
                            });

                foreach (var entry in minDates)
                {
                    string PatID = Convert.ToString(entry.PatID);
                    string NextVisitDate = Convert.ToString(entry.NextVisitDate);
                    string NextVisitHour = Convert.ToString(entry.NextVisitHour);
                    string NextVisitMin = Convert.ToString(entry.NextVisitMin);

                    DateTime dtNextVisitDate;
                    DateTime.TryParse(NextVisitDate, out dtNextVisitDate);

                    if (dtNextVisitDate != null)
                    {
                        dtNextVisitDate = dtNextVisitDate.AddHours(Convert.ToInt32(NextVisitHour)).AddMinutes(Convert.ToInt32(NextVisitMin));
                    }
                    newDtNextVisit.Rows.Add(entry.PatID, dtNextVisitDate);
                }


                try
                {
                    var NextVisitDate1 = from r1 in OdbcDt.AsEnumerable()
                                         join r2 in newDtNextVisit.AsEnumerable()
                                         on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patient_id"].ToString().Trim()
                                         into outer
                                         from r2 in outer.DefaultIfEmpty()
                                         select new { r1, NVD = (r2 != null ? (r2["Nextvisit_date"] != DBNull.Value && r2["Nextvisit_date"] != null ? r2["Nextvisit_date"] : "") : "") };
                 
                    var NextVisitDate = from r1 in OdbcDt.AsEnumerable()
                                        join r2 in newDtNextVisit.AsEnumerable()
                                        on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patient_id"].ToString().Trim()
                                        into outer
                                        from r2 in outer.DefaultIfEmpty()
                                        select new { r1, Nextvisit_date = (r2 != null ? (r2["Nextvisit_date"] != DBNull.Value && r2["Nextvisit_date"] != null ? r2["Nextvisit_date"] : "") : "") };
                 

                    foreach (var x in NextVisitDate)
                    {
                        x.r1.SetField("Nextvisit_date", x.Nextvisit_date == DBNull.Value ? "" : x.Nextvisit_date);
                    }
                }
                catch (Exception ex3)
                {
                    throw ex3;
                }
                #endregion

                #region CollectPayment
                OdbcDt.Columns.Add("collect_payment", typeof(string));
                dtSub = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataCollectPayment;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect.Replace("group by patid", "and patid in ('" + strPatID + "') group by patid");
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtSub);

                var collectPayment = from r1 in OdbcDt.AsEnumerable()
                                     join r2 in dtSub.AsEnumerable()
                                     on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patid"].ToString().Trim()
                                     into outer
                                     from r2 in outer.DefaultIfEmpty()
                                     select new { r1, collect_payment = (r2 == null ? "0" : r2["collect_payment"]) };
                foreach (var x in collectPayment)
                {
                    x.r1.SetField("collect_payment", x.collect_payment == DBNull.Value ? "0" : Convert.ToString(x.collect_payment));
                }
                #endregion

                #region PatientNotes
                OdbcDt.Columns.Add("patient_Note", typeof(string));
                OdbcDt.Columns.Add("PreferredLanguage", typeof(string));

                dtSub = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataPatientNotes;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect + " and Patid in ('" + strPatID + "')";
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtSub);

                var PatNote = from r1 in OdbcDt.AsEnumerable()
                              join r2 in dtSub.AsEnumerable()
                              on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patid"].ToString().Trim()
                              into outer
                              from r2 in outer.DefaultIfEmpty()
                              select new { r1, PatNote = (r2 == null ? "" : r2["patient_Note"]) };
                foreach (var x in PatNote)
                {
                    x.r1.SetField("patient_Note", x.PatNote == DBNull.Value ? "" : Convert.ToString(x.PatNote));
                    x.r1.SetField("PreferredLanguage", x.PatNote == DBNull.Value ? "" : Convert.ToString(x.PatNote).ToLower().Contains("spanish") ? "Spanish" : Convert.ToString(x.PatNote).ToLower().Contains("french") ? "French" : "English");
                }
                #endregion
                OdbcDt.Columns.Add("due_date", typeof(string));
                OdbcDt.Columns.Add("groupid", typeof(string));
                OdbcDt.Columns.Add("emergencycontactId", typeof(string));
                OdbcDt.Columns.Add("emergencycontactnumber", typeof(string));
                OdbcDt.Columns.Add("spouseId", typeof(string));
                OdbcDt.Columns.Add("Spouse_First_Name", typeof(string));
                OdbcDt.Columns.Add("Spouse_Last_Name", typeof(string));
                OdbcDt.Columns.Add("EmergencyContact_First_Name", typeof(string));
                OdbcDt.Columns.Add("EmergencyContact_Last_Name", typeof(string));
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixAppointmentsPatientDataNew(bool isApptPatient, string strPatID = "")
        {
            DataTable OdbcDt = new DataTable();
            DataTable dtSub = new DataTable();
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                DateTime ToDate = dtCurrentDtTime;

                OdbcCommand.CommandTimeout = 200;
                string OdbcSelect = "";
                if (isApptPatient)
                {
                    OdbcSelect = SynchDentrixQRY.GetAppointmentPatientData1;
                }
                else
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixPatientData1;
                }
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect + " and v_p.patid = '" + strPatID + "'";
                }

                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(OdbcDt);

                #region Employer
                OdbcDt.Columns.Add("employer", typeof(string));
                dtSub = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataEmpl;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtSub);

                var empl = from r1 in OdbcDt.AsEnumerable()
                           join r2 in dtSub.AsEnumerable()
                           on r1["empid"].ToString().Trim() equals r2["empid"].ToString().Trim()
                           into outer
                           from r2 in outer.DefaultIfEmpty()
                           select new { r1, employer = (r2 == null ? "" : r2["name"]) };
                foreach (var x in empl)
                {
                    x.r1.SetField("employer", x.employer == DBNull.Value ? "" : Convert.ToString(x.employer));
                }
                #endregion

                #region ClaimInfo
                OdbcDt.Columns.Add("school", typeof(string));
                dtSub = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataClaimInfo;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtSub);

                var ClaimInfo = from r1 in OdbcDt.AsEnumerable()
                                join r2 in dtSub.AsEnumerable()
                                on r1["claiminfid"].ToString().Trim() equals r2["claiminfid"].ToString().Trim()
                                into outer
                                from r2 in outer.DefaultIfEmpty()
                                select new { r1, school = (r2 == null ? "" : r2["school"]) };
                foreach (var x in ClaimInfo)
                {
                    x.r1.SetField("school", x.school == DBNull.Value ? "" : Convert.ToString(x.school));
                }
                #endregion

                #region Insurance

                OdbcDt.Columns.Add("Primary_Insurance", typeof(string));
                OdbcDt.Columns.Add("Primary_Insurance_CompanyName", typeof(string));
                OdbcDt.Columns.Add("Secondary_Insurance", typeof(string));
                OdbcDt.Columns.Add("Secondary_Insurance_CompanyName", typeof(string));
                OdbcDt.Columns.Add("remaining_benefit", typeof(string));
                OdbcDt.Columns.Add("used_benefit", typeof(string));
                OdbcDt.Columns.Add("Prim_Ins_Company_Phonenumber", typeof(string));
                OdbcDt.Columns.Add("Sec_Ins_Company_Phonenumber", typeof(string));
                OdbcDt.Columns.Add("Primary_Ins_Subscriber_ID", typeof(string));
                OdbcDt.Columns.Add("Secondary_Ins_Subscriber_ID", typeof(string));

                DataTable dtInsurance = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatietnDataInsurance;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect + " and p.patid in ('" + strPatID + "')";
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtInsurance);

                var Insurance = from r1 in OdbcDt.AsEnumerable()
                                join r2 in dtInsurance.AsEnumerable()
                                on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patid"].ToString().Trim()
                                into outer
                                from r2 in outer.DefaultIfEmpty()
                                select new
                                {
                                    r1,
                                    Primary_Insurance = (r2 == null ? "" : r2["Primary_Insurance"]),
                                    Primary_Insurance_CompanyName = (r2 == null ? "" : r2["Primary_Insurance_CompanyName"]),
                                    Secondary_Insurance = (r2 == null ? "" : r2["Secondary_Insurance"]),
                                    Secondary_Insurance_CompanyName = (r2 == null ? "" : r2["Secondary_Insurance_CompanyName"]),
                                    remaining_benefit = (r2 == null ? "" : r2["remaining_benefit"]),
                                    used_benefit = (r2 == null ? "" : r2["used_benefit"]),
                                    Prim_Ins_Company_Phonenumber = (r2 == null ? "" : r2["Prim_Ins_Company_Phonenumber"]),
                                    Sec_Ins_Company_Phonenumber = (r2 == null ? "" : r2["Sec_Ins_Company_Phonenumber"]),
                                    Primary_Ins_Subscriber_ID = (r2 == null ? "" : r2["Primary_Ins_Subscriber_ID"]),
                                    Secondary_Ins_Subscriber_ID = (r2 == null ? "" : r2["Secondary_Ins_Subscriber_ID"])
                                };
                foreach (var x in Insurance)
                {
                    x.r1.SetField("Primary_Insurance", x.Primary_Insurance == DBNull.Value ? "" : Convert.ToString(x.Primary_Insurance));
                    x.r1.SetField("Primary_Insurance_CompanyName", x.Primary_Insurance_CompanyName == DBNull.Value ? "" : Convert.ToString(x.Primary_Insurance_CompanyName));
                    x.r1.SetField("Secondary_Insurance", x.Secondary_Insurance == DBNull.Value ? "" : Convert.ToString(x.Secondary_Insurance));
                    x.r1.SetField("Secondary_Insurance_CompanyName", x.Secondary_Insurance_CompanyName == DBNull.Value ? "" : Convert.ToString(x.Secondary_Insurance_CompanyName));
                    x.r1.SetField("remaining_benefit", x.remaining_benefit == DBNull.Value ? "" : Convert.ToString(x.remaining_benefit));
                    x.r1.SetField("used_benefit", x.used_benefit == DBNull.Value ? "" : Convert.ToString(x.used_benefit));
                    x.r1.SetField("Prim_Ins_Company_Phonenumber", x.Prim_Ins_Company_Phonenumber == DBNull.Value ? "" : Convert.ToString(x.Prim_Ins_Company_Phonenumber));
                    x.r1.SetField("Sec_Ins_Company_Phonenumber", x.Sec_Ins_Company_Phonenumber == DBNull.Value ? "" : Convert.ToString(x.Sec_Ins_Company_Phonenumber));
                    x.r1.SetField("Primary_Ins_Subscriber_ID", x.Primary_Ins_Subscriber_ID == DBNull.Value ? "" : Convert.ToString(x.Primary_Ins_Subscriber_ID));
                    x.r1.SetField("Secondary_Ins_Subscriber_ID", x.Secondary_Ins_Subscriber_ID == DBNull.Value ? "" : Convert.ToString(x.Secondary_Ins_Subscriber_ID));
                }

                #endregion

                #region GuraratorAndBalance
                OdbcDt.Columns.Add("responsiblepartyId", typeof(string));
                OdbcDt.Columns.Add("ResponsibleParty_First_Name", typeof(string));
                OdbcDt.Columns.Add("ResponsibleParty_Last_Name", typeof(string));
                OdbcDt.Columns.Add("responsiblepartyssn", typeof(string));
                OdbcDt.Columns.Add("responsiblepartybirthdate", typeof(string));
                OdbcDt.Columns.Add("CurrentBal", typeof(string));
                OdbcDt.Columns.Add("ThirtyDay", typeof(string));
                OdbcDt.Columns.Add("SixtyDay", typeof(string));
                OdbcDt.Columns.Add("NinetyDay", typeof(string));
                OdbcDt.Columns.Add("Over90", typeof(string));


                dtSub = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataGurBalance;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtSub);

                var GurBal = from r1 in OdbcDt.AsEnumerable()
                             join r2 in dtSub.AsEnumerable()
                             on r1["guar_id"].ToString().Trim() equals r2["responsiblepartyId"].ToString().Trim()
                             into outer
                             from r2 in outer.DefaultIfEmpty()
                             select new
                             {
                                 r1,
                                 responsiblepartyId = (r2 == null ? "" : r2["responsiblepartyId"]),
                                 ResponsibleParty_First_Name = (r2 == null ? "" : r2["ResponsibleParty_First_Name"]),
                                 ResponsibleParty_Last_Name = (r2 == null ? "" : r2["ResponsibleParty_Last_Name"]),
                                 responsiblepartyssn = (r2 == null ? "" : r2["responsiblepartyssn"]),
                                 responsiblepartybirthdate = (r2 == null ? "" : r2["responsiblepartybirthdate"]),
                                 CurrentBal = (r2 == null ? "" : r2["CurrentBal"]),
                                 ThirtyDay = (r2 == null ? "" : r2["ThirtyDay"]),
                                 SixtyDay = (r2 == null ? "" : r2["SixtyDay"]),
                                 NinetyDay = (r2 == null ? "" : r2["NinetyDay"]),
                                 Over90 = (r2 == null ? "" : r2["Over90"])
                             };
                foreach (var x in GurBal)
                {
                    x.r1.SetField("responsiblepartyId", x.responsiblepartyId == DBNull.Value ? "" : Convert.ToString(x.responsiblepartyId));
                    x.r1.SetField("ResponsibleParty_First_Name", x.ResponsibleParty_First_Name == DBNull.Value ? "" : Convert.ToString(x.ResponsibleParty_First_Name));
                    x.r1.SetField("ResponsibleParty_Last_Name", x.ResponsibleParty_Last_Name == DBNull.Value ? "" : Convert.ToString(x.ResponsibleParty_Last_Name));
                    x.r1.SetField("responsiblepartyssn", x.responsiblepartyssn == DBNull.Value ? "" : Convert.ToString(x.responsiblepartyssn));
                    x.r1.SetField("responsiblepartybirthdate", x.responsiblepartybirthdate == DBNull.Value ? "" : Convert.ToString(x.responsiblepartybirthdate));
                    x.r1.SetField("CurrentBal", x.CurrentBal == DBNull.Value ? "" : Convert.ToString(x.CurrentBal));
                    x.r1.SetField("ThirtyDay", x.ThirtyDay == DBNull.Value ? "" : Convert.ToString(x.ThirtyDay));
                    x.r1.SetField("SixtyDay", x.SixtyDay == DBNull.Value ? "" : Convert.ToString(x.SixtyDay));
                    x.r1.SetField("NinetyDay", x.NinetyDay == DBNull.Value ? "" : Convert.ToString(x.NinetyDay));
                    x.r1.SetField("Over90", x.Over90 == DBNull.Value ? "" : Convert.ToString(x.Over90));
                }
                #endregion

                #region NextVisitDate
                OdbcDt.Columns.Add("Nextvisit_date", typeof(string));
                DataTable dtNextVisit = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataNextVisitDate;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect.Replace("group by ap.patid", " and ap.patid in ('" + strPatID + "') group by ap.patid");
                }
                OdbcSelect = OdbcSelect.Replace("@ToDate", ToDate.ToString("yyyy/MM/dd"));
                
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Clear();
                //OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtNextVisit);

                DataTable newDtNextVisit = new DataTable();
                newDtNextVisit.Columns.Add("patient_id");
                newDtNextVisit.Columns.Add("Nextvisit_date");

                var minDates = dtNextVisit.AsEnumerable()
                            .GroupBy(row => row["patient_id"])
                            .Select(g => new
                            {
                                    PatID = g.Key,
                                    NextVisitDate = g.Min(row => row["apptdate"]),
                                    NextVisitHour = g.Min(row => row["timehr"]),
                                    NextVisitMin = g.Min(row => row["timemin"])
                            });

                foreach (var entry in minDates)
                {
                    string PatID = Convert.ToString(entry.PatID);
                    string NextVisitDate = Convert.ToString(entry.NextVisitDate);
                    string NextVisitHour = Convert.ToString(entry.NextVisitHour);
                    string NextVisitMin = Convert.ToString(entry.NextVisitMin);

                    DateTime dtNextVisitDate;
                    DateTime.TryParse(NextVisitDate, out dtNextVisitDate);

                    if (dtNextVisitDate != null)
                    {
                        dtNextVisitDate = dtNextVisitDate.AddHours(Convert.ToInt32(NextVisitHour)).AddMinutes(Convert.ToInt32(NextVisitMin));
                    }
                    newDtNextVisit.Rows.Add(entry.PatID, dtNextVisitDate);
                }


                try
                {
                    var NextVisitDate1 = from r1 in OdbcDt.AsEnumerable()
                                         join r2 in newDtNextVisit.AsEnumerable()
                                         on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patient_id"].ToString().Trim()
                                         into outer
                                         from r2 in outer.DefaultIfEmpty()
                                         select new { r1, r2 };

                    var NextVisitDate = from r1 in OdbcDt.AsEnumerable()
                                        join r2 in newDtNextVisit.AsEnumerable()
                                        on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patient_id"].ToString().Trim()
                                        into outer
                                        from r2 in outer.DefaultIfEmpty()
                                        select new { r1, Nextvisit_date = r2 != null ? r2["Nextvisit_date"] : r1["Nextvisit_date"] };

                    foreach (var x in NextVisitDate)
                    {
                        x.r1.SetField("Nextvisit_date", x.Nextvisit_date == DBNull.Value ? "" : x.Nextvisit_date);
                    }
                }
                catch (Exception ex3)
                {
                    throw ex3;
                }
                #endregion

                #region CollectPayment
                OdbcDt.Columns.Add("collect_payment", typeof(string));
                dtSub = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataCollectPayment;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect.Replace("group by patid", "and patid in ('" + strPatID + "') group by patid");
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtSub);

                var collectPayment = from r1 in OdbcDt.AsEnumerable()
                                     join r2 in dtSub.AsEnumerable()
                                     on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patid"].ToString().Trim()
                                     into outer
                                     from r2 in outer.DefaultIfEmpty()
                                     select new { r1, collect_payment = (r2 == null ? "0" : r2["collect_payment"]) };
                foreach (var x in collectPayment)
                {
                    x.r1.SetField("collect_payment", x.collect_payment == DBNull.Value ? "0" : Convert.ToString(x.collect_payment));
                }
                #endregion

                #region PatientNotes
                OdbcDt.Columns.Add("patient_Note", typeof(string));
                OdbcDt.Columns.Add("PreferredLanguage", typeof(string));

                dtSub = new DataTable();
                OdbcSelect = SynchDentrixQRY.GetAppointmentPatientDataPatientNotes;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = OdbcSelect + " and Patid in ('" + strPatID + "')";
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDa.Fill(dtSub);

                var PatNote = from r1 in OdbcDt.AsEnumerable()
                              join r2 in dtSub.AsEnumerable()
                              on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patid"].ToString().Trim()
                              into outer
                              from r2 in outer.DefaultIfEmpty()
                              select new { r1, PatNote = (r2 == null ? "" : r2["patient_Note"]) };
                foreach (var x in PatNote)
                {
                    x.r1.SetField("patient_Note", x.PatNote == DBNull.Value ? "" : Convert.ToString(x.PatNote));
                    x.r1.SetField("PreferredLanguage", x.PatNote == DBNull.Value ? "" : Convert.ToString(x.PatNote).ToLower().Contains("spanish") ? "Spanish" : Convert.ToString(x.PatNote).ToLower().Contains("french") ? "French" : "English");
                }
                #endregion

                OdbcDt.Columns.Add("due_date", typeof(string));
                OdbcDt.Columns.Add("groupid", typeof(string));
                OdbcDt.Columns.Add("emergencycontactId", typeof(string));
                OdbcDt.Columns.Add("emergencycontactnumber", typeof(string));
                OdbcDt.Columns.Add("spouseId", typeof(string));
                OdbcDt.Columns.Add("Spouse_First_Name", typeof(string));
                OdbcDt.Columns.Add("Spouse_Last_Name", typeof(string));
                OdbcDt.Columns.Add("EmergencyContact_First_Name", typeof(string));
                OdbcDt.Columns.Add("EmergencyContact_Last_Name", typeof(string));
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static bool Save_Patient_Dentrix_To_Local_New(DataTable dtDentrixDataToSave, string Clinic_Number, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            try
            {
                SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtDentrixDataToSave, "0", "1");
            }
            catch (Exception)
            {

            }
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    using (SqlCeCommand SqlCeComBulk = new SqlCeCommand("", conn))
                    {
                        SqlCeComBulk.CommandType = CommandType.TableDirect;
                        SqlCeComBulk.CommandText = "Patient";
                        SqlCeComBulk.Connection = conn;
                        SqlCeComBulk.IndexName = "unique_Patient_EHR_ID";

                        SqlCeResultSet rs = SqlCeComBulk.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                        foreach (DataRow dr in dtDentrixDataToSave.Rows)
                        {
                            try
                            {
                                if (dr["CurrentBal"].ToString() == "")
                                {
                                    dr["CurrentBal"] = "0";
                                }
                                if (dr["ThirtyDay"].ToString() == "")
                                {
                                    dr["ThirtyDay"] = "0";
                                }
                                if (dr["SixtyDay"].ToString() == "")
                                {
                                    dr["SixtyDay"] = "0";
                                }
                                if (dr["NinetyDay"].ToString() == "")
                                {
                                    dr["NinetyDay"] = "0";
                                }
                                if (dr["Over90"].ToString() == "")
                                {
                                    dr["Over90"] = "0";
                                }

                                object responsiblebirthdte = null;
                                try
                                {
                                    responsiblebirthdte = Convert.ToDateTime(dr["responsiblepartybirthdate"]);
                                }
                                catch (Exception ex)
                                {
                                    responsiblebirthdte = DBNull.Value;
                                }

                                if (dr["InsUptDlt"].ToString() == "")
                                {
                                    dr["InsUptDlt"] = "0";
                                }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                                {
                                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
                                    {
                                        //Insert
                                        bool found = false;
                                        try
                                        {
                                            found = rs.Seek(DbSeekOptions.FirstEqual, new { PatID = dr["Patient_EHR_ID"].ToString().Trim(), CliNum = Clinic_Number, ServiceInstalledID = Service_Install_Id });
                                        }
                                        catch (Exception exFound)
                                        {
                                            if (exFound.Message.ToUpper().Contains("OBJECT MUST IMPLEMENT ICONVERTIBLE"))
                                            {
                                                found = rs.Seek(DbSeekOptions.FirstEqual, new object[] { dr["Patient_EHR_ID"].ToString().Trim(), Clinic_Number, Service_Install_Id });
                                            }
                                            else
                                            {
                                                throw exFound;
                                            }
                                        }
                                        SqlCeUpdatableRecord rec = rs.CreateRecord();
                                        rec.SetValue(rs.GetOrdinal("patient_ehr_id"), dr["patient_ehr_id"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("patient_Web_ID"), "");
                                        rec.SetValue(rs.GetOrdinal("First_name"), dr["First_name"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Last_name"), dr["Last_name"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Middle_Name"), dr["Middle_Name"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Salutation"), dr["Salutation"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("preferred_name"), dr["preferred_name"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Status"), dr["Status"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Sex"), dr["Sex"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("MaritalStatus"), dr["MaritalStatus"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Birth_Date"), Convert.ToString(dr["Birth_Date"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("Email"), dr["Email"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("Work_Phone"), Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("Address1"), dr["Address1"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Address2"), dr["Address2"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("City"), dr["City"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("State"), dr["State"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Zipcode"), dr["Zipcode"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("ResponsibleParty_Status"), "");
                                        rec.SetValue(rs.GetOrdinal("CurrentBal"), dr["CurrentBal"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("ThirtyDay"), dr["ThirtyDay"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("SixtyDay"), dr["SixtyDay"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("NinetyDay"), dr["NinetyDay"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Over90"), dr["Over90"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("FirstVisit_Date"), Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("Primary_Insurance"), dr["Primary_Insurance"].ToString());
                                        rec.SetValue(rs.GetOrdinal("Primary_Insurance_CompanyName"), dr["Primary_Insurance_CompanyName"].ToString());
                                        rec.SetValue(rs.GetOrdinal("Primary_Ins_Subscriber_ID"), dr["Primary_Ins_Subscriber_ID"].ToString());
                                        rec.SetValue(rs.GetOrdinal("Secondary_Insurance"), dr["Secondary_Insurance"].ToString());
                                        rec.SetValue(rs.GetOrdinal("Secondary_Insurance_CompanyName"), dr["Secondary_Insurance_CompanyName"].ToString());
                                        rec.SetValue(rs.GetOrdinal("Secondary_Ins_Subscriber_ID"), dr["Secondary_Ins_Subscriber_ID"].ToString());
                                        rec.SetValue(rs.GetOrdinal("Guar_ID"), dr["Guar_ID"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Pri_Provider_ID"), dr["Pri_Provider_ID"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Sec_Provider_ID"), dr["Sec_Provider_ID"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("ReceiveSMS"), dr["ReceiveSMS"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("ReceiveEmail"), "Y");
                                        rec.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("due_date"), Convert.ToString(dr["due_date"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("collect_payment"), dr["collect_payment"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("remaining_benefit"), dr["remaining_benefit"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("used_benefit"), dr["used_benefit"].ToString().Trim());
                                        if (dtDentrixDataToSave.Columns.Contains("EHR_Entry_DateTime"))
                                        {
                                            rec.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(DateTime.Now.ToString()));
                                        }
                                        rec.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                        rec.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                        rec.SetValue(rs.GetOrdinal("Clinic_Number"), "0");
                                        rec.SetValue(rs.GetOrdinal("Service_Install_Id"), "1");
                                        bool isDeleted = false;
                                        try
                                        {
                                            if (dr.Table.Columns.Contains("Ar_status"))
                                            {
                                                if (dr["Ar_status"].ToString() == "4")
                                                {
                                                    isDeleted = true;
                                                }
                                                else
                                                {
                                                    if (dr.Table.Columns.Contains("Is_Deleted"))
                                                    {
                                                        if (dr["Is_Deleted"] == DBNull.Value)
                                                        {
                                                            isDeleted = false;
                                                        }
                                                        else
                                                        {
                                                            isDeleted = Convert.ToBoolean(dr["Is_Deleted"]);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (dr.Table.Columns.Contains("Is_Deleted"))
                                                {
                                                    if (dr["Is_Deleted"] == DBNull.Value)
                                                    {
                                                        isDeleted = false;
                                                    }
                                                    else
                                                    {
                                                        isDeleted = Convert.ToBoolean(dr["Is_Deleted"]);
                                                    }
                                                }
                                            }

                                        }
                                        catch
                                        { }
                                        rec.SetValue(rs.GetOrdinal("is_deleted"), isDeleted);
                                        rec.SetValue(rs.GetOrdinal("EHR_Status"), dr["EHR_Status"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("ReceiveVoiceCall"), dr["ReceiveVoiceCall"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("PreferredLanguage"), dr["PreferredLanguage"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Patient_Note"), dr["Patient_Note"].ToString().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).ToString().Trim() : dr["Patient_Note"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("ssn"), dr["ssn"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("driverlicense"), dr["driverlicense"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("groupid"), dr["groupid"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("emergencycontactId"), dr["emergencycontactId"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("EmergencyContact_First_Name"), dr["EmergencyContact_First_Name"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("EmergencyContact_Last_Name"), dr["EmergencyContact_Last_Name"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("emergencycontactnumber"), dr["emergencycontactnumber"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("school"), dr["school"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("employer"), dr["employer"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("spouseId"), dr["spouseId"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Spouse_First_Name"), dr["Spouse_First_Name"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Spouse_Last_Name"), dr["Spouse_Last_Name"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("responsiblepartyId"), dr["responsiblepartyId"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("responsiblepartyssn"), dr["responsiblepartyssn"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("responsiblepartybirthdate"), responsiblebirthdte);
                                        rec.SetValue(rs.GetOrdinal("ResponsibleParty_First_Name"), dr["ResponsibleParty_First_Name"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("ResponsibleParty_Last_Name"), dr["ResponsibleParty_Last_Name"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Prim_Ins_Company_Phonenumber"), dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Sec_Ins_Company_Phonenumber"), dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                                        try
                                        {
                                            rs.Insert(rec);
                                        }
                                        catch (Exception exduplicate)
                                        {
                                            if (exduplicate.Message.ToString().ToUpper().Contains("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX."))
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                            }
                                        }
                                        //
                                    }
                                    else if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 2 || Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                    {
                                        //Update, Delete
                                        bool found = false;
                                        try
                                        {
                                            found = rs.Seek(DbSeekOptions.FirstEqual, new { PatID = dr["Patient_EHR_ID"].ToString().Trim(), CliNum = Clinic_Number, ServiceInstalledID = Service_Install_Id });
                                        }
                                        catch (Exception exFound)
                                        {
                                            if (exFound.Message.ToUpper().Contains("OBJECT MUST IMPLEMENT ICONVERTIBLE"))
                                            {
                                                found = rs.Seek(DbSeekOptions.FirstEqual, new object[] { dr["Patient_EHR_ID"].ToString().Trim(), Clinic_Number, Service_Install_Id });
                                            }
                                            else
                                            {
                                                throw exFound;
                                            }
                                        }

                                        if (found)
                                        {
                                            rs.Read();
                                            //rs.SetValue(rs.GetOrdinal("First_name"), dr["First_name"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Last_name"), dr["Last_name"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Middle_Name"), dr["Middle_Name"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Status"), dr["Status"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Email"), dr["Email"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                                            //rs.SetValue(rs.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                                            //rs.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                            //rs.SetValue(rs.GetOrdinal("ReceiveSMS"), dr["ReceiveSMS"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("ReceiveEmail"), "Y");
                                            //rs.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                            //rs.SetValue(rs.GetOrdinal("due_date"), Convert.ToString(dr["due_date"].ToString().Trim()));
                                            //rs.SetValue(rs.GetOrdinal("Clinic_Number"), dr["Clinic_Number"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("EHR_Status"), dr["EHR_Status"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("ReceiveVoiceCall"), dr["ReceiveVoiceCall"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            //rs.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString().Trim()));
                                            ////rs.SetValue(rs.GetOrdinal("patient_ehr_id"), dr["patient_ehr_id"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("patient_Web_ID"), "");
                                            rs.SetValue(rs.GetOrdinal("First_name"), dr["First_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Last_name"), dr["Last_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Middle_Name"), dr["Middle_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Salutation"), dr["Salutation"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("preferred_name"), dr["preferred_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Status"), dr["Status"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Sex"), dr["Sex"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("MaritalStatus"), dr["MaritalStatus"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Birth_Date"), Convert.ToString(dr["Birth_Date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Email"), dr["Email"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Work_Phone"), Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Address1"), dr["Address1"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Address2"), dr["Address2"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("City"), dr["City"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("State"), dr["State"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Zipcode"), dr["Zipcode"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_Status"), "");
                                            rs.SetValue(rs.GetOrdinal("CurrentBal"), dr["CurrentBal"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ThirtyDay"), dr["ThirtyDay"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("SixtyDay"), dr["SixtyDay"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("NinetyDay"), dr["NinetyDay"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Over90"), dr["Over90"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("FirstVisit_Date"), Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Primary_Insurance"), dr["Primary_Insurance"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Primary_Insurance_CompanyName"), dr["Primary_Insurance_CompanyName"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Primary_Ins_Subscriber_ID"), dr["Primary_Ins_Subscriber_ID"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Insurance"), dr["Secondary_Insurance"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Insurance_CompanyName"), dr["Secondary_Insurance_CompanyName"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Ins_Subscriber_ID"), dr["Secondary_Ins_Subscriber_ID"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Guar_ID"), dr["Guar_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Pri_Provider_ID"), dr["Pri_Provider_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Sec_Provider_ID"), dr["Sec_Provider_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveSMS"), dr["ReceiveSMS"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveEmail"), "Y");
                                            rs.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("due_date"), Convert.ToString(dr["due_date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("collect_payment"), dr["collect_payment"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("remaining_benefit"), dr["remaining_benefit"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("used_benefit"), dr["used_benefit"].ToString().Trim());
                                            if (dtDentrixDataToSave.Columns.Contains("EHR_Entry_DateTime"))
                                            {
                                                if (!string.IsNullOrEmpty(dr["EHR_Entry_DateTime"].ToString().Trim()))
                                                {
                                                    rs.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString().Trim()));
                                                }
                                            }
                                            rs.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rs.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rs.SetValue(rs.GetOrdinal("Clinic_Number"), dr["Clinic_Number"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Service_Install_Id"), Service_Install_Id);
                                            bool isDeleted = false;
                                            try
                                            {
                                                if (dr.Table.Columns.Contains("Ar_status"))
                                                {
                                                    if (dr["Ar_status"].ToString() == "4")
                                                    {
                                                        isDeleted = true;
                                                    }
                                                    else
                                                    {
                                                        if (dr.Table.Columns.Contains("Is_Deleted"))
                                                        {
                                                            if (dr["Is_Deleted"] == DBNull.Value)
                                                            {
                                                                isDeleted = false;
                                                            }
                                                            else
                                                            {
                                                                isDeleted = Convert.ToBoolean(dr["Is_Deleted"]);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (dr.Table.Columns.Contains("Is_Deleted"))
                                                    {
                                                        if (dr["Is_Deleted"] == DBNull.Value)
                                                        {
                                                            isDeleted = false;
                                                        }
                                                        else
                                                        {
                                                            isDeleted = Convert.ToBoolean(dr["Is_Deleted"]);
                                                        }
                                                    }
                                                }
                                            }
                                            catch
                                            { }
                                            rs.SetValue(rs.GetOrdinal("is_deleted"), Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3 ? true : isDeleted);
                                            rs.SetValue(rs.GetOrdinal("EHR_Status"), dr["EHR_Status"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveVoiceCall"), dr["ReceiveVoiceCall"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("PreferredLanguage"), dr["PreferredLanguage"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Patient_Note"), dr["Patient_Note"].ToString().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).ToString().Trim() : dr["Patient_Note"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ssn"), dr["ssn"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("driverlicense"), dr["driverlicense"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("groupid"), dr["groupid"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("emergencycontactId"), dr["emergencycontactId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("EmergencyContact_First_Name"), dr["EmergencyContact_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("EmergencyContact_Last_Name"), dr["EmergencyContact_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("emergencycontactnumber"), dr["emergencycontactnumber"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("school"), dr["school"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("employer"), dr["employer"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("spouseId"), dr["spouseId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Spouse_First_Name"), dr["Spouse_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Spouse_Last_Name"), dr["Spouse_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("responsiblepartyId"), dr["responsiblepartyId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("responsiblepartyssn"), dr["responsiblepartyssn"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("responsiblepartybirthdate"), responsiblebirthdte);
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_First_Name"), dr["ResponsibleParty_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_Last_Name"), dr["ResponsibleParty_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Prim_Ins_Company_Phonenumber"), dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Sec_Ins_Company_Phonenumber"), dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                                            try
                                            {
                                                rs.Update();
                                            }
                                            catch (Exception exduplicateupdate)
                                            {
                                                if (exduplicateupdate.Message.ToString().ToUpper().Contains("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX."))
                                                {
                                                    Utility.WriteToErrorLogFromAll("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX: Patient: " + dr["Patient_EHR_ID"].ToString().Trim() + ", Clinic_Number:" + Clinic_Number + ", Service_Installed_ID:" + Service_Install_Id);
                                                    continue;
                                                }
                                                else
                                                {
                                                    throw exduplicateupdate;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                Utility.WriteToErrorLogFromAll("Err_Patient Insert For Patient : " + dr["patient_ehr_id"].ToString() + "_" + ex1.Message.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        //public static bool Save_Patient_Dentrix_To_Local(DataTable dtDentrixPatient)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        // if (conn.State == ConnectionState.Closed) conn.Open();   
        //        string sqlSelect = string.Empty;
        //        string MaritalStatus = string.Empty;
        //        string Status = string.Empty;
        //        string tmpBirthDate = string.Empty;
        //        decimal curPatientcollect_payment = 0;
        //        string tmpReceive_Sms_Email = string.Empty;
        //        int tmpprivacy_flags = 0;

        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtDentrixPatient.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {

        //                tmpReceive_Sms_Email = "Y";
        //                tmpprivacy_flags = Convert.ToInt32(dr["privacy_flags"].ToString());
        //                if (tmpprivacy_flags == 2 || tmpprivacy_flags == 3 || tmpprivacy_flags == 6 || tmpprivacy_flags == 7)
        //                {
        //                    tmpReceive_Sms_Email = "N";
        //                }

        //                tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());

        //                if (tmpBirthDate != "")
        //                {
        //                    tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
        //                }

        //                try
        //                {
        //                    Status = dr["Status"].ToString().Trim();
        //                }
        //                catch (Exception)
        //                { Status = ""; }

        //                if (Status == "" || Status == "1")
        //                { Status = "A"; }
        //                else
        //                { Status = "I"; }

        //                try
        //                {
        //                    curPatientcollect_payment = 0;

        //                    if (dr["Patientcollect_payment"].ToString().Trim() != "" && dr["Patientcollect_payment"].ToString().Trim() != "0")
        //                    {
        //                        curPatientcollect_payment = Convert.ToDecimal(dr["Patientcollect_payment"].ToString().Trim());
        //                        curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
        //                        curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
        //                    }
        //                    else
        //                    {
        //                        curPatientcollect_payment = 0;
        //                    }
        //                }
        //                catch (Exception)
        //                {
        //                    curPatientcollect_payment = 0;
        //                }


        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("preferred_name", dr["preferred_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Status", Status);
        //                SqlCeCommand.Parameters.AddWithValue("Sex", dr["sex"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("MaritalStatus", dr["MaritalStatus"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Birth_Date", tmpBirthDate);
        //                SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Address1", dr["Address1"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["Zipcode"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Status", "");
        //                SqlCeCommand.Parameters.AddWithValue("CurrentBal", dr["CurrentBal"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("NinetyDay", dr["NinetyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dr["Primary_Insurance"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dr["Primary_Insurance_CompanyName"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dr["Secondary_Insurance"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dr["Secondary_Insurance_CompanyName"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", tmpReceive_Sms_Email);
        //                SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", tmpReceive_Sms_Email);
        //                SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("due_date", dr["due_date"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("remaining_benefit", dr["remaining_benefit"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("used_benefit", dr["used_benefit"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("collect_payment", curPatientcollect_payment.ToString());
        //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID","");
        //                SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID","");
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }

        //        //SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        // SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        public static bool Save_Patient_Dentrix_To_Local(DataTable dtDentrixPatient, string InsertTableName, DataTable dtDentrixPatientdue_date, DataTable dtLocalPatient, bool bSetDeleted = false)
        {
            bool _successfullstataus = true;
            SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtDentrixPatient , "0", "1");
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                string sqlSelect = string.Empty;
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                        {
                            SqlCeCommand.CommandText = "Delete from PatientCompare";
                            SqlCeCommand.ExecuteNonQuery();
                        }

                        foreach (DataRow dr in dtDentrixPatient.Rows)
                        {
                            //###
                            string Status = string.Empty;
                            string tmpBirthDate = string.Empty;
                            string tmpFirstVisit_Date = string.Empty;
                            string tmpLastVisit_Date = string.Empty;
                            string tmpnextvisit_date = string.Empty;
                            string tmpdue_date = string.Empty;
                            string tmpReceive_Sms_Email = string.Empty;
                            //decimal curPatientcollect_payment = 0;
                            //int tmpprivacy_flags = 0;


                            //tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());
                            //tmpFirstVisit_Date = Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim());
                            //tmpLastVisit_Date = Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim());

                            //tmpReceive_Sms_Email = "Y";

                            //tmpprivacy_flags = Convert.ToInt32(dr["privacy_flags"].ToString());

                            //if (tmpprivacy_flags == 2 || tmpprivacy_flags == 3 || tmpprivacy_flags == 6 || tmpprivacy_flags == 7)
                            //{
                            //    tmpReceive_Sms_Email = "N";
                            //}
                            //dr["ReceiveSMS"] = tmpReceive_Sms_Email.ToString();
                            //dr["ReceiveEmail"] = tmpReceive_Sms_Email.ToString();

                            //// https://app.asana.com/0/751059797849097/1149506260330945
                            //dr["nextvisit_date"] = Utility.SetNextVisitDate(dtDentrixPatientNextApptDate, "patid", "Patient_EHR_ID", "nextvisit_date", dr["Patient_EHR_ID"].ToString());
                            //tmpnextvisit_date = dr["nextvisit_date"].ToString();

                            //try
                            //{
                            //    DataRow[] drPatientcollect_payment = dtDentrixPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");

                            //    if (drPatientcollect_payment.Length > 0)
                            //    {
                            //        curPatientcollect_payment = Convert.ToDecimal(drPatientcollect_payment[0]["collect_payment"].ToString());
                            //        curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                            //        curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                            //        dr["collect_payment"] = curPatientcollect_payment.ToString();
                            //    }
                            //    else
                            //    {
                            //        curPatientcollect_payment = 0;
                            //        dr["collect_payment"] = curPatientcollect_payment.ToString();
                            //    }
                            //}
                            //catch (Exception)
                            //{
                            //    curPatientcollect_payment = 0;
                            //    dr["collect_payment"] = curPatientcollect_payment.ToString();
                            //}
                            try
                            {

                                DataRow[] Patdue_date = dtDentrixPatientdue_date.Copy().Select("patient_id = '" + dr["Patient_EHR_ID"] + "'");
                                tmpdue_date = string.Empty;

                                if (Patdue_date.Length > 0)
                                {
                                    if (Patdue_date.Length > 5)
                                    {
                                        DataTable tmpDatatableduedate = new DataTable();
                                        tmpDatatableduedate = Patdue_date.CopyToDataTable();
                                        DataView view = tmpDatatableduedate.DefaultView;
                                        view.Sort = "due_date desc";
                                        DataTable SortPatdue_date = view.ToTable();
                                        for (int i = 0; i < 5; i++)
                                        {
                                            tmpdue_date = SortPatdue_date.Rows[i]["due_date"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type_id"].ToString() + "|" + tmpdue_date;
                                        }
                                        dr["due_date"] = tmpdue_date;
                                    }
                                    else
                                    {
                                        DataTable tmpDatatableduedate = new DataTable();
                                        tmpDatatableduedate = Patdue_date.CopyToDataTable();
                                        DataView view = tmpDatatableduedate.DefaultView;
                                        view.Sort = "due_date desc";
                                        DataTable SortPatdue_date = view.ToTable();
                                        foreach (DataRow due_row in SortPatdue_date.Rows)
                                        {
                                            tmpdue_date = due_row["due_date"].ToString() + "@" + due_row["recall_type"].ToString() + "@" + due_row["recall_type_id"].ToString() + "|" + tmpdue_date;
                                        }
                                        dr["due_date"] = tmpdue_date;
                                    }
                                }
                            }
                            catch (Exception x)
                            {
                                Utility.WriteToErrorLogFromAll("Error in generating due/recall date: " + x.Message);
                                tmpdue_date = string.Empty;
                            }

                            //try
                            //{
                            //    Status = dr["Status"].ToString().Trim();
                            //}
                            //catch (Exception)
                            //{ Status = ""; }

                            //if (Status == "3")
                            //{ Status = "I"; }
                            //else
                            //{ Status = "A"; }

                            //dr["Status"] = Status;

                            //if (Convert.ToInt32(dr["sex"]) == 1)
                            //{
                            //    dr["sex"] = "Male";
                            //}
                            //else if (Convert.ToInt32(dr["sex"]) == 2)
                            //{
                            //    dr["sex"] = "Female";
                            //}
                            //else
                            //{
                            //    dr["sex"] = "Unknown";
                            //}

                            //if (Convert.ToInt32(dr["MaritalStatus"]) == 1)
                            //{
                            //    dr["MaritalStatus"] = "Single";
                            //}
                            //else if (Convert.ToInt32(dr["MaritalStatus"]) == 2)
                            //{
                            //    dr["MaritalStatus"] = "Married";
                            //}
                            //else if (Convert.ToInt32(dr["MaritalStatus"]) == 3)
                            //{
                            //    dr["MaritalStatus"] = "Child";
                            //}
                            //else if (Convert.ToInt32(dr["MaritalStatus"]) == 4)
                            //{
                            //    dr["MaritalStatus"] = "Other";
                            //}
                            //else
                            //{
                            //    dr["MaritalStatus"] = "Single";
                            //}
                            //if (dr["Primary_Insurance_CompanyName"].ToString() == "" && dr["Secondary_Insurance_CompanyName"].ToString() == "")
                            //{
                            //    dr["used_benefit"] = curPatientcollect_payment.ToString();
                            //    dr["remaining_benefit"] = curPatientcollect_payment.ToString();
                            //}
                            //DataRow[] row = dtLocalPatient.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");
                            //if (row.Length > 0)
                            //{
                            //    if (Utility.DateDiffBetweenTwoDate(tmpBirthDate, row[0]["Birth_Date"].ToString().Trim()))
                            //    {
                            //        dr["InsUptDlt"] = 2;
                            //    }
                            //    if (Utility.DateDiffBetweenTwoDate(tmpFirstVisit_Date, row[0]["FirstVisit_Date"].ToString().Trim()))
                            //    {
                            //        dr["InsUptDlt"] = 2;
                            //    }
                            //    if (Utility.DateDiffBetweenTwoDate(tmpLastVisit_Date, row[0]["LastVisit_Date"].ToString().Trim()))
                            //    {
                            //        dr["InsUptDlt"] = 2;
                            //    }
                            //    if (Utility.DateDiffBetweenTwoDate(tmpnextvisit_date, row[0]["nextvisit_date"].ToString().Trim()))
                            //    {
                            //        dr["InsUptDlt"] = 2;
                            //    }
                            //    if (row[0]["ReceiveSms"].ToString().Trim() != tmpReceive_Sms_Email)
                            //    {
                            //        dr["InsUptDlt"] = 2;
                            //    }
                            //    else if (row[0]["ReceiveEmail"].ToString().Trim() != tmpReceive_Sms_Email)
                            //    {
                            //        dr["InsUptDlt"] = 2;
                            //    }
                            //    else if (Status.Trim() != row[0]["Status"].ToString().Trim())
                            //    {
                            //        dr["InsUptDlt"] = 2;
                            //    }
                            //    if (tmpdue_date != row[0]["due_date"].ToString().Trim())
                            //    {
                            //        dr["InsUptDlt"] = 2;
                            //    }
                            //    if (Convert.ToDecimal(curPatientcollect_payment).ToString("0.##") != Convert.ToDecimal(row[0]["collect_payment"].ToString().Trim()).ToString("0.##"))
                            //    {
                            //        dr["InsUptDlt"] = 2;
                            //    }
                            //}
                            //###

                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                ExecuteQuery(InsertTableName, dr, SqlCeCommand);
                            }


                        }


                        if (bSetDeleted)
                        {
                            IEnumerable<string> PatientEHRIDs = dtDentrixPatient.AsEnumerable().Where(sid => sid["Service_Install_Id"].ToString() == "1").Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                            if (PatientEHRIDs != null && PatientEHRIDs.Any())
                            {

                                IEnumerable<string> LocalEHRIDs = dtLocalPatient.AsEnumerable()
                                    .Where(sid => sid["Service_Install_Id"].ToString() == "1")
                                    .Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                                if (LocalEHRIDs != null && LocalEHRIDs.Any())
                                {

                                    string DeletedEHRIDs = string.Join("','", LocalEHRIDs.Except(PatientEHRIDs).ToList());
                                    if (DeletedEHRIDs != string.Empty)
                                    {
                                        DeletedEHRIDs = "'" + DeletedEHRIDs + "'";
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        string SqlCeSelect = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                                        SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDs", DeletedEHRIDs);
                                        SqlCeCommand.CommandText = SqlCeSelect;
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }

                    #region Get Records from PatientCompareTAble
                    if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                    {
                        DataTable dtPatientCompareRec = new DataTable();
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        string SqlCeSelect = SynchLocalQRY.PatientCompareQuery;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.Add("Service_Install_Id", "1");
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                dtPatientCompareRec = new DataTable();
                                SqlCeDa.Fill(dtPatientCompareRec);
                            }
                            foreach (DataRow drRow in dtPatientCompareRec.Rows)
                            {
                                ExecuteQuery("Patient", drRow, SqlCeCommand);
                            }

                            //if (conn.State == ConnectionState.Closed) conn.Open();
                            //CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");


                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.Add("Service_Install_Id", "1");
                            SqlCeCommand.CommandText = "Delete from PatientCompare where Service_Install_Id = @Service_Install_Id ";
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }


        public static void ExecuteQuery(string InsertTableName, DataRow dr, SqlCeCommand SqlCeCommand)
        {
            try
            {
                string sqlSelect = string.Empty;
                string MaritalStatus = string.Empty;
                string Status = string.Empty;
                string tmpBirthDate = string.Empty;
                // decimal curPatientcollect_payment = 0;
                //string tmpReceive_Sms_Email = string.Empty;


                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                {
                    case 1:
                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
                        if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                        {
                            SqlCeCommand.CommandText = SqlCeCommand.CommandText.Replace("INSERT INTO Patient", "INSERT INTO PatientCompare");
                        }
                        break;
                    case 2:
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient;
                        break;
                    case 3:
                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient;
                        break;
                }

                //tmpReceive_Sms_Email = "Y";

                tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());

                if (tmpBirthDate != "")
                {
                    tmpBirthDate = !Utility.NotAllowToChangeSystemDateFormat ? Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy") : tmpBirthDate;// Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
                }

                try
                {
                    Status = dr["Status"].ToString().Trim();
                }
                catch (Exception)
                { Status = ""; }

                //if (Status == "" || Status == "1")
                //{ Status = "A"; }
                //else
                //{ Status = "I"; }

                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", Convert.ToInt64(dr["patient_ehr_id"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
                SqlCeCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("preferred_name", dr["preferred_name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Status", Status);
                SqlCeCommand.Parameters.AddWithValue("Sex", dr["sex"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("MaritalStatus", dr["MaritalStatus"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Birth_Date", tmpBirthDate);
                SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("Address1", dr["Address1"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["Zipcode"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Status", "");
                SqlCeCommand.Parameters.AddWithValue("CurrentBal", dr["CurrentBal"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("NinetyDay", dr["NinetyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dr["Primary_Insurance"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dr["Primary_Insurance_CompanyName"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dr["Secondary_Insurance"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dr["Secondary_Insurance_CompanyName"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", dr["ReceiveSMS"].ToString().Trim());//tmpReceive_Sms_Email);
                SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", dr["ReceiveEmail"].ToString().Trim());//tmpReceive_Sms_Email);
                SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("due_date", dr["due_date"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("remaining_benefit", dr["remaining_benefit"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("used_benefit", dr["used_benefit"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("collect_payment", dr["collect_payment"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", dr["Secondary_Ins_Subscriber_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", dr["Primary_Ins_Subscriber_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", (dr.Table.Columns.Contains("Ar_status") && (dr["Ar_status"].ToString() == "4")) ? 1 : (dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0));
                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                SqlCeCommand.Parameters.AddWithValue("EHR_Status", dr["EHR_Status"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ReceiveVoiceCall", dr["ReceiveVoiceCall"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("PreferredLanguage", dr["PreferredLanguage"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Patient_Note", dr["Patient_Note"].ToString().Trim().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).Trim() : dr["Patient_Note"].ToString().Trim());

                #region New Added Fields to Patient Form

                SqlCeCommand.Parameters.AddWithValue("ssn", dr["ssn"].ToString());
                SqlCeCommand.Parameters.AddWithValue("driverlicense", dr["driverlicense"].ToString());
                SqlCeCommand.Parameters.AddWithValue("school", dr["school"]);
                SqlCeCommand.Parameters.AddWithValue("employer", dr["employer"]);
                SqlCeCommand.Parameters.AddWithValue("Prim_Ins_Company_Phonenumber", dr["Prim_Ins_Company_Phonenumber"]);
                SqlCeCommand.Parameters.AddWithValue("Sec_Ins_Company_Phonenumber", dr["Sec_Ins_Company_Phonenumber"]);
                SqlCeCommand.Parameters.AddWithValue("groupid", dr["groupid"]);
                SqlCeCommand.Parameters.AddWithValue("responsiblepartyId", dr["responsiblepartyId"]);
                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_First_Name", dr["ResponsibleParty_First_Name"]);
                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Last_Name", dr["ResponsibleParty_Last_Name"]);
                SqlCeCommand.Parameters.AddWithValue("responsiblepartyssn", dr["responsiblepartyssn"]);
                SqlCeCommand.Parameters.AddWithValue("responsiblepartybirthdate", dr["responsiblepartybirthdate"]);

                SqlCeCommand.Parameters.AddWithValue("spouseid", dr["spouseid"]);
                SqlCeCommand.Parameters.AddWithValue("Spouse_First_Name", dr["Spouse_First_Name"]);
                SqlCeCommand.Parameters.AddWithValue("Spouse_Last_Name", dr["Spouse_Last_Name"]);
                SqlCeCommand.Parameters.AddWithValue("emergencycontactid", dr["emergencycontactid"]);
                SqlCeCommand.Parameters.AddWithValue("EmergencyContact_First_Name", dr["EmergencyContact_First_Name"]);
                SqlCeCommand.Parameters.AddWithValue("EmergencyContact_Last_Name", dr["EmergencyContact_Last_Name"]);
                SqlCeCommand.Parameters.AddWithValue("emergencycontactnumber", dr["emergencycontactnumber"]);

                //SqlCeCommand.Parameters.AddWithValue("Spouse_First_Name", dr["employer"]);

                #endregion

                SqlCeCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unique_Patient_EHR_ID"))
                {
                }
                else
                {
                    throw;
                }
            }
        }

        public static DataTable GetDentrixPatientNextApptDate()
        {

            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            string Apptdate = "", ApptTime = "";
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixPatientNextApptDate;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                foreach (DataRow drRow in OdbcDt.Rows)
                {
                    Apptdate = Convert.ToDateTime(drRow["nextvisit_date"].ToString()).ToString("dd/MM/yyyy");
                    ApptTime = Convert.ToInt32(drRow["start_hour"].ToString()).ToString("00") + ":" + Convert.ToInt32(drRow["start_minute"].ToString()).ToString("00");
                    drRow["nextvisit_date"] = DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
                OdbcDt.Columns.Remove("start_hour");
                OdbcDt.Columns.Remove("Start_minute");
                OdbcDt.Columns.Remove("length");
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixPatientdue_date(string strPatID = "")
        {
            DateTime dueDate = Utility.LastSyncDateAditServer;
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                DataTable OdbcDt = new DataTable();
                string OdbcSelect = "";
                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixPatientdue_date;
                    if (!string.IsNullOrEmpty(strPatID))
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatientdue_dateByPatID;
                        OdbcSelect = OdbcSelect.Replace("@Patient_EHR_ID", strPatID);
                        if (ToDate == default(DateTime))
                        {
                            ToDate = Utility.Datetimesetting().AddDays(-7);
                        }
                    }
                    OdbcSelect = OdbcSelect.Replace("@todate", "'" + dueDate.ToString("yyyy/MM/dd") + "'");
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                }
                else
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixPatient_recall;
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DataTable dtFill = new DataTable();
                    OdbcDa.Fill(dtFill);
                    if (!dtFill.Columns.Contains("recall_type") || !dtFill.Columns.Contains("recall_type_id") || !dtFill.Columns.Contains("due_date"))
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatient_recallCustom;
                        OdbcCommand = new OdbcCommand(); OdbcDa = null;
                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                        dtFill = new DataTable();
                        OdbcDa.Fill(dtFill);
                    }
                    OdbcDt = new DataTable();
                    if (!string.IsNullOrEmpty(strPatID))
                    {
                        var rwPatREcallDate = dtFill.Select("patient_id = '" + strPatID + "'");
                        if (rwPatREcallDate != null && rwPatREcallDate.Length > 0)
                        {
                            OdbcDt = dtFill.Select("patient_id = '" + strPatID + "'").CopyToDataTable();
                        }
                        else
                        {
                            OdbcDt = dtFill.Clone();
                        }
                    }
                    else
                    {
                        OdbcDt = dtFill.Clone();
                        //Added if condition because if dtfill does not contains any row it will give error - The source contains no DataRows.
                        if (dtFill.Rows.Count > 0) OdbcDt.Load(dtFill.Select().CopyToDataTable().CreateDataReader());
                    }
                    OdbcDt.Columns.Remove("patient_guid");
                    OdbcDt.Columns.Remove("Last_name");
                    OdbcDt.Columns.Remove("first_name");
                    OdbcDt.Columns.Remove("recall_description");
                    OdbcDt.Columns.Remove("prior_date");
                    OdbcDt.Columns.Remove("prov_id");
                    OdbcDt.Columns.Remove("provider_last_name");
                    OdbcDt.Columns.Remove("provider_first_name");
                }


                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixPatientdue_date_AllData()
        {
            DateTime dueDate = Utility.LastSyncDateAditServer;
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                DataTable OdbcDt = new DataTable();
                string OdbcSelect = "";
                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixPatientdue_date;
                    OdbcSelect = OdbcSelect.Replace("@todate", "'" + dueDate.ToString("yyyy/MM/dd") + "'");
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                }
                else
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixPatient_recall;
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                    if (!OdbcDt.Columns.Contains("recall_type") || !OdbcDt.Columns.Contains("recall_type_id") || !OdbcDt.Columns.Contains("due_date"))
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatient_recallCustom;
                        OdbcCommand = new OdbcCommand(); OdbcDa = null;
                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                        OdbcDt = new DataTable();
                        OdbcDa.Fill(OdbcDt);
                    }
                }

                return CreateDatatableforPatientRecall(OdbcDt);
                //return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static string GetPatGuaridAndProviders(string Patid)
        {
            string guar_id;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            // OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                OdbcCommand = new OdbcCommand();
                if (conn.State == ConnectionState.Closed) conn.Open();
                CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                OdbcCommand.CommandText = SynchDentrixQRY.GetpatGurIdAndProviders;
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("Patid", Patid);
                guar_id = Convert.ToString(OdbcCommand.ExecuteScalar()).Trim();
                return guar_id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable CreateDatatableforPatientRecall(DataTable dtPatientRecall)
        {
            DataTable dtResult = SynchLocalDAL.GetLocalPatientWiseRecallTypeBlankData();
            try
            {

                DataRow drNew = dtResult.NewRow();

                for (int i = 0; i < dtPatientRecall.Rows.Count; i++)
                {
                    drNew["Patient_Recall_Id"] = dtPatientRecall.Rows[i]["Patient_Id"].ToString() + dtPatientRecall.Rows[i]["recall_type_id"].ToString() + Convert.ToDateTime(dtPatientRecall.Rows[i]["due_date"]).Year.ToString() + Convert.ToDateTime(dtPatientRecall.Rows[i]["due_date"]).Month.ToString() + Convert.ToDateTime(dtPatientRecall.Rows[i]["due_date"]).Day.ToString();
                    drNew["Recall_Date"] = dtPatientRecall.Rows[i]["due_date"].ToString();
                    drNew["Provider_EHR_ID"] = dtPatientRecall.Rows[i]["Prov_id"].ToString();
                    drNew["RecallType_EHR_ID"] = dtPatientRecall.Rows[i]["recall_type_id"].ToString();
                    drNew["RecallType_Name"] = dtPatientRecall.Rows[i]["Recall_Type"].ToString();
                    drNew["RecallType_Descript"] = dtPatientRecall.Rows[i]["Recall_Description"].ToString();
                    drNew["Default_Recall"] = "N";
                    drNew["Entry_DateTime"] = DateTime.Now.ToString();
                    drNew["Last_Sync_Date"] = DateTime.Now.ToString();
                    drNew["EHR_Entry_DateTime"] = DateTime.Now.ToString();
                    drNew["Is_Deleted"] = 0;
                    drNew["Is_Adit_Updated"] = 0;
                    drNew["Clinic_Number"] = "0";
                    drNew["Service_Install_Id"] = "1";


                }
                return dtResult;
            }
            catch (Exception)
            {
                return dtResult;
            }
        }

        public static DataTable GetDentrixPatientcollect_payment()
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string OdbcSelect = SynchDentrixQRY.GetDentrixPatientcollect_payment;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixPatient_recall()
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixPatient_recall;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                if (!OdbcDt.Columns.Contains("recall_type") || !OdbcDt.Columns.Contains("recall_type_id") || !OdbcDt.Columns.Contains("due_date"))
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixPatient_recallCustom;
                    OdbcCommand = new OdbcCommand(); OdbcDa = null;
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixPatient_RecallType()
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixPatient_RecallType;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixPatientDiseaseData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixPatientDiseaseData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_PatientDisease_Dentrix_To_Local(DataTable dtDentrixPatientDisease)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixPatientDisease.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_PatientDisease;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientDisease;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_PatientDisease;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Name", dr["Disease_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", string.IsNullOrEmpty(Convert.ToString(dr["is_deleted"])) ? false : Convert.ToBoolean(dr["is_deleted"]));
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                }

                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static DataTable GetDentrixPatientMedicationData(string Patient_EHR_IDS)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            //Version BaseVersion = new Version("17.1.307.0");
            //Version EHRVersion = new Version(Utility.EHR_VersionNumber);
            double EHRVersion = GetInstalledDentrixVersion();
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                //if (strLike(Utility.EHR_VersionNumber, "17.1%") || EHRVersion >= BaseVersion)
                //if (EHRVersion >= new Version("17.1.0.0"))
                if (EHRVersion >= 7.1)
                {
                    if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatientMedicationDataG7New;
                    }
                    else
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatientMedicationDataG7New + " and D.patid in (" + Patient_EHR_IDS + ")";
                    }
                }
                //else if (strLike(Utility.EHR_VersionNumber, "17.0%") || EHRVersion < BaseVersion)
                //else if (EHRVersion >= new Version("17.0.0.0"))
                else if (EHRVersion >= 6.1)
                {
                    if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatientMedicationDataG62andG7;
                    }
                    else
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatientMedicationDataG62andG7 + " and PM.patid in (" + Patient_EHR_IDS + ")";
                    }
                }
                else
                {
                    if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatientMedicationDataG5andG6;
                    }
                    else
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixPatientMedicationDataG5andG6 + " Where D.patid in (" + Patient_EHR_IDS + ")";
                    }
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        #endregion

        #region  Disease

        public static DataTable GetDentrixDiseaseData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixDiseaseData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;

            }
            catch (Exception ex)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = SynchDentrixQRY.GetDentrixDiseaseDataG6andG5;
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DataTable OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                    return OdbcDt;
                }
                catch
                {
                    throw ex;
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        //public static bool strLike(string toSearch, string toFind)
        //{ return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch); ; }

        public static DataTable GetDentrixMedicationData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            //Version BaseVersion = new Version("17.1.307.0");
            //Version EHRVersion = new Version(Utility.EHR_VersionNumber);
            double EHRVersion = GetInstalledDentrixVersion();
            DataTable OdbcDt = new DataTable();
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                string OdbcSelect = "";
                if (conn.State == ConnectionState.Closed) conn.Open();

                //if (strLike(Utility.EHR_VersionNumber, "17.1%") || EHRVersion >= BaseVersion)
                //if (EHRVersion >= new Version("17.1.0.0"))
                if (EHRVersion >= 7.1)
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixMedicationDataG7New;
                }
                //else if (EHRVersion >= new Version("17.0.0.0"))
                else if (EHRVersion >= 6.1)
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixMedicationDataG62andG7;
                }
                else //if (EHRVersion >= new Version("16.0.0.0")) and also 15.xx, 14.xx
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixMedicationDataG6andG5;
                }
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixDieaseAlertData()
        {

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            DataTable resultdt = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixDiseaseAlertData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                resultdt = OdbcDt;
            }
            catch (Exception ex)
            {
                resultdt = GetDentrixDiseaseData();
                // throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return resultdt;
        }


        public static bool Save_Disease_Dentrix_To_Local(DataTable dtDentrixDisease)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixDisease.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Disease;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Disease;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Disease;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Name", dr["Disease_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                    //SqlCeCommand.Parameters.AddWithValue("is_deleted", Convert.ToBoolean(dr["is_deleted"]));
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", string.IsNullOrEmpty(dr["is_deleted"].ToString()) ? false : Convert.ToBoolean(dr["is_deleted"]));
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                }
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region RecallType

        public static DataTable GetDentrixRecallTypeData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixRecallTypeData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static bool Save_RecallType_Dentrix_To_Local(DataTable dtDentrixRecallType)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixRecallType.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_RecallType;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_RecallType;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_RecallType;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("RecallType_EHR_ID", dr["RecallType_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("RecallType_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("RecallType_Name", dr["RecallType_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("RecallType_Descript", dr["RecallType_Descript"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }
        #endregion

        #region Users
        public static bool Save_Users_Dentrix_To_Local(DataTable dtDentrixUser)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixUser.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_User;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_User;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_User;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.AddWithValue("User_EHR_ID", dr["User_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("User_EHR_ID", dr["User_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("User_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Updated_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Active", Convert.ToBoolean(dr["Is_Active"]));
                                    SqlCeCommand.Parameters.AddWithValue("Is_Deleted", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                }
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static string GetPatientName(Int64 patientid)
        {

            string patientname;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            // OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                OdbcCommand = new OdbcCommand();
                if (conn.State == ConnectionState.Closed) conn.Open();
                CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                OdbcCommand.CommandText = SynchDentrixQRY.GetPatientName;
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("patid", patientid);
                patientname = Convert.ToString(OdbcCommand.ExecuteScalar()).Trim();
                patientname = patientname.Replace(" ", "");
                return patientname;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

        }
        #endregion

        #region ApptStatus

        public static DataTable GetDentrixApptStatusData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixApptStatusData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static DataTable GetDentrixUsersData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixApptUsersData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToLower().Contains("keyword not supported: 'ssl'"))
                {
                    string s = Utility.DBConnString;
                    string[] UserconnString = s.Split(';');
                    string newdbconn = "";
                    // MessageBox.Show("Start");

                    foreach (string con in UserconnString)
                    {
                        if (!con.ToLower().Contains("ssl"))
                        {
                            newdbconn = newdbconn + con + ";";
                        }
                    }
                    try
                    {
                        conn = new OdbcConnection(newdbconn);
                        OdbcCommand = new OdbcCommand();
                        OdbcDa = null;

                        OdbcCommand.CommandTimeout = 200;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        string OdbcSelect = SynchDentrixQRY.GetDentrixApptUsersData;
                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                        DataTable OdbcDt = new DataTable();
                        OdbcDa.Fill(OdbcDt);
                        return OdbcDt;
                    }
                    catch (Exception inex)
                    {
                        throw inex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static bool Save_ApptStatus_Dentrix_To_Local(DataTable dtDentrixApptStatus)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixApptStatus.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_AppointmentStatus;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_AppointmentStatus;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_AppointmentStatus;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_EHR_ID", dr["ApptStatus_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Name", dr["ApptStatus_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Type", dr["ApptStatus_Type"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }
        #endregion

        #region Speciality

        public static bool Save_Speciality_Dentrix_To_Local(DataTable dtDentrixSpeciality)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open();   
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixSpeciality.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
                            {
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Speciality;
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Speciality_Name", dr["provider_speciality"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region SqlServer

        public static bool Save_Appointment_Dentrix_To_Local_SqlServer(DataTable dtDentrixAppointment)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //      SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //       SqlCetx = conn.BeginTransaction();
            try
            {
                //if (conn.State == ConnectionState.Closed) conn.Open(); 
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

                bool is_ehr_updated = false;
                string Apptdate = string.Empty;
                string ApptTime = string.Empty;
                string Mobile_Contact = string.Empty;
                string Email = string.Empty;
                string Home_Contact = string.Empty;
                string Address = string.Empty;
                string City = string.Empty;
                string State = string.Empty;
                string Zipcode = string.Empty;
                string patient_first_name = string.Empty;
                string patient_last_name = string.Empty;
                string patient_mi_name = string.Empty;
                string AppointmentStatus = string.Empty;

                foreach (DataRow dr in dtDentrixAppointment.Rows)
                {
                    is_ehr_updated = false;

                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }

                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {

                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Appointment;
                                is_ehr_updated = false;
                                break;
                            case 5:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Contact;
                                is_ehr_updated = true;
                                break;
                            case 4:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID;
                                is_ehr_updated = true;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                break;
                        }

                        if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                        {
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                            SqlCeCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            string[] patientinfo = dr["patient_name"].ToString().Split(',');
                            patient_last_name = patientinfo[0].ToString().Trim();
                            string[] patienfirstMI = patientinfo[1].ToString().Trim().Split(' ');
                            if (patienfirstMI.Length == 2)
                            {
                                patient_first_name = patienfirstMI[0].ToString().Trim();
                                patient_mi_name = patienfirstMI[1].ToString().Trim();
                            }
                            else
                            {
                                patient_first_name = patienfirstMI[0].ToString().Trim();
                                patient_mi_name = string.Empty;
                            }

                            Apptdate = Convert.ToDateTime(dr["appointment_date"].ToString()).ToString("dd/MM/yyyy");
                            ApptTime = Convert.ToInt32(dr["start_hour"].ToString()).ToString("00") + ":" + Convert.ToInt32(dr["start_minute"].ToString()).ToString("00");

                            Mobile_Contact = string.Empty;
                            Email = string.Empty;
                            Home_Contact = string.Empty;
                            Address = string.Empty;
                            City = string.Empty;
                            State = string.Empty;
                            Zipcode = string.Empty;

                            if (dr["patId"].ToString() == "0" || dr["patId"].ToString() == string.Empty)
                            {

                                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                                {
                                    Mobile_Contact = Utility.ConvertContactNumber(dr["Phone"].ToString().Trim());
                                    Email = "";
                                }
                                else
                                {
                                    string MobileEmail = dr["notetext"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace("\n", "");
                                    Mobile_Contact = string.Empty;
                                    Email = string.Empty;

                                    if (MobileEmail != string.Empty)
                                    {
                                        Mobile_Contact = MobileEmail.Substring(0, 10);
                                        Email = MobileEmail.Substring(10, MobileEmail.Length - 10);

                                        try
                                        {
                                            double isMobile = Convert.ToDouble(Mobile_Contact);
                                        }
                                        catch (FormatException)
                                        {
                                            Mobile_Contact = string.Empty;
                                            Email = MobileEmail.ToString();
                                        }

                                    }
                                    else
                                    {
                                        Mobile_Contact = dr["patMobile"].ToString();
                                        Email = dr["patEmail"].ToString();
                                    }
                                }

                                Home_Contact = dr["patient_phone"].ToString().Trim();
                                Address = dr["street1"].ToString().Trim();
                                City = dr["city"].ToString().Trim();
                                State = dr["state"].ToString().Trim();
                                Zipcode = dr["zipcode"].ToString().Trim();
                            }
                            else
                            {
                                Mobile_Contact = dr["patMobile"].ToString();
                                Email = dr["patEmail"].ToString();
                                Home_Contact = dr["patHomephone"].ToString().Trim();
                                Address = dr["patAddress"].ToString().Trim();
                                City = dr["patCity"].ToString().Trim();
                                State = dr["patState"].ToString().Trim();
                                Zipcode = dr["patZipcode"].ToString().Trim();
                            }

                            DateTime ApptDateTime = DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            DateTime ApptEndDateTime = ApptDateTime.AddMinutes(Convert.ToInt32(dr["length"].ToString().Trim()));

                            string birthdate = string.Empty;
                            if (!string.IsNullOrEmpty(dr["birth_date"].ToString()))
                            {
                                birthdate = dr["birth_date"].ToString();
                            }

                            if (dr["appointment_status_ehr_key"].ToString().Trim() == "-106")
                            {
                                AppointmentStatus = "completed";
                            }
                            else if (dr["appointment_status_ehr_key"].ToString().Trim() == "0")
                            {
                                AppointmentStatus = "none";
                            }
                            else
                            {
                                AppointmentStatus = dr["Appointment_Status"].ToString().Trim();
                            }


                            int commentlen = 1999;
                            if (dr["comment"].ToString().Trim().Length < commentlen)
                            {
                                commentlen = dr["comment"].ToString().Trim().Length;
                            }

                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", string.Empty);
                            SqlCeCommand.Parameters.AddWithValue("Last_Name", patient_last_name.Trim());
                            SqlCeCommand.Parameters.AddWithValue("First_Name", patient_first_name.Trim());
                            SqlCeCommand.Parameters.AddWithValue("MI", patient_mi_name.Trim());
                            SqlCeCommand.Parameters.AddWithValue("Home_Contact", Utility.ConvertContactNumber(Home_Contact.ToString().Trim()));
                            SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", Utility.ConvertContactNumber(Mobile_Contact.Trim()));
                            SqlCeCommand.Parameters.AddWithValue("Email", Email.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Address", Address.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("City", City.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("ST", State.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Zip", Zipcode.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["op_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["op_title"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["provider_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Provider_Name", dr["provider_last_name"].ToString().Trim() + " " + dr["provider_first_name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("ApptType", dr["ApptType_Name"].ToString().Trim().Replace(",", " "));
                            SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                            SqlCeCommand.Parameters.AddWithValue("birth_date", birthdate);
                            SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", ApptDateTime.ToString());
                            SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", ApptEndDateTime.ToString());
                            SqlCeCommand.Parameters.AddWithValue("Status", "1");
                            SqlCeCommand.Parameters.AddWithValue("Patient_Status", "new");
                            SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Appointment_Status", AppointmentStatus);
                            SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", "");
                            SqlCeCommand.Parameters.AddWithValue("confirmed_status", "");
                            SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", "");
                            SqlCeCommand.Parameters.AddWithValue("unschedule_status", "");
                            SqlCeCommand.Parameters.AddWithValue("Is_Appt", "EHR");
                            SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", is_ehr_updated);
                            SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                            SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                            SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
                            SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlCeCommand.Parameters.AddWithValue("Appt_LocalDB_ID", dr["Appt_LocalDB_ID"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patId"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                            SqlCeCommand.Parameters.AddWithValue("appt_treatmentcode", "");
                            SqlCeCommand.Parameters.AddWithValue("ProcedureDesc", "");
                            SqlCeCommand.Parameters.AddWithValue("ProcedureCode", "");
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                }
                // SqlCetx.Commit();
            }
            catch (Exception)
            {
                _successfullstataus = false;
                //SqlCetx.Rollback();
                // throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_OperatoryEvent_Dentrix_To_Local_SqlServer(DataTable dtDentrixOperatoryEvent)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //       SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //       SqlCetx = conn.BeginTransaction();
            try
            {
                //if (conn.State == ConnectionState.Closed) conn.Open(); 
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

                foreach (DataRow dr in dtDentrixOperatoryEvent.Rows)
                {

                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }

                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {

                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryEventData;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryEventData;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryEventData;
                                break;
                        }

                        if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                        {
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["event_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                            SqlCeCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            DateTime OE_Date = Convert.ToDateTime(dr["event_date"].ToString());

                            int OE_StartHour = Convert.ToInt32(dr["start_time"].ToString()) / 12;
                            int OE_StartMin = (Convert.ToInt32(dr["start_time"].ToString()) % 12) * 5;

                            int OE_EndHour = Convert.ToInt32(dr["end_time"].ToString()) / 12;
                            int OE_EndMin = (Convert.ToInt32(dr["end_time"].ToString()) % 12) * 5;


                            //DateTime StartTime = Convert.ToDateTime(OE_Date.ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(Convert.ToDateTime(OE_StartHour.ToString()).ToString("HH") + ":" + Convert.ToDateTime(OE_StartMin.ToString()).ToString("mm")).ToString("HH:mm"));
                            //DateTime EndTime = Convert.ToDateTime(OE_Date.ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(Convert.ToDateTime(OE_EndHour.ToString()).ToString("HH") + ":" + Convert.ToDateTime(OE_EndMin.ToString()).ToString("mm")).ToString("HH: mm"));

                            DateTime StartTime = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(OE_Date).ToString("MM/dd/yyyy") + " " + OE_StartHour.ToString() + ":" + OE_StartMin.ToString()).ToString("MM/dd/yyyy HH:mm"));
                            DateTime EndTime = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(OE_Date).ToString("MM/dd/yyyy") + " " + OE_EndHour.ToString() + ":" + OE_EndMin.ToString()).ToString("MM/dd/yyyy HH:mm"));



                            int commentlen = 1999;
                            if (dr["note"].ToString().Trim().Length < commentlen)
                            {
                                commentlen = dr["note"].ToString().Trim().Length;
                            }

                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["event_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", string.Empty);
                            SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["operatory_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("comment", dr["note"].ToString().Trim().Substring(0, commentlen));
                            SqlCeCommand.Parameters.AddWithValue("StartTime", StartTime.ToString());
                            SqlCeCommand.Parameters.AddWithValue("EndTime", EndTime.ToString());
                            SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                            SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                            SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                            SqlCeCommand.Parameters.AddWithValue("Allow_Book_Appt", Convert.ToBoolean(false));
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                }
                //SqlCetx.Commit();
            }
            catch (Exception)
            {
                _successfullstataus = false;
                // SqlCetx.Rollback();
                // throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_Provider_Dentrix_To_Local_SqlServer(DataTable dtDentrixProvider)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //       SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //      SqlCetx = conn.BeginTransaction();
            try
            {
                // if (conn.State == ConnectionState.Closed) conn.Open(); 
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtDentrixProvider.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {
                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Provider;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Provider;
                                break;
                        }

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Provider_Web_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("MI", dr["MI"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("gender", "");
                        SqlCeCommand.Parameters.AddWithValue("provider_speciality", dr["provider_speciality"].ToString().Trim().Substring(12, dr["provider_speciality"].ToString().Trim().Length - 12));
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                //SqlCetx.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;

                //SqlCetx.Rollback();
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_Speciality_Dentrix_To_Local_SqlServer(DataTable dtDentrixSpeciality)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //       SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //       SqlCetx = conn.BeginTransaction();
            try
            {
                // if (conn.State == ConnectionState.Closed) conn.Open();   
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtDentrixSpeciality.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
                    {
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Speciality;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Speciality_Name", dr["provider_speciality"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }

                //  SqlCetx.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                //  SqlCetx.Rollback();
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_Operatory_Dentrix_To_Local_SqlServer(DataTable dtDentrixOperatory)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //       SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //      SqlCetx = conn.BeginTransaction();
            try
            {
                //if (conn.State == ConnectionState.Closed) conn.Open();    
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtDentrixOperatory.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {
                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Operatory;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_Operatory;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Operatory;
                                break;
                        }
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Operatory_Web_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }

                //SqlCetx.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                //SqlCetx.Rollback();
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_Patient_Dentrix_To_Local_SqlServer(DataTable dtDentrixPatient)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //      SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //      SqlCetx = conn.BeginTransaction();
            try
            {
                // if (conn.State == ConnectionState.Closed) conn.Open();   
                string sqlSelect = string.Empty;
                string MaritalStatus = string.Empty;
                string Status = string.Empty;
                string tmpBirthDate = string.Empty;

                string tmpReceive_Sms_Email = string.Empty;
                int tmpprivacy_flags = 0;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtDentrixPatient.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {

                        tmpReceive_Sms_Email = "Y";
                        tmpprivacy_flags = Convert.ToInt32(dr["privacy_flags"].ToString());
                        if (tmpprivacy_flags == 2 || tmpprivacy_flags == 3 || tmpprivacy_flags == 6 || tmpprivacy_flags == 7)
                        {
                            tmpReceive_Sms_Email = "N";
                        }

                        tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());

                        if (tmpBirthDate != "")
                        {
                            tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
                        }

                        try
                        {
                            Status = dr["Status"].ToString().Trim();
                        }
                        catch (Exception)
                        { Status = ""; }

                        if (Status == "" || Status == "1")
                        { Status = "A"; }
                        else
                        { Status = "I"; }


                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient;
                                break;
                        }

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("preferred_name", dr["preferred_name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Status", Status);
                        SqlCeCommand.Parameters.AddWithValue("Sex", "Unknown");
                        SqlCeCommand.Parameters.AddWithValue("MaritalStatus", "Single");
                        SqlCeCommand.Parameters.AddWithValue("Birth_Date", tmpBirthDate);
                        SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                        SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                        SqlCeCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                        SqlCeCommand.Parameters.AddWithValue("Address1", dr["Address1"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["Zipcode"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Status", "");
                        SqlCeCommand.Parameters.AddWithValue("CurrentBal", "");
                        SqlCeCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("NinetyDay", dr["NinetyDay"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                        SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                        SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dr["Primary_Insurance"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dr["Primary_Insurance_CompanyName"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dr["Secondary_Insurance"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dr["Secondary_Insurance_CompanyName"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", tmpReceive_Sms_Email);
                        SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", tmpReceive_Sms_Email);
                        SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                        SqlCeCommand.Parameters.AddWithValue("due_date", dr["due_date"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("remaining_benefit", "");
                        SqlCeCommand.Parameters.AddWithValue("collect_payment", "");
                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
                        SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }

                //SqlCetx.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                //SqlCetx.Rollback();
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_ApptType_Dentrix_To_Local_SqlServer(DataTable dtDentrixApptType)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //     SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //    SqlCetx = conn.BeginTransaction();
            try
            {
                //  if (conn.State == ConnectionState.Closed) conn.Open();  
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtDentrixApptType.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {
                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptType;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_ApptType;
                                break;
                        }

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("ApptType_Web_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString().Trim().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                //SqlCetx.Commit();

            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                //SqlCetx.Rollback();
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_ApptStatus_Dentrix_To_Local_SqlServer(DataTable dtDentrixApptStatus)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //     SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //     SqlCetx = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtDentrixApptStatus.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {
                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_AppointmentStatus;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_AppointmentStatus;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_AppointmentStatus;
                                break;
                        }

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("ApptStatus_EHR_ID", dr["ApptStatus_EHR_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("ApptStatus_Web_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("ApptStatus_Name", dr["ApptStatus_Name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                //  SqlCetx.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;

                //SqlCetx.Rollback();
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_RecallType_Dentrix_To_Local_SqlServer(DataTable dtDentrixRecallType)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //    SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //    SqlCetx = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtDentrixRecallType.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {
                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_RecallType;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_RecallType;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_RecallType;
                                break;
                        }

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("RecallType_EHR_ID", dr["RecallType_EHR_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("RecallType_Web_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("RecallType_Name", dr["RecallType_Name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("RecallType_Descript", dr["RecallType_Descript"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                //SqlCetx.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;

                // SqlCetx.Rollback();
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Update_DeletedAppointment_Dentrix_To_Local_SqlServer(DataTable dtDentrixDeletedAppointment)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //    SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //    SqlCetx = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                string AppointmentStatus = string.Empty;
                foreach (DataRow dr in dtDentrixDeletedAppointment.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (dr["InsUptDlt"].ToString() == "2")
                    {
                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appointment_id"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Operatory_Name", "");
                        SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Provider_Name", "");
                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }

                }
                //SqlCetx.Commit();
            }
            catch (Exception)
            {
                _successfullstataus = false;
                // SqlCetx.Rollback();
                // throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        #endregion

        #region  Holidays

        public static DataTable GetDentrixHolidaysData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);

            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixHolidayData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@FromDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddMonths(12).ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                OdbcDt.DefaultView.RowFilter = "closed_flag in (2,3,4)";
                return OdbcDt.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixOperatoryHolidaysData(DataTable dtOperatory)
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);

            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                foreach (DataRow dr in dtOperatory.Rows)
                {
                    OdbcCommand OdbcCommand = new OdbcCommand();
                    OdbcDataAdapter OdbcDa = null;
                    //  MySqlCommand.CommandTimeout = 120;
                    OdbcCommand.CommandTimeout = 200;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = SynchDentrixQRY.GetDentrixOperatoryHolidaysData;
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.Parameters.AddWithValue("@H_Operatory_EHR_ID", dr["operatory_ehr_id"].ToString());
                    OdbcCommand.Parameters.Add("@FromDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DataTable dt = new DataTable();
                    OdbcDa.Fill(dt);
                    if (OdbcDt.Rows.Count > 0)
                    {
                        OdbcDt.Merge(dt);
                    }
                    else
                    {
                        OdbcDt = dt;
                    }

                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_Holidays_Dentrix_To_Local(DataTable dtDentrixHoliday)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //      SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        string AppointmentStatus = string.Empty;
                        foreach (DataRow dr in dtDentrixHoliday.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_HolidayData;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Dentrix_HolidayData;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Dentrix_HolidayData;
                                        break;
                                }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("SchedDate", dr["Sched_exception_date"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {

                                    int commentlen = 1999;
                                    if (dr["practice_name"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dr["practice_name"].ToString().Trim().Length;
                                    }
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("H_EHR_ID", dr["H_EHR_ID"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("H_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["practice_name"].ToString().Trim().Substring(0, commentlen));
                                    SqlCeCommand.Parameters.AddWithValue("SchedDate", Utility.CheckValidDatetime(dr["Sched_exception_date"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_1", dr["start_time1"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_1", dr["end_time1"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_2", dr["start_time2"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_2", dr["end_time2"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_3", dr["start_time3"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_3", dr["end_time3"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool Save_Opeatory_Holidays_Dentrix_To_Local(DataTable dtDentrixHoliday)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        string AppointmentStatus = string.Empty;
                        foreach (DataRow dr in dtDentrixHoliday.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_HolidayData;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Dentrix_Operatory_HolidayData;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Dentrix_Operatory_HolidayData;
                                        break;
                                }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", dr["op_id"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("SchedDate", dr["Sched_exception_date"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    int commentlen = 1999;
                                    if (dr["op_title"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dr["op_title"].ToString().Trim().Length;
                                    }
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("H_EHR_ID", dr["H_EHR_ID"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("H_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", dr["op_id"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["op_title"].ToString().Trim().Substring(0, commentlen));
                                    SqlCeCommand.Parameters.AddWithValue("SchedDate", Utility.CheckValidDatetime(dr["Sched_exception_date"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_1", dr["start_time1"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_1", dr["end_time1"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_2", dr["start_time2"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_2", dr["end_time2"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_3", dr["start_time3"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_3", dr["end_time3"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }


        #endregion

        #region Create Appointment

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType_EHR_ID;

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", tmpAppt_EHR_id);
                        SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", tmpAppt_Web_id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static int Save_Patient_Local_To_Dentrix(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, int tmpPatient_Gur_id, string Birth_Date)
        {
            int PatientId = 0;
            OdbcConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            OdbcCommand OdbcCommand = null;
            CommonDB.OdbcConnectionServer(ref conn);

            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                int AddressID = GetDentrixAddressId(tmpPatient_Gur_id);
                if (LastName.Length == 0)
                {
                    LastName += "NA";
                }
                if (LastName.Length == 1)
                {
                    LastName += "0";
                }
                //rooja - get dentrix auto chart number setup value
                GetDentrixChartNumberSettingsData();

                //int ChartNumber = GetDentrixChartNum(LastName.Substring(0, 2).ToString());
                string ChartNumber = GetDentrixChartNum(LastName.Substring(0, 2).ToString());
                string patBirthDate = "";
                try
                {
                    if (Birth_Date != "")
                    {
                        patBirthDate = !Utility.NotAllowToChangeSystemDateFormat ? Convert.ToDateTime(Birth_Date.ToString()).ToString("yyyy-MM-dd") : Convert.ToDateTime(Birth_Date.ToString()).ToString();// ("yyyy-MM-dd");
                    }
                }
                catch (Exception)
                {
                    patBirthDate = "";
                }

                string sqlSelect = string.Empty;
                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                if (patBirthDate == "")
                {
                    OdbcCommand.CommandText = SynchDentrixQRY.InsertPatientDetails;

                }
                else
                {
                    OdbcCommand.CommandText = SynchDentrixQRY.InsertPatientDetails_With_Birthdate;
                }
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("lastname", LastName);
                OdbcCommand.Parameters.AddWithValue("firstname", FirstName);
                OdbcCommand.Parameters.AddWithValue("mi", MiddleName);
                OdbcCommand.Parameters.AddWithValue("provid1", (PriProv == "0" || PriProv == "") ? Convert.ToString(GetDentrixIdelProvider().Rows[0]["provider_id"]) : PriProv);
                OdbcCommand.Parameters.AddWithValue("isguar", 0);
                OdbcCommand.Parameters.AddWithValue("gender", 1);
                OdbcCommand.Parameters.AddWithValue("firstvisitdate", Convert.ToDateTime(DateFirstVisit).ToString("yyyy-MM-dd"));
                OdbcCommand.Parameters.AddWithValue("emailaddr", Email);
                OdbcCommand.Parameters.AddWithValue("pager", Mobile);
                OdbcCommand.Parameters.AddWithValue("patguid", Guid.NewGuid().ToString());
                OdbcCommand.Parameters.AddWithValue("addrid", AddressID);
                if (ChartNumber != "0")
                {
                    OdbcCommand.Parameters.AddWithValue("chartnum", ChartNumber.ToString());
                }
                else
                {
                    OdbcCommand.Parameters.AddWithValue("chartnum", DBNull.Value);
                }
                OdbcCommand.Parameters.AddWithValue("status", 1);
                if (patBirthDate != "")
                {
                    OdbcCommand.Parameters.AddWithValue("birthdate", Convert.ToDateTime(patBirthDate));
                }
                OdbcCommand.ExecuteNonQuery();
                string QryIdentity = "Select top 1 patid as newId from admin.patient where firstname = '" + FirstName + "' and lastname = '" + LastName + "' and pager = '" + Mobile + "'";//"Select @@Identity as newId from patient";
                OdbcCommand.CommandText = QryIdentity;
                OdbcCommand.CommandType = CommandType.Text;
                OdbcCommand.Connection = conn;
                PatientId = Convert.ToInt32(OdbcCommand.ExecuteScalar());

                OdbcCommand.CommandText = SynchDentrixQRY.UpdatePatientGuarantorID;
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("isguar", tmpPatient_Gur_id > 0 ? 0 : 1);
                OdbcCommand.Parameters.AddWithValue("isheadofhouse", tmpPatient_Gur_id > 0 ? 0 : 1);
                OdbcCommand.Parameters.AddWithValue("Guarantor", tmpPatient_Gur_id == 0 ? PatientId : tmpPatient_Gur_id);
                OdbcCommand.Parameters.AddWithValue("famid", tmpPatient_Gur_id == 0 ? PatientId : tmpPatient_Gur_id);
                OdbcCommand.Parameters.AddWithValue("patid", PatientId);
                OdbcCommand.ExecuteNonQuery();
                InsertAgigPatient(Convert.ToInt64(tmpPatient_Gur_id == 0 ? PatientId : tmpPatient_Gur_id), conn);

            }
            catch (Exception ex)
            {
                PatientId = 0;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return PatientId;
        }

        public static string GetDentrixChartNum(string LastName)
        {
            string Chartnum = "";
            int chartnum = 0;
            string chartNumRtn = "";
            OdbcConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            OdbcCommand OdbcCommand = null;
            OdbcDataAdapter OdbcAdapter = null;
            CommonDB.OdbcConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                string QryIdentity = "";
                if (Utility.ChartNumberIsNumericstr == "Numeric")
                {
                    QryIdentity = "SELECT Trim(chartnum) as chartnum from admin.patient";
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.CommandType = CommandType.Text;
                    OdbcCommand.Connection = conn;
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcAdapter);
                    DataTable dt = new DataTable();
                    OdbcAdapter.Fill(dt);
                    var numbersonly = dt.AsEnumerable().Where(x => Regex.IsMatch(x.Field<string>("chartnum"), @"^\d{1,}$")).ToList();
                    List<Int32> ChartList = new List<Int32>();
                    foreach (DataRow dr in numbersonly)
                    {
                        try
                        {
                            ChartList.Add(Convert.ToInt32(dr["chartnum"].ToString().Trim()));
                        }
                        catch
                        {
                        }
                    }
                    if (ChartList.Count > 0)
                    {
                        Chartnum = (ChartList.Max()).ToString();
                    }
                    else
                    {
                        chartnum = 1;
                    }
                }
                else if (Utility.ChartNumberIsNumericstr == "AlphaNumeric")
                {
                    QryIdentity = "SELECT max(chartnum) from admin.patient where chartnum like '" + LastName.ToUpper() + "%%%%'";
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.CommandType = CommandType.Text;
                    OdbcCommand.Connection = conn;
                    Chartnum = OdbcCommand.ExecuteScalar().ToString();

                }

                if (Chartnum == "")
                {
                    chartnum = 1;
                    //return chartnum;
                }
                else
                {
                    //rooja
                    if (Utility.ChartNumberIsNumericstr == "Numeric")
                    {
                        chartnum = Convert.ToInt32(Chartnum.ToString()) + 1;
                    }
                    else if (Utility.ChartNumberIsNumericstr == "AlphaNumeric")
                    {
                        chartnum = Convert.ToInt16(Chartnum.ToString().Trim().Substring(2)) + 1;
                    }
                    else if (Utility.ChartNumberIsNumericstr == "None")
                    {
                        chartnum = 1;
                    }
                    //return chartnum + 1;
                }
                if (chartnum != 0)
                {
                    if (Utility.ChartNumberIsNumericstr == "Numeric")
                    {
                        chartNumRtn = chartnum.ToString("000000");
                    }
                    else if (Utility.ChartNumberIsNumericstr == "AlphaNumeric")
                    {
                        chartNumRtn = LastName.Substring(0, 2).ToString().ToUpper() + chartnum.ToString("0000");
                    }
                    else if (Utility.ChartNumberIsNumericstr == "None")
                    {
                        chartNumRtn = "      ";
                    }
                }

            }
            catch (Exception ex)
            {
                chartnum = 0;
                chartNumRtn = chartnum.ToString("000000");
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return chartNumRtn;
        }

        public static int GetDentrixAddressId(int PatientId)
        {
            int Address_Id = 0;
            OdbcConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            OdbcCommand OdbcCommand = null;
            CommonDB.OdbcConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {

                if (PatientId == 0)
                {
                    string sqlSelect = string.Empty;
                    CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.CommandText = SynchDentrixQRY.InsertPatientAddress;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.ExecuteNonQuery();

                    string QryIdentity = "Select max(addrid) as newId from admin.Address";
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.CommandType = CommandType.Text;
                    OdbcCommand.Connection = conn;
                    Address_Id = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                }
                else
                {
                    string sqlSelect = string.Empty;
                    CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                    string QryIdentity = "Select addrid as newId from admin.patient where patid = " + PatientId;
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.CommandType = CommandType.Text;
                    OdbcCommand.Connection = conn;
                    Address_Id = Convert.ToInt32(OdbcCommand.ExecuteScalar());

                }

            }
            catch (Exception ex)
            {
                Address_Id = 0;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return Address_Id;
        }

        public static string GetDentrixPrimaryProvider(string PatientId)
        {
            string providerid = "0";
            OdbcConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            OdbcCommand OdbcCommand = null;
            CommonDB.OdbcConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {

                if (PatientId != "0" || PatientId != "")
                {
                    string sqlSelect = string.Empty;
                    CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.CommandText = SynchDentrixQRY.GetPrimaryProviderId;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("patid", PatientId);
                    providerid = Convert.ToString(OdbcCommand.ExecuteScalar());
                }
                else
                {
                    providerid = Convert.ToString(GetDentrixIdelProvider().Rows[0]["provider_id"]);

                }

            }
            catch (Exception ex)
            {
                providerid = "0";
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return providerid;
        }

        public static int Save_Appointment_Local_To_Dentrix(string patid, string AppointmentConfirmationStatus, int length, string opid, string provid, string apptdate,
                                                                 string createdate, string appttypeid, string PatientName, string ApptComment, string TreatmentCodes)
        {
            int Appointment_Id = 0;
            OdbcConnection conn = null;
            //Utility.CheckEntryUserLoginIdExist();
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetDentrixUserLogin_ID();
            }

            long procid = 0;
            string proccodeid = "";
            List<string> allcodes = new List<string>();

            string sqlSelect = string.Empty;
            OdbcCommand OdbcCommand = null;
            CommonDB.OdbcConnectionServer(ref conn);

            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                sqlSelect = string.Empty;
                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.CommandText = SynchDentrixQRY.InsertAppointmentDetails;
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("patid", patid);
                OdbcCommand.Parameters.AddWithValue("status", AppointmentConfirmationStatus);
                OdbcCommand.Parameters.AddWithValue("apptlen", length);
                //  OdbcCommand.Parameters.AddWithValue("Confirmed", Confirmed);
                OdbcCommand.Parameters.AddWithValue("opid", opid);
                OdbcCommand.Parameters.AddWithValue("provid", provid);
                OdbcCommand.Parameters.AddWithValue("apptdate", Convert.ToDateTime(apptdate));
                // OdbcCommand.Parameters.AddWithValue("IsNewPatient", IsNewPatient);
                OdbcCommand.Parameters.AddWithValue("createdate", Convert.ToDateTime(createdate));
                OdbcCommand.Parameters.AddWithValue("appttype", appttypeid);
                OdbcCommand.Parameters.AddWithValue("timehr", Convert.ToDateTime(createdate).Hour);
                OdbcCommand.Parameters.AddWithValue("timeminn", Convert.ToDateTime(createdate).Minute);
                OdbcCommand.Parameters.AddWithValue("timeblock", 2);
                OdbcCommand.Parameters.AddWithValue("rsctype2", 1);
                OdbcCommand.Parameters.AddWithValue("rsctype", 3);
                OdbcCommand.Parameters.AddWithValue("rsctype3", 2);
                OdbcCommand.Parameters.AddWithValue("patname", PatientName);
                OdbcCommand.Parameters.AddWithValue("staffid", Utility.EHR_UserLogin_ID);
                OdbcCommand.Parameters.AddWithValue("apptreason", TreatmentCodes); // TODO : replace with code to update ApptReason

                for (int i = 0; i <= 19; i++)
                {
                    OdbcCommand.Parameters.AddWithValue("codeid" + (i + 1), "0");
                    OdbcCommand.Parameters.AddWithValue("codetype" + (i + 1), "0");
                }
                OdbcCommand.Parameters.AddWithValue("createdbyuserid", Utility.EHR_UserLogin_ID);
                OdbcCommand.ExecuteNonQuery();

                string QryIdentity = "Select max(apptid) as newId from admin.appt";
                OdbcCommand.CommandText = QryIdentity;
                OdbcCommand.CommandType = CommandType.Text;
                OdbcCommand.Connection = conn;

                Appointment_Id = Convert.ToInt32(OdbcCommand.ExecuteScalar());



                #region New Code Saving ProcedureLogs and Mapping

                //10_0:proccodeid_procid
                if (TreatmentCodes != null && TreatmentCodes != "")
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    try
                    {
                        int codeindex = 1, guarid = 0;
                        double amt = 0, totalamt = 0;
                        string procabbrev = "", proceduredesc = "";


                        OdbcCommand.CommandText = SynchDentrixQRY.GetGuarIdFromPatId;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("patid", patid);

                        guarid = Convert.ToInt16(OdbcCommand.ExecuteScalar());
                        //guarid = 1;


                       foreach (var treatcode in TreatmentCodes.Split(','))
                        {
                            sqlSelect = "";
                            proccodeid = treatcode.Substring(0, treatcode.IndexOf('_'));
                            procid = Convert.ToInt64(treatcode.Substring(treatcode.LastIndexOf('_') + 1));


                            OdbcCommand.CommandText = SynchDentrixQRY.GetAmountFromProcedure;
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("proccodeid", proccodeid);

                            amt = Convert.ToDouble(OdbcCommand.ExecuteScalar());

                            totalamt = totalamt + amt;

                            OdbcCommand.CommandText = SynchDentrixQRY.GetAbbrevDescFromProcedure;
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("proccodeid", proccodeid);

                            procabbrev = OdbcCommand.ExecuteScalar().ToString();

                            if (proceduredesc == "")
                                proceduredesc = procabbrev;

                            else
                                proceduredesc = proceduredesc + "," + procabbrev;

                            //System.Windows.Forms.MessageBox.Show(proceduredesc);

                            //index = TreatmentCodes.IndexOf(treatcode);
                            //63_32,3_0
                            if (procid == 0)
                            {
                                //$$$$$$$$$$$$$$$$$$$$ Commented and new by yogesh for get procid from admin.proccode if treatment is unplanned
                                //CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                                //OdbcCommand.CommandText = SynchDentrixQRY.InsertFullProcedureLog;
                                //OdbcCommand.Parameters.Clear();
                                //OdbcCommand.Parameters.AddWithValue("patid", patid);
                                //OdbcCommand.Parameters.AddWithValue("guarid", guarid);
                                //OdbcCommand.Parameters.AddWithValue("proccodeid", proccodeid);
                                //OdbcCommand.Parameters.AddWithValue("provid", provid);
                                //OdbcCommand.Parameters.AddWithValue("amt", amt);

                                //OdbcCommand.ExecuteNonQuery();

                                //// Get Full procedure logid
                                //CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                                //OdbcCommand.CommandText = SynchDentrixQRY.GetProcIdForProcedure;
                                //OdbcCommand.Parameters.Clear();
                                //OdbcCommand.Parameters.AddWithValue("patid", patid);
                                //OdbcCommand.Parameters.AddWithValue("provid", provid);
                                //OdbcCommand.Parameters.AddWithValue("proccodeid", proccodeid);
                                // Get Full procedure logid
                                //procid = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                //$$$$$$$$$$$$$$$$$$$$
                                
                                
                                // Update ApptTable
                                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                                sqlSelect = SynchDentrixQRY.UpdateProcedureCodeIdInAppointment;
                                sqlSelect = sqlSelect.Replace("_Number", codeindex.ToString());
                                OdbcCommand.CommandText = sqlSelect;
                                OdbcCommand.Parameters.Clear();

                                OdbcCommand.Parameters.AddWithValue("apptreason", proceduredesc);
                                OdbcCommand.Parameters.AddWithValue("CodeId", proccodeid);
                                OdbcCommand.Parameters.AddWithValue("codeType", 0);
                                OdbcCommand.Parameters.AddWithValue("amt", totalamt);
                                OdbcCommand.Parameters.AddWithValue("apptId", Appointment_Id);
                                OdbcCommand.ExecuteNonQuery();

                                codeindex++;

                            }
                            else
                            {

                                // Already entry available in FullProcLog Table
                                // Update CodeIds & Type into Appt Table
                                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                                sqlSelect = SynchDentrixQRY.UpdateProcedureCodeIdInAppointment;
                                sqlSelect = sqlSelect.Replace("_Number", codeindex.ToString());
                                OdbcCommand.CommandText = sqlSelect;
                                OdbcCommand.Parameters.Clear();

                                OdbcCommand.Parameters.AddWithValue("apptreason", proceduredesc);
                                OdbcCommand.Parameters.AddWithValue("CodeId", procid);
                                OdbcCommand.Parameters.AddWithValue("codeType", 1);
                                OdbcCommand.Parameters.AddWithValue("amt", totalamt);
                                OdbcCommand.Parameters.AddWithValue("apptId", Appointment_Id);
                                OdbcCommand.ExecuteNonQuery();

                                codeindex++;

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteToErrorLogFromAll("[FullProcdeureLog Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }
                }

                #endregion


                if (ApptComment.ToString().Trim() != "")
                {

                    bool isApptNote = Save_Appointment_Comment_Local_To_Dentrix(Appointment_Id.ToString(), ApptComment.ToString().Trim());

                }

            }

            catch (Exception ex)
            {
                Appointment_Id = 0;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return Appointment_Id;
        }

        public static bool Save_Appointment_Comment_Local_To_Dentrix(string AppointmentID, string ApptComment)
        {
            bool isApptComment = false;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = null;
            CommonDB.OdbcConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.CommandText = SynchDentrixQRY.InsertAppointmentComment;
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("notetype", 2);
                OdbcCommand.Parameters.AddWithValue("_unused1", 0);
                OdbcCommand.Parameters.AddWithValue("noteid", AppointmentID);
                OdbcCommand.Parameters.AddWithValue("notetext", ApptComment.ToString());
                OdbcCommand.ExecuteNonQuery();
                isApptComment = true;
            }
            catch (Exception ex)
            {
                isApptComment = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return isApptComment;
        }

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetBookOperatoryAppointmenetWiseDateTime;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.AddWithValue("ToDate", ApptDate.ToString("yyyy-MM-dd"));
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixPatientID_NameData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixPatientID_NameData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixIdelProvider(bool IsPrimary = true)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                DataTable OdbcDt = new DataTable();
                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                {
                    OdbcSelect = SynchDentrixQRY.GetDentrixIdelProviderG5;
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                }
                else
                {
                    if (IsPrimary)
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixIdelProvider + " and rscclass = 0";
                    }
                    else
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixIdelProvider + " and rscclass = 1";
                    }
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                    if (OdbcDt == null || (OdbcDt != null && OdbcDt.Rows.Count <= 0))
                    {
                        OdbcSelect = SynchDentrixQRY.GetDentrixIdelProvider;
                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                        OdbcDt = new DataTable();
                        OdbcDa.Fill(OdbcDt);
                    }

                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Update_Status_EHR_Appointment_Live_To_DentrixEHR(DataTable dtLiveAppointment, string _filename_EHR_appointment = "", string _EHRLogdirectory_EHR_appointment = "")
        {
            bool _successfullstataus = true;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            CommonDB.OdbcConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                string confirmed_status_ehr_ID = "";

                foreach (DataRow dr in dtLiveAppointment.Rows)
                {

                    confirmed_status_ehr_ID = dr["confirmed_status_ehr_key"].ToString();
                    if (confirmed_status_ehr_ID == "")
                    {
                        confirmed_status_ehr_ID = "2";
                    }

                    OdbcCommand.CommandText = SynchDentrixQRY.Update_Status_EHR_Appointment_Live_To_Local;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("status", confirmed_status_ehr_ID);
                    OdbcCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString());
                    OdbcCommand.ExecuteNonQuery();

                    if (InsertDataApptHistoryData(dr["Appt_EHR_ID"].ToString()))
                        Utility.WriteToSyncLogFile_All("Insert appt history data successfully appt_ehr_id : " + dr["Appt_EHR_ID"].ToString());
                    else
                        Utility.WriteToSyncLogFile_All("Insert appt history data failed appt_ehr_id : " + dr["Appt_EHR_ID"].ToString());

                    bool isApptConformStatus = SynchLocalDAL.UpdateLocalAppointmentConformStatusData(dr["Appt_EHR_ID"].ToString(), dr["Service_Install_Id"].ToString(), _filename_EHR_appointment, _EHRLogdirectory_EHR_appointment);
                    if (isApptConformStatus)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Update Status EHR Appointment Live To Local EHR appointment Confirmed for Appt_EHR_ID=" + dr["Appt_EHR_ID"] + " and status : " + confirmed_status_ehr_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Update_Receive_SMS_Patient_EHR_Live_To_DentrixEHR(DataTable dtLiveAppointment, string Locationid, string Loc_ID, string _filename_EHR_patientoptout = "", string _EHRLogdirectory_EHR_patientoptout = "")
        {
            bool _successfullstataus = true;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            CommonDB.OdbcConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                Patient_OptOutBO_StatusUpdate updatedPatientid = new Patient_OptOutBO_StatusUpdate();
                List<Patientids_OptOutBO_StatusUpdate> Patient_StatusUpdate = new List<Patientids_OptOutBO_StatusUpdate>();
                foreach (DataRow dr in dtLiveAppointment.Rows)
                {
                    try
                    {
                        OdbcCommand.CommandText = SynchDentrixQRY.Update_Receive_SMS_Patient_EHR_Live_To_Dentrix;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("receives_sms", dr["receive_sms"].ToString() == "Y" ? 0 : 1);
                        OdbcCommand.Parameters.AddWithValue("patient_id", dr["patient_ehr_id"].ToString());
                        OdbcCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_patientoptout, _EHRLogdirectory_EHR_patientoptout, " Update Receive SMS PatientEHR Live To Dentrix for patient_ehr_id=" + dr["patient_ehr_id"].ToString());
                        Patientids_OptOutBO_StatusUpdate Patientids = new Patientids_OptOutBO_StatusUpdate();
                        Patientids.esId = dr["esid"].ToString();
                        Patientids.patientId = dr["patientid"].ToString();
                        Patient_StatusUpdate.Add(Patientids);

                    }
                    catch (Exception)
                    {
                    }
                }
                if (Patient_StatusUpdate.Count > 0)
                {
                    updatedPatientid.locationId = Loc_ID;
                    updatedPatientid.organizationId = Utility.Organization_ID;
                    updatedPatientid.patientIds = Patient_StatusUpdate;
                    PushLiveDatabaseDAL.UpdatePatientReceive_SMStStatusToWeb(updatedPatientid, Locationid, Loc_ID);
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        #endregion


        #region Treatment DOC
        public static bool Save_Treatment_Document_in_Dentrix(string strTreatmentPlanID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            bool callLoop = false;
            try
            {
                CommonDB.OdbcConnectionServer(ref conn);
                //get data from local

                DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLocaleTreatmentDocData(strTreatmentPlanID);
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                    key2.SetValue("TreatmentIsSyncing", false);

                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        if (callLoop == false)
                        {
                            string sourceLocation = CommonUtility.GetAditTreatmentDocTempPath() + "\\" + dr["TreatmentDoc_Name"].ToString();
                            if (!System.IO.File.Exists(sourceLocation))
                            {
                                PullLiveDatabaseDAL.Update_TreatrmentDocNotFound_Live_To_Local(dr["TreatmentPlanId"].ToString());
                                continue;
                            }

                            string tmpFileOrgName = Path.GetFileName(sourceLocation);
                            string SourcePath = Path.GetDirectoryName(sourceLocation);

                            Thread.Sleep(100);
                            string showingName = dr["TreatmentPlanName"].ToString().Trim() + "-" + dr["PatientName"].ToString().Trim();

                            if (showingName.Length > 40)
                            {
                                showingName = showingName.Substring(0, 38);
                            }

                            if (AttachTreatmentDocument(sourceLocation, "/ID" + dr["Patient_EHR_ID"].ToString(), showingName, ref callLoop))
                            {
                                RegistryKey key1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DocumentAttachedId");
                                Int64 SavePatientDocId = 0;
                                if (key1 != null)
                                {
                                    SavePatientDocId = Convert.ToInt64(key1.GetValue("TreatmentDocumentId").ToString());
                                    if (SavePatientDocId > 0)
                                    {

                                        //update local 
                                        PullLiveDatabaseDAL.Update_TreatmentFormDoc_Local_To_EHR(dr["TreatmentDoc_Web_ID"].ToString(), SavePatientDocId.ToString());
                                        //PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), , "1");
                                        File.Delete(sourceLocation);
                                    }
                                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DocumentAttachedId");
                                    key.SetValue("TreatmentDocumentId", 0);
                                }
                                callLoop = false;
                            }
                            else
                            {
                                callLoop = false;
                            }
                        }
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        while (callLoop)
                        {
                            if (sw.Elapsed > TimeSpan.FromSeconds(120))
                            {
                                callLoop = false;
                                break;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private static bool AttachTreatmentDocument(string docPath, string patientId, string FormName, ref bool callloop)
        {
            Process myProcess = new Process();
            bool returnResult = false;
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DentrixAttachmentCall");
                bool IsSyncing = false;
                if (key != null)
                {
                    IsSyncing = bool.Parse(key.GetValue("TreatmentIsSyncing").ToString());
                }
                if (!IsSyncing)
                {
                    callloop = true;
                    RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                    key1.SetValue("TreatmentIsSyncing", true);

                    try
                    {
                        // RegistryKey keydocPath = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentDocumentPath");
                        //  keydocPath.SetValue("DentrixDocPath", docPath);

                        //  RegistryKey keyDocFileName = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentDocumentName");
                        //  keyDocFileName.SetValue("DentrixDocName", "Adit_PatientForm_"+DateTime.Now.ToString());  

                        myProcess.StartInfo.UseShellExecute = false;

                        myProcess.StartInfo.FileName = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName).ToString() + "\\DocumentDLL\\DentrixDocument.exe";
                        // myProcess.StartInfo.FileName = "C:\\Program Files (x86)\\Dentrix\\Document.CenterDoc.exe";
                        //  myProcess.StartInfo.Arguments = "" + patientId.ToString() + " " + Path.Combine(docPath) + " " + Utility.DentrixDocConnString + " " + Utility.DentrixDocPWD + "";
                        myProcess.StartInfo.Arguments = string.Format("{0} {1} {2} {3} {4} {5} {6}", "\"" + patientId.ToString() + "\"", "\"" + Path.Combine(docPath) + "\"", "\"" + Utility.DentrixDocConnString + "\"", "\"" + FormName + "\"", "\"" + "Treatment" + "\"", "\"" + "Patient Treatment" + "\"", "\"" + Utility.DentrixDocPWD + "\"");
                        // myProcess.StartInfo.Arguments = "" + patientId.ToString() + " \"C:\\Program Files (x86)\\PozativeDocument\\Patient\\1\\PendingDocument\\Remote Access Instructions.pdf\"";
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.StartInfo.Verb = "runas";
                        myProcess.Start();
                        myProcess.WaitForExit();
                        RegistryKey key2 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DentrixAttachmentCall");
                        if (bool.Parse(key2.GetValue("TreatmentIsSyncing").ToString()))
                        {
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (bool.Parse(key2.GetValue("TreatmentIsSyncing").ToString()))
                            {
                                returnResult = true;
                                if (sw.Elapsed > TimeSpan.FromSeconds(120))
                                {
                                    RegistryKey key3 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                                    key3.SetValue("TreatmentIsSyncing", false);
                                    returnResult = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            returnResult = true;
                        }

                        //if (!bool.Parse(key2.GetValue("IsSyncing").ToString()))
                        //{
                        //    return true;
                        //}
                        //else
                        //{
                        //    returnResult = false;
                        //    //return false;
                        //}                        
                    }
                    catch (Exception ex)
                    {
                        string abc = ex.Message;
                        key1.SetValue("TreatmentIsSyncing", false);
                        throw;
                    }
                }
                return returnResult;
            }
            catch (Exception e1)
            {
                RegistryKey key3 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                key3.SetValue("TreatmentIsSyncing", false);
                return returnResult;
            }
        }
        #endregion

        #region Patient_Form
        //public static bool Save_Patient_Form_Local_To_Dentrix(DataTable dtWebPatient_Form)
        //{
        //    string _successfullstataus = string.Empty;
        //    bool is_Record_Update = false;
        //    OdbcConnection conn = null;
        //    //MySqlCommand MySqlCommand = new MySqlCommand();
        //    OdbcCommand OdbcCommand = null;
        //    CommonDB.OdbcConnectionServer(ref conn);

        //    if (conn.State == ConnectionState.Closed) conn.Open();

        //    try
        //    {
        //        string OdbcSelect = string.Empty;
        //        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
        //        string Update_PatientForm_Record_ID = "";
        //        foreach (DataRow dr in dtWebPatient_Form.Rows)
        //        {
        //            if (dr["Patient_EHR_ID"].ToString() == "")
        //            {
        //                dr["Patient_EHR_ID"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["Patient_EHR_ID"].ToString()) != 0)
        //            {
        //                string strQauery = SynchDentrixQRY.Update_Patinet_Record_By_Patient_Form;

        //                strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());
        //                OdbcCommand.CommandText = strQauery;
        //                OdbcCommand.Parameters.Clear();
        //                OdbcCommand.Parameters.AddWithValue("ehrfield_value", dr["ehrfield_value"].ToString().Trim());
        //                OdbcCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
        //                //MySqlCommand.Parameters.AddWithValue("ehrfield", dr["ehrfield"].ToString().Trim());

        //                OdbcCommand.ExecuteNonQuery();
        //            }
        //        }
        //        string[] Update_PatientForm_Record_IDs;
        //        if (Update_PatientForm_Record_ID == string.Empty)
        //        {
        //            Update_PatientForm_Record_IDs = new string[0];
        //        }
        //        else
        //        {
        //            Update_PatientForm_Record_ID = Update_PatientForm_Record_ID.Remove(Update_PatientForm_Record_ID.Length - 1, 1);
        //            Update_PatientForm_Record_IDs = Update_PatientForm_Record_ID.ToString().Trim().Split(';');
        //        }

        //        DataView NewPatientListdv = new DataView(dtWebPatient_Form);
        //        NewPatientListdv.RowFilter = "Patient_EHR_ID = '0'";
        //        DataTable distinctValues = NewPatientListdv.ToTable(true, "PatientForm_Web_ID");

        //        foreach (DataRow dr in distinctValues.Rows)
        //        {
        //            string tmpNewPat = dr["PatientForm_Web_ID"].ToString();
        //            DataView NewPatientdv = new DataView(dtWebPatient_Form);
        //            NewPatientdv.RowFilter = "PatientForm_Web_ID = '" + tmpNewPat + "'";

        //            DataTable newPatientDt = NewPatientdv.ToTable();

        //            string tmpField = "";
        //            string tmpFiendValue = "";

        //            foreach (DataRow drNPat in newPatientDt.Rows)
        //            {
        //                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
        //                tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
        //            }

        //            tmpField = tmpField.Remove(tmpField.Length - 1, 1);
        //            tmpFiendValue = tmpFiendValue.Remove(tmpFiendValue.Length - 1, 1);

        //            string tmpNewPatQry = "Insert Into Patient (" + tmpField + ") Values (" + tmpFiendValue + ")";
        //            OdbcCommand.CommandText = tmpNewPatQry;
        //            OdbcCommand.ExecuteNonQuery();

        //            Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";

        //        }

        //        var JsonPatient_FormBO = new System.Text.StringBuilder();

        //        Push_Patient_FormBO Patient_FormBO = new Push_Patient_FormBO
        //        {
        //            id = Update_PatientForm_Record_IDs,
        //            status = "readytoimport",
        //        };
        //        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        JsonPatient_FormBO.Append(javaScriptSerializer.Serialize(Patient_FormBO) + ",");


        //        //  string jsonString = "[" + JsonPatient_FormBO.ToString().Remove(JsonPatient_FormBO.Length - 1) + "]";
        //        string jsonString = JsonPatient_FormBO.ToString().Remove(JsonPatient_FormBO.Length - 1);
        //        string RestURL = PullLiveDatabaseDAL.GetLiveRecord("PatientFormUpdateRecordID");
        //        var request = new RestRequest(Method.POST);
        //        var client = new RestClient(RestURL);
        //        ServicePointManager.Expect100Continue = true;
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        request.AddHeader("action", "EHRPFIMPORT");
        //        request.AddHeader("Content-Type", "application/json");
        //        request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
        //        IRestResponse response = client.Execute(request);

        //        if (response.ErrorMessage != null)
        //        {
        //            _successfullstataus = response.ErrorMessage;
        //        }

        //        is_Record_Update = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        is_Record_Update = false;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return is_Record_Update;
        //}


        public static bool Save_Patient_Form_Local_To_Dentrix(DataTable dtWebPatient_Form)
        {
            string _successfullstataus = string.Empty;
            bool is_Record_Update = false;
            OdbcConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            OdbcCommand OdbcCommand = null;
            CommonDB.OdbcConnectionServer(ref conn);

            Int32 ClmId = 0, EmpId = 0;
            int ColumnSize = 0;
            string SchlName = "";
            string EmpName = "";
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                //OdbcCommand.CommandTimeout = 200;
                string strQauery = string.Empty;
                string Update_PatientForm_Record_ID = "";
                string phoneNo = "";
                //string ColumnList = "";
                //string ValueList = "";
                Int64 patient_EHR_Id = 0;
                DataTable dtDentrixPatentColumns = GetDentrixTableColumnName("Patient");

                #region Insert Patient Into Dentrix

                patient_EHR_Id = 0;

                // DataTable dtDentrixPatentColumns = GetDentrixTableColumnName("Patient");
                string dentrixAddress = "";

                string LastName = "";

                //OdbcConnection conn = null;
                //OdbcCommand OdbcCommand = new OdbcCommand();
                //CommonDB.OdbcDentrixConnectionServer(ref conn);
                if (
                dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty).Count() > 0)
                {
                    dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {
                             string PatientEHRID = CheckDuplicatePatient(dtWebPatient_Form, o.ToString());
                             if (PatientEHRID == "")
                             {
                                 #region Update Patient Form Fields to other tables before Patient Insert

                                 foreach (DataRow dr in dtWebPatient_Form.Select(" PatientForm_Web_ID = '" + o.ToString() + "' "))
                                 {
                                     if (dr["ehrfield"] != null && dr["ehrfield_value"].ToString().Trim() != string.Empty)
                                     {
                                         if (dr["ehrfield"].ToString().Trim() != string.Empty)
                                         {
                                             if (dr["ehrfield"].ToString().ToLower() == "ssn")
                                             {
                                                 DataTable dtSSN = CheckDentrixDuplicatePatientSSNData(dr["ehrfield_value"].ToString().Trim());
                                                 if (dtSSN.Rows.Count > 0 && dtSSN.Rows[0][0] != null)
                                                 {
                                                     dr["ehrfield_value"] = null;
                                                 }
                                             }
                                         }

                                         if (dr["ehrfield"].ToString().Trim() != string.Empty)
                                         {
                                             if (dr["TableName"].ToString().Trim().ToLower() == "claiminfo" || dr["TableName"].ToString().Trim().ToLower() == "employer") //|| dr["TableName"].ToString().Trim().ToLower() == "school"
                                             {
                                                 if (dr["ehrfield"].ToString().ToLower() == "school")
                                                 {
                                                     SchlName = "";
                                                     ColumnSize = Convert.ToInt16(dtDentrixPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper() && r.Field<object>("ColumnSize") != null).Select(r => r.Field<object>("ColumnSize")).First().ToString());
                                                     if (dr["ehrfield_value"].ToString().Trim().Length > ColumnSize)
                                                     {
                                                         SchlName = (dr["ehrfield_value"].ToString().Trim().Substring(0, ColumnSize));
                                                     }
                                                     else
                                                     {
                                                         SchlName = (dr["ehrfield_value"].ToString().Trim());
                                                     }
                                                     ClmId = GetClaimId(SchlName);
                                                     if (ClmId == 0)
                                                     {
                                                         ClmId = InsertSchoolInClaimInfo(SchlName);
                                                     }
                                                 }
                                                 else if (dr["ehrfield"].ToString().Trim().ToLower() == "name") //|| dr["TableName"].ToString().Trim().ToLower() == "employer" || dr["ehrfield"].ToString().Trim().ToLower() == "name"
                                                 {
                                                     EmpName = "";
                                                     ColumnSize = Convert.ToInt16(dtDentrixPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == "NAME" && r.Field<object>("ColumnSize") != null).Select(r => r.Field<object>("ColumnSize")).First().ToString());

                                                     if (dr["ehrfield_value"].ToString().Length > ColumnSize)
                                                     {
                                                         EmpName = dr["ehrfield_value"].ToString().Trim().Substring(0, ColumnSize);
                                                     }
                                                     else
                                                     {
                                                         EmpName = dr["ehrfield_value"].ToString().Trim();
                                                     }
                                                     EmpId = GetEmpId(EmpName);
                                                     if (EmpId == 0)
                                                     {
                                                         EmpId = InsertEmployer(EmpName);
                                                     }
                                                 }
                                             }
                                         }
                                     }
                                 }

                                 #endregion

                                 string PrimaryInsuraceCompanyName = "";
                                 string SecondaryInsuraceCompanyName = "";
                                 string PrimaryInsuraceSubScriberId = "";
                                 string SecondaryInsuraceSubScriberId = "";
                                 strQauery = CreatePatientInsertQuery(dtWebPatient_Form, dtDentrixPatentColumns, o.ToString(), "Admin.Patient", ref dentrixAddress, ref LastName, ref PrimaryInsuraceCompanyName, ref SecondaryInsuraceCompanyName, ref PrimaryInsuraceSubScriberId, ref SecondaryInsuraceSubScriberId);
                                 if (conn.State == ConnectionState.Closed) conn.Open();
                                 CommonDB.OdbcCommandServer(dentrixAddress, conn, ref OdbcCommand, "txt");
                                 patient_EHR_Id = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                                 // Utility.WriteToErrorLogFromAll(dentrixAddress);
                                 dentrixAddress = "Select MAX(addrid) from admin.address";
                                 CommonDB.OdbcCommandServer(dentrixAddress, conn, ref OdbcCommand, "txt");
                                 //Utility.WriteToErrorLogFromAll(dentrixAddress);
                                 patient_EHR_Id = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                                 if (LastName.Length == 0)
                                 {
                                     LastName += "NA";
                                 }
                                 if (LastName.Length == 1)
                                 {
                                     LastName += "0";
                                 }
                                 string ChartCompare = "";
                                 //rooja - get dentrix auto chart number setup value
                                 GetDentrixChartNumberSettingsData();

                                 //int chartnum = GetDentrixChartNum(LastName.Substring(0, 2).ToString());
                                 string chartnum = GetDentrixChartNum(LastName.Substring(0, 2).ToString());

                                 if (chartnum == "0" || LastName == "" || LastName == string.Empty)
                                 {
                                     strQauery = strQauery.Replace("@chartnum", "NULL");
                                     ChartCompare = "NULL";
                                 }
                                 else
                                 {

                                     strQauery = strQauery.Replace("@chartnum", chartnum.ToString());
                                     ChartCompare = chartnum.ToString();

                                     //strQauery = strQauery.Replace("@chartnum", LastName.Substring(0, 2).ToString().ToUpper() + chartnum.ToString("0000"));
                                 }
                                 strQauery = strQauery.Replace("@addrid", patient_EHR_Id.ToString());
                                 CommonDB.OdbcCommandServer(strQauery, conn, ref OdbcCommand, "txt");
                                 //Utility.WriteToErrorLogFromAll(strQauery);
                                 OdbcCommand.ExecuteNonQuery();

                                 dentrixAddress = "Select MAX(patid) from admin.patient where trim(chartnum) = '" + ChartCompare + "'"; //dentrixAddress = "Select MAX(patid) from admin.patient";
                                 CommonDB.OdbcCommandServer(dentrixAddress, conn, ref OdbcCommand, "txt");
                                 patient_EHR_Id = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                                 //Utility.WriteToErrorLogFromAll(dentrixAddress);
                                 UpdateClaimIdInPatient(ClmId, patient_EHR_Id);
                                 UpdateEmpIdInPatient(EmpId, patient_EHR_Id);

                                 CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                                 OdbcCommand.CommandText = SynchDentrixQRY.UpdatePatientGuarantorID;
                                 OdbcCommand.Parameters.Clear();
                                 OdbcCommand.Parameters.AddWithValue("isguar", 1);
                                 OdbcCommand.Parameters.AddWithValue("isheadofhouse", 1);
                                 OdbcCommand.Parameters.AddWithValue("Guarantor", patient_EHR_Id);
                                 OdbcCommand.Parameters.AddWithValue("famid", patient_EHR_Id);
                                 OdbcCommand.Parameters.AddWithValue("patid", patient_EHR_Id);
                                 OdbcCommand.ExecuteNonQuery();
                                 conn.Close();
                                 InsertAgigPatient(Convert.ToInt64(patient_EHR_Id), conn);
                                 UpdatePatientInsurance(PrimaryInsuraceCompanyName, patient_EHR_Id, 1, PrimaryInsuraceSubScriberId);
                                 UpdatePatientInsurance(SecondaryInsuraceCompanyName, patient_EHR_Id, 2, SecondaryInsuraceSubScriberId);

                                 UpdatePatientEHRIdINPatientForm(patient_EHR_Id.ToString(), o.ToString().Trim());

                                 Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + o.ToString().Trim() + ";";
                             }
                             else
                             {
                                 foreach (DataRow dr in dtWebPatient_Form.Select(" PatientForm_Web_ID = '" + o.ToString() + "' "))
                                 {
                                     dr["Patient_EHR_ID"] = PatientEHRID.ToString();
                                 }
                                 UpdatePatientEHRIdINPatientForm(PatientEHRID.ToString(), o.ToString().Trim());
                             }
                             return true;
                         });

                }
                #endregion

                #region PatientForm Data
                if (dtWebPatient_Form.AsEnumerable()
                    .Where(o => o.Field<object>("Patient_EHR_ID") != null &&
                           o.Field<object>("Patient_EHR_ID").ToString() != string.Empty &&
                           o.Field<object>("PatientForm_Web_ID") != null &&
                           o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty &&
                           Convert.ToInt64(o.Field<object>("Patient_EHR_ID")) > 0
                           ).Count() > 0)
                {

                    dtWebPatient_Form.AsEnumerable()
                    .Where(o => o.Field<object>("Patient_EHR_ID") != null &&
                           o.Field<object>("Patient_EHR_ID").ToString() != string.Empty &&
                           o.Field<object>("PatientForm_Web_ID") != null &&
                           o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty &&
                           Convert.ToInt64(o.Field<object>("Patient_EHR_ID")) > 0
                           )
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {
                             foreach (DataRow dr in dtWebPatient_Form.Select(" PatientForm_Web_ID = '" + o.ToString() + "' "))
                             {
                                 if (dr["ehrfield"] != null || dr["ehrfield_value"].ToString().Trim() != string.Empty)
                                 {
                                     if (dr["ehrfield"].ToString().Trim() != string.Empty)
                                     {
                                         if (Convert.ToInt32(dr["Patient_EHR_ID"].ToString()) != 0)
                                         {

                                             if (dtDentrixPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper()).FirstOrDefault() != null)
                                             {
                                                 if (dr["ehrfield"].ToString().ToLower() != "addrid" && dr["ehrfield"].ToString() != "patguid" && dr["ehrfield"].ToString() != "chartnum" && dr["ehrfield"].ToString().Trim().ToLower() != "primary_insurance_companyname" && dr["ehrfield"].ToString().Trim().ToLower() != "secondary_insurance_companyname" && dr["ehrfield"].ToString().Trim().ToUpper() != "PRIMARY_SUBSCRIBER_ID" && dr["ehrfield"].ToString().Trim().ToUpper() != "SECONDARY_SUBSCRIBER_ID")
                                                 {
                                                     ColumnSize = Convert.ToInt16(dtDentrixPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper() && r.Field<object>("ColumnSize") != null).Select(r => r.Field<object>("ColumnSize")).First().ToString());
                                                     string ColumnType = dtDentrixPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper() && r.Field<object>("ColumnSize") != null).Select(r => r.Field<string>("EHRDataType")).First().ToString();


                                                     patient_EHR_Id = Convert.ToInt32(dr["Patient_EHR_ID"].ToString());

                                                     if (dr["ehrfield"].ToString().Trim().ToLower() == "street1" || dr["ehrfield"].ToString().Trim().ToLower() == "street2" ||
                                                            dr["ehrfield"].ToString().Trim().ToLower() == "city" || dr["ehrfield"].ToString().Trim().ToLower() == "state" || dr["ehrfield"].ToString().Trim().ToLower() == "zipcode" ||
                                                            dr["ehrfield"].ToString().Trim().ToLower() == "phone")
                                                     {

                                                         Int64 Addressid = GetDentrixAddressId(Convert.ToInt32(dr["Patient_EHR_ID"].ToString()));
                                                         strQauery = SynchDentrixQRY.Update_Patinet_Address_Record_By_Patient_Form;
                                                         strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());
                                                         if (dr["ehrfield"].ToString() != "")
                                                         {
                                                             if (dr["ehrfield"].ToString().Trim().ToLower() == "phone")
                                                             {
                                                                 phoneNo = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "").Trim().Replace("+", "");
                                                                 if (phoneNo.Length >= ColumnSize)
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + phoneNo.Substring(0, ColumnSize) + "'");
                                                                 }
                                                                 else
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + phoneNo + "'");
                                                                 }
                                                             }

                                                             else
                                                             {
                                                                 if (dr["ehrfield_value"].ToString().Trim().Length >= ColumnSize)
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim().Substring(0, ColumnSize) + "'");
                                                                 }
                                                                 else
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim() + "'");
                                                                 }
                                                             }
                                                         }
                                                         else
                                                         {
                                                             strQauery = strQauery.Replace("@ehrfield_value", "''");
                                                         }
                                                         strQauery = strQauery.Replace("@Patient_EHR_ID", Addressid.ToString());
                                                     }
                                                     else
                                                     {
                                                         strQauery = SynchDentrixQRY.Update_Patinet_Record_By_Patient_Form;
                                                         strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                                         if (dr["ehrfield"] != null || dr["ehrfield"].ToString() != "")
                                                         {
                                                             if (dr["ehrfield"].ToString().Trim().ToLower() == "workphone" || dr["ehrfield"].ToString().Trim().ToLower() == "pager")
                                                             {
                                                                 phoneNo = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "").Trim().Replace("+", "");
                                                                 if (phoneNo.Length >= ColumnSize)
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + phoneNo.Substring(0, ColumnSize) + "'");
                                                                 }
                                                                 else
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + phoneNo + "'");
                                                                 }
                                                             }
                                                             else if (dr["ehrfield"].ToString().Trim().ToLower() == "birthdate" || ColumnType.ToUpper() == "SYSTEM.DATETIME")
                                                             {
                                                                 try
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + Convert.ToDateTime(dr["ehrfield_value"]).ToString("yyyy/MM/dd HH:mm").ToString() + "'");

                                                                 }
                                                                 catch
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "NULL");

                                                                 }
                                                             }
                                                             else if (dr["ehrfield"].ToString().Trim().ToLower() == "ssn")
                                                             {

                                                                 try
                                                                 {
                                                                     if (dr["ehrfield_value"].ToString().Trim().ToLower().Length != 9)
                                                                     {
                                                                         //strQauery = strQauery.Replace("@ehrfield_value", "NULL");
                                                                         continue;
                                                                     }
                                                                     else
                                                                     {
                                                                         DataTable dtSSN = CheckDentrixDuplicatePatientSSNData(dr["ehrfield_value"].ToString().Trim());
                                                                         if (dtSSN.Rows.Count > 0 && dtSSN.Rows[0][0] != null)
                                                                         {
                                                                             continue;
                                                                         }
                                                                         else
                                                                         {
                                                                             strQauery = strQauery.Replace("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim() + "'");
                                                                         }
                                                                     }
                                                                 }
                                                                 catch
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "''");
                                                                 }
                                                             }
                                                             #region Update Patient Table For School & Employer
                                                             else if (dr["TableName"].ToString().Trim().ToLower() == "claiminfo" || dr["TableName"].ToString().Trim().ToLower() == "employer") //|| dr["TableName"].ToString().Trim().ToLower() == "school"
                                                             {
                                                                 if (dr["ehrfield"].ToString().ToLower() == "school")
                                                                 {
                                                                     SchlName = "";

                                                                     strQauery = SynchDentrixQRY.Update_Patinet_Record_By_Patient_Form;

                                                                     ColumnSize = Convert.ToInt16(dtDentrixPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper() && r.Field<object>("ColumnSize") != null).Select(r => r.Field<object>("ColumnSize")).First().ToString());

                                                                     if (dr["ehrfield_value"].ToString().Trim().Length > ColumnSize)
                                                                     {
                                                                         SchlName = (dr["ehrfield_value"].ToString().Trim().Substring(0, ColumnSize));
                                                                     }
                                                                     else
                                                                     {
                                                                         SchlName = (dr["ehrfield_value"].ToString().Trim());
                                                                     }
                                                                     ClmId = GetClaimId(SchlName);
                                                                     if (ClmId == 0)
                                                                     {
                                                                         ClmId = InsertSchoolInClaimInfo(SchlName);
                                                                     }
                                                                     strQauery = strQauery.Replace("ColumnName", "claiminfid");
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + ClmId + "'");
                                                                 }

                                                                 else if (dr["ehrfield"].ToString().Trim().ToLower() == "name") //|| dr["TableName"].ToString().Trim().ToLower() == "employer" || dr["ehrfield"].ToString().Trim().ToLower() == "name"
                                                                 {
                                                                     EmpName = "";
                                                                     strQauery = SynchDentrixQRY.Update_Patinet_Record_By_Patient_Form;

                                                                     ColumnSize = Convert.ToInt16(dtDentrixPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == "NAME" && r.Field<object>("ColumnSize") != null).Select(r => r.Field<object>("ColumnSize")).First().ToString());

                                                                     if (dr["ehrfield_value"].ToString().Length > ColumnSize)
                                                                     {
                                                                         EmpName = dr["ehrfield_value"].ToString().Trim().Substring(0, ColumnSize);
                                                                     }
                                                                     else
                                                                     {
                                                                         EmpName = dr["ehrfield_value"].ToString().Trim();
                                                                     }
                                                                     EmpId = GetEmpId(EmpName);
                                                                     if (EmpId == 0)
                                                                     {
                                                                         EmpId = InsertEmployer(EmpName);
                                                                     }
                                                                     strQauery = strQauery.Replace("ColumnName", "empid");
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + EmpId + "'");
                                                                 }
                                                             }
                                                             #endregion

                                                             else
                                                             {
                                                                 if (dr["ehrfield_value"].ToString().Trim().Length >= ColumnSize)
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim().Substring(0, ColumnSize) + "'");
                                                                 }
                                                                 else
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim() + "'");
                                                                 }
                                                             }
                                                         }
                                                         else
                                                         {
                                                             strQauery = strQauery.Replace("@ehrfield_value", "''");
                                                         }
                                                         strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                                     }
                                                     //strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                                     if (conn.State == ConnectionState.Closed) conn.Open();
                                                     CommonDB.OdbcCommandServer(strQauery, conn, ref OdbcCommand, "txt");

                                                     //#######---START due to provid1 is blank the error(RROR [86007] [FairCom][ODBC FairCom Driver 12.0.2.432(Build-220411)][ctreeSQL] -20148 Trigger Execution Failed.) was inturrupting the loop
                                                     if (dr["ehrfield"].ToString().Trim().ToLower() == "provid1" && (dr["ehrfield_value"] == null || dr["ehrfield_value"].ToString() == ""))
                                                     {
                                                         //System.Windows.Forms.MessageBox.Show("Found Blank provid1");//#######
                                                         continue;
                                                     }
                                                     //#######---END

                                                     OdbcCommand.ExecuteNonQuery();
                                                 }
                                             }
                                         }
                                     }
                                 }
                             }
                             UpdatePatientEHRIdINPatientForm(patient_EHR_Id.ToString(), o.ToString().Trim());
                             Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + o.ToString().Trim() + ";";
                             return true;
                         });
                }
                #endregion




                SynchLocalDAL.UpdatePatientFormEHR_Updateflg(dtWebPatient_Form);




                //   bool is_UpdateFlg = SynchLocalDAL.UpdatePatientFormSyncValue(NewPatientListUpdateFlgDt);

                is_Record_Update = true;
            }
            catch (Exception ex)
            {
                is_Record_Update = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return is_Record_Update;
        }

        #region New Patient Form Fields

        #region ClaimInfo

        private static int GetClaimId(string patschool)
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcDataReader OdbcRead;

            int ClaimId = 0;

            try
            {
                OdbcCommand OdbCommand = new OdbcCommand();


                if (conn.State == ConnectionState.Closed) conn.Open();

                string strQauery = "SELECT TOP 1 claiminfid FROM admin.claiminfo WHERE school = '" + patschool + "'";

                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                OdbCommand.CommandTimeout = 200;

                OdbcRead = OdbCommand.ExecuteReader();

                if (OdbcRead.HasRows)
                    ClaimId = Convert.ToInt16(OdbcRead["claiminfid"]);

                OdbcRead.Close();
                OdbcRead.Dispose();
                OdbCommand.Dispose();

            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

            return ClaimId;
        }

        private static int InsertSchoolInClaimInfo(string patschool)
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcDataAdapter OdbcDa = null;

            int ClaimId = 0;

            try
            {
                OdbcCommand OdbCommand = new OdbcCommand();

                if (conn.State == ConnectionState.Closed) conn.Open();

                string strQauery = "INSERT INTO admin.claiminfo (school,studentstatus) VALUES( '" + patschool + "',2)"; //INSERT INTO admin.claiminfo (school,studentstatus) VALUES(  'New High School' ,2)

                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                OdbCommand.CommandTimeout = 200;
                OdbCommand.ExecuteNonQuery();

                strQauery = "SELECT MAX(claiminfid) FROM admin.claiminfo";

                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                OdbCommand.CommandTimeout = 200;

                ClaimId = Convert.ToInt16(OdbCommand.ExecuteScalar());

                OdbCommand.Dispose();

            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

            return ClaimId;
        }

        public static void UpdateClaimIdInPatient(int claimid, long patid)
        {
            //bool Updated = false;
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcDataAdapter OdbcDa = null;

            try
            {
                OdbcCommand OdbCommand = new OdbcCommand();

                if (conn.State == ConnectionState.Closed) conn.Open();

                string strQauery = "UPDATE admin.patient SET claiminfid = '" + claimid + "'  WHERE patid = " + patid;

                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                OdbCommand.CommandTimeout = 200;

                OdbCommand.ExecuteNonQuery();

                OdbCommand.Dispose();

                //Updated = true;
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

            //return Updated;
        }

        #endregion

        #region Employer

        private static int GetEmpId(string empname)
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);

            int EmpId = 0;

            try
            {
                OdbcCommand OdbCommand = new OdbcCommand();
                OdbcDataReader OdbcRead;


                if (conn.State == ConnectionState.Closed) conn.Open();

                string strQauery = "SELECT top 1 empid FROM admin.employer WHERE name = '" + empname + "'";

                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                OdbCommand.CommandTimeout = 200;

                OdbcRead = OdbCommand.ExecuteReader();

                if (OdbcRead.HasRows)
                    EmpId = Convert.ToInt16(OdbcRead["empid"]);

                OdbcRead.Close();
                OdbcRead.Dispose();
                OdbCommand.Dispose();

            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

            return EmpId;
        }

        private static int InsertEmployer(string empname)
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcDataAdapter OdbcDa = null;

            int EmployerId = 0;

            try
            {
                OdbcCommand OdbCommand = new OdbcCommand();

                if (conn.State == ConnectionState.Closed) conn.Open();

                string strQauery = "INSERT INTO admin.employer (name) VALUES( '" + empname + "')";

                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                OdbCommand.CommandTimeout = 200;
                OdbCommand.ExecuteNonQuery();

                strQauery = "SELECT MAX(empid) FROM admin.employer";

                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                OdbCommand.CommandTimeout = 200;

                EmployerId = Convert.ToInt16(OdbCommand.ExecuteScalar());

                OdbCommand.Dispose();

            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

            return EmployerId;
        }

        public static void UpdateEmpIdInPatient(int empid, long patid)
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcDataAdapter OdbcDa = null;

            try
            {
                OdbcCommand OdbCommand = new OdbcCommand();

                if (conn.State == ConnectionState.Closed) conn.Open();

                string strQauery = "UPDATE admin.patient SET empid = '" + empid + "'  WHERE patid = " + patid;

                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                OdbCommand.CommandTimeout = 200;

                OdbCommand.ExecuteNonQuery();

                OdbCommand.Dispose();
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

        }


        #endregion

        #endregion

        private static void UpdatePatientInsurance(string curPatinetInsurance_Name, long PatientId, int InsuranceCount, string SubScriber)
        {
            if (curPatinetInsurance_Name == "")
            {
                return;
            }
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcDataAdapter OdbcDa = null;

            try
            {
                OdbcCommand OdbcCommand = new OdbcCommand();
                if (conn.State == ConnectionState.Closed) conn.Open();
                string strQauery = "select TOP 1 * from admin.inscarrier where replace(insconame,'''','') = '" + curPatinetInsurance_Name + "'";
                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbcCommand, "txt");
                OdbcCommand.CommandTimeout = 200;
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);

                DataTable table = new DataTable();
                OdbcDa.Fill(table);
                int inssubId = 0;
                if (table.Rows.Count > 0)
                {
                    //char[] separators = new char[] { '<', '>', '.', '?', '\t', '\n','/',':',';','\'','\"','{','}','[',']','\\','|','!','@','#','$','%',' ','^','&','*','(',')','-','_','+','=','~','`' };
                    //foreach (char c in separators)
                    //{ 
                    //    // SubScriber = SubScriber.Replace(c,'\0').Replace(" ","").ToString().Trim();
                    //    SubScriber = SubScriber.Replace(c.ToString(), "").ToString().Trim();
                    //}
                    OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_insurance;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("insplanid", Convert.ToInt32(table.Rows[0]["insid"]));
                    OdbcCommand.Parameters.AddWithValue("inspartyid", PatientId);
                    OdbcCommand.Parameters.AddWithValue("idnum", SubScriber.ToString().Trim().Length > 17 ? SubScriber.ToString().Trim().Substring(0, 17) : SubScriber.ToString().Trim());
                    OdbcCommand.ExecuteNonQuery();

                    string QryIdentity = "Select max(insuredid) as newId from admin.insured";
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.CommandType = CommandType.Text;
                    OdbcCommand.Connection = conn;
                    inssubId = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                    if (InsuranceCount == 1)
                    {
                        OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_primaryinsurance_patplan;
                    }
                    else
                    {
                        OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_secondaryinsurance_patplan;
                    }
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("insuredid", inssubId);
                    OdbcCommand.Parameters.AddWithValue("patid", PatientId);
                    OdbcCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private static void InsertAgigPatient(long PatientEHRID, OdbcConnection conn)
        {
            DataTable OdbcDt = new DataTable();
            using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
            {
                string OdbcSelect = " select * from admin.aging where guarid = ?";
                if (conn.State != ConnectionState.Open) conn.Open();
                OdbcCommand.CommandText = OdbcSelect;
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("guarid", PatientEHRID);

                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                {
                    OdbcDa.Fill(OdbcDt);
                }
            }
            if (OdbcDt.Rows.Count == 0)
            {
                using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                {
                    OdbcCommand.CommandType = CommandType.Text;
                    if (conn.State != ConnectionState.Open) conn.Open();
                    OdbcCommand.CommandText = SynchDentrixQRY.InsertPatientAggingGuarantorID;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("Guarantor", PatientEHRID);
                    OdbcCommand.Parameters.Add("Birthdate", OdbcType.Date).Value = Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd");
                    OdbcCommand.ExecuteNonQuery();
                }
            }
        }
        private static string CreatePatientInsertQuery(DataTable dtWebPatient_Form, DataTable dtDentrixPatentColumns, string patientFormWebId, string tableName, ref string dentrixAddress, ref string LastName, ref string PrimaryInsuranceCompanyName, ref string SecondaryInsuranceCompanyName, ref string PrimaryInsuranceSubScriberId, ref string SecondaryInsuraceSubScriberId)
        {
            try
            {
                string strQauery = "";
                string ColumnList = "";
                string ValueList = "";
                string AddressColumnList = "";
                string AddressValueList = "";
                string PatientLastName = "";
                string PInsuranceCompanyName = "";
                string SInsuranceCompanyName = "";
                string PInsuranceSubScriberId = "";
                string SInsuranceSubScriberId = "";
                bool IsSMS = false;
                dtDentrixPatentColumns.AsEnumerable().
                    Where(z => z.Field<string>("EHRColumnName") != "" &&
                        ((z.Field<string>("EHRColumnName").ToUpper() != "NAME") &&
                        (z.Field<string>("EHRColumnName").ToUpper() != "SCHOOL")))
                    .All(e =>
                    {
                        var dtColumnsExists = dtWebPatient_Form.AsEnumerable()
                            .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == patientFormWebId.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == e.Field<object>("EHRColumnName").ToString().ToUpper());


                        if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "lastname")
                        {
                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.FirstOrDefault() != null)
                            {
                                PatientLastName = dtColumnsExists.FirstOrDefault().Field<object>("ehrfield_value").ToString().Trim();
                            }
                            else
                            {
                                PatientLastName = "NA";
                            }
                        }
                        #region Insurance
                        if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME")
                        {
                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.FirstOrDefault() != null && dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                            {
                                PInsuranceCompanyName = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                            }
                            else
                            {
                                PInsuranceCompanyName = "";
                            }
                        }
                        else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME")
                        {
                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.FirstOrDefault() != null && dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                            {
                                SInsuranceCompanyName = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                            }
                            else
                            {
                                SInsuranceCompanyName = "";
                            }
                        }
                        else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                        {
                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.FirstOrDefault() != null && dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                            {
                                PInsuranceSubScriberId = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                            }
                            else
                            {
                                PInsuranceSubScriberId = "";
                            }
                        }
                        else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                        {
                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.FirstOrDefault() != null && dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                            {
                                SInsuranceSubScriberId = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                            }
                            else
                            {
                                SInsuranceSubScriberId = "";
                            }
                        }


                        #endregion

                        #region Address




                        else if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "street1" || e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "street2" ||
                            e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "city" || e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "state" ||
                             e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "zipcode" || e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "phone")
                        {
                            AddressColumnList = AddressColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";
                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0)
                            {
                                if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "phone")
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        string phoneno = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "").Trim().Replace("+", "");
                                        if (phoneno.Length >= Convert.ToInt16(e.Field<object>("ColumnSize").ToString()))
                                        {
                                            AddressValueList = AddressValueList + "'" + phoneno.Substring(0, Convert.ToInt16(e.Field<object>("ColumnSize").ToString())) + "'" + ",";
                                        }
                                        else
                                        {
                                            AddressValueList = AddressValueList + "'" + phoneno + "'" + ",";
                                        }
                                    }
                                    else
                                    {
                                        AddressValueList = AddressValueList + "''" + ",";
                                    }
                                }
                                else
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {

                                        if (dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Length >= Convert.ToInt16(e.Field<object>("ColumnSize").ToString()))
                                        {
                                            AddressValueList = AddressValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Substring(0, Convert.ToInt16(e.Field<object>("ColumnSize").ToString())) + "'" + ",";
                                        }
                                        else
                                        {
                                            AddressValueList = AddressValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "'" + ",";
                                        }


                                        //AddressValueList = AddressValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "'" + ",";
                                    }
                                    else if (e.Field<object>("DefaultValue") != null && e.Field<object>("DefaultValue").ToString().Trim().ToUpper() != "")
                                    {
                                        if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT16" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT32" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.DECIMAL")
                                        {
                                            AddressValueList = AddressValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                        }
                                        else
                                        {
                                            AddressValueList = AddressValueList + "'" + e.Field<object>("DefaultValue").ToString() + "'" + ",";
                                        }
                                    }
                                    else
                                    {
                                        AddressValueList = AddressValueList + "NULL" + ",";
                                    }
                                }

                            }
                            else
                            {
                                AddressValueList = AddressValueList + "''" + ",";

                            }
                        }
                        #endregion

                        #region patientprivacyflags
                        else if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "privacyflags")
                        {
                            if (!IsSMS)
                            {
                                ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";

                                if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.First() != null && dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                {
                                    ValueList = ValueList + "" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "" + ",";
                                }
                                else if (e.Field<object>("DefaultValue") != null && e.Field<object>("DefaultValue").ToString().Trim().ToUpper() != "")
                                {
                                    ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                }
                                else
                                {
                                    ValueList = ValueList + "0,";
                                }

                            }
                            if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "privacyflags")
                            {
                                IsSMS = true;
                            }
                        }
                        else if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "provid1" || e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "provid2")
                        {
                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.First().Field<object>("ehrfield_value") != null && dtColumnsExists.First().Field<object>("ehrfield_value").ToString() != "")
                            {
                                ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";
                                ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString() + "',";
                            }
                            else
                            {
                                ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";
                                ValueList = ValueList + "'" + e.Field<object>("DefaultValue").ToString() + "',";
                            }
                        }
                        #endregion

                        #region patguid
                        else if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "patguid")
                        {
                            ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";
                            ValueList = ValueList + "'" + Guid.NewGuid().ToString() + "',";
                        }
                        #endregion

                        #region patient
                        else
                        {
                            ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ","; //ColumnList = ColumnList + " " + e.Field<object>("EHRColumnName").ToString().Trim() + ",";
                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0)
                            {
                                if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "workphone" || e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "pager")
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        string PhoneNumber = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "").Trim().Replace("+", "");
                                        if (PhoneNumber.Length >= Convert.ToInt16(e.Field<object>("ColumnSize").ToString()))
                                        {
                                            ValueList = ValueList + "'" + PhoneNumber.Substring(0, Convert.ToInt16(e.Field<object>("ColumnSize").ToString())) + "'" + ",";
                                        }
                                        else
                                        {
                                            ValueList = ValueList + "'" + PhoneNumber + "'" + ",";
                                        }
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "'',";
                                    }
                                }
                                else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "BIRTHDATE")
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        try
                                        {
                                            ValueList = ValueList + "'" + Convert.ToDateTime(dtColumnsExists.First().Field<object>("ehrfield_value")).ToString("yyyy/MM/dd HH:mm").ToString() + "'" + ",";
                                        }
                                        catch
                                        {
                                            ValueList = ValueList + "NULL,";
                                        }
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "NULL,";
                                    }
                                }
                                else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "SSN")
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        try
                                        {
                                            if (dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Length != 9)
                                            {
                                                ValueList = ValueList + "''" + ",";
                                            }
                                            else
                                            {
                                                ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().ToString() + "'" + ",";
                                            }

                                        }
                                        catch
                                        {
                                            ValueList = ValueList + "'',";
                                        }
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "NULL,";
                                    }
                                }
                                else
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        if (dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Length >= Convert.ToInt16(e.Field<object>("ColumnSize").ToString()))
                                        {
                                            ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Substring(0, Convert.ToInt16(e.Field<object>("ColumnSize").ToString())) + "'" + ",";
                                        }
                                        else
                                        {
                                            ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "'" + ",";
                                        }

                                    }
                                    else if (e.Field<object>("DefaultValue") != null && e.Field<object>("DefaultValue").ToString().Trim().ToUpper() != "")
                                    {
                                        if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT16" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT32" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.DECIMAL")
                                        {
                                            ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                        }
                                        else
                                        {
                                            ValueList = ValueList + "'" + e.Field<object>("DefaultValue").ToString() + "'" + ",";
                                        }
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "NULL" + ",";
                                    }

                                }
                            }
                            #endregion

                            #region SetDefaultValue
                            else
                            {
                                if (e.Field<object>("EHRDataType") != null && e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.DATETIME")
                                {
                                    ValueList = ValueList + "NULL" + ",";
                                }
                                else if (e.Field<object>("DefaultValue") != null && e.Field<object>("DefaultValue").ToString().Trim().ToUpper() != "")
                                {
                                    if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT16" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT32" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.DECIMAL")
                                    {
                                        ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "'" + e.Field<object>("DefaultValue").ToString() + "'" + ",";
                                    }
                                }
                                else if (e.Field<object>("AllowNull").ToString().Trim().ToUpper() == "TRUE")
                                {
                                    ValueList = ValueList + "NULL" + ",";
                                }
                                else
                                {
                                    ValueList = ValueList + "''" + ",";
                                }
                            }
                            #endregion
                        }
                        return true;
                    });

                ColumnList = ColumnList.Substring(0, ColumnList.Length - 1);
                ValueList = ValueList.Substring(0, ValueList.Length - 1);
                AddressColumnList = AddressColumnList.Substring(0, AddressColumnList.Length - 1);
                AddressValueList = AddressValueList.Substring(0, AddressValueList.Length - 1);
                strQauery = " Insert into " + tableName + " ( " + ColumnList + " ) VALUES ( " + ValueList + " )";
                dentrixAddress = " Insert into admin.Address ( " + AddressColumnList + ",ptrcount,_unused1 ) VALUES ( " + AddressValueList + ",1,0 )";
                LastName = PatientLastName;
                PrimaryInsuranceCompanyName = PInsuranceCompanyName;
                SecondaryInsuranceCompanyName = SInsuranceCompanyName;
                PrimaryInsuranceSubScriberId = PInsuranceSubScriberId;
                SecondaryInsuraceSubScriberId = SInsuranceSubScriberId;
                return strQauery;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool UpdatePatientEHRIdINPatientForm(string PatientEHRId, string PatientFormWebId)
        {
            try
            {
                bool _successfullstataus = true;
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string sqlSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientForm_PatientEHRId;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_Id", PatientEHRId.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", PatientFormWebId.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                            SqlCeCommand.ExecuteNonQuery();
                        }
                        _successfullstataus = true;

                    }
                    catch (Exception ex)
                    {
                        _successfullstataus = false;
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
                return _successfullstataus;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static DataTable GetDentrixTableColumnName(string tableName)
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcDataAdapter OdbcDa = null;

            using (SqlCeConnection conn1 = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {

                    //CommonDB.OdbcConnectionServer(ref conn);
                    OdbcCommand OdbcCommand = new OdbcCommand();
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string strQauery = "select TOP 1 * from admin." + tableName;
                    CommonDB.OdbcCommandServer(strQauery, conn, ref OdbcCommand, "txt");
                    OdbcCommand.CommandTimeout = 200;
                    // CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DbDataReader reader = OdbcCommand.ExecuteReader();
                    DataTable table = reader.GetSchemaTable();
                    DataTable dtResult = table;

                    strQauery = "select TOP 1 * from admin.address";
                    CommonDB.OdbcCommandServer(strQauery, conn, ref OdbcCommand, "txt");
                    OdbcCommand.CommandTimeout = 200;
                    // CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DbDataReader reader1 = OdbcCommand.ExecuteReader();
                    DataTable table1 = reader1.GetSchemaTable();
                    DataTable dtResultAddress = table1;
                    DataTable OdbcDt = null;


                    string SqlCeSelect = " SELECT COLUMN_NAME,'' AS EHRColumnName,'' AS EHRDataType,'' AS AllowNull,'' AS DefaultValue,0 as ColumnSize  FROM information_Schema.columns where table_name = '" + tableName + "'"; ;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn1))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            OdbcDt = new DataTable();
                            SqlCeDa.Fill(OdbcDt);
                        }
                    }

                    //OdbcCommand.Parameters.AddWithValue("lastname", LastName);
                    //OdbcCommand.Parameters.AddWithValue("firstname", FirstName);
                    //OdbcCommand.Parameters.AddWithValue("mi", MiddleName);
                    //OdbcCommand.Parameters.AddWithValue("provid1", PriProv);
                    //OdbcCommand.Parameters.AddWithValue("isguar", 0);
                    //OdbcCommand.Parameters.AddWithValue("gender", 1);
                    //OdbcCommand.Parameters.AddWithValue("firstvisitdate", Convert.ToDateTime(DateFirstVisit).ToString("yyyy-MM-dd"));
                    //OdbcCommand.Parameters.AddWithValue("emailaddr", Email);
                    //OdbcCommand.Parameters.AddWithValue("pager", Mobile);
                    //OdbcCommand.Parameters.AddWithValue("patguid", Guid.NewGuid().ToString());
                    //OdbcCommand.Parameters.AddWithValue("addrid", AddressID);

                    OdbcDt.AsEnumerable()
                        .All(a =>
                        {
                            if (a["COLUMN_NAME"].ToString().ToUpper() == "PATIENT_EHR_ID")
                            {
                                a["EHRColumnName"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString() == "First_name")
                            {
                                a["EHRColumnName"] = "firstname";
                                a["DefaultValue"] = "NA";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "LAST_NAME")
                            {
                                a["EHRColumnName"] = "lastname";
                                a["DefaultValue"] = "NA";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MOBILE")
                            {
                                a["EHRColumnName"] = "pager";
                                a["DefaultValue"] = "000000000";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "STATUS")
                            {
                                a["EHRColumnName"] = "status";
                                a["DefaultValue"] = 1;
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToLower() == "address1")
                            {
                                a["EHRColumnName"] = "street1";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToLower() == "address2")
                            {
                                a["EHRColumnName"] = "street2";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "BIRTH_DATE")
                            {
                                a["EHRColumnName"] = "birthDate";
                                a["DefaultValue"] = null;
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MARITALSTATUS")
                            {
                                a["EHRColumnName"] = "fampos";
                                a["DefaultValue"] = 1;
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "STATE")
                            {
                                a["EHRColumnName"] = "state";
                                //a["DefaultValue"] = null;
                            }


                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "CITY")
                            {
                                a["EHRColumnName"] = "city";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "CURRENT_BAL")
                            {
                                a["EHRColumnName"] = "";
                                a["DefaultValue"] = "0";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "THIRTY_DAY")
                            {
                                a["EHRColumnName"] = "";
                                a["DefaultValue"] = "0";
                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SIXTY_DAY")
                            {
                                a["EHRColumnName"] = "";
                                a["DefaultValue"] = "0";
                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "NINETY_DAY")
                            {
                                a["EHRColumnName"] = "";
                                a["DefaultValue"] = "0";
                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "EMAIL")
                            {
                                a["EHRColumnName"] = "emailaddr";

                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "HOME_PHONE")
                            {
                                a["EHRColumnName"] = "phone";

                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MIDDLE_NAME")
                            {
                                a["EHRColumnName"] = "mi";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PREFERRED_NAME")
                            {
                                a["EHRColumnName"] = "prefname";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRI_PROVIDER_ID")
                            {
                                a["EHRColumnName"] = "provid1";
                                a["DefaultValue"] = GetDentrixIdelProvider().Rows[0]["provider_id"].ToString();
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE")
                            {
                                a["EHRColumnName"] = "";

                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME")
                            {
                                a["EHRColumnName"] = "PRIMARY_INSURANCE_COMPANYNAME";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INS_SUBSCRIBER_ID")
                            {
                                a["EHRColumnName"] = "PRIMARY_SUBSCRIBER_ID";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "RECEIVE_EMAIL")
                            {
                                a["EHRColumnName"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "RECEIVEEMAIL")
                            {
                                a["COLUMN_NAME"] = "RECEIVE_EMAIL";
                                a["EHRColumnName"] = "privacyflags";
                                a["DefaultValue"] = 0;
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "RECEIVESMS")
                            {
                                a["COLUMN_NAME"] = "RECEIVE_SMS";
                                a["EHRColumnName"] = "privacyflags";
                                a["DefaultValue"] = 0;
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SEC_PROVIDER_ID")
                            {
                                a["EHRColumnName"] = "provid2";
                                a["DefaultValue"] = GetDentrixIdelProvider(false).Rows[0]["provider_id"].ToString();
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE")
                            {
                                a["EHRColumnName"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME")
                            {
                                a["EHRColumnName"] = "SECONDARY_INSURANCE_COMPANYNAME";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INS_SUBSCRIBER_ID")
                            {
                                a["EHRColumnName"] = "SECONDARY_SUBSCRIBER_ID";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SEX")
                            {
                                a["EHRColumnName"] = "gender";
                                a["DefaultValue"] = 1;
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "WORK_PHONE")
                            {
                                a["EHRColumnName"] = "workphone";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "ZIPCODE")
                            {
                                a["EHRColumnName"] = "zipcode";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SALUTATION")
                            {
                                a["EHRColumnName"] = "salutation";
                                a["DefaultValue"] = "";
                            }

                            #region Patient Form New EHR Fields

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SSN")
                            {
                                a["EHRColumnName"] = "ssn";
                                a["DefaultValue"] = "";
                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "DRIVERLICENSE")
                            {
                                a["EHRColumnName"] = "driverslicense";
                                a["DefaultValue"] = "";
                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "EMPLOYER")
                            {
                                //a["TableName"] = "employer";
                                a["EHRColumnName"] = "name";
                                a["DefaultValue"] = "";
                                a["ColumnSize"] = "32";
                                //a["ehrfield"] = "employer";
                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SCHOOL")
                            {
                                //a["TableName"] = "claiminfo";
                                a["EHRColumnName"] = "school";
                                a["DefaultValue"] = "";
                                a["ColumnSize"] = "24";
                            }


                            #endregion

                            if (a["EHRColumnName"].ToString() != "")
                            {
                                if (a["EHRColumnName"].ToString().Trim().ToLower() == "street1" || a["EHRColumnName"].ToString().Trim().ToLower() == "street2" ||
                                  a["EHRColumnName"].ToString().Trim().ToLower() == "city" || a["EHRColumnName"].ToString().Trim().ToLower() == "state" || a["EHRColumnName"].ToString().Trim().ToLower() == "phone" ||
                                  a["EHRColumnName"].ToString().Trim().ToLower() == "zipcode")
                                {
                                    if (dtResultAddress.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Count() > 0)
                                    {
                                        a["EHRDataType"] = dtResultAddress.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<string>("COLUMNNAME").GetType()).First().ToString();
                                        a["AllowNull"] = dtResultAddress.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<object>("AllowDbNull")).First().ToString();
                                        if (a["EHRDataType"].ToString().Trim().ToUpper() == "SYSTEM.STRING")
                                        {
                                            a["ColumnSize"] = dtResultAddress.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<object>("NumericPrecision")).First().ToString();
                                        }
                                        else
                                        {
                                            a["ColumnSize"] = dtResultAddress.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<object>("ColumnSize")).First().ToString();
                                        }
                                    }
                                    else
                                    {
                                        a["EHRDataType"] = "System.String";
                                        a["AllowNull"] = "Yes";
                                    }
                                }
                                else if (dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Count() > 0)
                                {
                                    a["EHRDataType"] = dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<string>("COLUMNNAME").GetType()).First().ToString();
                                    a["AllowNull"] = dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<object>("AllowDbNull")).First().ToString();
                                    if (a["EHRDataType"].ToString().Trim().ToUpper() == "SYSTEM.STRING")
                                    {
                                        a["ColumnSize"] = dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<object>("NumericPrecision")).First().ToString();
                                    }
                                    else
                                    {
                                        a["ColumnSize"] = dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<object>("ColumnSize")).First().ToString();
                                    }
                                }

                                else
                                {
                                    a["EHRDataType"] = "System.String";
                                    a["AllowNull"] = "Yes";
                                }
                                if (a["EHRColumnName"].ToString().Trim().ToLower() == "phone" || a["EHRColumnName"].ToString().Trim().ToLower() == "workphone" || a["EHRColumnName"].ToString().Trim().ToLower() == "pager")
                                {
                                    a["ColumnSize"] = 10;
                                }
                            }

                            return true;
                        });


                    DataRow drNewRow = OdbcDt.NewRow();
                    drNewRow["COLUMN_NAME"] = "AddressId";
                    drNewRow["EHRColumnName"] = "addrid";
                    drNewRow["EHRDataType"] = "System.String";
                    drNewRow["AllowNull"] = "No";
                    drNewRow["DefaultValue"] = "@addrid";
                    OdbcDt.Rows.Add(drNewRow);

                    DataRow drNewRow1 = OdbcDt.NewRow();
                    drNewRow1["COLUMN_NAME"] = "patguid";
                    drNewRow1["EHRColumnName"] = "patguid";
                    drNewRow1["EHRDataType"] = "System.String";
                    drNewRow1["AllowNull"] = "No";
                    drNewRow1["DefaultValue"] = Guid.NewGuid().ToString();
                    OdbcDt.Rows.Add(drNewRow1);

                    DataRow drNewRow2 = OdbcDt.NewRow();
                    drNewRow2["COLUMN_NAME"] = "chartnum";
                    drNewRow2["EHRColumnName"] = "chartnum";
                    drNewRow2["EHRDataType"] = "System.String";
                    drNewRow2["AllowNull"] = "Yes";
                    drNewRow2["DefaultValue"] = "@chartnum";
                    OdbcDt.Rows.Add(drNewRow2);


                    return OdbcDt;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn1.State == ConnectionState.Open) conn1.Close();
                }

            }

        }

        public static bool Save_Document_in_Dentrix(string strPatientFormID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            bool callLoop = false;
            try
            {
                CommonDB.OdbcConnectionServer(ref conn);
                DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLiveDentrixPatientFormDocData("1", strPatientFormID);
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {

                    RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                    key2.SetValue("IsSyncing", false);
                    string ShowingName, SubmitedDate, FormName, PatientName = "";

                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        DataTable dt = CheckDentrixDuplicatePatientData("", "", dr["Patient_EHR_ID"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            if (callLoop == false)
                            {
                                string sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientDoc_Name"].ToString();
                                if (!System.IO.File.Exists(sourceLocation))
                                {
                                    if (SynchLocalDAL.CheckLivePatientFormDocDataSynced("1", dr["PatientDoc_Web_ID"].ToString()))
                                    {
                                        PullLiveDatabaseDAL.Update_PatientDocNotFound_Live_To_Local(dr["PatientDoc_Web_ID"].ToString(), "1");
                                    }
                                    continue;
                                }

                                string tmpFileOrgName = Path.GetFileName(sourceLocation);
                                string SourcePath = Path.GetDirectoryName(sourceLocation);

                                Thread.Sleep(100);

                                SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString("MM/dd/yy") : Convert.ToDateTime(dr["Entry_DateTime"]).ToString("MM/dd/yy");
                                FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                                PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString() : "";

                                try
                                {
                                    //showingName = dr["Form_Name"].ToString().Trim() + "-" + dr["Patient_Name"].ToString().Trim();
                                    switch (dr["DocNameFormat"].ToString().Trim())
                                    {
                                        case "DS-FN-PN":
                                            //SubmitedDate +"-"+
                                            ShowingName = FormName + "-" + PatientName;
                                            break;
                                        case "DS-PN-FN":
                                            //SubmitedDate + "-" +
                                            ShowingName = PatientName + "-" + FormName;
                                            break;
                                        case "DS-PN":
                                            //SubmitedDate + "-" +
                                            ShowingName = PatientName;
                                            break;
                                        case "DS-FN":
                                            //SubmitedDate + "-" +
                                            ShowingName = FormName;
                                            break;
                                        case "DS":
                                            ShowingName = "";
                                            break;
                                        default:
                                            ShowingName = SubmitedDate;
                                            break;
                                    }
                                    //DateTime.Now.ToString("HH:mm:ss")
                                    if (ShowingName.Length > 40)
                                    {
                                        ShowingName = ShowingName.Substring(0, 38);
                                    }
                                }
                                catch
                                {
                                    ShowingName = "";
                                }
                                if (AttachPatientDocument(sourceLocation, "/ID" + dr["Patient_EHR_ID"].ToString(), ShowingName, dr["folder_name"].ToString(), ref callLoop))
                                {
                                    RegistryKey key1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DocumentAttachedId");
                                    Int64 SavePatientDocId = 0;
                                    if (key1 != null)
                                    {
                                        SavePatientDocId = Convert.ToInt64(key1.GetValue("DocumentId").ToString());
                                        //MessageBox.Show("Document Attached succeddfully " + dr["Patient_EHR_ID"].ToString() + " Document_id :" + SavePatientDocId.ToString());
                                        if (SavePatientDocId > 0)
                                        {
                                            PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), SavePatientDocId.ToString(), "1");
                                            File.Delete(sourceLocation);
                                            Save_DocumentAttachment_in_Dentrix(dr["PatientDoc_Web_ID"].ToString());
                                        }
                                        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DocumentAttachedId");
                                        key.SetValue("DocumentId", 0);
                                    }
                                    callLoop = false;
                                }
                                else
                                {
                                    callLoop = false;
                                }
                            }
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (callLoop)
                            {
                                if (sw.Elapsed > TimeSpan.FromSeconds(120))
                                {
                                    callLoop = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_DocumentAttachment_in_Dentrix(string PatientForm_web_Id)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            bool callLoop = false;
            try
            {
                CommonDB.OdbcConnectionServer(ref conn);
                DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLiveDentrixPatientFormDocAttachmentData("1", PatientForm_web_Id);
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {

                    RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                    key2.SetValue("IsSyncing", false);
                    string ShowingName, SubmitedDate, FormName, PatientName = "";
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        DataTable dt = CheckDentrixDuplicatePatientData("", "", dr["Patient_EHR_ID"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            if (callLoop == false)
                            {
                                string sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientDoc_Name"].ToString();
                                if (!System.IO.File.Exists(sourceLocation))
                                {
                                    if (SynchLocalDAL.CheckLivePatientFormDocAttachmentDataSynced("1", dr["PatientDoc_Web_ID"].ToString()))
                                    {
                                        PullLiveDatabaseDAL.Update_PatientDocAttachmentNotFound_Live_To_Local(dr["PatientForm_web_Id"].ToString(), "1");
                                    }
                                    continue;
                                }
                                else
                                {
                                    long length = new System.IO.FileInfo(sourceLocation).Length;
                                    if (length <= 0)
                                    {
                                        PullLiveDatabaseDAL.Update_PatientDocAttachmentNotFound_Live_To_Local(dr["PatientForm_web_Id"].ToString(), "1");
                                        continue;
                                    }
                                }
                                string tmpFileOrgName = Path.GetFileName(sourceLocation);
                                string SourcePath = Path.GetDirectoryName(sourceLocation);

                                Thread.Sleep(100);

                                SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString("MM/dd/yy") : Convert.ToDateTime(dr["Entry_DateTime"]).ToString("MM/dd/yy");
                                FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                                PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString() : "";

                                try
                                {
                                    //showingName = dr["Form_Name"].ToString().Trim() + "-" + dr["Patient_Name"].ToString().Trim();
                                    switch (dr["DocNameFormat"].ToString().Trim())
                                    {
                                        case "FN-PN-DS":
                                            //SubmitedDate +"-"+
                                            ShowingName = FormName + "-" + PatientName + "-" + SubmitedDate;
                                            break;
                                        case "DS-FN-PN":
                                            //SubmitedDate +"-"+
                                            ShowingName = FormName + "-" + PatientName;
                                            break;
                                        case "DS-PN-FN":
                                            //SubmitedDate + "-" +
                                            ShowingName = PatientName + "-" + FormName;
                                            break;
                                        case "DS-PN":
                                            //SubmitedDate + "-" +
                                            ShowingName = PatientName;
                                            break;
                                        case "DS-FN":
                                            //SubmitedDate + "-" +
                                            ShowingName = FormName;
                                            break;
                                        case "DS":
                                            ShowingName = "";
                                            break;
                                        default:
                                            ShowingName = SubmitedDate;
                                            break;
                                    }
                                    //DateTime.Now.ToString("HH:mm:ss")
                                    if (ShowingName.Length > 40)
                                    {
                                        ShowingName = ShowingName.Substring(0, 38);
                                    }
                                }
                                catch
                                {
                                    ShowingName = "";
                                }
                                if (AttachPatientDocument(sourceLocation, "/ID" + dr["Patient_EHR_ID"].ToString(), ShowingName, dr["folder_name"].ToString(), ref callLoop))
                                {
                                    RegistryKey key1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DocumentAttachedId");
                                    Int64 SavePatientDocId = 0;
                                    if (key1 != null)
                                    {
                                        SavePatientDocId = Convert.ToInt64(key1.GetValue("DocumentId").ToString());
                                        //MessageBox.Show("Document Attached succeddfully " + dr["Patient_EHR_ID"].ToString() + " Document_id :" + SavePatientDocId.ToString());
                                        if (SavePatientDocId > 0)
                                        {
                                            PullLiveDatabaseDAL.Update_PatientFormDocAttachment_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), SavePatientDocId.ToString(), "1");
                                            File.Delete(sourceLocation);
                                        }
                                        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DocumentAttachedId");
                                        key.SetValue("DocumentId", 0);
                                    }
                                    callLoop = false;
                                }
                                else
                                {
                                    callLoop = false;
                                }
                            }
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (callLoop)
                            {
                                if (sw.Elapsed > TimeSpan.FromSeconds(120))
                                {
                                    callLoop = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        private static bool AttachPatientDocument(string docPath, string patientId, string FormName, string FolderName, ref bool callloop)
        {
            Process myProcess = new Process();
            bool returnResult = false;
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DentrixAttachmentCall");
                bool IsSyncing = false;
                if (key != null)
                {
                    IsSyncing = bool.Parse(key.GetValue("IsSyncing").ToString());
                }
                if (!IsSyncing)
                {
                    callloop = true;
                    RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                    key1.SetValue("IsSyncing", true);

                    try
                    {
                        // RegistryKey keydocPath = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentDocumentPath");
                        //  keydocPath.SetValue("DentrixDocPath", docPath);

                        //  RegistryKey keyDocFileName = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentDocumentName");
                        //  keyDocFileName.SetValue("DentrixDocName", "Adit_PatientForm_"+DateTime.Now.ToString());  

                        myProcess.StartInfo.UseShellExecute = false;

                        myProcess.StartInfo.FileName = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName).ToString() + "\\DocumentDLL\\DentrixDocument.exe";
                        // myProcess.StartInfo.FileName = "C:\\Program Files (x86)\\Dentrix\\Document.CenterDoc.exe";
                        //  myProcess.StartInfo.Arguments = "" + patientId.ToString() + " " + Path.Combine(docPath) + " " + Utility.DentrixDocConnString + " " + Utility.DentrixDocPWD + "";
                        myProcess.StartInfo.Arguments = string.Format("{0} {1} {2} {3} {4} {5} {6}", "\"" + patientId.ToString() + "\"", "\"" + Path.Combine(docPath) + "\"", "\"" + Utility.DentrixDocConnString + "\"", "\"" + FormName + "\"", "\"Document\"", "\"" + FolderName + "\"", "\"" + Utility.DentrixDocPWD + "\"");
                        // myProcess.StartInfo.Arguments = "" + patientId.ToString() + " \"C:\\Program Files (x86)\\PozativeDocument\\Patient\\1\\PendingDocument\\Remote Access Instructions.pdf\"";
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.StartInfo.Verb = "runas";
                        myProcess.Start();
                        myProcess.WaitForExit();
                        RegistryKey key2 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DentrixAttachmentCall");
                        if (bool.Parse(key2.GetValue("IsSyncing").ToString()))
                        {
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (bool.Parse(key2.GetValue("IsSyncing").ToString()))
                            {
                                returnResult = true;
                                if (sw.Elapsed > TimeSpan.FromSeconds(120))
                                {
                                    RegistryKey key3 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                                    key3.SetValue("IsSyncing", false);
                                    returnResult = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            returnResult = true;
                        }

                        //if (!bool.Parse(key2.GetValue("IsSyncing").ToString()))
                        //{
                        //    return true;
                        //}
                        //else
                        //{
                        //    returnResult = false;
                        //    //return false;
                        //}                        
                    }
                    catch (Exception ex)
                    {
                        key1.SetValue("IsSyncing", false);
                        throw;
                    }
                }
                return returnResult;
            }
            catch (Exception e1)
            {
                RegistryKey key3 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                key3.SetValue("IsSyncing", false);
                return returnResult;
            }
        }

        public static void CheckConnection(OdbcConnection CON)
        {
            if (CON.State != ConnectionState.Open)
                CON.Open();
        }

        #region Entry Userid 

        public static string GetDentrixUserLogin_ID()
        {
            string UserId = string.Empty;
            Int64 StaffId = 0;
            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            CommonDB.OdbcConnectionServer(ref conn);

            try
            {

                if (conn.State == ConnectionState.Closed) conn.Open();
                string strQauery = SynchDentrixQRY.GetUSerId;
                OdbcCommand.CommandText = strQauery;
                OdbcCommand.CommandType = CommandType.Text;
                OdbcCommand.Connection = conn;
                UserId = Convert.ToString(OdbcCommand.ExecuteScalar());

                if (UserId == "")
                {
                    string QryIdentity = "Select max(idnum) from admin.rsc";
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.CommandType = CommandType.Text;
                    OdbcCommand.Connection = conn;
                    StaffId = Convert.ToInt64(OdbcCommand.ExecuteScalar()) + 1;

                    OdbcCommand = new OdbcCommand();
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                    OdbcCommand.CommandText = SynchDentrixQRY.InsertUSerIdToStaff;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("idnum", StaffId);
                    UserId = Convert.ToString(OdbcCommand.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            //Utility.WriteToErrorLogFromAll("UserId is  " + UserId);
            return UserId;
        }
        #endregion
        public static string Save_PatientPaymentLog_LocalToDentrix(DataTable dtWebPatientPayment)
        {
            try
            {
                string NoteId = "";
                OdbcConnection conn = null;
                OdbcCommand OdbcCommand = new OdbcCommand();
                CommonDB.OdbcConnectionServer(ref conn);
                foreach (DataRow drRow in dtWebPatientPayment.Rows)
                {
                    Int32 Contactid = 0;
                    try
                    {
                        OdbcCommand = new OdbcCommand();
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                        OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_PaymentLog;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                        OdbcCommand.Parameters.AddWithValue("Contacttime", Convert.ToInt32(Convert.ToDateTime(drRow["PaymentDate"].ToString()).TimeOfDay.TotalMilliseconds.ToString().Substring(0, 5)));
                        OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                        OdbcCommand.Parameters.AddWithValue("Provid", GetDentrixIdelProvider().Rows[0]["provider_id"].ToString());
                        DateTime? PDate = Convert.ToDateTime(drRow["PaymentDate"]);
                        OdbcCommand.Parameters.AddWithValue("title", drRow["PaymentMode"].ToString() + " on " + PDate.Value.ToString("dd/MM/yyyy hh:mm tt")); //DateTime.ParseExact(Convert.ToDateTime(drRow["PaymentDate"].ToString()), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture));
                        OdbcCommand.Parameters.AddWithValue("contacttype", 5);
                        OdbcCommand.ExecuteNonQuery();

                        string QryIdentity = "Select max(Contactid) as newId from admin.Contact";
                        OdbcCommand.CommandText = QryIdentity;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Connection = conn;
                        Contactid = Convert.ToInt32(OdbcCommand.ExecuteScalar());

                        OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_PaymentLogNote;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("notetype", 49);
                        OdbcCommand.Parameters.AddWithValue("noteid", Contactid);
                        OdbcCommand.Parameters.AddWithValue("notetext", drRow["template"].ToString());
                        OdbcCommand.ExecuteNonQuery();

                        SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", Contactid.ToString(), "", "", "", Convert.ToInt32(drRow["TryInsert"]));

                    }
                    catch (Exception ex)
                    {
                        NoteId = "";
                        SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", ex.Message.ToString(), "", "", "", ex.Message.ToString(), Convert.ToInt32(drRow["TryInsert"]));
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }

                }
                return NoteId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Save_PatientSMSCallLog_LocalToDentrix(DataTable dtWebPatientPayment)
        {
            try
            {
                string NoteId = "";
                OdbcConnection conn = null;
                OdbcCommand OdbcCommand = new OdbcCommand();
                CommonDB.OdbcConnectionServer(ref conn);
                if (!dtWebPatientPayment.Columns.Contains("Log_Status"))
                {
                    dtWebPatientPayment.Columns.Add("Log_Status", typeof(string));
                }

                DataTable dtResultCopy = new DataTable();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    dtResultCopy = dtWebPatientPayment.Select("Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' and Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();

                    if (dtResultCopy != null)
                    {
                        if (dtResultCopy.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                            {
                                Utility.EHR_UserLogin_ID = GetDentrixUserLogin_ID();
                            }
                        }
                    }

                    foreach (DataRow drRow in dtResultCopy.Rows)
                    {
                        if (Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() == drRow["Clinic_Number"].ToString())
                        {
                            if (drRow["PatientEHRId"] != null && drRow["PatientEHRId"].ToString() != string.Empty && drRow["PatientEHRId"].ToString() != "")
                            {
                                try
                                {
                                    NoteId = CheckSMSCallLogRecordsExists(drRow);
                                    //providerid = GetDentrixPrimaryProvider(Convert.ToInt32(drRow["PatientEHRId"]));
                                    if (NoteId == "0" && ((Convert.ToInt16(drRow["LogType"]) == 0 && drRow["Mobile"].ToString() != "") || Convert.ToInt16(drRow["LogType"]) == 1))
                                    {
                                        Int32 Contactid = 0;

                                        OdbcCommand = new OdbcCommand();
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                                        OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_PaymentLog;
                                        OdbcCommand.Parameters.Clear();
                                        OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString());
                                        OdbcCommand.Parameters.AddWithValue("Contacttime", CheckSMSCallLogSameTimeEntry(drRow));
                                        OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                                        OdbcCommand.Parameters.AddWithValue("Provid", Utility.EHR_UserLogin_ID);
                                        OdbcCommand.Parameters.AddWithValue("title", drRow["app_alias"].ToString());
                                        if (drRow["message_type"].ToString().ToLower() == "call" || drRow["message_type"].ToString().ToLower() == "appointment_reminder_call")
                                        {
                                            OdbcCommand.Parameters.AddWithValue("contacttype", 3);
                                        }
                                        else
                                        {
                                            OdbcCommand.Parameters.AddWithValue("contacttype", 5);
                                        }

                                        OdbcCommand.ExecuteNonQuery();

                                        string QryIdentity = "Select max(Contactid) as newId from admin.Contact";
                                        OdbcCommand.CommandText = QryIdentity;
                                        OdbcCommand.CommandType = CommandType.Text;
                                        OdbcCommand.Connection = conn;
                                        Contactid = Convert.ToInt32(OdbcCommand.ExecuteScalar());

                                        OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_PaymentLogNote;
                                        OdbcCommand.Parameters.Clear();
                                        OdbcCommand.Parameters.AddWithValue("notetype", 49);
                                        OdbcCommand.Parameters.AddWithValue("noteid", Contactid);
                                        OdbcCommand.Parameters.AddWithValue("notetext", drRow["text"].ToString());
                                        OdbcCommand.ExecuteNonQuery();
                                        drRow["LogEHRId"] = Contactid.ToString();
                                        drRow["Log_Status"] = "completed";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Length >= 100)
                                    {
                                        drRow["Log_Status"] = "Err_" + ex.Message.Substring(0, 100);
                                    }
                                    else
                                    {
                                        drRow["Log_Status"] = "Err_" + ex.Message.ToString();
                                    }
                                    NoteId = "";
                                    //throw ex;
                                }
                                finally
                                {
                                    if (conn.State == ConnectionState.Open) conn.Close();
                                }

                            }
                        }
                    }
                    if (dtResultCopy.Rows.Count > 0)
                    {
                        if (dtResultCopy.Select("LogType = 0").Count() > 0)
                        {
                            SynchLocalDAL.CallPatientSMSCallAPIForStatusCompleted(dtResultCopy.Copy().Select("LogType = 0").CopyToDataTable(), "completed", "1", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                        }
                        if (dtResultCopy.Select("LogType = 1").Count() > 0)
                        {
                            SynchLocalDAL.CallPatientFollowUpStatusCompleted(dtResultCopy.Copy().Select("LogType = 1").CopyToDataTable(), "completed", "1", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                        }
                        // SynchLocalDAL.CallPatientSMSCallAPIForStatusCompleted(dtResultCopy, "completed", "1", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                    }
                }
                return NoteId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeleteDuplicatePatientLog()
        {
            //  bool is_Record_Update = false;
            string NoteId = "";
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            CommonDB.OdbcConnectionServer(ref conn);
            DateTime datetimeTemp = DateTime.Now;
            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                string sqlSelect = string.Empty;

                DataTable dtDuplicateRecords = GetDuplicateRecords();
                foreach (DataRow drRow in dtDuplicateRecords.Rows)
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                        OdbcCommand.CommandText = SynchDentrixQRY.DeleteDuplicateFullNoteLogs;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("noteid", drRow["LogId"].ToString());
                        OdbcCommand.ExecuteNonQuery();

                        OdbcCommand.CommandText = SynchDentrixQRY.DeleteDuplicateContactLogs;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("Contactid", drRow["LogId"].ToString());
                        OdbcCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex1)
                    {
                        Utility.WriteToSyncLogFile_All("Error While Delete " + ex1.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                // return "";
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static string CheckSMSCallLogRecordsExists(DataRow drRow)
        {
            try
            {
                #region Check For the Records exists
                OdbcConnection conn = null;
                OdbcCommand OdbcCommand = new OdbcCommand();
                CommonDB.OdbcConnectionServer(ref conn);
                DateTime datetimeTemp = DateTime.Now;
                CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                string noteId = "0";
                Int64 recordsCount = 0;
                if (conn.State == ConnectionState.Closed) conn.Open();
                OdbcCommand.CommandText = SynchDentrixQRY.CheckSMSCallRecordsBlankMobile;
                // Utility.WriteToSyncLogFile_All("Mobile is blank for patient " + drRow["PatientEHRId"].ToString());


                //Utility.WriteToSyncLogFile_All("Check Duplicate Records " + MySqlCommand.CommandText);

                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString());
                OdbcCommand.Parameters.AddWithValue("Contacttime", Convert.ToInt32(Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString()).TimeOfDay.TotalMilliseconds.ToString().Substring(0, 5)));
                OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                OdbcCommand.Parameters.AddWithValue("Provid", Utility.EHR_UserLogin_ID);//GetDentrixIdelProvider().Rows[0]["provider_id"].ToString());
                if (drRow["message_type"].ToString().ToLower() == "call" || drRow["message_type"].ToString().ToLower() == "appointment_reminder_call")
                {
                    OdbcCommand.Parameters.AddWithValue("contacttype", 3);
                }
                else
                {
                    OdbcCommand.Parameters.AddWithValue("contacttype", 5);
                }
                OdbcCommand.Parameters.AddWithValue("notetext", drRow["text"].ToString());
                OdbcDataAdapter OdbcDa = new OdbcDataAdapter();
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                //Utility.WriteToSyncLogFile_All("Check Duplicate Records " + MySqlCommand.CommandText + " PatientId : " + drRow["PatientEHRId"].ToString() + " LogDate : " + drRow["PatientSMSCallLogDate"].ToString() + " Note : " + drRow["text"].ToString());
                try
                {
                    OdbcDa.Fill(OdbcDt);
                }
                catch (Exception ex1)
                {
                    Utility.WriteToSyncLogFile_All("Error in executing and get group by records " + ex1.ToString());
                }
                if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                {
                    // Utility.WriteToSyncLogFile_All("Found Records " + MySqlCommand.CommandText + " PatientId : " + drRow["PatientEHRId"].ToString() + " LogDate : " + drRow["PatientSMSCallLogDate"].ToString() + " Note : " + drRow["text"].ToString() + " NoteId : " + MySqlDt.Rows[0][0].ToString() + " Count : " + MySqlDt.Rows[0][1].ToString());
                    noteId = OdbcDt.Rows[0][0].ToString();
                    recordsCount = Convert.ToInt64(OdbcDt.Rows[0][1]);
                }

                #endregion

                #region Check and Delete duplicate Records
                if (noteId != "0" && recordsCount > 1)
                {
                    CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");

                    if (conn.State == ConnectionState.Closed) conn.Open();
                    CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                    OdbcCommand.CommandText = SynchDentrixQRY.DeleteDuplicateFullNoteLogs;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("noteid", noteId);
                    OdbcCommand.ExecuteNonQuery();

                    OdbcCommand.CommandText = SynchDentrixQRY.DeleteDuplicateContactLogs;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("Contactid", noteId);
                    OdbcCommand.ExecuteNonQuery();
                }
                #endregion

                return noteId;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Error in executing Function " + ex.Message);
                return "-1";
            }
        }

        public static Int32 CheckSMSCallLogSameTimeEntry(DataRow drRow)
        {
            Int32 time = 0;
            try
            {
                time = Convert.ToInt32(Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString()).TimeOfDay.TotalMilliseconds.ToString().Substring(0, 5));
                RecalculateTime:
                #region Check For the Records exists
                OdbcConnection conn = null;
                OdbcCommand OdbcCommand = new OdbcCommand();
                CommonDB.OdbcConnectionServer(ref conn);
                DateTime datetimeTemp = DateTime.Now;
                CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                string noteId = "0";
                Int64 recordsCount = 0;
                if (conn.State == ConnectionState.Closed) conn.Open();
                OdbcCommand.CommandText = SynchDentrixQRY.CheckSMSCallDuplicateDateTime;
                // Utility.WriteToSyncLogFile_All("Mobile is blank for patient " + drRow["PatientEHRId"].ToString());


                //Utility.WriteToSyncLogFile_All("Check Duplicate Records " + MySqlCommand.CommandText);

                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString());
                OdbcCommand.Parameters.AddWithValue("Contacttime", time);
                OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                OdbcCommand.Parameters.AddWithValue("Provid", Utility.EHR_UserLogin_ID);//GetDentrixIdelProvider().Rows[0]["provider_id"].ToString());
                if (drRow["message_type"].ToString().ToLower() == "call" || drRow["message_type"].ToString().ToLower() == "appointment_reminder_call")
                {
                    OdbcCommand.Parameters.AddWithValue("contacttype", 3);
                }
                else
                {
                    OdbcCommand.Parameters.AddWithValue("contacttype", 5);
                }
                //OdbcCommand.Parameters.AddWithValue("notetext", drRow["text"].ToString());
                OdbcDataAdapter OdbcDa = new OdbcDataAdapter();
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                //Utility.WriteToSyncLogFile_All("Check Duplicate Records " + MySqlCommand.CommandText + " PatientId : " + drRow["PatientEHRId"].ToString() + " LogDate : " + drRow["PatientSMSCallLogDate"].ToString() + " Note : " + drRow["text"].ToString());
                try
                {
                    OdbcDa.Fill(OdbcDt);
                }
                catch (Exception ex1)
                {
                    Utility.WriteToSyncLogFile_All("Error in executing and get group by records " + ex1.ToString());
                }
                if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                {
                    // Utility.WriteToSyncLogFile_All("Found Records " + MySqlCommand.CommandText + " PatientId : " + drRow["PatientEHRId"].ToString() + " LogDate : " + drRow["PatientSMSCallLogDate"].ToString() + " Note : " + drRow["text"].ToString() + " NoteId : " + MySqlDt.Rows[0][0].ToString() + " Count : " + MySqlDt.Rows[0][1].ToString());
                    noteId = OdbcDt.Rows[0][0].ToString();
                    recordsCount = Convert.ToInt64(OdbcDt.Rows[0][1]);
                    time = time + 1;
                    goto RecalculateTime;
                }

                #endregion
                return time;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Error in executing Function " + ex.Message);
                return 0;
            }
        }

        public static Int16 GetPaymentType(string PaymentMode, Int16 deftype, OdbcConnection conn)
        {
            try
            {
                OdbcCommand OdbcCommand = null;
                Int16 defid = 0;
                string QryIdentity = "Select isnull(max(defid),-1) as newId from admin.fulldef where ltrim(rtrim(descript)) = ? and deftype = ?";
                CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                OdbcCommand.CommandText = QryIdentity;
                OdbcCommand.CommandType = CommandType.Text;
                OdbcCommand.Parameters.AddWithValue("descript", PaymentMode.Trim());
                OdbcCommand.Parameters.AddWithValue("deftype", deftype);
                CheckConnection(conn);
                defid = Convert.ToInt16(OdbcCommand.ExecuteScalar());
                if (defid == -1)
                {
                    QryIdentity = "Select max(defid)+1 as newId from admin.fulldef where deftype = ?";
                    CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.CommandType = CommandType.Text;
                    OdbcCommand.Parameters.Clear();
                    // OdbcCommand.Parameters.AddWithValue("descript", PaymentMode);
                    OdbcCommand.Parameters.AddWithValue("deftype", deftype);
                    defid = Convert.ToInt16(OdbcCommand.ExecuteScalar());

                    CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                    OdbcCommand.CommandText = "insert into admin.fulldef (defid,descript,deftype) values (?,?,?)";
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("defid", defid);
                    OdbcCommand.Parameters.AddWithValue("descript", PaymentMode);
                    OdbcCommand.Parameters.AddWithValue("deftype", deftype);
                    CheckConnection(conn);
                    OdbcCommand.ExecuteNonQuery();

                    return defid;
                }
                else
                {
                    return Convert.ToInt16(defid);
                }
            }
            catch
            {
                return 0;
            }
        }

        public static Int64 GetDuplicatePayment(string patid, string guarid, DateTime Procdate, string proclogorder, short proclogclass, string provid, decimal amt, OdbcConnection conn)
        {
            try
            {
                OdbcCommand OdbcCommand = null;
                Int64 defid = 0;
                string QryIdentity = "Select isnull(max(procid),-1) as newId from admin.fullproclog where patid = ? and guarid = ? and Procdate = ? and proclogclass = ? and proclogorder = ? and provid = ? and amt = ?";
                CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                OdbcCommand.CommandText = QryIdentity;
                OdbcCommand.CommandType = CommandType.Text;
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("patid", patid.Trim());
                OdbcCommand.Parameters.AddWithValue("guarid", guarid.Trim());
                OdbcCommand.Parameters.Add("Procdate", OdbcType.Date).Value = Procdate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.AddWithValue("proclogclass", proclogclass);
                OdbcCommand.Parameters.AddWithValue("proclogorder", Convert.ToInt16(proclogorder));
                OdbcCommand.Parameters.AddWithValue("provid", provid.Trim());
                OdbcCommand.Parameters.AddWithValue("amt", amt);
                CheckConnection(conn);
                defid = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                if (defid == -1)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt64(defid);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static Int64 SavePatientPaymentTOEHR(DataTable dtTable, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            Int64 TransactionHeaderId = 0;
            Int64 DiscountId = 0;
            try
            {
                CommonDB.OdbcConnectionServer(ref conn);
                Int16 PaymentRefundTypeID = 0;
                Int16 DiscountTypeID = 0;
                Int16 CareCreditPaymentRefundTypeID = 0;
                Int16 CareCreditDiscountTypeID = 0;
                Int16 TypeID = 0;
                Int32 Contactid = 0;
                decimal discount = 0;
                // string providerid = string.Empty;

                #region Check the Adit Pay payment Categories

                Int16 PaymentTypeID = GetPaymentType(" Adit Pay", 6, conn);

                var result = dtTable.AsEnumerable().Where(o => o.Field<object>("PaymentMode").ToString().ToUpper() == "REFUNDED" || o.Field<object>("PaymentMode").ToString().ToUpper() == "PARTIAL-REFUNDED").FirstOrDefault();
                if (result != null)
                {
                    PaymentRefundTypeID = GetPaymentType("+Adit Pay Refund", 9, conn);
                }
                result = dtTable.AsEnumerable().Where(o => o.Field<decimal>("Discount") > 0).FirstOrDefault();
                if (result != null)
                {
                    DiscountTypeID = GetPaymentType("-Adit Pay Discount", 9, conn);
                }
                #endregion

                #region Check the CareCredit payment Categories

                Int16 CareCreditModeId = GetPaymentType(" CareCredit", 6, conn);

                var resultt = dtTable.AsEnumerable().Where(o => o.Field<object>("PaymentMode").ToString().ToUpper() == "REFUNDED" || o.Field<object>("PaymentMode").ToString().ToUpper() == "PARTIAL-REFUNDED").FirstOrDefault();
                if (resultt != null)
                {
                    CareCreditPaymentRefundTypeID = GetPaymentType("+CareCredit Refund", 9, conn);
                }
                result = dtTable.AsEnumerable().Where(o => o.Field<decimal>("Discount") > 0).FirstOrDefault();
                if (result != null)
                {
                    CareCreditDiscountTypeID = GetPaymentType("-CareCredit Discount", 9, conn);
                }
                #endregion

                if (dtTable != null)
                {
                    if (dtTable.Rows.Count > 0)
                    {
                        Utility.CheckEntryUserLoginIdExist();
                        if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                        {
                            Utility.EHR_UserLogin_ID = GetDentrixUserLogin_ID();
                        }
                    }
                }

                foreach (DataRow drRow in dtTable.Rows)
                {
                    Contactid = 0;
                    discount = 0;
                    DiscountId = 0;
                    TransactionHeaderId = 0;
                    TypeID = 0;
                    try
                    {
                        //if (Convert.ToBoolean(drRow["is_distributed_payment"].ToString()) == false)
                        //    providerid = GetDentrixPrimaryProvider(Convert.ToString(drRow["PatientEHRId"])); 
                        //else
                        //    providerid = drRow["provider_id"].ToString();

                        string providerid = GetDentrixPrimaryProvider(Convert.ToString(drRow["PatientEHRId"]));

                        if (drRow["PaymentMethod"].ToString().ToLower() == "carecredit")
                        {
                            SaveCareCreditPaymentToEHR(TypeID, CareCreditModeId,CareCreditPaymentRefundTypeID, drRow, DiscountId, TransactionHeaderId, discount, CareCreditDiscountTypeID, providerid, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, Contactid);
                        }
                        else
                        {
                            SaveAditPayPaymentToEHR(TypeID, PaymentTypeID,PaymentRefundTypeID, drRow, DiscountId, TransactionHeaderId, discount, DiscountTypeID, providerid, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, Contactid);
                        }

                    }
                    catch (Exception ex1)
                    {
                        //NoteId = "";
                        Utility.WriteToErrorLogFromAll("In Main catch" + ex1.Message.ToString());
                        bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        Utility.WriteToSyncLogFile_All("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available." + ex1.Message.ToString());
                    }
                }
                if (conn.State != ConnectionState.Closed) conn.Close();
                return TransactionHeaderId;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public static void SaveCareCreditPaymentToEHR(Int16 TypeID, Int16 CareCreditModeId,Int16 CareCreditPaymentRefundTypeID, DataRow drRow,Int64 DiscountId,Int64 TransactionHeaderId,decimal discount,Int16 CareCreditDiscountTypeID, string providerid, string _filename_EHR_Payment, string _EHRLogdirectory_EHR_Payment,Int32 Contactid)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                #region Create Payment Entry in EHR if Payment Mode is Paid or Partial Paid and Log seeting is Ledger or Both
                if (Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 2 || Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 3)
                {
                    short procclass = 0;
                    decimal amount = 0;
                    bool is_payment = false;
                    if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {
                        is_payment = true;
                        procclass = 1;
                        amount = -Convert.ToDecimal(drRow["Amount"]) + Convert.ToDecimal(drRow["Discount"]);
                        TypeID = CareCreditModeId;
                        discount = Convert.ToDecimal(drRow["Discount"]);
                    }
                    else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        is_payment = true;
                        procclass = 2;
                        amount = Convert.ToDecimal(drRow["Amount"]);
                        TypeID = CareCreditPaymentRefundTypeID;
                    }
                    if (is_payment && TypeID > 0)
                    {
                        // string provid = GetProviderForPatient(drRow);
                        #region check payment exist already
                        TransactionHeaderId = GetDuplicatePayment(Convert.ToInt64(drRow["PatientEHRId"].ToString()) == Convert.ToInt64(drRow["Guar_ID"].ToString()) ? drRow["PatientEHRId"].ToString() : drRow["Guar_ID"].ToString(), drRow["Guar_ID"].ToString(), Convert.ToDateTime(drRow["PaymentDate"].ToString()), TypeID.ToString(), procclass, Convert.ToString(providerid), amount, conn);
                        #endregion

                        if (TransactionHeaderId == 0)
                        {
                            TransactionHeaderId = SavePayment(drRow, procclass, TypeID, providerid, amount, discount, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                            DiscountId = SavePaymentDiscount(drRow, providerid, discount, CareCreditDiscountTypeID, TransactionHeaderId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        }
                        else if (TransactionHeaderId > 0)
                        {
                            //Code added because dentrix ehr saves only paymentdate not a time so we checked duplicates with local also bcoz there is issue with payment on same day same amount entry is not done in ehr
                            #region check same amount payments exist ? if not then allow entry to ehr 
                            string note = string.Empty;
                            note = GetPaymentNote(TransactionHeaderId, conn);
                            if (!note.Contains(drRow["PatientPaymentWebId"].ToString()) && note != string.Empty)
                            {
                                TransactionHeaderId = SavePayment(drRow, procclass, TypeID, providerid, amount, discount, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                                DiscountId = SavePaymentDiscount(drRow, providerid, discount, CareCreditDiscountTypeID, TransactionHeaderId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                            }
                            #endregion

                        }
                    }

                }
                #endregion

                #region Create Payment Log entry bcoz Financial setting is only create log or Both
                if (Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 1 || Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 3)
                {
                    try
                    {

                        Int64 logid = 0;
                        string QryIdentity1 = "select isnull(max(contactid),0) as newId  from admin.Contact where Contactdate=? and Contacttime=? and patid=? and Provid=? and  Contacttype=?";
                        CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                        OdbcCommand.CommandText = QryIdentity1;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                        OdbcCommand.Parameters.AddWithValue("Contacttime", Convert.ToInt32(Convert.ToDateTime(drRow["PaymentDate"].ToString()).TimeOfDay.TotalMilliseconds.ToString().Substring(0, 5)));
                        OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                        OdbcCommand.Parameters.AddWithValue("Provid", Utility.EHR_UserLogin_ID);
                        OdbcCommand.Parameters.AddWithValue("contacttype", 5);
                        CheckConnection(conn);
                        logid = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get logid=" + logid.ToString());
                        if (logid == 0)
                        {
                            OdbcCommand = new OdbcCommand();
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                            OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_PaymentLog;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                            OdbcCommand.Parameters.AddWithValue("Contacttime", Convert.ToInt32(Convert.ToDateTime(drRow["PaymentDate"].ToString()).TimeOfDay.TotalMilliseconds.ToString().Substring(0, 5)));
                            OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                            OdbcCommand.Parameters.AddWithValue("Provid", Utility.EHR_UserLogin_ID);//GetDentrixIdelProvider().Rows[0]["provider_id"].ToString());
                            DateTime? PDate = Convert.ToDateTime(drRow["PaymentDate"]);
                            OdbcCommand.Parameters.AddWithValue("title", drRow["PaymentMode"].ToString() + " on " + PDate.Value.ToString("dd/MM/yyyy hh:mm tt")); //DateTime.ParseExact(Convert.ToDateTime(drRow["PaymentDate"].ToString()), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture));
                            OdbcCommand.Parameters.AddWithValue("contacttype", 5);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save patient PaymentLog for patid=" + drRow["PatientEHRId"].ToString() + " and  Provid=" + Utility.EHR_UserLogin_ID);
                        }

                        string QryIdentity = "Select max(Contactid) as newId from admin.Contact";
                        OdbcCommand.CommandText = QryIdentity;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Connection = conn;
                        Contactid = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                        Int64 noteid = 0;
                        string QryIdentity2 = "select isnull(max(noteid),0) as newId  from admin.FullNotes where notetype=? and noteid=?";
                        CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                        OdbcCommand.CommandText = QryIdentity2;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("notetype", 49);
                        OdbcCommand.Parameters.AddWithValue("noteid", Contactid);
                        CheckConnection(conn);
                        noteid = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get FullNotes noteid=" + noteid.ToString());
                        if (noteid == 0)
                        {
                            OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_PaymentLogNote;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("notetype", 49);
                            OdbcCommand.Parameters.AddWithValue("noteid", Contactid);
                            OdbcCommand.Parameters.AddWithValue("notetext", drRow["template"].ToString());
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save paitent Payment LogNote with noteid=" + Contactid.ToString());
                        }

                        //SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", Contactid.ToString());

                    }
                    catch (Exception ex)
                    {
                        // SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", "400", "");
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
                #endregion
                bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", TransactionHeaderId.ToString(), Contactid.ToString(), DiscountId.ToString(), "EHR Entry Success", Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);

            }
            catch (Exception ex1)
            {
                Utility.WriteToErrorLogFromAll("Error During patient payment "+ex1.Message);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", ex1.Message.ToString(), "", "", "", "EHR Entry Error " + ex1.Message.ToString(), Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }
        }

        public static void SaveAditPayPaymentToEHR(Int16 TypeID, Int16 PaymentTypeID,Int16 PaymentRefundTypeID, DataRow drRow,Int64 DiscountId, Int64 TransactionHeaderId, decimal discount, Int16 DiscountTypeID, string providerid, string _filename_EHR_Payment, string _EHRLogdirectory_EHR_Payment,Int32 Contactid)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                #region Create Payment Entry in EHR if Payment Mode is Paid or Partial Paid and Log seeting is Ledger or Both
                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 2 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                {
                    short procclass = 0;
                    decimal amount = 0;
                    bool is_payment = false;
                    if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {
                        is_payment = true;
                        procclass = 1;
                        amount = -Convert.ToDecimal(drRow["Amount"]) + Convert.ToDecimal(drRow["Discount"]);
                        TypeID = PaymentTypeID;
                        discount = Convert.ToDecimal(drRow["Discount"]);
                    }
                    else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        is_payment = true;
                        procclass = 2;
                        amount = Convert.ToDecimal(drRow["Amount"]);
                        TypeID = PaymentRefundTypeID;
                    }
                    if (is_payment && TypeID > 0)
                    {
                        // string provid = GetProviderForPatient(drRow);
                        TransactionHeaderId = GetDuplicatePayment(Convert.ToInt64(drRow["PatientEHRId"].ToString()) == Convert.ToInt64(drRow["Guar_ID"].ToString()) ? drRow["PatientEHRId"].ToString() : drRow["Guar_ID"].ToString(), drRow["Guar_ID"].ToString(), Convert.ToDateTime(drRow["PaymentDate"].ToString()), TypeID.ToString(), procclass, Convert.ToString(providerid), amount, conn);

                        if (TransactionHeaderId == 0)
                        {
                            TransactionHeaderId = SavePayment(drRow, procclass, TypeID, providerid, amount, discount, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                            DiscountId = SavePaymentDiscount(drRow, providerid, discount, DiscountTypeID, TransactionHeaderId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        }
                        else if (TransactionHeaderId > 0)
                        {
                            //Code added because dentrix ehr saves only paymentdate not a time so we checked duplicates with local also bcoz there is issue with payment on same day same amount entry is not done in ehr
                            #region check same amount payments exist ? if not then allow entry to ehr 
                            string note = string.Empty;
                            note = GetPaymentNote(TransactionHeaderId, conn);
                            if (!note.Contains(drRow["PatientPaymentWebId"].ToString()) && note != string.Empty)
                            {
                                TransactionHeaderId = SavePayment(drRow, procclass, TypeID, providerid, amount, discount, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                                DiscountId = SavePaymentDiscount(drRow, providerid, discount, DiscountTypeID, TransactionHeaderId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                            }
                            #endregion

                        }
                    }

                }
                #endregion

                #region Create Payment Log entry bcoz Financial setting is only create log or Both
                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 1 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                {
                    try
                    {

                        Int64 logid = 0;
                        string QryIdentity1 = "select isnull(max(contactid),0) as newId  from admin.Contact where Contactdate=? and Contacttime=? and patid=? and Provid=? and  Contacttype=?";
                        CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                        OdbcCommand.CommandText = QryIdentity1;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                        OdbcCommand.Parameters.AddWithValue("Contacttime", Convert.ToInt32(Convert.ToDateTime(drRow["PaymentDate"].ToString()).TimeOfDay.TotalMilliseconds.ToString().Substring(0, 5)));
                        OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                        OdbcCommand.Parameters.AddWithValue("Provid", Utility.EHR_UserLogin_ID);
                        OdbcCommand.Parameters.AddWithValue("contacttype", 5);
                        CheckConnection(conn);
                        logid = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get logid=" + logid.ToString());
                        if (logid == 0)
                        {
                            OdbcCommand = new OdbcCommand();
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                            OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_PaymentLog;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                            OdbcCommand.Parameters.AddWithValue("Contacttime", Convert.ToInt32(Convert.ToDateTime(drRow["PaymentDate"].ToString()).TimeOfDay.TotalMilliseconds.ToString().Substring(0, 5)));
                            OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                            OdbcCommand.Parameters.AddWithValue("Provid", Utility.EHR_UserLogin_ID);//GetDentrixIdelProvider().Rows[0]["provider_id"].ToString());
                            DateTime? PDate = Convert.ToDateTime(drRow["PaymentDate"]);
                            OdbcCommand.Parameters.AddWithValue("title", drRow["PaymentMode"].ToString() + " on " + PDate.Value.ToString("dd/MM/yyyy hh:mm tt")); //DateTime.ParseExact(Convert.ToDateTime(drRow["PaymentDate"].ToString()), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture));
                            OdbcCommand.Parameters.AddWithValue("contacttype", 5);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save patient PaymentLog for patid=" + drRow["PatientEHRId"].ToString() + " and  Provid=" + Utility.EHR_UserLogin_ID);
                        }

                        string QryIdentity = "Select max(Contactid) as newId from admin.Contact";
                        OdbcCommand.CommandText = QryIdentity;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Connection = conn;
                        Contactid = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                        Int64 noteid = 0;
                        string QryIdentity2 = "select isnull(max(noteid),0) as newId  from admin.FullNotes where notetype=? and noteid=?";
                        CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                        OdbcCommand.CommandText = QryIdentity2;
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("notetype", 49);
                        OdbcCommand.Parameters.AddWithValue("noteid", Contactid);
                        CheckConnection(conn);
                        noteid = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get FullNotes noteid=" + noteid.ToString());
                        if (noteid == 0)
                        {
                            OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_PaymentLogNote;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("notetype", 49);
                            OdbcCommand.Parameters.AddWithValue("noteid", Contactid);
                            OdbcCommand.Parameters.AddWithValue("notetext", drRow["template"].ToString());
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save paitent Payment LogNote with noteid=" + Contactid.ToString());
                        }

                        //SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", Contactid.ToString());

                    }
                    catch (Exception ex)
                    {
                        // SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", "400", "");
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
                #endregion

                bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", TransactionHeaderId.ToString(), Contactid.ToString(), DiscountId.ToString(), "EHR Entry Success", Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }
            catch (Exception ex1)
            {
                Utility.WriteToErrorLogFromAll("Error During patient payment " + ex1.Message);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", ex1.Message.ToString(), "", "", "", "EHR Entry Error " + ex1.Message.ToString(), Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }
        }

        public static string GetPaymentNote(Int64 TransactionHeaderId, OdbcConnection conn)
        {
            try
            {
                OdbcCommand OdbcCommand = null;
                string note = string.Empty;
                string QryIdentity = "select notetext from admin.FullNotes where notetype=1 and noteid = @noteid";
                CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                QryIdentity = QryIdentity.Replace("@noteid", TransactionHeaderId.ToString());
                OdbcCommand.CommandText = QryIdentity;
                OdbcCommand.CommandType = CommandType.Text;
                CheckConnection(conn);
                note = Convert.ToString(OdbcCommand.ExecuteScalar());
                return note;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error in GetPaymentNote " + ex.Message.ToString());
                return " ";
            }
        }
        public static Int64 SavePayment(DataRow drRow, short procclass, Int16 TypeID, string providerid, decimal amount, decimal discount, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            Int64 TransactionHeaderId = 0;
            try
            {

                CommonDB.OdbcConnectionServer(ref conn);
                string OdbcSelect = "";
                if ((Convert.ToDecimal(drRow["Amount"]) - discount) > 0)
                {

                    OdbcSelect = SynchDentrixQRY.InsertPaymentAmount;
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.Parameters.Clear();
                    //OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                    OdbcCommand.Parameters.AddWithValue("patid", Convert.ToInt64(drRow["PatientEHRId"].ToString()) == Convert.ToInt64(drRow["Guar_ID"].ToString()) ? Convert.ToInt64(drRow["PatientEHRId"].ToString()) : Convert.ToInt64(drRow["Guar_ID"].ToString()));
                    OdbcCommand.Parameters.AddWithValue("guarid", Convert.ToInt64(drRow["PatientEHRId"].ToString()) == Convert.ToInt64(drRow["Guar_ID"].ToString()) ? Convert.ToInt64(drRow["PatientEHRId"].ToString()) : Convert.ToInt64(drRow["Guar_ID"].ToString()));
                    OdbcCommand.Parameters.AddWithValue("Procdate", Convert.ToDateTime(drRow["PaymentDate"].ToString()));
                    OdbcCommand.Parameters.AddWithValue("proclogclass", procclass);
                    OdbcCommand.Parameters.AddWithValue("proclogorder", TypeID);
                    OdbcCommand.Parameters.AddWithValue("provid", providerid);
                    OdbcCommand.Parameters.AddWithValue("amt", amount);
                    OdbcCommand.Parameters.AddWithValue("checknum", drRow["ChequeNumber"].ToString());
                    OdbcCommand.Parameters.AddWithValue("proccodeid", Convert.ToInt64(drRow["PatientEHRId"].ToString()) == Convert.ToInt64(drRow["Guar_ID"].ToString()) ? 0 : Convert.ToInt64(drRow["PatientEHRId"].ToString()));
                    CheckConnection(conn);
                    OdbcCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Payment Amount for patid=" + drRow["PatientEHRId"].ToString() + " and provid Id =" + providerid.ToString() + ", Gaurdian =" + Convert.ToInt64(drRow["PatientEHRId"].ToString()));
                    if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {
                        OdbcSelect = "update admin.aging set aging0to30 = aging0to30 + ? ,lastpaydate = (case when lastpaydate > ? then lastpaydate else ? end),lastpayamt = (case when lastpaydate > ? then lastpayamt else ? end) where guarid = ?";
                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("amt", amount);
                        OdbcCommand.Parameters.AddWithValue("Procdate", Convert.ToDateTime(drRow["PaymentDate"].ToString()));
                        OdbcCommand.Parameters.AddWithValue("Procdate1", Convert.ToDateTime(drRow["PaymentDate"].ToString()));
                        OdbcCommand.Parameters.AddWithValue("Procdate2", Convert.ToDateTime(drRow["PaymentDate"].ToString()));
                        OdbcCommand.Parameters.AddWithValue("amt1", amount);
                        OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                    }
                    else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        InsertAgigPatient(Convert.ToInt64(drRow["Guar_ID"].ToString()), conn);
                        OdbcSelect = "update admin.aging set aging0to30 = aging0to30 + ?  where guarid = ?";
                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("amt", amount);
                        OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                    }
                    CheckConnection(conn);
                    OdbcCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Update aging for guarid=" + drRow["Guar_ID"].ToString());
                    string QryIdentity = "Select max(procid) as newId from admin.fullproclog";
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.CommandType = CommandType.Text;
                    TransactionHeaderId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                    //if (procclass == 1)
                    //{
                    // Update ApptTable
                    CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                    OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_PaymentLogNote;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("notetype", 1);
                    OdbcCommand.Parameters.AddWithValue("noteid", TransactionHeaderId);
                    OdbcCommand.Parameters.AddWithValue("notetext", drRow["template"].ToString());
                    CheckConnection(conn);
                    OdbcCommand.ExecuteNonQuery();
                    if (TransactionHeaderId > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save patient Payment LogNote with notetype= 1,noteid=" + TransactionHeaderId.ToString());
                    }
                    // }
                }
            }
            catch (Exception ex)
            {

                CommonDB.OdbcCommandServer("Delete from admin.FullNotes where notetype = 1 and noteid = " + TransactionHeaderId, conn, ref OdbcCommand, "txt");
                OdbcCommand.CommandType = CommandType.Text;
                CheckConnection(conn);
                OdbcCommand.ExecuteNonQuery();
                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Delete FullNotes for notetype = 1,noteid = " + TransactionHeaderId);
                CommonDB.OdbcCommandServer("Delete from admin.fullproclog where procid = " + TransactionHeaderId, conn, ref OdbcCommand, "txt");
                OdbcCommand.CommandType = CommandType.Text;
                CheckConnection(conn);
                OdbcCommand.ExecuteNonQuery();
                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Delete fullproclog for procid = " + TransactionHeaderId);

            }
            return TransactionHeaderId;
        }
        public static Int64 SavePaymentDiscount(DataRow drRow, string providerid, decimal discount, Int16 DiscountTypeID, Int64 TransactionHeaderId, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            Int64 DiscountId = 0;
            try
            {
                CommonDB.OdbcConnectionServer(ref conn);
                if (discount > 0 && DiscountTypeID > 0)
                {

                    DiscountId = GetDuplicatePayment(Convert.ToInt64(drRow["PatientEHRId"].ToString()) == Convert.ToInt64(drRow["Guar_ID"].ToString()) ? drRow["PatientEHRId"].ToString() : drRow["Guar_ID"].ToString(), drRow["Guar_ID"].ToString(), Convert.ToDateTime(drRow["PaymentDate"].ToString()), DiscountTypeID.ToString(), 2, Convert.ToString(providerid), -discount, conn);
                    if (DiscountId == 0)
                    {
                        try
                        {
                            string OdbcSelect = "";
                            OdbcSelect = SynchDentrixQRY.InsertPaymentAmount;
                            CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                            OdbcCommand.Parameters.Clear();
                            //  OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                            OdbcCommand.Parameters.AddWithValue("patid", Convert.ToInt64(drRow["PatientEHRId"].ToString()) == Convert.ToInt64(drRow["Guar_ID"].ToString()) ? Convert.ToInt64(drRow["PatientEHRId"].ToString()) : Convert.ToInt64(drRow["Guar_ID"].ToString()));
                            OdbcCommand.Parameters.AddWithValue("guarid", Convert.ToInt64(drRow["PatientEHRId"].ToString()) == Convert.ToInt64(drRow["Guar_ID"].ToString()) ? Convert.ToInt64(drRow["PatientEHRId"].ToString()) : Convert.ToInt64(drRow["Guar_ID"].ToString()));
                            OdbcCommand.Parameters.AddWithValue("Procdate", Convert.ToDateTime(drRow["PaymentDate"].ToString()));
                            OdbcCommand.Parameters.AddWithValue("proclogclass", 2);
                            OdbcCommand.Parameters.AddWithValue("proclogorder", DiscountTypeID);
                            OdbcCommand.Parameters.AddWithValue("provid", providerid);
                            OdbcCommand.Parameters.AddWithValue("amt", -discount);
                            OdbcCommand.Parameters.AddWithValue("checknum", drRow["ChequeNumber"].ToString());
                            OdbcCommand.Parameters.AddWithValue("proccodeid", Convert.ToInt64(drRow["PatientEHRId"].ToString()) == Convert.ToInt64(drRow["Guar_ID"].ToString()) ? 0 : Convert.ToInt64(drRow["PatientEHRId"].ToString()));

                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Payment Amount with  Patient Id =" + drRow["PatientEHRId"].ToString() + "and Provider Id " + providerid);
                            InsertAgigPatient(Convert.ToInt64(drRow["Guar_ID"].ToString()), conn);
                            OdbcSelect = "update admin.aging set aging0to30 = aging0to30 + ?  where guarid = ?";
                            CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("amt", -discount);
                            OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Update aging for Gaurdian =" + drRow["Guar_ID"].ToString());
                            string QryIdentity = "Select max(procid) as newId from admin.fullproclog";
                            CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                            OdbcCommand.CommandText = QryIdentity;
                            OdbcCommand.CommandType = CommandType.Text;
                            DiscountId = Convert.ToInt64(OdbcCommand.ExecuteScalar());

                            CommonDB.OdbcCommandServer("", conn, ref OdbcCommand, "txt");
                            OdbcCommand.CommandText = SynchDentrixQRY.Insert_paitent_PaymentLogNote;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("notetype", 1);
                            OdbcCommand.Parameters.AddWithValue("noteid", DiscountId);
                            OdbcCommand.Parameters.AddWithValue("notetext", drRow["template"].ToString());
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save paitent Payment LogNote with noteid=" + DiscountId.ToString() + ",notetype=1");
                        }
                        catch (Exception ex)
                        {
                            CommonDB.OdbcCommandServer("Delete from admin.FullNotes where notetype = 1 and noteid = " + DiscountId, conn, ref OdbcCommand, "txt");
                            OdbcCommand.CommandType = CommandType.Text;
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Delete FullNotes for notetype = 1,noteid = " + DiscountId);
                            CommonDB.OdbcCommandServer("Delete from admin.fullproclog where procid = " + DiscountId, conn, ref OdbcCommand, "txt");
                            OdbcCommand.CommandType = CommandType.Text;
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Delete fullproclog for procid = " + DiscountId);
                            CommonDB.OdbcCommandServer("Delete from admin.FullNotes where notetype = 1 and noteid = " + TransactionHeaderId, conn, ref OdbcCommand, "txt");
                            OdbcCommand.CommandType = CommandType.Text;
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Delete fullproclog for notetype = 1,noteid = " + TransactionHeaderId);
                            CommonDB.OdbcCommandServer("Delete from admin.fullproclog where procid = " + TransactionHeaderId, conn, ref OdbcCommand, "txt");
                            OdbcCommand.CommandType = CommandType.Text;
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Delete fullproclog for procid = " + TransactionHeaderId + ")");
                            CommonDB.OdbcCommandServer("update admin.aging set aging0to30 = aging0to30 - ? where guarid = ?", conn, ref OdbcCommand, "txt");
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("amt", -Convert.ToDecimal(drRow["Amount"]));
                            OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Update aging for guarid =" + drRow["Guar_ID"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error during save payment discount" + ex.Message.ToString());
                // throw;
            }
            return DiscountId;
        }

        public static string GetProviderForPatient(DataRow dr)
        {
            if ((dr["ProviderEHRId"].ToString() == "" || string.IsNullOrEmpty(dr["ProviderEHRId"].ToString())) &&
                (dr["Pri_Provider_ID"].ToString() == "" || string.IsNullOrEmpty(dr["Pri_Provider_ID"].ToString())) &&
                (dr["Sec_Provider_ID"].ToString() == "" || string.IsNullOrEmpty(dr["Sec_Provider_ID"].ToString())))
            {
                return GetDentrixIdelProvider().Rows[0]["provider_id"].ToString();
            }
            else
            {
                return ((dr["ProviderEHRId"].ToString() == "" || string.IsNullOrEmpty(dr["ProviderEHRId"].ToString())) ?
                    ((dr["Pri_Provider_ID"].ToString() == "" || string.IsNullOrEmpty(dr["Pri_Provider_ID"].ToString())) ? dr["Sec_Provider_ID"].ToString() : dr["Pri_Provider_ID"].ToString()) : dr["ProviderEHRId"].ToString());
            }
        }

        public static DataTable GetDuplicateRecords()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcDataAdapter odbcDa = null;

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string strQauery = SynchDentrixQRY.GetDuplicateRecords;
                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbcCommand, "txt");
                OdbcCommand.CommandTimeout = 200;
                // CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref odbcDa);
                DataTable MySqlDt = new DataTable();
                odbcDa.Fill(MySqlDt);
                return MySqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        #endregion

        #region MedicleForm

        public static DataTable GetDentrixMedicleFormData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixMedicleFormData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDentrixMedicleFormQuestionData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixMedicleFormQuestionData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                DataTable PartialDentrixControldt = OdbcDt.Copy();
                foreach (DataRow dr in OdbcDt.Rows)
                {
                    DataTable PartialControldt = GetDentrixMediclePartialQuestionData(dr["Dentrix_QuestionsTypeId"].ToString());

                    if (PartialControldt.Rows.Count > 0)
                    {
                        foreach (DataRow partialrow in PartialControldt.Rows)
                        {
                            string option = "";
                            bool is_required = false;
                            if (dr["Dentrix_QuestionsTypeId"].ToString() == "26")
                            {

                                DataSet ds = new DataSet();
                                StringReader theReader = new StringReader(dr["questioninfo"].ToString());
                                ds.ReadXml(theReader);
                                option = ds.Tables[0].Rows[0]["DefaultResponseText"].ToString().Replace(",", " Or ").Replace(Environment.NewLine, ",");
                                option = option.TrimEnd(',');
                                if (ds.Tables[0].Columns.Contains("IsRequired"))
                                {
                                    is_required = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRequired"].ToString());
                                }
                                else
                                {
                                    is_required = false;
                                }
                                //   is_required = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRequired"].ToString());

                            }
                            else
                            {
                                try
                                {
                                    DataSet ds = new DataSet();
                                    StringReader theReader = new StringReader(dr["questioninfo"].ToString());
                                    ds.ReadXml(theReader);
                                    option = partialrow["Options"].ToString();
                                    if (ds.Tables[0].Columns.Contains("IsRequired"))
                                    {
                                        is_required = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRequired"].ToString());
                                    }
                                    else
                                    {
                                        is_required = false;
                                    }
                                }
                                catch
                                { }
                                // is_required = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRequired"].ToString());
                            }
                            DataRow addnewrow = PartialDentrixControldt.NewRow();
                            addnewrow["Dentrix_FormQuestion_LocalDB_ID"] = dr["Dentrix_FormQuestion_LocalDB_ID"].ToString();
                            addnewrow["Dentrix_FormQuestion_Web_ID"] = dr["Dentrix_FormQuestion_Web_ID"].ToString();
                            addnewrow["Dentrix_Form_EHRUnique_ID"] = dr["Dentrix_Form_EHRUnique_ID"].ToString();
                            addnewrow["Dentrix_Question_EHR_ID"] = dr["Dentrix_Question_EHR_ID"].ToString();
                            addnewrow["Dentrix_Question_EHRUnique_ID"] = dr["Dentrix_Form_EHRUnique_ID"].ToString() + "_" + dr["Dentrix_Question_EHRUnique_ID"].ToString() + "_" + dr["Dentrix_Question_EHR_ID"].ToString() + "_" + dr["Dentrix_QuestionsTypeId"].ToString() + "_" + partialrow["Dentrix_ResponsetypeId"].ToString();
                            addnewrow["Dentrix_QuestionsTypeId"] = dr["Dentrix_QuestionsTypeId"].ToString();
                            addnewrow["Dentrix_QyestionTypeName"] = partialrow["Dentrix_QyestionTypeName"].ToString();
                            addnewrow["Dentrix_ResponsetypeId"] = partialrow["Dentrix_ResponsetypeId"].ToString();
                            addnewrow["Dentrix_QuestionName"] = partialrow["Dentrix_QuestionName"].ToString();
                            addnewrow["Dentrix_Question_DefaultValue"] = dr["Dentrix_Question_DefaultValue"].ToString();
                            addnewrow["QuestionVersion"] = dr["QuestionVersion"].ToString();
                            addnewrow["QuestionVersion_Date"] = (dr["QuestionVersion_Date"].ToString() == "" || dr["QuestionVersion_Date"].ToString() == string.Empty) ? DateTime.MinValue : Convert.ToDateTime(dr["QuestionVersion_Date"].ToString());
                            addnewrow["InputType"] = partialrow["InputType"].ToString();
                            addnewrow["Is_OptionField"] = Convert.ToBoolean(partialrow["Is_OptionField"].ToString());
                            addnewrow["Options"] = option;
                            addnewrow["Is_Required"] = is_required;
                            addnewrow["QuestionOrder"] = dr["QuestionOrder"].ToString();
                            addnewrow["EHR_Entry_DateTime"] = dr["EHR_Entry_DateTime"].ToString();
                            addnewrow["Last_Sync_Date"] = dr["Last_Sync_Date"].ToString();
                            addnewrow["Entry_DateTime"] = dr["Entry_DateTime"].ToString();
                            addnewrow["Is_Adit_Updated"] = dr["Is_Adit_Updated"].ToString();
                            addnewrow["is_deleted"] = dr["is_deleted"].ToString();
                            addnewrow["Is_MultiField"] = true;
                            addnewrow["Clinic_Number"] = "0";
                            addnewrow["Service_Install_Id"] = "1";
                            PartialDentrixControldt.Rows.Add(addnewrow);
                        }
                        DataRow deletedr = PartialDentrixControldt.Select("Dentrix_Form_EHRUnique_ID = '" + dr["Dentrix_Form_EHRUnique_ID"].ToString() + "' and Dentrix_Question_EHRUnique_ID = '" + dr["Dentrix_Question_EHRUnique_ID"].ToString() + "' and Dentrix_ResponsetypeId = 0").FirstOrDefault();
                        PartialDentrixControldt.Rows.Remove(deletedr);
                    }
                    else
                    {
                        DataRow Editdr = PartialDentrixControldt.Select("Dentrix_Form_EHRUnique_ID = '" + dr["Dentrix_Form_EHRUnique_ID"].ToString() + "' and Dentrix_Question_EHRUnique_ID = '" + dr["Dentrix_Question_EHRUnique_ID"].ToString() + "'").FirstOrDefault();
                        GetDentrixMedicleStaticQuestionData(PartialDentrixControldt, ref Editdr);
                        //if (Editdr["Dentrix_QuestionName"].ToString() == "" || Editdr["Dentrix_QuestionName"].ToString() == string.Empty)
                        //{
                        //    PartialDentrixControldt.Rows.Remove(Editdr);
                        //}
                    }
                }
                return PartialDentrixControldt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static void GetDentrixMedicleStaticQuestionData(DataTable PartialDentrixControldt, ref DataRow dr)
        {
            DataRow Editdr = PartialDentrixControldt.Select("Dentrix_Form_EHRUnique_ID = '" + dr["Dentrix_Form_EHRUnique_ID"].ToString() + "' and Dentrix_Question_EHRUnique_ID = '" + dr["Dentrix_Question_EHRUnique_ID"].ToString() + "'").FirstOrDefault();
            string Dentrix_QuestionsTypeId = dr["Dentrix_QuestionsTypeId"].ToString();
            string option = "";
            string DefaultResponseText = "";
            try
            {
                DataSet ds = new DataSet();
                StringReader theReader = new StringReader(dr["questioninfo"].ToString());
                ds.ReadXml(theReader);
                Editdr["Dentrix_Question_EHRUnique_ID"] = dr["Dentrix_Form_EHRUnique_ID"].ToString() + "_" + Editdr["Dentrix_Question_EHRUnique_ID"].ToString() + "_" + Editdr["Dentrix_Question_EHR_ID"].ToString() + "_" + Editdr["Dentrix_QuestionsTypeId"].ToString() + "_" + Editdr["Dentrix_QuestionsTypeId"].ToString();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("QuestionText"))
                    {
                        Editdr["Dentrix_QyestionTypeName"] = ds.Tables[0].Rows[0]["QuestionText"].ToString();
                        Editdr["Dentrix_QuestionName"] = ds.Tables[0].Rows[0]["QuestionText"].ToString();
                    }
                    else
                    {
                        Editdr["Dentrix_QyestionTypeName"] = "";
                        Editdr["Dentrix_QuestionName"] = "";
                    }
                    //   Editdr["Dentrix_QyestionTypeName"] = ds.Tables[0].Rows[0]["QuestionText"].ToString();
                    Editdr["Dentrix_ResponsetypeId"] = dr["Dentrix_QuestionsTypeId"].ToString();
                    //  
                    if (ds.Tables[0].Columns.Contains("IsRequired"))
                    {
                        Editdr["Is_Required"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRequired"].ToString());
                    }
                    else
                    {
                        Editdr["Is_Required"] = 0;
                    }
                    DefaultResponseText = ds.Tables[0].Rows[0]["DefaultResponseText"].ToString().Trim();
                }
                else
                {
                    Editdr["Dentrix_QyestionTypeName"] = "";
                    Editdr["Dentrix_QuestionName"] = "";
                    Editdr["Is_Required"] = 0;
                    DefaultResponseText = "";
                }
            }
            catch
            {

            }
            //    Editdr["Is_Required"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRequired"].ToString());
            Editdr["QuestionOrder"] = dr["QuestionOrder"].ToString();
            switch (Dentrix_QuestionsTypeId)
            {
                case "3":   // Body Text
                    Editdr["InputType"] = "Label";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    break;
                case "4": // Header Text
                    Editdr["InputType"] = "Label";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    break;
                case "5": // Sub Header Text
                    Editdr["InputType"] = "Label";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    break;
                case "6": // Consered Text
                    Editdr["InputType"] = "Label";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    break;
                case "7": // Note Response 
                    Editdr["InputType"] = "TextBox";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    Editdr["Dentrix_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "8": // Short Text Response
                    Editdr["InputType"] = "TextBox";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    Editdr["Dentrix_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "9": // DateTime
                    Editdr["InputType"] = "DateTime";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    Editdr["Dentrix_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "10": // Yes Or No Response
                    Editdr["InputType"] = "RadioButton";
                    Editdr["Is_OptionField"] = true;
                    Editdr["Options"] = "Yes[1],No[0]";
                    Editdr["Dentrix_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "11": // Number Response
                    Editdr["InputType"] = "Number";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "0";
                    Editdr["Dentrix_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "12": // Amount Response
                    Editdr["InputType"] = "Number";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "0.00";
                    Editdr["Dentrix_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "13": //One choice from list
                    Editdr["InputType"] = "RadioButton";
                    Editdr["Is_OptionField"] = true;
                    //option = ds.Tables[0].Rows[0]["DefaultResponseText"].ToString().Replace("]", "").Replace(Environment.NewLine, ",");
                    string[] rbtn = DefaultResponseText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    option = "";
                    foreach (string s in rbtn)
                    {
                        option = option + s.Replace(",", " Or ") + "[" + Array.IndexOf(rbtn, s).ToString() + "],";
                    }
                    option = option.TrimEnd(',');
                    Editdr["Options"] = option;
                    break;
                case "14": // Checkbox List
                    Editdr["InputType"] = "CheckBox";
                    Editdr["Is_OptionField"] = true;
                    // option = ds.Tables[0].Rows[0]["DefaultResponseText"].ToString().Replace("]", "]").Replace(Environment.NewLine, ",");
                    string[] cbhktn = DefaultResponseText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    option = "";
                    foreach (string s in cbhktn)
                    {
                        option = option + s.Replace(",", " Or ") + "[" + Array.IndexOf(cbhktn, s).ToString() + "],";
                    }
                    option = option.TrimEnd(',');
                    Editdr["Options"] = option;
                    break;
                case "15": // Confirmation
                    Editdr["InputType"] = "Confirmation";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    Editdr["Dentrix_Question_DefaultValue"] = DefaultResponseText;
                    break;

            }

        }

        public static DataTable GetDentrixMediclePartialQuestionData(string Dentrix_QuestionsTypeId)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchDentrixQRY.GetDentrixMediclePartialQuestionData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Dentrix_QuestionsTypeId", Dentrix_QuestionsTypeId);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }


        public static bool SaveMedicalHistoryLocalToDentrix(string strPatientFormID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            try
            {
                DataTable dtWebPatient_FormMedicalHistory = SynchLocalDAL.GetLiveDentrixPatientFormMedicalHistoryData(strPatientFormID);
                if (dtWebPatient_FormMedicalHistory != null)
                {
                    if (dtWebPatient_FormMedicalHistory.Rows.Count > 0)
                    {
                        DataTable LocalRespoanseSetDt = dtWebPatient_FormMedicalHistory.Copy().DefaultView.ToTable(true, "Patient_EHR_ID", "Dentrix_Form_EHRUnique_ID", "PatientForm_Web_ID");
                        if (LocalRespoanseSetDt != null)
                        {
                            foreach (DataRow dr in LocalRespoanseSetDt.Rows)
                            {
                                DataTable DentrixRespoanseSetDt = GetDentrixMediclePartialResponseData(dr["Dentrix_Form_EHRUnique_ID"].ToString(), dr["Patient_EHR_ID"].ToString());
                                string ResponsesetId = "";
                                if (DentrixRespoanseSetDt.Rows.Count == 0)
                                {
                                    string sqlSelect = string.Empty;
                                    CommonDB.OdbcConnectionServer(ref conn);
                                    CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                                    OdbcCommand.CommandText = SynchDentrixQRY.InsertDentrixMedicleResponseSetData;
                                    OdbcCommand.Parameters.Clear();
                                    OdbcCommand.Parameters.AddWithValue("Patient_EHR_id", dr["Patient_EHR_ID"].ToString());
                                    OdbcCommand.Parameters.AddWithValue("Dentrix_FormUniqueId", dr["Dentrix_Form_EHRUnique_ID"].ToString());
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();

                                    string QryIdentity = "Select max(responsesetuniqueid) as newId from admin.pq_responseset";
                                    OdbcCommand.CommandText = QryIdentity;
                                    OdbcCommand.CommandType = CommandType.Text;
                                    OdbcCommand.Connection = conn;
                                    CheckConnection(conn);
                                    ResponsesetId = Convert.ToString(OdbcCommand.ExecuteScalar());

                                }
                                else
                                {
                                    ResponsesetId = Convert.ToString(DentrixRespoanseSetDt.Rows[0]["responsesetuniqueidMain"].ToString());
                                    string sqlSelect = string.Empty;
                                    CommonDB.OdbcConnectionServer(ref conn);
                                    CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                                    OdbcCommand.CommandText = SynchDentrixQRY.UpdateDentrixMedicleResponseData;
                                    OdbcCommand.Parameters.Clear();
                                    OdbcCommand.Parameters.AddWithValue("ResponsesetId", ResponsesetId);
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();
                                }

                                DataRow[] LocalRespoanseDt = dtWebPatient_FormMedicalHistory.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"].ToString() + "' and Dentrix_Form_EHRUnique_ID = '" + dr["Dentrix_Form_EHRUnique_ID"].ToString() + "'");

                                if (LocalRespoanseDt != null)
                                {
                                    foreach (DataRow drRespoanse in LocalRespoanseDt)
                                    {
                                        string responseuniqueid = "";
                                        if (drRespoanse["Answer_Value"].ToString().Trim() != "" || drRespoanse["Answer_Value"].ToString().Trim() != string.Empty)
                                        {
                                            string[] arques = drRespoanse["Dentrix_Question_EHRUnique_ID"].ToString().Split('_');

                                            QuestionIds AllQues = new QuestionIds();
                                            AllQues.Dentrix_Form_EHRUnique_ID = arques[0];
                                            AllQues.Dentrix_Question_EHRUnique_ID = arques[1];
                                            AllQues.Dentrix_Question_EHR_ID = arques[2];
                                            AllQues.Dentrix_QuestionsTypeId = arques[3];
                                            AllQues.Dentrix_ResponsetypeId = arques[4];

                                            DataRow[] DentrixRespoanseDr = DentrixRespoanseSetDt.Copy().Select("responsesetuniqueid = '" + ResponsesetId + "' and questionuniqueid = '" + AllQues.Dentrix_Question_EHRUnique_ID + "' and responsetype = '" + AllQues.Dentrix_ResponsetypeId + "'");
                                            if (DentrixRespoanseDr == null || DentrixRespoanseDr.Count() == 0)
                                            {
                                                string sqlSelect = string.Empty;
                                                CommonDB.OdbcConnectionServer(ref conn);
                                                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                                                OdbcCommand.CommandText = SynchDentrixQRY.InsertDentrixMedicleResponseData;
                                                OdbcCommand.Parameters.Clear();
                                                OdbcCommand.Parameters.AddWithValue("responsesetuniqueid", ResponsesetId);
                                                OdbcCommand.Parameters.AddWithValue("questionuniqueid", AllQues.Dentrix_Question_EHRUnique_ID);
                                                OdbcCommand.Parameters.AddWithValue("responsetype", AllQues.Dentrix_ResponsetypeId);
                                                if (drRespoanse["InputType"].ToString().Trim().ToLower() == "datetime")
                                                {
                                                    string strAnsValue = drRespoanse["Answer_Value"].ToString();
                                                    try
                                                    {
                                                        DateTime dt = DateTime.Now;
                                                        DateTime.TryParse(strAnsValue.Trim(), out dt);
                                                        strAnsValue = dt.ToString("MM/dd/yyyy");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Utility.WriteToErrorLogFromAll("SaveMedicalHistoryLocalToDentrix: Converting value '" + strAnsValue + "' to DateTime fails.");
                                                    }
                                                    OdbcCommand.Parameters.AddWithValue("responseinfo", strAnsValue);
                                                }
                                                else
                                                {
                                                    OdbcCommand.Parameters.AddWithValue("responseinfo", drRespoanse["Answer_Value"].ToString());
                                                }
                                                CheckConnection(conn);
                                                OdbcCommand.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                string sqlSelect = string.Empty;
                                                CommonDB.OdbcConnectionServer(ref conn);
                                                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                                                OdbcCommand.CommandText = SynchDentrixQRY.InsertDentrixMedicleResponseData;
                                                OdbcCommand.Parameters.Clear();
                                                OdbcCommand.Parameters.AddWithValue("responsesetuniqueid", ResponsesetId);
                                                OdbcCommand.Parameters.AddWithValue("questionuniqueid", AllQues.Dentrix_Question_EHRUnique_ID);
                                                OdbcCommand.Parameters.AddWithValue("responsetype", AllQues.Dentrix_ResponsetypeId);
                                                if (drRespoanse["InputType"].ToString().Trim().ToLower() == "datetime")
                                                {
                                                    string strAnsValue = drRespoanse["Answer_Value"].ToString();
                                                    try
                                                    {
                                                        DateTime dt = DateTime.Now;
                                                        DateTime.TryParse(strAnsValue, out dt);
                                                        strAnsValue = dt.ToString("MM/dd/yyyy");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Utility.WriteToErrorLogFromAll("SaveMedicalHistoryLocalToDentrix: Converting value '" + strAnsValue + "' to DateTime fails.");
                                                    }
                                                    OdbcCommand.Parameters.AddWithValue("responseinfo", strAnsValue);
                                                }
                                                else
                                                {
                                                    OdbcCommand.Parameters.AddWithValue("responseinfo", drRespoanse["Answer_Value"].ToString());
                                                }
                                                CheckConnection(conn);
                                                OdbcCommand.ExecuteNonQuery();
                                            }

                                            string QryIdentity = "Select responseuniqueid as newId from admin.pq_response where responsesetuniqueid = '" + ResponsesetId + "' and questionuniqueid = '" + AllQues.Dentrix_Question_EHRUnique_ID + "' and responsetype = '" + AllQues.Dentrix_ResponsetypeId + "'";
                                            OdbcCommand.CommandText = QryIdentity;
                                            OdbcCommand.CommandType = CommandType.Text;
                                            OdbcCommand.Connection = conn;
                                            CheckConnection(conn);
                                            responseuniqueid = Convert.ToString(OdbcCommand.ExecuteScalar());

                                        }
                                        else
                                        {
                                            responseuniqueid = "0";
                                        }
                                        UpdateResponseUniqueEHRIdInDentrix_Response(responseuniqueid, drRespoanse["Dentrix_Question_EHRUnique_ID"].ToString());
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool SaveDiseaseLocalToDentrix(string strPatientFormID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            try
            {
                DataTable dtWebPatient_FormDisease = SynchLocalDAL.GetLocalPatientFormDiseaseResponseToSaveINEHR("1", strPatientFormID);
                // Below call should be condition wise If 
                DataTable dtDisease_Popupdata = GetDentrixDieaseAlertData();
                if (dtWebPatient_FormDisease != null)
                {
                    if (dtWebPatient_FormDisease.Rows.Count > 0)
                    {
                        DataTable dtDentrixPatientDisesase = GetDentrixPatientDiseaseData();
                        foreach (DataRow dr in dtWebPatient_FormDisease.Rows)
                        {
                            DataRow drPatientdiseaseresult = dtDentrixPatientDisesase.Select("Disease_EHR_Id = '" + dr["Disease_EHR_Id"].ToString() + "' and patient_EHR_id = '" + dr["PatientEHRID"].ToString() + "'").FirstOrDefault();
                            if (drPatientdiseaseresult == null)
                            {
                                DataRow diseaseresult = dtDisease_Popupdata.Select("Disease_EHR_Id = '" + dr["Disease_EHR_Id"].ToString() + "'").FirstOrDefault();
                                if (diseaseresult != null)
                                {
                                    string sqlSelect = string.Empty;
                                    CommonDB.OdbcConnectionServer(ref conn);
                                    CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                                    if (diseaseresult.Table.Columns.Contains("popup"))
                                    {
                                        if (diseaseresult["critical"] == null || diseaseresult["critical"].ToString() == string.Empty)
                                        {
                                            OdbcCommand.CommandText = SynchDentrixQRY.InsertDentrixDiseaseAlertDataWithoutCritical;
                                            OdbcCommand.Parameters.Clear();
                                            OdbcCommand.Parameters.AddWithValue("patid", dr["PatientEHRID"].ToString());
                                            OdbcCommand.Parameters.AddWithValue("hhitemid", dr["Disease_EHR_ID"].ToString());
                                            OdbcCommand.Parameters.AddWithValue("note", "Web");
                                            if (diseaseresult["popup"].ToString() != string.Empty && Convert.ToBoolean(diseaseresult["popup"].ToString()))
                                            {
                                                OdbcCommand.Parameters.AddWithValue("popup", true);
                                            }
                                            else
                                            {
                                                OdbcCommand.Parameters.AddWithValue("popup", false);
                                            }
                                        }
                                        else
                                        {
                                            OdbcCommand.CommandText = SynchDentrixQRY.InsertDentrixDiseaseAlertData;
                                            OdbcCommand.Parameters.Clear();
                                            OdbcCommand.Parameters.AddWithValue("patid", dr["PatientEHRID"].ToString());
                                            OdbcCommand.Parameters.AddWithValue("hhitemid", dr["Disease_EHR_ID"].ToString());
                                            OdbcCommand.Parameters.AddWithValue("note", "Web");
                                            if (diseaseresult["popup"].ToString() != string.Empty && Convert.ToBoolean(diseaseresult["popup"].ToString()))
                                            {
                                                OdbcCommand.Parameters.AddWithValue("popup", true);
                                            }
                                            else
                                            {
                                                OdbcCommand.Parameters.AddWithValue("popup", false);
                                            }
                                            if (diseaseresult["critical_TF"].ToString() != string.Empty && Convert.ToBoolean(diseaseresult["critical_TF"].ToString()))
                                            {
                                                OdbcCommand.Parameters.AddWithValue("critical", true);
                                            }
                                            else
                                            {
                                                OdbcCommand.Parameters.AddWithValue("critical", false);
                                            }
                                        }
                                        CheckConnection(conn);
                                        OdbcCommand.ExecuteNonQuery();
                                    }
                                    else
                                    {

                                        OdbcCommand.CommandText = SynchDentrixQRY.InsertDentrixDiseaseData;
                                        OdbcCommand.Parameters.Clear();
                                        OdbcCommand.Parameters.AddWithValue("patid", dr["PatientEHRID"].ToString());
                                        OdbcCommand.Parameters.AddWithValue("hhitemid", dr["Disease_EHR_ID"].ToString());
                                        OdbcCommand.Parameters.AddWithValue("note", "Web");
                                        CheckConnection(conn);
                                        OdbcCommand.ExecuteNonQuery();
                                    }

                                    string DiseaseId = "";
                                    string QryIdentity = "Select max(linkid) as newId from admin.hhlinkpat_item";
                                    OdbcCommand.CommandText = QryIdentity;
                                    OdbcCommand.CommandType = CommandType.Text;
                                    OdbcCommand.Connection = conn;
                                    CheckConnection(conn);
                                    DiseaseId = Convert.ToString(OdbcCommand.ExecuteScalar());
                                    SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), DiseaseId.ToString() == "" ? dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString() : DiseaseId.ToString(), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), "1");

                                }
                            }
                            else
                            {
                                SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString(), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), "1");
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Dentrix_Diesease_Err_" + ex.Message);
                return false;
                throw ex;
            }
            finally
            {
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool DeleteDiseaseLocalToDentrix(string strPatientFormID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            try
            {
                DataTable dtWebPatient_FormDeleteDisease = SynchLocalDAL.GetLocalPatientFormDiseaseDeleteResponseToSaveINEHR("1", strPatientFormID);
                // Below call should be condition wise If 

                if (dtWebPatient_FormDeleteDisease != null)
                {
                    if (dtWebPatient_FormDeleteDisease.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWebPatient_FormDeleteDisease.Rows)
                        {
                            string sqlSelect = string.Empty;
                            CommonDB.OdbcConnectionServer(ref conn);
                            CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                            OdbcCommand.CommandText = SynchDentrixQRY.DeleteDentrixDiseaseAlertData;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("patid", dr["Patient_EHR_ID"].ToString());
                            OdbcCommand.Parameters.AddWithValue("hhitemid", dr["Disease_EHR_ID"].ToString());
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            SynchLocalDAL.UpdateDeleteDiseaseEHR_Updateflg(dr["Disease_EHR_ID"].ToString(), dr["Patient_EHR_Id"].ToString(), "1");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Dentrix_Diesease_Err_" + ex.Message);
                return false;
                throw ex;
            }
            finally
            {
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private static bool UpdateResponseUniqueEHRIdInDentrix_Response(string responseuniqueid, string Dentrix_Question_EHRUnique_ID)
        {
            try
            {
                bool _successfullstataus = true;
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string sqlSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_Dentrix_Response_Response_EHR_ID;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Dentrix_Response_EHR_ID", responseuniqueid);
                            SqlCeCommand.Parameters.AddWithValue("Dentrix_Question_EHRUnique_ID", Dentrix_Question_EHRUnique_ID);
                            SqlCeCommand.ExecuteNonQuery();
                        }
                        _successfullstataus = true;

                    }
                    catch (Exception ex)
                    {
                        _successfullstataus = false;
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
                return _successfullstataus;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetDentrixMediclePartialResponseData(string Dentrix_FormUniqueId, string Patient_EHR_id)
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcDataAdapter OdbcDa = null;
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                OdbcCommand OdbcCommand = new OdbcCommand();
                if (conn.State == ConnectionState.Closed) conn.Open();
                string strQauery = SynchDentrixQRY.GetDentrixMediclePartialResponseData;
                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.AddWithValue("Patient_EHR_id", Patient_EHR_id);
                OdbcCommand.Parameters.AddWithValue("Dentrix_FormUniqueId", Dentrix_FormUniqueId);
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable table = new DataTable();
                OdbcDa.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        //SaveMedicationLocalToDentrix
        public static bool SaveMedicationLocalToDentrix(ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcCommand OdbcCommand = new OdbcCommand();
            Int64 MedicationPatientId = 0;
            Int64 MedicationNum = 0;
            string odbcSelect = "";
            DataTable dtMedication = new DataTable();
            DataTable dtPatientMedication = new DataTable();
            //Version BaseVersion = new Version("17.1.307.0");
            //Version EHRVersion = new Version(Utility.EHR_VersionNumber);
            double EHRVersion = GetInstalledDentrixVersion();
            try
            {

                DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationResponseToSaveINEHR("1", strPatientFormID);
                // Below call should be condition wise If 

                if (dtPatientMedicationResponse != null)
                {
                    if (dtPatientMedicationResponse.Rows.Count > 0)
                    {
                        if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                        {
                            Utility.EHR_UserLogin_ID = GetDentrixUserLogin_ID();
                        }
                        dtMedication = GetDentrixMedicationData();
                        dtPatientMedication = GetDentrixPatientMedicationData("");
                        foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                        {
                            MedicationNum = 0;
                            MedicationPatientId = 0;
                            DataRow[] drMedRow;
                            string Note = "", OrgNote = "";
                            Utility.WriteToErrorLogFromAll("Dentrix_Medication Version" + EHRVersion.ToString());
                            bool blnAddInactiveMedication = false;
                            if (EHRVersion >= 7.1)
                            {
                            }
                            else if (EHRVersion >= 6.1)
                            {
                            }
                            else
                            {
                                OrgNote = dr["Medication_Note"].ToString().Trim();
                                MedicationNum = Convert.ToInt64(dr["Medication_EHR_ID"].ToString().Trim());
                                //drMedRow = dtMedication.Copy().Select("Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "' And Medication_Notes = '" + dr["Medication_Note"].ToString().Trim() + "'");
                                drMedRow = dtMedication.Copy().Select("Medication_EHR_ID = " + MedicationNum.ToString().Trim());
                                if (drMedRow.Length > 0)
                                {
                                    //MedicationNum = Convert.ToInt64(drMedRow[0]["Medication_EHR_ID"]);
                                    Note = Convert.ToString(drMedRow[0]["Medication_Notes"]).Trim();
                                }

                                if (Note.Trim().ToUpper() != OrgNote.Trim().ToUpper())
                                {
                                    MedicationNum = 0;
                                    blnAddInactiveMedication = true;
                                }
                            }

                            if (dr["Medication_EHR_Id"] == DBNull.Value) dr["Medication_EHR_Id"] = "";
                            if (dr["Medication_EHR_Id"].ToString().Trim() == "" || dr["Medication_EHR_Id"].ToString().Trim() == "0" || blnAddInactiveMedication)
                            {
                                if (EHRVersion >= 6.1)
                                {
                                    Utility.WriteToErrorLogFromAll("Medication '" + dr["Medication_Name"].ToString().Trim() + "' not found. Medication ID: " + dr["Medication_EHR_Id"].ToString().Trim());
                                    continue;
                                }

                                if (EHRVersion >= 7.1)
                                {
                                    //odbcSelect = SynchDentrixQRY.InsertMedicationG7New;
                                    //odbcSelect = odbcSelect.Replace("@Medication_Name", "'" + dr["Medication_Name"].ToString().Trim() + "'");
                                    //OdbcCommand.CommandText = odbcSelect;
                                    //OdbcCommand.Parameters.Clear();

                                    //CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    //CheckConnection(conn);
                                    //OdbcCommand.ExecuteNonQuery();

                                    //odbcSelect = "Select max(itemid) as itemid from admin.hhitem where hhtype = 3";
                                    //OdbcCommand.CommandText = odbcSelect;
                                    //OdbcCommand.Parameters.Clear();
                                    //CheckConnection(conn);
                                    //MedicationNum = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                    //dr["Medication_EHR_ID"] = MedicationNum.ToString();
                                }
                                else if (EHRVersion >= 6.1)
                                {
                                    //odbcSelect = SynchDentrixQRY.InsertMedicationG7;
                                    ////@IsTemplate, @IsStandard, getdate(), getdate(), @DrugName,
                                    ////@Description, @Dispense, @AsWritten, @Refills, @PatID, @ProvID, @SIGTEXT, @NOTETEXT
                                    //odbcSelect = odbcSelect.Replace("@IsTemplate", "1");
                                    //odbcSelect = odbcSelect.Replace("@IsStandard", "1");
                                    //odbcSelect = odbcSelect.Replace("@DrugName", "'" + dr["Medication_Name"].ToString().Trim() + "'");
                                    //odbcSelect = odbcSelect.Replace("@Dispense", "''");
                                    //odbcSelect = odbcSelect.Replace("@Description", "''");
                                    //odbcSelect = odbcSelect.Replace("@AsWritten", "1");
                                    //odbcSelect = odbcSelect.Replace("@Refills", "0");
                                    //odbcSelect = odbcSelect.Replace("@PatID", "0");
                                    //odbcSelect = odbcSelect.Replace("@ProvID", "'" + Utility.EHR_UserLogin_ID + "'");
                                    //odbcSelect = odbcSelect.Replace("@SIGTEXT", "''");
                                    //odbcSelect = odbcSelect.Replace("@NOTETEXT", "'" + dr["Medication_Note"].ToString().Trim() + "'");

                                    //CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    //CheckConnection(conn);
                                    //OdbcCommand.ExecuteNonQuery();

                                    //odbcSelect = "Select max(rxid) as RxID from admin.rxrec where PatID = 0";
                                    //OdbcCommand.CommandText = odbcSelect;
                                    //OdbcCommand.Parameters.Clear();
                                    //CheckConnection(conn);
                                    //MedicationNum = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                    //dr["Medication_EHR_ID"] = MedicationNum.ToString();
                                }
                                else
                                {
                                    if (MedicationNum <= 0)
                                    {
                                        odbcSelect = SynchDentrixQRY.InsertMedicationG5andG6;
                                        //@Active,@std,@rxdate,@drugname,@descript,@dispense,@refills,@aswritten
                                        drMedRow = dtMedication.Copy().Select("Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "'");
                                        string strDesc = drMedRow.Length > 0 ? drMedRow[0]["Medication_Description"].ToString().Trim() : dr["Medication_Name"].ToString().Trim();
                                        string strDisp = drMedRow.Length > 0 ? drMedRow[0]["Drug_Quantity"].ToString().Trim() : "";
                                        string strRefill = drMedRow.Length > 0 ? drMedRow[0]["Refills"].ToString().Trim() : "0";
                                        string strNotes = drMedRow.Length > 0 ? drMedRow[0]["Medication_Notes"].ToString().Trim() : "0";
                                        string strSIG = drMedRow.Length > 0 ? drMedRow[0]["Medication_SIG"].ToString().Trim() : "0";

                                        if ((Note.Trim().ToUpper() != OrgNote.Trim().ToUpper()))
                                        {
                                            odbcSelect = odbcSelect.Replace("@Active", "0");
                                            odbcSelect = odbcSelect.Replace("@std", "0");
                                            odbcSelect = odbcSelect.Replace("@rxdate", "getdate()");
                                            odbcSelect = odbcSelect.Replace("@drugname", "'" + dr["Medication_Name"].ToString().Trim() + "'");
                                            odbcSelect = odbcSelect.Replace("@descript", "'" + strDesc + "'");
                                            odbcSelect = odbcSelect.Replace("@dispense", "'" + strDisp + "'");
                                            odbcSelect = odbcSelect.Replace("@refills", strRefill);
                                            odbcSelect = odbcSelect.Replace("@aswritten", "0");
                                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                            CheckConnection(conn);
                                            OdbcCommand.ExecuteNonQuery();

                                            odbcSelect = "Select Max(RxDefID) from admin.RxDef";

                                            OdbcCommand.CommandText = odbcSelect;
                                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                            OdbcCommand.Parameters.Clear();
                                            CheckConnection(conn);
                                            MedicationNum = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                            dr["Medication_EHR_ID"] = MedicationNum.ToString();

                                            odbcSelect = "Insert into admin.fullnotes(notetype,noteid,notedate,notetext) "
                                                + " values(@NoteType,@Medication_EHR_ID,getdate(),@Medication_Note)";
                                            odbcSelect = odbcSelect.Replace("@NoteType", "34");
                                            odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", MedicationNum.ToString());
                                            odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + OrgNote + "'");
                                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                            CheckConnection(conn);
                                            OdbcCommand.ExecuteNonQuery();

                                            odbcSelect = "Insert into admin.fullnotes(notetype,noteid,notedate,notetext) "
                                                + " values(@NoteType,@Medication_EHR_ID,getdate(),@Medication_Note)";
                                            odbcSelect = odbcSelect.Replace("@NoteType", "35");
                                            odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", MedicationNum.ToString());
                                            odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + strSIG + "'");
                                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                            CheckConnection(conn);
                                            OdbcCommand.ExecuteNonQuery();

                                            odbcSelect = "Update admin.RxDef set noteid = " + MedicationNum + ", sigid = " + MedicationNum + " where RxDefID = " + MedicationNum;
                                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                            CheckConnection(conn);
                                            OdbcCommand.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            Utility.WriteToErrorLogFromAll("Medication '" + dr["Medication_Name"].ToString().Trim() + "' not found. Medication ID: " + dr["Medication_EHR_Id"].ToString().Trim());
                                            continue;

                                            //odbcSelect = odbcSelect.Replace("@Active", "1");
                                            //odbcSelect = odbcSelect.Replace("@std", "1");
                                            //odbcSelect = odbcSelect.Replace("@rxdate", "getdate()");
                                            //odbcSelect = odbcSelect.Replace("@drugname", "'" + dr["Medication_Name"].ToString().Trim() + "'");
                                            //odbcSelect = odbcSelect.Replace("@descript", "''");
                                            //odbcSelect = odbcSelect.Replace("@dispense", "''");
                                            //odbcSelect = odbcSelect.Replace("@refills", "0");
                                            //odbcSelect = odbcSelect.Replace("@aswritten", "");

                                            //CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                            //CheckConnection(conn);
                                            //OdbcCommand.ExecuteNonQuery();

                                            //odbcSelect = "Select Max(RxDefID) from admin.RxDef";
                                            //OdbcCommand.CommandText = odbcSelect;
                                            //OdbcCommand.Parameters.Clear();
                                            //CheckConnection(conn);
                                            //MedicationNum = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                            //dr["Medication_EHR_ID"] = MedicationNum.ToString();

                                            //odbcSelect = "Insert into admin.fullnotes(notetype,noteid,notedate,notetext) "
                                            //    + " values(@NoteType,@Medication_EHR_ID,getdate(),@Medication_Note)";
                                            //odbcSelect = odbcSelect.Replace("@NoteType", "34");
                                            //odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", MedicationNum.ToString());
                                            //odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + dr["Medication_Notes"].ToString().Trim() + "'");
                                            //CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                            //CheckConnection(conn);
                                            //OdbcCommand.ExecuteNonQuery();

                                            //odbcSelect = "Insert into admin.fullnotes(notetype,noteid,notedate,notetext) "
                                            //    + " values(@NoteType,@Medication_EHR_ID,getdate(),@Medication_Note)";
                                            //odbcSelect = odbcSelect.Replace("@NoteType", "35");
                                            //odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", MedicationNum.ToString());
                                            //odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + strSIG + "'");
                                            //CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                            //CheckConnection(conn);
                                            //OdbcCommand.ExecuteNonQuery();

                                            //odbcSelect = "Update admin.RxDef set noteid = " + MedicationNum + ", sigid = " + MedicationNum + " where RxDefID = " + MedicationNum;
                                            //CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                            //CheckConnection(conn);
                                            //OdbcCommand.ExecuteNonQuery();
                                        }
                                    }
                                }

                                DataRow newRow = dtMedication.NewRow();
                                newRow["Medication_EHR_ID"] = MedicationNum;
                                newRow["Medication_Name"] = dr["Medication_Name"].ToString().Trim();
                                newRow["Medication_Description"] = dr["Medication_Name"].ToString().Trim();
                                newRow["Medication_Notes"] = dr["Medication_Note"].ToString().Trim();
                                //newRow["Medicatoin_Type"] = "Drug";
                                dtMedication.Rows.Add(newRow);
                                dtMedication.AcceptChanges();
                            }
                            else
                            {
                                if (MedicationNum <= 0)
                                    MedicationNum = Convert.ToInt64(dr["Medication_EHR_ID"]);
                                drMedRow = dtMedication.Copy().Select("Medication_EHR_ID = " + MedicationNum);
                            }

                            DataRow[] drPatMedRow;
                            if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "0";
                            if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" && dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                            {
                                MedicationPatientId = Convert.ToInt64(dr["PatientMedication_EHR_Id"].ToString().Trim());
                            }

                            if (MedicationPatientId <= 0)
                            {
                                string strSelect = "Patient_EHR_ID = " + dr["PatientEHRID"].ToString().Trim() +
                                            " And Medication_EHR_ID = " + MedicationNum + " And is_active1='True' ";

                                drPatMedRow = dtPatientMedication.Copy().Select(strSelect);
                                if (drPatMedRow.Length > 0)
                                {
                                    MedicationPatientId = Convert.ToInt64(drPatMedRow[0]["PatientMedication_EHR_ID"].ToString().Trim());
                                }
                            }

                            if (MedicationPatientId <= 0)
                            {
                                string ProvID = "";
                                odbcSelect = "Select provid1 from admin.patient  where patid = " + dr["PatientEHRID"].ToString().Trim();
                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                CheckConnection(conn);
                                ProvID = Convert.ToString(OdbcCommand.ExecuteScalar());

                                if (EHRVersion >= 7.1)
                                {
                                    DataRow drMedication = dtMedication.Copy().Select("Medication_EHR_ID='" + dr["Medication_EHR_ID"].ToString().Trim() + "'").FirstOrDefault();
                                    if (drMedication == null)
                                    {
                                        Utility.WriteToErrorLogFromAll("Medication '" + dr["Medication_Name"].ToString().Trim() + "' not found. Medication ID: " + dr["Medication_EHR_Id"].ToString().Trim());
                                        continue;
                                    }
                                    odbcSelect = SynchDentrixQRY.InsertPatientMedicationG7New;
                                    odbcSelect = odbcSelect.Replace("@Patient_EHR_ID", dr["PatientEHRID"].ToString().Trim());
                                    odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", dr["Medication_EHR_ID"].ToString().Trim());
                                    odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + dr["Medication_Note"].ToString().Trim() + "'");
                                    if (drMedication.Table.Columns.Contains("popup"))
                                    {
                                        odbcSelect = odbcSelect.Replace("@popup", drMedication["popup"].ToString().Trim().ToUpper() == "TRUE" ? "1" : "0");
                                    }
                                    else
                                    {
                                        odbcSelect = odbcSelect.Replace("@popup", "0");
                                    }
                                    OdbcCommand.CommandText = odbcSelect;
                                    OdbcCommand.Parameters.Clear();

                                    CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();

                                    odbcSelect = "Select max(linkid) as itemid from admin.hhlinkpat_item where patid = " + dr["PatientEHRID"].ToString();
                                    OdbcCommand.CommandText = odbcSelect;
                                    OdbcCommand.Parameters.Clear();

                                    CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    CheckConnection(conn);
                                    MedicationPatientId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                    dr["PatientMedication_EHR_ID"] = MedicationPatientId.ToString();
                                }
                                else if (EHRVersion >= 6.1)
                                {
                                    OdbcDataAdapter OdbcDa = null;
                                    odbcSelect = "Select istemplate,isstandard,createdate,rxdate,drugname,descript,dispense,dispenseaswritten,refills,patid,"
                                               + "provid,sigtext,notetext from admin.rxrec where rxid = " + dr["Medication_EHR_ID"].ToString().Trim();
                                    CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                                    DataTable OdbcDt = new DataTable();
                                    OdbcDa.Fill(OdbcDt);

                                    if (OdbcDt.Rows.Count > 0)
                                    {
                                        odbcSelect = SynchDentrixQRY.InsertMedicationG7;
                                        odbcSelect = odbcSelect.Replace("@IsTemplate", "0");
                                        odbcSelect = odbcSelect.Replace("@IsStandard", "1");
                                        odbcSelect = odbcSelect.Replace("@DrugName", "'" + dr["Medication_Name"].ToString().Trim() + "'");
                                        odbcSelect = odbcSelect.Replace("@Description", "'" + OdbcDt.Rows[0]["descript"].ToString().Trim() + "'");
                                        odbcSelect = odbcSelect.Replace("@Dispense", "''");
                                        odbcSelect = odbcSelect.Replace("@AsWritten", "0");
                                        odbcSelect = odbcSelect.Replace("@Refills", "0");
                                        odbcSelect = odbcSelect.Replace("@PatID", dr["PatientEHRId"].ToString().Trim());
                                        odbcSelect = odbcSelect.Replace("@ProvID", "'" + ProvID + "'");
                                        odbcSelect = odbcSelect.Replace("@SIGTEXT", "''");
                                        odbcSelect = odbcSelect.Replace("@NOTETEXT", "'" + dr["Medication_Note"].ToString().Trim() + "'");
                                        odbcSelect = odbcSelect.Replace("@RXGUID", "'" + Guid.NewGuid().ToString() + "'");
                                        CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                        CheckConnection(conn);
                                        OdbcCommand.ExecuteNonQuery();

                                        odbcSelect = "Select max(rxid) as RxID from admin.rxrec where PatID = " + dr["PatientEHRID"].ToString().Trim();
                                        OdbcCommand.CommandText = odbcSelect;
                                        OdbcCommand.Parameters.Clear();
                                        CheckConnection(conn);
                                        MedicationPatientId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                        dr["PatientMedication_EHR_ID"] = MedicationPatientId.ToString();
                                    }
                                    else
                                    {
                                        Utility.WriteToErrorLogFromAll("Medication not found in EHR Medication Master: " + odbcSelect);
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (MedicationNum > 0)
                                    {
                                        //@Medication_EHR_ID,@Patient_EHR_ID,getdate(),@Provider_EHR_ID
                                        odbcSelect = SynchDentrixQRY.InsertPatientMedicationG5andG6;
                                        odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", MedicationNum.ToString());
                                        odbcSelect = odbcSelect.Replace("@Patient_EHR_ID", dr["PatientEHRId"].ToString().Trim());
                                        odbcSelect = odbcSelect.Replace("@Provider_EHR_ID", "'" + ProvID.ToString().Trim() + "'");

                                        CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                        CheckConnection(conn);
                                        OdbcCommand.ExecuteNonQuery();

                                        odbcSelect = "Select max(rxid) as RxID from admin.RxPatient where PatID = " + dr["PatientEHRID"].ToString().Trim();
                                        OdbcCommand.CommandText = odbcSelect;
                                        OdbcCommand.Parameters.Clear();
                                        CheckConnection(conn);
                                        MedicationPatientId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                        dr["PatientMedication_EHR_ID"] = MedicationPatientId.ToString();
                                    }
                                    else
                                    {
                                        Utility.WriteToErrorLogFromAll("Medication not found in EHR Medication Master: " + odbcSelect);
                                        continue;
                                    }
                                }

                                DataRow NewRow = dtPatientMedication.NewRow();
                                NewRow["Patient_EHR_ID"] = dr["PatientEHRID"].ToString();
                                NewRow["Medication_EHR_ID"] = MedicationNum.ToString();
                                NewRow["PatientMedication_EHR_ID"] = MedicationPatientId.ToString();
                                NewRow["Medication_Name"] = dr["Medication_Name"].ToString();
                                //NewRow["Medication_Type"] = "Drug";
                                NewRow["Drug_Quantity"] = "";
                                NewRow["Medication_Note"] = dr["Medication_Note"].ToString();
                                NewRow["Provider_EHR_ID"] = "";
                                NewRow["is_active1"] = Convert.ToBoolean("True");

                                dtPatientMedication.Rows.Add(NewRow);
                                dtPatientMedication.AcceptChanges();
                            }
                            else
                            {
                                if (EHRVersion >= 7.1)
                                {
                                    odbcSelect = SynchDentrixQRY.UpdatePatientMedicationNotesG7New;
                                    odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", MedicationPatientId.ToString().Trim());
                                    odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + dr["Medication_Note"].ToString().Trim().Replace("'","''") + "'");
                                    odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", "'" + dr["Medication_EHR_ID"].ToString().Trim() + "'");
                                    OdbcCommand.CommandText = odbcSelect;
                                    OdbcCommand.Parameters.Clear();
                                    CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();
                                }
                                else if (EHRVersion >= 6.1)
                                {
                                    drMedRow = dtMedication.Copy().Select("Medication_EHR_ID = " + dr["Medication_EHR_ID"].ToString().Trim());
                                    string strName = drMedRow.Length > 0 ? drMedRow[0]["Medication_Name"].ToString().Trim() : dr["Medication_Name"].ToString().Trim();
                                    string strSIG = drMedRow.Length > 0 ? drMedRow[0]["Medication_SIG"].ToString().Trim() : "";
                                    string strDesp = drMedRow.Length > 0 ? drMedRow[0]["Medication_Description"].ToString().Trim() : dr["Medication_Name"].ToString().Trim();

                                    odbcSelect = SynchDentrixQRY.UpdatePatientMedicationNotesG7;
                                    odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", MedicationPatientId.ToString().Trim());
                                    odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + dr["Medication_Note"].ToString().Trim().Replace("'", "''") + "'");
                                    odbcSelect = odbcSelect.Replace("@Medication_Name", "'" + strName.Replace("'", "''") + "'");
                                    odbcSelect = odbcSelect.Replace("@Medication_Description", "'" + strDesp.Replace("'", "''") + "'");
                                    odbcSelect = odbcSelect.Replace("@Medication_SIG", "'" + strSIG.Replace("'", "''") + "'");
                                    OdbcCommand.CommandText = odbcSelect;
                                    OdbcCommand.Parameters.Clear();
                                    CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                }
                            }
                            if (!SavePatientEHRID.Contains("'" + dr["PatientEHRID"].ToString() + "'"))
                            {
                                SavePatientEHRID = SavePatientEHRID + "'" + dr["PatientEHRID"].ToString() + "',";
                            }
                            SynchLocalDAL.UpdateMedicationEHR_Updateflg(dr["MedicationResponse_Local_ID"].ToString(), MedicationPatientId.ToString(), dr["PatientEHRID"].ToString(), dr["Medication_EHR_Id"].ToString(), "1");
                        }
                        isRecordSaved = true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Dentrix_Medication_Err_" + ex.Message);
                //Utility.WriteToErrorLogFromAll("Dentrix_Medication_Err_" + ex.StackTrace);
                throw ex;
            }
            finally
            {
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool DeleteMedicationLocalToDentrix(ref bool isRecordDeleted, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            OdbcCommand OdbcCommand = new OdbcCommand();
            string odbcSelect = "";
            DeletePatientEHRID = "";
            //Version BaseVersion = new Version("17.1.307.0");
            //Version EHRVersion = new Version(Utility.EHR_VersionNumber);
            double EHRVersion = GetInstalledDentrixVersion();
            try
            {
                DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationRemovedResponseToSaveINEHR("1", strPatientFormID);
                if (dtPatientMedicationResponse != null)
                {
                    if (dtPatientMedicationResponse.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                        {
                            if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "";

                            if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" || dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                            {
                                //if (strLike(Utility.EHR_VersionNumber, "17.1%") || EHRVersion >= BaseVersion)
                                //if (EHRVersion >= new Version("17.1.0.0"))
                                if (EHRVersion >= 7.1)
                                {
                                    odbcSelect = SynchDentrixQRY.DeletePatientMedicationG7New;
                                    odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", dr["PatientMedication_EHR_ID"].ToString().Trim());
                                    OdbcCommand.CommandText = odbcSelect;
                                    OdbcCommand.Parameters.Clear();
                                    CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();
                                }
                                //else if (strLike(Utility.EHR_VersionNumber, "17.0%") || EHRVersion < BaseVersion)
                                //else if (EHRVersion >= new Version("17.0.0.0") || Utility.Application_Version.ToLower() == "DTX G6.2+".ToLower())
                                else if (EHRVersion >= 6.1)
                                {
                                    odbcSelect = "Delete From admin.rxrec where rxid = " + dr["PatientMedication_EHR_ID"].ToString().Trim();
                                    OdbcCommand.CommandText = odbcSelect;
                                    OdbcCommand.Parameters.Clear();
                                    CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    //Select* from admin.RxPatient WHere RxID = 12
                                    odbcSelect = "Delete From admin.RxPatient where RxID = " + dr["PatientMedication_EHR_ID"].ToString().Trim();
                                    OdbcCommand.CommandText = odbcSelect;
                                    OdbcCommand.Parameters.Clear();
                                    CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();
                                }

                                if (!DeletePatientEHRID.Contains("'" + dr["PatientEHRID"].ToString() + "'"))
                                {
                                    DeletePatientEHRID = DeletePatientEHRID + "'" + dr["PatientEHRID"].ToString() + "',";
                                }
                                SynchLocalDAL.UpdateRemovedMedicationEHR_Updateflg(dr["MedicationRemovedResponse_Local_ID"].ToString(), dr["PatientMedication_EHR_Id"].ToString(), dr["PatientEHRID"].ToString(), "1");
                            }
                        }
                        isRecordDeleted = true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Delete_Dentrix_Medication_Err_" + ex.Message);
                throw ex;
            }
            finally
            {
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        #endregion

        #region Insurance

        public static DataTable GetDentrixInsuranceData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchDentrixQRY.GetDentrixInsuranceData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                //return OdbcDt;

                //rooja 20-8-24
                //MySqlDa.Fill(MySqlDt);

                DataTable Dttmp = OdbcDt.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == "1")
                    {
                        for (int j = 0; j < OdbcDt.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp.NewRow();
                            Dr.ItemArray = OdbcDt.Rows[j].ItemArray;
                            Dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                            Dttmp.Rows.Add(Dr);
                        }
                    }
                }
                return Dttmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static bool Save_Insurance_Dentrix_To_Local(DataTable dtDentrixInsurance)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtDentrixInsurance.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Insurance;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Insurance;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Insurance;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Insurance_EHR_ID", dr["insid"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Name", dr["insconame"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address", dr["street1"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address2", "");
                                SqlCeCommand.Parameters.AddWithValue("City", dr["city"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("State", dr["state"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["zipcode"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Phone", dr["phone"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ElectId", dr["payerid"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("EmployerName", "");
                                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", false);
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                                Utility.WriteToErrorLogFromAll("Insurance Sync (" + Utility.Application_Name + " to Local Database) . synched Insurance from EHR to local" );
                            }
                        }
                    }
                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }
        #endregion

        #region Insurance Carrier
        public static bool Save_InsuranceCarrier_Document_in_Dentrix(string strInsuranceCarrierID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            bool callLoop = false;
            try
            {
                CommonDB.OdbcConnectionServer(ref conn);
                //get data from local

                DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLocaleInsuranceCarrierDocData(strInsuranceCarrierID,true);
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                    key2.SetValue("InsuranceCarrierIsSyncing", false);

                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        if (callLoop == false)
                        {
                            string sourceLocation = CommonUtility.GetAditInsuranceCarrierDocTempPath() + "\\" + dr["InsuranceCarrier_Doc_Name"].ToString();
                            if (!System.IO.File.Exists(sourceLocation))
                            {
                                PullLiveDatabaseDAL.Update_InsuranceCarrierDocNotFound_Live_To_Local(dr["InsuranceCarrier_Doc_Web_ID"].ToString());
                                continue;
                            }

                            string tmpFileOrgName = Path.GetFileName(sourceLocation);
                            string SourcePath = Path.GetDirectoryName(sourceLocation);

                            Thread.Sleep(100);
                            string showingName = dr["InsuranceCarrier_Doc_Name"].ToString().Trim();

                            if (showingName.Length > 40)
                            {
                                showingName = showingName.Substring(0, 39);
                            }

                            if (AttachInsuranceCarrierDocument(sourceLocation, "/ID" + dr["Patient_EHR_ID"].ToString(), showingName,dr["Folder_Name"].ToString(), ref callLoop))
                            {
                                RegistryKey key1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DocumentAttachedId");
                                Int64 SavePatientDocId = 0;
                                if (key1 != null)
                                {
                                    SavePatientDocId = Convert.ToInt64(key1.GetValue("InsuranceCarrierId").ToString());
                                    if (SavePatientDocId > 0)
                                    {

                                        //update local 
                                        PullLiveDatabaseDAL.Update_InsuranceCarrierDoc_Local_To_EHR(dr["InsuranceCarrier_Doc_Web_ID"].ToString(), SavePatientDocId.ToString());
                                        //PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), , "1");
                                        File.Delete(sourceLocation);
                                    }
                                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DocumentAttachedId");
                                    key.SetValue("InsuranceCarrierId", 0);
                                }
                                callLoop = false;
                            }
                            else
                            {
                                callLoop = false;
                            }
                        }
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        while (callLoop)
                        {
                            if (sw.Elapsed > TimeSpan.FromSeconds(120))
                            {
                                callLoop = false;
                                break;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private static bool AttachInsuranceCarrierDocument(string docPath, string patientId, string FormName, string FolderName, ref bool callloop)
        {
            Process myProcess = new Process();
            bool returnResult = false;
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DentrixAttachmentCall");
                bool IsSyncing = false;
                if (key != null)
                {
                    IsSyncing = bool.Parse(key.GetValue("InsuranceCarrierIsSyncing").ToString());
                }
                if (!IsSyncing)
                {
                    callloop = true;
                    RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                    key1.SetValue("InsuranceCarrierIsSyncing", true);

                    try
                    {
                        // RegistryKey keydocPath = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentDocumentPath");
                        //  keydocPath.SetValue("DentrixDocPath", docPath);

                        //  RegistryKey keyDocFileName = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentDocumentName");
                        //  keyDocFileName.SetValue("DentrixDocName", "Adit_PatientForm_"+DateTime.Now.ToString());  

                        myProcess.StartInfo.UseShellExecute = false;
                        Utility.WriteToErrorLogFromAll("Call Dentrix Exe to attach PDF for " + patientId.ToString() + " FolderName = " + FolderName.ToString() + " Doc Path " + docPath.ToString());
                        myProcess.StartInfo.FileName = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName).ToString() + "\\DocumentDLL\\DentrixDocument.exe";
                        // myProcess.StartInfo.FileName = "C:\\Program Files (x86)\\Dentrix\\Document.CenterDoc.exe";
                        //  myProcess.StartInfo.Arguments = "" + patientId.ToString() + " " + Path.Combine(docPath) + " " + Utility.DentrixDocConnString + " " + Utility.DentrixDocPWD + "";
                        myProcess.StartInfo.Arguments = string.Format("{0} {1} {2} {3} {4} {5} {6}", "\"" + patientId.ToString() + "\"", "\"" + Path.Combine(docPath) + "\"", "\"" + Utility.DentrixDocConnString + "\"", "\"" + FormName + "\"", "\"" + "Insurance" + "\"", "\"" + FolderName + "\"", "\"" + Utility.DentrixDocPWD + "\"");
                        // myProcess.StartInfo.Arguments = "" + patientId.ToString() + " \"C:\\Program Files (x86)\\PozativeDocument\\Patient\\1\\PendingDocument\\Remote Access Instructions.pdf\"";
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.StartInfo.Verb = "runas";
                        myProcess.Start();
                        myProcess.WaitForExit();
                        Utility.WriteToErrorLogFromAll("DONE Call Dentrix Exe to attach PDF for " + patientId.ToString() + " FolderName = " + FolderName.ToString() + " Doc Path " + docPath.ToString());
                        RegistryKey key2 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DentrixAttachmentCall");
                        if (bool.Parse(key2.GetValue("InsuranceCarrierIsSyncing").ToString()))
                        {
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (bool.Parse(key2.GetValue("InsuranceCarrierIsSyncing").ToString()))
                            {
                                returnResult = true;
                                if (sw.Elapsed > TimeSpan.FromSeconds(120))
                                {
                                    RegistryKey key3 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                                    key3.SetValue("InsuranceCarrierIsSyncing", false);
                                    returnResult = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            returnResult = true;
                        }

                        //if (!bool.Parse(key2.GetValue("IsSyncing").ToString()))
                        //{
                        //    return true;
                        //}
                        //else
                        //{
                        //    returnResult = false;
                        //    //return false;
                        //}                        
                    }
                    catch (Exception ex)
                    {
                        string abc = ex.Message;
                        key1.SetValue("InsuranceCarrierIsSyncing", false);
                        throw;
                    }
                }
                return returnResult;
            }
            catch (Exception e1)
            {
                RegistryKey key3 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DentrixAttachmentCall");
                key3.SetValue("InsuranceCarrierIsSyncing", false);
                return returnResult;
            }
        }
        #endregion


        public static DataTable GetDentrixPatientImagesData(string connectionString, string imagePathName)
        {
            try
            {

                DataTable dtPatientProfile = new DataTable();
                DataColumn dc1 = new DataColumn("Patient_Images_LocalDB_ID", typeof(string));
                DataColumn dc2 = new DataColumn("Patient_Images_Web_ID", typeof(string));
                DataColumn dc3 = new DataColumn("Patient_Images_EHR_ID", typeof(string));
                DataColumn dc4 = new DataColumn("Patient_EHR_ID", typeof(string));
                DataColumn dc5 = new DataColumn("Patient_Web_ID", typeof(string));
                DataColumn dc6 = new DataColumn("Image_EHR_Name", typeof(string));
                DataColumn dc14 = new DataColumn("Patient_Images_FilePath", typeof(string));
                DataColumn dc7 = new DataColumn("Entry_DateTime", typeof(DateTime));
                DataColumn dc8 = new DataColumn("AditApp_Entry_DateTime", typeof(DateTime));
                DataColumn dc9 = new DataColumn("Is_Deleted", typeof(bool));
                DataColumn dc10 = new DataColumn("Is_Adit_Updated", typeof(bool));
                DataColumn dc11 = new DataColumn("Clinic_Number", typeof(string));
                DataColumn dc12 = new DataColumn("Service_Install_Id", typeof(string));
                DataColumn dc13 = new DataColumn("SourceLocation", typeof(string));
                //dtOpenDentalPatientImages.Columns.Add("SourceLocation", typeof(string));

                dtPatientProfile.Columns.Add(dc1);
                dtPatientProfile.Columns.Add(dc2);
                dtPatientProfile.Columns.Add(dc3);
                dtPatientProfile.Columns.Add(dc4);
                dtPatientProfile.Columns.Add(dc5);
                dtPatientProfile.Columns.Add(dc6);
                dtPatientProfile.Columns.Add(dc14);
                dtPatientProfile.Columns.Add(dc7);
                dtPatientProfile.Columns.Add(dc8);
                dtPatientProfile.Columns.Add(dc9);
                dtPatientProfile.Columns.Add(dc10);
                dtPatientProfile.Columns.Add(dc11);
                dtPatientProfile.Columns.Add(dc12);
                dtPatientProfile.Columns.Add(dc13);

                DataTable dtPatient = GetDentrixPatientData();
                string patientPicName = "";
                foreach (DataRow drRow in dtPatient.Rows)
                {
                    patientPicName = GetPatientImageBaseFileName(Convert.ToInt64(drRow["Patient_EHR_ID"]));

                    DataRow drNew = dtPatientProfile.NewRow();                    

                    FileInfo oFileInfo = new FileInfo(imagePathName + "\\" + patientPicName);
                    
                    if (oFileInfo.Exists)
                    {
                        DateTime dtCreationTime = oFileInfo.LastAccessTime;
                        drNew["Patient_Images_LocalDB_ID"] = "";
                        drNew["Patient_Images_Web_ID"] = "";
                        drNew["Patient_Images_EHR_ID"] = oFileInfo.Name;
                        drNew["Patient_EHR_ID"] = drRow["Patient_EHR_ID"];
                        drNew["Patient_Web_ID"] = "";
                        drNew["Image_EHR_Name"] = "";
                        drNew["Patient_Images_FilePath"] = imagePathName + "\\" + patientPicName;
                        drNew["Entry_DateTime"] = dtCreationTime.ToString();
                        drNew["AditApp_Entry_DateTime"] = DateTime.Now;
                        drNew["Is_Deleted"] = 0;
                        drNew["Is_Adit_Updated"] = 0;
                        drNew["Clinic_Number"] = 0;
                        drNew["Service_Install_Id"] = 1;
                        drNew["SourceLocation"] = imagePathName + "\\" + patientPicName;
                        dtPatientProfile.Rows.Add(drNew);
                    }
                    else
                    {
                        try
                        {
                            #region new code for image extensions check : for task : https://app.asana.com/0/1203599217474380/1207428407611180/f

                            bool IsFileExists = false;
                            string supportedExtensions = "*.jpg,*.gif,*.png,*.bmp,*.jpe,*.jpeg,*.wmf,*.emf,*.xbm,*.ico,*.eps,*.tif,*.tiff,*.g01,*.g02,*.g03,*.g04,*.g05,*.g06,*.g07,*.g08";
                            //foreach (string imageFile in Directory.GetFiles(imagePathName, "*.*", SearchOption.TopDirectoryOnly).Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower())))
                            foreach (string imageFile in Directory.GetFiles(imagePathName, "" + patientPicName + ".*", SearchOption.TopDirectoryOnly).Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower())))
                            {
                                // MessageBox.Show("FileName :"+ imageFile.ToString());
                                string filePathwithext = @"" + imagePathName + "\\" + patientPicName;
                                string dirfile = @"" + imagePathName + "\\" + Path.GetFileNameWithoutExtension(imageFile.ToString());
                                if (filePathwithext.ToString().ToLower() == dirfile.ToString().ToLower())
                                {
                                    //  MessageBox.Show("file exists");
                                    IsFileExists = true;
                                }
                            }
                            if (IsFileExists)
                            {
                                DateTime dtCreationTime = DateTime.Now;
                                drNew["Patient_Images_LocalDB_ID"] = "";
                                drNew["Patient_Images_Web_ID"] = "";
                                drNew["Patient_Images_EHR_ID"] = patientPicName;
                                drNew["Patient_EHR_ID"] = drRow["Patient_EHR_ID"];
                                drNew["Patient_Web_ID"] = "";
                                drNew["Image_EHR_Name"] = "";
                                drNew["Patient_Images_FilePath"] = imagePathName + "\\" + patientPicName;
                                drNew["Entry_DateTime"] = dtCreationTime.ToString();
                                drNew["AditApp_Entry_DateTime"] = DateTime.Now;
                                drNew["Is_Deleted"] = 0;
                                drNew["Is_Adit_Updated"] = 0;
                                drNew["Clinic_Number"] = 0;
                                drNew["Service_Install_Id"] = 1;
                                drNew["SourceLocation"] = imagePathName + "\\" + patientPicName;
                                dtPatientProfile.Rows.Add(drNew);
                            }
                            #endregion
                        }
                        catch(Exception ex)
                        {
                            Utility.WriteToErrorLogFromAll("Patient Profile image with extensions error : " + ex.Message);                            
                        }
                    }
                }

                return dtPatientProfile;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static string GetPatientImageBaseFileName(Int64 patientId)
        {
            char ch;
            if (patientId > 0)
            {
                ch = 'P';
            }
            else
            {
                ch = '-';
                patientId = -patientId;
            }
            return string.Format("P{0}{1}", (object)ch, ConvertToRadixString(patientId, 36));
        }

        public static string ConvertToRadixString(Int64 sourceValue, int radix)
        {
            string str = string.Empty;
            if (radix < 2 || radix > 36)
                throw new NotSupportedException("Invalid Radix");
            if (sourceValue == 0)
            {
                str = "0";
            }
            else
            {
                for (uint index = (uint)sourceValue; index != 0U; index = (uint)((ulong)index / (ulong)radix))
                    str = Convert.ToString("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"[(int)((long)index % (long)radix)]) + str;
            }
            return str;
        }

        public static double GetInstalledDentrixVersion()
        {
            string strVersion = Utility.Application_Version.ToUpper().Replace("DTX G", "");
            string strTempDisplayVersion = "";
            double dblVersion = 0;
            try
            {
                var r = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Dentrix Dental Systems, Inc.\Dentrix\General", "Install GUID", null);
                var GUID = r.ToString();
                var DisplayName = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "DisplayName", null);
                try
                {
                    strTempDisplayVersion = DisplayName.ToString().ToUpper().Replace("DENTRIX", "");
                    strTempDisplayVersion = strTempDisplayVersion.ToString().ToUpper().Replace("G", "");
                    dblVersion = Convert.ToDouble(strTempDisplayVersion);
                }
                catch (Exception)
                {
                    strTempDisplayVersion = "7.1";
                }
                if (DisplayName.ToString().Trim().ToUpper() == "DENTRIX")
                {
                    var DisplayVersion = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "DisplayVersion", null);
                    Version BaseVersion = new Version("17.1.307.0");
                    Version EHRVersion = new Version(DisplayVersion.ToString().Trim());
                    if (EHRVersion >= BaseVersion)
                    {
                        strTempDisplayVersion = "7.1";
                    }
                }
                //if (DisplayName.ToString().Trim().ToUpper() == "DENTRIX")
                //{
                //    var DisplayVersion = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + GUID, "DisplayVersion", null);
                //    Version BaseVersion = new Version("17.1.307.0");
                //    Version EHRVersion = new Version(DisplayVersion.ToString().Trim());
                //    if (EHRVersion >= BaseVersion)
                //    {
                //        strTempDisplayVersion = "7.1";
                //    }
                //    else if (EHRVersion >= new Version("17.1.0.0") && EHRVersion < BaseVersion)
                //    {
                //        strTempDisplayVersion = "6.2";
                //    }
                //    else
                //    {
                //        strTempDisplayVersion = "6";
                //    }
                //}
            }
            catch (Exception ex)
            {
                strTempDisplayVersion = "7.1";
                throw ex;
            }
            finally
            {
                if (string.IsNullOrEmpty(strTempDisplayVersion))
                {
                    strTempDisplayVersion = strVersion.ToString();
                }
                else
                {
                    strTempDisplayVersion = strTempDisplayVersion.ToString();
                }
                dblVersion = Convert.ToDouble(strTempDisplayVersion.Replace("+", ""));
            }
            return dblVersion;
        }

        public static bool InsertDataApptHistoryData(string appt_ehr_id)
        {
            try
            {
                if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                {
                    Utility.EHR_UserLogin_ID = GetDentrixUserLogin_ID();
                }

                DataTable dtapptdetails = GetDentrixAppointment_Data_forApptHistory(appt_ehr_id);
                OdbcConnection conn = null;
                OdbcCommand OdbcCommand = new OdbcCommand();
                CommonDB.OdbcConnectionServer(ref conn);

                foreach (DataRow dr in dtapptdetails.Rows)
                {
                    OdbcCommand.CommandTimeout = 200;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = SynchDentrixQRY.InsertApptHistory;

                    //sqlSelect = sqlSelect.Replace("@appthistid", appthistId.ToString());
                    sqlSelect = sqlSelect.Replace("@apptid", appt_ehr_id);
                    sqlSelect = sqlSelect.Replace("@curdate", DateTime.Now.ToString("MM/dd/yyyy"));
                    sqlSelect = sqlSelect.Replace("@curtime", DateTime.Now.TimeOfDay.TotalMilliseconds.ToString());
                    sqlSelect = sqlSelect.Replace("@apptdate", dr["apptdate"].ToString().Trim());
                    sqlSelect = sqlSelect.Replace("@operatoryid", dr["opid"].ToString().Trim());
                    sqlSelect = sqlSelect.Replace("@patientid", dr["patid"].ToString().Trim());
                    sqlSelect = sqlSelect.Replace("@providerid", dr["provid"].ToString().Trim());
                    sqlSelect = sqlSelect.Replace("@user", Utility.EHR_UserLogin_ID);
                    sqlSelect = sqlSelect.Replace("@apptreason", dr["apptreason"].ToString().Trim());
                    //sqlSelect = sqlSelect.Replace("@apptreason", "ADITBYYASH");
                    sqlSelect = sqlSelect.Replace("@apptlen", dr["apptlen"].ToString().Trim());
                    sqlSelect = sqlSelect.Replace("@timehr", dr["timehr"].ToString().Trim());
                    sqlSelect = sqlSelect.Replace("@timemin", dr["timemin"].ToString().Trim());
                    CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll(ex.Message);
                return false;
            }
            return true;
        }

        public static DataTable GetDentrixAppointment_Data_forApptHistory(string appt_ehr_id)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string OdbcSelect = SynchDentrixQRY.GetapptData;
                OdbcSelect = OdbcSelect.Replace("@appointmentid", appt_ehr_id);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        #region CheckDuplicatePatient 
        //Dipika
        public static DataTable CheckDentrixDuplicatePatientData(string first_name, string last_name, string patientID = "0")
        {
            DataTable OdbcDt = new DataTable();
            try
            {
                using (OdbcConnection conn = new OdbcConnection(CommonUtility.DentrixConnectionString()))
                {
                    string OdbcSelect = "";
                    if (patientID == "0")
                    {
                        OdbcSelect = SynchDentrixQRY.CheckDentrixDuplicatePatientData;
                    }
                    else
                    {
                        OdbcSelect = SynchDentrixQRY.CheckDentrixPatientExistorNot;
                    }
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandTimeout = 200;
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.Parameters.Clear(); if (patientID == "0")
                        {
                            OdbcCommand.Parameters.AddWithValue("@first_name", first_name.ToString());
                            OdbcCommand.Parameters.AddWithValue("@last_name", last_name.ToString());
                        }
                        else
                        {
                            OdbcCommand.Parameters.AddWithValue("@PatientID", patientID.ToString());
                        }

                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable CheckDentrixDuplicatePatientSSNData(string ssn)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchDentrixQRY.CheckDentrixDuplicatePatientSSNData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("@ssn", ssn.ToString());
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static string CheckDuplicatePatient(DataTable dtWebPatient_Form, string PatientForm_Web_ID)
        {
            try
            {
                object ofName = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == PatientForm_Web_ID && a.Field<string>("EHRField").ToString().ToUpper() == "FIRSTNAME").Select(b => b.Field<string>("EHRField_Value")).FirstOrDefault();
                string fName = ofName == null ? "" : ofName.ToString();
                object olname = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == PatientForm_Web_ID && a.Field<string>("EHRField").ToString().ToUpper() == "LASTNAME").Select(b => b.Field<string>("EHRField_Value")).FirstOrDefault();
                string LName = olname == null ? "" : olname.ToString();
                object omobileno = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == PatientForm_Web_ID && a.Field<string>("EHRField").ToString().ToUpper() == "PAGER").Select(b => b.Field<string>("EHRField_Value")).FirstOrDefault();
                string mobileNo = omobileno == null ? "" : omobileno.ToString();
                DataTable dtEHRPatientData = CheckDentrixDuplicatePatientData(fName.ToString(), LName.ToString());
                if (dtEHRPatientData.Select(" First_name = '" + fName.ToString() + "' AND Last_name = '" + LName.ToString() + "'").Count() > 0)
                {
                    bool ismatchedrecords = false;

                    foreach (DataRow drRow in dtEHRPatientData.Select(" First_name = '" + fName.ToString() + "' AND Last_name = '" + LName.ToString() + "'"))
                    {
                        if (drRow["Mobile"] != null && drRow["Mobile"].ToString() != "" && Utility.ConvertContactNumber(drRow["Mobile"].ToString().ToUpper().Trim()) == Utility.ConvertContactNumber(mobileNo.ToString().Trim()).ToUpper())
                        {
                            ismatchedrecords = true;
                        }
                        else if (drRow["Home_Phone"] != null && drRow["Home_Phone"].ToString() != "" && Utility.ConvertContactNumber(drRow["Home_Phone"].ToString().ToUpper().Trim()) == Utility.ConvertContactNumber(mobileNo.ToString().Trim()).ToUpper())
                        {
                            ismatchedrecords = true;
                        }
                        else if (drRow["Work_Phone"] != null && drRow["Work_Phone"].ToString() != "" && Utility.ConvertContactNumber(drRow["Work_Phone"].ToString().ToUpper().Trim()) == Utility.ConvertContactNumber(mobileNo.ToString().Trim()).ToUpper())
                        {
                            ismatchedrecords = true;
                        }
                        if (ismatchedrecords)
                        {
                            UpdatePatientEHRIdINPatientForm(drRow["Patient_EHR_ID"].ToString(), PatientForm_Web_ID);
                            return drRow["Patient_EHR_ID"].ToString();
                        }

                    }
                    return "";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception PF)
            {
                Utility.WriteToErrorLogFromAll("PF Duplicate Patient Form : " + PF.Message.ToString());
                return "";

            }
        }
        #endregion

        public static void GetDentrixChartNumberSettingsData()
        {
            // bool ChartNumberStatus = false;
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string OdbcSelect = SynchDentrixQRY.GetDentrixChartNumberSettings;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                // = true;
                if (OdbcDt == null)
                {
                    Utility.ChartNumberIsNumericstr = Utility.ChartNumberIsNumeric ? "Numeric" : "AlphaNumeric";
                }
                else if (OdbcDt.Rows.Count <= 0)
                {
                    Utility.ChartNumberIsNumericstr = Utility.ChartNumberIsNumeric ? "Numeric" : "AlphaNumeric";
                }
                else
                {
                    if (OdbcDt.Rows[0]["newId"].ToString() != "")
                    {
                        //  MessageBox.Show("dtDTXChartNumberSettings count" + dtDTXChartNumberSettings.Rows[0]["newId"].ToString());
                        string ChartNumberSetting = OdbcDt.Rows[0]["newId"].ToString();
                        string ChartCharMatch = ChartNumberSetting.Substring(0, 4);
                        if (ChartCharMatch == "1000")
                        {
                            Utility.ChartNumberIsNumericstr = "Numeric";
                        }
                        else if (ChartCharMatch == "0100")
                        {
                            Utility.ChartNumberIsNumericstr = "AlphaNumeric";
                        }
                        else if (ChartCharMatch == "0010")
                        {
                            Utility.ChartNumberIsNumericstr = "None";
                        }
                    }
                }
                //return ChartNumberStatus;
            }
            catch (Exception ex)
            {
                throw ex;
                //ChartNumberStatus = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
    }

    public class QuestionIds
    {
        public string Dentrix_Form_EHRUnique_ID { get; set; }
        public string Dentrix_Question_EHRUnique_ID { get; set; }
        public string Dentrix_Question_EHR_ID { get; set; }
        public string Dentrix_QuestionsTypeId { get; set; }
        public string Dentrix_ResponsetypeId { get; set; }
    }
}
