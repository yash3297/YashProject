using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class LocationTimeZoneResBO
    {
        public string message { get; set; }
        public string error { get; set; }
        public LocationTimeZoneData data { get; set; }
    }

    public class LocationTimeZoneData
    {
        public string name { get; set; }
        public string _id { get; set; }
        public string zone { get; set; }
        public string gmt { get; set; }
        public string alias { get; set; }
                
    }
}
