using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.User
{
    public class Role:Entity<long>
    {
        public string RoleName { get; set; }
    }
}
