using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Domain.Entities
{
    public class FollowUp
    {
        public int Id { get; set; }
        public DateTime FollowUpDate { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }               // Pending, Completed, etc.

        public int JobApplicationId { get; set; }
        public JobApplication? JobApplication { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        public string CompanyEmail { get; set; } = "supraja.tangella@gmail.com";

        public string FollowUpMethod { get; set; }       // Email, Phone, In-Person
        public string FollowUpOutcome { get; set; }      // Positive, Negative, Neutral
        public string FollowUpReminder { get; set; }     // Optional, if reminders needed

        public bool IsReminderSent { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }            // User who created the follow-up
        public string? UpdatedBy { get; set; }            // User who updated the follow-up

    }
}
