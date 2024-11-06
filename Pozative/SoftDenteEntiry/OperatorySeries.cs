using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public class OperatorySeries
    {
        private List<Operatory> _Operatories = new List<Operatory>();

        [DataMember]
        public List<Operatory> Operatories
        {
            get
            {
                return this._Operatories;
            }
            set
            {
                this._Operatories = value;
            }
        }
    }
}
