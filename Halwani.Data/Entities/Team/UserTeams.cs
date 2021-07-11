using Halwani.Data.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Halwani.Data.Entities.Team
{
    public class UserTeams : Entity<long>
    {
        public long UserId { get; set; }
        public long TeamId { get; set; }
        public virtual Team Team { get; set; }
        public virtual User.User User { get; set; }
    }
}
