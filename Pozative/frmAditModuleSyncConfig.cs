using Pozative.BAL;
using Pozative.Properties;
using Pozative.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;
using TaskScheduler;
using Microsoft.Win32;

namespace Pozative
{
    public partial class frmAditModuleSyncConfig : Form
    {

        #region Variable

        public bool StatusAditSyncProcess;
        GoalBase ObjGoalBase = new GoalBase();

        DataTable TempdtAditModuleSyncTime = new DataTable();
        bool Is_ServiceInstalled = false;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        DataTable dtTempClinicTable = new DataTable();
        bool BlnMultiDatabase = false;
        #endregion

        #region Form Load

        public frmAditModuleSyncConfig()
        {
            InitializeComponent();
        }

        private void frmAditModuleSyncConfig_Load(object sender, EventArgs e)
        {
            try
            {

                if (Utility.Application_ID == 6)
                {
                    txtTimeslot.Visible = true;
                    lblTimeSlot.Visible = true;
                }
                else
                {
                    txtTimeslot.Visible = false;
                    lblTimeSlot.Visible = false;
                }

                Cursor.Current = Cursors.WaitCursor;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                tt.SetToolTip(picNotAllowToChangeSystemDateFormat, "By default pozative change the date format to work properly. In some specific case, it can be true. Default value is false");
                tt.SetToolTip(picDontAskPasswordOnSaveSetting, "By default on save this changes this screen will ask for password, Turn this settin on to do not ask password on Save.");

                TempdtAditModuleSyncTime = CreateTempdtAditModuleSyncTime();

                lblNote.Text = "Set minute value between [1-1440] to represent how often you would like for your EHR database to sync with the Adit application.";
                lblNote.ForeColor = Color.DarkGreen;

                lblFormHead.Anchor = System.Windows.Forms.AnchorStyles.Left;

                this.BackColor = WDSColor.FormHeadBackColor;
                tblformHead.BackColor = WDSColor.FormHeadBackColor;
                lblFormHead.ForeColor = WDSColor.FormHeadForeColor;
                tblBody.BackColor = Color.White;
                btnClose.ForeColor = WDSColor.FormHeadForeColor;

                btnRestore.BackColor = WDSColor.ButtonBackColor;
                btnRestore.ForeColor = WDSColor.ButtonForeColor;
                btnSaveConfig.BackColor = WDSColor.SaveButtonBackColor;
                btnSaveConfig.ForeColor = WDSColor.SaveButtonForeColor;
                BtnAddClinic.ForeColor = WDSColor.SaveButtonForeColor;
                BtnAddClinic.BackColor = WDSColor.SaveButtonBackColor;

                BtnAddDatabase.ForeColor = WDSColor.SaveButtonForeColor;
                BtnAddDatabase.BackColor = WDSColor.SaveButtonBackColor;

                btnCancel.BackColor = WDSColor.ButtonBackColor;
                btnCancel.ForeColor = WDSColor.ButtonForeColor;


                btnRestore.Font = WDSColor.FormButtonFont;
                btnSaveConfig.Font = WDSColor.FormButtonFont;
                btnCancel.Font = WDSColor.FormButtonFont;
                BtnAddClinic.Font = WDSColor.FormButtonFont;
                BtnAddDatabase.Font = WDSColor.FormButtonFont;

                SetModuleDefaultValue();

                if (Utility.Application_ID == 1) // EagleSoft
                {
                    BtnAddDatabase.Visible = true;
                    if (Utility.DtInstallServiceList.Rows.Count > 0)
                        BtnAddClinic.Visible = true;
                }
                else if (Utility.Application_ID == 2) // Open Dental
                {
                    BtnAddDatabase.Visible = true;

                    if (Utility.DtInstallServiceList.Rows.Count > 0)
                        BtnAddClinic.Visible = true;

                    else
                    {
                        dtTempClinicTable = SynchOpenDentalBAL.GetOpenDentalClinicData(Utility.DBConnString);

                        if (dtTempClinicTable.Rows.Count > 1)
                            BtnAddClinic.Visible = true;
                        else
                            BtnAddClinic.Visible = false;
                    }
                }
                else if (Utility.Application_ID == 12) // CrystalPM
                {
                  //  BtnAddDatabase.Visible = true;

                    if (Utility.DtInstallServiceList.Rows.Count > 0)
                        BtnAddClinic.Visible = true;

                    else
                    {
                        dtTempClinicTable = AditCrystalPM.BAL.Cls_Sync_Common.GetCrystalPMClinicData(Utility.DBConnString);

                        if (dtTempClinicTable.Rows.Count > 1)
                            BtnAddClinic.Visible = true;
                        else
                            BtnAddClinic.Visible = false;
                    }
                }
                else if (Utility.Application_ID == 13) // OfficeMate
                {
                    try
                    {
                        //  BtnAddDatabase.Visible = true;

                        if (Utility.DtInstallServiceList.Rows.Count > 0)
                            BtnAddClinic.Visible = true;

                        else
                        {
                            dtTempClinicTable = AditOfficeMate.BAL.Cls_Synch_Patient.GetOfficeMateClinicData(Utility.DBConnString);

                            if (dtTempClinicTable.Rows.Count > 1)
                                BtnAddClinic.Visible = true;
                            else
                                BtnAddClinic.Visible = false;
                        }
                    }
                    catch (Exception)
                    {

                    }                    
                }


                DataTable dtService_Installation = SystemBAL.GetService_Installation();
                if (dtService_Installation != null && dtService_Installation.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dtService_Installation.Rows[0]["DontAskPasswordOnSaveSetting"].ToString()) == true)
                    {
                        picDontAskPasswordOnSaveSetting.Image = Resources.ON;
                        picDontAskPasswordOnSaveSetting.Tag = "ON";
                    }
                    else
                    {
                        picDontAskPasswordOnSaveSetting.Image = Resources.OFF;
                        picDontAskPasswordOnSaveSetting.Tag = "OFF";
                    }

                    if (Convert.ToBoolean(dtService_Installation.Rows[0]["NotAllowToChangeSystemDateFormat"].ToString()) == true)
                    {
                        picNotAllowToChangeSystemDateFormat.Image = Resources.ON;
                        picNotAllowToChangeSystemDateFormat.Tag = "ON";
                    }
                    else
                    {
                        picNotAllowToChangeSystemDateFormat.Image = Resources.OFF;
                        picNotAllowToChangeSystemDateFormat.Tag = "OFF";
                    }
                }


                DataTable dtAditModuleSyncTime = SystemBAL.GetAditModuleSyncTime();

                if (dtAditModuleSyncTime != null && dtAditModuleSyncTime.Rows.Count > 0)
                {
                    // Appointment
                    DataRow[] AppointmentRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblAppointment.Text.ToString().ToLower().Trim() + "'");
                    if (AppointmentRow.Length > 0)
                    {
                        txtApptEHRTime.Text = AppointmentRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtApptPullTime.Text = AppointmentRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtApptPushTime.Text = AppointmentRow[0]["SyncModule_Push"].ToString().Trim();

                        if (txtApptEHRTime.Text == txtApptPullTime.Text && txtApptEHRTime.Text == txtApptPushTime.Text)
                        {
                            chkAppt.Checked = true;
                        }
                        else
                        {
                            chkAppt.Checked = false;
                        }
                    }

                    //OperatoryEvent
                    DataRow[] OperatoryEventRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblOperatoryEvent.Tag.ToString().ToLower().Trim() + "'");
                    if (OperatoryEventRow.Length > 0)
                    {
                        txtOperatoryEventEHRTime.Text = OperatoryEventRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtOperatoryEventPullTime.Text = OperatoryEventRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtOperatoryEventPushTime.Text = OperatoryEventRow[0]["SyncModule_Push"].ToString().Trim();
                        if (txtOperatoryEventEHRTime.Text == txtOperatoryEventPullTime.Text && txtOperatoryEventEHRTime.Text == txtOperatoryEventPushTime.Text)
                        {
                            chkOperatoryEvent.Checked = true;
                        }
                        else
                        {
                            chkOperatoryEvent.Checked = false;
                        }
                    }


                    //Provider
                    DataRow[] ProviderRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblProvider.Text.ToString().ToLower().Trim() + "'");
                    if (ProviderRow.Length > 0)
                    {
                        txtProviderEHRTime.Text = ProviderRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtProviderPullTime.Text = ProviderRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtProviderPushTime.Text = ProviderRow[0]["SyncModule_Push"].ToString().Trim();
                        if (txtProviderEHRTime.Text == txtProviderPullTime.Text && txtProviderEHRTime.Text == txtProviderPushTime.Text)
                        {
                            chkProvider.Checked = true;
                        }
                        else
                        {
                            chkProvider.Checked = false;
                        }
                    }

                    //Speciality
                    DataRow[] SpecialityRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblSpeciality.Text.ToString().ToLower().Trim() + "'");
                    if (SpecialityRow.Length > 0)
                    {
                        txtSpecialityEHRTime.Text = SpecialityRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtSpecialityPullTime.Text = SpecialityRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtSpecialityPushTime.Text = SpecialityRow[0]["SyncModule_Push"].ToString().Trim();
                        if (txtSpecialityEHRTime.Text == txtSpecialityPullTime.Text && txtSpecialityEHRTime.Text == txtSpecialityPushTime.Text)
                        {
                            chkSpeciality.Checked = true;
                        }
                        else
                        {
                            chkSpeciality.Checked = false;
                        }
                    }

                    //Operatory
                    DataRow[] OperatoryRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblOperatory.Text.ToString().ToLower().Trim() + "'");
                    if (OperatoryRow.Length > 0)
                    {
                        txtOperatoryEHRTime.Text = OperatoryRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtOperatoryPullTime.Text = OperatoryRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtOperatoryPushTime.Text = OperatoryRow[0]["SyncModule_Push"].ToString().Trim();
                        if (txtOperatoryEHRTime.Text == txtOperatoryPullTime.Text && txtOperatoryEHRTime.Text == txtOperatoryPushTime.Text)
                        {
                            chkOperatory.Checked = true;
                        }
                        else
                        {
                            chkOperatory.Checked = false;
                        }
                    }

                    //ApptType
                    DataRow[] ApptTypeRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblApptType.Text.ToString().ToLower().Trim() + "'");
                    if (ApptTypeRow.Length > 0)
                    {
                        txtApptTypeEHRTime.Text = ApptTypeRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtApptTypePullTime.Text = ApptTypeRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtApptTypePushTime.Text = ApptTypeRow[0]["SyncModule_Push"].ToString().Trim();
                        if (txtApptTypeEHRTime.Text == txtApptTypePullTime.Text && txtApptTypeEHRTime.Text == txtApptTypePushTime.Text)
                        {
                            chkApptType.Checked = true;
                        }
                        else
                        {
                            chkApptType.Checked = false;
                        }
                    }

                    //Patient
                    DataRow[] PatientRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblPatient.Text.ToString().ToLower().Trim() + "'");
                    if (PatientRow.Length > 0)
                    {
                        txtPatientEHRTime.Text = PatientRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtPatientPullTime.Text = PatientRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtPatientPushTime.Text = PatientRow[0]["SyncModule_Push"].ToString().Trim();
                        if (txtPatientEHRTime.Text == txtPatientPullTime.Text && txtPatientEHRTime.Text == txtPatientPushTime.Text)
                        {
                            chkPatient.Checked = true;
                        }
                        else
                        {
                            chkPatient.Checked = false;
                        }
                    }

                    //RecallType
                    DataRow[] RecallTypeRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblRecallType.Text.ToString().ToLower().Trim() + "'");
                    if (RecallTypeRow.Length > 0)
                    {
                        txtRecallTypeEHRTime.Text = RecallTypeRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtRecallTypePullTime.Text = RecallTypeRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtRecallTypePushTime.Text = RecallTypeRow[0]["SyncModule_Push"].ToString().Trim();
                        if (txtRecallTypeEHRTime.Text == txtRecallTypePullTime.Text && txtRecallTypeEHRTime.Text == txtRecallTypePushTime.Text)
                        {
                            chkRecallType.Checked = true;
                        }
                        else
                        {
                            chkRecallType.Checked = false;
                        }
                    }

                    //ApptStatus
                    DataRow[] ApptStatusRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblApptStatus.Text.ToString().ToLower().Trim() + "'");
                    if (ApptStatusRow.Length > 0)
                    {
                        txtApptStatusEHRTime.Text = ApptStatusRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtApptStatusPullTime.Text = ApptStatusRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtApptStatusPushTime.Text = ApptStatusRow[0]["SyncModule_Push"].ToString().Trim();
                        if (txtApptStatusEHRTime.Text == txtApptStatusPullTime.Text && txtApptStatusEHRTime.Text == txtApptStatusPushTime.Text)
                        {
                            chkApptStatus.Checked = true;
                        }
                        else
                        {
                            chkApptStatus.Checked = false;
                        }
                    }

                    //Holiday
                    DataRow[] HolidayRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblHoliday.Text.ToString().ToLower().Trim() + "'");
                    if (HolidayRow.Length > 0)
                    {
                        txtHolidayEHRTime.Text = HolidayRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtHolidayPullTime.Text = HolidayRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtHolidayPushTime.Text = HolidayRow[0]["SyncModule_Push"].ToString().Trim();
                        if (txtHolidayEHRTime.Text == txtHolidayPullTime.Text && txtHolidayEHRTime.Text == txtHolidayPushTime.Text)
                        {
                            chkHoliday.Checked = true;
                        }
                        else
                        {
                            chkHoliday.Checked = false;
                        }
                    }

                    //Patient Form
                    DataRow[] PatientFormRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblPatientForm.Text.ToString().ToLower().Trim() + "'");
                    if (PatientFormRow.Length > 0)
                    {
                        txtPatientFormEHRTime.Text = PatientFormRow[0]["SyncModule_EHR"].ToString().Trim();
                        txtPatientFormPullTime.Text = PatientFormRow[0]["SyncModule_Pull"].ToString().Trim();
                        txtPatientFormPushTime.Text = PatientFormRow[0]["SyncModule_Push"].ToString().Trim();
                        if (txtPatientFormEHRTime.Text == txtPatientFormPullTime.Text && txtPatientFormEHRTime.Text == txtPatientFormPushTime.Text)
                        {
                            chkPatientForm.Checked = true;
                        }
                        else
                        {
                            chkPatientForm.Checked = false;
                        }
                    }
                    

                    //Clear Local Records
                    DataRow[] ClearLocalRecordRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblClearLocalRecords.Text.ToString().ToLower().Trim() + "'");
                    if (ClearLocalRecordRow.Length > 0)
                    {
                        txtClearLocalRecords.Text = ClearLocalRecordRow[0]["SyncModule_EHR"].ToString().Trim();
                    }

                    //payment
                    DataRow[] PaymentRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = '" + lblpatientpaymentt.Text.ToString().ToLower().Trim() + "'");
                    if (PaymentRow.Length > 0)
                    {
                        txtpatientpayment.Text = PaymentRow[0]["SyncModule_EHR"].ToString().Trim();
                    }
                }
                var ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "PozativeAppStatus");
                if (ctl == null)
                {
                    Is_ServiceInstalled = false;
                    picAutoStartON_OFF.Image = Resources.OFF;
                    picAutoStartON_OFF.Tag = "OFF";
                }
                else
                {
                    Is_ServiceInstalled = true;
                    picAutoStartON_OFF.Image = Resources.ON;
                    picAutoStartON_OFF.Tag = "ON";
                }

                try
                {
                    string TaskName = "PozativeTask";
                    var ts = new TaskService();
                    var task = ts.RootFolder.Tasks.Where(a => a.Name.ToLower() == TaskName.ToLower()).FirstOrDefault();
                    if (task == null)
                    {
                        picCreateTaskScheduler.Image = Resources.OFF;
                        picCreateTaskScheduler.Tag = "OFF";
                    }
                    else
                    {
                        picCreateTaskScheduler.Image = Resources.ON;
                        picCreateTaskScheduler.Tag = "ON";
                    }
                }
                catch (Exception)
                {

                }


                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\UseSingleCPU");
                if (key != null && bool.Parse(key.GetValue("Value").ToString()))
                {
                    picUseSingleCPU.Image = Resources.ON;
                    picUseSingleCPU.Tag = "ON";
                }
                else
                {
                    picUseSingleCPU.Image = Resources.OFF;
                    picUseSingleCPU.Tag = "OFF";
                }


                if (Utility.DontAskPasswordOnSaveSetting == true)
                {
                    picDontAskPasswordOnSaveSetting.Image = Resources.ON;
                    picDontAskPasswordOnSaveSetting.Tag = "ON";
                }
                else
                {
                    picDontAskPasswordOnSaveSetting.Image = Resources.OFF;
                    picDontAskPasswordOnSaveSetting.Tag = "OFF";
                }

                if (Utility.NotAllowToChangeSystemDateFormat == true)
                {
                    picNotAllowToChangeSystemDateFormat.Image = Resources.ON;
                    picNotAllowToChangeSystemDateFormat.Tag = "ON";
                }
                else
                {
                    picNotAllowToChangeSystemDateFormat.Image = Resources.OFF;
                    picNotAllowToChangeSystemDateFormat.Tag = "OFF";
                }


                txtApptEHRTime.Focus();

                if (Utility.Application_ID == 4)
                {
                    //lblAppointment.Text = "SoftDent Sync Interval";
                    lblApptStatus.Visible = false;
                    txtApptStatusEHRTime.Visible = false;
                    txtApptStatusPullTime.Visible = false;
                    txtApptStatusPushTime.Visible = false;
                    chkApptStatus.Visible = false;

                    lblApptType.Visible = false;
                    txtApptTypeEHRTime.Visible = false;
                    txtApptTypePullTime.Visible = false;
                    txtApptTypePushTime.Visible = false;
                    chkApptType.Visible = false;

                    lblOperatory.Visible = false;
                    txtOperatoryEHRTime.Visible = false;
                    txtOperatoryPullTime.Visible = false;
                    txtOperatoryPushTime.Visible = false;
                    chkOperatory.Visible = false;

                    lblOperatoryEvent.Visible = false;
                    txtOperatoryEventEHRTime.Visible = false;
                    txtOperatoryEventPullTime.Visible = false;
                    txtOperatoryEventPushTime.Visible = false;
                    chkOperatoryEvent.Visible = false;

                    lblProvider.Visible = false;
                    txtProviderEHRTime.Visible = false;
                    txtProviderPullTime.Visible = false;
                    txtProviderPushTime.Visible = false;
                    chkProvider.Visible = false;

                    //lblPatient.Visible = false;
                    //txtPatientEHRTime.Visible = false;
                    //txtPatientPullTime.Visible = false;
                    //txtPatientPushTime.Visible = false;
                    //chkPatient.Visible = false;

                    lblRecallType.Visible = false;
                    txtRecallTypeEHRTime.Visible = false;
                    txtRecallTypePullTime.Visible = false;
                    txtRecallTypePushTime.Visible = false;
                    chkRecallType.Visible = false;

                    lblSpeciality.Visible = false;
                    txtSpecialityEHRTime.Visible = false;
                    txtSpecialityPullTime.Visible = false;
                    txtSpecialityPushTime.Visible = false;
                    chkSpeciality.Visible = false;

                    lblHoliday.Visible = false;
                    txtHolidayEHRTime.Visible = false;
                    txtHolidayPullTime.Visible = false;
                    txtHolidayPushTime.Visible = false;
                    chkHoliday.Visible = false;

                    lblPatientForm.Visible = false;
                    txtPatientFormEHRTime.Visible = false;
                    txtPatientFormPullTime.Visible = false;
                    txtPatientFormPushTime.Visible = false;
                    chkPatientForm.Visible = false;

                    lblClearLocalRecords.Visible=false;
                    txtClearLocalRecords.Visible = false;
                }
                if (Utility.IsApplicationIdleTimeSet)
                {
                    picAppIdleTime.Image = Resources.ON;
                    dtpStartTime.Value = Utility.AppIdleStartTime;

                    TimeSpan ts = Utility.AppIdleStopTime - Utility.AppIdleStartTime;
                    int EndMinutes = Convert.ToInt16(ts.TotalMinutes);
                    switch (EndMinutes)
                    {
                        case 15:
                            dtpEndTime.SelectedIndex = 0;
                            break;
                        case 30:
                            dtpEndTime.SelectedIndex = 1;
                            break;
                        case 60:
                            dtpEndTime.SelectedIndex = 2;
                            break;
                        case 120:
                            dtpEndTime.SelectedIndex = 3;
                            break;
                        case 180:
                            dtpEndTime.SelectedIndex = 4;
                            break;
                        case 5:
                            dtpEndTime.SelectedIndex = 5;
                            break;
                    }
                    picAppIdleTime.Tag = "ON";
                    dtpStartTime.Visible = true;
                    dtpEndTime.Visible = true;
                    lblStartTime.Visible = true;
                    lblEndTime.Visible = true;
                }
                else
                {
                    picAppIdleTime.Image = Resources.OFF;
                    picAppIdleTime.Tag = "OFF";
                    dtpStartTime.Visible = false;
                    dtpEndTime.Visible = false;
                    lblStartTime.Visible = false;
                    lblEndTime.Visible = false;
                }
                try
                {
                    if (Utility.Application_ID == 3)
                    {
                        string docpassword = Utility.GetAppSettingsString("docpassword", "docpassword", "");
                        Utility.DentrixDocPWD = (docpassword != null && docpassword != "") ? Utility.DecryptString(docpassword) : "";
                        if (Utility.DentrixDocPWD != null && Utility.DentrixDocPWD != "")
                        {
                            btn_DentrixPDFPhrase.Visible = false;
                        }
                        else
                        {
                            btn_DentrixPDFPhrase.Visible = true;
                        }
                    }
                    else
                    {
                        btn_DentrixPDFPhrase.Visible = false;
                    }
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit ModuleSyncConfig-Load", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Common Function

        private void SetModuleDefaultValue()
        {
            txtApptEHRTime.Text = "5";
            txtApptPullTime.Text = "5";
            txtApptPushTime.Text = "5";
            chkAppt.Checked = true;

            txtOperatoryEventEHRTime.Text = "30";
            txtOperatoryEventPullTime.Text = "30";
            txtOperatoryEventPushTime.Text = "30";
            chkOperatoryEvent.Checked = true;

            txtProviderEHRTime.Text = "50";
            txtProviderPullTime.Text = "50";
            txtProviderPushTime.Text = "50";
            chkProvider.Checked = true;

            txtSpecialityEHRTime.Text = "60";
            txtSpecialityPullTime.Text = "60";
            txtSpecialityPushTime.Text = "60";
            chkSpeciality.Checked = true;

            txtOperatoryEHRTime.Text = "70";
            txtOperatoryPullTime.Text = "70";
            txtOperatoryPushTime.Text = "70";
            chkOperatory.Checked = true;

            txtApptTypeEHRTime.Text = "180";
            txtApptTypePullTime.Text = "180";
            txtApptTypePushTime.Text = "180";
            chkApptType.Checked = true;

            txtPatientEHRTime.Text = "90";
            txtPatientPullTime.Text = "90";
            txtPatientPushTime.Text = "90";
            chkPatient.Checked = true;

            txtRecallTypeEHRTime.Text = "240";
            txtRecallTypePullTime.Text = "240";
            txtRecallTypePushTime.Text = "240";
            chkRecallType.Checked = true;

            txtApptStatusEHRTime.Text = "250";
            txtApptStatusPullTime.Text = "250";
            txtApptStatusPushTime.Text = "250";
            chkApptStatus.Checked = true;

            txtHolidayEHRTime.Text = "200";
            txtHolidayPullTime.Text = "200";
            txtHolidayPushTime.Text = "200";
            chkHoliday.Checked = true;

            txtPatientFormEHRTime.Text = "10";
            txtPatientFormPullTime.Text = "10";
            txtPatientFormPushTime.Text = "10";
            chkPatientForm.Checked = true;

            txtClearLocalRecords.Text = "10080";
            txtTimeslot.Text = "15";
            txtpatientpayment.Text = "5";
        }

        private bool CheckAllTextBoxValue()
        {
            bool isValidValue = false;

            foreach (var pb in tblFormControls.Controls.OfType<TextBox>())
            {
                TextBox textBox = (TextBox)pb;
                int n;
                bool isNumeric = int.TryParse(textBox.Text, out n);

                if (!isNumeric)
                {
                    ObjGoalBase.ErrorMsgBox("Invalid Input", "Empty value is not valid. please enter 1 - 1440 value ");
                    textBox.Text = string.Empty;
                    textBox.Focus();
                    isValidValue = false;
                    return isValidValue;
                }
                else
                {
                    if ((Convert.ToInt16(textBox.Text) == 0 || Convert.ToInt32(textBox.Text) > 45000 ) && textBox.Name==txtClearLocalRecords.Name)
                    {
                        ObjGoalBase.ErrorMsgBox("Invalid Input", textBox.Text + " is not valid. please enter 1 - 45000 value ");
                        textBox.Focus();
                        isValidValue = false;
                        return isValidValue;
                    }
                    else if ((Convert.ToInt16(textBox.Text) == 0 || Convert.ToInt16(textBox.Text) > 1440) && textBox.Name != txtClearLocalRecords.Name)
                    {
                        ObjGoalBase.ErrorMsgBox("Invalid Input", textBox.Text + " is not valid. please enter 1 - 1440 value ");
                        textBox.Focus();
                        isValidValue = false;
                        return isValidValue;
                    }
                }
            }
            //if (picAppIdleTime.Tag.ToString() == "ON")
            //{
            //    if (dtpEndTime.Value.Date > dtpStartTime.Value.Date)
            //    {
            //        ObjGoalBase.ErrorMsgBox("Invalid Idle Time","Set idle time within Same date.");
            //    }
            //}
            isValidValue = true;
            return isValidValue;
        }

        private DataTable CreateTempdtAditModuleSyncTime()
        {
            DataTable TempdtAditModuleSyncTime = new DataTable();
            TempdtAditModuleSyncTime.Clear();
            TempdtAditModuleSyncTime.Columns.Add("SyncModule_Name", typeof(string));
            TempdtAditModuleSyncTime.Columns.Add("SyncModule_EHR", typeof(string));
            TempdtAditModuleSyncTime.Columns.Add("SyncModule_Pull", typeof(string));
            TempdtAditModuleSyncTime.Columns.Add("SyncModule_Push", typeof(string));
            TempdtAditModuleSyncTime.Columns.Add("SyncDateTime", typeof(DateTime));
            TempdtAditModuleSyncTime.AcceptChanges();
            return TempdtAditModuleSyncTime;
        }

        private void CreateDatatableModuleSyncTime()
        {
            DataRow tmpModuleRow;

            // Appointment
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblAppointment.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtApptEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtApptPullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtApptPushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            // OperatoryEvent
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblOperatoryEvent.Tag.ToString().ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtOperatoryEventEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtOperatoryEventPullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtOperatoryEventPushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            // Provider
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblProvider.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtProviderEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtProviderPullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtProviderPushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            // Speciality
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblSpeciality.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtSpecialityEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtSpecialityPullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtSpecialityPushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            // Operatory
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblOperatory.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtOperatoryEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtOperatoryPullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtOperatoryPushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            // ApptType
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblApptType.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtApptTypeEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtApptTypePullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtApptTypePushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            // Patient
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblPatient.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtPatientEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtPatientPullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtPatientPushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            // RecallType
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblRecallType.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtRecallTypeEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtRecallTypePullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtRecallTypePushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            // ApptStatus
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblApptStatus.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtApptStatusEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtApptStatusPullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtApptStatusPushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            // Holiday
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblHoliday.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtHolidayEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtHolidayPullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtHolidayPushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            // Patient Form
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblPatientForm.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtPatientFormEHRTime.Text;
            tmpModuleRow["SyncModule_Pull"] = txtPatientFormPullTime.Text;
            tmpModuleRow["SyncModule_Push"] = txtPatientFormPushTime.Text;
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

            //Clear Local Records
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblClearLocalRecords.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtClearLocalRecords.Text.ToLower();
            tmpModuleRow["SyncModule_Pull"] = txtClearLocalRecords.Text.ToLower();
            tmpModuleRow["SyncModule_Push"] = txtClearLocalRecords.Text.ToLower();
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);

           //PatientPayment
            tmpModuleRow = TempdtAditModuleSyncTime.NewRow();
            tmpModuleRow["SyncModule_Name"] = lblpatientpaymentt.Text.ToLower();
            tmpModuleRow["SyncModule_EHR"] = txtpatientpayment.Text.ToLower();
            tmpModuleRow["SyncModule_Pull"] = txtpatientpayment.Text.ToLower();
            tmpModuleRow["SyncModule_Push"] = txtpatientpayment.Text.ToLower();
            TempdtAditModuleSyncTime.Rows.Add(tmpModuleRow);
        }


        #endregion

        #region Button Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                StatusAditSyncProcess = false;
                this.Close();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit ModuleSyncConfig-Close", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Utility.DontAskPasswordOnSaveSetting)
                {
                    frmPassword frmPass = new frmPassword();
                    frmPass.ShowDialog();
                    if (frmPass.Is_PasswordValid == false)
                    {
                        return;
                    }
                }
                Cursor.Current = Cursors.WaitCursor;
                bool isAllValid = CheckAllTextBoxValue();

                if (isAllValid)
                {
                    CreateDatatableModuleSyncTime();

                    bool ModuleSyncConfig = SystemBAL.Save_AditModuleSyncConfigTime(TempdtAditModuleSyncTime);

                    int EndMinutes = 0;
                    switch (dtpEndTime.SelectedIndex)
                    {
                        case 0:
                            EndMinutes = 15;
                            break;
                        case 1:
                            EndMinutes = 30;
                            break;
                        case 2:
                            EndMinutes = 60;
                            break;
                        case 3:
                            EndMinutes = 120;
                            break;
                        case 4:
                            EndMinutes = 180;
                            break;
                    }
                    #region TrackerScheduleInterval
                    if (Utility.Application_ID == 6)
                    {
                        RegistryKey key1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TrackerScheduleInterval");
                        if (key1 != null)
                        {
                            Utility.TrackerScheduleInterval = Int16.Parse(key1.GetValue("TrackerScheduleInterval").ToString());
                        }
                        else
                        {
                            RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TrackerScheduleInterval");
                            key1.SetValue("TrackerScheduleInterval", txtTimeslot.Text);

                        }
                    }
                    #endregion

                    bool ModuleSyncIdleConfig = SystemBAL.Save_AditModuleSyncIdleConfigTime(picAppIdleTime.Tag.ToString() == "ON" ? true : false, Convert.ToDateTime(dtpStartTime.Value), Convert.ToDateTime(dtpStartTime.Value.AddMinutes(EndMinutes)));
                    string FileName = "";

                    bool Is_ServiceOn = false;

                    if (picAutoStartON_OFF.Tag.ToString() == "ON")
                    {
                        Is_ServiceOn = true;
                        FileName = Application.StartupPath + "\\InstallWindowService.bat";
                    }
                    else
                    {

                        Is_ServiceOn = false;
                        FileName = Application.StartupPath + "\\UnInstallWindowService.bat";
                    }
                    if (Is_ServiceOn != Is_ServiceInstalled)
                    {
                        if (System.IO.File.Exists(FileName))
                        {
                            System.IO.File.Delete(FileName);
                        }
                        if (!File.Exists(FileName))
                        {
                            File.Create(FileName).Dispose();
                        }

                        using (StreamWriter SW = new StreamWriter(FileName))
                        {
                            if (picAutoStartON_OFF.Tag.ToString() == "ON")
                            {
                                SW.WriteLine(@"C:\windows\microsoft.net\framework\v4.0.30319\installutil.exe " + "\"" + Application.StartupPath + "\\PozativeAppStatus.exe\"");
                            }
                            else
                            {
                                SW.WriteLine(@"C:\windows\microsoft.net\framework\v4.0.30319\installutil.exe /u " + "\"" + Application.StartupPath + "\\PozativeAppStatus.exe\"");
                            }
                            SW.WriteLine(@"net start PozativeAppStatus");
                            SW.WriteLine(@"exit 0");
                            SW.Close();
                        }
                        ExecuteBatchFile(FileName);
                    }
                    try
                    {
                        CreateTaskScheduler();
                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.SuccessMsgBox("Adit Module Sync Configuration", "Task scheduler not created because of some error : " + ex1.Message);
                    }
                    if (ModuleSyncConfig)
                    {
                        StatusAditSyncProcess = true;
                        ObjGoalBase.SuccessMsgBox("Adit Module Sync Configuration", "Adit All Module Syncing process time is configured successfully.");                        
                        this.DialogResult = DialogResult.OK;
                        try
                        {
                            Utility.UpdateBackupDBFromLocalDB();
                        }
                        catch
                        { }                        
                        frmPozative.SynchDataLiveDB_Push_PozativeConfiguraion();
                        this.Close();
                    }


                    if (picUseSingleCPU.Tag.ToString() == "ON")
                    {
                        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\UseSingleCPU");
                        key.SetValue("Value", true);
                    }
                    else
                    {
                        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\UseSingleCPU");
                        key.SetValue("Value", false);
                    }

                    bool ConfigSettingSaved = SystemBAL.UpdateConfigSettingsInstallApplicationDetail(picDontAskPasswordOnSaveSetting.Tag.ToString() == "ON" ? true : false, picNotAllowToChangeSystemDateFormat.Tag.ToString() == "ON" ? true : false);                    

                    //if (Utility.DentrixDocPWD == null || Utility.DentrixDocPWD == "")
                    //{
                    //    ObjGoalBase.ComfirmMsgBox("Adit Module Sync Configuration", "Dentrix PDF attachment phrase not set properly do you want to enter again ?");
                    //    if (ObjGoalBase.StatusIsComfirm)
                    //    {
                    //        try
                    //        {
                    //            //Utility.SetRegistryObject(Registry.LocalMachine, "SyncCallCount", Utility.EncryptString("3"), null, RegistryValueKind.String);
                    //            GoalBase.GetConnectionStringforDoc(true);
                    //        }
                    //        catch (Exception ex1)
                    //        {
                    //            ObjGoalBase.WriteToSyncLogFile("Error while Find Doc pwd for pdf Attachment " + ex1.Message.ToString());
                    //        }
                    //    }

                    //}
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit ModuleSyncConfig-Save", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void CreateTaskScheduler()
        {
            try
            {
                string TaskName = "PozativeTask";
                var ts = new TaskService();
                var task = ts.RootFolder.Tasks.Where(a => a.Name.ToLower() == TaskName.ToLower()).FirstOrDefault();
                if (task == null)
                {
                    if (picCreateTaskScheduler.Tag.ToString() == "ON")
                    {
                        TaskDefinition td = ts.NewTask();
                        td.RegistrationInfo.Description = "Start Adit Application Daily";
                        td.Settings.Compatibility = TaskCompatibility.V2;
                        td.Triggers.AddNew(TaskTriggerType.Daily);
                        td.Actions.Add(new ExecAction(Application.StartupPath.ToString() + "\\Pozative.exe", "", null));
                        td.Settings.StopIfGoingOnBatteries = false;
                        td.Settings.DisallowStartIfOnBatteries = false;
                        td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
                        td.Principal.RunLevel = TaskRunLevel.Highest;                       
                        ts.RootFolder.RegisterTaskDefinition(@"" + TaskName.ToString() + "", td, TaskCreation.CreateOrUpdate, "NT AUTHORITY\\System", null, TaskLogonType.ServiceAccount, null);                       
                    }
                }
                else
                {
                    //if (picCreateTaskScheduler.Tag.ToString() == "ON")
                    //{
                    //    ts.RootFolder.DeleteTask(TaskName);
                    //    TaskDefinition td = ts.NewTask();
                    //    td.RegistrationInfo.Description = "Start Adit Application Daily";
                    //    td.Settings.Compatibility = TaskCompatibility.V2;
                    //    td.Triggers.AddNew(TaskTriggerType.Daily);
                    //    td.Actions.Add(new ExecAction(Application.StartupPath.ToString() + "\\Pozative.exe", "", null));
                    //    td.Settings.StopIfGoingOnBatteries = false;
                    //    td.Settings.DisallowStartIfOnBatteries = false;
                    //    td.Settings.MultipleInstances = TaskInstancesPolicy.StopExisting;
                    //    td.Principal.RunLevel = TaskRunLevel.Highest;
                    //    ts.RootFolder.RegisterTaskDefinition(@"" + TaskName.ToString() + "", td, TaskCreation.CreateOrUpdate, "NT AUTHORITY\\System", null, TaskLogonType.ServiceAccount, null);

                    //}
                    //else 
                    if (picCreateTaskScheduler.Tag.ToString() == "OFF")
                    {
                        ts.RootFolder.DeleteTask(TaskName);
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Error while create task scheduler " + ex.Message.ToString());
            }
        }

        void ExecuteBatchFile(string FileName)
        {
            var psi = new ProcessStartInfo();
            psi.CreateNoWindow = true; //This hides the dos-style black window that the command prompt usually shows
            psi.FileName = FileName;
            psi.Verb = "runas"; //This is what actually runs the command as administrator
            try
            {
                var process = new Process();
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo = psi;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception)
            {
                //If you are here the user clicked decline to grant admin privileges (or he's not administrator)
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                StatusAditSyncProcess = false;
                this.Close();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit ModuleSyncConfig-Cancel", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                SetModuleDefaultValue();
                picAutoStartON_OFF.Image = Resources.ON;
                picAutoStartON_OFF.Tag = "ON";
                picCreateTaskScheduler.Image = Resources.ON;
                picCreateTaskScheduler.Tag = "ON";

                picAppIdleTime.Image = Resources.OFF;
                picAppIdleTime.Tag = "OFF";
                picDontAskPasswordOnSaveSetting.Image = Resources.OFF;
                picDontAskPasswordOnSaveSetting.Tag = "OFF";
                picNotAllowToChangeSystemDateFormat.Image = Resources.OFF;
                picNotAllowToChangeSystemDateFormat.Tag = "OFF";
                dtpStartTime.Visible = false;
                dtpEndTime.Visible = false;
                lblStartTime.Visible = false;
                lblEndTime.Visible = false;
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit ModuleSyncConfig-Restore", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void picAutoStartON_OFF_Click(object sender, EventArgs e)
        {
            try
            {
                if (picAutoStartON_OFF.Tag.ToString() == "OFF")
                {
                    picAutoStartON_OFF.Image = Resources.ON;
                    picAutoStartON_OFF.Tag = "ON";
                    //MessageBox.Show("Install Succefully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                else
                {
                    picAutoStartON_OFF.Image = Resources.OFF;
                    picAutoStartON_OFF.Tag = "OFF";
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit Sync", ex.Message);
            }
        }


        private void picCreateTaskScheduler_Click(object sender, EventArgs e)
        {
            try
            {
                if (picCreateTaskScheduler.Tag.ToString() == "OFF")
                {
                    picCreateTaskScheduler.Image = Resources.ON;
                    picCreateTaskScheduler.Tag = "ON";
                }
                else
                {
                    picCreateTaskScheduler.Image = Resources.OFF;
                    picCreateTaskScheduler.Tag = "OFF";
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("TaskScheduler On_Off", ex.Message);
            }
        }

        private void picAppIdleTime_Click(object sender, EventArgs e)
        {
            if (picAppIdleTime.Tag.ToString() == "OFF")
            {
                picAppIdleTime.Image = Resources.ON;
                picAppIdleTime.Tag = "ON";
                dtpStartTime.Visible = true;
                dtpEndTime.Visible = true;
                lblStartTime.Visible = true;
                lblEndTime.Visible = true;
                dtpStartTime.Value = DateTime.Now;

                try
                {
                    dtpEndTime.SelectedIndex = 1;
                }
                catch (Exception)
                {
                    // ObjGoalBase.ErrorMsgBox("AppIdleTime On_Off", ex.Message);
                    dtpEndTime.SelectedIndex = 1;
                }
            }
            else
            {
                picAppIdleTime.Image = Resources.OFF;
                picAppIdleTime.Tag = "OFF";
                dtpStartTime.Visible = false;
                dtpEndTime.Visible = false;
                lblStartTime.Visible = false;
                lblEndTime.Visible = false;
            }
        }


        #endregion

        #region Checkbox Event

        private void chkAppt_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkAppt.Checked)
                {
                    txtApptPullTime.Enabled = false;
                    txtApptPushTime.Enabled = false;

                    txtApptPullTime.Text = txtApptEHRTime.Text;
                    txtApptPushTime.Text = txtApptEHRTime.Text;
                }
                else
                {
                    txtApptPullTime.Enabled = true;
                    txtApptPushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkOperatoryEvent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkOperatoryEvent.Checked)
                {
                    txtOperatoryEventPullTime.Enabled = false;
                    txtOperatoryEventPushTime.Enabled = false;

                    txtOperatoryEventPullTime.Text = txtOperatoryEventEHRTime.Text;
                    txtOperatoryEventPushTime.Text = txtOperatoryEventEHRTime.Text;
                }
                else
                {
                    txtOperatoryEventPullTime.Enabled = true;
                    txtOperatoryEventPushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkProvider_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkProvider.Checked)
                {
                    txtProviderPullTime.Enabled = false;
                    txtProviderPushTime.Enabled = false;

                    txtProviderPullTime.Text = txtProviderEHRTime.Text;
                    txtProviderPushTime.Text = txtProviderEHRTime.Text;
                }
                else
                {
                    txtProviderPullTime.Enabled = true;
                    txtProviderPushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkSpeciality_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkSpeciality.Checked)
                {
                    txtSpecialityPullTime.Enabled = false;
                    txtSpecialityPushTime.Enabled = false;

                    txtSpecialityPullTime.Text = txtSpecialityEHRTime.Text;
                    txtSpecialityPushTime.Text = txtSpecialityEHRTime.Text;
                }
                else
                {
                    txtSpecialityPullTime.Enabled = true;
                    txtSpecialityPushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkOperatory_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkOperatory.Checked)
                {
                    txtOperatoryPullTime.Enabled = false;
                    txtOperatoryPushTime.Enabled = false;

                    txtOperatoryPullTime.Text = txtOperatoryEHRTime.Text;
                    txtOperatoryPushTime.Text = txtOperatoryEHRTime.Text;
                }
                else
                {
                    txtOperatoryPullTime.Enabled = true;
                    txtOperatoryPushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkApptType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkApptType.Checked)
                {
                    txtApptTypePullTime.Enabled = false;
                    txtApptTypePushTime.Enabled = false;

                    txtApptTypePullTime.Text = txtApptTypeEHRTime.Text;
                    txtApptTypePushTime.Text = txtApptTypeEHRTime.Text;
                }
                else
                {
                    txtApptTypePullTime.Enabled = true;
                    txtApptTypePushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkPatient_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkPatient.Checked)
                {
                    txtPatientPullTime.Enabled = false;
                    txtPatientPushTime.Enabled = false;

                    txtPatientPullTime.Text = txtPatientEHRTime.Text;
                    txtPatientPushTime.Text = txtPatientEHRTime.Text;
                }
                else
                {
                    txtPatientPullTime.Enabled = true;
                    txtPatientPushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkRecallType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkRecallType.Checked)
                {
                    txtRecallTypePullTime.Enabled = false;
                    txtRecallTypePushTime.Enabled = false;

                    txtRecallTypePullTime.Text = txtRecallTypeEHRTime.Text;
                    txtRecallTypePushTime.Text = txtRecallTypeEHRTime.Text;
                }
                else
                {
                    txtRecallTypePullTime.Enabled = true;
                    txtRecallTypePushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkApptStatus_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkApptStatus.Checked)
                {
                    txtApptStatusPullTime.Enabled = false;
                    txtApptStatusPushTime.Enabled = false;

                    txtApptStatusPullTime.Text = txtApptStatusEHRTime.Text;
                    txtApptStatusPushTime.Text = txtApptStatusEHRTime.Text;
                }
                else
                {
                    txtApptStatusPullTime.Enabled = true;
                    txtApptStatusPushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkHoliday_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkHoliday.Checked)
                {
                    txtHolidayPullTime.Enabled = false;
                    txtHolidayPushTime.Enabled = false;

                    txtHolidayPullTime.Text = txtHolidayEHRTime.Text;
                    txtHolidayPushTime.Text = txtHolidayEHRTime.Text;
                }
                else
                {
                    txtHolidayPullTime.Enabled = true;
                    txtHolidayPushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkPatientForm_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (chkPatientForm.Checked)
                {
                    txtPatientFormPullTime.Enabled = false;
                    txtPatientFormPushTime.Enabled = false;

                    txtPatientFormPullTime.Text = txtPatientFormEHRTime.Text;
                    txtPatientFormPushTime.Text = txtPatientFormEHRTime.Text;
                }
                else
                {
                    txtPatientFormPullTime.Enabled = true;
                    txtPatientFormPushTime.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        #endregion

        #region Common Event

        private void tblformHead_MouseDown(object sender, MouseEventArgs e)
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

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void textBoxNum_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                int n;
                bool isNumeric = int.TryParse(textBox.Text, out n);

                if (!isNumeric)
                {
                    textBox.Text = string.Empty;
                    //textBox.Focus();
                }
                else
                {
                    if ((Convert.ToInt16(textBox.Text) == 0 || Convert.ToInt32(textBox.Text) > 45000) && textBox.Name == txtClearLocalRecords.Name)
                    {
                        ObjGoalBase.ErrorMsgBox("Invalid Input", textBox.Text + " is not valid. please enter 1 - 45000 value ");
                    }
                    else if ((Convert.ToInt32(textBox.Text) < 1 || Convert.ToInt32(textBox.Text) > 1440)  && textBox.Name != txtClearLocalRecords.Name)
                    {
                        ObjGoalBase.ErrorMsgBox("Invalid Input", textBox.Text + " is not valid. please enter 1 - 1440 value ");
                        // textBox.Focus();
                    }
                    else
                    {

                        if (textBox.Name.ToLower() == "txtApptEHRTime".ToLower() && chkAppt.Checked)
                        {
                            txtApptPullTime.Text = textBox.Text.ToString();
                            txtApptPushTime.Text = textBox.Text.ToString();
                        }
                        else if (textBox.Name.ToLower() == "txtOperatoryEventEHRTime".ToLower() && chkOperatoryEvent.Checked)
                        {
                            txtOperatoryEventPullTime.Text = textBox.Text.ToString();
                            txtOperatoryEventPushTime.Text = textBox.Text.ToString();
                        }
                        else if (textBox.Name.ToLower() == "txtProviderEHRTime".ToLower() && chkProvider.Checked)
                        {
                            txtProviderPullTime.Text = textBox.Text.ToString();
                            txtProviderPushTime.Text = textBox.Text.ToString();
                        }
                        else if (textBox.Name.ToLower() == "txtSpecialityEHRTime".ToLower() && chkSpeciality.Checked)
                        {
                            txtSpecialityPullTime.Text = textBox.Text.ToString();
                            txtSpecialityPushTime.Text = textBox.Text.ToString();
                        }
                        else if (textBox.Name.ToLower() == "txtOperatoryEHRTime".ToLower() && chkOperatory.Checked)
                        {
                            txtOperatoryPullTime.Text = textBox.Text.ToString();
                            txtOperatoryPushTime.Text = textBox.Text.ToString();
                        }
                        else if (textBox.Name.ToLower() == "txtApptTypeEHRTime".ToLower() && chkApptType.Checked)
                        {
                            txtApptTypePullTime.Text = textBox.Text.ToString();
                            txtApptTypePushTime.Text = textBox.Text.ToString();
                        }
                        else if (textBox.Name.ToLower() == "txtPatientEHRTime".ToLower() && chkPatient.Checked)
                        {
                            txtPatientPullTime.Text = textBox.Text.ToString();
                            txtPatientPushTime.Text = textBox.Text.ToString();
                        }
                        else if (textBox.Name.ToLower() == "txtRecallTypeEHRTime".ToLower() && chkRecallType.Checked)
                        {
                            txtRecallTypePullTime.Text = textBox.Text.ToString();
                            txtRecallTypePushTime.Text = textBox.Text.ToString();
                        }
                        else if (textBox.Name.ToLower() == "txtApptStatusEHRTime".ToLower() && chkApptStatus.Checked)
                        {
                            txtApptStatusPullTime.Text = textBox.Text.ToString();
                            txtApptStatusPushTime.Text = textBox.Text.ToString();
                        }

                        else if (textBox.Name.ToLower() == "txtHolidayEHRTime".ToLower() && chkHoliday.Checked)
                        {
                            txtHolidayPullTime.Text = textBox.Text.ToString();
                            txtHolidayPushTime.Text = textBox.Text.ToString();
                        }
                        else if (textBox.Name.ToLower() == "txtPatientFormEHRTime".ToLower() && chkPatientForm.Checked)
                        {
                            txtPatientFormPullTime.Text = textBox.Text.ToString();
                            txtPatientFormPushTime.Text = textBox.Text.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        private void dtpStartTime_ValueChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Config Clinic and Database

        private void BtnAddClinic_Click(object sender, EventArgs e)
        {
            try
            {
                frmClinicConfiguration Obj_ClinicConfiguration = new frmClinicConfiguration();
                Obj_ClinicConfiguration.ShowForm();
            }
            catch (Exception Ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Adit Sync" + Ex.Message);
            }
        }
        private void BtnAddDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                frmDatabaseConfiguration Obj_DatabaseConfiguration = new frmDatabaseConfiguration();
                Obj_DatabaseConfiguration.ShowForm();

                if (Utility.DtInstallServiceList.Rows.Count > 0)
                    BtnAddClinic.Visible = true;
            }
            catch (Exception Ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Adit Sync" + Ex.Message);
            }
        }
        #endregion Config Clinic and Database

        private void picUseSingleCPU_Click(object sender, EventArgs e)
        {
            try
            {
                if (picUseSingleCPU.Tag.ToString() == "OFF")
                {
                    picUseSingleCPU.Image = Resources.ON;
                    picUseSingleCPU.Tag = "ON";
                }

                else
                {
                    picUseSingleCPU.Image = Resources.OFF;
                    picUseSingleCPU.Tag = "OFF";
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit Sync", ex.Message);
            }
        }

        private void picDontAskPasswordOnSaveSetting_Click(object sender, EventArgs e)
        {
            try
            {
                if (picDontAskPasswordOnSaveSetting.Tag.ToString() == "OFF")
                {
                    picDontAskPasswordOnSaveSetting.Image = Resources.ON;
                    picDontAskPasswordOnSaveSetting.Tag = "ON";
                }

                else
                {
                    picDontAskPasswordOnSaveSetting.Image = Resources.OFF;
                    picDontAskPasswordOnSaveSetting.Tag = "OFF";
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit Sync", ex.Message);
            }
        }

        private void picNotAllowToChangeSystemDateFormat_Click(object sender, EventArgs e)
        {
            try
            {
                if (picNotAllowToChangeSystemDateFormat.Tag.ToString() == "OFF")
                {
                    picNotAllowToChangeSystemDateFormat.Image = Resources.ON;
                    picNotAllowToChangeSystemDateFormat.Tag = "ON";
                }

                else
                {
                    picNotAllowToChangeSystemDateFormat.Image = Resources.OFF;
                    picNotAllowToChangeSystemDateFormat.Tag = "OFF";
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit Sync", ex.Message);
            }
        }

        private void btn_DentrixPDFPhrase_Click(object sender, EventArgs e)
        {
            try
            {
                //Utility.SetRegistryObject(Registry.LocalMachine, "SyncCallCount", Utility.EncryptString("3"), null, RegistryValueKind.String);
                GoalBase.GetConnectionStringforDoc(true);
            }
            catch (Exception ex1)
            {
                ObjGoalBase.WriteToSyncLogFile("Error while Find Doc pwd for pdf Attachment " + ex1.Message.ToString());
            }
        }
    }
}
