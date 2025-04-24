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
    public class ReferralInfoService : IReferralInfoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReferralInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateReferralInfo(ReferralInfo referralInfo)
        {
            _unitOfWork.ReferralInfo.Add(referralInfo);
            _unitOfWork.Save();
        }

        public bool DeleteReferralInfo(int id)
        {
            var referralInfo = _unitOfWork.ReferralInfo.Get(n => n.Id == id);

            if (referralInfo != null)
            {
                _unitOfWork.ReferralInfo.Remove(referralInfo);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public IEnumerable<ReferralInfo> GetAllReferralInfos(string userId, string userRole)
        {
            if (userRole == "Admin")
            {
                // Admins can view all referral information
                return _unitOfWork.ReferralInfo.GetAll(includeProperties: "User")
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
            }
            else
            {
                // Regular users can only view their own referral information
                return _unitOfWork.ReferralInfo.GetAll(includeProperties: "User")
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            }
        }

       
        public ReferralInfo GetReferralInfoById(int id)
        {
            return _unitOfWork.ReferralInfo.Get(n=> n.Id == id);
        }

        public void UpdateReferralInfo(ReferralInfo referralInfo)
        {
            _unitOfWork.ReferralInfo.Update(referralInfo);
            _unitOfWork.Save();
        }
    }
}
