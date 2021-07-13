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

namespace Halwani.Core.ModelRepositories
{
    public class SLARepository : BaseRepository<SLA>, ISLARepository
    {
        private readonly IRequestTypeRepository _requestTypeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITeamRepository _teamRepository;
        public SLARepository(ITeamRepository teamRepository, IRequestTypeRepository requestTypeRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _requestTypeRepository = requestTypeRepository;
            _teamRepository = teamRepository;
        }

        public List<SLmMeasurement> LoadTicketSlm(CreateTicketViewModel model)
        {
            try
            {
                var productCategory = _categoryRepository.Find(e => e.Name == model.ProductCategoryName2).FirstOrDefault();
                if (productCategory == null)
                    return null;

                var requestType = _requestTypeRepository.GetById(model.RequestTypeId);
                if (requestType == null)
                    return null;

                var sla = Find(e => e.Priority == model.Priority && e.RequestType == requestType.Name && e.OpenStatus.Contains(Status.Created.ToString())).FirstOrDefault();
                if (sla == null)
                    return null;

                var totalWorkingHours = int.Parse(sla.WorkingHours.Split(",")[1]) - int.Parse(sla.WorkingHours.Split(",")[0]);
                var workDuration = productCategory.Goal.HasValue ? sla.SLADuration + productCategory.Goal : sla.SLADuration;
                double totalHours = 0;

                while (true)
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

        public List<SLmMeasurement> LoadTicketSlmPerStatus(Ticket ticket, Status status, out List<SLA> closeSla)
        {
            closeSla = null;
            try
            {
                var productCategory = _categoryRepository.Find(e => e.Name == ticket.ProductCategoryName2).FirstOrDefault();
                if (productCategory == null)
                    return null;

                var requestType = _requestTypeRepository.GetById(ticket.RequestTypeId);
                if (requestType == null)
                    return null;

                var openSla = Find(e => e.Priority == ticket.Priority && e.RequestType == requestType.Name && e.OpenStatus.Contains(status.ToString())).FirstOrDefault();
                if (openSla == null)
                    return null;

                closeSla = Find(e => e.Priority == ticket.Priority && e.RequestType == requestType.Name && e.CloseStatus.Contains(status.ToString())).ToList();

                var totalWorkingHours = int.Parse(openSla.WorkingHours.Split(",")[1]) - int.Parse(openSla.WorkingHours.Split(",")[0]);
                var workDuration = productCategory.Goal.HasValue ? openSla.SLADuration + productCategory.Goal : openSla.SLADuration;
                double totalHours = 0;

                while (true)
                {
                    if (workDuration < totalWorkingHours && int.Parse(openSla.WorkingHours.Split(",")[1]) > DateTime.Now.Hour)
                    {
                        totalHours = int.Parse(openSla.WorkingHours.Split(",")[1]) - DateTime.Now.Hour; workDuration -= totalHours;

                        if (workDuration == 0)
                            break;
                    }
                    else
                    {
                        if (int.Parse(openSla.WorkingHours.Split(",")[1]) > DateTime.Now.Hour)
                        {
                            workDuration -= int.Parse(openSla.WorkingHours.Split(",")[1]) - DateTime.Now.Hour;
                        }
                        totalHours += 24 - DateTime.Now.Hour;
                    }
                }

                return new List<SLmMeasurement>
                {
                    new SLmMeasurement
                    {
                        SLAId = openSla.Id,
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
