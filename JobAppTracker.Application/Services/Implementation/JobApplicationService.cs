using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobAppTracker.Application.Services.Interfaces;
using JobAppTracker.Application.Repositories.Interfaces;
using JobAppTracker.Domain.Entities;

namespace JobAppTracker.Application.Services.Implementation
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public JobApplication CreateJobApplication(JobApplication jobApplication)
        {
            _unitOfWork.JobApplication.Add(jobApplication);
            _unitOfWork.Save();
            return jobApplication;
        }

        public bool DeleteJobApplication(int id)
        {
            var jobApplication = _unitOfWork.JobApplication.Get(n => n.Id == id, includeProperties: "User, Resume");

            if (jobApplication != null)
            {
                _unitOfWork.JobApplication.Remove(jobApplication);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public IEnumerable<JobApplication> GetAllJobApplications(string userId, string userRole)
        {
            if (userRole == "Admin")
            {
                // Admins can view all job applications
                if (_unitOfWork.JobApplication != null)
                {
                    return _unitOfWork.JobApplication.GetAll(includeProperties: "User, Resume")
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
                }
                else
                {
                    return new List<JobApplication>();
                }
            }
            else
            {
                if (_unitOfWork.JobApplication != null)
                {
                    // Regular users can only view their own job applications
                    return _unitOfWork.JobApplication.GetAll(includeProperties: "User, Resume")
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
                }
                else
                {
                    return new List<JobApplication>();

                }
            }
        }

       
        public JobApplication GetJobApplicationById(int id)
        {
            return _unitOfWork.JobApplication.Get(n=> n.Id == id, includeProperties: "User, Resume");
        }

        public void UpdateJobApplication(JobApplication jobApplication)
        {
            _unitOfWork.JobApplication.Update(jobApplication);
            _unitOfWork.Save();
        }
    }
}
