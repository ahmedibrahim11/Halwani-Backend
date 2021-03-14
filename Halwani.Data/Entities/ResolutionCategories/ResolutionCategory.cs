using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.ResolutionCategories
{
   public class ResolutionCategory:Entity<long>
    {
        public ResolutionCategory()
        {
            ResolutionCategories = new HashSet<ResolutionCategory>();
        }
        public string Name  { get; set; }
        public long? ParentCategoryId { get; set; }
        public virtual ResolutionCategory Parent { get; set; }
        public virtual ICollection<ResolutionCategory> ResolutionCategories { get; set; }
    }
}
