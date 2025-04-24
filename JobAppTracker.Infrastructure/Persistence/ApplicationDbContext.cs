using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobAppTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobAppTracker.InfraStructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }

        public DbSet<FollowUp> FollowUps { get; set; }
        public DbSet<InterviewSchedule> InterviewSchedules { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<ReferralInfo> ReferralInfos { get; set; }
        public DbSet<ResumeFile> ResumeFiles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(login => new { login.LoginProvider, login.ProviderKey }); // Composite key

            modelBuilder.Entity<JobApplication>()
            .HasOne(j => j.Resume)
            .WithOne(r => r.JobApplication)
            .HasForeignKey<ResumeFile>(r => r.JobApplicationId);
        }

    }
}
