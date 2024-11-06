using Newtonsoft.Json;
using Pozative.BAL;
using Pozative.BO;
using Pozative.UTL;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pozative
{
    public partial class frmClinicConfiguration : Form
    {

        #region Variable

        GoalBase ObjGoalBase = new GoalBase();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        DataTable dtTempLocationTable = new DataTable();
        DataTable dtTempApptLocationTable = new DataTable();
        DataTable dtTempClinic = new DataTable();

        DataTable dtTempAddClinic = new DataTable();

        string UserAditLocationLinkList = "";
        #endregion

        #region Form_Load

        public frmClinicConfiguration()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            this.ShowDialog();
        }
        private DataTable CreateTempAppointmentLocationTableData()
        {
            DataTable dtApptLoc = new DataTable();
            dtApptLoc.Clear();
            dtApptLoc.Columns.Add("id", typeof(string));
            dtApptLoc.Columns.Add("name", typeof(string));
            dtApptLoc.Columns.Add("google_address", typeof(string));
            dtApptLoc.Columns.Add("phone", typeof(string));
            dtApptLoc.Columns.Add("email", typeof(string));
            dtApptLoc.Columns.Add("address", typeof(string));
            dtApptLoc.Columns.Add("timezone", typeof(string));
            dtApptLoc.Columns.Add("website_url", typeof(string));
            dtApptLoc.Columns.Add("system_mac_address", typeof(string));
            dtApptLoc.AcceptChanges();
            return dtApptLoc;
        }

        private DataTable CreateTempAddClinicData()
        {
            DataTable DtAddClinic = new DataTable();
            DtAddClinic.Clear();
            DtAddClinic.Columns.Add("Clinic_Number", typeof(string));
            DtAddClinic.Columns.Add("Clinic_Name", typeof(string));
            DtAddClinic.Columns.Add("Location", typeof(string));
            DtAddClinic.Columns.Add("Location_Id", typeof(string));
            DtAddClinic.Columns.Add("Is_Location_Config", typeof(bool));
            DtAddClinic.Columns.Add("Service_Install_Id", typeof(string));
            DtAddClinic.Columns.Add("AditSync", typeof(string));
            DtAddClinic.AcceptChanges();
            return DtAddClinic;
        }
        private DataTable CreateTempClinic()
        {
            DataTable DtAddClinic = new DataTable();
            DtAddClinic.Clear();
            DtAddClinic.Columns.Add("Clinic_Number", typeof(string));
            DtAddClinic.Columns.Add("Description", typeof(string));
            DtAddClinic.AcceptChanges();
            return DtAddClinic;
        }
        private DataTable CreateTempLocationTableData()
        {
            DataTable dtLoc = new DataTable();
            dtLoc.Clear();
            dtLoc.Columns.Add("id", typeof(string));
            dtLoc.Columns.Add("name", typeof(string));
            dtLoc.Columns.Add("google_address", typeof(string));
            dtLoc.Columns.Add("phone", typeof(string));
            dtLoc.Columns.Add("email", typeof(string));
            dtLoc.Columns.Add("address", typeof(string));
            dtLoc.Columns.Add("timezone", typeof(string));
            dtLoc.Columns.Add("website_url", typeof(string));
            dtLoc.Columns.Add("User_ID", typeof(string));
            dtLoc.Columns.Add("Loc_ID", typeof(string));
            dtLoc.Columns.Add("Clinic_Number", typeof(string));
            dtLoc.Columns.Add("OrganazationId", typeof(string));
            dtLoc.Columns.Add("Service_Install_Id", typeof(string));
            dtLoc.AcceptChanges();
            return dtLoc;

        }
        #endregion

        #region Button Click

        private void frmClinicConfiguration_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                btnCancel.BackColor = WDSColor.ButtonBackColor;
                btnCancel.ForeColor = WDSColor.ButtonForeColor;

                btnLocationSave.ForeColor = WDSColor.SaveButtonForeColor;
                btnLocationSave.BackColor = WDSColor.SaveButtonBackColor;

                btnCancel.Font = WDSColor.FormButtonFont;
                btnLocationSave.Font = WDSColor.FormButtonFont;

                string strAditLogin = SystemBAL.GetAdminUserLoginEmailIdPass();
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var clientLogin = new RestClient(strAditLogin);
                var requestLogin = new RestRequest(Method.POST);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                requestLogin.AddHeader("Postman-Token", "1d16df4c-48ba-4644-bc7a-9bcef2a86744");
                requestLogin.AddHeader("cache-control", "no-cache");
                requestLogin.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                AditLoginPostBO AditLoginPost = new AditLoginPostBO
                {
                    email = Utility.Adit_User_Email_Id.Trim(),
                    password = Utility.Adit_User_Email_Password.Trim()
                };
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(AditLoginPost);
                requestLogin.AddHeader("cache-control", "no-cache");
                requestLogin.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                IRestResponse responseLogin = clientLogin.Execute(requestLogin);

                if (responseLogin.StatusCode.ToString().ToLower() == "ServiceUnavailable".ToString().ToLower())
                {
                    ObjGoalBase.ErrorMsgBox("Adit Server", "Service Unavailable.");
                    return;
                }

                if (responseLogin.ErrorMessage != null)
                {
                    ObjGoalBase.ErrorMsgBox("Adit Server", "Server is down. Please try again after a few minutes.");
                    return;
                }

                var AdminLoginDto = JsonConvert.DeserializeObject<AdminLoginDetailBO>(responseLogin.Content);
                if (AdminLoginDto == null)
                {
                    ObjGoalBase.ErrorMsgBox("Adit App Admin User Authentication", responseLogin.ErrorMessage.ToString());
                    return;
                }
                if (AdminLoginDto.status == "true")
                {
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("Adit App Admin User Authentication", AdminLoginDto.message.ToString());
                    return;
                }

                UserAditLocationLinkList = string.Empty;
                string strApiLocOrg = SystemBAL.GetApiAditLocationAndOrganizationByAdminIdPassword();
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var client = new RestClient(strApiLocOrg);
                var request = new RestRequest(Method.POST);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("Postman-Token", "1d16df4c-48ba-4644-bc7a-9bcef2a86744");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                AditLoginPostBO AditLocOrgPost = new AditLoginPostBO
                {
                    email = Utility.Adit_User_Email_Id.Trim(),
                    password = Utility.Adit_User_Email_Password.Trim(),
                    created_by = Utility.User_ID
                };
                var javaScriptSerializer1 = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString1 = javaScriptSerializer1.Serialize(AditLocOrgPost);
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("application/json", jsonString1, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    ObjGoalBase.ErrorMsgBox("Adit App Admin User Authentication", response.ErrorMessage);
                    return;
                }

                UserAditLocationLinkList = response.Content;
                var customerDto = JsonConvert.DeserializeObject<LocationDetailBO>(response.Content);

                dtTempApptLocationTable = CreateTempAppointmentLocationTableData();
                dtTempApptLocationTable.Clear();
                DataRow drApptLocDef = dtTempApptLocationTable.NewRow();
                drApptLocDef["id"] = "0";
                drApptLocDef["name"] = "<Select>";
                dtTempApptLocationTable.Rows.Add(drApptLocDef);
                dtTempApptLocationTable.AcceptChanges();

                for (int i = 0; i < customerDto.data.Count; i++)
                {
                    DataRow drApptLoc = dtTempApptLocationTable.NewRow();
                    drApptLoc["id"] = customerDto.data[i]._id.ToString();
                    drApptLoc["name"] = customerDto.data[i].name.ToString();
                    drApptLoc["system_mac_address"] = customerDto.data[i].system_mac_address.ToString();
                    dtTempApptLocationTable.Rows.Add(drApptLoc);
                    dtTempApptLocationTable.AcceptChanges();
                }

                dtTempAddClinic = CreateTempAddClinicData();

                DataView dv = dtTempApptLocationTable.DefaultView;
                dv.Sort = "name";
                dtTempApptLocationTable = dv.ToTable();

                DGVMuliClinc.DataSource = dtTempAddClinic;
                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).DataSource = dtTempApptLocationTable.Copy();
                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).ValueMember = "id";
                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).DisplayMember = "name";

                cboDatabaseList.DataSource = Utility.DtInstallServiceList;
                cboDatabaseList.DisplayMember = "Database";
                cboDatabaseList.ValueMember = "Installation_ID";

                for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                {
                    if (DGVMuliClinc.Rows[i].Cells["Location"].Value.ToString() == "")
                    {
                        DGVMuliClinc.Rows[i].Cells["Location"].Value = "0";
                    }
                    else
                    {
                        string strApiAditLocationSyncEnable = SystemBAL.AditLocationSyncEnable(DGVMuliClinc.Rows[i].Cells["Location_Id"].Value.ToString(), Utility.User_ID);
                        client = new RestClient(strApiAditLocationSyncEnable);
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                        request = new RestRequest(Method.GET);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("Authorization", Utility.WebAdminUserToken);
                        request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(DGVMuliClinc.Rows[i].Cells["Location_Id"].Value.ToString()));
                        response = client.Execute(request);
                        if (response.ErrorMessage != null)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[AditLocationSyncEnable] : " + response.ErrorMessage);
                            return;
                        }
                        var IsAditLocationSyncEnable = JsonConvert.DeserializeObject<AditLocationSyncBO>(response.Content);
                        try
                        {
                            DGVMuliClinc.Rows[i].Cells["AditSync"].Value = Convert.ToBoolean(IsAditLocationSyncEnable.data.ehr_sync_status) == true ? "ON" : "OFF";
                            Utility.imageuploadbatch = Convert.ToInt16(IsAditLocationSyncEnable.data.imageuploadbatch);

                        }
                        catch (Exception)
                        {
                            DGVMuliClinc.Rows[i].Cells["AditSync"].Value = "ON";//Utility.ApptAutoBook                            
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Add Clinic-Load", ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Password-Close", ex.Message);
            }
        }

        private void btnLocationSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                {
                    if ((DGVMuliClinc.Rows[i].Cells["Location"] as DataGridViewComboBoxCell).Value != null && Convert.ToBoolean(DGVMuliClinc.Rows[i].Cells["Is_Location_Config"].Value) == false)
                    {
                        string SelectedValue = Convert.ToString((DGVMuliClinc.Rows[i].Cells["Location"] as DataGridViewComboBoxCell).Value.ToString());
                        if (SelectedValue != "0")
                        {
                            //bool IsExist = DGVMuliClinc.Rows.Cast<DataGridViewRow>()
                            //          .Count(c =>
                            //          c.Cells["Location"]
                            //          .EditedFormattedValue.ToString() == DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString()) > 1;

                            //if (IsExist)
                            //{
                            //    ObjGoalBase.ErrorMsgBox("Adit Location Name", "Please Select One Location For One Clinic.");
                            //    DGVMuliClinc.Rows[i].Cells["Location"].Selected = true;
                            //    return;
                            //}

                            string Pid = string.Empty;
                            DataRow[] PozLocrow = dtTempApptLocationTable.Copy().Select("id = '" + SelectedValue.Trim() + "' ");
                            if (PozLocrow.Length > 0)
                            {
                                Pid = PozLocrow[0]["system_mac_address"].ToString().Trim();
                            }

                            if (Pid.ToString().Trim() != string.Empty && Pid.ToString().Trim() != "0")
                            {
                                if (Utility.System_processorID.ToString() != Pid.ToString().Trim())
                                {
                                    ObjGoalBase.ErrorMsgBox("Location Name", DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString() + " Location is already configured with another system.");
                                    return;
                                }
                            }

                            if (Utility.DtLocationList.Select("Location_Id = '" + SelectedValue + "' ").Count() > 0)
                            {
                                ObjGoalBase.ErrorMsgBox("Location Name", DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString() + " Location is already configured with another Clinic Or Database...");
                                return;
                            }
                        }
                    }
                }

                var customerDto = JsonConvert.DeserializeObject<LocationDetailBO>(UserAditLocationLinkList);

                dtTempLocationTable = CreateTempLocationTableData();
                for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                {
                    var resultProvider = customerDto.data.AsEnumerable().Where(o => DGVMuliClinc.Rows[i].Cells["Location"].Value.ToString().ToUpper() == o._id.ToString().ToUpper() && Convert.ToBoolean(DGVMuliClinc.Rows[i].Cells["Is_Location_Config"].Value) == false);

                    if (resultProvider.Count() > 0)
                    {
                        List<Pozative.BO.MainData> o = resultProvider.ToList();

                        DataRow drLoc = dtTempLocationTable.NewRow();
                        drLoc["id"] = o[0]._id.ToString();
                        drLoc["name"] = o[0].name.ToString();
                        drLoc["phone"] = o[0].phone.ToString();
                        drLoc["email"] = o[0].email.ToString();
                        drLoc["address"] = string.Empty;
                        drLoc["timezone"] = Utility.LocationTimeZone;
                        drLoc["website_url"] = string.Empty;
                        drLoc["User_ID"] = Utility.User_ID;
                        drLoc["Loc_ID"] = o[0].Location._id.ToString();
                        drLoc["Clinic_Number"] = DGVMuliClinc.Rows[i].Cells["Clinic_Number"].Value.ToString();
                        drLoc["OrganazationId"] = o[0].organization._id.ToString();
                        drLoc["Service_Install_Id"] = DGVMuliClinc.Rows[i].Cells["Service_Install_Id"].Value.ToString();
                        dtTempLocationTable.Rows.Add(drLoc);
                    }
                }

                string strApiEHRList = SystemBAL.GetApiERHListWithWebId();
                var client = new RestClient(strApiEHRList);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var request = new RestRequest(Method.GET);
                //request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                //request.AddHeader("cache-control", "no-cache");

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);

                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    ObjGoalBase.WriteToErrorLogFile("[GetApiERHListWithWebId] : " + response.ErrorMessage);
                    return;
                }

                var GetEHRListWithWebId = JsonConvert.DeserializeObject<EHRListWithWebIdBO>(response.Content);
                //MessageBox.Show(response.Content.ToString());
                string tmpEHRMaster = string.Empty;
                foreach (var item in GetEHRListWithWebId.data)
                {
                    if (item.name.ToString().ToLower() == Utility.Application_Name.ToLower() && item.version.ToString().ToLower() == Utility.Application_Version.ToString().ToLower())
                    {
                        tmpEHRMaster = item._id.ToString();
                    }
                }

                if (tmpEHRMaster == string.Empty)
                {
                    if (Utility.Application_Name.ToUpper() == "SOFTDENT")
                    {
                        ObjGoalBase.ErrorMsgBox("EHR Configuration", Utility.Application_Name + " With " + Utility.Application_Version + " is under development");
                    }
                    else
                    {
                        ObjGoalBase.ErrorMsgBox("EHR Configuration", Utility.Application_Name + " With " + Utility.Application_Version + " cannot configure with Adit app.");
                    }
                    return;
                }


                for (int i = 0; i < dtTempLocationTable.Rows.Count; i++)
                {
                    LocationEHRBO LEHR = new LocationEHRBO
                    {
                        application_name = Utility.Application_Name,
                        application_version = Utility.Application_Version,
                        system_name = System.Environment.MachineName,
                        system_mac_address = Utility.System_processorID,
                        is_install_ehr = true,
                        install_date = Utility.Datetimesetting().ToString("yyyy-MM-ddT00:00:00"),
                        ehrmaster = tmpEHRMaster,
                        created_by = Utility.User_ID
                    };
                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonString = javaScriptSerializer.Serialize(LEHR);

                    string staLocation_EHR = PushLiveDatabaseBAL.Push_Location_EHR(jsonString, dtTempLocationTable.Rows[i]["id"].ToString());

                    if (staLocation_EHR == string.Empty)
                    {
                        ObjGoalBase.WriteToErrorLogFile("EHR Location " + staLocation_EHR);
                        return;
                    }
                    else
                    {
                        ObjGoalBase.WriteToSyncLogFile(Utility.Application_Name + "  Location " + dtTempLocationTable.Rows[i]["name"].ToString() + " configuration save successfully.");
                        ObjGoalBase.WriteToSyncLogFile("EHR Location save Successfully.");
                    }

                    SystemBAL.Save_LocationDetail(dtTempLocationTable.Rows[i]["id"].ToString(),
                                                                dtTempLocationTable.Rows[i]["name"].ToString(),
                                                                dtTempLocationTable.Rows[i]["website_url"].ToString(),
                                                                dtTempLocationTable.Rows[i]["phone"].ToString(),
                                                                dtTempLocationTable.Rows[i]["email"].ToString(),
                                                                dtTempLocationTable.Rows[i]["address"].ToString(),
                                                                Utility.HostName_Adit.Replace("api", "app"),
                                                                "EN",
                                                                dtTempLocationTable.Rows[i]["name"].ToString(),
                                                                "1",
                                                                dtTempLocationTable.Rows[i]["OrganazationId"].ToString(),
                                                                Utility.User_ID,
                                                                dtTempLocationTable.Rows[i]["Loc_ID"].ToString(),
                                                                "Insert",
                                                                dtTempLocationTable.Rows[i]["Clinic_Number"].ToString(),
                                                                dtTempLocationTable.Rows[i]["Service_Install_Id"].ToString());

                    if (Utility.DtInstallServiceList.Select("Installation_Id = '" + dtTempLocationTable.Rows[i]["Service_Install_Id"].ToString() + "' And Location_Id = '' ").Count() != 0)
                    {
                        SystemBAL.UpdateLocationId_InstallApplicationDetail(dtTempLocationTable.Rows[i]["id"].ToString(), dtTempLocationTable.Rows[i]["Service_Install_Id"].ToString());
                    }
                }

                Utility.DtLocationList = SystemBAL.GetLocationDetail();
                Utility.DtInstallServiceList = SystemBAL.GetInstallServiceDetail();

                this.Close();
            }
            catch (Exception Ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Adit Clinic Config Save] : " + Ex.Message);
            }
        }
        #endregion

        #region Common Event

        private void lblFormHead_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit Clinic Config Cancel", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        private void cboDatabaseList_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboDatabaseList.SelectedValue != null)
                {
                    dtTempAddClinic = CreateTempAddClinicData();

                    if (Utility.Application_ID == 1)
                    {
                        //*Commented for EagleSoft NSite* Start
                        dtTempClinic = CreateTempClinic();
                        DataRow DRow = dtTempClinic.NewRow();
                        DRow["Clinic_Number"] = "0";
                        DRow["Description"] = "head Quarters";
                        dtTempClinic.Rows.Add(DRow);
                        
                        //DataRow DRow = ((System.Data.DataRowView)(cboDatabaseList.SelectedItem)).Row;
                        //dtTempClinic = SynchEaglesoftBAL.GetEagleSoftSiteData(DRow["DBConnString"].ToString());
                        //bool bflgNoRows = false;
                        //if (dtTempClinic != null)
                        //{
                        //    if (dtTempClinic.Rows.Count <= 0)
                        //    {
                        //        bflgNoRows = true;
                        //    }
                        //    //else
                        //    //{
                        //    //    DataRow drFirstrow = dtTempClinic.NewRow();
                        //    //    drFirstrow["Clinic_Number"] = "0";
                        //    //    drFirstrow["Description"] = "Head Quartors";
                        //    //    dtTempClinic.Rows.InsertAt(drFirstrow, 0);
                        //    //}
                        //}
                        //else
                        //{
                        //    bflgNoRows = true;
                        //}
                        //if (bflgNoRows)
                        //{
                        //    dtTempClinic = new DataTable();
                        //    dtTempClinic = CreateTempClinic();
                        //    DataRow newDRow = dtTempClinic.NewRow();
                        //    newDRow["Clinic_Number"] = "0";
                        //    newDRow["Description"] = "head Quarters";
                        //    dtTempClinic.Rows.Add(newDRow);
                        //}
                        //*Commented for EagleSoft NSite* End
                    }
                    else if (Utility.Application_ID == 2)
                    {
                        DataRow DRow = ((System.Data.DataRowView)(cboDatabaseList.SelectedItem)).Row;
                        dtTempClinic = SynchOpenDentalBAL.GetOpenDentalClinicData(DRow["DBConnString"].ToString());
                    }
                    else if (Utility.Application_ID == 10)
                    {
                        DataRow DRow = ((System.Data.DataRowView)(cboDatabaseList.SelectedItem)).Row;
                        dtTempClinic = SynchPracticeWebBAL.GetPracticeWebClinicData(DRow["DBConnString"].ToString());
                    }
                    else if (Utility.Application_ID == 12)
                    {                       
                        DataRow DRow = ((System.Data.DataRowView)(cboDatabaseList.SelectedItem)).Row;
                        dtTempClinic = AditCrystalPM.BAL.Cls_Sync_Common.GetCrystalPMClinicData(DRow["DBConnString"].ToString());
                    }

                    //if (Utility.DtLocationList.Select("Service_Install_Id = '" + cboDatabaseList.SelectedValue.ToString() + "' ").Count() == 0)
                    //{
                    //    DataRow Dr = dtTempAddClinic.NewRow();
                    //    Dr["Clinic_Name"] = "head Quarters";
                    //    Dr["Clinic_Number"] = "0";
                    //    //Dr["Location"] = Utility.DtLocationList.Rows[i]["Name"].ToString();
                    //    Dr["Location_Id"] = "";
                    //    Dr["Is_Location_Config"] = false;
                    //    Dr["Service_Install_Id"] = cboDatabaseList.SelectedValue;
                    //    dtTempAddClinic.Rows.Add(Dr);
                    //}
                    //else
                    //{
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (cboDatabaseList.SelectedValue.ToString() == Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString())
                        //if (((System.Data.DataRowView)cboDatabaseList.SelectedValue).Row["Installation_Id"].ToString() == Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString()) 
                        {
                            DataRow Dr = dtTempAddClinic.NewRow();
                            //if (Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim() == "0")
                            //    Dr["Clinic_Name"] = "head Quarters";
                            //else
                            //{
                            DataRow[] row = dtTempClinic.Copy().Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' ");
                            //*Commented for EagleSoft NSite* Start
                            if (row.Length > 0)
                                Dr["Clinic_Name"] = row[0]["description"].ToString().Trim();
                            //if (row.Length > 0)
                            //{
                            //    Dr["Clinic_Name"] = row[0]["description"].ToString().Trim();
                            //}
                            //else
                            //{
                            //    if ((Utility.Application_ID == 1) && Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim() == "0")
                            //    {
                            //        Dr["Clinic_Name"] = "Head Quarters";
                            //    }
                            //}
                            //*Commented for EagleSoft NSite* End
                            //}
                            Dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                            //Dr["Location"] = Utility.DtLocationList.Rows[i]["Name"].ToString();
                            Dr["Location_Id"] = Utility.DtLocationList.Rows[i]["Location_Id"].ToString();
                            Dr["Is_Location_Config"] = true;
                            Dr["Service_Install_Id"] = cboDatabaseList.SelectedValue;
                            dtTempAddClinic.Rows.Add(Dr);
                        }
                    }
                    //}

                    foreach (DataRow dtDtxRow in dtTempClinic.Rows)
                    {
                        try
                        {
                            DataRow[] row = dtTempAddClinic.Copy().Select("Clinic_Number = '" + dtDtxRow["Clinic_Number"].ToString().Trim() + "'");
                            if (row.Length > 0)
                            {
                            }
                            else
                            {
                                DataRow Dr = dtTempAddClinic.NewRow();

                                Dr["Clinic_Name"] = dtDtxRow["description"].ToString().Trim();
                                Dr["Clinic_Number"] = dtDtxRow["Clinic_Number"].ToString().Trim();
                                Dr["Is_Location_Config"] = false;
                                Dr["Service_Install_Id"] = cboDatabaseList.SelectedValue;
                                dtTempAddClinic.Rows.Add(Dr);
                            }
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.ErrorMsgBox("Clinic Config Table", ex.Message);
                        }
                    }
                }
                else
                {
                    if (dtTempAddClinic != null)
                        dtTempAddClinic.Rows.Clear();
                }

                DGVMuliClinc.DataSource = dtTempAddClinic;
                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).DataSource = dtTempApptLocationTable.Copy();
                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).ValueMember = "id";
                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).DisplayMember = "name";

                for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                {
                    DGVMuliClinc.Rows[i].Cells["Location"].Value = DGVMuliClinc.Rows[i].Cells["Location_Id"].Value;
                    if (DGVMuliClinc.Rows[i].Cells["Location"].EditedFormattedValue != "")
                        DGVMuliClinc.Rows[i].Cells["Location"].ReadOnly = true;
                }
            }
            catch (Exception Ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit Clinic Config Value Change", Ex.Message);
            }
        }
    }
}
