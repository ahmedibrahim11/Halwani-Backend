using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.Incident;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Halwani.Core.ViewModels.GroupModels
{
    public class GroupListViewModel : LookupViewModel
    {
        public IEnumerable<RequestTypeViewModel> RequestTypes { get; set; }
    }
    public class RequestTypeViewModel : LookupViewModel
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketType TicketType { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string  DefaultTeam { get; set; }

        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public Priority Priority { get; set; }
        public TicketSeverity TicketSeverity { get; set; }
    }
    public class GroupList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }

    }
    public class GroupPageInputViewModel : PaginationViewModel
    {
        public Group Filter { get; set; }
        public GroupPageInputSort SortValue { get; set; }
    }

    public enum GroupPageInputSort
    {
        Name = 0,
        TopicCount = 1,
        
    }
    public class GroupResultViewModel : PageResult<GroupReturnedModel>
    {
        
    }
    public class GroupReturnedModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TopicCount { get; set; }
        public bool isVisable { get; set; }
    }
}
