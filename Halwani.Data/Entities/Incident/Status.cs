using System;
using System.Collections.Generic;
using System.Text;

namespace Halwani.Data.Entities.Incident
{
    public enum Status
    {
        Created = 0,
        Assigned = 1,
        WaitingResponse = 2,
        WaitingSupport = 3,
        InProgress = 4,
        Esclated = 5,
        Reopened = 6,
        Resolved = 7,
        OverDue = 8,
        Canceled = 9,
        Closed = 10
    }
}
