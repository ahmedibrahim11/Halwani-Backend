using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Incident
{
    public class Group
    {
        public Group()
        {
            RequestTypeGroups = new HashSet<RequestTypeGroups>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public virtual ICollection<RequestTypeGroups> RequestTypeGroups { get; set; }
    }
}
