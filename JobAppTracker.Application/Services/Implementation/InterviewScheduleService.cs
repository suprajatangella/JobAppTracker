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
    public class InterviewScheduleService : IInterviewScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InterviewScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateInterviewSchedule(InterviewSchedule interviewSchedule)
        {
            _unitOfWork.InterviewSchedule.Add(interviewSchedule);
            _unitOfWork.Save();
        }

        public bool DeleteInterviewSchedule(int id)
        {
            var interviewSchedule = _unitOfWork.InterviewSchedule.Get(n => n.Id == id);

            if (interviewSchedule != null)
            {
                interviewSchedule.Status = "Cancelled";
                interviewSchedule.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.InterviewSchedule.Update(interviewSchedule);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public bool IsInterviewOverlapping(string userId, DateTime interviewDate)
        {
            var isOverlapping = _unitOfWork.InterviewSchedule.GetAll().Any(i => i.UserId == userId && i.InterviewDate == interviewDate);
            return isOverlapping;
        }

        public IEnumerable<InterviewSchedule> GetAllInterviewSchedules(string userId, string userRole)
        {
            if (userRole == "Admin")
            {
                // Admins can view all Interview Schedules
                return _unitOfWork.InterviewSchedule.GetAll(includeProperties: "User, JobApplication")
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
            }
            else
            {
                // Regular users can only view their own interview schedules
                return _unitOfWork.InterviewSchedule.GetAll(includeProperties: "User, JobApplication")
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            }
        }

       
        public InterviewSchedule GetInterviewScheduleById(int id)
        {
            return _unitOfWork.InterviewSchedule.Get(n=> n.Id == id);
        }

        public void UpdateInterviewSchedule(InterviewSchedule interviewSchedule)
        {
            _unitOfWork.InterviewSchedule.Update(interviewSchedule);
            _unitOfWork.Save();
        }
    }
}
