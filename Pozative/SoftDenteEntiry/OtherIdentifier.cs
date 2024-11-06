using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/05/")]
    public class OtherIdentifier : BaseDataContract
    {
        private string _Identifier;
        private string _Source;
        private string _Description;

        public OtherIdentifier()
        {
            this.Identifier = string.Empty;
            this.Source = string.Empty;
            this.Description = string.Empty;
        }

        [DataMember]
        public string Identifier
        {
            get
            {
                return this._Identifier;
            }
            set
            {
                this._Identifier = value;
            }
        }

        [DataMember]
        public string Source
        {
            get
            {
                return this._Source;
            }
            set
            {
                this._Source = value;
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
            }
        }
    }
}
