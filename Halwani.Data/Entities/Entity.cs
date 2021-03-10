using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities
{
   public abstract class Entity<Tkey>
    {
        public Tkey Id  { get; set; }
    }
}
