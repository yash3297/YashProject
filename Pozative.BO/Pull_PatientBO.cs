using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_PatientBO
    {
        public string message { get; set; }
        public List<GetListpatient> data { get; set; }
    }

    public class GetListpatient
    {
        public string _id { get; set; }
        public string patient_localdb_id { get; set; }
        public string patient_ehr_id { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public string last_visit { get; set; }
        public string next_visit { get; set; }
        public string birth_date { get; set; }
        public string revenue { get; set; }

    }
}
