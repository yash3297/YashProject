using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/05/")]
    public class Appointment : RecordBased
    {
        private DateTime _DateAndTime;
        private Patient _Patient;
        private Provider _Provider;
        private Office _Office;
        private string _Notes;
        private string _ExamRoom;
        private string _Description;
        private bool _NewPatient;
        private bool _RequiresImage;
        private bool _IsCheckedIn;
        private bool _PreMedicateAlert;
        private bool _HygienistsAppointment;
        private string _Status;
        private int _SubStatus;
        private bool _IsSeated;
        private bool _IsCompleted;
        private bool _IsConfirmed;
        private bool _IsCheckedOut;
        private int _Duration;
        private List<Procedure> _Procedures;
        private string _patientName;
        private Int64 _patientId;
        private string _providerName;
        private Int64 _providerId;

        [DataMember(IsRequired = false)]
        public string PMSRecordID { get; set; }

        public Appointment()
        {
            this._NewPatient = false;
            this._RequiresImage = false;
            this._IsCheckedIn = false;
            this._Patient = new Patient();
            this._Provider = new Provider();
            this._Office = new Office();
            this.ExamRoom = string.Empty;
            this.Notes = string.Empty;
            this.Description = string.Empty;
            this.Duration = 0;
            this.Procedures = new List<Procedure>();
            this._PreMedicateAlert = false;
            this._HygienistsAppointment = false;
            this._Status = "";
            this._SubStatus = 0;
            this._IsSeated = false;
            this._IsCompleted = false;
            this._IsConfirmed = true;
            this._IsCheckedOut = false;
            this.Category = "";
            this._providerId = 0;
            this._providerName = string.Empty;
            this._patientId = 0;
            this._patientName = string.Empty;
        }

        [DataMember]
        public virtual bool NewPatient
        {
            get
            {
                return this._NewPatient;
            }
            set
            {
                this._NewPatient = value;
            }
        }

        [DataMember]
        public virtual bool RequiresImage
        {
            get
            {
                return this._RequiresImage;
            }
            set
            {
                this._RequiresImage = value;
            }
        }

        [DataMember]
        public virtual bool IsCheckedIn
        {
            get
            {
                return this._IsCheckedIn;
            }
            set
            {
                this._IsCheckedIn = value;
            }
        }

        [DataMember]
        public virtual string ExamRoom
        {
            get
            {
                return this._ExamRoom;
            }
            set
            {
                this._ExamRoom = value;
            }
        }

        [DataMember]
        public virtual string Notes
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

        [DataMember]
        public virtual string Description
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

        [DataMember]
        public virtual Provider Provider
        {
            get
            {
                return this._Provider;
            }
            set
            {
                this._Provider = value;
            }
        }

        [DataMember]
        public virtual Office Office
        {
            get
            {
                return this._Office;
            }
            set
            {
                this._Office = value;
            }
        }

        [DataMember]
        public virtual Patient Patient
        {
            get
            {
                return this._Patient;
            }
            set
            {
                this._Patient = value;
            }
        }

        [DataMember]
        public virtual DateTime DateAndTime
        {
            get
            {
                return this._DateAndTime;
            }
            set
            {
                this._DateAndTime = value;
            }
        }

        [DataMember]
        public virtual int Duration
        {
            get
            {
                return this._Duration;
            }
            set
            {
                this._Duration = value;
            }
        }

        [DataMember]
        public List<Procedure> Procedures
        {
            get
            {
                return this._Procedures;
            }
            set
            {
                this._Procedures = value;
            }
        }

        [DataMember]
        public virtual bool PreMedicateAlert
        {
            get
            {
                return this._PreMedicateAlert;
            }
            set
            {
                this._PreMedicateAlert = value;
            }
        }

        [DataMember]
        public virtual bool HygienistsAppointment
        {
            get
            {
                return this._HygienistsAppointment;
            }
            set
            {
                this._HygienistsAppointment = value;
            }
        }

        [DataMember]
        public virtual string Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                this._Status = value;
            }
        }

        [DataMember]
        public virtual int SubStatus
        {
            get
            {
                return this._SubStatus;
            }
            set
            {
                this._SubStatus = value;
            }
        }

        [DataMember]
        public virtual bool IsSeated
        {
            get
            {
                return this._IsSeated;
            }
            set
            {
                this._IsSeated = value;
            }
        }

        [DataMember]
        public virtual bool IsCompleted
        {
            get
            {
                return this._IsCompleted;
            }
            set
            {
                this._IsCompleted = value;
            }
        }

        [DataMember]
        public virtual bool IsConfirmed
        {
            get
            {
                return this._IsConfirmed;
            }
            set
            {
                this._IsConfirmed = value;
            }
        }

        [DataMember]
        public virtual bool IsCheckedOut
        {
            get
            {
                return this._IsCheckedOut;
            }
            set
            {
                this._IsCheckedOut = value;
            }
        }

        [DataMember]
        public virtual string Category { get; set; }

        [DataMember]
        public virtual string PatientName { get; set; }
        [DataMember]
        public virtual string ProviderName { get; set; }
        [DataMember]
        public virtual string PatientId { get; set; }
        [DataMember]
        public virtual string ProviderId { get; set; }
    }
}
