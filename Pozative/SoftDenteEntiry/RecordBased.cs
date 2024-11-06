using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/05/")]
    public class RecordBased : BaseDataContract
    {
        private string _RecordID;
        private DateTime _RecordCreated;
        private DateTime _RecordLastUpdated;
        private RecordStatus _RecordStatus;

        public RecordBased()
        {
            this.RecordID = string.Empty;
            this.RecordStatus = RecordStatus.Inactive;
            this.RecordCreated = new DateTime();
            this.RecordLastUpdated = new DateTime();
            this.RecordCreated = DateTime.Now;
            this.RecordLastUpdated = DateTime.Now;
        }

        [DataMember(IsRequired = true)]
        public virtual string RecordID
        {
            get
            {
                return this._RecordID;
            }
            set
            {
                this._RecordID = value;
            }
        }

        [DataMember]
        public DateTime RecordCreated
        {
            get
            {
                return this._RecordCreated;
            }
            set
            {
                this._RecordCreated = value;
            }
        }

        [DataMember]
        public virtual DateTime RecordLastUpdated
        {
            get
            {
                return this._RecordLastUpdated;
            }
            set
            {
                this._RecordLastUpdated = value;
            }
        }

        [DataMember]
        public RecordStatus RecordStatus
        {
            get
            {
                return this._RecordStatus;
            }
            set
            {
                this._RecordStatus = value;
            }
        }
    }
}
