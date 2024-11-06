using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_ProviderOfficeHours
    {
        public string Location_ID { get; set; }
        public string ParentLocation_ID { get; set; }
        public string Organization_ID { get; set; }
        public string POH_EHR_ID { get; set; }
        public string POH_Web_ID { get; set; }
        public string Provider_EHR_ID { get; set; }
        public string WeekDay { get; set; }
        public string StartTime1 { get; set; }
        public string EndTime1 { get; set; }
        public string StartTime2 { get; set; }
        public string EndTime2 { get; set; }
        public string StartTime3 { get; set; }
        public string EndTime3 { get; set; }        
        public bool is_deleted { get; set; }
        public string created_by { get; set; }
    }
}
