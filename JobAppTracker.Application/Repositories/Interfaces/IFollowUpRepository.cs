using JobAppTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Application.Repositories.Interfaces
{
    public interface IFollowUpRepository : IRepository<FollowUp>
    {
        void Update(FollowUp entity);
        void UpdateRange(List<FollowUp> followUps);
    }
}
