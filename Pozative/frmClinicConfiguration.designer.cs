namespace Pozative
{
    partial class frmClinicConfiguration
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
            this.tblViewBody = new System.Windows.Forms.TableLayoutPanel();
            this.pnlViewBody = new System.Windows.Forms.Panel();
            this.tblAdminUserMain = new System.Windows.Forms.TableLayoutPanel();
            this.grpAditLoc = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.DGVMuliClinc = new System.Windows.Forms.DataGridView();
            this.Is_Location_Config = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Service_Install_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Location_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Clinic_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Location = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Clinic_Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AditSync = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLocationSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cboDatabaseList = new System.Windows.Forms.ComboBox();
            this.LblAditLocationSingle = new System.Windows.Forms.Label();
            this.lblFormHead = new System.Windows.Forms.Label();
            this.tblformHead = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.picHead = new System.Windows.Forms.PictureBox();
            this.tblHomeMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHomeMain = new System.Windows.Forms.Panel();
            this.tblViewBody.SuspendLayout();
            this.pnlViewBody.SuspendLayout();
            this.tblAdminUserMain.SuspendLayout();
            this.grpAditLoc.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVMuliClinc)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tblformHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHead)).BeginInit();
            this.tblHomeMain.SuspendLayout();
            this.pnlHomeMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblViewBody
            // 
            this.tblViewBody.ColumnCount = 3;
            this.tblViewBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblViewBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblViewBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblViewBody.Controls.Add(this.pnlViewBody, 1, 0);
            this.tblViewBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblViewBody.Location = new System.Drawing.Point(0, 49);
            this.tblViewBody.Margin = new System.Windows.Forms.Padding(0);
            this.tblViewBody.Name = "tblViewBody";
            this.tblViewBody.RowCount = 2;
            this.tblViewBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblViewBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblViewBody.Size = new System.Drawing.Size(777, 476);
            this.tblViewBody.TabIndex = 3;
            // 
            // pnlViewBody
            // 
            this.pnlViewBody.Controls.Add(this.tblAdminUserMain);
            this.pnlViewBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlViewBody.Location = new System.Drawing.Point(2, 1);
            this.pnlViewBody.Margin = new System.Windows.Forms.Padding(1);
            this.pnlViewBody.Name = "pnlViewBody";
            this.pnlViewBody.Size = new System.Drawing.Size(773, 473);
            this.pnlViewBody.TabIndex = 0;
            // 
            // tblAdminUserMain
            // 
            this.tblAdminUserMain.BackColor = System.Drawing.Color.White;
            this.tblAdminUserMain.ColumnCount = 1;
            this.tblAdminUserMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblAdminUserMain.Controls.Add(this.grpAditLoc, 0, 0);
            this.tblAdminUserMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblAdminUserMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblAdminUserMain.Location = new System.Drawing.Point(0, 0);
            this.tblAdminUserMain.Margin = new System.Windows.Forms.Padding(0);
            this.tblAdminUserMain.Name = "tblAdminUserMain";
            this.tblAdminUserMain.RowCount = 1;
            this.tblAdminUserMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblAdminUserMain.Size = new System.Drawing.Size(773, 473);
            this.tblAdminUserMain.TabIndex = 5;
            // 
            // grpAditLoc
            // 
            this.grpAditLoc.Controls.Add(this.tableLayoutPanel11);
            this.grpAditLoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAditLoc.Location = new System.Drawing.Point(4, 4);
            this.grpAditLoc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpAditLoc.Name = "grpAditLoc";
            this.grpAditLoc.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpAditLoc.Size = new System.Drawing.Size(765, 465);
            this.grpAditLoc.TabIndex = 7;
            this.grpAditLoc.TabStop = false;
            this.grpAditLoc.Text = "Adit App";
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 4;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.Controls.Add(this.DGVMuliClinc, 2, 3);
            this.tableLayoutPanel11.Controls.Add(this.tableLayoutPanel1, 2, 4);
            this.tableLayoutPanel11.Controls.Add(this.tableLayoutPanel2, 2, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(4, 27);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 5;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(757, 434);
            this.tableLayoutPanel11.TabIndex = 0;
            // 
            // DGVMuliClinc
            // 
            this.DGVMuliClinc.AllowUserToAddRows = false;
            this.DGVMuliClinc.AllowUserToDeleteRows = false;
            this.DGVMuliClinc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVMuliClinc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Is_Location_Config,
            this.Service_Install_Id,
            this.Location_Id,
            this.Clinic_Name,
            this.Location,
            this.Clinic_Number,
            this.AditSync});
            this.DGVMuliClinc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVMuliClinc.Location = new System.Drawing.Point(4, 53);
            this.DGVMuliClinc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DGVMuliClinc.Name = "DGVMuliClinc";
            this.DGVMuliClinc.RowHeadersWidth = 5;
            this.DGVMuliClinc.Size = new System.Drawing.Size(749, 315);
            this.DGVMuliClinc.TabIndex = 5;
            // 
            // Is_Location_Config
            // 
            this.Is_Location_Config.DataPropertyName = "Is_Location_Config";
            this.Is_Location_Config.HeaderText = "Is_Location_Config";
            this.Is_Location_Config.MinimumWidth = 6;
            this.Is_Location_Config.Name = "Is_Location_Config";
            this.Is_Location_Config.ReadOnly = true;
            this.Is_Location_Config.Visible = false;
            this.Is_Location_Config.Width = 125;
            // 
            // Service_Install_Id
            // 
            this.Service_Install_Id.DataPropertyName = "Service_Install_Id";
            this.Service_Install_Id.HeaderText = "Service_Install_Id";
            this.Service_Install_Id.MinimumWidth = 6;
            this.Service_Install_Id.Name = "Service_Install_Id";
            this.Service_Install_Id.Visible = false;
            this.Service_Install_Id.Width = 125;
            // 
            // Location_Id
            // 
            this.Location_Id.DataPropertyName = "Location_Id";
            this.Location_Id.HeaderText = "Location Id";
            this.Location_Id.MinimumWidth = 6;
            this.Location_Id.Name = "Location_Id";
            this.Location_Id.Visible = false;
            this.Location_Id.Width = 125;
            // 
            // Clinic_Name
            // 
            this.Clinic_Name.DataPropertyName = "Clinic_Name";
            this.Clinic_Name.HeaderText = "Clinic Name";
            this.Clinic_Name.MinimumWidth = 6;
            this.Clinic_Name.Name = "Clinic_Name";
            this.Clinic_Name.ReadOnly = true;
            this.Clinic_Name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Clinic_Name.Width = 210;
            // 
            // Location
            // 
            this.Location.DataPropertyName = "Location";
            this.Location.HeaderText = "Location";
            this.Location.MinimumWidth = 6;
            this.Location.Name = "Location";
            this.Location.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Location.Width = 260;
            // 
            // Clinic_Number
            // 
            this.Clinic_Number.DataPropertyName = "Clinic_Number";
            this.Clinic_Number.HeaderText = "Clinic_Number";
            this.Clinic_Number.MinimumWidth = 6;
            this.Clinic_Number.Name = "Clinic_Number";
            this.Clinic_Number.Visible = false;
            this.Clinic_Number.Width = 125;
            // 
            // AditSync
            // 
            this.AditSync.DataPropertyName = "AditSync";
            this.AditSync.HeaderText = "AditSync";
            this.AditSync.MinimumWidth = 6;
            this.AditSync.Name = "AditSync";
            this.AditSync.ReadOnly = true;
            this.AditSync.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AditSync.Width = 80;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 376);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(748, 54);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel4.Controls.Add(this.btnCancel, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnLocationSave, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(748, 54);
            this.tableLayoutPanel4.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Location = new System.Drawing.Point(619, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(125, 46);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLocationSave
            // 
            this.btnLocationSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLocationSave.Location = new System.Drawing.Point(486, 4);
            this.btnLocationSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLocationSave.Name = "btnLocationSave";
            this.btnLocationSave.Size = new System.Drawing.Size(125, 46);
            this.btnLocationSave.TabIndex = 8;
            this.btnLocationSave.Text = "Save";
            this.btnLocationSave.UseVisualStyleBackColor = true;
            this.btnLocationSave.Click += new System.EventHandler(this.btnLocationSave_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.cboDatabaseList, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.LblAditLocationSingle, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(748, 41);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // cboDatabaseList
            // 
            this.cboDatabaseList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDatabaseList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDatabaseList.FormattingEnabled = true;
            this.cboDatabaseList.Location = new System.Drawing.Point(180, 2);
            this.cboDatabaseList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboDatabaseList.Name = "cboDatabaseList";
            this.cboDatabaseList.Size = new System.Drawing.Size(565, 33);
            this.cboDatabaseList.TabIndex = 7;
            this.cboDatabaseList.SelectedValueChanged += new System.EventHandler(this.cboDatabaseList_SelectedValueChanged);
            // 
            // LblAditLocationSingle
            // 
            this.LblAditLocationSingle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LblAditLocationSingle.AutoSize = true;
            this.LblAditLocationSingle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblAditLocationSingle.Location = new System.Drawing.Point(4, 8);
            this.LblAditLocationSingle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblAditLocationSingle.Name = "LblAditLocationSingle";
            this.LblAditLocationSingle.Size = new System.Drawing.Size(169, 25);
            this.LblAditLocationSingle.TabIndex = 4;
            this.LblAditLocationSingle.Text = "Database Name : ";
            // 
            // lblFormHead
            // 
            this.lblFormHead.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblFormHead.AutoSize = true;
            this.lblFormHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormHead.Location = new System.Drawing.Point(44, 9);
            this.lblFormHead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFormHead.Name = "lblFormHead";
            this.lblFormHead.Size = new System.Drawing.Size(390, 31);
            this.lblFormHead.TabIndex = 1;
            this.lblFormHead.Text = "Application Configuration Clinic";
            this.lblFormHead.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblFormHead_MouseDown);
            // 
            // tblformHead
            // 
            this.tblformHead.ColumnCount = 4;
            this.tblformHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblformHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblformHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblformHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tblformHead.Controls.Add(this.btnClose, 3, 0);
            this.tblformHead.Controls.Add(this.lblFormHead, 1, 0);
            this.tblformHead.Controls.Add(this.picHead, 0, 0);
            this.tblformHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblformHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblformHead.Location = new System.Drawing.Point(0, 0);
            this.tblformHead.Margin = new System.Windows.Forms.Padding(0);
            this.tblformHead.Name = "tblformHead";
            this.tblformHead.RowCount = 1;
            this.tblformHead.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblformHead.Size = new System.Drawing.Size(777, 49);
            this.tblformHead.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(717, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(60, 49);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = " X ";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // picHead
            // 
            this.picHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picHead.Image = global::Pozative.Properties.Resources.PozativeIcon;
            this.picHead.Location = new System.Drawing.Point(4, 4);
            this.picHead.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picHead.Name = "picHead";
            this.picHead.Size = new System.Drawing.Size(32, 41);
            this.picHead.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picHead.TabIndex = 2;
            this.picHead.TabStop = false;
            // 
            // tblHomeMain
            // 
            this.tblHomeMain.ColumnCount = 1;
            this.tblHomeMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHomeMain.Controls.Add(this.tblformHead, 0, 0);
            this.tblHomeMain.Controls.Add(this.tblViewBody, 0, 1);
            this.tblHomeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblHomeMain.Location = new System.Drawing.Point(3, 2);
            this.tblHomeMain.Margin = new System.Windows.Forms.Padding(0);
            this.tblHomeMain.Name = "tblHomeMain";
            this.tblHomeMain.RowCount = 2;
            this.tblHomeMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tblHomeMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHomeMain.Size = new System.Drawing.Size(777, 525);
            this.tblHomeMain.TabIndex = 2;
            // 
            // pnlHomeMain
            // 
            this.pnlHomeMain.Controls.Add(this.tblHomeMain);
            this.pnlHomeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHomeMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlHomeMain.Location = new System.Drawing.Point(0, 0);
            this.pnlHomeMain.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHomeMain.Name = "pnlHomeMain";
            this.pnlHomeMain.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlHomeMain.Size = new System.Drawing.Size(783, 529);
            this.pnlHomeMain.TabIndex = 2;
            // 
            // frmClinicConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 529);
            this.Controls.Add(this.pnlHomeMain);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(798, 490);
            this.Name = "frmClinicConfiguration";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Clinic";
            this.Load += new System.EventHandler(this.frmClinicConfiguration_Load);
            this.tblViewBody.ResumeLayout(false);
            this.pnlViewBody.ResumeLayout(false);
            this.tblAdminUserMain.ResumeLayout(false);
            this.grpAditLoc.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVMuliClinc)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tblformHead.ResumeLayout(false);
            this.tblformHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHead)).EndInit();
            this.tblHomeMain.ResumeLayout(false);
            this.pnlHomeMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblViewBody;
        private System.Windows.Forms.Label lblFormHead;
        private System.Windows.Forms.TableLayoutPanel tblformHead;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox picHead;
        private System.Windows.Forms.TableLayoutPanel tblHomeMain;
        private System.Windows.Forms.Panel pnlHomeMain;
        private System.Windows.Forms.Panel pnlViewBody;
        private System.Windows.Forms.TableLayoutPanel tblAdminUserMain;
        private System.Windows.Forms.GroupBox grpAditLoc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.DataGridView DGVMuliClinc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLocationSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label LblAditLocationSingle;
        private System.Windows.Forms.ComboBox cboDatabaseList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Is_Location_Config;
        private System.Windows.Forms.DataGridViewTextBoxColumn Service_Install_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Location_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Clinic_Name;
        private System.Windows.Forms.DataGridViewComboBoxColumn Location;
        private System.Windows.Forms.DataGridViewTextBoxColumn Clinic_Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn AditSync;
    }
}