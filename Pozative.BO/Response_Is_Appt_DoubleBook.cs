using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Response_Is_Appt_DoubleBook
    {

        public string message { get; set; }
        public List<GetListIs_Appt_DoubleBook> data { get; set; }

    }

    public class GetListIs_Appt_DoubleBook
   {
       public string _id { get; set; }
     

   }
}
