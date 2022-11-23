using FitUp.Domain.Models;
using FitUp.Repository.Interface;
using FitUp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitUp.Service.Implementations
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _mailRepository;

        public BackgroundEmailSender(IEmailService emailService, IRepository<EmailMessage> mailRepository)
        {
            _emailService = emailService;
            _mailRepository = mailRepository;
        }
        public async Task doWork()
        {
            List<EmailMessage> emailMessages = _mailRepository.GetAll().Where(z => !z.Status).ToList();
            await _emailService.SendEmailAsync(emailMessages);
            foreach (EmailMessage message in emailMessages)
            {
                message.Status = true;
                _mailRepository.Update(message);
            }
            
        }
    }
}
