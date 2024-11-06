using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pozative;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/Base/2010/05/")]
    public class Person : RecordBased
    {
        private string _NationalID;
        private List<OtherIdentifier> _OtherIdentifiers;
        private string _FirstName;
        private string _MiddleName;
        private string _LastName;
        private string _NickName;
        private string _Honorific;
        private string _Title;
        private string _CultureAndRegionCode;
        private DateTime? _Birthdate;
        private Sex _Sex;
        private MaritalStatus _MaritalStatus;
        private ContactInformation _ContactInformation;
        private Guid _globalID;
        private User _user;

        public Person()
        {
            this.OtherIdentifiers = new List<OtherIdentifier>();
            this.NationalID = string.Empty;
            this.FirstName = string.Empty;
            this.MiddleName = string.Empty;
            this.LastName = string.Empty;
            this.NickName = string.Empty;
            this.Honorific = string.Empty;
            this.Title = string.Empty;
            this.CultureAndRegionCode = string.Empty;
            this.Sex = Sex.Unknown;
            this.MaritalStatus = MaritalStatus.Unknown;
            this._ContactInformation = new ContactInformation();
            this.PMSRecordID = string.Empty;
        }

        [DataMember]
        public string NationalID
        {
            get
            {
                return this._NationalID;
            }
            set
            {
                this._NationalID = value;
            }
        }

        [DataMember]
        public List<OtherIdentifier> OtherIdentifiers
        {
            get
            {
                return this._OtherIdentifiers;
            }
            set
            {
                this._OtherIdentifiers = value;
            }
        }

        [DataMember]
        public virtual string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                this._FirstName = value;
            }
        }

        [DataMember]
        public virtual string MiddleName
        {
            get
            {
                return this._MiddleName;
            }
            set
            {
                this._MiddleName = value;
            }
        }

        [DataMember]
        public virtual string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                this._LastName = value;
            }
        }

        [DataMember]
        public virtual string NickName
        {
            get
            {
                return this._NickName;
            }
            set
            {
                this._NickName = value;
            }
        }

        [DataMember]
        public string Honorific
        {
            get
            {
                return this._Honorific;
            }
            set
            {
                this._Honorific = value;
            }
        }

        [DataMember]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
            }
        }

        [DataMember]
        public string CultureAndRegionCode
        {
            get
            {
                return this._CultureAndRegionCode;
            }
            set
            {
                this._CultureAndRegionCode = value;
            }
        }

        [DataMember]
        public virtual DateTime? Birthdate
        {
            get
            {
                return this._Birthdate;
            }
            set
            {
                this._Birthdate = value;
            }
        }

        [DataMember]
        public Sex Sex
        {
            get
            {
                return this._Sex;
            }
            set
            {
                this._Sex = value;
            }
        }

        [DataMember]
        public MaritalStatus MaritalStatus
        {
            get
            {
                return this._MaritalStatus;
            }
            set
            {
                this._MaritalStatus = value;
            }
        }

        [DataMember]
        public ContactInformation ContactInformation
        {
            get
            {
                return this._ContactInformation;
            }
            set
            {
                this._ContactInformation = value;
            }
        }

        [DataMember]
        public Guid GlobalID
        {
            get
            {
                return this._globalID;
            }
            set
            {
                this._globalID = value;
            }
        }

        [DataMember]
        public User User
        {
            get
            {
                return this._user;
            }
            set
            {
                this._user = value;
            }
        }

        [DataMember(IsRequired = false)]
        public string PMSRecordID { get; set; }
    }
}
