using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Pozative
{
    public partial class frmMessageBox : Form
    {
        public bool Status;
        string tmpMsgHead;
        string tmpMsgTxt;
        int tmpMsgType;
        DateTime duration;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public frmMessageBox(string MsgHead, string MsgTxt, int MsgType)
        {
            InitializeComponent();
            tmpMsgHead = MsgHead;
            tmpMsgTxt = MsgTxt;
            tmpMsgType = MsgType;
        }

        private void frmMessageBox_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Status = false;
                lblHead.Text = tmpMsgHead;
                lblMessagetext.Text = tmpMsgTxt;

                btnOk.ForeColor = WDSColor.MessageboxButtonForeColor;
                BtnYes.ForeColor = WDSColor.MessageboxButtonForeColor;
                btnNo.ForeColor = WDSColor.MessageboxButtonForeColor;

                btnOk.BackColor = WDSColor.MessageboxButtonBackColor;
                BtnYes.BackColor = WDSColor.MessageboxButtonBackColor;
                btnNo.BackColor = WDSColor.MessageboxButtonBackColor;


                switch (tmpMsgType)
                {
                    case 1:
                        pnlMessageBox.BackColor = Color.FromArgb(252, 139, 18);
                        tblMessagebox.BackColor = Color.White;

                        lblHead.ForeColor = Color.White;
                        lblMessagetext.ForeColor = Color.FromArgb(252, 139, 18);

                        BtnYes.Visible = true;
                        btnNo.Visible = true;
                        btnOk.Visible = false;
                        break;
                    case 2:
                        pnlMessageBox.BackColor = WDSColor.MessageboxErrorBackColor;

                        pnlMessageBox.BackColor = Color.FromArgb(150, 1, 2);
                        tblMessagebox.BackColor = Color.White;

                        lblHead.ForeColor = Color.White;
                        lblMessagetext.ForeColor = Color.FromArgb(150, 1, 2);

                        BtnYes.Visible = false;
                        btnNo.Visible = false;
                        btnOk.Visible = true;
                        break;
                    case 3:
                        pnlMessageBox.BackColor = WDSColor.MessageboxSuccessBackColor;
                        lblHead.ForeColor = WDSColor.MessageboxSuccessForeColor;
                        lblMessagetext.ForeColor = WDSColor.MessageboxSuccessForeColor;

                        pnlMessageBox.BackColor = Color.FromArgb(0, 128, 0);
                        tblMessagebox.BackColor = Color.White;

                        lblHead.ForeColor = Color.White;
                        lblMessagetext.ForeColor = Color.FromArgb(0, 128, 0);

                        BtnYes.Visible = false;
                        btnNo.Visible = false;
                        btnOk.Visible = true;
                        break;

                    case 4:
                        pnlMessageBox.BackColor = WDSColor.MessageboxErrorBackColor;

                        pnlMessageBox.BackColor = Color.FromArgb(244, 137, 33);
                        tblMessagebox.BackColor = Color.White;

                        lblHead.ForeColor = Color.White;
                        lblMessagetext.ForeColor = Color.FromArgb(244, 137, 33);

                        BtnYes.Visible = false;
                        btnNo.Visible = false;
                        btnOk.Visible = true;
                        break;
                }

                //if (tmpMsgType == 1) // Validation message
                //{

                //    pnlMessageBox.BackColor = Color.FromArgb(252, 139, 18);
                //    tblMessagebox.BackColor = Color.White;

                //    lblHead.ForeColor = Color.White;
                //    lblMessagetext.ForeColor = Color.FromArgb(252, 139, 18);

                //    BtnYes.Visible = true;
                //    btnNo.Visible = true;
                //    btnOk.Visible = false;
                //}

                //else if (tmpMsgType == 2)  // Error message
                //{
                //    pnlMessageBox.BackColor = WDSColor.MessageboxErrorBackColor;

                //    pnlMessageBox.BackColor = Color.FromArgb(150,1,2);
                //    tblMessagebox.BackColor = Color.White;

                //    lblHead.ForeColor = Color.White;
                //    lblMessagetext.ForeColor = Color.FromArgb(150, 1, 2);

                //    BtnYes.Visible = false;
                //    btnNo.Visible = false;
                //    btnOk.Visible = true;
                //}

                //else if (tmpMsgType == 3)  // Success message
                //{
                //    pnlMessageBox.BackColor = WDSColor.MessageboxSuccessBackColor;
                //    lblHead.ForeColor = WDSColor.MessageboxSuccessForeColor;
                //    lblMessagetext.ForeColor = WDSColor.MessageboxSuccessForeColor;

                //    pnlMessageBox.BackColor = Color.FromArgb(0, 128, 0);
                //    tblMessagebox.BackColor = Color.White;

                //    lblHead.ForeColor = Color.White;
                //    lblMessagetext.ForeColor = Color.FromArgb(0, 128, 0);

                //    BtnYes.Visible = false;
                //    btnNo.Visible = false;
                //    btnOk.Visible = true;
                //}
            }
            catch (Exception)
            {
            }
            finally
            {
                duration = DateTime.Now;
                tmr_AutoClose.Start();
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void BtnYes_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Status = true;
                this.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Status = false;
                this.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void pnlMessageBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void lblHead_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void tmr_AutoClose_Tick(object sender, EventArgs e)
        {
            if (duration.AddSeconds(30) <= DateTime.Now)
            {
                Status = true;
                this.Close();
            }
        }

    }
}
