using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_CustomHoursBO
    {
        public string message { get; set; }
        public string status { get; set; }
        public string error { get; set; }
        public GetListCustomHours data { get; set; }
    }

    public class GetListCustomHours
    {
        public string _id { get; set; }
        public string is_online_scheduled { get; set; }
        public string[] operatory { get; set; }
        public string[] provider { get; set; }
    }
}
