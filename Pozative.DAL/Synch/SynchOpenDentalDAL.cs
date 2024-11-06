using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Pozative.QRY;
using System.Data.SqlServerCe;
using Pozative.UTL;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using RestSharp;
using System.Net;
using System.Net.Security;
using Pozative.BO;
using System.Web.Script.Serialization;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pozative.DAL
{
    public class SynchOpenDentalDAL
    {

        public static bool GetOpenDentalConnection(string DbString)
        {
            Utility.ODsqlConnErrorMsg = "";
            MySqlConnection conn = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            bool IsConnected = false;
            try
            {
                conn.Open();
                IsConnected = true;
            }
            catch (Exception ex)
            {
                Utility.WriteToConnectionLog("Pozative to EHR Database Connection Failed- " + ex.Message);
                IsConnected = false;
                Utility.WriteToErrorLogFromAll("MySQL connection issue - " + ex.Message);
                Utility.ODsqlConnErrorMsg = "MySQL connection issue - " + ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return IsConnected;
        }

        public static string GetOpenDentalActualVersion(string DbString)
        {
            string version = "";
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string MySqlSelect = SynchOpenDentalQRY.GetEHRActualVersionOpendental;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
                if (MySqlDt.Rows.Count > 0)
                    version = MySqlDt.Rows[0]["ValueString"].ToString().TrimEnd('\\');
                else
                    version = "";

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

        public static DataTable GetOpenDentalAppointmentData(string DbString, string Appt_EHR_ID)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = "";
                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalAppointmentData_15_4;
                }
                else
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalAppointmentData;
                }

                if (!string.IsNullOrEmpty(Appt_EHR_ID))
                {
                    MySqlSelect = MySqlSelect + " And a.AptNum = '" + Appt_EHR_ID + "'";
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                else
                {
                    MySqlSelect = MySqlSelect + ";";
                }

                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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


        public static DataTable GetOpenDentalAppointmentIds(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = "";
                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalAppointmentEhrIDs_15_4;
                }
                else
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalAppointmentEhrIDs;
                }

                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpendentalAppointment_Procedures_Data(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = "";

                MySqlSelect = SynchOpenDentalQRY.OpendentalAppointment_Procedures_Data;

                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalDeletedAppointmentData(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();

                //string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalDeletedAppointmentData;
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalDeletedAppointmentData_Clinic_Number;
                if (Utility.Application_Version.ToLower() == "17.2+")
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalDeletedAppointmentData17_2Plus;
                }
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalOperatoryEventData(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalOperatoryEventData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalOperatoryAllEventData(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalOperatoryAllEventData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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


        public static DataTable GetOpenDentalHolidayData(string DbString)
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = "";
                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalHoliday_15_4;
                }
                else
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalHoliday;
                }
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalProviderData(string DbString, string Service_Install_Id)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = "";
                if (Utility.Application_Version.ToLower() == "15.4".ToLower() || Utility.UseProvidersForAllClinicOpendental)
                {
                    Utility.WriteToErrorLogFromAll("Executing query UseProvidersForAllClinicOpendental = true");
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalProviderData_15_4;
                }
                else
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalProviderData_link;
                }
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                try
                {
                    MySqlDa.Fill(MySqlDt);
                }
                catch
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalProviderData;
                    CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                    CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                    MySqlDt = new DataTable();
                    MySqlDa.Fill(MySqlDt);
                }

                DataTable Dttmp = MySqlDt.Clone();
                if (MySqlDt.AsEnumerable().Where(o => Convert.ToInt32(o.Field<object>("Clinic_Number")) == 0).Count() == MySqlDt.AsEnumerable().Count())
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                        {
                            for (int j = 0; j < MySqlDt.Rows.Count; j++)
                            {
                                DataRow Dr = Dttmp.NewRow();
                                Dr.ItemArray = MySqlDt.Rows[j].ItemArray;
                                Dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                                Dttmp.Rows.Add(Dr);
                            }
                        }
                    }
                }
                else
                {
                    Dttmp = MySqlDt;
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

        public static DataTable GetOpenDentalProviderHoursData(string DbString)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = "";
                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalProviderHoursData_15_4;
                }
                else
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalProviderHoursData;
                }
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalDiseaseData(string DbString, string Service_Install_Id)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalDiseaseData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);

                DataTable Dttmp = MySqlDt.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                    {
                        for (int j = 0; j < MySqlDt.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp.NewRow();
                            Dr.ItemArray = MySqlDt.Rows[j].ItemArray;
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

        public static DataTable GetOpenDentalMedicationData(string DbString, string Service_Install_Id)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalMedicationDataNew; //SynchOpenDentalQRY.GetOpenDentalMedicationData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);

                DataTable Dttmp = MySqlDt.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                    {
                        for (int j = 0; j < MySqlDt.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp.NewRow();
                            Dr.ItemArray = MySqlDt.Rows[j].ItemArray;
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

        public static DataTable GetOpenDentalPatientDiseaseData(string DbString, string Service_Install_Id)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientDiseaseData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);

                //DataTable Dttmp = MySqlDt.Clone();
                //for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                //{
                //    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                //    {
                //        for (int j = 0; j < MySqlDt.Rows.Count; j++)
                //        {
                //            DataRow Dr = Dttmp.NewRow();
                //            Dr.ItemArray = MySqlDt.Rows[j].ItemArray;
                //            Dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                //            Dttmp.Rows.Add(Dr);
                //        }
                //    }
                //}
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

        public static DataTable GetOpenDentalPatientMedicationData(string DbString, string Service_Install_Id, string Patient_EHR_IDS)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = "";
                if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientMedicationData;
                }
                else
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientMedicationData + " Where mp.patnum in (" + Patient_EHR_IDS + ")";
                }
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);

                //DataTable Dttmp = MySqlDt.Clone();
                //for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                //{
                //    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                //    {
                //        for (int j = 0; j < MySqlDt.Rows.Count; j++)
                //        {
                //            DataRow Dr = Dttmp.NewRow();
                //            Dr.ItemArray = MySqlDt.Rows[j].ItemArray;
                //            Dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                //            Dttmp.Rows.Add(Dr);
                //        }
                //    }
                //}
                //return Dttmp;
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

        public static DataTable GetOpenDentalOperatoryData(string DbString)
        {

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalOperatoryData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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


        public static DataTable GetOpenDentalFolderListData(string DbString)
        {

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalFolderListData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalDeletedOperatoryData(string DbString)
        {

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalDeletedOperatoryData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalDeletedFolderListData(string DbString)
        {

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalDeletedFolderListData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalApptTypeData(string DbString, string Service_Install_Id)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalApptTypeData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);

                DataTable Dttmp = MySqlDt.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                    {
                        for (int j = 0; j < MySqlDt.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp.NewRow();
                            Dr.ItemArray = MySqlDt.Rows[j].ItemArray;
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

        /// <summary>
        /// GetPatientWisePendingAmount used to Get PendingAmount of all patient
        /// </summary>
        /// <returns></returns>
        /// 

        public static DataTable GetPatientWisePendingAmount(string Clinic_number, string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetPatientWisePendingAmount;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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
        public static DataTable GetOpenDentalInsertPatientData(string Clinic_Number, string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalInsertPatientData;

                if (Clinic_Number.Trim() == "")
                {
                    foreach (DataRow rw in Utility.DtLocationList.Rows)
                    {
                        Clinic_Number += (Clinic_Number.Trim() != "") ? "," : "";
                        Clinic_Number += rw["Clinic_Number"].ToString();
                    }
                }
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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
        public static DataTable GetOpenDentalPatientData(string Clinic_Number, string DbString, bool isNewQuery, string PatientEHRID = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if (isNewQuery)
                {
                    return GetOpenDentalPatientDataNew(Clinic_Number, DbString, PatientEHRID);
                }
                string MySqlSelect = Clinic_Number != "" ? (isNewQuery ? SynchOpenDentalQRY.GetOpenDentalPatientData_Clinic_New : SynchOpenDentalQRY.GetOpenDentalPatientData_Clinic)
                        : (isNewQuery ? SynchOpenDentalQRY.GetOpenDentalPatientData_New : SynchOpenDentalQRY.GetOpenDentalPatientData);


                if (!string.IsNullOrEmpty(PatientEHRID))
                {
                    StringBuilder strNewQuery = new StringBuilder();
                    strNewQuery.Append(MySqlSelect);
                    strNewQuery.Append(" And p.PatNum in (" + PatientEHRID + ");");
                    MySqlSelect = strNewQuery.ToString();
                }
                else
                {
                    MySqlSelect = MySqlSelect + ";";
                }

                if (Utility.EHR_VersionNumber.ToString().Contains("15.4"))
                {
                    MySqlSelect = MySqlSelect.Replace("pn.ICEName", "''");
                    MySqlSelect = MySqlSelect.Replace("pn.ICEPhone", "''");
                }

                //#######
                if (Clinic_Number.Trim() == "")
                {
                    foreach (DataRow rw in Utility.DtLocationList.Rows)
                    {
                        Clinic_Number += (Clinic_Number.Trim() != "") ? "," : "";
                        Clinic_Number += rw["Clinic_Number"].ToString();
                    }
                }
                //#######

                //MySqlSelect = MySqlSelect.Replace("@Clinic_Number", Clinic_Number.ToString());
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalPatientDataNew(string Clinic_Number, string DbString, string PatientEHRID = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DataTable MySqlDt = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = Clinic_Number != "" ?
                    " SELECT " +
                    " DISTINCT '0' as 'Patient_LocalDB_ID', p.PatNum AS Patient_EHR_ID, '0' as 'Patient_Web_ID', p.fname AS First_name, p.lname AS Last_name, p.MiddleI AS Middle_Name, p.Salutation, case when p.patstatus = 0 then 'A' else 'I' end as Status, case when p.Gender = 0 then 'Male' when p.Gender = 1 then 'Female' when p.Gender > 1 then 'Unknown' end AS Sex, case when p.Position = 0 then 'Single' when p.Position = 1 then 'Married' when p.Position = 2 then 'Child' when p.Position = 3 then 'Widowed' when p.Position = 4 then 'Divorced' end AS MaritalStatus, IF(p.Birthdate = '0001-01-01' or p.Birthdate = '0000-00-00', NULL, p.Birthdate) AS Birth_Date, p.Email AS Email, REPLACE(REPLACE(REPLACE(REPLACE(p.WirelessPhone, '(', ''), ')', ''), '-', ''), ' ', '') AS Mobile, REPLACE( REPLACE(REPLACE(REPLACE(p.HmPhone, '(', ''), ')', ''), '-', ''), ' ', '' ) AS Home_Phone, REPLACE( REPLACE(REPLACE(REPLACE(p.WkPhone, '(', ''), ')', ''), '-', ''), ' ', '' ) AS Work_Phone, p.Address AS Address1, p.Address2 AS Address2, p.City AS City, p.State AS State, p.Zip AS Zipcode, '' as ResponsibleParty_Status, p.EstBalance AS CurrentBal, p.Bal_0_30 AS ThirtyDay, p.Bal_31_60 AS SixtyDay, p.Bal_61_90 AS NinetyDay, p.BalOver90 AS Over90, IF(p.DateFirstVisit = '0001-01-01' or p.DateFirstVisit = '0000-00-00', NULL, p.DateFirstVisit) AS FirstVisit_Date, LastVisit_Date," +
                    " p.Guarantor AS Guar_ID, p.PriProv AS Pri_Provider_ID, p.SecProv AS Sec_Provider_ID, ( case when Cast(p.TxtMsgOk As Char) = '0' or Cast(p.TxtMsgOk As Char) = '' or Cast(p.TxtMsgOk As Char) = '1' then 'Y' else 'N' end ) as ReceiveSMS, 'Y' as ReceiveEmail, NextVisit_Date," +
                    "  Now() as Last_Sync_Date, p.dateTStamp AS EHR_Entry_DateTime, 'false' as 'Is_Adit_Updated', p.Preferred As preferred_name, p.clinicnum as Clinic_Number, '' as 'Service_Install_Id', case when p.patstatus IN(4, 5) then 1 else 0 end as is_deleted, case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then 'NonPatient' when p.patstatus = 2 then 'InActive' when p.patstatus = 3 then 'InActive' when p.patstatus IN(4, 5) then 'Deleted' when p.patstatus = 6 then 'NonPatient' end as EHR_Status, case when p.prefercontactmethod = 1 then 'N' else 'Y' end as ReceiveVoiceCall, '' as 'Patient_status', '' as 'Patient_status_Compare', p.AddrNote AS Patient_Note, CASE WHEN ISNULL(p.language) THEN 'eng' When p.Language = '' Then 'eng' ELSE p.language END AS PreferredLanguage, p.SSN as SSN, '' as driverlicense, '' AS groupid, '' AS emergencycontactId, pn.ICEPhone AS emergencycontactnumber, p.SchoolName as school, emp.EmpName AS employer, PG.spouseId, resp.responsparty AS responsiblepartyId, resp.SSN AS responsiblepartyssn, resp.Birthdate AS responsiblepartybirthdate, PG.Spouse_Last_Name, resp.FName AS ResponsibleParty_First_Name, resp.Lname AS ResponsibleParty_Last_Name, " +
                    " pn.ICEName AS EmergencyContact_First_Name, '' AS EmergencyContact_Last_Name, PG.Spouse_First_Name, 'false' as Is_Status_Adit_Updated, 1 AS InsUptDlt" +
                    " FROM" +
                    " patient p" +
                    " left join employer as emp on p.EmployerNum = emp.EmployerNum" +
                    " left join patientnote as pn on p.Patnum = pn.Patnum" +
                    " left join patient as resp on p.Patnum = resp.responsparty and resp.responsparty > 0" +
                    " left join(SELECT PT.patnum, max(PT.AptDateTime) as LastVisit_Date from appointment PT WHERE AptDateTime < Now() AND PT.ClinicNum = @Clinic_Number GROUP by PT.patnum ) Ldata on p.PatNum = Ldata.patnum" +
                    " left join(SELECT PT.patnum, min(PT.AptDateTime) as nextvisit_date from appointment PT WHERE AptDateTime > Now() AND AptStatus NOT IN(3, 5) AND PT.ClinicNum = @Clinic_Number GROUP by PT.patnum ) Ndata on p.PatNum = Ndata.patnum" +
                    " left join(select patnumchild, max(patnumguardian) as spouseId, FName as Spouse_First_Name, LName as Spouse_Last_Name from patient pa INNER JOIN guardian g ON pa.patnum = g.patnumguardian and relationship = 18 group by patnumchild ) PG on p.patnum = PG.patnumchild" +
                    " where p.PatNum <> '' AND p.lname <> '' and(Not((p.fname = '' OR p.lname = '') and p.patstatus = 4) ) and(p.clinicnum = @Clinic_Number)"
                    :
                    " SELECT DISTINCT '0' as 'Patient_LocalDB_ID', p.PatNum AS Patient_EHR_ID, '0' as 'Patient_Web_ID', p.fname AS First_name, p.lname AS Last_name, p.MiddleI AS Middle_Name, p.Salutation, case when p.patstatus = 0 then 'A' else 'I' end as Status, case when p.Gender = 0 then 'Male' when p.Gender = 1 then 'Female' when p.Gender > 1 then 'Unknown' end AS Sex, case when p.Position = 0 then 'Single' when p.Position = 1 then 'Married' when p.Position = 2 then 'Child' when p.Position = 3 then 'Widowed' when p.Position = 4 then 'Divorced' end AS MaritalStatus, IF( p.Birthdate = '0001-01-01' or p.Birthdate = '0000-00-00', NULL, p.Birthdate ) AS Birth_Date, p.Email AS Email, REPLACE( REPLACE( REPLACE( REPLACE(p.WirelessPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Mobile, REPLACE( REPLACE( REPLACE( REPLACE(p.HmPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Home_Phone, REPLACE( REPLACE( REPLACE( REPLACE(p.WkPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Work_Phone, p.Address AS Address1, p.Address2 AS Address2, p.City AS City, p.State AS State, p.Zip AS Zipcode, '' as ResponsibleParty_Status, p.EstBalance AS CurrentBal, p.Bal_0_30 AS ThirtyDay, p.Bal_31_60 AS SixtyDay, p.Bal_61_90 AS NinetyDay, p.BalOver90 AS Over90, IF( p.DateFirstVisit = '0001-01-01' or p.DateFirstVisit = '0000-00-00', NULL, p.DateFirstVisit ) AS FirstVisit_Date, LastVisit_Date," +
                    " p.Guarantor AS Guar_ID, p.PriProv AS Pri_Provider_ID, p.SecProv AS Sec_Provider_ID, ( case when Cast(p.TxtMsgOk As Char) = '0' or Cast(p.TxtMsgOk As Char) = '' or Cast(p.TxtMsgOk As Char) = '1' then 'Y' else 'N' end ) as ReceiveSMS, p.email as ReceiveEmail, NextVisit_Date," +
                    " Now() as Last_Sync_Date, p.dateTStamp AS EHR_Entry_DateTime, 'false' as 'Is_Adit_Updated', p.Preferred As preferred_name," +
                    " p.clinicnum as Clinic_Number, '' as 'Service_Install_Id', case when p.patstatus IN(4, 5) then 1 else 0 end as is_deleted, case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then 'NonPatient' when p.patstatus = 2 then 'InActive' when p.patstatus = 3 then 'InActive' when p.patstatus IN(4, 5) then 'Deleted' when p.patstatus = 6 then 'NonPatient' end as EHR_Status, case when p.prefercontactmethod = 1 then 'N' else 'Y' end as ReceiveVoiceCall, '' as 'Patient_status', '' as 'Patient_status_Compare', p.AddrNote AS Patient_Note, CASE WHEN ISNULL(p.language) THEN 'eng' When p.Language = '' Then 'eng' ELSE p.language END AS PreferredLanguage," +
                    " p.SSN as SSN, '' as driverlicense, '' AS groupid, '' AS emergencycontactId, pn.ICEPhone AS emergencycontactnumber, p.SchoolName as school, emp.EmpName AS employer, resp.responsparty AS responsiblepartyId," +
                    " resp.SSN AS responsiblepartyssn, resp.Birthdate AS responsiblepartybirthdate, PG.Spouse_Last_Name, resp.FName AS ResponsibleParty_First_Name, resp.Lname AS ResponsibleParty_Last_Name," +
                    " pn.ICEName AS EmergencyContact_First_Name, '' AS EmergencyContact_Last_Name, PG.Spouse_First_Name, 'false' as Is_Status_Adit_Updated, 1 AS InsUptDlt" +
                    " FROM" +
                    " patient p" +
                    " left join employer as emp on p.EmployerNum = emp.EmployerNum" +
                    " left join patientnote as pn on p.Patnum = pn.Patnum" +
                    " left join patient as resp on p.Patnum = resp.responsparty and resp.responsparty > 0" +
                    " left join(SELECT PT.patnum, max(PT.AptDateTime) as LastVisit_Date from appointment PT WHERE AptDateTime < Now() GROUP by PT.patnum ) Ldata on p.PatNum = Ldata.patnum" +
                    " left join(SELECT PT.patnum, min(PT.AptDateTime) as nextvisit_date from appointment PT WHERE AptDateTime > Now() AND AptStatus NOT IN(3, 5) GROUP by PT.patnum ) Ndata on p.PatNum = Ndata.patnum" +
                    " left join(select patnumchild, max(patnumguardian) as spouseId, FName as Spouse_First_Name, LName as Spouse_Last_Name from patient pa INNER JOIN guardian g ON pa.patnum = g.patnumguardian and relationship = 18 group by patnumchild ) PG on p.patnum = PG.patnumchild" +
                    " where p.PatNum <> '' AND p.lname <> '' and(Not((p.fname = '' OR p.lname = '') and p.patstatus = 4) )";

                if (!string.IsNullOrEmpty(PatientEHRID))
                {
                    StringBuilder strNewQuery = new StringBuilder();
                    strNewQuery.Append(MySqlSelect);
                    strNewQuery.Append(" And p.PatNum in (" + PatientEHRID + ");");
                    MySqlSelect = strNewQuery.ToString();
                }
                else
                {
                    MySqlSelect = MySqlSelect + ";";
                }

                if (Utility.EHR_VersionNumber.ToString().Contains("15.4"))
                {
                    MySqlSelect = MySqlSelect.Replace("pn.ICEName", "''");
                    MySqlSelect = MySqlSelect.Replace("pn.ICEPhone", "''");
                }

                //#######
                if (Clinic_Number.Trim() == "")
                {
                    foreach (DataRow rw in Utility.DtLocationList.Rows)
                    {
                        Clinic_Number += (Clinic_Number.Trim() != "") ? "," : "";
                        Clinic_Number += rw["Clinic_Number"].ToString();
                    }
                }
                //#######
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 1");
                //MySqlSelect = MySqlSelect.Replace("@Clinic_Number", Clinic_Number.ToString());
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(MySqlDt);
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 2");

                MySqlDt.Columns.Add("Primary_Insurance");//PI.Primary_Insurance, 
                MySqlDt.Columns.Add("Primary_Insurance_CompanyName");//PI.Primary_Insurance_CompanyName, 
                MySqlDt.Columns.Add("Primary_Ins_Subscriber_ID");//PI.SubscriberID as Primary_Ins_Subscriber_ID, 
                MySqlDt.Columns.Add("Secondary_Insurance");//SI.Secondary_Insurance, 
                MySqlDt.Columns.Add("Secondary_Insurance_CompanyName");//SI.Secondary_Insurance_CompanyName, 
                MySqlDt.Columns.Add("Secondary_Ins_Subscriber_ID");//SI.SubscriberID as Secondary_Ins_Subscriber_ID, 
                MySqlDt.Columns.Add("due_date");//DD.DueDate as due_date, 
                MySqlDt.Columns.Add("remaining_benefit");//IFNULL(AC.TotalBenafit, 0) AS remaining_benefit,
                MySqlDt.Columns.Add("collect_payment");//CP.CollectPayment AS collect_payment, 
                MySqlDt.Columns.Add("used_benefit");//IFNULL(AA.UsedBenafit, 0) As used_benefit,
                MySqlDt.Columns.Add("Prim_Ins_Company_Phonenumber");//'' AS Prim_Ins_Company_Phonenumber,
                MySqlDt.Columns.Add("Sec_Ins_Company_Phonenumber");//'' as Sec_Ins_Company_Phonenumber,

                DataTable dtUsedBenefit = new DataTable();
                string strUsedBenefit = Clinic_Number != "" ?
                    "select patnum, IFNULL( sum(InsPayAmt), 0 ) AS UsedBenafit from claimproc Where ClinicNum = @Clinic_Number"
                    :
                    "select patnum, IFNULL( sum(InsPayAmt), 0 ) AS UsedBenafit from claimproc ";
                if (!string.IsNullOrEmpty(PatientEHRID))
                {
                    StringBuilder strNewQuery = new StringBuilder();
                    strNewQuery.Append(strUsedBenefit);
                    strNewQuery.Append(" And patnum in (" + PatientEHRID + ")");
                    strUsedBenefit = strNewQuery.ToString();
                }
                strUsedBenefit = strUsedBenefit + " group by patnum";
                CommonDB.MySqlCommandServer(strUsedBenefit, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtUsedBenefit);
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 3");
                var updateUsedBenefit = from r1 in MySqlDt.AsEnumerable()
                                        join r2 in dtUsedBenefit.AsEnumerable()
                                        on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patnum"].ToString().Trim()
                                        into outer
                                        from r2 in outer.DefaultIfEmpty()
                                        select new { r1, UsedBenefit = (r2 == null ? "0" : r2["UsedBenafit"]) };
                foreach (var x in updateUsedBenefit)
                {
                    x.r1.SetField("used_benefit", x.UsedBenefit == DBNull.Value ? "0" : Convert.ToString(x.UsedBenefit));
                }
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 4");
                DataTable dtCollectPayment = new DataTable();
                string strCollectPayment = Clinic_Number != "" ?
                    "SELECT PatNum, IFNULL( Sum(Payamt), 0 ) AS CollectPayment FROM Payment WHERE ClinicNum = @Clinic_Number "
                    :
                    "SELECT PatNum, IFNULL( Sum(Payamt), 0 ) AS CollectPayment FROM Payment ";
                if (!string.IsNullOrEmpty(PatientEHRID))
                {
                    StringBuilder strNewQuery = new StringBuilder();
                    strNewQuery.Append(strCollectPayment);
                    strNewQuery.Append(" And patnum in (" + PatientEHRID + ")");
                    strCollectPayment = strNewQuery.ToString();
                }
                strCollectPayment = strCollectPayment + " Group by PatNum";

                CommonDB.MySqlCommandServer(strCollectPayment, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtCollectPayment);
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 5");
                var updateCollectPayment = from r1 in MySqlDt.AsEnumerable()
                                           join r2 in dtCollectPayment.AsEnumerable()
                                           on r1["Patient_EHR_ID"] equals r2["patnum"]
                                           into outer
                                           from r2 in outer.DefaultIfEmpty()
                                           select new { r1, CollectPayment = (r2 == null ? "0" : r2["CollectPayment"]) };
                foreach (var x in updateCollectPayment)
                {
                    x.r1.SetField("collect_payment", x.CollectPayment == DBNull.Value ? "0" : Convert.ToString(x.CollectPayment));
                }
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 6");
                DataTable dtTotalBenefit = new DataTable();
                string strTotalBenefit = Clinic_Number != "" ?
                    "SELECT distinct i.subscriber AS PatNum, b.benefitNum, IFNULL( (Case When b.MonetaryAmt < 0 Then 0 Else b.MonetaryAmt END), 0 ) AS TotalBenafit FROM benefit b INNER JOIN inssub i on i.plannum = b.plannum and benefitType = 5 and CoverageLevel = 1"
                    :
                    "SELECT distinct i.subscriber AS PatNum, b.benefitNum, IFNULL( (Case When b.MonetaryAmt < 0 Then 0 Else b.MonetaryAmt END), 0 ) AS TotalBenafit FROM benefit b INNER JOIN inssub i on i.plannum = b.plannum and benefitType = 5 and CoverageLevel = 1";
                if (!string.IsNullOrEmpty(PatientEHRID))
                {
                    StringBuilder strNewQuery = new StringBuilder();
                    strNewQuery.Append(strTotalBenefit);
                    strNewQuery.Append(" And i.subscriber in (" + PatientEHRID + ")");
                    strTotalBenefit = strNewQuery.ToString();
                }
                strTotalBenefit = strTotalBenefit + " group by i.subscriber";
                CommonDB.MySqlCommandServer(strTotalBenefit, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtTotalBenefit);
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 7");
                var updateTotalBenefit = from r1 in MySqlDt.AsEnumerable()
                                         join r2 in dtTotalBenefit.AsEnumerable()
                                         on r1["Patient_EHR_ID"] equals r2["patnum"]
                                         into outer
                                         from r2 in outer.DefaultIfEmpty()
                                         select new { r1, TotalBenafit = (r2 == null ? "0" : r2["TotalBenafit"]) };
                foreach (var x in updateTotalBenefit)
                {
                    x.r1.SetField("remaining_benefit", x.TotalBenafit == DBNull.Value ? "0" : Convert.ToString(x.TotalBenafit));
                }
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 8");

                DataTable dtDueDate = new DataTable();
                string strDueDate = Clinic_Number != "" ?
                    "Select patient_id, group_concat( DueDate Order By DateDue Asc, Description Asc separator '|' ) as DueDate FROM ( SELECT r.patnum AS patient_id, p.clinicnum As Clinic_Number, CONCAT( r.DateDue, '@', rt.description, '@', rt.RecallTypeNum ) AS DueDate, r.DateDue, rt.description FROM recall r INNER Join patient p on p.PatNum = r.PatNum AND P.ClinicNum = @Clinic_Number INNER JOIN appointment ap on ap.patnum = p.patnum AND ap.ClinicNum = @Clinic_Number INNER JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum where p.clinicnum = @Clinic_Number AND r.DateDue > '00010101' Group By r.patnum, r.RecallTypeNum Order By r.DateDue ) AS B "
                    :
                    "Select patient_id, group_concat( DueDate Order By DateDue Asc, Description Asc separator '|' ) as DueDate FROM ( SELECT r.patnum AS patient_id, p.clinicnum As Clinic_Number, CONCAT( r.DateDue, '@', rt.description, '@', rt.RecallTypeNum ) AS DueDate, r.DateDue, rt.description FROM recall r INNER Join patient p on p.PatNum = r.PatNum INNER JOIN appointment ap on ap.patnum = p.patnum INNER JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum where r.DateDue > '00010101' Group By r.patnum, r.RecallTypeNum Order By r.DateDue ) AS B";
                if (!string.IsNullOrEmpty(PatientEHRID))
                {
                    StringBuilder strNewQuery = new StringBuilder();
                    strNewQuery.Append(strDueDate);
                    strNewQuery.Append(" Where patient_id in (" + PatientEHRID + ")");
                    strDueDate = strNewQuery.ToString();
                }
                strDueDate = strDueDate + " Group By B.patient_id";
                CommonDB.MySqlCommandServer(strDueDate, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtDueDate);
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 9");
                var updateDueDate = from r1 in MySqlDt.AsEnumerable()
                                    join r2 in dtDueDate.AsEnumerable()
                                    on r1["Patient_EHR_ID"] equals r2["patient_id"]
                                    into outer
                                    from r2 in outer.DefaultIfEmpty()
                                    select new { r1, Due_Date = (r2 == null ? "" : r2["DueDate"]) };
                foreach (var x in updateDueDate)
                {
                    x.r1.SetField("due_date", x.Due_Date);
                }
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 10");
                DataTable dtPrimaryInsurance = new DataTable();
                string strPrimaryInsurance = Clinic_Number != "" ?
                    "SELECT pat.PatNum, ip.carrierNum as Primary_Insurance, c.CarrierName as Primary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum AND pat.Clinicnum = @Clinic_Number join carrier c on c.carrierNum = ip.carrierNum where pat.clinicnum = @Clinic_Number and pp.ordinal = 1"
                    :
                    "SELECT pat.PatNum, ip.carrierNum as Primary_Insurance, c.CarrierName as Primary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum join carrier c on c.carrierNum = ip.carrierNum where PP.Ordinal = 1";
                if (!string.IsNullOrEmpty(PatientEHRID))
                {
                    StringBuilder strNewQuery = new StringBuilder();
                    strNewQuery.Append(strPrimaryInsurance);
                    strNewQuery.Append(" And pat.PatNum in (" + PatientEHRID + ")");
                    strPrimaryInsurance = strNewQuery.ToString();
                }

                CommonDB.MySqlCommandServer(strPrimaryInsurance, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtPrimaryInsurance);
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 11");
                var updatePrimaryInsurance = from r1 in MySqlDt.AsEnumerable()
                                             join r2 in dtPrimaryInsurance.AsEnumerable()
                                             on r1["Patient_EHR_ID"] equals r2["patnum"]
                                             into outer
                                             from r2 in outer.DefaultIfEmpty()
                                             select new
                                             {
                                                 r1,
                                                 Primary_Insurance = (r2 == null ? "" : r2["Primary_Insurance"]),
                                                 Primary_Insurance_CompanyName = (r2 == null ? "" : r2["Primary_Insurance_CompanyName"]),
                                                 SubscriberID = (r2 == null ? "" : r2["SubscriberID"]),
                                                 Prim_Ins_Company_Phonenumber = (r2 == null ? "" : r2["Prim_Ins_Company_Phonenumber"])
                                             };
                foreach (var x in updatePrimaryInsurance)
                {
                    x.r1.SetField("Primary_Insurance", x.Primary_Insurance);
                    x.r1.SetField("Primary_Insurance_CompanyName", x.Primary_Insurance_CompanyName);
                    x.r1.SetField("Primary_Ins_Subscriber_ID", x.SubscriberID);
                    x.r1.SetField("Prim_Ins_Company_Phonenumber", x.Prim_Ins_Company_Phonenumber);
                }
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 12");

                DataTable dtSecondaryInsurance = new DataTable();
                string strSecondaryInsurance = Clinic_Number != "" ?
                    "SELECT pat.PatNum, ip.carrierNum as Secondary_Insurance, c.CarrierName as Secondary_Insurance_CompanyName, c.Phone as Secondary_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum AND pat.clinicnum = @Clinic_Number join carrier c on c.carrierNum = ip.carrierNum where pat.clinicnum = @Clinic_Number and pp.ordinal = 2"
                    :
                    "SELECT pat.PatNum, ip.carrierNum as Secondary_Insurance, c.CarrierName as Secondary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid FROM patplan pp join inssub i on pp.InsSubNum = i.InsSubNum join insplan ip on ip.PlanNum = i.PlanNum join patient pat on pat.PatNum = pp.PatNum join carrier c on c.carrierNum = ip.carrierNum where PP.Ordinal = 2";
                if (!string.IsNullOrEmpty(PatientEHRID))
                {
                    StringBuilder strNewQuery = new StringBuilder();
                    strNewQuery.Append(strSecondaryInsurance);
                    strNewQuery.Append(" And pat.PatNum in (" + PatientEHRID + ")");
                    strSecondaryInsurance = strNewQuery.ToString();
                }
                CommonDB.MySqlCommandServer(strSecondaryInsurance, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtSecondaryInsurance);
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 13");
                var updateSecondaryInsurance = from r1 in MySqlDt.AsEnumerable()
                                               join r2 in dtSecondaryInsurance.AsEnumerable()
                                               on r1["Patient_EHR_ID"] equals r2["patnum"]
                                               into outer
                                               from r2 in outer.DefaultIfEmpty()
                                               select new
                                               {
                                                   r1,
                                                   Secondary_Insurance = (r2 == null ? "" : r2["Secondary_Insurance"]),
                                                   Secondary_Insurance_CompanyName = (r2 == null ? "" : r2["Secondary_Insurance_CompanyName"]),
                                                   SubscriberID = (r2 == null ? "" : r2["SubscriberID"]),
                                                   Sec_Ins_Company_Phonenumber = (r2 == null ? "" : r2["Secondary_Ins_Company_Phonenumber"])
                                               };
                foreach (var x in updateSecondaryInsurance)
                {
                    x.r1.SetField("Secondary_Insurance", x.Secondary_Insurance);
                    x.r1.SetField("Secondary_Insurance_CompanyName", x.Secondary_Insurance_CompanyName);
                    x.r1.SetField("Secondary_Ins_Subscriber_ID", x.SubscriberID);
                    x.r1.SetField("Sec_Ins_Company_Phonenumber", x.Sec_Ins_Company_Phonenumber);
                }
                Utility.WriteToSyncLogFile_All("GetOpenDentalPatientDataNew 14, Rows Count: " + MySqlDt.Rows.Count);
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

        public static DataTable GetOpenDentalPatientStatusData(string Clinic_Number, string DbString, string Patient_EHR_ID = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = Clinic_Number != "" ? SynchOpenDentalQRY.GetOpenDentalPatientStatusNew_Existing_Clinic : SynchOpenDentalQRY.GetOpenDentalPatientStatusNew_Existing;
                if (!string.IsNullOrEmpty(Patient_EHR_ID))
                {
                    MySqlSelect = MySqlSelect + " And PatNum = '" + Patient_EHR_ID + "'";
                }
                else
                {
                    MySqlSelect = MySqlSelect + ";";
                }
                MySqlSelect = MySqlSelect.Replace("curdate()", "'" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'");
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalApptPatientdue_date(string Clinic_number, string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalApptPatientdue_date;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("clinicnum", Clinic_number);
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy/MM/dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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
        public static DataTable GetOpenDentalAppointmentsPatientData(string Clinic_Number, string DbString, bool isNew, string Pat_EHR_ID = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                if (isNew)
                {
                    return GetOpenDentalAppointmentsPatientDataNew(Clinic_Number, DbString, Pat_EHR_ID);
                }
                // MySqlCommand.CommandTimeout = 120;
                //CommonDB.MySQLConnectionServer(ref conn, DbString);
                if (conn.State == ConnectionState.Closed) conn.Open();
                // string MySqlSelect = Clinic_Number != "" ? SynchOpenDentalQRY.GetOpenDentalAppointmentsPatientData_Clinic : SynchOpenDentalQRY.GetOpenDentalAppointmentsPatientData;
                string MySqlSelect = "";
                if (!isNew)
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalAppointmentsPatientData_Clinic;
                }
                else
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalAppointmentsPatientData_Clinic_New;
                }

                if (!string.IsNullOrEmpty(Pat_EHR_ID))
                {
                    MySqlSelect = MySqlSelect + " And p.PatNum = '" + Pat_EHR_ID + "'";
                }

                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy/MM/dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalAppointmentsPatientDataNew(string Clinic_Number, string DbString, string Pat_EHR_ID = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                // string MySqlSelect = Clinic_Number != "" ? SynchOpenDentalQRY.GetOpenDentalAppointmentsPatientData_Clinic : SynchOpenDentalQRY.GetOpenDentalAppointmentsPatientData;
                string MySqlSelect = "SELECT DISTINCT '0' as 'Patient_LocalDB_ID', p.PatNum AS Patient_EHR_ID, '0' as 'Patient_Web_ID', p.fname AS First_name, p.lname AS Last_name, p.MiddleI AS Middle_Name, p.Salutation, case when p.patstatus = 0 then 'A' else 'I' end as Status, case when p.Gender = 0 then 'Male' when p.Gender = 1 then 'Female' when p.Gender > 1 then 'Unknown' end AS Sex, case when p.Position = 0 then 'Single' when p.Position = 1 then 'Married' when p.Position = 2 then 'Child' when p.Position = 3 then 'Widowed' when p.Position = 4 then 'Divorced' end AS MaritalStatus, IF( p.Birthdate = '0001-01-01' or p.Birthdate = '0000-00-00', NULL, p.Birthdate ) AS Birth_Date, p.Email AS Email, REPLACE( REPLACE( REPLACE( REPLACE(p.WirelessPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Mobile, REPLACE( REPLACE( REPLACE( REPLACE(p.HmPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Home_Phone, REPLACE( REPLACE( REPLACE( REPLACE(p.WkPhone, '(', ''), ')', '' ), '-', '' ), ' ', '' ) AS Work_Phone, p.Address AS Address1, p.Address2 AS Address2, p.City AS City, p.State AS State, p.Zip AS Zipcode, '' as ResponsibleParty_Status, p.EstBalance AS CurrentBal, p.Bal_0_30 AS ThirtyDay, p.Bal_31_60 AS SixtyDay, p.Bal_61_90 AS NinetyDay, p.BalOver90 AS Over90, IF( p.DateFirstVisit = '0001-01-01' or p.DateFirstVisit = '0000-00-00', NULL, p.DateFirstVisit ) AS FirstVisit_Date, LastVisit_Date," +
                                     " p.Guarantor AS Guar_ID, p.PriProv AS Pri_Provider_ID, p.SecProv AS Sec_Provider_ID, (case when Cast(p.TxtMsgOk As Char) = '0' or Cast(p.TxtMsgOk As Char) = '' or Cast(p.TxtMsgOk As Char) = '1' then 'Y' else 'N' end) as ReceiveSMS, p.email as ReceiveEmail, NextVisit_Date, " +
                                     " Now() as Last_Sync_Date, p.dateTStamp AS EHR_Entry_DateTime, 'false' as 'Is_Adit_Updated', p.Preferred As preferred_name, " +
                                     " p.clinicnum as Clinic_Number, '' as 'Service_Install_Id', case when p.patstatus IN(4, 5) then 1 else 0 end as is_deleted, case when p.patstatus = 0 then 'Active' when p.patstatus = 1 then 'NonPatient' when p.patstatus = 2 then 'InActive' when p.patstatus = 3 then 'InActive' when p.patstatus IN(4, 5) then 'Deleted' when p.patstatus = 6 then 'NonPatient' end as EHR_Status, case when p.prefercontactmethod = 1 then 'N' else 'Y' end as ReceiveVoiceCall, '' as 'Patient_status', '' as 'Patient_status_Compare', p.AddrNote AS Patient_Note, CASE WHEN ISNULL(p.language) THEN 'eng' When p.Language = '' Then 'eng' ELSE p.language END AS PreferredLanguage, p.SSN as SSN, '' as driverlicense, '' AS groupid, '' AS emergencycontactId, pn.ICEPhone AS emergencycontactnumber, p.SchoolName as school, emp.EmpName AS employer, PG.spouseId, " +
                                     " resp.responsparty AS responsiblepartyId, resp.SSN AS responsiblepartyssn, resp.Birthdate AS responsiblepartybirthdate, PG.Spouse_Last_Name, " +
                                     " resp.FName AS ResponsibleParty_First_Name, resp.Lname AS ResponsibleParty_Last_Name, " +
                                     " pn.ICEName AS EmergencyContact_First_Name, '' AS EmergencyContact_Last_Name, " +
                                     " PG.Spouse_First_Name, 'false' as Is_Status_Adit_Updated, 1 AS InsUptDlt " +
                                     " FROM " +
                                     "  patient p " +
                                     "  INNER join(select patnum, min(aptdatetime) as aptdatetime from appointment where aptdatetime > @ToDate group by patnum) ap on p.patnum = ap.patnum " +
                                     "  left join employer as emp on p.EmployerNum = emp.EmployerNum " +
                                     "  left join patientnote as pn on p.Patnum = pn.Patnum " +
                                     "  left join patient as resp on p.Patnum = resp.responsparty and resp.responsparty > 0 " +
                                     "  LEFT JOIN(SELECT PT.patnum, max(PT.AptDateTime) as LastVisit_Date from appointment PT WHERE AptDateTime < Now() AND PT.ClinicNum = @Clinic_Number GROUP by PT.patnum) Ldata on p.PatNum = Ldata.patnum " +
                                     "  LEFT JOIN(select patnumchild, max(patnumguardian) as spouseId, FName as Spouse_First_Name, LName as Spouse_Last_Name from patient pa INNER JOIN guardian g ON pa.patnum = g.patnumguardian and relationship = 18 group by patnumchild ) PG on p.patnum = PG.patnumchild " +
                                     "  LEFT JOIN(SELECT PT.patnum, max(PT.AptDateTime) as nextvisit_date from appointment PT WHERE AptDateTime > Now() AND AptStatus NOT IN(3, 5) AND PT.ClinicNum = @Clinic_Number GROUP by PT.patnum) Ndata on p.PatNum = Ndata.patnum " +
                                     (!string.IsNullOrEmpty(Pat_EHR_ID) ? " Where p.PatNum = '" + Pat_EHR_ID + "'" : "");

                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 1");
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy/MM/dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 2");

                MySqlDt.Columns.Add("Primary_Insurance");//PI.Primary_Insurance, 
                MySqlDt.Columns.Add("Primary_Insurance_CompanyName");//PI.Primary_Insurance_CompanyName, 
                MySqlDt.Columns.Add("Primary_Ins_Subscriber_ID");//PI.SubscriberID as Primary_Ins_Subscriber_ID, 
                MySqlDt.Columns.Add("Secondary_Insurance");//SI.Secondary_Insurance, 
                MySqlDt.Columns.Add("Secondary_Insurance_CompanyName");//SI.Secondary_Insurance_CompanyName, 
                MySqlDt.Columns.Add("Secondary_Ins_Subscriber_ID");//SI.SubscriberID as Secondary_Ins_Subscriber_ID, 
                MySqlDt.Columns.Add("due_date");//DD.DueDate as due_date, 
                MySqlDt.Columns.Add("remaining_benefit");//IFNULL(AC.TotalBenafit, 0) AS remaining_benefit,
                MySqlDt.Columns.Add("collect_payment");//CP.CollectPayment AS collect_payment, 
                MySqlDt.Columns.Add("used_benefit");//IFNULL(AA.UsedBenafit, 0) As used_benefit,
                MySqlDt.Columns.Add("Prim_Ins_Company_Phonenumber");//'' AS Prim_Ins_Company_Phonenumber,
                MySqlDt.Columns.Add("Sec_Ins_Company_Phonenumber");//'' as Sec_Ins_Company_Phonenumber,

                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 3");
                DataTable dtUsedBenefit = new DataTable();
                string strUsedBenefit = "select CP.patnum, IFNULL(sum(CP.InsPayAmt), 0) AS UsedBenafit from claimproc CP INNER join appointment  ap on CP.patnum = ap.patnum  Where CP.ClinicNum = @Clinic_Number AND ap.aptdatetime >  @ToDate " + 
                                        (!string.IsNullOrEmpty(Pat_EHR_ID) ? " And CP.PatNum = '" + Pat_EHR_ID + "'" : "") + 
                                        " group by CP.patnum";
                CommonDB.MySqlCommandServer(strUsedBenefit, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy/MM/dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtUsedBenefit);
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 4.");
                var updateUsedBenefit = from r1 in MySqlDt.AsEnumerable()
                                        join r2 in dtUsedBenefit.AsEnumerable()
                                        on r1["Patient_EHR_ID"].ToString().Trim() equals r2["patnum"].ToString().Trim()
                                        into outer
                                        from r2 in outer.DefaultIfEmpty()
                                        select new { r1, UsedBenefit = (r2 == null ? "0" : r2["UsedBenafit"]) };
                foreach (var x in updateUsedBenefit)
                {
                    x.r1.SetField("used_benefit", x.UsedBenefit == DBNull.Value ? "0" : Convert.ToString(x.UsedBenefit));
                }
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 5");

                DataTable dtCollectPayment = new DataTable();
                string strCollectPayment = "SELECT PP.PatNum, IFNULL(Sum(PP.Payamt), 0) AS CollectPayment FROM payment PP INNER JOIN Appointment AP ON PP.PatNum = AP.Patnum WHERE PP.ClinicNum = @Clinic_Number AND ap.aptdatetime >  @ToDate " +
                                           (!string.IsNullOrEmpty(Pat_EHR_ID) ? " And PP.PatNum = '" + Pat_EHR_ID + "'" : "") + 
                                           " Group by PatNum";
                CommonDB.MySqlCommandServer(strCollectPayment, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy/MM/dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtCollectPayment);
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 6");

                var updateCollectPayment = from r1 in MySqlDt.AsEnumerable()
                                           join r2 in dtCollectPayment.AsEnumerable()
                                           on r1["Patient_EHR_ID"] equals r2["patnum"]
                                           into outer
                                           from r2 in outer.DefaultIfEmpty()
                                           select new { r1, CollectPayment = (r2 == null ? "0" : r2["CollectPayment"]) };
                foreach (var x in updateCollectPayment)
                {
                    x.r1.SetField("collect_payment", x.CollectPayment == DBNull.Value ? "0" : Convert.ToString(x.CollectPayment));
                }
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 7");
                DataTable dtTotalBenefit = new DataTable();
                string strTotalBenefit = "SELECT distinct i.subscriber AS PatNum, b.benefitNum, IFNULL((Case When b.MonetaryAmt < 0 Then 0 Else b.MonetaryAmt END), 0) AS TotalBenafit " +
                                         "  FROM benefit b  " +
                                         "  INNER JOIN inssub i on i.plannum = b.plannum and benefitType = 5 and CoverageLevel = 1  " +
                                         "  INNER join appointment ap on i.subscriber = ap.patnum  " +
                                         "  where ap.aptdatetime > @ToDate " +
                                         (!string.IsNullOrEmpty(Pat_EHR_ID) ? " And i.subscriber = '" + Pat_EHR_ID + "'" : "") +
                                         "  group by i.subscriber";
                CommonDB.MySqlCommandServer(strTotalBenefit, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy/MM/dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtTotalBenefit);
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 8");
                var updateTotalBenefit = from r1 in MySqlDt.AsEnumerable()
                                         join r2 in dtTotalBenefit.AsEnumerable()
                                         on r1["Patient_EHR_ID"] equals r2["patnum"]
                                         into outer
                                         from r2 in outer.DefaultIfEmpty()
                                         select new { r1, TotalBenafit = (r2 == null ? "0" : r2["TotalBenafit"]) };
                foreach (var x in updateTotalBenefit)
                {
                    x.r1.SetField("remaining_benefit", x.TotalBenafit == DBNull.Value ? "0" : Convert.ToString(x.TotalBenafit));
                }
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 9");
                DataTable dtDueDate = new DataTable();
                string strDueDate = "Select patient_id, group_concat( DueDate Order By DateDue Asc, Description Asc separator '|' ) as DueDate FROM ( SELECT r.patnum AS patient_id, p.clinicnum As Clinic_Number, CONCAT( r.DateDue, '@', rt.description, '@', rt.RecallTypeNum ) AS DueDate, r.DateDue, rt.description FROM recall r INNER Join patient p on p.patnum = r.patnum AND p.clinicnum = @Clinic_Number INNER JOIN appointment ap on ap.patnum = p.patnum INNER JOIN recalltype rt on r.RecallTypeNum = rt.RecallTypeNum where ap.aptdatetime > @ToDate AND p.clinicnum = @Clinic_Number AND r.DateDue > '00010101' Group By r.patnum, r.RecallTypeNum Order By r.DateDue ) AS B Group By B.patient_id";
                CommonDB.MySqlCommandServer(strDueDate, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy/MM/dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtDueDate);
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 10");
                var updateDueDate = from r1 in MySqlDt.AsEnumerable()
                                    join r2 in dtDueDate.AsEnumerable()
                                    on r1["Patient_EHR_ID"] equals r2["patient_id"]
                                    into outer
                                    from r2 in outer.DefaultIfEmpty()
                                    select new { r1, Due_Date = (r2 == null ? "" : r2["DueDate"]) };
                foreach (var x in updateDueDate)
                {
                    x.r1.SetField("due_date", x.Due_Date);
                }
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 11");
                DataTable dtPrimaryInsurance = new DataTable();
                string strPrimaryInsurance = "SELECT pat.PatNum, ip.carrierNum as Primary_Insurance, c.CarrierName as Primary_Insurance_CompanyName, c.Phone as Prim_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid " +
                                              "  FROM patient pat " +
                                              "  INNER join appointment ap on pat.patnum = ap.patnum " +
                                              "  INNER join Patplan pp  on pat.PatNum = pp.PatNum AND pat.Clinicnum = @Clinic_Number " +
                                              "  INNER join inssub i on pp.InsSubNum = i.InsSubNum " +
                                              "  INNER join insplan ip on ip.PlanNum = i.PlanNum " +
                                              "  INNER join carrier c on c.carrierNum = ip.carrierNum " +
                                              "  where pat.clinicnum = @Clinic_Number AND ap.aptdatetime > @ToDate " +
                                              (!string.IsNullOrEmpty(Pat_EHR_ID) ? " And pat.PatNum = '" + Pat_EHR_ID + "'" : "") +
                                              " and pp.ordinal = 1";
                CommonDB.MySqlCommandServer(strPrimaryInsurance, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy/MM/dd"));

                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtPrimaryInsurance);
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 12");
                var updatePrimaryInsurance = from r1 in MySqlDt.AsEnumerable()
                                             join r2 in dtPrimaryInsurance.AsEnumerable()
                                             on r1["Patient_EHR_ID"] equals r2["patnum"]
                                             into outer
                                             from r2 in outer.DefaultIfEmpty()
                                             select new
                                             {
                                                 r1,
                                                 Primary_Insurance = (r2 == null ? "" : r2["Primary_Insurance"]),
                                                 Primary_Insurance_CompanyName = (r2 == null ? "" : r2["Primary_Insurance_CompanyName"]),
                                                 SubscriberID = (r2 == null ? "" : r2["SubscriberID"]),
                                                 Prim_Ins_Company_Phonenumber = (r2 == null ? "" : r2["Prim_Ins_Company_Phonenumber"])
                                             };
                foreach (var x in updatePrimaryInsurance)
                {
                    x.r1.SetField("Primary_Insurance", x.Primary_Insurance);
                    x.r1.SetField("Primary_Insurance_CompanyName", x.Primary_Insurance_CompanyName);
                    x.r1.SetField("Primary_Ins_Subscriber_ID", x.SubscriberID);
                    x.r1.SetField("Prim_Ins_Company_Phonenumber", x.Prim_Ins_Company_Phonenumber);
                }
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 13");
                DataTable dtSecondaryInsurance = new DataTable();
                string strSecondaryInsurance = " SELECT pat.PatNum, ip.carrierNum as Secondary_Insurance, c.CarrierName as Secondary_Insurance_CompanyName, c.Phone as Secondary_Ins_Company_Phonenumber, i.SubscriberID, pat.clinicnum as Clinic_Number, GroupNum as groupid " +
                                              "  FROM patient pat " +
                                              "  INNER join appointment ap on pat.patnum = ap.patnum" +
                                              "  INNER join Patplan pp  on pat.PatNum = pp.PatNum AND pat.Clinicnum = @Clinic_Number" +
                                              "  INNER join inssub i on pp.InsSubNum = i.InsSubNum" +
                                              "  INNER join insplan ip on ip.PlanNum = i.PlanNum" +
                                              "  INNER join carrier c on c.carrierNum = ip.carrierNum" +
                                              "  where pat.clinicnum = @Clinic_Number AND ap.aptdatetime > @ToDate " +
                                              (!string.IsNullOrEmpty(Pat_EHR_ID) ? " And pat.PatNum = '" + Pat_EHR_ID + "'" : "") +
                                              "  and pp.ordinal = 2";
                CommonDB.MySqlCommandServer(strSecondaryInsurance, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy/MM/dd"));

                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtSecondaryInsurance);
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 14");
                var updateSecondaryInsurance = from r1 in MySqlDt.AsEnumerable()
                                               join r2 in dtSecondaryInsurance.AsEnumerable()
                                               on r1["Patient_EHR_ID"] equals r2["patnum"]
                                               into outer
                                               from r2 in outer.DefaultIfEmpty()
                                               select new
                                               {
                                                   r1,
                                                   Secondary_Insurance = (r2 == null ? "" : r2["Secondary_Insurance"]),
                                                   Secondary_Insurance_CompanyName = (r2 == null ? "" : r2["Secondary_Insurance_CompanyName"]),
                                                   SubscriberID = (r2 == null ? "" : r2["SubscriberID"]),
                                                   Sec_Ins_Company_Phonenumber = (r2 == null ? "" : r2["Secondary_Ins_Company_Phonenumber"])
                                               };
                foreach (var x in updateSecondaryInsurance)
                {
                    x.r1.SetField("Secondary_Insurance", x.Secondary_Insurance);
                    x.r1.SetField("Secondary_Insurance_CompanyName", x.Secondary_Insurance_CompanyName);
                    x.r1.SetField("Secondary_Ins_Subscriber_ID", x.SubscriberID);
                    x.r1.SetField("Sec_Ins_Company_Phonenumber", x.Sec_Ins_Company_Phonenumber);
                }
                Utility.WriteToSyncLogFile_All("OpenDentalGetApptPatDate: 15, Rows Count: " + MySqlDt.Rows.Count);
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

        public static DataTable GetOpenDentalPatientDocData(string DbString, string strPaientID = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientDocData;
                if (!string.IsNullOrEmpty(strPaientID))
                {
                    MySqlSelect = MySqlSelect + " Where p.PatNum = '" + strPaientID + "';";
                }
                else
                {
                    MySqlSelect = MySqlSelect + ";";
                }
                if (!string.IsNullOrEmpty(strPaientID))
                {
                    MySqlSelect = MySqlSelect + "";
                }
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalPatientdue_date(string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientdue_date;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalPatientWiseRecallDate(string DbString, string serviceInstallId)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientWiseRecallDates;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("@Service_Install_Id", serviceInstallId);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalPatientNextApptDate(string Clinic_number, string DbString)
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalNextApptDate;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Clinic_number", Clinic_number);
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd hh:mm:00"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalPatientInsBenafit(string Clinic_number, string PatientId, string DbString)
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {

                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientInsBenafit;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("Clinic_number", Clinic_number);
                MySqlCommand.Parameters.AddWithValue("PatientId", PatientId);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalPatientImagesData(string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if(DbString.ToLower().Contains("freedental"))
                {
                    string MySqlSelect = SynchOpenDentalQRY.GetPWPatientImagesData;
                    CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                }
                else
                {
                    // MySqlCommand.CommandTimeout = 120;
                    string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientImagesData;
                    CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                }

                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalPatientLastVisit_Date(string Clinic_number, string DbString)
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientLastVisit_Date;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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


        public static DataTable GetOpenDentalRecallTypeData(string DbString, string Service_Install_Id)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalRecallTypeData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);

                DataTable Dttmp = MySqlDt.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                    {
                        for (int j = 0; j < MySqlDt.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp.NewRow();
                            Dr.ItemArray = MySqlDt.Rows[j].ItemArray;
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

        public static DataTable GetOpenDentalPatientID_NameData(string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientID_NameData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalIdelProvider(string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalIdelProvider;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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
        public static DataTable GetOpenDentalPatientInsuranceData(string Clinic_Number, string PatientId, string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalPatientInsuranceData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("Clinic_number", Clinic_Number);
                MySqlCommand.Parameters.AddWithValue("PatientId", PatientId);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static DataTable GetOpenDentalAppointmentStatus(string DbString, string Service_Install_Id)
        {


            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalAppointmentStatus;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);

                DataTable Dttmp = MySqlDt.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                    {
                        for (int j = 0; j < MySqlDt.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp.NewRow();
                            Dr.ItemArray = MySqlDt.Rows[j].ItemArray;
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

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
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
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);

                        SqlCeCommand.ExecuteNonQuery();
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

        //rooja add Insurance Master
        public static DataTable GetOpenDentalInsuranceData(string DbString, string Service_Install_Id)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalInsuranceData;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.Clear();
                //MySqlCommand.Parameters.AddWithValue("Clinic_number", Clinic_Number);

                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
                //rooja 20-8-24              

                DataTable Dttmp = MySqlDt.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                    {
                        for (int j = 0; j < MySqlDt.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp.NewRow();
                            Dr.ItemArray = MySqlDt.Rows[j].ItemArray;
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


        public static bool Is_Update_Status_EHR_Appointment_Live_To_Opendental(string Appt_EHR_ID, string DbString)
        {
            bool _successfullstataus = true;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.Is_Update_Status_EHR_Appointment_Live_To_Opendental;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("@Appt_EHR_ID", Appt_EHR_ID);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);

                if (MySqlDt.Rows.Count > 0)
                {
                    if (MySqlDt.Rows[0]["AptStatus"].ToString() == "2")
                    {
                        _successfullstataus = false;
                    }
                }
                else
                {
                    _successfullstataus = false;
                }
                return _successfullstataus;
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

        public static bool Update_Status_EHR_Appointment_Live_To_Opendental(DataTable dtLiveAppointment, string DbString,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            bool _successfullstataus = true;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string confirmed_status_id = string.Empty;
                string sqlSelect = string.Empty;
                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
                foreach (DataRow dr in dtLiveAppointment.Rows)
                {
                    confirmed_status_id = dr["confirmed_status_ehr_key"].ToString();

                    if (confirmed_status_id == "")
                    {
                        confirmed_status_id = "21";
                    }

                    MySqlCommand.CommandText = SynchOpenDentalQRY.Update_Status_EHR_Appointment_Live_To_Opendental;

                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString());
                    MySqlCommand.Parameters.AddWithValue("AptStatus", confirmed_status_id);
                    MySqlCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString());
                    MySqlCommand.ExecuteNonQuery();
                    if (_filename_EHR_appointment != "" && _EHRLogdirectory_EHR_appointment != "")
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Update Status EHR Appointment Live To Opendental EHR appointment Confirmed in EHR for Appt_EHR_ID : (" + dr["Appt_EHR_ID"] + ") and AptStatus :(" + confirmed_status_id.ToString() + ")");
                    }
                    bool isApptConformStatus = SynchLocalDAL.UpdateLocalAppointmentConformStatusData(dr["Appt_EHR_ID"].ToString(), dr["Service_Install_Id"].ToString(), _filename_EHR_appointment, _EHRLogdirectory_EHR_appointment);
                    if (isApptConformStatus)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Update Local Appointment Conform Status : success");
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

        public static bool Update_Receive_SMS_Patient_EHR_Live_To_Opendental(DataTable dtLiveAppointment, string DbString, string LocationId, string Loc_ID,string _filename_EHR_patientoptout = "",string _EHRLogdirectory_EHR_patientoptout = "")
        {
            bool _successfullstataus = true;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string confirmed_status_id = string.Empty;
                string sqlSelect = string.Empty;
                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
                Patient_OptOutBO_StatusUpdate updatedPatientid = new Patient_OptOutBO_StatusUpdate();
                List<Patientids_OptOutBO_StatusUpdate> Patient_StatusUpdate = new List<Patientids_OptOutBO_StatusUpdate>();
                foreach (DataRow dr in dtLiveAppointment.Rows)
                {
                    try
                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.Update_Receive_SMS_Patient_EHR_Live_To_Opendental;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("TxtMsgOk", dr["receive_sms"].ToString() == "Y" ? 1 : 2);
                        MySqlCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString());
                        MySqlCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_patientoptout, _EHRLogdirectory_EHR_patientoptout, " Update Receive SMS Patient EHR Live To Opendental for  patient_ehr_id=" + dr["patient_ehr_id"].ToString() + " and receive_sms=" + dr["receive_sms"].ToString());
                        Patientids_OptOutBO_StatusUpdate Patientids = new Patientids_OptOutBO_StatusUpdate();
                        Patientids.esId = dr["esid"].ToString();
                        Patientids.patientId = dr["patientid"].ToString();
                        Patient_StatusUpdate.Add(Patientids);
                    }
                    catch
                    { }

                }
                if (Patient_StatusUpdate.Count > 0)
                {
                    updatedPatientid.locationId = Loc_ID;
                    updatedPatientid.organizationId = Utility.Organization_ID;
                    updatedPatientid.patientIds = Patient_StatusUpdate;
                    PushLiveDatabaseDAL.UpdatePatientReceive_SMStStatusToWeb(updatedPatientid, LocationId, Loc_ID);
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
        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate, string ClinicNumber, string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetBookOperatoryAppointmenetWiseDateTime;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ApptDate.ToString("yyyy-MM-dd"));
                MySqlCommand.Parameters.AddWithValue("Clinic_Number", ClinicNumber);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static Int64 Save_Patient_Local_To_OpenDental(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, Int64 tmpPatient_Gur_id, string Birth_Date, string Clinic_Number, string DbString)
        {
            Int64 PatientId = 0;
            MySqlConnection conn = null;
            if (Utility.dtLocationWiseUser != null)
            {
                if (Utility.dtLocationWiseUser.Rows.Count > 0)
                {
                    DataRow[] drClinicUser = Utility.dtLocationWiseUser.Copy().Select("ClinicNumber = " + Clinic_Number.ToString().Trim());
                    Utility.EHR_UserLogin_ID = drClinicUser[0]["EHR_UserLogin_ID"].ToString().Trim();
                }
            }
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = Save_NewAditUser_In_OpenDental(DbString);
            }
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                string sqlSelect = string.Empty;
                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                if (LastName.Length == 0)
                {
                    LastName += "NA";
                }
                string Firstvisitdate = "";
                try
                {
                    Firstvisitdate = Convert.ToDateTime(DateFirstVisit).ToString("yyyy-MM-dd");
                }
                catch (Exception)
                {
                    Firstvisitdate = DBNull.Value.ToString();
                }


                string patBirthDate = "";
                try
                {
                    patBirthDate = Convert.ToDateTime(Birth_Date.ToString()).ToString("yyyy-MM-dd");
                }
                catch (Exception)
                {
                    patBirthDate = "";
                }

                if (patBirthDate == "")
                {
                    if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientDetails_15_4;
                    }
                    else

                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientDetails;
                    }
                }
                else
                {
                    if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientDetails_With_BirthDate_15_4;
                    }
                    else

                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientDetails_With_BirthDate;
                    }
                }
                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("LName", LastName);
                MySqlCommand.Parameters.AddWithValue("FName", FirstName);
                MySqlCommand.Parameters.AddWithValue("Middlei", MiddleName);
                MySqlCommand.Parameters.AddWithValue("WirelessPhone", Mobile);
                MySqlCommand.Parameters.AddWithValue("Email", Email);
                MySqlCommand.Parameters.AddWithValue("PriProv", PriProv);
                MySqlCommand.Parameters.AddWithValue("DateFirstVisit", Firstvisitdate);
                MySqlCommand.Parameters.AddWithValue("SchedBeforeTime", Firstvisitdate);
                MySqlCommand.Parameters.AddWithValue("SchedAfterTime", Firstvisitdate);
                MySqlCommand.Parameters.AddWithValue("Birth_Date", patBirthDate);
                MySqlCommand.Parameters.AddWithValue("Guarantor", tmpPatient_Gur_id.ToString());
                MySqlCommand.Parameters.AddWithValue("clinicnum", Clinic_Number);
                if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                {
                    MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                }
                MySqlCommand.ExecuteNonQuery();

                string QryIdentity = "Select @@Identity as newId from patient";
                MySqlCommand.CommandText = QryIdentity;
                MySqlCommand.CommandType = CommandType.Text;
                MySqlCommand.Connection = conn;
                PatientId = Convert.ToInt64(MySqlCommand.ExecuteScalar());

                if (tmpPatient_Gur_id == 0)
                {
                    MySqlCommand.CommandText = SynchOpenDentalQRY.UpdatePatientGuarantorID;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.Parameters.AddWithValue("Guarantor", PatientId);
                    MySqlCommand.Parameters.AddWithValue("PatNum", PatientId);
                    MySqlCommand.ExecuteNonQuery();
                }

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

        public static string GetProcedureDesc(string codenum, string service_install_id, string DbString)
        {
            string procdesc = "";
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetProcDescription;
                MySqlSelect = MySqlSelect.Replace("@codenum", codenum);
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                Utility.WriteToErrorLogFromAll("desc query is=" + MySqlCommand.CommandText.ToString());
                procdesc = Convert.ToString(MySqlCommand.ExecuteScalar());
                procdesc = "#" + procdesc;
                Utility.WriteToErrorLogFromAll("procdesc from mtd   " + procdesc);
                return procdesc;

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
        public static long Save_Appointment_Local_To_OpenDental(string PatNum, string AptStatus, string Pattern, string Confirmed, string Op, string ProvNum,
                                                                 string AptDateTime, string IsNewPatient, string DateTStamp, string AppointmentTypeNum, string apptcomment, string Clinic_Number, string TreatmentCodes, string DbString)
        {
            long Appointment_Id = 0;
            Int64 TreatmentPlanId = 0;
            Int64 ProcNum = 0;

            string CodeNum, treatcode;
            Int64 treatmentplan_key;
            if (Utility.dtLocationWiseUser != null)
            {
                if (Utility.dtLocationWiseUser.Rows.Count > 0)
                {
                    DataRow[] drClinicUser = Utility.dtLocationWiseUser.Copy().Select("ClinicNumber = " + Clinic_Number.ToString().Trim());
                    Utility.EHR_UserLogin_ID = drClinicUser[0]["EHR_UserLogin_ID"].ToString().Trim();
                }
            }
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = Save_NewAditUser_In_OpenDental(DbString);
            }

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                string procedesc = "";
                #region Proceduredesc for appt 
                if (TreatmentCodes != null && TreatmentCodes.Length > 0)
                {
                    string codenumbers = "";
                    foreach (var treatmentCode in TreatmentCodes.Split(','))
                    {
                        CodeNum = treatmentCode.Substring(0, treatmentCode.IndexOf('_'));
                        codenumbers = codenumbers + "," + CodeNum;

                    }
                    codenumbers = codenumbers.TrimStart(',');
                    procedesc = GetProcedureDesc(codenumbers, Clinic_Number, DbString);
                }
                string[] codes = procedesc.Split(',');
                string ProcString = "";
                foreach (string code in codes)
                {
                    ProcString = ProcString + "<span color=\"-16777216\">" + code.Trim() + "</span>";
                }
                #endregion
                string sqlSelect = string.Empty;
                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                { MySqlCommand.CommandText = SynchOpenDentalQRY.InsertAppointmentDetails_15_4; }
                else
                { MySqlCommand.CommandText = SynchOpenDentalQRY.InsertAppointmentDetails; }

                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("PatNum", PatNum);
                MySqlCommand.Parameters.AddWithValue("AptStatus", AptStatus);
                MySqlCommand.Parameters.AddWithValue("Pattern", Pattern);
                MySqlCommand.Parameters.AddWithValue("Confirmed", Confirmed);
                MySqlCommand.Parameters.AddWithValue("Op", Op);
                MySqlCommand.Parameters.AddWithValue("ProvNum", ProvNum);
                MySqlCommand.Parameters.AddWithValue("AptDateTime", Convert.ToDateTime(AptDateTime));
                MySqlCommand.Parameters.AddWithValue("IsNewPatient", IsNewPatient);
                MySqlCommand.Parameters.AddWithValue("DateTStamp", Convert.ToDateTime(Utility.GetCurrentDatetimestring()));
                MySqlCommand.Parameters.AddWithValue("AppointmentTypeNum", AppointmentTypeNum);
                MySqlCommand.Parameters.AddWithValue("DateTimeArrived", Convert.ToDateTime(DateTStamp).ToString("yyyy-MM-dd"));
                MySqlCommand.Parameters.AddWithValue("DateTimeSeated", Convert.ToDateTime(DateTStamp).ToString("yyyy-MM-dd"));
                MySqlCommand.Parameters.AddWithValue("DateTimeDismissed", Convert.ToDateTime(DateTStamp).ToString("yyyy-MM-dd"));
                MySqlCommand.Parameters.AddWithValue("ProcDescription", procedesc);
                MySqlCommand.Parameters.AddWithValue("ProcsColored", ProcString);
                MySqlCommand.Parameters.AddWithValue("apptcomment", apptcomment);
                MySqlCommand.Parameters.AddWithValue("clinicnum", Clinic_Number);
                if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                {
                    MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                }
                MySqlCommand.ExecuteNonQuery();

                string QryIdentity = "Select @@Identity as newId from appointment";
                MySqlCommand.CommandText = QryIdentity;
                MySqlCommand.CommandType = CommandType.Text;
                MySqlCommand.Connection = conn;
                Appointment_Id = Convert.ToInt32(MySqlCommand.ExecuteScalar());

                #region Log Entries For Appt(Newly Added For ApptDescription is not Showing on appt)
                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                { MySqlCommand.CommandText = SynchOpenDentalQRY.InsertHistAppointmentDetails_15_4; }
                else
                { MySqlCommand.CommandText = SynchOpenDentalQRY.InsertHistAppointmentDetails; }

                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("PatNum", PatNum);
                MySqlCommand.Parameters.AddWithValue("AptNum", Appointment_Id);
                MySqlCommand.Parameters.AddWithValue("AptStatus", AptStatus);
                MySqlCommand.Parameters.AddWithValue("Pattern", Pattern);
                MySqlCommand.Parameters.AddWithValue("Confirmed", Confirmed);
                MySqlCommand.Parameters.AddWithValue("Op", Op);
                MySqlCommand.Parameters.AddWithValue("ProvNum", ProvNum);
                MySqlCommand.Parameters.AddWithValue("AptDateTime", Convert.ToDateTime(AptDateTime));
                MySqlCommand.Parameters.AddWithValue("IsNewPatient", IsNewPatient);
                MySqlCommand.Parameters.AddWithValue("AppointmentTypeNum", AppointmentTypeNum);
                MySqlCommand.Parameters.AddWithValue("DateTStamp", Convert.ToDateTime(Utility.GetCurrentDatetimestring()));
                MySqlCommand.Parameters.AddWithValue("ProcDescription", procedesc);
                MySqlCommand.Parameters.AddWithValue("clinicnum", Clinic_Number);
                if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                {
                    MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                }
                MySqlCommand.ExecuteNonQuery();


                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertRecallEntryForAppt;
                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("PatNum", PatNum);
                MySqlCommand.ExecuteNonQuery();

                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                { MySqlCommand.CommandText = SynchOpenDentalQRY.InsertSecuritylogEntryforAppt_15_4; }
                else
                { MySqlCommand.CommandText = SynchOpenDentalQRY.InsertSecuritylogEntryforAppt; }

                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("PatNum", PatNum);
                if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                {
                    MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                }
                MySqlCommand.ExecuteNonQuery();

                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertSecuritylogHashEntryForAppt;
                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("PatNum", PatNum);
                MySqlCommand.ExecuteNonQuery();

                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertSignalodEntryForAppt;
                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("PatNum", PatNum);
                MySqlCommand.ExecuteNonQuery();

                #endregion
                #region New Code Saving TreatmentPlans and Procdeure Mapping

                if (TreatmentCodes != null && TreatmentCodes.Length > 0)
                {
                    foreach (var treatmentCode in TreatmentCodes.Split(','))
                    {
                        CodeNum = treatmentCode.Substring(0, treatmentCode.IndexOf('_'));
                        treatmentplan_key = Convert.ToInt64(treatmentCode.Substring(treatmentCode.LastIndexOf('_') + 1));
                        ProcNum = 0;

                        if (treatmentplan_key == 0)//&& TreatmentPlanId == 0)
                        {
                            #region If TretmentPlan Key is zero then Insert Records in TreatPlan, Procedurelog & TreatPlanAttach
                            try
                            {
                                if (TreatmentPlanId == 0)
                                {
                                    if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                                    {
                                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertTreatmentPlan_15_4;
                                    }
                                    else

                                    {
                                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertTreatmentPlan;
                                    }
                                    MySqlCommand.Parameters.Clear();
                                    MySqlCommand.Parameters.AddWithValue("@PatID", PatNum);
                                    if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                                    {
                                        MySqlCommand.Parameters.AddWithValue("@EHR_User_ID", Utility.EHR_UserLogin_ID);
                                    }
                                    MySqlCommand.ExecuteNonQuery();

                                    MySqlCommand.CommandText = SynchOpenDentalQRY.GetTreatmentPlanKey;
                                    MySqlCommand.Parameters.Clear();
                                    MySqlCommand.Parameters.AddWithValue("@PatID", PatNum);

                                    TreatmentPlanId = Convert.ToInt64(MySqlCommand.ExecuteScalar());
                                }

                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToErrorLogFromAll("[TreatmentPlan Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }


                            try
                            {
                                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                                {
                                    MySqlCommand.CommandText = SynchOpenDentalQRY.InsertProcedureLog_15_4;
                                }
                                else
                                {
                                    MySqlCommand.CommandText = SynchOpenDentalQRY.InsertProcedureLog;
                                }
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("@PatNum", PatNum);
                                MySqlCommand.Parameters.AddWithValue("@AptNum", Appointment_Id);
                                MySqlCommand.Parameters.AddWithValue("@ProcDate", Convert.ToDateTime(AptDateTime));
                                MySqlCommand.Parameters.AddWithValue("@ProvNum", ProvNum);
                                MySqlCommand.Parameters.AddWithValue("@DateEntryC", Convert.ToDateTime(Utility.GetCurrentDatetimestring()));
                                MySqlCommand.Parameters.AddWithValue("@ClinicNum", Clinic_Number);
                                MySqlCommand.Parameters.AddWithValue("@CodeNum", CodeNum);
                                MySqlCommand.Parameters.AddWithValue("@DateTP", Convert.ToDateTime(DateTStamp).ToString("yyyy-MM-dd"));
                                MySqlCommand.Parameters.AddWithValue("@DateTStamp", Convert.ToDateTime(Utility.GetCurrentDatetimestring()));
                                if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                                {
                                    MySqlCommand.Parameters.AddWithValue("@SecDateEntry", Convert.ToDateTime(DateTStamp).ToString("yyyy-MM-dd"));

                                    MySqlCommand.Parameters.AddWithValue("@EHR_User_ID", Utility.EHR_UserLogin_ID);
                                }
                                MySqlCommand.ExecuteNonQuery();

                                MySqlCommand.CommandText = SynchOpenDentalQRY.GetProcNumForProcedureLog;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("@AptID", Appointment_Id);
                                MySqlCommand.Parameters.AddWithValue("@CodeNum", CodeNum);

                                ProcNum = Convert.ToInt64(MySqlCommand.ExecuteScalar());

                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToSyncLogFile_All("[Procedurelog Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }


                            try
                            {

                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertNewTreatPlanAttach;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("@TreatPlanNum", TreatmentPlanId);
                                MySqlCommand.Parameters.AddWithValue("@ProcNum", ProcNum);
                                MySqlCommand.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToErrorLogFromAll("[TreatmentPlanAttach Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }

                            #endregion
                        }

                        else if (treatmentplan_key > 0)//&& TreatmentPlanId == 0) //30_78
                        {
                            #region IF Tretment Plan key is > 0 then Insert records in Procedurelog

                            try
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.updateProcedureLog;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("@AptNum", Appointment_Id);
                                MySqlCommand.Parameters.AddWithValue("@ProcNum", treatmentplan_key);
                                //MySqlCommand.Parameters.AddWithValue("@ProvNum", ProvNum);
                                //MySqlCommand.Parameters.AddWithValue("@DateEntryC", Convert.ToDateTime(Utility.GetCurrentDatetimestring()));
                                //MySqlCommand.Parameters.AddWithValue("@ClinicNum", Clinic_Number);
                                //MySqlCommand.Parameters.AddWithValue("@CodeNum", CodeNum);
                                //MySqlCommand.Parameters.AddWithValue("@DateTP", Convert.ToDateTime(DateTStamp).ToString("yyyy-MM-dd"));
                                //MySqlCommand.Parameters.AddWithValue("@DateTStamp", Convert.ToDateTime(Utility.GetCurrentDatetimestring()));
                                //MySqlCommand.Parameters.AddWithValue("@SecDateEntry", Convert.ToDateTime(DateTStamp).ToString("yyyy-MM-dd"));
                                MySqlCommand.ExecuteNonQuery();

                                MySqlCommand.CommandText = SynchOpenDentalQRY.GetProcNumForProcedureLog;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("@AptID", Appointment_Id);
                                MySqlCommand.Parameters.AddWithValue("@CodeNum", CodeNum);

                                ProcNum = Convert.ToInt64(MySqlCommand.ExecuteScalar());

                                MySqlCommand.CommandText = SynchOpenDentalQRY.GetTreatPlanNumByProcNum;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("@ProcNum", ProcNum);

                                TreatmentPlanId = Convert.ToInt64(MySqlCommand.ExecuteScalar());

                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToErrorLogFromAll("[ Procedurelog Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }


                            try
                            {

                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertNewTreatPlanAttach;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("@TreatPlanNum", TreatmentPlanId);
                                MySqlCommand.Parameters.AddWithValue("@ProcNum", ProcNum);
                                MySqlCommand.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToErrorLogFromAll("[TreatmentPlanAttach Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }

                            #endregion
                        }
                    }

                    #endregion

                }
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                Appointment_Id = 0;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return Appointment_Id;
        }

        #region SqlCeDatabase

        public static bool Save_Appointment_OpenDental_To_Local(DataTable dtOpenDentalAppointment, string Service_Install_Id)
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
                        bool is_ehr_updated = false;
                        string AppointmentStatus = string.Empty;
                        bool is_asap = false;
                        foreach (DataRow dr in dtOpenDentalAppointment.Rows)
                        {
                            try
                            {

                                is_ehr_updated = false;
                                is_asap = false;
                                if (dr["InsUptDlt"].ToString() == "")
                                {
                                    dr["InsUptDlt"] = "0";
                                }
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                                {
                                    //#region Check Appointment Change Clinic Or Not

                                    //DataTable Dt = SynchLocalDAL.GetLocalAppointmentDataWithEhrId(dr["Appt_EHR_ID"].ToString().Trim());

                                    //if (Dt != null && Dt.Rows.Count > 0)
                                    //{
                                    //    DataRow[] Dr = Dt.Copy().Select("Clinic_Number <> '" + dr["Clinic_Number"].ToString().Trim() + "' ");

                                    //    if (Dr.Length > 0)
                                    //    {
                                    //        foreach (DataRow d in Dr)
                                    //        {
                                    //            if (d["is_deleted"].ToString() == "False")
                                    //            {
                                    //                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment_With_Clinic_Number;
                                    //                SqlCeCommand.Parameters.Clear();
                                    //                SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                                    //                SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", Utility.GetCurrentDatetimestring());
                                    //                SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", Utility.GetCurrentDatetimestring());
                                    //                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    //                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", d["Clinic_Number"].ToString().Trim());
                                    //                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    //                SqlCeCommand.ExecuteNonQuery();
                                    //            }
                                    //        }

                                    //        if (Dt.Copy().Select("Clinic_Number = '" + dr["Clinic_Number"].ToString().Trim() + "' ").Length > 0)
                                    //        {
                                    //            dr["InsUptDlt"] = 7;
                                    //            dr.AcceptChanges();
                                    //        }
                                    //        else
                                    //        {
                                    //            dr["InsUptDlt"] = 1;
                                    //            dr.AcceptChanges();
                                    //        }
                                    //    }
                                    //}

                                    //#endregion

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
                                            SqlCeCommand.CommandText = SynchLocalQRY.Update_AppointmentMultiLocation_Where_Appt_EHR_ID;
                                            is_ehr_updated = true;
                                            break;
                                        case 7:
                                            SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID_Clinic_Number;
                                            is_ehr_updated = true;
                                            break;
                                        case 3:
                                            SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                            break;
                                        case 6:
                                            SqlCeCommand.CommandText = SynchLocalQRY.InsertAppointment_With_DeleteFlag;
                                            break;
                                    }



                                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3 || Convert.ToInt32(dr["InsUptDlt"].ToString()) == 6)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", Utility.GetCurrentDatetimestring());
                                        SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", Utility.GetCurrentDatetimestring());
                                        SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        string ApptType = string.Empty;
                                        if (dr["ApptType_EHR_ID"].ToString() == "0")
                                        {
                                            ApptType = "none";
                                        }
                                        else
                                        {
                                            ApptType = dr["ApptType"].ToString().Trim();
                                        }

                                        if (dr["Appointment_Status"].ToString().Trim() == "1")
                                        {
                                            AppointmentStatus = "Scheduled";
                                        }
                                        else if (dr["Appointment_Status"].ToString().Trim() == "2")
                                        {
                                            AppointmentStatus = "Completed";
                                        }
                                        else if (dr["Appointment_Status"].ToString().Trim() == "3")
                                        {
                                            AppointmentStatus = "UnschedList";
                                        }
                                        else if (dr["Appointment_Status"].ToString().Trim() == "4")
                                        {
                                            if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                                            {
                                                AppointmentStatus = "ASAP";
                                                is_asap = true;
                                            }
                                            else
                                            {
                                                if (Convert.ToBoolean(dr["is_asap"].ToString()) == true)
                                                {
                                                    is_asap = true;
                                                }
                                            }
                                        }
                                        else if (dr["Appointment_Status"].ToString().Trim() == "5")
                                        {
                                            AppointmentStatus = "Broken";
                                        }
                                        else if (dr["Appointment_Status"].ToString().Trim() == "7")
                                        {
                                            AppointmentStatus = "Patient Note";
                                        }
                                        else if (dr["Appointment_Status"].ToString().Trim() == "8")
                                        {
                                            AppointmentStatus = "Cmp. Patient Note";
                                        }
                                        else
                                        {
                                            AppointmentStatus = "Scheduled";
                                        }
                                        DateTime ApptEndDateTime = Convert.ToDateTime(dr["Appt_DateTime"].ToString()).AddMinutes(Convert.ToInt32(dr["ApptMin"].ToString().Trim()));

                                        int commentlen = 1999;
                                        if (dr["comment"].ToString().Trim().Length < commentlen)
                                        {
                                            commentlen = dr["comment"].ToString().Trim().Length;
                                        }

                                        if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                                        {
                                            if (dr["is_asap"] != null && dr["is_asap"].ToString() != string.Empty && Convert.ToBoolean(dr["is_asap"]) == true)
                                            {
                                                is_asap = true;
                                            }
                                        }

                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["PatNum"].ToString().Trim());
                                        if (dtOpenDentalAppointment.Columns.Contains("Appt_Web_ID"))
                                        {
                                            string strApptWebID = "";
                                            try
                                            {
                                                strApptWebID = dr["Appt_Web_ID"].ToString().Trim();
                                            }
                                            catch
                                            { 
                                            }
                                            SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", strApptWebID);
                                        }
                                        else
                                        {
                                            SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", "");
                                        }
                                        SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("MI", dr["MI"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Home_Contact", Utility.ConvertContactNumber(dr["Home_Contact"].ToString().Trim()));
                                        SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", Utility.ConvertContactNumber(dr["Mobile_Contact"].ToString().Trim()));
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
                                        SqlCeCommand.Parameters.AddWithValue("ApptType", ApptType);
                                        SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                                        SqlCeCommand.Parameters.AddWithValue("birth_date", Utility.CheckValidDatetime(dr["birth_date"].ToString()));
                                        SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", dr["Appt_DateTime"].ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", ApptEndDateTime.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Status", "1");
                                        SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Appointment_Status", AppointmentStatus);
                                        SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", dr["confirmed_status_ehr_key"].ToString());
                                        SqlCeCommand.Parameters.AddWithValue("confirmed_status", dr["confirmed_status"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", dr["unschedule_status_ehr_key"].ToString());
                                        SqlCeCommand.Parameters.AddWithValue("unschedule_status", dr["Unsched_Status"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("Patient_Status", "new");
                                        SqlCeCommand.Parameters.AddWithValue("Is_Appt", "EHR");
                                        SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", is_ehr_updated);
                                        SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString()));
                                        SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
                                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                        SqlCeCommand.Parameters.AddWithValue("Appt_LocalDB_ID", dr["Appt_LocalDB_ID"].ToString().Trim());
                                        SqlCeCommand.Parameters.AddWithValue("is_asap", is_asap);
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.Parameters.AddWithValue("appt_treatmentcode", "");
                                        SqlCeCommand.Parameters.AddWithValue("ProcedureDesc", dr["ProcedureDesc"].ToString());
                                        SqlCeCommand.Parameters.AddWithValue("ProcedureCode", dr["ProcedureCode"].ToString());

                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception)
                            {
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

        public static bool Save_OperatoryEvent_OpenDental_To_Local(DataTable dtOpenDentalOperatoryEvent, string Service_Install_Id)
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
                        string AppointmentStatus = string.Empty;
                        foreach (DataRow dr in dtOpenDentalOperatoryEvent.Rows)
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
                                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["ScheduleNum"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {

                                    DateTime StartTime = Convert.ToDateTime((Convert.ToDateTime(dr["SchedDate"]).ToString("MM/dd/yyyy")).ToString() + " " + dr["StartTime"].ToString().Trim());
                                    DateTime EndTime = Convert.ToDateTime((Convert.ToDateTime(dr["SchedDate"]).ToString("MM/dd/yyyy")).ToString() + " " + dr["StopTime"].ToString().Trim());

                                    int commentlen = 1999;
                                    if (dr["Note"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dr["Note"].ToString().Trim().Length;
                                    }

                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["ScheduleNum"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["OperatoryNum"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["Note"].ToString().Trim().Substring(0, commentlen));
                                    SqlCeCommand.Parameters.AddWithValue("StartTime", StartTime);
                                    SqlCeCommand.Parameters.AddWithValue("EndTime", EndTime);
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.CheckValidDatetime(dr["DateTStamp"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.Parameters.AddWithValue("Allow_Book_Appt", Convert.ToBoolean(dr["Allow_Book_Appt"].ToString().Trim()));
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

        public static bool Save_Holiday_OpenDental_To_Local(DataTable dtOpenDentalHoliday, string Service_Install_Id)
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
                        foreach (DataRow dr in dtOpenDentalHoliday.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_HolidayData;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_HolidayData;
                                        break;
                                }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("H_EHR_ID", dr["ScheduleNum"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {

                                    int commentlen = 1999;
                                    if (dr["Note"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dr["Note"].ToString().Trim().Length;
                                    }

                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("H_EHR_ID", dr["ScheduleNum"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("H_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["Note"].ToString().Trim().Substring(0, commentlen));
                                    SqlCeCommand.Parameters.AddWithValue("SchedDate", Utility.CheckValidDatetime(dr["SchedDate"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_1", "");
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_1", "");
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_2", "");
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_2", "");
                                    SqlCeCommand.Parameters.AddWithValue("StartTime_3", "");
                                    SqlCeCommand.Parameters.AddWithValue("EndTime_3", "");
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.CheckValidDatetime(dr["DateTStamp"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("clinic_Number", dr["clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
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

        public static bool Save_Provider_OpenDental_To_Local(DataTable dtOpenDentalProvider, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //         SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtOpenDentalProvider.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("MI", dr["mi"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("gender", "");
                                SqlCeCommand.Parameters.AddWithValue("provider_speciality", dr["provider_speciality"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("is_active", Convert.ToInt16(dr["is_active"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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

        public static bool Save_ProviderHours_OpenDental_To_Local(DataTable dtOpenDentalProviderHours, string Service_Install_Id)
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
                        foreach (DataRow dr in dtOpenDentalProviderHours.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                //if (dr["Operatory_EHR_ID"].ToString().Trim() != "" || Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                //{ 

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
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();


                                //if (dr["Operatory_EHR_ID"].ToString().Trim() != "" || Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)

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
                                SqlCeCommand.Parameters.AddWithValue("OH_EHR_ID", dr["PH_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("OH_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("StartTime", dr["StartTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("EndTime", dr["EndTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dr["Entry_DateTime"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3 && Utility.Application_Version.ToLower() == "17.2+")
                                {

                                    DateTime startDate = DateTime.ParseExact(Convert.ToDateTime(dr["StartTime"].ToString()).ToString("yyyyMMdd") + " 00:00:01", "yyyyMMdd HH:mm:ss", null);
                                    DateTime endDate = DateTime.ParseExact(Convert.ToDateTime(dr["StartTime"].ToString()).ToString("yyyyMMdd") + " 23:59:59", "yyyyMMdd HH:mm:ss", null);

                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_PH_SameDate_Is_Adit_Updated_flg;
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("StartTime", startDate.ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("EndTime", endDate.ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();

                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_OH_SameDate_Is_Adit_Updated_flg;
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("StartTime", startDate.ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("EndTime", endDate.ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
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

        public static bool Save_Speciality_OpenDental_To_Local(DataTable dtOpenDentalSpeciality, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //        SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtOpenDentalSpeciality.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"]) == 1)
                            {
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Speciality;
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Speciality_Name", dr["provider_speciality"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
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

        public static bool Save_Operatory_OpenDental_To_Local(DataTable dtOpenDentalOperatory, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //        SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtOpenDentalOperatory.Rows)
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
                                    case 4:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_False_Operatory;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("OperatoryOrder", dr["OperatoryOrder"].ToString());
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

        public static bool Save_FolderList_OpenDental_To_Local(DataTable dtOpenDentalOperatory, string Service_Install_Id, string clinicNumber)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //        SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtOpenDentalOperatory.Rows)
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
                                    case 4:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_False_FolderList;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("FolderList_EHR_ID", dr["FolderList_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Folder_Name", dr["Folder_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", clinicNumber);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.Parameters.AddWithValue("FolderOrder", dr["FolderOrder"].ToString());
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


        public static bool Save_ApptType_OpenDental_To_Local(DataTable dtOpenDentalApptType, string Service_Install_Id)
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
                        foreach (DataRow dr in dtOpenDentalApptType.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
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
            }
            return _successfullstataus;
        }

        public static bool Save_PatientAppointment_OpenDental_To_Local(DataTable dtOpenDentalPatient, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            try
            {


                SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtOpenDentalPatient, "0", Service_Install_Id);
            }
            catch (Exception)
            {

            }
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        foreach (DataRow dtDtxRow in dtOpenDentalPatient.Rows)
                        {
                            try
                            {
                                switch (Convert.ToInt32(dtDtxRow["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptPatient;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptPatient;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                                        break;
                                }

                                #region SqlCEConnection

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dtDtxRow["patient_ehr_id"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("First_name", dtDtxRow["First_name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_name", dtDtxRow["Last_name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Middle_Name", dtDtxRow["Middle_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Status", dtDtxRow["Status"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Email", dtDtxRow["Email"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dtDtxRow["Mobile"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dtDtxRow["Home_Phone"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dtDtxRow["LastVisit_Date"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", dtDtxRow["ReceiveSMS"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", "Y");
                                SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dtDtxRow["nextvisit_date"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("due_date", Convert.ToString(dtDtxRow["due_date"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dtDtxRow["Clinic_Number"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.Parameters.AddWithValue("EHR_Status", dtDtxRow["EHR_Status"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.CheckValidDatetime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("ReceiveVoiceCall", "Y");
                                // SqlCeCommand.Parameters.AddWithValue("PreferredLanguage", dtDtxRow["PreferredLanguage"].ToString().Trim());
                                if (SqlCeCommand.Connection.State != ConnectionState.Open)
                                    SqlCeCommand.Connection.Open();
                                SqlCeCommand.ExecuteNonQuery();
                                //  }
                                #endregion
                            }
                            catch (Exception ex1)
                            {
                                Utility.WriteToErrorLogFromAll("Err_Appointment Patient Sync. Patient EHR Id : " + dtDtxRow["patient_ehr_id"].ToString() + "_" + ex1.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
                return _successfullstataus;
            }
        }

        public static bool Save_Patient_OpenDental_To_Local(DataTable dtOpenDentalPatient, string Service_Install_Id)
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
                        string due_date = string.Empty;

                        foreach (DataRow dr in dtOpenDentalPatient.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {

                                if (dr["Birth_Date"].ToString() != "")
                                {
                                    dr["Birth_Date"] = Convert.ToDateTime(dr["Birth_Date"].ToString()).ToString("MM/dd/yyyy");
                                }

                                try
                                {
                                    due_date = dr["due_date"].ToString().Trim();
                                }
                                catch (Exception)
                                {
                                    due_date = "";
                                }

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
                                SqlCeCommand.Parameters.AddWithValue("Status", "A");
                                SqlCeCommand.Parameters.AddWithValue("Sex", dr["tmpSex"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("MaritalStatus", dr["tmpMaritalStatus"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Birth_Date", dr["tmpBirth_Date"].ToString().Trim());
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
                                SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["tmpFirstVisit_Date"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["tmpLastVisit_Date"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", "");
                                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", "");
                                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", "");
                                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", "");
                                SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", dr["tmpReceiveSMS"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", "Y");
                                SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["tmpnextvisit_date"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("due_date", due_date);
                                SqlCeCommand.Parameters.AddWithValue("remaining_benefit", "");
                                SqlCeCommand.Parameters.AddWithValue("collect_payment", "");
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
                                SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
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

        public static bool Save_RecallType_OpenDental_To_Local(DataTable dtOpenDentalRecallType, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtOpenDentalRecallType.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("RecallType_Descript", "");
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
        public static bool Save_PatientDisease_OpenDental_To_Local(DataTable dtOpenDentalPatientDisease, string Service_Install_Id)
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
                        foreach (DataRow dr in dtOpenDentalPatientDisease.Rows)
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
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);

                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Name", dr["Disease_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                    //SqlCeCommand.Parameters.AddWithValue("is_deleted", dr["is_deleted"].ToString().Trim() == "" || string.IsNullOrEmpty(dr["is_deleted"].ToString()) ? 0 : Convert.ToInt16(dr["is_deleted"].ToString().Trim()));
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", (dr["is_deleted"].ToString().Trim().ToUpper() == "TRUE" ? 1 : 0));
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
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


        public static bool Save_Disease_OpenDental_To_Local(DataTable dtOpenDentalDisease, string Service_Install_Id)
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
                        foreach (DataRow dr in dtOpenDentalDisease.Rows)
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
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);

                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Name", dr["Disease_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", (dr["is_deleted"].ToString().Trim().ToUpper() == "TRUE" ? 1 : 0));
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
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

        public static bool Save_ApptStatus_OpenDental_To_Local(DataTable dtOpenDentalApptStatus, string Service_Install_Id)
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
                        foreach (DataRow dr in dtOpenDentalApptStatus.Rows)
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

        public static bool Save_Insurance_OpenDental_To_Local(DataTable dtOpenDentalInsurance, string Service_Install_Id)
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
                        foreach (DataRow dr in dtOpenDentalInsurance.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("Insurance_EHR_ID", dr["CarrierNum"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Name", dr["CarrierName"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address", dr["Address"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["Zip"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Phone", dr["Phone"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ElectId", dr["ElectID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("EmployerName", "");
                                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr["IsHidden"].ToString().Trim() == "0" ? false : true);
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

        #region SqlServerDatabase

        public static bool Save_Appointment_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalAppointment, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //  SqlTransaction sqlEx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //   sqlEx = conn.BeginTransaction();
            try
            {
                //if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                //  SqlCommand.Transaction = sqlEx;

                bool is_ehr_updated = false;
                string AppointmentStatus = string.Empty;
                foreach (DataRow dr in dtOpenDentalAppointment.Rows)
                {
                    is_ehr_updated = false;
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {

                        //#region Check Appointment Change Clinic Or Not

                        //DataTable Dt = SynchLocalDAL.GetLocalAppointmentDataWithEhrId(dr["Appt_EHR_ID"].ToString().Trim());

                        //if (Dt != null && Dt.Rows.Count > 0)
                        //{
                        //    DataRow[] Dr = Dt.Copy().Select("Clinic_Number <> '" + dr["Clinic_Number"].ToString().Trim() + "' ");

                        //    if (Dr.Length > 0)
                        //    {
                        //        foreach (DataRow d in Dr)
                        //        {
                        //            if (d["is_deleted"].ToString() == "False")
                        //            {
                        //                SqlCommand.CommandText = SynchLocalQRY.Delete_Appointment_With_Clinic_Number;
                        //                SqlCommand.Parameters.Clear();
                        //                SqlCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                        //                SqlCommand.Parameters.AddWithValue("Appt_DateTime", Utility.GetCurrentDatetimestring());
                        //                SqlCommand.Parameters.AddWithValue("Appt_EndDateTime", Utility.GetCurrentDatetimestring());
                        //                SqlCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                        //                SqlCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        //                SqlCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                        //                SqlCommand.Parameters.AddWithValue("Clinic_Number", d["Clinic_Number"].ToString().Trim());
                        //                SqlCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        //                SqlCommand.ExecuteNonQuery();
                        //            }
                        //        }

                        //        if (Dt.Copy().Select("Clinic_Number = '" + dr["Clinic_Number"].ToString().Trim() + "' ").Length > 0)
                        //        {
                        //            dr["InsUptDlt"] = 7;
                        //            dr.AcceptChanges();
                        //        }
                        //        else
                        //        {
                        //            dr["InsUptDlt"] = 1;
                        //            dr.AcceptChanges();
                        //        }
                        //    }
                        //}

                        //#endregion

                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCommand.CommandText = SynchLocalQRY.Insert_Appointment;
                                is_ehr_updated = true;
                                break;
                            case 5:
                                SqlCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Contact;
                                is_ehr_updated = true;
                                break;
                            case 4:
                                SqlCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID;
                                is_ehr_updated = true;
                                break;
                            case 7:
                                SqlCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID_Clinic_Number;
                                is_ehr_updated = true;
                                break;
                            case 3:
                                SqlCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                break;
                        }

                        if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                        {
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            string ApptType = string.Empty;
                            if (dr["ApptType_EHR_ID"].ToString() == "0")
                            {
                                ApptType = "none";
                            }
                            else
                            {
                                ApptType = dr["ApptType"].ToString().Trim();
                            }

                            if (dr["Appointment_Status"].ToString().Trim() == "1")
                            {
                                AppointmentStatus = "Scheduled";
                            }
                            else if (dr["Appointment_Status"].ToString().Trim() == "2")
                            {
                                AppointmentStatus = "Completed";
                            }
                            else if (dr["Appointment_Status"].ToString().Trim() == "3")
                            {
                                AppointmentStatus = "UnschedList";
                            }
                            else if (dr["Appointment_Status"].ToString().Trim() == "4")
                            {
                                AppointmentStatus = "ASAP";
                            }
                            else if (dr["Appointment_Status"].ToString().Trim() == "5")
                            {
                                AppointmentStatus = "Broken";
                            }
                            else if (dr["Appointment_Status"].ToString().Trim() == "7")
                            {
                                AppointmentStatus = "Patient Note";
                            }
                            else if (dr["Appointment_Status"].ToString().Trim() == "8")
                            {
                                AppointmentStatus = "Cmp. Patient Note";
                            }
                            else
                            {
                                AppointmentStatus = "Scheduled";
                            }
                            DateTime ApptEndDateTime = Convert.ToDateTime(dr["Appt_DateTime"].ToString()).AddMinutes(Convert.ToInt32(dr["ApptMin"].ToString().Trim()));

                            int commentlen = 1999;
                            if (dr["comment"].ToString().Trim().Length < commentlen)
                            {
                                commentlen = dr["comment"].ToString().Trim().Length;
                            }

                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("patient_ehr_id", dr["PatNum"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Appt_Web_ID", "");
                            SqlCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("MI", dr["MI"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Home_Contact", Utility.ConvertContactNumber(dr["Home_Contact"].ToString().Trim()));
                            SqlCommand.Parameters.AddWithValue("Mobile_Contact", Utility.ConvertContactNumber(dr["Mobile_Contact"].ToString().Trim()));
                            SqlCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Address", dr["Address"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("ST", dr["ST"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Zip", dr["Zip"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Provider_Name", dr["Provider_Name"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("ApptType", ApptType);
                            SqlCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                            SqlCommand.Parameters.AddWithValue("birth_date", Utility.CheckValidDatetime(dr["birth_date"].ToString()));
                            SqlCommand.Parameters.AddWithValue("Appt_DateTime", dr["Appt_DateTime"].ToString());
                            SqlCommand.Parameters.AddWithValue("Appt_EndDateTime", ApptEndDateTime.ToString());
                            SqlCommand.Parameters.AddWithValue("Status", "1");
                            SqlCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString());
                            SqlCommand.Parameters.AddWithValue("Appointment_Status", AppointmentStatus);
                            SqlCommand.Parameters.AddWithValue("confirmed_status_ehr_key", dr["confirmed_status_ehr_key"].ToString());
                            SqlCommand.Parameters.AddWithValue("confirmed_status", dr["confirmed_status"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("unschedule_status_ehr_key", dr["unschedule_status_ehr_key"].ToString());
                            SqlCommand.Parameters.AddWithValue("unschedule_status", dr["Unsched_Status"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Patient_Status", "new");
                            SqlCommand.Parameters.AddWithValue("Is_Appt", "EHR");
                            SqlCommand.Parameters.AddWithValue("is_ehr_updated", is_ehr_updated);
                            SqlCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                            SqlCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                            SqlCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString()));
                            SqlCommand.Parameters.AddWithValue("is_deleted", 0);
                            SqlCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlCommand.Parameters.AddWithValue("Appt_LocalDB_ID", dr["Appt_LocalDB_ID"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                            SqlCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlCommand.Parameters.AddWithValue("appt_treatmentcode", "");
                            SqlCommand.Parameters.AddWithValue("ProcedureDesc", dr["ProcedureDesc"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("ProcedureCode", dr["ProcedureCode"].ToString().Trim());
                            SqlCommand.ExecuteNonQuery();
                        }
                    }
                }

                // sqlEx.Commit();
            }
            catch (Exception)
            {
                _successfullstataus = false;
                // sqlEx.Rollback();
                //  throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_OperatoryEvent_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalOperatoryEvent, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlExCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //    SqlTransaction SqlExTransaction;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //    SqlExTransaction = conn.BeginTransaction();
            try
            {
                //if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                // SqlExCommand.Transaction = SqlExTransaction;

                string AppointmentStatus = string.Empty;
                foreach (DataRow dr in dtOpenDentalOperatoryEvent.Rows)
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
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_OperatoryEventData;

                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_OperatoryEventData;

                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_OperatoryEventData;
                                break;
                        }

                        if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                        {
                            SqlExCommand.Parameters.Clear();
                            SqlExCommand.Parameters.AddWithValue("OE_EHR_ID", dr["ScheduleNum"].ToString().Trim());
                            SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlExCommand.ExecuteNonQuery();
                        }
                        else
                        {

                            DateTime StartTime = Convert.ToDateTime((Convert.ToDateTime(dr["SchedDate"]).ToString("MM/dd/yyyy")).ToString() + " " + dr["StartTime"].ToString().Trim());
                            DateTime EndTime = Convert.ToDateTime((Convert.ToDateTime(dr["SchedDate"]).ToString("MM/dd/yyyy")).ToString() + " " + dr["StopTime"].ToString().Trim());

                            int commentlen = 1999;
                            if (dr["Note"].ToString().Trim().Length < commentlen)
                            {
                                commentlen = dr["Note"].ToString().Trim().Length;
                            }

                            SqlExCommand.Parameters.Clear();
                            SqlExCommand.Parameters.AddWithValue("OE_EHR_ID", dr["ScheduleNum"].ToString().Trim());
                            SqlExCommand.Parameters.AddWithValue("OE_Web_ID", "");
                            SqlExCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["OperatoryNum"].ToString().Trim());
                            SqlExCommand.Parameters.AddWithValue("comment", dr["Note"].ToString().Trim().Substring(0, commentlen));
                            SqlExCommand.Parameters.AddWithValue("StartTime", StartTime);
                            SqlExCommand.Parameters.AddWithValue("EndTime", EndTime);
                            SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                            //SqlExCommand.Parameters.AddWithValue("Entry_DateTime", Utility.CheckValidDatetime(dr["DateTStamp"].ToString()));
                            SqlExCommand.Parameters.AddWithValue("Entry_DateTime", Convert.ToDateTime(dr["DateTStamp"]).ToString("yyyy-MM-dd"));

                            SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["clinic_Number"].ToString().Trim());
                            SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlExCommand.Parameters.AddWithValue("Allow_Book_Appt", Convert.ToBoolean(dr["Allow_Book_Appt"].ToString().Trim()));
                            SqlExCommand.ExecuteNonQuery();
                        }
                    }
                }

                //SqlExTransaction.Commit();
            }
            catch (Exception)
            {
                _successfullstataus = false;
                //SqlExTransaction.Rollback();
                //  throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_Provider_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalProvider, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlExCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            if (conn.State == ConnectionState.Closed) conn.Open();

            //     SqlTransaction transaction = conn.BeginTransaction("");


            try
            {
                //if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                //   SqlExCommand.Transaction = transaction;
                foreach (DataRow dr in dtOpenDentalProvider.Rows)
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
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_Provider;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_Provider;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_Provider;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Provider_Web_ID", "");
                        SqlExCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("MI", dr["mi"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("gender", "");
                        SqlExCommand.Parameters.AddWithValue("provider_speciality", dr["provider_speciality"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);

                        SqlExCommand.ExecuteNonQuery();
                    }
                }
                //transaction.Commit();

            }
            catch (Exception)
            {
                //  transaction.Rollback();
                _successfullstataus = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_Speciality_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalSpeciality, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlExCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //   SqlTransaction SqlExTransaction;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //    SqlExTransaction = conn.BeginTransaction();
            try
            {
                //if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");

                //SqlExCommand.Transaction = SqlExTransaction;

                foreach (DataRow dr in dtOpenDentalSpeciality.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"]) == 1)
                    {
                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.CommandText = SynchLocalQRY.Insert_Speciality;
                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.Parameters.AddWithValue("Speciality_Name", dr["provider_speciality"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlExCommand.ExecuteNonQuery();
                    }
                }
                //SqlExTransaction.Commit();
            }
            catch (Exception)
            {
                _successfullstataus = false;
                //SqlExTransaction.Rollback();
                //throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_Operatory_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalOperatory, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlExCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //     SqlTransaction SqlExTransaction;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //     SqlExTransaction = conn.BeginTransaction();
            try
            {
                // if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                //SqlExCommand.Transaction = SqlExTransaction;

                foreach (DataRow dr in dtOpenDentalOperatory.Rows)
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
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_Operatory;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_Operatory;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_Operatory;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Operatory_Web_ID", "");
                        SqlExCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlExCommand.ExecuteNonQuery();
                    }
                }

                //SqlExTransaction.Commit();
            }
            catch (Exception)
            {
                _successfullstataus = false;
                //SqlExTransaction.Rollback();
                //throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_FolderList_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalFolderList, string Service_Install_Id, string clinicNumber)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlExCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //     SqlTransaction SqlExTransaction;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //     SqlExTransaction = conn.BeginTransaction();
            try
            {
                // if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                //SqlExCommand.Transaction = SqlExTransaction;

                foreach (DataRow dr in dtOpenDentalFolderList.Rows)
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
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_FolderList;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_FolderList;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_FolderList;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.Parameters.AddWithValue("FolderList_EHR_ID", dr["FolderList_EHR_ID"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("FolderList_Web_ID", "");
                        SqlExCommand.Parameters.AddWithValue("Folder_Name", dr["Folder_Name"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("FolderList_Web_ID", 0);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", clinicNumber);
                        SqlExCommand.Parameters.AddWithValue("FolderList_Web_ID", 0);
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlExCommand.Parameters.AddWithValue("FolderOrder", dr["FolderOrder"].ToString());
                        SqlExCommand.ExecuteNonQuery();
                    }
                }

                //SqlExTransaction.Commit();
            }
            catch (Exception)
            {
                _successfullstataus = false;
                //SqlExTransaction.Rollback();
                //throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }


        public static bool Save_ApptType_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalApptType, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlExCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //     SqlTransaction SqlExTransaction;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //    SqlExTransaction = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                //   SqlExCommand.Transaction = SqlExTransaction;

                foreach (DataRow dr in dtOpenDentalApptType.Rows)
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
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_ApptType;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_ApptType;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_ApptType;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("ApptType_Web_ID", "");
                        SqlExCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlExCommand.ExecuteNonQuery();
                    }
                }

                //SqlExTransaction.Commit();
            }
            catch (Exception)
            {
                _successfullstataus = false;
                // SqlExTransaction.Rollback();
                //throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_Patient_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalPatient, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlExCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //   SqlTransaction SqlExTransaction;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //  SqlExTransaction = conn.BeginTransaction();

            try
            {
                // if (conn.State == ConnectionState.Closed) conn.Open();

                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                //     SqlExCommand.Transaction = SqlExTransaction;

                string due_date = string.Empty;

                foreach (DataRow dr in dtOpenDentalPatient.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {

                        if (dr["Birth_Date"].ToString() != "")
                        {
                            dr["Birth_Date"] = Convert.ToDateTime(dr["Birth_Date"].ToString()).ToString("MM/dd/yyyy");
                        }

                        try
                        {
                            due_date = dr["due_date"].ToString().Trim();
                        }
                        catch (Exception)
                        {
                            due_date = "";
                        }

                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_Patient;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_Patient;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_Patient;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("patient_Web_ID", "");
                        SqlExCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Status", "A");
                        SqlExCommand.Parameters.AddWithValue("Sex", dr["tmpSex"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("MaritalStatus", dr["tmpMaritalStatus"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Birth_Date", dr["tmpBirth_Date"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                        SqlExCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                        SqlExCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                        SqlExCommand.Parameters.AddWithValue("Address1", dr["Address1"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Zipcode", dr["Zipcode"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("ResponsibleParty_Status", "");
                        SqlExCommand.Parameters.AddWithValue("CurrentBal", dr["CurrentBal"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("NinetyDay", dr["NinetyDay"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["tmpFirstVisit_Date"].ToString().Trim()));
                        SqlExCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["tmpLastVisit_Date"].ToString().Trim()));
                        SqlExCommand.Parameters.AddWithValue("Primary_Insurance", "");
                        SqlExCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", "");
                        SqlExCommand.Parameters.AddWithValue("Secondary_Insurance", "");
                        SqlExCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", "");
                        SqlExCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("ReceiveSMS", dr["tmpReceiveSMS"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("ReceiveEmail", "Y");
                        SqlExCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["tmpnextvisit_date"].ToString().Trim()));
                        SqlExCommand.Parameters.AddWithValue("due_date", due_date);
                        SqlExCommand.Parameters.AddWithValue("remaining_benefit", "");
                        SqlExCommand.Parameters.AddWithValue("collect_payment", "");
                        SqlExCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString().Trim()));
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlExCommand.Parameters.AddWithValue("Is_Deleted", dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
                        SqlExCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", "");
                        SqlExCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", "");
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlExCommand.ExecuteNonQuery();

                    }
                }

                //SqlExTransaction.Commit();

            }
            catch (Exception)
            {
                _successfullstataus = false;

                //SqlExTransaction.Rollback();
                // throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_RecallType_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalRecallType, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlExCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //    SqlTransaction SqlExTransaction;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //     SqlExTransaction = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                //    SqlExCommand.Transaction = SqlExTransaction;

                foreach (DataRow dr in dtOpenDentalRecallType.Rows)
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
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_RecallType;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_RecallType;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_RecallType;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.Parameters.AddWithValue("RecallType_EHR_ID", dr["RecallType_EHR_ID"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("RecallType_Web_ID", "");
                        SqlExCommand.Parameters.AddWithValue("RecallType_Name", dr["RecallType_Name"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("RecallType_Descript", "");
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"]);
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlExCommand.ExecuteNonQuery();
                    }
                }

                //SqlExTransaction.Commit();
            }
            catch (Exception)
            {
                _successfullstataus = false;
                //SqlExTransaction.Rollback();
                //throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }



        #region User

        public static string Save_NewAditUser_In_OpenDental(string DbString)
        {
            string loginId = string.Empty;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            try
            {


                CommonDB.MySQLConnectionServer(ref conn, DbString);
                string sqlSelect = string.Empty;
                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
                CheckConnection(conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                {
                    try
                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertUserLogin_ID_15_4;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("username", "Adit");
                        MySqlCommand.Parameters.AddWithValue("clinicnum", "0");
                        MySqlCommand.Parameters.AddWithValue("lastlogindatetime", Convert.ToDateTime(System.DateTime.Now));
                        MySqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteToErrorLogFromAll("error in save adit user" + ex.Message.ToString());
                    }

                    string UserIdentity = "Select usernum from userod where username = 'Adit'";
                    MySqlCommand.CommandText = UserIdentity;
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    loginId = Convert.ToString(MySqlCommand.ExecuteScalar());

                }
                else
                {
                    try
                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertUserLogin_ID;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("username", "Adit");
                        MySqlCommand.Parameters.AddWithValue("clinicnum", "0");
                        MySqlCommand.Parameters.AddWithValue("lastlogindatetime", Convert.ToDateTime(System.DateTime.Now));
                        MySqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteToErrorLogFromAll("error in save adit user" + ex.Message.ToString());
                    }

                    string UserIdentity = "Select usernum from userod where username = 'Adit'";
                    MySqlCommand.CommandText = UserIdentity;
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    loginId = Convert.ToString(MySqlCommand.ExecuteScalar());

                    if (loginId != null & loginId != "")
                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertAdit_Usergroup;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("usernum", loginId);
                        MySqlCommand.ExecuteNonQuery();

                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertAdit_Userodpref;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("usernum", loginId);
                        MySqlCommand.Parameters.AddWithValue("clinicnum", "0");
                        MySqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
            return loginId;
        }

        public static DataTable GetOpenDentalUserData(string DbString, string Service_Install_Id)
        {
            bool fieldNotExists = false;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                ReSyncUserFromEHR:
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalUserData;
                if (Utility.Application_Version.ToLower() == "15.4".ToLower()|| fieldNotExists)
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalUserData_15_4;
                }
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                try
                {
                    MySqlDa.Fill(MySqlDt);
                    fieldNotExists = false;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Unknown column 'DateTLastLogin'"))
                    {
                        fieldNotExists = true;
                        goto ReSyncUserFromEHR;
                    }
                }
               

                DataTable dtResult = new DataTable();
                DataTable dtResultReturn = new DataTable();
                DataTable Dttmp = MySqlDt.Clone();
                dtResult = MySqlDt.Select("Clinic_Number = 0").CopyToDataTable();
                DataTable dtClinics = new DataTable();

                if (Utility.DtLocationList.Select("Clinic_Number <> 0").Count() > 0)
                {
                    dtClinics = Utility.DtLocationList.Select("Clinic_Number <> 0").CopyToDataTable();
                }

                for (int i = 0; i < dtClinics.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                    {
                        if (MySqlDt.Select("Clinic_Number = " + dtClinics.Rows[i]["Clinic_Number"].ToString()).Count() == 0)
                        {
                            for (int j = 0; j < dtResult.Rows.Count; j++)
                            {
                                DataRow Dr = Dttmp.NewRow();
                                Dr.ItemArray = dtResult.Rows[j].ItemArray;
                                Dr["Clinic_Number"] = dtClinics.Rows[i]["Clinic_Number"].ToString();
                                Dttmp.Rows.Add(Dr);
                            }
                        }
                    }
                }
                if (Dttmp.Rows.Count > 0)
                {
                    MySqlDt.Load(Dttmp.CreateDataReader());
                }

                return MySqlDt;

                //dtResultReturn = MySqlDt.Select("Clinic_Number <> 0").CopyToDataTable();

                //for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                //{
                //    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                //    {
                //        if (MySqlDt.Select("Clinic_Number = " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString()).Count() == 0)
                //        {
                //            for (int j = 0; j < dtResult.Rows.Count; j++)
                //            {
                //                DataRow Dr = Dttmp.NewRow();
                //                Dr.ItemArray = dtResult.Rows[j].ItemArray;
                //                Dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                //                Dttmp.Rows.Add(Dr);
                //            }
                //        }
                //    }
                //}
                //dtResultReturn.Load(Dttmp.CreateDataReader());

                //return MySqlDt;
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

        public static bool Save_User_OpenDental_To_Local(DataTable dtOpendentalUser, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    Utility.WriteToSyncLogFile_All(" In Save_User_OpenDental_To_Local  ");
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtOpendentalUser.Rows)
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
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                }
                                // Utility.WriteToErrorLogFromAll(" query ready for execute ");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    Utility.WriteToErrorLogFromAll(" In Save_User_OpenDental_To_Local  " + ex.Message);
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
                return _successfullstataus;
            }
        }


        #endregion

        public static bool Save_ApptStatus_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalApptStatus, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlExCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //   SqlTransaction SqlExTransaction;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //    SqlExTransaction = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                //   SqlExCommand.Transaction = SqlExTransaction;

                foreach (DataRow dr in dtOpenDentalApptStatus.Rows)
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
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_AppointmentStatus;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_AppointmentStatus;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_AppointmentStatus;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.Parameters.AddWithValue("ApptStatus_EHR_ID", dr["ApptStatus_EHR_ID"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("ApptStatus_Web_ID", "");
                        SqlExCommand.Parameters.AddWithValue("ApptStatus_Name", dr["ApptStatus_Name"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlExCommand.ExecuteNonQuery();
                    }
                }
                //SqlExTransaction.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;

                //SqlExTransaction.Rollback();
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_Insurance_OpenDental_To_Local_SqlServer(DataTable dtOpenDentalInsurance, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlExCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //   SqlTransaction SqlExTransaction;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //    SqlExTransaction = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                //   SqlExCommand.Transaction = SqlExTransaction;

                foreach (DataRow dr in dtOpenDentalInsurance.Rows)
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
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_Insurance;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_Insurance;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_Insurance;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.Parameters.AddWithValue("Insurance_EHR_ID", dr["CarrierNum"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Insurance_Web_ID", "");
                        SqlExCommand.Parameters.AddWithValue("Insurance_Name", dr["CarrierName"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Address", dr["Address"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Zipcode", dr["Zip"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Phone", dr["Phone"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("ElectId", dr["ElectID"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("EmployerName", "");
                        SqlExCommand.Parameters.AddWithValue("Is_Deleted", dr["IsHidden"].ToString().Trim());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        //SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlExCommand.ExecuteNonQuery();
                    }
                }
                //SqlExTransaction.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;

                //SqlExTransaction.Rollback();
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        #endregion

        public static bool Save_Patient_Form_Local_To_OpenDental(DataTable dtWebPatient_Form, string DbString, string Installation_ID)
        {
            string _successfullstataus = string.Empty;


            bool is_Record_Update = false;
            MySqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                string sqlSelect = string.Empty;
                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
                string Update_PatientForm_Record_ID = "";

                foreach (DataRow dr in dtWebPatient_Form.Rows)
                {
                    if (dr["Patient_EHR_ID"].ToString() == "" || dr["Patient_EHR_ID"].ToString() == "-")
                    {
                        dr["Patient_EHR_ID"] = "0";
                    }
                    if (Convert.ToInt64(dr["Patient_EHR_ID"].ToString()) != 0)
                    {
                        if (dr["ehrfield"].ToString().ToLower().Trim() == "PRIMARY_INSURANCE_COMPANYNAME".ToLower().Trim() ||
                            dr["ehrfield"].ToString().ToLower().Trim() == "PRIMARY_INS_SUBSCRIBER_ID".ToLower().Trim() ||
                            dr["ehrfield"].ToString().ToLower().Trim() == "SECONDARY_INSURANCE_COMPANYNAME".ToLower().Trim() ||
                            dr["ehrfield"].ToString().ToLower().Trim() == "SECONDARY_INS_SUBSCRIBER_ID".ToLower().Trim() ||
                             dr["ehrfield"].ToString().ToLower().Trim() == "DRIVERLICENSE".ToLower().Trim()
                            )
                        {

                        }
                        else
                        {
                            string strQauery = "";
                            if (dr["Table_Name"].ToString().ToLower().Trim() == "PATIENT".ToLower().Trim())
                            {

                                strQauery = SynchOpenDentalQRY.Update_Patinet_Record_By_Patient_Form;

                            }
                            else if (dr["Table_Name"].ToString().ToLower().Trim() == "PATIENTNOTE".ToLower().Trim())
                            {
                                strQauery = SynchOpenDentalQRY.Update_PatientEmergencyC_By_Patient_Form;
                            }
                            else if (dr["Table_Name"].ToString().ToLower().Trim() == "EMPLOYER".ToLower().Trim())
                            {
                                GetEmployerOpendental(dr["Patient_EHR_ID"].ToString(), DbString, dr["ehrfield"].ToString(), dr["ehrfield_value"].ToString());

                            }

                            strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());
                            if (strQauery != "" && strQauery != String.Empty)
                            {
                                MySqlCommand.CommandText = strQauery;
                                MySqlCommand.CommandType = CommandType.Text;
                                MySqlCommand.Connection = conn;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                //MySqlCommand.Parameters.AddWithValue("ehrfield", dr["ehrfield"].ToString().Trim());
                                MySqlCommand.Parameters.AddWithValue("ehrfield_value", dr["ehrfield_value"].ToString().Trim());
                                MySqlCommand.ExecuteNonQuery();

                                Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                            }
                        }
                    }
                }

                DataView NewPatientListdv = new DataView(dtWebPatient_Form);
                NewPatientListdv.RowFilter = "Patient_EHR_ID = '0'";
                DataTable distinctValues = NewPatientListdv.ToTable(true, "PatientForm_Web_ID", "Clinic_Number");

                foreach (DataRow dr in distinctValues.Rows)
                {
                    if (Utility.dtLocationWiseUser != null)
                    {
                        if (Utility.dtLocationWiseUser.Rows.Count > 0)
                        {
                            DataRow[] drClinicUser = Utility.dtLocationWiseUser.Copy().Select("ClinicNumber = " + dr["Clinic_Number"].ToString().Trim());
                            Utility.EHR_UserLogin_ID = drClinicUser[0]["EHR_UserLogin_ID"].ToString().Trim();
                        }
                    }

                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                    {
                        Utility.EHR_UserLogin_ID = Save_NewAditUser_In_OpenDental(DbString);
                    }
                    string tmpNewPat = dr["PatientForm_Web_ID"].ToString();
                    DataView NewPatientdv = new DataView(dtWebPatient_Form);
                    NewPatientdv.RowFilter = "PatientForm_Web_ID = '" + tmpNewPat + "'";

                    DataTable newPatientDt = NewPatientdv.ToTable();

                    string tmpField = "";
                    string tmpFiendValue = "";
                    string tmpField1 = "";
                    string tmpFiendValue1 = "";
                    string tmpEmpField = "";
                    string tmpEmpFiendValue = "";
                    Int64 PatientId = 0;
                    string tmpNewPatQry = "", tmpNewPatQry1 = "";

                    DataView NewPatientFielddv = new DataView(newPatientDt);
                    DataTable NewPatientFielddt = NewPatientFielddv.ToTable(true, "ehrfield", "ehrfield_value", "Table_Name");



                    string curPatinetInsurancePri_Name = "";
                    string curPatinetInsurancePri_Sub_ID = "";
                    string curPatinetInsuranceSec_Name = "";
                    string curPatinetInsuranceSec_Sub_ID = "";

                    foreach (DataRow drNPat in NewPatientFielddt.Rows)
                    {

                        //if (drNPat["ehrfield"].ToString().ToLower() != "txtmsgok" )
                        //{
                        if ((drNPat["Table_Name"].ToString().ToLower() == "patient"))
                        {

                            //tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                            //tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";

                            if (drNPat["ehrfield"].ToString().ToLower() == "PRIMARY_INSURANCE_COMPANYNAME".ToLower())
                            {
                                curPatinetInsurancePri_Name = drNPat["ehrfield_value"].ToString();
                            }
                            else if (drNPat["ehrfield"].ToString().ToLower() == "PRIMARY_INS_SUBSCRIBER_ID".ToLower())
                            {
                                curPatinetInsurancePri_Sub_ID = drNPat["ehrfield_value"].ToString();
                            }
                            else if (drNPat["ehrfield"].ToString().ToLower() == "SECONDARY_INSURANCE_COMPANYNAME".ToLower())
                            {
                                curPatinetInsuranceSec_Name = drNPat["ehrfield_value"].ToString();
                            }
                            else if (drNPat["ehrfield"].ToString().ToLower() == "SECONDARY_INS_SUBSCRIBER_ID".ToLower())
                            {
                                curPatinetInsuranceSec_Sub_ID = drNPat["ehrfield_value"].ToString();
                            }
                            else if (drNPat["ehrfield"].ToString().ToLower() == "DRIVERLICENSE".ToLower())
                            {
                                //curPatinetInsuranceSec_Sub_ID = drNPat["ehrfield_value"].ToString();
                            }
                            else
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                            }
                        }
                        else if (drNPat["Table_Name"].ToString().ToLower() == "patientnote")
                        {
                            string a = drNPat["ehrfield"].ToString();
                            if (drNPat["ehrfield"].ToString().ToLower() == "icename" || drNPat["ehrfield"].ToString().ToLower() == "icephone")
                            {
                                string k = drNPat["ehrfield"].ToString();

                                tmpField1 = tmpField1 + drNPat["ehrfield"].ToString() + ",";
                                tmpFiendValue1 = tmpFiendValue1 + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";

                            }


                        }
                        else if (drNPat["Table_Name"].ToString().ToLower() == "employer")
                        {
                            tmpEmpField = drNPat["ehrfield"].ToString();
                            tmpEmpFiendValue = drNPat["ehrfield_value"].ToString();


                        }


                        //}
                        //if (drNPat["ehrfield"].ToString().ToLower() == "txtmsgok ")
                        //{


                        //}





                    }

                    if (NewPatientFielddt.AsEnumerable().Where(o => o.Field<string>("ehrfield") == "PriProv").Count() == 0)
                    {
                        tmpField = tmpField + "PriProv,";
                        tmpFiendValue = tmpFiendValue + "(select provNum from provider where issecondary = 0 and ishidden = 0 order by fname limit 1),";
                    }


                    tmpField = tmpField.Remove(tmpField.Length - 1, 1);
                    tmpFiendValue = tmpFiendValue.Remove(tmpFiendValue.Length - 1, 1);

                    if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                    {
                        tmpNewPatQry = "Insert Into Patient (" + tmpField + ",clinicNum ) Values (" + tmpFiendValue + ",'" + dr["Clinic_Number"].ToString() + "')";
                    }
                    else
                    {
                        tmpNewPatQry = "Insert Into Patient (" + tmpField + ",clinicNum,SecUserNumEntry) Values (" + tmpFiendValue + ",'" + dr["Clinic_Number"].ToString() + "','" + Utility.EHR_UserLogin_ID + "')";
                    }
                    MySqlCommand.CommandText = tmpNewPatQry;
                    MySqlCommand.ExecuteNonQuery();

                    string QryIdentity = "Select @@Identity as newId from patient";
                    MySqlCommand.CommandText = QryIdentity;
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    PatientId = Convert.ToInt64(MySqlCommand.ExecuteScalar());


                    if (tmpField1 != "" && tmpFiendValue1 != "")
                    {
                        tmpField1 = tmpField1.Remove(tmpField1.Length - 1, 1);
                        tmpFiendValue1 = tmpFiendValue1.Remove(tmpFiendValue1.Length - 1, 1);

                        tmpNewPatQry1 = "Insert Into Patientnote (patnum," + tmpField1 + ") Values " + "(" + PatientId + "," + tmpFiendValue1 + ")";
                        MySqlCommand.CommandText = tmpNewPatQry1;
                        MySqlCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                        {
                            tmpNewPatQry1 = "INSERT INTO patientnote (patnum,icename,icephone)VALUES" + "(" + PatientId + ",'','');";
                            MySqlCommand.CommandText = tmpNewPatQry1;
                            MySqlCommand.ExecuteNonQuery();
                        }

                    }


                    MySqlCommand.CommandText = SynchOpenDentalQRY.UpdatePatientGuarantorID;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.Parameters.AddWithValue("Guarantor", PatientId);
                    MySqlCommand.Parameters.AddWithValue("PatNum", PatientId);
                    MySqlCommand.ExecuteNonQuery();


                    if (tmpEmpField != "" && tmpEmpFiendValue != "")
                    {
                        GetEmployerOpendental(PatientId.ToString(), DbString, tmpEmpField, tmpEmpFiendValue);
                    }
                    UpdatePatientEHRIdINPatientForm(PatientId.ToString(), dr["PatientForm_Web_ID"].ToString().Trim(), Installation_ID);

                    Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";

                    if (curPatinetInsurancePri_Name != "")
                    {
                        UpdatePatientInsurance(curPatinetInsurancePri_Name, PatientId, 1, "insert", curPatinetInsurancePri_Sub_ID, DbString);
                    }
                    if (curPatinetInsuranceSec_Name != "")
                    {
                        UpdatePatientInsurance(curPatinetInsuranceSec_Name, PatientId, 2, "insert", curPatinetInsuranceSec_Sub_ID, DbString);
                    }
                }

                SynchLocalDAL.UpdatePatientFormEHR_Updateflg(dtWebPatient_Form);

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

        public static void GetEmployerOpendental(string Patient_EHR_ID, string DbString, string EName, string EMPValue)
        {

            MySqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = new MySqlCommand();
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            string strQauery1, EmpIdentity;
            Int64 EMPNum;
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();


                strQauery1 = "Select EmployerNum from employer where  EmpName = '" + EMPValue + "';";
                MySqlCommand cmd2 = new MySqlCommand(strQauery1, conn);
                cmd2.CommandType = CommandType.Text;
                string employid = Convert.ToString(cmd2.ExecuteScalar()).ToString();
                if (employid == "")
                {
                    strQauery1 = "Insert Into employer (" + EName + ") Values ('" + EMPValue + "')";
                    MySqlCommand.CommandText = strQauery1;
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    MySqlCommand.ExecuteNonQuery();
                    EmpIdentity = "Select @@Identity as newId from employer";
                    MySqlCommand.CommandText = EmpIdentity;
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    EMPNum = Convert.ToInt64(MySqlCommand.ExecuteScalar());

                }
                else
                {
                    EMPNum = Convert.ToInt64(employid);
                }

                string UpdateEMPNum = "update patient set EmployerNum=" + EMPNum + "   where PatNum=" + Patient_EHR_ID;
                MySqlCommand.CommandText = UpdateEMPNum;
                MySqlCommand.Connection = conn;
                MySqlCommand.ExecuteNonQuery();


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

        public static Int64 savepayment(DataRow dr,string DbString,Int32 PaymentMode, Int32 PaymentRefundMode,string defaultProvider,string _filename_EHR_Payment = "",string _EHRLogdirectory_EHR_Payment = "")
        {
            Int64 paymentid = 0;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = null;
            string sqlSelect = string.Empty;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
            if (conn.State == ConnectionState.Closed) conn.Open();
            //CheckConnection(conn);
            try
            {
                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                {
                    MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientPayment_15_4;
                }
                else

                {
                    MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientPayment;
                }
                MySqlCommand.Parameters.Clear();


                // MySqlCommand.Parameters.AddWithValue("paytype", dr["paytype"].ToString());
                MySqlCommand.Parameters.AddWithValue("paydate", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                {
                    MySqlCommand.Parameters.AddWithValue("paytype", PaymentMode.ToString());
                    MySqlCommand.Parameters.AddWithValue("payamt", (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString());
                }
                else if (dr["PaymentMode"].ToString().ToUpper() == "REFUNDED" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                {
                    MySqlCommand.Parameters.AddWithValue("paytype", PaymentRefundMode.ToString());
                    MySqlCommand.Parameters.AddWithValue("payamt", Convert.ToDecimal((Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString()) * -1);
                }
                MySqlCommand.Parameters.AddWithValue("checknum", dr["ChequeNumber"].ToString());
                MySqlCommand.Parameters.AddWithValue("bankbranch", dr["BankOrBranchName"].ToString());
                MySqlCommand.Parameters.AddWithValue("paynote", dr["template"].ToString());
                MySqlCommand.Parameters.AddWithValue("patnum", dr["PatientEHRId"].ToString());
                MySqlCommand.Parameters.AddWithValue("clinicnum", dr["Clinic_Number"].ToString());
                if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                {
                    MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                }
                MySqlCommand.ExecuteNonQuery();
                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Payment_15.4 PatientEHRId=" + dr["PatientEHRId"].ToString() + ",PaymentMode=" + dr["PaymentMode"].ToString().ToUpper());
                }
                else
                { 
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Payment PatientEHRId=" + dr["PatientEHRId"].ToString() + ",PaymentMode=" + dr["PaymentMode"].ToString().ToUpper());
                }
                string QryIdentity = "Select @@Identity as newId from Payment;";
                MySqlCommand.CommandText = QryIdentity;
                MySqlCommand.Parameters.Clear();
                MySqlCommand.CommandType = CommandType.Text;
                MySqlCommand.Connection = conn;
                //CheckConnection(conn);
                paymentid = Convert.ToInt32(MySqlCommand.ExecuteScalar());

                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                {
                    MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientSplitPayment_15_4;
                }
                else

                {
                    MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientSplitPayment;
                }
                MySqlCommand.Parameters.Clear();
                if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                {
                    MySqlCommand.Parameters.AddWithValue("splitamt", (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString());
                }
                else if (dr["PaymentMode"].ToString().ToUpper() == "REFUNDED" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                {
                    MySqlCommand.Parameters.AddWithValue("splitamt", Convert.ToDecimal((Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString()) * -1);
                }
                MySqlCommand.Parameters.AddWithValue("patnum", dr["PatientEHRId"].ToString());
                MySqlCommand.Parameters.AddWithValue("procdate", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                MySqlCommand.Parameters.AddWithValue("paynum", paymentid);
                MySqlCommand.Parameters.AddWithValue("provnum", defaultProvider);
                MySqlCommand.Parameters.AddWithValue("datepay", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                MySqlCommand.Parameters.AddWithValue("clinicnum", dr["Clinic_Number"].ToString());
                if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                {
                    MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                }
                MySqlCommand.ExecuteNonQuery();

                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patien Payment 15.4 PatientEHRId=" + dr["PatientEHRId"].ToString() + ",paymentid = " + paymentid.ToString ());
                }
                else
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patien Payment PatientEHRId=" + dr["PatientEHRId"].ToString() + ",paymentid=" + paymentid.ToString());
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error in Save Payment");
                throw;
            }
            return paymentid;
        }

        public static string GetPaymentNote(string DbString,Int64 PaymentId)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            string note = string.Empty;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = "select PayNote from payment where PayNum=@PaymentId";
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("PaymentId", PaymentId);
                note = Convert.ToString(MySqlCommand.ExecuteScalar());
                return note;
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

        public static bool Save_PatientPayment_Local_To_OpenDental(DataTable dtWebPatientPayment, string DbString, string Installation_ID,string _filename_EHR_Payment = "",string _EHRLogdirectory_EHR_Payment = "")
        {
            bool is_Record_Update = false;
            MySqlConnection conn = null;
            Int32 DiscountMode = 0;
            Int32 PaymentMode = 0;           
            Int32 PaymentRefundMode = 0;
            Int64 paymentId = 0;
            Int64 discountId = 0;
            Int64 noteId = 0;
            Int32 CareCreditMode = 0;
            Int32 CareCreditRefundMode = 0;
            Int32 CareCreditDiscountMode = 0;

            string defaultProvider = "";
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                {
                    Utility.EHR_UserLogin_ID = Save_NewAditUser_In_OpenDental(DbString);
                }
                string sqlSelect = string.Empty;
                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                #region Check Adit Pay Payment Categeries

                #region Check for the Adit Pay
                string QryIdentity = "SELECT IF( EXISTS( select 1  From definition  where category = 10 and itemname like 'Adit Pay'), 1, 0)";
                MySqlCommand.CommandText = QryIdentity;
                MySqlCommand.Parameters.Clear();
                MySqlCommand.CommandType = CommandType.Text;
                MySqlCommand.Connection = conn;
                PaymentMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check for the Adit Pay  for category = 10 and itemname like 'Adit Pay'");
                if (PaymentMode == 0)
                {
                    QryIdentity = "(select MAX(itemorder) + 1 From definition  where category = 10)";
                    MySqlCommand.CommandText = QryIdentity;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    int orderId = Convert.ToInt32(MySqlCommand.ExecuteScalar());

                    if (orderId > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get new orderId for category = 10");
                    }

                    QryIdentity = "INSERT INTO definition (category,itemorder,itemname,itemvalue,itemcolor,ishidden) VALUES(10," + orderId.ToString() + ",'Adit Pay','',0,0);";
                    QryIdentity = QryIdentity + " SELECT LAST_INSERT_ID() AS TableID;";
                    MySqlCommand.CommandText = QryIdentity;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    PaymentMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    if (PaymentMode > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "save  definition for category=10  and orderid=" + orderId.ToString() + "and  PaymentMode=" + PaymentMode.ToString());
                    }
                }
                else
                {
                    QryIdentity = "select defNum  From definition  where category = 10 and itemname like 'Adit Pay'";
                    MySqlCommand.CommandText = QryIdentity;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    PaymentMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    if (PaymentMode > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get defNum for category = 10 and itemname like 'Adit Pay' with PaymentMode=" + PaymentMode.ToString());
                    }
                }
                #endregion


                #region Check for the Adit Pay Refund
                var result = dtWebPatientPayment.AsEnumerable().Where(o => o.Field<object>("PaymentMode").ToString().ToUpper() == "REFUNDED" || o.Field<object>("PaymentMode").ToString().ToUpper() == "PARTIAL-REFUNDED").FirstOrDefault();
                if (result != null)
                {
                    QryIdentity = "SELECT IF( EXISTS( select 1  From definition  where category = 10 and itemname like 'Adit Pay Refund'), 1, 0)";
                    MySqlCommand.CommandText = QryIdentity;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    PaymentRefundMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check for the Adit Pay Refund for category = 10 and itemname like 'Adit Pay Refund' with PaymentRefundMode=" + PaymentRefundMode.ToString());
                    if (PaymentRefundMode == 0)
                    {
                        QryIdentity = "(select MAX(itemorder) + 1 From definition  where category = 10)";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        int orderId = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (orderId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get new order id=" + orderId + " and category = 10");
                        }
                        QryIdentity = "INSERT INTO definition (category,itemorder,itemname,itemvalue,itemcolor,ishidden) VALUES(10," + orderId.ToString() + ",'Adit Pay Refund','',0,0);";
                        QryIdentity = QryIdentity + " SELECT LAST_INSERT_ID() AS TableID;";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        PaymentRefundMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (PaymentRefundMode > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "save  definition for category=10  and orderid=" + orderId.ToString() + " with  PaymentRefundMode=" + PaymentRefundMode.ToString());
                        }
                    }
                    else
                    {
                        QryIdentity = "select defNum  From definition  where category = 10 and itemname like 'Adit Pay Refund'";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        PaymentRefundMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (PaymentRefundMode > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get defNum for category=10 ,itemname like 'Adit Pay Refund' and  PaymentRefundMode=" + PaymentRefundMode.ToString());
                        }
                    }
                }
                #endregion

                #region Check for the Adit Pay Discount
                result = dtWebPatientPayment.AsEnumerable().Where(o => o.Field<decimal>("Discount") > 0).FirstOrDefault();
                if (result != null)
                {
                    QryIdentity = "SELECT IF( EXISTS( select 1  From definition  where category = 1 and itemname like 'Adit Pay Discount'), 1, 0)";
                    MySqlCommand.CommandText = QryIdentity;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    DiscountMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check for the Adit Pay Discount for category = 1 and itemname like 'Adit Pay Discount' with  DiscountMode=" + DiscountMode.ToString());

                    if (DiscountMode == 0)
                    {
                        QryIdentity = "(select MAX(itemorder) + 1 From definition  where category = 1)";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        int orderId = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (orderId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get new order id=" + orderId + " for category = 1");
                        }

                        QryIdentity = "INSERT INTO definition (category,itemorder,itemname,itemvalue,itemcolor,ishidden) VALUES(1," + orderId.ToString() + ",'Adit Pay Discount','-',0,0);";
                        QryIdentity = QryIdentity + " SELECT LAST_INSERT_ID() AS TableID;";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        DiscountMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (DiscountMode > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "save  definition for category=1  and orderid=" + orderId.ToString() + "and itemvalue='Adit Pay Discount' and DiscountMode=" + DiscountMode.ToString());
                        }
                    }
                    else
                    {
                        QryIdentity = "select defNum  From definition  where category = 1 and itemname like 'Adit Pay Discount'";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        DiscountMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (DiscountMode > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get defNum for category=1  and and itemname like 'Adit Pay Discount' with DiscountMode=" + DiscountMode.ToString());
                        }
                    }
                }
                #endregion

                #endregion

                #region Check CareCredit Payment Categeries

                #region Check for the CareCredit
                string QryIdentity1 = "SELECT IF( EXISTS( select 1  From definition  where category = 10 and itemname like 'CareCredit'), 1, 0)";
                MySqlCommand.CommandText = QryIdentity1;
                MySqlCommand.Parameters.Clear();
                MySqlCommand.CommandType = CommandType.Text;
                MySqlCommand.Connection = conn;
                CareCreditMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check for the CareCredit  for category = 10 and itemname like 'CareCredit'");
                if (CareCreditMode == 0)
                {
                    QryIdentity1 = "(select MAX(itemorder) + 1 From definition  where category = 10)";
                    MySqlCommand.CommandText = QryIdentity1;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    int orderIdd = Convert.ToInt32(MySqlCommand.ExecuteScalar());

                    if (orderIdd > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get new orderId for category = 10");
                    }

                    QryIdentity1 = "INSERT INTO definition (category,itemorder,itemname,itemvalue,itemcolor,ishidden) VALUES(10," + orderIdd.ToString() + ",'CareCredit','',0,0);";
                    QryIdentity1 = QryIdentity1 + " SELECT LAST_INSERT_ID() AS TableID;";
                    MySqlCommand.CommandText = QryIdentity1;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    CareCreditMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    if (CareCreditMode > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "save  definition for category=10  and orderid=" + orderIdd.ToString() + "and  CareCreditMode=" + CareCreditMode.ToString());
                    }
                }
                else
                {
                    QryIdentity1 = "select defNum  From definition  where category = 10 and itemname like 'CareCredit'";
                    MySqlCommand.CommandText = QryIdentity1;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    CareCreditMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    if (CareCreditMode > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get defNum for category = 10 and itemname like 'CareCredit' with CareCreditMode=" + CareCreditMode.ToString());
                    }
                }
                #endregion

                #region Check for the CareCredit Refund
                var resultt = dtWebPatientPayment.AsEnumerable().Where(o => o.Field<object>("PaymentMode").ToString().ToUpper() == "REFUNDED" || o.Field<object>("PaymentMode").ToString().ToUpper() == "PARTIAL-REFUNDED").FirstOrDefault();
                if (resultt != null)
                {
                    QryIdentity = "SELECT IF( EXISTS( select 1  From definition  where category = 10 and itemname like 'CareCredit Refund'), 1, 0)";
                    MySqlCommand.CommandText = QryIdentity;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    CareCreditRefundMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check for the CareCredit Refund for category = 10 and itemname like 'CareCredit Refund' with CareCreditRefundMode=" + CareCreditRefundMode.ToString());
                    if (CareCreditRefundMode == 0)
                    {
                        QryIdentity = "(select MAX(itemorder) + 1 From definition  where category = 10)";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        int orderId = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (orderId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get new order id=" + orderId + " and category = 10");
                        }
                        QryIdentity = "INSERT INTO definition (category,itemorder,itemname,itemvalue,itemcolor,ishidden) VALUES(10," + orderId.ToString() + ",'CareCredit Refund','',0,0);";
                        QryIdentity = QryIdentity + " SELECT LAST_INSERT_ID() AS TableID;";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        CareCreditRefundMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (CareCreditRefundMode > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "save  definition for category=10  and orderid=" + orderId.ToString() + " with  CareCreditRefundMode=" + CareCreditRefundMode.ToString());
                        }
                    }
                    else
                    {
                        QryIdentity = "select defNum  From definition  where category = 10 and itemname like 'CareCredit Refund'";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        CareCreditRefundMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (CareCreditRefundMode > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get defNum for category=10 ,itemname like 'CareCredit Refund' and  CareCreditRefundMode=" + CareCreditRefundMode.ToString());
                        }
                    }
                }
                #endregion

                #region Check for the CareCredit Discount
                result = dtWebPatientPayment.AsEnumerable().Where(o => o.Field<decimal>("Discount") > 0).FirstOrDefault();
                if (result != null)
                {
                    QryIdentity = "SELECT IF( EXISTS( select 1  From definition  where category = 1 and itemname like 'CareCredit Discount'), 1, 0)";
                    MySqlCommand.CommandText = QryIdentity;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    CareCreditDiscountMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check for the CareCredit Discount for category = 1 and itemname like 'CareCredit Discount' with  CareCreditDiscountMode=" + CareCreditDiscountMode.ToString());

                    if (CareCreditDiscountMode == 0)
                    {
                        QryIdentity = "(select MAX(itemorder) + 1 From definition  where category = 1)";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        int orderId = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (orderId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get new order id=" + orderId + " for category = 1");
                        }

                        QryIdentity = "INSERT INTO definition (category,itemorder,itemname,itemvalue,itemcolor,ishidden) VALUES(1," + orderId.ToString() + ",'CareCredit Discount','-',0,0);";
                        QryIdentity = QryIdentity + " SELECT LAST_INSERT_ID() AS TableID;";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        CareCreditDiscountMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (CareCreditDiscountMode > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "save  definition for category=1  and orderid=" + orderId.ToString() + "and itemvalue='CareCredit Discount' and CareCreditDiscountMode=" + CareCreditDiscountMode.ToString());
                        }
                    }
                    else
                    {
                        QryIdentity = "select defNum  From definition  where category = 1 and itemname like 'CareCredit Discount'";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        CareCreditDiscountMode = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (CareCreditDiscountMode > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get defNum for category=1  and and itemname like 'CareCredit Discount' with CareCreditDiscountMode=" + CareCreditDiscountMode.ToString());
                        }
                    }
                }
                #endregion

                #endregion

                foreach (DataRow dr in dtWebPatientPayment.Rows)
                {
                    try
                    {
                        if (Utility.dtLocationWiseUser != null)
                        {
                            if (Utility.dtLocationWiseUser.Rows.Count > 0)
                            {
                                DataRow[] drClinicUser = Utility.dtLocationWiseUser.Copy().Select("ClinicNumber = " + dr["Clinic_Number"].ToString().Trim());
                                Utility.EHR_UserLogin_ID = drClinicUser[0]["EHR_UserLogin_ID"].ToString().Trim();
                            }
                        }
                        if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                        {
                            Utility.EHR_UserLogin_ID = Save_NewAditUser_In_OpenDental(DbString);
                        }

                        #region Get PrimaryProvider For Patient
                        QryIdentity = "select priprov From patient where patnum = " + dr["PatientEHRId"].ToString();
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        defaultProvider = MySqlCommand.ExecuteScalar().ToString();

                        if (defaultProvider != null)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get Primary provider for PatientEHRId = " + dr["PatientEHRId"].ToString());
                        }
                        #endregion

                        paymentId = 0;
                        discountId = 0;
                        noteId = 0;

                        if (dr["PaymentMethod"].ToString().ToLower() == "carecredit")
                        {
                            SaveCareCreditPaymentToEHR(DbString, Installation_ID, dr, CareCreditMode,defaultProvider, CareCreditRefundMode, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment,paymentId, discountId,CareCreditDiscountMode,noteId);
                        }
                        else
                        {
                            SaveAditPayPaymentToEHR(DbString, Installation_ID, dr, PaymentMode, defaultProvider, PaymentRefundMode, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment,paymentId,discountId, DiscountMode, noteId);
                        }
                    }
                    catch (Exception ex1)
                    {
                        bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(dr,_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
                        if (issavedlocalstatus)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment, " Save PatientPaymentLog To Local status :Success");
                        }
                        else
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment, " Save PatientPaymentLog To Local status :Failed");
                        }
                    }
                }

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

        public static void SaveCareCreditPaymentToEHR(string DbString, string Installation_ID, DataRow dr,Int32 CareCreditMode,string defaultProvider,Int32 CareCreditRefundMode, string _filename_EHR_Payment, string _EHRLogdirectory_EHR_Payment, Int64 paymentId, Int64 discountId,Int32 CareCreditDiscountMode, Int64 noteId)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            if (conn.State == ConnectionState.Closed) conn.Open();
            string sqlSelect = string.Empty;
            CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
            int recordsexists = 0;
            try
            {
                #region Create Payment Entry in EHR if Payment Mode is Paid or Partial Paid and Log seeting is Ledger or Both
                if (Convert.ToInt16(dr["EHRSyncFinancialLogSetting"]) == 2 || Convert.ToInt16(dr["EHRSyncFinancialLogSetting"]) == 3)
                {
                    recordsexists = 0;
                    paymentId = 0;

                    #region check payment exist in payment table
                    string QryIdentity = SynchOpenDentalQRY.checkAlreadyExistsPayment;
                    MySqlCommand.CommandText = QryIdentity;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                    //MySqlCommand.Parameters.AddWithValue("paytype", CareCreditMode.ToString());
                    if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {
                        MySqlCommand.Parameters.AddWithValue("paytype", CareCreditMode.ToString());
                        MySqlCommand.Parameters.AddWithValue("payamt", (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString());
                    }
                    else if (dr["PaymentMode"].ToString().ToUpper() == "REFUNDED" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        MySqlCommand.Parameters.AddWithValue("paytype", CareCreditRefundMode.ToString());
                        MySqlCommand.Parameters.AddWithValue("payamt", (Convert.ToDecimal((Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString()) * -1).ToString());
                    }
                    MySqlCommand.Parameters.AddWithValue("patnum", dr["PatientEHRId"].ToString());
                    MySqlCommand.Parameters.AddWithValue("clinicnum", dr["Clinic_Number"].ToString());
                    // MySqlCommand.Parameters.AddWithValue("paynote", dr["template"].ToString());
                    MySqlCommand.Parameters.AddWithValue("paydate", Convert.ToDateTime(dr["PaymentDate"].ToString()).ToString("yyyy/MM/dd"));
                    //
                    recordsexists = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    #endregion

                    if (recordsexists == 0)//&& (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])) > 0)
                    {
                        #region If record is not exist, save payment to payment table 
                        paymentId = savepayment(dr, DbString, CareCreditMode, CareCreditRefundMode, defaultProvider, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        if (paymentId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "save payment for PatientEHRId = " + dr["PatientEHRId"].ToString() + ",CareCreditRefundMode=" + CareCreditRefundMode.ToString() + " and defaultProvider=" + defaultProvider + " with paymentId=" + paymentId.ToString());

                        }
                        #endregion

                    }
                    else
                    {
                        #region If Record exists already then select paymentid from table
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check Already Exists Payment for  PatientEHRId = " + dr["PatientEHRId"].ToString() + ",PaymentMode=" + dr["PaymentMode"].ToString());
                        QryIdentity = " select payNum  From payment  where paytype = @paytype and paydate = @paydate and payamt = @payamt and patnum =  @patnum and  clinicnum = @clinicnum ";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                        {
                            MySqlCommand.Parameters.AddWithValue("paytype", CareCreditMode.ToString());
                            MySqlCommand.Parameters.AddWithValue("payamt", (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString());
                        }
                        else if (dr["PaymentMode"].ToString().ToUpper() == "REFUNDED" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                        {
                            MySqlCommand.Parameters.AddWithValue("paytype", CareCreditRefundMode.ToString());
                            MySqlCommand.Parameters.AddWithValue("payamt", (Convert.ToDecimal((Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString()) * -1).ToString());
                        }
                        MySqlCommand.Parameters.AddWithValue("patnum", dr["PatientEHRId"].ToString());
                        MySqlCommand.Parameters.AddWithValue("clinicnum", dr["Clinic_Number"].ToString());
                        //  MySqlCommand.Parameters.AddWithValue("paynote", dr["template"].ToString());
                        MySqlCommand.Parameters.AddWithValue("paydate", Convert.ToDateTime(dr["PaymentDate"].ToString()).ToString("yyyy/MM/dd"));
                        //
                        paymentId = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (paymentId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "get paymentId of Payment for PatientEHRID=" + dr["PatientEHRId"].ToString() + " Payment Type =" + CareCreditRefundMode.ToString() + " and PaymentDate =" + Convert.ToDateTime(dr["PaymentDate"].ToString()).ToString("yyyy/MM/dd") + " and  PatientEHRId =" + dr["PatientEHRId"].ToString());
                        }
                        #endregion

                        #region check same amount payments exist ? if not then allow entry to ehr 
                        // int recordexist = SynchLocalDAL.CheckDuplicatePaymentsinLocal(dr["PatientEHRId"].ToString(), Convert.ToDateTime(dr["PaymentDate"].ToString()), dr["Amount"].ToString(), dr["PaymentNote"].ToString(), dr["PaymentMode"].ToString());
                        string note = string.Empty;
                        note = GetPaymentNote(DbString, paymentId);
                        if (!note.Contains(dr["PatientPaymentWebId"].ToString()) && note != string.Empty)
                        {
                            paymentId = savepayment(dr, DbString, CareCreditMode, CareCreditRefundMode, defaultProvider, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                            if (paymentId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save payment for PatientEHRId = " + dr["PatientEHRId"].ToString() + ",CareCreditRefundMode=" + CareCreditRefundMode.ToString() + " and defaultProvider=" + defaultProvider + " ,Note not contain : " + note.Contains(dr["PatientPaymentWebId"].ToString()));
                            }

                        }
                        #endregion

                        #region check and insert payment to paysplit table
                        int splitrecordsexists = 0;
                        QryIdentity = SynchOpenDentalQRY.checkAlreadyExistsSplitPayment;
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        MySqlCommand.Parameters.AddWithValue("paytype", CareCreditMode.ToString());
                        if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                        {
                            MySqlCommand.Parameters.AddWithValue("splitamt", (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString());
                        }
                        else if (dr["PaymentMode"].ToString().ToUpper() == "REFUNDED" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                        {
                            MySqlCommand.Parameters.AddWithValue("splitamt", Convert.ToDecimal((Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString()) * -1);
                        }
                        MySqlCommand.Parameters.AddWithValue("patnum", dr["PatientEHRId"].ToString());
                        MySqlCommand.Parameters.AddWithValue("clinicnum", dr["Clinic_Number"].ToString());
                        MySqlCommand.Parameters.AddWithValue("paynum", paymentId);
                        MySqlCommand.Parameters.AddWithValue("paydate", Convert.ToDateTime(dr["PaymentDate"].ToString()).ToString("yyyy/MM/dd"));

                        splitrecordsexists = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (splitrecordsexists == 0)
                        {
                            CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
                            if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientSplitPayment_15_4;
                            }
                            else
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientSplitPayment;
                            }
                            MySqlCommand.Parameters.Clear();
                            if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                MySqlCommand.Parameters.AddWithValue("splitamt", (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString());
                            }
                            else if (dr["PaymentMode"].ToString().ToUpper() == "REFUNDED" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                            {
                                MySqlCommand.Parameters.AddWithValue("splitamt", Convert.ToDecimal((Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString()) * -1);
                            }
                            MySqlCommand.Parameters.AddWithValue("patnum", dr["PatientEHRId"].ToString());
                            MySqlCommand.Parameters.AddWithValue("procdate", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                            MySqlCommand.Parameters.AddWithValue("paynum", paymentId);
                            MySqlCommand.Parameters.AddWithValue("provnum", defaultProvider);
                            MySqlCommand.Parameters.AddWithValue("datepay", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                            MySqlCommand.Parameters.AddWithValue("clinicnum", dr["Clinic_Number"].ToString());
                            if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                            {
                                MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                            }
                            MySqlCommand.ExecuteNonQuery();
                            if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient SplitPayment for 15.4 with  PaymentId=" + paymentId.ToString() + ",PatientEHRId=" + dr["PatientEHRId"].ToString() + "and Provder ID=" + defaultProvider);
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient SplitPayment with paymentId=" + paymentId.ToString() + ",PatientEHRId=" + dr["PatientEHRId"].ToString() + "and Provider ID=" + defaultProvider);
                            }

                        }
                        #endregion

                    }

                    #region If Discount amount is greater then insert discount intry to Adjustment table
                    if (Convert.ToDecimal(dr["Discount"]) > 0)
                    {
                        //check discount entry
                        int discountexist = 0;
                        QryIdentity = SynchOpenDentalQRY.checkdiscountPayment;
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        MySqlCommand.Parameters.AddWithValue("PaymentDate", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                        if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                        {
                            MySqlCommand.Parameters.AddWithValue("Amount", Convert.ToDecimal(dr["Discount"]) * -1);
                        }
                        MySqlCommand.Parameters.AddWithValue("PatientId", dr["PatientEHRId"].ToString());
                        MySqlCommand.Parameters.AddWithValue("clinicNumber", dr["Clinic_Number"].ToString());
                        MySqlCommand.Parameters.AddWithValue("Note", dr["template"].ToString());
                        discountexist = Convert.ToInt32(MySqlCommand.ExecuteScalar());

                        if (discountexist == 0)
                        {
                            if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientDiscount_15_4;
                            }
                            else
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientDiscount;
                            }
                            MySqlCommand.Parameters.Clear();

                            MySqlCommand.Parameters.AddWithValue("PaymentDate", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                            if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                MySqlCommand.Parameters.AddWithValue("Amount", Convert.ToDecimal(dr["Discount"]) * -1);
                            }
                            MySqlCommand.Parameters.AddWithValue("PatientId", dr["PatientEHRId"].ToString());
                            MySqlCommand.Parameters.AddWithValue("discountMode", CareCreditDiscountMode);
                            MySqlCommand.Parameters.AddWithValue("ProviderId", defaultProvider);
                            MySqlCommand.Parameters.AddWithValue("Note", dr["template"].ToString());
                            MySqlCommand.Parameters.AddWithValue("clinicNumber", dr["Clinic_Number"].ToString());
                            if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                            {
                                MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                            }
                            MySqlCommand.ExecuteNonQuery();
                            if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Discount for 15.4 with  discountMode=" + CareCreditDiscountMode.ToString() + ",PatientEHRId=" + dr["PatientEHRId"].ToString() + "and ProviderId=" + defaultProvider);
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Discount  with  discountMode=" + CareCreditDiscountMode.ToString() + ",PatientEHRId=" + dr["PatientEHRId"].ToString() + "and ProviderId=" + defaultProvider);
                            }

                            QryIdentity = "Select @@Identity as newId from Adjustment;";
                            MySqlCommand.CommandText = QryIdentity;
                            MySqlCommand.Parameters.Clear();
                            MySqlCommand.CommandType = CommandType.Text;
                            MySqlCommand.Connection = conn;
                            //CheckConnection(conn);
                            discountId = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                            if (discountId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get Identity for Adjustment discountId=" + discountId.ToString());
                            }
                        }

                    }
                    #endregion

                    SynchLocalDAL.UpdatePatientPaymentEHRId_In_Local(paymentId.ToString(), dr["PatientPaymentWebId"].ToString().Trim(), Installation_ID, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);

                }
                #endregion  

                #region Create Payment Log entry bcoz Financial setting is only create log or Both
                if (Convert.ToInt16(dr["EHRSyncFinancialLogSetting"]) == 1 || Convert.ToInt16(dr["EHRSyncFinancialLogSetting"]) == 3)
                {
                    noteId = Save_PatientPaymentLog_LocalToOpenDental(dr, DbString, Installation_ID, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
                #endregion  

                if (Convert.ToInt16(dr["EHRSyncFinancialLogSetting"]) == 0)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(dr["Patient_Web_ID"].ToString().Trim(), dr["PatientPaymentWebId"].ToString().Trim(), "completed", Installation_ID.Trim(), dr["Clinic_Number"].ToString().Trim(), "Sync Log and Payment is disabled from Adit App", paymentId.ToString(), noteId.ToString(), CareCreditMode.ToString(), "Sync Log and Payment is disabled from Adit App", Convert.ToInt32(dr["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
                bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(dr);
                if (issavedlocalstatus)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save PatientPaymentLog To Local status :Success");
                }
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(dr["Patient_Web_ID"].ToString().Trim(), dr["PatientPaymentWebId"].ToString().Trim(), "completed", Installation_ID.Trim(), dr["Clinic_Number"].ToString().Trim(), "", paymentId.ToString(), noteId.ToString(), CareCreditMode.ToString(), "", Convert.ToInt32(dr["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }
            catch (Exception ex1)
            {
                Utility.WriteToErrorLogFromAll("Error during CareCreditPayment is "+ ex1.Message);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(dr["Patient_Web_ID"].ToString().Trim(), dr["PatientPaymentWebId"].ToString().Trim(), "error", Installation_ID.Trim(), dr["Clinic_Number"].ToString().Trim(), ex1.Message.ToString(), paymentId.ToString(), noteId.ToString(), CareCreditMode.ToString(), ex1.Message.ToString(), Convert.ToInt32(dr["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }

        }

        public static void SaveAditPayPaymentToEHR(string DbString, string Installation_ID, DataRow dr, Int32 PaymentMode, string defaultProvider, Int32 PaymentRefundMode, string _filename_EHR_Payment, string _EHRLogdirectory_EHR_Payment,Int64 paymentId,Int64 discountId,Int32 DiscountMode,Int64 noteId)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            if (conn.State == ConnectionState.Closed) conn.Open();
            string sqlSelect = string.Empty;
            CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
            int recordsexists = 0;
            try
            {
                #region Create Payment Entry in EHR if Payment Mode is Paid or Partial Paid and Log seeting is Ledger or Both
                if (Convert.ToInt16(dr["EHRSyncPaymentLog"]) == 2 || Convert.ToInt16(dr["EHRSyncPaymentLog"]) == 3)
                {
                    recordsexists = 0;
                    paymentId = 0;
                    #region check payment exist in payment table
                    string QryIdentity = SynchOpenDentalQRY.checkAlreadyExistsPayment;
                    MySqlCommand.CommandText = QryIdentity;
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.CommandType = CommandType.Text;
                    MySqlCommand.Connection = conn;
                 
                    MySqlCommand.Parameters.AddWithValue("paydate", Convert.ToDateTime(dr["PaymentDate"].ToString()).ToString("yyyy/MM/dd"));
                    if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {
                        MySqlCommand.Parameters.AddWithValue("paytype", PaymentMode.ToString());
                        MySqlCommand.Parameters.AddWithValue("payamt", (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString());
                    }
                    else if (dr["PaymentMode"].ToString().ToUpper() == "REFUNDED" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        MySqlCommand.Parameters.AddWithValue("paytype", PaymentRefundMode.ToString());
                        MySqlCommand.Parameters.AddWithValue("payamt", (Convert.ToDecimal((Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString()) * -1).ToString());
                    }
                    MySqlCommand.Parameters.AddWithValue("patnum", dr["PatientEHRId"].ToString());
                    MySqlCommand.Parameters.AddWithValue("clinicnum", dr["Clinic_Number"].ToString());
                    // MySqlCommand.Parameters.AddWithValue("paynote", dr["template"].ToString());
                  
                    //
                    recordsexists = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                    #endregion

                    if (recordsexists == 0)//&& (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])) > 0)
                    {
                        #region If record is not exist, save payment to payment table 
                        paymentId = savepayment(dr, DbString, PaymentMode, PaymentRefundMode, defaultProvider, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        if (paymentId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "save payment for PatientEHRId = " + dr["PatientEHRId"].ToString() + ",PaymentRefundMode=" + PaymentRefundMode.ToString() + " and defaultProvider=" + defaultProvider + " with paymentId=" + paymentId.ToString());

                        }
                        #endregion
                    }
                    else
                    {
                        #region If Record exists already then select paymentid from table
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check Already Exists Payment for  PatientEHRId = " + dr["PatientEHRId"].ToString() + ",PaymentMode=" + dr["PaymentMode"].ToString());
                        QryIdentity = " select payNum  From payment  where paytype = @paytype and paydate = @paydate and payamt = @payamt and patnum =  @patnum and  clinicnum = @clinicnum ";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;

                        if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                        {
                            MySqlCommand.Parameters.AddWithValue("paytype", PaymentMode.ToString());
                            MySqlCommand.Parameters.AddWithValue("payamt", (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString());
                        }
                        else if (dr["PaymentMode"].ToString().ToUpper() == "REFUNDED" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                        {
                            MySqlCommand.Parameters.AddWithValue("paytype", PaymentRefundMode.ToString());
                            MySqlCommand.Parameters.AddWithValue("payamt", (Convert.ToDecimal((Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString()) * -1).ToString());
                        }
                        MySqlCommand.Parameters.AddWithValue("patnum", dr["PatientEHRId"].ToString());
                        MySqlCommand.Parameters.AddWithValue("clinicnum", dr["Clinic_Number"].ToString());
                        //  MySqlCommand.Parameters.AddWithValue("paynote", dr["template"].ToString());
                        MySqlCommand.Parameters.AddWithValue("paydate", Convert.ToDateTime(dr["PaymentDate"].ToString()).ToString("yyyy/MM/dd"));
                        //
                        paymentId = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (paymentId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "get paymentId of Payment for PatientEHRID=" + dr["PatientEHRId"].ToString() + " Payment Type =" + PaymentRefundMode.ToString() + " and PaymentDate =" + Convert.ToDateTime(dr["PaymentDate"].ToString()).ToString("yyyy/MM/dd") + " and  PatientEHRId =" + dr["PatientEHRId"].ToString());
                        }
                        #endregion

                        #region check same amount payments exist ? if not then allow entry to ehr 
                        // int recordexist = SynchLocalDAL.CheckDuplicatePaymentsinLocal(dr["PatientEHRId"].ToString(), Convert.ToDateTime(dr["PaymentDate"].ToString()), dr["Amount"].ToString(), dr["PaymentNote"].ToString(), dr["PaymentMode"].ToString());
                        string note = string.Empty;
                        note = GetPaymentNote(DbString, paymentId);
                        if (!note.Contains(dr["PatientPaymentWebId"].ToString()) && note != string.Empty)
                        {
                            paymentId = savepayment(dr, DbString, PaymentMode, PaymentRefundMode, defaultProvider, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                            if (paymentId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save payment for PatientEHRId = " + dr["PatientEHRId"].ToString() + ",PaymentRefundMode=" + PaymentRefundMode.ToString() + " and defaultProvider=" + defaultProvider + " ,Note not contain : " + note.Contains(dr["PatientPaymentWebId"].ToString()));
                            }

                        }
                        #endregion

                        #region check and insert payment to paysplit table
                        int splitrecordsexists = 0;
                        QryIdentity = SynchOpenDentalQRY.checkAlreadyExistsSplitPayment;
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        
                        if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                        {
                            MySqlCommand.Parameters.AddWithValue("splitamt", (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString());
                        }
                        else
                        {
                            MySqlCommand.Parameters.AddWithValue("splitamt", Convert.ToDecimal((Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString()) * -1);
                        }
                        MySqlCommand.Parameters.AddWithValue("patnum", dr["PatientEHRId"].ToString());
                        MySqlCommand.Parameters.AddWithValue("clinicnum", dr["Clinic_Number"].ToString());
                        MySqlCommand.Parameters.AddWithValue("paynum", paymentId);
                        MySqlCommand.Parameters.AddWithValue("paydate", Convert.ToDateTime(dr["PaymentDate"].ToString()).ToString("yyyy/MM/dd"));

                        splitrecordsexists = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                        if (splitrecordsexists == 0)
                        {
                            CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
                            if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientSplitPayment_15_4;
                            }
                            else
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientSplitPayment;
                            }
                            MySqlCommand.Parameters.Clear();
                            if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                MySqlCommand.Parameters.AddWithValue("splitamt", (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString());
                            }
                            else if (dr["PaymentMode"].ToString().ToUpper() == "REFUNDED" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                            {
                                MySqlCommand.Parameters.AddWithValue("splitamt", Convert.ToDecimal((Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["Discount"])).ToString()) * -1);
                            }
                            MySqlCommand.Parameters.AddWithValue("patnum", dr["PatientEHRId"].ToString());
                            MySqlCommand.Parameters.AddWithValue("procdate", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                            MySqlCommand.Parameters.AddWithValue("paynum", paymentId);
                            MySqlCommand.Parameters.AddWithValue("provnum", defaultProvider);
                            MySqlCommand.Parameters.AddWithValue("datepay", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                            MySqlCommand.Parameters.AddWithValue("clinicnum", dr["Clinic_Number"].ToString());
                            if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                            {
                                MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                            }
                            MySqlCommand.ExecuteNonQuery();
                            if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient SplitPayment for 15.4 with  PaymentId=" + paymentId.ToString() + ",PatientEHRId=" + dr["PatientEHRId"].ToString() + "and Provder ID=" + defaultProvider);
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient SplitPayment with paymentId=" + paymentId.ToString() + ",PatientEHRId=" + dr["PatientEHRId"].ToString() + "and Provider ID=" + defaultProvider);
                            }

                        }
                        #endregion  
                    }

                    #region If Discount amount is greater then insert discount intry to Adjustment table
                    if (Convert.ToDecimal(dr["Discount"]) > 0)
                    {
                        //check discount entry
                        int discountexist = 0;
                        QryIdentity = SynchOpenDentalQRY.checkdiscountPayment;
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        MySqlCommand.Parameters.AddWithValue("PaymentDate", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                        if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                        {
                            MySqlCommand.Parameters.AddWithValue("Amount", Convert.ToDecimal(dr["Discount"]) * -1);
                        }
                        MySqlCommand.Parameters.AddWithValue("PatientId", dr["PatientEHRId"].ToString());
                        MySqlCommand.Parameters.AddWithValue("clinicNumber", dr["Clinic_Number"].ToString());
                        MySqlCommand.Parameters.AddWithValue("Note", dr["template"].ToString());
                        discountexist = Convert.ToInt32(MySqlCommand.ExecuteScalar());

                        if (discountexist == 0)
                        {
                            if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientDiscount_15_4;
                            }
                            else
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientDiscount;
                            }
                            MySqlCommand.Parameters.Clear();

                            MySqlCommand.Parameters.AddWithValue("PaymentDate", Convert.ToDateTime(dr["PaymentDate"].ToString()));
                            if (dr["PaymentMode"].ToString().ToUpper() == "PAID" || dr["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                MySqlCommand.Parameters.AddWithValue("Amount", Convert.ToDecimal(dr["Discount"]) * -1);
                            }
                            MySqlCommand.Parameters.AddWithValue("PatientId", dr["PatientEHRId"].ToString());
                            MySqlCommand.Parameters.AddWithValue("discountMode", DiscountMode);
                            MySqlCommand.Parameters.AddWithValue("ProviderId", defaultProvider);
                            MySqlCommand.Parameters.AddWithValue("Note", dr["template"].ToString());
                            MySqlCommand.Parameters.AddWithValue("clinicNumber", dr["Clinic_Number"].ToString());
                            if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                            {
                                MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                            }
                            MySqlCommand.ExecuteNonQuery();
                            if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Discount for 15.4 with  discountMode=" + DiscountMode.ToString() + ",PatientEHRId=" + dr["PatientEHRId"].ToString() + "and ProviderId=" + defaultProvider);
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Discount  with  discountMode=" + DiscountMode.ToString() + ",PatientEHRId=" + dr["PatientEHRId"].ToString() + "and ProviderId=" + defaultProvider);
                            }

                            QryIdentity = "Select @@Identity as newId from Adjustment;";
                            MySqlCommand.CommandText = QryIdentity;
                            MySqlCommand.Parameters.Clear();
                            MySqlCommand.CommandType = CommandType.Text;
                            MySqlCommand.Connection = conn;
                            //CheckConnection(conn);
                            discountId = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                            if (discountId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get Identity for Adjustment discountId=" + discountId.ToString());
                            }
                        }

                    }
                    #endregion  

                    SynchLocalDAL.UpdatePatientPaymentEHRId_In_Local(paymentId.ToString(), dr["PatientPaymentWebId"].ToString().Trim(), Installation_ID, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
                #endregion

                #region Create Payment Log entry bcoz Financial setting is only create log or Both
                if (Convert.ToInt16(dr["EHRSyncPaymentLog"]) == 1 || Convert.ToInt16(dr["EHRSyncPaymentLog"]) == 3)
                {
                    noteId = Save_PatientPaymentLog_LocalToOpenDental(dr, DbString, Installation_ID, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
                #endregion
                if (Convert.ToInt16(dr["EHRSyncPaymentLog"]) == 0)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(dr["Patient_Web_ID"].ToString().Trim(), dr["PatientPaymentWebId"].ToString().Trim(), "completed", Installation_ID.Trim(), dr["Clinic_Number"].ToString().Trim(), "Sync Log and Payment is disabled from Adit App", paymentId.ToString(), noteId.ToString(), discountId.ToString(), "Sync Log and Payment is disabled from Adit App", Convert.ToInt32(dr["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
                bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(dr);
                if (issavedlocalstatus)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save PatientPaymentLog To Local status :Success");
                }
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(dr["Patient_Web_ID"].ToString().Trim(), dr["PatientPaymentWebId"].ToString().Trim(), "completed", Installation_ID.Trim(), dr["Clinic_Number"].ToString().Trim(), "", paymentId.ToString(), noteId.ToString(), discountId.ToString(), "", Convert.ToInt32(dr["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }
            catch (Exception ex1)
            {
                Utility.WriteToErrorLogFromAll("Error during CareCreditPayment is " + ex1.Message);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(dr["Patient_Web_ID"].ToString().Trim(), dr["PatientPaymentWebId"].ToString().Trim(), "error", Installation_ID.Trim(), dr["Clinic_Number"].ToString().Trim(), ex1.Message.ToString(), paymentId.ToString(), noteId.ToString(), discountId.ToString(), ex1.Message.ToString(), Convert.ToInt32(dr["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }
        }

            private static void UpdatePatientInsurance(string curPatinetInsurance_Name, Int64 PatientId, int InsuranceCount, string insert_update_insurance, string Subscriber_ID, string DbString)
        {
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = Save_NewAditUser_In_OpenDental(DbString);
            }

            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);

            try
            {
                Int64 updateExistinginsurancePatPlanId = 0;
                Int64 updateExistinginsurancePatSubId = 0;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if (insert_update_insurance.ToLower() == "update")
                {
                    string SelectInsu = " Select ins.insSubNum,pp.patplannum,ordinal from inssub ins JOIN patplan pp ON ins.inssubnum = pp.inssubnum "
                                       + " where pp.patnum = " + PatientId + " and ordinal = " + InsuranceCount + ";";
                    CommonDB.MySqlCommandServer(SelectInsu, conn, ref MySqlCommand, "txt");
                    CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                    DataTable MySqlDtIns = new DataTable();
                    MySqlDa.Fill(MySqlDtIns);

                    if (MySqlDtIns.Rows.Count > 0)
                    {
                        updateExistinginsurancePatPlanId = Convert.ToInt64(MySqlDtIns.Rows[0]["patplannum"].ToString());
                        updateExistinginsurancePatSubId = Convert.ToInt64(MySqlDtIns.Rows[0]["insSubNum"].ToString());
                    }
                }

                // MySqlCommand.CommandTimeout = 120;

                string MySqlSelect = " select PlanNum from insplan ip Join carrier c on c.carrierNum = ip.carrierNum where replace(c.CarrierName,'''','') = '" + curPatinetInsurance_Name + "';";
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);

                if (MySqlDt.Rows.Count > 0)
                {
                    if (updateExistinginsurancePatSubId == 0)
                    {
                        if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                        {
                            MySqlCommand.CommandText = SynchOpenDentalQRY.Insert_paitent_insurance_15_4;
                        }
                        else

                        {
                            MySqlCommand.CommandText = SynchOpenDentalQRY.Insert_paitent_insurance;
                        }
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("plannum", MySqlDt.Rows[0][0].ToString());
                        MySqlCommand.Parameters.AddWithValue("subscriber", PatientId);
                        MySqlCommand.Parameters.AddWithValue("SubscriberID", Subscriber_ID.ToString().Trim());
                        if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                        {
                            MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                        }
                        MySqlCommand.ExecuteNonQuery();
                        string QryIdentity = "Select @@Identity as newId from inssub";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        Int64 inssubId = Convert.ToInt64(MySqlCommand.ExecuteScalar());

                        MySqlCommand.CommandText = SynchOpenDentalQRY.Insert_paitent_insurance_patplan;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("patnum", PatientId);
                        MySqlCommand.Parameters.AddWithValue("ordinal", InsuranceCount);
                        MySqlCommand.Parameters.AddWithValue("inssubnum", inssubId);
                        MySqlCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.Update_paitent_insurance;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("plannum", MySqlDt.Rows[0][0].ToString());
                        MySqlCommand.Parameters.AddWithValue("insSubNum", updateExistinginsurancePatSubId);
                        MySqlCommand.Parameters.AddWithValue("SubscriberID", Subscriber_ID);
                        MySqlCommand.ExecuteNonQuery();

                        MySqlCommand.CommandText = SynchOpenDentalQRY.Update_paitent_insurance_patplan;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("inssubnum", updateExistinginsurancePatSubId);
                        MySqlCommand.Parameters.AddWithValue("patplannum", updateExistinginsurancePatPlanId);
                        MySqlCommand.ExecuteNonQuery();
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

        private static bool UpdatePatientEHRIdINPatientForm(string PatientEHRId, string PatientFormWebId, string Service_Install_Id)
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
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.ExecuteNonQuery();
                            _successfullstataus = true;
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
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Save_Document_in_OpenDental(string DbString, string Service_Install_Id, string DocPath, string strPatientFormID = "", string strPatientID = "")
        {
            Int64 DocId = 0;
            bool IsDocUpdate = false;
            bool IsFolderUpadte = false;
            MySqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocData(Service_Install_Id, strPatientFormID);
            if (dtWebPatient_FormDoc.Rows.Count > 0)
            {
                DataTable dtopendentalPatient = GetOpenDentalPatientDocData(DbString, strPatientID);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string ShowingName, SubmitedDate, FormName, PatientName = "";
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientDoc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            if (SynchLocalDAL.CheckLivePatientFormDocDataSynced(Service_Install_Id, dr["PatientDoc_Web_ID"].ToString()))
                            {
                                PullLiveDatabaseDAL.Update_PatientDocNotFound_Live_To_Local(dr["PatientDoc_Web_ID"].ToString(), Service_Install_Id);
                            }
                            continue;
                        }

                        string LastName = "";
                        string FirstName = "";
                        string ImageFolder = "";
                        IsFolderUpadte = false;
                        DataRow dxdr = dtopendentalPatient.Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"].ToString().Trim() + "'").FirstOrDefault();
                        if (dxdr != null)
                        {
                            LastName = dxdr["Last_name"].ToString() == string.Empty || dxdr["Last_name"].ToString() == "" ? "NA" : dxdr["Last_name"].ToString().Split(' ')[0];
                            FirstName = dxdr["First_name"].ToString() == string.Empty || dxdr["First_name"].ToString() == "" ? "NA" : dxdr["First_name"].ToString().Split(' ')[0];
                        }
                        if (dxdr["ImageFolder"] == null || dxdr["ImageFolder"].ToString() == string.Empty || dxdr["ImageFolder"].ToString() == "")
                        {
                            ImageFolder = LastName + FirstName + dr["Patient_EHR_ID"].ToString().Trim();
                            IsFolderUpadte = true;
                        }
                        else
                        {
                            ImageFolder = dxdr["ImageFolder"].ToString();
                        }

                        string destPatientDocPath = "";

                        if (DocPath == string.Empty || DocPath == "")
                        {
                            destPatientDocPath = DocPath + "\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        else if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        else
                        {
                            if (!Directory.Exists(Utility.EHRDocPath))
                            {
                                destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                            }
                            else
                            {
                                destPatientDocPath = Utility.EHRDocPath + "\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                            }
                        }
                        //Utility.WriteToErrorLogFromAll(destPatientDocPath.ToString());
                        //  string destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;

                        string dstnLocation = Path.Combine(destPatientDocPath, Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }

                        System.IO.File.Copy(sourceLocation, dstnLocation, true);

                        FileInfo fi = new FileInfo(dstnLocation);
                        string FileExtension = fi.Extension;

                        string sqlSelect = string.Empty;
                        CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                        string QryDoc = SynchOpenDentalQRY.InsertPatientDocData;

                        MySqlCommand.CommandText = QryDoc;
                        MySqlCommand.Parameters.Clear();
                        //rooja 
                        //MySqlC--ommand.Parameters.AddWithValue("Description", DateTime.Now.ToString("HH:mm:ss"));


                        SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString("MM/dd/yy") : Convert.ToDateTime(dr["Entry_DateTime"]).ToString("MM/dd/yy");
                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                        PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString() : "";

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
                        MySqlCommand.Parameters.AddWithValue("Description", ShowingName);
                        if (dr["folder_ehr_id"].ToString().Trim() == "0" || dr["folder_ehr_id"].ToString().Trim() == "")
                        {
                            MySqlCommand.Parameters.AddWithValue("DocCategory", GetTreatmentPlanDocCategory(DbString, "F", "Patient Information"));
                        }
                        else
                        {
                            MySqlCommand.Parameters.AddWithValue("DocCategory", dr["folder_ehr_id"].ToString().Trim());
                        }

                        //MySqlCommand.Parameters.AddWithValue("Description", dr["Form_Name"].ToString() + "-" + dr["Patient_Name"].ToString());
                        //MySqlCommand.Parameters.AddWithValue("DocCategory", "138");
                        MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString().Trim());
                        MySqlCommand.Parameters.AddWithValue("FileName", dr["PatientDoc_Name"].ToString());
                        MySqlCommand.Parameters.AddWithValue("ImgType", "0");
                        MySqlCommand.Parameters.AddWithValue("ToothNumbers", "");
                        MySqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from document";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        DocId = Convert.ToInt64(MySqlCommand.ExecuteScalar());
                        string RenameFileName = LastName + FirstName + DocId.ToString() + FileExtension;                        
                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + RenameFileName);

                        string UpdateDocName = SynchOpenDentalQRY.UpdatePatientDocFileName;
                        MySqlCommand.CommandText = UpdateDocName;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("FileName", RenameFileName);
                        MySqlCommand.Parameters.AddWithValue("DocNum", DocId.ToString());
                        MySqlCommand.ExecuteNonQuery();

                        if (IsFolderUpadte)
                        {
                            string UpdatePatientFileName = SynchOpenDentalQRY.UpdatePatientFileName;
                            MySqlCommand.CommandText = UpdatePatientFileName;
                            MySqlCommand.Parameters.Clear();
                            MySqlCommand.Parameters.AddWithValue("ImageFolder", ImageFolder);
                            MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString().Trim());
                            MySqlCommand.ExecuteNonQuery();
                        }

                        PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), DocId.ToString(), Service_Install_Id);
                        System.IO.File.Delete(sourceLocation);
                        Save_DocumentAttachment_in_OpenDental(DbString, Service_Install_Id, DocPath, dr["PatientDoc_Web_ID"].ToString());
                    }

                    IsDocUpdate = true;
                }
                catch (Exception ex)
                {
                    DocId = 0;
                    IsDocUpdate = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return IsDocUpdate;
        }

        public static bool Save_DocumentAttachment_in_OpenDental(string DbString, string Service_Install_Id, string DocPath,string PatientForm_web_Id)
        {
            Int64 DocId = 0;
            bool IsDocUpdate = false;
            bool IsFolderUpadte = false;
            MySqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocAttachmentData(Service_Install_Id, PatientForm_web_Id);
            if (dtWebPatient_FormDoc.Rows.Count > 0)
            {
                DataTable dtopendentalPatient = GetOpenDentalPatientDocData(DbString);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string ShowingName, SubmitedDate, FormName, PatientName = "";
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientDoc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            if (SynchLocalDAL.CheckLivePatientFormDocAttachmentDataSynced(Service_Install_Id, dr["PatientDoc_Web_ID"].ToString()))
                            {
                                PullLiveDatabaseDAL.Update_PatientDocAttachmentNotFound_Live_To_Local(dr["PatientForm_web_Id"].ToString(), Service_Install_Id);
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
                        string LastName = "";
                        string FirstName = "";
                        string ImageFolder = "";
                        IsFolderUpadte = false;
                        DataRow dxdr = dtopendentalPatient.Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"].ToString().Trim() + "'").FirstOrDefault();
                        if (dxdr != null)
                        {
                            LastName = dxdr["Last_name"].ToString() == string.Empty || dxdr["Last_name"].ToString() == "" ? "NA" : dxdr["Last_name"].ToString().Split(' ')[0];
                            FirstName = dxdr["First_name"].ToString() == string.Empty || dxdr["First_name"].ToString() == "" ? "NA" : dxdr["First_name"].ToString().Split(' ')[0];
                        }
                        if (dxdr["ImageFolder"] == null || dxdr["ImageFolder"].ToString() == string.Empty || dxdr["ImageFolder"].ToString() == "")
                        {
                            ImageFolder = LastName + FirstName + dr["Patient_EHR_ID"].ToString().Trim();
                            IsFolderUpadte = true;
                        }
                        else
                        {
                            ImageFolder = dxdr["ImageFolder"].ToString();
                        }

                        string destPatientDocPath = "";

                        if (DocPath == string.Empty || DocPath == "")
                        {
                            destPatientDocPath = DocPath + "\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        else if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        else
                        {
                            if (!Directory.Exists(Utility.EHRDocPath))
                            {
                                destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                            }
                            else
                            {
                                destPatientDocPath = Utility.EHRDocPath + "\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                            }
                        }
                        //Utility.WriteToErrorLogFromAll(destPatientDocPath.ToString());
                        //  string destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;

                        string dstnLocation = Path.Combine(destPatientDocPath, Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }

                        System.IO.File.Copy(sourceLocation, dstnLocation, true);

                        FileInfo fi = new FileInfo(dstnLocation);
                        string FileExtension = fi.Extension;

                        string sqlSelect = string.Empty;
                        CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                        string QryDoc = SynchOpenDentalQRY.InsertPatientDocData;

                        MySqlCommand.CommandText = QryDoc;
                        MySqlCommand.Parameters.Clear();
                        //rooja 
                        //MySqlC--ommand.Parameters.AddWithValue("Description", DateTime.Now.ToString("HH:mm:ss"));


                        SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString("MM/dd/yy") : Convert.ToDateTime(dr["Entry_DateTime"]).ToString("MM/dd/yy");
                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                        PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString() : "";

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
                        if (ShowingName.Length > 255)
                        {
                            ShowingName = ShowingName.Substring(0, 254);
                        }
                        //DateTime.Now.ToString("HH:mm:ss")
                        MySqlCommand.Parameters.AddWithValue("Description", ShowingName);
                        if (dr["folder_ehr_id"].ToString().Trim() == "0" || dr["folder_ehr_id"].ToString().Trim() == "")
                        {
                            MySqlCommand.Parameters.AddWithValue("DocCategory", GetTreatmentPlanDocCategory(DbString, "F", "Patient Information"));
                        }
                        else
                        {
                            MySqlCommand.Parameters.AddWithValue("DocCategory", dr["folder_ehr_id"].ToString().Trim());
                        }

                        //MySqlCommand.Parameters.AddWithValue("Description", dr["Form_Name"].ToString() + "-" + dr["Patient_Name"].ToString());
                        //MySqlCommand.Parameters.AddWithValue("DocCategory", "138");
                        MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString().Trim());
                        MySqlCommand.Parameters.AddWithValue("FileName", dr["PatientDoc_Name"].ToString());
                        MySqlCommand.Parameters.AddWithValue("ImgType", dr["DocType"].ToString().ToLower().Contains("pdf") ? "0" : "2");
                        MySqlCommand.Parameters.AddWithValue("ToothNumbers", "");
                        MySqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from document";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        DocId = Convert.ToInt64(MySqlCommand.ExecuteScalar());
                        string RenameFileName = LastName + FirstName + DocId.ToString() + FileExtension;
                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + RenameFileName);

                        string UpdateDocName = SynchOpenDentalQRY.UpdatePatientDocFileName;
                        MySqlCommand.CommandText = UpdateDocName;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("FileName", RenameFileName);
                        MySqlCommand.Parameters.AddWithValue("DocNum", DocId.ToString());
                        MySqlCommand.ExecuteNonQuery(); 

                        if (IsFolderUpadte)
                        {
                            string UpdatePatientFileName = SynchOpenDentalQRY.UpdatePatientFileName;
                            MySqlCommand.CommandText = UpdatePatientFileName;
                            MySqlCommand.Parameters.Clear();
                            MySqlCommand.Parameters.AddWithValue("ImageFolder", ImageFolder);
                            MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString().Trim());
                            MySqlCommand.ExecuteNonQuery();
                        }

                        PullLiveDatabaseDAL.Update_PatientFormDocAttachment_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), DocId.ToString(), Service_Install_Id);
                        System.IO.File.Delete(sourceLocation);
                    }

                    IsDocUpdate = true;
                }
                catch (Exception ex)
                {
                    DocId = 0;
                    IsDocUpdate = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return IsDocUpdate;
        }

        public static bool Save_DocumentAttachment_in_OpenDental(string DbString, string Service_Install_Id, string DocPath, string PatientForm_web_Id, string strPatientID = "")
        {
            Int64 DocId = 0;
            bool IsDocUpdate = false;
            bool IsFolderUpadte = false;
            MySqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocAttachmentData(Service_Install_Id, PatientForm_web_Id);
            if (dtWebPatient_FormDoc.Rows.Count > 0)
            {
                DataTable dtopendentalPatient = GetOpenDentalPatientDocData(DbString, strPatientID);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string ShowingName, SubmitedDate, FormName, PatientName = "";
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientDoc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            if (SynchLocalDAL.CheckLivePatientFormDocAttachmentDataSynced(Service_Install_Id, dr["PatientDoc_Web_ID"].ToString()))
                            {
                                PullLiveDatabaseDAL.Update_PatientDocAttachmentNotFound_Live_To_Local(dr["PatientForm_web_Id"].ToString(), Service_Install_Id);
                            }
                            continue;
                        }

                        string LastName = "";
                        string FirstName = "";
                        string ImageFolder = "";
                        IsFolderUpadte = false;
                        DataRow dxdr = dtopendentalPatient.Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"].ToString().Trim() + "'").FirstOrDefault();
                        if (dxdr != null)
                        {
                            LastName = dxdr["Last_name"].ToString() == string.Empty || dxdr["Last_name"].ToString() == "" ? "NA" : dxdr["Last_name"].ToString().Split(' ')[0];
                            FirstName = dxdr["First_name"].ToString() == string.Empty || dxdr["First_name"].ToString() == "" ? "NA" : dxdr["First_name"].ToString().Split(' ')[0];
                        }
                        if (dxdr["ImageFolder"] == null || dxdr["ImageFolder"].ToString() == string.Empty || dxdr["ImageFolder"].ToString() == "")
                        {
                            ImageFolder = LastName + FirstName + dr["Patient_EHR_ID"].ToString().Trim();
                            IsFolderUpadte = true;
                        }
                        else
                        {
                            ImageFolder = dxdr["ImageFolder"].ToString();
                        }

                        string destPatientDocPath = "";

                        if (DocPath == string.Empty || DocPath == "")
                        {
                            destPatientDocPath = DocPath + "\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        else if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        else
                        {
                            if (!Directory.Exists(Utility.EHRDocPath))
                            {
                                destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                            }
                            else
                            {
                                destPatientDocPath = Utility.EHRDocPath + "\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                            }
                        }
                        //Utility.WriteToErrorLogFromAll(destPatientDocPath.ToString());
                        //  string destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;

                        string dstnLocation = Path.Combine(destPatientDocPath, Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }

                        System.IO.File.Copy(sourceLocation, dstnLocation, true);

                        FileInfo fi = new FileInfo(dstnLocation);
                        string FileExtension = fi.Extension;

                        string sqlSelect = string.Empty;
                        CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                        string QryDoc = SynchOpenDentalQRY.InsertPatientDocData;

                        MySqlCommand.CommandText = QryDoc;
                        MySqlCommand.Parameters.Clear();
                        //rooja 
                        //MySqlC--ommand.Parameters.AddWithValue("Description", DateTime.Now.ToString("HH:mm:ss"));


                        SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString() : Convert.ToDateTime(dr["Entry_DateTime"]).ToString();
                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                        PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString() : "";

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
                        MySqlCommand.Parameters.AddWithValue("Description", ShowingName);
                        if (dr["folder_ehr_id"].ToString().Trim() == "0" || dr["folder_ehr_id"].ToString().Trim() == "")
                        {
                            MySqlCommand.Parameters.AddWithValue("DocCategory", GetTreatmentPlanDocCategory(DbString, "F", "Patient Information"));
                        }
                        else
                        {
                            MySqlCommand.Parameters.AddWithValue("DocCategory", dr["folder_ehr_id"].ToString().Trim());
                        }

                        //MySqlCommand.Parameters.AddWithValue("Description", dr["Form_Name"].ToString() + "-" + dr["Patient_Name"].ToString());
                        //MySqlCommand.Parameters.AddWithValue("DocCategory", "138");
                        MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString().Trim());
                        MySqlCommand.Parameters.AddWithValue("FileName", dr["PatientDoc_Name"].ToString());
                        MySqlCommand.Parameters.AddWithValue("ImgType", dr["DocType"].ToString().ToLower().Contains("pdf") ? "0" : "2");
                        MySqlCommand.Parameters.AddWithValue("ToothNumbers", "");
                        MySqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from document";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        DocId = Convert.ToInt64(MySqlCommand.ExecuteScalar());
                        string RenameFileName = LastName + FirstName + DocId.ToString() + FileExtension;
                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + RenameFileName);

                        string UpdateDocName = SynchOpenDentalQRY.UpdatePatientDocFileName;
                        MySqlCommand.CommandText = UpdateDocName;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("FileName", RenameFileName);
                        MySqlCommand.Parameters.AddWithValue("DocNum", DocId.ToString());
                        MySqlCommand.ExecuteNonQuery();

                        if (IsFolderUpadte)
                        {
                            string UpdatePatientFileName = SynchOpenDentalQRY.UpdatePatientFileName;
                            MySqlCommand.CommandText = UpdatePatientFileName;
                            MySqlCommand.Parameters.Clear();
                            MySqlCommand.Parameters.AddWithValue("ImageFolder", ImageFolder);
                            MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString().Trim());
                            MySqlCommand.ExecuteNonQuery();
                        }

                        PullLiveDatabaseDAL.Update_PatientFormDocAttachment_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), DocId.ToString(), Service_Install_Id);
                        System.IO.File.Delete(sourceLocation);
                    }

                    IsDocUpdate = true;
                }
                catch (Exception ex)
                {
                    DocId = 0;
                    IsDocUpdate = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return IsDocUpdate;
        }
        public static long GetTreatmentPlanDocCategory(string DbString, string strItemValue, string strItemName)
        {
            long lngDocCategory = 0;
            string strQuery = "";
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = null;
            try
            {
                strQuery = "select DefNum from definition where category = 18 And ItemValue = '" + strItemValue + "' And IsHidden = 0 Limit 1";
                CommonDB.MySQLConnectionServer(ref conn, DbString);
                if (conn.State == ConnectionState.Closed) conn.Open();
                CommonDB.MySqlCommandServer(strQuery, conn, ref MySqlCommand, "txt");
                object objDocCategory = MySqlCommand.ExecuteScalar();
                if (objDocCategory != null || objDocCategory != DBNull.Value)
                {
                    lngDocCategory = Convert.ToInt64(objDocCategory);
                }
                if (lngDocCategory <= 0)
                {
                    strQuery = string.Empty;
                    CommonDB.MySqlCommandServer(strQuery, conn, ref MySqlCommand, "txt");
                    strQuery = "select DefNum from definition where category = 18 And ItemName = '" + strItemName + "' Limit 1";
                    MySqlCommand.CommandText = strQuery;
                    MySqlCommand.Parameters.Clear();
                    objDocCategory = MySqlCommand.ExecuteScalar();
                    if (objDocCategory != null || objDocCategory != DBNull.Value)
                    {
                        lngDocCategory = Convert.ToInt64(objDocCategory);
                    }

                    if (lngDocCategory <= 0)
                    {
                        strQuery = string.Empty;
                        CommonDB.MySqlCommandServer(strQuery, conn, ref MySqlCommand, "txt");
                        strQuery = "select Max(ItemOrder) + 1 as ItemOrder from definition where category = 18";
                        MySqlCommand.CommandText = strQuery;
                        MySqlCommand.Parameters.Clear();
                        object objItemOrder = MySqlCommand.ExecuteScalar();
                        long lngItemOrder = 1;
                        if (objItemOrder != null || objItemOrder != DBNull.Value)
                        {
                            lngItemOrder = Convert.ToInt64(objItemOrder);
                        }

                        strQuery = "Insert into definition(category,ItemOrder,ItemName,ItemValue,ItemColor,IsHidden) Values(@category,@ItemOrder,"
                                 + "@ItemName,@ItemValue,@ItemColor,@IsHidden);";
                        MySqlCommand.CommandText = strQuery;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("category", "18");
                        MySqlCommand.Parameters.AddWithValue("ItemOrder", lngItemOrder);
                        MySqlCommand.Parameters.AddWithValue("ItemName", strItemName);
                        MySqlCommand.Parameters.AddWithValue("ItemValue", strItemValue);
                        MySqlCommand.Parameters.AddWithValue("ItemColor", "0");
                        MySqlCommand.Parameters.AddWithValue("IsHidden", "0");
                        MySqlCommand.ExecuteNonQuery();

                        strQuery = "select DefNum from definition where category = 18 And ItemValue = '" + strItemValue + "' And IsHidden = 0 Limit 1";
                        CommonDB.MySQLConnectionServer(ref conn, DbString);
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.MySqlCommandServer(strQuery, conn, ref MySqlCommand, "txt");
                        objDocCategory = MySqlCommand.ExecuteScalar();
                        if (objDocCategory != null || objDocCategory != DBNull.Value)
                        {
                            lngDocCategory = Convert.ToInt64(objDocCategory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lngDocCategory;
        }
       
        public static bool Save_Treatment_Document_in_OpenDental(string DbString, string Service_Install_Id, string DocPath, string strTreatmentPlanID = "", string strPatientEHRId = "")
        {
            Int64 DocId = 0;
            bool IsDocUpdate = false;
            bool IsFolderUpadte = false;
            MySqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DataTable dtLocalTreatment_FormDoc = SynchLocalDAL.GetLocaleTreatmentDocData(strTreatmentPlanID);

            if (dtLocalTreatment_FormDoc.Rows.Count > 0)
            {
                DataTable dtopendentalPatient = GetOpenDentalPatientDocData(DbString, strPatientEHRId);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    foreach (DataRow dr in dtLocalTreatment_FormDoc.Rows)
                    {
                        //string FileName = dr["SubmittedDate"].ToString().Trim() + "-" + dr["TreatmentPlanName"].ToString().Trim() + "-" + dr["PatientName"].ToString().Trim() + ".pdf";
                        //FileName = FileName.Replace(":", "");
                        //FileName = FileName.Replace("/", "-");

                        string sourceLocation = CommonUtility.GetAditTreatmentDocTempPath() + "\\" + dr["TreatmentDoc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            PullLiveDatabaseDAL.Update_TreatrmentDocNotFound_Live_To_Local(dr["TreatmentPlanId"].ToString());
                            continue;
                        }

                        string LastName = "";
                        string FirstName = "";
                        string ImageFolder = "";
                        IsFolderUpadte = false;
                        DataRow dxdr = dtopendentalPatient.Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"].ToString().Trim() + "'").FirstOrDefault();
                        if (dxdr != null)
                        {
                            LastName = dxdr["Last_name"].ToString() == string.Empty || dxdr["Last_name"].ToString() == "" ? "NA" : dxdr["Last_name"].ToString().Split(' ')[0];
                            FirstName = dxdr["First_name"].ToString() == string.Empty || dxdr["First_name"].ToString() == "" ? "NA" : dxdr["First_name"].ToString().Split(' ')[0];
                        }
                        if (dxdr["ImageFolder"] == null || dxdr["ImageFolder"].ToString() == string.Empty || dxdr["ImageFolder"].ToString() == "")
                        {
                            ImageFolder = LastName + FirstName + dr["Patient_EHR_ID"].ToString().Trim();
                            IsFolderUpadte = true;
                        }
                        else
                        {
                            ImageFolder = dxdr["ImageFolder"].ToString();
                        }

                        string destPatientDocPath = "";

                        if (DocPath == string.Empty || DocPath == "")
                        {
                            destPatientDocPath = DocPath + "\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        else if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        else
                        {
                            destPatientDocPath = Utility.EHRDocPath + "\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        //  string destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;

                        string dstnLocation = Path.Combine(destPatientDocPath, Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }

                        System.IO.File.Copy(sourceLocation, dstnLocation, true);

                        FileInfo fi = new FileInfo(dstnLocation);
                        string FileExtension = fi.Extension;

                        string sqlSelect = string.Empty;
                        CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                        string QryDoc = SynchOpenDentalQRY.InsertPatientDocData;

                        string showingName = dr["TreatmentPlanName"].ToString().Trim() + "-" + dr["PatientName"].ToString().Trim();

                        MySqlCommand.CommandText = QryDoc;
                        MySqlCommand.Parameters.Clear();
                        //MySqlCommand.Parameters.AddWithValue("Description", DateTime.Now.ToString("HH:mm:ss"));
                        MySqlCommand.Parameters.AddWithValue("Description", showingName);
                        MySqlCommand.Parameters.AddWithValue("DocCategory", GetTreatmentPlanDocCategory(DbString, "R", "Treatment Plans"));
                        MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString().Trim());
                        MySqlCommand.Parameters.AddWithValue("FileName", dr["TreatmentPlanName"].ToString());
                        MySqlCommand.Parameters.AddWithValue("ImgType", "0");
                        MySqlCommand.Parameters.AddWithValue("ToothNumbers", "");
                        MySqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from document";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        DocId = Convert.ToInt64(MySqlCommand.ExecuteScalar());
                        string RenameFileName = LastName + FirstName + DocId.ToString() + FileExtension;
                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + RenameFileName);

                        string UpdateDocName = SynchOpenDentalQRY.UpdatePatientDocFileName;
                        MySqlCommand.CommandText = UpdateDocName;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("FileName", RenameFileName);
                        MySqlCommand.Parameters.AddWithValue("DocNum", DocId.ToString());
                        MySqlCommand.ExecuteNonQuery();

                        if (IsFolderUpadte)
                        {
                            string UpdatePatientFileName = SynchOpenDentalQRY.UpdatePatientFileName;
                            MySqlCommand.CommandText = UpdatePatientFileName;
                            MySqlCommand.Parameters.Clear();
                            MySqlCommand.Parameters.AddWithValue("ImageFolder", ImageFolder);
                            MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString().Trim());
                            MySqlCommand.ExecuteNonQuery();
                        }
                        PullLiveDatabaseDAL.Update_TreatmentFormDoc_Local_To_EHR(dr["TreatmentDoc_Web_ID"].ToString(), DocId.ToString());
                        System.IO.File.Delete(sourceLocation);
                    }
                    IsDocUpdate = true;
                }
                catch (Exception ex)
                {
                    DocId = 0;
                    IsDocUpdate = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return IsDocUpdate;
        }

        public static bool Save_InsuranceCarrier_Document_in_OpenDental(string DbString, string Service_Install_Id, string DocPath, string strInsuranceCarrierID = "", string strPatientEHRId = "")
        {
            Int64 DocId = 0;
            bool IsDocUpdate = false;
            bool IsFolderUpadte = false;
            MySqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DataTable dtLocalInsuranceCarrier_FormDoc = SynchLocalDAL.GetLocaleInsuranceCarrierDocData(strInsuranceCarrierID);

            if (dtLocalInsuranceCarrier_FormDoc.Rows.Count > 0)
            {
                DataTable dtopendentalPatient = GetOpenDentalPatientDocData(DbString, strPatientEHRId);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    foreach (DataRow dr in dtLocalInsuranceCarrier_FormDoc.Rows)
                    {
                        //string FileName = dr["SubmittedDate"].ToString().Trim() + "-" + dr["TreatmentPlanName"].ToString().Trim() + "-" + dr["PatientName"].ToString().Trim() + ".pdf";
                        //FileName = FileName.Replace(":", "");
                        //FileName = FileName.Replace("/", "-");

                        string sourceLocation = CommonUtility.GetAditInsuranceCarrierDocTempPath() + "\\" + dr["InsuranceCarrier_Doc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            PullLiveDatabaseDAL.Update_InsuranceCarrierDocNotFound_Live_To_Local(dr["InsuranceCarrier_Doc_Web_ID"].ToString());
                            continue;
                        }

                        string LastName = "";
                        string FirstName = "";
                        string ImageFolder = "";
                        IsFolderUpadte = false;
                        DataRow dxdr = dtopendentalPatient.Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"].ToString().Trim() + "'").FirstOrDefault();
                        if (dxdr != null)
                        {
                            LastName = dxdr["Last_name"].ToString() == string.Empty || dxdr["Last_name"].ToString() == "" ? "NA" : dxdr["Last_name"].ToString().Split(' ')[0];
                            FirstName = dxdr["First_name"].ToString() == string.Empty || dxdr["First_name"].ToString() == "" ? "NA" : dxdr["First_name"].ToString().Split(' ')[0];
                        }
                        if (dxdr["ImageFolder"] == null || dxdr["ImageFolder"].ToString() == string.Empty || dxdr["ImageFolder"].ToString() == "")
                        {
                            ImageFolder = LastName + FirstName + dr["Patient_EHR_ID"].ToString().Trim();
                            IsFolderUpadte = true;
                        }
                        else
                        {
                            ImageFolder = dxdr["ImageFolder"].ToString();
                        }

                        string destPatientDocPath = "";

                        if (DocPath == string.Empty || DocPath == "")
                        {
                            destPatientDocPath = DocPath + "\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        else if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        else
                        {
                            destPatientDocPath = Utility.EHRDocPath + "\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;
                        }
                        //  string destPatientDocPath = "C:\\OpenDentImages\\" + ImageFolder.Substring(0, 1).ToUpper() + "\\" + ImageFolder;

                        string dstnLocation = Path.Combine(destPatientDocPath, Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }

                        System.IO.File.Copy(sourceLocation, dstnLocation, true);

                        FileInfo fi = new FileInfo(dstnLocation);
                        string FileExtension = fi.Extension;

                        string sqlSelect = string.Empty;
                        CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                        string QryDoc = SynchOpenDentalQRY.InsertPatientDocData;

                        string showingName = dr["InsuranceCarrier_Doc_Name"].ToString().Trim();

                        MySqlCommand.CommandText = QryDoc;
                        MySqlCommand.Parameters.Clear();
                        //MySqlCommand.Parameters.AddWithValue("Description", DateTime.Now.ToString("HH:mm:ss"));
                        MySqlCommand.Parameters.AddWithValue("Description", showingName);
                        MySqlCommand.Parameters.AddWithValue("DocCategory", dr["InsuranceCarrier_FolderName"]);
                        MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString().Trim());
                        MySqlCommand.Parameters.AddWithValue("FileName", dr["InsuranceCarrier_Doc_Name"].ToString());
                        MySqlCommand.Parameters.AddWithValue("ImgType", "0");
                        MySqlCommand.Parameters.AddWithValue("ToothNumbers", "");
                        MySqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from document";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        DocId = Convert.ToInt64(MySqlCommand.ExecuteScalar());
                        string RenameFileName = LastName + FirstName + DocId.ToString() + FileExtension;
                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + RenameFileName);

                        string UpdateDocName = SynchOpenDentalQRY.UpdatePatientDocFileName;
                        MySqlCommand.CommandText = UpdateDocName;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("FileName", RenameFileName);
                        MySqlCommand.Parameters.AddWithValue("DocNum", DocId.ToString());
                        MySqlCommand.ExecuteNonQuery();

                        if (IsFolderUpadte)
                        {
                            string UpdatePatientFileName = SynchOpenDentalQRY.UpdatePatientFileName;
                            MySqlCommand.CommandText = UpdatePatientFileName;
                            MySqlCommand.Parameters.Clear();
                            MySqlCommand.Parameters.AddWithValue("ImageFolder", ImageFolder);
                            MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString().Trim());
                            MySqlCommand.ExecuteNonQuery();
                        }
                        PullLiveDatabaseDAL.Update_InsuranceCarrierDoc_Local_To_EHR(dr["InsuranceCarrier_Doc_Web_ID"].ToString(), DocId.ToString());
                        System.IO.File.Delete(sourceLocation);
                    }
                    IsDocUpdate = true;
                }
                catch (Exception ex)
                {
                    DocId = 0;
                    IsDocUpdate = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return IsDocUpdate;
        }
        public static string GetOpenDentalDocPath(string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalDocPath;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
                if (MySqlDt.Rows.Count > 0)
                    return MySqlDt.Rows[0]["ValueString"].ToString().TrimEnd('\\');
                else
                    return "";
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

        public static DataSet GetOpenDentalMedicalHistoryData(string DbString, string Service_Install_Id)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DataSet dsResult = new DataSet();
            DataTable dtMedicalHistoryForm = new DataTable();
            DataTable dtMedicalHistoryFields = new DataTable();

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalMedicalHistoryForm;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtMedicalHistoryForm);
                dtMedicalHistoryForm.TableName = "OD_SheetDef";

                DataTable Dttmp = dtMedicalHistoryForm.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                    {
                        for (int j = 0; j < dtMedicalHistoryForm.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp.NewRow();
                            Dr.ItemArray = dtMedicalHistoryForm.Rows[j].ItemArray;
                            Dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                            Dttmp.Rows.Add(Dr);
                        }
                    }
                }
                Dttmp.TableName = "OD_SheetDef";
                dsResult.Tables.Add(Dttmp);

                if (conn.State == ConnectionState.Closed) conn.Open();
                if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalMedicalHistoryFields_15_4;
                }
                else
                {
                    MySqlSelect = SynchOpenDentalQRY.GetOpenDentalMedicalHistoryFields_17_Plus;
                }
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                MySqlDa.Fill(dtMedicalHistoryFields);
                dtMedicalHistoryFields.TableName = "OD_SheetFieldDef";

                DataTable Dttmp1 = dtMedicalHistoryFields.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                    {
                        for (int j = 0; j < dtMedicalHistoryFields.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp1.NewRow();
                            Dr.ItemArray = dtMedicalHistoryFields.Rows[j].ItemArray;
                            Dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                            Dttmp1.Rows.Add(Dr);
                        }
                    }
                }
                Dttmp1.TableName = "OD_SheetFieldDef";
                dsResult.Tables.Add(Dttmp1);

                return dsResult;
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


        #region LocalToOpendental MediclerHistory
        public static void CheckConnection(MySqlConnection CON)
        {
            if (CON.State != ConnectionState.Open)
                CON.Open();
        }


        public static bool SavePatientDisease(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            Int64 DiseaseId = 0;
            try
            {
                DataTable dtPatientDiseaseResponse = SynchLocalDAL.GetLocalPatientFormDiseaseResponseToSaveINEHR(Service_Install_Id, strPatientFormID);
                if (dtPatientDiseaseResponse != null)
                {
                    if (dtPatientDiseaseResponse.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPatientDiseaseResponse.Rows)
                        {
                            CommonDB.MySQLConnectionServer(ref conn, DbString);
                            string sqlSelect = string.Empty;
                            CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                            CheckConnection(conn);
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            if (dr["Disease_Type"].ToString() == "A")
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientAllergies;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("AllergyDefNum", dr["Disease_EHR_Id"].ToString());
                                MySqlCommand.Parameters.AddWithValue("PatNum", dr["PatientEHRID"].ToString());
                                MySqlCommand.Parameters.AddWithValue("Reaction", "");
                                MySqlCommand.Parameters.AddWithValue("StatusIsActive", 1);
                                MySqlCommand.Parameters.AddWithValue("dateTStamp", Convert.ToDateTime(System.DateTime.Now));
                                MySqlCommand.Parameters.AddWithValue("dateAdverseReaction", "0001-01-01");
                                MySqlCommand.Parameters.AddWithValue("snomedReaction", "Web");

                                MySqlCommand.ExecuteNonQuery();

                                string QryIdentity = "Select @@Identity as newId from allergy";
                                MySqlCommand.CommandText = QryIdentity;
                                MySqlCommand.CommandType = CommandType.Text;
                                MySqlCommand.Connection = conn;
                                DiseaseId = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                                SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), DiseaseId.ToString() == "" ? dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString() : DiseaseId.ToString(), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), Service_Install_Id);

                            }
                            else
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientProblems;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("PatNum", dr["PatientEHRID"].ToString());
                                MySqlCommand.Parameters.AddWithValue("DiseaseDefNum", dr["Disease_EHR_Id"].ToString());
                                MySqlCommand.Parameters.AddWithValue("patnote", "");
                                MySqlCommand.Parameters.AddWithValue("dateTStamp", Convert.ToDateTime(System.DateTime.Now));
                                MySqlCommand.Parameters.AddWithValue("probstatus", 0);
                                MySqlCommand.Parameters.AddWithValue("datestart", "0001-01-01");
                                MySqlCommand.Parameters.AddWithValue("datestop", "0001-01-01");
                                MySqlCommand.Parameters.AddWithValue("snomedproblemtype", "");
                                MySqlCommand.Parameters.AddWithValue("functionstatus", "");
                                MySqlCommand.ExecuteNonQuery();

                                string QryIdentity = "Select @@Identity as newId from disease";
                                MySqlCommand.CommandText = QryIdentity;
                                MySqlCommand.CommandType = CommandType.Text;
                                MySqlCommand.Connection = conn;
                                DiseaseId = Convert.ToInt32(MySqlCommand.ExecuteScalar());
                                SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), DiseaseId.ToString() == "" ? dr["PatientEHRId"].ToString() + "_" + dr["Disease_EHR_Id"].ToString() : DiseaseId.ToString(), dr["PatientEHRId"].ToString(), dr["Disease_EHR_Id"].ToString(), Service_Install_Id);
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
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool DeletePatientDisease(string DbString, string Service_Install_Id, string strPatientFormId = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            Int64 DiseaseId = 0;
            try
            {
                DataTable dtPatientDiseaseResponse = SynchLocalDAL.GetLocalPatientFormDiseaseDeleteResponseToSaveINEHR(Service_Install_Id, strPatientFormId);
                if (dtPatientDiseaseResponse != null)
                {
                    if (dtPatientDiseaseResponse.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPatientDiseaseResponse.Rows)
                        {
                            CommonDB.MySQLConnectionServer(ref conn, DbString);
                            string sqlSelect = string.Empty;
                            CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                            CheckConnection(conn);
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            if (dr["Disease_Type"].ToString() == "A")
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.DeletePatientAllergies;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("AllergyDefNum", dr["Disease_EHR_Id"].ToString());
                                MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString());
                                MySqlCommand.ExecuteNonQuery();
                                SynchLocalDAL.UpdateDeleteDiseaseEHR_Updateflg(dr["Disease_EHR_Id"].ToString(), dr["Patient_EHR_Id"].ToString(), Service_Install_Id);

                            }
                            else
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.DeletePatientProblems;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString());
                                MySqlCommand.Parameters.AddWithValue("DiseaseDefNum", dr["Disease_EHR_Id"].ToString());
                                MySqlCommand.ExecuteNonQuery();
                                SynchLocalDAL.UpdateDeleteDiseaseEHR_Updateflg(dr["Disease_EHR_Id"].ToString(), dr["Patient_EHR_Id"].ToString(), Service_Install_Id);
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
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static bool SaveMedicalHistoryLocalToOpenDental(string DbString, string Service_Install_Id, string strPatientFormID = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                DataTable LocalODForm = SynchLocalDAL.GetLocalMedicalHistoryRecords("OD_SheetDef", Service_Install_Id, true);

                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Service_Install_Id)
                    {
                        DataTable dtWebPatient_FormMedicalHistory = SynchLocalDAL.GetLiveOpenDentalPatientFormMedicalHistoryData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), strPatientFormID);
                        if (dtWebPatient_FormMedicalHistory != null)
                        {
                            if (dtWebPatient_FormMedicalHistory.Rows.Count > 0)
                            {
                                DataTable LocalRespoanseSetDt = dtWebPatient_FormMedicalHistory.Copy();
                                if (LocalRespoanseSetDt != null)
                                {
                                    DataRow LocalODFormRow = null;
                                    string ResponsesetId = "";
                                    foreach (DataRow dr in LocalRespoanseSetDt.Rows)
                                    {
                                        if (LocalODFormRow == null)
                                        {
                                            LocalODFormRow = LocalODForm.Select("SheetDefNum_EHR_ID = '" + dr["SheetDefNum_EHR_ID"].ToString() + "' And Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' ").FirstOrDefault();
                                        }
                                        string MySqlSelect = string.Empty;

                                        if (ResponsesetId == "")
                                        {
                                            if (conn.State == ConnectionState.Closed) conn.Open();
                                            CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                                            if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                                            {
                                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertODPatMedicleResponseData_15_4;
                                            }
                                            else
                                            {
                                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertODPatMedicleResponseData;
                                            }
                                            MySqlCommand.Parameters.Clear();
                                            MySqlCommand.Parameters.AddWithValue("SheetType", dr["SheetType"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("PatNum", dr["Patient_EHR_ID"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("FontSize", LocalODFormRow["FontSize"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("FontName", LocalODFormRow["FontName"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("Width", LocalODFormRow["Width"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("Height", LocalODFormRow["Height"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("IsLandScape", LocalODFormRow["IsLandScape"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("Description", LocalODFormRow["Sheet_Name"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("IsMultiPage", LocalODFormRow["IsMultiPage"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("ClinicNum", LocalODFormRow["Clinic_Number"].ToString());
                                            if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                                            {
                                                MySqlCommand.Parameters.AddWithValue("SheetDefnum", LocalODFormRow["SheetDefNum_EHR_ID"].ToString());
                                            }
                                            CheckConnection(conn);
                                            MySqlCommand.ExecuteNonQuery();

                                            string QryIdentity = "Select max(SheetNum) as newId from Sheet";
                                            MySqlCommand.CommandText = QryIdentity;
                                            MySqlCommand.CommandType = CommandType.Text;
                                            MySqlCommand.Connection = conn;
                                            CheckConnection(conn);
                                            ResponsesetId = Convert.ToString(MySqlCommand.ExecuteScalar());

                                        }
                                        MySqlSelect = string.Empty;
                                        CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                                        if (Utility.Application_Version.ToLower() == "15.4".ToLower())
                                        {
                                            MySqlCommand.CommandText = SynchOpenDentalQRY.InsertODPatMedicleQuestionResponseData_15_4;
                                        }
                                        else
                                        {
                                            MySqlCommand.CommandText = SynchOpenDentalQRY.InsertODPatMedicleQuestionResponseData_17_Plus;
                                        }
                                        MySqlCommand.Parameters.Clear();
                                        MySqlCommand.Parameters.AddWithValue("SheetNum", ResponsesetId);
                                        MySqlCommand.Parameters.AddWithValue("FieldType", dr["FieldType"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("FieldName", dr["FieldName"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("FieldValue", (dr["FieldValue"].ToString() == "True" && dr["FieldType"].ToString() == "8") ? "X" : dr["FieldValue"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("FontSize", dr["FontSize"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("FontName", dr["FontName"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("FontIsBold", dr["FontIsBold"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("XPos", dr["XPos"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("YPos", dr["YPos"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("Width", dr["Width"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("Height", dr["Height"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("GrowthBehavior", dr["GrowthBehavior"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("RadioButtonValue", dr["RadioButtonValue"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("RadioButtonGroup", dr["RadioButtonGroup"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("IsRequired", Convert.ToBoolean(dr["IsRequired"].ToString()));
                                        MySqlCommand.Parameters.AddWithValue("TabOrder", dr["TabOrder"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("ReportableName", dr["ReportableName"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("TextAlign", dr["TextAlign"].ToString());
                                        MySqlCommand.Parameters.AddWithValue("ItemColor", dr["ItemColor"].ToString());
                                        if (Utility.Application_Version.ToLower() != "15.4".ToLower())
                                        {
                                            MySqlCommand.Parameters.AddWithValue("IsLocked", dr["IsLocked"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("TabOrderMobile", dr["TabOrderMobile"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("UiLabelMobile", dr["UiLabelMobile"].ToString());
                                            MySqlCommand.Parameters.AddWithValue("UiLabelMobileRadioButton", dr["UiLabelMobileRadioButton"].ToString());
                                        }
                                        CheckConnection(conn);
                                        MySqlCommand.ExecuteNonQuery();

                                        UpdateResponseUniqueEHRIdInCD_Response(dr["OD_Response_LocalDB_ID"].ToString(), Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString());

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
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private static bool UpdateResponseUniqueEHRIdInCD_Response(string LocalResponseID, string Service_Install_Id)
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
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_OpenDental_Response_EHR_ID;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("OD_Response_LocalDB_ID", LocalResponseID);
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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




        #endregion

        #region Clinic

        public static DataTable GetOpenDentalClinicData(string DbString)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = SynchOpenDentalQRY.GetOpenDentalClinic;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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

        public static string GetUserId(string DbString)
        {
            MySqlConnection conn = null;
            string employid = "";
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = new MySqlCommand();
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            string strQauery1 = "";
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();


                strQauery1 = "select usernum from commlog order by commlognum desc limit 1;";
                MySqlCommand cmd2 = new MySqlCommand(strQauery1, conn);
                cmd2.CommandType = CommandType.Text;
                employid = Convert.ToString(cmd2.ExecuteScalar()).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return employid;
        }

        public static Int64 Save_PatientPaymentLog_LocalToOpenDental(DataRow drRow, string DbConnectionString, string ServiceInstalltionId,string _filename_EHR_Payment = "",string _EHRLogdirectory_EHR_Payment = "")
        {
            //  bool is_Record_Update = false;
            MySqlConnection conn = null;
            Int64 noteId = 0;
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = Save_NewAditUser_In_OpenDental(DbConnectionString);
            }
            // string userNum = GetUserId(DbConnectionString);
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbConnectionString);
            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                string sqlSelect = string.Empty;
                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                //if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 1 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                //{
                    try
                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientPaymentLog;
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("PatientEHRId", drRow["PatientEHRId"].ToString());
                        MySqlCommand.Parameters.AddWithValue("CommentDate", Convert.ToDateTime(drRow["PaymentDate"].ToString()));
                        MySqlCommand.Parameters.AddWithValue("CommTyep", 237);
                        MySqlCommand.Parameters.AddWithValue("Mode", 0);
                        MySqlCommand.Parameters.AddWithValue("Note", drRow["template"].ToString());
                        if (drRow["PaymentMode"].ToString().ToLower() == "partial-paid")
                        {
                            MySqlCommand.Parameters.AddWithValue("SentOrReceive", 2);
                        }
                        else if (drRow["PaymentMode"].ToString().ToLower() == "paid")
                        {
                            MySqlCommand.Parameters.AddWithValue("SentOrReceive", 2);
                        }
                        else if (drRow["PaymentMode"].ToString().ToLower() == "refunded")
                        {
                            MySqlCommand.Parameters.AddWithValue("SentOrReceive", 1);
                        }
                        else if (drRow["PaymentMode"].ToString().ToLower() == "partial-refunded")
                        {
                            MySqlCommand.Parameters.AddWithValue("SentOrReceive", 1);
                        }
                        else
                        {
                            MySqlCommand.Parameters.AddWithValue("SentOrReceive", 1);
                        }
                        // MySqlCommand.Parameters.AddWithValue("UserNum", userNum);
                        MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                        MySqlCommand.ExecuteScalar();
                   
                    
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Payment Log with PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",PaymentMod=" + drRow["PaymentMode"].ToString().ToLower());

                        string QryIdentity = "Select max(CommlogNum) as noteId from commlog";
                        MySqlCommand.CommandText = QryIdentity;
                        MySqlCommand.CommandType = CommandType.Text;
                        MySqlCommand.Connection = conn;
                        CheckConnection(conn);
                        noteId = Convert.ToInt64(MySqlCommand.ExecuteScalar());
                        if (noteId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Get noteId=" + noteId.ToString());
                        }
                        // is_Record_Update = true;

                    }
                    catch (Exception ex1)
                    {
                        // SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", ServiceInstalltionId.Trim(), drRow["Clinic_Number"].ToString().Trim(), ex1.Message, noteId);
                        throw;
                    }
                //}
                // }
                return noteId;
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            //return is_Record_Update;
        }

        public static string CheckSMSCallLogRecordsExists(DataRow drRow, string DbConnectionString)
        {
            try
            {
                #region Check For the Records exists
                string userNum = GetUserId(DbConnectionString);
                //Utility.WriteToSyncLogFile_All("CheckSMSCallLogRecordsExists Start ");
                MySqlConnection conn = null;
                DateTime datetimeTemp = System.DateTime.Now;
                string noteId = "0";
                Int64 recordsCount = 0;
                MySqlDataAdapter MySqlDa = null;
                MySqlCommand MySqlCommand = null;
                CommonDB.MySQLConnectionServer(ref conn, DbConnectionString);
                if (conn.State == ConnectionState.Closed) conn.Open();
                CommonDB.MySqlCommandServer("", conn, ref MySqlCommand, "txt");
                if (drRow["Mobile"].ToString() == "")
                {
                    MySqlCommand.CommandText = SynchOpenDentalQRY.CheckSMSCallRecordsBlankMobile;
                    // Utility.WriteToSyncLogFile_All("Mobile is blank for patient " + drRow["PatientEHRId"].ToString());
                }
                else
                {
                    MySqlCommand.CommandText = SynchOpenDentalQRY.CheckSMSCallRecords;
                    // Utility.WriteToSyncLogFile_All("Mobile is exists for patient " + drRow["PatientEHRId"].ToString());
                }

                //Utility.WriteToSyncLogFile_All("Check Duplicate Records " + MySqlCommand.CommandText);

                MySqlCommand.Parameters.Clear();
                MySqlCommand.Parameters.AddWithValue("PatientEHRId", drRow["PatientEHRId"].ToString());
                datetimeTemp = Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString());

                if (drRow["Mobile"].ToString() != "")
                {
                    MySqlCommand.Parameters.AddWithValue("CommentDate", Convert.ToDateTime(datetimeTemp.ToShortDateString() + " " + datetimeTemp.Hour.ToString() + ":00:00"));
                }
                //MySqlCommand.Parameters.AddWithValue("CommentDate", Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString()));
                MySqlCommand.Parameters.AddWithValue("CommTyep", 239);
                if (drRow["message_type"].ToString().ToLower() == "call")
                {
                    MySqlCommand.Parameters.AddWithValue("Mode", 3);
                }
                else if (drRow["message_type"].ToString().ToLower() == "appointment_reminder_call")
                {
                    MySqlCommand.Parameters.AddWithValue("Mode", 3);
                }
                else
                {
                    MySqlCommand.Parameters.AddWithValue("Mode", 5);
                }
                MySqlCommand.Parameters.AddWithValue("Note", drRow["text"].ToString());
                if (drRow["message_direction"].ToString().ToUpper() == "OUTBOUND")
                {
                    MySqlCommand.Parameters.AddWithValue("SentOrReceive", 1);
                }
                else
                {
                    MySqlCommand.Parameters.AddWithValue("SentOrReceive", 2);
                }
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                //Utility.WriteToSyncLogFile_All("Check Duplicate Records " + MySqlCommand.CommandText + " PatientId : " + drRow["PatientEHRId"].ToString() + " LogDate : " + drRow["PatientSMSCallLogDate"].ToString() + " Note : " + drRow["text"].ToString());
                try
                {
                    MySqlDa.Fill(MySqlDt);
                }
                catch (Exception ex1)
                {
                    Utility.WriteToSyncLogFile_All("Error in executing and get group by records " + ex1.ToString());
                }


                if (MySqlDt != null && MySqlDt.Rows.Count > 0)
                {
                    // Utility.WriteToSyncLogFile_All("Found Records " + MySqlCommand.CommandText + " PatientId : " + drRow["PatientEHRId"].ToString() + " LogDate : " + drRow["PatientSMSCallLogDate"].ToString() + " Note : " + drRow["text"].ToString() + " NoteId : " + MySqlDt.Rows[0][0].ToString() + " Count : " + MySqlDt.Rows[0][1].ToString());
                    noteId = MySqlDt.Rows[0][0].ToString();
                    recordsCount = Convert.ToInt64(MySqlDt.Rows[0][1]);
                }

                #endregion

                #region Check and Delete duplicate Records
                if (noteId != "0" && recordsCount > 1)
                {
                    conn = null;

                    // Utility.WriteToSyncLogFile_All("Records exists and execute to delete with note id " + noteId);
                    //MySqlCommand MySqlCommand = new MySqlCommand();
                    MySqlCommand = null;
                    CommonDB.MySQLConnectionServer(ref conn, DbConnectionString);

                    if (conn.State == ConnectionState.Closed) conn.Open();
                    CommonDB.MySqlCommandServer("", conn, ref MySqlCommand, "txt");
                    if (drRow["Mobile"].ToString() == "")
                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.DeleteDuplicateLogsBlankMobile;
                    }
                    else
                    {
                        MySqlCommand.CommandText = SynchOpenDentalQRY.DeleteDuplicateLogs;
                    }
                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.Parameters.AddWithValue("PatientEHRId", drRow["PatientEHRId"].ToString());
                    datetimeTemp = Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString());
                    if (drRow["Mobile"].ToString() != "")
                    {
                        MySqlCommand.Parameters.AddWithValue("CommentDate", Convert.ToDateTime(datetimeTemp.ToShortDateString() + " " + datetimeTemp.Hour.ToString() + ":00:00"));
                    }
                    //MySqlCommand.Parameters.AddWithValue("CommentDate", Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString()));
                    MySqlCommand.Parameters.AddWithValue("CommTyep", 239);
                    if (drRow["message_type"].ToString().ToLower() == "call")
                    {
                        MySqlCommand.Parameters.AddWithValue("Mode", 3);
                    }
                    else if (drRow["message_type"].ToString().ToLower() == "appointment_reminder_call")
                    {
                        MySqlCommand.Parameters.AddWithValue("Mode", 3);
                    }
                    else
                    {
                        MySqlCommand.Parameters.AddWithValue("Mode", 5);
                    }
                    MySqlCommand.Parameters.AddWithValue("Note", drRow["text"].ToString());
                    if (drRow["message_direction"].ToString().ToUpper() == "OUTBOUND")
                    {
                        MySqlCommand.Parameters.AddWithValue("SentOrReceive", 1);
                    }
                    else
                    {
                        MySqlCommand.Parameters.AddWithValue("SentOrReceive", 2);
                    }
                    //MySqlCommand.Parameters.AddWithValue("UserNum", 1);
                    MySqlCommand.Parameters.AddWithValue("NoteId", noteId);
                    // Utility.WriteToSyncLogFile_All("Delete Records exept records " + MySqlCommand.CommandText + " PatientId : " + drRow["PatientEHRId"].ToString() + " LogDate : " + drRow["PatientSMSCallLogDate"].ToString() + " Note : " + drRow["text"].ToString() + " NoteId : " + MySqlDt.Rows[0][0].ToString() + " Count : " + MySqlDt.Rows[0][1].ToString());
                    MySqlCommand.ExecuteNonQuery();
                    // Utility.WriteToSyncLogFile_All("Deleted Completed " + MySqlCommand.CommandText + " PatientId : " + drRow["PatientEHRId"].ToString() + " LogDate : " + drRow["PatientSMSCallLogDate"].ToString() + " Note : " + drRow["text"].ToString() + " NoteId : " + MySqlDt.Rows[0][0].ToString() + " Count : " + MySqlDt.Rows[0][1].ToString());
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


        public static string Save_PatientSMSCallLog_LocalToOpenDental(DataTable dtLivePatientSMSCallLog, string DbConnectionString, string ServiceInstalltionId)
        {
            //if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            //{
            //    Utility.EHR_UserLogin_ID = Save_NewAditUser_In_OpenDental(DbConnectionString);
            //}
            //  bool is_Record_Update = false;
            MySqlConnection conn = null;
            // string userNum = GetUserId(DbConnectionString);
            string noteId = "";
            //MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbConnectionString);
            DateTime datetimeTemp = DateTime.Now;
            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                string sqlSelect = string.Empty;
                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
                if (!dtLivePatientSMSCallLog.Columns.Contains("Log_Status"))
                {
                    dtLivePatientSMSCallLog.Columns.Add("Log_Status", typeof(string));
                }

                DataTable dtResultCopy = new DataTable();
                //System.Windows.Forms.MessageBox.Show("Call Save Function");
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    // System.Windows.Forms.MessageBox.Show("1");
                    dtResultCopy = dtLivePatientSMSCallLog.Select("Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' and Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                    // System.Windows.Forms.MessageBox.Show("2");
                    foreach (DataRow drRow in dtResultCopy.Rows)
                    {
                        // System.Windows.Forms.MessageBox.Show("3");
                        if (drRow["PatientEHRId"] != null && drRow["PatientEHRId"].ToString() != string.Empty && drRow["PatientEHRId"].ToString() != "")
                        {
                            //System.Windows.Forms.MessageBox.Show("4");
                            noteId = CheckSMSCallLogRecordsExists(drRow, DbConnectionString);
                            //System.Windows.Forms.MessageBox.Show("5");
                            // Utility.WriteToSyncLogFile_All("Checked Records And NOte Id is " + MySqlCommand.CommandText + " PatientId : " + drRow["PatientEHRId"].ToString() + " LogDate : " + drRow["PatientSMSCallLogDate"].ToString() + " Note : " + drRow["text"].ToString() + " NoteId : " + noteId.ToString() );
                            if (noteId == "0" && ((Convert.ToInt16(drRow["LogType"]) == 0 && drRow["Mobile"].ToString() != "") || Convert.ToInt16(drRow["LogType"]) == 1))
                            {
                                Utility.WriteToSyncLogFile_All("Insert Records if note is zero");
                                try
                                {
                                    if (Utility.dtLocationWiseUser != null)
                                    {
                                        if (Utility.dtLocationWiseUser.Rows.Count > 0)
                                        {
                                            DataRow[] drClinicUser = Utility.dtLocationWiseUser.Copy().Select("ClinicNumber = " + drRow["Clinic_Number"].ToString().Trim());
                                            Utility.EHR_UserLogin_ID = drClinicUser[0]["EHR_UserLogin_ID"].ToString().Trim();
                                        }
                                    }

                                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                                    {
                                        Utility.EHR_UserLogin_ID = Save_NewAditUser_In_OpenDental(DbConnectionString);
                                    }
                                    // System.Windows.Forms.MessageBox.Show("6");
                                    MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientPaymentLog;
                                    MySqlCommand.Parameters.Clear();
                                    MySqlCommand.Parameters.AddWithValue("PatientEHRId", drRow["PatientEHRId"].ToString());
                                    datetimeTemp = Convert.ToDateTime(drRow["PatientSMSCallLogDate"].ToString());
                                    MySqlCommand.Parameters.AddWithValue("CommentDate", Convert.ToDateTime(datetimeTemp.ToShortDateString() + " " + datetimeTemp.Hour.ToString() + ":" + datetimeTemp.Minute.ToString() + ":00"));
                                    MySqlCommand.Parameters.AddWithValue("CommTyep", 239);
                                    if (drRow["message_type"].ToString().ToLower() == "call")
                                    {
                                        MySqlCommand.Parameters.AddWithValue("Mode", 3);
                                    }
                                    else if (drRow["message_type"].ToString().ToLower() == "appointment_reminder_call")
                                    {
                                        MySqlCommand.Parameters.AddWithValue("Mode", 3);
                                    }
                                    else if (drRow["message_type"].ToString().ToLower() == "followup")
                                    {
                                        MySqlCommand.Parameters.AddWithValue("Mode", 0);
                                    }
                                    else
                                    {
                                        MySqlCommand.Parameters.AddWithValue("Mode", 5);
                                    }
                                    MySqlCommand.Parameters.AddWithValue("Note", drRow["text"].ToString());
                                    if (drRow["message_direction"].ToString().ToUpper() == "OUTBOUND")
                                    {
                                        MySqlCommand.Parameters.AddWithValue("SentOrReceive", 1);
                                    }
                                    else if (drRow["message_direction"].ToString().ToLower() == "followup")
                                    {
                                        MySqlCommand.Parameters.AddWithValue("SentOrReceive", 0);
                                    }
                                    else
                                    {
                                        MySqlCommand.Parameters.AddWithValue("SentOrReceive", 2);
                                    }
                                    // MySqlCommand.Parameters.AddWithValue("UserNum", userNum);
                                    MySqlCommand.Parameters.AddWithValue("EHR_User_ID", Utility.EHR_UserLogin_ID);
                                    MySqlCommand.ExecuteScalar();
                                    //  System.Windows.Forms.MessageBox.Show("8");
                                    string QryIdentity = "Select max(CommlogNum) as noteId from commlog";
                                    MySqlCommand.CommandText = QryIdentity;
                                    MySqlCommand.CommandType = CommandType.Text;
                                    MySqlCommand.Connection = conn;
                                    CheckConnection(conn);
                                    noteId = Convert.ToString(MySqlCommand.ExecuteScalar());
                                    drRow["LogEHRId"] = noteId.ToString();
                                    drRow["Log_Status"] = "completed";
                                    // System.Windows.Forms.MessageBox.Show("7");
                                    // is_Record_Update = true;

                                }
                                catch (Exception ex1)
                                {
                                    //  System.Windows.Forms.MessageBox.Show("Error To save records " + ex1.ToString());
                                    if (ex1.InnerException.Message.Length >= 100)
                                    {
                                        drRow["Log_Status"] = "Err_" + ex1.InnerException.Message.Substring(0, 100);
                                    }
                                    else
                                    {
                                        drRow["Log_Status"] = "Err_" + ex1.InnerException.Message.ToString();
                                    }
                                    //throw;
                                }
                            }
                            else if (noteId != "-1")
                            {
                                //Utility.WriteToSyncLogFile_All("Records exists update status to completed " + MySqlCommand.CommandText + " PatientId : " + drRow["PatientEHRId"].ToString() + " LogDate : " + drRow["PatientSMSCallLogDate"].ToString() + " Note : " + drRow["text"].ToString() + " NoteId : " + noteId.ToString());
                                drRow["LogEHRId"] = noteId.ToString();
                                drRow["Log_Status"] = "completed";
                            }
                        }
                    }
                    if (dtResultCopy.Rows.Count > 0)
                    {
                        if (dtResultCopy.Select("LogType = 0").Count() > 0)
                        {
                            SynchLocalDAL.CallPatientSMSCallAPIForStatusCompleted(dtResultCopy.Copy().Select("LogType = 0").CopyToDataTable(), "completed", ServiceInstalltionId.Trim(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                        }
                        if (dtResultCopy.Select("LogType = 1").Count() > 0)
                        {
                            SynchLocalDAL.CallPatientFollowUpStatusCompleted(dtResultCopy.Copy().Select("LogType = 1").CopyToDataTable(), "completed", ServiceInstalltionId.Trim(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                        }
                    }
                }
                return noteId;
            }
            catch (Exception ex)
            {
                return "";
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            //return is_Record_Update;
        }

        public static DataTable GetDuplicateRecords(string DbConnectionString, string ServiceInstalltionId)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLConnectionServer(ref conn, DbConnectionString);

            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = "";
                MySqlSelect = SynchOpenDentalQRY.GetDuplicateRecords;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
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


        public static void DeleteDuplicatePatientLog(string DbConnectionString, string ServiceInstalltionId)
        {
            //  bool is_Record_Update = false;
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = null;
            CommonDB.MySQLConnectionServer(ref conn, DbConnectionString);
            DateTime datetimeTemp = DateTime.Now;
            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {
                string sqlSelect = string.Empty;

                DataTable dtDuplicateRecords = GetDuplicateRecords(DbConnectionString, ServiceInstalltionId);
                foreach (DataRow drRow in dtDuplicateRecords.Rows)
                {

                    try
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.MySqlCommandServer("", conn, ref MySqlCommand, "txt");
                        if (drRow["Mobile"].ToString() == "")
                        {
                            MySqlCommand.CommandText = SynchOpenDentalQRY.DeleteDuplicateLogsBlankMobileWithDate;
                        }
                        else
                        {
                            MySqlCommand.CommandText = SynchOpenDentalQRY.DeleteDuplicateLogsWithDate;
                        }

                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.AddWithValue("PatientEHRId", drRow["PatNum"].ToString());
                        datetimeTemp = Convert.ToDateTime(drRow["commdatetime"].ToString());

                        MySqlCommand.Parameters.AddWithValue("commdatetime", Convert.ToDateTime(datetimeTemp.ToShortDateString() + " " + datetimeTemp.Hour.ToString() + ":00:00"));
                        MySqlCommand.Parameters.AddWithValue("CommTyep", 239);
                        MySqlCommand.Parameters.AddWithValue("Mode", drRow["Mode_"].ToString());
                        MySqlCommand.Parameters.AddWithValue("Note", drRow["Note"].ToString());
                        MySqlCommand.Parameters.AddWithValue("SentOrReceive", drRow["SentorReceived"].ToString());
                        // MySqlCommand.Parameters.AddWithValue("UserNum", 1);
                        MySqlCommand.Parameters.AddWithValue("NoteId", drRow["logId"].ToString());
                        MySqlCommand.ExecuteNonQuery();

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

        public static bool SavePatientMedication(string DbString, string Service_Install_Id, ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            Int64 MedicationPatientId = 0;
            Int64 MedicationNum = 0;
            DataTable dtMedication;
            DataTable dtPatientMedication;
            SavePatientEHRID = "";
            try
            {
                DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationResponseToSaveINEHR(Service_Install_Id, strPatientFormID);
                if (dtPatientMedicationResponse != null)
                {
                    if (dtPatientMedicationResponse.Rows.Count > 0)
                    {
                        dtMedication = GetOpenDentalMedicationData(DbString, Service_Install_Id);
                        dtPatientMedication = GetOpenDentalPatientMedicationData(DbString, Service_Install_Id, "");
                        foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                        {
                            MedicationNum = 0;
                            MedicationPatientId = 0;
                            CommonDB.MySQLConnectionServer(ref conn, DbString);
                            string sqlSelect = string.Empty;
                            CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                            CheckConnection(conn);
                            if (conn.State == ConnectionState.Closed) conn.Open();

                            if (dr["Medication_EHR_Id"] == DBNull.Value) dr["Medication_EHR_Id"] = "";

                            if (dr["Medication_EHR_Id"].ToString().Trim() == "" || dr["Medication_EHR_Id"].ToString().Trim() == "0")
                            {
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

                                    //MySqlCommand.CommandText = SynchOpenDentalQRY.InsertMedication;
                                    //MySqlCommand.Parameters.Clear();
                                    ////@MedicationName,@GenericNum,@MedicationNote
                                    //MySqlCommand.Parameters.AddWithValue("MedicationName", dr["Medication_Name"].ToString().Trim());
                                    //MySqlCommand.Parameters.AddWithValue("GenericNum", 0);
                                    //MySqlCommand.Parameters.AddWithValue("MedicationNote", dr["Medication_Note"].ToString().Trim());
                                    //MySqlCommand.ExecuteNonQuery();

                                    //MySqlCommand.CommandText = "Select @@Identity as newId from medication;";
                                    //MySqlCommand.CommandType = CommandType.Text;
                                    //MySqlCommand.Connection = conn;
                                    //MedicationNum = (Int64)MySqlCommand.ExecuteScalar();

                                    //string qUpdateGenericNum = "Update medication set GenericNum = @GenericNum where MedicationNum = @MedicationNum;";
                                    //MySqlCommand.CommandText = qUpdateGenericNum;
                                    //MySqlCommand.CommandType = CommandType.Text;
                                    //MySqlCommand.Connection = conn;
                                    //MySqlCommand.Parameters.Clear();
                                    //MySqlCommand.Parameters.AddWithValue("GenericNum", MedicationNum);
                                    //MySqlCommand.Parameters.AddWithValue("MedicationNum", MedicationNum);
                                    //MySqlCommand.ExecuteNonQuery();

                                    //DataRow newMedRow = dtMedication.NewRow();
                                    //newMedRow["Medication_EHR_ID"] = MedicationNum.ToString();
                                    //newMedRow["Medication_Name"] = dr["Medication_Name"].ToString().Trim();
                                    //newMedRow["Medication_Description"] = "";
                                    //newMedRow["Medication_Notes"] = dr["Medication_Note"].ToString().Trim();
                                    //newMedRow["Medication_Sig"] = "";
                                    //newMedRow["Medication_Parent_EHR_ID"] = "";
                                    //newMedRow["Medication_Type"] = "";
                                    //newMedRow["Allow_Generic_Sub"] = "";
                                    //newMedRow["Drug_Quantity"] = "";
                                    //newMedRow["Refills"] = "";
                                    //newMedRow["Is_Active"] = "";
                                    //newMedRow["EHR_Entry_DateTime"] = "";
                                    //newMedRow["Medication_Provider_ID"] = "";
                                    //newMedRow["is_deleted"] = "";
                                    //newMedRow["Is_Adit_Updated"] = "";
                                    //newMedRow["Clinic_Number"] = "";
                                    //dtMedication.Rows.Add(dtMedication);
                                    //dtMedication.AcceptChanges();
                                }
                                dr["Medication_EHR_Id"] = MedicationNum.ToString();
                            }
                            else
                            {
                                MedicationNum = Convert.ToInt64(dr["Medication_EHR_Id"].ToString());
                            }

                            if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "0";
                            if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" && dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                            {
                                MedicationPatientId = Convert.ToInt64(dr["PatientMedication_EHR_Id"]);
                            }
                            if (MedicationPatientId <= 0)
                            {
                                string strSelect = "Patient_EHR_ID = " + dr["PatientEHRID"].ToString().Trim() +
                                            " And Medication_EHR_ID = " + MedicationNum.ToString().Trim() + " And is_active='True' ";

                                DataRow[] drPatMedRow = dtPatientMedication.Copy().Select(strSelect);
                                if (drPatMedRow.Length > 0)
                                {
                                    MedicationPatientId = Convert.ToInt64(drPatMedRow[0]["PatientMedication_EHR_ID"].ToString().Trim());
                                }
                            }

                            if (MedicationPatientId <= 0)
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.InsertPatientMedication;
                                //@PatNum,@MedicationNum,@PatNote,@ProvNum
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("MedicationNum", dr["Medication_EHR_Id"].ToString());
                                MySqlCommand.Parameters.AddWithValue("PatNum", dr["PatientEHRID"].ToString());
                                MySqlCommand.Parameters.AddWithValue("PatNote", dr["Medication_Note"].ToString());
                                //MySqlCommand.Parameters.AddWithValue("ProvNum", ProviderID);

                                MySqlCommand.ExecuteNonQuery();

                                string QryIdentity = "Select @@Identity as newId from medicationpat;";
                                MySqlCommand.CommandText = QryIdentity;
                                MySqlCommand.CommandType = CommandType.Text;
                                MySqlCommand.Connection = conn;
                                MedicationPatientId = Convert.ToInt64(MySqlCommand.ExecuteScalar());
                                dr["PatientMedication_EHR_ID"] = MedicationPatientId.ToString().Trim();

                                DataRow newPatMedRow = dtPatientMedication.NewRow();
                                newPatMedRow["Patient_EHR_ID"] = dr["PatientEHRID"].ToString();
                                newPatMedRow["Medication_EHR_ID"] = dr["Medication_EHR_Id"].ToString();
                                newPatMedRow["PatientMedication_EHR_ID"] = MedicationPatientId.ToString();
                                newPatMedRow["Drug_Quantity"] = "";
                                newPatMedRow["Medication_Name"] = dr["Medication_Name"].ToString();
                                newPatMedRow["Medication_Type"] = "";
                                newPatMedRow["Medication_Note"] = dr["Medication_Note"].ToString();
                                newPatMedRow["Provider_EHR_ID"] = 0;
                                newPatMedRow["Start_Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                newPatMedRow["End_Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                newPatMedRow["EHR_Entry_DateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                newPatMedRow["is_deleted"] = "0";
                                newPatMedRow["Clinic_Number"] = "0";
                                newPatMedRow["is_active"] = "True";
                                dtPatientMedication.Rows.Add(newPatMedRow);
                                dtPatientMedication.AcceptChanges();
                            }
                            else
                            {
                                MySqlCommand.CommandText = SynchOpenDentalQRY.UpdatePatientMedicationNotes;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("PatientMedication_EHR_ID", MedicationPatientId);
                                MySqlCommand.Parameters.AddWithValue("Medication_Note", dr["Medication_Note"].ToString());
                                MySqlCommand.Parameters.AddWithValue("MedicationNum", dr["Medication_EHR_Id"].ToString());
                                MySqlCommand.ExecuteNonQuery();
                            }
                            if (!SavePatientEHRID.Contains("'" + dr["PatientEHRID"].ToString() + "'"))
                            {
                                SavePatientEHRID = SavePatientEHRID + "'" + dr["PatientEHRID"].ToString() + "',";
                            }
                            SynchLocalDAL.UpdateMedicationEHR_Updateflg(dr["MedicationResponse_Local_ID"].ToString(), MedicationPatientId.ToString(), dr["PatientEHRID"].ToString(), dr["Medication_EHR_Id"].ToString(), Service_Install_Id);
                        }
                        isRecordSaved = true;
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
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool DeletePatientMedication(string DbString, string Service_Install_Id, ref bool isRecordDeleted, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            CommonDB.MySQLConnectionServer(ref conn, DbString);
            DeletePatientEHRID = "";
            try
            {
                DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationRemovedResponseToSaveINEHR(Service_Install_Id, strPatientFormID);
                if (dtPatientMedicationResponse != null)
                {
                    if (dtPatientMedicationResponse.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                        {
                            CommonDB.MySQLConnectionServer(ref conn, DbString);
                            string sqlSelect = string.Empty;
                            CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                            CheckConnection(conn);
                            if (conn.State == ConnectionState.Closed) conn.Open();

                            if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "";

                            if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" || dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                            {
                                //sqlSelect = SynchOpenDentalQRY.DeletePatientMedication;
                                MySqlCommand.CommandText = SynchOpenDentalQRY.DeletePatientMedication;
                                MySqlCommand.CommandType = CommandType.Text;
                                MySqlCommand.Connection = conn;
                                MySqlCommand.Parameters.Clear();
                                MySqlCommand.Parameters.AddWithValue("PatientMedication_EHR_ID", dr["PatientMedication_EHR_Id"].ToString());
                                //MySqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["PatientEHRID"].ToString());
                                MySqlCommand.ExecuteNonQuery();
                                if (!(DeletePatientEHRID.ToUpper().Trim().Contains("'" + dr["PatientEHRID"].ToString().Trim() + "'")))
                                {
                                    DeletePatientEHRID = DeletePatientEHRID + "'" + dr["PatientEHRID"].ToString() + "',";
                                }
                                SynchLocalDAL.UpdateRemovedMedicationEHR_Updateflg(dr["MedicationRemovedResponse_Local_ID"].ToString(), dr["PatientMedication_EHR_Id"].ToString(), dr["PatientEHRID"].ToString(), Service_Install_Id);
                            }
                        }
                        isRecordDeleted = true;
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
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_Patient_OpenDental_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id)
        {
            bool _successfullstataus = true;
           SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtOpenDentalDataToSave, Clinic_Number, Service_Install_Id);
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
                        foreach (DataRow dr in dtOpenDentalDataToSave.Rows)
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
                                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
                                    {
                                        //Insert
                                        if (found == false)
                                        {
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
                                            rec.SetValue(rs.GetOrdinal("remaining_benefit"), Math.Round(double.Parse(dr["remaining_benefit"].ToString().Trim()), 2));
                                            rec.SetValue(rs.GetOrdinal("used_benefit"), Math.Round(double.Parse(dr["used_benefit"].ToString().Trim()), 2));
                                            rec.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rec.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rec.SetValue(rs.GetOrdinal("Clinic_Number"), dr["Clinic_Number"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Service_Install_Id"), Service_Install_Id);
                                            bool isDeleted = false;
                                            try
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
                                                    Utility.WriteToErrorLogFromAll("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX: Patient: " + dr["Patient_EHR_ID"].ToString().Trim() + ", Clinic_Number:" + Clinic_Number + ", Service_Installed_ID:" + Service_Install_Id);
                                                    continue;
                                                }
                                                else
                                                {
                                                    throw exduplicate;
                                                }
                                            }
                                        }
                                    }
                                    else if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 2 || Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                    {
                                        //Update, Delete
                                        if (found == true)
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
                                            bool isDeleted = false;
                                            try
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

       
    }
}
