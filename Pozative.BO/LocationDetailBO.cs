using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class LocationDetailBO
    {
        public string message { get; set; }
        public string error { get; set; }
        public List<MainData> data { get; set; }
    }

    public class MainData
    {
     
        public string name { get; set; }
        public string system_mac_address { get; set; }
        public string _id { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string is_active { get; set; }
        public string website_url { get; set; }
        public LocTimezone timezone { get; set; }
        public LocID Location { get; set; }
        public OrganizationDetail organization { get; set; }
    }



    public class LocAddress
    {
        public string line_one { get; set; }
        public string line_two { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
    }

    public class LocTimezone
    {
        public string name { get; set; }
    }

    public class LocID
    {
        public string _id { get; set; }
    }


    public class OrganizationDetail
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
  
    }

    public class ApptLocation
    {
        public string name { get; set; }
        public string _id { get; set; }
    
    }

    public class ApptOrganization
    {
        public string _ref { get; set; }
        public string _id { get; set; }
    }

}
