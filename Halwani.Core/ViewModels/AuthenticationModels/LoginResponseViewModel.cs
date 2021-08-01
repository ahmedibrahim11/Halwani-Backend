using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.Authentication
{
    public class LoginResponseViewModel
    {
        public string Token { get; set; }
        public bool SetTicketHigh { get; set; }
        public DateTime Expiration { get; set; }
        public RepositoryOutput Result { set; get; }
        public UserProfileModel UserProfile { set; get; }
        public List<string> Permissions { get; set; }
    }
    public class UserProfileModel
    {
        public long Id { get; set; }
        public string UserEmail { get; set; }
        public RoleEnum RoleEnum { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
    }
}
