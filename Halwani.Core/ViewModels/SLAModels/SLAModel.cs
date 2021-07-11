using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.SLA;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ViewModels.SLAModels
{
    public class SLAModel
    {
        public long Id { get; set; }
        public SLAType SLAType { get; set; }
        public Priority Priority { get; set; }
        public string TeamName { get; set; }
        public string WorkingHours { get; set; }
        public string WorkingDays { get; set; }
        public double SLADuration { get; set; }
        public string ProductCategoryName { get; set; }
    }
}
