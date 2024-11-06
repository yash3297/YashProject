using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_PatientFollowUp
    {
        public string locationId { get; set; }
        public string organizationId { get; set; }
        public List<Push_PatientFollowUp_NoteId> note_ids { get; set; }
    }
    public class Push_PatientFollowUp_NoteId
    {
        public string id { get; set; }
        public string status { get; set; }
        public string error { get; set; }
    }
}
