using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Application.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IFollowUpRepository FollowUp { get; }
        IInterviewScheduleRepository InterviewSchedule { get; }
        //IExpenseCategoryRepository ExpenseCategory { get; }
        IJobApplicationRepository JobApplication { get; }
        IReferralInfoRepository ReferralInfo { get; }
        IResumeFileRepository ResumeFile { get; }
        IUserRepository User { get; }
        void Save();
    }
}
