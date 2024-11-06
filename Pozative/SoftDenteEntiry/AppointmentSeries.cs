using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/05/")]
    public class AppointmentSeries : BaseDataContract
    {
        private List<Appointment> _Appointments;

        public AppointmentSeries()
        {
            this._Appointments = new List<Appointment>();
        }

        [DataMember]
        public List<Appointment> Appointments
        {
            get
            {
                return this._Appointments;
            }
            set
            {
                this._Appointments = value;
            }
        }
    }
}
