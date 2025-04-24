using JobAppTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Application.Services.Interfaces
{
    public interface IReferralInfoService
    {
        IEnumerable<ReferralInfo> GetAllReferralInfos(string userId, string userRole);
        ReferralInfo GetReferralInfoById(int id);
        void CreateReferralInfo(ReferralInfo referralInfo);
        void UpdateReferralInfo(ReferralInfo referralInfo);
        bool DeleteReferralInfo(int id);
    }
}
