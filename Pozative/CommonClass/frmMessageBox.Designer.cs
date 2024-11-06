namespace Pozative
{
    partial class frmMessageBox
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
            this.pnlMessageBox = new System.Windows.Forms.Panel();
            this.tblMessagebox = new System.Windows.Forms.TableLayoutPanel();
            this.tblMessageButton = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new System.Windows.Forms.Button();
            this.BtnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.lblMessagetext = new System.Windows.Forms.Label();
            this.lblHead = new System.Windows.Forms.Label();
            this.tmr_AutoClose = new System.Windows.Forms.Timer(this.components);
            this.pnlMessageBox.SuspendLayout();
            this.tblMessagebox.SuspendLayout();
            this.tblMessageButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMessageBox
            // 
            this.pnlMessageBox.Controls.Add(this.tblMessagebox);
            this.pnlMessageBox.Controls.Add(this.lblHead);
            this.pnlMessageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMessageBox.Location = new System.Drawing.Point(0, 0);
            this.pnlMessageBox.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMessageBox.Name = "pnlMessageBox";
            this.pnlMessageBox.Size = new System.Drawing.Size(617, 190);
            this.pnlMessageBox.TabIndex = 1;
            this.pnlMessageBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlMessageBox_MouseDown);
            // 
            // tblMessagebox
            // 
            this.tblMessagebox.ColumnCount = 1;
            this.tblMessagebox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMessagebox.Controls.Add(this.tblMessageButton, 0, 1);
            this.tblMessagebox.Controls.Add(this.lblMessagetext, 0, 0);
            this.tblMessagebox.Location = new System.Drawing.Point(1, 40);
            this.tblMessagebox.Margin = new System.Windows.Forms.Padding(1);
            this.tblMessagebox.Name = "tblMessagebox";
            this.tblMessagebox.Padding = new System.Windows.Forms.Padding(1);
            this.tblMessagebox.RowCount = 2;
            this.tblMessagebox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMessagebox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tblMessagebox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblMessagebox.Size = new System.Drawing.Size(615, 149);
            this.tblMessagebox.TabIndex = 0;
            // 
            // tblMessageButton
            // 
            this.tblMessageButton.ColumnCount = 5;
            this.tblMessageButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblMessageButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblMessageButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblMessageButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblMessageButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblMessageButton.Controls.Add(this.btnOk, 2, 0);
            this.tblMessageButton.Controls.Add(this.BtnYes, 3, 0);
            this.tblMessageButton.Controls.Add(this.btnNo, 4, 0);
            this.tblMessageButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMessageButton.Location = new System.Drawing.Point(4, 91);
            this.tblMessageButton.Name = "tblMessageButton";
            this.tblMessageButton.RowCount = 1;
            this.tblMessageButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMessageButton.Size = new System.Drawing.Size(607, 54);
            this.tblMessageButton.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOk.Location = new System.Drawing.Point(287, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 48);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // BtnYes
            // 
            this.BtnYes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnYes.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnYes.Location = new System.Drawing.Point(393, 3);
            this.BtnYes.Name = "BtnYes";
            this.BtnYes.Size = new System.Drawing.Size(100, 48);
            this.BtnYes.TabIndex = 0;
            this.BtnYes.Text = "Yes";
            this.BtnYes.UseVisualStyleBackColor = true;
            this.BtnYes.Click += new System.EventHandler(this.BtnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnNo.Location = new System.Drawing.Point(499, 3);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(105, 48);
            this.btnNo.TabIndex = 0;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // lblMessagetext
            // 
            this.lblMessagetext.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMessagetext.AutoSize = true;
            this.lblMessagetext.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.lblMessagetext.Location = new System.Drawing.Point(4, 33);
            this.lblMessagetext.Name = "lblMessagetext";
            this.lblMessagetext.Size = new System.Drawing.Size(82, 23);
            this.lblMessagetext.TabIndex = 1;
            this.lblMessagetext.Text = "Message";
            // 
            // lblHead
            // 
            this.lblHead.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblHead.AutoSize = true;
            this.lblHead.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHead.Location = new System.Drawing.Point(3, 5);
            this.lblHead.Name = "lblHead";
            this.lblHead.Size = new System.Drawing.Size(141, 23);
            this.lblHead.TabIndex = 1;
            this.lblHead.Text = "MessageHead";
            this.lblHead.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHead_MouseDown);
            // 
            // tmr_AutoClose
            // 
            this.tmr_AutoClose.Enabled = true;
            this.tmr_AutoClose.Interval = 1000;
            this.tmr_AutoClose.Tick += new System.EventHandler(this.tmr_AutoClose_Tick);
            // 
            // frmMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 190);
            this.ControlBox = false;
            this.Controls.Add(this.pnlMessageBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMessageBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmMessageBox_Load);
            this.pnlMessageBox.ResumeLayout(false);
            this.pnlMessageBox.PerformLayout();
            this.tblMessagebox.ResumeLayout(false);
            this.tblMessagebox.PerformLayout();
            this.tblMessageButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMessageBox;
        private System.Windows.Forms.TableLayoutPanel tblMessagebox;
        private System.Windows.Forms.TableLayoutPanel tblMessageButton;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button BtnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Label lblHead;
        private System.Windows.Forms.Label lblMessagetext;
        private System.Windows.Forms.Timer tmr_AutoClose;
    }
}