using JobAppTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAppTracker.Application.Services.Interfaces
{
        public interface IEmailService
        {
            Task<EmailResult> SendEmailAsync(string toEmail, string subject, string body);
            Task<EmailResult> SendEmailWithAttachment(byte[] file, string fileName);
        }
}
