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
    public partial class frmAppointmentDetail : Form
    {

        #region Variable

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")] // Dipika
        public static extern bool ReleaseCapture();

        #endregion

        #region Form Load

        public frmAppointmentDetail(string PatientName, string HomeContact, string MobileContact, string Email, string Address,
                                    string City, string Zip, string State, string Date, string Time, string Operatory, string Provider,
                                    string Type, string Status)
        {
            InitializeComponent();

            txtPatientName.Text = PatientName;
            txtHomeContact.Text = HomeContact;
            txtMobileContact.Text = MobileContact;
            txtEmail.Text = Email;
            txtAddress.Text = Address;
            txtCity.Text = City;
            txtZip.Text = Zip;
            txtState.Text = State;
            txtDate.Text = Convert.ToString(Convert.ToDateTime(Date).ToString("MM/dd/yyyy"));
            txtTime.Text = Convert.ToString(Convert.ToDateTime(Time).ToString("hh:mm tt"));
            txtProvider.Text = Provider.Replace(";","");
            txtOperatory.Text = Operatory;
            txtType.Text = Type;
            txtStatus.Text = Status;
        }

        private void frmAppointmentDetail_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                pnlHead.BackColor = WDSColor.FormHeadBackColor;
                lblHead.ForeColor = WDSColor.FormHeadForeColor;
                btnClose.ForeColor = WDSColor.FormHeadForeColor;
                this.BackColor = WDSColor.FormHeadBackColor;

                lblPatientInfo.Focus();
                lblPatientInfo.Select();
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

        #region Button Click

        private void btnClose_Click(object sender, EventArgs e)
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
 
        #endregion

        #region Common Event

        private void tblHead_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void lblHeading_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

    }
}
