using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_InsuranceBO
    {        
        public string location { get; set; }
        public string insurance_id { get; set; }
        public string insurancename { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string electid { get; set; }
        public string ehr_id { get; set; }
        public bool is_deleted { get; set; }
        public string employername { get; set; }              
    }
}
