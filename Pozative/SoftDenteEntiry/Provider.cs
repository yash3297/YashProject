using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/Clinical/ProviderInfo/2010/05/")]
    public class Provider : Person
    {
        private string _Type;
        private Office _DefaultOffice;

        [DataMember]
        public string Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                this._Type = value;
            }
        }

        [DataMember]
        public byte TypeID { get; set; }

        [DataMember]
        public string LicenseNumber { get; set; }

        [DataMember]
        public bool IsConsulting { get; set; }

        [DataMember]
        public Guid EmployeeID { get; set; }

        [DataMember]
        public string TaxID { get; set; }

        [DataMember]
        public string NationalProviderIdentifier { get; set; }

        [DataMember]
        public string MedicaidID { get; set; }

        [DataMember]
        public string MedicaidGroupID { get; set; }

        [DataMember]
        public string MedicareID { get; set; }

        [DataMember]
        public string BlueCrossBlueShieldID { get; set; }

        [DataMember]
        public string AnesthesiaLicenseNumber { get; set; }

        [DataMember]
        public string Degree { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public Guid? SpecialtyID { get; set; }

        [DataMember]
        public string SsoUserIdentity { get; set; }

        [DataMember]
        public Guid SsoUpn { get; set; }

        [DataMember]
        public string UPIN { get; set; }

        [DataMember]
        public string PreferredAssistantID { get; set; }

        public Provider()
        {
            this._Type = string.Empty;
            this._DefaultOffice = new Office();
        }

        [DataMember(IsRequired = false)]
        public Office DefaultOffice
        {
            get
            {
                return this._DefaultOffice;
            }
            set
            {
                this._DefaultOffice = value;
            }
        }

        [DataMember]
        public string NHSPerformerNumber { get; set; }
    }
}
