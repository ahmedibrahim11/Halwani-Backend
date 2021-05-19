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

namespace Halwani.Core.ModelRepositories
{
    public class SLARepository : BaseRepository<SLA>, ISLARepository
    {
        private readonly ITeamRepository _teamRepository;
        public SLARepository(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public List<SLmMeasurement> LoadTicketSlm(CreateTicketViewModel model, SLAType slaType)
        {
            try
            {
                var team = _teamRepository.GetByName(model.TeamName);
                if (team == null)
                    return null;

                var sla = Find(e => e.Priority == model.Priority && e.SLAType == slaType && e.ProductCategoryName == model.ProductCategoryName2 && e.ServiceLine == team.ServiceLine).FirstOrDefault();
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
