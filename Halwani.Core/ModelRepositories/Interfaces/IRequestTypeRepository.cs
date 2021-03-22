using Halawani.Core;
using Halwani.Core.ViewModels.CategoryModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.RequestTypeModels;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface IRequestTypeRepository : IBaseRepository<RequestType>
    {
        IEnumerable<RequestTypeListViewModel> List();
        RepositoryOutput Add(List<CreateRequestTypeModel> model);
    }
}
