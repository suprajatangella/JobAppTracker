using JobAppTracker.Application.Repositories.Interfaces;
using JobAppTracker.Domain.Entities;
using JobAppTracker.InfraStructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.InfraStructure.Repositories.Implementation
{
    public class ReferralInfoRepository : Repository<ReferralInfo>, IReferralInfoRepository
    {
        private readonly ApplicationDbContext _db;
        public ReferralInfoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ReferralInfo referralInfo)
        {
            _db.ReferralInfos.Update(referralInfo);
        }
    }
}
