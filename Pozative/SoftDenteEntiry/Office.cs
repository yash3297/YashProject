using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/05/")]
    public class Office : RecordBased
    {
        private Office.Mode _OfficeMode;
        private byte _SlotLength;
        private string _Name;
        private string _ShortName;
        private ContactInformation _ContactInformation;
        private DateTime _OpenTime;
        private DateTime _CloseTime;
        private List<DefaultOfficeHour> _DefaultOfficeHours;
        private string _RegionName;

        [DataMember]
        public Office.Mode OfficeMode
        {
            get
            {
                return this._OfficeMode;
            }
            set
            {
                this._OfficeMode = value;
            }
        }

        [DataMember]
        public byte SlotLength
        {
            get
            {
                return this._SlotLength;
            }
            set
            {
                this._SlotLength = value;
            }
        }

        public Office()
        {
            this.Name = string.Empty;
            this.ShortName = string.Empty;
            this._ContactInformation = new ContactInformation();
            this._DefaultOfficeHours = new List<DefaultOfficeHour>();
            this._SlotLength = (byte)5;
            this._OfficeMode = Office.Mode.Mode1;
        }

        [DataMember]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }

        [DataMember]
        public string ShortName
        {
            get
            {
                return this._ShortName;
            }
            set
            {
                this._ShortName = value;
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
        public DateTime OpenTime
        {
            get
            {
                return this._OpenTime;
            }
            set
            {
                this._OpenTime = value;
            }
        }

        [DataMember]
        public DateTime CloseTime
        {
            get
            {
                return this._CloseTime;
            }
            set
            {
                this._CloseTime = value;
            }
        }

        [DataMember]
        public List<DefaultOfficeHour> DefaultOfficeHours
        {
            get
            {
                return this._DefaultOfficeHours;
            }
            set
            {
                this._DefaultOfficeHours = value;
            }
        }

        [DataMember]
        public string RegionName
        {
            get
            {
                return this._RegionName;
            }
            set
            {
                this._RegionName = value;
            }
        }

        [DataMember(IsRequired = false)]
        public string PMSRecordID { get; set; }

        [DataMember]
        public string LocationNumber { get; set; }

        public enum Mode
        {
            Mode1,
            Mode2,
        }
    }
}
