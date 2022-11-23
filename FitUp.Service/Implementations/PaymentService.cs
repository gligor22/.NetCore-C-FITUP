using FitUp.Domain.Exceptions;
using FitUp.Domain.Models;
using FitUp.Repository.Interface;
using FitUp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitUp.Service.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IClientService _clientService;
        private readonly IRepository<EmailMessage> _emailMessageRepository;

        public PaymentService(IPaymentRepository paymentRepository, IClientService clientService, IRepository<EmailMessage> emailMessageRepository)
        {
            _paymentRepository = paymentRepository;
            _clientService = clientService;
            _emailMessageRepository = emailMessageRepository;
        }

        public bool checkPaymentValid(Payment c)
        {
            DateTime dateTime = new DateTime(1,1,1); 
            if(c.Client.day_last_payment==dateTime)
                return true;
            int days = c.payment_date.Subtract(c.Client.day_last_payment).Days;
            if (days > 28 && days<60)
                return true;
            else 
                return false;
        }

        public void CreateNew(Payment c)
        {
            if (this.checkPaymentValid(c) == false)
            {
                throw new MonthAlreadyPayed(c.Client.day_last_payment.ToString());
            }
            if (c.Payed_Price == 1800)
                c.week_number_trainings = 3;
            else if (c.Payed_Price == 2000)
                c.week_number_trainings = 4;
            else
                c.week_number_trainings = 7;

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.MailTo = c.Client.Email;
            emailMessage.Subject = "Payment Confirmed";
            emailMessage.Status = false;
            emailMessage.Content = "Your Payment is confirmed. We expect to see you at out gym: " + c.ToString();

            _emailMessageRepository.Insert(emailMessage);
            _clientService.newPayment(c.Client, c.payment_date,c.week_number_trainings);
            _paymentRepository.Insert(c);
        }

        public void Delete(Guid id)
        {
            var c = _paymentRepository.Get(id);
            if (c != null)
                _paymentRepository.Delete(c);
        }

        public List<Payment> getAll(int k)
        {
            return _paymentRepository.GetAll(k).ToList();
        }

        public List<Payment> getAllbyClient(Client c)
        {
            return _paymentRepository.getByClient(c);
        }

        public Payment GetDetails(Guid? id)
        {
            return _paymentRepository.Get(id);
        }

        

        public void Update(Payment c)
        {
            if (c.Payed_Price == 1800)
                c.week_number_trainings = 3;
            else if (c.Payed_Price == 2000)
                c.week_number_trainings = 4;
            else
                c.week_number_trainings = 7;


            EmailMessage emailMessage = new EmailMessage();
            emailMessage.MailTo = c.Client.Email;
            emailMessage.Subject = "Payment Confirmed";
            emailMessage.Status = false;
            emailMessage.Content = "Your Payment is confirmed. We expect to see you at out gym. Reserve a session soon." + c.ToString();

            _emailMessageRepository.Insert(emailMessage);
            _clientService.newPayment(c.Client, c.payment_date,c.week_number_trainings);
            _paymentRepository.Update(c);
        }
    }
}

