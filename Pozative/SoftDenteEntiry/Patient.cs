using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/06/")]
    public class Patient : Person
    {
        private string _ChartNumber;
        private Provider _Doctor;
        private Provider _Assistant;
        private Person _EmergencyContactInfo;
        private string _Ethnicity;
       // private PWImage _CurrentPortrait;
        //private ClinicalInformation _ClinicalInformation;
        private Office _Location;
        private DateTime _FirstVisit;
        private DateTime _LastVisit;
        private DateTime _NextAppointment;
        private string _ImagesDirectory;
        private byte[] _CurrentPortraitImage;
        private double _Balance;
        private string _Notes;
      //  private List<MedicalAlert> _MedicalAlerts;
      //  private List<ResponsibleParty> _ResponsibleParties;
        private bool _hasInsurance;
        private List<string> _ExemptionEvidence;

        public Patient()
        {
            this.ChartNumber = string.Empty;
            this.Ethnicity = string.Empty;
            this.ImagesDirectory = string.Empty;
            this._Doctor = new Provider();
            this._Assistant = new Provider();
            this._EmergencyContactInfo = new Person();
          //  this._CurrentPortrait = new PWImage();
          //  this._ClinicalInformation = new ClinicalInformation();
            this._Location = new Office();
          //  this._MedicalAlerts = new List<MedicalAlert>();
          //  this._ResponsibleParties = new List<ResponsibleParty>();
            this._ExemptionEvidence = new List<string>();
        }

        [DataMember]
        public string ChartNumber
        {
            get
            {
                return this._ChartNumber;
            }
            set
            {
                this._ChartNumber = value;
            }
        }

        [DataMember]
        public Provider Doctor
        {
            get
            {
                return this._Doctor;
            }
            set
            {
                this._Doctor = value;
            }
        }

        [DataMember]
        public Provider Assistant
        {
            get
            {
                return this._Assistant;
            }
            set
            {
                this._Assistant = value;
            }
        }

        [DataMember]
        public Person EmergencyContact
        {
            get
            {
                return this._EmergencyContactInfo;
            }
            set
            {
                this._EmergencyContactInfo = value;
            }
        }

        [DataMember]
        public string Ethnicity
        {
            get
            {
                return this._Ethnicity;
            }
            set
            {
                this._Ethnicity = value;
            }
        }

        //[DataMember]
        //public virtual PWImage CurrentPortrait
        //{
        //    get
        //    {
        //        return this._CurrentPortrait;
        //    }
        //    set
        //    {
        //        this._CurrentPortrait = value;
        //    }
        //}

        //[DataMember]
        //public ClinicalInformation ClinicalInformation
        //{
        //    get
        //    {
        //        return this._ClinicalInformation;
        //    }
        //    set
        //    {
        //        this._ClinicalInformation = value;
        //    }
        //}

        [DataMember]
        public Office Location
        {
            get
            {
                return this._Location;
            }
            set
            {
                this._Location = value;
            }
        }

        [DataMember]
        public DateTime FirstVisit
        {
            get
            {
                return this._FirstVisit;
            }
            set
            {
                this._FirstVisit = value;
            }
        }

        [DataMember]
        public DateTime LastVisit
        {
            get
            {
                return this._LastVisit;
            }
            set
            {
                this._LastVisit = value;
            }
        }

        [DataMember]
        public DateTime NextAppointment
        {
            get
            {
                return this._NextAppointment;
            }
            set
            {
                this._NextAppointment = value;
            }
        }

        [DataMember]
        public string ImagesDirectory
        {
            get
            {
                return this._ImagesDirectory;
            }
            set
            {
                this._ImagesDirectory = value;
            }
        }

        [DataMember]
        public byte[] CurrentPortraitImage
        {
            get
            {
                return this._CurrentPortraitImage;
            }
            set
            {
                this._CurrentPortraitImage = value;
            }
        }

        [DataMember]
        public double Balance
        {
            get
            {
                return this._Balance;
            }
            set
            {
                this._Balance = value;
            }
        }

        [DataMember]
        public string Notes
        {
            get
            {
                return this._Notes;
            }
            set
            {
                this._Notes = value;
            }
        }

        //[DataMember]
        //public List<MedicalAlert> MedicalAlerts
        //{
        //    get
        //    {
        //        return this._MedicalAlerts;
        //    }
        //    set
        //    {
        //        this._MedicalAlerts = value;
        //    }
        //}

        //[DataMember]
        //public List<ResponsibleParty> ResponsibleParties
        //{
        //    get
        //    {
        //        return this._ResponsibleParties;
        //    }
        //    set
        //    {
        //        this._ResponsibleParties = value;
        //    }
        //}

        [DataMember]
        public bool HasInsurance
        {
            get
            {
                return this._hasInsurance;
            }
            set
            {
                this._hasInsurance = value;
            }
        }

        [DataMember]
        public string ETDNumber { get; set; }

        [DataMember]
        public string EthnicGroup { get; set; }

        [DataMember]
        public string ExemptionType { get; set; }

        [DataMember]
        public bool ExemptionSeen { get; set; }

        [DataMember]
        public List<string> ExemptionEvidence { get; set; }

        [DataMember]
        public DateTime DateOfAcceptance { get; set; }

        [DataMember]
        public DateTime DateOfCompletion { get; set; }
    }
}
