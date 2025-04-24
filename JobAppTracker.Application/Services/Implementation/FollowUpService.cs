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
    public class FollowUpService : IFollowUpService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowUpService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateFollowUp(FollowUp followUp)
        {
            _unitOfWork.FollowUp.Add(followUp);
            _unitOfWork.Save();
        }

        public bool DeleteFollowUp(int id)
        {
            var followUp = _unitOfWork.FollowUp.Get(n => n.Id == id);

            if (followUp != null)
            {
                _unitOfWork.FollowUp.Remove(followUp);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public IEnumerable<FollowUp> GetAllFollowUps(string userId, string userRole)
        {
            if (userRole == "Admin")
            {
                // Admins can view all FollowUps
                return _unitOfWork.FollowUp.GetAll(includeProperties: "User")
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
            }
            else
            {
                // Regular users can only view their own FollowUps
                return _unitOfWork.FollowUp.GetAll(includeProperties: "User")
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            }
        }

       
        public FollowUp GetFollowUpById(int id)
        {
            return _unitOfWork.FollowUp.Get(n=> n.Id == id);
        }

        public void UpdateFollowUp(FollowUp followUp)
        {
            _unitOfWork.FollowUp.Update(followUp);
            _unitOfWork.Save();
        }
    }
}
