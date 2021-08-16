using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.RequestTypeModels
{
   public class UserRequestViewModel
    {
        public string Text { get; set;}
        public BugType? bugType { get; set; }
        public SupportTypes? supportTypes { get; set; }
        public UserFeedBack? userFeedBack { get; set; }
    }
}
