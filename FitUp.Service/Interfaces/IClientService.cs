using FitUp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitUp.Service.Interfaces
{
    public interface IClientService
    {
        List<Client> getAll();
        Client GetDetails(Guid? id);
        void CreateNew(Client c);
        void Update(Client c);
        void Delete(Guid id);
        void clientReserve(Client c);
        void clientRemoveReservation(Client c);
        void newPayment(Client c,DateTime date,int allowed);
        void deactive(Client c);
        Client findByEmail(String email);
    }
}

