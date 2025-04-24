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
    public class ResumeFileRepository : Repository<ResumeFile>, IResumeFileRepository
    {
        private readonly ApplicationDbContext _db;
        public ResumeFileRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ResumeFile resumeFile)
        {
            _db.ResumeFiles.Update(resumeFile);
        }
    }
}
