using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/05/")]
    public class ContactInformation : RecordBased
    {
        private Address _HomeAddress;
        private Address _WorkAddress;
        private List<Address> _OtherAddresses;
        private PhoneNumber _HomePhone;
        private PhoneNumber _WorkPhone;
        private PhoneNumber _Mobile;
        private PhoneNumber _Fax;
        private PhoneNumber _OtherPhone;
        private string _EMailAddress1;
        private string _EMailAddress2;
        private string _EMailAddress3;

        public ContactInformation()
        {
            this.OtherAddresses = new List<Address>();
            this._HomeAddress = new Address();
            this._WorkAddress = new Address();
            this._HomePhone = new PhoneNumber();
            this._WorkPhone = new PhoneNumber();
            this._OtherPhone = new PhoneNumber();
            this._Mobile = new PhoneNumber();
            this._Fax = new PhoneNumber();
            this._EMailAddress1 = string.Empty;
            this._EMailAddress2 = string.Empty;
            this._EMailAddress3 = string.Empty;
        }

        [DataMember]
        public Address HomeAddress
        {
            get
            {
                return this._HomeAddress;
            }
            set
            {
                this._HomeAddress = value;
            }
        }

        [DataMember]
        public Address WorkAddress
        {
            get
            {
                return this._WorkAddress;
            }
            set
            {
                this._WorkAddress = value;
            }
        }

        [DataMember]
        public List<Address> OtherAddresses
        {
            get
            {
                return this._OtherAddresses;
            }
            set
            {
                this._OtherAddresses = value;
            }
        }

        [DataMember]
        public PhoneNumber HomePhone
        {
            get
            {
                return this._HomePhone;
            }
            set
            {
                this._HomePhone = value;
            }
        }

        [DataMember]
        public PhoneNumber WorkPhone
        {
            get
            {
                return this._WorkPhone;
            }
            set
            {
                this._WorkPhone = value;
            }
        }

        [DataMember]
        public PhoneNumber Mobile
        {
            get
            {
                return this._Mobile;
            }
            set
            {
                this._Mobile = value;
            }
        }

        [DataMember]
        public PhoneNumber Fax
        {
            get
            {
                return this._Fax;
            }
            set
            {
                this._Fax = value;
            }
        }

        [DataMember]
        public PhoneNumber OtherPhone
        {
            get
            {
                return this._OtherPhone;
            }
            set
            {
                this._OtherPhone = value;
            }
        }

        [DataMember]
        public string EMailAddress1
        {
            get
            {
                return this._EMailAddress1;
            }
            set
            {
                this._EMailAddress1 = value;
            }
        }

        [DataMember]
        public string EMailAddress2
        {
            get
            {
                return this._EMailAddress2;
            }
            set
            {
                this._EMailAddress2 = value;
            }
        }

        [DataMember]
        public string EMailAddress3
        {
            get
            {
                return this._EMailAddress3;
            }
            set
            {
                this._EMailAddress3 = value;
            }
        }
    }
}
