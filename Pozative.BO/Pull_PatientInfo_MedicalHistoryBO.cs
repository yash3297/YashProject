using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_PatientInfo_MedicalHistoryBO
    {
        public data data { get; set; }
    }

    public class data
    {
        public string _id{get;set;}
        public string patientWebId {get;set;}
        public List<Question> Question { get; set; }

    }
    public class Question
    {
        public string _id { get; set; }
        public string _type { get; set; }
        public string alertmaster_ehr_id { get; set; }
        public bool alertonno { get; set; }
        public bool alertonyes { get; set; }
        public bool allowcomment { get; set; }
        public bool alloweditcomment { get; set; }
        public string answer_type { get; set; }
        //public string created_at { get; set; }
        public int deleted_at { get; set; }
        public string formmaster_ehr_id { get; set; }
        public string sectionitemmaster_ehr_id { get; set; }
        public string sectionmaster_ehr_id { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public string name { get; set; }
        public string numberofcolumns { get; set; }
        public string question_type { get; set; }
        public int sectionitemtype { get; set; }
        public int sectionitemorder { get; set; }
        public string AnsValue { get; set; }
        public object AnsKey { get; set; }
        public List<sectionItemOptionMaster> sectionItemOptionMaster { get; set; }
        public string easydental_questionid { get; set; }
        public string questiontype { get; set; }
        public string row { get; set; }

        #region DentrixResponseDataField
        public string dentrix_form_ehrunique_id { get; set; }
        public string dentrix_formquestion_ehr_id { get; set; }
        public string dentrix_formquestion_ehrunique_id { get; set; }
        public string dentrix_formquestion_localdb_id { get; set; }
        public dynamic answer_value { get; set; }
        public string questionstypeid { get; set; }
        public string responsetypeid { get; set; }
        public string inputtype { get; set; }

        #endregion

        #region AbeldentresponseDataField
        public string abeldent_form_ehrunique_id { get; set; }
        public string abeldent_formquestion_ehr_id { get; set; }
        public string abeldent_formquestion_ehrunique_id { get; set; }
        public string abeldent_formquestion_localdb_id { get; set; }
        //public dynamic answer_value { get; set; }
        //public string questionstypeid { get; set; }
        //public string responsetypeid { get; set; }
        //public string inputtype { get; set; }
        #endregion

        #region CleardentResponseDataField

        public string cd_formmaster_ehr_id { get; set; }
        public string cd_questionmaster_ehr_id { get; set; }
        public string cd_questionmaster_localdb_id { get; set; }
        public string comment { get; set; }
        public string cd_form_name { get; set; }
        public string question_description { get; set; }
        public int question_sequence { get; set; }
        public bool question_warnings { get; set; }

        #endregion

        #region OpenDentalResponseDataField

        public string sheetfielddefnum_ehr_id { get; set; }
        public string sheetfielddefnum_localdb_id { get; set; }
        public string sheetdefnum_ehr_id { get; set; }
        public string fieldtype { get; set; }
        public string fieldvalue { get; set; }
        public string fontisbold { get; set; }
        public string fontname { get; set; }
        public string fontsize { get; set; }
        public string xpos { get; set; }
        public string ypos { get; set; }
        public string width { get; set; }
        public string growthbehavior { get; set; }
        public string height { get; set; }
        public string isrequired { get; set; }
        public string itemcolor { get; set; }
        public string radiobuttongroup { get; set; }
        public string radiobuttonvalue { get; set; }
        public string reportablename { get; set; }
        public string taborder { get; set; }
        public bool textalign { get; set; }
        public string sheettype { get; set; }
        public string fieldname { get; set; }
        public bool islocked { get; set; }
        public int tabordermobile { get; set; }
        public string uilabelmobile { get; set; }
        public string uilabelmobileradiobutton { get; set; }

        #endregion

    }
    public class sectionItemOptionMaster
    {
        public string AnsKey { get; set; }
        public string _id { get; set;  }
        public string answer_type { get; set; }
        public int deleted_at { get; set; }
        public string formmaster_ehr_id { get; set; }
        public string name { get; set; }
        public string numberofcolumns { get; set; }
        public string question_type {get;set;}
        public string sectionitemmaster_ehr_id { get; set; }
        public string sectionitemoptionmaster_ehr_id { get; set; }
        public string sectionitemoptionmaster_localdb_id { get; set; }
        public string sectionmaster_ehr_id { get; set; }

    }

}
