using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public class PatientMinSeries
    {
        private List<PatientMin> _Patients = new List<PatientMin>();

        [DataMember]
        public List<PatientMin> PatientMins
        {
            get
            {
                return this._Patients;
            }
            set
            {
                this._Patients = value;
            }
        }
    }
}
