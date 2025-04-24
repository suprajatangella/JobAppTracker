using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }       // Example: Supraja M
        public string LinkedInUrl { get; set; }

        public ICollection<ReferralInfo> Referrals { get; set; }
        public ICollection<FollowUp> FollowUps { get; set; }
        public ICollection<InterviewSchedule> InterviewSchedules { get; set; }
        public ICollection<JobApplication> JobApplications { get; set; }
        public ICollection<ResumeFile> ResumeFiles { get; set; }
        public string ProfilePictureUrl { get; set; } // URL to the user's profile picture
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateTime? UpdatedDate { get; set; }
    }
}
