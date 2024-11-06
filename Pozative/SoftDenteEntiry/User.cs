using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    [DataContract(Name = "User", Namespace = "urn:PW.Security.CRS")]
    public class User : RecordBased
    {
        [DataMember(IsRequired = true, Name = "UserName", Order = 1)]
        public string UserName { get; set; }

        [DataMember(IsRequired = true, Name = "Password", Order = 2)]
        public string Password { get; set; }

        public User()
        {
        }

        public User(Guid recordID, string userName, string password, Guid personID, Guid roleID)
        {
            this.UserName = userName;
            this.Password = password;
        }

        public User(string userName, string password, Guid personID, Guid roleID)
        {
            this.UserName = userName;
            this.Password = password;
        }
    }
}
