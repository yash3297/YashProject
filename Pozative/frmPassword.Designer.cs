namespace Pozative
{
    partial class frmPassword
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
            this.txtAdminPassword = new System.Windows.Forms.TextBox();
            this.lblAditPassword = new System.Windows.Forms.Label();
            this.btnOkay = new System.Windows.Forms.Button();
            this.tblAdminUserMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlViewBody = new System.Windows.Forms.Panel();
            this.tblViewBody = new System.Windows.Forms.TableLayoutPanel();
            this.lblFormHead = new System.Windows.Forms.Label();
            this.tblformHead = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.picHead = new System.Windows.Forms.PictureBox();
            this.tblHomeMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHomeMain = new System.Windows.Forms.Panel();
            this.tblAdminUserMain.SuspendLayout();
            this.pnlViewBody.SuspendLayout();
            this.tblViewBody.SuspendLayout();
            this.tblformHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHead)).BeginInit();
            this.tblHomeMain.SuspendLayout();
            this.pnlHomeMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAdminPassword
            // 
            this.txtAdminPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAdminPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAdminPassword.Location = new System.Drawing.Point(107, 45);
            this.txtAdminPassword.Name = "txtAdminPassword";
            this.txtAdminPassword.PasswordChar = '*';
            this.txtAdminPassword.Size = new System.Drawing.Size(257, 26);
            this.txtAdminPassword.TabIndex = 2;
            this.txtAdminPassword.Leave += new System.EventHandler(this.txtAdminPassword_Leave);
            // 
            // lblAditPassword
            // 
            this.lblAditPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAditPassword.AutoSize = true;
            this.lblAditPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAditPassword.Location = new System.Drawing.Point(3, 48);
            this.lblAditPassword.Name = "lblAditPassword";
            this.lblAditPassword.Size = new System.Drawing.Size(98, 20);
            this.lblAditPassword.TabIndex = 3;
            this.lblAditPassword.Text = "Password :   ";
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(370, 41);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(94, 34);
            this.btnOkay.TabIndex = 0;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // tblAdminUserMain
            // 
            this.tblAdminUserMain.BackColor = System.Drawing.Color.White;
            this.tblAdminUserMain.ColumnCount = 3;
            this.tblAdminUserMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblAdminUserMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblAdminUserMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tblAdminUserMain.Controls.Add(this.txtAdminPassword, 1, 1);
            this.tblAdminUserMain.Controls.Add(this.lblAditPassword, 0, 1);
            this.tblAdminUserMain.Controls.Add(this.btnOkay, 2, 1);
            this.tblAdminUserMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblAdminUserMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblAdminUserMain.Location = new System.Drawing.Point(0, 0);
            this.tblAdminUserMain.Margin = new System.Windows.Forms.Padding(0);
            this.tblAdminUserMain.Name = "tblAdminUserMain";
            this.tblAdminUserMain.RowCount = 3;
            this.tblAdminUserMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblAdminUserMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblAdminUserMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblAdminUserMain.Size = new System.Drawing.Size(467, 116);
            this.tblAdminUserMain.TabIndex = 5;
            // 
            // pnlViewBody
            // 
            this.pnlViewBody.Controls.Add(this.tblAdminUserMain);
            this.pnlViewBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlViewBody.Location = new System.Drawing.Point(2, 1);
            this.pnlViewBody.Margin = new System.Windows.Forms.Padding(1);
            this.pnlViewBody.Name = "pnlViewBody";
            this.pnlViewBody.Size = new System.Drawing.Size(467, 116);
            this.pnlViewBody.TabIndex = 0;
            // 
            // tblViewBody
            // 
            this.tblViewBody.ColumnCount = 3;
            this.tblViewBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblViewBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblViewBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblViewBody.Controls.Add(this.pnlViewBody, 1, 0);
            this.tblViewBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblViewBody.Location = new System.Drawing.Point(0, 40);
            this.tblViewBody.Margin = new System.Windows.Forms.Padding(0);
            this.tblViewBody.Name = "tblViewBody";
            this.tblViewBody.RowCount = 2;
            this.tblViewBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblViewBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tblViewBody.Size = new System.Drawing.Size(471, 119);
            this.tblViewBody.TabIndex = 3;
            // 
            // lblFormHead
            // 
            this.lblFormHead.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblFormHead.AutoSize = true;
            this.lblFormHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormHead.Location = new System.Drawing.Point(33, 7);
            this.lblFormHead.Name = "lblFormHead";
            this.lblFormHead.Size = new System.Drawing.Size(352, 25);
            this.lblFormHead.TabIndex = 1;
            this.lblFormHead.Text = "Application Configuration Password";
            this.lblFormHead.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblFormHead_MouseDown);
            // 
            // tblformHead
            // 
            this.tblformHead.ColumnCount = 4;
            this.tblformHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblformHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblformHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblformHead.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
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
            this.tblformHead.Size = new System.Drawing.Size(471, 40);
            this.tblformHead.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(426, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(45, 40);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = " X ";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // picHead
            // 
            this.picHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picHead.Image = global::Pozative.Properties.Resources.PozativeIcon;
            this.picHead.Location = new System.Drawing.Point(3, 3);
            this.picHead.Name = "picHead";
            this.picHead.Size = new System.Drawing.Size(24, 34);
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
            this.tblHomeMain.Location = new System.Drawing.Point(2, 2);
            this.tblHomeMain.Margin = new System.Windows.Forms.Padding(0);
            this.tblHomeMain.Name = "tblHomeMain";
            this.tblHomeMain.RowCount = 2;
            this.tblHomeMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblHomeMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblHomeMain.Size = new System.Drawing.Size(471, 159);
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
            this.pnlHomeMain.Padding = new System.Windows.Forms.Padding(2);
            this.pnlHomeMain.Size = new System.Drawing.Size(475, 163);
            this.pnlHomeMain.TabIndex = 2;
            // 
            // frmPassword
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 163);
            this.Controls.Add(this.pnlHomeMain);
            this.Name = "frmPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPassword";
            this.Load += new System.EventHandler(this.frmPassword_Load);
            this.tblAdminUserMain.ResumeLayout(false);
            this.tblAdminUserMain.PerformLayout();
            this.pnlViewBody.ResumeLayout(false);
            this.tblViewBody.ResumeLayout(false);
            this.tblformHead.ResumeLayout(false);
            this.tblformHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHead)).EndInit();
            this.tblHomeMain.ResumeLayout(false);
            this.pnlHomeMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtAdminPassword;
        private System.Windows.Forms.Label lblAditPassword;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.TableLayoutPanel tblAdminUserMain;
        private System.Windows.Forms.Panel pnlViewBody;
        private System.Windows.Forms.TableLayoutPanel tblViewBody;
        private System.Windows.Forms.Label lblFormHead;
        private System.Windows.Forms.TableLayoutPanel tblformHead;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox picHead;
        private System.Windows.Forms.TableLayoutPanel tblHomeMain;
        private System.Windows.Forms.Panel pnlHomeMain;
    }
}