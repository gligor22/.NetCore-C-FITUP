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
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Reservation> entities;
        string errorMessage = string.Empty;

        public ReservationRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Reservation>();
        }
        public IEnumerable<Reservation> GetAll()
        {
            return entities.Include(z => z.Client).Include(z=>z.Training).Include(z => z.Training.Reservations).AsEnumerable();
        }

        public Reservation Get(Guid? id)
        {
            return entities.Include(z => z.Client).Include(z => z.Training).Include(z => z.Training.Reservations).SingleOrDefault(s => s.id == id);
        }
        public void Insert(Reservation entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(Reservation entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(Reservation entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public IEnumerable<Reservation> GetAllbyUser(string email)
        {
            return entities.Include(z => z.Client).Include(z => z.Training).Include(z => z.Training.Reservations).Where(z=> z.Client.Email==email).AsEnumerable();
        }
    }
}
