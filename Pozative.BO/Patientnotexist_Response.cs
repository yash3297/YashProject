using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Patientnotexist_Response
    {
        public string message { get; set; }
        public Int32 total { get; set; }
        public int batchsize { get; set; }
    }
}
