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
using Microsoft.AspNetCore.Http;
using System.Transactions;
using System.IO;

namespace Halwani.Core.ModelRepositories
{
    public class RequestTypeRepository : BaseRepository<RequestType>, IRequestTypeRepository
    {
        public IEnumerable<RequestTypeListViewModel> List()
        {
            try
            {
                var groupBy = Find(null, null, "DefaultTeam").GroupBy(e => e.TicketType);

                return groupBy.Select(e => new RequestTypeListViewModel
                {
                    TicketType = e.FirstOrDefault().TicketType,
                    Topics = e.Select(a => new RequestTypeViewModel
                    {
                        Id = a.Id,
                        Text = a.Name,
                        TicketType = a.TicketType,
                        Icon = a.Icon,
                        Description = a.Description,
                        TeamName = a.TeamName,  
                        Priority = a.Priority,
                        Severity = a.Severity
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
                    Description = item.Description,
                    TeamName = item.TeamName,
                    Priority = item.Priority,
                    Severity = item.Severity,
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
        public RepositoryOutput Add(CreateRequestTypeModel model, IEnumerable<IFormFile> attachments, string saveFilePath, string loggedUserId, string token)
        {
            try
            {
                var RT = new RequestType()
                {
                    Name = model.Name,
                    Icon =model.Icon==null?"": model.Icon,
                    TicketType = model.TicketType,
                    Description = model.Description,
                    TeamName = model.TeamName,
                    Priority = model.Priority,
                    Severity = model.Severity,
                    RequestTypeGroups = model.GroupIds.Select(e => new RequestTypeGroups
                    {
                        GroupId = e
                    }).ToList()
                };
                Add(RT);
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Save() < 1)
                        return RepositoryOutput.CreateErrorResponse("");
                    List<string> result = StoreFiles(attachments, null, saveFilePath, RT);

                    RT.Icon = string.Join(",", result);
                    Update(RT);
                    if (Save() < 1)
                        return RepositoryOutput.CreateErrorResponse("");

                    

                    scope.Complete();
                }

               

                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }
        private static List<string> StoreFiles(IEnumerable<IFormFile> attachments, List<string> oldAttachments, string saveFilePath, RequestType RT)
        {
            var result = new List<string>();
            if (attachments == null)
                return result;

            foreach (var file in attachments)
            {
                var fileName = Guid.NewGuid().ToString() + file.FileName;
                var filePath = saveFilePath + "/" + RT.Id + "/" + fileName;
                if (!Directory.Exists(saveFilePath + "/" + RT.Id))
                {
                    Directory.CreateDirectory(saveFilePath + "/" + RT.Id);
                }
                using var fileSteam = new FileStream(filePath, FileMode.Create);
                file.CopyToAsync(fileSteam).GetAwaiter().GetResult();
                result.Add(fileName);
            }
            if (oldAttachments != null)
            {
                foreach (var item in oldAttachments)
                {
                    result.Add(item);
                }
            }
            return result;
        }


    }
}
