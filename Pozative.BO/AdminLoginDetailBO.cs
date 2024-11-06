using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class AdminLoginDetailBO
    {
        public string status { get; set; }
        public string Token { get; set; }
        public string message { get; set; }
        public string error { get; set; }

        public dataId data { get; set; }

    }
    public class dataId
    {
        public string _id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user_name { get; set; }
        public List<dataOrganization> organizations { get; set; }
        public string syncLocationId { get; set; }
    }
    public class dataOrganization
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
