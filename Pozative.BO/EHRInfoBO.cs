using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class EHRInfoBO
    {
        public string _id { get; set; }
        public List<ehrinfo> data { get; set; }
    }

    public class ehrinfo
    {
        public string ehr_pass { get; set; }
        public string ehr_user { get; set; }
        public string ehr_result { get; set; }
        public string server_pass { get; set; }
        public string server_user { get; set; }
        public bool server_result { get; set; }
    }

    public class ehrinfoupdate
    {
        public string locationId { get; set; }
        public bool is_valid { get; set; }
    }

    public class ZohoInstallInfoBO
    {
        public string message { get; set; }
        public bool status { get; set; }
        public List<ZohoInstallInfo> data { get; set; }
    }

    public class ZohoInstallInfo
    {
        public string ehr_pass { get; set; }
        public string ehr_user { get; set; }
        public bool is_confirmed { get; set; }
        public bool is_installed { get; set; }
        public object is_valid { get; set; }
        public string server_pass { get; set; }
        public string server_user { get; set; }
        public string locationId { get; set; }
        public string location_name { get; set; }
        public string organizationId { get; set; }
        public string organization_name { get; set; }
        public string userId { get; set; }
        public string user_name { get; set; }
    }

    public class ZohoInstallUpdate
    {
        public string locationId { get; set; }
        public bool is_installed { get; set; }

        public string message { get; set; }
    }

    public class SaveServerUsers
    {
        public string user_name { get; set; }
        public string password { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public string appointmentlocation { get; set; }
        public string location { get; set; }
        public string organization { get; set; }

    }


    public class SaveServerUsersMain
    {
        public string message { get; set; }

        public List<SaveServerUsersId> data { get; set; }
    }
    public class SaveServerUsersId
    {
        public string _id { get; set; }
        public string user_name { get; set; }
        
    }
}
