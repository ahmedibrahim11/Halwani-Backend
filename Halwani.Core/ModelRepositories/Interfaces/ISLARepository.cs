using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.SLAModels;
using Halwani.Core.ViewModels.TicketModels;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.SLA;
using Halwani.Data.Entities.SLM_Measurement;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface ISLARepository
    {
        List<SLmMeasurement> LoadTicketSlm(CreateTicketViewModel model);
        List<SLmMeasurement> LoadTicketSlmPerStatus(Ticket ticket, Status status, out List<SLA> closeSla);
        RepositoryOutput Add(SLAModel model);
        RepositoryOutput Edit(SLAModel model);
        SLAResultViewModel List(SLAPageInputViewModel model, ClaimsIdentity userClaims, out RepositoryOutput response);
        SLAModel GetForEdit(long ID);
        //List<SLmMeasurement> UpdateTicketSlm(CreateTicketViewModel model, long ticketId, SLAType slaType);
    }
}
