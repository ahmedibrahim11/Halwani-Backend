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
        RepositoryOutput Add(CreateTicketViewModel model, IEnumerable<IFormFile> attachments, string saveFilePath, string loggedUserId, string token); 
        RepositoryOutput UpdateTicket(UpdateTicketModel model, IEnumerable<IFormFile> attachments, string saveFilePath, string loggedUserId, string token);
        RepositoryOutput UpdateStatus(UpdateStatusViewModel model, string loggedUserId, string token);
        RepositoryOutput EsclateTicket(EsclateTicketViewModel model, string loggedUserId, string token);
        RepositoryOutput AssignTicket(AssignTicketViewModel model, string loggedUserId, string token);
        RepositoryOutput AssignTicket(AssignMulipleTicketViewModel model, string loggedUserId, string token);
        int GetCount();
        TicketDetailsModel GetTicket(long Id, string returnFilePath);
        IEnumerable<string> getTicketNO();
        List<string> RemoveAttachments(string filePath,string [] attachment);

        #endregion
    }
}
