using JobAppTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Application.Services.Interfaces
{
    public interface IJobApplicationService
    {
        IEnumerable<JobApplication> GetAllJobApplications(string userId, string userRole);
        JobApplication GetJobApplicationById(int id);
        JobApplication CreateJobApplication(JobApplication jobApplication);
        void UpdateJobApplication(JobApplication jobApplication);
        bool DeleteJobApplication(int id);
    }
}
