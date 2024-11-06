using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class RecallTypeBO
    {


    }

    public class RecallTypeMainBO
    {
        public string ap_status { get; set; }
        public string recall { get; set; }
        public string organization { get; set; }
        public string[] locationId { get; set; }
        public string created_by { get; set; }
        public bool is_write { get; set; }
    }

    public class RecallTypeSubBO
    {
        public RecallTypeSubBO()
        {
            name = string.Empty;
            key = string.Empty;
            ehr_key = string.Empty;
            is_deleted = false;
        }

        public string name { get; set; }
        public string key { get; set; }
        public string ehr_key { get; set; }
        public bool is_deleted { get; set; }
    }
}
