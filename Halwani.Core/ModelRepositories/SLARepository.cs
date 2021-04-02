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
                
                //TODO: Handle TargetDate .
                return new List<SLmMeasurement>
                {
                    new SLmMeasurement
                    {
                        SLAId = sla.Id,
                        ModifiedDate = DateTime.Now,
                        //TargetDate = ,
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
