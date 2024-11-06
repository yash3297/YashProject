using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{

    public class Push_ProvidersBO
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
            public string organization { get; set; }

            public string locations { get; set; }
            public string created_by { get; set; }
            public string[] accredation { get; set; }
            public string[] membership { get; set; }
            public string[] languages { get; set; }
            
            public bool is_active { get; set; }

            public Push_ProvidersBO()
            {
                specialities = new List<Push_SpecialitiesListAry>();
                education = new List<Push_educationListAry>();
            }

            public List<Push_SpecialitiesListAry> specialities { get; set; }
            public List<Push_educationListAry> education { get; set; }
       
    }
  
    public class LocationListAry
    {
        public LocationListAry()
        {
            Location_id = string.Empty;

        }
        public string Location_id { get; set; }

    }

    public class Push_SpecialitiesListAry
    {
        public Push_SpecialitiesListAry()
        {
            name = string.Empty;
        }
        public string name { get; set; }
    }

    public class Push_educationListAry
    {
        public Push_educationListAry()
        {
            university = string.Empty;
            degree = string.Empty;
        }
        public string university { get; set; }
        public string degree { get; set; }
    }

}
