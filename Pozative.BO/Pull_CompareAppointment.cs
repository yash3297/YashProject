using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   
        public class Datum1
        {
            public string appt_ehr_id { get; set; }
        }

        public class Pull_CompareAppointment
        {
            public bool status { get; set; }
            public List<Datum1> data { get; set; }
            public object error { get; set; }
            public string message { get; set; }
        }

        public class Push_DeletedAppt
        {
            public string Location_ID { get; set; }
            public string Organization_ID { get; set; }
            public string appt_ehr_id { get; set; }
            public string created_by { get; set; }
        }
    }

