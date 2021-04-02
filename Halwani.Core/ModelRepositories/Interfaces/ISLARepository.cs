using Halwani.Core.ViewModels.TicketModels;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.SLA;
using Halwani.Data.Entities.SLM_Measurement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface ISLARepository
    {
        List<SLmMeasurement> LoadTicketSlm(CreateTicketViewModel model, SLAType slaType);
        //List<SLmMeasurement> UpdateTicketSlm(CreateTicketViewModel model, long ticketId, SLAType slaType);
    }
}
