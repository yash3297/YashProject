using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_UsersLocal_To_LiveBO
    {
        public string message { get; set; }
        public int code { get; set; }
        public List<GetListUsers> data { get; set; }
        public bool status { get; set; }
    }

    public class GetListUsers
    {
        //public bool status { get; set; }
        //public string ehr_user_id { get; set; }
        //public string _id { get; set; }
        public string _id { get; set; }
        public string _type { get; set; }
        public Appointmentlocation appointmentlocation { get; set; }
        public object created_at { get; set; }
        public string firstname { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public string lastname { get; set; }
        public Location location { get; set; }
        public Organization organization { get; set; }
        public string password { get; set; }
        public object updated_at { get; set; }
        public string user_ehr_id { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

   
    public class Appointmentlocation
    {
        //public string _ref { get; set; }
        public string _type { get; set; }
    }



    public class Location
    {
        //public string _ref { get; set; }
        public string _type { get; set; }
    }

    public class Organization
    {
        //public string _ref { get; set; }
        public string _type { get; set; }
    }



}
