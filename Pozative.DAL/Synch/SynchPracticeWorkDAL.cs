using Pozative.QRY;
using Pozative.UTL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.DAL
{
    public class SynchPracticeWorkDAL
    {
        public static bool GetPracticeWorkConnection()
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

        public static bool Save_PracticeWork_To_Local(DataTable dtSoftDentDataToSave, string tablename, string ignoreColumnsName, string primaryKeyColumnsName)
        {
            throw new NotImplementedException();
        }

        public static bool Save_Appointment_PracticeWork_To_Local(DataTable dtDentrixAppointment)
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

                        foreach (DataRow dr in dtDentrixAppointment.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Appointment;

                                        break;
                                    case 5:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Contact;

                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID;

                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                        break;
                                }
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", dr["Appt_Web_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("MI", dr["MI"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Home_Contact", dr["Home_Contact"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", dr["Mobile_Contact"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Address", dr["Address"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ST", dr["ST"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Zip", dr["Zip"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Provider_Name", dr["Provider_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ApptType", dr["ApptType"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("birth_date", dr["birth_date"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", dr["Appt_DateTime"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", dr["Appt_EndDateTime"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Status", dr["Status"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Patient_Status", dr["Patient_Status"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Appointment_Status", dr["Appointment_Status"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", dr["confirmed_status_ehr_key"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("confirmed_status", dr["confirmed_status"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", dr["unschedule_status_ehr_key"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("unschedule_status", dr["unschedule_status"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Appt", dr["Is_Appt"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Appt_LocalDB_ID", dr["Appt_LocalDB_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("is_asap", Convert.ToBoolean(dr["is_asap"].ToString().Trim()));
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
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }


        public static bool Update_Status_EHR_Appointment_Live_To_PracticeWorkEHR(DataTable dtLiveAppointment, string _filename_EHR_appointment = "", string _EHRLogdirectory_EHR_appointment = "")
        {
            bool _successfullstataus = true;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;

                foreach (DataRow dr in dtLiveAppointment.Rows)
                {

                    string OdbcSelect = SynchPracticeWorkQRY.UpdateAppointmentStatus;

                    if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 2)
                    {
                        OdbcSelect = OdbcSelect.Replace("@FieldToUpdate", " " + "\"Confirmed date\"" + " = '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    }
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 3)
                    {
                        OdbcSelect = OdbcSelect.Replace("@FieldToUpdate", " " + "\"Check in time\"" + " = '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    }
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 4)
                    {
                        OdbcSelect = OdbcSelect.Replace("@FieldToUpdate", " " + "\"Seated time\"" + " = '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    }
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 5)
                    {
                        OdbcSelect = OdbcSelect.Replace("@FieldToUpdate", " " + "\"Check out time \"" + " = '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "' ");
                    }

                    OdbcSelect = OdbcSelect.Replace("@AppointmentId", dr["Appt_EHR_ID"].ToString());

                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    Utility.WriteToSyncLogFile_All("Update Status " + OdbcSelect);

                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    OdbcCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Confirmed EHR Appointment Live To PracticeWorkEHR  for confirmed_status_ehr_key=" + dr["confirmed_status_ehr_key"] + " and AppointmentId=" + dr["Appt_EHR_ID"].ToString());
                    Utility.WriteToSyncLogFile_All("Status Updated");

                    try
                    {
                        bool isApptConformStatus = SynchLocalDAL.UpdateLocalAppointmentConformStatusData(dr["Appt_EHR_ID"].ToString(), dr["Service_Install_Id"].ToString());
                        if (isApptConformStatus)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Local Appointment Conform : success for  AppointmentId: (" + dr["Appt_EHR_ID"] + ")");
                        }
                    }
                    catch (Exception)
                    {
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

        public static long Save_Patient_Local_To_PracticeWork(string LastName, string FirstName, string MiddleName, string MobileNo, string Email, string ApptProv, string AppointmentDateTime, Int64 Patient_Gur_id, int OperatoryId, string Birth_Date)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                Utility.WriteToSyncLogFile_All("Patient_Start");
                Int64 Patient_Id = GetMaxPatientIdFromPatientTable();
                string OdbcSelect = SynchPracticeWorkQRY.InsertPracticeWorkPatientFile;

                if (Patient_Gur_id != null && Patient_Gur_id.ToString() != string.Empty && Patient_Gur_id == 0)
                {
                    Patient_Gur_id = Patient_Id;
                }
                if (LastName.Length == 0)
                {
                    LastName += "NA";
                }


                OdbcSelect = OdbcSelect.Replace("@PatientId", Patient_Id.ToString());
                OdbcSelect = OdbcSelect.Replace("@LastName", "'" + LastName + "'");
                OdbcSelect = OdbcSelect.Replace("@FirstName", "'" + FirstName + "'");
                OdbcSelect = OdbcSelect.Replace("@GuarId", Patient_Gur_id.ToString());
                OdbcSelect = OdbcSelect.Replace("@Full_time_Student", "0");
                OdbcSelect = OdbcSelect.Replace("@School_name", "''");
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                Utility.WriteToSyncLogFile_All("Save Patient File " + OdbcSelect);

                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcCommand.ExecuteNonQuery();
                Utility.WriteToSyncLogFile_All("Patient File Save");

                try
                {


                    OdbcSelect = SynchPracticeWorkQRY.InsertPracticeWorkPersonFile;
                    Utility.WriteToSyncLogFile_All("Save Person File " + OdbcSelect);
                    OdbcSelect = OdbcSelect.Replace("@Filler", "'" + MobileNo + "'");
                    if (Utility.Is_PWPatientCellPhoneAvailable)
                    {
                        OdbcSelect = OdbcSelect.Replace("Filler", "CellPhone");
                    }
                    OdbcSelect = OdbcSelect.Replace("@PatientId", Patient_Id.ToString());
                    OdbcSelect = OdbcSelect.Replace("@LastName", "'" + LastName + "'");
                    OdbcSelect = OdbcSelect.Replace("@FirstName", "'" + FirstName + "'");
                    OdbcSelect = OdbcSelect.Replace("@MiddleName", "'" + MiddleName + "'");
                    OdbcSelect = OdbcSelect.Replace("@LegalName", "'" + FirstName + "'");
                    OdbcSelect = OdbcSelect.Replace("@Address1", "''");
                    OdbcSelect = OdbcSelect.Replace("@Address2", "''");
                    OdbcSelect = OdbcSelect.Replace("@City", "''");
                    OdbcSelect = OdbcSelect.Replace("@State", "''");
                    OdbcSelect = OdbcSelect.Replace("@Zip", "''");
                    OdbcSelect = OdbcSelect.Replace("@HomePhone", "''");
                    OdbcSelect = OdbcSelect.Replace("@WorkPhone1", "''");
                    OdbcSelect = OdbcSelect.Replace("@WorkPhone2", "''");
                    OdbcSelect = OdbcSelect.Replace("@OfficePhone", "''");
                    OdbcSelect = OdbcSelect.Replace("@Email", "'" + Email + "'");
                    try
                    {
                        OdbcSelect = OdbcSelect.Replace("@BirthDate", "'" + Convert.ToDateTime(Birth_Date).ToString("yyyy-MM-dd") + "'");
                    }
                    catch (Exception)
                    {
                        OdbcSelect = OdbcSelect.Replace("@BirthDate", "NULL");
                    }

                    OdbcSelect = OdbcSelect.Replace("@Sex", "''");
                    OdbcSelect = OdbcSelect.Replace("@MaritalStatus", "''");
                    OdbcSelect = OdbcSelect.Replace("@SSN", "''");
                    Utility.WriteToSyncLogFile_All("Save Person File " + OdbcSelect);
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");

                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    OdbcCommand.ExecuteNonQuery();
                    Utility.WriteToSyncLogFile_All("Person File Save");
                }
                catch(Exception ex)
                {
                    Utility.WriteToErrorLogFromAll("TestLog Save_patient_local_to_practiceWork Inser Person file :- " + ex.Message);
                }

                OdbcSelect = SynchPracticeWorkQRY.UpdateNextPatientId;
                OdbcSelect = OdbcSelect.Replace("@PatientIdNew", "'" + (Patient_Id + 1) + "'");
                OdbcSelect = OdbcSelect.Replace("@PatientId", "'" + Patient_Id + "'");
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcCommand.ExecuteNonQuery();

                return Patient_Id;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Error While Save Patient File Or Person File " + ex.Message);
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static long Save_Appointment_Local_To_PracticeWork(string FirstNameLastName, DateTime AppointmentStartTime, DateTime AppointmentEndTime, string PatNum, string OperatoryId, string classification, string ApptTypeId, DateTime AppointedDateTime, string ProvNum, string AppointmentConfirmationStatus, bool allday_event, bool sooner_if_possible, bool privateAppointment, bool auto_confirm_sent)
        {
            //Utility.WriteToErrorLogFromAll("In Save_Appointment_Local_To_PracticeWork");
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                Int64 AppointmentId = GetMaxAppointmentId();
                //Utility.WriteToErrorLogFromAll("AppointmentId id is=="+AppointmentId.ToString());
                string OdbcSelect = SynchPracticeWorkQRY.InsertPracticeWorkAppointment;
                Utility.WriteToSyncLogFile_All("Save Appointment File Appointment Start & End TIme " + Convert.ToDateTime(AppointmentStartTime).ToString("HH:mm").ToString() + " " + Convert.ToDateTime(AppointmentEndTime).ToString("HH:mm").ToString());
                OdbcSelect = OdbcSelect.Replace("@AppointmentDate", "'" + Convert.ToDateTime(AppointmentStartTime).ToString("yyyy-MM-dd") + "'");
                //Utility.WriteToErrorLogFromAll("AppointmentDate ==" + Convert.ToDateTime(AppointmentStartTime).ToString("yyyy-MM-dd") );
                OdbcSelect = OdbcSelect.Replace("@OperatoryId", OperatoryId.ToString());
                //Utility.WriteToErrorLogFromAll("OperatoryId is==" + OperatoryId.ToString());
                OdbcSelect = OdbcSelect.Replace("@StartDate", "'" + Convert.ToDateTime(AppointmentStartTime).ToString("HH:mm") + "'");
                //Utility.WriteToErrorLogFromAll("StartDate ==" + Convert.ToDateTime(AppointmentStartTime).ToString("HH:mm") );
                OdbcSelect = OdbcSelect.Replace("@EndDate", "'" + Convert.ToDateTime(AppointmentEndTime.AddMinutes(-1)).ToString("HH:mm") + "'");
                //Utility.WriteToErrorLogFromAll("EndDate ==" +Convert.ToDateTime(AppointmentEndTime.AddMinutes(-1)).ToString("HH:mm"));
                OdbcSelect = OdbcSelect.Replace("@AppointmentId", AppointmentId.ToString());
                OdbcSelect = OdbcSelect.Replace("@ProviderId", ProvNum.ToString());
                OdbcSelect = OdbcSelect.Replace("@PatientId", PatNum.ToString());
                OdbcSelect = OdbcSelect.Replace("@TaxClassId", "7");
                //Utility.WriteToSyncLogFile_All("Save Appointment File " + OdbcSelect.ToString());
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                //OdbcCommand.Parameters.Add("@AppointmentDate", AppointmentId.ToString());
                //OdbcCommand.Parameters.Add("@OperatoryId", AppointmentId.ToString());
                //OdbcCommand.Parameters.Add("@StartDate", AppointmentId.ToString());
                //OdbcCommand.Parameters.Add("@EndDate", AppointmentId.ToString());
                //OdbcCommand.Parameters.Add("@AppointmentId", AppointmentId.ToString());
                //OdbcCommand.Parameters.Add("@ProviderId", AppointmentId.ToString());
                //OdbcCommand.Parameters.Add("@PatientId", AppointmentId.ToString());
                //OdbcCommand.Parameters.Add("@TaxClassId", AppointmentId.ToString());

                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                OdbcCommand.ExecuteNonQuery();

                Utility.WriteToSyncLogFile_All("Appointment Insert In to EHR");


                OdbcSelect = SynchPracticeWorkQRY.UpdateNextVisitId;
                OdbcSelect = OdbcSelect.Replace("@visitIdNew", "'" + (AppointmentId + 1) + "'");
                OdbcSelect = OdbcSelect.Replace("@visitId", "'" + AppointmentId + "'");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.ExecuteNonQuery();

                return AppointmentId;
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

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime datetime)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                if (Utility.Is_PWPatientCellPhoneAvailable && Utility.Is_PWPatientFillerAvailable)
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetOPeratpryDateRangeWise.Replace("@FillerCondition", "( CASE WHEN  P.cellphone = '' THEN P.\"Filler\" ELSE P.cellphone END )");
                }
                else if (Utility.Is_PWPatientCellPhoneAvailable && !Utility.Is_PWPatientFillerAvailable)
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetOPeratpryDateRangeWise.Replace("@FillerCondition", "P." + "\"cellphone\"");
                }
                else
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetOPeratpryDateRangeWise.Replace("@FillerCondition", "P." + "\"Filler\"");
                }
                Utility.WriteToSyncLogFile_All("Get Booked Appointment string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddMonths(8).ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("Got Booked Appointment Fill Datatable");
                return OdbcDt;
            }
            catch (Exception ex)
            {
                try
                {
                    OdbcCommand.CommandTimeout = 200;
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string OdbcSelect = SynchPracticeWorkQRY.GetOPeratpryDateRangeWise.Replace("@FillerCondition", " pff.Filler");
                    Utility.WriteToSyncLogFile_All("Get Booked Appointment string " + OdbcSelect);
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddMonths(8).ToString("yyyy/MM/dd");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DataTable OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                    Utility.WriteToSyncLogFile_All("Got Booked Appointment Fill Datatable");
                    return OdbcDt;
                }
                catch (Exception ex1)
                {
                    throw ex1;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static Int64 GetMaxPatientIdFromPatientTable()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetMaxPatientIdFromPatient;
                Utility.WriteToSyncLogFile_All("Providers string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                {
                    return Convert.ToInt64(OdbcDt.Rows[0][0].ToString());
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static Int64 GetMaxAppointmentId()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetMaxAppointmentId;
                Utility.WriteToSyncLogFile_All("Find Max Appointment Id " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                if (OdbcDt != null && OdbcDt.Rows.Count > 0)
                {
                    return Convert.ToInt64(OdbcDt.Rows[0][0].ToString());
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetPracticeWorkAppointmentData(string strApptID = "")
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                if (!string.IsNullOrEmpty(strApptID))
                {
                    if (Utility.Is_PWPatientCellPhoneAvailable && Utility.Is_PWPatientFillerAvailable)
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentDataByApptID.Replace("@FillerCondition", "( CASE WHEN PF.cellphone = '' THEN PF.\"Filler\" ELSE PF.cellphone END )");
                    }
                    else if (Utility.Is_PWPatientCellPhoneAvailable && !Utility.Is_PWPatientFillerAvailable)
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentDataByApptID.Replace("@FillerCondition", "PF." + "\"cellphone\"");
                    }
                    else
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentDataByApptID.Replace("@FillerCondition", "PF." + "\"Filler\"");
                    }
                    OdbcSelect = OdbcSelect.Replace("@Appt_EHR_ID", strApptID);
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                else
                {
                    if (Utility.Is_PWPatientCellPhoneAvailable && Utility.Is_PWPatientFillerAvailable)
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentData.Replace("@FillerCondition", "( CASE WHEN PF.cellphone = '' THEN PF.\"Filler\" ELSE PF.cellphone END )");
                    }
                    else if (Utility.Is_PWPatientCellPhoneAvailable && !Utility.Is_PWPatientFillerAvailable)
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentData.Replace("@FillerCondition", "PF." + "\"cellphone\"");
                    }
                    else
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentData.Replace("@FillerCondition", "PF." + "\"Filler\"");
                    }
                }
                //string OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentData.Replace("@FillerCondition", "( CASE WHEN  PF." + "\"Filler\"" + " = '' THEN PF.cellphone ELSE PF." + "\"Filler\"" + " END )");
                Utility.WriteToSyncLogFile_All("Appointment string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@UpToDate", OdbcType.Date).Value = ToDate.AddMonths(4).ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All(" Appointment Records Fill in Datatable");
                return OdbcDt;
            }
            catch (Exception ex)
            {
                try
                {
                    OdbcCommand.CommandTimeout = 200;
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentData.Replace("@FillerCondition", " PF." + "\"Filler\"" + " ");
                    Utility.WriteToSyncLogFile_All("Appointment string " + OdbcSelect);
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    OdbcCommand.Parameters.Add("@UpToDate", OdbcType.Date).Value = ToDate.AddMonths(4).ToString("yyyy/MM/dd");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DataTable OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                    Utility.WriteToSyncLogFile_All(" Appointment Records Fill in Datatable");
                    return OdbcDt;
                }
                catch (Exception ex1)
                {
                    throw ex1;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetPracticeWorkAppointment_Procedures_Data(string strApptID = "")
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                if (!string.IsNullOrEmpty(strApptID))
                {
                    OdbcSelect = SynchPracticeWorkQRY.PraticeWorkAppointment_Procedures_DataByApptID;
                    OdbcSelect = OdbcSelect.Replace("@Appt_EHR_ID", strApptID);
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                else
                {
                    OdbcSelect = SynchPracticeWorkQRY.PraticeWorkAppointment_Procedures_Data;
                }
                
                Utility.WriteToSyncLogFile_All("Appointment string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@UpToDate", OdbcType.Date).Value = ToDate.AddMonths(4).ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("Appointment Procedures Data Records Fill in Datatable");
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

        public static DataTable GetPracticeWorkAppointmentIds()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                if (Utility.Is_PWPatientCellPhoneAvailable && Utility.Is_PWPatientFillerAvailable)
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentEhrIds.Replace("@FillerCondition", "( CASE WHEN  PF.cellphone = '' THEN PF." + "\"Filler\"" + " ELSE PF.cellphone END )");
                }
                else if (Utility.Is_PWPatientCellPhoneAvailable && !Utility.Is_PWPatientFillerAvailable)
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentEhrIds.Replace("@FillerCondition", "PF." + "\"cellphone\"");
                }
                else
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentEhrIds.Replace("@FillerCondition", "PF." + "\"Filler\"");
                }
                // string OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentEhrIds.Replace("@FillerCondition", "( case when PF.Filler = '' then PF.cellphone else PF.Filler end )");
                //string OdbcSelect = SynchPracticeWorkQRY.GetPraticeWorkAppointentData.Replace("@FillerCondition", "( CASE WHEN  PF." + "\"Filler\"" + " = '' THEN PF.cellphone ELSE PF." + "\"Filler\"" + " END )");
                Utility.WriteToSyncLogFile_All("Appointment string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.Parameters.Add("@UpToDate", OdbcType.Date).Value = ToDate.AddMonths(4).ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All(" Appointment Records Fill in Datatable");
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





        public static DataTable GetPracticeWorkOperatoryEventData()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkOperatoryEventData;
                Utility.WriteToSyncLogFile_All("Operatory Event string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                //Utility.WriteToSyncLogFile_All("Operataory records found " + OdbcDt.Rows.Count.ToString());
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

        public static DataTable GetPracticeWorkProviderData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkProviderData;
                Utility.WriteToSyncLogFile_All("Providers string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
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

        public static DataTable GetPracticeWorkDefaultProviderData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetDefaultProvider;
                Utility.WriteToSyncLogFile_All("Providers Default string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
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

        public static DataTable GetPracticeWorkOperatoryData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkOperatoryData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                GetPracticeWorkChairSequence(OdbcDt);
                if (GetPracticeWorkChairSequence(OdbcDt) == null)
                {
                    return OdbcDt;
                }
                else
                {
                    return GetPracticeWorkChairSequence(OdbcDt);
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

        public static DataTable GetPracticeWorkChairSequence(DataTable dtOperatoryData)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkOperatorySequence;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);

                string columnsName = "Resource Id ";
                for (int i = 1; i <= 15; i++)
                {
                    columnsName = "Resource Id " + i.ToString();
                    var result = dtOperatoryData.AsEnumerable().Where(o => o.Field<object>("Operatory_EHR_ID").ToString() == OdbcDt.Rows[0][columnsName].ToString());
                    if (result != null && result.Count() > 0)
                    {
                        result.All(o => { o["OperatoryOrder"] = i.ToString(); return true; });
                    }
                }

                return dtOperatoryData;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

        }


        public static DataTable GetPracticeWorkSpecialityData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkSpecialtyData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                int i = 1;
                foreach (DataRow drRow in OdbcDt.Rows)
                {
                    drRow["Speciality_EHR_ID"] = drRow["Speciality_Name"].ToString().Substring(0, 2) + drRow["Speciality_Name"].ToString().Substring(drRow["Speciality_Name"].ToString().Length - 1, 1) + i.ToString();
                    i = i++;
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

        public static DataTable GetPracticeWorkApptTypeData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkAppointmentType;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
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

        public static DataTable GetPracticeWorkNewPatientData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            Utility.WriteToSyncLogFile_All("New Patient string DAL GetPracticeWorkPatientData");
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";

                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkNewPatientData;

                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("New Patient records fill in datatable");
                return OdbcDt;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error in Patient records fill in datatable " + ex.Message.ToString());
                return null;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static DataTable GetPracticeWorkPatientData(string strPatID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            Utility.WriteToSyncLogFile_All("Patient string DAL GetPracticeWorkPatientData");
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                RefreshPatientCellPhoneStatus();
                if (strPatID == "")
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkPatientData;
                }
                else
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkNewAllPatientData;
                    OdbcSelect = OdbcSelect.Replace("@PaientEHRIDs", strPatID);
                }
                if (Utility.Is_PWPatientCellPhoneAvailable && Utility.Is_PWPatientFillerAvailable)
                {
                    OdbcSelect = OdbcSelect.Replace("@FillerCondition", "( CASE WHEN  pff.cellphone = '' THEN pff.\"Filler\" ELSE pff.cellphone END )");
                }
                else if (Utility.Is_PWPatientCellPhoneAvailable && !Utility.Is_PWPatientFillerAvailable)
                {
                    OdbcSelect = OdbcSelect.Replace("@FillerCondition", "pff.cellphone");
                }
                else
                {
                    OdbcSelect = OdbcSelect.Replace("@FillerCondition", "pff." + "\"Filler\"");
                }
                OdbcSelect = OdbcSelect.Replace("@persenType", Utility.PW_InactivePatientCodes);
                Utility.WriteToSyncLogFile_All("Patient string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("Patient records fill in datatable");
                return OdbcDt;
            }
            catch (Exception ex)
            {
                try
                {
                    OdbcCommand.CommandTimeout = 200;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = "";
                    if (strPatID == "")
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkPatientData;
                    }
                    else
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkNewAllPatientData;
                        OdbcSelect = OdbcSelect.Replace("@PaientEHRIDs", strPatID);
                    }
                    OdbcSelect = OdbcSelect.Replace("@FillerCondition", " pff.Filler");
                    OdbcSelect = OdbcSelect.Replace("@persenType", Utility.PW_InactivePatientCodes);
                    Utility.WriteToSyncLogFile_All("Patient string " + OdbcSelect);
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DataTable OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                    Utility.WriteToSyncLogFile_All("Patient records fill in datatable");
                    return OdbcDt;
                }
                catch (Exception ex1)
                {
                    throw ex1;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_Patient_PracticeWork_To_Local_New(DataTable dtDentrixDataToSave, string Clinic_Number, string Service_Install_Id)
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
                                        rec.SetValue(rs.GetOrdinal("CurrentBal"), Math.Round(double.Parse(dr["CurrentBal"].ToString().Trim()), 2));
                                        rec.SetValue(rs.GetOrdinal("ThirtyDay"), Math.Round(double.Parse(dr["ThirtyDay"].ToString().Trim()), 2));
                                        rec.SetValue(rs.GetOrdinal("SixtyDay"), Math.Round(double.Parse(dr["SixtyDay"].ToString().Trim()), 2));
                                        rec.SetValue(rs.GetOrdinal("NinetyDay"), Math.Round(double.Parse(dr["NinetyDay"].ToString().Trim()), 2));
                                        rec.SetValue(rs.GetOrdinal("Over90"), Math.Round(double.Parse(dr["Over90"].ToString().Trim()), 2));
                                        rec.SetValue(rs.GetOrdinal("FirstVisit_Date"), Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("Primary_Insurance"), dr["Primary_Insurance"].ToString());
                                        rec.SetValue(rs.GetOrdinal("Primary_Insurance_CompanyName"), dr["Primary_Insurance_CompanyName"].ToString());
                                        rec.SetValue(rs.GetOrdinal("Primary_Ins_Subscriber_ID"), "");
                                        rec.SetValue(rs.GetOrdinal("Secondary_Insurance"), dr["Secondary_Insurance"].ToString());
                                        rec.SetValue(rs.GetOrdinal("Secondary_Insurance_CompanyName"), dr["Secondary_Insurance_CompanyName"].ToString());
                                        rec.SetValue(rs.GetOrdinal("Secondary_Ins_Subscriber_ID"), "");
                                        rec.SetValue(rs.GetOrdinal("Guar_ID"), dr["Guar_ID"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Pri_Provider_ID"), dr["Pri_Provider_ID"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("Sec_Provider_ID"), dr["Sec_Provider_ID"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("ReceiveSMS"), dr["ReceiveSMS"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("ReceiveEmail"), "Y");
                                        rec.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("due_date"), Convert.ToString(dr["due_date"].ToString().Trim()));
                                        rec.SetValue(rs.GetOrdinal("collect_payment"), dr["collect_payment"].ToString().Trim());
                                        rec.SetValue(rs.GetOrdinal("remaining_benefit"), Math.Round(double.Parse(dr["remaining_benefit"].ToString().Trim()), 2));
                                        rec.SetValue(rs.GetOrdinal("used_benefit"), Math.Round(double.Parse(dr["used_benefit"].ToString().Trim()), 2));
                                        rec.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(DateTime.Now.ToString()));
                                        rec.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                        rec.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                        rec.SetValue(rs.GetOrdinal("Clinic_Number"), "0");
                                        rec.SetValue(rs.GetOrdinal("Service_Install_Id"), "1");
                                        rec.SetValue(rs.GetOrdinal("is_deleted"), dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
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
                                        if (rs.Seek(DbSeekOptions.FirstEqual, new { PatID = dr["Patient_EHR_ID"].ToString().Trim(), CliNum = Clinic_Number, ServiceInstalledID = Service_Install_Id }))
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
                                            rs.SetValue(rs.GetOrdinal("CurrentBal"), Math.Round(double.Parse(dr["CurrentBal"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("ThirtyDay"), Math.Round(double.Parse(dr["ThirtyDay"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("SixtyDay"), Math.Round(double.Parse(dr["SixtyDay"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("NinetyDay"), Math.Round(double.Parse(dr["NinetyDay"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("Over90"), Math.Round(double.Parse(dr["Over90"].ToString().Trim()), 2));
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
                                            rs.SetValue(rs.GetOrdinal("remaining_benefit"), Math.Round(double.Parse(dr["remaining_benefit"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("used_benefit"), Math.Round(double.Parse(dr["used_benefit"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rs.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rs.SetValue(rs.GetOrdinal("Clinic_Number"), dr["Clinic_Number"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Service_Install_Id"), Service_Install_Id);
                                            rs.SetValue(rs.GetOrdinal("is_deleted"), Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3 ? true : Convert.ToBoolean(dr["is_deleted"]));
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
                                            rs.Update();
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
        public static bool GetPracticeWorkPatientCellPhoneStatusData(bool IsCellPhoneAvailable)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            Utility.WriteToSyncLogFile_All("Patient string DAL GetPracticeWorkPatientData");
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                if (IsCellPhoneAvailable)
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkPatientCellPhoneStatusData.Replace("@FillerCondition", "pff.cellphone");
                }
                else
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkPatientCellPhoneStatusData.Replace("@FillerCondition", "pff." + "\"Filler\"");
                }
                Utility.WriteToSyncLogFile_All("Patient string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                OdbcCommand.ExecuteScalar();
                Utility.WriteToSyncLogFile_All("Patient records fill in datatable");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetPracticeWorkAppointmentsPatientData(string strPatID = "")
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            Utility.WriteToSyncLogFile_All("Appointmet's Patient string DAL GetPracticeWorkPatientData");
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                RefreshPatientCellPhoneStatus();
                if (!string.IsNullOrEmpty(strPatID))
                {
                    if (Utility.Is_PWPatientCellPhoneAvailable && Utility.Is_PWPatientFillerAvailable)
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkAppointmentsPatientDataByPatID.Replace("@FillerCondition", "( CASE WHEN  PFF.cellphone = '' THEN PFF.\"Filler\" ELSE PFF.cellphone END )");
                    }
                    else if (Utility.Is_PWPatientCellPhoneAvailable && !Utility.Is_PWPatientFillerAvailable)
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkAppointmentsPatientDataByPatID.Replace("@FillerCondition", "PFF." + "\"cellphone\"");
                    }
                    else
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkAppointmentsPatientDataByPatID.Replace("@FillerCondition", "PFF." + "\"Filler\"");
                    }
                    OdbcSelect = OdbcSelect.Replace("@Patient_EHR_ID", strPatID);
                }
                else
                {
                    if (Utility.Is_PWPatientCellPhoneAvailable && Utility.Is_PWPatientFillerAvailable)
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkAppointmentsPatientData.Replace("@FillerCondition", "( CASE WHEN  PFF.cellphone = '' THEN PFF.\"Filler\" ELSE PFF.cellphone END )");
                    }
                    else if (Utility.Is_PWPatientCellPhoneAvailable && !Utility.Is_PWPatientFillerAvailable)
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkAppointmentsPatientData.Replace("@FillerCondition", "PFF." + "\"cellphone\"");
                    }
                    else
                    {
                        OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkAppointmentsPatientData.Replace("@FillerCondition", "PFF." + "\"Filler\"");
                    }
                }
                //OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkAppointmentsPatientData.Replace("@FillerCondition", "( CASE WHEN  PFF." + "\"Filler\"" + " = '' THEN PFF.cellphone ELSE PFF." + "\"Filler\"" + " END )");
                OdbcSelect = OdbcSelect.Replace("@persenType", Utility.PW_InactivePatientCodes);
                Utility.WriteToSyncLogFile_All("Patient string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("Patient records fill in datatable");
                return OdbcDt;
            }
            catch (Exception ex)
            {
                try
                {
                    OdbcCommand.CommandTimeout = 200;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = "";
                    OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkAppointmentsPatientData.Replace("@FillerCondition", " PFF.Filler");
                    OdbcSelect = OdbcSelect.Replace("@persenType", Utility.PW_InactivePatientCodes);
                    Utility.WriteToSyncLogFile_All("Patient string " + OdbcSelect);
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DataTable OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                    Utility.WriteToSyncLogFile_All("Patient records fill in datatable");
                    return OdbcDt;
                }
                catch (Exception ex1)
                {
                    throw ex1;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetPracticeWorkPatientListData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            Utility.WriteToSyncLogFile_All("Patient string DAL GetPracticeWorkPatientData");
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                RefreshPatientCellPhoneStatus();
                if (Utility.Is_PWPatientCellPhoneAvailable && Utility.Is_PWPatientFillerAvailable)
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkPatientDataList.Replace("@FillerCondition", "( CASE WHEN  pff.cellphone = '' THEN pff.\"Filler\" ELSE pff.cellphone END )");
                }
                else if (Utility.Is_PWPatientCellPhoneAvailable && !Utility.Is_PWPatientFillerAvailable)
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkPatientDataList.Replace("@FillerCondition", "pff." + "\"cellphone\"");
                }
                else
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkPatientDataList.Replace("@FillerCondition", "pff." + "\"Filler\"");
                }
                // OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkPatientDataList.Replace("@FillerCondition", "( CASE WHEN  pff." + "\"Filler\"" + " = '' THEN pff.cellphone ELSE pff." + "\"Filler\"" + " END )");
                Utility.WriteToSyncLogFile_All("Patient string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("Patient records fill in datatable");
                return OdbcDt;
            }
            catch (Exception ex)
            {
                try
                {
                    OdbcCommand.CommandTimeout = 200;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string OdbcSelect = "";
                    OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkPatientDataList.Replace("@FillerCondition", " pff.Filler");
                    Utility.WriteToSyncLogFile_All("Patient string " + OdbcSelect);
                    CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                    // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                    DataTable OdbcDt = new DataTable();
                    OdbcDa.Fill(OdbcDt);
                    Utility.WriteToSyncLogFile_All("Patient records fill in datatable");
                    return OdbcDt;
                }
                catch (Exception ex1)
                {
                    throw ex1;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

        }

        public static DataTable GetPracticeWorkRecallTypeData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkRecallData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
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

        public static DataTable GetPracticeWorkApptStatusData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkAppointmentStatus;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
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

        public static DataTable GetPracticeWorkHolidaysData()
        {
            //throw new NotImplementedException();
            return null;
        }

        public static bool Save_Patient_Form_Local_To_PracticeWork(DataTable dtWebPatient_Form, string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;
            bool is_Record_Update = false;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                Utility.WriteToSyncLogFile_All("Start SAve From DAL");

                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string strQauery = string.Empty;
                string strQauery1 = string.Empty;
                string Update_PatientForm_Record_ID = "";
                string phoneNo = "";
                //string ColumnList = "";
                //string ValueList = "";
                string patient_EHR_Id = "";
                bool executePatientQuery = false;
                bool executePersonQuery = false;

                if (dtWebPatient_Form.AsEnumerable()
                    .Where(o => o.Field<object>("Patient_EHR_ID") != null &&
                           o.Field<object>("Patient_EHR_ID").ToString() != string.Empty &&
                           o.Field<object>("PatientForm_Web_ID") != null &&
                           o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty).Count() > 0)
                {

                    dtWebPatient_Form.AsEnumerable()
                    .Where(o => o.Field<object>("Patient_EHR_ID") != null &&
                           o.Field<object>("Patient_EHR_ID").ToString() != string.Empty &&
                           o.Field<object>("PatientForm_Web_ID") != null &&
                           o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {
                             Utility.WriteToSyncLogFile_All("Start Update Patient Form ");
                             foreach (DataRow dr in dtWebPatient_Form.Select(" PatientForm_Web_ID = '" + o.ToString() + "' "))
                             {
                                 if (dr["OriginalFieldName"].ToString().Trim() != string.Empty)
                                 {
                                     if (dr["Patient_EHR_ID"].ToString() != string.Empty && dr["OriginalFieldName"].ToString().Trim().ToLower() != "prim_member_id" && dr["OriginalFieldName"].ToString().Trim().ToLower() != "sec_member_id")
                                     {
                                         patient_EHR_Id = dr["Patient_EHR_ID"].ToString();
                                         strQauery = SynchPracticeWorkQRY.Update_Person_Record_By_Patient_Form;
                                         strQauery1 = SynchPracticeWorkQRY.Update_Patinet_Record_By_Patient_Form;
                                         if (dr["IsPatient"].ToString().Trim() == "Both" || dr["IsPatient"].ToString().Trim() == "Person")
                                         {
                                             executePersonQuery = true;
                                             strQauery = strQauery.Replace("FieldToUpdate", dr["OriginalFieldName"].ToString().Trim());
                                             if (dr["ehrfield"].ToString().Trim().ToUpper() == "HOME_PHONE" || dr["ehrfield"].ToString().Trim().ToUpper() == "WORK_PHONE" || dr["ehrfield"].ToString().Trim().ToUpper() == "MOBILE")
                                             {
                                                 phoneNo = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "");
                                                 strQauery = strQauery.Replace("ehrfield_value", "'" + phoneNo + "'");
                                             }
                                             else if (dr["ehrfield"].ToString().Trim().ToUpper() == "BIRTH_DATE")
                                             {
                                                 try
                                                 {
                                                     strQauery = strQauery.Replace("ehrfield_value", "'" + Convert.ToDateTime(dr["ehrfield_value"]).ToString("yyyy-MM-dd") + "'");
                                                 }
                                                 catch (Exception)
                                                 {
                                                     strQauery = strQauery.Replace("ehrfield_value", "NULL");
                                                 }
                                             }
                                             else if (dr["ehrfield"].ToString().Trim().ToUpper() == "SEX")
                                             {
                                                 if (dr["ehrfield_value"].ToString().ToUpper() == "MALE")
                                                 {
                                                     strQauery = strQauery.Replace("ehrfield_value", "0");
                                                 }
                                                 else
                                                 {
                                                     strQauery = strQauery.Replace("ehrfield_value", "1");
                                                 }
                                                 //OdbcSelect1 = OdbcSelect1.Replace("@Sex", "'" + gender + "'");
                                             }
                                             else if (dr["ehrfield"].ToString().Trim().ToUpper() == "SSN")
                                             {
                                                 strQauery = strQauery.Replace("ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim().Replace("@", "").Replace("\\", "").Replace("'", "") + "'");
                                                 //OdbcSelect1 = OdbcSelect1.Replace("@Sex", "'" + gender + "'");
                                             }
                                             else if (dr["ehrfield"].ToString().Trim().ToUpper() == "HOME_PHONE" || dr["ehrfield"].ToString().Trim().ToUpper() == "WORK_PHONE" || dr["ehrfield"].ToString().Trim().ToUpper() == "MOBILE")
                                             {
                                                 phoneNo = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "-").Trim().Replace(" ", "");
                                                 strQauery = strQauery.Replace("ehrfield_value", "'" + phoneNo + "'");
                                             }
                                             else if (dr["ehrfield"].ToString().Trim().ToString().Trim().ToUpper() == "MARITAL_STATUS")
                                             {
                                                 if (dr["ehrfield_value"].ToString().ToUpper() == "SINGLE")
                                                 {
                                                     strQauery = strQauery.Replace("ehrfield_value", "0");
                                                 }
                                                 else if (dr["ehrfield_value"].ToString().ToUpper() == "MARRIED")
                                                 {
                                                     strQauery = strQauery.Replace("ehrfield_value", "1");
                                                 }
                                                 else if (dr["ehrfield_value"].ToString().ToUpper() == "DIVORCED")
                                                 {
                                                     strQauery = strQauery.Replace("ehrfield_value", "2");
                                                 }
                                                 else
                                                 {
                                                     strQauery = strQauery.Replace("ehrfield_value", "3");
                                                 }

                                             }
                                             else
                                             {
                                                 strQauery = strQauery.Replace("ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim() + "'");
                                             }
                                             strQauery = strQauery.Replace("@Patient_EHR_ID", "'" + dr["Patient_EHR_ID"].ToString().Trim() + "'");
                                         }
                                         if (dr["IsPatient"].ToString().Trim() == "Both" || dr["IsPatient"].ToString().Trim() == "Patient")
                                         {
                                             executePatientQuery = true;
                                             strQauery1 = strQauery1.Replace("FieldToUpdate", dr["OriginalFieldName"].ToString().Trim());
                                             if (dr["ehrfield"].ToString().Trim().ToUpper() == "BIRTH_DATE")
                                             {
                                                 try
                                                 {
                                                     strQauery1 = strQauery1.Replace("ehrfield_value", "'" + Convert.ToDateTime(dr["ehrfield_value"]).ToString("yyyy-MM-dd") + "'");
                                                 }
                                                 catch (Exception)
                                                 {
                                                     strQauery1 = strQauery1.Replace("ehrfield_value", "NULL");
                                                 }
                                             }
                                             else if (dr["ehrfield"].ToString().Trim().ToUpper() == "SEX")
                                             {
                                                 if (dr["ehrfield_value"].ToString().ToUpper() == "MALE")
                                                 {
                                                     strQauery1 = strQauery1.Replace("ehrfield_value", "0");
                                                 }
                                                 else
                                                 {
                                                     strQauery1 = strQauery1.Replace("ehrfield_value", "1");
                                                 }
                                                 //OdbcSelect1 = OdbcSelect1.Replace("@Sex", "'" + gender + "'");
                                             }
                                             else if (dr["ehrfield"].ToString().Trim().ToUpper() == "SCHOOL")
                                             {
                                                 strQauery1 = strQauery1.Replace("ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim().Replace("@", "").Replace("\\", "").Replace("'", "") + "',\"Full time Student\" = 1");
                                             }
                                             else if (dr["ehrfield"].ToString().Trim().ToString().Trim().ToUpper() == "MARITAL_STATUS")
                                             {
                                                 if (dr["ehrfield_value"].ToString().ToUpper() == "SINGLE")
                                                 {
                                                     strQauery1 = strQauery1.Replace("ehrfield_value", "0");
                                                 }
                                                 else if (dr["ehrfield_value"].ToString().ToUpper() == "MARRIED")
                                                 {
                                                     strQauery1 = strQauery1.Replace("ehrfield_value", "1");
                                                 }
                                                 else if (dr["ehrfield_value"].ToString().ToUpper() == "DIVORCED")
                                                 {
                                                     strQauery1 = strQauery1.Replace("ehrfield_value", "2");
                                                 }
                                                 else
                                                 {
                                                     strQauery1 = strQauery1.Replace("ehrfield_value", "3");
                                                 }

                                             }
                                             else
                                             {
                                                 strQauery1 = strQauery1.Replace("ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim() + "'");
                                             }
                                             strQauery1 = strQauery1.Replace("@Patient_EHR_ID", "'" + dr["Patient_EHR_ID"].ToString().Trim() + "'");
                                         }

                                         Utility.WriteToSyncLogFile_All("Update Person From Patient Form " + strQauery);
                                         if (executePersonQuery)
                                         {
                                             if (conn.State == ConnectionState.Closed) conn.Open();
                                             if (Utility.Is_PWPatientCellPhoneAvailable)
                                             {
                                                 strQauery.Replace("Filler", "Cellphone");
                                             }
                                             // Utility.WriteToSyncLogFile_All("Updated Person " + strQauery);
                                             CommonDB.OdbcCommandServer(strQauery, conn, ref OdbcCommand, "txt");
                                             OdbcCommand.ExecuteNonQuery();
                                         }

                                         Utility.WriteToSyncLogFile_All("Update Patient From Patient Form " + strQauery1);
                                         if (executePatientQuery)
                                         {
                                             if (conn.State == ConnectionState.Closed) conn.Open();
                                             // Utility.WriteToSyncLogFile_All("Updated Patient " + strQauery1);
                                             CommonDB.OdbcCommandServer(strQauery1, conn, ref OdbcCommand, "txt");
                                             OdbcCommand.ExecuteNonQuery();
                                         }



                                         Utility.WriteToSyncLogFile_All("Updated Patient ");
                                     }
                                 }
                                 executePatientQuery = false;
                                 executePersonQuery = false;

                             }
                             SynchEaglesoftDAL.UpdatePatientEHRIdINPatientForm(patient_EHR_Id.ToString(), o.ToString().Trim(), Service_Install_Id);
                             Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + o.ToString().Trim() + ";";
                             return true;
                         });
                }


                Int64 Patient_Id = 0;
                string OdbcSelect = "";
                string OdbcSelect1 = "";
                int gender = 0;
                int maritalStatus = 0;
                Utility.WriteToSyncLogFile_All("Save Patient Into the EHR _ Start");
                if (
                dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty).Count() > 0)
                {
                    dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {
                             // Utility.WriteToSyncLogFile_All("Save Patient For " + o.ToString());
                             Patient_Id = GetMaxPatientIdFromPatientTable();
                             OdbcSelect = SynchPracticeWorkQRY.InsertPracticeWorkPatientFile;

                             OdbcSelect = OdbcSelect.Replace("@PatientId", Patient_Id.ToString());
                             OdbcSelect = OdbcSelect.Replace("@GuarId", Patient_Id.ToString());

                             // Utility.WriteToSyncLogFile_All("Patient Save 1 ." + OdbcSelect.ToString());

                             var dtColumnsExists = dtWebPatient_Form.AsEnumerable()
                             .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "LAST_NAME");

                             // Utility.WriteToSyncLogFile_All("Patient Save 1.2 ." + OdbcSelect.ToString());

                             if (dtColumnsExists != null && dtColumnsExists.Count() > 0)
                             {
                                 // Utility.WriteToSyncLogFile_All("Patient Save 1.3 ." + OdbcSelect.ToString());

                                 OdbcSelect = OdbcSelect.Replace("@LastName", "'" + dtColumnsExists.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 // Utility.WriteToSyncLogFile_All("Patient Save 1.4 ." + OdbcSelect.ToString());
                                 OdbcSelect = OdbcSelect.Replace("@LastName", "''");
                             }


                             var dtColumnsLastName = dtWebPatient_Form.AsEnumerable()
                           .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "FIRST_NAME");
                             if (dtColumnsLastName != null && dtColumnsLastName.Count() > 0)
                             {
                                 OdbcSelect = OdbcSelect.Replace("@FirstName", "'" + dtColumnsLastName.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect = OdbcSelect.Replace("@FirstName", "''");
                             }

                             OdbcSelect = OdbcSelect.Replace("@GuarId", "'" + Patient_Id.ToString() + "'");
                             var dtColumnsSchool = dtWebPatient_Form.AsEnumerable()
                            .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "SCHOOL");
                             if (dtColumnsSchool != null && dtColumnsSchool.Count() > 0)
                             {
                                 OdbcSelect = OdbcSelect.Replace("@School_name", "'" + dtColumnsSchool.First().Field<string>("ehrfield_value").ToString().Replace("@", "").Replace("\\", "").Replace("'", "") + "'");
                                 OdbcSelect = OdbcSelect.Replace("@Full_time_Student", "1");
                             }
                             else
                             {
                                 OdbcSelect = OdbcSelect.Replace("@School_name", "''");
                                 OdbcSelect = OdbcSelect.Replace("@Full_time_Student", "0");
                             }
                             Utility.WriteToSyncLogFile_All("Patient File Save : " + OdbcSelect);
                             CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                             //Utility.WriteToSyncLogFile_All("Save Patient File " + OdbcSelect);
                             // Utility.WriteToSyncLogFile_All("Patient Save 1.5 ." + OdbcSelect.ToString());
                             CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                             OdbcCommand.ExecuteNonQuery();
                             Utility.WriteToSyncLogFile_All("Patient File Save");

                             OdbcSelect1 = SynchPracticeWorkQRY.InsertPracticeWorkPersonFile;
                             //Utility.WriteToSyncLogFile_All("Save Person File " + OdbcSelect1);
                             OdbcSelect1 = OdbcSelect1.Replace("@PatientId", Patient_Id.ToString());


                             if (dtColumnsExists != null && dtColumnsExists.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@LastName", "'" + dtColumnsExists.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@LastName", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File LastName");
                             if (dtColumnsLastName != null && dtColumnsLastName.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@FirstName", "'" + dtColumnsLastName.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@FirstName", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File FirstName");
                             var dtColumnsMiddle = dtWebPatient_Form.AsEnumerable()
                            .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "MIDDLE_NAME");
                             if (dtColumnsMiddle != null && dtColumnsMiddle.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@MiddleName", "'" + dtColumnsMiddle.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@MiddleName", "''");
                             }
                             var dtColumnsSSN = dtWebPatient_Form.AsEnumerable()
                           .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "SSN");
                             if (dtColumnsSSN != null && dtColumnsSSN.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@SSN", "'" + dtColumnsSSN.First().Field<string>("ehrfield_value").ToString().Replace("@", "").Replace("\\", "").Replace("'", "") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@SSN", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File MiddleName");
                             var dtColumnsLegal = dtWebPatient_Form.AsEnumerable()
                            .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "PREFERRED_NAME");
                             if (dtColumnsLegal != null && dtColumnsLegal.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@LegalName", "'" + dtColumnsLegal.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@LegalName", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File LegalName");
                             var dtColumnsAddress = dtWebPatient_Form.AsEnumerable()
                           .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "ADDRESS_ONE");
                             if (dtColumnsAddress != null && dtColumnsAddress.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@Address1", "'" + dtColumnsAddress.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@Address1", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File Address1");
                             var dtColumnsAddress2 = dtWebPatient_Form.AsEnumerable()
                          .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "ADDRESS_TWO");
                             if (dtColumnsAddress2 != null && dtColumnsAddress2.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@Address2", "'" + dtColumnsAddress2.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@Address2", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File Address2");
                             var dtColumnsCity = dtWebPatient_Form.AsEnumerable()
                         .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "CITY");
                             if (dtColumnsCity != null && dtColumnsCity.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@City", "'" + dtColumnsCity.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@City", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File City");
                             var dtColumnsState = dtWebPatient_Form.AsEnumerable()
                        .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "STATE");
                             if (dtColumnsState != null && dtColumnsState.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@State", "'" + dtColumnsState.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@State", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File State");
                             var dtColumnsZip = dtWebPatient_Form.AsEnumerable()
                      .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "ZIPCODE");
                             if (dtColumnsZip != null && dtColumnsZip.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@Zip", "'" + dtColumnsZip.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@Zip", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File Zip");
                             var dtColumnsHomePhone = dtWebPatient_Form.AsEnumerable()
                      .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "HOME_PHONE");
                             if (dtColumnsHomePhone != null && dtColumnsHomePhone.Count() > 0)
                             {
                                 phoneNo = dtColumnsHomePhone.First().Field<string>("ehrfield_value").ToString().Trim().Replace("(", "").Replace(")", "-").Trim().Replace(" ", "");
                                 OdbcSelect1 = OdbcSelect1.Replace("@HomePhone", "'" + phoneNo + "'");
                                 phoneNo = "";
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@HomePhone", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File HomePhone");
                             var dtColumnsMobile = dtWebPatient_Form.AsEnumerable()
                     .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "MOBILE");
                             if (dtColumnsMobile != null && dtColumnsMobile.Count() > 0)
                             {
                                 phoneNo = dtColumnsMobile.First().Field<string>("ehrfield_value").ToString().Trim().Replace("(", "").Replace(")", "-").Trim().Replace(" ", "");
                                 OdbcSelect1 = OdbcSelect1.Replace("@Filler", "'" + phoneNo + "'");
                                 phoneNo = "";
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@Filler", "''");
                             }
                             if (Utility.Is_PWPatientCellPhoneAvailable)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("Filler", "Cellphone");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File Filler");
                             var dtColumnsWorkPhone1 = dtWebPatient_Form.AsEnumerable()
                    .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "WORK_PHONE");
                             if (dtColumnsWorkPhone1 != null && dtColumnsWorkPhone1.Count() > 0)
                             {
                                 phoneNo = dtColumnsWorkPhone1.First().Field<string>("ehrfield_value").ToString().Trim().Replace("(", "").Replace(")", "-").Trim().Replace(" ", "");
                                 OdbcSelect1 = OdbcSelect1.Replace("@WorkPhone1", "'" + phoneNo + "'");
                                 phoneNo = "";
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@WorkPhone1", "''");
                             }
                             OdbcSelect1 = OdbcSelect1.Replace("@WorkPhone2", "''");
                             OdbcSelect1 = OdbcSelect1.Replace("@OfficePhone", "''");
                             //Utility.WriteToSyncLogFile_All("Save Person File WorkPhone1");
                             var dtColumnsEmail = dtWebPatient_Form.AsEnumerable()
                    .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "EMAIL");
                             if (dtColumnsEmail != null && dtColumnsEmail.Count() > 0)
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@Email", "'" + dtColumnsEmail.First().Field<string>("ehrfield_value") + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@Email", "''");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File Email");
                             var dtColumnsBirthdate = dtWebPatient_Form.AsEnumerable()
                 .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "BIRTH_DATE");
                             if (dtColumnsBirthdate != null && dtColumnsBirthdate.Count() > 0)
                             {
                                 try
                                 {
                                     OdbcSelect1 = OdbcSelect1.Replace("@BirthDate", "'" + Convert.ToDateTime(dtColumnsBirthdate.First().Field<string>("ehrfield_value")).ToString("yyyy-MM-dd") + "'");
                                 }
                                 catch (Exception)
                                 {
                                     OdbcSelect1 = OdbcSelect1.Replace("@BirthDate", "NULL");
                                 }
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@BirthDate", "NULL");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File BirthDate");
                             var dtColumnsSex = dtWebPatient_Form.AsEnumerable()
                .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "SEX");
                             if (dtColumnsSex != null && dtColumnsSex.Count() > 0)
                             {
                                 if (dtColumnsSex.First().Field<string>("ehrfield_value").ToString().ToUpper() == "MALE")
                                 {
                                     gender = 0;
                                 }
                                 else
                                 {
                                     gender = 1;
                                 }

                                 OdbcSelect1 = OdbcSelect1.Replace("@Sex", "'" + gender + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@Sex", "0");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File Sex");
                             var dtColumnsMariStatus = dtWebPatient_Form.AsEnumerable()
                 .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == "MARITAL_STATUS");
                             if (dtColumnsMariStatus != null && dtColumnsMariStatus.Count() > 0)
                             {
                                 if (dtColumnsMariStatus.First().Field<string>("ehrfield_value").ToUpper() == "SINGLE")
                                 {
                                     maritalStatus = 0;
                                 }
                                 else if (dtColumnsMariStatus.First().Field<string>("ehrfield_value").ToUpper() == "MARRIED")
                                 {
                                     maritalStatus = 1;
                                 }
                                 else if (dtColumnsMariStatus.First().Field<string>("ehrfield_value").ToUpper() == "DIVORCED")
                                 {
                                     maritalStatus = 2;
                                 }
                                 else
                                 {
                                     maritalStatus = 3;
                                 }

                                 OdbcSelect1 = OdbcSelect1.Replace("@MaritalStatus", "'" + maritalStatus + "'");
                             }
                             else
                             {
                                 OdbcSelect1 = OdbcSelect1.Replace("@MaritalStatus", "0");
                             }
                             //Utility.WriteToSyncLogFile_All("Save Person File Sex");
                             Utility.WriteToSyncLogFile_All("Save Person File " + OdbcSelect1);
                             CommonDB.OdbcCommandServer(OdbcSelect1, conn, ref OdbcCommand, "txt");

                             CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                             OdbcCommand.ExecuteNonQuery();
                             Utility.WriteToSyncLogFile_All("Person File Save");


                             OdbcSelect = SynchPracticeWorkQRY.UpdateNextPatientId;
                             OdbcSelect = OdbcSelect.Replace("@PatientIdNew", "'" + (Patient_Id + 1) + "'");
                             OdbcSelect = OdbcSelect.Replace("@PatientId", "'" + Patient_Id + "'");
                             CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                             CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                             OdbcCommand.ExecuteNonQuery();

                             SynchEaglesoftDAL.UpdatePatientEHRIdINPatientForm(Patient_Id.ToString(), o.ToString().Trim(), Service_Install_Id);

                             Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + o.ToString().Trim() + ";";

                             return true;
                         });
                }

                DataView view = new DataView(dtWebPatient_Form);
                DataTable distinctValues = view.ToTable(true, "PatientForm_Web_ID", "Service_Install_Id");

                SynchLocalDAL.UpdatePatientFormEHR_Updateflg(distinctValues);

                is_Record_Update = true;

            }
            catch (Exception ex)
            {
                is_Record_Update = false;
                Utility.WriteToSyncLogFile_All("Error While Save Patient File Or Person File " + ex.Message);
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return is_Record_Update;
        }

        public static DataTable GetProviderCustomHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                Utility.WriteToSyncLogFile_All("GetProviderCustomHours Start");
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkProviderCustomeHOurs;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                Utility.WriteToSyncLogFile_All("GetProviderCustomHours string " + OdbcSelect);
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("GetProviderCustomHours got");
                return CreateProviderCustomeHours(OdbcDt);
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

        public static DataTable CreateProviderCustomeHours(DataTable dtResult)
        {
            DataTable dtResultnew = new DataTable();
            DataTable dtProvider = new DataTable();
            try
            {
                dtResultnew = SynchLocalDAL.GetLocalProviderHoursData_BlankStructure();
                dtProvider = SynchLocalDAL.GetLocalProviderData("", "1");
                // DateTime FromDate = DateTime.Now; //Convert.ToDateTime( dtResult.Rows[0][0].ToString());
                int j = (dtResult.Rows.Count > 180 ? 180 : dtResult.Rows.Count);
                Utility.WriteToSyncLogFile_All("Create Slot Start");
                for (int i = 0; i < j; i++)
                {
                    foreach (DataRow drRow in dtProvider.Rows)
                    {
                        DataRow drNew = dtResultnew.NewRow();
                        drNew["PH_LocalDB_ID"] = "0";
                        drNew["PH_EHR_ID"] = drRow["Provider_EHR_ID"].ToString() + "_" + Convert.ToDateTime(dtResult.Rows[i]["Date"]).Day.ToString() + "_" + Convert.ToDateTime(dtResult.Rows[i]["Date"]).Month.ToString();
                        drNew["PH_Web_ID"] = "";
                        drNew["Provider_EHR_ID"] = drRow["Provider_EHR_ID"].ToString();
                        drNew["Operatory_EHR_ID"] = "0";
                        //Utility.WriteToSyncLogFile_All("Slot for " + drRow["Provider_EHR_ID"].ToString() + " " + FromDate.ToString("yyyy-MM-dd").ToString().Replace(" 12:00:00 AM", "") + " " + dtResult.Rows[0]["OpenTime"].ToString().ToString());
                        drNew["StartTime"] = Convert.ToDateTime(Convert.ToDateTime(dtResult.Rows[i]["Date"]).ToString("yyyy-MM-dd").ToString().Replace(" 12:00:00 AM", "") + " " + dtResult.Rows[i]["OpenTime"].ToString().ToString());
                        drNew["EndTime"] = Convert.ToDateTime(Convert.ToDateTime(dtResult.Rows[i]["Date"]).ToString("yyyy-MM-dd").ToString().Replace(" 12:00:00 AM", "") + " " + dtResult.Rows[i]["CloseTime"].ToString().ToString());
                        drNew["comment"] = "";
                        drNew["Entry_DateTime"] = DateTime.Now;
                        drNew["Last_Sync_Date"] = DateTime.Now;
                        drNew["is_deleted"] = 0;
                        drNew["Is_Adit_Updated"] = 0;
                        drNew["Clinic_Number"] = "0";
                        drNew["Service_Install_Id"] = "1";
                        dtResultnew.Rows.Add(drNew);
                    }
                    // FromDate = FromDate.AddDays(1);
                }
                Utility.WriteToSyncLogFile_All("Slot Created");
                return dtResultnew;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable CreateOperatoryCustomeHours(DataTable dtResult)
        {
            DataTable dtResultnew = new DataTable();
            DataTable dtProvider = new DataTable();
            try
            {
                dtResultnew = SynchLocalDAL.GetLocalOperatoryHoursData_BlankStructure();
                dtProvider = SynchLocalDAL.GetLocalOperatoryData("1");
                DateTime EHROfficeHourStart = DateTime.Now;
                if (dtResult.Rows.Count > 0 && dtResult.Rows[0]["Date"] != null && dtResult.Rows[0]["Date"].ToString() != string.Empty)
                {
                    EHROfficeHourStart = Convert.ToDateTime(dtResult.Rows[0]["Date"]);
                }

                // DateTime FromDate = DateTime.Now; //Convert.ToDateTime( dtResult.Rows[0][0].ToString());
                int j = (dtResult.Rows.Count > 180 ? 180 : dtResult.Rows.Count);
                //int 1;
                for (int i = 0; i < j; i++)
                {
                    foreach (DataRow drRow in dtProvider.Rows)
                    {
                        DataRow drNew = dtResultnew.NewRow();
                        drNew["OH_LocalDB_ID"] = "0";
                        drNew["OH_EHR_ID"] = drRow["Operatory_EHR_ID"].ToString() + "_" + Convert.ToDateTime(dtResult.Rows[i]["Date"]).Day.ToString() + "_" + Convert.ToDateTime(dtResult.Rows[i]["Date"]).Month.ToString();
                        drNew["OH_Web_ID"] = "";
                        drNew["Operatory_EHR_ID"] = drRow["Operatory_EHR_ID"].ToString();

                        drNew["StartTime"] = Convert.ToDateTime(Convert.ToDateTime(dtResult.Rows[i]["Date"]).ToString("yyyy-MM-dd").ToString().Replace(" 12:00:00 AM", "") + " " + dtResult.Rows[i]["OpenTime"].ToString().ToString());
                        drNew["EndTime"] = Convert.ToDateTime(Convert.ToDateTime(dtResult.Rows[i]["Date"]).ToString("yyyy-MM-dd").ToString().Replace(" 12:00:00 AM", "") + " " + dtResult.Rows[i]["CloseTime"].ToString().ToString());
                        drNew["comment"] = "";
                        drNew["Entry_DateTime"] = DateTime.Now;
                        drNew["Last_Sync_Date"] = DateTime.Now;
                        drNew["is_deleted"] = 0;
                        drNew["Is_Adit_Updated"] = 0;
                        drNew["Clinic_Number"] = "0";
                        drNew["Service_Install_Id"] = "1";
                        dtResultnew.Rows.Add(drNew);
                    }
                    // FromDate = FromDate.AddDays(1);
                    //if (EHROfficeHourStart > FromDate)
                    //{
                    //    FromDate = EHROfficeHourStart;
                    //}
                }
                return dtResultnew;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetOperatoryCustomHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                Utility.WriteToSyncLogFile_All("GetOperatoryCustomHours Start");
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkOperatoryCustomeHOurs;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                OdbcCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                Utility.WriteToSyncLogFile_All("GetOperatoryCustomHours " + OdbcSelect.ToString());
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("GetOperatoryCustomHours End");
                return CreateOperatoryCustomeHours(OdbcDt);
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

        public static DataTable GetPracticeWorkProviderOfficeHours()
        {
            throw new NotImplementedException();
        }

        public static DataTable GetPracticeWorkOperatoryOfficeHours()
        {
            throw new NotImplementedException();
        }

        public static DataTable GetPracticeWorkOperatoryChairData()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkOperatoryTimeOff;
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

        public static bool Save_OperatoryDayOff_PracticeWork_To_Local(DataTable dtPracticeWorkOperatoryChair)
        {
            throw new NotImplementedException();
        }

        public static DataTable GetPracticeWorkDiseaseData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkDiseaseMaster;
                Utility.WriteToSyncLogFile_All("Diease string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("Fill DieaseRecords");
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


        public static DataTable GetPracticeWorkMedicationData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetMedicationMaster;
                Utility.WriteToSyncLogFile_All("Diease string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("Fill DieaseRecords");
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

        public static DataTable GetPracticeWorkPatientMedicationData(string Patient_EHR_IDS)
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPatientMedication;
                }
                else
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPatientMedication + " Where DRH." + "\"PatientPersonID\" in (" + Patient_EHR_IDS + ")";
                }
                Utility.WriteToSyncLogFile_All("Diease string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("Fill DieaseRecords");
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

        public static DataTable GetPracticeWorkPatientDiseaseData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeWorkPatientDisease;
                Utility.WriteToSyncLogFile_All("Diease string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("Fill DieaseRecords");
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

        public static bool SaveAllergiesToPracticeWork(string Service_Install_Id, string strPatientFormID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();

            CommonDB.PracticeWorkConnectionServer(ref conn);
            string OdbcSelect = "";
            try
            {
                OdbcCommand.CommandTimeout = 200;
                DataTable dtPatientDisease = SynchLocalDAL.GetLocalPatientFormDiseaseResponseToSaveINEHR(Service_Install_Id, strPatientFormID);
                if (dtPatientDisease.Rows.Count > 0)
                {
                    DataTable dtPWPatientDisesase = GetPracticeWorkPatientDiseaseData();
                    foreach (DataRow dr in dtPatientDisease.Rows)
                    {
                        DataRow drPatientdiseaseresult = dtPWPatientDisesase.Select("Disease_EHR_Id = '" + dr["Disease_EHR_Id"].ToString() + "' and patient_EHR_id = '" + dr["PatientEHRID"].ToString() + "'").FirstOrDefault();
                        if (drPatientdiseaseresult == null)
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();

                            //Int64 Patient_Id = GetMaxPatientIdFromPatientTable();
                            //string OdbcSelect = SynchPracticeWorkQRY.InsertMedicalAlerts;

                            OdbcSelect = SynchPracticeWorkQRY.InsertMedicalAlerts;
                            OdbcSelect = OdbcSelect.Replace("@Patient_id", "'" + dr["PatientEHRId"] + "'");
                            OdbcSelect = OdbcSelect.Replace("@Alert_id", dr["Disease_EHR_Id"].ToString());

                            CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                            OdbcCommand.ExecuteNonQuery();

                            Utility.WriteToSyncLogFile_All("Save Medical Alerts " + OdbcSelect);

                            //OdbcCommand.ExecuteNonQuery();

                        }
                        SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), (dr["PatientEHRId"].ToString() + "_" + dr["Disease_EHR_Id"].ToString()), dr["PatientEHRId"].ToString(), dr["Disease_EHR_Id"].ToString(), Service_Install_Id);
                    }
                }
                Utility.WriteToSyncLogFile_All("Medical Alerts Save");
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Error While Save Patient File Or Person File " + ex.Message);
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool DeleteAllergiesToPracticeWork(string Service_Install_Id, string strPatientFormID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();

            CommonDB.PracticeWorkConnectionServer(ref conn);
            string OdbcSelect = "";
            try
            {
                OdbcCommand.CommandTimeout = 200;
                DataTable dtPatientDeleteDisease = SynchLocalDAL.GetLocalPatientFormDiseaseDeleteResponseToSaveINEHR(Service_Install_Id, strPatientFormID);
                if (dtPatientDeleteDisease.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPatientDeleteDisease.Rows)
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        //Int64 Patient_Id = GetMaxPatientIdFromPatientTable();
                        //string OdbcSelect = SynchPracticeWorkQRY.InsertMedicalAlerts;

                        OdbcSelect = SynchPracticeWorkQRY.DeleteMedicalAlerts;
                        OdbcSelect = OdbcSelect.Replace("@Patient_id", "'" + dr["Patient_EHR_Id"] + "'");
                        OdbcSelect = OdbcSelect.Replace("@Alert_id", dr["Disease_EHR_Id"].ToString());

                        CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                        OdbcCommand.ExecuteNonQuery();

                        Utility.WriteToSyncLogFile_All("Save Medical Alerts " + OdbcSelect);

                        //OdbcCommand.ExecuteNonQuery();

                        SynchLocalDAL.UpdateDeleteDiseaseEHR_Updateflg(dr["Disease_EHR_Id"].ToString(), dr["Patient_EHR_Id"].ToString(), Service_Install_Id);

                    }
                }
                Utility.WriteToSyncLogFile_All("Medical Alerts Save");
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Error While Save Patient File Or Person File " + ex.Message);
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static DataTable GetPracticeWorkPatientStatusData(string clinicNumber, string dbString, string strPatID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                if (!string.IsNullOrEmpty(strPatID))
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPatientStatusByPatID;
                    OdbcSelect = OdbcSelect.Replace("@Patient_EHR_ID", strPatID);
                }
                else
                {
                    OdbcSelect = SynchPracticeWorkQRY.GetPatientStatus;
                }
                OdbcSelect = OdbcSelect.Replace("curdate()", "'" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                OdbcSelect = OdbcSelect.Replace("@persenType", Utility.PW_InactivePatientCodes);
                Utility.WriteToSyncLogFile_All("GetPracticeWorkPatientStatusData string " + OdbcSelect);
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);
                Utility.WriteToSyncLogFile_All("Fill GetPracticeWorkPatientStatusData");
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

        private static void RefreshPatientCellPhoneStatus()
        {
            try
            {
                Utility.Is_PWPatientCellPhoneAvailable = GetPracticeWorkPatientCellPhoneStatusData(true);
                Utility.Is_PWPatientFillerAvailable = GetPracticeWorkPatientCellPhoneStatusData(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //SaveMedicationToPracticeWork
        public static bool SaveMedicationToPracticeWork(string Service_Install_Id, ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            CommonDB.PracticeWorkConnectionServer(ref conn);
            string odbcSelect = "";
            Int64 MedicationPatientId = 0;
            Int64 MedicationNum = 0;
            SavePatientEHRID = "";
            try
            {
                OdbcCommand.CommandTimeout = 200;
                DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationResponseToSaveINEHR(Service_Install_Id, strPatientFormID);
                if (dtPatientMedicationResponse.Rows.Count > 0)
                {
                    DataTable dtMedication = GetPracticeWorkMedicationData();
                    DataTable dtPatientMedicationData = GetPracticeWorkPatientMedicationData("");
                    foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                    {
                        if (dr["Medication_EHR_Id"] == DBNull.Value) dr["Medication_EHR_Id"] = "";
                        if (dr["Medication_EHR_Id"].ToString().Trim() == "" || dr["Medication_EHR_Id"].ToString().Trim() == "0")
                        {
                            DataRow[] drMedRow = dtMedication.Copy().Select("Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "'");
                            DataRow[] drMedRows = dtMedication.Copy().Select("Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "'");
                            if (drMedRows.Length > 0)
                            {
                                MedicationNum = Convert.ToInt64(drMedRows[0]["Medication_EHR_ID"]);
                            }
                            else
                            {
                                MedicationNum = 0;
                            }

                            if (MedicationNum <= 0)
                            {
                                Utility.WriteToErrorLogFromAll("Medication '" + dr["Medication_Name"].ToString().Trim() + "' not found. Medication ID: " + dr["Medication_EHR_Id"].ToString().Trim());
                                continue;
                                //throw new Exception("Medication '" + dr["Medication_Name"].ToString().Trim() + "' not found. Medication ID: " + dr["Medication_EHR_Id"].ToString().Trim());

                                //odbcSelect = SynchPracticeWorkQRY.InsertMedication;
                                //odbcSelect = odbcSelect.Replace("@Medication_Name", "'" + dr["Medication_Name"].ToString().Trim() + "'");
                                //OdbcCommand.CommandText = odbcSelect;
                                //OdbcCommand.Parameters.Clear();
                                ////OdbcCommand.Parameters.AddWithValue("description", dr["Medication_Name"]);
                                //CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                //OdbcCommand.ExecuteNonQuery();

                                //odbcSelect = "Select max(\"DrugID\") as itemid from \"Drugs\"";
                                //OdbcCommand.CommandText = odbcSelect;
                                //OdbcCommand.Parameters.Clear();
                                //MedicationNum = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                                //dr["Medication_EHR_ID"] = MedicationNum.ToString();

                                //DataRow newRow = dtMedication.NewRow();
                                //newRow["Medication_EHR_ID"] = MedicationNum;
                                //newRow["Medication_Name"] = dr["Medication_Name"].ToString().Trim();
                                //newRow["Medication_Description"] = dr["Medication_Name"].ToString().Trim();
                                //newRow["Medication_Notes"] = dr["Medication_Note"].ToString().Trim();
                                ////newRow["Medicatoin_Type"] = "Drug";
                                //dtMedication.Rows.Add(newRow);
                                //dtMedication.AcceptChanges();
                            }
                            else
                            {
                                MedicationNum = Convert.ToInt64(drMedRow[0]["Medication_EHR_ID"].ToString().Trim());
                                dr["Medication_EHR_ID"] = MedicationNum.ToString();
                            }
                        }
                        else
                        {
                            MedicationNum = Convert.ToInt64(dr["Medication_EHR_ID"]);
                        }

                        if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "0";
                        if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" && dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                        {
                            MedicationPatientId = Convert.ToInt64(dr["PatientMedication_EHR_Id"]);
                        }
                        if (MedicationPatientId <= 0)
                        {
                            string strSelect = "Patient_EHR_ID = " + dr["PatientEHRID"].ToString().Trim() +
                                        " And Medication_EHR_ID = " + MedicationNum + " And is_active='True' ";
                            DataRow[] drPatMedRow = dtPatientMedicationData.Copy().Select(strSelect);
                            if (drPatMedRow.Length > 0)
                            {
                                MedicationPatientId = Convert.ToInt64(drPatMedRow[0]["PatientMedication_EHR_ID"].ToString().Trim());
                            }
                        }

                        if (MedicationPatientId <= 0)
                        {

                            if (conn.State == ConnectionState.Closed) conn.Open();
                            odbcSelect = "Select IsNull(Max(\"Text ID\") + 1, 1) as \"TextID\" from \"Chart Text\"";
                            Utility.WriteToSyncLogFile_All("Get Medication Chart History : " + odbcSelect);
                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                            Int64 MedicationNoteID = Convert.ToInt64(OdbcCommand.ExecuteScalar());

                            odbcSelect = "Insert into \"Chart Text\"(\"Text ID\",\"SeqNum\", \"Rep Flag\", \"Compression Flag\", \"Text\") Values('" + MedicationNoteID + "','0', '0', '0', @Medication_Note)";
                            odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + dr["Medication_Note"].ToString().Trim() + "'");
                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                            Utility.WriteToSyncLogFile_All("Save Medication Note " + odbcSelect);
                            OdbcCommand.ExecuteNonQuery();

                            //odbcSelect = "Select Max(\"Text ID\") as MedicatoinNoteID from \"Chart Text\" ";
                            //CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                            //Utility.WriteToSyncLogFile_All("Get Medication Note ID " + odbcSelect);
                            //Int64 MedicationNoteID = Convert.ToInt64(OdbcCommand.ExecuteScalar());

                            odbcSelect = "Select \"Provider Emp ID\" from \"Patient File\" Where \"person ID\" = " + dr["PatientEHRID"].ToString().Trim();
                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                            Utility.WriteToSyncLogFile_All("Get Medication Note ID " + odbcSelect);
                            Int64 ProviderEHRID = Convert.ToInt64(OdbcCommand.ExecuteScalar());

                            DataRow[] drMedRow = dtMedication.Copy().Select("Medication_EHR_ID = " + dr["Medication_EHR_ID"].ToString().Trim());
                            string strDesc = drMedRow.Length > 0 ? drMedRow[0]["Medication_Description"].ToString().Trim() : "";
                            string strDrugQuantity = drMedRow.Length > 0 ? drMedRow[0]["Drug_Quantity"].ToString().Trim() : "";
                            string strMedQuantity = drMedRow.Length > 0 ? drMedRow[0]["Medication_Quantity"].ToString().Trim() : "";
                            string strRefills = drMedRow.Length > 0 ? drMedRow[0]["Refills"].ToString().Trim() : "";
                            string strMedicationSIG = drMedRow.Length > 0 ? drMedRow[0]["Medication_SIG"].ToString().Trim() : "";

                            odbcSelect = SynchPracticeWorkQRY.InsertPatientMedication;

                            odbcSelect = odbcSelect.Replace("@Medication_EHR_ID", dr["Medication_EHR_ID"].ToString().Trim()); //@Medication_EHR_ID,                             
                            odbcSelect = odbcSelect.Replace("@Patient_EHR_ID", dr["PatientEHRID"].ToString().Trim()); //@Patient_EHR_ID,                             
                            odbcSelect = odbcSelect.Replace("@Medication_Description", "'" + strDesc.ToString().Trim() + "'"); //@Medication_Description,                             
                            odbcSelect = odbcSelect.Replace("@Medication_Name", "'" + dr["Medication_Name"].ToString().Trim() + "'"); //@Medication_Name,                             
                            odbcSelect = odbcSelect.Replace("@Drug_Quantity", "'" + strDrugQuantity.ToString().Trim() + "'"); //@Drug_Quantity,                             
                            odbcSelect = odbcSelect.Replace("@Medication_Quantity", "'" + strMedQuantity.ToString().Trim() + "'"); //@Medication_Quantity,                             
                            odbcSelect = odbcSelect.Replace("@Refills", "'" + strRefills.ToString().Trim() + "'"); //@Refills,                             
                            odbcSelect = odbcSelect.Replace("@Medication_SIG", "'" + strMedicationSIG.ToString().Trim() + "'"); //@Medication_SIG,                             
                            odbcSelect = odbcSelect.Replace("@Medication_Note_ID", "'" + MedicationNoteID.ToString().Trim() + "'"); //@Medication_Note_ID,                             
                            odbcSelect = odbcSelect.Replace("@Provider_EHR_ID", "'" + ProviderEHRID.ToString().Trim() + "'"); //@Provider_EHR_ID

                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                            Utility.WriteToSyncLogFile_All("Save Medication " + odbcSelect);
                            OdbcCommand.ExecuteNonQuery();

                            odbcSelect = "Select Max(\"DrugHistoryID\") as PatientMedication_EHR_ID  from \"DrugHistory\"";
                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                            MedicationPatientId = Convert.ToInt64(OdbcCommand.ExecuteScalar());
                            dr["PatientMedication_EHR_Id"] = MedicationPatientId.ToString().Trim();

                            odbcSelect = "Select IfNull(Max(\"Chart entry seq\")+1,1) as SeqNum  from \"Chart History\" where \"Person ID\" = " + dr["PatientEHRID"].ToString().Trim() + " and \"Chart entry ID\" = 5300 and \"Attach Area\" = 50";
                            Utility.WriteToSyncLogFile_All("Get Medication Chart History : " + odbcSelect);
                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                            Int64 SeqNum = Convert.ToInt64(OdbcCommand.ExecuteScalar());

                            odbcSelect = "Insert into \"Chart History\"(\"Person ID\", " +
                                "\"Chart Entry ID\", " +
                                "\"Attach Area\", " +
                                "\"Chart date\", " +
                                "\"Chart Entry seq\", " +
                                "\"Commit Status\", " +
                                "\"Charted by emp ID\", " +
                                "\"Entry date\", " +
                                "\"Sub object ID\", " +
                                "\"LedgerTransID\", " +
                                "\"Data1\") " + "Values" +
                                "(@Patient_EHR_ID, 5300, 50, @CurrentDate, @SeqNum, 1, @EnteredBy, @CurrentDate, 0, 0, @PatientMedication_EHR_ID)";
                            odbcSelect = odbcSelect.Replace("@Patient_EHR_ID", dr["PatientEHRID"].ToString().Trim());
                            odbcSelect = odbcSelect.Replace("@CurrentDate", "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                            odbcSelect = odbcSelect.Replace("@EnteredBy", ProviderEHRID.ToString().Trim());
                            odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", MedicationPatientId.ToString().Trim());
                            odbcSelect = odbcSelect.Replace("@SeqNum", SeqNum.ToString().Trim());
                            Utility.WriteToSyncLogFile_All("Save Medication Chart History : " + odbcSelect);

                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                            OdbcCommand.ExecuteNonQuery();

                            DataRow newPatMedRow = dtPatientMedicationData.NewRow();
                            newPatMedRow["Patient_EHR_ID"] = dr["PatientEHRID"].ToString();
                            newPatMedRow["Medication_EHR_ID"] = dr["Medication_EHR_Id"].ToString();
                            newPatMedRow["PatientMedication_EHR_ID"] = MedicationPatientId.ToString();
                            newPatMedRow["Medication_Name"] = dr["Medication_Name"].ToString();
                            newPatMedRow["Medication_Note"] = dr["Medication_Note"].ToString();
                            newPatMedRow["is_active"] = "True";

                            dtPatientMedicationData.Rows.Add(newPatMedRow);
                            dtPatientMedicationData.AcceptChanges();
                        }
                        else
                        {
                            MedicationPatientId = Convert.ToInt64(dr["PatientMedication_EHR_Id"].ToString().Trim());
                            odbcSelect = SynchPracticeWorkQRY.UpdatePatientMedicationNotes;
                            odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", MedicationPatientId.ToString().Trim());
                            odbcSelect = odbcSelect.Replace("@Medication_Note", "'" + dr["Medication_Note"].ToString().Trim() + "'");
                            CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                            Utility.WriteToSyncLogFile_All("Update Medication Note " + odbcSelect);
                            OdbcCommand.ExecuteNonQuery();
                        }
                        if (SavePatientEHRID.ToUpper().Trim().Contains("'" + dr["PatientEHRID"].ToString().Trim() + "'"))
                        {
                            SavePatientEHRID = SavePatientEHRID + "'" + dr["PatientEHRID"].ToString() + "',";
                        }
                        SynchLocalDAL.UpdateMedicationEHR_Updateflg(dr["MedicationResponse_Local_ID"].ToString(), MedicationPatientId.ToString(), dr["PatientEHRID"].ToString(), dr["Medication_EHR_Id"].ToString(), Service_Install_Id);
                    }
                    isRecordSaved = true;
                }
                Utility.WriteToSyncLogFile_All("Medication Save");
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Error While Save Patient File Or Person File " + ex.Message);
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static bool DeleteMedicationToPracticeWork(string Service_Install_Id, ref bool isRecordDeleted, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            CommonDB.PracticeWorkConnectionServer(ref conn);
            string odbcSelect = "";
            DeletePatientEHRID = "";
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
                                odbcSelect = "Delete from \"DrugHistory\" where \"DrugHistoryID\" = @PatientMedication_EHR_ID";
                                odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", dr["PatientMedication_EHR_Id"].ToString().Trim());
                                Utility.WriteToSyncLogFile_All("Delete Medication " + odbcSelect);

                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                Utility.WriteToSyncLogFile_All("Delete Medication " + odbcSelect);
                                OdbcCommand.ExecuteNonQuery();

                                odbcSelect = "Delete from \"Chart History\" where \"Data1\" = @PatientMedication_EHR_ID And \"Person ID\" = @Patient_EHR_ID And \"Chart entry ID\" = 5300 And \"Attach area\" = 50";
                                odbcSelect = odbcSelect.Replace("@PatientMedication_EHR_ID", dr["PatientMedication_EHR_Id"].ToString().Trim());
                                odbcSelect = odbcSelect.Replace("@Patient_EHR_ID", dr["PatientEHRId"].ToString().Trim());
                                Utility.WriteToSyncLogFile_All("Delete Medication " + odbcSelect);

                                CommonDB.OdbcCommandServer(odbcSelect, conn, ref OdbcCommand, "txt");
                                Utility.WriteToSyncLogFile_All("Delete Medication " + odbcSelect);
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
                Utility.WriteToSyncLogFile_All("Error While Deleting Patient File Or Person File " + ex.Message);
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        #region Insurance
        public static DataTable GetPracticeWorkInsuranceData()
        {
            OdbcConnection conn = null;
            OdbcCommand OdbcCommand = new OdbcCommand();
            OdbcDataAdapter OdbcDa = null;
            CommonDB.PracticeWorkConnectionServer(ref conn);
            try
            {
                OdbcCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string OdbcSelect = "";
                OdbcSelect = SynchPracticeWorkQRY.GetPracticeworkInsuranceData;
                CommonDB.OdbcCommandServer(OdbcSelect, conn, ref OdbcCommand, "txt");
                // OdbcCommand.Parameters.Add("@provider_Id", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.OdbcDatatAdapterServer(OdbcCommand, ref OdbcDa);
                DataTable OdbcDt = new DataTable();
                OdbcDa.Fill(OdbcDt);              
                //rooja 20-8-24               

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

        public static bool Save_Insurance_PrackticeWork_To_Local(DataTable dtPrackticeWorkInsurance, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //       SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtPrackticeWorkInsurance.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("Insurance_EHR_ID", dr["Insurance Co ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Name", dr["Company Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address", dr["Company Address1"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address2", dr["Company Address2"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("City", dr["Company City"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("State", dr["Company State"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["Company ZIP"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Phone", dr["Company phone"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ElectId", dr["Elect Claims ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("EmployerName", "");
                                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr["Inactive"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();
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
            }
            return _successfullstataus;
        }

        #endregion

    }
}
