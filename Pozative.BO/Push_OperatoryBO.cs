using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
  public class Push_OperatoryBO
    {

      public string appointmentlocation { get; set; }
      public string name { get; set; }
      public string organization { get; set; }
      public string operatory_Web_ID { get; set; }
      public string operatory_localdb_id { get; set; }
      public string operatory_ehr_id { get; set; }
      public bool is_deleted { get; set; }
      public string created_by { get; set; }
      public int sort_order { get; set; }

  }
}
