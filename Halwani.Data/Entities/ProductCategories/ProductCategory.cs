using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.ProductCategories
{
    public class ProductCategory : Entity<long>
    {
        public ProductCategory()
        {
            ProductCategories = new HashSet<ProductCategory>();
        }
        public string Name { get; set; }
        public long? ParentCategoryId { get; set; }
        public double? Goal { get; set; }
        public virtual ProductCategory Parent { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
