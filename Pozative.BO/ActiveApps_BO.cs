using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class ActiveApps_BO
    {
        public bool status { get; set; }
        public string message { get; set; }

        public List<GetListApps> data { get; set; }
    }

    public class GetListApps
    {
        public string alias { get; set; }
    }
}
