using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pozative
{
    public partial class frmPassword : Form
    {

        #region Variable

        GoalBase ObjGoalBase = new GoalBase();
        public bool Is_PasswordValid = false;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        #endregion

        #region Form_Load

        public frmPassword()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Click

        private void frmPassword_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                Is_PasswordValid = false;
                txtAdminPassword.Focus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Password-Load", ex.Message);
            }
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdminPassword.Text == "Smile")
                {
                    Is_PasswordValid = true;
                    this.Close();
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("Password", "you have entered an invalid password");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Password-Okay", ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Is_PasswordValid = false;
                this.Close();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Password-Close", ex.Message);
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
               
        private void txtAdminPassword_Leave(object sender, EventArgs e)
        {
            btnOkay.Focus();
        }

        #endregion

    }
}
