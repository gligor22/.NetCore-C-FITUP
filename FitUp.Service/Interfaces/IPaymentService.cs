using FitUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Service.Interfaces
{
    public interface IPaymentService
    {
        List<Payment> getAll(int k);
        Payment GetDetails(Guid? id);
        void CreateNew(Payment c);
        void Update(Payment c);
        void Delete(Guid id);
        bool checkPaymentValid(Payment c);
        List<Payment> getAllbyClient(Client c);
    }
}
