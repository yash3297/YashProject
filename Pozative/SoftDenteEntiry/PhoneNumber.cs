using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/05/")]
    public class PhoneNumber : RecordBased
    {
        private string _CountryCode;
        private string _Number;
        private string _Extension;
        private string _Description;

        public PhoneNumber()
        {
            this.CountryCode = string.Empty;
            this.Number = string.Empty;
            this.Extension = string.Empty;
            this.Description = string.Empty;
        }

        [DataMember]
        public string CountryCode
        {
            get
            {
                return this._CountryCode;
            }
            set
            {
                this._CountryCode = value;
            }
        }

        [DataMember]
        public string Number
        {
            get
            {
                return this._Number;
            }
            set
            {
                this._Number = value;
            }
        }

        [DataMember]
        public string Extension
        {
            get
            {
                return this._Extension;
            }
            set
            {
                this._Extension = value;
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
