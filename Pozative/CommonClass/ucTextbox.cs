using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pozative
{
    public partial class ucTextbox : UserControl
    {
        public Label ucLabel { get; set; }
        public TextBox ucTxtbox { get; set; }

        public ucTextbox()
        {
            InitializeComponent();

            ucLabel = new Label();
            ucLabel = lblControlName;

            ucTxtbox = new TextBox();
            ucTxtbox = txtControlValue;

            lblControlName.ForeColor = WDSColor.FormControlForeColor ;
            txtControlValue.ForeColor = WDSColor.FormControlForeColor;
            txtControlValue.Font = WDSColor.FormControlFont;
        }



    }
}
