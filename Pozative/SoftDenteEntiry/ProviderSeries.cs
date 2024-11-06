using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public class ProviderSeries
    {
        private List<Provider> _Providers = new List<Provider>();

        [DataMember]
        public List<Provider> Providers
        {
            get
            {
                return this._Providers;
            }
            set
            {
                this._Providers = value;
            }
        }
    }
}
