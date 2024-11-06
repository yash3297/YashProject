using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_DiseaseBO
    {
        public string message { get; set; }
        public List<GetListDisease> data { get; set; }

    }

    public class GetListDisease
    {
        public string disease_name { get; set; }
        public string disease_type { get; set; }
        public string is_deleted { get; set; }
        public string disease_Web_ID { get; set; }
        public string disease_localdb_id { get; set; }
        public string disease_ehr_id { get; set; }

        public string _id { get; set; }

    }
    public class Pull_PatientDiseaseBO
    {
        public string message { get; set; }
        public List<GetListPatientDisease> data { get; set; }

    }

    public class GetListPatientDisease
    {
        public string disease_name { get; set; }
        public string disease_type { get; set; }
        public string is_deleted { get; set; }
        public string Patientdisease_Web_ID { get; set; }
        public string disease_localdb_id { get; set; }
        public string disease_ehr_id { get; set; }
        public string Patient_ehr_id { get; set; }

        public string _id { get; set; }

    }
}
