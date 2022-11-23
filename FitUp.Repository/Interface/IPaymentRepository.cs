using FitUp.Domain;
using FitUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Repository.Interface
{
    public interface IPaymentRepository
    {
        IEnumerable<Payment> GetAll(int k);
        Payment Get(Guid? id);
        void Insert(Payment entity);
        void Update(Payment entity);
        void Delete(Payment entity);
        List<Payment> getByClient(Client c);
    }
}
