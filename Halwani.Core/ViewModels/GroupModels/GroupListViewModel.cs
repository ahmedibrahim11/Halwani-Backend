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
    }
}
