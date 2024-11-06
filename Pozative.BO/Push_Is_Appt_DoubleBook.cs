using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_Is_Appt_DoubleBook
    {

        public Push_Is_Appt_DoubleBook()
        {
            conflictAppointments = new List<Push_conflictAppointmentsListAry>();
        }

        public List<Push_conflictAppointmentsListAry> conflictAppointments { get; set; }

    }

    public class Push_conflictAppointmentsListAry
    {
        public Push_conflictAppointmentsListAry()
        {
            Appt_Web_ID = string.Empty;
            Organization_ID = string.Empty;
            Location_ID = string.Empty;
            created_by = string.Empty;
        }
        public string Appt_Web_ID { get; set; }
        public string Organization_ID { get; set; }
        public string Location_ID { get; set; }
        public string created_by { get; set; }
    }
}
