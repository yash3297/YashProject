using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_OperatoryHoursBO
    {
        public string message { get; set; }
        public List<GetListOperatoryHours> data { get; set; }
    }

    public class GetListOperatoryHours
    {
        public string oh_localdb_id { get; set; }
        public string oh_ehr_id { get; set; }
        public string _id { get; set; }
        public string operatory_ehr_id { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string comment { get; set; }
        public string is_deleted { get; set; }
    }

}