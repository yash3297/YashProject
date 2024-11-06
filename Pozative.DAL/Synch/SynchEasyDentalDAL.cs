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



namespace Pozative.DAL
{
    public class SynchEasyDentalDAL
    {
        public static bool GetEasyDentalConnection()
        {
            bool IsConnected = false;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    IsConnected = true;
                }
            }
            catch (Exception ex)
            {
                IsConnected = false;
            }
            return IsConnected;
        }

        #region Appointment

        public static DataTable GetEasyDentalAppointmentData()
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalAppointmentData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalAppointmentIds()
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalAppointmentEhrIds.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalAppointment_Procedures_Data()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalAppointment_Procedures_Data.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalAppointment_Procedures_SecondData()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalAppointment_Procedures_Data_Second_Table.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //public static DataTable GetEasyDentalAppointment_Procedures_Data()
        //{
        //    DateTime ToDate = Utility.LastSyncDateAditServer;
        //    try
        //    {
        //        DataTable OdbcDt = new DataTable();
        //        using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
        //        {
        //            if (conn.State != ConnectionState.Open) conn.Open();
        //            string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalAppointment_Procedures_Data.Trim();
        //            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
        //            {
        //                OdbcCommand.CommandType = CommandType.Text;
        //                OdbcCommand.Parameters.Clear();
        //                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
        //                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
        //                {
        //                    OdbcDa.Fill(OdbcDt);
        //                }
        //            }
        //        }
        //        return OdbcDt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static DataTable GetEasyDentalAppointment_Procedures_SecondData()
        //{
        //    DateTime ToDate = Utility.LastSyncDateAditServer;
        //    try
        //    {
        //        DataTable OdbcDt = new DataTable();
        //        using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
        //        {
        //            if (conn.State != ConnectionState.Open) conn.Open();
        //            string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalAppointment_Procedures_Data_Second_Table.Trim();
        //            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
        //            {
        //                OdbcCommand.CommandType = CommandType.Text;
        //                OdbcCommand.Parameters.Clear();
        //                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
        //                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
        //                {
        //                    OdbcDa.Fill(OdbcDt);
        //                }
        //            }
        //        }
        //        return OdbcDt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static DataTable GetEasyDentalAppointment_Procedures_SubData()
        //{
        //    DateTime ToDate = Utility.LastSyncDateAditServer;
        //    try
        //    {
        //        DataTable OdbcDt = new DataTable();
        //        using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
        //        {
        //            if (conn.State != ConnectionState.Open) conn.Open();
        //            string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalAppointment_Procedures_Data_FirstSub.Trim();
        //            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
        //            {
        //                OdbcCommand.CommandType = CommandType.Text;
        //                OdbcCommand.Parameters.Clear();
        //                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
        //                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
        //                {
        //                    OdbcDa.Fill(OdbcDt);
        //                }
        //            }
        //        }
        //        return OdbcDt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    } 
        //}

        public static string GetPatientName(int patientid)
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
                OdbcCommand.CommandText = SynchEasyDentalQRY.GetPatientName;
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

        #region Getting Procedure Data for Appointments

        public static DataTable GetEasyDentalAppointment_ApptId_Procedures_Data()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalAppointmentIDsForProcDesc.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalAppointment_Procedures_Type0_Data()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            //DateTime ToDate = new DateTime(2022,7,1);
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalProcsForCodeType0.Trim();
                    for (int index = 1; index <= 20; index++)
                    {
                        OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalProcsForCodeType0.Trim();

                        OdbcSelect = OdbcSelect.Replace("_Number", index.ToString());

                        using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                        {
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                            using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                            {
                                OdbcDa.Fill(OdbcDt);
                            }
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalAppointment_Procedures_Type1_Data()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            //DateTime ToDate = new DateTime(2022, 7, 1);

            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalProcsForCodeType0.Trim();
                    for (int index = 1; index <= 20; index++)
                    {
                        OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalProcsForCodeType1.Trim();

                        OdbcSelect = OdbcSelect.Replace("_Number", index.ToString());

                        using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                        {
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                            using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                            {
                                OdbcDa.Fill(OdbcDt);
                            }
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalProcDescForCodeType0_Data()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalProcDescForCodeType0.Trim();

                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }

                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalProcDescForCodeType1_Data()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalProcDescForCodeType1.Trim();

                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }

                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        public static DataTable GetEasyDentalAppointmentNoteData(string AppointmentId)
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalAppointmentNoteData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        //  OdbcCommand.Parameters.Clear();
                        //   OdbcCommand.Parameters.AddWithValue("AppointmentId", Convert.ToInt32(AppointmentId.ToString()));
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalApplicationVersion()
        {


            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.OdbcConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalApplicationVersion;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
        }

        public static bool Save_Appointment_EasyDental_To_Local(DataTable dtEasyDentalAppointment)
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

                        foreach (DataRow dr in dtEasyDentalAppointment.Rows)
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

                                    Apptdate = Convert.ToDateTime(dr["appointment_date"].ToString()).ToString("dd/MM/yyyy");
                                    ApptTime = dr["start_hour"].ToString().Split(':')[0].ToString() + ":" + dr["start_hour"].ToString().Split(':')[1].ToString();

                                    Mobile_Contact = string.Empty;
                                    Email = string.Empty;
                                    Home_Contact = string.Empty;
                                    Address = string.Empty;
                                    City = string.Empty;
                                    State = string.Empty;
                                    Zipcode = string.Empty;
                                    Mobile_Contact = dr["patMobile"].ToString();
                                    Email = dr["patEmail"].ToString();
                                    Home_Contact = dr["patient_phone"].ToString().Trim();
                                    Address = dr["Address1"].ToString().Trim() + " , " + dr["Address2"].ToString().Trim();
                                    City = dr["city"].ToString().Trim();
                                    State = dr["state"].ToString().Trim();
                                    Zipcode = dr["zipcode"].ToString().Trim();
                                    DateTime ApptDateTime = DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
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
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString() == "" ? Utility.GetCurrentDatetimestring() : dr["EHR_Entry_DateTime"].ToString());
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
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        #region Deleted Appointment

        public static DataTable GetEasyDentalDeletedAppointmentData()
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalDeletedAppointmentData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public static bool Update_DeletedAppointment_EasyDental_To_Local(DataTable dtEasyDentalDeletedAppointment)
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
                        foreach (DataRow dr in dtEasyDentalDeletedAppointment.Rows)
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #endregion

        #region OperatoryEvent

        public static DataTable GetEasyDentalOperatoryEventData()
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

                string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalOperatoryEventData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@EventDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
        }

        public static bool Save_OperatoryEvent_EasyDental_To_Local(DataTable dtEasyDentalOperatoryEvent)
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
                        foreach (DataRow dr in dtEasyDentalOperatoryEvent.Rows)
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }
        #endregion

        #region Provider

        #region Provider

        public static DataTable GetEasyDentalProviderData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalProviderData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static bool Save_Provider_EasyDental_To_Local(DataTable dtEasyDentalProvider)
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
                        foreach (DataRow dr in dtEasyDentalProvider.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("is_active", Convert.ToBoolean(dr["is_active"].ToString().Trim()));
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region ProviderHours

        public static DataTable GetEasyDentalProviderHoursData()
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
                    string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalProviderHoursData;
                    //  OdbcSelect = OdbcSelect.Replace("@provider_Id", drRow["Provider_EHR_Id"].ToString());
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.Parameters.Clear();
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
        }

        public static bool Save_ProviderHours_EasyDental_To_Local(DataTable dtEasyDentalProviderHours)
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
                        foreach (DataRow dr in dtEasyDentalProviderHours.Rows)
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region ProviderOfficeHours

        public static DataTable GetEasyDentalProviderOfficeHours()
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
                    string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalProviderOfficeHours;
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalPRoviderIds;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
        }


        #endregion

        #endregion

        #region Operatory

        #region Operatory

        public static DataTable GetEasyDentalOperatoryData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalOperatoryData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool Save_Operatory_EasyDental_To_Local(DataTable dtEasyDentalOperatory)
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
                        foreach (DataRow dr in dtEasyDentalOperatory.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("OperatoryOrder", Convert.ToInt16(dr["OperatoryOrder"]));
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region OperatoryHours

        public static DataTable GetEasyDentalOperatoryHoursData()
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
                    string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalOperatoryHoursData;
                    //  OdbcSelect = OdbcSelect.Replace("@Operatory_Id", drRow["Operatory_EHR_Id"].ToString());
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.Parameters.Clear();
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
        }

        public static bool Save_OperatoryHours_EasyDental_To_Local(DataTable dtEasyDentalOperatoryHours)
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
                        foreach (DataRow dr in dtEasyDentalOperatoryHours.Rows)
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region OperatoryOfficeHours

        public static DataTable GetEasyDentalOperatoryOfficeHours()
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
                    string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalOperatoryOfficeHours;
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
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

                string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalOperatoryIds;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dtResultOperatoryOfficeHours;
        }

        #endregion

        #endregion

        #region ApptType

        public static DataTable GetEasyDentalApptTypeData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalApptTypeData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool Save_ApptType_EasyDental_To_Local(DataTable dtEasyDentalApptType)
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
                        foreach (DataRow dr in dtEasyDentalApptType.Rows)
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region Patient

        public static DataTable GetEasyDentalPatientData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalPatientData(string PatientEHRIDs)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientData.Trim() + " Where patid in (" + PatientEHRIDs + ")";
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable GetEasyDentalPatientData(Int32 PatientId)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientData.Trim() + " where patid in (select patid from appt_dat)";
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        // OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.AddWithValue("PatientId", PatientId);
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable GetEasyDentalInsuranceData()
        {

            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalInsuranceData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalPatientNoteData()
        {

            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientNoteData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalEmployerData()
        {

            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalEmployerData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static DataTable GetEasyDentalResponsiblePartyData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalResponsiblePartyData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalAgingData()
        {

            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalAgingData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        //public static bool Save_Patient_EasyDental_To_Local(DataTable dtEasyDentalPatient)
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
        //        foreach (DataRow dr in dtEasyDentalPatient.Rows)
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

        public static bool Save_Patient_EasyDental_To_Local(DataTable dtEasyDentalPatient, string InsertTableName, DataTable dtEasyDentalPatientNextApptDate, DataTable dtEasyDentalPatientdue_date, DataTable dtLocalPatient, bool bSetDeleted = false)
        {
            bool _successfullstataus = true;
            SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtEasyDentalPatient, "0", "1");
            DataTable dtEasyDentalPatientcollect_payment = new DataTable();


            string sqlSelect = string.Empty;
            // SqlCeTransaction SqlCetx;

            // SqlCetx = conn.BeginTransaction();
            DataTable dtEasyDentalPatientIns_data = GetEasyDentalInsuranceData();
            DataTable dtEasyDentalPatientAging_data = GetEasyDentalAgingData();
            DataTable dtEasyDentalPatientNote = GetEasyDentalPatientNoteData();
            DataTable dtEasyDentalEmployer = GetEasyDentalEmployerData();
            DataTable dtEasyDentalResponsibleParty = GetEasyDentalResponsiblePartyData();
            try
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                        {
                            SqlCeCommand.CommandText = "Delete from PatientCompare where Service_Install_Id = 1";
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                }

                for (int j = 0; j < dtEasyDentalPatient.Rows.Count; j++)
                {
                    DataRow dr = dtEasyDentalPatient.Rows[j];
                    //###
                    if (dr != null)
                    {
                        string Status = string.Empty;
                        string tmpBirthDate = string.Empty;
                        string tmpFirstVisit_Date = string.Empty;
                        string tmpLastVisit_Date = string.Empty;
                        string tmpnextvisit_date = string.Empty;
                        string tmpdue_date = string.Empty;
                        string tmpReceive_Sms_Email = string.Empty;
                        string tmpReceiveVoiceCall = "Y";
                        decimal curPatientcollect_payment = 0;
                        int tmpprivacy_flags = 0;


                        tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());
                        tmpFirstVisit_Date = Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim());
                        tmpLastVisit_Date = Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim());

                        tmpReceive_Sms_Email = "Y";
                        //  case when privacyflags in (1,3,5,7) then 'N' else 'Y' end as f For ReceiveVoiceCall
                        tmpprivacy_flags = Convert.ToInt32(dr["privacy_flags"].ToString());

                        if (tmpprivacy_flags == 2 || tmpprivacy_flags == 3 || tmpprivacy_flags == 6 || tmpprivacy_flags == 7)
                        {
                            tmpReceive_Sms_Email = "N";
                        }
                        if (tmpprivacy_flags == 1 || tmpprivacy_flags == 3 || tmpprivacy_flags == 5 || tmpprivacy_flags == 7)
                        {
                            tmpReceiveVoiceCall = "N";
                        }
                        dr["ReceiveSMS"] = tmpReceive_Sms_Email.ToString();
                        dr["ReceiveEmail"] = tmpReceive_Sms_Email.ToString();
                        dr["ReceiveVoiceCall"] = tmpReceiveVoiceCall.ToString();

                        // https://app.asana.com/0/751059797849097/1149506260330945
                        dr["nextvisit_date"] = Utility.SetNextVisitDate(dtEasyDentalPatientNextApptDate, "Patient_EHR_ID", "Patient_EHR_ID", "nextvisit_date", dr["Patient_EHR_ID"].ToString());
                        tmpnextvisit_date = dr["nextvisit_date"].ToString();

                        try
                        {
                            dtEasyDentalPatientcollect_payment = SynchEasyDentalDAL.GetEasyDentalPatientcollect_payment(dr["Patient_EHR_ID"].ToString().Trim());
                            //   DataRow[] drPatientcollect_payment = dtEasyDentalPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");

                            if (dtEasyDentalPatientcollect_payment.Rows.Count > 0)
                            {
                                curPatientcollect_payment = Convert.ToDecimal(dtEasyDentalPatientcollect_payment.Rows[0]["collect_payment"].ToString());
                                curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                dr["collect_payment"] = curPatientcollect_payment.ToString();
                            }
                            else
                            {
                                curPatientcollect_payment = 0;
                                dr["collect_payment"] = curPatientcollect_payment.ToString();
                            }
                        }
                        catch (Exception)
                        {
                            curPatientcollect_payment = 0;
                            dr["collect_payment"] = curPatientcollect_payment.ToString();
                        }
                        try
                        {

                            DataRow[] Patdue_date = dtEasyDentalPatientdue_date.Copy().Select("patient_id = '" + dr["Patient_EHR_ID"] + "'");
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
                                        tmpdue_date = Convert.ToDateTime(SortPatdue_date.Rows[i]["due_date"].ToString()).ToString("MM/dd/yyyy HH:mm:ss") + "@" + SortPatdue_date.Rows[i]["recall_type"].ToString().Trim() + "@" + SortPatdue_date.Rows[i]["recall_type_id"].ToString() + "|" + tmpdue_date;
                                    }
                                    dr["due_date"] = tmpdue_date;
                                }
                                else
                                {
                                    foreach (DataRow due_row in Patdue_date)
                                    {
                                        tmpdue_date = Convert.ToDateTime(due_row["due_date"].ToString()).ToString("MM/dd/yyyy HH:mm:ss") + "@" + due_row["recall_type"].ToString().Trim() + "@" + due_row["recall_type_id"].ToString() + "|" + tmpdue_date;
                                    }
                                    dr["due_date"] = tmpdue_date;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            tmpdue_date = string.Empty;
                        }

                        try
                        {
                            curPatientcollect_payment = 0;
                            DataRow[] PatIns = dtEasyDentalPatientIns_data.Copy().Select("InsuredId = '" + dr["Primary_Insurance"] + "' and patient_id = '" + dr["Patient_EHR_ID"] + "'");
                            if (PatIns.Length > 0)
                            {
                                dr["Primary_Insurance"] = PatIns[0]["InsId"].ToString().Trim();
                                dr["Primary_Insurance_CompanyName"] = PatIns[0]["InsCoName"].ToString().Trim();
                                dr["Prim_Ins_Company_Phonenumber"] = PatIns[0]["Phone"].ToString().Trim();
                                dr["Primary_Ins_Subscriber_ID"] = PatIns[0]["IdNum"].ToString().Trim();
                                if (dr["Patient_EHR_ID"].ToString().Trim() == dr["Guar_ID"].ToString().Trim())
                                {
                                    curPatientcollect_payment = Convert.ToDecimal(PatIns[0]["PerCoverage"].ToString().Trim());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["remaining_benefit"] = curPatientcollect_payment;
                                }
                            }
                            else
                            {
                                curPatientcollect_payment = 0;
                                dr["Primary_Insurance"] = 0;
                                dr["Primary_Insurance_CompanyName"] = "";
                                dr["Primary_Ins_Subscriber_ID"] = "";
                                dr["Prim_Ins_Company_Phonenumber"] = "";
                            }
                            curPatientcollect_payment = 0;
                            PatIns = dtEasyDentalPatientIns_data.Copy().Select("InsuredId = '" + dr["Secondary_Insurance"] + "' and patient_id = '" + dr["Patient_EHR_ID"] + "'");
                            if (PatIns.Length > 0)
                            {
                                dr["Secondary_Insurance"] = PatIns[0]["InsId"].ToString().Trim();
                                dr["Secondary_Insurance_CompanyName"] = PatIns[0]["InsCoName"].ToString().Trim();
                                dr["Secondary_Ins_Subscriber_ID"] = PatIns[0]["IdNum"].ToString().Trim();
                                dr["Sec_Ins_Company_Phonenumber"] = PatIns[0]["Phone"].ToString().Trim();
                                if (dr["Patient_EHR_ID"].ToString().Trim() == dr["Guar_ID"].ToString().Trim())
                                {
                                    curPatientcollect_payment = Convert.ToDecimal(PatIns[0]["PerCoverage"].ToString().Trim());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);

                                    curPatientcollect_payment = Convert.ToDecimal(dr["remaining_benefit"].ToString()) + curPatientcollect_payment;
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["remaining_benefit"] = curPatientcollect_payment;
                                }
                            }
                            else
                            {
                                dr["Secondary_Insurance"] = 0;
                                dr["Secondary_Insurance_CompanyName"] = "";
                                dr["Secondary_Ins_Subscriber_ID"] = "";
                                dr["Sec_Ins_Company_Phonenumber"] = "";
                            }
                            curPatientcollect_payment = 0;
                            DataRow[] PatAging = dtEasyDentalPatientAging_data.Copy().Select("GuaratorId = '" + dr["Patient_EHR_ID"].ToString().Trim() + "'");
                            try
                            {
                                if (PatAging.Length > 0)
                                {
                                    curPatientcollect_payment = Convert.ToDecimal(PatAging[0]["Aging0to30"].ToString());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["ThirtyDay"] = curPatientcollect_payment;
                                    curPatientcollect_payment = Convert.ToDecimal(PatAging[0]["Aging31to60"].ToString());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["SixtyDay"] = curPatientcollect_payment;
                                    curPatientcollect_payment = Convert.ToDecimal(PatAging[0]["Aging61to90"].ToString());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["NinetyDay"] = curPatientcollect_payment;
                                    curPatientcollect_payment = Convert.ToDecimal(PatAging[0]["Aging91Plus"].ToString());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["Over90"] = curPatientcollect_payment;
                                }
                                else
                                {
                                    curPatientcollect_payment = 0;
                                    dr["ThirtyDay"] = curPatientcollect_payment.ToString();
                                    dr["SixtyDay"] = curPatientcollect_payment.ToString();
                                    dr["NinetyDay"] = curPatientcollect_payment.ToString();
                                    dr["Over90"] = curPatientcollect_payment.ToString();
                                }
                            }
                            catch
                            {
                                curPatientcollect_payment = 0;
                                dr["ThirtyDay"] = curPatientcollect_payment.ToString();
                                dr["SixtyDay"] = curPatientcollect_payment.ToString();
                                dr["NinetyDay"] = curPatientcollect_payment.ToString();
                                dr["Over90"] = curPatientcollect_payment.ToString();
                            }

                            curPatientcollect_payment = 0;
                            dr["CurrentBal"] = Convert.ToDecimal(dr["ThirtyDay"].ToString()) + Convert.ToDecimal(dr["SixtyDay"].ToString()) + Convert.ToDecimal(dr["NinetyDay"].ToString()) + Convert.ToDecimal(dr["Over90"].ToString());
                            DataRow[] UsedBen = dtEasyDentalPatient.Copy().Select("Guar_ID = '" + dr["Patient_EHR_ID"] + "'");
                            if (UsedBen.Length > 1)
                            {
                                decimal used_benefit = 0;
                                foreach (DataRow ur in UsedBen)
                                {
                                    used_benefit = used_benefit + Convert.ToDecimal(ur["used_benefit"].ToString());
                                }
                                curPatientcollect_payment = decimal.Round(used_benefit, 2, MidpointRounding.AwayFromZero);
                                curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                dr["used_benefit"] = curPatientcollect_payment;
                            }
                            curPatientcollect_payment = Convert.ToDecimal(dr["remaining_benefit"]) - Convert.ToDecimal(dr["used_benefit"]);
                            curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                            curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                            dr["remaining_benefit"] = curPatientcollect_payment;
                            curPatientcollect_payment = 0;
                            if (dr["Primary_Insurance_CompanyName"].ToString() == "" && dr["Secondary_Insurance_CompanyName"].ToString() == "")
                            {
                                dr["used_benefit"] = curPatientcollect_payment.ToString();
                                dr["remaining_benefit"] = curPatientcollect_payment.ToString();
                            }

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        DataRow drPatientNote = dtEasyDentalPatientNote.Copy().Select("NoteID = '" + dr["Patient_EHR_ID"].ToString() + "'").FirstOrDefault();
                        if (drPatientNote != null)
                        {
                            dr["Patient_Note"] = drPatientNote["Text"].ToString();
                        }
                        DataRow drPatientEmployer = dtEasyDentalEmployer.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"].ToString() + "'").FirstOrDefault();
                        if (drPatientEmployer != null)
                        {
                            dr["employer"] = drPatientEmployer["employer"].ToString();
                        }
                        DataRow drPatientResponsibleParty = dtEasyDentalResponsibleParty.Copy().Select("responsiblepartyId = '" + dr["Guar_ID"].ToString() + "'").FirstOrDefault();
                        if (drPatientResponsibleParty != null)
                        {
                            dr["responsiblepartyId"] = drPatientResponsibleParty["responsiblepartyId"].ToString();
                            dr["ResponsibleParty_First_Name"] = drPatientResponsibleParty["ResponsibleParty_First_Name"].ToString();
                            dr["ResponsibleParty_Last_Name"] = drPatientResponsibleParty["ResponsibleParty_Last_Name"].ToString();
                            dr["responsiblepartyssn"] = drPatientResponsibleParty["responsiblepartyssn"].ToString();
                            dr["responsiblepartybirthdate"] = drPatientResponsibleParty["responsiblepartybirthdate"].ToString();
                        }
                        if (Convert.ToString(dr["sex"]) == "M")
                        {
                            dr["sex"] = "Male";
                        }
                        else if (Convert.ToString(dr["sex"]) == "F")
                        {
                            dr["sex"] = "Female";
                        }
                        else
                        {
                            dr["sex"] = "Unknown";
                        }
                        switch (Convert.ToInt32(dr["Status"].ToString()))
                        {
                            case 1:
                                dr["EHR_Status"] = "Active";
                                break;
                            case 2:
                                dr["EHR_Status"] = "NonPatient";
                                break;
                            case 3:
                                dr["EHR_Status"] = "InActive";
                                break;
                            case 4:
                                dr["EHR_Status"] = "InActive";
                                break;
                            default:
                                dr["EHR_Status"] = "InActive";
                                break;
                        }
                        if (dr["InsUptDlt"].ToString() == "")
                        {
                            dr["InsUptDlt"] = "0";
                        }
                        if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                        {
                            ExecuteQuery(InsertTableName, dr);
                        }
                    }
                }


                //if (bSetDeleted)
                //{
                //    string PatientEHRIDs = string.Join("','", dtEasyDentalPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));
                //    if (PatientEHRIDs != string.Empty)
                //    {
                //        PatientEHRIDs = "'" + PatientEHRIDs + "'";

                //        if (conn.State == ConnectionState.Closed) conn.Open();
                //        string SqlCeSelect = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                //        SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDs", PatientEHRIDs);
                //        SqlCeCommand.CommandText = SqlCeSelect;
                //        SqlCeCommand.ExecuteNonQuery();
                //    }
                //}

                if (bSetDeleted)
                {
                    IEnumerable<string> PatientEHRIDs = dtEasyDentalPatient.AsEnumerable().Where(sid => sid["Service_Install_Id"].ToString() == "1").Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
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

                                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                                {
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    string SqlCeSelect = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                                    SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDs", DeletedEHRIDs);
                                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                    {
                                        SqlCeCommand.CommandType = CommandType.Text;
                                        SqlCeCommand.CommandText = SqlCeSelect;
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
                #region Get Records from PatientCompareTAble
                if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                {
                    DataTable dtPatientCompareRec = new DataTable();
                    using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                    {
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
                        }
                        foreach (DataRow drRow in dtPatientCompareRec.Rows)
                        {
                            ExecuteQuery("Patient", drRow);
                        }
                        //if (conn.State == ConnectionState.Closed) conn.Open();
                        //CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
                        SqlCeSelect = "Delete from PatientCompare where Service_Install_Id = @Service_Install_Id ";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.Add("Service_Install_Id", "1");
                            SqlCeCommand.ExecuteNonQuery();
                        }
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }

                }
                #endregion

            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                // SqlCetx.Rollback();
                throw new Exception(ex.Message);
            }

            return _successfullstataus;
        }

        public static bool Save_ApptPatient_EasyDental_To_Local(DataTable dtEasyDentalPatient, string InsertTableName, DataTable dtEasyDentalPatientNextApptDate, DataTable dtEasyDentalPatientdue_date)
        {
            bool _successfullstataus = true;
            DataTable dtEasyDentalPatientcollect_payment = new DataTable();

            string sqlSelect = string.Empty;

            DataTable dtEasyDentalPatientIns_data = GetEasyDentalInsuranceData();
            DataTable dtEasyDentalPatientAging_data = GetEasyDentalAgingData();
            try
            {
                if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                {
                    using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = "Delete from PatientCompare where Service_Install_Id = 1";
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                }

                for (int j = 0; j < dtEasyDentalPatient.Rows.Count; j++)
                {
                    DataRow dr = dtEasyDentalPatient.Rows[j];
                    //###
                    if (dr != null)
                    {
                        string Status = string.Empty;
                        string tmpBirthDate = string.Empty;
                        string tmpFirstVisit_Date = string.Empty;
                        string tmpLastVisit_Date = string.Empty;
                        string tmpnextvisit_date = string.Empty;
                        string tmpdue_date = string.Empty;
                        string tmpReceive_Sms_Email = string.Empty;
                        string tmpReceiveVoiceCall = "Y";
                        decimal curPatientcollect_payment = 0;
                        int tmpprivacy_flags = 0;


                        tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());
                        tmpFirstVisit_Date = Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim());
                        tmpLastVisit_Date = Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim());

                        tmpReceive_Sms_Email = "Y";
                        //  case when privacyflags in (1,3,5,7) then 'N' else 'Y' end as f For ReceiveVoiceCall
                        tmpprivacy_flags = Convert.ToInt32(dr["privacy_flags"].ToString());

                        if (tmpprivacy_flags == 2 || tmpprivacy_flags == 3 || tmpprivacy_flags == 6 || tmpprivacy_flags == 7)
                        {
                            tmpReceive_Sms_Email = "N";
                        }
                        if (tmpprivacy_flags == 1 || tmpprivacy_flags == 3 || tmpprivacy_flags == 5 || tmpprivacy_flags == 7)
                        {
                            tmpReceiveVoiceCall = "N";
                        }
                        dr["ReceiveSMS"] = tmpReceive_Sms_Email.ToString();
                        dr["ReceiveEmail"] = tmpReceive_Sms_Email.ToString();
                        dr["ReceiveVoiceCall"] = tmpReceiveVoiceCall.ToString();

                        // https://app.asana.com/0/751059797849097/1149506260330945
                        dr["nextvisit_date"] = Utility.SetNextVisitDate(dtEasyDentalPatientNextApptDate, "Patient_EHR_ID", "Patient_EHR_ID", "nextvisit_date", dr["Patient_EHR_ID"].ToString());
                        tmpnextvisit_date = dr["nextvisit_date"].ToString();

                        try
                        {
                            dtEasyDentalPatientcollect_payment = SynchEasyDentalDAL.GetEasyDentalPatientcollect_payment(dr["Patient_EHR_ID"].ToString().Trim());
                            //   DataRow[] drPatientcollect_payment = dtEasyDentalPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");

                            if (dtEasyDentalPatientcollect_payment.Rows.Count > 0)
                            {
                                curPatientcollect_payment = Convert.ToDecimal(dtEasyDentalPatientcollect_payment.Rows[0]["collect_payment"].ToString());
                                curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                dr["collect_payment"] = curPatientcollect_payment.ToString();
                            }
                            else
                            {
                                curPatientcollect_payment = 0;
                                dr["collect_payment"] = curPatientcollect_payment.ToString();
                            }
                        }
                        catch (Exception)
                        {
                            curPatientcollect_payment = 0;
                            dr["collect_payment"] = curPatientcollect_payment.ToString();
                        }
                        try
                        {

                            DataRow[] Patdue_date = dtEasyDentalPatientdue_date.Copy().Select("patient_id = '" + dr["Patient_EHR_ID"] + "'");
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
                                        tmpdue_date = Convert.ToDateTime(SortPatdue_date.Rows[i]["due_date"].ToString()).ToString("MM/dd/yyyy HH:mm:ss") + "@" + SortPatdue_date.Rows[i]["recall_type"].ToString().Trim() + "@" + SortPatdue_date.Rows[i]["recall_type_id"].ToString() + "|" + tmpdue_date;
                                    }
                                    dr["due_date"] = tmpdue_date;
                                }
                                else
                                {
                                    foreach (DataRow due_row in Patdue_date)
                                    {
                                        tmpdue_date = Convert.ToDateTime(due_row["due_date"].ToString()).ToString("MM/dd/yyyy HH:mm:ss") + "@" + due_row["recall_type"].ToString().Trim() + "@" + due_row["recall_type_id"].ToString() + "|" + tmpdue_date;
                                    }
                                    dr["due_date"] = tmpdue_date;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            tmpdue_date = string.Empty;
                        }

                        try
                        {
                            curPatientcollect_payment = 0;
                            DataRow[] PatIns = dtEasyDentalPatientIns_data.Copy().Select("InsuredId = '" + dr["Primary_Insurance"] + "' and patient_id = '" + dr["Patient_EHR_ID"] + "'");
                            if (PatIns.Length > 0)
                            {
                                dr["Primary_Insurance"] = PatIns[0]["InsId"].ToString().Trim();
                                dr["Primary_Insurance_CompanyName"] = PatIns[0]["InsCoName"].ToString().Trim();
                                dr["Primary_Ins_Subscriber_ID"] = PatIns[0]["IdNum"].ToString().Trim();
                                if (dr["Patient_EHR_ID"].ToString().Trim() == dr["Guar_ID"].ToString().Trim())
                                {
                                    curPatientcollect_payment = Convert.ToDecimal(PatIns[0]["PerCoverage"].ToString().Trim());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["remaining_benefit"] = curPatientcollect_payment;
                                }
                            }
                            else
                            {
                                curPatientcollect_payment = 0;
                                dr["Primary_Insurance"] = 0;
                                dr["Primary_Insurance_CompanyName"] = "";
                                dr["Primary_Ins_Subscriber_ID"] = "";
                            }
                            curPatientcollect_payment = 0;
                            PatIns = dtEasyDentalPatientIns_data.Copy().Select("InsuredId = '" + dr["Secondary_Insurance"] + "' and patient_id = '" + dr["Patient_EHR_ID"] + "'");
                            if (PatIns.Length > 0)
                            {
                                dr["Secondary_Insurance"] = PatIns[0]["InsId"].ToString().Trim();
                                dr["Secondary_Insurance_CompanyName"] = PatIns[0]["InsCoName"].ToString().Trim();
                                dr["Secondary_Ins_Subscriber_ID"] = PatIns[0]["IdNum"].ToString().Trim();
                                if (dr["Patient_EHR_ID"].ToString().Trim() == dr["Guar_ID"].ToString().Trim())
                                {
                                    curPatientcollect_payment = Convert.ToDecimal(PatIns[0]["PerCoverage"].ToString().Trim());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);

                                    curPatientcollect_payment = Convert.ToDecimal(dr["remaining_benefit"].ToString()) + curPatientcollect_payment;
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["remaining_benefit"] = curPatientcollect_payment;
                                }
                            }
                            else
                            {
                                dr["Secondary_Insurance"] = 0;
                                dr["Secondary_Insurance_CompanyName"] = "";
                                dr["Secondary_Ins_Subscriber_ID"] = "";
                            }
                            curPatientcollect_payment = 0;
                            DataRow[] PatAging = dtEasyDentalPatientAging_data.Copy().Select("GuaratorId = '" + dr["Patient_EHR_ID"].ToString().Trim() + "'");
                            try
                            {
                                if (PatAging.Length > 0)
                                {
                                    curPatientcollect_payment = Convert.ToDecimal(PatAging[0]["Aging0to30"].ToString());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["ThirtyDay"] = curPatientcollect_payment;
                                    curPatientcollect_payment = Convert.ToDecimal(PatAging[0]["Aging31to60"].ToString());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["SixtyDay"] = curPatientcollect_payment;
                                    curPatientcollect_payment = Convert.ToDecimal(PatAging[0]["Aging61to90"].ToString());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["NinetyDay"] = curPatientcollect_payment;
                                    curPatientcollect_payment = Convert.ToDecimal(PatAging[0]["Aging91Plus"].ToString());
                                    curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                                    curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                    dr["Over90"] = curPatientcollect_payment;
                                }
                                else
                                {
                                    curPatientcollect_payment = 0;
                                    dr["ThirtyDay"] = curPatientcollect_payment.ToString();
                                    dr["SixtyDay"] = curPatientcollect_payment.ToString();
                                    dr["NinetyDay"] = curPatientcollect_payment.ToString();
                                    dr["Over90"] = curPatientcollect_payment.ToString();
                                }
                            }
                            catch
                            {
                                curPatientcollect_payment = 0;
                                dr["ThirtyDay"] = curPatientcollect_payment.ToString();
                                dr["SixtyDay"] = curPatientcollect_payment.ToString();
                                dr["NinetyDay"] = curPatientcollect_payment.ToString();
                                dr["Over90"] = curPatientcollect_payment.ToString();
                            }

                            curPatientcollect_payment = 0;
                            dr["CurrentBal"] = Convert.ToDecimal(dr["ThirtyDay"].ToString()) + Convert.ToDecimal(dr["SixtyDay"].ToString()) + Convert.ToDecimal(dr["NinetyDay"].ToString()) + Convert.ToDecimal(dr["Over90"].ToString());
                            DataRow[] UsedBen = dtEasyDentalPatient.Copy().Select("Guar_ID = '" + dr["Patient_EHR_ID"] + "'");
                            if (UsedBen.Length > 1)
                            {
                                decimal used_benefit = 0;
                                foreach (DataRow ur in UsedBen)
                                {
                                    used_benefit = used_benefit + Convert.ToDecimal(ur["used_benefit"].ToString());
                                }
                                curPatientcollect_payment = decimal.Round(used_benefit, 2, MidpointRounding.AwayFromZero);
                                curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                                dr["used_benefit"] = curPatientcollect_payment;
                            }
                            curPatientcollect_payment = Convert.ToDecimal(dr["remaining_benefit"]) - Convert.ToDecimal(dr["used_benefit"]);
                            curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
                            curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
                            dr["remaining_benefit"] = curPatientcollect_payment;
                            curPatientcollect_payment = 0;
                            if (dr["Primary_Insurance_CompanyName"].ToString() == "" && dr["Secondary_Insurance_CompanyName"].ToString() == "")
                            {
                                dr["used_benefit"] = curPatientcollect_payment.ToString();
                                dr["remaining_benefit"] = curPatientcollect_payment.ToString();
                            }

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        if (Convert.ToString(dr["sex"]) == "M")
                        {
                            dr["sex"] = "Male";
                        }
                        else if (Convert.ToString(dr["sex"]) == "F")
                        {
                            dr["sex"] = "Female";
                        }
                        else
                        {
                            dr["sex"] = "Unknown";
                        }
                        if (dr["InsUptDlt"].ToString() == "")
                        {
                            dr["InsUptDlt"] = "0";
                        }
                        if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                        {
                            ExecuteQuery(InsertTableName, dr);
                        }
                    }
                }

                #region Deleted
                //if (bSetDeleted)
                //{
                //    string PatientEHRIDs = string.Join("','", dtEasyDentalPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));
                //    if (PatientEHRIDs != string.Empty)
                //    {
                //        PatientEHRIDs = "'" + PatientEHRIDs + "'";

                //        if (conn.State == ConnectionState.Closed) conn.Open();
                //        string SqlCeSelect = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                //        SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDs", PatientEHRIDs);
                //        SqlCeCommand.CommandText = SqlCeSelect;
                //        SqlCeCommand.ExecuteNonQuery();
                //    }
                //}                
                #endregion

            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                // SqlCetx.Rollback();
                throw new Exception(ex.Message);
            }
            return _successfullstataus;
        }


        public static void ExecuteQuery(string InsertTableName, DataRow dr)
        {
            try
            {
                string sqlSelect = string.Empty;
                string MaritalStatus = string.Empty;
                string Status = string.Empty;
                string EHR_Status = string.Empty;
                string tmpBirthDate = string.Empty;
                // decimal curPatientcollect_payment = 0;
                //string tmpReceive_Sms_Email = string.Empty;
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
                                try
                                {
                                    Status = dr["Status"].ToString().Trim();
                                }
                                catch (Exception)
                                { Status = ""; }


                                if (Status == "1")
                                { EHR_Status = "Active"; }
                                else if (Status == "2")
                                { EHR_Status = "NonPatient"; }
                                else if (Status == "3")
                                { EHR_Status = "InActive"; }
                                else if (Status == "4")
                                { EHR_Status = "InActive"; }
                                else
                                {
                                    EHR_Status = "InActive";
                                }
                                if (Status == "1")
                                { Status = "A"; }
                                else
                                { Status = "I"; }



                                if (dr["MaritalStatus"].GetType() != typeof(string))
                                {
                                    if (Convert.ToInt32(dr["MaritalStatus"]) == 1)
                                    {
                                        MaritalStatus = "Single";
                                    }
                                    else if (Convert.ToInt32(dr["MaritalStatus"]) == 2)
                                    {
                                        MaritalStatus = "Married";
                                    }
                                    else if (Convert.ToInt32(dr["MaritalStatus"]) == 3)
                                    {
                                        MaritalStatus = "Child";
                                    }
                                    else if (Convert.ToInt32(dr["MaritalStatus"]) == 4)
                                    {
                                        MaritalStatus = "Other";
                                    }
                                    else
                                    {
                                        MaritalStatus = "Single";
                                    }
                                }
                                if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                                {
                                    SqlCeCommand.CommandText = SqlCeCommand.CommandText.Replace("INSERT INTO Patient", "INSERT INTO PatientCompare");
                                }
                                break;
                            case 2:
                                try
                                {
                                    Status = dr["Status"].ToString().Trim();
                                    EHR_Status = dr["EHR_Status"].ToString().Trim();
                                    MaritalStatus = dr["MaritalStatus"].ToString().Trim();
                                }
                                catch (Exception)
                                { Status = ""; }
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
                            tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy HH:mm:ss");
                        }




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
                        SqlCeCommand.Parameters.AddWithValue("MaritalStatus", MaritalStatus.ToString().Trim());
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
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
                        SqlCeCommand.Parameters.AddWithValue("EHR_Status", EHR_Status);
                        SqlCeCommand.Parameters.AddWithValue("ReceiveVoiceCall", dr["ReceiveVoiceCall"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("PreferredLanguage", dr["Patient_Note"].ToString().ToUpper().Contains("SPANISH") ? "SPENISH" : (dr["Patient_Note"].ToString().ToUpper().Contains("FRENCH") ? "FRENCH" : "ENGLISH"));
                        SqlCeCommand.Parameters.AddWithValue("Patient_Note", dr["Patient_Note"].ToString().Trim().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).Trim() : dr["Patient_Note"].ToString().Trim());

                        #region New Patient Form Fields
                        SqlCeCommand.Parameters.AddWithValue("ssn", dr["ssn"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("driverlicense", dr["driverlicense"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("employer", dr["employer"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("Prim_Ins_Company_Phonenumber", dr["Prim_Ins_Company_Phonenumber"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("Sec_Ins_Company_Phonenumber", dr["Sec_Ins_Company_Phonenumber"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("responsiblepartyId", dr["responsiblepartyId"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_First_Name", dr["ResponsibleParty_First_Name"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Last_Name", dr["ResponsibleParty_Last_Name"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("responsiblepartyssn", dr["responsiblepartyssn"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("responsiblepartybirthdate", dr["responsiblepartybirthdate"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("groupid", dr["groupid"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("school", dr["school"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("spouseid", dr["spouseid"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("Spouse_First_Name", dr["Spouse_First_Name"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("Spouse_Last_Name", dr["Spouse_Last_Name"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("emergencycontactid", dr["emergencycontactid"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("EmergencyContact_First_Name", dr["EmergencyContact_First_Name"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("EmergencyContact_Last_Name", dr["EmergencyContact_Last_Name"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("emergencycontactnumber", dr["emergencycontactnumber"].ToString());
                        #endregion

                        SqlCeCommand.ExecuteNonQuery();
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unique_Patient_EHR_ID"))
                {
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public static DataTable GetEasyDentalPatientNextApptDate()
        {
            try
            {
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                DateTime ToDate = dtCurrentDtTime;
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientNextApptDate.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalPatientNextApptDate(string PatientEHRIDs)
        {
            try
            {
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                DateTime ToDate = dtCurrentDtTime;
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalApptPatientNextApptDate.Trim();
                    OdbcSelect = OdbcSelect.Replace("@patientid", PatientEHRIDs);
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalPatientdue_date()
        {
            DataTable OdbcDt = new DataTable();
            try
            {
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                DateTime ToDate = dtCurrentDtTime;


                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientdue_date.Trim();

                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        //  OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddDays(-30).ToString("yyyy/MM/dd");
                        // OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddDays(-30).ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                if (OdbcDt != null)
                {
                    DataView view = new DataView(OdbcDt);
                    DataTable distinctValues = view.ToTable(true, "patient_id", "recall_type_id", "due_date", "recall_type");

                    return distinctValues;
                }
                else
                {
                    return OdbcDt;
                }
            }
            catch (Exception ex)
            {
                return OdbcDt;
            }

        }

        public static DataTable GetEasyDentalPatientdue_date(string PatientEHRIDs)
        {
            DataTable OdbcDt = new DataTable();
            try
            {
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                DateTime ToDate = dtCurrentDtTime;


                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientdue_date.Trim();
                    OdbcSelect = OdbcSelect.Replace("@patientid", PatientEHRIDs);
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddDays(-30).ToString("yyyy/MM/dd");
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                if (OdbcDt != null)
                {
                    DataView view = new DataView(OdbcDt);
                    DataTable distinctValues = view.ToTable(true, "patient_id", "recall_type_id", "due_date", "recall_type");

                    return distinctValues;
                }
                else
                {
                    return OdbcDt;
                }
            }
            catch (Exception ex)
            {
                return OdbcDt;
            }

        }

        public static DataTable GetEasyDentalPatientcollect_payment(string PatientId)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalPatientcollect_payment;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("@PatientId", PatientId);
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {

                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalPatient_RecallType()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatient_RecallType.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalPatientStatusAppointmentData(string PatientEHRIDs)
        {

            DateTime ToDate = DateTime.Now;
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = "";
                    if (PatientEHRIDs == string.Empty)
                    {
                        OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientStatusAppointmentData.Trim();
                    }
                    else
                    {
                        OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientStatusAppointmentData.Trim() + " and Patientid in (" + PatientEHRIDs + ")";
                    }
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("@ToLessDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        OdbcCommand.Parameters.Add("@ToLessTime", OdbcType.Time).Value = new TimeSpan(24, 0, 0);
                        OdbcCommand.Parameters.Add("@ToEqualDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        OdbcCommand.Parameters.Add("@ToEqualTime", OdbcType.Time).Value = new TimeSpan(ToDate.Hour, ToDate.Minute, ToDate.Second);
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region  Disease

        public static DataTable GetEasyDentalDiseaseData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalDiseaseData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public static bool Save_Disease_EasyDental_To_Local(DataTable dtEasyDentalDisease)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtEasyDentalDisease.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Disease_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Disease_Name", dr["Disease_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        #endregion

        

        #region RecallType

        public static DataTable GetEasyDentalRecallTypeData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalRecallTypeData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public static bool Save_RecallType_EasyDental_To_Local(DataTable dtEasyDentalRecallType)
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
                        foreach (DataRow dr in dtEasyDentalRecallType.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }
        #endregion

        #region User

        public static DataTable GetEasyDentalUserData()
        {
            try
            {
                string id = GetOrCreateUserId();
                Utility.WriteToErrorLogFromAll("userid = " + id);
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalUserData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public static bool Save_User_EasyDental_To_Local(DataTable dtEasyDentalUser)
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
                        foreach (DataRow dr in dtEasyDentalUser.Rows)
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
                                    SqlCeCommand.Parameters.AddWithValue("Is_Active", Convert.ToInt32(dr["Is_Active"]));// == 0 ? 1 : 0);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Deleted", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", 1);
                                }
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    Utility.WriteToErrorLogFromAll("Error In User" + ex.Message);
                    _successfullstataus = false;

                    //SqlCetx.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        public static string GetOrCreateUserId()
        {
            OdbcConnection conn = null;
            CommonDB.OdbcConnectionServer(ref conn);
            Utility.WriteToErrorLogFromAll("In Getorcreateuser");
            string UserId = string.Empty;

            try
            {
                OdbcCommand OdbCommand = new OdbcCommand();
                OdbcDataReader OdbcRead;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string strQauery = SynchEasyDentalQRY.CheckAditUserExist;
                Utility.WriteToErrorLogFromAll("CheckAditUserExist  " + strQauery);
                Utility.WriteToErrorLogFromAll("1");
                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                OdbCommand.CommandTimeout = 200;
                OdbcRead = OdbCommand.ExecuteReader();

                if (OdbcRead.HasRows)
                    UserId = Convert.ToString(OdbcRead["RscId"]);
                Utility.WriteToErrorLogFromAll("UserId =" + UserId);

                OdbcRead.Close();
                OdbcRead.Dispose();
                OdbCommand.Dispose();
                Utility.WriteToErrorLogFromAll("3");
                if (UserId == string.Empty || UserId == "")
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string strQauery1 = SynchEasyDentalQRY.InsertAditUser;
                    Utility.WriteToErrorLogFromAll("InsertAditUser =" + strQauery1);
                    CommonDB.OdbcCommandServer(strQauery1, conn, ref OdbCommand, "txt");
                    OdbCommand.CommandTimeout = 200;
                    Utility.WriteToErrorLogFromAll("4");
                    OdbCommand.ExecuteNonQuery();

                    strQauery = " SELECT RscId FROM RscProv_dat where RscId='ADIT' ";

                    CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                    OdbCommand.CommandTimeout = 200;

                    UserId = Convert.ToString(OdbCommand.ExecuteScalar());

                    //OdbCommand.Dispose();
                    //if (OdbCommand.ExecuteNonQuery() == 1)
                    //{
                    //    UserId = "ADIT";
                    //    Utility.WriteToErrorLogFromAll("5 if");
                    //}
                    //else
                    //{
                    //    UserId = "ADIT";
                    //    Utility.WriteToErrorLogFromAll("5 else");
                    //}                   

                    OdbcRead.Close();
                    OdbcRead.Dispose();
                    OdbCommand.Dispose();
                    Utility.WriteToErrorLogFromAll("6");
                }

            }

            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("error " + ex.Message);
                throw ex;
            }

            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

            return UserId;
        }


        #endregion

        #region ApptStatus

        public static DataTable GetEasyDentalApptStatusData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalApptStatusData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool Save_ApptStatus_EasyDental_To_Local(DataTable dtEasyDentalApptStatus)
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
                        foreach (DataRow dr in dtEasyDentalApptStatus.Rows)
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }
        #endregion

        #region Speciality

        public static DataTable GetEasyDentalSpecialityData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalSpecialityData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static bool Save_Speciality_EasyDental_To_Local(DataTable dtEasyDentalSpeciality)
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
                        foreach (DataRow dr in dtEasyDentalSpeciality.Rows)
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region SqlServer

        public static bool Save_Appointment_EasyDental_To_Local_SqlServer(DataTable dtEasyDentalAppointment)
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

                foreach (DataRow dr in dtEasyDentalAppointment.Rows)
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
                conn.Dispose();
            }
            return _successfullstataus;
        }

        public static bool Save_OperatoryEvent_EasyDental_To_Local_SqlServer(DataTable dtEasyDentalOperatoryEvent)
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

                foreach (DataRow dr in dtEasyDentalOperatoryEvent.Rows)
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
                conn.Dispose();
            }
            return _successfullstataus;
        }

        public static bool Save_Provider_EasyDental_To_Local_SqlServer(DataTable dtEasyDentalProvider)
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
                foreach (DataRow dr in dtEasyDentalProvider.Rows)
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return _successfullstataus;
        }

        public static bool Save_Speciality_EasyDental_To_Local_SqlServer(DataTable dtEasyDentalSpeciality)
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
                foreach (DataRow dr in dtEasyDentalSpeciality.Rows)
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return _successfullstataus;
        }

        public static bool Save_Operatory_EasyDental_To_Local_SqlServer(DataTable dtEasyDentalOperatory)
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
                foreach (DataRow dr in dtEasyDentalOperatory.Rows)
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return _successfullstataus;
        }

        public static bool Save_Patient_EasyDental_To_Local_SqlServer(DataTable dtEasyDentalPatient)
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
                foreach (DataRow dr in dtEasyDentalPatient.Rows)
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return _successfullstataus;
        }

        public static bool Save_ApptType_EasyDental_To_Local_SqlServer(DataTable dtEasyDentalApptType)
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
                foreach (DataRow dr in dtEasyDentalApptType.Rows)
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return _successfullstataus;
        }

        public static bool Save_ApptStatus_EasyDental_To_Local_SqlServer(DataTable dtEasyDentalApptStatus)
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
                foreach (DataRow dr in dtEasyDentalApptStatus.Rows)
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return _successfullstataus;
        }

        public static bool Save_RecallType_EasyDental_To_Local_SqlServer(DataTable dtEasyDentalRecallType)
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
                foreach (DataRow dr in dtEasyDentalRecallType.Rows)
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return _successfullstataus;
        }

        public static bool Update_DeletedAppointment_EasyDental_To_Local_SqlServer(DataTable dtEasyDentalDeletedAppointment)
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
                foreach (DataRow dr in dtEasyDentalDeletedAppointment.Rows)
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
                conn.Dispose();
            }
            return _successfullstataus;
        }

        #endregion

        #region  Holidays

        public static DataTable GetEasyDentalHolidaysData()
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
                string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalHolidayData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.Add("@FromDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                OdbcDt.DefaultView.RowFilter = "closed_flag in (2,3,4)";
                return OdbcDt.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
        }

        public static DataTable GetEasyDentalOperatoryHolidaysData(DataTable dtOperatory)
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
                    string OdbcSelect = SynchEasyDentalQRY.GetEasyDentalOperatoryHolidaysData;
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.Parameters.Clear();
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
        }

        public static bool Save_Holidays_EasyDental_To_Local(DataTable dtEasyDentalHoliday)
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
                        foreach (DataRow dr in dtEasyDentalHoliday.Rows)
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        public static bool Save_Opeatory_Holidays_EasyDental_To_Local(DataTable dtEasyDentalHoliday)
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
                        foreach (DataRow dr in dtEasyDentalHoliday.Rows)
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
            return _successfullstataus;
        }

        public static int Save_Patient_Local_To_EasyDental(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, int tmpPatient_Gur_id, string Birth_Date)
        {
            int PatientId = 0;
            OdbcConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            OdbcCommand OdbcCommand = null;
            CommonDB.OdbcConnectionServer(ref conn);
            
            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                int AddressID = GetEasyDentalAddressId(tmpPatient_Gur_id);
                if (LastName.Length == 0)
                {
                    LastName += "NA";
                }
                if (LastName.Length == 1)
                {
                    LastName += "0";
                }
                int ChartNumber = GetEasyDentalChartNum(LastName.Substring(0, 2).ToString());

                string patBirthDate = "";
                try
                {
                    if (Birth_Date != "")
                    {
                        patBirthDate = Convert.ToDateTime(Birth_Date.ToString()).ToString("yyyy-MM-dd");
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
                    OdbcCommand.CommandText = SynchEasyDentalQRY.InsertPatientDetails;

                }
                else
                {
                    OdbcCommand.CommandText = SynchEasyDentalQRY.InsertPatientDetails_With_Birthdate;
                }
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("LastName", LastName);
                OdbcCommand.Parameters.AddWithValue("FirstName", FirstName);
                OdbcCommand.Parameters.AddWithValue("MI", MiddleName);
                OdbcCommand.Parameters.AddWithValue("PrefName", "");
                if (ChartNumber != 0)
                {
                    if (Utility.ChartNumberIsNumeric)
                    {
                        OdbcCommand.Parameters.AddWithValue("ChartNum", ChartNumber.ToString("000000"));
                    }
                    else
                    {
                        OdbcCommand.Parameters.AddWithValue("ChartNum", LastName.Substring(0, 2).ToString().ToUpper() + ChartNumber.ToString("0000"));
                    }
                }
                else
                {
                    OdbcCommand.Parameters.AddWithValue("chartnum", DBNull.Value);
                }
                OdbcCommand.Parameters.AddWithValue("ProvId1", PriProv);
                OdbcCommand.Parameters.AddWithValue("IsGuarantor", 0);
                OdbcCommand.Parameters.AddWithValue("Gender", "M");
                OdbcCommand.Parameters.AddWithValue("Status", 1);
                OdbcCommand.Parameters.AddWithValue("FamPos", 1);
                OdbcCommand.Parameters.AddWithValue("AddressId", AddressID);
                OdbcCommand.Parameters.AddWithValue("Email", Email);
                OdbcCommand.Parameters.AddWithValue("Pager", Mobile);
                OdbcCommand.Parameters.AddWithValue("GuidId", Guid.NewGuid().ToString());
                OdbcCommand.Parameters.Add("FirstVisit", OdbcType.Date).Value = Convert.ToDateTime(DateFirstVisit).ToString("yyyy-MM-dd");
                // OdbcCommand.Parameters.Add("FirstVisit", OdbcType.Date).Value = DBNull.Value;
                if (patBirthDate != "")
                {
                    OdbcCommand.Parameters.Add("birthdate", OdbcType.Date).Value = Convert.ToDateTime(patBirthDate).ToString("yyyy-MM-dd");
                    //OdbcCommand.Parameters.AddWithValue("birthdate", Convert.ToDateTime(patBirthDate));
                }
                OdbcCommand.ExecuteNonQuery();
                string QryIdentity = " Select max(patid) as newId from Pat_dat where firstname = '" + FirstName + "' and lastname = '" + LastName + "' and pager = '" + SetFormatePhoneNumber(Mobile) + "'";//"Select @@Identity as newId from patient";
                OdbcCommand.CommandText = QryIdentity;
                OdbcCommand.CommandType = CommandType.Text;
                OdbcCommand.Connection = conn;
                PatientId = Convert.ToInt32(OdbcCommand.ExecuteScalar());

                OdbcCommand.CommandText = SynchEasyDentalQRY.UpdatePatientGuarantorID;
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return PatientId;
        }

        public static int GetEasyDentalChartNum(string LastName)
        {
            string Chartnum = "";
            int chartnum = 0;
            try
            {

                if (LastName != "")
                {
                    string sqlSelect = string.Empty;

                    string QryIdentity = "";
                    if (Utility.ChartNumberIsNumeric)
                    {
                        using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            QryIdentity = " SELECT chartnum as chartnum from Pat_dat";
                            using (OdbcCommand OdbcCommand = new OdbcCommand(QryIdentity, conn))
                            {
                                OdbcCommand.CommandType = CommandType.Text;
                                OdbcCommand.Parameters.Clear();
                                DataTable dt = new DataTable();
                                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                                {
                                    OdbcDa.Fill(dt);
                                }
                                var numbersonly = dt.AsEnumerable().Where(x => Regex.IsMatch(x.Field<string>("chartnum").Trim(), @"^\d{1,}$")).ToList();
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

                                Chartnum = (ChartList.Max()).ToString();
                            }
                        }
                    }
                    else
                    {
                        using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            QryIdentity = " SELECT max(chartnum) from Pat_dat where chartnum like '" + LastName.ToUpper() + "%%%%'";
                            using (OdbcCommand OdbcCommand = new OdbcCommand(QryIdentity, conn))
                            {
                                OdbcCommand.CommandType = CommandType.Text;
                                OdbcCommand.Parameters.Clear();
                                DataTable dt = new DataTable();
                                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                                {
                                    OdbcDa.Fill(dt);
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    Chartnum = dt.Rows[0][0].ToString();
                                }
                                else
                                {
                                    Chartnum = "";
                                }
                            }
                        }
                    }
                    if (Chartnum == "")
                    {
                        chartnum = 1;
                        //return chartnum;
                    }
                    else
                    {
                        if (Utility.ChartNumberIsNumeric)
                        {
                            chartnum = Convert.ToInt32(Chartnum.ToString()) + 1;
                        }
                        else
                        {
                            chartnum = Convert.ToInt16(Chartnum.ToString().Trim().Substring(2)) + 1;
                        }
                        //return chartnum + 1;
                    }
                }
                else
                {
                    chartnum = 0;
                }

            }
            catch (Exception ex)
            {
                chartnum = 0;
                throw new Exception(ex.Message);
            }
            return chartnum;
        }

        public static int GetEasyDentalAddressId(int PatientId)
        {
            int Address_Id = 0;
            try
            {
                if (PatientId == 0)
                {
                    try
                    {
                        DataTable OdbcDt = new DataTable();
                        using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            string OdbcSelect = " " + SynchEasyDentalQRY.InsertPatientAddress.Trim();
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandType = CommandType.Text;
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.ExecuteNonQuery();
                            }
                            string QryIdentity = " Select max(addressid) as newId from Address_dat";
                            using (OdbcCommand OdbcCommand = new OdbcCommand(QryIdentity, conn))
                            {
                                OdbcCommand.CommandType = CommandType.Text;
                                OdbcCommand.Parameters.Clear();
                                Address_Id = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        string QryIdentity = " Select Addressid as newId from Pat_dat where patid = " + PatientId;
                        using (OdbcCommand OdbcCommand = new OdbcCommand(QryIdentity, conn))
                        {
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Parameters.Clear();
                            Address_Id = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Address_Id = 0;
                throw new Exception(ex.Message);
            }
            return Address_Id;
        }

        public static string SetFormatePhoneNumber(string Phonenumber)
        {
            try
            {
                if (Phonenumber.Length != 10)
                {
                    return "";
                }
                if (Phonenumber.Length >= 3)
                {
                    if (Phonenumber.Length > 6 && Phonenumber.Length == 10)
                    {
                        Phonenumber = "(" + Phonenumber.Substring(0, 3) + ")" + Phonenumber.Substring(3, 3) + "-" + Phonenumber.Substring(6, 4);
                    }
                    else
                    {
                        Phonenumber = "(" + Phonenumber.Substring(0, 3) + ")" + Phonenumber.Substring(0, Phonenumber.Length - 3);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Phonenumber;
        }

        public static int Save_Appointment_Local_To_EasyDental(string patid, int length, string opid, string provid, string apptdate,
                                                                 string createdate, string appttypeid, string PatientName, string ApptComment)
        {
            int Appointment_Id = 0;            
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetOrCreateUserId();
            }
            try
            {
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string QryIdentity = " " + SynchEasyDentalQRY.InsertAppointmentDetails.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(QryIdentity, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("patientid", patid);
                        OdbcCommand.Parameters.AddWithValue("status", 0);
                        OdbcCommand.Parameters.AddWithValue("appointlen", length);
                        OdbcCommand.Parameters.AddWithValue("time", Convert.ToDateTime(createdate).Hour.ToString("00") + ":" + Convert.ToDateTime(createdate).Minute.ToString("00") + ":" + Convert.ToDateTime(createdate).Second.ToString("00"));
                        //  OdbcCommand.Parameters.AddWithValue("Confirmed", Confirmed);
                        OdbcCommand.Parameters.AddWithValue("operatoryid", opid);
                        OdbcCommand.Parameters.AddWithValue("providerid", provid);
                        OdbcCommand.Parameters.AddWithValue("date", Convert.ToDateTime(apptdate));
                        // OdbcCommand.Parameters.AddWithValue("IsNewPatient", IsNewPatient);
                        OdbcCommand.Parameters.AddWithValue("Modifiedate", Convert.ToDateTime(createdate));
                        OdbcCommand.Parameters.AddWithValue("appttype", appttypeid);
                        OdbcCommand.Parameters.AddWithValue("timeblock", 2);
                        OdbcCommand.Parameters.AddWithValue("rsctype2", 1);
                        OdbcCommand.Parameters.AddWithValue("rsctype", 3);
                        OdbcCommand.Parameters.AddWithValue("patname", PatientName);
                        OdbcCommand.Parameters.AddWithValue("rsctype3", 2);
                        OdbcCommand.Parameters.AddWithValue("staffid", Utility.EHR_UserLogin_ID);
                        Utility.WriteToErrorLogFromAll("End Qry Parameters  ");
                        OdbcCommand.ExecuteNonQuery();
                    }
                    QryIdentity = " Select max(appointmentid) as newId from appt_dat";
                    using (OdbcCommand OdbcCommand = new OdbcCommand(QryIdentity, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State != ConnectionState.Open) conn.Open();
                        Appointment_Id = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                    }
                }





                if (ApptComment.ToString().Trim() != "")
                {
                    bool isApptNote = Save_Appointment_Comment_Local_To_EasyDental(Appointment_Id.ToString(), ApptComment.ToString().Trim());

                }

            }
            catch (Exception ex)
            {
                Appointment_Id = 0;
                Utility.WriteToErrorLogFromAll("Error in appt write  =  " + ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
            }
            return Appointment_Id;
        }

        public static bool Save_Appointment_Comment_Local_To_EasyDental(string AppointmentID, string ApptComment)
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
                OdbcCommand.CommandText = SynchEasyDentalQRY.InsertAppointmentComment;
                OdbcCommand.Parameters.Clear();
                OdbcCommand.Parameters.AddWithValue("notetype", 2);
                OdbcCommand.Parameters.AddWithValue("noteid", AppointmentID);
                OdbcCommand.Parameters.AddWithValue("notetext", ApptComment.ToString());
                OdbcCommand.ExecuteNonQuery();
                isApptComment = true;
            }
            catch (Exception ex)
            {
                isApptComment = false;
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return isApptComment;
        }

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetBookOperatoryAppointmenetWiseDateTime.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.AddWithValue("ToDate", ApptDate.ToString("yyyy-MM-dd"));
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalPatientID_NameData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientID_NameData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable GetEasyDentalPatientStatusData(string PatientEHRIDs)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = "";
                    if (PatientEHRIDs == string.Empty)
                    {
                        OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientStatusData.Trim();
                    }
                    else
                    {
                        OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientStatusData.Trim() + " Where patid in (" + PatientEHRIDs + ")";
                    }
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalIdelProvider()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalIdelProvider.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool Update_Status_EHR_Appointment_Live_To_EasyDentalEHR(DataTable dtLiveAppointment,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
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

                    OdbcCommand.CommandText = SynchEasyDentalQRY.Update_Status_EHR_Appointment_Live_To_Local;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("status", confirmed_status_ehr_ID);
                    OdbcCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString());
                    OdbcCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Update Status EHR Appointment Live To EasyDentalEHR for  EHR for Appt_EHR_ID : (" + dr["Appt_EHR_ID"] + ") and status (" + confirmed_status_ehr_ID + ")");
                    bool isApptConformStatus = SynchLocalDAL.UpdateLocalAppointmentConformStatusData(dr["Appt_EHR_ID"].ToString(), dr["Service_Install_Id"].ToString(),_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
                    if(isApptConformStatus)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Update Local Appointment Conform Status for  Appt_EHR_ID=" + dr["Appt_EHR_ID"].ToString () );
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
            return _successfullstataus;
        }

        public static bool Update_Receive_SMS_Patient_EHR_Live_To_EasyDentalEHR(DataTable dtLiveAppointment,string Locationid, string Loc_ID,string _filename_EHR_patientoptout = "", string _EHRLogdirectory_EHR_patientoptout = "")
        {
            bool _successfullstataus = true;
            try
            {
                string sqlSelect = string.Empty;
                Patient_OptOutBO_StatusUpdate updatedPatientid = new Patient_OptOutBO_StatusUpdate();
                List<Patientids_OptOutBO_StatusUpdate> Patient_StatusUpdate = new List<Patientids_OptOutBO_StatusUpdate>();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.Update_Receive_SMS_Patient_EHR_Live_To_EasyDental.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.CommandText = OdbcSelect;
                        foreach (DataRow dr in dtLiveAppointment.Rows)
                        {
                            try
                            {
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.Parameters.AddWithValue("receives_sms", dr["receive_sms"].ToString() == "Y" ? 0 : 1);
                                OdbcCommand.Parameters.AddWithValue("patient_id", dr["patient_ehr_id"].ToString());
                                OdbcCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_patientoptout, _EHRLogdirectory_EHR_patientoptout, " Update Receive SMS Patient EHR Live To EasyDental for  patient_ehr_id=" + dr["patient_ehr_id"].ToString());
                                Patientids_OptOutBO_StatusUpdate Patientids = new Patientids_OptOutBO_StatusUpdate();
                                Patientids.esId = dr["esid"].ToString();
                                Patientids.patientId = dr["patientid"].ToString();
                                Patient_StatusUpdate.Add(Patientids);
                            }
                            catch (Exception)
                            {
                            }
                        }
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
                throw new Exception(ex.Message);
            }
            finally
            {
            }
            return _successfullstataus;
        }
        #endregion

        #region Patient_Form
        //public static bool Save_Patient_Form_Local_To_EasyDental(DataTable dtWebPatient_Form)
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
        //                string strQauery = SynchEasyDentalQRY.Update_Patinet_Record_By_Patient_Form;

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


        public static bool Save_Patient_Form_Local_To_EasyDental(DataTable dtWebPatient_Form)
        {
            string _successfullstataus = string.Empty;
            bool is_Record_Update = false;
            //MySqlCommand MySqlCommand = new MySqlCommand();           
            string strQauery = string.Empty;
            Int32 AddressId = 0;

            int EmpId = 0;
            int ColumnSize = 0;
            string EmpName = "";

            try
            {
                //OdbcCommand.CommandTimeout = 200;

                string Update_PatientForm_Record_ID = "";
                string phoneNo = "";
                //string ColumnList = "";
                //string ValueList = "";
                Int64 patient_EHR_Id = 0;
                DataTable dtEasyDentalPatentColumns = GetEasyDentalTableColumnName();

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
                           o.Field<object>("Patient_EHR_ID").ToString() != string.Empty && o.Field<object>("Patient_EHR_ID").ToString() != "" &&
                           o.Field<object>("PatientForm_Web_ID") != null &&
                           o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty &&
                           Convert.ToInt64(o.Field<object>("Patient_EHR_ID")) > 0
                           )
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {
                             foreach (DataRow dr in dtWebPatient_Form.Select(" PatientForm_Web_ID = '" + o.ToString() + "' "))
                             {
                                 string BirthDate = "";
                                 if (dr["ehrfield"] != null || dr["ehrfield_value"].ToString().Trim() != string.Empty)
                                 {
                                     if (dr["ehrfield"].ToString().Trim() != string.Empty)
                                     {
                                         if (Convert.ToInt32(dr["Patient_EHR_ID"].ToString()) != 0)
                                         {

                                             if (dtEasyDentalPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper()).FirstOrDefault() != null)
                                             {
                                                 if (dr["ehrfield"].ToString().ToLower() != "addressid" && dr["ehrfield"].ToString() != "patguid" && dr["ehrfield"].ToString() != "chartnum" && dr["ehrfield"].ToString().Trim().ToLower() != "primary_insurance_companyname" && dr["ehrfield"].ToString().Trim().ToLower() != "secondary_insurance_companyname" && dr["ehrfield"].ToString().Trim().ToUpper() != "PRIMARY_SUBSCRIBER_ID" && dr["ehrfield"].ToString().Trim().ToUpper() != "SECONDARY_SUBSCRIBER_ID")
                                                 {
                                                     ColumnSize = Convert.ToInt16(dtEasyDentalPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper() && r.Field<object>("ColumnSize") != null).Select(r => r.Field<object>("ColumnSize")).First().ToString());
                                                     string ColumnType = dtEasyDentalPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper() && r.Field<object>("ColumnSize") != null).Select(r => r.Field<string>("EHRDataType")).First().ToString();


                                                     patient_EHR_Id = Convert.ToInt32(dr["Patient_EHR_ID"].ToString());

                                                     #region Address
                                                     if (dr["ehrfield"].ToString().Trim().ToLower() == "street" || dr["ehrfield"].ToString().Trim().ToLower() == "street2" ||
                                                            dr["ehrfield"].ToString().Trim().ToLower() == "city" || dr["ehrfield"].ToString().Trim().ToLower() == "state" || dr["ehrfield"].ToString().Trim().ToLower() == "zip" ||
                                                            dr["ehrfield"].ToString().Trim().ToLower() == "phone")
                                                     {

                                                         Int64 Addressid = GetEasyDentalAddressId(Convert.ToInt32(dr["Patient_EHR_ID"].ToString()));
                                                         strQauery = SynchEasyDentalQRY.Update_Patinet_Address_Record_By_Patient_Form;
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
                                                     #endregion
                                                     else
                                                     {
                                                         strQauery = SynchEasyDentalQRY.Update_Patinet_Record_By_Patient_Form;
                                                         strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                                         if (dr["ehrfield_value"] != null || dr["ehrfield_value"].ToString() != "")
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
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "?");
                                                                     BirthDate = Convert.ToDateTime(dr["ehrfield_value"]).ToString("yyyy/MM/dd").ToString();
                                                                     //  strQauery = strQauery.Replace("@ehrfield_value", "'" + Convert.ToDateTime(dr["ehrfield_value"]).ToString("yyyy/MM/dd").ToString() + "'");

                                                                 }
                                                                 catch
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "NULL");

                                                                 }
                                                             }
                                                             else if (dr["ehrfield"].ToString().Trim().ToLower() == "ssn")          // SSN to be entered in specific format with hyphens
                                                             {
                                                                 string SocSecNum = dr["ehrfield_value"].ToString().Replace("-", string.Empty);

                                                                 SocSecNum = SocSecNum.Substring(0, 3) + "-" + SocSecNum.Substring(3, 2) + "-" + SocSecNum.ToString().Substring(5);
                                                                 if (SocSecNum.Length >= ColumnSize)
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + SocSecNum.Substring(0, ColumnSize) + "'");
                                                                 }

                                                                 else
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "''");
                                                                 }
                                                             }

                                                             #region Test Code
                                                             else if (dr["ehrfield"].ToString().Trim().ToLower() == "name") //|| dr["TableName"].ToString().Trim().ToLower() == "employer" || dr["ehrfield"].ToString().Trim().ToLower() == "name"
                                                             {
                                                                 strQauery = SynchEasyDentalQRY.Update_Patinet_Record_By_Patient_Form;
                                                                 ColumnSize = Convert.ToInt16(dtEasyDentalPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == "NAME" && r.Field<object>("ColumnSize") != null).Select(r => r.Field<object>("ColumnSize")).First().ToString());

                                                                 EmpName = "";

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
                                                                 strQauery = strQauery.Replace("@ehrfield_value", EmpId.ToString());

                                                             }
                                                             #endregion
                                                             else
                                                             {

                                                                 if (dr["ehrfield_value"].ToString().Trim().Length >= ColumnSize)
                                                                 {

                                                                     if (ColumnType.Trim().ToUpper() == "SYSTEM.INT16" || ColumnType.ToString().Trim().ToUpper() == "SYSTEM.INT32" || ColumnType.ToString().Trim().ToUpper() == "SYSTEM.DECIMAL")
                                                                     {
                                                                         strQauery = strQauery.Replace("@ehrfield_value", dr["ehrfield_value"].ToString().Trim().Substring(0, ColumnSize));
                                                                     }
                                                                     else
                                                                     {
                                                                         strQauery = strQauery.Replace("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim().Substring(0, ColumnSize) + "'");
                                                                     }

                                                                 }
                                                                 else
                                                                 {
                                                                     if (ColumnType.Trim().ToUpper() == "SYSTEM.INT16" || ColumnType.ToString().Trim().ToUpper() == "SYSTEM.INT32" || ColumnType.ToString().Trim().ToUpper() == "SYSTEM.DECIMAL")
                                                                     {
                                                                         strQauery = strQauery.Replace("@ehrfield_value", dr["ehrfield_value"].ToString().Trim());
                                                                     }
                                                                     else
                                                                     {
                                                                         strQauery = strQauery.Replace("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim() + "'");
                                                                     }


                                                                 }
                                                             }
                                                         }
                                                         else
                                                         {
                                                             if (ColumnType.Trim().ToUpper() == "SYSTEM.INT16" || ColumnType.ToString().Trim().ToUpper() == "SYSTEM.INT32" || ColumnType.ToString().Trim().ToUpper() == "SYSTEM.DECIMAL")
                                                             {
                                                                 strQauery = strQauery.Replace("@ehrfield_value", "0");
                                                             }
                                                             else
                                                             {
                                                                 strQauery = strQauery.Replace("@ehrfield_value", "''");
                                                             }

                                                         }
                                                         strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                                     }
                                                     //strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                                     using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                                                     {
                                                         using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                                         {
                                                             OdbcCommand.CommandType = CommandType.Text;
                                                             if (BirthDate != "")
                                                             {
                                                                 OdbcCommand.Parameters.Add("@Birthdate", OdbcType.Date).Value = Convert.ToDateTime(BirthDate).ToString("yyyy/MM/dd");
                                                             }
                                                             if (conn.State == ConnectionState.Closed) conn.Open();
                                                             OdbcCommand.ExecuteNonQuery();
                                                         }
                                                     }
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

                #region Insert Patient Into EasyDental

                patient_EHR_Id = 0;

                // DataTable dtEasyDentalPatentColumns = GetEasyDentalTableColumnName("Patient");
                string EasyDentalAddress = "";

                string LastName = "";

                //OdbcConnection conn = null;
                //OdbcCommand OdbcCommand = new OdbcCommand();
                //CommonDB.OdbcEasyDentalConnectionServer(ref conn);
                if (
                dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty).Count() > 0)
                {
                    dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {

                             #region Updating Employer Table before Patient Insert

                             foreach (DataRow dr in dtWebPatient_Form.Select(" PatientForm_Web_ID = '" + o.ToString() + "' "))
                             {
                                 if (dr["ehrfield"] != null || dr["ehrfield_value"].ToString().Trim() != string.Empty)
                                 {
                                     if (dr["ehrfield"].ToString().Trim() != string.Empty)
                                     {
                                         if (dr["TableName"].ToString() == "employer")
                                         {
                                             ColumnSize = Convert.ToInt16(dtEasyDentalPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == "NAME" && r.Field<object>("ColumnSize") != null).Select(r => r.Field<object>("ColumnSize")).First().ToString());

                                             EmpName = "";
                                             //EmpName = "Test Employer";

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

                             #endregion

                             string PrimaryInsuraceCompanyName = "";
                             string SecondaryInsuraceCompanyName = "";
                             string PrimaryInsuraceSubScriberId = "";
                             string SecondaryInsuraceSubScriberId = "";
                             string Birthdate = "";
                             strQauery = CreatePatientInsertQuery(dtWebPatient_Form, dtEasyDentalPatentColumns, o.ToString(), "Pat_dat", ref EasyDentalAddress, ref LastName, ref PrimaryInsuraceCompanyName, ref SecondaryInsuraceCompanyName, ref PrimaryInsuraceSubScriberId, ref SecondaryInsuraceSubScriberId, ref Birthdate);
                             AddressId = 0;
                             #region Save Address
                             using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                             {
                                 using (OdbcCommand OdbcCommand = new OdbcCommand(EasyDentalAddress, conn))
                                 {
                                     OdbcCommand.CommandType = CommandType.Text;
                                     if (conn.State == ConnectionState.Closed) conn.Open();
                                     OdbcCommand.ExecuteNonQuery();
                                 }
                                 EasyDentalAddress = " Select MAX(AddressId) from address_dat";
                                 using (OdbcCommand OdbcCommand = new OdbcCommand(EasyDentalAddress, conn))
                                 {
                                     OdbcCommand.CommandType = CommandType.Text;
                                     if (conn.State == ConnectionState.Closed) conn.Open();
                                     AddressId = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                                 }
                             }
                             strQauery = strQauery.Replace("@addrid", AddressId.ToString());
                             #endregion

                             #region GenerateChartNumber

                             if (LastName.Length == 1)
                             {
                                 LastName += "0";
                             }
                             int chartnum = GetEasyDentalChartNum(LastName.Substring(0, 2).ToString());
                             if (chartnum == 0 || LastName == "" || LastName == string.Empty)
                             {
                                 strQauery = strQauery.Replace("@chartnum", "NULL");
                             }
                             else
                             {
                                 if (Utility.ChartNumberIsNumeric)
                                 {
                                     strQauery = strQauery.Replace("@chartnum", chartnum.ToString("000000"));
                                 }
                                 else
                                 {
                                     strQauery = strQauery.Replace("@chartnum", LastName.Substring(0, 2).ToString().ToUpper() + chartnum.ToString("0000"));
                                 }

                                 //strQauery = strQauery.Replace("@chartnum", LastName.Substring(0, 2).ToString().ToUpper() + chartnum.ToString("0000"));
                             }
                             #endregion

                             using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                             {
                                 using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                                 {
                                     OdbcCommand.CommandType = CommandType.Text;
                                     if (conn.State != ConnectionState.Open) conn.Open();
                                     if (Birthdate != "")
                                     {
                                         OdbcCommand.Parameters.Add("@Birthdate", OdbcType.Date).Value = Convert.ToDateTime(Birthdate).ToString("yyyy/MM/dd");
                                     }
                                     OdbcCommand.ExecuteNonQuery();
                                 }
                                 string EasyDentalPatientId = " Select MAX(patid) from pat_dat";
                                 using (OdbcCommand OdbcCommand = new OdbcCommand(EasyDentalPatientId, conn))
                                 {
                                     OdbcCommand.CommandType = CommandType.Text;
                                     if (conn.State != ConnectionState.Open) conn.Open();
                                     patient_EHR_Id = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                                 }

                                 UpdateEmpIdInPatient(EmpId, patient_EHR_Id); // Updates EmployerId in Patient Table after Patient Insert

                                 using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                                 {
                                     OdbcCommand.CommandType = CommandType.Text;
                                     if (conn.State != ConnectionState.Open) conn.Open();
                                     OdbcCommand.CommandText = SynchEasyDentalQRY.UpdatePatientGuarantorID;
                                     OdbcCommand.Parameters.Clear();
                                     OdbcCommand.Parameters.AddWithValue("isguar", 1);
                                     OdbcCommand.Parameters.AddWithValue("isheadofhouse", 1);
                                     OdbcCommand.Parameters.AddWithValue("Guarantor", patient_EHR_Id);
                                     OdbcCommand.Parameters.AddWithValue("famid", patient_EHR_Id);
                                     OdbcCommand.Parameters.AddWithValue("patid", patient_EHR_Id);
                                     OdbcCommand.ExecuteNonQuery();
                                 }
                                 InsertAgigPatient(patient_EHR_Id, conn);
                                 //using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                                 //{
                                 //    OdbcCommand.CommandType = CommandType.Text;
                                 //    if (conn.State != ConnectionState.Open) conn.Open();
                                 //    OdbcCommand.CommandText = SynchEasyDentalQRY.InsertPatientAggingGuarantorID;
                                 //    OdbcCommand.Parameters.Clear();
                                 //    OdbcCommand.Parameters.AddWithValue("Guarantor", patient_EHR_Id);
                                 //    OdbcCommand.Parameters.Add("Birthdate", OdbcType.Date).Value = Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd");
                                 //    OdbcCommand.ExecuteNonQuery();
                                 //}
                             }
                             UpdatePatientInsurance(PrimaryInsuraceCompanyName, patient_EHR_Id, 1, PrimaryInsuraceSubScriberId);
                             UpdatePatientInsurance(SecondaryInsuraceCompanyName, patient_EHR_Id, 2, SecondaryInsuraceSubScriberId);



                             //patient_EHR_Id = Convert.ToInt64(OdbcCommand.ExecuteScalar());

                             //if (Convert.ToInt64(patient_EHR_Id) > 0)
                             //{
                             //    strQauery = SynchEasyDentalQRY.Update_Patinet_Record_By_Patient_Form;
                             //    strQauery = strQauery.Replace("@Patient_Id", patient_EHR_Id.ToString());
                             //    CommonDB.OdbcCommandServer(strQauery, conn, ref OdbcCommand, "txt");
                             //    OdbcCommand.ExecuteNonQuery();
                             //}
                             UpdatePatientEHRIdINPatientForm(patient_EHR_Id.ToString(), o.ToString().Trim());


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
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                {
                    string EasyDentalAddress = " Delete from address_dat where AddressId = " + AddressId;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(EasyDentalAddress, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.ExecuteNonQuery();
                    }
                }
                throw new Exception(strQauery.ToString());
            }
            finally
            {

            }
            return is_Record_Update;
        }

        #region New Patient Form Fields

        #region Employer
        private static DataTable InsertAgigPatient(long PatientEHRID, OdbcConnection conn)
        {
            DataTable OdbcDt = new DataTable();
            using (OdbcCommand OdbcCom = new OdbcCommand("", conn))
            {
                //string OdbcSelect =
                if (conn.State != ConnectionState.Open) conn.Open();
                string strsql = " " + "select * from aging_dat where guaratorid = guirid";
                strsql = strsql.Replace("guirid", PatientEHRID.ToString());
                OdbcCom.CommandText = strsql;
                OdbcCom.CommandType = CommandType.Text;

                // OdbcCom.Parameters.Clear();
                //OdbcCom.Parameters.AddWithValue("guarid", PatientEHRID);
                //string result = OdbcCom.ExecuteScalar().ToString();
                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCom))
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
                    OdbcCommand.CommandText = SynchEasyDentalQRY.InsertPatientAggingGuarantorID;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.Parameters.AddWithValue("Guarantor", PatientEHRID);
                    OdbcCommand.Parameters.Add("Birthdate", OdbcType.Date).Value = Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd");
                    OdbcCommand.ExecuteNonQuery();
                }
            }
            OdbcDt = new DataTable();
            using (OdbcCommand OdbcCom = new OdbcCommand("", conn))
            {
                //string OdbcSelect =
                if (conn.State != ConnectionState.Open) conn.Open();
                string strsql = " " + "select * from aging_dat where guaratorid = guirid";
                strsql = strsql.Replace("guirid", PatientEHRID.ToString());
                OdbcCom.CommandText = strsql;
                OdbcCom.CommandType = CommandType.Text;

                // OdbcCom.Parameters.Clear();
                //OdbcCom.Parameters.AddWithValue("guarid", PatientEHRID);
                //string result = OdbcCom.ExecuteScalar().ToString();
                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCom))
                {
                    OdbcDa.Fill(OdbcDt);
                }
            }
            return OdbcDt;
        }
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

                string strQauery = " SELECT empid FROM emp_dat WHERE name = '" + empname + "'";

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

                string strQauery = " INSERT INTO emp_dat (name) VALUES( '" + empname + "')";

                CommonDB.OdbcCommandServer(strQauery, conn, ref OdbCommand, "txt");
                OdbCommand.CommandTimeout = 200;
                OdbCommand.ExecuteNonQuery();

                strQauery = " SELECT MAX(empid) FROM emp_dat";

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

                string strQauery = " UPDATE pat_dat SET empid = " + empid + "  WHERE patid = " + patid;

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


            try
            {
                DataTable table = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                {
                    string strQauery = " select * from ins_dat where insconame = '" + curPatinetInsurance_Name + "'";
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(table);
                        }
                        if (conn.State != ConnectionState.Open) conn.Open();
                        OdbcCommand.ExecuteNonQuery();
                    }
                }
                int inssubId = 0;
                if (table.Rows.Count > 0)
                {
                    //char[] separators = new char[] { '<', '>', '.', '?', '\t', '\n','/',':',';','\'','\"','{','}','[',']','\\','|','!','@','#','$','%',' ','^','&','*','(',')','-','_','+','=','~','`' };
                    //foreach (char c in separators)
                    //{ 
                    //    // SubScriber = SubScriber.Replace(c,'\0').Replace(" ","").ToString().Trim();
                    //    SubScriber = SubScriber.Replace(c.ToString(), "").ToString().Trim();
                    //}
                    using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                    {
                        string strQauery = " " + SynchEasyDentalQRY.Insert_paitent_insurance.Trim();
                        using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                        {
                            if (conn.State != ConnectionState.Open) conn.Open();
                            OdbcCommand.CommandText = strQauery;
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("insplanid", Convert.ToInt32(table.Rows[0]["insid"]));
                            OdbcCommand.Parameters.AddWithValue("inspartyid", PatientId);
                            OdbcCommand.Parameters.AddWithValue("idnum", SubScriber.ToString().Trim().Length > 17 ? SubScriber.ToString().Trim().Substring(0, 17) : SubScriber.ToString().Trim());
                            OdbcCommand.ExecuteNonQuery();
                        }
                        string QryIdentity = " Select max(insuredid) as newId from insured_dat";
                        using (OdbcCommand OdbcCommand = new OdbcCommand(QryIdentity, conn))
                        {
                            OdbcCommand.CommandText = QryIdentity;
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Connection = conn;
                            if (conn.State != ConnectionState.Open) conn.Open();
                            inssubId = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                        }
                        using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                        {
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Connection = conn;
                            if (InsuranceCount == 1)
                            {
                                OdbcCommand.CommandText = " " + SynchEasyDentalQRY.Insert_paitent_primaryinsurance_patplan.Trim();
                            }
                            else
                            {
                                OdbcCommand.CommandText = " " + SynchEasyDentalQRY.Insert_paitent_secondaryinsurance_patplan.Trim();
                            }
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("insuredid", inssubId);
                            OdbcCommand.Parameters.AddWithValue("patid", PatientId);
                            if (conn.State != ConnectionState.Open) conn.Open();
                            OdbcCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string CreatePatientInsertQuery(DataTable dtWebPatient_Form, DataTable dtEasyDentalPatentColumns, string patientFormWebId, string tableName, ref string EasyDentalAddress, ref string LastName, ref string PrimaryInsuranceCompanyName, ref string SecondaryInsuranceCompanyName, ref string PrimaryInsuranceSubScriberId, ref string SecondaryInsuraceSubScriberId, ref string BirthDate)
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
                string Birthdate = "";
                bool IsSMS = false;
                dtEasyDentalPatentColumns.AsEnumerable().Where(z => z.Field<string>("EHRColumnName") != "" &&
                    (z.Field<string>("EHRColumnName").ToUpper() != "NAME"))
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




                        else if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "street" || e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "street2" ||
                            e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "city" || e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "state" ||
                             e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "zip" || e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "phone")
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
                            ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";
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
                                            ValueList = ValueList + "?,";
                                            Birthdate = Convert.ToDateTime(dtColumnsExists.First().Field<object>("ehrfield_value")).ToString("yyyy/MM/dd").ToString();
                                            // ValueList = ValueList + "'" + Convert.ToDateTime(dtColumnsExists.First().Field<object>("ehrfield_value")).ToString("yyyy/MM/dd").ToString() + "'" + ",";
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
                                else if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "ssn")
                                {
                                    string SocSecNum = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().ToString().Replace("-", string.Empty);

                                    SocSecNum = SocSecNum.Substring(0, 3) + "-" + SocSecNum.Substring(3, 2) + "-" + SocSecNum.ToString().Substring(5);
                                    if (SocSecNum.Length >= Convert.ToInt16(e.Field<object>("ColumnSize").ToString()))
                                    {
                                        ValueList = ValueList + "'" + SocSecNum.Substring(0, Convert.ToInt16(e.Field<object>("ColumnSize").ToString())) + "'" + ",";
                                    }

                                    else
                                    {
                                        ValueList = ValueList + "''" + ",";
                                    }
                                }
                                else
                                {

                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        if (dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Length >= Convert.ToInt16(e.Field<object>("ColumnSize").ToString()))
                                        {
                                            if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT16" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT32" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.DECIMAL")
                                            {
                                                ValueList = ValueList + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Substring(0, Convert.ToInt16(e.Field<object>("ColumnSize").ToString())) + ",";
                                            }
                                            else
                                            {
                                                ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Substring(0, Convert.ToInt16(e.Field<object>("ColumnSize").ToString())) + "'" + ",";
                                            }

                                        }
                                        else
                                        {
                                            if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT16" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT32" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.DECIMAL")
                                            {
                                                ValueList = ValueList + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + ",";
                                            }
                                            else
                                            {
                                                ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "'" + ",";
                                            }
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



                                    //}

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
                                    if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.STRING" && e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() != "BIRTHDATE")
                                    {
                                        ValueList = ValueList + "''" + ",";
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "NULL" + ",";
                                    }

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
                BirthDate = Birthdate;
                strQauery = " Insert into pat_dat( " + ColumnList + " ) VALUES ( " + ValueList + " )";
                EasyDentalAddress = " Insert into Address_dat ( " + AddressColumnList + ",ptrcount) VALUES ( " + AddressValueList + ",1 )";
                LastName = PatientLastName;
                PrimaryInsuranceCompanyName = PInsuranceCompanyName;
                SecondaryInsuranceCompanyName = SInsuranceCompanyName;
                PrimaryInsuranceSubScriberId = PInsuranceSubScriberId;
                SecondaryInsuraceSubScriberId = SInsuranceSubScriberId;
                return strQauery;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                        conn.Dispose();
                    }
                }
                return _successfullstataus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static DataTable GetEasyDentalTableColumnName()
        {

            using (SqlCeConnection conn1 = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    DataTable dtResult = new DataTable();
                    DataTable dtResultAddress = new DataTable();
                    try
                    {
                        using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            string OdbcSelect = " select * from pat_dat where 1 <> 1";
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandType = CommandType.Text;
                                DbDataReader reader = OdbcCommand.ExecuteReader();
                                DataTable table = reader.GetSchemaTable();
                                dtResult = table;
                            }
                            OdbcSelect = " select * from address_dat where 1 <> 1";
                            using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                            {
                                OdbcCommand.CommandType = CommandType.Text;
                                DbDataReader reader1 = OdbcCommand.ExecuteReader();
                                DataTable table1 = reader1.GetSchemaTable();
                                dtResultAddress = table1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    DataTable OdbcDt = new DataTable();
                    string SqlCeSelect = " SELECT COLUMN_NAME,'' AS EHRColumnName,'' AS EHRDataType,'' AS AllowNull,'' AS DefaultValue,0 as ColumnSize  FROM information_Schema.columns where table_name = 'Patient'"; ;
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
                                a["EHRColumnName"] = "street";
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
                                a["EHRColumnName"] = "email";

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
                                a["DefaultValue"] = GetEasyDentalIdelProvider().Rows[0]["RscId"].ToString();
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
                                a["DefaultValue"] = "";
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
                                a["DefaultValue"] = "M";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "WORK_PHONE")
                            {
                                a["EHRColumnName"] = "workphone";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "ZIPCODE")
                            {
                                a["EHRColumnName"] = "zip";
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

                            #endregion

                            if (a["EHRColumnName"].ToString() != "")
                            {
                                if (a["EHRColumnName"].ToString().Trim().ToLower() == "street" || a["EHRColumnName"].ToString().Trim().ToLower() == "street2" ||
                                  a["EHRColumnName"].ToString().Trim().ToLower() == "city" || a["EHRColumnName"].ToString().Trim().ToLower() == "state" || a["EHRColumnName"].ToString().Trim().ToLower() == "phone" ||
                                  a["EHRColumnName"].ToString().Trim().ToLower() == "zip")
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
                                if (a["EHRColumnName"].ToString().Trim().ToLower() == "status" || a["EHRColumnName"].ToString().Trim().ToLower() == "fampos")
                                {
                                    a["EHRDataType"] = "SYSTEM.INT16";
                                }
                            }

                            return true;
                        });


                    DataRow drNewRow = OdbcDt.NewRow();
                    drNewRow["COLUMN_NAME"] = "AddressId";
                    drNewRow["EHRColumnName"] = "AddressId";
                    drNewRow["EHRDataType"] = "SYSTEM.INT16";
                    drNewRow["AllowNull"] = "No";
                    drNewRow["DefaultValue"] = "@addrid";
                    OdbcDt.Rows.Add(drNewRow);

                    DataRow drNewRow1 = OdbcDt.NewRow();
                    drNewRow1["COLUMN_NAME"] = "GuidId";
                    drNewRow1["EHRColumnName"] = "GuidId";
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
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn1.State == ConnectionState.Open) conn1.Close();
                    conn1.Dispose();
                }
            }

        }

        public static bool Save_Document_in_EasyDental()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            bool callLoop = false;
            try
            {
                CommonDB.OdbcConnectionServer(ref conn);
                DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocData("1");
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {

                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        if (callLoop == false)
                        {
                            string sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientDoc_Name"].ToString();
                            if (!System.IO.File.Exists(sourceLocation))
                            {
                                PullLiveDatabaseDAL.Update_PatientDocNotFound_Live_To_Local(dr["PatientDoc_Web_ID"].ToString(), "1");
                                continue;
                            }

                            string tmpFileOrgName = Path.GetFileName(sourceLocation);
                            string SourcePath = Path.GetDirectoryName(sourceLocation);

                            Thread.Sleep(100);

                            if (AttachPatientDocument(sourceLocation, "/ID" + dr["Patient_EHR_ID"].ToString(), ref callLoop))
                            {
                                RegistryKey key1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DocumentAttachedId");
                                Int64 SavePatientDocId = 0;
                                if (key1 != null)
                                {
                                    SavePatientDocId = Convert.ToInt64(key1.GetValue("DocumentId").ToString());
                                    if (SavePatientDocId > 0)
                                    {
                                        PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), SavePatientDocId.ToString(), "1");
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
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                conn.Dispose();
            }
        }

        private static bool AttachPatientDocument(string docPath, string patientId, ref bool callloop)
        {
            Process myProcess = new Process();
            bool returnResult = false;
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EasyDentalAttachmentCall");
                bool IsSyncing = false;
                if (key != null)
                {
                    IsSyncing = bool.Parse(key.GetValue("IsSyncing").ToString());
                }
                if (!IsSyncing)
                {
                    callloop = true;
                    RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EasyDentalAttachmentCall");
                    key1.SetValue("IsSyncing", true);

                    try
                    {
                        myProcess.StartInfo.UseShellExecute = false;

                        myProcess.StartInfo.FileName = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName).ToString() + "\\DocumentDLL\\Document.CenterDoc.exe";

                        myProcess.StartInfo.Arguments = "" + patientId.ToString() + " " + Path.Combine(docPath) + "";
                        // myProcess.StartInfo.Arguments = "" + patientId.ToString() + " \"C:\\Program Files (x86)\\PozativeDocument\\Patient\\1\\PendingDocument\\Remote Access Instructions.pdf\"";
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();

                        RegistryKey key2 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\EasyDentalAttachmentCall");
                        if (bool.Parse(key2.GetValue("IsSyncing").ToString()))
                        {
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (bool.Parse(key2.GetValue("IsSyncing").ToString()))
                            {
                                returnResult = true;
                                if (sw.Elapsed > TimeSpan.FromSeconds(120))
                                {
                                    RegistryKey key3 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EasyDentalAttachmentCall");
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
                        throw new Exception(ex.Message);
                    }
                }
                return returnResult;
            }
            catch (Exception)
            {
                RegistryKey key3 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\EasyDentalAttachmentCall");
                key3.SetValue("IsSyncing", false);
                return returnResult;
            }
        }

        public static void CheckConnection(OdbcConnection CON)
        {
            if (CON.State != ConnectionState.Open)
                CON.Open();
        }
        #region MedicleForm

        public static DataTable GetEasyDentalMedicleQuestionData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalMedicleQuestionData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                            DataColumn objDataColumn = new DataColumn();
                            objDataColumn.ColumnName = "EasyDental_DefaultValue";
                            OdbcDt.Columns.Add(objDataColumn);
                            objDataColumn = new DataColumn();
                            objDataColumn.ColumnName = "EHR_Entry_DateTime";
                            objDataColumn.DefaultValue = DateTime.Now.ToString();
                            OdbcDt.Columns.Add(objDataColumn);
                            objDataColumn = new DataColumn();
                            objDataColumn.ColumnName = "Last_Sync_Date";
                            objDataColumn.DefaultValue = DateTime.Now.ToString();
                            OdbcDt.Columns.Add(objDataColumn);
                            objDataColumn = new DataColumn();
                            objDataColumn.ColumnName = "Entry_DateTime";
                            objDataColumn.DefaultValue = DateTime.Now.ToString();
                            OdbcDt.Columns.Add(objDataColumn);
                            foreach (DataRow dr in OdbcDt.Rows)
                            {
                                if (dr["EasyDental_QuestionType"].ToString() == "1")
                                {
                                    dr["EasyDental_DefaultValue"] = dr["DefaultAnsText"].ToString();
                                }
                                else if (dr["EasyDental_QuestionType"].ToString() == "2")
                                {
                                    dr["EasyDental_DefaultValue"] = dr["DefaultAnsNumber"].ToString();
                                }
                                else if (dr["EasyDental_QuestionType"].ToString() == "3")
                                {
                                    dr["EasyDental_DefaultValue"] = dr["DefaultAnsBool"].ToString();
                                }
                                else if (dr["EasyDental_QuestionType"].ToString() == "4")
                                {
                                    dr["EasyDental_DefaultValue"] = dr["DefaultAnsDate"].ToString();
                                }
                            }
                            OdbcDt.Columns.Remove("DefaultAnsText");
                            OdbcDt.Columns.Remove("DefaultAnsNumber");
                            OdbcDt.Columns.Remove("DefaultAnsBool");
                            OdbcDt.Columns.Remove("DefaultAnsDate");
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static bool SaveMedicalHistoryLocalToEasyDental()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            try
            {
                DataTable dtWebPatient_FormMedicalHistory = SynchLocalDAL.GetLiveEasyDentalPatientFormMedicalHistoryData();
                if (dtWebPatient_FormMedicalHistory != null)
                {
                    if (dtWebPatient_FormMedicalHistory.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWebPatient_FormMedicalHistory.Rows)
                        {
                            DataTable EasyDentalRespoanseSetDt = GetEasyDentalMedicleResponseData(dr["Patient_EHR_ID"].ToString(), dr["EasyDental_QuestionId"].ToString());

                            if (EasyDentalRespoanseSetDt.Rows.Count == 0)
                            {
                                string sqlSelect = string.Empty;
                                CommonDB.OdbcConnectionServer(ref conn);
                                CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                                OdbcCommand.CommandText = SynchEasyDentalQRY.InsertEasyDentalMedicleResponseSetData;
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.Parameters.AddWithValue("patid", dr["Patient_EHR_ID"].ToString());
                                OdbcCommand.Parameters.AddWithValue("QuestionId", dr["EasyDental_QuestionId"].ToString());
                                OdbcCommand.Parameters.AddWithValue("QuestionType", dr["EasyDental_QuestionType"].ToString());
                                if (dr["EasyDental_QuestionType"].ToString() != "0")
                                {
                                    if (dr["EasyDental_QuestionType"].ToString() == "1")
                                    {
                                        OdbcCommand.Parameters.AddWithValue("AnsText", dr["Answer_Value"].ToString());
                                        OdbcCommand.Parameters.AddWithValue("AnsNumber", 0);
                                        OdbcCommand.Parameters.AddWithValue("AnsBool", 0);
                                        OdbcCommand.Parameters.Add("AnsDate", OdbcType.Date).Value = DBNull.Value;
                                    }
                                    else if (dr["EasyDental_QuestionType"].ToString() == "2")
                                    {
                                        OdbcCommand.Parameters.AddWithValue("AnsText", "");
                                        OdbcCommand.Parameters.AddWithValue("AnsNumber", Convert.ToDecimal(dr["Answer_Value"].ToString()));
                                        OdbcCommand.Parameters.AddWithValue("AnsBool", 0);
                                        OdbcCommand.Parameters.Add("AnsDate", OdbcType.Date).Value = DBNull.Value;
                                    }
                                    else if (dr["EasyDental_QuestionType"].ToString() == "3")
                                    {
                                        OdbcCommand.Parameters.AddWithValue("AnsText", "");
                                        OdbcCommand.Parameters.AddWithValue("AnsNumber", 0);
                                        OdbcCommand.Parameters.AddWithValue("AnsBool", Convert.ToInt16(dr["Answer_Value"].ToString() == "Y" ? 1 : 0));
                                        OdbcCommand.Parameters.Add("AnsDate", OdbcType.Date).Value = DBNull.Value;
                                    }
                                    else if (dr["EasyDental_QuestionType"].ToString() == "4")
                                    {
                                        OdbcCommand.Parameters.AddWithValue("AnsText", "");
                                        OdbcCommand.Parameters.AddWithValue("AnsNumber", 0);
                                        OdbcCommand.Parameters.AddWithValue("AnsBool", 0);
                                        try
                                        {
                                            OdbcCommand.Parameters.Add("AnsDate", OdbcType.Date).Value = Convert.ToDateTime(dr["Answer_Value"]).ToString("yyyy/MM/dd");
                                        }
                                        catch
                                        {
                                            OdbcCommand.Parameters.Add("AnsDate", OdbcType.Date).Value = DBNull.Value;
                                        }
                                    }
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();
                                }
                            }
                            UpdateResponseUniqueEHRIdInEasyDental_Response(dr["Patient_EHR_ID"].ToString(), dr["EasyDental_QuestionId"].ToString());
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
        }


        public static bool SaveDiseaseLocalToEasyDental()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            try
            {
                DataTable dtWebPatient_FormDisease = SynchLocalDAL.GetLocalPatientFormDiseaseResponseToSaveINEHR("1");
                if (dtWebPatient_FormDisease != null)
                {
                    if (dtWebPatient_FormDisease.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWebPatient_FormDisease.Rows)
                        {
                            string sqlSelect = string.Empty;
                            CommonDB.OdbcConnectionServer(ref conn);
                            CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");
                            OdbcCommand.CommandText = SynchEasyDentalQRY.InsertEasyDentalDiseaseData;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("patid", dr["PatientEHRID"].ToString());
                            OdbcCommand.Parameters.AddWithValue("hhitemid", dr["Disease_EHR_ID"].ToString());
                            OdbcCommand.Parameters.AddWithValue("note", "Web");
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();

                            string DiseaseId = "";
                            string QryIdentity = "Select max(linkid) as newId from admin.hhlinkpat_item";
                            OdbcCommand.CommandText = QryIdentity;
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Connection = conn;
                            CheckConnection(conn);
                            DiseaseId = Convert.ToString(OdbcCommand.ExecuteScalar());
                            SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), DiseaseId.ToString() == "" ? dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString() : DiseaseId.ToString(), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), "1");
                            //SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["DiseaseResponse_Local_ID"].ToString(), DiseaseId, dr["Patient_EHR_Id"].ToString(), "1");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
        }

        private static bool UpdateResponseUniqueEHRIdInEasyDental_Response(string Patient_EHR_ID, string EasyDental_QuestionId)
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
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_EasyDental_Response;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", Patient_EHR_ID);
                            SqlCeCommand.Parameters.AddWithValue("EasyDental_QuestionId", EasyDental_QuestionId);
                            SqlCeCommand.ExecuteNonQuery();
                            _successfullstataus = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        _successfullstataus = false;
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                        conn.Dispose();
                    }
                }
                return _successfullstataus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable GetEasyDentalMedicleResponseData(string Patient_EHR_id, string EasyDental_QuestionId)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalMedicleResponseData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("Patient_EHR_id", Patient_EHR_id);
                        OdbcCommand.Parameters.AddWithValue("EasyDental_QuestionId", EasyDental_QuestionId);
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string Save_PatientPaymentLog_LocalToEasyDental(DataRow drRow)
        {
            Int32 Contactid = 0;
            try
            {                
                if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                {
                    Utility.EHR_UserLogin_ID = GetOrCreateUserId();
                }
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                {
                    try
                    {
                        using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                        {
                            if (conn.State != ConnectionState.Open) conn.Open();
                            OdbcCommand.CommandText = SynchEasyDentalQRY.Insert_paitent_PaymentLog;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                            OdbcCommand.Parameters.AddWithValue("Contacttime", Convert.ToInt64(Convert.ToDateTime(drRow["PaymentDate"].ToString()).TimeOfDay.TotalMilliseconds.ToString().Substring(0, 5)));
                            OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                            OdbcCommand.Parameters.AddWithValue("Provid", Utility.EHR_UserLogin_ID);// GetEasyDentalIdelProvider().Rows[0]["RscId"].ToString() 
                            DateTime? PDate = Convert.ToDateTime(drRow["PaymentDate"]);
                            OdbcCommand.Parameters.AddWithValue("title", drRow["PaymentMode"].ToString() + " on " + PDate.Value.ToString("dd/MM/yyyy hh:mm tt")); //DateTime.ParseExact(Convert.ToDateTime(drRow["PaymentDate"].ToString()), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture));
                            OdbcCommand.Parameters.AddWithValue("contacttype", 5);

                            //  OdbcCommand.Parameters.AddWithValue("title", drRow["PaymentMode"].ToString() + " on " + DateTime.ParseExact(drRow["PaymentDate"].ToString(), "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture));
                            OdbcCommand.ExecuteNonQuery();
                        }
                        string QryIdentity = " Select max(Contactid) as newId from Contact_dat";
                        using (OdbcCommand OdbcCommand = new OdbcCommand(QryIdentity, conn))
                        {
                            OdbcCommand.CommandText = QryIdentity;
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Connection = conn;
                            if (conn.State != ConnectionState.Open) conn.Open();
                            Contactid = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                        }
                        using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                        {
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Connection = conn;
                            OdbcCommand.CommandText = SynchEasyDentalQRY.Insert_paitent_PaymentLogNote;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("notetype", 49);
                            OdbcCommand.Parameters.AddWithValue("noteid", Contactid);
                            OdbcCommand.Parameters.AddWithValue("notetext", drRow["template"].ToString());
                            if (conn.State != ConnectionState.Open) conn.Open();
                            OdbcCommand.ExecuteNonQuery();
                        }
                        //SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", Contactid.ToString(), "", "", "");

                    }
                    catch (Exception ex)
                    {
                        return "";
                        //SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", "400", "", "", "", "");
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
                return Contactid.ToString();
            }
            catch (Exception ex)
            {
                return "";
                throw new Exception(ex.Message);
            }
        }
        #endregion


        public static string Save_PatientSMSCallLog_LocalToEasyDental(DataTable dtWebPatientPayment)
        {
            try
            {
                string NoteId = "";                
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                {
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
                                    Utility.EHR_UserLogin_ID = GetOrCreateUserId();
                                }
                            }
                        }
                        foreach (DataRow drRow in dtResultCopy.Rows)
                        {
                            if (drRow["PatientEHRId"] != null && drRow["PatientEHRId"].ToString() != string.Empty && drRow["PatientEHRId"].ToString() != "")
                            {
                                NoteId = CheckSMSCallLogRecordsExists(drRow);
                                if (NoteId == "0" && ((Convert.ToInt16(drRow["LogType"]) == 0 && drRow["Mobile"].ToString() != "") || Convert.ToInt16(drRow["LogType"]) == 1))
                                {
                                    Int32 Contactid = 0;
                                    try
                                    {
                                        using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                                        {
                                            if (conn.State != ConnectionState.Open) conn.Open();

                                            OdbcCommand.CommandText = SynchEasyDentalQRY.Insert_paitent_PaymentLog;
                                            OdbcCommand.Parameters.Clear();
                                            OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString());
                                            OdbcCommand.Parameters.AddWithValue("Contacttime", Convert.ToInt64(Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString()).TimeOfDay.TotalMilliseconds.ToString().Substring(0, 5)));
                                            OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                                            // OdbcCommand.Parameters.AddWithValue("Provid", GetEasyDentalIdelProvider().Rows[0]["RscId"].ToString());  
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
                                        }
                                        string QryIdentity = " Select max(Contactid) as newId from Contact_dat";
                                        using (OdbcCommand OdbcCommand = new OdbcCommand(QryIdentity, conn))
                                        {
                                            OdbcCommand.CommandText = QryIdentity;
                                            OdbcCommand.CommandType = CommandType.Text;
                                            OdbcCommand.Connection = conn;
                                            if (conn.State != ConnectionState.Open) conn.Open();
                                            Contactid = Convert.ToInt32(OdbcCommand.ExecuteScalar());
                                        }
                                        using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                                        {
                                            OdbcCommand.CommandType = CommandType.Text;
                                            OdbcCommand.Connection = conn;
                                            OdbcCommand.CommandText = SynchEasyDentalQRY.Insert_paitent_PaymentLogNote;
                                            OdbcCommand.Parameters.Clear();
                                            OdbcCommand.Parameters.AddWithValue("notetype", 49);
                                            OdbcCommand.Parameters.AddWithValue("noteid", Contactid);
                                            OdbcCommand.Parameters.AddWithValue("notetext", drRow["text"].ToString());
                                            if (conn.State != ConnectionState.Open) conn.Open();
                                            OdbcCommand.ExecuteNonQuery();
                                        }
                                        drRow["LogEHRId"] = Contactid.ToString();
                                        drRow["Log_Status"] = "completed";
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
                            //SynchLocalDAL.CallPatientSMSCallAPIForStatusCompleted(dtResultCopy, "completed", "1", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                        }

                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Int16 GetPaymentType(string PaymentMode, Int16 deftype, string TableName, OdbcConnection conn,string _filename_EHR_Payment = "",string _EHRLogdirectory_EHR_Payment = "")
        {
            try
            {
                Int16 defid = 0;
                using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                {
                    string QryIdentity = " Select max(defid) as newId from " + TableName + " where description = ? and type = ?";
                    if (conn.State != ConnectionState.Open) conn.Open();
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.CommandType = CommandType.Text;
                    OdbcCommand.Parameters.AddWithValue("descript", PaymentMode);
                    OdbcCommand.Parameters.AddWithValue("deftype", deftype);
                    CheckConnection(conn);
                    defid = OdbcCommand.ExecuteScalar() == null ? Convert.ToInt16(-1) : Convert.ToInt16(OdbcCommand.ExecuteScalar());
                    Utility.WriteSyncPullLog(_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment, " Get Payment Type new defid=" + defid.ToString() + " for description = " + PaymentMode + " and type =" + deftype);
                }
                if (defid == -1)
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.CommandText = " Select max(defid)+1 as newId from " + TableName + " where type = ?";
                        OdbcCommand.Parameters.Clear();
                        // OdbcCommand.Parameters.AddWithValue("descript", PaymentMode);
                        OdbcCommand.Parameters.AddWithValue("deftype", deftype);
                        CheckConnection(conn);
                        defid = Convert.ToInt16(OdbcCommand.ExecuteScalar());
                        Utility.WriteSyncPullLog(_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment, " Get Payment Type new defid=" + defid.ToString() + " for type =" + deftype);
                    }

                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.CommandText = " insert into " + TableName + " (defid,description,type) values (?,?,?)";
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("defid", defid);
                        OdbcCommand.Parameters.AddWithValue("descript", PaymentMode);
                        OdbcCommand.Parameters.AddWithValue("deftype", deftype);
                        CheckConnection(conn);
                        OdbcCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Payment Type with defid=" + defid.ToString() + " ,description = " + PaymentMode + " and type =" + deftype);
                    }
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

        public static string GetPatientPrimaryProvider(string PatId, OdbcConnection conn)
        {
            string providerid = "";
            try
            {
                using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                {
                    string QryIdentity = SynchEasyDentalQRY.GetPatientPrimaryProvider;
                    if (conn.State != ConnectionState.Open) conn.Open();
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.Parameters.Clear();
                    OdbcCommand.CommandType = CommandType.Text;
                    OdbcCommand.Parameters.AddWithValue("patid", PatId);
                    CheckConnection(conn);
                    providerid = Convert.ToString(OdbcCommand.ExecuteScalar());
                    Utility.WriteToErrorLogFromAll("primary provider is =" + providerid);
                }
                return providerid;
            }

            catch (Exception ex)
            {
                return "0";
            }

        }
        public static Int64 GetDuplicatePayment(string patid, string guarid, DateTime Procdate, string proclogorder, short proclogclass, string provid, decimal amt, OdbcConnection conn)
        {
            try
            {
                Int64 defid = 0;
                using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                {
                    string QryIdentity = " " + "Select max(procedureid) as newId from proc_log_dat where patid = @patId and guarid = @guarid and date = ? and providerid = @providerId and amount = @amount and class = @class and proccodeid = @proccodeId";
                    //string QryIdentity = " " + "Select max(procedureid) as newId from proc_log_dat where patid = ? and guarid = ? and date = ? and providerid = ? and amount = ? and class = ? and proccodeid = ?";
                    if (conn.State != ConnectionState.Open) conn.Open();
                    QryIdentity = QryIdentity.Replace("@patId", guarid.ToString());
                    QryIdentity = QryIdentity.Replace("@guarid", guarid.ToString());
                    // QryIdentity = QryIdentity.Replace("@date", "'" + Convert.ToDateTime(Procdate).ToString("yyyy/MM/dd") + "'");
                    QryIdentity = QryIdentity.Replace("@providerId", "'" + provid.ToString() + "'");
                    QryIdentity = QryIdentity.Replace("@amount", amt.ToString());
                    QryIdentity = QryIdentity.Replace("@class", proclogclass.ToString());
                    QryIdentity = QryIdentity.Replace("@proccodeId", patid.ToString());
                    OdbcCommand.CommandText = QryIdentity;
                    OdbcCommand.Parameters.Clear();
                    //OdbcCommand.Parameters.Add("patid", OdbcType.BigInt).Value = Convert.ToInt64(guarid);
                    //OdbcCommand.Parameters.Add("guarid", OdbcType.BigInt).Value = Convert.ToInt64(guarid);
                    //OdbcCommand.Parameters.Add(new OdbcParameter("guarid", Convert.ToInt64(guarid)));
                    OdbcCommand.Parameters.Add("Procdate", OdbcType.Date).Value = Convert.ToDateTime(Procdate).ToString("yyyy/MM/dd");
                    //OdbcCommand.Parameters.Add(new OdbcParameter("provid", provid.Trim()));
                    //OdbcCommand.Parameters.Add(new OdbcParameter("amt", Convert.ToDecimal(amt)));
                    //OdbcCommand.Parameters.Add(new OdbcParameter("proclogclass", proclogclass));
                    //OdbcCommand.Parameters.Add(new OdbcParameter("proccodeid", Convert.ToInt64(patid)));
                    CheckConnection(conn);
                    try
                    {
                        object def = OdbcCommand.ExecuteScalar();
                        defid = def == null ? Convert.ToInt16(-1) : Convert.ToInt64(def);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                    //defid = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                }
                if (defid == -1)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt64(defid);
                }
            }
            catch
            {
                return 0;
            }
        }

        public static string GetProviderForPatient(DataRow dr)
        {
            if ((dr["ProviderEHRId"].ToString() == "" || string.IsNullOrEmpty(dr["ProviderEHRId"].ToString())) &&
                (dr["Pri_Provider_ID"].ToString() == "" || string.IsNullOrEmpty(dr["Pri_Provider_ID"].ToString())) &&
                (dr["Sec_Provider_ID"].ToString() == "" || string.IsNullOrEmpty(dr["Sec_Provider_ID"].ToString())))
            {
                return GetEasyDentalIdelProvider().Rows[0]["RscId"].ToString();
            }
            else
            {
                return ((dr["ProviderEHRId"].ToString() == "" || string.IsNullOrEmpty(dr["ProviderEHRId"].ToString())) ?
                    ((dr["Pri_Provider_ID"].ToString() == "" || string.IsNullOrEmpty(dr["Pri_Provider_ID"].ToString())) ? dr["Sec_Provider_ID"].ToString() : dr["Pri_Provider_ID"].ToString()) : dr["ProviderEHRId"].ToString());
            }
        }

        public static Int64 SavePatientPaymentTOEHR(DataTable dtTable,string _filename_EHR_Payment = "",string _EHRLogdirectory_EHR_Payment = "")
        {


            try
            {
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                {
                    Int64 TransactionHeaderId = 0;
                    //  Int64 splitduplicateid = 0;
                    string LogEHRId = "0";
                    Int64 DiscountId = 0;
                    decimal discount = 0;
                    Int16 PaymentRefundTypeID = 0;
                    Int16 DiscountTypeID = 0;
                    Int16 PaymentTypeID = GetPaymentType(" Adit Pay", 6, "DefPayment_dat", conn);
                    var result = dtTable.AsEnumerable().Where(o => o.Field<object>("PaymentMode").ToString().ToUpper() == "REFUNDED" || o.Field<object>("PaymentMode").ToString().ToUpper() == "PARTIAL-REFUNDED").FirstOrDefault();
                    if (result != null)
                    {
                        PaymentRefundTypeID = GetPaymentType("+Adit Pay Refund", 9, "DefAdjustment_dat", conn, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        Utility.WriteSyncPullLog(_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment, " Get PaymentRefundTypeID=" + PaymentRefundTypeID.ToString());
                    }
                    result = dtTable.AsEnumerable().Where(o => o.Field<decimal>("Discount") > 0).FirstOrDefault();
                    if (result != null)
                    {
                        DiscountTypeID = GetPaymentType("-Adit Pay Discount", 9, "DefAdjustment_dat", conn);
                    }
                    Int16 TypeID = 0;
                    if (dtTable != null)
                    {
                        if (dtTable.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist(); 
                        }
                    }
                    foreach (DataRow drRow in dtTable.Rows)
                    {
                        discount = 0;
                        LogEHRId = "0";
                        DiscountId = 0;
                        TransactionHeaderId = 0;
                        TypeID = 0;
                        try
                        {
                            if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 2 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                            {
                                short procclass = 0;
                                decimal amount = 0;
                                bool is_payment = false;
                                DateTime Deleteprocdate;
                                decimal Deleteamount = 0;
                                decimal aging0to30 = 0;
                                bool Is_Payment_successfull = false;
                                bool Is_Discount_successfull = false;
                                if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                                {
                                    is_payment = true;
                                    procclass = 1;
                                    amount = -Convert.ToDecimal(drRow["Amount"]) + Convert.ToDecimal(drRow["Discount"]);
                                    discount = Convert.ToDecimal(drRow["Discount"]);
                                    TypeID = PaymentTypeID;
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
                                    DataTable OdbcDt = new DataTable();
                                    OdbcDt = InsertAgigPatient(Convert.ToInt64(drRow["Guar_ID"].ToString()), conn);
                                    if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                                    {
                                        if (OdbcDt.Rows[0]["aging0to30"] == null)
                                        {
                                            aging0to30 = 0;
                                        }
                                        else
                                        {
                                            aging0to30 = Convert.ToDecimal(OdbcDt.Rows[0]["aging0to30"]);
                                        }
                                        if (OdbcDt.Rows[0]["lastpaydate"] != DBNull.Value)
                                        {
                                            Deleteprocdate = Convert.ToDateTime(OdbcDt.Rows[0]["lastpaydate"]);
                                            Deleteamount = Convert.ToDecimal(OdbcDt.Rows[0]["lastpayamount"]);
                                        }
                                        else
                                        {
                                            Deleteprocdate = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                                            Deleteamount = 0;
                                        }
                                    }
                                    else
                                    {
                                        Deleteprocdate = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                                        Deleteamount = Convert.ToDecimal(drRow["Amount"]);
                                    }
                                    if ((Convert.ToDecimal(drRow["Amount"]) - discount) > 0)
                                    {
                                        decimal AgingAmount = 0;
                                        string OdbcSelect = "";
                                        // string provid = GetProviderForPatient(drRow); 
                                        string provid = GetPatientPrimaryProvider(drRow["PatientEHRId"].ToString(), conn);
                                        if (provid != null)
                                        {
                                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get Patient Primary PatientEHRId=" + drRow["PatientEHRId"].ToString());
                                        }
                                        TransactionHeaderId = GetDuplicatePayment(drRow["PatientEHRId"].ToString(), drRow["Guar_ID"].ToString(), Convert.ToDateTime(drRow["PaymentDate"].ToString()), TypeID.ToString(), procclass, provid, amount, conn);
                                        if (TransactionHeaderId == 0)
                                        {
                                            Is_Payment_successfull = false;
                                            TransactionHeaderId = SavePayment(drRow, procclass, PaymentTypeID, provid, amount, aging0to30, Deleteamount, Deleteprocdate, OdbcDt, Is_Payment_successfull,_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
                                            if (TransactionHeaderId > 0)
                                            {
                                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Payment with PaymentTypeID=" + PaymentTypeID.ToString() + "provid=" + provid + ",TransactionHeaderId=" + TransactionHeaderId.ToString());
                                            }
                                        }
                                        else
                                        {
                                            #region check same amount payments exist ? if not then allow entry to ehr 
                                            string note = string.Empty;
                                            note = GetPaymentNote(TransactionHeaderId);
                                            if (!note.Contains(drRow["PatientPaymentWebId"].ToString()) && note != string.Empty)
                                            {
                                                Is_Payment_successfull = false;
                                                TransactionHeaderId = SavePayment(drRow, procclass, PaymentTypeID, provid, amount, aging0to30, Deleteamount, Deleteprocdate, OdbcDt, Is_Payment_successfull);
                                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Payment with PaymentTypeID=" + PaymentTypeID.ToString() + ",provid=" + provid + ",TransactionHeaderId=" + TransactionHeaderId.ToString());
                                            }
                                            #endregion
                                        }
                                    }

                                    if (discount > 0 && DiscountTypeID > 0)
                                    {
                                        // string provid = GetProviderForPatient(drRow); 
                                        string provid = GetPatientPrimaryProvider(drRow["PatientEHRId"].ToString(), conn);
                                        Is_Discount_successfull = false;
                                        DiscountId = GetDuplicatePayment(drRow["PatientEHRId"].ToString(), drRow["Guar_ID"].ToString(), Convert.ToDateTime(drRow["PaymentDate"].ToString()), TypeID.ToString(), 2, provid, -discount, conn);
                                        if (DiscountId == 0)
                                        {
                                            Is_Discount_successfull = false;
                                            DiscountId = SavePaymentDiscount(drRow, provid, discount, TransactionHeaderId, aging0to30, Deleteamount, Deleteprocdate, OdbcDt, Is_Payment_successfull, Is_Discount_successfull,_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
                                            if (DiscountId > 0)
                                            {
                                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Payment Discount provid="+ provid + ",discount=" + discount.ToString() + ",TransactionHeaderId=" + TransactionHeaderId.ToString());
                                            }
                                        }
                                        else
                                        {
                                            #region check same amount payments exist ? if not then allow entry to ehr 
                                            string note = string.Empty;
                                            note = GetPaymentNote(DiscountId);
                                            if (!note.Contains(drRow["PatientPaymentWebId"].ToString()) && note != string.Empty)
                                            {
                                                Is_Discount_successfull = false;
                                                DiscountId = SavePaymentDiscount(drRow, provid, discount, TransactionHeaderId, aging0to30, Deleteamount, Deleteprocdate, OdbcDt, Is_Payment_successfull, Is_Discount_successfull, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                                                if(DiscountId>0)
                                                {
                                                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Payment Discount provid=" + provid + ",discount=" + discount.ToString() + ",TransactionHeaderId=" + TransactionHeaderId.ToString());
                                                }
                                            }
                                            #endregion
                                        }

                                    }
                                }
                            }
                            if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 1 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                            {
                                LogEHRId = Save_PatientPaymentLog_LocalToEasyDental(drRow);
                            }
                            bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow);
                            Utility.WriteToErrorLogFromAll("Check update 1..");
                            SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", TransactionHeaderId.ToString(), LogEHRId, DiscountId.ToString(), "", Convert.ToInt32(drRow["TryInsert"]));

                        }
                        catch (Exception ex1)
                        {
                            //NoteId = "";
                            bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow);
                            SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", ex1.Message.ToString(), "", "", "", ex1.Message.ToString(), Convert.ToInt32(drRow["TryInsert"]));

                        }
                    }
                }

                return 0;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public static Int64 SavePayment(DataRow drRow, short procclass, Int16 PaymentTypeID, string provid, decimal amount,decimal aging0to30,decimal Deleteamount,DateTime Deleteprocdate, DataTable OdbcDt,bool Is_Payment_successfull,string _filename_EHR_Payment = "",string _EHRLogdirectory_EHR_Payment = "")
        {
            Int64 TransactionHeaderId = 0;
            string OdbcSelect = "";
            decimal AgingAmount = 0;
            using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
            {
                try
                {
                    //#region splitpayment
                    //if (splittable.Rows.Count > 0)
                    //{
                    //    foreach (DataRow splitdrrow in splittable.Rows)
                    //    {
                    //        decimal splitamount = -Convert.ToDecimal(splitdrrow["Amount"]);
                    //        splitduplicateid = GetDuplicatePayment(splitdrrow["PatientEHRId"].ToString(), drRow["Guar_ID"].ToString(), Convert.ToDateTime(drRow["PaymentDate"].ToString()), TypeID.ToString(), procclass, splitdrrow["ProviderEHRId"].ToString(), splitamount, conn);
                    //        if (splitduplicateid == 0)
                    //        {
                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();
                        OdbcCommand.CommandText = " " + SynchEasyDentalQRY.InsertProcLogAmount;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("patid", drRow["Guar_ID"].ToString());
                        OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                        OdbcCommand.Parameters.AddWithValue("date", OdbcType.Date).Value = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                        //                OdbcCommand.Parameters.AddWithValue("proccodeid", splitdrrow["PatientEHRId"].ToString());
                        //                OdbcCommand.Parameters.AddWithValue("class", procclass);
                        //                OdbcCommand.Parameters.AddWithValue("Providerid", splitdrrow["ProviderEHRId"].ToString());
                        //                OdbcCommand.Parameters.AddWithValue("amt", splitamount);
                        //                OdbcCommand.Parameters.AddWithValue("createdate", OdbcType.Date).Value = Convert.ToDateTime(DateTime.Now);
                        //                CheckConnection(conn);
                        //                OdbcCommand.ExecuteNonQuery();
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                        //else
                        //{
                        //    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                        //    {
                        //        if (conn.State != ConnectionState.Open) conn.Open();
                        //        OdbcCommand.CommandText = " " + SynchEasyDentalQRY.InsertProcLogAmount;
                        //        OdbcCommand.Parameters.Clear();
                        //        OdbcCommand.Parameters.AddWithValue("patid", drRow["Guar_ID"].ToString());
                        //        OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                        //        OdbcCommand.Parameters.AddWithValue("date", OdbcType.Date).Value = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                        OdbcCommand.Parameters.AddWithValue("proccodeid", drRow["PatientEHRId"].ToString());
                        OdbcCommand.Parameters.AddWithValue("class", procclass);
                        OdbcCommand.Parameters.AddWithValue("Providerid", provid);
                        OdbcCommand.Parameters.AddWithValue("amt", amount);
                        OdbcCommand.Parameters.AddWithValue("createdate", OdbcType.Date).Value = Convert.ToDateTime(DateTime.Now);
                        CheckConnection(conn);
                        OdbcCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment, " Save  Proc Log Amount  Gaurdian =" + drRow["Guar_ID"].ToString() + ", Providerid" + provid);
                    }
                    //  }

                    // Get Full procedure logid
                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();
                        OdbcCommand.CommandText = " Select procedureid from proc_log_dat";
                        OdbcCommand.CommandType = CommandType.Text;
                        DataTable OdbcDt1 = new DataTable();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt1);
                        }
                        TransactionHeaderId = Convert.ToInt64(OdbcDt1.AsEnumerable().Select(a => a.Field<object>("procedureid")).Max());
                    }
                    if (drRow["PaymentMode"].ToString().ToLower() == "paid" || drRow["PaymentMode"].ToString().ToLower() == "partial-paid")
                    {
                        using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                        {
                            if (conn.State != ConnectionState.Open) conn.Open();
                            OdbcCommand.CommandText = " update payment_dat set defpayid = ?,checknum = ?,banknumber = ? where paymentid = ?";
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("defpayid", PaymentTypeID);
                            OdbcCommand.Parameters.AddWithValue("checknum", drRow["ChequeNumber"].ToString());
                            OdbcCommand.Parameters.AddWithValue("banknum", drRow["BankOrBranchName"].ToString());
                            OdbcCommand.Parameters.AddWithValue("paymentid", Convert.ToInt64(TransactionHeaderId));
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Update payment with  defpayid=" + PaymentTypeID + " and paymentid" + TransactionHeaderId.ToString ());
                        }
                        // Update ApptTable
                        using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                        {
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.CommandText = SynchEasyDentalQRY.Insert_paitent_PaymentLogNote;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("notetype", 1);
                            OdbcCommand.Parameters.AddWithValue("noteid", TransactionHeaderId);
                            OdbcCommand.Parameters.AddWithValue("notetext", drRow["template"].ToString());
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Insert paitent PaymentLogNote   noteid=" + TransactionHeaderId.ToString());
                        }
                    }

                    Is_Payment_successfull = false;
                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {
                        OdbcSelect = "";
                        AgingAmount = 0;
                        if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                        {
                            DateTime procdate;
                            if (OdbcDt.Rows.Count == 0 || OdbcDt.Rows[0]["lastpaydate"] == DBNull.Value)
                            {
                                procdate = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                            }
                            else if (Convert.ToDateTime(drRow["PaymentDate"].ToString()) < Convert.ToDateTime(OdbcDt.Rows[0]["lastpaydate"]))
                            {
                                procdate = Convert.ToDateTime(OdbcDt.Rows[0]["lastpaydate"]);
                                amount = Convert.ToDecimal(OdbcDt.Rows[0]["lastpayamount"]);
                            }
                            else
                            {
                                procdate = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                            }
                            AgingAmount = Convert.ToDecimal(aging0to30) + -Convert.ToDecimal(drRow["Amount"]);
                            OdbcSelect = " update aging_dat set aging0to30 =  ? ,lastpaydate = ?,lastpayamount = ? where guaratorid = ? ";
                            OdbcCommand.CommandText = OdbcSelect;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("amt", AgingAmount);
                            OdbcCommand.Parameters.AddWithValue("Procdate2", OdbcType.Date).Value = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                            OdbcCommand.Parameters.AddWithValue("amt1", Convert.ToDecimal(amount));
                            OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                        }
                        else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                        {
                            AgingAmount = Convert.ToDecimal(aging0to30) + Convert.ToDecimal(drRow["Amount"]);
                            OdbcSelect = " update aging_dat set aging0to30 =  ? where guaratorid = ? ";
                            OdbcCommand.CommandText = OdbcSelect;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("amt", AgingAmount);
                            OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                        }

                        CheckConnection(conn);
                        OdbcCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Update aging with  guarid=" + drRow["Guar_ID"].ToString());
                        Is_Payment_successfull = true;
                    }

                }
                catch (Exception ex)
                {
                    DeleteDataFromTable("Notes_dat" , "notetype = 1 and noteid = " + TransactionHeaderId, conn);
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " delete  Notes_dat  for notetype = 1 and noteid=" + TransactionHeaderId.ToString());
                    DeleteDataFromTable("proc_log_dat", "procedureid = " + TransactionHeaderId, conn);
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " delete  proc_log_dat for procedureid=" + TransactionHeaderId.ToString());
                    DeleteDataFromTable("payment_dat", "paymentid = " + TransactionHeaderId, conn);
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " delete  payment_dat for paymentid=" + TransactionHeaderId.ToString());
                    if (Is_Payment_successfull)
                    {
                        if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                        {
                            using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                            {
                                AgingAmount = Convert.ToDecimal(aging0to30) - -Convert.ToDecimal(drRow["Amount"]);
                                OdbcSelect = " update aging_dat set aging0to30 =  ? ,lastpaydate = ?,lastpayamount = ? where guaratorid = ? ";
                                OdbcCommand.CommandText = OdbcSelect;
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.Parameters.AddWithValue("amt", AgingAmount);
                                if (Deleteamount == 0)
                                {
                                    OdbcCommand.Parameters.AddWithValue("Procdate2", OdbcType.Date).Value = DBNull.Value;
                                }
                                else
                                {
                                    OdbcCommand.Parameters.AddWithValue("Procdate2", OdbcType.Date).Value = Convert.ToDateTime(Deleteprocdate);
                                }
                                OdbcCommand.Parameters.AddWithValue("amt1", Convert.ToDecimal(Deleteamount));
                                OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                                CheckConnection(conn);
                                OdbcCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Update aging_dat with guaratorid=" + drRow["Guar_ID"].ToString());
                            }
                        }
                        else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                        {
                            using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                            {
                                AgingAmount = Convert.ToDecimal(aging0to30) - Convert.ToDecimal(drRow["Amount"]);
                                OdbcSelect = " update aging_dat set aging0to30 =  ? where guaratorid = ? ";
                                OdbcCommand.CommandText = OdbcSelect;
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.Parameters.AddWithValue("amt", AgingAmount);
                                OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                                CheckConnection(conn);
                                OdbcCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Update aging_dat  with guaratorid=" + drRow["Guar_ID"].ToString());
                            }
                        }
                    }
                }
            }
              
            return TransactionHeaderId ; 
        }

        public static Int64 SavePaymentDiscount(DataRow drRow, string provid, decimal discount, Int64 TransactionHeaderId, decimal aging0to30, decimal Deleteamount, DateTime Deleteprocdate, DataTable OdbcDt, bool Is_Payment_successfull,bool Is_Discount_successfull,string _filename_EHR_Payment = "",string _EHRLogdirectory_EHR_Payment = "")
        {
            Int64 DiscountId = 0;
            using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
            {
                try
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();
                        OdbcCommand.CommandText = " " + SynchEasyDentalQRY.InsertProcLogAmount;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("patid", drRow["Guar_ID"].ToString());
                        OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                        OdbcCommand.Parameters.AddWithValue("date", OdbcType.Date).Value = Convert.ToDateTime(drRow["PaymentDate"].ToString());
                        OdbcCommand.Parameters.AddWithValue("proccodeid", drRow["PatientEHRId"].ToString());
                        OdbcCommand.Parameters.AddWithValue("class", 2);
                        OdbcCommand.Parameters.AddWithValue("Providerid", provid);
                        OdbcCommand.Parameters.AddWithValue("amt", -discount);
                        OdbcCommand.Parameters.AddWithValue("createdate", OdbcType.Date).Value = Convert.ToDateTime(DateTime.Now);
                        CheckConnection(conn);
                        OdbcCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Proc Log Amount with Providerid=" + provid + " and patid = " + drRow["Guar_ID"].ToString());
                    }
                    // Get Full procedure logid
                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();
                        OdbcCommand.CommandText = " Select procedureid from proc_log_dat";
                        OdbcCommand.CommandType = CommandType.Text;
                        DataTable OdbcDt1 = new DataTable();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt1);
                        }
                        DiscountId = Convert.ToInt64(OdbcDt1.AsEnumerable().Select(a => a.Field<object>("procedureid")).Max());
                        if (DiscountId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment, " Get Full procedure procedureid " + DiscountId.ToString());
                        }
                  }

                    Is_Discount_successfull = false;
                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {
                        decimal AgingAmount = 0;
                        AgingAmount = Convert.ToDecimal(aging0to30) + -discount;
                        string OdbcSelect = "";
                        OdbcSelect = " update aging_dat set aging0to30 =  ? where guaratorid = ? ";
                        OdbcCommand.CommandText = OdbcSelect;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("amt", AgingAmount);
                        OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                        CheckConnection(conn);
                        OdbcCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " update  aging_dat for  guarid =" + drRow["Guar_ID"].ToString());
                    }
                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        OdbcCommand.CommandText = SynchEasyDentalQRY.Insert_paitent_PaymentLogNote;
                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.AddWithValue("notetype", 1);
                        OdbcCommand.Parameters.AddWithValue("noteid", DiscountId);
                        OdbcCommand.Parameters.AddWithValue("notetext", drRow["template"].ToString());
                        CheckConnection(conn);
                        OdbcCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save paitent Payment Log Note with  noteid=" + DiscountId.ToString());
                    }
                    Is_Discount_successfull = true;
                }
                catch (Exception ex)
                {
                    DeleteDataFromTable("Notes_dat", "notetype = 1 and noteid = " + DiscountId, conn);
                    Utility.WriteSyncPullLog(_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment, "delete Notes_dat for notetype = 1 and noteid = " + DiscountId.ToString());
                    DeleteDataFromTable("proc_log_dat", "procedureid = " + DiscountId, conn);
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "delete proc_log_dat for procedureid=" + DiscountId.ToString());
                    DeleteDataFromTable("payment_dat", "paymentid = " + DiscountId, conn);
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "delete payment_dat for paymentid=" + DiscountId.ToString());
                    DeleteDataFromTable("Notes_dat", "notetype = 1 and noteid = " + TransactionHeaderId, conn);
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "delete Notes_dat for notetype = 1 and noteid = " + TransactionHeaderId.ToString());
                    DeleteDataFromTable("proc_log_dat", "procedureid = " + TransactionHeaderId, conn);
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "delete proc_log_dat for  procedureid=" + TransactionHeaderId.ToString());
                    DeleteDataFromTable("payment_dat", "paymentid = " + TransactionHeaderId, conn);
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "delete payment_dat for paymentid=" + TransactionHeaderId.ToString());
                    if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                    {
                        if (Is_Payment_successfull)
                        {
                            if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                                {
                                    string OdbcSelect = "";
                                    decimal AgingAmount = Convert.ToDecimal(aging0to30) - -Convert.ToDecimal(drRow["Amount"]);
                                    OdbcSelect = " update aging_dat set aging0to30 = ? ,lastpaydate = ?,lastpayamount = ? where guaratorid = ? ";
                                    OdbcCommand.CommandText = OdbcSelect;
                                    OdbcCommand.Parameters.Clear();
                                    OdbcCommand.Parameters.AddWithValue("amt", AgingAmount);
                                    if (Deleteamount == 0)
                                    {
                                        OdbcCommand.Parameters.AddWithValue("Procdate2", OdbcType.Date).Value = DBNull.Value;
                                    }
                                    else
                                    {
                                        OdbcCommand.Parameters.AddWithValue("Procdate2", OdbcType.Date).Value = Convert.ToDateTime(Deleteprocdate);
                                    }
                                    OdbcCommand.Parameters.AddWithValue("amt1", Convert.ToDecimal(Deleteamount));
                                    OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();
                                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Payment successfull , Update aging_dat for guarid=" + drRow["Guar_ID"].ToString().ToString());
                                }
                            }

                        }
                        if (Is_Discount_successfull)
                        {
                            using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                            {
                                decimal AgingAmount = 0;
                                AgingAmount = Convert.ToDecimal(aging0to30) - -discount;
                                string OdbcSelect = "";
                                OdbcSelect = " update aging_dat set aging0to30 = ? where guaratorid = ? ";
                                OdbcCommand.CommandText = OdbcSelect;
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.Parameters.AddWithValue("amt", AgingAmount);
                                OdbcCommand.Parameters.AddWithValue("guarid", drRow["Guar_ID"].ToString());
                                CheckConnection(conn);
                                OdbcCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Discount successfull, update aging_dat  query  (" + OdbcCommand.CommandText + ") for guarid=" + drRow["Guar_ID"].ToString().ToString());

                            }
                        }
                    }
                    OdbcDt = new DataTable();
                    Is_Payment_successfull = false;
                    Is_Discount_successfull = false;
                }
            }
                return DiscountId;
        }

        public static string GetPaymentNote(Int64 TransactionHeaderId)
        {
            Utility.WriteToErrorLogFromAll("In GetPayment Note mtd and TransactionHeaderId = "+ TransactionHeaderId.ToString());
            try
            {
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                {
                    string note = string.Empty;
                    
                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();
                        OdbcCommand.CommandText = " select text from Notes_dat where type = 1 and noteid in (" + TransactionHeaderId + ")";
                        note = Convert.ToString(OdbcCommand.ExecuteScalar());
                        return note;
                    }
                }
                
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error in GetPaymentNote " + ex.Message.ToString());
                return " ";
            }
        }

        public static string GetPatientGuarid(string Patid)
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
                OdbcCommand.CommandText = SynchEasyDentalQRY.GetpatientGurId;
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

        

        #endregion
        public static void DeleteDataFromTable(string tablename, string wherecondition, OdbcConnection conn)
        {
            using (OdbcCommand OdbcCommand = new OdbcCommand("Delete from " + tablename + " where " + wherecondition, conn))
            {
                OdbcCommand.CommandType = CommandType.Text;
                CheckConnection(conn);
                OdbcCommand.ExecuteNonQuery();
            }
        }

        public static DataTable GetEasyDentalPatientImagesData(string connectionString, string imagePathName)
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

                DataTable dtPatient = GetEasyDentalPatientID_NameData();
                string patientPicName = "";
                foreach (DataRow drRow in dtPatient.Rows)
                {
                    patientPicName = GetPatientImageBaseFileName(Convert.ToInt64(drRow["Patient_EHR_ID"]));
                    DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(imagePathName);
                    FileInfo oFileInfo = hdDirectoryInWhichToSearch.GetFiles(patientPicName + ".*").FirstOrDefault();

                    if (oFileInfo != null && oFileInfo.Exists)
                    {
                        DataRow drNew = dtPatientProfile.NewRow();
                        DateTime dtCreationTime = oFileInfo.LastAccessTime;
                        drNew["Patient_Images_LocalDB_ID"] = "";
                        drNew["Patient_Images_Web_ID"] = "";
                        drNew["Patient_Images_EHR_ID"] = oFileInfo.Name;
                        drNew["Patient_EHR_ID"] = drRow["Patient_EHR_ID"];
                        drNew["Patient_Web_ID"] = "";
                        drNew["Image_EHR_Name"] = "";
                        drNew["Patient_Images_FilePath"] = imagePathName + oFileInfo.Name;
                        drNew["Entry_DateTime"] = dtCreationTime.ToString();
                        drNew["AditApp_Entry_DateTime"] = DateTime.Now;
                        drNew["Is_Deleted"] = 0;
                        drNew["Is_Adit_Updated"] = 0;
                        drNew["Clinic_Number"] = 0;
                        drNew["Service_Install_Id"] = 1;
                        drNew["SourceLocation"] = imagePathName + oFileInfo.Name;
                        dtPatientProfile.Rows.Add(drNew);
                    }

                }
                dtPatientProfile.AcceptChanges();
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

        public static void DeleteDuplicatePatientLog()
        {
            //  bool is_Record_Update = false;
            string NoteId = "";

            DateTime datetimeTemp = DateTime.Now;
            using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
            {
                try
                {
                    DataTable dtDuplicateRecords = GetDuplicateRecords();
                    foreach (DataRow drRow in dtDuplicateRecords.Rows)
                    {
                        try
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                            {
                                OdbcCommand.CommandText = SynchEasyDentalQRY.DeleteDuplicateFullNoteLogs;
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.Parameters.AddWithValue("noteid", drRow["LogId"].ToString());
                                OdbcCommand.ExecuteNonQuery();
                            }

                            using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                            {
                                OdbcCommand.CommandText = SynchEasyDentalQRY.DeleteDuplicateContactLogs;
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.Parameters.AddWithValue("Contactid", drRow["LogId"].ToString());
                                OdbcCommand.ExecuteNonQuery();
                            }
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
        }

        public static DataTable GetDuplicateRecords()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
            {
                try
                {
                    // MySqlCommand.CommandTimeout = 120;

                    string strQauery = SynchEasyDentalQRY.GetDuplicateRecords;
                    using (OdbcCommand OdbcCommand = new OdbcCommand(strQauery, conn))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        // CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                        OdbcCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                        DataTable OdbcDt = new DataTable();
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                        return OdbcDt;
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

        public static string CheckSMSCallLogRecordsExists(DataRow drRow)
        {
            try
            {
                #region Check For the Records exists
                string noteId = "0";
                Int64 recordsCount = 0;
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                {
                    using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                    {

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OdbcCommand.CommandText = SynchEasyDentalQRY.CheckSMSCallRecordsBlankMobile;
                        // Utility.WriteToSyncLogFile_All("Mobile is blank for patient " + drRow["PatientEHRId"].ToString());


                        //Utility.WriteToSyncLogFile_All("Check Duplicate Records " + MySqlCommand.CommandText);

                        OdbcCommand.Parameters.Clear();
                        OdbcCommand.Parameters.Add("Contactdate", OdbcType.Date).Value = Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString());
                        OdbcCommand.Parameters.AddWithValue("Contacttime", Convert.ToInt32(Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString()).TimeOfDay.TotalMilliseconds.ToString().Substring(0, 5)));
                        OdbcCommand.Parameters.AddWithValue("patid", drRow["PatientEHRId"].ToString());
                        OdbcCommand.Parameters.AddWithValue("Provid", Utility.EHR_UserLogin_ID);
                        if (drRow["message_type"].ToString().ToLower() == "call" || drRow["message_type"].ToString().ToLower() == "appointment_reminder_call")
                        {
                            OdbcCommand.Parameters.AddWithValue("contacttype", 3);
                        }
                        else
                        {
                            OdbcCommand.Parameters.AddWithValue("contacttype", 5);
                        }
                        OdbcCommand.Parameters.AddWithValue("notetext", drRow["text"].ToString());
                        //OdbcCommand.Parameters.Add("Contactd", OdbcType.Date).Value = Convert.ToDateTime(new DateTime(2021, 11, 15));
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
                    }
                }
                #endregion

                #region Check and Delete duplicate Records
                if (noteId != "0" && recordsCount > 1)
                {
                    using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString))
                    {
                        using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            OdbcCommand.CommandText = SynchEasyDentalQRY.DeleteDuplicateFullNoteLogs;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("noteid", noteId);
                            OdbcCommand.ExecuteNonQuery();
                        }
                        using (OdbcCommand OdbcCommand = new OdbcCommand("", conn))
                        {
                            OdbcCommand.CommandText = SynchEasyDentalQRY.DeleteDuplicateContactLogs;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("Contactid", noteId);
                            OdbcCommand.ExecuteNonQuery();
                        }
                    }
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

        #region "Medication"
        public static DataTable GetEasyDentalMedicationData()
        {
            DataTable DtMedication = new DataTable();
            DataTable OdbcDt = new DataTable();
            DataTable OdbcDtNotes = new DataTable();
            try
            {
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalMedicationData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                OdbcDtNotes = GetEasyDentalMedicationNotesData();

                DtMedication = OdbcDt.Clone();
                DtMedication.Columns["Medication_Notes"].DataType = typeof(string);
                DtMedication.Columns["Medication_Sig"].DataType = typeof(string);
                foreach (DataRow row in OdbcDt.Rows)
                {
                    DataRow newRow = DtMedication.NewRow();
                    string notetext = "";
                    if (row["Medication_Notes"] != DBNull.Value)
                    {
                        if (row["Medication_Notes"].ToString().Trim() != "")
                        {
                            DataRow[] dr = OdbcDtNotes.Select("Type = 34 And NoteID = " + row["Medication_Notes"].ToString());
                            if (dr.Length > 0)
                            {
                                notetext = dr[0]["Text"].ToString();
                            }
                        }
                    }

                    string sigtext = "";
                    if (row["Medication_Sig"] != DBNull.Value)
                    {
                        if (row["Medication_Sig"].ToString().Trim() != "")
                        {
                            DataRow[] dr = OdbcDtNotes.Select("Type = 35 And NoteID = " + row["Medication_Sig"].ToString());
                            if (dr.Length > 0)
                            {
                                sigtext = dr[0]["Text"].ToString();
                            }
                        }
                    }

                    newRow["Medication_EHR_ID"] = row["Medication_EHR_ID"];
                    newRow["Medication_Name"] = row["Medication_Name"];
                    newRow["Medication_Description"] = row["Medication_Description"];
                    newRow["Medication_Notes"] = notetext;
                    newRow["Medication_Sig"] = sigtext;
                    newRow["Medication_Parent_EHR_ID"] = row["Medication_Parent_EHR_ID"];
                    newRow["Medication_Type"] = row["Medication_Type"];
                    newRow["Allow_Generic_Sub"] = row["Allow_Generic_Sub"];
                    newRow["Drug_Quantity"] = row["Drug_Quantity"];
                    newRow["Refills"] = row["Refills"];
                    newRow["Is_Active"] = row["Is_Active"];
                    newRow["EHR_Entry_DateTime"] = row["EHR_Entry_DateTime"];
                    newRow["Medication_Provider_ID"] = row["Medication_Provider_ID"];
                    newRow["is_deleted"] = row["is_deleted"];
                    newRow["Is_Adit_Updated"] = row["Is_Adit_Updated"];
                    newRow["Clinic_Number"] = row["Clinic_Number"];
                    DtMedication.Rows.Add(newRow);
                    DtMedication.AcceptChanges();
                }

                return DtMedication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable GetEasyDentalMedicationNotesData()
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalMedicationNotesData.Trim();
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                return OdbcDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool SavePatientMedicationLocalToEasyDental()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            Int64 MedicationPatientId = 0;
            Int64 MedicationNum = 0;
            string OdbcSelect = "";
            try
            {
                DataTable dtWebPatient_FormDisease = SynchLocalDAL.GetLocalPatientFormMedicationResponseToSaveINEHR("1");
                if (dtWebPatient_FormDisease != null)
                {
                    if (dtWebPatient_FormDisease.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWebPatient_FormDisease.Rows)
                        {
                            string sqlSelect = string.Empty;
                            CommonDB.OdbcConnectionServer(ref conn);
                            CommonDB.OdbcCommandServer(sqlSelect, conn, ref OdbcCommand, "txt");

                            if (dr["Medication_EHR_Id"] == DBNull.Value) dr["Medication_EHR_Id"] = "";
                            if (dr["Medication_EHR_Id"].ToString().Trim() == "" || dr["Medication_EHR_Id"].ToString().Trim() == "0")
                            {
                                OdbcSelect = "";
                            }

                            OdbcCommand.CommandText = SynchEasyDentalQRY.InsertEasyDentalDiseaseData;
                            OdbcCommand.Parameters.Clear();
                            OdbcCommand.Parameters.AddWithValue("patid", dr["PatientEHRID"].ToString());
                            OdbcCommand.Parameters.AddWithValue("hhitemid", dr["Disease_EHR_ID"].ToString());
                            OdbcCommand.Parameters.AddWithValue("note", "Web");
                            CheckConnection(conn);
                            OdbcCommand.ExecuteNonQuery();

                            string DiseaseId = "";
                            string QryIdentity = "Select max(linkid) as newId from admin.hhlinkpat_item";
                            OdbcCommand.CommandText = QryIdentity;
                            OdbcCommand.CommandType = CommandType.Text;
                            OdbcCommand.Connection = conn;
                            CheckConnection(conn);
                            DiseaseId = Convert.ToString(OdbcCommand.ExecuteScalar());
                            SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), DiseaseId.ToString() == "" ? dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString() : DiseaseId.ToString(), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), "1");
                            //SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["DiseaseResponse_Local_ID"].ToString(), DiseaseId, dr["Patient_EHR_Id"].ToString(), "1");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
        }
        
        //SaveMedicationLocalToEasyDental
        public static bool SaveMedicationLocalToEasyDental(ref bool isRecordSaved, ref string SavePatientEHRID)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            Int64 MedicationPatientId = 0;
            Int64 MedicationNum = 0;
            string odbcSelect = "";
            string ProviderID = "";
            DataTable dtMedication = new DataTable();
            DataTable dtPatientMedication = new DataTable();
            SavePatientEHRID = "";
            try
            {
                DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationResponseToSaveINEHR("1");
                if (dtPatientMedicationResponse != null)
                {
                    if (dtPatientMedicationResponse.Rows.Count > 0)
                    {
                        dtMedication = GetEasyDentalMedicationData();
                        dtPatientMedication = GetEasyDentalPatientMedication("");
                        foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                        {
                            MedicationPatientId = 0;
                            MedicationNum = 0;

                            string Note = "", OrgNote = "";
                            OrgNote = dr["Medication_Note"].ToString().Trim();
                            MedicationNum = Convert.ToInt64(dr["Medication_EHR_ID"].ToString().Trim());
                            //DataRow[] drMedRow = dtMedication.Copy().Select("Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "' And Medication_Notes = '" + dr["Medication_Note"].ToString().Trim() + "'");
                            DataRow[] drMedRow = dtMedication.Copy().Select("Medication_EHR_ID = " + MedicationNum.ToString().Trim());
                            if (drMedRow.Length > 0)
                            {
                                //MedicationNum = Convert.ToInt64(drMedRow[0]["Medication_EHR_ID"]);
                                Note = Convert.ToString(drMedRow[0]["Medication_Notes"]).Trim();
                            }

                            bool blnAddInactiveMedication = false;
                            if (Note.Trim().ToUpper() != OrgNote.Trim().ToUpper())
                            {
                                MedicationNum = 0;
                                blnAddInactiveMedication = true;
                                DataRow[] drPatMedRow = dtPatientMedication.Copy().Select("Patient_EHR_ID = " + dr["PatientEHRId"].ToString().Trim() + " And Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "' And Is_Active = true");
                                if (drPatMedRow.Length > 0)
                                {
                                    MedicationNum = Convert.ToInt64(drPatMedRow[0]["Medication_EHR_ID"]);
                                    blnAddInactiveMedication = false;
                                    DataRow[] drMedRow1 = dtMedication.Copy().Select("Medication_EHR_ID = " + MedicationNum + " And Is_Active = false");
                                    if (drMedRow1.Length <= 0)
                                    {
                                        MedicationNum = 0;
                                        blnAddInactiveMedication = true;
                                    }
                                }
                                //else
                                //{
                                //    MedicationNum = 0;
                                //}
                            }

                            if (dr["Medication_EHR_Id"] == DBNull.Value) dr["Medication_EHR_Id"] = "";
                            if (dr["Medication_EHR_Id"].ToString().Trim() == "" || dr["Medication_EHR_Id"].ToString().Trim() == "0" || blnAddInactiveMedication)
                            {
                                MedicationNum = 0;

                                if (MedicationNum <= 0)
                                {
                                    if ((Note.Trim().ToUpper() != OrgNote.Trim().ToUpper()))
                                    {
                                        odbcSelect = SynchEasyDentalQRY.InsertMedicationInactive;
                                        odbcSelect = odbcSelect.Replace("@Medication_Name", "'" + dr["Medication_Name"].ToString().Trim() + "'");
                                        if (drMedRow.Length > 0)
                                        { odbcSelect = odbcSelect.Replace("@Medication_Description", "'" + drMedRow[0]["Medication_Description"].ToString().Trim() + "'"); }
                                        else
                                        { odbcSelect = odbcSelect.Replace("@Medication_Description", "'" + dr["Medication_Name"].ToString().Trim() + "'"); }
                                    }
                                    else
                                    {
                                        Utility.WriteToErrorLogFromAll("Medication : " + dr["Medication_Name"].ToString().Trim() + " not found. Medication ID: " + dr["Medication_EHR_ID"].ToString().Trim());
                                        continue;

                                        odbcSelect = SynchEasyDentalQRY.InsertMedication;
                                        odbcSelect = odbcSelect.Replace("@Medication_Name", "'" + dr["Medication_Name"].ToString().Trim() + "'");
                                    }

                                    CommonDB.OdbcConnectionServer(ref conn);
                                    if (conn.State != ConnectionState.Open) conn.Open();
                                    CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    OdbcCommand.Parameters.Clear();
                                    OdbcCommand.Parameters.Add("CreateDate", OdbcType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
                                    CheckConnection(conn);
                                    OdbcCommand.ExecuteNonQuery();

                                    odbcSelect = " Select max(RxDefID) from RxDef_dat";
                                    CommonDB.OdbcConnectionServer(ref conn);
                                    if (conn.State != ConnectionState.Open) conn.Open();
                                    CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                    OdbcCommand.Parameters.Clear();
                                    CheckConnection(conn);
                                    MedicationNum = Convert.ToInt64(OdbcCommand.ExecuteScalar());

                                    if (MedicationNum > 0)
                                    {
                                        odbcSelect = " INSERT INTO Notes_dat(type,noteid,text,Date)VALUES(34,@Medication_EHR_ID,@Medication_Note,?)";
                                        odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", MedicationNum.ToString().Trim());
                                        odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + dr["Medication_Note"].ToString().Trim() + "'");
                                        OdbcCommand.CommandText = odbcSelect;
                                        OdbcCommand.Parameters.Clear();
                                        OdbcCommand.Parameters.Add("Date", OdbcType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
                                        OdbcCommand.ExecuteNonQuery();

                                        DataRow[] drSigMedRow = dtMedication.Copy().Select("Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "' And is_active = true ");
                                        string strSig = "";
                                        if (drSigMedRow.Length > 0)
                                        {
                                            strSig = Convert.ToString(drSigMedRow[0]["Medication_Sig"]).Trim();
                                        }

                                        odbcSelect = " INSERT INTO Notes_dat(type,noteid,text,Date)VALUES(35,@Medication_EHR_ID,@Medication_Note,?)";
                                        odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", MedicationNum.ToString().Trim());
                                        odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + strSig + "'"); //Medication_Sig
                                        OdbcCommand.CommandText = odbcSelect;
                                        OdbcCommand.Parameters.Clear();
                                        OdbcCommand.Parameters.Add("Date", OdbcType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
                                        OdbcCommand.ExecuteNonQuery();

                                        DataRow newRow = dtMedication.NewRow();
                                        newRow["Medication_EHR_ID"] = MedicationNum;
                                        newRow["Medication_Name"] = dr["Medication_Name"].ToString().Trim();
                                        newRow["Medication_Description"] = dr["Medication_Name"].ToString().Trim();
                                        newRow["Medication_Notes"] = dr["Medication_Note"].ToString().Trim();
                                        dtMedication.Rows.Add(newRow);
                                        dtMedication.AcceptChanges();

                                        odbcSelect = " Update RxDef_dat set SigNoteID = " + MedicationNum + ", RxNoteID = " + MedicationNum + " where RxDefID = " + MedicationNum;
                                        OdbcCommand.CommandText = odbcSelect;
                                        OdbcCommand.Parameters.Clear();
                                        OdbcCommand.ExecuteNonQuery();
                                    }
                                }
                                dr["Medication_EHR_ID"] = MedicationNum.ToString().Trim();
                            }
                            else
                            {
                                if (MedicationNum <= 0)
                                {
                                    MedicationNum = Convert.ToInt64(dr["Medication_EHR_ID"].ToString().Trim());
                                }
                            }

                            if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "0";
                            if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" && dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                            {
                                MedicationPatientId = Convert.ToInt64(dr["PatientMedication_EHR_Id"]);
                            }
                            if (MedicationPatientId <= 0)
                            {
                                string strSelect = "Patient_EHR_ID = " + dr["PatientEHRID"].ToString().Trim() +
                                            " And Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "' And is_active='True' ";

                                DataRow[] drPatMedRow = dtPatientMedication.Copy().Select(strSelect);
                                if (drPatMedRow.Length == 1)
                                {
                                    MedicationPatientId = Convert.ToInt64(drPatMedRow[0]["PatientMedication_EHR_ID"].ToString().Trim());
                                }
                                else if (drPatMedRow.Length > 1)
                                {
                                    foreach (DataRow drMed in drPatMedRow)
                                    {
                                        DataRow[] rowMed = dtMedication.Copy().Select("Medication_EHR_ID = " + drMed["Medication_EHR_ID"].ToString().Trim() + " And Is_Active = false");
                                        if (rowMed.Length > 0)
                                        {
                                            MedicationPatientId = Convert.ToInt64(drMed["PatientMedication_EHR_ID"].ToString().Trim());
                                        }
                                    }
                                }
                            }

                            if (MedicationPatientId <= 0)
                            {
                                #region Get Primary Provider
                                odbcSelect = " Select Provid1, Provid2 from pat_dat where PatID = @Patient_EHR_ID";
                                odbcSelect = odbcSelect.Replace("@Patient_EHR_ID", dr["PatientEHRId"].ToString().Trim());
                                CommonDB.OdbcConnectionServer(ref conn);
                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                OdbcCommand.Parameters.Clear();
                                CheckConnection(conn);
                                DataTable OdbcDt = new DataTable();
                                using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                                {
                                    OdbcDa.Fill(OdbcDt);
                                }
                                if (OdbcDt.Rows.Count > 0)
                                {
                                    ProviderID = OdbcDt.Rows[0]["Provid1"] != DBNull.Value ? Convert.ToString(OdbcDt.Rows[0]["Provid1"]) : (OdbcDt.Rows[0]["Provid2"] != DBNull.Value ? Convert.ToString(OdbcDt.Rows[0]["Provid2"]) : "null");
                                }
                                #endregion

                                odbcSelect = SynchEasyDentalQRY.InsertPatientMedication;
                                odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", dr["Medication_EHR_ID"].ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@Patient_EHR_ID", dr["PatientEHRId"].ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@Provider_EHR_ID", ProviderID.Trim() != "" ? "'" + ProviderID.ToString().Trim() + "'" : "null");
                                CommonDB.OdbcConnectionServer(ref conn);
                                if (conn.State != ConnectionState.Open) conn.Open();
                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.Parameters.Add("date", OdbcType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
                                CheckConnection(conn);
                                OdbcCommand.ExecuteNonQuery();

                                odbcSelect = " Select Max(RxId) from RxPat_dat where PatId = @Patient_EHR_ID and RxDefId = @Medication_EHR_ID";
                                odbcSelect = odbcSelect.Replace("@Patient_EHR_ID", dr["PatientEHRId"].ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", dr["Medication_EHR_ID"].ToString().Trim());
                                OdbcCommand.CommandText = odbcSelect;
                                OdbcCommand.Parameters.Clear();
                                CommonDB.OdbcConnectionServer(ref conn);
                                if (conn.State != ConnectionState.Open) conn.Open();
                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                OdbcCommand.Parameters.Clear();
                                MedicationPatientId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                dr["PatientMedication_EHR_ID"] = MedicationPatientId.ToString().Trim();

                                DataRow NewRow = dtPatientMedication.NewRow();
                                NewRow["Patient_EHR_ID"] = dr["PatientEHRID"].ToString();
                                NewRow["Medication_EHR_ID"] = MedicationNum.ToString();
                                NewRow["PatientMedication_EHR_ID"] = MedicationPatientId.ToString();
                                NewRow["Medication_Name"] = dr["Medication_Name"].ToString();
                                //NewRow["Medication_Type"] = "Drug";
                                NewRow["Medication_Note"] = dr["Medication_Note"].ToString();
                                NewRow["is_active"] = "True";

                                dtPatientMedication.Rows.Add(NewRow);
                                dtPatientMedication.AcceptChanges();

                                dr["PatientMedication_EHR_ID"] = MedicationPatientId.ToString().Trim();
                            }
                            else
                            {
                                odbcSelect = " Update RxPat_dat set RxDefID = @Medication_EHR_ID where RxID = @PatientMedication_EHR_ID";
                                odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", MedicationPatientId.ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", MedicationNum.ToString().Trim());
                                OdbcCommand.CommandText = odbcSelect;
                                OdbcCommand.Parameters.Clear();
                                CommonDB.OdbcConnectionServer(ref conn);
                                if (conn.State != ConnectionState.Open) conn.Open();
                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.ExecuteScalar();

                                odbcSelect = " Update Notes_dat set text = '@Medication_Note' where NoteID in (@Medication_EHR_ID) and Type = 34";
                                odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", MedicationNum.ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@Medication_Note", dr["Medication_Note"].ToString().Trim());
                                OdbcCommand.CommandText = odbcSelect;
                                OdbcCommand.Parameters.Clear();
                                CommonDB.OdbcConnectionServer(ref conn);
                                if (conn.State != ConnectionState.Open) conn.Open();
                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                OdbcCommand.Parameters.Clear();
                                OdbcCommand.ExecuteScalar();
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
        }
        public static bool DeleteMedicationLocalToEasyDental(ref bool isRecordDeleted, ref string DeletePatientEHRID)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            string odbcSelect = "";
            DeletePatientEHRID = "";
            try
            {
                DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationRemovedResponseToSaveINEHR("1");
                if (dtPatientMedicationResponse != null)
                {
                    if (dtPatientMedicationResponse.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                        {
                            if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "";

                            if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" || dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                            {
                                odbcSelect = SynchEasyDentalQRY.DeletePatientMedication;
                                odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", dr["PatientMedication_EHR_ID"].ToString().Trim());
                                CommonDB.OdbcConnectionServer(ref conn);
                                if (conn.State != ConnectionState.Open) conn.Open();
                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                OdbcCommand.Parameters.Clear();
                                CheckConnection(conn);
                                OdbcCommand.ExecuteNonQuery();

                                if (!DeletePatientEHRID.ToUpper().Trim().Contains("'" + dr["PatientEHRID"].ToString().Trim() + "'"))
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
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Dispose();
                }
            }
        }

        public static DataTable GetEasyDentalPatientMedication(string Patient_EHR_IDS)
        {
            try
            {
                DataTable OdbcDt = new DataTable();
                DataTable OdbcDtMasterData = new DataTable();
                using (OdbcConnection conn = new OdbcConnection(Utility.DBConnString.Trim()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = "";
                    if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                    {
                        OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientMedicationData.Trim();
                    }
                    else 
                    {
                        OdbcSelect = " " + SynchEasyDentalQRY.GetEasyDentalPatientMedicationData.Trim() + " where PatID IN ("+ Patient_EHR_IDS.Replace("'","") +")";
                    }   
                    using (OdbcCommand OdbcCommand = new OdbcCommand(OdbcSelect, conn))
                    {
                        OdbcCommand.CommandType = CommandType.Text;
                        using (OdbcDataAdapter OdbcDa = new OdbcDataAdapter(OdbcCommand))
                        {
                            OdbcDa.Fill(OdbcDt);
                        }
                    }
                }
                OdbcDtMasterData = GetEasyDentalMedicationData(); //GetEasyDentalPatMedicationDataFromMaster();
                DataTable DtPatMedication = new DataTable();
                DtPatMedication = OdbcDt.Clone();

                foreach (DataRow row in OdbcDt.Rows)
                {
                    DataRow newRow = DtPatMedication.NewRow();
                    DataRow[] dr = OdbcDtMasterData.Select("Medication_EHR_ID = " + row["Medication_EHR_ID"].ToString());
                    string MedicationName = "";
                    string Medication_SIG = "";
                    string Patient_Notes = "";
                    string drug_quantity = "";
                    string refills = "";
                    if (dr.Length > 0)
                    {
                        MedicationName = dr[0]["Medication_Name"].ToString();
                        Medication_SIG = dr[0]["Medication_SIG"].ToString();
                        drug_quantity = dr[0]["Drug_Quantity"].ToString();
                        Patient_Notes = dr[0]["Medication_Notes"].ToString();
                        refills = dr[0]["Refills"].ToString();
                    }

                    newRow["PatientMedication_EHR_ID"] = row["PatientMedication_EHR_ID"];
                    newRow["Medication_EHR_ID"] = row["Medication_EHR_ID"];
                    newRow["Medication_Name"] = MedicationName;
                    //newRow["MedicalDescription"] = Medication_Description;
                    newRow["Medication_Note"] = Medication_SIG;
                    newRow["Patient_Notes"] = Patient_Notes;
                    newRow["Patient_EHR_ID"] = row["Patient_EHR_ID"];
                    newRow["Provider_EHR_ID"] = row["Provider_EHR_ID"];
                    newRow["Medication_Type"] = row["Medication_Type"];
                    newRow["Start_Date"] = row["Start_Date"];
                    newRow["End_Date"] = row["End_Date"];
                    newRow["Drug_Quantity"] = drug_quantity;
                    newRow["Refills"] = refills;
                    newRow["EHR_Entry_DateTime"] = row["EHR_Entry_DateTime"];
                    newRow["Last_Sync_Date"] = row["Last_Sync_Date"];
                    newRow["is_deleted"] = row["is_deleted"];
                    newRow["Clinic_Number"] = row["Clinic_Number"];
                    newRow["is_active"] = row["is_active"];
                    DtPatMedication.Rows.Add(newRow);
                    DtPatMedication.AcceptChanges();
                }
                return DtPatMedication;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
    public class EasyDentalQuestionIds
    {
        public string EasyDental_Form_EHRUnique_ID { get; set; }
        public string EasyDental_Question_EHRUnique_ID { get; set; }
        public string EasyDental_Question_EHR_ID { get; set; }
        public string EasyDental_QuestionsTypeId { get; set; }
        public string EasyDental_ResponsetypeId { get; set; }
    }


}
