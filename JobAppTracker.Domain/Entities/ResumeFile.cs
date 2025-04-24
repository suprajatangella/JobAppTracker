using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Domain.Entities
{
    public class ResumeFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedOn { get; set; }

        public int? JobApplicationId { get; set; }
        public JobApplication JobApplication { get; set; } // Navigation property
        public string UserId { get; set; }
        public User User { get; set; } // Navigation property
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }            // User who created the follow-up
        public string? UpdatedBy { get; set; }
    }
}
