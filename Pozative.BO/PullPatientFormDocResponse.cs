using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class PullPatientFormDocResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string error { get; set; }
        public PullPatientFormDocList data { get; set; }
    }
    public class PullPatientFormDocList
    {
        public string submissionId { get; set; }
        public PullPatientFormDoc[] fileList { get; set; }
    }
    public class PullPatientFormDoc
    {
        public string fileExtension { get; set; }
        public string fileName { get; set; }
        public string name { get; set; }
        public string fieldName { get; set; }
    }
}
