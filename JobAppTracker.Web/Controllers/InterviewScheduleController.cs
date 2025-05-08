using JobAppTracker.Application.Services.Implementation;
using JobAppTracker.Application.Services.Interfaces;
using JobAppTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JobAppTracker.Web.Controllers
{
    public class InterviewScheduleController : Controller
    {
        private readonly IInterviewScheduleService _interviewScheduleServie;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IJobApplicationService _applicationService;
        public InterviewScheduleController(IInterviewScheduleService interviewScheduleServie, UserManager<User> userManager, IEmailService emailService, IJobApplicationService applicationService)
        {
            _interviewScheduleServie = interviewScheduleServie;
            _userManager = userManager;
            _emailService = emailService;
            _applicationService = applicationService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            var interviews = _interviewScheduleServie.GetAllInterviewSchedules(user.Id, role.FirstOrDefault())
            .Where(i => i.UserId == user.Id)
             .ToList();
            return View(interviews);
        }

        public async Task<IActionResult> CreateInterview()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            PopulateTimeZones();
            ViewBag.JobApplicationList = GetJobApplicationSelectList(user.Id, role.FirstOrDefault());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateInterview(InterviewSchedule model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            if (!_interviewScheduleServie.IsInterviewOverlapping(user.Id, model.InterviewDate))
            {
                model.UserId = user.Id;
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = User.Identity.Name;
                var tz = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZoneId);
                model.InterviewDate = TimeZoneInfo.ConvertTimeToUtc(model.InterviewDate, tz);
                _interviewScheduleServie.CreateInterviewSchedule(model);
                TempData["success"] = "Interview scheduled successfully";
                _emailService.SendEmailAsync("supraja.tangella@gmail.com", "Interview Scheduled", "The interview scheduled on " + TimeZoneInfo.ConvertTimeFromUtc(model.InterviewDate, tz));
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "This interview is overlapping with existing interview";
                PopulateTimeZones();
                ViewBag.JobApplicationList = GetJobApplicationSelectList(user.Id, role.FirstOrDefault());
                return View(model);
            }
        }

        public async Task<IActionResult> UpdateInterview(int id)
        {
            var existing = _interviewScheduleServie.GetInterviewScheduleById(id);
            if (existing == null) return NotFound();
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            ViewBag.JobApplicationList = GetJobApplicationSelectList(user.Id, role.FirstOrDefault());
            PopulateTimeZones();
            var tz = TimeZoneInfo.FindSystemTimeZoneById(existing.TimeZoneId);
            existing.InterviewDate = TimeZoneInfo.ConvertTimeFromUtc(existing.InterviewDate, tz);
            return View(existing);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInterview(int id, InterviewSchedule updated)
        {
            var existing = _interviewScheduleServie.GetInterviewScheduleById(id);
            if (existing == null) return NotFound();
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            if (!_interviewScheduleServie.IsInterviewOverlapping(user.Id, updated.InterviewDate))
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(existing.TimeZoneId);
                existing.InterviewDate = TimeZoneInfo.ConvertTimeToUtc(updated.InterviewDate, tz);
                existing.InterviewerName = updated.InterviewerName;
                existing.InterviewMode = updated.InterviewMode;
                existing.Notes = updated.Notes;
                existing.Status = updated.Status;
                existing.UpdatedDate = DateTime.UtcNow;
                existing.UpdatedBy = User.Identity.Name;
                existing.UserId = user.Id;
                _interviewScheduleServie.UpdateInterviewSchedule(existing);
                TempData["success"] = "Interview updated successfully";
                _emailService.SendEmailAsync("supraja.tangella@gmail.com", "Interview Scheduled", "The interview scheduled on " + TimeZoneInfo.ConvertTimeFromUtc(existing.InterviewDate, tz));
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "This interview is overlapping with existing interview";
                PopulateTimeZones();
                ViewBag.JobApplicationList = GetJobApplicationSelectList(user.Id, role.FirstOrDefault());
                return View(updated);
            }

        }

        public async Task<IActionResult> CancelInterview(int id)
        {
            var existing = _interviewScheduleServie.GetInterviewScheduleById(id);
            if (existing == null) return NotFound();
            var tz = TimeZoneInfo.FindSystemTimeZoneById(existing.TimeZoneId);
            existing.InterviewDate = TimeZoneInfo.ConvertTimeFromUtc(existing.InterviewDate, tz);
            return View(existing);
        }

        [HttpPost]
        public async Task<IActionResult> CancelInterview(InterviewSchedule updated)
        {
            var existing = _interviewScheduleServie.GetInterviewScheduleById(updated.Id);
            if (existing == null) return NotFound();

            _interviewScheduleServie.DeleteInterviewSchedule(existing.Id);
            TempData["success"] = "Interview cancelled successfully";
            _emailService.SendEmailAsync("supraja.tangella@gmail.com", "Interview Cancelled", "The scheduled interview is Cancelled");
            return RedirectToAction(nameof(Index));

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
        private void PopulateTimeZones()
        {
            var timeZones = TimeZoneInfo.GetSystemTimeZones()
                .Select(tz => new SelectListItem
                {
                    Value = tz.Id,
                    Text = tz.StandardName
                }).ToList();
            ViewBag.TimeZones = timeZones.OrderBy(tz => tz.Text); 
        }
    }
}
