using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.User
{
    public class User : Entity<long>
    {
        public User()
        {
            UserTeams = new HashSet<UserTeams>();
        }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserStatusEnum UserStatus { get; set; }
        public long RoleId { get; set; }
        public bool SetTicketHigh { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<UserTeams> UserTeams { get; set; }
    }
}
