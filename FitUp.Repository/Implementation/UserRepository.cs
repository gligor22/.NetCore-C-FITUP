using FitUp.Domain.Identity;
using FitUp.Repository.Data;
using FitUp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitUp.Repository.Implementation
{
    public class UserRepository : UserRepoInterface
    {
        private readonly ApplicationDbContext context;
        private DbSet<FitUpApplicationUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<FitUpApplicationUser>();
        }
        public IEnumerable<FitUpApplicationUser> GetAll()
        {
            return entities;
        }

        public FitUpApplicationUser GetById(string id)
        {
            return entities
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(FitUpApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(FitUpApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(FitUpApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
