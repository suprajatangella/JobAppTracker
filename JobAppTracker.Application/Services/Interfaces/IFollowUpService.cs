using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobAppTracker.Domain.Entities;

namespace JobAppTracker.Application.Services.Interfaces
{
    public interface IFollowUpService
    {
        IEnumerable<FollowUp> GetAllFollowUps(string userId, string userRole);
        FollowUp GetFollowUpById(int id);
        void CreateFollowUp(FollowUp followUp);
        void UpdateFollowUp(FollowUp followUp);
        bool DeleteFollowUp(int id);
    }
}
