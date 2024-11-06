using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_FolderListBO
    {
        public string message { get; set; }
        public List<GetListFolderList> data { get; set; }
    }

    public class GetListFolderList
    {
        public string _id { get; set; }
        public string ehrfolder_ehr_id { get; set; }
    }
}
