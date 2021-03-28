using Halwani.Core.ViewModels.GenericModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.UserModels
{
    public class UserLookupViewModel : LookupViewModel
    {
        public string Email { get; set; }
        public string Team { get; set; }
        public string UserName { get; set; }
    }
}
