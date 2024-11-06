using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public class DataMemberAttribute : Attribute
    {
        public bool IsRequired { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}
