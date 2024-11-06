using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pozative.BAL;
using Pozative.UTL;
using Pozative.DAL;
using Pozative.BO;
using System.Security;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.IO;

namespace Pozative
{
    public partial class frmPozative
    {

        #region Appointment
        public static bool CreateAppointmentFromEvent(string strContent, string Service_Install_ID, string Clinic_Number)
        {
            bool flag = false;
            try
            {
                if (!strContent.StartsWith("{") && strContent.StartsWith("data:"))
                {
                    strContent = strContent.Substring(strContent.IndexOf("{"), strContent.Length - strContent.IndexOf("{"));
                }
                var AppointmentDto = Newtonsoft.Json.JsonConvert.DeserializeObject<Pull_AppointmentBO>(strContent);
                if (AppointmentDto != null && AppointmentDto.data != null && AppointmentDto.data.Count > 0)
                {
                    var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
                    string Location_ID = LocID[0]["Location_ID"].ToString();
                    SendAcknowledgement("create_appointment", AppointmentDto.data[0]._id, Location_ID);

                    //DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                    DataTable dtapptStatus = SynchLocalBAL.GetLocalAppointmentStatusData(Service_Install_ID);
                    DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData_AllRecords(Service_Install_ID);
                    DataTable dtLiveAppointment = dtLocalAppointment.Clone();
                    dtLiveAppointment.Columns.Add("InsUptDlt", typeof(int));
                    string Appt_Operatory_Web_Id = string.Empty;
                    string Appt_Operatory_EHR_Id = string.Empty;
                    string Appt_Operatory = string.Empty;
                    string Appt_Status_EHR_Id = string.Empty;
                    string Appt_Status = string.Empty;
                    string Appt_Provider_Web_Id = string.Empty;
                    string Appt_Provider_EHR_Id = string.Empty;
                    string Appt_Providers = string.Empty;
                    int cur_Appt_EHR_ID = 0;

                    foreach (var item in AppointmentDto.data)
                    {
                        Appt_Operatory = string.Empty;
                        if (item.operatory.Count > 0)
                        {
                            Appt_Operatory = item.operatory[0].name.ToString();
                        }
                        Appt_Operatory_EHR_Id = string.Empty;
                        if (item.operatory != null)
                        {
                            for (int i = 0; i < item.operatory.Count; i++)
                            {
                                Appt_Operatory_EHR_Id = Appt_Operatory_EHR_Id + item.operatory[i].Operatory_EHR_ID.ToString() + ";";
                            }
                            if (Appt_Operatory_EHR_Id.Length > 0)
                            {
                                Appt_Operatory_EHR_Id = Appt_Operatory_EHR_Id.Substring(0, Appt_Operatory_EHR_Id.Length - 1);
                            }
                        }

                        Appt_Provider_EHR_Id = "0";
                        if (item.provider != null)
                        {
                            if (item.provider.Count > 0)
                            {
                                Appt_Provider_EHR_Id = item.provider[0].Provider_EHR_ID.ToString();
                            }
                        }

                        Appt_Providers = string.Empty;
                        if (item.provider != null)
                        {
                            for (int i = 0; i < item.provider.Count; i++)
                            {
                                Appt_Providers = Appt_Providers + item.provider[i].display_name.ToString() + ";";
                            }
                            if (Appt_Providers.Length > 0)
                            {
                                Appt_Providers = Appt_Providers.Substring(0, Appt_Providers.Length - 1);
                            }
                        }

                        cur_Appt_EHR_ID = 0;
                        try
                        {
                            cur_Appt_EHR_ID = Int32.Parse(item.appt_ehr_id);
                        }
                        catch (Exception)
                        {
                            cur_Appt_EHR_ID = 0;
                        }

                        DateTime curAppt_DateTime = Utility.Datetimesetting();
                        try
                        {
                            //curAppt_DateTime = DateTime.Parse(item.shedule_time.ToString());
                            curAppt_DateTime = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(item.shedule_time.ToString()));
                        }
                        catch (Exception)
                        {
                            curAppt_DateTime = Utility.Datetimesetting();
                        }

                        DateTime curAppt_EndDateTime = Utility.Datetimesetting();
                        try
                        {
                            //curAppt_DateTime = DateTime.Parse(item.shedule_time.ToString());
                            curAppt_EndDateTime = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(item.end_time.ToString()));
                        }
                        catch (Exception)
                        {
                            curAppt_EndDateTime = curAppt_DateTime.AddMinutes(30);
                        }
                        if (curAppt_DateTime > curAppt_EndDateTime)
                        {
                            curAppt_EndDateTime = curAppt_DateTime.AddMinutes(10);
                        }
                        string patBirthDate = "";
                        try
                        {
                            patBirthDate = DateTime.Parse(Utility.ConvertApptPatientBirthdateToCurrentLocationFormat(item.birth_date.ToString())).ToString();
                        }
                        catch (Exception)
                        {
                            patBirthDate = "";
                        }
                        string ApptTypeEHRId = string.Empty;
                        string ApptTypeName = string.Empty;
                        try
                        {
                            //*Shruti  Need to verify
                            if (item.appointmenttype.apptype_ehr_id != null)
                            {
                                ApptTypeEHRId = item.appointmenttype.apptype_ehr_id.ToString();
                            }
                            else
                            {
                                ApptTypeEHRId = "0";
                            }
                            if (item.appointmenttype.name != null)
                            {
                                ApptTypeName = item.appointmenttype.name.ToString();
                            }
                            else
                            {
                                ApptTypeName = "none";
                            }

                        }
                        catch (Exception)
                        {
                            ApptTypeEHRId = "0";
                            ApptTypeName = "none";
                        }
                        DataRow RowAppt = dtLiveAppointment.NewRow();

                        RowAppt["Appt_EHR_ID"] = cur_Appt_EHR_ID;
                        if (item.patient_ehr_id.ToString() != "-" || item.patient_ehr_id.ToString() != "")
                        {
                            RowAppt["patient_ehr_id"] = item.patient_ehr_id;
                        }
                        else
                        {
                            RowAppt["patient_ehr_id"] = "0";
                        }
                        RowAppt["Appt_Web_ID"] = item._id;
                        RowAppt["Last_Name"] = item.last_name;
                        RowAppt["First_Name"] = item.first_name;
                        RowAppt["MI"] = string.Empty;
                        RowAppt["birth_date"] = patBirthDate;
                        RowAppt["Home_Contact"] = string.Empty;
                        RowAppt["Mobile_Contact"] = item.mobile;
                        RowAppt["Email"] = item.email;
                        RowAppt["Address"] = string.Empty;
                        RowAppt["City"] = string.Empty;
                        RowAppt["ST"] = string.Empty;
                        RowAppt["Zip"] = string.Empty;
                        if (Appt_Operatory_EHR_Id == null || Appt_Operatory_EHR_Id.ToString() == string.Empty)
                        {
                            //DataTable dtoperatory = SynchLocalBAL.GetLocalOperatoryData(Service_Install_ID, Clinic_Number);
                            DataTable dtoperatory = SynchLocalBAL.GetLocalOperatoryData(Service_Install_ID);
                            if (dtoperatory.Rows.Count > 0)
                            {
                                Appt_Operatory_EHR_Id = dtoperatory.Rows[0]["Operatory_EHR_ID"].ToString();
                            }
                        }
                        RowAppt["Operatory_EHR_ID"] = Appt_Operatory_EHR_Id;
                        RowAppt["Operatory_Name"] = Appt_Operatory;
                        RowAppt["Provider_EHR_ID"] = Appt_Provider_EHR_Id;
                        RowAppt["Provider_Name"] = Appt_Providers;
                        RowAppt["ApptType_EHR_ID"] = ApptTypeEHRId;
                        RowAppt["ApptType"] = ApptTypeName;
                        RowAppt["Appt_DateTime"] = curAppt_DateTime;
                        RowAppt["Appt_EndDateTime"] = curAppt_EndDateTime;
                        RowAppt["comment"] = item.comment;
                        RowAppt["Status"] = "0";
                        RowAppt["Patient_Status"] = item.patient_status;
                        if (item.is_appointment.ToString().ToLower() == "pa")
                        {

                            try
                            {
                                if (item.appointment_status != null && item.appointment_status.ToString() != string.Empty)
                                {
                                    DataRow drapptstatus = dtapptStatus.Select("ApptStatus_Name = '" + item.appointment_status + "'").FirstOrDefault();
                                    if (drapptstatus != null)
                                    {
                                        Appt_Status_EHR_Id = drapptstatus["ApptStatus_EHR_ID"].ToString();
                                        Appt_Status = item.appointment_status;
                                    }
                                }
                                else
                                {
                                    Appt_Status = "pending";
                                    Appt_Status_EHR_Id = "0";
                                }
                            }
                            catch (Exception)
                            {
                                Appt_Status = "pending";
                                Appt_Status_EHR_Id = "0";
                            }
                            try
                            {
                                if (item.patient_ehr_id != null && item.patient_ehr_id.ToString() != string.Empty)
                                {
                                    RowAppt["patient_ehr_id"] = item.patient_ehr_id;
                                }
                                else
                                {
                                    RowAppt["patient_ehr_id"] = "";
                                }
                            }
                            catch (Exception)
                            {
                                RowAppt["patient_ehr_id"] = "";
                            }
                            RowAppt["appointment_status_ehr_key"] = Appt_Status_EHR_Id;
                            RowAppt["Appointment_Status"] = Appt_Status; // item.appointment_status;
                        }
                        else
                        {
                            switch (Utility.Application_ID)
                            {
                                case 1: // Eaglesoft
                                    RowAppt["appointment_status_ehr_key"] = "0";
                                    RowAppt["Appointment_Status"] = "pending"; // item.appointment_status;  
                                    break;
                                case 2: // opendental
                                    RowAppt["appointment_status_ehr_key"] = "0";
                                    RowAppt["Appointment_Status"] = "pending"; // item.appointment_status;  
                                    break;
                                case 3: // Dentrix
                                    RowAppt["appointment_status_ehr_key"] = "0";
                                    RowAppt["Appointment_Status"] = "pending"; // item.appointment_status;  
                                    break;
                                case 5: // Cleardent
                                    RowAppt["appointment_status_ehr_key"] = "1";
                                    RowAppt["Appointment_Status"] = "Booked"; // item.appointment_status;  
                                    break;
                                case 6: // Tracker
                                    RowAppt["appointment_status_ehr_key"] = "1";
                                    RowAppt["Appointment_Status"] = "Booked"; // item.appointment_status;  
                                    break;
                                //case 8: // EzDental
                                //    RowAppt["appointment_status_ehr_key"] = "0";
                                //    RowAppt["Appointment_Status"] = "pending"; // item.appointment_status;  
                                //    break;
                                case 7: // PracticeWork 
                                    RowAppt["appointment_status_ehr_key"] = "0";
                                    RowAppt["Appointment_Status"] = "pending"; // item.appointment_status;  
                                    break;
                                case 10: //PracticeWeb
                                    RowAppt["appointment_status_ehr_key"] = "0";
                                    RowAppt["Appointment_Status"] = "pending";
                                    break;
                            }
                        }
                        RowAppt["Is_Appt"] = item.is_appointment;
                        RowAppt["appt_treatmentcode"] = item.appt_treatmentcode;
                        RowAppt["Clinic_Number"] = Clinic_Number;
                        RowAppt["Service_Install_Id"] = Service_Install_ID;
                        RowAppt["confirmed_status_ehr_key"] = item.confirmed_status_ehr_key;
                        RowAppt["confirmed_status"] = item.confirmed_status;
                        dtLiveAppointment.Rows.Add(RowAppt);
                        dtLiveAppointment.AcceptChanges();
                    }
                    //ObjGoalBase.WriteToErrorLogFile("Pull Appointment Loop END ");

                    foreach (DataRow dtDtxRow in dtLiveAppointment.Rows)
                    {
                        DataRow[] row = dtLocalAppointment.Select("Appt_Web_ID = '" + dtDtxRow["Appt_Web_ID"].ToString() + "' And  Clinic_Number = '" + Clinic_Number + "' ");
                        if (row.Length > 0)
                        {
                            if (row[0]["Appt_EHR_ID"].ToString() == "0" && Convert.ToBoolean(row[0]["Is_Appt_DoubleBook"].ToString()) == false)
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 7;
                            }
                        }
                        else
                        {
                            dtDtxRow["InsUptDlt"] = 1;
                        }
                    }
                    dtLiveAppointment.AcceptChanges();
                    PullLiveDatabaseBAL.Save_Appointment_Live_To_Local(dtLiveAppointment, Utility._filename_appointment, Utility._EHRLogdirectory_appointment);
                    if (dtLiveAppointment != null && dtLiveAppointment.Rows.Count > 0)
                    {
                        if (Utility.Application_ID == 1)
                        {
                            if (Utility.ISEagelsoftConnected)
                            {
                                //flag = Sync_EagleSoft.SynchDataLocalToEagleSoft_Appointment(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                                flag = SynchDataLocalToEagleSoft_AppointmentFromEvent(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                            }
                            else
                            {
                                Utility.WritetoAditEventErrorLogFile_Static("EagleSoft is not connected.");
                            }
                        }
                        else if (Utility.Application_ID == 2)
                        {
                            //flag = Sync_OpenDental.SynchDataOpenDental_AppointmentFromEvent(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                            flag = SynchDataOpenDental_AppointmentFromEvent(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                        }
                        else if (Utility.Application_ID == 3)
                        {
                            //flag = Sync_Dentrix.SynchDataDentrix_AppointmentFromEvent(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                            flag = SynchDataDentrix_AppointmentFromEvent(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                        }
                        else if (Utility.Application_ID == 5)
                        {
                            //flag = Sync_ClearDent.SynchDataLocalToClearDent_Appointment(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                            flag = SynchDataLocalToClearDent_AppointmentFromEvent(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                        }

                        else if (Utility.Application_ID == 6)
                        {
                            //flag = Sync_Tracker.SynchDataLocalToTracker_Appointment(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                            flag = SynchDataLocalToTracker_AppointmentFromEvent(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                        }
                        else if (Utility.Application_ID == 7)
                        {
                            //flag = Sync_PracticeWork.SynchDataLocalToPracticeWork_Appointment(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                            flag = SynchDataLocalToPracticeWork_Appointment(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                        }
                        else if (Utility.Application_ID == 10)
                        {
                            //flag = Sync_PracticeWeb.SynchDataLocalToPracticeWeb_Appointment(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                            flag = SynchDataLocalToPracticeWeb_Appointment(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                        }
                        else if (Utility.Application_ID == 11)
                        {
                            //flag = Sync_AbelDent.SynchDataLocalToAbelDent_Appointment(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                            flag = SynchDataLocalToAbelDent_Appointment(dtLiveAppointment, Clinic_Number, Service_Install_ID);
                        }
                        if (flag == false)
                        {
                            Utility.WritetoAditEventSyncLogFile_Static("Appointment Event (Adit Server to Local Database) Can not boook appointment for : " + System.Environment.NewLine
                                + "First Name: " + dtLiveAppointment.Rows[0]["First_Name"] + System.Environment.NewLine
                                + "Last Name: " + dtLiveAppointment.Rows[0]["Last_Name"] + System.Environment.NewLine
                                + "Appointment DateTime: " + dtLiveAppointment.Rows[0]["Appt_DateTime"] + System.Environment.NewLine
                                + "Clinic Number: " + Clinic_Number + System.Environment.NewLine
                                + "Service Install ID: " + Service_Install_ID);
                        }
                    }
                    else
                    {
                        Utility.WritetoAditEventSyncLogFile_Static("Appointment Event (Adit Server To Local Database).. No Records Found. Service Install Id : " + Service_Install_ID + " And Clinic : " + Clinic_Number + " Successfully.");
                    }
                }
                else
                {
                    Utility.WritetoAditEventSyncLogFile_Static("Appointment Event (Adit Server To Local Database).. No Records Found. Service Install Id : " + Service_Install_ID + " And Clinic : " + Clinic_Number + " Successfully.");
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("Appointment Event (Adit Server To Local Database) Error in [CreateAppointment] : " + ex.Message);
                flag = false;
            }
            return flag;
        }
        #endregion

        #region Payment
        public static bool CreatePaymentFromEvent(string strContent, string Service_Install_ID, string Clinic_Number)
        {
            bool flag = false;

            try
            {
                #region Initialise Table
                DataTable dtLivePatientPaymentLog = new DataTable();
                DataTable dtWebPatientPayment = new DataTable();
                dtWebPatientPayment.Columns.Add("PatientEHRId", typeof(string));
                dtWebPatientPayment.Columns.Add("Patient_Web_ID", typeof(string));
                dtWebPatientPayment.Columns.Add("PatientPaymentWebId", typeof(string));
                dtWebPatientPayment.Columns.Add("ProviderEHRId", typeof(string));
                dtWebPatientPayment.Columns.Add("PaymentDate", typeof(DateTime));
                dtWebPatientPayment.Columns.Add("Amount", typeof(decimal));
                dtWebPatientPayment.Columns.Add("PaymentNote", typeof(string));
                dtWebPatientPayment.Columns.Add("PaymentMode", typeof(string));
                dtWebPatientPayment.Columns.Add("PaymentType", typeof(string));
                dtWebPatientPayment.Columns.Add("template", typeof(string));
                dtWebPatientPayment.Columns.Add("Fees", typeof(decimal));
                dtWebPatientPayment.Columns.Add("Discount", typeof(decimal));
                dtWebPatientPayment.Columns.Add("FirstName", typeof(string));
                dtWebPatientPayment.Columns.Add("LastName", typeof(string));
                dtWebPatientPayment.Columns.Add("Mobile", typeof(string));
                dtWebPatientPayment.Columns.Add("Email", typeof(string));
                dtWebPatientPayment.Columns.Add("EHRSyncPaymentLog", typeof(int));
                dtWebPatientPayment.Columns.Add("Clinic_Number", typeof(string));
                dtWebPatientPayment.Columns.Add("Service_Install_Id", typeof(string));
                dtWebPatientPayment.Columns.Add("PaymentInOut", typeof(string));
                dtWebPatientPayment.Columns.Add("ChequeNumber", typeof(string));
                dtWebPatientPayment.Columns.Add("BankOrBranchName", typeof(string));
                dtWebPatientPayment.Columns.Add("EHRErroLog", typeof(string));
                dtWebPatientPayment.Columns.Add("PaymentUpdatedEHR", typeof(bool));
                dtWebPatientPayment.Columns.Add("PaymentReceivedLocal", typeof(bool));
                dtWebPatientPayment.Columns.Add("PaymentEntryDatetimeLocal", typeof(DateTime));
                dtWebPatientPayment.Columns.Add("PaymentUpdatedEHRDateTime", typeof(DateTime));
                dtWebPatientPayment.Columns.Add("PaymentStatusCompletedAdit", typeof(bool));
                dtWebPatientPayment.Columns.Add("PaymentStatusCompletedDateTime", typeof(DateTime));
                dtWebPatientPayment.Columns.Add("PaymentEHRId", typeof(string));
                dtWebPatientPayment.Columns.Add("TryInsert", typeof(int));
                dtWebPatientPayment.Columns.Add("PaymentMethod", typeof(string));
                dtWebPatientPayment.Columns.Add("EHRSyncFinancialLogSetting", typeof(int));

                #endregion

                if (!strContent.StartsWith("{") && strContent.StartsWith("data:"))
                {
                    strContent = strContent.Substring(strContent.IndexOf("{"), strContent.Length - strContent.IndexOf("{"));
                    strContent = strContent.Replace("\"data\":{", "\"data\":[{");
                    strContent = strContent.Substring(0, strContent.Length - 1) + "]}";
                }

                var ProvidersDto = Newtonsoft.Json.JsonConvert.DeserializeObject<Pull_PatientPayment>(strContent);
                if (ProvidersDto != null && ProvidersDto.data != null && ProvidersDto.data.Count > 0)
                {
                    var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
                    string Location_ID = LocID[0]["Location_ID"].ToString();
                    SendAcknowledgement("adit_pay", ProvidersDto.data[0].apEhrSyncId, Location_ID);

                    #region Make Rows
                    dtWebPatientPayment.Rows.Clear();
                    foreach (var item in ProvidersDto.data)
                    {
                        if (item.patient_ehr_id != null || item.patient_first_name != null)
                        {
                            if ((item.payment_status.ToString().ToUpper() == "PAID" || item.payment_status.ToString().ToUpper() == "PARTIAL-PAID" || item.payment_status.ToString().ToUpper() == "REFUNDED" || item.payment_status.ToString().ToUpper() == "PARTIAL-REFUNDED"))
                            {
                                DataRow RowPro = dtLivePatientPaymentLog.NewRow();
                                if (item.patient_ehr_id != null && item.patient_ehr_id.ToString() != string.Empty && item.patient_ehr_id.ToString().Trim() != "-")
                                {
                                    DataRow dataRow;
                                    dataRow = dtWebPatientPayment.NewRow();

                                    dataRow["PatientEHRId"] = item.patient_ehr_id.ToString();
                                    dataRow["Patient_Web_ID"] = item.patientId.ToString();
                                    dataRow["ProviderEHRId"] = "";
                                    dataRow["PaymentDate"] = item.created_at.ToString();
                                    if (item.amount != null)
                                    {
                                        dataRow["Amount"] = item.amount.ToString();
                                    }
                                    else
                                    {
                                        dataRow["Amount"] = 0;
                                    }
                                    dataRow["PaymentNote"] = item.template.ToString();
                                    dataRow["PaymentMode"] = item.payment_status.ToString();
                                    dataRow["PaymentType"] = item.payment_type.ToString();
                                    dataRow["template"] = item.template.ToString() + "-[UQID:" + item.apEhrSyncId + "]";  //[UQID:12j3213hk21hk3hk1223j432h4kj23h4] 
                                    if (item.fees != null)
                                    {
                                        dataRow["Fees"] = item.fees.ToString();
                                    }
                                    else
                                    {
                                        dataRow["Fees"] = 0;
                                    }
                                    dataRow["Discount"] = item.discount.ToString();
                                    dataRow["FirstName"] = item.patient_first_name.ToString();
                                    dataRow["LastName"] = item.patient_last_name.ToString();
                                    dataRow["Mobile"] = item.patient_number.ToString();
                                    dataRow["Email"] = item.patient_email.ToString();
                                    dataRow["EHRSyncPaymentLog"] = item.log_setting;
                                    dataRow["Clinic_Number"] = Clinic_Number;
                                    dataRow["Service_Install_Id"] = Service_Install_ID;
                                    dataRow["ChequeNumber"] = "";
                                    dataRow["BankOrBranchName"] = "";
                                    dataRow["PaymentInOut"] = "In";
                                    dataRow["PaymentReceivedLocal"] = 1;
                                    dataRow["PaymentEntryDatetimeLocal"] = DateTime.Now.ToString();
                                    dataRow["PaymentUpdatedEHR"] = 0;
                                    dataRow["PaymentUpdatedEHRDateTime"] = DBNull.Value;
                                    dataRow["PaymentStatusCompletedAdit"] = 0;
                                    dataRow["PaymentStatusCompletedDateTime"] = DBNull.Value;
                                    dataRow["PaymentEHRId"] = "";
                                    dataRow["PatientPaymentWebId"] = item.apEhrSyncId;
                                    dataRow["EHRErroLog"] = "";
                                    dataRow["TryInsert"] = 0;
                                    dataRow["PaymentMethod"] = item.payment_method.ToString();
                                    dataRow["EHRSyncFinancialLogSetting"] =item.financing_log_setting;
                                    dtWebPatientPayment.Rows.Add(dataRow);
                                }
                            }
                        }

                    }
                    #endregion

                    dtWebPatientPayment.AcceptChanges();

                    if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                    {
                        string strDbConnString = "";
                        var dbConStr = Utility.DtInstallServiceList.Select("Installation_Id = '" + Service_Install_ID + "'");
                        strDbConnString = dbConStr[0]["DBConnString"].ToString();
                        // save payment to ehr
                        #region EagleSoft
                        if (Utility.Application_ID == 1)
                        {
                            //save payment to eaglesoft
                            try
                            {
                                long TransactionHeaderID = SynchEaglesoftBAL.SavePatientPaymentTOEHR(strDbConnString, dtWebPatientPayment, Service_Install_ID);
                            }
                            catch (Exception ex)
                            {
                                Utility.WritetoAditEventErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                            }
                        }
                        #endregion

                        #region OpenDental
                        if (Utility.Application_ID == 2)
                        {
                            //save payment to opendental
                            try
                            {
                                bool Is_Record_Update = SynchOpenDentalBAL.Save_PatientPayment_Local_To_OpenDental(dtWebPatientPayment, strDbConnString, Service_Install_ID);
                            }
                            catch (Exception ex)
                            {
                                Utility.WritetoAditEventErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                            }
                        }
                        #endregion

                        #region Dentrix
                        if (Utility.Application_ID == 3)
                        {
                            //save payment to dentrix
                            try
                            {
                                string Guar_ID = "";
                                if (!dtWebPatientPayment.Columns.Contains("Guar_ID"))
                                {
                                    dtWebPatientPayment.Columns.Add("Guar_ID", typeof(string));
                                }
                                foreach (DataRow drRow in dtWebPatientPayment.Rows)
                                {
                                    Guar_ID = SynchDentrixBAL.GetPatGuaridAndProviders(drRow["PatientEHRId"].ToString());
                                    drRow["Guar_ID"] = Guar_ID;
                                }
                                SynchDentrixBAL.SavePatientPaymentTOEHR(dtWebPatientPayment);
                            }
                            catch (Exception ex)
                            {

                                Utility.WritetoAditEventErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                            }
                        }
                        #endregion

                        #region ClearDent
                        if (Utility.Application_ID == 5)
                        {
                            //save payment to cleardent
                            try
                            {
                                SynchClearDentBAL.Save_PatientPayment_Local_To_ClearDent(dtWebPatientPayment, strDbConnString, Service_Install_ID);
                            }
                            catch (Exception ex)
                            {
                                Utility.WritetoAditEventErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                            }

                        }
                        #endregion

                        #region Tracker
                        if (Utility.Application_ID == 6)
                        {
                            //save payment to tracker
                            try
                            {
                                SynchTrackerBAL.SavePatientPaymentTOEHR(strDbConnString, dtWebPatientPayment, Service_Install_ID);
                            }
                            catch (Exception ex)
                            {
                                Utility.WritetoAditEventErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                            }

                        }
                        #endregion

                        #region PracticeWork
                        if (Utility.Application_ID == 7)
                        {
                            //save payment to Practicework
                        }
                        #endregion

                        #region EasyDental
                        if (Utility.Application_ID == 8)
                        {
                            //save payment to easydental
                            try
                            {
                                string Guar_ID = "";
                                if (!dtWebPatientPayment.Columns.Contains("Guar_ID"))
                                {
                                    dtWebPatientPayment.Columns.Add("Guar_ID", typeof(string));
                                }
                                foreach (DataRow drRow in dtWebPatientPayment.Rows)
                                {
                                    Guar_ID = SynchEasyDentalBAL.GetPatientGuarid(drRow["PatientEHRId"].ToString());
                                    drRow["Guar_ID"] = Guar_ID;
                                }
                                SynchEasyDentalBAL.SavePatientPaymentTOEHR(dtWebPatientPayment);
                            }
                            catch (Exception ex)
                            {
                                Utility.WritetoAditEventErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                            }

                        }
                        #endregion

                        #region PracticeWeb
                        if (Utility.Application_ID == 10)
                        {
                            //save payment to practiceweb
                            try
                            {
                                bool Is_Record_Update = SynchPracticeWebBAL.Save_PatientPayment_Local_To_PracticeWeb(dtWebPatientPayment, strDbConnString, Service_Install_ID);
                            }
                            catch (Exception ex)
                            {
                                Utility.WritetoAditEventErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                            }

                        }
                        #endregion

                        #region AbelDent
                        if (Utility.Application_ID == 11)
                        {
                            //save payment to abeldent
                            try
                            {
                                SynchAbelDentBAL.SavePatientPaymentTOEHR(strDbConnString, dtWebPatientPayment, Service_Install_ID);
                            }
                            catch (Exception ex)
                            {
                                Utility.WritetoAditEventErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                            }
                        }
                        #endregion
                    }

                }

                flag = true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("Payment Event (Adit Server To Local Database) Error in [CreatePayment] : " + ex.Message);
                flag = false;
            }
            return flag;
        }
        #endregion

        #region Patient Form
        public static bool Create_Patient_Form_FromEvent(string strContent, string Service_Install_ID, string Clinic_Number)
        {
            string firstname = "", lastname = "";
            DataTable dtEHRPatientData = new DataTable();
            string PatientFormWebID = "", strPatientID = "";
            try
            {
                if (!strContent.StartsWith("{") && strContent.StartsWith("data:"))
                {
                    strContent = strContent.Substring(strContent.IndexOf("{"), strContent.Length - strContent.IndexOf("{"));
                }
                var ApptPatientFormDto = Newtonsoft.Json.JsonConvert.DeserializeObject<Pull_PatientFormBO>(strContent);
                if (ApptPatientFormDto != null && ApptPatientFormDto.data != null)
                {
                    var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
                    string Location_ID = LocID[0]["Location_ID"].ToString();
                    SendAcknowledgement("patient_form", ApptPatientFormDto.data[0]._id, Location_ID);

                    if (ApptPatientFormDto.data != null && ApptPatientFormDto.data.Count() > 0)
                    {
                        //Utility.WriteToSyncLogFile_All("Response Received count " + ApptPatientFormDto.data.Count().ToString());

                        DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPatientFormData(Clinic_Number, Service_Install_ID);
                        dtEHRPatientData.Rows.Clear();
                        DataTable dtEagleSoftInsuranceName = new DataTable();
                        // ObjGoalBase.WriteToSyncLogFile("DONE");
                        //EagleSoft
                        if (Utility.Application_ID == 1)
                        {
                            //dtEHRPatientData = SynchEaglesoftBAL.GetEaglesoftPatientData(Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
                            dtEHRPatientData = SynchEaglesoftBAL.GetEaglesoftPatientData(Utility.GetDataBaseConnectionByServicesInstallId(Service_Install_ID));
                            dtEagleSoftInsuranceName = SynchEaglesoftBAL.GetInsuratnce_CompanyName(Utility.GetDataBaseConnectionByServicesInstallId(Service_Install_ID));
                        }
                        // OpenDental
                        else if (Utility.Application_ID == 2)
                        {
                            dtEHRPatientData = SynchOpenDentalBAL.GetOpenDentalPatientData(Clinic_Number, Utility.GetDataBaseConnectionByServicesInstallId(Service_Install_ID), (Utility.OpenDentalOldPatSync ? false : true));

                        }
                        // Dentrix
                        else if (Utility.Application_ID == 3)
                        {
                            dtEHRPatientData = SynchDentrixBAL.GetDentrixPatientData();
                        }
                        //Softdent
                        else if (Utility.Application_ID == 4)
                        {
                            dtEHRPatientData = SynchLocalBAL.GetLocalPatientData(Service_Install_ID);
                        }
                        //ClearDent
                        else if (Utility.Application_ID == 5)
                        {
                            dtEHRPatientData = SynchClearDentBAL.GetClearDentPatientData();
                        }
                        //Tracker
                        else if (Utility.Application_ID == 6)
                        {
                            dtEHRPatientData = SynchTrackerBAL.GetTrackerPatientData();
                        }
                        // PracticeWork
                        else if (Utility.Application_ID == 7)
                        {
                            dtEHRPatientData = SynchPracticeWorkBAL.GetPracticeWorkPatientData();
                        }
                        //EasyDental
                        else if (Utility.Application_ID == 8)
                        {
                            dtEHRPatientData = SynchEasyDentalBAL.GetEasyDentalPatientData();
                        }
                        else if (Utility.Application_ID == 10)
                        {
                            dtEHRPatientData = SynchPracticeWebBAL.GetPracticeWebPatientData(Clinic_Number, Utility.GetDataBaseConnectionByServicesInstallId(Service_Install_ID));

                        }
                        //AbelDent
                        else if (Utility.Application_ID == 11)
                        {
                            dtEHRPatientData = SynchAbelDentBAL.GetAbelDentPatientData();

                        }
                        if (!dtEHRPatientData.Columns.Contains("Patient_Web_ID"))
                        {
                            dtEHRPatientData.Columns.Add("Patient_Web_ID", typeof(string));
                        }

                        DataTable dtLocalProviderData = SynchLocalBAL.GetLocalProviderData(Clinic_Number, Service_Install_ID);
                        DataTable dtLivePatientForm = dtLocalPatientForm.Clone();
                        dtLivePatientForm.Columns.Add("InsUptDlt", typeof(int));

                        DataTable dtPatientFormDiseaseResponse = SynchLocalBAL.GetLocalPatientFormDiseaseResponse(Clinic_Number, Service_Install_ID);
                        DataTable dtLivePatientFormDiseaseResponse = dtPatientFormDiseaseResponse.Clone();
                        dtLivePatientFormDiseaseResponse.Columns.Add("InsUptDlt", typeof(int));

                        DataTable dtPatientFormDiseaseDeleteResponse = SynchLocalBAL.GetLocalPatientFormDiseaseDeleteResponse(Clinic_Number, Service_Install_ID);
                        DataTable dtLivePatientFormDiseaseDeleteResponse = dtPatientFormDiseaseDeleteResponse.Clone();
                        dtLivePatientFormDiseaseDeleteResponse.Columns.Add("InsUptDlt", typeof(int));
                        //   Utility.WriteToSyncLogFile_All("Data Received " + ApptPatientFormDto.data.ToString());

                        DataTable dtPatientFormMedicationResponse = SynchLocalBAL.GetLocalPatientFormMedicationResponse(Clinic_Number, Service_Install_ID);
                        DataTable dtLivePatientFormMedicationResponse = dtPatientFormMedicationResponse.Clone();
                        dtLivePatientFormMedicationResponse.Columns.Add("InsUptDlt", typeof(int));

                        DataTable dtPatientFormMedicationRemovedResponse = SynchLocalBAL.GetLocalPatientFormMedicationRemovedResponse(Clinic_Number, Service_Install_ID);
                        DataTable dtLivePatientFormMedicationRemovedResponse = dtPatientFormMedicationRemovedResponse.Clone();
                        dtLivePatientFormMedicationRemovedResponse.Columns.Add("InsUptDlt", typeof(int));

                        int folder_ehr_id = 0;

                        string folder_name = "";
                        string DocNameFormat = "";
                        string Form_Name = "";
                        string Patient_Name = "";
                        DateTime submit_time = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(DateTime.Now.ToString()));
                        foreach (var item in ApptPatientFormDto.data)
                        {
                            try
                            {
                                folder_ehr_id = item.folder_ehr_id == "" ? 0 : Convert.ToInt32(item.folder_ehr_id);
                                folder_name = item.folder_name;
                                DocNameFormat = item.form_name_format;
                                Form_Name = item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name;
                                Patient_Name = item.patient_name;
                                submit_time = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(item.submit_time.ToString()));
                            }
                            catch (Exception dp)
                            {
                                Utility.WriteToErrorLogFromAll("Error Getting in PatientForm DOCAttachment info done by dipika " + dp.Message.ToString());
                            }
                            if (item.ehrmap != null && item.ehrmap.Count() > 0)
                            {
                                foreach (var subitem in item.ehrmap)
                                {
                                    lastname = ""; firstname = "";
                                    if (subitem.ehrField.ToString().Trim().ToUpper() == "FIRST_NAME" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "LAST_NAME" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "MOBILE" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "ADDRESS_ONE" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "ADDRESS_TWO" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "BIRTH_DATE" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "CITY" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "EMAIL" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "HOME_PHONE" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "MARITAL_STATUS" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "MIDDLE_NAME" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "PREFERRED_NAME" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "PRI_PROVIDER_ID" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "RECEIVE_EMAIL" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "RECEIVE_SMS" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "SALUTATION" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "SEC_PROVIDER_ID" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "SEX" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "WORK_PHONE" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "STATE" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "ZIPCODE" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "SSN" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "SCHOOL" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "EMPLOYER" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "DRIVERLICENSE" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "EMERGENCYCONTACTNAME" ||
                                        subitem.ehrField.ToString().Trim().ToUpper() == "EMERGENCYCONTACTNUMBER")
                                    {
                                        if (subitem.value.ToString() != "")
                                        {
                                            DataRow RowPatientForm = dtLivePatientForm.NewRow();
                                            if (item.pinfo != null)
                                            {
                                                RowPatientForm["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                                RowPatientForm["Patient_Web_ID"] = item.pinfo._id;
                                            }
                                            RowPatientForm["PatientForm_Web_ID"] = item._id;
                                            try
                                            {
                                                AddinfoinForm(ref RowPatientForm, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);
                                            }
                                            catch (Exception)
                                            {

                                            }

                                            //RowPatientForm["submit_time"] = Convert.ToDateTime(item.submit_time).ToString("MM/dd/yyyy");
                                            RowPatientForm["ehrfield"] = subitem.ehrField;
                                            if ((RowPatientForm["ehrfield"].ToString().ToLower() == "primary_insurance_companyname" || RowPatientForm["ehrfield"].ToString().ToLower() == "secondary_insurance_companyname") && Utility.Application_ID == 1 && Utility.Application_ID == 8)
                                            {
                                                RowPatientForm["ehrfield_value"] = subitem.value.ToString();
                                            }
                                            else
                                            {
                                                RowPatientForm["ehrfield_value"] = subitem.value.ToString().Replace("'", "");
                                            }

                                            try
                                            {
                                                if (RowPatientForm["ehrfield"].ToString().ToLower() == "birth_date")
                                                {
                                                    RowPatientForm["ehrfield_value"] = Convert.ToDateTime(Utility.CheckValidDatetime(RowPatientForm["ehrfield_value"].ToString())).ToString("yyyy/MM/dd");
                                                }
                                            }
                                            catch (Exception)
                                            {

                                            }
                                            RowPatientForm["Clinic_Number"] = Clinic_Number;
                                            RowPatientForm["Service_Install_Id"] = Service_Install_ID;
                                            AddinfoinForm(ref RowPatientForm, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);

                                            dtLivePatientForm.Rows.Add(RowPatientForm);
                                            dtLivePatientForm.AcceptChanges();
                                        }
                                    }
                                    // Utility.WriteToSyncLogFile_All("Set Allergies");
                                    if (subitem.Alg_Prb_value != null)
                                    {
                                        foreach (var Allergy in subitem.Alg_Prb_value)
                                        {
                                            DataRow drRepDiese = dtLivePatientFormDiseaseResponse.NewRow();
                                            drRepDiese["DiseaseMaster_Web_ID"] = Allergy._id.ToString();
                                            drRepDiese["PatientForm_Web_ID"] = item._id.ToString();
                                            if (item.pinfo != null)
                                            {
                                                drRepDiese["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                            }
                                            else
                                            {
                                                drRepDiese["Patient_EHR_ID"] = "";
                                            }
                                            drRepDiese["Disease_EHR_Id"] = Allergy.disease_ehr_id.ToString();
                                            drRepDiese["Disease_Type"] = Allergy.disease_type.ToString();
                                            drRepDiese["Name"] = Allergy.name.ToString();
                                            drRepDiese["Clinic_Number"] = Clinic_Number;
                                            drRepDiese["Service_Install_Id"] = Service_Install_ID;
                                            dtLivePatientFormDiseaseResponse.Rows.Add(drRepDiese);
                                        }
                                    }
                                    if (subitem.Removed_Alg_Prb_value != null)
                                    {
                                        foreach (var Allergy in subitem.Removed_Alg_Prb_value)
                                        {
                                            DataRow drRepDiese = dtLivePatientFormDiseaseDeleteResponse.NewRow();
                                            //drRepDiese["DiseaseDeleteResponse_Web_ID"] = Allergy._id.ToString();
                                            drRepDiese["PatientForm_Web_ID"] = item._id.ToString();
                                            drRepDiese["Patient_EHR_ID"] = Allergy.patient_ehr_id.ToString();
                                            drRepDiese["Disease_EHR_Id"] = Allergy.disease_ehr_id.ToString();
                                            drRepDiese["Disease_Type"] = Allergy.disease_type.ToString();
                                            drRepDiese["Clinic_Number"] = Clinic_Number;
                                            drRepDiese["Service_Install_Id"] = Service_Install_ID;
                                            dtLivePatientFormDiseaseDeleteResponse.Rows.Add(drRepDiese);
                                        }
                                    }
                                    // Utility.WriteToSyncLogFile_All("Set Allergies Completed");

                                    if (subitem.Removed_Medication_value != null)
                                    {
                                        foreach (var RemovedMedValue in subitem.Removed_Medication_value)
                                        {
                                            DataRow drMedicationRemoved = dtLivePatientFormMedicationRemovedResponse.NewRow();
                                            drMedicationRemoved["PatientForm_Web_ID"] = item._id.ToString();
                                            drMedicationRemoved["Patient_EHR_ID"] = item.pinfo == null ? "" : item.pinfo.patient_ehr_id.ToString().Trim();
                                            string MedicationID = "0";
                                            if (RemovedMedValue.medication_ehr_id != null)
                                            {
                                                MedicationID = RemovedMedValue.medication_ehr_id.ToString();
                                            }
                                            if (MedicationID.Trim() != "" && MedicationID.Trim() != "0")
                                            {
                                                drMedicationRemoved["Medication_EHR_Id"] = MedicationID.Trim();
                                            }
                                            else
                                            {
                                                if (Utility.Application_ID != 6)
                                                    continue;
                                                else
                                                    drMedicationRemoved["Medication_EHR_Id"] = "0";
                                            }
                                            drMedicationRemoved["Medication_Name"] = RemovedMedValue.medication_name.ToString();
                                            drMedicationRemoved["Medication_Note"] = RemovedMedValue.patientnote.ToString();
                                            drMedicationRemoved["PatientMedication_EHR_ID"] = RemovedMedValue.patientmedication_ehr_id.ToString();
                                            drMedicationRemoved["Clinic_Number"] = Clinic_Number;
                                            drMedicationRemoved["Service_Install_Id"] = Service_Install_ID;
                                            dtLivePatientFormMedicationRemovedResponse.Rows.Add(drMedicationRemoved);
                                        }
                                    }

                                    if (subitem.Medication_value != null)
                                    {
                                        foreach (var medicationValue in subitem.Medication_value)
                                        {
                                            DataRow drMedication = dtLivePatientFormMedicationResponse.NewRow();
                                            drMedication["MedicationMaster_Web_ID"] = ""; //medicationValue._id.ToString();
                                            drMedication["PatientForm_Web_ID"] = item._id.ToString();
                                            drMedication["Patient_EHR_ID"] = item.pinfo == null ? "" : item.pinfo.patient_ehr_id.ToString().Trim();
                                            string MedicationID = "0";
                                            if (medicationValue.medication_ehr_id != null)
                                            {
                                                MedicationID = medicationValue.medication_ehr_id.ToString();
                                            }
                                            if (MedicationID.Trim() != "" && MedicationID.Trim() != "0")
                                            {
                                                drMedication["Medication_EHR_Id"] = MedicationID.Trim();
                                            }
                                            else
                                            {
                                                if (Utility.Application_ID != 6)
                                                    continue;
                                                else
                                                    drMedication["Medication_EHR_Id"] = "0";
                                            }

                                            string PatientMedicationID = "0";
                                            if (medicationValue.patientmedication_ehr_id != null)
                                            {
                                                PatientMedicationID = medicationValue.patientmedication_ehr_id.ToString();
                                            }
                                            if (PatientMedicationID.Trim() != "" && PatientMedicationID.Trim() != "0")
                                            {
                                                drMedication["PatientMedication_EHR_Id"] = PatientMedicationID.Trim();
                                            }

                                            drMedication["Medication_Type"] = medicationValue.medication_type.ToString();
                                            drMedication["Medication_Name"] = medicationValue.medication_name.ToString();
                                            drMedication["Medication_Note"] = medicationValue.medication_note.ToString();
                                            drMedication["Clinic_Number"] = Clinic_Number;
                                            drMedication["Service_Install_Id"] = Service_Install_ID;
                                            dtLivePatientFormMedicationResponse.Rows.Add(drMedication);
                                        }
                                    }

                                }
                                if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && item.pinfo != null)
                                    || (((item.ehrmap.Where(s => s.Alg_Prb_value != null).Count() > 0) || (item.ehrmap.Where(s => s.Removed_Alg_Prb_value != null).Count() > 0)
                                    || (item.ehrmap.Where(s => s.Medication_value != null).Count() > 0) || (item.ehrmap.Where(s => s.Removed_Medication_value != null).Count() > 0)) && item.pinfo != null))
                                {
                                    var result = dtLivePatientForm.AsEnumerable().Where(o => o.Field<object>("PatientForm_Web_ID").ToString().ToUpper() == item._id.ToUpper()
                                    && o.Field<object>("ehrfield").ToString().Trim().ToUpper() == "FIRST_NAME");

                                    if (result == null || (result != null && result.Count() == 0))
                                    {
                                        InsertRowINtodatatable(ref dtLivePatientForm, "first_name", item.pinfo.first_name, item._id, item.pinfo.patient_ehr_id, item.pinfo._id, Clinic_Number, Service_Install_ID, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);
                                    }
                                    var result1 = dtLivePatientForm.AsEnumerable().Where(o => o.Field<object>("PatientForm_Web_ID").ToString().ToUpper() == item._id.ToUpper()
                                       && o.Field<object>("ehrfield").ToString().Trim().ToUpper() == "LAST_NAME");

                                    if (result1 == null || (result1 != null && result1.Count() == 0))
                                    {
                                        if (result1 == null)
                                        {
                                            lastname = "NA";
                                        }
                                        else if (result1 != null && result1.Count() == 0 && item.pinfo.last_name.ToString() == "")
                                        {
                                            lastname = "NA";
                                        }
                                        else
                                        {
                                            lastname = item.pinfo.last_name;
                                        }
                                        InsertRowINtodatatable(ref dtLivePatientForm, "last_name", lastname, item._id, item.pinfo.patient_ehr_id, item.pinfo._id, Clinic_Number, Service_Install_ID, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);
                                    }
                                    var result2 = dtLivePatientForm.AsEnumerable().Where(o => o.Field<object>("PatientForm_Web_ID").ToString().ToUpper() == item._id.ToUpper()
                                       && o.Field<object>("ehrfield").ToString().Trim().ToUpper() == "MOBILE");

                                    if (result2 == null || (result2 != null && result2.Count() == 0))
                                    {
                                        InsertRowINtodatatable(ref dtLivePatientForm, "mobile", item.pinfo.mobile == null ? "0000000000" : item.pinfo.mobile, item._id, item.pinfo.patient_ehr_id, item.pinfo._id, Clinic_Number
                                            , Service_Install_ID, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);
                                    }
                                }
                            }
                            else
                            {
                                if (item.pinfo != null)
                                {
                                    DataRow RowPatientForm = dtLivePatientForm.NewRow();
                                    if (item.pinfo != null)
                                    {
                                        RowPatientForm["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                        RowPatientForm["Patient_Web_ID"] = item.pinfo._id;
                                        RowPatientForm["ehrfield_value"] = item.pinfo.first_name.Replace("'", "''");
                                        RowPatientForm["ehrfield"] = "first_name";
                                        RowPatientForm["PatientForm_Web_ID"] = item._id;
                                        RowPatientForm["Clinic_Number"] = Clinic_Number;
                                        RowPatientForm["Service_Install_Id"] = Service_Install_ID;
                                    }

                                    //rooja 2-5-23
                                    try
                                    {
                                        AddinfoinForm(ref RowPatientForm, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);

                                    }
                                    catch (Exception)
                                    {

                                    }

                                    dtLivePatientForm.Rows.Add(RowPatientForm);
                                    dtLivePatientForm.AcceptChanges();

                                    DataRow RowPatientForm1 = dtLivePatientForm.NewRow();
                                    if (item.pinfo != null)
                                    {
                                        RowPatientForm1["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                        RowPatientForm1["Patient_Web_ID"] = item.pinfo._id;
                                        RowPatientForm1["ehrfield_value"] = item.pinfo.mobile == null ? "0000000000" : item.pinfo.mobile;
                                        RowPatientForm1["ehrfield"] = "mobile";
                                        RowPatientForm1["PatientForm_Web_ID"] = item._id;
                                        RowPatientForm1["Clinic_Number"] = Clinic_Number;
                                        RowPatientForm1["Service_Install_Id"] = Service_Install_ID;
                                    }


                                    //rooja 2-5-23
                                    try
                                    {
                                        AddinfoinForm(ref RowPatientForm1, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);

                                    }
                                    catch (Exception)
                                    {

                                    }
                                    dtLivePatientForm.Rows.Add(RowPatientForm1);
                                    dtLivePatientForm.AcceptChanges();

                                    DataRow RowPatientForm2 = dtLivePatientForm.NewRow();
                                    if (item.pinfo != null)
                                    {
                                        if (item.pinfo.last_name.ToString() == "")
                                        {
                                            lastname = "NA";
                                        }
                                        else
                                        {
                                            lastname = item.pinfo.last_name.Replace("'", "''");
                                        }
                                        RowPatientForm2["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                        RowPatientForm2["Patient_Web_ID"] = item.pinfo._id;
                                        RowPatientForm2["ehrfield_value"] = lastname;
                                        RowPatientForm2["ehrfield"] = "last_name";
                                        RowPatientForm2["PatientForm_Web_ID"] = item._id;
                                        RowPatientForm2["Clinic_Number"] = Clinic_Number;
                                        RowPatientForm2["Service_Install_Id"] = Service_Install_ID;
                                    }
                                    //rooja 2-5-23
                                    try
                                    {
                                        AddinfoinForm(ref RowPatientForm2, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);

                                    }
                                    catch (Exception)
                                    {

                                    }
                                    dtLivePatientForm.Rows.Add(RowPatientForm2);
                                    dtLivePatientForm.AcceptChanges();
                                }
                            }
                            if (item.ehr_value != null)
                            {
                                firstname = "";
                                lastname = "";
                                if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty && item.ehr_value.patientName.Contains(" "))
                                {
                                    firstname = item.ehr_value.patientName.Substring(0, item.ehr_value.patientName.IndexOf(" ")).Trim();
                                    lastname = item.ehr_value.patientName.Substring(item.ehr_value.patientName.IndexOf(" "), (item.ehr_value.patientName.Length - item.ehr_value.patientName.IndexOf(" "))).Trim();
                                }
                                else if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty && !item.ehr_value.patientName.Contains(" "))
                                {
                                    firstname = item.ehr_value.patientName.ToString();
                                    lastname = "NA";
                                }

                                if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty)
                                {
                                    if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count == 0) || (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && dtLivePatientForm.Select("PatientForm_Web_ID = '" + item._id.ToString() + "' AND ehrfield = 'first_name'").Count() == 0))
                                    {
                                        if (firstname != string.Empty)
                                        {
                                            DataRow RowPatientForm = dtLivePatientForm.NewRow();
                                            RowPatientForm["ehrfield_value"] = firstname.Replace("'", "''");
                                            RowPatientForm["ehrfield"] = "first_name";
                                            RowPatientForm["PatientForm_Web_ID"] = item._id;
                                            RowPatientForm["Clinic_Number"] = Clinic_Number;
                                            RowPatientForm["Service_Install_Id"] = Service_Install_ID;

                                            AddinfoinForm(ref RowPatientForm, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);

                                            dtLivePatientForm.Rows.Add(RowPatientForm);
                                        }
                                    }
                                }


                                if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty)
                                {
                                    if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count == 0) || (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && dtLivePatientForm.Select("PatientForm_Web_ID = '" + item._id.ToString() + "' AND ehrfield = 'last_name'").Count() == 0))
                                    {
                                        if (lastname != string.Empty)
                                        {
                                            DataRow RowPatientForm1 = dtLivePatientForm.NewRow();
                                            RowPatientForm1["ehrfield_value"] = lastname.Replace("'", "''"); ;
                                            RowPatientForm1["ehrfield"] = "last_name";
                                            RowPatientForm1["PatientForm_Web_ID"] = item._id;
                                            RowPatientForm1["Clinic_Number"] = Clinic_Number;
                                            RowPatientForm1["Service_Install_Id"] = Service_Install_ID;
                                            AddinfoinForm(ref RowPatientForm1, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);

                                            dtLivePatientForm.Rows.Add(RowPatientForm1);
                                        }
                                    }
                                }


                                if (item.ehr_value.phone != null && item.ehr_value.phone != string.Empty)
                                {
                                    if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count == 0) || (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && dtLivePatientForm.Select("PatientForm_Web_ID = '" + item._id.ToString() + "' AND ehrfield = 'mobile'").Count() == 0))
                                    {
                                        DataRow RowPatientForm2 = dtLivePatientForm.NewRow();
                                        RowPatientForm2["ehrfield_value"] = item.ehr_value.phone;
                                        RowPatientForm2["ehrfield"] = "mobile";
                                        RowPatientForm2["PatientForm_Web_ID"] = item._id;
                                        RowPatientForm2["Clinic_Number"] = Clinic_Number;
                                        RowPatientForm2["Service_Install_Id"] = Service_Install_ID;
                                        AddinfoinForm(ref RowPatientForm2, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name.Length > 50 ? item.form_name.Substring(0, 49) : item.form_name, item.patient_name, item.submit_time);

                                        dtLivePatientForm.Rows.Add(RowPatientForm2);
                                    }
                                }

                            }
                        }
                        //ObjGoalBase.WriteToSyncLogFile("Comapre patient");
                        DataTable dtPatientFormCopy = dtLivePatientForm.Clone();
                        dtPatientFormCopy.Load(dtLivePatientForm.CreateDataReader());
                        string firstName = "";
                        string LastName = "";
                        string MiddleName = "";
                        string mobileNo = "";
                        string primaryProviderFirstName = "", secondaryProviderFirstName = "";

                        //ObjGoalBase.WriteToSyncLogFile("Start Set Patient Insurance ." + dtPatientFormCopy.Rows.Count.ToString());

                        dtPatientFormCopy.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                             o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                           .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                           .All(o =>
                           {
                               var resultFirst_Name = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                    && a.Field<string>("ehrfield").ToString().ToUpper() == "FIRST_NAME");
                               var resultLastName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                   && a.Field<string>("ehrfield").ToString().ToUpper() == "LAST_NAME");
                               var resultMobile = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                   && a.Field<string>("ehrfield").ToString().ToUpper() == "MOBILE");

                               var primaryInsurance = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                   && a.Field<string>("ehrfield").ToString().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME");

                               var secondaryInsurance = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                   && a.Field<string>("ehrfield").ToString().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME");

                               if (resultFirst_Name.Count() == 0)
                               {
                                   IndertDefaultRowForFirstNameLastNameMobile(o.ToString(), "first_name", ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                               }
                               if (resultLastName.Count() == 0)
                               {
                                   IndertDefaultRowForFirstNameLastNameMobile(o.ToString(), "last_name", ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                               }
                               if (resultMobile.Count() == 0)
                               {
                                   IndertDefaultRowForFirstNameLastNameMobile(o.ToString(), "mobile", ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                               }
                               #region Get Patient INsurance
                               if (primaryInsurance.Count() > 0)
                               {
                                   var primaryInsuranceName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                   && a.Field<string>("ehrfield").ToString().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME").Select(b => b.Field<object>("ehrfield_value").ToString());
                                   if (primaryInsuranceName.Count() > 0)
                                   {
                                       var primararyInsuranceName = dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == primaryInsuranceName.First().ToString().ToUpper());
                                       if (primararyInsuranceName.Count() > 0)
                                       {
                                           InsertPatientInsuranceColumns(o.ToString(), "prim_relationship", "S", ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           InsertPatientInsuranceColumns(o.ToString(), "prim_employer_id", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == primaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("employer_id")).First().ToString(), ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           InsertPatientInsuranceColumns(o.ToString(), "prim_outstanding_balance", "0", ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           InsertPatientInsuranceColumns(o.ToString(), "prim_benefits_remaining", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == primaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("benefits_remaining")).First().ToString(), ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           InsertPatientInsuranceColumns(o.ToString(), "prim_remaining_deductible", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == primaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("remaining_deductible")).First().ToString(), ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           InsertPatientInsuranceColumns(o.ToString(), "patient_status", "Y", ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           InsertPatientInsuranceColumns(o.ToString(), "policy_holder_status", "Y", ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                       }
                                       else if (Utility.Application_ID == 1)
                                       {
                                           var primarySubScriber = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                  && a.Field<string>("ehrfield").ToString().ToUpper() == "PRIMARY_SUBSCRIBER_ID");
                                           if (primarySubScriber != null && primarySubScriber.Count() > 0)
                                           {
                                               primarySubScriber.All(v => { v["ehrfield_value"] = ""; return true; });
                                           }
                                       }
                                   }
                               }
                               else if (primaryInsurance.Count() <= 0)
                               {
                                   var primarySubScriber = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                          && a.Field<string>("ehrfield").ToString().ToUpper() == "PRIMARY_SUBSCRIBER_ID");
                                   if (primarySubScriber != null && primarySubScriber.Count() > 0)
                                   {
                                       primarySubScriber.All(v => { v["ehrfield_value"] = ""; return true; });
                                   }
                               }
                               if (secondaryInsurance.Count() > 0)
                               {
                                   var secondaryInsuranceName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                   && a.Field<string>("ehrfield").ToString().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME").Select(b => b.Field<object>("ehrfield_value").ToString());

                                   if (secondaryInsuranceName.Count() > 0)
                                   {
                                       var secInsuranceName = dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == secondaryInsuranceName.First().ToString().ToUpper());
                                       if (secInsuranceName.Count() > 0)
                                       {
                                           InsertPatientInsuranceColumns(o.ToString(), "sec_relationship", "S", ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           InsertPatientInsuranceColumns(o.ToString(), "sec_employer_id", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == secondaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("employer_id")).First().ToString(), ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           InsertPatientInsuranceColumns(o.ToString(), "sec_outstanding_balance", "0", ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           InsertPatientInsuranceColumns(o.ToString(), "sec_benefits_remaining", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == secondaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("benefits_remaining")).First().ToString(), ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           InsertPatientInsuranceColumns(o.ToString(), "sec_remaining_deductible", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == secondaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("remaining_deductible")).First().ToString(), ref dtLivePatientForm, Clinic_Number, Service_Install_ID, folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                       }
                                   }
                                   else
                                   {
                                       var primarySubScriber = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                 && a.Field<string>("ehrfield").ToString().ToUpper() == "SECONDARY_SUBSCRIBER_ID");
                                       if (primarySubScriber != null && primarySubScriber.Count() > 0)
                                       {
                                           primarySubScriber.All(v => { v["ehrfield_value"] = ""; return true; });
                                       }
                                   }
                               }
                               else if (secondaryInsurance.Count() <= 0)
                               {
                                   var primarySubScriber = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                          && a.Field<string>("ehrfield").ToString().ToUpper() == "SECONDARY_SUBSCRIBER_ID");
                                   if (primarySubScriber != null && primarySubScriber.Count() > 0)
                                   {
                                       primarySubScriber.All(v => { v["ehrfield_value"] = ""; return true; });
                                   }
                               }
                               #endregion
                               return
               true;
                           });
                        dtLivePatientForm.AcceptChanges();
                        //ObjGoalBase.WriteToSyncLogFile("1123");
                        dtPatientFormCopy.Clear();
                        dtPatientFormCopy = dtLivePatientForm.Clone();
                        dtPatientFormCopy.Load(dtLivePatientForm.CreateDataReader());

                        // ObjGoalBase.WriteToSyncLogFile("Start Set FirstName,LastName,MiddleName,Mobile & Providers ." + dtPatientFormCopy.Rows.Count.ToString());

                        dtPatientFormCopy.AsEnumerable()
                            .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                            .All(o =>
                            {
                                var result = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                    && a.Field<string>("ehrfield").ToString().ToUpper() == "FIRST_NAME");
                                var result1 = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                    && a.Field<string>("ehrfield").ToString().ToUpper() == "LAST_NAME");
                                var result2 = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                    && a.Field<string>("ehrfield").ToString().ToUpper() == "MIDDLE_NAME");
                                var result3 = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                    && a.Field<string>("ehrfield").ToString().ToUpper() == "MOBILE");
                                var resultPreProvider = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                    && a.Field<string>("ehrfield").ToString().ToUpper() == "PRI_PROVIDER_ID");
                                var resultSecondaryProvider = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                    && a.Field<string>("ehrfield").ToString().ToUpper() == "SEC_PROVIDER_ID");

                                //ObjGoalBase.WriteToSyncLogFile("Loop PatientName " + result.Count().ToString() + " " + result1.Count().ToString() + " " + result3.Count().ToString());

                                if (result != null && result1 != null && result3 != null && result.Count() > 0 && result1.Count() > 0 && result3.Count() > 0)
                                {
                                    firstName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "FIRST_NAME").Select(b => b.Field<string>("EHRField_Value")).First().ToString();
                                    LastName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "LAST_NAME").Select(b => b.Field<string>("EHRField_Value")).First().ToString();
                                    //MiddleName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "MIDDLE_NAME").Select(b => b.Field<string>("EHRField_Value")).First().ToString();
                                    mobileNo = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "MOBILE").Select(b => b.Field<string>("EHRField_Value")).First().ToString();

                                    // ObjGoalBase.WriteToSyncLogFile("Condition True " + firstName.ToString() +  " " + LastName.ToString() + " " + mobileNo.ToString() );
                                    //dtEHRPatientData.Select(" First_name = '" + firstName.ToString() + "' AND Last_name = '" + LastName.ToString() + "' AND ( ( (Mobile = '' OR Mobile = null) AND " + Utility.ConvertContactNumber(mobileNo.ToString().Trim()) + " = '0000000000') OR ( Mobile <> '' AND Mobile <> null AND Mobile = '" + Utility.ConvertContactNumber(mobileNo.ToString().Trim()) + "' ))")
                                    if (dtEHRPatientData.Select(" First_name = '" + firstName.ToString() + "' AND Last_name = '" + LastName.ToString() + "'").Count() > 0)
                                    {

                                        bool ismatchedrecords = false;
                                        //rooja for task https://app.asana.com/0/1204010716278938/1204810921234162/f
                                        foreach (DataRow drRow in dtEHRPatientData.Select(" First_name = '" + firstName.ToString() + "' AND Last_name = '" + LastName.ToString() + "' and EHR_STATUS = 'Active'"))
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
                                                dtLivePatientForm.AsEnumerable().Where(a =>
                                                (a.Field<object>("Patient_EHR_ID") == null || (a.Field<object>("Patient_EHR_ID") != null
                                                && a.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (a.Field<object>("Patient_EHR_ID") != null
                                                && a.Field<object>("Patient_EHR_ID").ToString() == "0")) && a.Field<string>("PatientForm_Web_ID") == o.ToString())
                                                    .All(d =>
                                                    {
                                                        d["Patient_EHR_ID"] = drRow["Patient_EHR_ID"];
                                                        return true;
                                                    });
                                                break;
                                            }
                                        }

                                    }
                                }
                                //ObjGoalBase.WriteToSyncLogFile("Check Provider " + resultPreProvider.Count().ToString());
                                if ((resultPreProvider != null && resultPreProvider.Count() > 0) || (resultSecondaryProvider != null && resultSecondaryProvider.Count() > 0))
                                {
                                    //ObjGoalBase.WriteToSyncLogFile(" Provider condition true " + resultPreProvider.Count().ToString());
                                    if (resultPreProvider.Count() > 0)
                                    {
                                        primaryProviderFirstName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "PRI_PROVIDER_ID").Select(b => b.Field<string>("EHRField_Value")).First().ToString();
                                        SetPrimarySecondaryProviderId(primaryProviderFirstName.ToUpper(), dtLocalProviderData, "PRI_PROVIDER_ID", o.ToString(), ref dtLivePatientForm);
                                    }
                                    if (resultSecondaryProvider.Count() > 0)
                                    {
                                        secondaryProviderFirstName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "SEC_PROVIDER_ID").Select(b => b.Field<string>("EHRField_Value")).First().ToString();
                                        SetPrimarySecondaryProviderId(secondaryProviderFirstName.ToUpper(), dtLocalProviderData, "SEC_PROVIDER_ID", o.ToString(), ref dtLivePatientForm);
                                    }
                                }

                                return true;
                            });
                        //ObjGoalBase.WriteToSyncLogFile("Check for Update Records ." + dtLivePatientForm.Rows.Count.ToString());
                        foreach (DataRow dtDtxRow in dtLivePatientForm.Rows)
                        {
                            DataRow[] row = dtLocalPatientForm.Copy().Select("PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND ehrfield = '" + dtDtxRow["ehrfield"].ToString().Trim() + "' ");
                            if (row.Length > 0)
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }

                        foreach (DataRow dtDtxRow in dtLivePatientFormDiseaseResponse.Rows)
                        {
                            DataRow[] row = dtPatientFormDiseaseResponse.Copy().Select("DiseaseMaster_Web_ID = '" + dtDtxRow["DiseaseMaster_Web_ID"].ToString().Trim() + "' AND PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND Disease_EHR_Id = '" + dtDtxRow["Disease_EHR_Id"].ToString().Trim() + "'");
                            if (row.Length > 0)
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                                dtDtxRow["DiseaseResponse_Local_ID"] = row[0]["DiseaseResponse_Local_ID"];
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        foreach (DataRow dtDtxRow in dtLivePatientFormDiseaseDeleteResponse.Rows)
                        {
                            DataRow[] row = dtPatientFormDiseaseDeleteResponse.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + "' AND PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND Disease_EHR_Id = '" + dtDtxRow["Disease_EHR_Id"].ToString().Trim() + "' AND Disease_Type = '" + dtDtxRow["Disease_Type"].ToString().Trim() + "'");
                            if (row.Length > 0)
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                                dtDtxRow["DiseaseDeleteResponse_Local_ID"] = row[0]["DiseaseDeleteResponse_Local_ID"];
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }

                        foreach (DataRow dtDtxRow in dtLivePatientFormMedicationRemovedResponse.Rows)
                        {
                            DataRow[] row = dtPatientFormMedicationRemovedResponse.Copy().Select("PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND PatientMedication_EHR_Id = '" + dtDtxRow["PatientMedication_EHR_Id"].ToString().Trim() + "'");
                            if (row.Length > 0)
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                                dtDtxRow["MedicationRemovedResponse_Local_ID"] = row[0]["MedicationRemovedResponse_Local_ID"];
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        foreach (DataRow dtDtxRow in dtLivePatientFormMedicationResponse.Rows)
                        {
                            DataRow[] row = dtPatientFormMedicationResponse.Copy().Select("PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND PatientMedication_EHR_Id = '" + dtDtxRow["PatientMedication_EHR_Id"].ToString().Trim() + "'");
                            if (row.Length > 0)
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                                dtDtxRow["MedicationResponse_Local_ID"] = row[0]["MedicationResponse_Local_ID"];
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        dtLivePatientForm.AcceptChanges();
                        // Utility.WriteToSyncLogFile_All("Check to save records in Local " + dtLivePatientForm.Rows.Count.ToString());
                        if (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0)
                        {
                            DataTable dtSaveRecords = dtLivePatientForm.Clone();
                            if (dtLivePatientForm.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtLivePatientForm.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                            }
                            // Utility.WriteToSyncLogFile_All("Send to save records in Local " + dtLivePatientForm.Rows.Count.ToString());
                            // Check SSN Condition
                            dtSaveRecords = CheckPatientWiseSSNvalidation(dtSaveRecords, dtEHRPatientData);

                            bool status = PullLiveDatabaseBAL.Save_PatientForm_Live_To_Local(dtSaveRecords);

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");
                                SynchTrackerBAL.Save_Tracker_To_Local(dtLivePatientFormDiseaseResponse, "DiseaseResponse", "DiseaseResponse_Local_ID", "DiseaseResponse_Local_ID");
                                SynchTrackerBAL.Save_Tracker_To_Local(dtLivePatientFormDiseaseDeleteResponse, "DiseaseDeleteResponse", "DiseaseDeleteResponse_Local_ID", "DiseaseDeleteResponse_Local_ID");
                                SynchTrackerBAL.Save_Tracker_To_Local(dtLivePatientFormMedicationResponse, "MedicationResponse", "MedicationResponse_Local_ID", "MedicationResponse_Local_ID");
                                SynchTrackerBAL.Save_Tracker_To_Local(dtLivePatientFormMedicationRemovedResponse, "MedicationRemovedResponse", "MedicationRemovedResponse_Local_ID", "MedicationRemovedResponse_Local_ID");

                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                // bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientForm_Pull");
                                GoalBase.WriteToSyncLogFile_Static("PatientForm Sync (Adit Server To Local Database) Service Install Id : " + Service_Install_ID + " And  Clinic : " + Clinic_Number + " Successfully.");
                            }
                            //dtLivePatientForm.Select("InsUptDlt IN (1,2,3)")

                        }
                        else
                        {
                            GoalBase.WriteToSyncLogFile_Static("PatientForm Sync (Adit Server To Local Database) Service Install Id : " + Service_Install_ID + " And Clinic : " + Clinic_Number
                                + " Records Not found on Adit Server");
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");
                        }
                        // GetPatientDocument();
                    }
                    else
                    {
                        GoalBase.WriteToSyncLogFile_Static("PatientForm Sync (Adit Server To Local Database) Service Install Id : " + Service_Install_ID + " And Clinic : " + Clinic_Number + " Pending Records Not found on Adit Server");
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");
                    }
                }
                else
                {
                    //Utility.WriteToSyncLogFile_All("PatientForm_AditLocationSyncEnable False : " + Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString());
                    Utility.WriteToSyncLogFile_All("PatientForm_AditLocationSyncEnable False");
                }

                using (frmPozative objPoz = new frmPozative())
                {
                    #region Sync Patient Form Local to EHR
                    //EagleSoft
                    if (Utility.Application_ID == 1)
                    {
                        objPoz.SynchDataLocalToEagleSoft_Patient_Form_FromEvent(PatientFormWebID, Clinic_Number, Service_Install_ID);
                    }
                    // OpenDental
                    else if (Utility.Application_ID == 2)
                    {
                        objPoz.SynchDataLocalToOpenDental_Patient_Form_FromEvent(PatientFormWebID, Clinic_Number, Service_Install_ID);
                    }
                    // Dentrix
                    else if (Utility.Application_ID == 3)
                    {
                        objPoz.SynchDataLocalToDentrix_Patient_Form_FromEvent(PatientFormWebID, Clinic_Number, Service_Install_ID);
                    }
                    //Softdent
                    else if (Utility.Application_ID == 4)
                    {

                    }
                    //ClearDent
                    else if (Utility.Application_ID == 5)
                    {
                        objPoz.SynchDataLocalToClearDent_Patient_Form_FromEvent(PatientFormWebID, Clinic_Number, Service_Install_ID);
                    }
                    //Tracker
                    else if (Utility.Application_ID == 6)
                    {
                        objPoz.SynchDataLocalToTracker_Patient_Form_FromEvent(PatientFormWebID, Clinic_Number, Service_Install_ID);
                    }
                    // PracticeWork
                    else if (Utility.Application_ID == 7)
                    {
                        objPoz.SynchDataLocalToPracticeWork_Patient_Form_FromEvent(PatientFormWebID, Clinic_Number, Service_Install_ID);
                    }
                    //EasyDental
                    else if (Utility.Application_ID == 8)
                    {

                    }
                    //PracticeWeb
                    else if (Utility.Application_ID == 10)
                    {
                        objPoz.SynchDataLocalToPracticeWeb_Patient_Form_FromEvent(PatientFormWebID, Clinic_Number, Service_Install_ID);
                    }
                    //AbelDent
                    else if (Utility.Application_ID == 11)
                    {
                        objPoz.SynchDataLocalToAbelDent_Patient_Form_FromEvent(PatientFormWebID, Clinic_Number, Service_Install_ID);
                    }
                    #endregion
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("Create_Patient_Form_FromEvent Error : " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Appointment Confirmation Status
        public static bool SynchDataLiveDB_Pull_EHR_appointment_FromEvent(string strContent, string Clinic_Number, string Service_Install_ID)
        {
            string Location_ID = "";
            try
            {
                var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
                Location_ID = LocID[0]["Location_ID"].ToString();

                if (!strContent.StartsWith("{") && strContent.StartsWith("data:"))
                {
                    strContent = strContent.Substring(strContent.IndexOf("{"), strContent.Length - strContent.IndexOf("{"));
                }
                var EHR_appointmentDto = Newtonsoft.Json.JsonConvert.DeserializeObject<Pull_AppointmentBO>(strContent);
                if (EHR_appointmentDto != null && EHR_appointmentDto.data != null)
                {
                    SendAcknowledgement("confirm_appointment", EHR_appointmentDto.data[0]._id, Location_ID);

                    if (Utility.Application_ID == 11)
                    {

                        DataTable dtLiveEHR_appointment = new DataTable();
                        dtLiveEHR_appointment.Columns.Add("Appt_EHR_ID", typeof(string));
                        dtLiveEHR_appointment.Columns.Add("Appt_Web_ID", typeof(string));
                        dtLiveEHR_appointment.Columns.Add("confirmed_status_ehr_key", typeof(string));
                        dtLiveEHR_appointment.Columns.Add("confirmed_status", typeof(string));
                        dtLiveEHR_appointment.Columns.Add("InsUptDlt", typeof(int));
                        dtLiveEHR_appointment.Columns.Add("Clinic_Number", typeof(string));
                        dtLiveEHR_appointment.Columns.Add("Service_Install_ID", typeof(string));

                        string cur_Appt_EHR_ID = string.Empty;


                        foreach (var item in EHR_appointmentDto.data)
                        {
                            cur_Appt_EHR_ID = "";
                            try
                            {
                                cur_Appt_EHR_ID = item.appt_ehr_id;
                            }
                            catch (Exception)
                            {
                                cur_Appt_EHR_ID = "";
                            }

                            if (cur_Appt_EHR_ID != "" && cur_Appt_EHR_ID != string.Empty)
                            {
                                DataTable AppointmentConformStatus = SynchLocalBAL.GetLocalAppointmentConformStatusData(cur_Appt_EHR_ID.ToString(), Service_Install_ID);

                                if (AppointmentConformStatus != null && AppointmentConformStatus.Rows.Count > 0)
                                {
                                    DataRow RowAppt = dtLiveEHR_appointment.NewRow();
                                    RowAppt["Appt_EHR_ID"] = cur_Appt_EHR_ID;
                                    RowAppt["Appt_Web_ID"] = item._id;
                                    RowAppt["confirmed_status_ehr_key"] = item.confirmed_status_ehr_key;
                                    RowAppt["confirmed_status"] = item.confirmed_status;
                                    RowAppt["Clinic_Number"] = Clinic_Number;
                                    RowAppt["Service_Install_Id"] = Service_Install_ID;
                                    dtLiveEHR_appointment.Rows.Add(RowAppt);
                                    dtLiveEHR_appointment.AcceptChanges();
                                    GoalBase.WriteToSyncLogFile_Static("StatusAppointmentlist(" + cur_Appt_EHR_ID + "-" + item.confirmed_status_ehr_key + ")");
                                }
                            }
                        }

                        if (dtLiveEHR_appointment != null && dtLiveEHR_appointment.Rows.Count > 0)
                        {
                            //CheckEntryUserLoginIdExist();
                            bool status = false;

                            status = SynchAbelDentBAL.Update_Status_EHR_Appointment_Live_To_AbelDentEHR(dtLiveEHR_appointment);

                            if (status)
                            {
                                if (SynchAbelDentBAL.Insert_Status_EHR_Appointment_To_AbelDentEHR(dtLiveEHR_appointment))
                                {
                                    if (status)
                                    {
                                        Update_Status_EHR_Appointment_EHR_To_Live(dtLiveEHR_appointment);
                                    }
                                    else if (Utility.AppointmentEHRIds.ToString() != "")
                                    {
                                        GoalBase.WriteToSyncLogFile_Static("StatusAppointmentlist(" + Utility.AppointmentEHRIds.ToString() + ") Sync Update on Adit Server With Service Install Id : " + Service_Install_ID + " And  Clinic : " + Clinic_Number + " Successfully.");
                                    }
                                }
                            }
                        }

                    }
                    else
                    {

                        DataTable dtLiveEHR_appointment = new DataTable();
                        dtLiveEHR_appointment.Columns.Add("Appt_EHR_ID", typeof(Int64));
                        dtLiveEHR_appointment.Columns.Add("Appt_Web_ID", typeof(string));
                        dtLiveEHR_appointment.Columns.Add("confirmed_status_ehr_key", typeof(string));
                        dtLiveEHR_appointment.Columns.Add("confirmed_status", typeof(string));
                        dtLiveEHR_appointment.Columns.Add("InsUptDlt", typeof(int));
                        dtLiveEHR_appointment.Columns.Add("Clinic_Number", typeof(string));
                        dtLiveEHR_appointment.Columns.Add("Service_Install_ID", typeof(string));

                        Int64 cur_Appt_EHR_ID = 0;

                        foreach (var item in EHR_appointmentDto.data)
                        {
                            cur_Appt_EHR_ID = 0;
                            try
                            {
                                cur_Appt_EHR_ID = Convert.ToInt64(item.appt_ehr_id);
                            }
                            catch (Exception)
                            {
                                cur_Appt_EHR_ID = 0;
                            }

                            if (cur_Appt_EHR_ID != 0)
                            {
                                DataTable AppointmentConformStatus = SynchLocalBAL.GetLocalAppointmentConformStatusData(cur_Appt_EHR_ID.ToString(), Service_Install_ID);

                                if (AppointmentConformStatus != null && AppointmentConformStatus.Rows.Count > 0)
                                {
                                    DataRow RowAppt = dtLiveEHR_appointment.NewRow();
                                    RowAppt["Appt_EHR_ID"] = cur_Appt_EHR_ID;
                                    RowAppt["Appt_Web_ID"] = item._id;
                                    RowAppt["confirmed_status_ehr_key"] = item.confirmed_status_ehr_key;
                                    RowAppt["confirmed_status"] = item.confirmed_status;
                                    RowAppt["Clinic_Number"] = Clinic_Number;
                                    RowAppt["Service_Install_Id"] = Service_Install_ID;
                                    dtLiveEHR_appointment.Rows.Add(RowAppt);
                                    dtLiveEHR_appointment.AcceptChanges();
                                }
                            }
                        }
                        // GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment Count " + dtLiveEHR_appointment.Rows.Count.ToString());
                        if (dtLiveEHR_appointment != null && dtLiveEHR_appointment.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                            bool status = false;

                            if (Utility.Application_ID == 1)
                            {
                                Utility.AppointmentEHRIds = "";
                                // GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment Send to Save " + dtLiveEHR_appointment.Rows.Count.ToString());
                                status = SynchEaglesoftBAL.Update_Status_EHR_Appointment_Live_To_Eaglesoft(dtLiveEHR_appointment, Utility.GetDataBaseConnectionByServicesInstallId(Service_Install_ID), Location_ID);
                            }
                            else if (Utility.Application_ID == 2)
                            {
                                status = SynchOpenDentalBAL.Update_Status_EHR_Appointment_Live_To_Opendental(dtLiveEHR_appointment, Utility.GetDataBaseConnectionByServicesInstallId(Service_Install_ID));
                            }
                            else if (Utility.Application_ID == 3)
                            {
                                status = SynchDentrixBAL.Update_Status_EHR_Appointment_Live_To_DentrixEHR(dtLiveEHR_appointment);
                            }
                            else if (Utility.Application_ID == 5)
                            {
                                status = SynchClearDentBAL.Update_Status_EHR_Appointment_Live_To_ClearDentEHR(dtLiveEHR_appointment);
                            }
                            else if (Utility.Application_ID == 6)
                            {
                                status = SynchTrackerBAL.Update_Status_EHR_Appointment_Live_To_TrackerEHR(dtLiveEHR_appointment);
                            }
                            else if (Utility.Application_ID == 7)
                            {
                                status = SynchPracticeWorkBAL.Update_Status_EHR_Appointment_Live_To_PracticeWorkEHR(dtLiveEHR_appointment);
                            }
                            else if (Utility.Application_ID == 8)
                            {
                                status = SynchEasyDentalBAL.Update_Status_EHR_Appointment_Live_To_EasyDentalEHR(dtLiveEHR_appointment);
                            }
                            else if (Utility.Application_ID == 10)
                            {
                                status = SynchPracticeWebBAL.Update_Status_EHR_Appointment_Live_To_PracticeWeb(dtLiveEHR_appointment, Utility.GetDataBaseConnectionByServicesInstallId(Service_Install_ID));
                            }
                            if (status && Utility.Application_ID != 1)
                            {
                                Update_Status_EHR_Appointment_EHR_To_Live(dtLiveEHR_appointment);
                            }
                            else if (Utility.AppointmentEHRIds.ToString() != "" && Utility.Application_ID == 1)
                            {
                                GoalBase.WriteToSyncLogFile_Static("StatusAppointmentlist(" + Utility.AppointmentEHRIds.ToString() + ") Sync Update on Adit Server With Service Install Id : " + Service_Install_ID + " And  Clinic : " + Clinic_Number + " Successfully.");
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[EHR_appointment From Events Sync (Adit Server To Local Database)] : " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Treatment Document
        public static bool SynchDataLiveDB_Pull_treatmentDoc_FromEvent(string strContent, string Clinic_Number, string Service_Install_ID)
        {
            string TreatmentPlanId = "", Patient_EHR_ID = "";
            try
            {
                if (!strContent.StartsWith("{") && strContent.StartsWith("data:"))
                {
                    strContent = strContent.Substring(strContent.IndexOf("{"), strContent.Length - strContent.IndexOf("{"));
                }
                string Location_ID = "", strDbConnString = "", strDocumentPath = "";
                var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
                Location_ID = LocID[0]["Location_ID"].ToString();

                var dbConStr = Utility.DtInstallServiceList.Select("Installation_Id = '" + Service_Install_ID + "'");
                strDbConnString = dbConStr[0]["DBConnString"].ToString();
                strDocumentPath = dbConStr[0]["Document_Path"].ToString();


                var AppTreatmentDocDto = Newtonsoft.Json.JsonConvert.DeserializeObject<Pull_TreatmentDocBO>(strContent);
                if (AppTreatmentDocDto != null && AppTreatmentDocDto.data != null)
                {
                    SendAcknowledgement("treatment_plan", AppTreatmentDocDto.data[0].treatmentPlanId, Location_ID);

                    if (AppTreatmentDocDto.data != null && AppTreatmentDocDto.data.Count() > 0)
                    {
                        Utility.WriteToSyncLogFile_All("Response Received count " + AppTreatmentDocDto.data.Count().ToString());
                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent : Response Received count " + AppTreatmentDocDto.data.Count().ToString());

                        int count = 0;
                        foreach (var item in AppTreatmentDocDto.data)
                        {
                            //check for sync is done or not
                            bool DocSynced = SynchLocalBAL.Sync_check_for_treatmentDoct(item.treatmentPlanId);
                            Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent : DocSynced " + DocSynced);
                            if (DocSynced)
                            {
                                if (item.patient_ehr_id != null && item.patient_ehr_id != "")
                                {
                                    count++;
                                    string Patient_Web_ID = item.patientId;
                                    TreatmentPlanId = item.treatmentPlanId;
                                    string TreatmentPlanName = item.planName;
                                    //string Clinic_Number = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(); ;
                                    //string Service_Install_Id = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                    Patient_EHR_ID = item.patient_ehr_id;
                                    string PatientName = item.patientName;
                                    string SubmittedDate = Convert.ToString(DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(item.treatment_plan_submitted_at)));
                                    Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent : CreateRecordForTreatmentDoc start");
                                    SynchLocalBAL.CreateRecordForTreatmentDoc(PatientName, SubmittedDate, Patient_EHR_ID, Patient_Web_ID, TreatmentPlanId, TreatmentPlanName, Clinic_Number, Service_Install_ID);
                                    Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent : CreateRecordForTreatmentDoc end");
                                }
                            }
                        }

                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent : CheckEntryUserLoginIdExist start");
                        Utility.CheckEntryUserLoginIdExist();
                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent : CheckEntryUserLoginIdExist end");

                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent : SyncTreatmentDocument_FromEvent start");
                        SyncTreatmentDocument_FromEvent(TreatmentPlanId, Location_ID, Clinic_Number, Service_Install_ID);
                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent : SyncTreatmentDocument_FromEvent end");


                        if (Utility.Application_ID == 1)
                        {
                            SynchEaglesoftBAL.Save_Treatment_Document_in_EagleSoft(strDbConnString, Service_Install_ID, strDocumentPath, TreatmentPlanId);
                        }
                        else if (Utility.Application_ID == 2)
                        {
                            SynchOpenDentalBAL.Save_Treatment_Document_in_OpenDental(strDbConnString, Service_Install_ID, strDocumentPath, TreatmentPlanId, Patient_EHR_ID);
                        }
                        else if (Utility.Application_ID == 3)
                        {
                            //Dentrix
                            GoalBase.GetConnectionStringforDoc(false);
                            Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent : GetConnectionStringforDoc done.");
                            SynchDentrixBAL.Save_Treatment_Document_in_Dentrix(TreatmentPlanId);
                            Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent : Save_Treatment_Document_in_Dentrix done.");
                        }
                        else if (Utility.Application_ID == 5)
                        {
                            //ClearDent
                            SynchClearDentBAL.Save_Treatment_Document_in_ClearDent(TreatmentPlanId);
                        }
                        else if (Utility.Application_ID == 6)
                        {
                            //Tracker
                            SynchTrackerBAL.Save_TreatmentDocument_Form_Local_To_Tracker(TreatmentPlanId);
                        }
                        else if (Utility.Application_ID == 7)
                        {
                            //PracticeWork
                        }
                        else if (Utility.Application_ID == 8)
                        {
                            //EasyDental
                        }
                        else if (Utility.Application_ID == 10)
                        {
                            //PracticeWeb
                            SynchPracticeWebBAL.Save_Treatment_Document_in_PracticeWeb(strDbConnString, Service_Install_ID, strDocumentPath, TreatmentPlanId, Patient_EHR_ID);
                        }
                        else if (Utility.Application_ID == 11)
                        {
                            //AbleDent
                        }

                        #region change status as treatment doc impotred Completed
                        DataTable statusCompleted = SynchLocalBAL.ChangeStatusForTreatmentDoc("Completed");
                        if (statusCompleted.Rows.Count > 0)
                        {
                            Change_Status_TreatmentDoc(statusCompleted, "Completed");
                        }
                        #endregion
                    }
                    else
                    {
                        GoalBase.WriteToSyncLogFile_Static("TreatmentDoc Sync (Adit Server To Local Database) Service Install Id : " + Service_Install_ID + " And Clinic : " + Clinic_Number + " Pending Records Not found on Adit Server");
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");
                    }
                }
                else
                {
                    Utility.WriteToSyncLogFile_All("TreatmentDoc_AditLocationSyncEnable False : " + LocID[0]["AditLocationSyncEnable"].ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_treatmentDoc_FromEvent :");
                Utility.WriteToErrorLogFromAll("[SynchDataLiveDB_Pull_treatmentDoc_FromEvent Sync] : " + ex.Message);
                return false;
            }
        }

        private static void SyncTreatmentDocument_FromEvent(string TreatmentPlanId, string Location_ID, string Clinic_Number, string Service_Install_ID)
        {
            #region SavePatientDoc
            try
            {
                DataTable dtLocalPenddingTreatmentDocs = SynchLocalBAL.GetLocalPendingTreatmentDocData(Service_Install_ID, TreatmentPlanId);
                foreach (DataRow dr in dtLocalPenddingTreatmentDocs.Rows)
                {
                    #region TreatmentDocPullLive
                    try
                    {
                        string FileName = dr["SubmittedDate"].ToString().Trim() + "-" + dr["TreatmentPlanName"].ToString().Trim() + "-" + dr["PatientName"].ToString().Trim() + ".pdf";
                        FileName = FileName.Replace(":", "");
                        FileName = FileName.Replace("/", "-");

                        string filepath = CommonUtility.GetAditTreatmentDocTempPath() + "\\" + FileName;

                        if (!System.IO.File.Exists(filepath))
                        {
                            string strApiPatientForm = PullLiveDatabaseBAL.GetTreatmentDocFromWeb("treatmentplan_pdf", dr["TreatmentPlanId"].ToString());

                            TreatmentDoc treatmentDoc = new TreatmentDoc
                            {
                                returnType = "base64"
                            };
                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            string jsonString = javaScriptSerializer.Serialize(treatmentDoc);
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
                            var clientdoc = new RestSharp.RestClient(strApiPatientForm);
                            var requestdoc = new RestSharp.RestRequest(RestSharp.Method.POST);
                            System.Net.ServicePointManager.Expect100Continue = true;
                            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                            requestdoc.AddHeader("Authorization", Utility.WebAdminUserToken);
                            requestdoc.AddHeader("cache-control", "no-cache");
                            requestdoc.AddHeader("Content-Type", "application/json");
                            requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_ID));
                            requestdoc.AddParameter("application/json", jsonString, RestSharp.ParameterType.RequestBody);
                            Utility.WriteToSyncLogFile_All("PatientTreatmentPlanDocument_Request (Patient_EHR_ID = " + dr["TreatmentDoc_Web_ID"] + ") Called " + strApiPatientForm.ToString());
                            RestSharp.IRestResponse response = clientdoc.Execute(requestdoc);
                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                {
                                    Utility.WriteToErrorLogFromAll("[PatientForm_Sync_Error : " + response.ErrorMessage);
                                }
                                else
                                {
                                    Utility.WriteToErrorLogFromAll("[PatientForm Sync (Adit Server To Local Database)] Service Install Id  : " + Service_Install_ID + " And  Clinic :" + Clinic_Number + " " + response.ErrorMessage);
                                }
                                return;
                            }
                            Utility.WriteToSyncLogFile_All("PatientDocument_Request Response Received.");// + response.Content.ToString());
                            var jObject = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                            if (jObject != null)
                            {

                                string DocData = jObject.GetValue("data").ToString();
                                DocData = DocData.Replace("data:application/pdf;base64,", String.Empty);

                                // Document. 
                                bool Docstatus = WriteByteArrayToPdfFromEvent(DocData, CommonUtility.GetAditTreatmentDocTempPath(), FileName);
                                if (Docstatus)
                                {
                                    SynchLocalBAL.UPDATERecordForTreatmentDoc(dr["TreatmentPlanId"].ToString().Trim(), FileName);
                                }
                            }
                        }
                    }
                    catch (Exception ex1)
                    {
                        Utility.WriteToErrorLogFromAll("[PatientFormDocument Sync From Event (Adit Server To Local Database)] for Id " + dr["PatientForm_Web_ID"].ToString() + " Error " + ex1.Message);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("[PatientFormDocument Sync From Event (Adit Server To Local Database)] : " + ex.Message);
            }
            #endregion
        }

        public static bool WriteByteArrayToPdfFromEvent(string inPDFByteArrayStream, string pdflocation, string fileName)
        {
            try
            {
                byte[] data = Convert.FromBase64String(inPDFByteArrayStream);
                if (!System.IO.Directory.Exists(pdflocation))
                {
                    System.IO.Directory.CreateDirectory(pdflocation);
                }
                pdflocation = pdflocation + "\\" + fileName;
                using (System.IO.FileStream Writer = new System.IO.FileStream(pdflocation, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    Writer.Write(data, 0, data.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("[WriteByteArrayToPdfFromEvent Sync (Adit Server To Local Database)] : " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Insurance Carrier Document

        public static bool SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent(string strContent, string Clinic_Number1, string Service_Install_ID1)
        {
            //Utility.WriteToSyncLogFile_All("strContent -" + strContent.ToString());
            try
            {

                //if (!strContent.StartsWith("{") && strContent.StartsWith("data:"))
                //{
                //    strContent = strContent.Substring(strContent.IndexOf("{"), strContent.Length - strContent.IndexOf("{"));
                //}

                string Location_ID = "", strDbConnString = "", strDocumentPath = "";
                var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number1 + "'");
                Location_ID = LocID[0]["Location_ID"].ToString();

                var dbConStr = Utility.DtInstallServiceList.Select("Installation_Id = '" + Service_Install_ID1 + "'");
                strDbConnString = dbConStr[0]["DBConnString"].ToString();
                strDocumentPath = dbConStr[0]["Document_Path"].ToString();

                string PatientName = "";
                string SubmittedDate = "";
                string Patient_EHR_ID = "0";
                string Patient_Web_ID = "";
                string InsuranceCarrierId = "";

                string InsuranceCarrierDocName = "";
                string InsuranceCarrierFolderName = "";
                string Clinic_Number = "0";
                string Service_Install_Id = "1";
                bool FileCreated = false;
                string filepath = "";

                var AppInsuranceCarrierDocDto = Newtonsoft.Json.JsonConvert.DeserializeObject<Pull_InsuranceCarrierDocBO>(strContent);
                if (AppInsuranceCarrierDocDto != null && AppInsuranceCarrierDocDto.data != null)
                {
                    SendAcknowledgement("InsuranceCarrier_", AppInsuranceCarrierDocDto.data[0]._id, Location_ID);

                    if (AppInsuranceCarrierDocDto.data != null && AppInsuranceCarrierDocDto.data.Count() > 0)
                    {
                        Utility.WriteToSyncLogFile_All("Response Received count " + AppInsuranceCarrierDocDto.data.Count().ToString());
                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent : Response Received count " + AppInsuranceCarrierDocDto.data.Count().ToString());

                        int count = 0;
                        foreach (var item in AppInsuranceCarrierDocDto.data)
                        {
                            //check for sync is done or not
                            bool DocSynced = SynchLocalBAL.Sync_check_for_InsuranceCarrier(item._id);
                            Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent : DocSynced " + DocSynced);
                            if (DocSynced)
                            {
                                if (item.patientEhrId != null && item.patientEhrId != "")
                                {
                                    count++;

                                    PatientName = item.patientFullName;
                                    SubmittedDate = Convert.ToString(DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(item.submitted_at)));
                                    Patient_EHR_ID = item.patientEhrId;
                                    Patient_Web_ID = item.patientId;
                                    InsuranceCarrierId = item._id;

                                    InsuranceCarrierDocName = item.pdfName + ".pdf";
                                    InsuranceCarrierFolderName = item.foldername;
                                    Clinic_Number = Clinic_Number1;
                                    Service_Install_Id = Service_Install_ID1;

                                    filepath = CommonUtility.GetAditInsuranceCarrierDocTempPath() + "\\" + item.pdfName + ".pdf";
                                    FileCreated = false;
                                    //GoalBase.WriteToErrorLogFile_Static("[InsuranceCarrierDoc Sync (Adit Server To Local Database)] : before PDF file copy from adit to local");

                                    //using (WebClient clientdownload = new WebClient())
                                    //{
                                    //clientdownload.DownloadFile(item.pdffile, filepath);
                                    Utility.DownloadFileWithProgress(item.pdffile, filepath);
                                    if (File.Exists(filepath))
                                    {
                                        FileCreated = true;
                                    }
                                    //}
                                    Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent : CreateRecordForInsuranceCarrierDoc start");
                                    SynchLocalBAL.CreateRecordForInsuranceCarrierDoc(PatientName, SubmittedDate, Patient_EHR_ID, Patient_Web_ID, InsuranceCarrierId, InsuranceCarrierDocName, InsuranceCarrierFolderName, Clinic_Number, Service_Install_Id, FileCreated, Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document);

                                    Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent : CreateRecordForInsuranceCarrierDoc end");
                                }
                            }

                            Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent : CheckEntryUserLoginIdExist start");
                            Utility.CheckEntryUserLoginIdExist();
                            Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent : CheckEntryUserLoginIdExist end");

                            Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent : SyncInsuranceCarrierDocument_FromEvent start");
                            // SyncInsuranceCarrierDocument_FromEvent(InsuranceCarrierId, Location_ID, Clinic_Number, Service_Install_ID1);
                            Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent : SyncInsuranceCarrierDocument_FromEvent end");


                            if (Utility.Application_ID == 1)
                            {
                                SynchEaglesoftBAL.Save_InsuranceCarrier_Document_in_EagleSoft(strDbConnString, Service_Install_ID1, strDocumentPath, InsuranceCarrierId);
                            }
                            else if (Utility.Application_ID == 2)
                            {
                                SynchOpenDentalBAL.Save_InsuranceCarrier_Document_in_OpenDental(strDbConnString, Service_Install_ID1, strDocumentPath, InsuranceCarrierId, Patient_EHR_ID);
                            }
                            else if (Utility.Application_ID == 3)
                            {
                                //Dentrix
                                GoalBase.GetConnectionStringforDoc(false);
                                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent : GetConnectionStringforDoc done.");
                                SynchDentrixBAL.Save_InsuranceCarrier_Document_in_Dentrix(InsuranceCarrierId);
                                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent : Save_InsuranceCarrier_Document_in_Dentrix done.");
                            }
                            else if (Utility.Application_ID == 5)
                            {
                                //ClearDent
                                SynchClearDentBAL.Save_InsuranceCarrier_Document_in_ClearDent(InsuranceCarrierId);
                            }
                            else if (Utility.Application_ID == 6)
                            {
                                //Tracker
                                SynchTrackerBAL.Save_InsuranceCarrierDocument_Form_Local_To_Tracker(InsuranceCarrierId);
                            }
                            else if (Utility.Application_ID == 7)
                            {
                                //PracticeWork
                            }
                            else if (Utility.Application_ID == 8)
                            {
                                //EasyDental
                            }
                            else if (Utility.Application_ID == 10)
                            {
                                //PracticeWeb
                                SynchPracticeWebBAL.Save_InsuranceCarrier_Document_in_PracticeWeb(strDbConnString, Service_Install_ID1, strDocumentPath, InsuranceCarrierId, Patient_EHR_ID);
                            }
                            else if (Utility.Application_ID == 11)
                            {
                                //AbleDent
                            }

                            #region change status as InsuranceCarrier doc impotred Completed
                            DataTable statusCompleted = SynchLocalBAL.ChangeStatusForInsuranceCarrierDoc("Completed");
                            if (statusCompleted.Rows.Count > 0)
                            {
                                Change_Status_InsuranceCarrierDoc(statusCompleted, "Completed");
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        GoalBase.WriteToSyncLogFile_Static("InsuranceCarrierDoc Sync (Adit Server To Local Database) Service Install Id : " + Service_Install_ID1 + " And Clinic : " + Clinic_Number + " Pending Records Not found on Adit Server");
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");
                    }
                }
                else
                {
                    Utility.WriteToSyncLogFile_All("InsuranceCarrierDoc_AditLocationSyncEnable False : " + LocID[0]["AditLocationSyncEnable"].ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent :");
                Utility.WriteToErrorLogFromAll("[SynchDataLiveDB_Pull_InsuranceCarrierDoc_FromEvent Sync] : " + ex.Message);
                return false;
            }
        }
        #endregion

        #region ZohoDetails
        public static bool SynchDataLiveDB_Pull_ZohoInstall_FromEvent(string strContent, string ClinicNumber, String ServiceInstallID)
        {
            bool Is_Confirmed = false;
            object Is_Valid = null;
            bool Is_Installed = false;
            string UserID = "";
            string UserName = "";
            string strOrgName = "";
            string strLocationName = "";
            string JsonString = "";
            string strErrMsg = "";
            string WindowsUser = "";
            string WindowsPass = "";
            bool WindowsUserResult = false;
            string EHRUser = "";
            string EHRPass = "";
            bool EHRUserResult = false;
            string LocationId = "";
            string LocationName = "";
            string OrganisationID = "";
            string OrganisationName = "";
            string Clinic_Number = "";
            string Service_Install_Id = "";
            int ZohoLocalDBID = 0;
            try
            {
                bool IsAdminPrivileges = IsRunningAsAdministrator();
                if (!IsAdminPrivileges)
                {
                    Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_ZohoInstall_FromEvent : AditEventListener is not running with Administrator Privileges.");
                    Utility.WriteToErrorLogFromAll("[SynchDataLiveDB_Pull_ZohoInstall_FromEvent Sync] : AditEventListener is not running with Administrator Privileges.");
                    return false;
                }

                if (!strContent.StartsWith("{") && strContent.StartsWith("data:"))
                {
                    strContent = strContent.Substring(strContent.IndexOf("{"), strContent.Length - strContent.IndexOf("{"));
                }

                ZohoInstallInfoBO EHR_InfoDto;
                try
                {
                    EHR_InfoDto = Newtonsoft.Json.JsonConvert.DeserializeObject<ZohoInstallInfoBO>(strContent);
                }
                catch
                {
                    strContent = strContent.Replace("\"data\":{", "\"data\":[{").Replace("}}", "}]}");
                    EHR_InfoDto = Newtonsoft.Json.JsonConvert.DeserializeObject<ZohoInstallInfoBO>(strContent);
                }


                if (EHR_InfoDto != null && EHR_InfoDto.data != null && EHR_InfoDto.data.Count > 0)
                {
                    EHRUser = EHR_InfoDto.data[0].ehr_user;
                    EHRPass = EHR_InfoDto.data[0].ehr_pass;
                    WindowsUser = EHR_InfoDto.data[0].server_user;
                    WindowsPass = EHR_InfoDto.data[0].server_pass;
                    Is_Valid = EHR_InfoDto.data[0].is_valid;
                    Is_Confirmed = EHR_InfoDto.data[0].is_confirmed;
                    Is_Installed = EHR_InfoDto.data[0].is_installed;
                    UserID = EHR_InfoDto.data[0].userId;
                    UserName = EHR_InfoDto.data[0].user_name;

                    OrganisationID = EHR_InfoDto.data[0].organizationId;
                    OrganisationName = Utility.Organization_Name.ToString();
                    LocationId = EHR_InfoDto.data[0].locationId;
                    try
                    {
                        LocationName = Convert.ToString(Utility.DtLocationList.Select("Loc_Id = '" + LocationId + "'").FirstOrDefault()["name"]);
                    }
                    catch
                    {
                    }

                    EHRUserResult = false;

                    DataTable dtLocalZohoDetails = SynchLocalBAL.GetLocalZohoDetailsData();
                    DataRow drLocalZohoDetails = dtLocalZohoDetails.Select("Location_Id = '" + LocationId + "'").FirstOrDefault();

                    #region CheckDetails
                    bool blnCheck = false;
                    try
                    {
                        if (drLocalZohoDetails != null)
                        {
                            if (drLocalZohoDetails["Zoho_LocalDB_ID"] != DBNull.Value)
                            {
                                ZohoLocalDBID = Convert.ToInt32(drLocalZohoDetails["Zoho_LocalDB_ID"]);
                            }

                            string strTmpServerUser = "";
                            if (drLocalZohoDetails["Server_User"] != DBNull.Value)
                            {
                                strTmpServerUser = Convert.ToString(drLocalZohoDetails["Server_User"]);
                                Utility.WindowsUserName = strTmpServerUser;
                            }
                            if (strTmpServerUser != WindowsUser.ToString().Trim())
                            {
                                Utility.WindowsUserName = WindowsUser;
                                Utility.WindowsUserResult = false;
                                blnCheck = true;
                            }


                            string strTmpServerPass = "";
                            if (drLocalZohoDetails["Server_Pass"] != DBNull.Value)
                            {
                                strTmpServerPass = Convert.ToString(drLocalZohoDetails["Server_Pass"]);
                                Utility.WindowsUserPassword = strTmpServerPass;
                            }
                            if (strTmpServerPass != WindowsPass.ToString().Trim())
                            {
                                Utility.WindowsUserPassword = WindowsPass;
                                Utility.WindowsUserResult = false;
                                blnCheck = true;
                            }

                            bool blnTmpIsConfirmed = false;
                            if (drLocalZohoDetails["Is_Confirmed"] != DBNull.Value)
                            {
                                blnTmpIsConfirmed = Convert.ToBoolean(drLocalZohoDetails["Is_Confirmed"]);
                                Utility.IsConfirmed = blnTmpIsConfirmed;
                            }
                            if (blnTmpIsConfirmed != Is_Confirmed)
                            {
                                Utility.IsConfirmed = Is_Confirmed;
                                if (blnTmpIsConfirmed == false && Is_Confirmed == true)
                                {
                                    Utility.WindowsUserResult = false;
                                    blnCheck = true;
                                }
                            }


                            bool blnTmpIsValid = false;
                            if (drLocalZohoDetails["Is_Valid"] != DBNull.Value)
                            {
                                blnTmpIsValid = Convert.ToBoolean(drLocalZohoDetails["Is_Valid"]);
                                Utility.WindowsUserResult = blnTmpIsValid;
                            }

                            Utility.WindowsUserResult = Convert.ToBoolean(Is_Valid);
                            if (Utility.WindowsUserResult == false)
                            {
                                blnCheck = true;
                            }
                        }
                        else
                        {
                            blnCheck = true;
                            Utility.WindowsUserName = WindowsUser;
                            Utility.WindowsUserPassword = WindowsPass;
                            Utility.WindowsUserResult = false;
                            Utility.IsConfirmed = Is_Confirmed;
                        }
                    }
                    catch
                    {
                        blnCheck = true;
                    }


                    Utility.IsInstalled = CheckAppExist();
                    if (Is_Installed == false && Is_Confirmed == true)
                    {
                        blnCheck = true;
                    }

                    if (blnCheck && Is_Confirmed)
                    {
                        if (!string.IsNullOrEmpty(Utility.WindowsUserName) && !string.IsNullOrEmpty(Utility.WindowsUserPassword))
                        {                        
                            WindowsUserResult = IsValidUser(Utility.WindowsUserName, AESHelper.Decrypt2(Utility.WindowsUserPassword), ref strErrMsg);
                            if (Utility.WindowsUserResult == false && WindowsUserResult == true)
                            {
                                UpdateTaskScheduler(Utility.WindowsUserName, AESHelper.Decrypt2(Utility.WindowsUserPassword), false);
                                UpdateWindowsService(Utility.WindowsUserName, AESHelper.Decrypt2(Utility.WindowsUserPassword), false);
                            }
                        }
                        Is_Valid = WindowsUserResult;
                    }

                    if (WindowsUserResult != Utility.WindowsUserResult)
                    {
                        Utility.WindowsUserResult = WindowsUserResult;
                        SynchDataLiveDB_Push_EHRAndSystemCreds_Results();
                    }



                    if (Utility.WindowsUserResult == true && Utility.IsConfirmed == true && Utility.IsInstalled == false)
                    {
                        Utility.ZohoInstall = "install";
                        Utility.IsInstalled = InstallZohoAccess(strLocationName, strOrgName, System.Environment.MachineName);
                        Is_Installed = Utility.IsInstalled;
                        SynchLocalBAL.Save_ZohoDetailsData(ZohoLocalDBID, OrganisationID, OrganisationName, LocationId, LocationName, EHRPass, EHRUser, WindowsUser, WindowsPass, Is_Confirmed, Is_Installed, Convert.ToBoolean(Is_Valid), UserID, UserName, Clinic_Number, Service_Install_Id);
                        SynchDataLiveDB_Push_ZohoInstall_Results();
                    }
                    
                    #endregion

                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLiveDB_Pull_ZohoInstall_FromEvent :" + ex.Message);
                Utility.WriteToErrorLogFromAll("[SynchDataLiveDB_Pull_ZohoInstall_FromEvent Sync] : " + ex.Message);
                return false;
            }
        }
        #endregion

        #region SendAcknowledgement
        public static void SendAcknowledgement(string strType, string strWebId, string strLocationID)
        {
            try
            {
                SynchDataLiveDB_Send_EventAcknowledgement(strType, strWebId, strLocationID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        private static bool IsRunningAsAdministrator()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }




    }
}
