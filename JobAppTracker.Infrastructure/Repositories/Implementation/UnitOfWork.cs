using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobAppTracker.Application.Repositories.Interfaces;
using JobAppTracker.InfraStructure.Persistence;

namespace JobAppTracker.InfraStructure.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository User { get; private set; }

        public IFollowUpRepository FollowUp { get; private set; }

        public IInterviewScheduleRepository InterviewSchedule { get; private set; }

        public IJobApplicationRepository JobApplication { get; private set; }

        public IReferralInfoRepository ReferralInfo { get; private set; }

        public IResumeFileRepository ResumeFile { get; private set; }

        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            //FollowUp = new FollowUpRepository(_db);
            //Budget = new BudgetRepository(_db);
            //Category = new CategoryRepository(_db);
            ////ExpenseCategory = new ExpenseCategoryRepository(_db);
            //Notification = new NotificationRepository(_db);
            User= new UserRepository(_db);
            FollowUp = new FollowUpRepository(_db);
            InterviewSchedule = new InterviewScheduleRepository(_db);
            JobApplication = new JobApplicationRepository(_db);
            ReferralInfo = new ReferralInfoRepository(_db);
            ResumeFile = new ResumeFileRepository(_db);

        }

        void IUnitOfWork.Save()
        {
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //await _db.SaveChangesAsync();
        }
    }
}
