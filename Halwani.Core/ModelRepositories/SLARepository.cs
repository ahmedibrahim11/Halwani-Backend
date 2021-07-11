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
        private readonly ITeamRepository _teamRepository;
        private readonly IAuthenticationRepository _authenticationRepository;

        public SLARepository(ITeamRepository teamRepository, IAuthenticationRepository authenticationRepository)
        {
            _teamRepository = teamRepository;
            _authenticationRepository = authenticationRepository;
        }

        public List<SLmMeasurement> LoadTicketSlm(CreateTicketViewModel model, SLAType slaType)
        {
            try
            {
                //var team = _teamRepository.GetByName(model.TeamName);
                //if (team == null)
                //    return null;

                var sla = Find(e => e.Priority == model.Priority && e.SLAType == slaType && e.ProductCategoryName == model.ProductCategoryName2 && e.ServiceLine == model.TeamName).FirstOrDefault();
                if (sla == null)
                    return null;

                var allWeekDays = new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
                var workingDays = sla.WorkingDays.Split(",").Select(e => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), e)).ToList();
                var totalWorkingHours = int.Parse(sla.WorkingHours.Split(",")[1]) - int.Parse(sla.WorkingHours.Split(",")[0]);
                var currentDay = DateTime.Now.DayOfWeek;
                var currentDayIndex = allWeekDays.IndexOf(currentDay);
                var workDuration = sla.SLADuration;
                double totalHours = 0;
                var currentLoopDay = currentDay;
                var today = true;

                while (true)
                {
                    if (today)
                    {
                        if (workDuration < totalWorkingHours && int.Parse(sla.WorkingHours.Split(",")[1]) > DateTime.Now.Hour)
                        {
                            totalHours = int.Parse(sla.WorkingHours.Split(",")[1]) - DateTime.Now.Hour; workDuration -= totalHours;

                            if (workDuration == 0)
                                break;
                        }
                        else
                        {
                            if (int.Parse(sla.WorkingHours.Split(",")[1]) > DateTime.Now.Hour)
                            {
                                workDuration -= int.Parse(sla.WorkingHours.Split(",")[1]) - DateTime.Now.Hour;
                            }
                            totalHours += 24 - DateTime.Now.Hour;
                        }
                    }
                    else
                    {
                        if (!workingDays.Contains(currentLoopDay))
                        {
                            totalHours += 24;
                        }
                        else
                        {
                            if (workDuration <= totalWorkingHours)
                            {
                                totalHours += int.Parse(sla.WorkingHours.Split(",")[0]) + workDuration;
                                break;
                            }
                            else
                                totalHours += 24;
                            workDuration -= totalWorkingHours;
                        }
                        var indexOfCurrentLoopDay = allWeekDays.IndexOf(currentLoopDay);
                        if (indexOfCurrentLoopDay == allWeekDays.Count - 1)
                            indexOfCurrentLoopDay = -1;
                        currentLoopDay = allWeekDays.ElementAt(++indexOfCurrentLoopDay);
                    }
                    today = false;
                }

                return new List<SLmMeasurement>
                {
                    new SLmMeasurement
                    {
                        SLAId = sla.Id,
                        ModifiedDate = DateTime.Now,
                        TargetDate = DateTime.Now.AddHours(totalHours)
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
                old.ProductCategoryName = model.ProductCategoryName;
                old.ServiceLine = model.TeamName;
                old.SLADuration = model.SLADuration;
                old.SLAName = "";
                old.SLAType = model.SLAType;
                old.WorkingDays = model.WorkingDays;
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
                Add(new SLA
                {
                    Priority = model.Priority,
                    ProductCategoryName = model.ProductCategoryName,
                    ServiceLine = model.TeamName,
                    SLADuration = model.SLADuration,
                    SLAName = "",
                    SLAType = model.SLAType,
                    WorkingDays = model.WorkingDays,
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
                case SLAPageInputSort.Team:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.ServiceLine);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.ServiceLine);
                            break;
                        default:
                            break;
                    }
                    break;
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
                case SLAPageInputSort.ProductCtaegoryName:
                    switch (model.SortDirection)
                    {
                        case SortDirection.Asc:
                            query = query.OrderBy(e => e.ProductCategoryName);
                            break;
                        case SortDirection.Des:
                            query = query.OrderByDescending(e => e.ProductCategoryName);
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
                   Id=item.Id,
                   Priority=item.Priority,
                   SLAType=item.SLAType,
                   SLADuration=item.SLADuration,
                   TeamName=item.ServiceLine,
                    ProductCategoryName=item.ProductCategoryName
                   


                });
            }
        }

        public SLAModel GetForEdit(long ID)
        {
           var sla=Find(r => r.Id == ID).FirstOrDefault();
            return new SLAModel()
            {
                Id = sla.Id,
                TeamName = sla.ServiceLine,
                ProductCategoryName = sla.ProductCategoryName,
                WorkingDays = sla.WorkingDays,
                WorkingHours = sla.WorkingHours,
                SLADuration = sla.SLADuration,
                Priority = sla.Priority,
                SLAType=sla.SLAType
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
