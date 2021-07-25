//using Halawani.Core;
//using Halawani.Core.Helper;
//using Halwani.Core.ModelRepositories.Interfaces;
//using Halwani.Core.ViewModels.RequestTypeModels;
//using Halwani.Core.ViewModels.GenericModels;
//using Halwani.Data.Entities.ProductCategories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Halwani.Data.Entities.Incident;
//using Microsoft.AspNetCore.Http;
//using System.Transactions;
//using System.IO;
//using System.Security.Claims;
//using Halwani.Data.Entities.User;
//using Halwani.Core.ViewModels.TicketModels;

//namespace Halwani.Core.ModelRepositories
//{
//    public class UserRequestRepository
//    {
//        public RepositoryOutput Add(List<CreateRequestTypeModel> model)
//        {
//            try
//            {
//                AddRange(model.Select(item => new RequestType()
//                {
//                    Name = item.Name,
//                    Icon = item.Icon,
//                    TicketType = item.TicketType,
//                    Description = item.Description,
//                    TeamName = item.TeamName,
//                    Priority = item.Priority,
//                    Severity = item.Severity,
//                    RequestTypeGroups = item.GroupIds.Select(e => new RequestTypeGroups
//                    {
//                        GroupId = e
//                    }).ToList()
//                }).ToList());

//                if (Save() < 1)
//                    return RepositoryOutput.CreateErrorResponse("");
//                return RepositoryOutput.CreateSuccessResponse();
//            }
//            catch (Exception ex)
//            {
//                RepositoryHelper.LogException(ex);
//                return RepositoryOutput.CreateErrorResponse(ex.Message);
//            }
//        }
//    }
//}
