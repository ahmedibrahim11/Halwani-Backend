using Halwani.Core.ViewModels.GenericModels;
using Halwani.Data.Entities.Incident;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Halwani.Core.ViewModels.RequestTypeModels
{
    public class RequestTypeListViewModel
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketType TicketType { get; set; }
        public IEnumerable<RequestTypeViewModel> Topics { get; set; }
    }
    public class RequestTypeViewModel : LookupViewModel
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketType TicketType { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string TeamName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priority Priority { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketSeverity Severity { get; set; }
    }
}
