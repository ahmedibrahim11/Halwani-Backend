using Halawani.Core;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.TicketModels;
using Halwani.Data.Entities.Incident;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface ITicketRepository : IBaseRepository<Ticket>
    {
        #region Custom Methouds
        TicketPageResultViewModel List(TicketPageInputViewModel model, ClaimsIdentity userClaims, out RepositoryOutput response);
        Task<List<string>> PostFilesAsync(IFormFileCollection attachments, string saveFilePath);
        RepositoryOutput Add(CreateTicketViewModel model, IEnumerable<IFormFile> attachments, string saveFilePath); 
        RepositoryOutput UpdateTicket(UpdateTicketModel model, IEnumerable<IFormFile> attachments, string saveFilePath);
        RepositoryOutput UpdateStatus(UpdateStatusViewModel model);
        RepositoryOutput AssignTicket(AssignTicketViewModel model);
        RepositoryOutput AssignTicket(AssignMulipleTicketViewModel model);


        RepositoryOutput UpdateTicket(long Id,UpdateTicketModel model);

        int GetCount();
        TicketDetailsModel GetTicket(long Id, string returnFilePath);
       
        #endregion
    }
}
