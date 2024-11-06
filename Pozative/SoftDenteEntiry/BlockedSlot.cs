using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public class BlockedSlot
    {
        public string DataContractVersion { get; set; }
        public string RecordID { get; set; }
        public string RecordStatus { get; set; }
        public string RecordCreated { get; set; }
        public string RecordLastUpdated { get; set; }
        public string DateAndTime { get; set; }
        public string ExamRoom { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
    }
}
