using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Team
{
    public class Team : Entity<long>
    {
        public Team()
        {
            UserTeams = new HashSet<UserTeams>();
        }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ServiceLine { get; set; }
        public DateTime ModfiedDate { get; set; }
        public virtual ICollection<UserTeams> UserTeams{ get; set; }
    }
}
