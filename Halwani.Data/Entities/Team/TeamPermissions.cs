using Halwani.Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Team
{
    public class TeamPermissions : Entity<long>
    {
        public long TeamId { get; set; }
        public long RoleId { get; set; }
        public bool IsAllTeams { get; set; }
        public string AllowedTeams { get; set; }
        public virtual Team Team { get; set; }
        public virtual Role Role { get; set; }
    }
}
