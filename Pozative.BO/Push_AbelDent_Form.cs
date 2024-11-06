using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_AbelDent_Form
    {
        public string appointmentlocation { get; set; }
        public string organization { get; set; }
        public string location { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public string version_date { get; set; }
        public string formrespondenttype { get; set; }
        public string categoryid { get; set; }
        public string formflags { get; set; }
        public string monthtoexpiration { get; set; }
        public string abeldent_form_localdb_id { get; set; }
        public string abeldent_form_ehrunique_id { get; set; }
        public string abeldent_form_ehr_id { get; set; }
        public bool is_deleted { get; set; }
        public bool is_active { get; set; }
    }
    public class Push_AbelDent_FormQuestion
    {
        public string appointmentlocation { get; set; }
        public string organization { get; set; }
        public string location { get; set; }
        public string questionstypeid { get; set; }
        public string questionstypename { get; set; }
        public string questionversion_date { get; set; }
        public string responsetypeid { get; set; }
        public string questionname { get; set; }
        public string question_defaultvalue { get; set; }
        public string questionversion { get; set; }
        public string inputtype { get; set; }
        public string options { get; set; }
        public string questionorder { get; set; }
        public string answer_value { get; set; }
        public string abeldent_formquestion_ehr_id { get; set; }
        public string abeldent_formquestion_localdb_id { get; set; }
        public string abeldent_form_ehrunique_id { get; set; }
        public string abeldent_formquestion_ehrunique_id { get; set; }
        public bool is_multifield { get; set; }
        public bool is_optionfiled { get; set; }
        public bool is_required { get; set; }
        public bool is_deleted { get; set; }
        public bool is_active { get; set; }
    }
}
