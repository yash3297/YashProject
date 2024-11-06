using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pozative
{
    public partial class ShowMessage : Form
    {
        //public string MessageDisplay = "";
        bool playAudio = false; 
        string audioText = "";
        public ShowMessage(string MessageDisplay, bool PlayAudio = false, string AudioText = "")
        {
            InitializeComponent();
            tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            lblMessage.BackColor = System.Drawing.Color.Transparent;
            lblMessage.Text = MessageDisplay;
            playAudio = PlayAudio;
            audioText = AudioText;
        }

        private void btnOkay_Click(object sender, EventArgs e)
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

        private void ShowMessage_Shown(object sender, EventArgs e)
        {
            groupBox1.Focus();
            if (playAudio)
            {
                CommonFunction.TextToSpeech(audioText);
            }
        }

        private void lblMessage_Resize(object sender, EventArgs e)
        {
            if(lblMessage.Height > 20)
            {
                int h = lblMessage.Height - 20;
                tableLayoutPanel1.Height = 46 + h;
                groupBox1.Height = 143 + h;
                this.Height = 135 + h;
            }
        }
    }
}
