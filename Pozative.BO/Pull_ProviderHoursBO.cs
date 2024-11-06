using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
  public  class Pull_ProviderHoursBO
    {
        public string message { get; set; }
        public List<GetListProviderHours> data { get; set; }
    }

  public class GetListProviderHours
  {
      public string ph_localdb_id { get; set; }
      public string ph_ehr_id { get; set; }
      public string _id { get; set; }
      public string provider_ehr_id { get; set; }
      public string operatory_ehr_id { get; set; }
      public string starttime { get; set; }
      public string endtime { get; set; }
      public string comment { get; set; }
      public string is_deleted { get; set; }

    
  }
   
}
