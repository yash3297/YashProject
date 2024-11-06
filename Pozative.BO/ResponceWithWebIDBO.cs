using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   public class ResponceWithWebIDBO
    {
        public string message { get; set; }
        public AptMainData data { get; set; }

    }

   public class AptMainData
   {
       public string _id { get; set; }
   }
}
