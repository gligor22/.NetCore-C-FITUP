using FitUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Repository.Interface
{
    public interface IReservationRepository
    {
        IEnumerable<Reservation> GetAll();
        Reservation Get(Guid? id);
        void Insert(Reservation entity);
        void Update(Reservation entity);
        void Delete(Reservation entity);
        IEnumerable<Reservation> GetAllbyUser(string email);

    }
}
