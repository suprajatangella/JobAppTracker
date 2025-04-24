using JobAppTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Application.Repositories.Interfaces
{
    public interface IReferralInfoRepository : IRepository<ReferralInfo>
    {
        void Update(ReferralInfo entity);
    }
}
