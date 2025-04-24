using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Domain.Enums
{
    public enum ApplicationStatus
    {
        Applied,
        InterviewScheduled,
        Interviewed,
        Offered,
        Rejected,
        Withdrawn
    }
}
