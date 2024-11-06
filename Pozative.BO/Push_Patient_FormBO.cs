using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_Patient_FormBO
    {
        public string[] id { get; set; }
        public string status { get; set; }
    }

    public class Statusupdate
    {
        public string esId { get; set; }
        public string patientId { get; set; }
        public string ehrsyncstatus { get; set; }
    }

    public class Push_PatientPortal_StatusUpdate
    {
        public List<Statusupdate> statusupdate { get; set; }
    }
}
