namespace Pozative
{
    partial class frmPozative
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPozative));
            this.NotifyIconPozative = new System.Windows.Forms.NotifyIcon(this.components);
            this.tblHome = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.grpPozative = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPozativeAppt = new System.Windows.Forms.Button();
            this.btnPozativeSyncAppt = new System.Windows.Forms.Button();
            this.txtConnectionLog = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGridView = new System.Windows.Forms.Panel();
            this.grdAppointment = new System.Windows.Forms.DataGridView();
            this.Appt_DateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Patient_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mobile_Contact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Provider_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Operatory_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApptType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Appointment_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ViewHere = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Appt_EHR_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Last_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.First_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Home_Contact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remind_DateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Entry_DateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Appt_LocalDB_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdPozativeAppointment = new System.Windows.Forms.DataGridView();
            this.Poz_Appt_DateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Poz_Patient_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Poz_Mobile_Contact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Poz_Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Poz_Operatory_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Poz_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Poz_Appt_EHR_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Poz_Entry_DateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Poz_Appt_LocalDB_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSyncLog = new System.Windows.Forms.Label();
            this.tmrConnectionLog = new System.Windows.Forms.Timer(this.components);
            this.tmrConsoleRun = new System.Windows.Forms.Timer(this.components);
            this.TTPView = new System.Windows.Forms.ToolTip(this.components);
            this.tmrCheckApplicationUpdate = new System.Windows.Forms.Timer(this.components);
            this.tmrAditLocationSyncEnable = new System.Windows.Forms.Timer(this.components);
            this.startGetPatientRecord = new System.Windows.Forms.Timer(this.components);
            this.tmrApplicationIdleTime = new System.Windows.Forms.Timer(this.components);
            this.tmrPaymentSMSCallLog = new System.Windows.Forms.Timer(this.components);
            this.tmrSMSLog = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pnlHome = new System.Windows.Forms.Panel();
            this.grpAdit = new System.Windows.Forms.GroupBox();
            this.btnSyncPatient = new Pozative.cButton();
            this.btnAppt = new Pozative.cButton();
            this.btnPatient = new Pozative.cButton();
            this.btnSyncHoliday = new Pozative.cButton();
            this.lblPatient = new System.Windows.Forms.Label();
            this.btnHoliday = new Pozative.cButton();
            this.lblApptType = new System.Windows.Forms.Label();
            this.lblHoliday = new System.Windows.Forms.Label();
            this.btnApptType = new Pozative.cButton();
            this.btnSyncOperatoryEvent = new Pozative.cButton();
            this.btnSyncApptType = new Pozative.cButton();
            this.btnOperatoryEvent = new Pozative.cButton();
            this.lblOperatories = new System.Windows.Forms.Label();
            this.lblOperatoryEvent = new System.Windows.Forms.Label();
            this.lblProviders = new System.Windows.Forms.Label();
            this.btnSyncApptStatus = new Pozative.cButton();
            this.btnOperatories = new Pozative.cButton();
            this.btnSyncRecallType = new Pozative.cButton();
            this.btnSyncAppt = new Pozative.cButton();
            this.btnApptStatus = new Pozative.cButton();
            this.btnSyncOperatories = new Pozative.cButton();
            this.btnRecallType = new Pozative.cButton();
            this.lblRecallType = new System.Windows.Forms.Label();
            this.btnProviders = new Pozative.cButton();
            this.lblApptStatus = new System.Windows.Forms.Label();
            this.lblAppointment = new System.Windows.Forms.Label();
            this.btnSyncSpeciality = new Pozative.cButton();
            this.btnSyncProviders = new Pozative.cButton();
            this.btnSpeciality = new Pozative.cButton();
            this.lblSpeciality = new System.Windows.Forms.Label();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.lblConnectionLog = new System.Windows.Forms.Label();
            this.lblEHR = new System.Windows.Forms.Label();
            this.btnEHR = new Pozative.cButton();
            this.pnlHead = new Pozative.CPanel();
            this.tblHead = new System.Windows.Forms.TableLayoutPanel();
            this.picHead = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.lblHead = new System.Windows.Forms.Label();
            this.picAppUpdate = new System.Windows.Forms.PictureBox();
            this.btnAppRestart = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tblHome.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.grpPozative.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAppointment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPozativeAppointment)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlHome.SuspendLayout();
            this.grpAdit.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.pnlHead.SuspendLayout();
            this.tblHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAppUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // NotifyIconPozative
            // 
            this.NotifyIconPozative.Text = "Pozative";
            this.NotifyIconPozative.Visible = true;
            this.NotifyIconPozative.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Pozative_MouseDoubleClick);
            // 
            // tblHome
            // 
            this.tblHome.BackColor = System.Drawing.Color.White;
            this.tblHome.ColumnCount = 1;
            this.tblHome.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHome.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tblHome.Location = new System.Drawing.Point(3, 82);
            this.tblHome.Name = "tblHome";
            this.tblHome.RowCount = 3;
            this.tblHome.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblHome.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 440F));
            this.tblHome.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHome.Size = new System.Drawing.Size(158, 85);
            this.tblHome.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtConnectionLog, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel9, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 40);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(158, 440);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.grpPozative, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 43);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(470, 394);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // grpPozative
            // 
            this.grpPozative.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPozative.Controls.Add(this.tableLayoutPanel7);
            this.grpPozative.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.grpPozative.Location = new System.Drawing.Point(3, 3);
            this.grpPozative.Name = "grpPozative";
            this.grpPozative.Size = new System.Drawing.Size(464, 1);
            this.grpPozative.TabIndex = 1;
            this.grpPozative.TabStop = false;
            this.grpPozative.Text = "Pozative";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 3;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnPozativeAppt, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnPozativeSyncAppt, 2, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(458, 0);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Appointment";
            // 
            // btnPozativeAppt
            // 
            this.btnPozativeAppt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPozativeAppt.FlatAppearance.BorderSize = 0;
            this.btnPozativeAppt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPozativeAppt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnPozativeAppt.Location = new System.Drawing.Point(153, 3);
            this.btnPozativeAppt.Name = "btnPozativeAppt";
            this.btnPozativeAppt.Size = new System.Drawing.Size(148, 29);
            this.btnPozativeAppt.TabIndex = 5;
            this.btnPozativeAppt.Text = "-";
            this.btnPozativeAppt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPozativeAppt.UseVisualStyleBackColor = true;
            // 
            // btnPozativeSyncAppt
            // 
            this.btnPozativeSyncAppt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPozativeSyncAppt.FlatAppearance.BorderSize = 0;
            this.btnPozativeSyncAppt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPozativeSyncAppt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnPozativeSyncAppt.Location = new System.Drawing.Point(307, 3);
            this.btnPozativeSyncAppt.Name = "btnPozativeSyncAppt";
            this.btnPozativeSyncAppt.Size = new System.Drawing.Size(148, 29);
            this.btnPozativeSyncAppt.TabIndex = 5;
            this.btnPozativeSyncAppt.Text = "-";
            this.btnPozativeSyncAppt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPozativeSyncAppt.UseVisualStyleBackColor = true;
            // 
            // txtConnectionLog
            // 
            this.txtConnectionLog.BackColor = System.Drawing.Color.White;
            this.txtConnectionLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConnectionLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConnectionLog.Location = new System.Drawing.Point(479, 43);
            this.txtConnectionLog.Name = "txtConnectionLog";
            this.txtConnectionLog.ReadOnly = true;
            this.txtConnectionLog.Size = new System.Drawing.Size(40, 54);
            this.txtConnectionLog.TabIndex = 6;
            this.txtConnectionLog.Text = "";
            this.txtConnectionLog.Visible = false;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 6;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(476, 0);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(764, 40);
            this.tableLayoutPanel9.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Location = new System.Drawing.Point(3, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(158, 65);
            this.panel2.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.pnlGridView, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblSyncLog, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(478, 255);
            this.tableLayoutPanel2.TabIndex = 0;
            this.tableLayoutPanel2.Visible = false;
            // 
            // pnlGridView
            // 
            this.pnlGridView.Controls.Add(this.grdAppointment);
            this.pnlGridView.Controls.Add(this.grdPozativeAppointment);
            this.pnlGridView.Location = new System.Drawing.Point(0, 35);
            this.pnlGridView.Margin = new System.Windows.Forms.Padding(0);
            this.pnlGridView.Name = "pnlGridView";
            this.pnlGridView.Size = new System.Drawing.Size(478, 220);
            this.pnlGridView.TabIndex = 4;
            // 
            // grdAppointment
            // 
            this.grdAppointment.AllowUserToAddRows = false;
            this.grdAppointment.AllowUserToDeleteRows = false;
            this.grdAppointment.AllowUserToOrderColumns = true;
            this.grdAppointment.AllowUserToResizeColumns = false;
            this.grdAppointment.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.grdAppointment.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdAppointment.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdAppointment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdAppointment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAppointment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Appt_DateTime,
            this.Patient_Name,
            this.Mobile_Contact,
            this.Provider_Name,
            this.Operatory_Name,
            this.ApptType,
            this.Appointment_Status,
            this.Status,
            this.ViewHere,
            this.Appt_EHR_ID,
            this.Last_Name,
            this.First_Name,
            this.MI,
            this.Home_Contact,
            this.Email,
            this.Address,
            this.City,
            this.ST,
            this.Zip,
            this.Remind_DateTime,
            this.Entry_DateTime,
            this.Appt_LocalDB_ID});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdAppointment.DefaultCellStyle = dataGridViewCellStyle4;
            this.grdAppointment.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdAppointment.Location = new System.Drawing.Point(0, 0);
            this.grdAppointment.Margin = new System.Windows.Forms.Padding(0);
            this.grdAppointment.Name = "grdAppointment";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdAppointment.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.grdAppointment.RowHeadersVisible = false;
            this.grdAppointment.RowHeadersWidth = 51;
            this.grdAppointment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdAppointment.Size = new System.Drawing.Size(470, 220);
            this.grdAppointment.TabIndex = 3;
            this.grdAppointment.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdAppointment_CellClick);
            this.grdAppointment.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdAppointment_CellFormatting);
            // 
            // Appt_DateTime
            // 
            this.Appt_DateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Appt_DateTime.DataPropertyName = "Appt_DateTime";
            dataGridViewCellStyle3.Format = "MM/dd/yyyy hh:mm tt";
            this.Appt_DateTime.DefaultCellStyle = dataGridViewCellStyle3;
            this.Appt_DateTime.HeaderText = "DateTime";
            this.Appt_DateTime.MinimumWidth = 6;
            this.Appt_DateTime.Name = "Appt_DateTime";
            // 
            // Patient_Name
            // 
            this.Patient_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Patient_Name.DataPropertyName = "Patient_Name";
            this.Patient_Name.HeaderText = "Patient Name";
            this.Patient_Name.MinimumWidth = 6;
            this.Patient_Name.Name = "Patient_Name";
            // 
            // Mobile_Contact
            // 
            this.Mobile_Contact.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Mobile_Contact.DataPropertyName = "Mobile_Contact";
            this.Mobile_Contact.HeaderText = "Mobile Contact";
            this.Mobile_Contact.MinimumWidth = 6;
            this.Mobile_Contact.Name = "Mobile_Contact";
            // 
            // Provider_Name
            // 
            this.Provider_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Provider_Name.DataPropertyName = "Provider_Name";
            this.Provider_Name.HeaderText = "Provider Name";
            this.Provider_Name.MinimumWidth = 6;
            this.Provider_Name.Name = "Provider_Name";
            // 
            // Operatory_Name
            // 
            this.Operatory_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Operatory_Name.DataPropertyName = "Operatory_Name";
            this.Operatory_Name.HeaderText = "Operatory";
            this.Operatory_Name.MinimumWidth = 6;
            this.Operatory_Name.Name = "Operatory_Name";
            // 
            // ApptType
            // 
            this.ApptType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ApptType.DataPropertyName = "ApptType";
            this.ApptType.HeaderText = "Appt Type";
            this.ApptType.MinimumWidth = 6;
            this.ApptType.Name = "ApptType";
            // 
            // Appointment_Status
            // 
            this.Appointment_Status.DataPropertyName = "Appointment_Status";
            this.Appointment_Status.HeaderText = "Status";
            this.Appointment_Status.MinimumWidth = 6;
            this.Appointment_Status.Name = "Appointment_Status";
            this.Appointment_Status.ReadOnly = true;
            this.Appointment_Status.Width = 125;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.Visible = false;
            // 
            // ViewHere
            // 
            this.ViewHere.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ViewHere.HeaderText = "View Here";
            this.ViewHere.MinimumWidth = 6;
            this.ViewHere.Name = "ViewHere";
            this.ViewHere.Text = "View Here";
            this.ViewHere.UseColumnTextForLinkValue = true;
            // 
            // Appt_EHR_ID
            // 
            this.Appt_EHR_ID.DataPropertyName = "Appt_EHR_ID";
            this.Appt_EHR_ID.HeaderText = "Appt_EHR_ID";
            this.Appt_EHR_ID.MinimumWidth = 6;
            this.Appt_EHR_ID.Name = "Appt_EHR_ID";
            this.Appt_EHR_ID.Visible = false;
            this.Appt_EHR_ID.Width = 125;
            // 
            // Last_Name
            // 
            this.Last_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Last_Name.DataPropertyName = "Last_Name";
            this.Last_Name.HeaderText = "Last Name";
            this.Last_Name.MinimumWidth = 6;
            this.Last_Name.Name = "Last_Name";
            this.Last_Name.Visible = false;
            // 
            // First_Name
            // 
            this.First_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.First_Name.DataPropertyName = "First_Name";
            this.First_Name.HeaderText = "First Name";
            this.First_Name.MinimumWidth = 6;
            this.First_Name.Name = "First_Name";
            this.First_Name.Visible = false;
            // 
            // MI
            // 
            this.MI.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.MI.DataPropertyName = "MI";
            this.MI.HeaderText = "MI";
            this.MI.MinimumWidth = 6;
            this.MI.Name = "MI";
            this.MI.Visible = false;
            // 
            // Home_Contact
            // 
            this.Home_Contact.DataPropertyName = "Home_Contact";
            this.Home_Contact.HeaderText = "Home_Contact";
            this.Home_Contact.MinimumWidth = 6;
            this.Home_Contact.Name = "Home_Contact";
            this.Home_Contact.Visible = false;
            this.Home_Contact.Width = 125;
            // 
            // Email
            // 
            this.Email.DataPropertyName = "Email";
            this.Email.HeaderText = "Email";
            this.Email.MinimumWidth = 6;
            this.Email.Name = "Email";
            this.Email.Visible = false;
            this.Email.Width = 125;
            // 
            // Address
            // 
            this.Address.DataPropertyName = "Address";
            this.Address.HeaderText = "Address";
            this.Address.MinimumWidth = 6;
            this.Address.Name = "Address";
            this.Address.Visible = false;
            this.Address.Width = 125;
            // 
            // City
            // 
            this.City.DataPropertyName = "City";
            this.City.HeaderText = "City";
            this.City.MinimumWidth = 6;
            this.City.Name = "City";
            this.City.Visible = false;
            this.City.Width = 125;
            // 
            // ST
            // 
            this.ST.DataPropertyName = "ST";
            this.ST.HeaderText = "ST";
            this.ST.MinimumWidth = 6;
            this.ST.Name = "ST";
            this.ST.Visible = false;
            this.ST.Width = 125;
            // 
            // Zip
            // 
            this.Zip.DataPropertyName = "Zip";
            this.Zip.HeaderText = "Zip";
            this.Zip.MinimumWidth = 6;
            this.Zip.Name = "Zip";
            this.Zip.Visible = false;
            this.Zip.Width = 125;
            // 
            // Remind_DateTime
            // 
            this.Remind_DateTime.DataPropertyName = "Remind_DateTime";
            this.Remind_DateTime.HeaderText = "Remind_DateTime";
            this.Remind_DateTime.MinimumWidth = 6;
            this.Remind_DateTime.Name = "Remind_DateTime";
            this.Remind_DateTime.Visible = false;
            this.Remind_DateTime.Width = 125;
            // 
            // Entry_DateTime
            // 
            this.Entry_DateTime.DataPropertyName = "Entry_DateTime";
            this.Entry_DateTime.HeaderText = "Entry_DateTime";
            this.Entry_DateTime.MinimumWidth = 6;
            this.Entry_DateTime.Name = "Entry_DateTime";
            this.Entry_DateTime.Visible = false;
            this.Entry_DateTime.Width = 125;
            // 
            // Appt_LocalDB_ID
            // 
            this.Appt_LocalDB_ID.DataPropertyName = "Appt_LocalDB_ID";
            this.Appt_LocalDB_ID.HeaderText = "Appt_LocalDB_ID";
            this.Appt_LocalDB_ID.MinimumWidth = 6;
            this.Appt_LocalDB_ID.Name = "Appt_LocalDB_ID";
            this.Appt_LocalDB_ID.Visible = false;
            this.Appt_LocalDB_ID.Width = 125;
            // 
            // grdPozativeAppointment
            // 
            this.grdPozativeAppointment.AllowUserToAddRows = false;
            this.grdPozativeAppointment.AllowUserToDeleteRows = false;
            this.grdPozativeAppointment.AllowUserToOrderColumns = true;
            this.grdPozativeAppointment.AllowUserToResizeColumns = false;
            this.grdPozativeAppointment.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            this.grdPozativeAppointment.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.grdPozativeAppointment.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdPozativeAppointment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.grdPozativeAppointment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPozativeAppointment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Poz_Appt_DateTime,
            this.Poz_Patient_Name,
            this.Poz_Mobile_Contact,
            this.Poz_Email,
            this.Poz_Operatory_Name,
            this.Poz_Status,
            this.Poz_Appt_EHR_ID,
            this.Poz_Entry_DateTime,
            this.Poz_Appt_LocalDB_ID});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdPozativeAppointment.DefaultCellStyle = dataGridViewCellStyle9;
            this.grdPozativeAppointment.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdPozativeAppointment.Location = new System.Drawing.Point(0, 0);
            this.grdPozativeAppointment.Margin = new System.Windows.Forms.Padding(0);
            this.grdPozativeAppointment.Name = "grdPozativeAppointment";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdPozativeAppointment.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.grdPozativeAppointment.RowHeadersVisible = false;
            this.grdPozativeAppointment.RowHeadersWidth = 51;
            this.grdPozativeAppointment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdPozativeAppointment.Size = new System.Drawing.Size(461, 220);
            this.grdPozativeAppointment.TabIndex = 4;
            // 
            // Poz_Appt_DateTime
            // 
            this.Poz_Appt_DateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Poz_Appt_DateTime.DataPropertyName = "Appt_DateTime";
            dataGridViewCellStyle8.Format = "MM/dd/yyyy hh:mm tt";
            this.Poz_Appt_DateTime.DefaultCellStyle = dataGridViewCellStyle8;
            this.Poz_Appt_DateTime.HeaderText = "DateTime";
            this.Poz_Appt_DateTime.MinimumWidth = 6;
            this.Poz_Appt_DateTime.Name = "Poz_Appt_DateTime";
            // 
            // Poz_Patient_Name
            // 
            this.Poz_Patient_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Poz_Patient_Name.DataPropertyName = "Patient_Name";
            this.Poz_Patient_Name.HeaderText = "Patient Name";
            this.Poz_Patient_Name.MinimumWidth = 6;
            this.Poz_Patient_Name.Name = "Poz_Patient_Name";
            // 
            // Poz_Mobile_Contact
            // 
            this.Poz_Mobile_Contact.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Poz_Mobile_Contact.DataPropertyName = "Mobile_Contact";
            this.Poz_Mobile_Contact.HeaderText = "Mobile Contact";
            this.Poz_Mobile_Contact.MinimumWidth = 6;
            this.Poz_Mobile_Contact.Name = "Poz_Mobile_Contact";
            // 
            // Poz_Email
            // 
            this.Poz_Email.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Poz_Email.DataPropertyName = "Email";
            this.Poz_Email.HeaderText = "Email";
            this.Poz_Email.MinimumWidth = 6;
            this.Poz_Email.Name = "Poz_Email";
            // 
            // Poz_Operatory_Name
            // 
            this.Poz_Operatory_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Poz_Operatory_Name.DataPropertyName = "Operatory_Name";
            this.Poz_Operatory_Name.HeaderText = "Operatory";
            this.Poz_Operatory_Name.MinimumWidth = 6;
            this.Poz_Operatory_Name.Name = "Poz_Operatory_Name";
            // 
            // Poz_Status
            // 
            this.Poz_Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Poz_Status.DataPropertyName = "Status";
            this.Poz_Status.HeaderText = "Status";
            this.Poz_Status.MinimumWidth = 6;
            this.Poz_Status.Name = "Poz_Status";
            this.Poz_Status.Visible = false;
            this.Poz_Status.Width = 125;
            // 
            // Poz_Appt_EHR_ID
            // 
            this.Poz_Appt_EHR_ID.DataPropertyName = "Appt_EHR_ID";
            this.Poz_Appt_EHR_ID.HeaderText = "Appt_EHR_ID";
            this.Poz_Appt_EHR_ID.MinimumWidth = 6;
            this.Poz_Appt_EHR_ID.Name = "Poz_Appt_EHR_ID";
            this.Poz_Appt_EHR_ID.Visible = false;
            this.Poz_Appt_EHR_ID.Width = 125;
            // 
            // Poz_Entry_DateTime
            // 
            this.Poz_Entry_DateTime.DataPropertyName = "Entry_DateTime";
            this.Poz_Entry_DateTime.HeaderText = "Entry_DateTime";
            this.Poz_Entry_DateTime.MinimumWidth = 6;
            this.Poz_Entry_DateTime.Name = "Poz_Entry_DateTime";
            this.Poz_Entry_DateTime.Visible = false;
            this.Poz_Entry_DateTime.Width = 125;
            // 
            // Poz_Appt_LocalDB_ID
            // 
            this.Poz_Appt_LocalDB_ID.DataPropertyName = "Appt_LocalDB_ID";
            this.Poz_Appt_LocalDB_ID.HeaderText = "Appt_LocalDB_ID";
            this.Poz_Appt_LocalDB_ID.MinimumWidth = 6;
            this.Poz_Appt_LocalDB_ID.Name = "Poz_Appt_LocalDB_ID";
            this.Poz_Appt_LocalDB_ID.Visible = false;
            this.Poz_Appt_LocalDB_ID.Width = 125;
            // 
            // lblSyncLog
            // 
            this.lblSyncLog.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSyncLog.AutoSize = true;
            this.lblSyncLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblSyncLog.Location = new System.Drawing.Point(3, 7);
            this.lblSyncLog.Name = "lblSyncLog";
            this.lblSyncLog.Size = new System.Drawing.Size(146, 20);
            this.lblSyncLog.TabIndex = 3;
            this.lblSyncLog.Text = "Appointment Log";
            // 
            // tmrConnectionLog
            // 
            this.tmrConnectionLog.Interval = 1000;
            this.tmrConnectionLog.Tick += new System.EventHandler(this.tmrConnectionLog_Tick);
            // 
            // tmrConsoleRun
            // 
            this.tmrConsoleRun.Interval = 1000;
            this.tmrConsoleRun.Tick += new System.EventHandler(this.tmrConsoleRun_Tick);
            // 
            // tmrCheckApplicationUpdate
            // 
            this.tmrCheckApplicationUpdate.Enabled = true;
            this.tmrCheckApplicationUpdate.Interval = 300000;
            this.tmrCheckApplicationUpdate.Tick += new System.EventHandler(this.tmrCheckApplicationUpdate_Tick);
            // 
            // tmrAditLocationSyncEnable
            // 
            this.tmrAditLocationSyncEnable.Interval = 60000;
            this.tmrAditLocationSyncEnable.Tick += new System.EventHandler(this.tmrAditLocationSyncEnable_Tick);
            // 
            // startGetPatientRecord
            // 
            this.startGetPatientRecord.Interval = 60000;
            this.startGetPatientRecord.Tick += new System.EventHandler(this.startGetPatientRecord_Tick);
            // 
            // tmrApplicationIdleTime
            // 
            this.tmrApplicationIdleTime.Interval = 1000;
            // 
            // tmrPaymentSMSCallLog
            // 
            this.tmrPaymentSMSCallLog.Interval = 1000;
            // 
            // tmrSMSLog
            // 
            this.tmrSMSLog.Interval = 1000;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.tblHome);
            this.panel1.Controls.Add(this.lblVersion);
            this.panel1.Location = new System.Drawing.Point(283, 259);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(173, 170);
            this.panel1.TabIndex = 3;
            this.panel1.Visible = false;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(43)))), ((int)(((byte)(64)))));
            this.lblVersion.Location = new System.Drawing.Point(137, -5);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(3);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(66, 25);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Version";
            this.lblVersion.UseCompatibleTextRendering = true;
            // 
            // pnlHome
            // 
            this.pnlHome.BackColor = System.Drawing.Color.White;
            this.pnlHome.Controls.Add(this.grpAdit);
            this.pnlHome.Controls.Add(this.tableLayoutPanel8);
            this.pnlHome.Controls.Add(this.pnlHead);
            this.pnlHome.Location = new System.Drawing.Point(226, 114);
            this.pnlHome.Name = "pnlHome";
            this.pnlHome.Size = new System.Drawing.Size(449, 422);
            this.pnlHome.TabIndex = 0;
            // 
            // grpAdit
            // 
            this.grpAdit.Controls.Add(this.btnSyncPatient);
            this.grpAdit.Controls.Add(this.btnAppt);
            this.grpAdit.Controls.Add(this.btnPatient);
            this.grpAdit.Controls.Add(this.btnSyncHoliday);
            this.grpAdit.Controls.Add(this.lblPatient);
            this.grpAdit.Controls.Add(this.btnHoliday);
            this.grpAdit.Controls.Add(this.lblApptType);
            this.grpAdit.Controls.Add(this.lblHoliday);
            this.grpAdit.Controls.Add(this.btnApptType);
            this.grpAdit.Controls.Add(this.btnSyncOperatoryEvent);
            this.grpAdit.Controls.Add(this.btnSyncApptType);
            this.grpAdit.Controls.Add(this.btnOperatoryEvent);
            this.grpAdit.Controls.Add(this.lblOperatories);
            this.grpAdit.Controls.Add(this.lblOperatoryEvent);
            this.grpAdit.Controls.Add(this.lblProviders);
            this.grpAdit.Controls.Add(this.btnSyncApptStatus);
            this.grpAdit.Controls.Add(this.btnOperatories);
            this.grpAdit.Controls.Add(this.btnSyncRecallType);
            this.grpAdit.Controls.Add(this.btnSyncAppt);
            this.grpAdit.Controls.Add(this.btnApptStatus);
            this.grpAdit.Controls.Add(this.btnSyncOperatories);
            this.grpAdit.Controls.Add(this.btnRecallType);
            this.grpAdit.Controls.Add(this.lblRecallType);
            this.grpAdit.Controls.Add(this.btnProviders);
            this.grpAdit.Controls.Add(this.lblApptStatus);
            this.grpAdit.Controls.Add(this.lblAppointment);
            this.grpAdit.Controls.Add(this.btnSyncSpeciality);
            this.grpAdit.Controls.Add(this.btnSyncProviders);
            this.grpAdit.Controls.Add(this.btnSpeciality);
            this.grpAdit.Controls.Add(this.lblSpeciality);
            this.grpAdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpAdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.grpAdit.Location = new System.Drawing.Point(10, 79);
            this.grpAdit.Name = "grpAdit";
            this.grpAdit.Size = new System.Drawing.Size(429, 333);
            this.grpAdit.TabIndex = 0;
            this.grpAdit.TabStop = false;
            this.grpAdit.Text = "Adit";
            this.grpAdit.UseCompatibleTextRendering = true;
            // 
            // btnSyncPatient
            // 
            this.btnSyncPatient.BackColor = System.Drawing.Color.White;
            this.btnSyncPatient.BackgroundColor = System.Drawing.Color.White;
            this.btnSyncPatient.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSyncPatient.BorderRadius = 5;
            this.btnSyncPatient.BorderSize = 1;
            this.btnSyncPatient.FlatAppearance.BorderSize = 0;
            this.btnSyncPatient.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSyncPatient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncPatient.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSyncPatient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncPatient.IsCommandButton = false;
            this.btnSyncPatient.Location = new System.Drawing.Point(284, 201);
            this.btnSyncPatient.Name = "btnSyncPatient";
            this.btnSyncPatient.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSyncPatient.Size = new System.Drawing.Size(122, 25);
            this.btnSyncPatient.TabIndex = 5;
            this.btnSyncPatient.Text = "-";
            this.btnSyncPatient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncPatient.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncPatient.UseCompatibleTextRendering = true;
            this.btnSyncPatient.UseVisualStyleBackColor = false;
            this.btnSyncPatient.MouseEnter += new System.EventHandler(this.btnSyncPatient_MouseEnter);
            // 
            // btnAppt
            // 
            this.btnAppt.BackColor = System.Drawing.Color.White;
            this.btnAppt.BackgroundColor = System.Drawing.Color.White;
            this.btnAppt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnAppt.BorderRadius = 5;
            this.btnAppt.BorderSize = 1;
            this.btnAppt.FlatAppearance.BorderSize = 0;
            this.btnAppt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnAppt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAppt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnAppt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnAppt.IsCommandButton = false;
            this.btnAppt.Location = new System.Drawing.Point(156, 21);
            this.btnAppt.Name = "btnAppt";
            this.btnAppt.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnAppt.Size = new System.Drawing.Size(122, 25);
            this.btnAppt.TabIndex = 5;
            this.btnAppt.Text = "Connecting";
            this.btnAppt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppt.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnAppt.UseCompatibleTextRendering = true;
            this.btnAppt.UseVisualStyleBackColor = false;
            // 
            // btnPatient
            // 
            this.btnPatient.BackColor = System.Drawing.Color.White;
            this.btnPatient.BackgroundColor = System.Drawing.Color.White;
            this.btnPatient.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnPatient.BorderRadius = 5;
            this.btnPatient.BorderSize = 1;
            this.btnPatient.FlatAppearance.BorderSize = 0;
            this.btnPatient.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnPatient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPatient.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnPatient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnPatient.IsCommandButton = false;
            this.btnPatient.Location = new System.Drawing.Point(156, 201);
            this.btnPatient.Name = "btnPatient";
            this.btnPatient.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnPatient.Size = new System.Drawing.Size(122, 25);
            this.btnPatient.TabIndex = 5;
            this.btnPatient.Text = "-";
            this.btnPatient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPatient.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnPatient.UseCompatibleTextRendering = true;
            this.btnPatient.UseVisualStyleBackColor = false;
            this.btnPatient.MouseEnter += new System.EventHandler(this.btnPatient_MouseEnter);
            // 
            // btnSyncHoliday
            // 
            this.btnSyncHoliday.BackColor = System.Drawing.Color.White;
            this.btnSyncHoliday.BackgroundColor = System.Drawing.Color.White;
            this.btnSyncHoliday.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSyncHoliday.BorderRadius = 5;
            this.btnSyncHoliday.BorderSize = 1;
            this.btnSyncHoliday.FlatAppearance.BorderSize = 0;
            this.btnSyncHoliday.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSyncHoliday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncHoliday.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSyncHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncHoliday.IsCommandButton = false;
            this.btnSyncHoliday.Location = new System.Drawing.Point(284, 292);
            this.btnSyncHoliday.Name = "btnSyncHoliday";
            this.btnSyncHoliday.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSyncHoliday.Size = new System.Drawing.Size(122, 25);
            this.btnSyncHoliday.TabIndex = 5;
            this.btnSyncHoliday.Text = "-";
            this.btnSyncHoliday.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncHoliday.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncHoliday.UseCompatibleTextRendering = true;
            this.btnSyncHoliday.UseVisualStyleBackColor = false;
            // 
            // lblPatient
            // 
            this.lblPatient.AutoSize = true;
            this.lblPatient.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblPatient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblPatient.Location = new System.Drawing.Point(20, 199);
            this.lblPatient.Name = "lblPatient";
            this.lblPatient.Size = new System.Drawing.Size(47, 20);
            this.lblPatient.TabIndex = 4;
            this.lblPatient.Text = "Patient";
            this.lblPatient.UseCompatibleTextRendering = true;
            // 
            // btnHoliday
            // 
            this.btnHoliday.BackColor = System.Drawing.Color.White;
            this.btnHoliday.BackgroundColor = System.Drawing.Color.White;
            this.btnHoliday.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnHoliday.BorderRadius = 5;
            this.btnHoliday.BorderSize = 1;
            this.btnHoliday.FlatAppearance.BorderSize = 0;
            this.btnHoliday.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnHoliday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHoliday.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnHoliday.IsCommandButton = false;
            this.btnHoliday.Location = new System.Drawing.Point(156, 292);
            this.btnHoliday.Name = "btnHoliday";
            this.btnHoliday.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnHoliday.Size = new System.Drawing.Size(122, 25);
            this.btnHoliday.TabIndex = 5;
            this.btnHoliday.Text = "-";
            this.btnHoliday.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHoliday.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnHoliday.UseCompatibleTextRendering = true;
            this.btnHoliday.UseVisualStyleBackColor = false;
            // 
            // lblApptType
            // 
            this.lblApptType.AutoSize = true;
            this.lblApptType.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblApptType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblApptType.Location = new System.Drawing.Point(20, 175);
            this.lblApptType.Name = "lblApptType";
            this.lblApptType.Size = new System.Drawing.Size(113, 20);
            this.lblApptType.TabIndex = 7;
            this.lblApptType.Text = "Appointment Type";
            this.lblApptType.UseCompatibleTextRendering = true;
            // 
            // lblHoliday
            // 
            this.lblHoliday.AutoSize = true;
            this.lblHoliday.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblHoliday.Location = new System.Drawing.Point(20, 296);
            this.lblHoliday.Name = "lblHoliday";
            this.lblHoliday.Size = new System.Drawing.Size(50, 20);
            this.lblHoliday.TabIndex = 4;
            this.lblHoliday.Text = "Holiday";
            this.lblHoliday.UseCompatibleTextRendering = true;
            // 
            // btnApptType
            // 
            this.btnApptType.BackColor = System.Drawing.Color.White;
            this.btnApptType.BackgroundColor = System.Drawing.Color.White;
            this.btnApptType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnApptType.BorderRadius = 5;
            this.btnApptType.BorderSize = 1;
            this.btnApptType.FlatAppearance.BorderSize = 0;
            this.btnApptType.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnApptType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApptType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnApptType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnApptType.IsCommandButton = false;
            this.btnApptType.Location = new System.Drawing.Point(156, 171);
            this.btnApptType.Name = "btnApptType";
            this.btnApptType.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnApptType.Size = new System.Drawing.Size(122, 25);
            this.btnApptType.TabIndex = 5;
            this.btnApptType.Text = "-";
            this.btnApptType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApptType.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnApptType.UseCompatibleTextRendering = true;
            this.btnApptType.UseVisualStyleBackColor = false;
            // 
            // btnSyncOperatoryEvent
            // 
            this.btnSyncOperatoryEvent.BackColor = System.Drawing.Color.White;
            this.btnSyncOperatoryEvent.BackgroundColor = System.Drawing.Color.White;
            this.btnSyncOperatoryEvent.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSyncOperatoryEvent.BorderRadius = 5;
            this.btnSyncOperatoryEvent.BorderSize = 1;
            this.btnSyncOperatoryEvent.FlatAppearance.BorderSize = 0;
            this.btnSyncOperatoryEvent.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSyncOperatoryEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncOperatoryEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSyncOperatoryEvent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncOperatoryEvent.IsCommandButton = false;
            this.btnSyncOperatoryEvent.Location = new System.Drawing.Point(284, 51);
            this.btnSyncOperatoryEvent.Name = "btnSyncOperatoryEvent";
            this.btnSyncOperatoryEvent.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSyncOperatoryEvent.Size = new System.Drawing.Size(122, 25);
            this.btnSyncOperatoryEvent.TabIndex = 5;
            this.btnSyncOperatoryEvent.Text = "Synced";
            this.btnSyncOperatoryEvent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncOperatoryEvent.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncOperatoryEvent.UseCompatibleTextRendering = true;
            this.btnSyncOperatoryEvent.UseVisualStyleBackColor = false;
            this.btnSyncOperatoryEvent.MouseEnter += new System.EventHandler(this.btnSyncOperatoryEvent_MouseEnter);
            // 
            // btnSyncApptType
            // 
            this.btnSyncApptType.BackColor = System.Drawing.Color.White;
            this.btnSyncApptType.BackgroundColor = System.Drawing.Color.White;
            this.btnSyncApptType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSyncApptType.BorderRadius = 5;
            this.btnSyncApptType.BorderSize = 1;
            this.btnSyncApptType.FlatAppearance.BorderSize = 0;
            this.btnSyncApptType.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSyncApptType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncApptType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSyncApptType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncApptType.IsCommandButton = false;
            this.btnSyncApptType.Location = new System.Drawing.Point(284, 171);
            this.btnSyncApptType.Name = "btnSyncApptType";
            this.btnSyncApptType.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSyncApptType.Size = new System.Drawing.Size(122, 25);
            this.btnSyncApptType.TabIndex = 5;
            this.btnSyncApptType.Text = "-";
            this.btnSyncApptType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncApptType.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncApptType.UseCompatibleTextRendering = true;
            this.btnSyncApptType.UseVisualStyleBackColor = false;
            // 
            // btnOperatoryEvent
            // 
            this.btnOperatoryEvent.BackColor = System.Drawing.Color.White;
            this.btnOperatoryEvent.BackgroundColor = System.Drawing.Color.White;
            this.btnOperatoryEvent.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnOperatoryEvent.BorderRadius = 5;
            this.btnOperatoryEvent.BorderSize = 1;
            this.btnOperatoryEvent.FlatAppearance.BorderSize = 0;
            this.btnOperatoryEvent.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnOperatoryEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOperatoryEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnOperatoryEvent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnOperatoryEvent.IsCommandButton = false;
            this.btnOperatoryEvent.Location = new System.Drawing.Point(156, 51);
            this.btnOperatoryEvent.Name = "btnOperatoryEvent";
            this.btnOperatoryEvent.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnOperatoryEvent.Size = new System.Drawing.Size(122, 25);
            this.btnOperatoryEvent.TabIndex = 5;
            this.btnOperatoryEvent.Text = "Connected";
            this.btnOperatoryEvent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOperatoryEvent.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnOperatoryEvent.UseCompatibleTextRendering = true;
            this.btnOperatoryEvent.UseVisualStyleBackColor = false;
            // 
            // lblOperatories
            // 
            this.lblOperatories.AutoSize = true;
            this.lblOperatories.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblOperatories.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblOperatories.Location = new System.Drawing.Point(20, 145);
            this.lblOperatories.Name = "lblOperatories";
            this.lblOperatories.Size = new System.Drawing.Size(64, 20);
            this.lblOperatories.TabIndex = 4;
            this.lblOperatories.Text = "Operatory";
            this.lblOperatories.UseCompatibleTextRendering = true;
            // 
            // lblOperatoryEvent
            // 
            this.lblOperatoryEvent.AutoSize = true;
            this.lblOperatoryEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblOperatoryEvent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblOperatoryEvent.Location = new System.Drawing.Point(20, 56);
            this.lblOperatoryEvent.Name = "lblOperatoryEvent";
            this.lblOperatoryEvent.Size = new System.Drawing.Size(102, 20);
            this.lblOperatoryEvent.TabIndex = 4;
            this.lblOperatoryEvent.Text = "Operatory Event";
            this.lblOperatoryEvent.UseCompatibleTextRendering = true;
            // 
            // lblProviders
            // 
            this.lblProviders.AutoSize = true;
            this.lblProviders.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblProviders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblProviders.Location = new System.Drawing.Point(20, 86);
            this.lblProviders.Name = "lblProviders";
            this.lblProviders.Size = new System.Drawing.Size(55, 20);
            this.lblProviders.TabIndex = 4;
            this.lblProviders.Text = "Provider";
            this.lblProviders.UseCompatibleTextRendering = true;
            // 
            // btnSyncApptStatus
            // 
            this.btnSyncApptStatus.BackColor = System.Drawing.Color.White;
            this.btnSyncApptStatus.BackgroundColor = System.Drawing.Color.White;
            this.btnSyncApptStatus.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSyncApptStatus.BorderRadius = 5;
            this.btnSyncApptStatus.BorderSize = 1;
            this.btnSyncApptStatus.FlatAppearance.BorderSize = 0;
            this.btnSyncApptStatus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSyncApptStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncApptStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSyncApptStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncApptStatus.IsCommandButton = false;
            this.btnSyncApptStatus.Location = new System.Drawing.Point(284, 262);
            this.btnSyncApptStatus.Name = "btnSyncApptStatus";
            this.btnSyncApptStatus.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSyncApptStatus.Size = new System.Drawing.Size(122, 25);
            this.btnSyncApptStatus.TabIndex = 5;
            this.btnSyncApptStatus.Text = "-";
            this.btnSyncApptStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncApptStatus.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncApptStatus.UseCompatibleTextRendering = true;
            this.btnSyncApptStatus.UseVisualStyleBackColor = false;
            // 
            // btnOperatories
            // 
            this.btnOperatories.BackColor = System.Drawing.Color.White;
            this.btnOperatories.BackgroundColor = System.Drawing.Color.White;
            this.btnOperatories.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnOperatories.BorderRadius = 5;
            this.btnOperatories.BorderSize = 1;
            this.btnOperatories.FlatAppearance.BorderSize = 0;
            this.btnOperatories.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnOperatories.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOperatories.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnOperatories.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnOperatories.IsCommandButton = false;
            this.btnOperatories.Location = new System.Drawing.Point(156, 141);
            this.btnOperatories.Name = "btnOperatories";
            this.btnOperatories.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnOperatories.Size = new System.Drawing.Size(122, 25);
            this.btnOperatories.TabIndex = 5;
            this.btnOperatories.Text = "-";
            this.btnOperatories.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOperatories.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnOperatories.UseCompatibleTextRendering = true;
            this.btnOperatories.UseVisualStyleBackColor = false;
            // 
            // btnSyncRecallType
            // 
            this.btnSyncRecallType.BackColor = System.Drawing.Color.White;
            this.btnSyncRecallType.BackgroundColor = System.Drawing.Color.White;
            this.btnSyncRecallType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSyncRecallType.BorderRadius = 5;
            this.btnSyncRecallType.BorderSize = 1;
            this.btnSyncRecallType.FlatAppearance.BorderSize = 0;
            this.btnSyncRecallType.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSyncRecallType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncRecallType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSyncRecallType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncRecallType.IsCommandButton = false;
            this.btnSyncRecallType.Location = new System.Drawing.Point(284, 231);
            this.btnSyncRecallType.Name = "btnSyncRecallType";
            this.btnSyncRecallType.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSyncRecallType.Size = new System.Drawing.Size(122, 25);
            this.btnSyncRecallType.TabIndex = 5;
            this.btnSyncRecallType.Text = "-";
            this.btnSyncRecallType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncRecallType.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncRecallType.UseCompatibleTextRendering = true;
            this.btnSyncRecallType.UseVisualStyleBackColor = false;
            // 
            // btnSyncAppt
            // 
            this.btnSyncAppt.BackColor = System.Drawing.Color.White;
            this.btnSyncAppt.BackgroundColor = System.Drawing.Color.White;
            this.btnSyncAppt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSyncAppt.BorderRadius = 5;
            this.btnSyncAppt.BorderSize = 1;
            this.btnSyncAppt.FlatAppearance.BorderSize = 0;
            this.btnSyncAppt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSyncAppt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncAppt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSyncAppt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncAppt.IsCommandButton = false;
            this.btnSyncAppt.Location = new System.Drawing.Point(284, 21);
            this.btnSyncAppt.Name = "btnSyncAppt";
            this.btnSyncAppt.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSyncAppt.Size = new System.Drawing.Size(122, 25);
            this.btnSyncAppt.TabIndex = 5;
            this.btnSyncAppt.Text = "Syncing";
            this.btnSyncAppt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncAppt.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncAppt.UseCompatibleTextRendering = true;
            this.btnSyncAppt.UseVisualStyleBackColor = false;
            // 
            // btnApptStatus
            // 
            this.btnApptStatus.BackColor = System.Drawing.Color.White;
            this.btnApptStatus.BackgroundColor = System.Drawing.Color.White;
            this.btnApptStatus.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnApptStatus.BorderRadius = 5;
            this.btnApptStatus.BorderSize = 1;
            this.btnApptStatus.FlatAppearance.BorderSize = 0;
            this.btnApptStatus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnApptStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApptStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnApptStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnApptStatus.IsCommandButton = false;
            this.btnApptStatus.Location = new System.Drawing.Point(156, 262);
            this.btnApptStatus.Name = "btnApptStatus";
            this.btnApptStatus.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnApptStatus.Size = new System.Drawing.Size(122, 25);
            this.btnApptStatus.TabIndex = 5;
            this.btnApptStatus.Text = "-";
            this.btnApptStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApptStatus.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnApptStatus.UseCompatibleTextRendering = true;
            this.btnApptStatus.UseVisualStyleBackColor = false;
            // 
            // btnSyncOperatories
            // 
            this.btnSyncOperatories.BackColor = System.Drawing.Color.White;
            this.btnSyncOperatories.BackgroundColor = System.Drawing.Color.White;
            this.btnSyncOperatories.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSyncOperatories.BorderRadius = 5;
            this.btnSyncOperatories.BorderSize = 1;
            this.btnSyncOperatories.FlatAppearance.BorderSize = 0;
            this.btnSyncOperatories.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSyncOperatories.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncOperatories.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSyncOperatories.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncOperatories.IsCommandButton = false;
            this.btnSyncOperatories.Location = new System.Drawing.Point(284, 141);
            this.btnSyncOperatories.Name = "btnSyncOperatories";
            this.btnSyncOperatories.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSyncOperatories.Size = new System.Drawing.Size(122, 25);
            this.btnSyncOperatories.TabIndex = 5;
            this.btnSyncOperatories.Text = "-";
            this.btnSyncOperatories.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncOperatories.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncOperatories.UseCompatibleTextRendering = true;
            this.btnSyncOperatories.UseVisualStyleBackColor = false;
            // 
            // btnRecallType
            // 
            this.btnRecallType.BackColor = System.Drawing.Color.White;
            this.btnRecallType.BackgroundColor = System.Drawing.Color.White;
            this.btnRecallType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnRecallType.BorderRadius = 5;
            this.btnRecallType.BorderSize = 1;
            this.btnRecallType.FlatAppearance.BorderSize = 0;
            this.btnRecallType.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRecallType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecallType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnRecallType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnRecallType.IsCommandButton = false;
            this.btnRecallType.Location = new System.Drawing.Point(156, 231);
            this.btnRecallType.Name = "btnRecallType";
            this.btnRecallType.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnRecallType.Size = new System.Drawing.Size(122, 25);
            this.btnRecallType.TabIndex = 5;
            this.btnRecallType.Text = "-";
            this.btnRecallType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecallType.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnRecallType.UseCompatibleTextRendering = true;
            this.btnRecallType.UseVisualStyleBackColor = false;
            // 
            // lblRecallType
            // 
            this.lblRecallType.AutoSize = true;
            this.lblRecallType.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblRecallType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblRecallType.Location = new System.Drawing.Point(20, 235);
            this.lblRecallType.Name = "lblRecallType";
            this.lblRecallType.Size = new System.Drawing.Size(76, 20);
            this.lblRecallType.TabIndex = 4;
            this.lblRecallType.Text = "Recall Type";
            this.lblRecallType.UseCompatibleTextRendering = true;
            // 
            // btnProviders
            // 
            this.btnProviders.BackColor = System.Drawing.Color.White;
            this.btnProviders.BackgroundColor = System.Drawing.Color.White;
            this.btnProviders.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnProviders.BorderRadius = 5;
            this.btnProviders.BorderSize = 1;
            this.btnProviders.FlatAppearance.BorderSize = 0;
            this.btnProviders.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnProviders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProviders.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnProviders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnProviders.IsCommandButton = false;
            this.btnProviders.Location = new System.Drawing.Point(156, 81);
            this.btnProviders.Name = "btnProviders";
            this.btnProviders.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnProviders.Size = new System.Drawing.Size(122, 25);
            this.btnProviders.TabIndex = 5;
            this.btnProviders.Text = "-";
            this.btnProviders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProviders.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnProviders.UseCompatibleTextRendering = true;
            this.btnProviders.UseVisualStyleBackColor = false;
            // 
            // lblApptStatus
            // 
            this.lblApptStatus.AutoSize = true;
            this.lblApptStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblApptStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblApptStatus.Location = new System.Drawing.Point(20, 266);
            this.lblApptStatus.Name = "lblApptStatus";
            this.lblApptStatus.Size = new System.Drawing.Size(122, 20);
            this.lblApptStatus.TabIndex = 4;
            this.lblApptStatus.Text = "Appointment Status";
            this.lblApptStatus.UseCompatibleTextRendering = true;
            // 
            // lblAppointment
            // 
            this.lblAppointment.AutoSize = true;
            this.lblAppointment.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAppointment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblAppointment.Location = new System.Drawing.Point(20, 24);
            this.lblAppointment.Name = "lblAppointment";
            this.lblAppointment.Size = new System.Drawing.Size(80, 20);
            this.lblAppointment.TabIndex = 4;
            this.lblAppointment.Text = "Appointment";
            this.lblAppointment.UseCompatibleTextRendering = true;
            // 
            // btnSyncSpeciality
            // 
            this.btnSyncSpeciality.BackColor = System.Drawing.Color.White;
            this.btnSyncSpeciality.BackgroundColor = System.Drawing.Color.White;
            this.btnSyncSpeciality.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSyncSpeciality.BorderRadius = 5;
            this.btnSyncSpeciality.BorderSize = 1;
            this.btnSyncSpeciality.FlatAppearance.BorderSize = 0;
            this.btnSyncSpeciality.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSyncSpeciality.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncSpeciality.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSyncSpeciality.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncSpeciality.IsCommandButton = false;
            this.btnSyncSpeciality.Location = new System.Drawing.Point(284, 111);
            this.btnSyncSpeciality.Name = "btnSyncSpeciality";
            this.btnSyncSpeciality.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSyncSpeciality.Size = new System.Drawing.Size(122, 25);
            this.btnSyncSpeciality.TabIndex = 5;
            this.btnSyncSpeciality.Text = "-";
            this.btnSyncSpeciality.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncSpeciality.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncSpeciality.UseCompatibleTextRendering = true;
            this.btnSyncSpeciality.UseVisualStyleBackColor = false;
            // 
            // btnSyncProviders
            // 
            this.btnSyncProviders.BackColor = System.Drawing.Color.White;
            this.btnSyncProviders.BackgroundColor = System.Drawing.Color.White;
            this.btnSyncProviders.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSyncProviders.BorderRadius = 5;
            this.btnSyncProviders.BorderSize = 1;
            this.btnSyncProviders.FlatAppearance.BorderSize = 0;
            this.btnSyncProviders.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSyncProviders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncProviders.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSyncProviders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncProviders.IsCommandButton = false;
            this.btnSyncProviders.Location = new System.Drawing.Point(284, 81);
            this.btnSyncProviders.Name = "btnSyncProviders";
            this.btnSyncProviders.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSyncProviders.Size = new System.Drawing.Size(122, 25);
            this.btnSyncProviders.TabIndex = 5;
            this.btnSyncProviders.Text = "-";
            this.btnSyncProviders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncProviders.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSyncProviders.UseCompatibleTextRendering = true;
            this.btnSyncProviders.UseVisualStyleBackColor = false;
            // 
            // btnSpeciality
            // 
            this.btnSpeciality.BackColor = System.Drawing.Color.White;
            this.btnSpeciality.BackgroundColor = System.Drawing.Color.White;
            this.btnSpeciality.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSpeciality.BorderRadius = 5;
            this.btnSpeciality.BorderSize = 1;
            this.btnSpeciality.FlatAppearance.BorderSize = 0;
            this.btnSpeciality.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSpeciality.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSpeciality.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSpeciality.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSpeciality.IsCommandButton = false;
            this.btnSpeciality.Location = new System.Drawing.Point(156, 111);
            this.btnSpeciality.Name = "btnSpeciality";
            this.btnSpeciality.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnSpeciality.Size = new System.Drawing.Size(122, 25);
            this.btnSpeciality.TabIndex = 5;
            this.btnSpeciality.Text = "-";
            this.btnSpeciality.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSpeciality.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnSpeciality.UseCompatibleTextRendering = true;
            this.btnSpeciality.UseVisualStyleBackColor = false;
            // 
            // lblSpeciality
            // 
            this.lblSpeciality.AutoSize = true;
            this.lblSpeciality.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSpeciality.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblSpeciality.Location = new System.Drawing.Point(20, 115);
            this.lblSpeciality.Name = "lblSpeciality";
            this.lblSpeciality.Size = new System.Drawing.Size(62, 20);
            this.lblSpeciality.TabIndex = 4;
            this.lblSpeciality.Text = "Speciality";
            this.lblSpeciality.UseCompatibleTextRendering = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 3;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.lblConnectionLog, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.lblEHR, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnEHR, 1, 0);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(10, 41);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(436, 33);
            this.tableLayoutPanel8.TabIndex = 7;
            // 
            // lblConnectionLog
            // 
            this.lblConnectionLog.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblConnectionLog.AutoSize = true;
            this.lblConnectionLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblConnectionLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblConnectionLog.Location = new System.Drawing.Point(210, 7);
            this.lblConnectionLog.Name = "lblConnectionLog";
            this.lblConnectionLog.Size = new System.Drawing.Size(58, 19);
            this.lblConnectionLog.TabIndex = 3;
            this.lblConnectionLog.Text = "Error Log";
            this.lblConnectionLog.UseCompatibleTextRendering = true;
            this.lblConnectionLog.Click += new System.EventHandler(this.lblConnectionLog_Click);
            // 
            // lblEHR
            // 
            this.lblEHR.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEHR.AutoSize = true;
            this.lblEHR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblEHR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(39)))));
            this.lblEHR.Location = new System.Drawing.Point(3, 7);
            this.lblEHR.Name = "lblEHR";
            this.lblEHR.Size = new System.Drawing.Size(71, 19);
            this.lblEHR.TabIndex = 4;
            this.lblEHR.Text = "EHR Status";
            this.lblEHR.UseCompatibleTextRendering = true;
            // 
            // btnEHR
            // 
            this.btnEHR.BackColor = System.Drawing.Color.White;
            this.btnEHR.BackgroundColor = System.Drawing.Color.White;
            this.btnEHR.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnEHR.BorderRadius = 5;
            this.btnEHR.BorderSize = 1;
            this.btnEHR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEHR.FlatAppearance.BorderSize = 0;
            this.btnEHR.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnEHR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEHR.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnEHR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnEHR.IsCommandButton = false;
            this.btnEHR.Location = new System.Drawing.Point(80, 3);
            this.btnEHR.Name = "btnEHR";
            this.btnEHR.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnEHR.Size = new System.Drawing.Size(124, 27);
            this.btnEHR.TabIndex = 5;
            this.btnEHR.Text = "-";
            this.btnEHR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEHR.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(89)))), ((int)(((byte)(91)))));
            this.btnEHR.UseCompatibleTextRendering = true;
            this.btnEHR.UseVisualStyleBackColor = false;
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHead.BorderColor = System.Drawing.SystemColors.Control;
            this.pnlHead.BorderFocusColor = System.Drawing.SystemColors.Control;
            this.pnlHead.BorderRadius = 0;
            this.pnlHead.BorderSize = 2;
            this.pnlHead.Controls.Add(this.tblHead);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(449, 32);
            this.pnlHead.TabIndex = 3;
            this.pnlHead.UnderlinedStyle = false;
            // 
            // tblHead
            // 
            this.tblHead.BackColor = System.Drawing.SystemColors.Control;
            this.tblHead.ColumnCount = 6;
            this.tblHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tblHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblHead.Controls.Add(this.picHead, 0, 0);
            this.tblHead.Controls.Add(this.btnClose, 3, 0);
            this.tblHead.Controls.Add(this.btnConfig, 4, 0);
            this.tblHead.Controls.Add(this.lblHead, 1, 0);
            this.tblHead.Controls.Add(this.picAppUpdate, 2, 0);
            this.tblHead.Controls.Add(this.btnAppRestart, 5, 0);
            this.tblHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblHead.Location = new System.Drawing.Point(0, 0);
            this.tblHead.Name = "tblHead";
            this.tblHead.RowCount = 1;
            this.tblHead.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHead.Size = new System.Drawing.Size(449, 32);
            this.tblHead.TabIndex = 0;
            this.tblHead.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tblHead_MouseDown);
            // 
            // picHead
            // 
            this.picHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picHead.Image = global::Pozative.Properties.Resources.PozativeIcon;
            this.picHead.Location = new System.Drawing.Point(3, 3);
            this.picHead.Name = "picHead";
            this.picHead.Size = new System.Drawing.Size(24, 26);
            this.picHead.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picHead.TabIndex = 3;
            this.picHead.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::Pozative.Properties.Resources.Minimize;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(332, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Windows.Forms.Padding(3);
            this.btnClose.Size = new System.Drawing.Size(34, 26);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.BackgroundImage = global::Pozative.Properties.Resources.Setting;
            this.btnConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConfig.FlatAppearance.BorderSize = 0;
            this.btnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfig.Location = new System.Drawing.Point(372, 3);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(34, 26);
            this.btnConfig.TabIndex = 5;
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // lblHead
            // 
            this.lblHead.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblHead.AutoSize = true;
            this.lblHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHead.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(43)))), ((int)(((byte)(64)))));
            this.lblHead.Location = new System.Drawing.Point(33, 5);
            this.lblHead.Name = "lblHead";
            this.lblHead.Size = new System.Drawing.Size(120, 22);
            this.lblHead.TabIndex = 1;
            this.lblHead.Text = "Adit Sync Server";
            this.lblHead.UseCompatibleTextRendering = true;
            this.lblHead.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHeading_MouseDown);
            // 
            // picAppUpdate
            // 
            this.picAppUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picAppUpdate.Image = global::Pozative.Properties.Resources.update_icon;
            this.picAppUpdate.Location = new System.Drawing.Point(279, 3);
            this.picAppUpdate.Name = "picAppUpdate";
            this.picAppUpdate.Size = new System.Drawing.Size(47, 26);
            this.picAppUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAppUpdate.TabIndex = 6;
            this.picAppUpdate.TabStop = false;
            this.picAppUpdate.Click += new System.EventHandler(this.picAppUpdate_Click);
            // 
            // btnAppRestart
            // 
            this.btnAppRestart.BackgroundImage = global::Pozative.Properties.Resources.reload_icon;
            this.btnAppRestart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAppRestart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAppRestart.FlatAppearance.BorderSize = 0;
            this.btnAppRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAppRestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnAppRestart.Location = new System.Drawing.Point(412, 3);
            this.btnAppRestart.Name = "btnAppRestart";
            this.btnAppRestart.Padding = new System.Windows.Forms.Padding(3);
            this.btnAppRestart.Size = new System.Drawing.Size(34, 26);
            this.btnAppRestart.TabIndex = 0;
            this.btnAppRestart.UseVisualStyleBackColor = true;
            this.btnAppRestart.Click += new System.EventHandler(this.btnAppRestart_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(408, 629);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 21);
            this.label5.TabIndex = 26;
            this.label5.Text = "©2023 Adit";
            this.label5.UseCompatibleTextRendering = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.BackgroundImage = global::Pozative.Properties.Resources.Adit_Bg_Image;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(406, 587);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(83, 41);
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // frmPozative
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::Pozative.Properties.Resources.Adit_Bg_Image;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pnlHome);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPozative";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPozative_FormClosing);
            this.Load += new System.EventHandler(this.Pozative_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmPozative_MouseDown);
            this.Resize += new System.EventHandler(this.Pozative_Resize);
            this.tblHome.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.grpPozative.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.pnlGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAppointment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPozativeAppointment)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlHome.ResumeLayout(false);
            this.grpAdit.ResumeLayout(false);
            this.grpAdit.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.pnlHead.ResumeLayout(false);
            this.tblHead.ResumeLayout(false);
            this.tblHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAppUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon NotifyIconPozative;
        private System.Windows.Forms.Panel pnlHome;
        private System.Windows.Forms.TableLayoutPanel tblHome;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblHead;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblSyncLog;
        private System.Windows.Forms.Label lblConnectionLog;
        private System.Windows.Forms.Label lblEHR;
        private System.Windows.Forms.Label lblAppointment;
        private System.Windows.Forms.Label lblPatient;
        private System.Windows.Forms.Label lblOperatories;
        private System.Windows.Forms.Label lblProviders;
        private cButton btnEHR;
        private CPanel pnlHead;
        private System.Windows.Forms.Timer tmrConnectionLog;
        public System.Windows.Forms.RichTextBox txtConnectionLog;
        private System.Windows.Forms.DataGridView grdAppointment;
        private System.Windows.Forms.TableLayoutPanel tblHead;
        private System.Windows.Forms.Label lblApptType;
        private System.Windows.Forms.PictureBox picHead;
        private System.Windows.Forms.Timer tmrConsoleRun;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.GroupBox grpAdit;
        private System.Windows.Forms.GroupBox grpPozative;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPozativeAppt;
        private System.Windows.Forms.Button btnPozativeSyncAppt;
        private System.Windows.Forms.Panel pnlGridView;
        private System.Windows.Forms.DataGridView grdPozativeAppointment;
        private System.Windows.Forms.Button btnAppRestart;
        private System.Windows.Forms.ToolTip TTPView;
        private System.Windows.Forms.Label lblSpeciality;
        private System.Windows.Forms.Timer tmrCheckApplicationUpdate;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Poz_Appt_DateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Poz_Patient_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Poz_Mobile_Contact;
        private System.Windows.Forms.DataGridViewTextBoxColumn Poz_Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn Poz_Operatory_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Poz_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Poz_Appt_EHR_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Poz_Entry_DateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Poz_Appt_LocalDB_ID;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.DataGridViewTextBoxColumn Appt_DateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Patient_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mobile_Contact;
        private System.Windows.Forms.DataGridViewTextBoxColumn Provider_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Operatory_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn ApptType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Appointment_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewLinkColumn ViewHere;
        private System.Windows.Forms.DataGridViewTextBoxColumn Appt_EHR_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Last_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn First_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn MI;
        private System.Windows.Forms.DataGridViewTextBoxColumn Home_Contact;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn City;
        private System.Windows.Forms.DataGridViewTextBoxColumn ST;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zip;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remind_DateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Entry_DateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Appt_LocalDB_ID;
        private System.Windows.Forms.Timer tmrAditLocationSyncEnable;
        private System.Windows.Forms.Label lblApptStatus;
        private System.Windows.Forms.Label lblRecallType;
        private System.Windows.Forms.Timer startGetPatientRecord;
        private System.Windows.Forms.Label lblOperatoryEvent;
        private System.Windows.Forms.PictureBox picAppUpdate;
        private System.Windows.Forms.Label lblHoliday;
        private cButton btnAppt;
        private cButton btnPatient;
        private cButton btnOperatories;
        private cButton btnProviders;
        private cButton btnSyncAppt;
        private cButton btnSyncPatient;
        private cButton btnSyncProviders;
        private cButton btnSyncOperatories;
        private cButton btnApptType;
        private cButton btnSyncApptType;
        private cButton btnSpeciality;
        private cButton btnSyncSpeciality;
        private cButton btnRecallType;
        private cButton btnApptStatus;
        private cButton btnSyncRecallType;
        private cButton btnSyncApptStatus;
        private cButton btnOperatoryEvent;
        private cButton btnSyncOperatoryEvent;
        private cButton btnHoliday;
        private cButton btnSyncHoliday;
        private System.Windows.Forms.Timer tmrApplicationIdleTime;
        private System.Windows.Forms.Timer tmrPaymentSMSCallLog;
        private System.Windows.Forms.Timer tmrSMSLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}