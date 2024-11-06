using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_OD_SheetDef
    {
        public string appointmentlocation {get;set;}
        public string name  {get;set;}
        public string organization  {get;set;}
        public string fontsize  {get;set;}
        public string fontname  {get;set;}
        public string width  {get;set;}
        public string height  {get;set;}
        public bool islandscape  {get;set;}
        public bool pagecount  {get;set;}
        public bool ismultipage  {get;set;}
        public string sheetdefnum_ehr_id  {get;set;}
        public string sheetdefnum_localdb_id  {get;set;}
        public string sheetdefnum_web_id { get; set; }
        public string location  {get;set;}
        public bool is_deleted { get; set; }
    }
}
