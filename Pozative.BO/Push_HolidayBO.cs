using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_HolidayBO
    {
        public string Location_ID { get; set; }
        public string ParentLocation_ID { get; set; }
        public string Organization_ID { get; set; }
        public string H_EHR_ID { get; set; }
        public string H_Web_ID { get; set; }
        public string H_Operatory_EHR_ID { get; set; }
        public string SchedDate { get; set; }
        public string StartTime_1 { get; set; }
        public string EndTime_1 { get; set; }
        public string StartTime_2 { get; set; }
        public string EndTime_2 { get; set; }
        public string StartTime_3 { get; set; }
        public string EndTime_3 { get; set; }
        public string comment { get; set; }
        public bool is_deleted { get; set; }               
        public string created_by { get; set; }
        
    }
}
