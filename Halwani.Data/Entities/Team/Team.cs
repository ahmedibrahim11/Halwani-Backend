using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Team
{
    public class Team : Entity<long>
    {
        public Team()
        {
            //TeamUsers = new HashSet<TeamUsers>();
        }
        public string Name { get; set; }
        public virtual ICollection<User.User> Users { get; set; }
    }
}
