using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_FolderListBO
    {
        public string folder_name { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public string description { get; set; }
        public string ehrfolder_ehr_id { get; set; }
        public string appointmentlocation { get; set; }
        public string location { get; set; }
        public string organization { get; set; }
        public int order_id { get; set; }
        public string _id { get; set; }
    }
}
