using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.User
{
   public class User:Entity<long>
    {
        public string Name { get; set;}
        public string Email { get; set; }
        public long RoleId { get; set; }
        public virtual Role Role { get; set; }


    }
}
