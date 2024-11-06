using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Namespace = "http://datacontracts.practiceworks.com/2010/05/")]
    public class Procedure : RecordBased
    {
        private DateTime _DateOfService;
        private string _ProcedureCode;
        private string _Description;

        public Procedure()
        {
            this.ProcedureCode = string.Empty;
            this.Description = string.Empty;
        }

        [DataMember]
        public virtual DateTime DateOfService
        {
            get
            {
                return this._DateOfService;
            }
            set
            {
                this._DateOfService = value;
            }
        }

        [DataMember]
        public string ProcedureCode
        {
            get
            {
                return this._ProcedureCode;
            }
            set
            {
                this._ProcedureCode = value;
            }
        }

        [DataMember]
        public string Description
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
    }
}
