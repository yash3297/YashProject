using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   public class AditLoginPostBO
    {
        public string email { get; set; }
        public string password { get; set; }
        public string created_by { get; set; }
        public bool self_installable { get; set; }
    }
   public class PatientDoc
   {
       public string locationId { get; set; }
       public string sid { get; set; }
      
   }

    public class TreatmentDoc
    {
        public string returnType { get; set; }

    }

    public class TreatmentDocStatusChange
    {
        public List< string> id { get; set; }
        public string status { get; set; }

    }

    public class InsuranceCarrierDocStatusChange
    {
        public List<string> _id { get; set; }      
        public string status { get; set; }
    }
    public class PatientMedicalHistoryBO
   {
       public string loc { get; set; }
       public string org { get; set; }
       public string id { get; set; }
   }
    public class AditLoginPostOTPBO
    {
        public string syncappcode { get; set; }

    }
}
