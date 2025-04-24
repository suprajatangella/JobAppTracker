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
    public class InterviewScheduleRepository : Repository<InterviewSchedule>, IInterviewScheduleRepository
    {
        private readonly ApplicationDbContext _db;
        public InterviewScheduleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InterviewSchedule interviewSchedule)
        {
            _db.InterviewSchedules.Update(interviewSchedule);
        }
    }
}
