using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.Authentication
{
    public class LoginResponseViewModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public RepositoryOutput Result { set; get; }
        public List<string> Permissions { get; set; }
    }
}
