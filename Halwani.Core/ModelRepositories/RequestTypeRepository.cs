using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.RequestTypeModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Halwani.Data.Entities.Incident;

namespace Halwani.Core.ModelRepositories
{
    public class RequestTypeRepository : BaseRepository<RequestType>, IRequestTypeRepository
    {
        public IEnumerable<RequestTypeListViewModel> List()
        {
            try
            {
                var groupBy = Find().GroupBy(e => e.TicketType);
                return groupBy.Select(e => new RequestTypeListViewModel
                {
                    TicketType = e.FirstOrDefault().TicketType,
                    Topics = e.Select(a => new RequestTypeViewModel
                    {
                        Id = a.Id,
                        Text = a.Name,
                        TicketType = a.TicketType,
                        Icon = a.Icon
                    })
                });
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        public RepositoryOutput Add(List<CreateRequestTypeModel> model)
        {
            try
            {
                AddRange(model.Select(item => new RequestType()
                {
                    Name = item.Name,
                    Icon = item.Icon,
                    TicketType = item.TicketType,
                    RequestTypeGroups = item.GroupIds.Select(e => new RequestTypeGroups
                    {
                        GroupId = e
                    }).ToList()
                }).ToList());

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
    }
}
