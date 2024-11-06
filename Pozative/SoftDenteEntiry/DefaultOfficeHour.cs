using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/07/")]
    public class DefaultOfficeHour : RecordBased
    {
        private DateTime _OpenTime;
        private DateTime _CloseTime;
        private DateTime _LunchOpenTime;
        private DateTime _LunchCloseTime;

        public DefaultOfficeHour()
        {
            this.OfficeID = Guid.Empty;
            this._OpenTime = DateTime.Today.AddHours(9.0);
            this._CloseTime = DateTime.Today.AddHours(17.0);
            this._LunchOpenTime = DateTime.Today.AddHours(13.0);
            this._LunchCloseTime = DateTime.Today.AddHours(14.0);
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
        public DateTime LunchOpenTime
        {
            get
            {
                return this._LunchOpenTime;
            }
            set
            {
                this._LunchOpenTime = value;
            }
        }

        [DataMember]
        public DateTime LunchCloseTime
        {
            get
            {
                return this._LunchCloseTime;
            }
            set
            {
                this._LunchCloseTime = value;
            }
        }

        [DataMember]
        public Guid OfficeID { get; set; }

        [DataMember]
        public DayOfWeek Weekday { get; set; }

        [DataMember(IsRequired = false)]
        public string PMSRecordID { get; set; }
    }
}
