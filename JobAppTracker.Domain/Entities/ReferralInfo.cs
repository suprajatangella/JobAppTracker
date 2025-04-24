using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Domain.Entities
{
    public class ReferralInfo
    {
        public int Id { get; set; }
        public string ReferrerName { get; set; }   // Name of the person who referred
        public string ReferrerEmail { get; set; }
        public string ReferrerPosition { get; set; }
        public string CompanyName { get; set; }
        public DateTime ReferredDate { get; set; }
        public string Notes { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
        public int JobApplicationId { get; set; }
        public JobApplication JobApplication { get; set; } // Navigation property
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }            // User who created the follow-up
        public string? UpdatedBy { get; set; }
    }
}
