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
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Client> entities;
        string errorMessage = string.Empty;

        public ClientRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Client>();
        }
        public IEnumerable<Client> GetAll()
        {
            return entities.Include(z => z.Reservations).Include(z => z.Payments).AsEnumerable();
        }

        public Client Get(Guid? id)
        {
            return entities.Include(z => z.Reservations).Include(z => z.Reservations).Include(z => z.Payments).SingleOrDefault(s => s.id == id);
        }
        public void Insert(Client entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(Client entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(Client entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public Client findByEmail(string email)
        {
            return entities.Include(z => z.Reservations).Include(z => z.Payments).SingleOrDefault(s => s.Email == email);
        }
    }
}
