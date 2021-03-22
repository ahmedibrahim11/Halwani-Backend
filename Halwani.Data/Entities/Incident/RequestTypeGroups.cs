using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Incident
{
    public class RequestTypeGroups
    {
        public int Id { get; set; }
        public int RequestTypeId { get; set; }
        public int GroupId { get; set; }
        public virtual RequestType RequestType { get; set; }
        public virtual Group Group { get; set; }
    }
}
