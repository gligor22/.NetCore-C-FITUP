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
    public class TrainingRepository :ITrainingRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Training> entities;
        string errorMessage = string.Empty;

        public TrainingRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Training>();
        }
        public IEnumerable<Training> GetAll()
        {
            return entities.Include(z => z.Coach).Include(z => z.Reservations).AsEnumerable();
        }

        public Training Get(Guid? id)
        {
            return entities.Include(z => z.Coach).Include(z => z.Reservations).SingleOrDefault(s => s.id == id);
        }
        public void Insert(Training entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.Reservations = new List<Reservation>();
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(Training entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(Training entity)
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
