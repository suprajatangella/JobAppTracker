using JobAppTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Application.Services.Interfaces
{
    public interface IResumeFileService
    {
        IEnumerable<ResumeFile> GetAllResumeFiles(string userId, string userRole);
        ResumeFile GetResumeFileById(int id);
        ResumeFile CreateResumeFile(ResumeFile resumeFile);
        void UpdateResumeFile(ResumeFile resumeFile);
        bool DeleteResumeFile(int id);
    }
}
