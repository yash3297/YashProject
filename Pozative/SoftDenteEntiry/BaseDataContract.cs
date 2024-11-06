using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{   
    public class BaseDataContract
    {
        //[DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/05/")]
        public BaseDataContract()
        {
            this.DataContractVersion = 1;
        }

        [DataMember]
        public int DataContractVersion { get; set; }
    }
}
