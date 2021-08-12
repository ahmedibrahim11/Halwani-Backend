using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.Authentication;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Core.ViewModels.NotificationModels;
using Halwani.Core.ViewModels.TicketModels;
using Halwani.Data;
using Halwani.Data.Entities;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.SLA;
using Halwani.Data.Entities.User;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Halwani.Core.ModelRepositories
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ISLARepository _slaRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public TicketRepository(IAuthenticationRepository authenticationRepository, ISLARepository slaRepository, IConfiguration configuration, IUserRepository userRepository, INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _slaRepository = slaRepository;
            _authenticationRepository = authenticationRepository;
        }

        public TicketPageResultViewModel List(TicketPageInputViewModel model, ClaimsIdentity userClaims, out RepositoryOutput response)
        {
            try
            {
                response = new RepositoryOutput();
                TicketPageResultViewModel result = new TicketPageResultViewModel
                {
                    CanAdd = true
                };

                var qurey = Find(null, null, "");

                qurey = FilterLoggedUser(userClaims, qurey);
                qurey = FilterList(model, userClaims, qurey);
                qurey = SortList(model, qurey);
                PagingList(model, result, qurey);

                return result;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                response = RepositoryOutput.CreateErrorResponse(ex.Message);
                return null;
            }
        }

        public async Task<List<string>> PostFilesAsync(IFormFileCollection attachments, string saveFilePath)
        {
            try
            {
                var result = new List<string>();
                foreach (var file in attachments)
                {
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = saveFilePath + "/" + fileName;
                    if (!Directory.Exists(saveFilePath))
                    {
                        Directory.CreateDirectory(saveFilePath);
                    }

                    using var fileSteam = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(fileSteam);
                    result.Add(fileName);
                }
                return result;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        public RepositoryOutput Add(CreateTicketViewModel model, IEnumerable<IFormFile> attachments, string saveFilePath, string loggedUserId, string token)
        {
            try
            {
                Random rnd = new Random();
                var userData = new UserRepository().GetById(long.Parse(loggedUserId));
                var ticket = new Ticket()
                {
                    Description = model.Description,
                    TicketStatus = model.TicketStatus,
                    SubmitterTeam = "test",
                    SubmitterEmail = userData.Email,
                    ReportedSource = model.ReportedSource,
                    TeamName = model.TeamName,
                    Location = model.Location,
                    SubmitterName = userData.Name,
                    TicketName = model.Summary,
                    SubmitDate = DateTime.Now,
                    RequestTypeId = model.RequestTypeId,
                    Source = model.Source,
                    Priority = model.Priority,
                    TicketSeverity = model.TicketSeverity,
                    ProductCategoryName1 = model.ProductCategoryName1,
                    ProductCategoryName2 = model.ProductCategoryName2,
                    TicketNumber = "SR-" + rnd.Next(),
                    TicketHistories = new List<TicketHistory> {
                        new TicketHistory
                        {
                            OldStatus = null,
                            NewStatus = Status.Created,
                            ModifiedDate = DateTime.Now,
                        }
                    },
                    LastModifiedDate = DateTime.Now,
                    SLmMeasurements = _slaRepository.LoadTicketSlm(model),
                };
                Add(ticket);
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (Save() < 1)
                        return RepositoryOutput.CreateErrorResponse("");
                    List<string> result = StoreFiles(attachments, null, saveFilePath, ticket);

                    ticket.Attachement = string.Join(",", result);
                    Update(ticket);
                    if (Save() < 1)
                        return RepositoryOutput.CreateErrorResponse("");

                    scope.Complete();
                }

                if (ticket.SLmMeasurements != null)
                {
                    foreach (var slm in ticket.SLmMeasurements)
                    {
                        var jobId = BackgroundJob.Schedule(() => IsMet(ticket.Id, slm.Id, token),
slm.TargetDate);
                    }
                }

                var userIds = _userRepository.Find(e => e.UserTeams.Any(t => t.Team.Name == ticket.TeamName)).Select(e => e.Id.ToString());

                SendNotification(ticket.Id, NotificationType.NewTicket, loggedUserId, token, "NewTicket", userIds.ToList());

                SendSignalR(token, "updateTickets", userIds.ToArray()).GetAwaiter().GetResult();

                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }

        public RepositoryOutput UpdateTicket(UpdateTicketModel model, IEnumerable<IFormFile> attachments, string saveFilePath, string loggedUserId, string token)
        {
            try
            {
                var ticket = Find(e => e.Id == model.Id).FirstOrDefault();
                if (ticket == null)
                    return RepositoryOutput.CreateNotFoundResponse();
                ticket.Description = model.Description;
                ticket.LastModifiedDate = DateTime.Now;
                ticket.Location = model.Location;
                ticket.Priority = model.Priority;
                ticket.TicketSeverity = model.TicketSeverity;
                ticket.RequestTypeId = model.RequestTypeId;
                ticket.ReportedSource = model.ReportedSource;
                ticket.Source = model.Source;
                if (ticket.TeamName != model.TeamName)
                    ticket.TicketHistories.Add(new TicketHistory
                    {
                        FromTeam = ticket.TeamName,
                        ModifiedDate = DateTime.Now,
                        ToTeam = model.TeamName
                    });
                ticket.TeamName = model.TeamName;
                var old = model.Attachement.Split(",");
                List<string> result = StoreFiles(attachments, old.ToList(), saveFilePath, ticket);

                ticket.Attachement = string.Join(",", result);
                ticket.TicketName = model.Summary;
                //ticket.Attachement = model.Attachement + (result.Any() ? "," + string.Join(",", result) : "");
                Update(ticket);
                if (Save() < 1)
                    return RepositoryOutput.CreateErrorResponse("");

                var userIds = _userRepository.Find(e => e.UserTeams.Any(t => t.Team.Name == ticket.TeamName)).Select(e => e.Id.ToString());

                SendNotification(ticket.Id, NotificationType.NewTicket, loggedUserId, token, "UpdateTicket", userIds.ToList());

                SendSignalR(token, "updateTickets", userIds.ToArray()).Wait();

                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }

        public List<string> RemoveAttachments(string filePath, string[] attachment)
        {
            DirectoryInfo directory = new DirectoryInfo(filePath);
            var result = new List<string>();
            var fileNames = directory.GetFiles().Select(s => s.Name);
            var files = directory.GetFiles();
            if (Directory.Exists(filePath))
            {
                var OldAttachment = fileNames.Except(attachment);

                foreach (var item in OldAttachment)
                {
                    File.Delete(filePath + "/" + item);
                }

                var repteadAttachement = files.Where(x => attachment.Any(y => x.Name.Contains(y))).ToList();

                foreach (var item in repteadAttachement)
                {
                    result.Add(item.Name);
                }
                return result;
            }
            else
            {
                return null;
            }

        }

        private static List<string> StoreFiles(IEnumerable<IFormFile> attachments, List<string> oldAttachments, string saveFilePath, Ticket ticket)
        {
            var result = new List<string>();
            if (attachments == null)
                return result;

            foreach (var file in attachments)
            {
                var fileName = Guid.NewGuid().ToString() + file.FileName;
                var filePath = saveFilePath + "/" + ticket.Id + "/" + fileName;
                if (!Directory.Exists(saveFilePath + "/" + ticket.Id))
                {
                    Directory.CreateDirectory(saveFilePath + "/" + ticket.Id);
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

        public RepositoryOutput UpdateStatus(UpdateStatusViewModel model, string loggedUserId, string token)
        {
            try
            {
                var ticket = Find(e => e.Id == model.TicketId, null, "SLmMeasurements").FirstOrDefault();
                if (ticket == null)
                    return RepositoryOutput.CreateNotFoundResponse();

                //if (model.Status == Status.InProgress)
                //{
                //    var intervention = ticket.SLmMeasurements.FirstOrDefault(e => e.SLA.SLAType == Data.Entities.SLA.SLAType.Intervention);
                //    if (intervention != null && intervention.TargetDate < DateTime.Now)
                //        intervention.SLAStatus = SLAStatus.Meet;
                //}
                //if ((ticket.TicketStatus.Value == Status.Created || ticket.TicketStatus.Value == Status.Assigned) && model.Status == Status.InProgress)
                //{
                var newSla = _slaRepository.LoadTicketSlmPerStatus(ticket, model.Status, out List<SLA> closeSla);
                if (newSla != null)
                {
                    foreach (var slm in newSla)
                    {
                        ticket.SLmMeasurements.Add(slm);
                    }
                }

                if (closeSla != null)
                {
                    foreach (var sla in closeSla)
                    {
                        var ticketSlms = ticket.SLmMeasurements.Where(e => e.SLAId == sla.Id);
                        foreach (var ticketSlm in ticketSlms)
                        {
                            if (ticketSlm.SLAStatus != SLAStatus.Deattached)
                                ticketSlm.SLAStatus = SLAStatus.Meet;
                            ticketSlm.ModifiedDate = DateTime.Now;
                        }
                    }
                }

                foreach (var slm in newSla)
                {
                    var jobId = BackgroundJob.Schedule(() => IsMet(ticket.Id, slm.Id, token),
    slm.TargetDate);
                }
                //}

                ticket.TicketHistories.Add(new TicketHistory
                {
                    OldStatus = ticket.TicketStatus,
                    ModifiedDate = DateTime.Now,
                    NewStatus = model.Status
                });

                ticket.TicketStatus = model.Status;
                if (model.Status == Status.Resolved)
                    ticket.ResolveText = model.ResolveText;

                Update(ticket);
                if (Save() < 1)
                    return RepositoryOutput.CreateErrorResponse("");

                var userIds = _userRepository.Find(e => e.UserTeams.Any(t => t.Team.Name == ticket.TeamName)).Select(e => e.Id.ToString());

                SendNotification(ticket.Id, NotificationType.NewTicket, loggedUserId, token, "UpdateTicketStatus", userIds.ToList());

                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }

        public RepositoryOutput EsclateTicket(EsclateTicketViewModel model, string loggedUserId, string token)
        {
            try
            {
                var ticket = Find(e => e.Id == model.TicketId, null, "SLmMeasurements").FirstOrDefault();
                if (ticket == null)
                    return RepositoryOutput.CreateNotFoundResponse();

                var newSla = _slaRepository.LoadTicketSlmPerStatus(ticket, Status.Esclated, out List<SLA> closeSla);
                if (newSla != null)
                {
                    foreach (var slm in newSla)
                    {
                        ticket.SLmMeasurements.Add(slm);
                    }
                }

                if (closeSla != null)
                {
                    foreach (var sla in closeSla)
                    {
                        var ticketSlms = ticket.SLmMeasurements.Where(e => e.SLAId == sla.Id);
                        foreach (var ticketSlm in ticketSlms)
                        {
                            if (ticketSlm.SLAStatus != SLAStatus.Deattached)
                                ticketSlm.SLAStatus = SLAStatus.Meet;
                            ticketSlm.ModifiedDate = DateTime.Now;
                        }
                    }
                }

                foreach (var slm in newSla)
                {
                    var jobId = BackgroundJob.Schedule(() => IsMet(ticket.Id, slm.Id, token),
    slm.TargetDate);
                }

                ticket.TicketHistories.Add(new TicketHistory
                {
                    OldStatus = ticket.TicketStatus,
                    ModifiedDate = DateTime.Now,
                    NewStatus = Status.Esclated
                });

                ticket.TicketStatus = Status.Esclated;
                ticket.EsclationReason = model.EsclationReason;

                Update(ticket);
                if (Save() < 1)
                    return RepositoryOutput.CreateErrorResponse("");

                var userIds = _userRepository.Find(e => e.UserTeams.Any(t => t.Team.Name == ticket.TeamName)).Select(e => e.Id.ToString());

                SendNotification(ticket.Id, NotificationType.NewTicket, loggedUserId, token, "UpdateTicketStatus", userIds.ToList());

                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }

        public RepositoryOutput AssignTicket(AssignMulipleTicketViewModel model, string loggedUserId, string token)
        {
            try
            {
                foreach (var item in model.TicketIds)
                {
                    var ticket = Find(s => s.Id == item, null, "").FirstOrDefault();

                    if (ticket == null)
                        return RepositoryOutput.CreateNotFoundResponse();

                    ticket.TicketHistories.Add(new TicketHistory
                    {
                        FromTeam = ticket.AssignedUser,
                        ModifiedDate = DateTime.Now,
                        ToTeam = model.UserName
                    });
                    var newSla = _slaRepository.LoadTicketSlmPerStatus(ticket, Status.Assigned, out List<SLA> closeSla);
                    if (newSla != null)
                    {
                        foreach (var slm in newSla)
                        {
                            ticket.SLmMeasurements.Add(slm);
                        }
                    }

                    if (closeSla != null)
                    {
                        foreach (var sla in closeSla)
                        {
                            var ticketSlms = ticket.SLmMeasurements.Where(e => e.SLAId == sla.Id);
                            foreach (var ticketSlm in ticketSlms)
                            {
                                if (ticketSlm.SLAStatus != SLAStatus.Deattached)
                                    ticketSlm.SLAStatus = SLAStatus.Meet;
                                ticketSlm.ModifiedDate = DateTime.Now;
                            }
                        }
                    }

                    foreach (var slm in newSla)
                    {
                        var jobId = BackgroundJob.Schedule(() => IsMet(ticket.Id, slm.Id, token),
        slm.TargetDate);
                    }
                    ticket.AssignedUser = model.UserName;
                    ticket.TicketStatus = Status.Assigned;
                    Update(ticket);
                    if (Save() < 1)
                        return RepositoryOutput.CreateErrorResponse("");

                    var userIds = _userRepository.Find(e => e.UserTeams.Any(t => t.Team.Name == ticket.TeamName)).Select(e => e.Id.ToString());

                    SendNotification(ticket.Id, NotificationType.NewTicket, loggedUserId, token, "TicketAssignedToUser", userIds.ToList());

                    return RepositoryOutput.CreateSuccessResponse();
                }

                return RepositoryOutput.CreateSuccessResponse();

            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }

        public RepositoryOutput AssignTicket(AssignTicketViewModel model, string loggedUserId, string token)
        {
            try
            {
                var ticket = Find(e=> e.Id == model.TicketId).FirstOrDefault();
                if (ticket == null)
                    return RepositoryOutput.CreateNotFoundResponse();

                ticket.TicketHistories.Add(new TicketHistory
                {
                    FromTeam = ticket.AssignedUser,
                    ModifiedDate = DateTime.Now,
                    ToTeam = model.UserName
                });
                var newSla = _slaRepository.LoadTicketSlmPerStatus(ticket, Status.Assigned, out List<SLA> closeSla);
                if (newSla != null)
                {
                    foreach (var slm in newSla)
                    {
                        ticket.SLmMeasurements.Add(slm);
                    }
                }

                if (closeSla != null)
                {
                    foreach (var sla in closeSla)
                    {
                        var ticketSlms = ticket.SLmMeasurements.Where(e => e.SLAId == sla.Id);
                        foreach (var ticketSlm in ticketSlms)
                        {
                            if (ticketSlm.SLAStatus != SLAStatus.Deattached)
                                ticketSlm.SLAStatus = SLAStatus.Meet;
                            ticketSlm.ModifiedDate = DateTime.Now;
                        }
                    }
                }

                foreach (var slm in newSla)
                {
                    var jobId = BackgroundJob.Schedule(() => IsMet(ticket.Id, slm.Id, token),
    slm.TargetDate);
                }
                ticket.AssignedUser = model.UserName;
                ticket.TicketStatus = Status.Assigned;
                Update(ticket);
                if (Save() < 1)
                    return RepositoryOutput.CreateErrorResponse("");

                var userIds = _userRepository.Find(e => e.UserTeams.Any(t => t.Team.Name == ticket.TeamName)).Select(e => e.Id.ToString());

                SendNotification(ticket.Id, NotificationType.NewTicket, loggedUserId, token, "TicketAssignedToUser", userIds.ToList());

                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse(ex.Message);
            }
        }

        public TicketDetailsModel GetTicket(long id, string returnFilePath)
        {
            try
            {
                var ticket = Find(e => e.Id == id).FirstOrDefault();
                if (ticket == null)
                    return null;
                var attachementsList = ticket.Attachement.Split(",", StringSplitOptions.None);
                var slm = ticket.SLmMeasurements;
                var interventionTime = slm.FirstOrDefault(e => e.SLA.SLAType == Data.Entities.SLA.SLAType.Intervention)?.TargetDate - DateTime.Now;
                var resolutionTime = slm.FirstOrDefault(e => e.SLA.SLAType == Data.Entities.SLA.SLAType.Resolution)?.TargetDate - DateTime.Now;
                return new TicketDetailsModel
                {
                    RequestTypeId = ticket.RequestTypeId,
                    LastModifiedDate = ticket.LastModifiedDate,
                    AssignedUser = ticket.AssignedUser,
                    Description = ticket.Description,
                    Priority = Enum.GetName(typeof(Priority), ticket.Priority),
                    ProductCategoryName1 = ticket.ProductCategoryName1,
                    ProductCategoryName2 = ticket.ProductCategoryName2,
                    ReportedSource = ticket.ReportedSource,
                    ResolvedDate = ticket.ResolvedDate,
                    ResolveText = ticket.ResolveText,
                    TeamName = ticket.TeamName,
                    Location = ticket.Location,
                    Source = Enum.GetName(typeof(Source), ticket.Source),
                    SubmitterEmail = ticket.SubmitterEmail,
                    SubmitDate = ticket.SubmitDate,
                    SubmitterName = ticket.SubmitterName,
                    SubmitterTeam = ticket.SubmitterTeam,
                    TicketName = ticket.TicketName,
                    TicketNumber = ticket.TicketNumber,
                    TicketSeverity = Enum.GetName(typeof(TicketSeverity), ticket.TicketSeverity),
                    TicketStatus = ticket.TicketStatus,
                    Attachement = attachementsList.Select(e => returnFilePath + e).ToArray(),
                    RequestType = new RequestTypeModel
                    {
                        Id = ticket.RequestType.Id,
                        Icon = returnFilePath + "/" + ticket.RequestTypeId + "/" + ticket.RequestType.Icon,
                        Name = ticket.RequestType.Name,
                        TicketType = ticket.RequestType.TicketType
                    },
                    TicketSlms = ticket.SLmMeasurements.Select(e => new TicketSLAModel
                    {
                        TargetDate = e.TargetDate,
                        CloseDate = e.ModifiedDate,
                        SLAType = e.SLA.SLAType,
                        Status = e.SLAStatus
                    }).ToList(),
                    InterventionSLA = interventionTime.HasValue ? interventionTime.Value.TotalHours.ToString() : "",
                    ResolutionSLA = resolutionTime.HasValue ? resolutionTime.Value.TotalHours.ToString() : ""
                };
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }
        }

        public void IsMet(long ticketId, long ticketSLMId, string token)
        {
            try
            {
                var ticket = Find(e => e.Id == ticketId).FirstOrDefault();

                var slm = ticket.SLmMeasurements.FirstOrDefault(e => e.Id == ticketSLMId);
                if (slm != null && slm.SLA.OpenStatus.Contains(ticket.TicketStatus.ToString()))
                {
                    slm.SLAStatus = SLAStatus.Deattached;
                    ticket.TicketStatus = Status.OverDue;

                    Update(ticket);
                    Save();

                    var userIds = _userRepository.Find(e => e.UserTeams.Any(t => t.Team.Name == ticket.TeamName)).Select(e => e.Id.ToString());

                    SendNotification(ticket.Id, NotificationType.NewTicket, "", token, "TicketLate", userIds.ToList());
                }
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
            }
        }

        #region Private Methods

        private void SendNotification(long ticketId, NotificationType notificationType, string madeBy, string token, string resourceKey, List<string> userIds)
        {
            try
            {
                _notificationRepository.Add(new ViewModels.NotificationModels.AddNotificationModel
                {
                    ObjectId = ticketId.ToString(),
                    NotificationType = notificationType,
                    ResourceKey = resourceKey,
                    UsersIds = userIds
                }, madeBy, token);
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
            }
        }

        public async Task SendSignalR(string token, string eventName,/* List<string> userIds,*/ params string[] paramters)
        {
            try
            {
                var signalrUrl = _configuration["SignalR:Url"];
                //var signalrUrl = "https://localhost:44312/hubs";
                var hubConnectionBuilder = new HubConnectionBuilder();
                var hubConnection = hubConnectionBuilder.WithUrl(signalrUrl, Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets, options =>
                {
                    options.AccessTokenProvider = async () => { return token; };
                    options.UseDefaultCredentials = true;
                }).Build();
                await hubConnection.StartAsync();
                await hubConnection.InvokeAsync(eventName, paramters);
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
            }
        }

        private void PagingList(TicketPageInputViewModel model, TicketPageResultViewModel result, IEnumerable<Ticket> qurey)
        {
            result.TotalCount = qurey.Count();
            if (!model.IsPrint)
                qurey = qurey.Skip(model.PageNumber * model.PageSize).Take(model.PageSize);

            foreach (var item in qurey)
            {
                result.PageData.Add(new TicketPageData
                {
                    ID = item.Id,
                    CanEsclate = true,
                    CanAssign = true,
                    CanView = true,
                    CanDelete = true,
                    CreationDate = item.SubmitDate,
                    Severity = item.TicketSeverity,
                    Status = item.TicketStatus,
                    TicketTopic = item.TicketName,
                    TicketNumber = item.TicketNumber,
                    TeamName = item.TeamName,
                    RequestType = new RequestTypeModel
                    {
                        Id = item.RequestType.Id,
                        Name = item.RequestType.Name,
                        Icon = item.RequestType.Icon,
                        TicketType = item.RequestType.TicketType
                    },
                    RasiedBy = new RasiedByViewModel
                    {
                        Email = item.SubmitterEmail,
                        Name = item.SubmitterName
                    }
                });
            }
        }

        private IEnumerable<Ticket> SortList(TicketPageInputViewModel model, IEnumerable<Ticket> query)
        {
            switch (model.SortValue)
            {
                case TicketPageInputSort.RasiedBy:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.SubmitterName);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.SubmitterName);
                            break;
                        default:
                            break;
                    }
                    break;
                case TicketPageInputSort.CreationDate:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.SubmitDate);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.SubmitDate);
                            break;
                        default:
                            break;
                    }
                    break;
                case TicketPageInputSort.Topic:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.TicketName);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.TicketName);
                            break;
                        default:
                            break;
                    }
                    break;
                case TicketPageInputSort.Severity:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.TicketSeverity);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.TicketSeverity);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    //switch (model.SortDirection)
                    //{
                    //    case SortDirection.Asc:
                    //        query = query.OrderBy(e => e.SubmitDate);
                    //        break;
                    //case SortDirection.Des:
                    query = query.OrderByDescending(e => e.SubmitDate);
                    //        break;
                    //    default:
                    //        break;
                    //}
                    break;
            }
            return query;
        }

        public IEnumerable<string> getTicketNO()
        {
            try
            {
                var t = Find(t => t.TicketNumber != null).Select(t => t.TicketNumber).ToList();
                return t;
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return null;
            }

        }

        private IEnumerable<Ticket> FilterList(TicketPageInputViewModel model, ClaimsIdentity userClaims, IEnumerable<Ticket> query)
        {
            if (model.SearchText != null && model.SearchText.Any())
                query = query.Where(e => model.SearchText[0] == e.TicketNumber).ToList();
            if (model.Filter != null)
            {
                if (model.Filter.Priority.HasValue)
                    query = query.Where(e => e.Priority == model.Filter.Priority);
                if (model.Filter.Severity.HasValue)
                    query = query.Where(e => e.TicketSeverity == model.Filter.Severity);
                if (model.Filter.TicketType.HasValue)
                {
                    if (model.Filter.TicketType != TicketType.All)
                    {
                        query = query.Where(e => e.RequestType.TicketType == model.Filter.TicketType);
                    }

                }
                if (model.Filter.TicketTabs == TicketTabs.AssignedToMe)
                {
                    var userSession = _authenticationRepository.LoadUserSession(userClaims);
                    query = query.Where(e => e.AssignedUser == userSession.UserName);
                }
                if (!string.IsNullOrEmpty(model.Filter.SubmitterName))
                    query = query.Where(e => e.SubmitterName == model.Filter.SubmitterName);
                if (model.Filter.Source.HasValue)
                    query = query.Where(e => e.Source == model.Filter.Source);
                if (model.Filter.State.HasValue)
                    query = query.Where(e => e.TicketStatus == model.Filter.State);
                if (model.Filter.Date.HasValue)
                    query = query.Where(e => e.SubmitDate.Date == model.Filter.Date.Value.Date);
                //TODO: Add Location Filter.
                if (model.Filter.Location != null)
                    query = query.Where(e => e.Location == model.Filter.Location);
            }
            return query;
        }

        private IEnumerable<Ticket> FilterLoggedUser(ClaimsIdentity userClaims, IEnumerable<Ticket> query)
        {
            var userSession = _authenticationRepository.LoadUserSession(userClaims);
            if (!userSession.IsAllTeams && userSession.Role != RoleEnum.User)
            {
                query = query.Where(e => userSession.TeamsIds.Contains(e.TeamName));
            }
            return query;
        }

        public int GetCount()
        {
            return Count();
        }

        #endregion
    }
}
