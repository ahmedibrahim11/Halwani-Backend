using Halawani.Core;
using Halwani.Core.ViewModels.CategoryModels;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.RequestTypeModels;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.ProductCategories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Halwani.Core.ModelRepositories.Interfaces
{
    public interface IRequestTypeRepository : IBaseRepository<RequestType>
    {
        IEnumerable<RequestTypeListViewModel> List(string rootFile);
        RepositoryOutput Add(List<CreateRequestTypeModel> model);
        RepositoryOutput Add(CreateRequestTypeModel model, IEnumerable<IFormFile> attachments, string saveFilePath, string loggedUserId, string token);
        RepositoryOutput Update(CreateRequestTypeModel model, IEnumerable<IFormFile> attachments, string saveFilePath, string loggedUserId, string token);
        RepositoryOutput UpdateVisiblity(int id, bool isVisible);
        RequestTypeResultViewModel List(RequestTypeInputViewModel model, ClaimsIdentity userClaims, out RepositoryOutput response);
        GetRequestType Get(int id, string rootFile);
    }
}
