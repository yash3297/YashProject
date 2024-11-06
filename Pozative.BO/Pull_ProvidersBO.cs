
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{


    public class Pull_ProvidersBO
    {
        public string message { get; set; }
        public List<GetListProviders> data { get; set; }

    }

    public class GetListProviders
    {
        public string display_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string bio { get; set; }
        public string treatment_min_age { get; set; }
        public string treatment_max_age { get; set; }
        public string Provider_Web_ID { get; set; }
        public string provider_localdb_id { get; set; }
        public string provider_ehr_id { get; set; }
        public string is_active { get; set; }
        public string _id { get; set; }
              
        public string[] accredation { get; set; }
        public string[] membership { get; set; }
        public string[] languages { get; set; }

        public List<Pull_SpecialitiesListAry> Specialities { get; set; }
        public List<Pull_educationListAry> education { get; set; }
       
    }
   
    public class Pull_SpecialitiesListAry
    {
        public Pull_SpecialitiesListAry()
        {
            name = string.Empty;
        }
        public string name { get; set; }
    }

    public class Pull_educationListAry
    {
        public Pull_educationListAry()
        {
            university = string.Empty;
            degree = string.Empty;
        }
        public string university { get; set; }
        public string degree { get; set; }
    }

}
