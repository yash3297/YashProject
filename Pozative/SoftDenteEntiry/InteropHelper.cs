using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pozative
{
    public static class InteropHelper
    {
        public static string PtrToString(IntPtr ptr)
        {
            return InteropHelper.PtrToStringOrNull(ptr) ?? string.Empty;
        }

        public static string PtrToStringOrNull(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return (string)null;
            string stringBstr = Marshal.PtrToStringBSTR(ptr);
           // MessageBox.Show("PtrToStringOrNull 1");
            Marshal.FreeBSTR(ptr);
           // MessageBox.Show("PtrToStringOrNull 2");
            return stringBstr;
        }

        public static void DateTimeToString(DateTime dateTime, out string date, out string time)
        {
            date = dateTime.ToShortDateString();
            time = dateTime.ToShortTimeString();
        }
    }
}
