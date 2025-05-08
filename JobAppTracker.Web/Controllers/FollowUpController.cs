using JobAppTracker.Application.Repositories.Interfaces;
using JobAppTracker.Application.Services.Interfaces;
using JobAppTracker.Domain.Entities;
using JobAppTracker.Domain.Enums;
using JobAppTracker.InfraStructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobAppTracker.Web.Controllers
{
    public class FollowupController : Controller
    {
        private readonly IFollowUpService _followUpService;
        //private readonly IUnitOfWork _unitOfWork;
        private readonly IJobApplicationService _applicationService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        public FollowupController(IFollowUpService followUpService,  IJobApplicationService applicationService, UserManager<User> userManager, IEmailService emailService)
        {
            _followUpService = followUpService;
            //_unitOfWork = unitOfWork;
            _applicationService = applicationService;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: /Followup/Create?jobApplicationId=1
        public async Task<IActionResult> Create(int jobApplicationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            ViewBag.JobApplicationId = jobApplicationId;
           
            ViewBag.FollowUpStatusList = GetFollowUpStatusSelectList();
            ViewBag.JobApplicationList = GetJobApplicationSelectList(user.Id, role.FirstOrDefault());
            return View();
        }

        // POST: /Followup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FollowUp followup)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                followup.UserId = user.Id;
                followup.CreatedBy = user.Id;
                _followUpService.CreateFollowUp(followup);
                //_unitOfWork.Save();
                return RedirectToAction("Index", "FollowUp", new { id = followup.JobApplicationId });
            }
            return View(followup);
        }

        // GET: /Followup/Index?jobApplicationId=1
        public async Task<IActionResult> Index(string userId, string userRole)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            var followups =  _followUpService.GetAllFollowUps(user.Id, role.FirstOrDefault())
                //.Where(f => f.JobApplicationId == jobApplicationId)
                .OrderBy(f => f.FollowUpDate)
                .ToList();

            //ViewBag.JobApplicationId = jobApplicationId;
            return View(followups);
        }

        // GET: /Followup/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var followup = _followUpService.GetFollowUpById(id);
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            if (followup == null)
            {
                return NotFound();
            }
            ViewBag.JobApplicationList = GetJobApplicationSelectList(user.Id, role.FirstOrDefault());
            ViewBag.FollowUpStatusList = GetFollowUpStatusSelectList();
            return View(followup);
        }

        // POST: /Followup/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FollowUp followup)
        {
            if (id != followup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _followUpService.UpdateFollowUp(followup);
                //_unitOfWork.Save();
                return RedirectToAction("Index", new { jobApplicationId = followup.JobApplicationId });
            }
            return View(followup);
        }

        // GET: /Followup/Delete/5
        public IActionResult Delete(int id)
        {
            var followup = _followUpService.GetFollowUpById(id);
            if (followup == null)
            {
                return NotFound();
            }

            return View(followup);
        }

        // POST: /Followup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var followup = _followUpService.GetFollowUpById(id);
            _followUpService.DeleteFollowUp(followup.Id);
            //_unitOfWork.Save();
            return RedirectToAction("Index", new { jobApplicationId = followup.JobApplicationId });
        }

        private IEnumerable<SelectListItem> GetFollowUpStatusSelectList()
        {
            return Enum.GetValues(typeof(FollowUpStatus))
                       .Cast<FollowUpStatus>()
                       .Select(status => new SelectListItem
                       {
                           Value = ((int)status).ToString(),
                           Text = status.ToString()
                       });
        }

        private IEnumerable<SelectListItem> GetJobApplicationSelectList(string userId, string role)
        {
            
            return _applicationService.GetAllJobApplications(userId, role)
                           .Select(app => new SelectListItem
                           {
                               Value = app.Id.ToString(),
                               Text = app.CompanyName
                           });
        }

        [HttpGet("reminder-followups")]
        public async Task<IActionResult> RemainderList()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            var followups = _followUpService.GetAllFollowUps(user.Id, role.FirstOrDefault())
                .Where(f => f.FollowUpDate <= DateTime.Now && f.IsReminderSent == false)
                .ToList();

            return View(followups);
        }

        // Button click: Send reminder emails and bulk update status
        [HttpPost("send-reminder-emails")]
        public async Task<IActionResult> SendReminderEmails()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            var followups = _followUpService.GetAllFollowUps(user.Id, role.FirstOrDefault())
                .Where(f => f.FollowUpDate <= DateTime.Now && f.IsReminderSent == false)
                .ToList();

            if (followups.Count == 0)
            {
                return BadRequest("No follow-ups pending reminders.");
            }

            var emailsSent = new List<string>();

            foreach (var followUp in followups)
            {
                // Send email asynchronously
                var result = await _emailService.SendEmailAsync(followUp.CompanyEmail, "Application staus Follow-up","I want to followup my application status");
                if (result.Success)
                {
                    emailsSent.Add(followUp.CompanyEmail);
                    followUp.IsReminderSent = true; // Mark as sent
                    followUp.User = null;
                }
                else
                {
                    // Log the error or handle it as needed
                    Console.WriteLine($"Failed to send email to {followUp.CompanyEmail}: {result.Message}");
                    return BadRequest($"Failed to send email to {followUp.CompanyEmail}: {result.Message}");
                }
            }

            // Bulk update IsReminderSent status
            _followUpService.BulkUpdateFollowUp(followups);

            return View(nameof(RemainderList));
        }
    }

}
