using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_PatientBalance
    {
        public string current_bal { get; set; }
        public string thirty_day { get; set; }
        public string sixty_day { get; set; }
        public string ninety_day { get; set; }
        public string over_ninty { get; set; }
        public string remaining_benefit { get; set; }
        public string used_benefit { get; set; }
        public string collect_payment { get; set; }
        public string patient_ehr_id { get; set; }
        public string appointmentlocation { get; set; }
        public string organization { get; set; }
    }

    public class Push_PatientBalance_Response
    {
        public string message { get; set; }
        public string code { get; set; }
        public string status { get; set; }
        public List<GetListpatientBalance> data { get; set; }
    }

    public class GetListpatientBalance
    {
        public string _id { get; set; }
        public string patient_ehr_id { get; set; }

    }

}
