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
    public class ResumeFileService : IResumeFileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResumeFileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResumeFile CreateResumeFile(ResumeFile resumeFile)
        {
           _unitOfWork.ResumeFile.Add(resumeFile);
            _unitOfWork.Save();
            return resumeFile;
        }

        public bool DeleteResumeFile(int id)
        {
            var resumeFile = _unitOfWork.ResumeFile.Get(n => n.Id == id);

            if (resumeFile != null)
            {
                _unitOfWork.ResumeFile.Remove(resumeFile);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public IEnumerable<ResumeFile> GetAllResumeFiles(string userId, string userRole)
        {
            if (userRole == "Admin")
            {
                // Admins can view all resumes
                return _unitOfWork.ResumeFile.GetAll(includeProperties: "User")
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
            }
            else
            {
                // Regular users can only view their own resume information
                return _unitOfWork.ResumeFile.GetAll(includeProperties: "User")
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            }
        }

       
        public ResumeFile GetResumeFileById(int id)
        {
            return _unitOfWork.ResumeFile.Get(n=> n.Id == id);
        }

        public void UpdateResumeFile(ResumeFile resumeFile)
        {
            _unitOfWork.ResumeFile.Update(resumeFile);
            _unitOfWork.Save();
        }
    }
}
