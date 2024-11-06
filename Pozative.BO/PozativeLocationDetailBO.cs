using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   public class PozativeLocationDetailBO
    {
        public string message { get; set; }
        public string success { get; set; }
        public string error { get; set; }
        public List<PozativeData> data { get; set; }
    }

   public class PozativeData
   {
       public string id { get; set; }
       public string locationname { get; set; }
       public string machineid { get; set; }
    }
}
