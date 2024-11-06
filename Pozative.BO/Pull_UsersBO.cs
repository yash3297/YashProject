using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   public class Pull_UsersBO
    {
            public string message { get; set; }
            public int code { get; set; }
            public List<UsersList> data { get; set; }
            public bool status { get; set; }
        
    }

    public class UsersList
    {
        public string _id { get; set; }
        public string default_ehruser { get; set; }
    }
}
