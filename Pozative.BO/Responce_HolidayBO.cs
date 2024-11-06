using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Responce_HolidayBO
    {
        public string message { get; set; }
        public List<GetListHoliday> data { get; set; }

    }

    public class GetListHoliday
    {
        public string _id { get; set; }
        public string H_localdb_id { get; set; }
        public string appt_ehr_id { get; set; }
        
    }
}