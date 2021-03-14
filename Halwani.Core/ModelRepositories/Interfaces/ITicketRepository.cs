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
        RepositoryOutput Add(CreateTicketViewModel model);
        #endregion
    }
}
