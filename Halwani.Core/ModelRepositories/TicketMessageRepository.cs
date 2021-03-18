using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.Message;
using Halwani.Data.Entities.Message;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halwani.Core.ModelRepositories
{
    public class TicketMessageRepository : BaseRepository<TicketMessage>, ITicketMessageRepository
    {
        public RepositoryOutput Add(TicketMessageDTO model)
        {
            try
            {
                Add(new TicketMessage()
                {
                    Submitter=model.Submitter,
                    MessageText=model.MessageText,
                    TicketId=model.TicketID
                    
                });

                //TODO: Handle SLA.
                if (Save() < 1)
                    return RepositoryOutput.CreateErrorResponse("");

                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }

        }

        public IEnumerable<TicketMessageDTO> List(long TicketID)
        {
            try
            {
                return Find(e => e.TicketId==TicketID).Select(e => new TicketMessageDTO
                {
                    MessageText=e.MessageText,
                    Submitter=e.Submitter
                });
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }
    }
}
