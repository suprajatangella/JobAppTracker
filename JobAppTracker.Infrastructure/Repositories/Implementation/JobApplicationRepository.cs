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
    public class JobApplicationRepository : Repository<JobApplication>, IJobApplicationRepository
    {
        private readonly ApplicationDbContext _db;
        public JobApplicationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(JobApplication jobApplication)
        {
            _db.JobApplications.Update(jobApplication);
        }
    }
}
