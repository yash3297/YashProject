using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   public class Push_SectionItemOptionMasterBO
    {
       public string  appointmentlocation {get;set;}
       public string  name {get;set;}
       public string  organization {get;set;}
       public string  question_type {get;set;}
       public string  answer_type {get;set;}
       public string  numberofcolumns {get;set;}
       public string  formmaster_ehr_id {get;set;}
       public string  sectionmaster_ehr_id {get;set;}
       public string  sectionitemmaster_ehr_id {get;set;}
       public string  sectionitemoptionmaster_ehr_id {get;set;}
       public string  sectionitemoptionmaster_localdb_id {get;set;}
       public string  location {get;set;}
       public bool  is_deleted {get;set;}
    }
}
