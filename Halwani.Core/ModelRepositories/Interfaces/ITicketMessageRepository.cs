using Halawani.Core;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ModelRepositories.Interfaces
{
   public interface ITicketMessageRepository : IBaseRepository<TicketMessage>
    {
        IEnumerable<ViewModels.Message.TicketMessageDTO> List(long TicketID);
        RepositoryOutput Add(ViewModels.Message.TicketMessageDTO model);
    }
}
