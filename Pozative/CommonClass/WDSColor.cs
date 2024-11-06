using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Pozative
{
    class WDSColor
    {

        #region Common

        static string ApplicationFontType = "Microsoft Sans Serif";
        
        public static Color FormHeadBackColor = Color.FromArgb(226, 226, 226);
        public static Color FormHeadForeColor = Color.Black;
        public static Font FormHeadFont = new Font(ApplicationFontType, 16, FontStyle.Bold);

        public static Color FormBodyBackColor = Color.FromArgb(241, 239, 239);

        public static Color SaveButtonBackColor = Color.FromArgb(244, 137, 33);
        public static Color SaveButtonForeColor = Color.White;

        public static Color ButtonBackColor = Color.FromArgb(154, 155, 157);
        public static Color ButtonForeColor = Color.White;

        public static Font FormControlHeadFont = new Font(ApplicationFontType, 12, FontStyle.Bold);

        public static Color FormControlForeColor = Color.Black;
        public static Color FormControlBackColor = Color.White;
        public static Font FormControlFont = new Font(ApplicationFontType, 10, FontStyle.Regular);
        public static Font FormButtonFont = new Font(ApplicationFontType, 12, FontStyle.Bold);
        
        #endregion

        #region PopupNotifier

        public static Font PopupNotifierHeadFont = new Font(ApplicationFontType, 10, FontStyle.Bold);
        public static Font PopupNotifierControlFont = new Font(ApplicationFontType, 10, FontStyle.Regular);

        public static Color PopupNotifierHeadBackColor = Color.FromArgb(226, 226, 226);
        public static Color PopupNotifierHeadForeColor = Color.FromArgb(100, 100, 100);
        public static Color PopupNotifierBodyBackColor = Color.White;
        public static Color PopupNotifierControlForeColor = Color.FromArgb(100, 100, 100);

        #endregion

        #region Messagebox

        public static Color MessageboxButtonBackColor = Color.FromArgb(75, 75, 75);
        public static Color MessageboxButtonForeColor = Color.White;

        public static Color MessageboxValidationBackColor = Color.FromArgb(252, 139, 18);   // Color.FromArgb(255, 159, 0);
        public static Color MessageboxValidationForeColor = Color.White;

        public static Color MessageboxErrorBackColor = Color.White ;
        public static Color MessageboxErrorForeColor = Color.Black ;

        public static Color MessageboxSuccessBackColor = Color.White;
        public static Color MessageboxSuccessForeColor = Color.Black ;

        #endregion

    }
}
