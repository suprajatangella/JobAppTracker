using JobAppTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Application.Services.Interfaces
{
    public interface IInterviewScheduleService
    {
        IEnumerable<InterviewSchedule> GetAllInterviewSchedules(string userId, string userRole);
        InterviewSchedule GetInterviewScheduleById(int id);
        void CreateInterviewSchedule(InterviewSchedule interviewSchedule);
        void UpdateInterviewSchedule(InterviewSchedule interviewSchedule);
        bool DeleteInterviewSchedule(int id);
        bool IsInterviewOverlapping(string userId, DateTime interviewDate);
    }
}
