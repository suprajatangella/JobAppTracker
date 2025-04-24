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
    public class FollowUpRepository : Repository<FollowUp>, IFollowUpRepository
    {
        private readonly ApplicationDbContext _db;
        public FollowUpRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(FollowUp followUp)
        {
            _db.FollowUps.Update(followUp);
        }
    }
}
