using Halwani.Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.AuthenticationModels
{
    public class UserSessionDataViewModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public RoleEnum Role { get; set; }
        public List<long> TeamsIds { get; set; }
        public List<string> Permissions { get; set; }
        public bool? IsAllTeams { get; set; }
    }
}
