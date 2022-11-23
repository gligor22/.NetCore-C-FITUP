using FitUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Service.Interfaces
{
    public interface IReservationService
    {
        List<Reservation> getAll();
        Reservation GetDetails(Guid? id);
        void CreateNew(Reservation c);
        void Update(Reservation c);
        void Delete(Guid id);
        List<Reservation> GetAllByUser(string Email);
    }
}
