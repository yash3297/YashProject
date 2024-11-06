using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class EHRListWithWebIdBO
    {
        public string message { get; set; }
        public string error { get; set; }
        public List<MainDataEHRWebId> data { get; set; }
    }

    public class MainDataEHRWebId
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string version { get; set; }
   }

}
