using JobAppTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Domain.Entities
{
    public class JobApplication
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string JobLocation { get; set; }
        public string? JobLink { get; set; }
        public DateTime AppliedDate { get; set; }
        public ApplicationStatus Status { get; set; } // Enum (Applied, Interviewing, Offered, Rejected)
        public string Notes { get; set; }

        public int? ResumeFileId { get; set; }
        public ResumeFile? Resume { get; set; } // Navigation property

        public string? UserId { get; set; }
        public User? User { get; set; } // Navigation property
        public List<InterviewSchedule>? InterviewSchedules { get; set; } = new List<InterviewSchedule>();
        public List<FollowUp>? FollowUps { get; set; } = new List<FollowUp>();
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }            // User who created the follow-up
        public string? UpdatedBy { get; set; }
    }
}
