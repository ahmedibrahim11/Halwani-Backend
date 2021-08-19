using Halawani.Core;
using Halawani.Core.Helper;
using Halwani.Core.ModelRepositories.Interfaces;
using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Halwani.Data.Entities.Incident;
using Halwani.Data.Entities.SLA;
using Halwani.Core.ViewModels.TicketModels;
using Halwani.Data.Entities.SLM_Measurement;
using Halwani.Core.ViewModels.SLAModels;
using System.Security.Claims;
using Halwani.Data.Entities.User;

namespace Halwani.Core.ModelRepositories
{
    public class SLARepository : BaseRepository<SLA>, ISLARepository
    {
        private readonly IRequestTypeRepository _requestTypeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IAuthenticationRepository _authenticationRepository;

        public SLARepository(ITeamRepository teamRepository, IRequestTypeRepository requestTypeRepository, ICategoryRepository categoryRepository, IAuthenticationRepository authenticationRepository)
        {
            _categoryRepository = categoryRepository;
            _requestTypeRepository = requestTypeRepository;
            _teamRepository = teamRepository;
            _authenticationRepository = authenticationRepository;
        }

        public List<SLmMeasurement> LoadTicketSlm(CreateTicketViewModel model)
        {
            try
            {
                var productCategory = _categoryRepository.Find(e => e.Name == model.ProductCategoryName2).FirstOrDefault();
                //if (productCategory == null)
                //    return null;

                var requestType = _requestTypeRepository.GetById(model.RequestTypeId);
                if (requestType == null)
                    return null;

                var sla = Find(e => e.Priority == model.Priority && e.RequestType == requestType.Name && e.OpenStatus.Contains(((int)Status.Created).ToString())).FirstOrDefault();
                if (sla == null)
                    return null;

                var totalWorkingHours = int.Parse(sla.WorkingHours.Split(",")[1]) - int.Parse(sla.WorkingHours.Split(",")[0]);
                var workDuration = productCategory != null && productCategory.Goal.HasValue ? sla.SLADuration + (double)productCategory.Goal : sla.SLADuration;
                double totalHours = 0;
                bool firstIteration = true;

                while (true)
                {
                    if (workDuration < totalWorkingHours)
                    {
                        if (workDuration == 0)
                            break;

                        if (firstIteration)
                        {
                            if (int.Parse(sla.WorkingHours.Split(",")[1]) > DateTime.Now.Hour)
                            {
                                totalHours = int.Parse(sla.WorkingHours.Split(",")[1]) - DateTime.Now.Hour;
                                workDuration -= totalHours;
                                if (workDuration > 0)
                                    totalHours += int.Parse(sla.WorkingHours.Split(",")[0]);
                            }
                            else
                            {
                                totalHours += 24 - DateTime.Now.Hour + workDuration + int.Parse(sla.WorkingHours.Split(",")[0]);
                                workDuration = 0;
                            }
                        }
                        else
                        {
                            totalHours += workDuration;
                            workDuration = 0;
                        }
                    }
                    else
                    {
                        if (firstIteration)
                        {
                            if (int.Parse(sla.WorkingHours.Split(",")[1]) > DateTime.Now.Hour)
                            {
                                workDuration -= int.Parse(sla.WorkingHours.Split(",")[1]) - DateTime.Now.Hour;
                            }
                            totalHours += 24 - DateTime.Now.Hour + int.Parse(sla.WorkingHours.Split(",")[0]);
                        }
                        else
                        {
                            workDuration -= totalWorkingHours;
                            totalHours += 24;
                        }
                    }
                    firstIteration = false;
                }

                return new List<SLmMeasurement>
                {
                    new SLmMeasurement
                    {
                        SLAId = sla.Id,
                        ModifiedDate = null,
                        TargetDate = DateTime.Now.AddHours(totalHours).AddHours(Strings.AddedHoursToDate)
                    }
                };
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                throw ex;
            }
        }

        public List<SLmMeasurement> LoadTicketSlmPerStatus(Ticket ticket, Status status, out List<SLA> closeSla)
        {
            closeSla = null;
            try
            {
                var productCategory = _categoryRepository.Find(e => e.Name == ticket.ProductCategoryName2).FirstOrDefault();

                var requestType = _requestTypeRepository.GetById(ticket.RequestTypeId);
                if (requestType == null)
                    return null;

                closeSla = Find(e => e.Priority == ticket.Priority && e.RequestType == requestType.Name && e.CloseStatus.Contains(((int)status).ToString())).ToList();

                var openSla = Find(e => e.Priority == ticket.Priority && e.RequestType == requestType.Name && e.OpenStatus.Contains(((int)status).ToString())).FirstOrDefault();
                if (openSla == null)
                    return new List<SLmMeasurement>();

                var totalWorkingHours = int.Parse(openSla.WorkingHours.Split(",")[1]) - int.Parse(openSla.WorkingHours.Split(",")[0]);
                var workDuration = productCategory != null && productCategory.Goal.HasValue ? openSla.SLADuration + (double)productCategory.Goal : openSla.SLADuration;
                double totalHours = 0;
                bool firstIteration = true;

                while (true)
                {
                    if (workDuration < totalWorkingHours)
                    {
                        if (workDuration == 0)
                            break;

                        if (firstIteration)
                        {
                            if (int.Parse(openSla.WorkingHours.Split(",")[1]) > DateTime.Now.Hour)
                            {
                                totalHours = int.Parse(openSla.WorkingHours.Split(",")[1]) - DateTime.Now.Hour;
                                workDuration -= totalHours;
                                if (workDuration > 0)
                                    totalHours += int.Parse(openSla.WorkingHours.Split(",")[0]);
                            }
                            else
                            {
                                totalHours += 24 - DateTime.Now.Hour + workDuration + int.Parse(openSla.WorkingHours.Split(",")[0]);
                                workDuration = 0;
                            }
                        }
                        else
                        {
                            totalHours += workDuration;
                            workDuration = 0;
                        }
                    }
                    else
                    {
                        if (firstIteration)
                        {
                            if (int.Parse(openSla.WorkingHours.Split(",")[1]) > DateTime.Now.Hour)
                            {
                                workDuration -= int.Parse(openSla.WorkingHours.Split(",")[1]) - DateTime.Now.Hour;
                            }
                            totalHours += 24 - DateTime.Now.Hour + int.Parse(openSla.WorkingHours.Split(",")[0]);
                        }
                        else
                        {
                            workDuration -= totalWorkingHours;
                            totalHours += 24;
                        }
                    }
                    firstIteration = false;
                }

                return new List<SLmMeasurement>
                {
                    new SLmMeasurement
                    {
                        SLAId = openSla.Id,
                        ModifiedDate = null,
                        TargetDate = totalHours> totalWorkingHours ? DateTime.Now.AddHours(totalHours).AddMinutes(-DateTime.Now.Minute).AddHours(Strings.AddedHoursToDate) : DateTime.Now.AddHours(totalHours).AddHours(Strings.AddedHoursToDate)
                    }
                };
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                throw ex;
            }
        }

        public RepositoryOutput Edit(SLAModel model)
        {
            try
            {
                var old = Find(e => e.Id == model.Id).FirstOrDefault();
                if (old == null)
                    return RepositoryOutput.CreateNotFoundResponse();
                old.Priority = model.Priority;
                old.RequestType = model.RequestType;
                old.SLADuration = model.SLADuration;
                old.SLAType = model.SLAType;
                old.CloseStatus = model.CloseStatus;
                old.OpenStatus = model.OpenStatus;
                old.WorkingHours = model.WorkingHours;

                Update(old);
                if (Save() < 1)
                    return RepositoryOutput.CreateErrorResponse();
                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse();
            }
        }

        public RepositoryOutput Add(SLAModel model)
        {
            try
            {
                //var workDays = "";
                //var days = model.WorkingDays.Split(",");
                //foreach (var day in days)
                //{
                //    workDays += (Enum.Parse(typeof(DayOfWeek),day)).ToString()+",";
                //}
                //workDays = workDays.Substring(0, workDays.Length - 1);
                Add(new SLA
                {
                    Priority = model.Priority,
                    RequestType = model.RequestType,
                    SLADuration = model.SLADuration,
                    SLAType = model.SLAType,
                    CloseStatus = model.CloseStatus,
                    OpenStatus = model.OpenStatus,
                    WorkingHours = model.WorkingHours
                });
                if (Save() < 1)
                    return RepositoryOutput.CreateErrorResponse();
                return RepositoryOutput.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                RepositoryHelper.LogException(ex);
                return RepositoryOutput.CreateErrorResponse();
            }
        }

        public SLAResultViewModel List(SLAPageInputViewModel model, ClaimsIdentity userClaims, out RepositoryOutput response)
        {
            try
            {
                response = new RepositoryOutput();
                SLAResultViewModel result = new SLAResultViewModel();


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
        private IEnumerable<SLA> FilterLoggedUser(ClaimsIdentity userClaims, IEnumerable<SLA> query)
        {
            var userSession = _authenticationRepository.LoadUserSession(userClaims);
            if (userSession.Role != RoleEnum.User)
            {
                return query;
            }
            return new List<SLA>();
        }
        private IEnumerable<SLA> FilterList(SLAPageInputViewModel model, ClaimsIdentity userClaims, IEnumerable<SLA> query)
        {


            return query;
        }
        private IEnumerable<SLA> SortList(SLAPageInputViewModel model, IEnumerable<SLA> query)
        {
            switch (model.SortValue)
            {
                case SLAPageInputSort.Priority:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.Priority);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.Priority);
                            break;
                        default:
                            break;
                    }
                    break;
                //case SLAPageInputSort.Team:
                //    switch (model.SortDirection)
                //    {
                //        case SortDirection.Asc:
                //            query = query.OrderBy(e => e.ServiceLine);
                //            break;
                //        case SortDirection.Des:
                //            query = query.OrderByDescending(e => e.ServiceLine);
                //            break;
                //        default:
                //            break;
                //    }
                //    break;
                case SLAPageInputSort.SLAGoal:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.SLADuration);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.SLADuration);
                            break;
                        default:
                            break;
                    }
                    break;
                case SLAPageInputSort.SLAType:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.SLAType);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.SLAType);
                            break;
                        default:
                            break;
                    }
                    break;
                case SLAPageInputSort.RequestType:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.RequestType);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.RequestType);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.Id);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.Id);
                            break;
                        default:
                            break;
                    }
                    break;
            }
            return query;
        }
        private void PagingList(SLAPageInputViewModel model, SLAResultViewModel result, IEnumerable<SLA> qurey)
        {
            result.TotalCount = qurey.Count();
            if (!model.IsPrint)
                qurey = qurey.Skip(model.PageNumber * model.PageSize).Take(model.PageSize);

            foreach (var item in qurey)
            {
                result.PageData.Add(new SLAModel
                {
                    Id = item.Id,
                    Priority = item.Priority,
                    SLAType = item.SLAType,
                    SLADuration = item.SLADuration,
                    OpenStatus = item.OpenStatus,
                    CloseStatus = item.CloseStatus,
                    RequestType = item.RequestType,
                    WorkingHours = item.WorkingHours
                });
            }
        }

        public SLAModel GetForEdit(long ID)
        {
            var sla = Find(r => r.Id == ID).FirstOrDefault();
            return new SLAModel()
            {
                Id = sla.Id,
                Priority = sla.Priority,
                SLAType = sla.SLAType,
                SLADuration = sla.SLADuration,
                OpenStatus = sla.OpenStatus,
                CloseStatus = sla.CloseStatus,
                RequestType = sla.RequestType,
                WorkingHours = sla.WorkingHours
            };
        }


        //public List<SLmMeasurement> UpdateTicketSlm(CreateTicketViewModel model, Ticket ticket, SLAType slaType)
        //{
        //    try
        //    {
        //        var team = _teamRepository.GetByName(model.TeamName);
        //        if (team == null)
        //            return null;

        //        foreach (var slm in ticket.SLmMeasurements)
        //        {
        //            slm.SLAStatus = SLAStatus.Deattached; 
        //        }

        //        var sla = Find(e => e.Priority == model.Priority && e.SLAType == slaType && e.ProductCategoryName == model.ProductCategoryName2 && e.ServiceLine == team.ServiceLine).FirstOrDefault();
        //        if (sla == null)
        //            return null;

        //        //TODO: Handle TargetDate .
        //        return new List<SLmMeasurement>
        //        {
        //            new SLmMeasurement
        //            {
        //                SLAId = sla.Id,
        //                ModifiedDate = DateTime.Now,
        //                //TargetDate = ,
        //            }
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        RepositoryHelper.LogException(ex);
        //        throw ex;
        //    }
        //}
    }
}
