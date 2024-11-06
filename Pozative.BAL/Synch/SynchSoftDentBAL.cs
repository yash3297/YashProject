using System;
using Pozative.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pozative.DAL.Synch;

namespace Pozative.BAL.Synch
{
    public class SynchSoftDentBAL
    {    

        public static bool Save_SoftDent_To_Local(DataTable dtSoftDentDataToSave, string tablename, string ignoreColumnsName, string primaryKeyColumnsName)
        {
            try
            {
                return SynchSoftDentDAL.Save_SoftDent_To_Local(dtSoftDentDataToSave, tablename, ignoreColumnsName, primaryKeyColumnsName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool DeleteByPatientID(string DeletedEHRIDs)
        {
            try
            {
                return SynchSoftDentDAL.DeleteByPatientID(DeletedEHRIDs);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
