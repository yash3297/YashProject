using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Responce_ProviderOfficeHours
    {
        public string message { get; set; }
        public List<GetListProviderOfficeHours> data { get; set; }

    }

    public class GetListProviderOfficeHours
    {        
        public string provider_ehr_id { get; set; }
        public string _id { get; set; }

    }
}
