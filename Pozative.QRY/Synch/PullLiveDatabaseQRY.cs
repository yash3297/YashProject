using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pozative.QRY
{
  public  class PullLiveDatabaseQRY
  {

      #region Appointment

      public static string GetLive_Appointment = "";

      public static string GetLocal_AppointmentData = "SELECT * FROM Appointment WITH(NOLOCK) ";

      public static string Insert_Appointment = " INSERT INTO Appointment (Appt_Web_ID, Operatory_Name, Provider_Name, Last_Name, First_Name, MI, Home_Contact, Mobile_Contact, " 
                                              + " Email, Address, City, ST, Zip, ApptType, Appt_DateTime, Status , Patient_Status , Appointment_Status , Is_Appt ) "
                                              + " VALUES (@Appt_Web_ID, @Operatory_Name, @Provider_Name, @Last_Name, @First_Name, @MI, @Home_Contact, @Mobile_Contact, " 
                                              + " @Email, @Address, @City, @ST, @Zip, @ApptType, @Appt_DateTime, @Status, @Patient_Status, @Appointment_Status, @Is_Appt)";

        public static string Update_Appointment = " ";

        public static string Delete_Appointment = " ";

        #endregion

    }
}
