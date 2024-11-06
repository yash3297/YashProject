using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/05/")]
    public class Address : RecordBased
    {
        private AddressType _AddressType;
        private string _OtherAddressType;
        private List<string> _StreetAddressLines;
        private string _AdministrativeArea;
        private string _Locale;
        private string _PostalCode;
        private string _Country;
        private List<string> _FormattedAddress;

        public Address()
        {
            this.AddressType = AddressType.Unknown;
            this.StreetAddressLines = new List<string>();
            this.FormattedAddress = new List<string>();
            this.OtherAddressType = string.Empty;
            this.AdministrativeArea = string.Empty;
            this.Locale = string.Empty;
            this.PostalCode = string.Empty;
            this.Country = string.Empty;
        }

        [DataMember]
        public AddressType AddressType
        {
            get
            {
                return this._AddressType;
            }
            set
            {
                this._AddressType = value;
            }
        }

        public string OtherAddressType
        {
            get
            {
                return this._OtherAddressType;
            }
            set
            {
                this._OtherAddressType = value;
            }
        }

        [DataMember]
        public List<string> StreetAddressLines
        {
            get
            {
                return this._StreetAddressLines;
            }
            set
            {
                this._StreetAddressLines = value;
            }
        }

        [DataMember]
        public string AdministrativeArea
        {
            get
            {
                return this._AdministrativeArea;
            }
            set
            {
                this._AdministrativeArea = value;
            }
        }

        [DataMember]
        public string Locale
        {
            get
            {
                return this._Locale;
            }
            set
            {
                this._Locale = value;
            }
        }

        [DataMember]
        public string PostalCode
        {
            get
            {
                return this._PostalCode;
            }
            set
            {
                this._PostalCode = value;
            }
        }

        [DataMember]
        public string Country
        {
            get
            {
                return this._Country;
            }
            set
            {
                this._Country = value;
            }
        }

        public List<string> FormattedAddress
        {
            get
            {
                return this._FormattedAddress;
            }
            set
            {
                this._FormattedAddress = value;
            }
        }
    }
}
