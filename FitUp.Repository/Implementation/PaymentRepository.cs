using FitUp.Domain;
using FitUp.Domain.Models;
using FitUp.Repository.Data;
using FitUp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitUp.Repository.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Payment> entities;
        string errorMessage = string.Empty;

        public PaymentRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Payment>();
        }
        public IEnumerable<Payment> GetAll(int k)
        {
            return entities.Include(z => z.Client).Where(z=>z.payment_date.Month==k && z.payment_date.Year==DateTime.UtcNow.Year);
        }

        public Payment Get(Guid? id)
        {
            return entities.Include(z => z.Client).SingleOrDefault(s => s.id == id);
        }
        public void Insert(Payment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(Payment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(Payment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public List<Payment> getByClient(Client c)
        {
               return entities.Include(z => z.Client).Where(z => z.Client == c).ToList();
        }
    }
}
