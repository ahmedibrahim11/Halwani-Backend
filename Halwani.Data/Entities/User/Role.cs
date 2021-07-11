using Halwani.Data.Entities.Team;
using System.Collections.Generic;

namespace Halwani.Data.Entities.User
{
    public class Role : Entity<long>
    {
        public Role()
        {
            //TeamPermissions = new HashSet<UserTeams>();
        }
        public string RoleName { get; set; }
        public string Permissions { get; set; }
        //public virtual ICollection<UserTeams> TeamPermissions { get; set; }
    }
}
