using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_EasyDental_Question
    {
        public string appointmentlocation { get; set; }      
        public string organization { get; set; }
        public string location { get; set; }
        public string questiontype { get; set; }
        public string question { get; set; }
        public int row { get; set; }
        public string defaultvalue { get; set; }
        public string easydental_form_localdb_id { get; set; }
        public string easydental_questionid { get; set; }     
        public bool is_deleted { get; set; }
        public bool is_active { get; set; }
        public string easydentalformmaster { get; set; }
    }
    public class Push_EasyDental_Form
    {
        public string appointmentlocation { get; set; }
        public string organization { get; set; }
        public string location { get; set; }
        public string name { get; set; }
        public string easydental_formmasterid { get; set; }       
        public string easydental_formmaster_localdb_id { get; set; }     
        public bool is_deleted { get; set; }
        public bool is_active { get; set; }
    }
  
}
