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
        public string Priority { get; set; }
        public string Team { get; set; }
        public double WorkingHours { get; set; }
        public string WorkingDays { get; set; }
        public string SLADuration { get; set; } 
        #endregion

        #region [SlM-Measurement]
        public virtual ICollection<SLmMeasurement> SLmMeasurements { get; set; } 
        #endregion

    }

}
