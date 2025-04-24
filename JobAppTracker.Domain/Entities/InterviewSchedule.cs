using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Domain.Entities
{
    public class InterviewSchedule
    {
        public int Id { get; set; }
        public DateTime InterviewDate { get; set; }
        public string InterviewerName { get; set; }
        public string InterviewMode { get; set; } // e.g., Online, Phone, In-Person
        public string Notes { get; set; }
        public int JobApplicationId { get; set; }
        public JobApplication JobApplication { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Status { get; set; } // e.g., Scheduled, Completed, Cancelled

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }            // User who created the Interview schedule
        public string? UpdatedBy { get; set; }
    }
}
