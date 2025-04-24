using JobAppTracker.Application.Services.Interfaces;
using JobAppTracker.Domain.Entities;
using JobAppTracker.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobAppTracker.Web.Controllers
{
    public class JobApplicationController : Controller
    {
        public readonly IJobApplicationService _jobApplicationService;
        public readonly IResumeFileService _resumeFileService;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;
        public JobApplicationController(IJobApplicationService jobApplicationService, UserManager<User> userManager, IWebHostEnvironment environment, IResumeFileService resumeFileService)
        {
            _jobApplicationService = jobApplicationService;
            _userManager = userManager;
            _environment = environment;
            _resumeFileService = resumeFileService;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            var jobApplications = _jobApplicationService.GetAllJobApplications(user.Id, role.FirstOrDefault());
            return View(jobApplications);
        }
        public IActionResult Create()
        {
            GetApplicationStatuses();
            return View();
        }
        // POST: JobApplication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobApplication jobApplication, IFormFile resume)
        {
            if (ModelState.IsValid)
            {
                if (resume != null && resume.Length > 0)
                {
                    var resumeFile = await SaveResumeFile(resume);
                    jobApplication.Resume = resumeFile;
                }
                jobApplication.JobLink = "https://linkedin.com";
                jobApplication.UserId = _userManager.GetUserId(User);
                jobApplication.CreatedBy = User.Identity.Name;
                jobApplication.CreatedDate = DateTime.UtcNow;
                jobApplication.ResumeFileId = jobApplication.Resume?.Id ?? 0;
                // Save and get the new JobApplication with its ID
                var createdJobApp = _jobApplicationService.CreateJobApplication(jobApplication);

                // Associate resume with JobApplicationId
                if (jobApplication.Resume != null)
                {
                    jobApplication.Resume.JobApplicationId = createdJobApp.Id;
                    _resumeFileService.UpdateResumeFile(jobApplication.Resume);
                }
                return RedirectToAction(nameof(Index));
            }
            GetApplicationStatuses();
            return View(jobApplication);
        }

        // GET: JobApplication/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var jobApplication = _jobApplicationService.GetJobApplicationById(id);
            if (jobApplication == null)
                return NotFound();
            GetApplicationStatuses();
            return View(jobApplication);
        }

        // POST: JobApplication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobApplication jobApplication, IFormFile newResume)
        {
            if (id != jobApplication.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingJobApp = _jobApplicationService.GetJobApplicationById(id);

                    if (existingJobApp == null)
                        return NotFound();

                    existingJobApp.JobTitle = jobApplication.JobTitle;
                    existingJobApp.Notes = jobApplication.Notes;
                    existingJobApp.CompanyName = jobApplication.CompanyName;
                    existingJobApp.JobLocation = jobApplication.JobLocation;
                    existingJobApp.JobLink = jobApplication.JobLink;
                    existingJobApp.UpdatedBy = User.Identity.Name;
                    existingJobApp.UpdatedDate = DateTime.UtcNow;

                    if (newResume != null && newResume.Length > 0)
                    {
                        if (existingJobApp.Resume != null)
                        {
                            DeleteResumeFile(existingJobApp.Resume.FilePath);
                            _resumeFileService.DeleteResumeFile(existingJobApp.Resume.Id);
                        }
                        var newResumeFile = await SaveResumeFile(newResume);
                        existingJobApp.Resume = newResumeFile;
                        existingJobApp.ResumeFileId = newResumeFile.Id;
                        _resumeFileService.UpdateResumeFile(newResumeFile);
                    }
                    _jobApplicationService.UpdateJobApplication(existingJobApp);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_jobApplicationService.GetJobApplicationById(jobApplication.Id) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            GetApplicationStatuses();
            return View(jobApplication);
        }

        // GET: JobApplication/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var jobApplication = _jobApplicationService.GetJobApplicationById(id);
            if (jobApplication == null)
                return NotFound();

            return View(jobApplication);
        }

        // POST: JobApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = _jobApplicationService.GetJobApplicationById(id);
            if (jobApplication != null)
            {
                if (jobApplication.Resume != null)
                {
                    DeleteResumeFile(jobApplication.Resume.FilePath);
                    _resumeFileService.DeleteResumeFile(jobApplication.Resume.Id);
                }

                _jobApplicationService.DeleteJobApplication(jobApplication.Id);
            }
            return RedirectToAction(nameof(Index));
        }

        // Helper to Save Resume File
        private async Task<ResumeFile> SaveResumeFile(IFormFile resume)
        {
            if (resume.Length > 5 * 1024 * 1024)
            {
                TempData["Error"] = "File size must be less than 5MB.";
                return null;
            }

            if (Path.GetExtension(resume.FileName).ToLower() != ".pdf")
            {
                TempData["Error"] = "Only PDF files are allowed.";
                return null;
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "resumes");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(resume.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await resume.CopyToAsync(fileStream);
            }

            var resumeFile = new ResumeFile
            {
                FileName = resume.FileName,
                FilePath = "/resumes/" + uniqueFileName,
                UploadedOn = DateTime.Now,
                CreatedBy = User.Identity.Name,
                UserId = _userManager.GetUserId(User)
            };

            resumeFile = _resumeFileService.CreateResumeFile(resumeFile);

            return resumeFile;
        }

        // Helper to Delete Resume File
        private void DeleteResumeFile(string filePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        private void GetApplicationStatuses()
        {
            ViewBag.ApplicationStatuses = Enum.GetValues(typeof(ApplicationStatus))
                .Cast<ApplicationStatus>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString() // Or use Display name logic if needed
                }).ToList();
        }
    }
}
