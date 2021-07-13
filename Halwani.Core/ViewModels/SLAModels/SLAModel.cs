using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.SLA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.SLAModels
{
    public class SLAModel
    {
        public int Id { get; set; }
        public SLAType SLAType { get; set; }
        public Priority Priority { get; set; }
        //public string ServiceLine { get; set; }
        public string WorkingHours { get; set; }
        public string RequestType { get; set; }
        public string OpenStatus { get; set; }
        public string CloseStatus { get; set; }
        //public string WorkingDays { get; set; }
        public double SLADuration { get; set; } 
    }
}
