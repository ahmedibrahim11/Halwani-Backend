using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.SLM_Measurement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.SLA
{
   public class SLA :Entity<long>
    {
        #region [SLA Properties]
        public string SLAName { get; set; }
        public SLAType SLAType { get; set; }
        public Priority Priority { get; set; }
        public string ServiceLine { get; set; }
        public string WorkingHours { get; set; }
        public string WorkingDays { get; set; }
        public double SLADuration { get; set; } 
        public string ProductCategoryName { get; set; }
        #endregion

        #region [SlM-Measurement]
        public virtual ICollection<SLmMeasurement> SLmMeasurements { get; set; } 
        #endregion

    }

}
